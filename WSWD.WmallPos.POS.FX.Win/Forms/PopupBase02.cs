using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    public partial class PopupBase02 : PopupBase
    {
        public PopupBase02()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public override string StatusMessage
        {
            get
            {
                return MessageBar.Text;
            }
            set
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        MessageBar.Text = value;
                    });
                }
                else
                {
                    MessageBar.Text = value;
                }                
            }
        }
    }
}
