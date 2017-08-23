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
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public partial class TopBarV2 : UserControl
    {
        #region 초기화

        public TopBarV2()
        {
            InitializeComponent();
            ControlInit();
        }

        private void ControlInit()
        {
            this.Load += new EventHandler(TopBar_Load);
            this.ControlRemoved += new ControlEventHandler(TopBar_ControlRemoved);
            this.lblNoticeState.Click += new EventHandler(lblNoticeState_Click);
            tmSystemTime.Tick += new EventHandler(tmSystemTime_Tick);
        }

        #endregion

        #region 속성 & 이벤트

        public event EventHandler NoticeClick;

        private bool m_hasNotice = false;
        public bool HasNotice
        {
            get
            {
                return m_hasNotice;
            }
            set
            {
                m_hasNotice = value;                
            }
        }

        void UpdateNoticeState()
        {
            if (m_hasNotice)
            {
                bool flag = lblNoticeState.Tag == null ? false : (bool)lblNoticeState.Tag;
                flag = !flag;

                lblNoticeState.Image = flag ? Properties.Resources.notice_state_new : Properties.Resources.notice_state_norm;
                lblNoticeState.Tag = flag;
            }
            else
            {
                lblNoticeState.Image = Properties.Resources.notice_state_norm;
            }

            if (ConfigData.Current != null && ConfigData.Current.AppConfig != null)
            {
                string sysDate = DateTime.Today.ToString("yyyyMMdd");
                if (!sysDate.Equals(ConfigData.Current.AppConfig.PosInfo.SaleDate))
                {
                    bool flag = tbiSaleDate.Tag == null ? false : (bool)tbiSaleDate.Tag;
                    flag = !flag;

                    tbiSaleDate.ForeColor = flag ?
                        Color.FromArgb(247, 239, 170) :
                        Color.FromArgb(255, 0, 0);

                    tbiSaleDate.Tag = flag;
                }
                else
                {
                    tbiSaleDate.ForeColor = Color.FromArgb(247, 239, 170);
                }
            }
        }

        private bool m_serverConnected = false;
        public bool ServerConnected
        {
            get
            {
                return m_serverConnected;
            }
            set
            {
                m_serverConnected = value;
                if (DesignMode)
                {
                    return;
                }
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        lblServerState.Image = value ? Properties.Resources.server_state_conn : 
                            Properties.Resources.server_state_disc;
                    });
                }
                else
                {
                    lblServerState.Image = value ? Properties.Resources.server_state_conn :
                        Properties.Resources.server_state_disc;
                }
            }
        }

        /// <summary>
        /// 보류건수
        /// </summary>
        public int SaleHoldCount
        {
            set
            {
                if (InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        tbiSaleHold.Text = value.ToString();
                    });
                }
                else
                {
                    tbiSaleHold.Text = value.ToString();
                }
            }
        }

        private int m_uploadedTransCount = 0;
        /// <summary>
        /// 업로드건수
        /// </summary>
        public int UploadedTransCount
        {
            get
            {
                return m_uploadedTransCount;
            }
            set
            {
                m_uploadedTransCount = value;

                if (value == 0)
                {
                    return;
                }

                if (InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        tbiDataStatus.Text = string.Format("{0}/{1}", UploadedTransCount, TotalTransCount);
                    });
                }
                else
                {
                    tbiDataStatus.Text = string.Format("{0}/{1}", UploadedTransCount, TotalTransCount);
                }
            }
        }

        private int m_totalTransCount = 0;
        /// <summary>
        /// Total trans count
        /// </summary>
        public int TotalTransCount
        {
            get
            {
                return m_totalTransCount;
            }
            set
            {
                m_totalTransCount = value;

                if (value == 0)
                {
                    return;
                }

                if (InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        tbiDataStatus.Text = string.Format("{0}/{1}", UploadedTransCount, TotalTransCount);
                    });
                }
                else
                {
                    tbiDataStatus.Text = string.Format("{0}/{1}", UploadedTransCount, TotalTransCount);
                }

            }
        }

        /// <summary>
        /// set 반품상태
        /// </summary>
        private bool m_stateRefund = false;
        public bool StateRefund
        {
            set
            {
                m_stateRefund = value;
                if (value)
                {
                    this.BackColor = Color.FromArgb(165, 32, 33);
                }
                else
                {
                    this.BackColor = Color.FromArgb(61, 39, 111);
                }
            }
            get
            {
                return m_stateRefund;
            }
        }

        public string ActiveTitle
        {
            set
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        lblActiveTitle.Text = value;
                    });
                }
                else
                {
                    lblActiveTitle.Text = value;
                }
            }
        }

        public override Font Font
        {
            get
            {
                return new Font("돋움", 9, FontStyle.Bold);                
            }
        }

        #endregion

        #region 이벤트처리

        void TopBar_ControlRemoved(object sender, ControlEventArgs e)
        {
            tmSystemTime.Enabled = false;
        }

        void TopBar_Load(object sender, EventArgs e)
        {
            tmSystemTime.Enabled = true;
        }

        void tmSystemTime_Tick(object sender, EventArgs e)
        {
            tbiSysTime.Text = string.Format("{0:yyyy/MM/dd} ({1}) {0:HH:mm:ss}", DateTime.Now, 
                DateTimeHelper.GetDateOfWeekNameShort(DateTime.Today));
            UpdateNoticeState();
        }

        void lblNoticeState_Click(object sender, EventArgs e)
        {
            if (NoticeClick != null)
            {
                NoticeClick(sender, e);
            }
        }

        public void RefreshGlobalConfig()
        {
            if (ConfigData.Current == null || ConfigData.Current.AppConfig == null || ConfigData.Current.AppConfig.PosInfo == null)
            {
                return;
            }

            this.SuspendLayout();
            tbiPOSNo.Text = ConfigData.Current.AppConfig.PosInfo.PosNo;
            tbiCasName.Text = ConfigData.Current.AppConfig.PosInfo.CasName;
            tbiSaleDate.Text = DateTimeUtils.FormatDateString(ConfigData.Current.AppConfig.PosInfo.SaleDate, "-");

            tbiTranNo.Text = ConfigData.Current.AppConfig.PosInfo.TrxnNo;
            tbiStoreNm.Text = ConfigData.Current.AppConfig.PosInfo.StoreName;
            this.ResumeLayout();
        }

        #endregion
    }
}
