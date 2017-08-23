using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace WSWD.WmallPos.POS.VersionManager.Utils
{
    public class clsUtilsFTP
    {
        public delegate void FtpEventDelegate(int barValue, int barMax);
        public event FtpEventDelegate ftpEvent;

        #region 변수

        /// <summary>
        /// FTP 기본주소
        /// </summary>
        private string strBaseUrl = "ftp://";

        /// <summary>
        /// FTP 기본폴더주소
        /// </summary>
        private string strBasePath = ".";

        /// <summary>
        /// 버전 파일명
        /// </summary>
        private string strVersionFile = "version.xml";

        /// <summary>
        /// FTP 주소
        /// </summary>
        public string strFullPath = string.Empty;

        /// <summary>
        /// 서버IP
        /// </summary>
        public string strServerIP = string.Empty;

        /// <summary>
        /// 서버 포트
        /// </summary>
        public string strServerPort = string.Empty;

        /// <summary>
        /// 사용자 아이디
        /// </summary>
        public string strUserID = string.Empty;

        /// <summary>
        /// 사용자 암호
        /// </summary>
        public string strUserPass = string.Empty;

        /// <summary>
        /// FTP 타임 아웃
        /// </summary>
        public int iTimeOut = 10000;

        /// <summary>
        /// 성공여부
        /// </summary>
        private bool bSuccess = false;
        public bool _bSuccess
        {
            get { return this.bSuccess; }
            set { this.bSuccess = value; }
        }

        /// <summary>
        /// 에러메세지
        /// </summary>
        private string strErrMsg = string.Empty;
        public string _strErrMsg
        {
            get { return this.strErrMsg; }
            set { this.strErrMsg = value; }
        }

        #endregion

        #region 생성자

        private static clsUtilsFTP m_instance = new clsUtilsFTP();
        public static clsUtilsFTP Instance
        {
            get
            {
                return m_instance;
            }
        }

        ///// <summary>
        ///// FTP WebClient
        ///// </summary>
        ///// <param name="_strServerIP">서버IP</param>
        ///// <param name="strUserID">사용자 아이디</param>
        ///// <param name="strUserPass">사용자 암호</param>
        //public FtpWebClient(string _strServerIP, string _strUserID, string _strUserPass)
        //{
        //    strServerIP = _strServerIP;
        //    strServerPort = "21";
        //    strUserID = _strUserID;
        //    strUserPass = _strUserPass;
        //}

        ///// <summary>
        ///// FTP WebClient
        ///// </summary>
        ///// <param name="_strServerIP">서버IP</param>
        ///// <param name="strServerPort">서버 포트</param>
        ///// <param name="strUserID">사용자 아이디</param>
        ///// <param name="strUserPass">사용자 암호</param>
        //public FtpWebClient(string _strServerIP, string _strServerPort, string _strUserID, string _strUserPass)
        //{
        //    strServerIP = _strServerIP;
        //    strServerPort = _strServerPort;
        //    strUserID = _strUserID;
        //    strUserPass = _strUserPass;
        //}

        ///// <summary>
        ///// FTP WebClient
        ///// </summary>
        ///// <param name="_strServerIP">서버IP</param>
        ///// <param name="strServerPort">서버 포트</param>
        ///// <param name="strUserID">사용자 아이디</param>
        ///// <param name="strUserPass">사용자 암호</param>
        ///// <param name="_iTimeOut">타임아웃 설정</param>
        //public FtpWebClient(string _strServerIP, string _strServerPort, string _strUserID, string _strUserPass, int _iTimeOut)
        //{
        //    strServerIP = _strServerIP;
        //    strServerPort = _strServerPort;
        //    strUserID = _strUserID;
        //    strUserPass = _strUserPass;
        //    iTimeOut = _iTimeOut;
        //}

        #endregion

        #region 다운로드

        /// <summary>
        /// 다운로드
        /// </summary>
        /// <param name="strServerPath">서버 주소(파일명포함)</param>
        /// <returns></returns>
        public bool FtpDownloadAll(string strServerPath)
        {
            return FtpDownloadAll(strServerPath, "");
        }

        /// <summary>
        /// 다운로드
        /// </summary>
        /// <param name="strServerPath">서버 주소</param>
        /// <param name="strLocalPath">다운로드받을 주소(파일명을 제외한 경로 : 파일명은 서버의 파일명으로 대체)</param>
        /// <returns></returns>
        public bool FtpDownloadAll(string strServerPath, string strLocalPath)
        {
            bool bReturn = false;

            try
            {
                //로컬 주소 확인
                if (strLocalPath.Length <= 0)
                {
                    strLocalPath = Application.StartupPath;
                }

                List<clsDirectoryItem> list = FtpGetList(strServerPath, true);

                if (list != null && list.Count > 0)
                {
                    foreach (clsDirectoryItem item in list)
                    {
                        string strPath = item.ParentNm + "/" + item.Name;

                        if (item.IsDirectory)
                        {
                            if (!LocalMakeDirectory(strLocalPath + strPath)) return false;
                        }
                        else
                        {
                            //서버 풀 주소(파일명 포함)
                            strFullPath = string.Format("{0}{1}:{2}/{3}", strBaseUrl, strServerIP, strServerPort, strPath);

                            FtpWebRequest req = GetRequest(WebRequestMethods.Ftp.GetFileSize);
                            long fileSize;
                            using (FtpWebResponse resp = (FtpWebResponse)req.GetResponse())
                            {
                                fileSize = resp.ContentLength;
                            }

                            req = GetRequest(WebRequestMethods.Ftp.DownloadFile);

                            // FTP Request 결과를 가져온다.
                            using (FtpWebResponse resp = (FtpWebResponse)req.GetResponse())
                            {
                                // FTP 결과 스트림
                                using (Stream stream = resp.GetResponseStream())
                                {
                                    using (FileStream writeStream = new FileStream(strLocalPath + strPath, FileMode.Create))
                                    {
                                        var buffer = new byte[1024 * 1024];
                                        int totalReadBytesCount = 0;
                                        int readBytesCount;

                                        while ((readBytesCount = stream.Read(buffer, 0, buffer.Length)) > 0)
                                        {
                                            writeStream.Write(buffer, 0, readBytesCount);
                                            totalReadBytesCount += readBytesCount;

                                            int totalSize = (int)(fileSize) / 1000; // Kbytes

                                            if (ftpEvent != null)
                                            {
                                                //ftpEvent((totalReadBytesCount / 1000), totalSize);
                                                ftpEvent(0, 0);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                bSuccess = false;
                strErrMsg = ex.Message.ToString();
            }
            finally
            {

            }

            return bReturn;
        }

        /// <summary>
        /// 다운로드
        /// </summary>
        /// <param name="strServerPath">서버 주소(파일명포함)</param>
        /// <returns></returns>
        public bool FtpDownload(string strServerPath)
        {
            return FtpDownload(strServerPath, "", "");
        }

        /// <summary>
        /// 다운로드
        /// </summary>
        /// <param name="strServerPath">서버 주소(파일명포함)</param>
        /// <param name="strLocalPath">다운로드받을 주소(파일명포함)</param>
        /// <returns></returns>
        public bool FtpDownload(string strServerPath, string strLocalPath, string strLocalFileNm)
        {
            bool bReturn = false;

            try
            {
                //로컬 주소 확인
                if (strLocalPath.Length <= 0)
                {
                    strLocalPath = Application.StartupPath + strServerPath;
                }

                //서버 풀 주소(파일명 포함)
                strFullPath = string.Format("{0}{1}:{2}/{3}", strBaseUrl, strServerIP, strServerPort, strServerPath);

                FtpWebRequest req = GetRequest(WebRequestMethods.Ftp.GetFileSize);
                long fileSize;
                using (FtpWebResponse resp = (FtpWebResponse)req.GetResponse())
                {
                    fileSize = resp.ContentLength;
                }

                req = GetRequest(WebRequestMethods.Ftp.DownloadFile);

                // FTP Request 결과를 가져온다.
                using (FtpWebResponse resp = (FtpWebResponse)req.GetResponse())
                {
                    // FTP 결과 스트림
                    using (Stream stream = resp.GetResponseStream())
                    {
                        if (!LocalMakeDirectory(strLocalPath))
                        {
                            return false;
                        }

                        using (FileStream writeStream = new FileStream(strLocalPath + "/" + strLocalFileNm, FileMode.Create))
                        {
                            var buffer = new byte[1024 * 1024];
                            int totalReadBytesCount = 0;
                            int readBytesCount;

                            while ((readBytesCount = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                writeStream.Write(buffer, 0, readBytesCount);
                                totalReadBytesCount += readBytesCount;

                                int totalSize = (int)(fileSize) / 1000; // Kbytes

                                if (ftpEvent != null)
                                {
                                    //ftpEvent((totalReadBytesCount / 1000), totalSize);
                                    ftpEvent(0, 0);
                                }
                            }

                            bReturn = true;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                bSuccess = false;
                strErrMsg = ex.Message.ToString();
            }
            finally
            {

            }

            return bReturn;
        }

        #endregion

        #region 업로드

        /// <summary>
        /// 업로드 (폴더전체)
        /// </summary>
        /// <param name="strLocalPath">로컬 경로</param>
        /// <returns></returns>
        public bool FtpUploadAll(string strLocalPath)
        {
            return FtpUploadAll(strLocalPath, "");
        }

        /// <summary>
        /// 업로드 (폴더전체)
        /// </summary>
        /// <param name="strLocalPath">로컬 경로</param>
        /// <param name="strServerPath">서버 경로</param>
        /// <returns></returns>
        public bool FtpUploadAll(string strLocalPath, string strServerPath)
        {
            bool bReturn = false;

            try
            {
                DirectoryInfo di = new DirectoryInfo(strLocalPath);

                //로컬 폴더 경로 확인
                if (di.Exists)
                {
                    //서버 폴더 경로 확인
                    if (strServerPath.Length <= 0)
                    {
                        strServerPath = di.FullName;
                    }

                    //서버 폴더 확인
                    if (FtpMakeDirectory(strServerPath))
                    {
                        foreach (FileInfo fi in di.GetFiles())
                        {
                            //각 파일 업로드
                            FtpUpload(string.Format("/{0}/{1}", fi.DirectoryName, fi.FullName), string.Format("/{0}/{1}", strServerPath, fi.FullName));
                        }
                    }
                }
                else
                {
                    //err
                }
            }
            catch (WebException ex)
            {
                bSuccess = false;
                strErrMsg = ex.Message.ToString();
            }
            finally
            {

            }

            return bReturn;
        }

        /// <summary>
        /// 업로드
        /// </summary>
        /// <param name="strLocalPath">로컬 </param>
        /// <returns></returns>
        public bool FtpUpload(string strLocalPath)
        {
            return FtpUpload(strLocalPath, "");
        }

        /// <summary>
        /// 업로드
        /// </summary>
        /// <returns></returns>
        public bool FtpUpload(string strLocalPath, string strServerPath)
        {
            bool bReturn = false;

            try
            {
                if (strServerPath.Length <= 0)
                {
                    strServerPath = Path.GetFileName(strLocalPath);
                }

                //string[] arrServerPath = strServerPath.Split('/');

                //string strFolder = string.Empty;

                //for (int i = 1; i < arrServerPath.Length - 1; i++)
                //{
                //    strFolder = strFolder + "/" + arrServerPath[i];
                //    if (!FtpIsExistsFile(strFolder))
                //    {
                //        if (!FtpMakeDirectory(strFolder))
                //        {
                //            strErrMsg = "폴더가 존재하지 않습니다.";
                //            return false;
                //        }
                //    }
                //}


                if (!FtpMakeDirectory(Path.GetDirectoryName(strServerPath).Replace("\\","/")))
                {
                    strErrMsg = "폴더가 존재하지 않습니다.";
                    return false;
                }

                //서버 풀 주소(파일명 포함)
                strFullPath = string.Format("{0}{1}:{2}/{3}", strBaseUrl, strServerIP, strServerPort, strServerPath);

                FtpWebRequest req = GetRequest(WebRequestMethods.Ftp.UploadFile);

                using (FileStream fs = File.OpenRead(strLocalPath))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();

                    using (Stream stream = req.GetRequestStream())
                    {
                        stream.Write(buffer, 0, buffer.Length);
                        stream.Close();
                    }
                }

                bReturn = true;
            }
            catch (WebException ex)
            {
                bSuccess = false;
                strErrMsg = ex.Message.ToString();
            }
            finally
            {

            }

            return bReturn;
        }

        #endregion

        #region 삭제

        public bool FtpDelete(string strServerPath, bool bDirectory)
        {
            bool bReturn = false;

            try
            {
                //서버 풀 주소(파일명 포함)
                strFullPath = string.Format("{0}{1}:{2}/{3}", strBaseUrl, strServerIP, strServerPort, strServerPath);

                FtpWebRequest req = GetRequest(bDirectory ? WebRequestMethods.Ftp.RemoveDirectory : WebRequestMethods.Ftp.DeleteFile);

                // FTP Request 결과를 가져온다.
                string result = string.Empty;
                using (FtpWebResponse resp = (FtpWebResponse)req.GetResponse())
                {
                    long size = resp.ContentLength;
                    using (Stream datastream = resp.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(datastream))
                        {
                            result = sr.ReadToEnd();
                        }
                    }
                }
                bReturn = true;
            }
            catch (WebException ex)
            {
                bSuccess = false;
                strErrMsg = ex.Message.ToString();
            }
            finally
            {

            }

            return bReturn;
        }

        public bool FtpDeleteAll(string strServerPath)
        {
            bool bReturn = false;

            try
            {
                //서버 풀 주소(파일명 포함)
                strFullPath = string.Format("{0}{1}:{2}/{3}", strBaseUrl, strServerIP, strServerPort, strServerPath);

                List<clsDirectoryItem> list = FtpGetList(strServerPath, true);
                string strPath = string.Empty;
                string result = string.Empty;
                if (list != null && list.Count > 0)
                {
                    foreach (clsDirectoryItem item in list)
                    {
                        strPath = item.ParentNm + "/" + item.Name;

                        if (!item.IsDirectory)
                        {
                            if (!FtpDelete(strPath, false))
                            {
                                return false;
                            }
                        }
                    }

                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        clsDirectoryItem item = list[i];

                        strPath = item.ParentNm + "/" + item.Name;

                        if (item.IsDirectory)
                        {
                            if (!FtpDelete(strPath, true))
                            {
                                return false;
                            }
                        }
                    }
                }

                if (!FtpDelete(strServerPath, true))
                {
                    return false;
                }

                bReturn = true;
            }
            catch (WebException ex)
            {
                bSuccess = false;
                strErrMsg = ex.Message.ToString();
            }
            finally
            {

            }

            return bReturn;
        }

        #endregion

        #region FTP 서버

        /// <summary>
        /// 서버 폴더 및 파일목록 조회
        /// </summary>
        /// <param name="strPath">서버 경로</param>
        /// <returns></returns>
        public List<clsDirectoryItem> FtpGetList(string strPath, bool bAllList)
        {
            List<clsDirectoryItem> listDirectoryItem = new List<clsDirectoryItem>();

            try
            {
                //서버 풀 주소
                strFullPath = string.Format("{0}{1}:{2}/{3}", strBaseUrl, strServerIP, strServerPort, strPath);

                FtpWebRequest req = GetRequest(WebRequestMethods.Ftp.ListDirectoryDetails);

                // FTP Request 결과를 가져온다.
                using (FtpWebResponse resp = (FtpWebResponse)req.GetResponse())
                {
                    // FTP 결과 스트림
                    using (Stream stream = resp.GetResponseStream())
                    {
                        // 결과를 문자열로 읽기 (바이너리로 읽을 수도 있다)
                        string[] list = null;
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            list = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        }

                        foreach (string line in list)
                        {
                            // Windows FTP Server Response Format
                            // DateCreated    IsDirectory    Name
                            string data = line;

                            // Parse date
                            string date = data.Substring(0, 17);
                            DateTime dateTime = DateTime.Parse(date, System.Globalization.CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat); //생성일자
                            data = data.Remove(0, 24);

                            // Parse <DIR>
                            string dir = data.Substring(0, 5);
                            bool isDirectory = dir.Equals("<dir>", StringComparison.InvariantCultureIgnoreCase); //폴더여부
                            data = data.Remove(0, 5);
                            string size = data.Substring(0, 10);
                            data = data.Remove(0, 10);

                            // Parse name
                            string name = data; //폴더명 or 파일명

                            clsDirectoryItem item = new clsDirectoryItem();
                            item.DateCreated = dateTime;
                            item.IsDirectory = isDirectory;
                            item.Name = name;
                            item.ParentNm = strPath;
                            item.Size = size.Replace(" ","");
                            listDirectoryItem.Add(item);

                            if (bAllList)
                            {
                                if (isDirectory)
                                {
                                    List<clsDirectoryItem> addlistDirectoryItem = FtpGetList(string.Format("{0}/{1}", strPath, name), bAllList);

                                    if (addlistDirectoryItem == null)
                                    {
                                        return null;
                                    }
                                    else
                                    {
                                        foreach (clsDirectoryItem addItem in addlistDirectoryItem)
                                        {
                                            listDirectoryItem.Add(addItem);
                                        }
                                    }
                                }
                            }
                            
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                bSuccess = false;
                strErrMsg = ex.Message.ToString();
                return null;
            }
            finally
            {

            }

            return listDirectoryItem;
        }

        /// <summary>
        /// FTP 디렉토리 생성
        /// </summary>
        /// <param name="strDirPath"></param>
        /// <returns></returns>
        public bool FtpMakeDirectory(string strDirPath)
        {
            try
            {
                string[] arrDirPath = strDirPath.Split('/');

                if (arrDirPath.Length > 0)
                {
                    strDirPath = string.Empty;
                    foreach (string strTemp in arrDirPath)
                    {
                        strDirPath = strDirPath.Length > 0 ? string.Format("{0}/{1}", strDirPath, strTemp) : strTemp;

                        if (!FtpIsExistsFile(strDirPath))
                        {
                            strFullPath = string.Format("{0}{1}:{2}/{3}", strBaseUrl, strServerIP, strServerPort, strDirPath);
                            FtpWebRequest req = GetRequest(WebRequestMethods.Ftp.MakeDirectory);

                            // FTP Request 결과를 가져온다.
                            using (FtpWebResponse resp = (FtpWebResponse)req.GetResponse())
                            {
                                // FTP 결과 스트림
                                using (Stream stream = resp.GetResponseStream())
                                {
                                    using (StreamReader reader = new StreamReader(stream))
                                    {
                                        reader.ReadToEnd();
                                        reader.Close();
                                    }

                                    stream.Close();
                                }
                            }
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// FTP 파일 유무 확인
        /// </summary>
        /// <param name="strPath">FTP 파일 주소(파일명 포함)</param>
        /// <returns></returns>
        public bool FtpIsExistsFile(string strDirPath)
        {
            try
            {
                List<clsDirectoryItem> list = FtpGetList(strDirPath, false);

                if (list == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (WebException ex)
            {
                return false;
            }

            return false;
        }

        #endregion

        #region 로컬 서버

        /// <summary>
        /// 로컬 폴더 생성(폴더확인하여 없으면 생성)
        /// </summary>
        /// <param name="strPath">로컬 폴더 경로</param>
        /// <returns></returns>
        public bool LocalMakeDirectory(string strPath)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(strPath);

                if (!di.Exists)
                {
                    di.Create();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 로컬 파일 유무 확인
        /// </summary>
        /// <param name="strPath">로컬 폴더 경로</param>
        /// <returns></returns>
        public FileInfo IsExistsFile(string strPath)
        {
            FileInfo info = null;

            try
            {
                info = new FileInfo(strPath);

                if (!info.Exists)
                {
                    info = null;
                }
            }
            catch (Exception ex)
            {
                info = null;
            }

            return info;
        }

        #endregion

        #region 기타

        /// <summary>
        /// FTP GetRequest
        /// </summary>
        /// <param name="strUrl">FTP 방식(파일 다운로드, 업로드..등) 설정</param>
        /// <returns></returns>
        private FtpWebRequest GetRequest(string strMethod)
        {
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(strFullPath);
            req.Credentials = new NetworkCredential(strUserID, strUserPass);
            req.Method = strMethod;
            req.UsePassive = true;
            req.UseBinary = true;
            req.Timeout = iTimeOut;
            req.KeepAlive = false;
            return req;
        }

        #endregion
    }

    public class clsDirectoryItem
    {
        /// <summary>
        /// 생성일자
        /// </summary>
        public DateTime DateCreated;
        /// <summary>
        /// 폴더여부(true : 폴더, false : 파일)
        /// </summary>
        public bool IsDirectory;
        /// <summary>
        /// 폴더 또는 파일 이름
        /// </summary>
        public string Name;
        /// <summary>
        /// 부모 폴더 이름
        /// </summary>
        public string ParentNm;
        /// <summary>
        /// 파일사이즈
        /// </summary>
        public string Size;
    }
}
