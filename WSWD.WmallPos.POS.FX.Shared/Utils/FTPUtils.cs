using System;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;

namespace WSWD.WmallPos.POS.FX.Shared.Utils
{

    public class FtpUtils
    {
        private static int BUFFER_SIZE = 512;
        private static Encoding ASCII = Encoding.ASCII;

        /// <summary>
        /// 디버그모드
        /// </summary>
        private bool verboseDebugging = false;
        /// <summary>
        /// 서버주소
        /// </summary>
        private string server = string.Empty;
        /// <summary>
        /// 서버포트
        /// </summary>
        private int port = 21;
        /// <summary>
        /// 루트경로
        /// </summary>
        private string remotePath = ".";
        /// <summary>
        /// 서버ID
        /// </summary>
        private string username = "anonymous";
        /// <summary>
        /// 서버PASS
        /// </summary>
        private string password = "anonymous@anonymous.net";
        /// <summary>
        /// 메세지
        /// </summary>
        private string message = null;
        /// <summary>
        /// 결과값
        /// </summary>
        private string result = null;

        
        private int bytes = 0;
        private int resultCode = 0;

        private bool loggedin = false;
        private bool binMode = false;

        private Byte[] buffer = new Byte[BUFFER_SIZE];
        private Socket clientSocket = null;

        private int timeoutSeconds = 10;

        /// <summary>
        /// Default contructor
        /// </summary>
        public FtpUtils()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public FtpUtils(string server, string username, string password)
        {
            this.server = server;
            this.username = username;
            this.password = password;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="timeoutSeconds"></param>
        /// <param name="port"></param>
        public FtpUtils(string server, string username, string password, int timeoutSeconds, int port)
        {
            this.server = server;
            this.username = username;
            this.password = password;
            this.timeoutSeconds = timeoutSeconds;
            this.port = port;
        }

        /// <summary>
        /// Display all communications to the debug log
        /// </summary>
        public bool VerboseDebugging
        {
            get
            {
                return this.verboseDebugging;
            }
            set
            {
                this.verboseDebugging = value;
            }
        }
        /// <summary>
        /// Remote server port. Typically TCP 21
        /// </summary>
        public int Port
        {
            get
            {
                return this.port;
            }
            set
            {
                this.port = value;
            }
        }
        /// <summary>
        /// Timeout waiting for a response from server, in seconds.
        /// </summary>
        public int Timeout
        {
            get
            {
                return this.timeoutSeconds;
            }
            set
            {
                this.timeoutSeconds = value;
            }
        }
        /// <summary>
        /// Gets and Sets the name of the FTP server.
        /// </summary>
        /// <returns></returns>
        public string Server
        {
            get
            {
                return this.server;
            }
            set
            {
                this.server = value;
            }
        }
        /// <summary>
        /// Gets and Sets the port number.
        /// </summary>
        /// <returns></returns>
        public int RemotePort
        {
            get
            {
                return this.port;
            }
            set
            {
                this.port = value;
            }
        }
        /// <summary>
        /// GetS and Sets the remote directory.
        /// </summary>
        public string RemotePath
        {
            get
            {
                return this.remotePath;
            }
            set
            {
                this.remotePath = value;
            }

        }
        /// <summary>
        /// Gets and Sets the username.
        /// </summary>
        public string Username
        {
            get
            {
                return this.username;
            }
            set
            {
                this.username = value;
            }
        }
        /// <summary>
        /// Gets and Set the password.
        /// </summary>
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        /// <summary>
        /// If the value of mode is true, set binary mode for downloads, else, Ascii mode.
        /// </summary>
        public bool BinaryMode
        {
            get
            {
                return this.binMode;
            }
            set
            {
                if (this.binMode == value) return;

                if (value)
                    sendCommand("TYPE I");

                else
                    sendCommand("TYPE A");

                if (this.resultCode != 200)
                {
                    //return false;// throw;// new FtpException(result.Substring(4));
                } 
            }
        }
        /// <summary>
        /// Login to the remote server.
        /// </summary>
        public bool Login(out string _strFtpMsg)
        {
            bool bRetrun = true;
            _strFtpMsg = string.Empty;

            if (this.loggedin)
            {
                this.Close();
            }

            IPAddress addr = null;
            IPEndPoint ep = null;

            try
            {
                this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                addr = Dns.Resolve(this.server).AddressList[0];
                ep = new IPEndPoint(addr, this.port);
                this.clientSocket.Connect(ep);
            }
            catch (Exception ex)
            {
                if (this.clientSocket != null && this.clientSocket.Connected)
                {
                    this.clientSocket.Close();
                }

                bRetrun = false;
                _strFtpMsg = ex.ToString();
            }

            if (bRetrun)
            {
                this.readResponse();

                if (this.resultCode != 220)
                {
                    this.Close();
                    bRetrun = false;
                    _strFtpMsg = this.result.Substring(4);
                }
                else
                {
                    this.sendCommand("USER " + username);

                    if (!(this.resultCode == 331 || this.resultCode == 230))
                    {
                        this.cleanup();
                        bRetrun = false;
                        _strFtpMsg = this.result.Substring(4);
                    }
                    else
                    {
                        if (this.resultCode != 230)
                        {
                            this.sendCommand("PASS " + password);

                            if (!(this.resultCode == 230 || this.resultCode == 202))
                            {
                                this.cleanup();
                                bRetrun = false;
                                _strFtpMsg = this.result.Substring(4);
                            }
                        }

                        if (bRetrun)
                        {
                            this.loggedin = true;
                            this.ChangeDir(this.remotePath, out _strFtpMsg);    
                        }
                    }
                }
            }

            return bRetrun;
        }

        /// <summary>
        /// Close the FTP connection.
        /// </summary>
        public void Close()
        {
            if (this.clientSocket != null)
            {
                this.sendCommand("QUIT");
            }

            this.cleanup();
        }

        /// <summary>
        /// Return a string array containing the remote directory's file list.
        /// </summary>
        /// <returns></returns>
        public string[] GetFileList(out bool bReturn, out string _strFtpMsg)
        {
            return this.GetFileList("*.*", out bReturn, out _strFtpMsg);
        }

        /// <summary>
        /// Return a string array containing the remote directory's file list.
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        public string[] GetFileList(string mask, out bool bReturn, out string _strFtpMsg)
        {
            string[] retrunMsg = new string[0];

            bReturn = true;
            _strFtpMsg = string.Empty;

            try
            {
                if (!this.loggedin)
                {
                    bReturn = Login(out _strFtpMsg);
                }

                if (bReturn)
                {
                    Socket cSocket = createDataSocket();

                    this.sendCommand("NLST " + mask);

                    if (!(this.resultCode == 150 || this.resultCode == 125 || this.resultCode == 226))
                    {
                        bReturn = false;
                        _strFtpMsg = result.Substring(4);
                    }

                    if (bReturn)
                    {
                        DateTime timeout = DateTime.Now.AddSeconds(this.timeoutSeconds);

                        this.message = string.Empty;

                        while (timeout > DateTime.Now)
                        {
                            int bytes = cSocket.Receive(buffer, buffer.Length, 0);
                            this.message += ASCII.GetString(buffer, 0, bytes);

                            if (bytes < this.buffer.Length) break;
                        }

                        retrunMsg = this.message.Replace("\r", "").Split('\n');

                        cSocket.Close();

                        if (this.message.IndexOf("No such file or directory") != -1)
                        {
                            _strFtpMsg = "파일 또는 폴더가 존재하지 않습니다.";
                        }

                        this.readResponse();

                        if (this.resultCode != 226)
                        {
                            bReturn = false;
                            _strFtpMsg = result.Substring(4);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bReturn = false;
                _strFtpMsg = ex.ToString();
            }

            return retrunMsg;
        }

        /// <summary>
        /// Return the size of a file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public long GetFileSize(string fileName, out bool bReturn, out string _strFtpMsg)
        {
            long fileSize = 0;

            bReturn = true;
            _strFtpMsg = string.Empty;

            try
            {
                if (!this.loggedin)
                {
                    bReturn = Login(out _strFtpMsg);
                }

                if (bReturn)
                {
                    this.sendCommand("SIZE " + fileName);

                    if (this.resultCode == 213)
                    {
                        fileSize = long.Parse(this.result.Substring(4));
                    }
                    else
                    {
                        bReturn = false;
                        _strFtpMsg = this.result.Substring(4);
                    }
                }
            }
            catch (Exception ex)
            {
                bReturn = false;
                _strFtpMsg = ex.ToString();
            }

            return fileSize;
        }


        ///// <summary>
        ///// Download a file to the Assembly's local directory,
        ///// keeping the same file name.
        ///// </summary>
        ///// <param name="remFileName"></param>
        //public void Download(string remFileName, out string _strFtpMsg)
        //{
        //    this.Download(remFileName,"",false, out strFtpMsg);
        //}

        ///// <summary>
        ///// Download a remote file to the Assembly's local directory,
        ///// keeping the same file name, and set the resume flag.
        ///// </summary>
        ///// <param name="remFileName"></param>
        ///// <param name="resume"></param>
        //public void Download(string remFileName, Boolean resume, out string _strFtpMsg)
        //{
        //    this.Download(remFileName,"",resume);
        //}

        ///// <summary>
        ///// Download a remote file to a local file name which can include
        ///// a path. The local file name will be created or overwritten,
        ///// but the path must exist.
        ///// </summary>
        ///// <param name="remFileName"></param>
        ///// <param name="locFileName"></param>
        //public void Download(string remFileName, string locFileName, out string _strFtpMsg)
        //{
        //    this.Download(remFileName,locFileName,false);
        //}

        ///// <summary>
        ///// Download a remote file to a local file name which can include
        ///// a path, and set the resume flag. The local file name will be
        ///// created or overwritten, but the path must exist.
        ///// </summary>
        ///// <param name="remFileName"></param>
        ///// <param name="locFileName"></param>
        ///// <param name="resume"></param>
        //public void Download(string remFileName, string locFileName, Boolean resume, out string _strFtpMsg)
        //{
        //    if ( !this.loggedin ) this.Login();

        //    this.BinaryMode = true;

        //    Debug.WriteLine("Downloading file " + remFileName + " from " + server + "/" + remotePath, "FtpClient" );

        //    if (locFileName.Equals(""))
        //    {
        //        locFileName = remFileName;
        //    }

        //    FileStream output = null;

        //    if ( !File.Exists(locFileName) )
        //        output = File.Create(locFileName);

        //    else
        //        output = new FileStream(locFileName,FileMode.Open);

        //    Socket cSocket = createDataSocket();

        //    long offset = 0;

        //    if ( resume )
        //    {
        //        offset = output.Length;

        //        if ( offset > 0 )
        //        {
        //            this.sendCommand( "REST " + offset );
        //            if ( this.resultCode != 350 )
        //            {
        //                //Server dosnt support resuming
        //                offset = 0;
        //                Debug.WriteLine("Resuming not supported:" + result.Substring(4), "FtpClient" );
        //            }
        //            else
        //            {
        //                Debug.WriteLine("Resuming at offset " + offset, "FtpClient" );
        //                output.Seek( offset, SeekOrigin.Begin );
        //            }
        //        }
        //    }

        //    this.sendCommand("RETR " + remFileName);

        //    if ( this.resultCode != 150 && this.resultCode != 125 )
        //    {
        //        throw new FtpException(this.result.Substring(4));
        //    }

        //    DateTime timeout = DateTime.Now.AddSeconds(this.timeoutSeconds);

        //    while ( timeout > DateTime.Now )
        //    {
        //        this.bytes = cSocket.Receive(buffer, buffer.Length, 0);
        //        output.Write(this.buffer,0,this.bytes);

        //        if ( this.bytes <= 0)
        //        {
        //            break;
        //        }
        //    }

        //    output.Close();

        //    if ( cSocket.Connected ) cSocket.Close();

        //    this.readResponse();

        //    if( this.resultCode != 226 && this.resultCode != 250 )
        //        throw new FtpException(this.result.Substring(4));
        //}


        /// <summary>
        /// Upload a file.
        /// </summary>
        /// <param name="fileName"></param>
        public bool Upload(string fileName, out string _strFtpMsg)
        {
            bool bReturn = true;
            _strFtpMsg = string.Empty;

            try
            {
                bReturn = Upload(fileName, false, out _strFtpMsg);
            }
            catch (Exception ex)
            {
                bReturn = false;
                _strFtpMsg = ex.ToString();
            }

            return bReturn;
        }

        /// <summary>
        /// Upload a file and set the resume flag.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="resume"></param>
        public bool Upload(string fileName, bool resume, out string _strFtpMsg)
        {
            bool bReturn = true;
            _strFtpMsg = string.Empty;

            try
            {
                if (!this.loggedin)
                {
                    bReturn = Login(out _strFtpMsg);
                }

                if (bReturn)
                {
                    Socket cSocket = null;
                    long offset = 0;

                    if (resume)
                    {
                        try
                        {
                            this.BinaryMode = true;

                            offset = GetFileSize(Path.GetFileName(fileName), out bReturn, out _strFtpMsg);
                        }
                        catch (Exception)
                        {
                            // file not exist
                            offset = 0;
                        }
                    }

                    if (bReturn)
                    {
                        // open stream to read file
                        FileStream input = new FileStream(fileName, FileMode.Open);

                        if (resume && input.Length <= offset)
                        {
                            // different file size
                            //Debug.WriteLine("Overwriting " + fileName, "FtpClient");
                            offset = 0;
                        }
                        //else if (resume && input.Length == offset)
                        //{
                        //    // file done
                        //    input.Close();
                        //    //Debug.WriteLine("Skipping completed " + fileName + " - turn resume off to not detect.", "FtpClient");
                        //    bReturn = false;
                        //}

                        // dont create untill we know that we need it
                        cSocket = this.createDataSocket();

                        if (offset > 0)
                        {
                            this.sendCommand("REST " + offset);
                            if (this.resultCode != 350)
                            {
                                Debug.WriteLine("Resuming not supported", "FtpClient");
                                offset = 0;
                            }
                        }

                        this.sendCommand("STOR " + Path.GetFileName(fileName));

                        if (this.resultCode != 125 && this.resultCode != 150)
                        {
                            bReturn = false;
                            _strFtpMsg = result.Substring(4);
                        }
                        else
                        {
                            if (offset != 0)
                            {
                                input.Seek(offset, SeekOrigin.Begin);
                            }

                            while ((bytes = input.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                cSocket.Send(buffer, bytes, 0);
                            }

                            input.Close();

                            if (cSocket.Connected)
                            {
                                cSocket.Close();
                            }

                            this.readResponse();

                            if (this.resultCode != 226 && this.resultCode != 250)
                            {
                                bReturn = false;
                                _strFtpMsg = result.Substring(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bReturn = false;
                _strFtpMsg = ex.ToString();
            }

            return bReturn;

        }

        /// <summary>
        /// Upload a directory and its file contents
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recurse">Whether to recurse sub directories</param>
        public bool UploadDirectory(string path, bool recurse, out string _strFtpMsg)
        {
            bool bReturn = true;
            _strFtpMsg = string.Empty;

            try
            {
                bReturn = UploadDirectory(path, recurse, "*.*", out _strFtpMsg);
            }
            catch (Exception ex)
            {
                bReturn = false;
                _strFtpMsg = ex.ToString();
            }

            return bReturn;
        }

        /// <summary>
        /// Upload a directory and its file contents
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recurse">Whether to recurse sub directories</param>
        /// <param name="mask">Only upload files of the given mask - everything is '*.*'</param>
        public bool UploadDirectory(string path, bool recurse, string mask, out string _strFtpMsg)
        {
            bool bReturn = true;
            _strFtpMsg = string.Empty;

            try
            {
                string[] dirs = path.Replace("/", @"\").Split('\\');
                string rootDir = dirs[dirs.Length - 1];

                //폴더 검사
                string[] strTemp = GetFileList(rootDir, out bReturn, out _strFtpMsg);

                if (bReturn)
                {
                    if (strTemp.Length < 1)
                    {
                        //폴더 생성
                        bReturn = MakeDir(rootDir, out _strFtpMsg);
                    }
                }

                if (bReturn)
                {
                    //폴더 변경
                    bReturn = ChangeDir(rootDir, out _strFtpMsg);


                    if (bReturn)
                    {
                        foreach (string file in Directory.GetFiles(path, mask))
                        {
                            //업로드
                            bReturn = Upload(file, true, out _strFtpMsg);

                            if (!bReturn)
                            {
                                break;
                            }
                        }

                        if (bReturn)
                        {
                            if (recurse)
                            {
                                foreach (string directory in Directory.GetDirectories(path))
                                {
                                    //업로드
                                    bReturn = UploadDirectory(directory, recurse, mask, out _strFtpMsg);

                                    if (!bReturn)
                                    {
                                        break;
                                    }
                                }
                            }

                            if (bReturn)
                            {
                                //폴더 변경
                                bReturn = ChangeDir("..", out _strFtpMsg);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bReturn = false;
                _strFtpMsg = ex.ToString();
            }

            return bReturn;
        }

        ///// <summary>
        ///// Delete a file from the remote FTP server.
        ///// </summary>
        ///// <param name="fileName"></param>
        //public void DeleteFile(string fileName, out string _strFtpMsg)
        //{
        //    if ( !this.loggedin ) this.Login();

        //    this.sendCommand( "DELE " + fileName );

        //    if ( this.resultCode != 250 ) throw new FtpException(this.result.Substring(4));

        //    Debug.WriteLine( "Deleted file " + fileName, "FtpClient" );
        //}

        ///// <summary>
        ///// Rename a file on the remote FTP server.
        ///// </summary>
        ///// <param name="oldFileName"></param>
        ///// <param name="newFileName"></param>
        ///// <param name="overwrite">setting to false will throw exception if it exists</param>
        //public void RenameFile(string oldFileName, string newFileName, bool overwrite, out string _strFtpMsg)
        //{
        //    if ( !this.loggedin ) this.Login();

        //    this.sendCommand( "RNFR " + oldFileName );

        //    if ( this.resultCode != 350 ) throw new FtpException(this.result.Substring(4));

        //    if ( !overwrite && this.GetFileList(newFileName).Length > 0 ) throw new FtpException("File already exists");

        //    this.sendCommand( "RNTO " + newFileName );

        //    if ( this.resultCode != 250 ) throw new FtpException(this.result.Substring(4));

        //    Debug.WriteLine( "Renamed file " + oldFileName + " to " + newFileName, "FtpClient" );
        //}

        /// <summary>
        /// Create a directory on the remote FTP server.
        /// </summary>
        /// <param name="dirName"></param>
        public bool MakeDir(string dirName, out string _strFtpMsg)
        {
            bool bReturn = true;
            _strFtpMsg = string.Empty;

            try
            {
                bReturn = MakeDir(dirName, true, out _strFtpMsg);
            }
            catch (Exception ex)
            {
                bReturn = false;
            }

            return bReturn;
        }

        /// <summary>
        /// Create a directory on the remote FTP server.
        /// </summary>
        /// <param name="dirName"></param>
        public bool MakeDir(string dirName, bool bMove, out string _strFtpMsg)
        {
            bool bReturn = true;
            _strFtpMsg = string.Empty;

            try
            {
                if (!this.loggedin)
                {
                    bReturn = Login(out _strFtpMsg);
                }

                if (bReturn)
                {
                    this.sendCommand("MKD " + dirName);

                    if (this.resultCode != 250 && this.resultCode != 257)
                    {
                        _strFtpMsg = this.result.Substring(4);
                    }
                    else
                    {
                        if (bMove)
                        {
                            bReturn = ChangeDir(dirName,out _strFtpMsg);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                bReturn = false;
            }

            return bReturn;
        }

        ///// <summary>
        ///// Delete a directory on the remote FTP server.
        ///// </summary>
        ///// <param name="dirName"></param>
        //public void RemoveDir(string dirName, out string _strFtpMsg)
        //{
        //    if ( !this.loggedin ) this.Login();

        //    this.sendCommand( "RMD " + dirName );

        //    if ( this.resultCode != 250 ) throw new FtpException(this.result.Substring(4));

        //    Debug.WriteLine( "Removed directory " + dirName, "FtpClient" );
        //}

        /// <summary>
        /// Change the current working directory on the remote FTP server.
        /// </summary>
        /// <param name="dirName"></param>
        public bool ChangeDir(string dirName, out string _strFtpMsg)
        {
            bool bReturn = true;
            _strFtpMsg = string.Empty;

            try
            {
                if (dirName.Length <= 0)
                {
                    _strFtpMsg = "변경할 서버 디렉토리를 설정하십시오.";
                    bReturn = false;
                }

                if (bReturn)
                {
                    if (!this.loggedin)
                    {
                        bReturn = Login(out _strFtpMsg);
                    }

                    if (bReturn)
                    {
                        this.sendCommand("CWD " + dirName);

                        if (this.resultCode != 250)
                        {
                            bReturn = false;
                            _strFtpMsg = result.Substring(4);
                        }
                        else
                        {
                            this.sendCommand("PWD");

                            if (this.resultCode != 257)
                            {
                                bReturn = false;
                                _strFtpMsg = result.Substring(4);
                            }
                            else
                            {
                                this.remotePath = this.message.Split('"')[1];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bReturn = false;
            }

            return bReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        private void readResponse()
        {
            this.message = "";
            this.result = this.readLine();

            if (this.result.Length > 3)
                this.resultCode = int.Parse(this.result.Substring(0, 3));
            else
                this.result = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string readLine()
        {
            while (true)
            {
                this.bytes = clientSocket.Receive(this.buffer, this.buffer.Length, 0);
                this.message += ASCII.GetString(this.buffer, 0, this.bytes);

                if (this.bytes < this.buffer.Length)
                {
                    break;
                }
            }

            string[] msg = this.message.Split('\n');

            if (this.message.Length > 2)
                this.message = msg[msg.Length - 2];

            else
                this.message = msg[0];


            if (this.message.Length > 4 && !this.message.Substring(3, 1).Equals(" ")) return this.readLine();

            if (this.verboseDebugging)
            {
                for (int i = 0; i < msg.Length - 1; i++)
                {
                    Debug.Write(msg[i], "FtpClient");
                }
            }

            return message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        private void sendCommand(String command)
        {
            if (this.verboseDebugging) Debug.WriteLine(command, "FtpClient");

            Byte[] cmdBytes = Encoding.ASCII.GetBytes((command + "\r\n").ToCharArray());
            clientSocket.Send(cmdBytes, cmdBytes.Length, 0);
            this.readResponse();
        }

        /// <summary>
        /// when doing data transfers, we need to open another socket for it.
        /// </summary>
        /// <returns>Connected socket</returns>
        private Socket createDataSocket()
        {
            this.sendCommand("PASV");

            if (this.resultCode != 227) return null;// throw;// new FtpException(this.result.Substring(4));

            int index1 = this.result.IndexOf('(');
            int index2 = this.result.IndexOf(')');

            string ipData = this.result.Substring(index1 + 1, index2 - index1 - 1);

            int[] parts = new int[6];

            int len = ipData.Length;
            int partCount = 0;
            string buf = "";

            for (int i = 0; i < len && partCount <= 6; i++)
            {
                char ch = char.Parse(ipData.Substring(i, 1));

                if (char.IsDigit(ch))
                    buf += ch;

                else if (ch != ',')
                    return null;// new FtpException("Malformed PASV result: " + result);

                if (ch == ',' || i + 1 == len)
                {
                    try
                    {
                        parts[partCount++] = int.Parse(buf);
                        buf = "";
                    }
                    catch (Exception ex)
                    {
                        throw;// new FtpException("Malformed PASV result (not supported?): " + this.result, ex);
                    }
                }
            }

            string ipAddress = parts[0] + "." + parts[1] + "." + parts[2] + "." + parts[3];

            int port = (parts[4] << 8) + parts[5];

            Socket socket = null;
            IPEndPoint ep = null;

            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ep = new IPEndPoint(Dns.Resolve(ipAddress).AddressList[0], port);
                socket.Connect(ep);
            }
            catch (Exception ex)
            {
                // doubtfull....
                if (socket != null && socket.Connected) socket.Close();
                throw;// new FtpException("Can't connect to remote server", ex);
            }

            return socket;
        }

        /// <summary>
        /// Always release those sockets.
        /// </summary>
        private void cleanup()
        {
            if (this.clientSocket != null)
            {
                this.clientSocket.Close();
                this.clientSocket = null;
            }
            this.loggedin = false;
        }

        /// <summary>
        /// Destuctor
        /// </summary>
        ~FtpUtils()
        {
            this.cleanup();
        }
    }
}

