using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSWD.WmallPos.POS.FX.Win.Forms;

namespace WSWD.WmallPos.POS.SL.Controls
{
    public partial class SalePayItem : UserControl
    {
        public SalePayItem()
        {
            InitializeComponent();
        }

        bool IsRefund
        {
            get
            {
                if (this.ParentForm != null)
                {
                    var fb = (FrameBase)this.ParentForm;
                    return fb.StateRefund;
                }
                return false;
            }
        }

        public string PayItemKey { get; set; }

        public string PayItemName
        {
            get
            {
                return lblPayItemName.Text;
            }
            set
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        lblPayItemName.Text = value;
                    });
                }
                else
                {
                    lblPayItemName.Text = value;
                }
            }
        }

        public override Color ForeColor
        {
            get
            {
                return Color.FromArgb(96, 96, 96);
            }            
        }

        private long m_payAmt = 0;
        [Category("Appearance"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public long PayAmt
        {
            get
            {
                return m_payAmt;
            }
            set
            {
                m_payAmt = value;
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        lblPayAmt.Text = string.IsNullOrEmpty(PayItemName) ? string.Empty :
                            string.Format("{1}{0:#,##0}", value, IsRefund && value > 0 ? "-" : "");
                    });
                }
                else
                {
                    lblPayAmt.Text = string.IsNullOrEmpty(PayItemName) ? string.Empty :
                            string.Format("{1}{0:#,##0}", value, IsRefund && value > 0 ? "-" : "");
                }
            }
        }
    }
}
