using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.SqlClient;

using WSWD.WmallPos.FX.Shared;

namespace WSWD.WmallPos.POS.FX.Win.Utils
{
    public class LogHelper : IFileLogHandler
    {
        public static LogHelper Instance
        {
            get;
            set;
        }

        static private int s_ProcID = 0;

        private static Process s_Process;

        static LogHelper()
        {
            try
            {
                s_Process = Process.GetCurrentProcess();
                s_ProcID = s_Process.Id;
            }
            catch (Exception)
            {

            }
        }

        public void Log(string method)
        {
            string log;
            log = string.Format("{0},{1},{2},{3},{4}"
                , DateTime.Now.ToLongTimeString()
                , method
                , s_Process.TotalProcessorTime
                , s_Process.PrivateMemorySize64
                , s_Process.PeakVirtualMemorySize64);

            Instance.Log(LogTypes.Program, log);
        }

        #region IFileLogHandler members

        public void Log(string logMessage, params object[] args)
        {
            Log(LogTypes.Program, logMessage, args);
        }

        public void LogException(string logMessage, params object[] args)
        {
            Log(LogTypes.Exception, logMessage, args);
        }

        /// <summary>
        /// overridable
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="logMessage"></param>
        /// <param name="args"></param>
        public virtual void LogByType(string logType, string logMessage, params object[] args)
        {
            Log(logType, logMessage, args);
        }

        public virtual string LogBaseDirectory()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
        }

        #endregion

        protected void Log(LogTypes logType, string message, params object[] args)
        {
            Log(LogTypesToName(logType), message, args);
        }

        /// <summary>
        /// LogMessage
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        protected void Log(string logType, string message, params object[] args)
        {
            string logMessage = message;
            if (args != null && args.Length > 0)
            {
                logMessage = string.Format(message, args);
            }

            StreamWriter sw = null;
            try
            {
                string logFile = Path.Combine(LogDirByType(logType), LogFileByType(logType, DateTime.Today));
                string logFolder = Path.GetDirectoryName(logFile);

                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }

                if (!File.Exists(logFile))
                {
                    File.Create(logFile).Close();
                }

                sw = new StreamWriter(logFile, true, System.Text.Encoding.UTF8);
                string log = string.Format("[{0:yyyy-MM-dd HH:mm:ss}]\n{1}", DateTime.Now, logMessage);

                sw.WriteLine(log);
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

        #region privates

        private string GetLogFileByDate(LogTypes logType, DateTime logDate)
        {
            return Path.Combine(LogDirByType(logType), LogFileByType(logType, logDate));
        }

        private string LogFileByType(string type, DateTime date)
        {
            return string.Format("{1}_{0:yyyyMMdd}.log", date, type);
        }

        private string LogFileByType(LogTypes logType, DateTime date)
        {
            return string.Format("{1}_{0:yyyyMMdd}.log", date, logType == LogTypes.Program ? logType.ToString().ToLower() : "error");
        }

        private string LogTypesToName(LogTypes logType)
        {
            switch (logType)
            {
                case LogTypes.Program:
                    return "program";
                case LogTypes.Exception:
                    return "error";
                default:
                    return string.Empty;
            }
        }

        private string LogDirByType(string type)
        {
            return Path.Combine(LogBaseDirectory(), type);
        }

        private string LogDirByType(LogTypes logType)
        {
            return Path.Combine(LogBaseDirectory(), logType == LogTypes.Program ? "program" : "error");
        }


        #endregion

        #region Log reading

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="logDate"></param>
        /// <returns></returns>
        public String ReadLog(LogTypes logType)
        {
            return ReadLog(logType, DateTime.Today);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="logDate"></param>
        /// <returns></returns>
        public String ReadLog(LogTypes logType, DateTime logDate)
        {
            string logFile = GetLogFileByDate(logType, logDate);
            return File.Exists(logFile) ? File.ReadAllText(logFile) : string.Empty;
        }


        #endregion

        #region Exception handling, formating

        /// <summary>
        /// Log exceptions
        /// </summary>
        /// <param name="ex"></param>
        public static void LogException(Exception ex)
        {
            Instance.Log(LogTypes.Exception, Format(ex));
        }

        /// <summary>
        /// Create a DataSet contains 1 DataRow of TraceInfo
        /// Compress this dataset to binary and convert to Base64 string
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        static public string Format(System.Exception exception)
        {
            string tmp = string.Empty;
            StringBuilder msg = null;
            StackTrace st1 = null;
            StackTrace st2 = null;
            StackFrame sf = null;

            DateTime dateTime = DateTime.Now;
            try
            {
                msg = new StringBuilder();
                st1 = new StackTrace(1, true);
                msg.AppendLine();
                msg.AppendLine("==============[System Error Tracing]==============");
                msg.AppendLine("[CallStackTrace]");
                for (int i = 0; i < st1.FrameCount; i++)
                {
                    sf = st1.GetFrame(i);
                    tmp = sf.GetMethod().DeclaringType.FullName + "." + sf.GetMethod().Name;
                    if (tmp.IndexOf("System") != 0 && tmp.IndexOf("Microsoft") != 0)
                    {
                        msg.Append(tmp);
                        msg.Append(" : (" + sf.GetFileLineNumber() + ")");
                        msg.Append("\r\n");
                    }
                }

                //Exception을 트래이싱하는부분
                st2 = new StackTrace(exception, true);
                msg.AppendLine("[ErrStackTrace]");
                for (int i = 0; i < st2.FrameCount; i++)
                {
                    sf = st2.GetFrame(i);
                    tmp = sf.GetMethod().DeclaringType.FullName + "." + sf.GetMethod().Name;
                    if (tmp.IndexOf("System") != 0 && tmp.IndexOf("Microsoft") != 0)
                    {
                        msg.Append(tmp);
                        msg.Append(" : (" + sf.GetFileLineNumber() + ")");
                        msg.Append("\r\n");
                    }
                }

                //msg에 현재 트래이싱된 시간을 담도록 한다.
                msg.Append("\r\n[DateTime] : " + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");

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
                    msg.Append("\r\nTargetSite: ").Append(sqlErr.TargetSite);
                    msg.Append("\r\nHelpLink: ").Append(sqlErr.HelpLink);
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

        #region SendReport to email

        /// <summary>
        /// 
        /// </summary>
        static private void WriteToLog_RAM_CPU()
        {
            Instance.Log("------------------------------------------------", new object[] { });

            if (s_ProcID == 0) return;
            Process proc = Process.GetProcessById(s_ProcID);
            if (proc == null) return;

            Instance.Log(LogTypes.Exception, string.Format("[{0:yyyy-MM-dd HH:mm:ss}]", DateTime.Now));
            Instance.Log(LogTypes.Exception, string.Format("MachineName:\t{0}", proc.MachineName));
            Instance.Log(LogTypes.Exception, string.Format("BasePriority:\t{0}", proc.BasePriority));
            Instance.Log(LogTypes.Exception, string.Format("WorkingSet:\t{0}", proc.WorkingSet64));
            Instance.Log(LogTypes.Exception, string.Format("MaxWorkingSet:\t{0}", proc.MaxWorkingSet));
            Instance.Log(LogTypes.Exception, string.Format("MinWorkingSet:\t{0}", proc.MinWorkingSet));
            Instance.Log(LogTypes.Exception, string.Format("NonpagedSystemMemorySize:\t{0}", proc.NonpagedSystemMemorySize64));
            Instance.Log(LogTypes.Exception, string.Format("PagedMemorySize:\t{0}\r\n", proc.PagedMemorySize64));
            Instance.Log(LogTypes.Exception, string.Format("PagedSystemMemorySize:\t{0}", proc.PagedSystemMemorySize64));
            Instance.Log(LogTypes.Exception, string.Format("PeakPagedMemorySize:\t{0}", proc.PeakPagedMemorySize64));
            Instance.Log(LogTypes.Exception, string.Format("PeakVirtualMemorySize:\t{0}", proc.PeakVirtualMemorySize64));
            Instance.Log(LogTypes.Exception, string.Format("PeakWorkingSet:\t{0}", proc.PeakWorkingSet64));
            Instance.Log(LogTypes.Exception, string.Format("PrivateMemorySize:\t{0}", proc.PrivateMemorySize64));
            Instance.Log(LogTypes.Exception, string.Format("VirtualMemorySize:\t{0}", proc.VirtualMemorySize64));
            Instance.Log(LogTypes.Exception, string.Format("UserProcessorTime:\t{0}", proc.UserProcessorTime));
            Instance.Log(LogTypes.Exception, string.Format("TotalProcessorTime:\t{0}", proc.TotalProcessorTime));
            Instance.Log(LogTypes.Exception, string.Format("Responding:\t{0}", proc.Responding));
        }

        static private void WriteToLog_RAM_CPU(int interval, bool notSendMail)
        {
            if (notSendMail)
            {
                Instance.Log("------------------------------------------------", new object[] { });

                try
                {
                    System.Threading.Thread thread = new System.Threading.Thread(delegate()
                    {
                        if (s_ProcID == 0) return;
                        Process proc = Process.GetProcessById(s_ProcID);
                        if (proc == null) return;

                        string strW = "\r\n---------------------------------------------------------\r\n";
                        strW += string.Format("[{0:yyyy-MM-dd HH:mm:ss}]\r\n", DateTime.Now);
                        strW += string.Format("MachineName:\t{0}\r\n", proc.MachineName);
                        strW += string.Format("BasePriority:\t{0}\r\n", proc.BasePriority);
                        strW += string.Format("WorkingSet:\t{0}\r\n", proc.WorkingSet64);
                        strW += string.Format("MaxWorkingSet:\t{0}\r\n", proc.MaxWorkingSet);
                        strW += string.Format("MinWorkingSet:\t{0}\r\n", proc.MinWorkingSet);
                        strW += string.Format("NonpagedSystemMemorySize:\t{0}\r\n", proc.NonpagedSystemMemorySize64);
                        strW += string.Format("PagedMemorySize:\t{0}\r\n", proc.PagedMemorySize64);
                        strW += string.Format("PagedSystemMemorySize:\t{0}\r\n", proc.PagedSystemMemorySize64);
                        strW += string.Format("PeakPagedMemorySize:\t{0}\r\n", proc.PeakPagedMemorySize64);
                        strW += string.Format("PeakVirtualMemorySize:\t{0}\r\n", proc.PeakVirtualMemorySize64);
                        strW += string.Format("PeakWorkingSet:\t{0}\r\n", proc.PeakWorkingSet64);
                        strW += string.Format("PrivateMemorySize:\t{0}\r\n", proc.PrivateMemorySize64);
                        strW += string.Format("VirtualMemorySize:\t{0}\r\n", proc.VirtualMemorySize64);
                        strW += string.Format("UserProcessorTime:\t{0}\r\n", proc.UserProcessorTime);
                        strW += string.Format("TotalProcessorTime:\t{0}\r\n", proc.TotalProcessorTime);
                        strW += string.Format("Responding:\t{0}\r\n", proc.Responding);
                        Instance.Log(LogTypes.Exception, strW);
                    });
                    thread.IsBackground = true;
                    thread.Priority = System.Threading.ThreadPriority.BelowNormal;
                    thread.Start();
                }
                catch
                {

                }
            }
        }

        static private void WriteToLog_RAM_CPU(int interval)
        {
            Instance.Log("------------------------------------------------", new object[] { });

            Timer time = new Timer();
            time.Interval = interval;
            try
            {
                time.Tick += delegate(object sender0, EventArgs e0)
                {
                    System.Threading.Thread thread = new System.Threading.Thread(delegate()
                    {
                        if (s_ProcID == 0) return;
                        Process proc = Process.GetProcessById(s_ProcID);
                        if (proc == null) return;

                        string strW = "\r\n---------------------------------------------------------\r\n";
                        strW += string.Format("[{0:yyyy-MM-dd HH:mm:ss}]\r\n", DateTime.Now);
                        strW += string.Format("MachineName:\t{0}\r\n", proc.MachineName);
                        strW += string.Format("BasePriority:\t{0}\r\n", proc.BasePriority);
                        strW += string.Format("WorkingSet:\t{0}\r\n", proc.WorkingSet64);
                        strW += string.Format("MaxWorkingSet:\t{0}\r\n", proc.MaxWorkingSet);
                        strW += string.Format("MinWorkingSet:\t{0}\r\n", proc.MinWorkingSet);
                        strW += string.Format("NonpagedSystemMemorySize:\t{0}\r\n", proc.NonpagedSystemMemorySize64);
                        strW += string.Format("PagedMemorySize:\t{0}\r\n", proc.PagedMemorySize64);
                        strW += string.Format("PagedSystemMemorySize:\t{0}\r\n", proc.PagedSystemMemorySize64);
                        strW += string.Format("PeakPagedMemorySize:\t{0}\r\n", proc.PeakPagedMemorySize64);
                        strW += string.Format("PeakVirtualMemorySize:\t{0}\r\n", proc.PeakVirtualMemorySize64);
                        strW += string.Format("PeakWorkingSet:\t{0}\r\n", proc.PeakWorkingSet64);
                        strW += string.Format("PrivateMemorySize:\t{0}\r\n", proc.PrivateMemorySize64);
                        strW += string.Format("VirtualMemorySize:\t{0}\r\n", proc.VirtualMemorySize64);
                        strW += string.Format("UserProcessorTime:\t{0}\r\n", proc.UserProcessorTime);
                        strW += string.Format("TotalProcessorTime:\t{0}\r\n", proc.TotalProcessorTime);
                        strW += string.Format("Responding:\t{0}\r\n", proc.Responding);

                        Instance.Log(LogTypes.Exception, strW);

                        //if (logFile != string.Empty || strW != string.Empty) return;
                        WriteToEmail("Test Not Responding", new string[] { Instance.GetLogFileByDate(LogTypes.Exception, DateTime.Today) }, strW);

                    });
                    thread.IsBackground = true;
                    thread.Priority = System.Threading.ThreadPriority.BelowNormal;
                    thread.Start();
                };
                time.Start();
            }
            catch
            {

            }
        }

        static private bool WriteToEmail(string subjectPrefix, string[] textFiles)
        {
            string userName = "receiveerror@gmail.com";
            string password = "receiveerror1234";
            string to = "receiveerror@gmail.com";

            string from = to;
            if (string.IsNullOrEmpty(to))
            {
                return true;
            }

            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                System.Net.NetworkCredential cred = new System.Net.NetworkCredential(userName, password);
                mail.To.Add(to);
                mail.Subject = subjectPrefix;

                mail.From = new System.Net.Mail.MailAddress(userName, from);
                mail.IsBodyHtml = false; // Accept HTML
                mail.Body = "Not Responding" + "\nComputer Name: " + System.Environment.MachineName;

                //Attachment file error list
                foreach (string file in textFiles)
                {
                    mail.Attachments.Add(new System.Net.Mail.Attachment(file));
                }

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("vikosoftware.com");
                smtp.UseDefaultCredentials = false;
                //smtp.EnableSsl = true; //for Gmail
                smtp.EnableSsl = true;
                smtp.Credentials = cred;

                smtp.Send(mail);
                foreach (System.Net.Mail.Attachment file in mail.Attachments)
                {
                    file.ContentStream.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        static private bool WriteToEmail(string subjectPrefix, string[] textFiles, string errorString)
        {
            string userName = "receiveerror@gmail.com";//"nguyenducduy@vikosoftware.com";
            string password = "receiveerror1234";//"hang05";

            string to = "receiveerror@gmail.com";//"nguyenducduy@vikosoftware.com";
            string from = to;
            if (string.IsNullOrEmpty(to))
            {
                return true;
            }

            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                System.Net.NetworkCredential cred = new System.Net.NetworkCredential(userName, password);
                mail.To.Add(to);
                mail.Subject = subjectPrefix;

                mail.From = new System.Net.Mail.MailAddress(userName, from);
                mail.IsBodyHtml = false; // Accept HTML
                mail.Body = string.Format("Not Responding" + "\nComputer Name: {0}\r\n{1}",
                    System.Environment.MachineName, errorString);

                //Attachment file error list
                foreach (string file in textFiles)
                {
                    mail.Attachments.Add(new System.Net.Mail.Attachment(file));
                }

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = cred;

                smtp.Send(mail);
                foreach (System.Net.Mail.Attachment file in mail.Attachments)
                {
                    file.ContentStream.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        static private bool SendReport()
        {
            string path = Instance.LogBaseDirectory();

            string[] bLogFile = Directory.GetFiles(path, "BLog*.log");
            foreach (var file in bLogFile)
            {
                DateTime writtenTimeFile = File.GetLastWriteTime(file);
                if ((DateTime.Now - writtenTimeFile).Days == 7)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
            }

            string[] logFiles = Directory.GetFiles(path, "Log*.log");
            bool result = WriteToEmail("Error List", logFiles);
            if (result)
            {
                foreach (var file in logFiles)
                {
                    string[] filePart = file.Split('\\');
                    File.Copy(file, path + "B" + filePart[filePart.Length - 1], true);
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
            }
            return result;
        }

        #endregion
    }
}
