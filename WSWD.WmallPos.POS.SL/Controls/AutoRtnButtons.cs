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
    public partial class AutoRtnButtons : UserControl
    {
        #region Events
        /// <summary>
        /// 자동반푼진행 확정
        /// </summary>
        public event EventHandler Confirmed;

        /// <summary>
        /// 자동반품취소
        /// </summary>
        public event EventHandler Cancelled;

        #endregion

        #region 이벤트정의
        public AutoRtnButtons()
        {
            InitializeComponent();

            this.btnConfirm.Click += new EventHandler(btnConfirm_Click);
            this.btnClose.Click += new EventHandler(btnClose_Click);
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            if (Cancelled != null)
            {
                this.Cancelled(sender, e);
            }
        }

        void btnConfirm_Click(object sender, EventArgs e)
        {
            if (Confirmed != null)
            {
                btnConfirm.Enabled = btnClose.Enabled = false;
                this.Confirmed(sender, e);
            }
        } 
        #endregion

        #region 사용자정의
        internal void SetEnableButton(int btnIndex, bool enabled)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    if (btnIndex == 0)
                    {
                        this.btnConfirm.Enabled = enabled;
                    }
                    else
                    {
                        this.btnClose.Enabled = enabled;
                    }
                });
            }
            else
            {
                if (btnIndex == 0)
                {
                    this.btnConfirm.Enabled = enabled;
                }
                else
                {
                    this.btnClose.Enabled = enabled;
                }
            }
        }

        internal void ResetState()
        {
            btnConfirm.Enabled = false;
            btnClose.Enabled = true;
        } 
        #endregion
    }
}
