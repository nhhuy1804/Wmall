using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSWD.WmallPos.POS.FX.Win.Controls;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.Shared.Utils;
using System.Runtime.InteropServices;
using WSWD.WmallPos.POS.FX.Win.Forms;

namespace WSWD.WmallPos.POS.SL.Controls
{
    public partial class SaleSummaryControl : BorderUserControl
    {
        
        public SaleSummaryControl()
        {
            InitializeComponent();
            m_pays = new List<BasketPay>();
            this.btnUp.MouseUp += new MouseEventHandler(btnUp_MouseUp);
            this.btnUp.MouseDown += new MouseEventHandler(btnUp_MouseDown);
            this.btnDn.MouseUp += new MouseEventHandler(btnUp_MouseUp);
            this.btnDn.MouseDown += new MouseEventHandler(btnUp_MouseDown);
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

        void btnUp_MouseUp(object sender, MouseEventArgs e)
        {
            RoundedButton bt = (RoundedButton)sender;
            if (bt.Name.Contains("Up"))
            {
                bt.Image = Properties.Resources.ico_list_up;
                if (m_scrollPos < 0)
                {
                    m_scrollPos++;
                    ScrollPaysIntoView();
                }
            }
            else
            {
                bt.Image = Properties.Resources.ico_list_dn;
                if (m_scrollPos > m_maxScroll)
                {
                    m_scrollPos--;
                    ScrollPaysIntoView();
                }
            }
        }

        public string RecvMoney
        {
            set
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        sumRecvMoney.Text = value;
                    });
                }
                else
                {
                    sumRecvMoney.Text = value;
                }
            }
            get
            {
                return sumRecvMoney.Text.Replace(",", "");
            }
        }

        public string PaidMoney
        {
            set
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        sumPaidMoney.Text = value;
                    });
                }
                else
                {
                    sumPaidMoney.Text = value;
                }
            }
        }

        public string DiscAmt
        {
            set
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        sumDisc.Text = value;
                    });
                }
                else
                {
                    sumDisc.Text = value;
                }
            }
        }

        public string ChangeAmt
        {
            set
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        sumChange.Text = value;
                    });
                }
                else
                {
                    sumChange.Text = value;
                }
            }
        }

        public string TotalAmt
        {
            set
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        sumTotalAmt.Text = value;
                    });
                }
                else
                {
                    sumTotalAmt.Text = value;
                }
            }
            get
            {
                return sumTotalAmt.Text.Replace(",", "");
            }
        }

        bool IsRefund
        {
            get
            {
                var fb = (FrameBase)this.FindForm();
                return fb != null ? fb.StateRefund : false;
            }
        }

        public void ClearPayList()
        {
            foreach (var item in this.lPnlPayList.Controls)
            {
                var pay = (SalePayItem)item;
                pay.PayItemKey = string.Empty;
                pay.PayItemName = string.Empty;
                pay.PayAmt = 0;
            }
        }

        private List<BasketPay> m_pays;
        private int m_scrollPos = 0;
        private int m_maxScroll = 0;

        /// <summary>
        /// 결제내역표시
        /// </summary>
        /// <param name="pays"></param>
        public void UpdatePayList(List<BasketPay> pays)
        {
            UpdatePayList(pays, true);
        }

        /// <summary>
        /// 결제내역표시
        /// </summary>
        /// <param name="pays"></param>
        /// <param name="scrollToView"></param>
        public void UpdatePayList(List<BasketPay> pays, bool scrollToView)
        {
            ClearPayList();
            if (pays == null)
            {
                return;
            }

            m_pays.Clear();
            m_pays.AddRange(pays);

            // 4, 2, = 2 => 0;
            // 4,6, => -2; 3
            m_scrollPos = this.lPnlPayList.Controls.Count - pays.Count;
            m_maxScroll = this.lPnlPayList.Controls.Count - pays.Count;
            if (m_scrollPos > 0)
            {
                m_scrollPos = 0;
                m_maxScroll = 0;
            }

            if (scrollToView)
            {
                ScrollPaysIntoView();
            }
        }

        private void ScrollPaysIntoView()
        {
            int itemCount = Math.Min(this.lPnlPayList.Controls.Count, m_pays.Count);

            int fi = Math.Abs(m_scrollPos);
            int ci = 0;
            while (fi <= this.m_pays.Count - 1)
            {
                if (ci > this.lPnlPayList.Controls.Count - 1)
                {
                    break;
                }

                SalePayItem item = (SalePayItem)this.lPnlPayList.Controls[this.lPnlPayList.Controls.Count - ci - 1];

                var pay = m_pays[fi];
                item.PayItemKey = pay.PayDtlCd;
                item.PayItemName = NetCommConstants.PaymentTypeName(pay.PayGrpCd, pay.PayDtlCd);
                item.PayAmt = IsRefund ? TypeHelper.ToInt32(pay.PayAmt) -
                                TypeHelper.ToInt32(pay.BalAmt) : TypeHelper.ToInt32(pay.PayAmt);

                fi++;
                ci++;
            }
        }
    }
}
