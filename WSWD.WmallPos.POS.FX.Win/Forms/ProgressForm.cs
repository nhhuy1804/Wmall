using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    public partial class ProgressForm : Form
    {
        // IMessageFilter msgFilter = null;
        static string DEF_MESSAGE = "처리 중입니다. 잠시만 기다려 주세요.";
        public ProgressForm(string message) : this()
        {
            if (!string.IsNullOrEmpty(message))
            {
                lblMessage.Text = message;
            }            
        }

        public ProgressForm()
        {
            InitializeComponent();
            
            this.Activated += new EventHandler(ProgressForm_Activated);
            this.FormClosed += new FormClosedEventHandler(ProgressForm_FormClosed);
            KeyInputForm.ProgressHandle = this.Handle;
        }

        void ProgressForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Activated -= new EventHandler(ProgressForm_Activated);
            this.FormClosed -= new FormClosedEventHandler(ProgressForm_FormClosed);
            
            if (this.Owner != null)
            {
                KeyInputForm.ActiveHandle = this.Owner.Handle;
            }            
        }

        void ProgressForm_Activated(object sender, EventArgs e)
        {
            KeyInputForm.ActiveHandle = this.Handle;
        }

        public void SetMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                lblMessage.Text = message;
            }
            else
            {
                lblMessage.Text = DEF_MESSAGE;
            }
        }

        /// <summary>
        /// Hide form
        /// </summary>
        public void HideProgress()
        {
            this.Hide();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.Focus();            
            KeyInputForm.ActiveHandle = this.Handle;
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            this.Focus();            
            KeyInputForm.ActiveHandle = this.Handle;
        }
    }

    class CaptureEvents : IMessageFilter
    {
        public bool PreFilterMessage(ref Message m)
        {
            return false;
        }
    }
}
