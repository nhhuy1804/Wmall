using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using WSWD.WmallPos.FX.Shared.Exceptions;

namespace WSWD.WmallPos.POS.FX.Shared.Exceptions
{
    public class POSExceptionHandler : HandlerBase
    {
        static IExceptionPublisher m_publisher = null;

        #region Static Entry Points

        public static void AddHandler(bool isConsoleApp)
        {
            _isConsoleApp = isConsoleApp;
            AddHandler(!_isConsoleApp, _isConsoleApp, _isConsoleApp, null);
        }

        public static void AddHandler(bool screenshot, bool showUser, bool kill, IExceptionPublisher publisher)
        {
            _makeScreenshot = screenshot;
            _logToUI = showUser;

            // track the parent assembly that set up error handling
            // need to call this NOW so we set it appropriately; otherwise
            // we may get the wrong assembly at exception time!
            if (Assembly.GetEntryAssembly() == null)
                _parentAssembly = Assembly.GetCallingAssembly();
            else
                _parentAssembly = Assembly.GetEntryAssembly();

            // Attach handlers
            m_publisher = publisher;

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(ThreadExceptionHandler);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="showToUI"></param>
        public static void HandleException(Exception ex, bool unhandled)
        {
            // display message to the user
            if (_logToUI)
            {
                try
                {
                    ExceptionToUI(ex);
                }
                catch
                {
                    // Already logged other problems, can't attempt to log this for the risk of a closed loop
                }
            }

            Thread thread = new Thread(new ParameterizedThreadStart(HandleExceptionThread));
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.Normal;
            thread.Start(new object[] { ex, unhandled });
        }

        private static void HandleExceptionThread(object userState)
        {
            Exception ex = (Exception)((object[])userState)[0];
            bool unhandled = (bool)((object[])userState)[1];
            POSExceptionHandler handler = new POSExceptionHandler();
            handler.HandleExceptionInternal(ex, _logToUI, unhandled);
        }

        #endregion

        #region Entry Point Worker Methods

        private void HandleExceptionInternal(Exception exceptn, bool showToUI, bool isUnhandled)
        {
            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            // Initialize properties
            if (!string.IsNullOrEmpty(_exceptionText))
            {
                _exceptionText += Environment.NewLine;
            }

            _exceptionType = string.Empty;
            _screenshotFullName = string.Empty;
            _logfileFullName = string.Empty;

            //_logToScreenshotOK = false;
            _logToDatabaseOK = false;
            _logToFileOK = false;
            _logToEventLogOK = false;

            // turn the exception into an informative string
            try
            {
                _exceptionText += ExceptionToString(exceptn, true);
                _exceptionType = exceptn.GetType().FullName;
            }
            catch (Exception ex)
            {
                _exceptionText += "Error while generating exception string: " + ex.ToString();
                _exceptionType = ex.GetType().FullName;
            }

            if (_makeScreenshot)
            {
                try
                {
                    TakeScreenshot();
                    _exceptionText += Environment.NewLine + "Screenshot placed at:	" + _screenshotFullName;
                }
                catch (Exception ex)
                {
                    _exceptionText += Environment.NewLine + "Screenshot Error:" + ExceptionToString(ex, true);
                }
            }

            Write(isUnhandled, AppDomain.CurrentDomain.FriendlyName, "Application Error: ", new string[] { _screenshotFullName },
                new Dictionary<string, Stream>());

            if (showToUI)
            {
                ExceptionToUI(_exceptionText);
            }

            Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        public static void ThreadExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs args)
        {
            HandleException(args.Exception, false);
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException((Exception)e.ExceptionObject, true);
        }

        #endregion

        #region Config Fields
        private static bool _makeScreenshot = false;
        private static bool _logToUI = false;
        // private static bool _killAppOnException = false;
        private static bool _isConsoleApp = false;

        private static string _screenshotFileName = "ErrorScreen";
        private static ImageFormat _screenshotImageFormat = ImageFormat.Png;
        #endregion

        #region Exception Processing State Fields

        private string _screenshotFullName = string.Empty;
        private string _logfileFullName = string.Empty;

        #endregion

        #region Inputs

        #region Win32api screenshot calls
        // Windows API calls necessary to support screen capture
        [DllImport("gdi32.dll")]
        private static extern int BitBlt(int hDestDC, int x, int y, int nWidth, int nHeight, int hSrcDC, int xSrc, int ySrc, int dwRop);

        [DllImport("user32.dll")]
        private static extern int GetDC(int hwnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(int hwnd, int hdc);
        #endregion

        private void TakeScreenshot()
        {
            // note that screenshotname does not include the file type extension
            try
            {
                Rectangle screenRect = Screen.PrimaryScreen.Bounds;
                Bitmap screenBitmap = new Bitmap(screenRect.Width, screenRect.Height);
                Graphics screenGraphics = Graphics.FromImage(screenBitmap);

                // get a device context to the windows desktop and our destination  bitmaps
                int hdcSrc = GetDC(0);
                IntPtr hdcDest = screenGraphics.GetHdc();

                // copy what is on the desktop to the bitmap
                const int SRCCOPY = 0xCC0020;
                BitBlt(hdcDest.ToInt32(), 0, 0, screenRect.Width, screenRect.Height, hdcSrc, 0, 0, SRCCOPY);

                // release device contexts
                screenGraphics.ReleaseHdc(hdcDest);
                ReleaseDC(0, hdcSrc);

                string extension = "." + _screenshotImageFormat.ToString().ToLower();

                // Find a name that isn't used (decorated by time)
                do
                {
                    _screenshotFullName = Path.Combine(GetExceptionLogFolder(), string.Format("{0}[{1}]", _screenshotFileName, DateTime.Now.Ticks));

                    if (Path.GetExtension(_screenshotFullName) != extension)
                        _screenshotFullName += extension;

                } while (File.Exists(_screenshotFullName));

                //switch( strFormatExtension )
                //{
                //case "jpeg":
                //   BitmapToJPEG( screenBitmap, strFilename, 80 );
                //   break;

                //default:
                screenBitmap.Save(_screenshotFullName, _screenshotImageFormat);
                //   break;
                //}

                //_logToScreenshotOK = true;
            }
            catch (Exception)
            {
                //_logToScreenshotOK = false;
            }
        }

        #endregion

        #region Outputs

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        private static void ExceptionToUI(Exception ex)
        {
            if (m_publisher != null)
            {
                m_publisher.ShowException(ex);
                return;
            }

            string msg = string.Format("{0} 오류가 발생했습니다.마지막 작업이 실패 했습니다.", ex.Message);

            //( "A {0} error occurred.  Your last action probably failed to finish.", _exceptionType );
            msg += Environment.NewLine + Environment.NewLine + "이 응용 프로그램을 종료합니다.";
            MessageBox.Show(msg, "Warning");
        }

        private static void ExceptionToUI(string exceptionText)
        {
            if (m_publisher != null)
            {
                m_publisher.ShowException(exceptionText);
                return;
            }

            string msg = string.Format("{0} 오류가 발생했습니다.마지막 작업이 실패 했습니다.", exceptionText);

            //( "A {0} error occurred.  Your last action probably failed to finish.", _exceptionType );
            msg += Environment.NewLine + Environment.NewLine + "이 응용 프로그램을 종료합니다.";
            MessageBox.Show(msg, "Warning");
        }

        #endregion

        #region String Conversions

        /// <summary>
        /// gather some system information that is helpful to diagnosing exception
        /// </summary>
        /// <returns></returns>
        protected override string SysInfoToString()
        {
            return SysInfoToString(false);
        }

        /// <summary>
        /// gather some system information that is helpful to diagnosing exception
        /// </summary>
        /// <param name="blnIncludeStackTrace"></param>
        /// <returns></returns>
        private string SysInfoToString(bool blnIncludeStackTrace)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("Date and Time:         ");
            builder.Append(DateTime.Now);
            builder.Append(Environment.NewLine);

            builder.Append("Machine Name:          ");
            try
            {
                builder.Append(Environment.MachineName);
            }
            catch (Exception e)
            {
                builder.Append(e.Message);
            }
            builder.Append(Environment.NewLine);

            builder.Append("IP Address:            ");
            builder.Append(GetCurrentIP());
            builder.Append(Environment.NewLine);

            builder.Append("Current User:          ");
            builder.Append(ProcessIdentity());
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);

            builder.Append("Application Domain:    ");
            try
            {
                builder.Append(System.AppDomain.CurrentDomain.FriendlyName);
            }
            catch (Exception e)
            {
                builder.Append(e.Message);
            }


            builder.Append(Environment.NewLine);
            builder.Append("Assembly Codebase:     ");
            try
            {
                builder.Append(_parentAssembly.CodeBase);
            }
            catch (Exception e)
            {
                builder.Append(e.Message);
            }
            builder.Append(Environment.NewLine);

            builder.Append("Assembly Full Name:    ");
            try
            {
                builder.Append(_parentAssembly.FullName);
            }
            catch (Exception e)
            {
                builder.Append(e.Message);
            }
            builder.Append(Environment.NewLine);

            builder.Append("Assembly Version:      ");
            try
            {
                builder.Append(_parentAssembly.GetName().Version.ToString());
            }
            catch (Exception e)
            {
                builder.Append(e.Message);
            }
            builder.Append(Environment.NewLine);

            builder.Append("Assembly Build Date:   ");
            try
            {
                builder.Append(GetAssemblyBuildDate(_parentAssembly, false).ToString());
            }
            catch (Exception e)
            {
                builder.Append(e.Message);
            }
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);

            if (blnIncludeStackTrace)
                builder.Append(EnhancedStackTrace());

            return builder.ToString();
        }

        /// <summary>
        /// get IP address of this machine
        /// </summary>
        /// <remarks>Not an ideal method for a number of reasons (guess why!) but the alternatives are very ugly</remarks>
        /// <returns></returns>
        private static string GetCurrentIP()
        {
            try
            {
                return System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[0].ToString();
            }
            catch (Exception)
            {
                return "127.0.0.1";
            }
        }

        #endregion

        #region FilePath, LogFolder

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetExceptionLogFolder()
        {
            if (m_publisher != null)
            {
                return m_publisher.GetExceptionLogFolder();
            }

            return base.GetExceptionLogFolder();
        }

        protected override string GetExceptionLogFileName(bool unhandled)
        {
            if (unhandled)
            {
                return base.GetExceptionLogFileName(unhandled);
            }
            else if (m_publisher != null)
            {
                return m_publisher.GetExceptionLogFileName(unhandled);
            }

            return string.Format("{error_{0:yyyyyMMdd}", DateTime.Today);
        }


        #endregion
    }
}
