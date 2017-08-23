using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

using WSWD.WmallPos.POS.AppStarter.Net;
using System.Xml;
using System.Data.SQLite;
using WSWD.WmallPos.FX.Shared.Nini.Ini;
using System.Collections;

namespace WSWD.WmallPos.POS.AppStarter
{
    /// <summary>
    /// 프로그램 자동업데이트
    /// </summary>
    public partial class UpdateForm : Form
    {
        #region 변수
        const string CONN_STRING = "Data Source={0};Version=3;";
        const string POSAppExe = @"..\bin\WmallPOS.exe";
        const string XCopyAppExe = @"..\bin\XCopyApp.exe";

        private const int THREAD_VERSION_CHECK = 1;
        private const int THREAD_VERSION_DOWNLOAD = 2;

        private string m_storeNo = string.Empty;
        private string m_posNo = string.Empty;

        private bool m_activated = false;
        private string m_posFolder = string.Empty;
        private string m_appStarterFolder = Application.StartupPath;
        private string m_posConfigFolder = Application.StartupPath;
        private string m_tempFolder = Application.StartupPath + "\\temp";

        private string m_newVersion = string.Empty;
        private string m_localVersionFile = string.Empty;
        private string m_localVersionFolder = string.Empty;
        private string m_serverVersionFolder = string.Empty;
        private List<string> m_versionList = null;

        private string VERSION_FILE = "version.xml";
        private string m_serverPOSFolder = "/data/upgrade/pos";

        private POSFtpClient m_ftpClient = null;

        public event EventHandler<UpdateProgressEventArgs> UpdateProgress;

        DataSet dsConfig = new DataSet();
        
        

        #endregion

        #region 생성자

        public UpdateForm()
        {
            InitializeComponent();

            DataTable dtApp = new DataTable();
            dtApp.TableName = "AppConfig.ini";
            dtApp.Columns.Add("Section");
            dtApp.Columns.Add("Key");
            dtApp.Columns.Add("Value");
            dsConfig.Tables.Add(dtApp);

            DataTable dtDev = new DataTable();
            dtDev.TableName = "DevConfig.ini";
            dtDev.Columns.Add("Section");
            dtDev.Columns.Add("Key");
            dtDev.Columns.Add("Value");
            dsConfig.Tables.Add(dtDev);

            this.FormClosed += new FormClosedEventHandler(UpdateForm_FormClosed);
            this.UpdateProgress += new EventHandler<UpdateProgressEventArgs>(UpdateForm_UpdateProgress);
        }

        #endregion

        #region 이벤트정의

        /// <summary>
        /// 시작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void UpdateForm_Activated(object sender, System.EventArgs e)
        {
            if (m_activated)
            {
                return;
            }

            m_activated = true;
            Application.DoEvents();

            if (!ValidateOnStart())
            {
                CloseAndRunAppExe(true, string.Empty);
                return;
            }

            // Preparing
            Step01_PrepareStarter();

            // Server checking
            if (!Step01_CheckingServer())
            {
                CloseAndRunAppExe(true, string.Empty);
                return;
            }

            // Checking new version
            Step02_CheckingNewVersions();
        }

        /// <summary>
        /// Form closed 시 FTPCLient dispose한다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void UpdateForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_ftpClient != null)
            {
                m_ftpClient.Dispose();
            }
        }

        /// <summary>
        /// Progress update on form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void UpdateForm_UpdateProgress(object sender, UpdateProgressEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    lblStatus.Text = e.StatusMessage;
                    UpdateDownloadFileStatus(e.ProgressMessage);
                    colorProgressBar1.Percentage = e.Percentage;
                    Application.DoEvents();
                });
            }
            else
            {
                lblStatus.Text = e.StatusMessage;
                UpdateDownloadFileStatus(e.ProgressMessage);
                colorProgressBar1.Percentage = e.Percentage;
                Application.DoEvents();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_ftpClient_ProgressChanged(object sender, ProgressEventArgs e)
        {
            if (e.ThreadId == THREAD_VERSION_CHECK)
            {
                if (e.Percentage == POSFtpClient.ProgressCompleted ||
                    e.Percentage == POSFtpClient.ProgressFailed)
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate()
                        {
                            Step02_GetVersionResult();
                        });
                    }
                    else
                    {
                        Step02_GetVersionResult();
                    }
                }

                return;
            }


            // folder download
            if (e.Percentage != POSFtpClient.ProgressCompleted)
            {
                UpdateProgress(this, new UpdateProgressEventArgs()
                {
                    StatusMessage = string.Format("{0} 버전을 다운로드 하는 중...", m_newVersion),
                    ProgressMessage = string.Format("{0}/0", Path.GetFileName(e.ServerFile)),
                    Percentage = e.Percentage,
                    TotalPercentage = e.TotalPercentage
                });
            }
            else
            {
                UpdateProgress(this, new UpdateProgressEventArgs()
                {
                    StatusMessage = string.Format("{0} 버전을 다운로드 하는 중...", m_newVersion),
                    ProgressMessage = string.Format("{0}/1", Path.GetFileName(e.ServerFile)),
                    Percentage = e.Percentage,
                    TotalPercentage = e.TotalPercentage
                });

                if (m_ftpClient.RemainFileCount == 0)
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate()
                        {
                            Step03_DownloadVersionFolderCompleted();
                        });
                    }
                    else
                    {
                        Step03_DownloadVersionFolderCompleted();
                    }
                }
            }
        }

        /// <summary>
        /// FTP Download, connection failed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_ftpClient_ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            // dont retry after 3times failed
            //e.Retry = true;

            this.BeginInvoke((MethodInvoker)delegate()
            {
                if (txtLines.Text.Length != 0)
                {
                    txtLines.AppendText(Environment.NewLine);
                }
                txtLines.AppendText("서버 연결 오류입니다.");
                txtLines.AppendText(Environment.NewLine);
                txtLines.AppendText("재시도 하는 중...");
            });
        }

        #endregion

        #region 사용자정의

        /// <summary>
        /// POS실행중이면 확인, 종료
        /// </summary>
        /// <returns></returns>
        bool ValidateOnStart()
        {
            if (NativeMethods.IsRunning("WmallPOS.exe"))
            {
                var res = MessageBox.Show("WmallPOS 프로그램을 실행 중입니다. \n종료하시겠습니까?",
                    "종료", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (res == DialogResult.OK)
                {
                    NativeMethods.KillRunning("WmallPOS.exe");
                    return true;
                }

                return false;
            }

            // Check AppConfig.ini
            m_posFolder = Directory.GetParent(m_appStarterFolder).FullName;
            m_posConfigFolder = m_posFolder + "\\config";

            string configFilePath = Path.Combine(m_posConfigFolder, "AppConfig.ini");
            if (!File.Exists(configFilePath))
            {
                MessageBox.Show("POS 설정파일 확인 하십시오.");
                return false;
            }

            // Check POSNo & StoreNo
            string storeNo = GetPOSConfig("PosInfo", "StoreNo");
            string posNo = GetPOSConfig("PosInfo", "PosNo");
            if (string.IsNullOrEmpty(storeNo) || string.IsNullOrEmpty(posNo))
            {
                MessageBox.Show("POS 환경설정을 확인 하십시오.");
                return false;
            }

            m_serverPOSFolder = GetPOSConfig("PosFTP", "VersionInfoPath");

            // get ftp info
            string ftpServer = GetPOSConfig("PosFTP", "FtpSvrIP1");
            string sFtpPort = GetPOSConfig("PosFTP", "FtpComPort1");
            string ftpUser = GetPOSConfig("PosFTP", "User");
            string ftpUserPass = GetPOSConfig("PosFTP", "Pass");
            if (string.IsNullOrEmpty(m_serverPOSFolder) ||
                string.IsNullOrEmpty(ftpServer) ||
                string.IsNullOrEmpty(sFtpPort) ||
                string.IsNullOrEmpty(ftpUser) ||
                string.IsNullOrEmpty(ftpUserPass))
            {
                MessageBox.Show("POS 자동 업데이트 서버설정 확인 하십시오.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 사전준비작업
        /// </summary>
        void Step01_PrepareStarter()
        {
            if (!Directory.Exists(m_tempFolder))
            {
                Directory.CreateDirectory(m_tempFolder);
            }

            // get ftp info
            string ftpServer = GetPOSConfig("PosFTP", "FtpSvrIP1");
            string sFtpPort = GetPOSConfig("PosFTP", "FtpComPort1");
            Int16 ftpPort = string.IsNullOrEmpty(sFtpPort) ? (Int16)21 : Convert.ToInt16(sFtpPort);
            string ftpUser = GetPOSConfig("PosFTP", "User");
            string ftpUserPass = GetPOSConfig("PosFTP", "Pass");

            m_ftpClient = new POSFtpClient(ftpServer, ftpPort,
                new System.Net.NetworkCredential()
                {
                    UserName = ftpUser,
                    Password = ftpUserPass
                });

            m_ftpClient.ProgressChanged += new EventHandler<ProgressEventArgs>(m_ftpClient_ProgressChanged);
            m_ftpClient.ConnectionFailed += new EventHandler<ConnectionFailedEventArgs>(m_ftpClient_ConnectionFailed);
            m_localVersionFile = Path.Combine(m_tempFolder, VERSION_FILE);

            GetStorePosInfo();

            // SERVER Upgrade경로
            m_serverPOSFolder = GetPOSConfig("PosFTP", "VersionInfoPath");

            // delete previous date folder version;
            CleanOldTempFolders();
        }

        /// <summary>
        /// FTP 서버 확인작업
        /// </summary>
        private bool Step01_CheckingServer()
        {
            UpdateProgress(this, new UpdateProgressEventArgs()
            {
                StatusMessage = "서버 연결 중...",
                Percentage = 0
            });

            var res = m_ftpClient.TestConnection();
            if (!res)
            {
                UpdateProgress(this, new UpdateProgressEventArgs()
                {
                    StatusMessage = "서버 연결 실패!. 프로그램 종료 합니다.",
                    Percentage = 0
                });
            }

            return res;
        }

        /// <summary>
        /// 새버전을 확인
        /// </summary>
        /// <returns></returns>
        void Step02_CheckingNewVersions()
        {
            UpdateProgress(this, new UpdateProgressEventArgs()
            {
                StatusMessage = "새 버전을 확인 중...",
                Percentage = 0
            });

            if (File.Exists(m_localVersionFile))
            {
                File.Delete(m_localVersionFile);
            }

            m_ftpClient.DownloadFilesAsync(m_serverPOSFolder, m_tempFolder,
                new string[] { VERSION_FILE }, THREAD_VERSION_CHECK);
        }

        /// <summary>
        /// 
        /// </summary>
        void Step02_GetVersionResult()
        {
            if (!File.Exists(m_localVersionFile))
            {
                Step99_UpdateVersionClose(string.Empty);
                return;
            }

            // load version list first
            GetVersionList();

            // Checking new version
            m_newVersion = GetNextVersion(string.Empty);
            if (string.IsNullOrEmpty(m_newVersion))
            {
                Step99_UpdateVersionClose(string.Empty);
                return;
            }

            bool versionExist = Step03_ValidateVersionFolder(m_newVersion);
            while (!versionExist)
            {
                m_newVersion = GetNextVersion(m_newVersion);
                if (string.IsNullOrEmpty(m_newVersion))
                {
                    break;
                }

                versionExist = Step03_ValidateVersionFolder(m_newVersion);
            }

            if (string.IsNullOrEmpty(m_newVersion))
            {
                Step99_UpdateVersionClose(string.Empty);
            }
            else
            {
                Step03_DownloadVersionFolder(m_newVersion);
            }
        }

        /// <summary>
        /// 버전폴더 있는지 확인
        /// </summary>
        /// <param name="newVersion"></param>
        /// <returns></returns>
        bool Step03_ValidateVersionFolder(string newVersion)
        {
            UpdateProgress(this, new UpdateProgressEventArgs()
            {
                StatusMessage = string.Format("버전 {0}을 확인 중...", newVersion),
                Percentage = 0
            });

            if (string.IsNullOrEmpty(newVersion))
            {
                return false;
            }

            m_serverVersionFolder = m_serverPOSFolder + "/" + newVersion;
            var files = m_ftpClient.ListDirectoryDetail(m_serverVersionFolder);
            return files.Count > 0;
        }

        /// <summary>
        /// Download new version
        /// </summary>
        /// <param name="newVersion"></param>
        void Step03_DownloadVersionFolder(string newVersion)
        {
            UpdateProgress(this, new UpdateProgressEventArgs()
            {
                StatusMessage = string.Format("버전 {0}을 다운로드 시작...", newVersion),
                Percentage = 0
            });

            m_localVersionFolder = Path.Combine(m_tempFolder, newVersion);
            if (!Directory.Exists(m_localVersionFolder))
            {
                Directory.CreateDirectory(m_localVersionFolder);
            }

            m_serverVersionFolder = m_serverPOSFolder + "/" + newVersion;
            m_ftpClient.StartNewFolderDownload();
            m_ftpClient.DownloadFolderAsync(m_serverVersionFolder, m_tempFolder, THREAD_VERSION_DOWNLOAD);
        }

        /// <summary>
        /// Files and folder are downloaded to m_localVersionFolder
        /// </summary>
        void Step03_DownloadVersionFolderCompleted()
        {
            if (m_ftpClient.DownloadFailedCount > 0)
            {
                // failed count, restart and retry
                Step99_UpdateVersionClose(string.Empty);
                return;
            }

            // execute sql file
            RunPatchSql();

            // Update ini file
            Step04_UpdateConfigIni();

            // UpdateVersion, close and xcopy, completed
            Step99_UpdateVersionClose(m_localVersionFolder);
        }

        /// <summary>
        /// AppConfig.ini, DevConfig.ini update
        /// </summary>
        void Step04_UpdateConfigIni()
        {
            Step04_UpdateConfigIni("AppConfig.ini");
            Step04_UpdateConfigIni("DevConfig.ini");
        }

        /// <summary>
        /// Update config item for file.ini
        /// </summary>
        /// <param name="fileName"></param>
        void Step04_UpdateConfigIni(string fileName)
        {
            dsConfig.Tables[fileName].Clear();

            string configFilePath = Path.Combine(m_localVersionFolder, fileName);
            if (!File.Exists(configFilePath))
            {
                return;
            }

            // read config in AppConfig.ini, update each to POS config file
            //IniDocument doc = new IniDocument(configFilePath);
            //foreach (DictionaryEntry section in doc.Sections)
            //{
            //    var sec = doc.Sections[section.Key.ToString()];
            //    var keys = sec.GetKeys();

            //    foreach (var key in keys)
            //    {
            //        var value = sec.GetValue(key);

            //        // value format should be [store][pos]value
            //        string configValue = string.Empty;
            //        if (ParseConfigValue(value, out configValue))
            //        {
            //            // Update config directly
            //            UpdatePOSConfigValue(fileName, sec.Name, key, configValue);
            //        }
            //    }
            //}

            string line = string.Empty;
            string strTempSection = string.Empty;
            string strTempKey = string.Empty;
            string strTemp = string.Empty;
            using (StreamReader file = new StreamReader(configFilePath))
            {
                while ((line = file.ReadLine()) != null)
                {
                    strTempKey = string.Empty;
                    strTemp = string.Empty;

                    if (line == string.Format("[{0}]", "PosComm"))
                    {
                        strTempSection = "PosComm";
                    }
                    else if (line == string.Format("[{0}]", "PosFTP"))
                    {
                        strTempSection = "PosFTP";
                    }
                    else if (line == string.Format("[{0}]", "PosVan"))
                    {
                        strTempSection = "PosVan";
                    }
                    else if (line == string.Format("[{0}]", "PosOption"))
                    {
                        strTempSection = "PosOption";
                    }
                    else if (line == string.Format("[{0}]", "ScannerGun"))
                    {
                        strTempSection = "ScannerGun";
                    }
                    else if (line == string.Format("[{0}]", "LineDisplay"))
                    {
                        strTempSection = "LineDisplay";
                    }
                    else if (line == string.Format("[{0}]", "Printer"))
                    {
                        strTempSection = "Printer";
                    }
                    else if (line == string.Format("[{0}]", "MSR"))
                    {
                        strTempSection = "MSR";
                    }
                    else if (line == string.Format("[{0}]", "CashDrawer"))
                    {
                        strTempSection = "CashDrawer";
                    }
                    else if (line == string.Format("[{0}]", "SignPad"))
                    {
                        strTempSection = "SignPad";
                    }
                    else
                    {
                        if (!line.Contains('='))
                        {
                            strTempSection = string.Empty;
                        }
                        else
                        {
                            strTempKey = line.ToString().Split('=')[0];
                            strTemp = line.ToString().Split('=')[1];
                        }
                    }

                    if (strTempSection.Length > 0 && strTempKey.Length > 0 && strTemp.Length > 0)
                    {
                        DataRow NewDr = dsConfig.Tables[fileName].NewRow();
                        NewDr["Section"] = strTempSection;
                        NewDr["Key"] = strTempKey;
                        NewDr["Value"] = strTemp;
                        dsConfig.Tables[fileName].Rows.Add(NewDr);
                    }
                }

                file.Close();
            }

            if (dsConfig.Tables[fileName].Rows.Count > 0)
            {
                foreach (DataRow dr in dsConfig.Tables[fileName].Rows)
                {
                    string configValue = string.Empty;
                    if (ParseConfigValue(dr["Value"].ToString().Trim(), out configValue))
                    {
                        // Update config directly
                        UpdatePOSConfigValue(fileName, dr["Section"].ToString(), dr["Key"].ToString(), configValue);
                    }
                }   
            }
        }

        /// <summary>
        /// Close this app
        /// Run XCopyApp to copy folder
        /// </summary>
        /// <param name="versionFolder"></param>
        void Step99_UpdateVersionClose(string versionFolder)
        {
            // update pos version
            if (!string.IsNullOrEmpty(m_newVersion))
            {
                UpdatePOSVersion(m_newVersion);
            }

            if (string.IsNullOrEmpty(versionFolder))
            {
                CloseAndRunAppExe(true, string.Empty);
            }
            else
            {
                // copy XCopyApp.exe
                CopySpecialFiles(new string[] { "bin\\" + Path.GetFileName(XCopyAppExe) });

                // end this app
                string encFolderPath = string.IsNullOrEmpty(versionFolder) ? string.Empty :
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(versionFolder));
                CloseAndRunAppExe(false, encFolderPath);
            }
        }

        #endregion

        #region 기타 Utilities

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runPOS"></param>
        /// <param name="runParams"></param>
        void CloseAndRunAppExe(bool runPOS, string runParams)
        {
            this.Close();
            Application.Exit();
            string exePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),
                runPOS ? POSAppExe : XCopyAppExe);

            if (File.Exists(exePath))
            {
                ProcessStartInfo psi = new ProcessStartInfo()
                {
                    FileName = exePath,
                    Arguments = runParams,
                };

                Process.Start(psi);
            }
        }

        /// <summary>
        /// Get list of version in version.xml file
        /// </summary>
        void GetVersionList()
        {
            m_versionList = new List<string>();

            // read xml file to get list of version
            XmlDocument doc = new XmlDocument();
            doc.Load(m_localVersionFile);
            var node = doc.SelectSingleNode("/versions");
            if (!node.HasChildNodes)
            {
                return;
            }

            List<string> versions = new List<string>();
            foreach (XmlNode n in node.ChildNodes)
            {
                versions.Add(n.InnerText);
            }

            // Sorting version
            m_versionList = versions.OrderBy(p => p).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>empty, no version to download</returns>
        string GetNextVersion(string currentVersion)
        {
            if (string.IsNullOrEmpty(currentVersion))
            {

                // read current version
                currentVersion = GetPOSVersion();

                if (string.IsNullOrEmpty(currentVersion) || currentVersion.Length < 11)
                {
                    return m_versionList[0];
                }
            }

            string newVersion = string.Empty;
            Int64 cv = 0;
            Int64.TryParse(currentVersion, out cv);
            foreach (var v in m_versionList)
            {
                Int64 nv = 0;
                Int64.TryParse(v, out nv);

                if (nv > cv)
                {
                    newVersion = nv.ToString();
                    break;
                }
            }

            return newVersion;
        }

        /// <summary>
        /// Update pos to new version
        /// </summary>
        /// <param name="version"></param>
        void UpdatePOSVersion(string version)
        {
            UpdatePOSConfigValue("AppConfig.ini", "PosInfo", "Version", version);
        }

        /// <summary>
        /// Get current pos version
        /// </summary>
        /// <returns></returns>
        string GetPOSVersion()
        {
            return GetPOSConfig("PosInfo", "Version");
        }

        /// <summary>
        /// POS AppConfig.ini setting
        /// </summary>
        /// <param name="section"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        string GetPOSConfig(string section, string config)
        {
            string configFilePath = Path.Combine(m_posConfigFolder, "AppConfig.ini");
            IniDocument doc = new IniDocument(configFilePath);
            return doc.Sections[section].GetValue(config);
        }

        /// <summary>
        /// Update config file value
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="section"></param>
        /// <param name="item"></param>
        /// <param name="value"></param>
        void UpdatePOSConfigValue(string fileName, string section, string item, string value)
        {
            string configFilePath = Path.Combine(m_posConfigFolder, fileName);
            IniDocument doc = new IniDocument(configFilePath);
            doc.Sections[section.Trim()].Set(item.Trim(), value);
            doc.Save(configFilePath);
        }

        /// <summary>
        /// Parse and get value, 
        /// return true if value is applied to my pos
        /// </summary>
        /// <param name="value">[storeno][pos1,pos2]value</param>
        /// <param name="configValue"></param>
        /// <returns></returns>
        bool ParseConfigValue(string value, out string configValue)
        {
            // storeno = 00, posno = 0000: 전체
            configValue = string.Empty;
            var sp = value.Split(new char[] { ']' });
            if (sp.Length != 3)
            {
                return false;
            }

            bool allStore = false;
            bool myStore = false;

            bool allPos = false;
            bool myPos = false;

            string stores = sp[0].Substring(1);
            string[] sl = stores.Split(new char[] { ',' });
            foreach (var sno in sl)
            {
                if ("**".Equals(sno))
                {
                    allStore = true;
                    break;
                }

                if (m_storeNo.Equals(sno))
                {
                    myStore = true;
                    break;
                }
            }

            string poss = sp[1].Substring(1);
            string[] pl = poss.Split(new char[] { ',' });
            foreach (var pno in pl)
            {
                if ("****".Equals(pno))
                {
                    allPos = true;
                    break;
                }

                if (m_posNo.Equals(pno))
                {
                    myPos = true;
                    break;
                }
            }

            configValue = sp[2].Trim();
            if ((allStore || myStore) && (allPos || myPos))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Retrieve StoreNo and PosNo
        /// </summary>
        void GetStorePosInfo()
        {
            m_storeNo = GetPOSConfig("PosInfo", "StoreNo");
            m_posNo = GetPOSConfig("PosInfo", "PosNo");
        }

        /// <summary>
        /// Run patch sql files on mst and tran folder
        /// </summary>
        void RunPatchSql()
        {
            UpdateProgress(this, new UpdateProgressEventArgs()
            {
                StatusMessage = "데이타 스키마 변경 확인 중...",
                Percentage = 0
            });

            string folder = Path.Combine(m_localVersionFolder, "\\data\\patch\\mst");
            if (Directory.Exists(folder))
            {
                RunPatchSql(folder, "master.db");
            }

            folder = Path.Combine(m_localVersionFolder, "\\data\\patch\\tran");
            if (Directory.Exists(folder))
            {
                RunPatchSql(folder, "tran.db");
            }
        }

        /// <summary>
        /// Run all sql files in a folder
        /// </summary>
        /// <param name="folder"></param>
        void RunPatchSql(string folder, string dbFile)
        {
            string[] sqlFiles = Directory.GetFiles(folder, "*.sql");
            foreach (var filePath in sqlFiles)
            {
                string sqlCommand = File.ReadAllText(filePath);

                try
                {
                    string connString = string.Format(CONN_STRING, dbFile);
                    using (SQLiteConnection conn = new SQLiteConnection(connString))
                    {
                        using (var cmd = new SQLiteCommand(sqlCommand, conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            if (conn.State != ConnectionState.Open)
                            {
                                conn.Open();
                            }

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    POSFtpClient.LogMessage(ex);
                }
            }
        }

        /// <summary>
        /// Clear old folders
        /// </summary>
        void CleanOldTempFolders()
        {
            var folders = Directory.GetDirectories(m_tempFolder);
            Int64 pv = 0;
            Int64.TryParse(GetPOSVersion(), out pv);
            foreach (var folder in folders)
            {
                Int64 vf = 0;
                string f = folder.Substring(m_tempFolder.Length);
                f = f.Replace("\\", "");
                Int64.TryParse(f, out vf);
                if (vf < pv)
                {
                    try
                    {
                        Directory.Delete(folder, true);
                    }
                    catch (Exception ex)
                    {
                        POSFtpClient.LogMessage(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Download status
        /// </summary>
        /// <param name="progressMessage"></param>
        void UpdateDownloadFileStatus(string progressMessage)
        {
            if (string.IsNullOrEmpty(progressMessage))
            {
                return;
            }
            string[] fs = progressMessage.Split(new char[] { '/' });
            string fileName = fs[0];
            bool completed = "1".Equals(fs[1]);

            if (!completed)
            {
                string eFileName = string.Empty;
                if (txtLines.Text.Length > 0)
                {
                    eFileName = txtLines.Lines[txtLines.Lines.Length - 1];
                    if (eFileName.StartsWith(fileName))
                    {
                        return;
                    }

                    txtLines.AppendText(Environment.NewLine);
                }

                txtLines.AppendText(string.Format("{0} 다운로드 하는 중...", fileName));
            }
            else
            {
                int i = txtLines.Text.IndexOf(fileName);
                if (i >= 0)
                {
                    txtLines.Text = txtLines.Text.Substring(0, i) +
                        string.Format("{0} 다운로드 완료.", fileName);
                }
            }

            txtLines.ScrollToCaret();
        }

        /// <summary>
        /// Copy files to target folder
        /// </summary>
        /// <param name="fileNames"></param>
        void CopySpecialFiles(string[] fileNames)
        {
            foreach (var fileName in fileNames)
            {
                string filePath = Path.Combine(m_localVersionFolder, fileName);
                if (!File.Exists(filePath))
                {
                    continue;
                }

                string targetFilePath = Path.Combine(m_posFolder, fileName);
                try
                {
                    File.Copy(filePath, targetFilePath, true);
                }
                catch
                {
                }
            }
        }

        #endregion
    }

}
