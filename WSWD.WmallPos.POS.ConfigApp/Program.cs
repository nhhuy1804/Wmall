using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Shared.Utils;
using System.Text;
using System.IO;
using WSWD.WmallPos.FX.Shared.Config;

namespace WSWD.WmallPos.POS.Config
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppInitialize();            
            Application.Run(new MainForm());
        }

        static void AppInitialize()
        {
            string keyBoardHookFile = FXConsts.FOLDER_OCX.GetLibFolder() + "\\KeyboardHook.ocx";
            FileUtils.RegSvr32(keyBoardHookFile);

            ConfigData config = new ConfigData()
            {
                AppConfig = AppConfig.Load(),
                KeyMapConfig = KeyMapConfig.Load()
            };

            ConfigData.Initialize(config);

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogMessage((Exception)e.ExceptionObject);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogMessage(e.Exception);
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
                string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\log\error");
                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }
                string logFile = Path.Combine(logFolder, string.Format("error_{0:yyyyMMdd}.log", DateTime.Today));
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

    }
}
