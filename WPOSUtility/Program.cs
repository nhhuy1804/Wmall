using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Config;

namespace WSWD.WmallPos.POS.Utils
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
            Application.Run(new MainForm());
        }
    }
}
