using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.POS.FX.Win;
using WSWD.WmallPos.FX.Shared;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public partial class StatusBar : UserControl
    {
        public StatusBar()
        {
            InitializeComponent();
        }

        public override Font Font
        {
            get
            {
                return new Font("돋움", 9, FontStyle.Bold);                
            }
        }

        public string StatusMessage
        {
            set
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        lblStatusMessage.Text = value;
                    });
                }
                else
                {
                    lblStatusMessage.Text = value;
                }
            }
            get
            {
                return lblStatusMessage.Text;
            }
        }

        /// <summary>
        /// update status of communication
        /// </summary>
        /// <param name="item"></param>
        /// <param name="value"></param>
        public void UpdateCommStatus(string item, string value)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    lblItemStatus.UpdateItem(item, value);
                });
            }
            else
            {
                lblItemStatus.UpdateItem(item, value);
            }
        }

        public void RefreshGlobalConfig()
        {
            VersionText.Text = ConfigData.Current == null ? string.Empty : ("#######WMPOS10A1 / ###KSP-6000S1201 v" + ConfigData.Current.AppConfig.PosInfo.Version);
        }

        ///// <summary>
        ///// set 
        ///// </summary>
        //private bool m_stateRefund = false;
        //public bool StateRefund
        //{
        //    set
        //    {
        //        m_stateRefund = value;
        //        if (value)
        //        {
        //            this.BackColor = Color.FromArgb(165, 32, 33);
        //        }
        //        else
        //        {
        //            this.BackColor = Color.FromArgb(61, 39, 111);
        //        }
        //    }
        //    get
        //    {
        //        return m_stateRefund;
        //    }
        //}
    }
}
