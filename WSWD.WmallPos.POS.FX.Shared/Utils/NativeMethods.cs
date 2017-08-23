using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WSWD.WmallPos.POS.FX.Shared.Utils
{
    public class NativeMethods
    {
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        public static bool ActivateRunning()
        {
            /*
            const int SW_HIDE = 0;
            const int SW_SHOWNORMAL = 1;
            const int SW_SHOWMINIMIZED = 2;
            const int SW_SHOWMAXIMIZED = 3;
            const int SW_SHOWNOACTIVATE = 4;
            const int SW_RESTORE = 9;
            const int SW_SHOWDEFAULT = 10;
            */
            const int swRestore = 9;

            var me = Process.GetCurrentProcess();
            var arrProcesses = Process.GetProcessesByName(me.ProcessName);

            if (arrProcesses.Length > 1)
            {
                for (var i = 0; i < arrProcesses.Length; i++)
                {
                    if (arrProcesses[i].Id != me.Id)
                    {
                        // get the window handle
                        IntPtr hWnd = arrProcesses[i].MainWindowHandle;

                        // if iconic, we need to restore the window
                        if (IsIconic(hWnd))
                        {
                            ShowWindowAsync(hWnd, swRestore);
                        }

                        // bring it to the foreground
                        SetForegroundWindow(hWnd);
                        break;
                    }
                }
                return true;
            }

            return false;
        }

        public static void KillRunning(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);

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
    }
}
