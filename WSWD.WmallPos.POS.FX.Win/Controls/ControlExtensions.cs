using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.FX.Win.Controls
{
    public static class ControlExtensions
    {
        public static void InvokeIfNeeded(this Control control, Action action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }

        public static void InvokeIfNeeded<T>(this Control control, Action<T> action, T arg)
        {
            if (control.InvokeRequired)
                control.Invoke(action, arg);
            else
                action(arg);
        }

        public static void BeginInvokeIfNeeded(this Control control, Action action)
        {
            if (control.InvokeRequired)
                control.BeginInvoke(action);
            else
                action();
        }

        public static void BeginInvokeIfNeeded<T>(this Control control, Action<T> action, T arg)
        {
            if (control.InvokeRequired)
                control.BeginInvoke(action, arg);
            else
                action(arg);
        }
    }
}
