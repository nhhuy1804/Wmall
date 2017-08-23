using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;

using WSWD.WmallPos.FX.Shared.Listeners;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.FX.Shared.Utils;

namespace BasketTestApp
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

            //LogUtils.Instance = new LogUtils();
            //WLogUtils.Instance = new WLogUtils();
            //Trace.Listeners.Add(new LogFileTraceListener(WLogUtils.Instance));

            //// Create directories
            //ConfigData config = new ConfigData()
            //{
            //    AppConfig = AppConfig.Load(),
            //    DevConfig = DevConfig.Load(),
            //    KeyMapConfig = KeyMapConfig.Load(),
            //    SysMessage = SysMessage.Load()
            //};

            //ConfigData.Initialize(config);
             

            Application.Run(new StartupForm());
        }
    }
}
