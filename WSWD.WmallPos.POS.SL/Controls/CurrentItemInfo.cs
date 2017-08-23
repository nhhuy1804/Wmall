using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.POS.FX.Win;
using WSWD.WmallPos.POS.SL.Data;
using WSWD.WmallPos.POS.FX.Win.Forms;

namespace WSWD.WmallPos.POS.SL.Controls
{
    public partial class CurrentItemInfo : UserControl
    {
        public CurrentItemInfo()
        {
            InitializeComponent();
            lblFormular.ForeColor = Color.FromArgb(242, 239, 249);
        }

        private Int64 m_price = 0;
        private Int64 m_qty = 0;

        public long Price
        {
            set
            {
                m_price = value;
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        ShowAmountCalc();
                    });
                }
                else
                {
                    ShowAmountCalc();
                }
            }
            private get
            {
                return m_price;
            }
        }

        public Int64 Qty
        {
            set
            {
                m_qty = value;
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        ShowAmountCalc();
                    });
                }
                else
                {
                    ShowAmountCalc();
                }
            }
            private get
            {
                return m_qty;
            }
        }

        public string CdDp { get; set; }

        private string m_nmClass = string.Empty;
        public string NmClass
        {
            get
            {
                return m_nmClass;
            }
            set
            {
                m_nmClass = value;
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        ShowItemText();
                    });
                }
                else
                {
                    ShowItemText();
                }
            }
        }

        private string m_nmItem = string.Empty;
        public string NmItem
        {
            get
            {
                return m_nmItem;
            }
            set
            {
                m_nmItem = value;
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        ShowItemText();
                    });
                }
                else
                {
                    ShowItemText();
                }
            }
        }

        public bool IsRefund
        {
            get
            {
                if (this.ParentForm == null)
                {
                    return false;
                }

                var fb = (FrameBase)this.ParentForm;
                return fb.StateRefund;
            }
        }

        void ShowAmountCalc()
        {
            lblFormular.Text = Price == 0 || Qty == 0 ? string.Empty : string.Format("{0:#,##0}x{2}{1} =", Price, Qty, 
                IsRefund ? "-" : string.Empty);
            lblTotal.Text = Price == 0 || Qty == 0 ? string.Empty : string.Format("{1}{0:#,##0}", Price * Qty,
                IsRefund ? "-" : string.Empty);

            if (lblTotal.Text.Length == 0)
            {
                return;
            }

            AutoResize();
        }

        void ShowItemText()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    lblItem.Text = SLExtensions.CDDP_PB.Equals(this.CdDp) ?
                        string.Format("{0}{1}", string.IsNullOrEmpty(NmClass) ? string.Empty : NmClass + "_",
                                NmItem) : NmItem;
                });
            }
            else
            {
                lblItem.Text = SLExtensions.CDDP_PB.Equals(this.CdDp) ?
                        string.Format("{0}{1}", string.IsNullOrEmpty(NmClass) ? string.Empty : NmClass + "_",
                                NmItem) : NmItem;
            }
        }

        /// <summary>
        /// Reset to empty
        /// </summary>
        public void ResetState()
        {
            lblItem.Text = lblFormular.Text = lblTotal.Text = string.Empty;
        }

        void AutoResize()
        {
            this.SuspendDrawing();

            lblFormular.Visible = false;

            lblFormular.AutoSize = true;
            int w = lblFormular.Width;
            lblFormular.AutoSize = false;
            lblFormular.Width = w;

            lblTotal.Visible = false;
            lblTotal.AutoSize = true;
            w = lblTotal.Width;
            lblTotal.AutoSize = false;
            lblTotal.Width = w;

            lblFormular.Visible = true;
            lblTotal.Visible = true;

            this.ResumeDrawing();
        }

        void AutoResize(Label lbl)
        {
            lbl.Parent.SuspendLayout();
            lbl.SuspendLayout();
            lbl.AutoSize = true;
            int w = lbl.Width;
            lbl.AutoSize = false;
            lbl.Width = w;
            lbl.ResumeLayout();
            lbl.Parent.ResumeLayout();
        }
    }
}
