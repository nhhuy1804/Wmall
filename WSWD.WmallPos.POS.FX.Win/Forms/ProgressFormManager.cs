using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WSWD.WmallPos.FX.Shared.Utils;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    class ProgressFormManager
    {
        //Delegate for cross thread call to close
        private delegate void CloseDelegate();

        //The type of form to be displayed as the splash screen.
        private static ProgressForm progressForm = new ProgressForm();
        static public void ShowProgress(string message)
        {
            // Make sure it is only launched once.
            if (progressForm.Visible)
            {
                progressForm.SetMessage(message);
                return;
            }

            Thread thread = new Thread(new ParameterizedThreadStart(ProgressFormManager.ShowForm));
            thread.IsBackground = false;
            thread.Start(message);
        }

        static private void ShowForm(object message)
        {
            progressForm.SetMessage((string)message);
            progressForm.ShowDialog();
        }

        static public void CloseForm()
        {
            try
            {
                if (progressForm.InvokeRequired)
                {
                    progressForm.BeginInvoke((MethodInvoker)delegate()
                    {
                        //progressForm.Close();
                        //progressForm = null;
                        progressForm.Hide();
                    });
                }
                else
                {
                    //progressForm.Close();
                    //progressForm = null;
                    progressForm.Hide();
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
        }

    }
}
