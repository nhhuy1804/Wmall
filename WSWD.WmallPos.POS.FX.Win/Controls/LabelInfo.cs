using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace WSWD.WmallPos.POS.FX.Win.Controls
{
    /// <summary>
    /// 자동반품용
    /// 거래정보표시
    /// 
    /// 개발자     : TCL
    /// 개발일자   : 06.02
    /// 
    /// </summary>
    public partial class LabelInfo : UserControl
    {
        private bool m_canUp = false;
        private bool CanUp
        {
            get
            {
                return m_canUp;
            }
            set
            {
                m_canUp = value;
                btnUp.IsHighlight = value;
            }
        }

        private bool m_canDown = false;
        private bool CanDown
        {
            get
            {
                return m_canDown;
            }
            set
            {
                m_canDown = value;
                btnDown.IsHighlight = value;
            }
        }

        public LabelInfo()
        {
            InitializeComponent();

            //this.lblText.Font = new Font("돋움", 11, FontStyle.Regular);
            this.btnUp.MouseUp += new MouseEventHandler(btnUp_MouseUp);
            this.btnUp.MouseDown += new MouseEventHandler(btnUp_MouseDown);

            this.btnDown.MouseUp += new MouseEventHandler(btnUp_MouseUp);
            this.btnDown.MouseDown += new MouseEventHandler(btnUp_MouseDown);
        }

        /// <summary>
        /// 공지사항
        /// </summary>
        /// <param name="strNotice"></param>
        public void BindNoticeInfo(string strNotice)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    lblText.Visible = false;
                    lblText.AutoSize = true;
                    lblText.Text = strNotice;
                    int height = lblText.Height;
                    lblText.AutoSize = false;
                    lblText.Width = this.pnlContent.Width;
                    lblText.Height = height;
                    lblText.Visible = true;

                    lblText.Top = 0;
                    CanUp = false;
                    CanDown = lblText.Top + lblText.Height >= this.pnlContent.Height;
                });
            }
            else
            {
                lblText.Visible = false;
                lblText.AutoSize = true;
                lblText.Text = strNotice;
                int height = lblText.Height;
                lblText.AutoSize = false;
                lblText.Width = this.pnlContent.Width;
                lblText.Height = height;
                lblText.Visible = true;

                lblText.Top = 0;
                CanUp = false;
                CanDown = lblText.Top + lblText.Height >= this.pnlContent.Height;
            }
        }

        public void Clear()
        {
            lblText.Text = string.Empty;
        }

        void btnUp_MouseUp(object sender, MouseEventArgs e)
        {
            RoundedButton bt = (RoundedButton)sender;
            if (bt.Name.Contains("Up"))
            {
                bt.Image = Properties.Resources.ico_list_up;
                DoScroll(true);
            }
            else
            {
                bt.Image = Properties.Resources.ico_list_dn;
                DoScroll(false);
            }
        }

        void btnUp_MouseDown(object sender, MouseEventArgs e)
        {
            RoundedButton bt = (RoundedButton)sender;
            if (bt.Name.Contains("Up"))
            {
                bt.Image = Properties.Resources.ico_list_up_P;
            }
            else
            {
                bt.Image = Properties.Resources.ico_list_dn_P;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isUp"></param>
        void DoScroll(bool isUp)
        {
            int clientHeight = this.pnlContent.Height;
            if (clientHeight >= lblText.Height)
            {
                CanUp = false;
                CanDown = false;
                return;
            }

            if (isUp)
            {
                if (lblText.Top >= 0)
                {
                    return;
                }

                lblText.Visible = false;
                lblText.Top += clientHeight;
                lblText.Visible = true;
            }
            else
            {
                if (lblText.Top + lblText.Height < clientHeight)
                {
                    return;
                }

                lblText.Visible = false;
                lblText.Top -= clientHeight;
                lblText.Visible = true;
            }

            CanUp = lblText.Top < 0;
            CanDown = lblText.Top + lblText.Height >= clientHeight;
        }
    }
}
