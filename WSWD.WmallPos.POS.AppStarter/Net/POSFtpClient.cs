using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;


using POSFileInfoCol = System.Collections.Generic.List<WSWD.WmallPos.POS.AppStarter.Net.POSFileSystemInfo>;
using POSFtpTranferCol = System.Collections.Generic.List<WSWD.WmallPos.POS.AppStarter.Net.POSFtpTranferInfo>;
using System.IO;
using System.Globalization;
using System.Windows.Forms;
using System.Threading;

namespace WSWD.WmallPos.POS.AppStarter.Net
{
    /// <summary>
    /// W-MALL POS FTP Client
    /// 개발자 : TCL
    /// 개발일 : 06.22
    /// </summary>
    public class POSFtpClient : WebClient
    {
        static Regex m_UnixListLineExpression = new Regex(@"(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\w+\s+\w+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{4})\s+(?<name>[^\r\n]+)(\n|\r)");
        static Regex m_UnixListLineExpression1 = new Regex(@"(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\d+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{4})\s+(?<name>[^\r\n]+)(\n|\r)");
        static Regex m_UnixListLineExpression2 = new Regex(@"(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\d+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{2}:\d{2})\s+(?<name>[^\r\n]+)(\n|\r)");
        static Regex m_DosListLineExpression = new Regex(@"(?<timestamp>\d{2}\-\d{2}\-\d{2}\s+\d{2}:\d{2}[Aa|Pp][mM])\s+(?<dir>\<\w+\>){0,1}(?<size>\d+){0,1}\s+(?<name>[^\r\n]+)(\n|\r)");

        public const int ProgressCompleted = int.MaxValue;
        public const int ProgressFailed = int.MinValue;
        private List<POSFileSystemInfo> m_allFiles = null;
        private const int CONN_TIMES = 3;

        public event EventHandler<ProgressEventArgs> ProgressChanged;

        /// <summary>
        /// Connection failed event
        /// </summary>
        public event EventHandler<ConnectionFailedEventArgs> ConnectionFailed;

        private void RaiseBatchProgressChange(string serverFile, string localFile, int threadId, int percentage)
        {
            if (ProgressChanged != null)
            {
                if (m_allFiles != null)
                {
                    var fileInfo = FindFileSystemInfor(m_allFiles, serverFile);
                    if (fileInfo != null)
                    {
                        if (percentage != ProgressCompleted)
                        {
                            var pc = Convert.ToInt64(Convert.ToDouble(percentage) * 100 / fileInfo.Length);

                            if (pc > ProgressCompleted)
                            {
                                percentage = ProgressCompleted;
                            }
                            else
                            {
                                percentage = Convert.ToInt32(pc);
                            }
                        }
                        else
                        {
                            // set last modified datetime
                            File.SetLastWriteTime(localFile, fileInfo.LastModifiedTime);
                        }

                        ProgressChanged(this, new ProgressEventArgs(serverFile, localFile, threadId, percentage, 0));
                        return;
                    }

                    ProgressChanged(this, new ProgressEventArgs(serverFile, localFile, threadId, percentage, 0));
                    return;
                }

                ProgressChanged(this, new ProgressEventArgs(serverFile, localFile, threadId, percentage, 0));
            }
        }

        private string m_Server;
        private short m_Port;
        private object m_CriticalFlag = new object();

        /// <summary>
        /// Server IP
        /// </summary>
        public string Server
        {
            get { return m_Server; }
        }

        /// <summary>
        /// Port
        /// </summary>
        public short Port
        {
            get { return m_Port; }
        }

        public int RemainFileCount
        {
            get
            {
                return m_DownloadingCollection.Count;
            }
        }

        /// <summary>
        /// 다운로드 실패건수
        /// </summary>
        public int DownloadFailedCount { get; private set; }

        /// <summary>
        /// Total bytes to download
        /// </summary>
        public Int64 TotalBytesCount
        {
            get
            {
                if (m_allFiles != null)
                {
                    return m_allFiles.Sum(p => p.Length);
                }
                return 1;
            }
        }

        /// <summary>
        /// POS FTP Client Constructor
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="credentical"></param>
        public POSFtpClient(string server, short port, NetworkCredential credentical)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 1000;
            m_Server = server;
            m_Port = port;
            Credentials = credentical;

        }

        #region Folder Listing

        public POSFileInfoCol ListDirectoryDetail(string directory)
        {
            POSFileInfoCol result = new POSFileInfoCol();
            string response = string.Empty;
            response = string.Empty;

            int connTimes = CONN_TIMES;
            bool checkOK = false;

            while (!checkOK && connTimes > 0)
            {
                try
                {
                    Uri uri = new Uri(GetAbsolutePath(directory, true));
                    FtpWebRequest request = (FtpWebRequest)GetWebRequest(uri);
                    request.Timeout = 5000;
                    request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                    request.UsePassive = true;

                    WebResponse rs = GetWebResponse(request);
                    Stream dataStream = rs.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    response = reader.ReadToEnd();

                    dataStream.Close();
                    reader.Close();
                    rs.Close();

                    checkOK = true;

                    // Log to test
                    LogMessage(response);
                }
                catch (Exception e)
                {
                    LogMessage(e);
                    connTimes--;
                    checkOK = false;

                    if (connTimes <= 0)
                    {
                        if (ConnectionFailed != null)
                        {
                            ConnectionFailedEventArgs ev = new ConnectionFailedEventArgs();
                            ConnectionFailed(this, ev);
                            if (ev.Retry)
                            {
                                connTimes = CONN_TIMES;
                            }
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(3000);
                    }
                }
                finally
                {

                }
            }

            Regex reg = GetDirectoryReg(response);
            if (reg != null)
            {
                MatchCollection matches = reg.Matches(response);
                foreach (Match match in matches)
                {
                    POSFileSystemInfo fileInfor;
                    string dir = match.Groups["dir"].Value;
                    if (dir != "" && dir != "-")
                    {
                        fileInfor = new POSDirectoryInfor();
                    }
                    else
                    {
                        fileInfor = new POSFileInfo();
                    }

                    fileInfor.Name = match.Groups["name"].Value;

                    DateTimeFormatInfo invariantDateTimeInfo = new DateTimeFormatInfo();
                    DateTime time = DateTime.Now;
                    bool canParse = DateTime.TryParse(match.Groups["timestamp"].Value, invariantDateTimeInfo, DateTimeStyles.None, out time);
                    if (!canParse)
                    {
                        time = DateTime.Now;
                    }

                    fileInfor.LastModifiedTime = time;
                    int size = 0;
                    int.TryParse(match.Groups["size"].Value, out size);
                    fileInfor.Length = size;

                    fileInfor.Permission = match.Groups["permission"].Value;
                    result.Add(fileInfor);
                }
            }

            return result;
        }

        #endregion

        #region Download

        private POSFtpTranferCol m_DownloadingCollection = new POSFtpTranferCol();
        public POSFtpTranferInfo[] DownloadingCollection
        {
            get { return m_DownloadingCollection.ToArray(); }
        }

        public void StartNewFolderDownload()
        {
            m_allFiles = new List<POSFileSystemInfo>();
            DownloadFailedCount = 0;
        }

        /// <summary>
        /// Download files in thread
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="localPath"></param>
        /// <param name="fileNames"></param>
        /// <param name="maxThread"></param>
        public void DownloadFilesAsync(string serverPath, string localPath, string[] fileNames, int threadId)
        {
            lock (m_CriticalFlag)
            {
                string tempPath = serverPath.Substring(serverPath.LastIndexOf("/") + 1);
                tempPath = tempPath.Trim();
                string newLocalPath = string.Format("{0}\\{1}", localPath, tempPath);
                foreach (string fileName in fileNames)
                {
                    POSFtpTranferInfo tranferInfo = new POSFtpTranferInfo(serverPath + "/" +
                        fileName, localPath + "\\" + fileName, false, fileName);
                    m_DownloadingCollection.Add(tranferInfo);
                }
            }

            StartDownload(threadId);
        }

        /// <summary>
        /// Download a folder
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="localPath"></param>
        /// <param name="maxThread"></param>
        public void DownloadFolderAsync(string serverPath, string localPath, short threadId)
        {
            string tempPath = serverPath.Substring(serverPath.LastIndexOf("/") + 1);
            tempPath = tempPath.Trim();
            string newLocalPath = string.Format("{0}\\{1}", localPath, tempPath);

            lock (m_CriticalFlag)
            {
                POSFtpTranferInfo tranferInfo = new POSFtpTranferInfo(serverPath, newLocalPath, true);
                m_DownloadingCollection.Add(tranferInfo);
            }

            StartDownload(threadId);
        }

        /// <summary>
        /// Start download by thread
        /// </summary>
        /// <param name="maxThread"></param>
        private void StartDownload(int threadId)
        {
            lock (m_CriticalFlag)
            {
                Thread thread = new Thread(delegate()
                {
                    InternalDownloadFolderAsyn(threadId);
                });
                thread.Start();
            }
        }

        /// <summary>
        /// Downloading folder by a thread
        /// </summary>
        /// <param name="threadId"></param>
        private void InternalDownloadFolderAsyn(int threadId)
        {
            POSFtpTranferInfo tranferInfo = null;
            lock (m_CriticalFlag)
            {
                if (m_DownloadingCollection.Count > 0)
                {
                    tranferInfo = m_DownloadingCollection[0];
                    m_DownloadingCollection.RemoveAt(0);
                }
            }

            if (tranferInfo != null && tranferInfo.IsDirectory)
            {
                UpdateDownloadList(tranferInfo);
                InternalDownloadFolderAsyn(threadId);
            }
            else if (tranferInfo != null)
            {
                DownloadOneFile(tranferInfo.ServerPath, tranferInfo.LocalPath, threadId);
            }
        }

        private void UpdateDownloadList(POSFtpTranferInfo tranferInfo)
        {
            lock (m_CriticalFlag)
            {
                List<POSFileSystemInfo> filesInfor = ListDirectoryDetail(tranferInfo.ServerPath);

                //Add files to list first
                foreach (POSFileSystemInfo fileInfor in filesInfor)
                {
                    if (fileInfor is POSFileInfo)
                    {
                        string newServerPath = tranferInfo.ServerPath + "/" + fileInfor.Name;
                        string newLocalPath = tranferInfo.LocalPath + "\\" + fileInfor.Name;
                        POSFtpTranferInfo newItem = new POSFtpTranferInfo(newServerPath, newLocalPath, false);
                        newItem.SourceLength = fileInfor.Length;
                        newItem.SourceLastModified = fileInfor.LastModifiedTime;
                        m_DownloadingCollection.Add(newItem);

                        fileInfor.FullName = newServerPath;
                        m_allFiles.Add(fileInfor);
                    }
                }

                //Add Folder to list
                foreach (POSFileSystemInfo fileInfor in filesInfor)
                {
                    if (fileInfor is POSDirectoryInfor)
                    {
                        string newServerPath = tranferInfo.ServerPath + "/" + fileInfor.Name;
                        string newLocalPath = tranferInfo.LocalPath + "\\" + fileInfor.Name;
                        POSFtpTranferInfo newItem = new POSFtpTranferInfo(newServerPath, newLocalPath, true);
                        m_DownloadingCollection.Add(newItem);
                    }
                }
            }
        }

        /// <summary>
        /// Download a single file
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="localFile"></param>
        /// <param name="threadId"></param>
        private void DownloadOneFile(string serverPath, string localFile, int threadId)
        {
            bool transferred = false;
            Stream dataStream = null;
            FileStream fileStream = null;
            FtpWebRequest request = null;
            FtpWebResponse response = null;
            int connTimes = CONN_TIMES;

        _retry:

            try
            {
                string ftpUrl = GetAbsolutePath(serverPath, false);
                request = (FtpWebRequest)FtpWebRequest.Create(ftpUrl);
                request.Timeout = 5000;
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.UseBinary = true;
                request.UsePassive = true;
                request.Credentials = Credentials;

                response = (FtpWebResponse)request.GetResponse();
                dataStream = response.GetResponseStream();

                // Check existing file
                if (File.Exists(localFile))
                {
                    try
                    {
                        File.SetAttributes(localFile, FileAttributes.Normal);
                        File.Delete(localFile);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    if (!Directory.Exists(Path.GetDirectoryName(localFile)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(localFile));
                    }
                }

                fileStream = File.Create(localFile);

                int totalBytesReaded = 0;
                byte[] buffer = new byte[8192];
                int bytesReaded = dataStream.Read(buffer, 0, buffer.Length);

                while (bytesReaded > 0)
                {
                    fileStream.Write(buffer, 0, bytesReaded);
                    totalBytesReaded += bytesReaded;
                    bytesReaded = dataStream.Read(buffer, 0, buffer.Length);
                    RaiseBatchProgressChange(serverPath, localFile, threadId, bytesReaded);
                }

                fileStream.Close();
                transferred = true;
                RaiseBatchProgressChange(serverPath, localFile, threadId, ProgressCompleted);

                InternalDownloadFolderAsyn(threadId);
            }
            catch (Exception ex)
            {
                LogMessage(ex);
            }
            finally
            {
                if (dataStream != null)
                {
                    dataStream.Dispose();
                }
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
                if (response != null)
                {
                    response.Close();
                }
            }

            if (!transferred)
            {
                connTimes--;

                if (connTimes > 0)
                {
                    goto _retry;
                }

                if (ConnectionFailed != null)
                {
                    ConnectionFailedEventArgs ev = new ConnectionFailedEventArgs();
                    ConnectionFailed(this, ev);
                    if (ev.Retry)
                    {
                        connTimes = CONN_TIMES;
                        goto _retry;
                    }
                }

                // report failed download
                RaiseBatchProgressChange(serverPath, localFile, threadId, ProgressFailed);
                InternalDownloadFolderAsyn(threadId);
                DownloadFailedCount++;
            }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// FTP 연결 테스트
        /// </summary>
        /// <param name="url"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool TestConnection()
        {
            int rCount = 3;
            _retry:

            var avail = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            if (!avail)
            {
                Thread.Sleep(5000);
                goto _retry;
            }

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + m_Server);
                request.Timeout = 1000 * 5;
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = Credentials;
                request.GetResponse();
            }
            catch (WebException ex)
            {
                rCount--;
                if (rCount <= 0)
                {
                    return false;
                }
                else
                {
                    goto _retry;
                }
            }

            return true;
        }

        /// <summary>
        /// Ftp server ready or not
        /// </summary>
        /// <returns></returns>
        private bool IsServerReady()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("ftp://" + m_Server + ":" + m_Port.ToString()))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Find file info in list
        /// </summary>
        /// <param name="filesInfor"></param>
        /// <param name="fullName"></param>
        /// <returns></returns>
        private POSFileSystemInfo FindFileSystemInfor(List<POSFileSystemInfo> filesInfor, string fullName)
        {
            foreach (POSFileSystemInfo fileInfo in filesInfor)
            {
                if (fileInfo.FullName == fullName)
                {
                    return fileInfo;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="isFolder"></param>
        /// <returns></returns>
        private string GetAbsolutePath(string relativePath, bool isFolder)
        {
            return string.Format("ftp://{0}:{1}/{2}{3}", m_Server, Port.ToString(),
                relativePath.StartsWith("/") ? relativePath.Substring(1) : relativePath,
                isFolder ? "/" : "");
        }

        /// <summary>
        /// Log exception to file
        /// </summary>
        /// <param name="ex"></param>
        static public void LogMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendFormat("[Error] {0}", ex.Message);
            sb.AppendLine();
            sb.AppendLine("[StackTrace]");
            sb.Append(ex.StackTrace);
            LogMessage(sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        static public void LogMessage(string msg)
        {
            StreamWriter sw = null;
            try
            {
                string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }
                string logFile = Path.Combine(logFolder, string.Format("{0:yyyyMMdd}.log", DateTime.Today));
                if (!File.Exists(logFile))
                {
                    File.Create(logFile).Close();
                }

                sw = new StreamWriter(logFile, true, Encoding.UTF8);
                sw.WriteLine(string.Format("[{0:yyyy-MM-dd HH:mm:ss}] {1}", DateTime.Now, msg));
            }
            catch
            {
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        private Regex GetDirectoryReg(string sample)
        {
            Match match;
            match = m_DosListLineExpression.Match(sample);
            if (match.Success) return m_DosListLineExpression;

            match = m_UnixListLineExpression.Match(sample);
            if (match.Success) return m_UnixListLineExpression;

            match = m_UnixListLineExpression2.Match(sample);
            if (match.Success) return m_UnixListLineExpression2;

            match = m_UnixListLineExpression1.Match(sample);
            if (match.Success) return m_UnixListLineExpression1;

            return null;
        }

        #endregion
    }
}
