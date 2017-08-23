using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using System.Diagnostics;

using System.Drawing;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.FX.Shared.Listeners;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.FX.Shared.Exceptions;
using WSWD.WmallPos.POS.FX.Shared.Exceptions;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.POS.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.FX.Shared.Helper;

namespace WSWD.WmallPos.POS.App
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new LogFileTraceListener(LogUtils.Instance));
            if (!AppInitialize())
                return;

            if (NativeMethods.ActivateRunning())
            {
                return;
            }
            
            TraceHelper.Instance.JournalWrite("PROGRAM", "WmallPOS 시작 되었습니다.");

#if (DEBUG)
            RunInDebugMode();
#else
            RunInReleaseMode();
#endif

            TraceHelper.Instance.JournalWrite("PROGRAM", "WmallPOS 종료 되었습니다.");
        }

        private static void RunInDebugMode()
        {
            POSExceptionHandler.AddHandler(true, true, false, new CustomExceptionPublisher());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);

            try
            {
                //Form1 t = new Form1();
                //t.Show();
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                POSExceptionHandler.ThreadExceptionHandler(new object(), new System.Threading.ThreadExceptionEventArgs(ex));
            }
            finally
            {
                ConfigData.Current.AppConfig.Save();
            }
        }

        private static void RunInReleaseMode()
        {
            POSExceptionHandler.AddHandler(true, true, true, new CustomExceptionPublisher());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                NativeMethods.KillRunning("AppStarter");
                NativeMethods.KillRunning("POSConfig");

                //Form1 t = new Form1();
                //t.Show();

                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                POSExceptionHandler.ThreadExceptionHandler(new object(), new System.Threading.ThreadExceptionEventArgs(ex));
            }
            finally
            {
                ConfigData.Current.AppConfig.Save();
            }
        }

        private static void KillAppStarter()
        {
            Process[] processes = Process.GetProcessesByName("AppStarter");

            //Loop through the running processes in with the same name
            foreach (Process process in processes)
            {
                try
                {
                    process.Kill();
                }
                catch { }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private class CustomExceptionPublisher : IExceptionPublisher
        {
            #region IExceptionPublisher Members

            public string GetExceptionLogFolder()
            {
                return Path.Combine(LogUtils.Instance.LogBaseDirectory(), "error");
            }

            public void ShowException(Exception ex)
            {
                try
                {
                    Application.OpenForms[0].BeginInvoke((MethodInvoker)delegate()
                    {
                        new MessageDialog(ex).ShowDialog(Application.OpenForms[0]);
                    });
                }
                catch
                {

                }
            }

            public void ShowException(string exceptionText)
            {
                try
                {
                    Application.OpenForms[0].BeginInvoke((MethodInvoker)delegate()
                    {
                        new MessageDialog(MessageDialogType.Error, null, string.Empty,
                            exceptionText).ShowDialog(Application.OpenForms[0]);
                    });
                }
                catch
                {

                }
            }

            public string GetExceptionLogFileName(bool unhandled)
            {
                return string.Format("error_{0:yyyyMMdd}", DateTime.Today);
            }

            #endregion
        }

        private static string MSG_POS_ENV_INVALID = "설정파일 존재하지 않습니다.\n포스가 종료 됩니다.";

        /// <summary>
        /// 
        /// </summary>
        static bool AppInitialize()
        {
            var exists = AppConfig.Exists() &&
                DevConfig.Exists() &&
                KeyMapConfig.Exists();

            exists &= MasterDbHelper.DbExists() && TranDbHelper.DbExists();

            if (exists)
            {
                // Create directories
                ConfigData config = new ConfigData()
                {
                    AppConfig = AppConfig.Load(),
                    DevConfig = DevConfig.Load(),
                    KeyMapConfig = KeyMapConfig.Load(),
                    SysMessage = SysMessage.Load()
                };

                ConfigData.Initialize(config);

                exists &= !string.IsNullOrEmpty(config.AppConfig.PosInfo.PosNo) &&
                        !string.IsNullOrEmpty(config.AppConfig.PosInfo.StoreNo);
            }

            if (!exists)
            {
                new MessageDialog(MessageDialogType.Warning, null, string.Empty,
                    MSG_POS_ENV_INVALID).ShowDialog();
            }
            else
            {
                // set path to damo
                AddPathSegments(FXConsts.FOLDER_DAMO.GetLibFolder());

                // DAMO 암호화 모듈 초기설정
                DataUtils.DamoInitialize();

                // OCXRegister
                string ksNetFile = FXConsts.FOLDER_OCX.GetLibFolder() + "\\KSNet_Dongle.ocx";
                FileUtils.RegSvr32(ksNetFile);

                string keyBoardHookFile = FXConsts.FOLDER_OCX.GetLibFolder() + "\\KeyboardHook.ocx";
                FileUtils.RegSvr32(keyBoardHookFile);
            }

            return exists;
        }

        #region helper funcs

        static void AddPathSegments(string pathSegment)
        {
            string allPaths = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            if (allPaths != null)
                allPaths = pathSegment + "; " + allPaths;
            else
                allPaths = pathSegment;
            Environment.SetEnvironmentVariable("PATH", allPaths, EnvironmentVariableTarget.Process);
        }

        #endregion
    }
}
