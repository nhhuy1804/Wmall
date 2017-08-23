using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.SL.Controls
{
    public partial class AutoRtnPanel : UserControl
    {
        public event EventHandler Confirmed;
        public event EventHandler Cancelled;

        public AutoRtnPanel()
        {
            InitializeComponent();

            this.autoRtnButtons1.Confirmed += new EventHandler(autoRtnButtons1_Confirmed);
            this.autoRtnButtons1.Cancelled += new EventHandler(autoRtnButtons1_Cancelled);
        }

        void autoRtnButtons1_Cancelled(object sender, EventArgs e)
        {
            this.Cancelled(sender, e);
        }

        void autoRtnButtons1_Confirmed(object sender, EventArgs e)
        {
            this.Confirmed(sender, e);            
        }

        public void SetEnableButton(int btnIndex, bool enable)
        {
            this.autoRtnButtons1.SetEnableButton(btnIndex, enable);
        }

        public void BindTrxnInfo(Dictionary<string, string> keyValues)
        {
            autoRtnTrxnInfo1.BindTrxnInfo(keyValues);
        }

        public void ShowProgressMessage(string message)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    autoRtnProgress1.AddMessage(message);
                });
            }
            else
            {
                autoRtnProgress1.AddMessage(message);
            }
        }

        internal void ResetState()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    autoRtnButtons1.ResetState();
                    autoRtnProgress1.Clear();
                    autoRtnTrxnInfo1.Clear();
                });
            }
            else
            {
                autoRtnButtons1.ResetState();
                autoRtnProgress1.Clear();
                autoRtnTrxnInfo1.Clear();                
            }
        }
    }
}
