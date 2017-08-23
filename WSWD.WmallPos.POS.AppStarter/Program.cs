using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace WSWD.WmallPos.POS.AppStarter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (NativeMethods.ActivateRunning())
            {
                return;
            }

            NativeMethods.KillRunning("WmallPOS");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UpdateForm());
        }
    }
}
