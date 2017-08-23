using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;

namespace WSWD.WmallPos.POS.FX.Shared.Exceptions
{
    public abstract class HandlerBase
    {
        #region Handler State

        protected const string _defaultLogName = "UnhandledException.log";
        protected const string _rootException = "System.Web.HttpUnhandledException";
        protected const string _rootWsException = "System.Web.Services.Protocols.SoapException";

        protected string _logFilePath = Config.GetPath("PathLogFile");

        protected string _exceptionType = string.Empty;
        protected string _exceptionText = string.Empty;

        protected bool _logToDatabaseOK = false;
        protected bool _logToFileOK = false;
        protected bool _logToEventLogOK = false;

        protected NameValueCollection _results = new NameValueCollection();

        protected static Assembly _parentAssembly = null;
        #endregion

        #region Inputs

        protected static string ApplicationPath
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        protected static DateTime GetAssemblyBuildDate(Assembly a)
        {
            return GetAssemblyBuildDate(a, false);
        }

        protected static DateTime GetAssemblyBuildDate(Assembly asm, bool forceFileDate)
        {
            Version ver = asm.GetName().Version;
            DateTime result;

            if (forceFileDate)
                result = GetAssemblyFileTime(asm);
            else
            {
                result = new DateTime(2000, 1, 1).AddDays(ver.Build).AddSeconds(ver.Revision * 2);

                if (TimeZone.IsDaylightSavingTime(result, TimeZone.CurrentTimeZone.GetDaylightChanges(result.Year)))
                    result = result.AddHours(1);

                if (result > DateTime.Now || ver.Build < 730 || ver.Revision == 0)
                    result = GetAssemblyFileTime(asm);

            }

            return result;
        }

        //
        // exception-safe file attrib retrieval; we don't care if this fails
        //
        protected static DateTime GetAssemblyFileTime(Assembly asm)
        {
            try
            {
                return System.IO.File.GetLastWriteTime(asm.Location);
            }
            catch (Exception)
            {
                return DateTime.MaxValue;
            }
        }

        /// <summary>
        /// exception-safe WindowsIdentity.GetCurrent retrieval; returns "domain\username"
        /// </summary>
        /// <remarks>
        /// per MS, this can sometimes randomly fail with "Access Denied" on NT4
        /// </remarks>
        protected static string CurrentWindowsIdentity()
        {
            try
            {
                return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// exception-safe System.Environment "domain\username" retrieval
        /// </summary>
        protected static string CurrentEnvironmentIdentity()
        {
            try
            {
                return System.Environment.UserDomainName + "\\" + System.Environment.UserName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// retrieve Process identity with fallback on error to safer method
        /// </summary>
        protected static string ProcessIdentity()
        {
            string strTemp = CurrentWindowsIdentity();

            if (string.IsNullOrEmpty(strTemp))
                strTemp = CurrentEnvironmentIdentity();

            return strTemp;
        }

        #endregion

        #region Outputs

        /// <summary>
        /// Write exception
        /// </summary>
        /// <param name="isUnhandled"></param>
        /// <param name="source"></param>
        /// <param name="prefix"></param>
        /// <param name="attachmentFiles"></param>
        /// <param name="attachmentStreams"></param>
        /// <returns></returns>
        protected bool Write(bool isUnhandled, string source, 
            string prefix, 
            string[] attachmentFiles, 
            Dictionary<string, Stream> attachmentStreams)
        {
            bool result = false;

            if (!WriteToDatabase())
            {
                result = WriteToEventLog(source);
            }

            result &= WriteToFile(isUnhandled);
            result &= WriteToEmail(prefix, attachmentFiles, attachmentStreams);

            return result;
        }

        /// <summary>
        /// write current exception info to a text file; 
        /// requires write permissions for the target folder
        /// </summary>
        protected bool WriteToFile(bool unhandled)
        {
            string logFolderPath = GetExceptionLogFolder();
            string logFileName = GetExceptionLogFileName(unhandled);
            string logFile = Path.Combine(logFolderPath, _defaultLogName);

            StreamWriter sw = null;

            try
            {
                using (sw = new StreamWriter(logFile, true))
                {
                    sw.WriteLine("**** - " + DateTime.Now.ToString("s"));
                    sw.Write(_exceptionType);
                    sw.WriteLine();
                    sw.Write(_exceptionText);
                    sw.WriteLine();
                    sw.Close();
                }

                _logToFileOK = true;
            }
            catch (Exception ex)
            {
                _results.Add("LogToFile", ex.Message);
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
            return _logToFileOK;
        }

        /// <summary>
        /// send current exception info via email
        /// </summary>


        protected bool WriteToEmail(string subjectPrefix, string[] attachmentFiles, Dictionary<string, Stream> attachmentStreams)
        {
            bool mailOK = false;
            string userName = "receiveerror@gmail.com";
            string password = "apperror";
            mailOK = MailTo(userName, password, subjectPrefix, attachmentFiles, attachmentStreams);
            return mailOK;
        }

        private bool MailTo(string userName, string password, string subjectPrefix, string[] attachmentFiles, Dictionary<string, Stream> attachmentStreams)
        {
            string to = Config.GetString("EmailTo", userName);
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
                mail.Subject = "subject";

                mail.From = new System.Net.Mail.MailAddress(userName, from);
                mail.IsBodyHtml = false;
                mail.Body = _exceptionText;

                foreach (string file in attachmentFiles)
                {
                    try
                    {
                        if (file.Trim() == string.Empty) continue;
                        mail.Attachments.Add(new Attachment(file));
                    }
                    catch
                    {

                    }
                }

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com"); //seleccionamos nuetsro servidor smtp
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = cred;

                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                _results.Add("LogToEmail", ex.Message);
            }

            return false;
        }

        protected bool WriteToEventLog(string source)
        {
            try
            {
                EventLog.WriteEntry(source, Environment.NewLine + _exceptionText, EventLogEntryType.Error);

                _logToEventLogOK = true;
            }
            catch (Exception ex)
            {
                _results.Add("LogToEventLog", ex.Message);
            }
            return _logToEventLogOK;
        }

        public delegate void DatabaseWriteEventHandler(string type, string text);
        public static event DatabaseWriteEventHandler OnWriteToDatabase;

        protected bool WriteToDatabase()
        {
            _logToDatabaseOK = false;

            if (OnWriteToDatabase != null)
            {
                try
                {
                    OnWriteToDatabase(_exceptionType, _exceptionText);
                    _logToDatabaseOK = true;
                }
                catch (Exception ex)
                {
                    _results.Add("LogToDatabase", ex.ToString());
                }
            }
            else
            {
                _results.Add("LogToDatabase", "No subscriptions to OnWriteToDatabase event");
            }

            return _logToDatabaseOK;
        }

        #endregion

        #region String Conversions
        /// <summary>
        /// turns a single stack frame object into an informative string
        /// </summary>
        /// <param name="sf"></param>
        /// <returns></returns>
        protected string StackFrameToString(StackFrame sf)
        {
            StringBuilder sb = new StringBuilder();
            int intParam;
            MemberInfo mi = sf.GetMethod();

            // build method name
            sb.Append("	");
            sb.Append(mi.DeclaringType.Namespace);
            sb.Append(".");
            sb.Append(mi.DeclaringType.Name);
            sb.Append(".");
            sb.Append(mi.Name);

            // build method params
            ParameterInfo[] objParameters = sf.GetMethod().GetParameters();

            sb.Append("( ");
            intParam = 0;
            foreach (ParameterInfo objParameter in objParameters)
            {
                intParam += 1;
                if (intParam > 1) sb.Append(", ");

                sb.Append(objParameter.ParameterType.Name);
                sb.Append(" ");
                sb.Append(objParameter.Name);
            }
            sb.Append(" )");
            sb.Append(Environment.NewLine);

            // if source code is available, append location info
            sb.Append("		");
            if (string.IsNullOrEmpty(sf.GetFileName()))
            {
                if (_parentAssembly != null)
                    sb.Append(System.IO.Path.GetFileName(_parentAssembly.CodeBase));
                else
                    sb.Append("(unknown file)");

                // native code offset is always available
                sb.Append(": N ");
                sb.Append(String.Format("{0:#00000}", sf.GetNativeOffset()));
            }
            else
            {
                sb.Append(System.IO.Path.GetFileName(sf.GetFileName()));
                sb.Append(": line ");
                sb.Append(String.Format("{0:#0000}", sf.GetFileLineNumber()));
                sb.Append(", col ");
                sb.Append(String.Format("{0:#00}", sf.GetFileColumnNumber()));
                // if IL is available, append IL location info
                if (sf.GetILOffset() != System.Diagnostics.StackFrame.OFFSET_UNKNOWN)
                {
                    sb.Append(", IL ");
                    sb.Append(String.Format("{0:#0000}", sf.GetILOffset()));
                }
            }
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }

        protected abstract string SysInfoToString();

        /// <summary>
        /// translate exception object to string, with additional system info
        /// </summary>
        /// <param name="exceptn">Exception to be converted</param>
        /// <returns></returns>
        protected string ExceptionToString(Exception ex, bool includeSysInfo)
        {
            StringBuilder builder = new StringBuilder();

            // Inner exceptions are handled recursively
            if (!(ex.InnerException == null))
            {
                // sometimes the original exception is wrapped in a more relevant outer exception
                // the detail exception is the "inner" exception
                // see http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnbda/html/exceptdotnet.asp

                // don't return the outer root ASP exception; it is redundant.
                if (ex.GetType().ToString() == _rootException || ex.GetType().ToString() == _rootWsException)
                    return ExceptionToString(ex.InnerException, true);
                else
                {
                    builder.Append("(Inner Exception)");
                    builder.Append(Environment.NewLine);
                    builder.Append(ExceptionToString(ex.InnerException, false));
                    builder.Append(Environment.NewLine);
                    builder.Append("(Outer Exception)");
                    builder.Append(Environment.NewLine);
                }
            }

            // get general system and app information
            // we only really want to do this on the outermost exception in the stack
            if (includeSysInfo)
            {
                builder.Append(SysInfoToString());
                builder.Append(AssemblyInfoToString(ex));
                builder.Append(Environment.NewLine);
            }

            // get exception-specific information
            builder.Append("Exception Source:      ");
            try
            {
                builder.Append(ex.Source);
            }
            catch (Exception e)
            {
                builder.Append("* Exception Source Error: " + e.ToString());
            }

            builder.Append(Environment.NewLine);

            builder.Append("Exception Type:        ");
            try
            {
                builder.Append(ex.GetType().FullName);
            }
            catch (Exception e)
            {
                builder.Append("* Exception Type Error: " + e.ToString());
            }

            builder.Append(Environment.NewLine);
            builder.Append("Exception Message:     ");

            try
            {
                builder.Append(ex.Message);
            }
            catch (Exception e)
            {
                builder.Append("* Exception Message Error: " + e.ToString());
            }

            builder.Append(Environment.NewLine);
            builder.Append("Exception Target Site: ");

            try
            {
                builder.Append(ex.TargetSite.Name);
            }
            catch (Exception e)
            {
                builder.Append("* Exception Target Site Error: " + e.ToString());
            }

            builder.Append(Environment.NewLine);

            // Check for specific exception types (and output specific details if known)
            if (ex is SqlException)
            {
                SqlExceptionToString(builder, ex);
            }

            try
            {
                string x = EnhancedStackTrace2(ex);//EnhancedStackTrace( ex );
                builder.Append(x);
            }
            catch (Exception e)
            {
                builder.Append("* Exception Stack Trace Error: " + e.ToString());
            }

            builder.Append(Environment.NewLine);

            return builder.ToString();
        }

        private void SqlExceptionToString(StringBuilder builder, Exception exceptn)
        {
            SqlException se = exceptn as SqlException;

            builder.Append(Environment.NewLine);
            builder.Append("SQL Procedure:         ");
            try
            {
                builder.Append(se.Procedure);
            }
            catch (Exception e)
            {
                builder.Append(e.Message);
            }
            builder.Append(Environment.NewLine);

            builder.Append("SQL Server:            ");
            try
            {
                builder.Append(se.Server);
            }
            catch (Exception e)
            {
                builder.Append(e.Message);
            }
            builder.Append(Environment.NewLine);

            builder.Append("SQL State:             ");
            try
            {
                builder.Append(se.State);
            }
            catch (Exception e)
            {
                builder.Append(e.Message);
            }
            builder.Append(Environment.NewLine);

            builder.Append("SQL Source:            ");
            try
            {
                builder.Append(se.Source);
            }
            catch (Exception e)
            {
                builder.Append(e.Message);
            }
            builder.Append(Environment.NewLine);

            builder.Append("SQL Error Number:      ");
            try
            {
                builder.Append(se.Number);
            }
            catch (Exception e)
            {
                builder.Append(e.Message);
            }
            builder.Append(Environment.NewLine);

            builder.Append("SQL Line Number:       ");
            try
            {
                builder.Append(se.LineNumber);
            }
            catch (Exception e)
            {
                builder.Append(e.Message);
            }
            builder.Append(Environment.NewLine);

            foreach (SqlError error in se.Errors)
            {
                builder.Append("SQL Error Class:       ");
                try
                {
                    builder.Append(error.Class);
                }
                catch (Exception e)
                {
                    builder.Append(e.Message);
                }
                builder.Append(Environment.NewLine);

                builder.Append("SQL Error Line Number: ");
                try
                {
                    builder.Append(error.LineNumber);
                }
                catch (Exception e)
                {
                    builder.Append(e.Message);
                }
                builder.Append(Environment.NewLine);

                builder.Append("SQL Error Message:     ");
                try
                {
                    builder.Append(error.Message);
                }
                catch (Exception e)
                {
                    builder.Append(e.Message);
                }
                builder.Append(Environment.NewLine);

                builder.Append("SQL Error Number:      ");
                try
                {
                    builder.Append(error.Number);
                }
                catch (Exception e)
                {
                    builder.Append(e.Message);
                }
                builder.Append(Environment.NewLine);

                builder.Append("SQL Error Procedure:   ");
                try
                {
                    builder.Append(error.Procedure);
                }
                catch (Exception e)
                {
                    builder.Append(e.Message);
                }
                builder.Append(Environment.NewLine);

                builder.Append("SQL Error Server:      ");
                try
                {
                    builder.Append(error.Server);
                }
                catch (Exception e)
                {
                    builder.Append(e.Message);
                }
                builder.Append(Environment.NewLine);

                builder.Append("SQL Error Source:      ");
                try
                {
                    builder.Append(error.Source);
                }
                catch (Exception e)
                {
                    builder.Append(e.Message);
                }
                builder.Append(Environment.NewLine);

                builder.Append("SQL Error State:       ");
                try
                {
                    builder.Append(error.State);
                }
                catch (Exception e)
                {
                    builder.Append(e.Message);
                }
                builder.Append(Environment.NewLine);
                builder.Append(Environment.NewLine);
            }
        }

        /// <summary>
        /// retrieve relevant assembly details for this exception, if possible
        /// </summary>
        private string AssemblyInfoToString(Exception ex)
        {
            // ex.source USUALLY contains the name of the assembly that generated the exception
            // at least, according to the MSDN documentation..
            Assembly a = GetAssemblyFromName(ex.Source);

            if (a == null)
                return AllAssemblyDetailsToString();
            else
                return AssemblyDetailsToString(a);
        }

        /// <summary>
        /// matches assembly by Assembly.GetName.Name; returns nothing if no match
        /// </summary>
        private Assembly GetAssemblyFromName(string assemblyName)
        {
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (a.GetName().Name == assemblyName)
                    return a;
            }
            return null;
        }

        /// <summary>
        /// returns brief summary info for all assemblies in the current AppDomain
        /// </summary>
        private string AllAssemblyDetailsToString()
        {
            StringBuilder sb = new StringBuilder();
            const string strLineFormat = "    {0, -30} {1, -15} {2}";

            sb.Append(Environment.NewLine);
            sb.Append(String.Format(strLineFormat, "Assembly", "Version", "BuildDate"));
            sb.Append(Environment.NewLine);
            sb.Append(String.Format(strLineFormat, "--------", "-------", "---------"));
            sb.Append(Environment.NewLine);

            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                NameValueCollection nvc = AssemblyAttribs(a);
                // assemblies without versions are weird (dynamic?)
                if (nvc["Version"] != "0.0.0.0")
                {
                    sb.Append(String.Format(strLineFormat,
                        System.IO.Path.GetFileName(nvc["CodeBase"]), nvc["Version"], nvc["BuildDate"]));
                    sb.Append(Environment.NewLine);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// returns string name / string value pair of all attribs for the specified assembly
        /// </summary>
        /// <remarks>
        /// note that Assembly* values are pulled from AssemblyInfo file in project folder
        ///
        /// Trademark       = AssemblyTrademark string
        /// Debuggable      = True
        /// GUID            = 7FDF68D5-8C6F-44C9-B391-117B5AFB5467
        /// CLSCompliant    = True
        /// Product         = AssemblyProduct string
        /// Copyright       = AssemblyCopyright string
        /// Company         = AssemblyCompany string
        /// Description     = AssemblyDescription string
        /// Title           = AssemblyTitle string
        /// </remarks>
        private NameValueCollection AssemblyAttribs(Assembly a)
        {
            string Name;
            string Value;
            NameValueCollection nvc = new NameValueCollection();

            object[] attrs = new object[0];
            try
            {
                attrs = a.GetCustomAttributes(false);
            }
            catch
            {
                attrs = new object[0];
            }

            foreach (object attrib in attrs)
            {
                Name = attrib.GetType().ToString();
                Value = "";

                switch (Name)
                {
                    case "System.Diagnostics.DebuggableAttribute":
                        Name = "Debuggable";
                        Value = ((System.Diagnostics.DebuggableAttribute)attrib).IsJITTrackingEnabled.ToString();
                        break;

                    case "System.CLSCompliantAttribute":
                        Name = "CLSCompliant";
                        Value = ((System.CLSCompliantAttribute)attrib).IsCompliant.ToString();
                        break;

                    case "System.Runtime.InteropServices.GuidAttribute":
                        Name = "GUID";
                        Value = ((System.Runtime.InteropServices.GuidAttribute)attrib).Value.ToString();
                        break;

                    case "System.Reflection.AssemblyTrademarkAttribute":
                        Name = "Trademark";
                        Value = ((AssemblyTrademarkAttribute)attrib).Trademark.ToString();
                        break;

                    case "System.Reflection.AssemblyProductAttribute":
                        Name = "Product";
                        Value = ((AssemblyProductAttribute)attrib).Product.ToString();
                        break;

                    case "System.Reflection.AssemblyCopyrightAttribute":
                        Name = "Copyright";
                        Value = ((AssemblyCopyrightAttribute)attrib).Copyright.ToString();
                        break;

                    case "System.Reflection.AssemblyCompanyAttribute":
                        Name = "Company";
                        Value = ((AssemblyCompanyAttribute)attrib).Company.ToString();
                        break;

                    case "System.Reflection.AssemblyTitleAttribute":
                        Name = "Title";
                        Value = ((AssemblyTitleAttribute)attrib).Title.ToString();
                        break;

                    case "System.Reflection.AssemblyDescriptionAttribute":
                        Name = "Description";
                        Value = ((AssemblyDescriptionAttribute)attrib).Description.ToString();
                        break;

                    default:
                        Console.WriteLine(Name);
                        break;
                }

                if (!string.IsNullOrEmpty(Value))
                {
                    if (string.IsNullOrEmpty(nvc[Name]))
                        nvc.Add(Name, Value);
                }
            }

            // add some extra values that are not in the AssemblyInfo, but nice to have
            nvc.Add("CodeBase", a.CodeBase.Replace("file:///", ""));
            nvc.Add("BuildDate", GetAssemblyBuildDate(a).ToString());
            nvc.Add("Version", a.GetName().Version.ToString());
            nvc.Add("FullName", a.FullName);

            return nvc;
        }

        /// <summary>
        /// returns more detailed information for a single assembly
        /// </summary>
        private string AssemblyDetailsToString(Assembly a)
        {
            StringBuilder sb = new StringBuilder();
            NameValueCollection nvc = AssemblyAttribs(a);

            sb.Append("Assembly Codebase:     ");

            try
            {
                sb.Append(nvc["CodeBase"]);
            }
            catch (Exception e)
            {
                sb.Append(e.Message);
            }

            sb.Append(Environment.NewLine);
            sb.Append("Assembly Full Name:    ");

            try
            {
                sb.Append(nvc["FullName"]);
            }
            catch (Exception e)
            {
                sb.Append(e.Message);
            }

            sb.Append(Environment.NewLine);

            sb.Append("Assembly Version:      ");
            try
            {
                sb.Append(nvc["Version"]);
            }
            catch (Exception e)
            {
                sb.Append(e.Message);
            }
            sb.Append(Environment.NewLine);

            sb.Append("Assembly Build Date:   ");
            try
            {
                sb.Append(nvc["BuildDate"]);
            }
            catch (Exception e)
            {
                sb.Append(e.Message);
            }
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }

        /// <summary>
        /// attempts to coerce the value object using the .ToString method if possible, 
        /// then appends a formatted key/value string pair to a StringBuilder. 
        /// will display the type name if the object cannot be coerced.
        /// </summary>
        protected void AppendLine(StringBuilder sb, string Key, object Value)
        {
            string strValue = "(NULL)";

            if (Value != null)
            {
                try
                {
                    strValue = Value.ToString();
                }
                catch (Exception)
                {
                    strValue = "(" + Value.GetType().ToString() + ")";
                }
            }

            AppendLine(sb, Key, strValue);
        }

        /// <summary>
        /// appends a formatted key/value string pair to a StringBuilder
        /// </summary>
        protected void AppendLine(StringBuilder sb, string Key, string strValue)
        {
            sb.Append(String.Format("    {0, -30}{1}", Key, strValue));
            sb.Append(Environment.NewLine);
        }

        #region Enhanced Stack Trace
        /// <summary>
        /// enhanced stack trace generator, using current execution as start point
        /// </summary>
        protected string EnhancedStackTrace()
        {
            return EnhancedStackTrace(new StackTrace(true), "ASPUnhandledException");
        }

        /// <summary>
        /// enhanced stack trace generator, using existing exception as start point
        /// </summary>
        protected string EnhancedStackTrace(Exception ex)
        {
            return EnhancedStackTrace(new StackTrace(ex), null);
        }

        protected string EnhancedStackTrace2(Exception ex)
        {
            return EnhancedStackTrace(new StackTrace(ex), ex, null);
        }

        /// <summary>
        /// enhanced stack trace generator (prebuilt stack trace, class name to skip)
        /// </summary>
        /// <param name="objStackTrace"></param>
        /// <param name="strSkipClassName"></param>
        /// <returns></returns>
        protected string EnhancedStackTrace(StackTrace objStackTrace, string strSkipClassName)
        {
            System.Text.StringBuilder sb = new StringBuilder();

            sb.Append(Environment.NewLine);
            sb.Append("---- Stack Trace ----");
            sb.Append(Environment.NewLine);

            for (int intFrame = 0; intFrame < objStackTrace.FrameCount; intFrame++)
            {
                StackFrame sf = objStackTrace.GetFrame(intFrame);
                MemberInfo mi = sf.GetMethod();

                if (!string.IsNullOrEmpty(strSkipClassName) && (mi.DeclaringType.Name.IndexOf(strSkipClassName) > -1))
                {
                    // don't include frames with this name
                }
                else
                {
                    sb.Append(StackFrameToString(sf));
                    sb.Append(Environment.NewLine);
                }
            }
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }
        protected string EnhancedStackTrace(StackTrace objStackTrace, Exception exception, string strSkipClassName)
        {
            System.Text.StringBuilder sb = new StringBuilder();

            sb.Append(Environment.NewLine);
            sb.Append("---- Stack Trace ----");
            sb.Append(Environment.NewLine);

            for (int intFrame = 0; intFrame < objStackTrace.FrameCount; intFrame++)
            {
                StackFrame sf = objStackTrace.GetFrame(intFrame);
                MemberInfo mi = sf.GetMethod();

                if (!string.IsNullOrEmpty(strSkipClassName) && (mi.DeclaringType.Name.IndexOf(strSkipClassName) > -1))
                {
                    // don't include frames with this name
                }
                else
                {
                    sb.Append(StackFrameToString(sf));
                    sb.Append(Environment.NewLine);
                }
            }
            sb.Append(exception.StackTrace);
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }

        #endregion

        #endregion

        #region Exception Log folder

        /// <summary>
        /// Default log file folder
        /// </summary>
        /// <returns></returns>
        protected virtual string GetExceptionLogFolder()
        {
            if (string.IsNullOrEmpty(_logFilePath))
            {
                _logFilePath = Path.Combine(ApplicationPath, "ErrorLog");
            }

            if (!Directory.Exists(_logFilePath))
            {
                Directory.CreateDirectory(_logFilePath);
            }

            return _logFilePath;
        }

        protected virtual string GetExceptionLogFileName(bool unhandled)
        {
            return _defaultLogName;
        }

        #endregion

    }
}
