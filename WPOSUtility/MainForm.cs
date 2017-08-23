using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.FX.Shared.Utils;
using System.IO;
using WSWD.WmallPos.Service.Shared;
using Oracle.DataAccess.Client;
using WSWD.WmallPos.FX.Shared;
using System.Data.SqlClient;

namespace WSWD.WmallPos.POS.Utils
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            OracleDacHelper.DefaultConnectionString = Properties.Settings.Default.WmallDB;
            OracleDacHelper.THROW_EXCEPTION = true;
        }

        #region 메시지 디비화

        /// <summary>
        /// 메시지 전체 일괄등록
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProcMsg_Click(object sender, EventArgs e)
        {
            lstFiles.Items.Clear();
            btnProcMsg.Enabled = false;
            ProcessFolder(txtFolder.Text);

            MessageBox.Show("Done");
            btnProcMsg.Enabled = true;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            var res = dlg.ShowDialog(this);
            if (res == DialogResult.OK && Directory.Exists(dlg.SelectedPath))
            {
                txtFolder.Text = dlg.SelectedPath;
            }
        }

        /// <summary>
        /// Process one folder
        /// </summary>
        /// <param name="folder"></param>
        void ProcessFolder(string folder)
        {
            // show status
            lstFiles.Items.Add(string.Format("Processing...{0}", folder));
            lstFiles.SelectedIndex = lstFiles.Items.Count - 1;

            Application.DoEvents();

            // get list of files exception designer.cs
            var files = Directory.GetFiles(folder, "*.cs", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                string fn = Path.GetFileName(file);
                if (fn.EndsWith("Designer.cs"))
                {
                    continue;
                }

                lstFiles.Items.Add(string.Format("Processing...{0}", fn));
                lstFiles.SelectedIndex = lstFiles.Items.Count - 1;

                Application.DoEvents();
                ProcessFile(file);
            }

            // get sub folder
            var folders = Directory.GetDirectories(folder, "*", SearchOption.TopDirectoryOnly);
            foreach (var fd in folders)
            {
                ProcessFolder(fd);
            }
        }

        void ProcessFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return;
            }

            string content = File.ReadAllText(fileName, Encoding.UTF8);
            var ms = Regex.Matches(content, "\".*?\"");
            int changeCount = 0;
            foreach (Match m in ms)
            {
                if (!m.Success)
                {
                    continue;
                }

                string text = m.Groups[0].Value.Substring(1, m.Groups[0].Value.Length - 2);
                if (!IsKoreanString(text))
                {
                    continue;
                }

                if (!IsStaticMessage(content, m.Groups[0].Index))
                {
                    continue;
                }

                // get inner text                
                var rtext = FindMessageCode(text);
                if (string.IsNullOrEmpty(rtext))
                {
                    continue;
                }

                // replace string
                content = content.Replace(m.Groups[0].Value, rtext);
                changeCount++;
            }

            if (changeCount > 0)
            {
                lstFiles.Items[lstFiles.Items.Count - 1] = lstFiles.Items[lstFiles.Items.Count - 1].ToString() + "processed.";
                File.WriteAllText(fileName, content, Encoding.UTF8);
            }
        }

        bool IsKoreanString(string text)
        {
            return Regex.Match(text, @"[\u3130-\u318F\uAC00-\uD7AF]").Success;
        }

        bool IsStaticMessage(string contentText, int foundIndex)
        {
            // get next \n
            var tidx = contentText.IndexOf('\n', foundIndex);
            int i = tidx - 1;
            while (true)
            {
                if (contentText[i] == '\n')
                {
                    break;
                }

                i--;
            }

            var fidx = i + 1;
            if (fidx != -1 && fidx < tidx)
            {
                string lineText = contentText.Substring(fidx, tidx - fidx);
                return !lineText.Contains("Font(") && (lineText.Contains("static") || lineText.Contains("const") || lineText.Contains("="));
            }

            return false;
        }

        string FindMessageCode(string message)
        {
            string type = string.Empty;
            string msg = ParseCorrectMessage(message, out type);
            try
            {
                using (var dac = new OracleDacHelper())
                {
                    var query = "SELECT CD_MSG FROM SYT012T WHERE CD_MSG_DIV = 'A' AND CD_BIZ_DIV = 'PS' AND NM_MSG = :NM_MSG";
                    var code = dac.ExecuteScalar(query,
                        new string[] {
                        ":NM_MSG"
                    }, new object[] { 
                        msg
                    });

                    string codeMsg = string.Empty;
                    bool found = code != null && !string.IsNullOrEmpty(code.ToString());
                    if (!found)
                    {
                        codeMsg = InsertNewMessage(message);
                    }
                    else
                    {
                        codeMsg = code.ToString();
                    }

                    return string.Format("WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage(\"{0}\")", codeMsg);
                }
            }
            catch (Exception ex)
            {
                new LogFileHandler().LogException(ex);
            }
            finally
            {
            }

            return string.Empty;
        }

        string ParseCorrectMessage(string message, out string type)
        {
            type = "01";
            if (message.StartsWith("ER_"))
            {
                type = "00";
                message = message.Substring(3);
            }
            else if (message.StartsWith("WR_"))
            {
                type = "04";
                message = message.Substring(3);
            }
            else if (message.StartsWith("QS_"))
            {
                type = "03";
                message = message.Substring(3);
            }
            else if (message.StartsWith("IN_"))
            {
                type = "01";
                message = message.Substring(3);
            }
            else
            {
                type = "05";
            }

            return message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        string InsertNewMessage(string message)
        {
            string type = "01";
            message = ParseCorrectMessage(message, out type);

            string cdMsg = string.Empty;
            OracleDacHelper db = null;
            OracleTransaction trans = null;

            try
            {
                db = new OracleDacHelper();
                trans = db.BeginTransaction();

                var query = "SELECT NVL(MAX(CD_MSG),'00000') FROM SYT012T WHERE CD_MSG_DIV = 'A' AND CD_BIZ_DIV = 'PS'";
                var max = db.ExecuteScalar(query, null, null, trans);
                int nm = TypeHelper.ToInt32(max) + 1;
                cdMsg = nm.ToString("d5");

                query = string.Format(@"INSERT INTO SYT012T(CD_MSG_DIV,CD_BIZ_DIV,CD_MSG,CD_MSG_TYPE,NM_MSG_TITLE,NM_MSG,
                                        NM_DESC,FG_USE,DT_CRTN,NO_CRTN,DT_CHG,NO_CHG,TT_IUDT) 
                        VALUES('A','PS',:CD_MSG,:MSG_TYPE,'  ',:NM_MSG,
                                        '','1',sysdate,'9999999',sysdate,'9999999','{0:yyyyMMddHHmmss}')", DateTime.Now);
                db.ExecuteNonQuery(query,
                    new string[] {
                        ":CD_MSG",
                        ":MSG_TYPE",
                        ":NM_MSG"
                    }, new object[] { 
                        cdMsg,
                        type,
                        message
                    }, trans);

                trans.Commit();

            }
            catch (Exception ex)
            {
                new LogFileHandler().LogException(ex);
                trans.Rollback();
            }
            finally
            {
                db.Dispose();
                trans.Dispose();
            }


            return cdMsg;
        }

        #endregion

        #region Utils

        static string fnGetBarCodeCheckDigit(string barcode)
        {
            string cNum = null;
            long iSum = 0;
            long iBuf = 0;

            for (int i = 0; i < barcode.Length; i++)
            {
                cNum = barcode.Substring(i, 1);
                if ((i + 1) % 2 == 1)
                    iSum = iSum + Convert.ToInt64(cNum);
                else
                    iSum = iSum + Convert.ToInt64(cNum) * 3;
            }

            iBuf = iSum / 10;
            iBuf = (iBuf + 1) * 10;

            return Convert.ToString((iBuf - iSum) % 10);
        }

        #endregion

        #region Encoding/Decoding

        private void btnEnc_Click(object sender, EventArgs e)
        {
            txtEncText.Text = Convert.ToBase64String(Encoding.UTF8.GetBytes(txtText.Text));
        }

        private void btnDec_Click(object sender, EventArgs e)
        {
            txtDecText.Text = Encoding.UTF8.GetString(Convert.FromBase64String(txtEncText.Text));
        }
        #endregion

        #region 메시지 중복 삭제 및 소스 처리

        /// <summary>
        /// 메시지 중복 처리
        /// 1) 중복 메시지 코드 찾기
        /// 2) 한 코드만 사용, 나머지 삭제
        /// 3) 삭제 하기 전에 소스 바꾸고 저장 & 메시지 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartChkDupMsg_Click(object sender, EventArgs e)
        {
            try
            {
                btnStartChkDupMsg.Enabled = false;
                txtDupSrcPath.ReadOnly = true;
                btnBrowse1.Enabled = false;
                ProcessDupMsgFiles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnStartChkDupMsg.Enabled = true;
                txtDupSrcPath.ReadOnly = false;
                btnBrowse1.Enabled = true;
            }
        }

        /// <summary>
        /// 메시지 중복 처리
        /// 1) 중복 메시지 코드 찾기
        /// 2) 한 코드만 사용, 나머지 삭제
        /// 3) 삭제 하기 전에 소스 바꾸고 저장 & 메시지 삭제
        /// </summary>
        private void ProcessDupMsgFiles()
        {
            #region Find dup msg and and get list of code

            lblProgress.Text = "중복 메시지 검색 중...";
            Application.DoEvents();

            /*
             SELECT * FROM SYT012T mto 
	            INNER JOIN 
	            (SELECT NM_MSG FROM SYT012T mtp 
		            WHERE mtp.CD_BIZ_DIV = 'PS' AND mtp.CD_MSG_DIV = 'P'
		            GROUP BY mtp.NM_MSG
		            HAVING COUNT(*) > 1
		            ) mtp ON mto.NM_MSG = mtp.NM_MSG
            WHERE mto.CD_BIZ_DIV = 'PS' AND mto.CD_MSG_DIV = 'P'
            ORDER BY mto.NM_MSG DESC;
             * 
            */

            Dictionary<string, List<string>> dupCodes = new Dictionary<string, List<string>>();
            using (var dac = new OracleDacHelper())
            {
                var query = "SELECT * FROM SYT012T mto INNER JOIN (SELECT NM_MSG FROM SYT012T mtp WHERE mtp.CD_BIZ_DIV = 'PS' AND mtp.CD_MSG_DIV = 'A' GROUP BY mtp.NM_MSG HAVING COUNT(*) > 1) mtp ON mto.NM_MSG = mtp.NM_MSG WHERE mto.CD_BIZ_DIV = 'PS' AND mto.CD_MSG_DIV = 'A' ORDER BY mto.NM_MSG DESC, mto.CD_MSG ASC";

                var dupDataset = dac.ExecuteQuery(query, null, null);

                string lastNmMsg = string.Empty;
                string lastCdMsg = string.Empty;
                foreach (DataRow dr in dupDataset.Tables[0].Rows)
                {
                    List<string> lst = null;

                    string cdMsg = dr["CD_MSG"].ToString();
                    string nmMsg = dr["NM_MSG"].ToString();

                    if (!lastNmMsg.Equals(nmMsg))
                    {
                        lst = new List<string>();
                        dupCodes.Add(cdMsg, lst);

                        lastCdMsg = cdMsg;
                        lastNmMsg = nmMsg;
                    }
                    else
                    {
                        lst = dupCodes[lastCdMsg];
                    }

                    lst.Add(cdMsg);
                }
            }

            #endregion

            #region Search and get related cs file

            lblProgress.Text = "처리 할 소스 파일 리스트 만드는 중...";
            Application.DoEvents();

            // get list of files exception designer.cs
            List<string> processingFiles = new List<string>();
            var files = Directory.GetFiles(txtDupSrcPath.Text, "*.cs", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                try
                {
                    string fileContent = File.ReadAllText(file, Encoding.UTF8);
                    if (fileContent.Contains("ConfigData.Current.SysMessage.GetMessage"))
                    {
                        processingFiles.Add(file);

                        lblProgress.Text = "처리 할 소스 파일 리스트 만드는 중..." + Path.GetFileName(file);
                        Application.DoEvents();
                    }
                }
                catch
                {
                }
            }

            #endregion

            #region Search in POS source, replace all code with first code if above

            lblProgress.Text = "중복 메시지를 처리 중...";
            Application.DoEvents();

            // dupCodes
            // Search in each key of dupCodes, find all msg code, replace with keyCode
            foreach (var file in processingFiles)
            {
                try
                {
                    string fileContent = File.ReadAllText(file, Encoding.UTF8);
                    lblProgress.Text = "중복 메시지를 처리 중..." + Path.GetFileName(file);
                    Application.DoEvents();

                    bool changed = false;

                    foreach (var key in dupCodes.Keys)
                    {
                        List<string> lst = dupCodes[key];
                        lst.Remove(key);

                        foreach (var code in lst)
                        {
                            string rt = string.Format("Current.SysMessage.GetMessage(\"{0}\")", code);
                            if (!fileContent.Contains(rt))
                            {
                                continue;
                            }

                            fileContent = fileContent.Replace(rt, string.Format("Current.SysMessage.GetMessage(\"{0}\")", key));
                            changed = true;
                        }
                    }

                    if (changed)
                    {
                        File.WriteAllText(file, fileContent, Encoding.UTF8);
                    }
                }
                catch
                {
                }
            }

            lblProgress.Text = "중복 메시지를 처리 완료..." + processingFiles.Count.ToString();
            Application.DoEvents();

            #endregion

            #region 메시지를 디비에서 삭제

            // 메시지를 디비에서 삭제
            lblProgress.Text = "중복 메시지를 처리 중...";
            Application.DoEvents();

            using (var dac = new OracleDacHelper())
            {
                foreach (var key in dupCodes.Keys)
                {
                    List<string> lst = dupCodes[key];
                    lst.Remove(key);
                    foreach (var code in lst)
                    {
                        // Delete from DB
                        var query = string.Format("DELETE FROM SYT012T mtp WHERE mtp.CD_BIZ_DIV = 'PS' AND mtp.CD_MSG_DIV = 'A' AND mtp.CD_MSG = '{0}'", code);
                        dac.ExecuteNonQuery(query, null, null);
                    }
                }
            }

            #endregion

            lblProgress.Text = "작업 완료.";
            Application.DoEvents();
        }

        /// <summary>
        /// 메시지 중복 할 경로 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            var res = dlg.ShowDialog(this);
            if (res == DialogResult.OK && Directory.Exists(dlg.SelectedPath))
            {
                txtDupSrcPath.Text = dlg.SelectedPath;
            }
        }

        #endregion


    }

    #region 기타 classes

    /// <summary>
    /// TRACE내용을 파일로저장한다
    /// </summary>
    public class LogFileHandler : IFileLogHandler
    {
        #region IFileLogHandler Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="args"></param>
        public void Log(string logMessage, params object[] args)
        {
            LogByType(LogTypes.Program.ToString().ToLower(), logMessage, args);
        }

        /// <summary>
        /// Exception log
        /// </summary>
        /// <param name="ex"></param>
        public void LogException(Exception ex)
        {
            LogByType("error", Format(ex));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="args"></param>
        public void LogException(string logMessage, params object[] args)
        {
            LogByType("error", logMessage, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="logMessage"></param>
        /// <param name="args"></param>
        public void LogByType(string logType, string logMessage, params object[] args)
        {
            string logFolder = Path.Combine(LogBaseDirectory(), logType);
            string logFile = Path.Combine(logFolder, string.Format("{1}_{0:yyyyMMdd}.log", DateTime.Today, logType));
            WriteLog(logFile, args != null && args.Length > 0 ? string.Format(logMessage, args) : logMessage);
        }

        #endregion

        #region Privates

        #region Journal, Trace, Log Writing

        /// <summary>
        /// Log folder
        /// </summary>
        /// <returns></returns>
        private string LogBaseDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "\\log";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logFilePath"></param>
        /// <param name="logMessage"></param>
        private void WriteLog(string logFilePath, string logMessage)
        {
            StreamWriter sw = null;
            try
            {
                string logFolder = Path.GetDirectoryName(logFilePath);
                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }

                if (!File.Exists(logFilePath))
                {
                    File.Create(logFilePath).Close();
                }

                sw = new StreamWriter(logFilePath, true);
                string writeMessage = string.Format("[{0:yyyy-MM-dd HH:mm:ss}]\n{1}", DateTime.Now, logMessage);
                sw.WriteLine(writeMessage);
            }
            catch
            {
            }
            finally
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        #endregion

        #endregion

        #region Exception helper

        /// <summary>
        /// Format exception
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        static public string Format(System.Exception exception)
        {
            string tmp = string.Empty;
            StringBuilder msg = null;
            DateTime dateTime = DateTime.Now;
            try
            {
                msg = new StringBuilder();
                msg.AppendLine();
                msg.AppendLine("==============[System Error Tracing]==============");
                msg.AppendLine("[CallStackTrace]");
                msg.AppendLine(exception.StackTrace);

                //msg에 현재 트래이싱된 시간을 담도록 한다.
                msg.AppendLine("\r\n[DateTime] : " + dateTime.ToString("yyyy-MM-dd HH:mm:ss"));

                //만약 Sql관련 Exception이면 추가항목을 넣어주도록 한다.
                if (exception.GetType() == typeof(System.Data.SqlClient.SqlException))
                {
                    SqlException sqlErr = (SqlException)exception;
                    msg.Append("\r\n[SqlException] ");
                    msg.Append("\r\nException Type: ").Append(sqlErr.GetType());
                    msg.Append("\r\nErrors: ").Append(sqlErr.Errors);
                    msg.Append("\r\nClass: ").Append(sqlErr.Class);
                    msg.Append("\r\nLineNumber: ").Append(sqlErr.LineNumber);
                    msg.Append("\r\nMessage: ").Append("{" + sqlErr.Message + "}");
                    msg.Append("\r\nNumber: ").Append(sqlErr.Number);
                    msg.Append("\r\nProcedure: ").Append(sqlErr.Procedure);
                    msg.Append("\r\nServer: ").Append(sqlErr.Server);
                    msg.Append("\r\nState: ").Append(sqlErr.State);
                    msg.Append("\r\nSource: ").Append(sqlErr.Source);
                }
                //Sql관련Exception외에 작업들..
                else
                {
                    msg.Append("\r\n[Exception] ");
                    msg.Append("\r\n" + "DetailMsg: {" + exception.Message + "}");
                }
            }
            catch
            {
                //throw ex;
            }

            return msg.ToString();
        }

        #endregion
    }

    #endregion
}
