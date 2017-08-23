using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.SL.Data;
using WSWD.WmallPos.POS.SL.VI;
using WSWD.WmallPos.POS.SL.PI;
using WSWD.WmallPos.POS.SL.PT;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.FX.Shared;
using System.Diagnostics;

namespace WSWD.WmallPos.POS.SL.VC
{
    /// <summary>
    /// 화면       : 보류해제
    /// 개발자     : TCL
    /// 만든날짜   : 04.22
    /// </summary>
    public partial class POS_SL_P003 : PopupBase01, ISLP003View
    {
        #region Constants, 변수, 속성

        static string LABEL_NO = "NO";
        static string LABEL_ITEM = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00058");
        static string LABEL_TIME = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00073");
        static string LABEL_QTY = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00059");
        static string LABEL_AMT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00074");
        static string MSG_WAITING = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00022");
        static string MSG_ERR_INVALID_SCAN_NO_BORU = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00075");
        static string MSG_ERR_NO_BORU_NOT_EXISTS = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00076");

        private ISLP003Presenter m_presenter = null;
        private string m_selectedNoBoru = string.Empty;
        private bool m_scanner = false;

        private HoldUISelectionMode m_mode = HoldUISelectionMode.HoldTrxnList;
        HoldUISelectionMode SelectedMode
        {
            get
            {
                return m_mode;
            }
            set
            {
                m_mode = value;
                if (value == HoldUISelectionMode.HoldItemList)
                {
                    gpHoldList.SendToBack();
                }
                else
                {
                    gpItemsList.SendToBack();
                }
            }
        }

        #endregion

        #region 생성자, 초고로드

        public POS_SL_P003()
        {
            InitializeComponent();
            FormInitialize();
        }

        void FormInitialize()
        {
            m_presenter = new SLP003Presenter(this);

            #region 보류리스트
            gpHoldList.InitializeCell += new WSWD.WmallPos.POS.FX.Win.UserControls.CellDataBoundEventHandler(gpHoldList_InitializeCell);
            gpHoldList.RowDataBound += new WSWD.WmallPos.POS.FX.Win.UserControls.RowDataBoundEventHandler(gpHoldList_RowDataBound);

            gpHoldList.PageIndex = 0;
            gpHoldList.SelectedRowIndex = -1;

            gpHoldList.SetColumn(0, LABEL_NO, 110);
            gpHoldList.SetColumn(1, LABEL_TIME);
            gpHoldList.SetColumn(2, LABEL_AMT, 120);

            ((System.Windows.Forms.TableLayoutPanel)(gpHoldList.Controls[2])).RowStyles[1] = new RowStyle(SizeType.Absolute, 60);
            ((System.Windows.Forms.Label)(gpHoldList.Controls[2].Controls[1])).AutoSize = false;
            ((System.Windows.Forms.Label)(gpHoldList.Controls[2].Controls[1])).Font = new Font("돋움체", 9, FontStyle.Bold);

            #endregion

            #region 보류상품리스트
            gpItemsList.InitializeCell += new WSWD.WmallPos.POS.FX.Win.UserControls.CellDataBoundEventHandler(gpItemsList_InitializeCell);
            gpItemsList.RowDataBound += new WSWD.WmallPos.POS.FX.Win.UserControls.RowDataBoundEventHandler(gpItemsList_RowDataBound);

            gpItemsList.PageIndex = 0;
            gpItemsList.SelectedRowIndex = -1;
            gpItemsList.UnSelectable = true;

            gpItemsList.SetColumn(0, LABEL_NO, 40);
            gpItemsList.SetColumn(1, LABEL_ITEM);
            gpItemsList.SetColumn(2, LABEL_QTY, 50);
            gpItemsList.SetColumn(3, LABEL_AMT, 110);

            ((System.Windows.Forms.TableLayoutPanel)(gpItemsList.Controls[2])).RowStyles[1] = new RowStyle(SizeType.Absolute, 60);
            ((System.Windows.Forms.Label)(gpItemsList.Controls[2].Controls[1])).AutoSize = false;
            ((System.Windows.Forms.Label)(gpItemsList.Controls[2].Controls[1])).Font = new Font("돋움체", 9, FontStyle.Bold);

            #endregion

            this.StatusMessage = MSG_WAITING;
            POSDeviceManager.Scanner.DataEvent += new POSDataEventHandler(Scanner_DataEvent);

            this.KeyEvent += new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(POS_SL_P003_KeyEvent);
            this.Activated += new EventHandler(POS_SL_P003_Activated);
            this.Deactivate += new EventHandler(POS_SL_P003_Deactivate);
            this.FormClosed += new FormClosedEventHandler(POS_SL_P003_FormClosed);
            this.Load += new EventHandler(POS_SL_P003_Load);

            this.iptNoBoru.SetFocus();
        }

        /// <summary>
        /// 보류리스트
        /// </summary>
        /// <param name="holdList"></param>
        public void BindHoldList(SAT900TData[] holdList)
        {
            gpHoldList.ClearAll();
            foreach (var item in holdList)
            {
                gpHoldList.AddRow(item, false);
            }

            gpHoldList.PageIndex = 0;
            gpHoldList.RefreshPageRows();
            gpHoldList.SelectedRowIndex = 0;
            gpHoldList.BringToFront();
        }

        /// <summary>
        /// Bind 보류 항목
        /// </summary>
        /// <param name="itemList"></param>
        public void BindHoldItems(SAT900TItemData[] itemList)
        {
            gpItemsList.ClearAll();
            foreach (var item in itemList)
            {
                gpItemsList.AddRow(item, false);
            }

            gpItemsList.PageIndex = 0;
            gpItemsList.RefreshPageRows();
            gpItemsList.BringToFront();

            SelectedMode = HoldUISelectionMode.HoldItemList;
        }

        /// <summary>
        /// 보류조회
        /// </summary>
        void SelectBoruData()
        {
            if (string.IsNullOrEmpty(iptNoBoru.Text))
            {
                if (gpHoldList.RowCount == 0)
                {
                    return;
                }

                SAT900TData data = (SAT900TData)gpHoldList.GetSelectedRow().ItemData;
                iptNoBoru.Text = data.NoBoru;
            }

            if (this.iptNoBoru.Text.Length < 4)
            {
                this.ReportError(SLP003HoldErrorState.NoBoruNotExists);
                return;
            }

            string ddSale = ConfigData.Current.AppConfig.PosInfo.SaleDate;
            m_selectedNoBoru = string.Empty;
            if (this.iptNoBoru.Text.Length >= 4)
            {
                m_selectedNoBoru = iptNoBoru.Text.Substring(iptNoBoru.Text.Length - 4, 4);
            }

            if (iptNoBoru.Text.Length == 14)
            {
                ddSale = "20" + this.iptNoBoru.Text.Substring(0, 6);
                string posNo = this.iptNoBoru.Text.Substring(6, 4);
                if (!ConfigData.Current.AppConfig.PosInfo.PosNo.Equals(posNo))
                {
                    this.ReportError(SLP003HoldErrorState.NoBoruNotExists);
                    return;
                }
            }
            else if (iptNoBoru.Text.Length == 16)
            {
                ddSale = "20" + this.iptNoBoru.Text.Substring(2, 6);
                string posNo = this.iptNoBoru.Text.Substring(8, 4);
                if (!ConfigData.Current.AppConfig.PosInfo.PosNo.Equals(posNo))
                {
                    this.ReportError(SLP003HoldErrorState.NoBoruNotExists);
                    return;
                }
            }

            if (string.IsNullOrEmpty(m_selectedNoBoru))
            {
                this.ReportError(SLP003HoldErrorState.NoBoruNotExists);
                return;
            }

            iptNoBoru.Text = string.Empty;
            m_presenter.LoadHoldItems(ddSale, m_selectedNoBoru);
        }

        /// <summary>
        /// 오류메시지
        /// </summary>
        /// <param name="errorState"></param>
        public void ReportError(SLP003HoldErrorState errorState)
        {
            switch (errorState)
            {
                case SLP003HoldErrorState.NoError:
                    StatusMessage = string.Empty;
                    break;
                case SLP003HoldErrorState.NoBoruNotExists:
                    StatusMessage = MSG_ERR_NO_BORU_NOT_EXISTS;
                    iptNoBoru.Text = string.Empty;
                    break;
                case SLP003HoldErrorState.InvalidScanNoBoru:
                    StatusMessage = MSG_ERR_INVALID_SCAN_NO_BORU;
                    iptNoBoru.Text = string.Empty;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region 이벤트정의

        /// <summary>
        /// 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btOK_Click(object sender, EventArgs e)
        {
            if (m_mode == HoldUISelectionMode.HoldItemList)
            {
                var holdItems = m_presenter.ReleaseHoldTrxn(m_selectedNoBoru);

                this.ReturnResult.Add("RETURN_DATA", holdItems);
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                SelectBoruData();
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            if (SelectedMode == HoldUISelectionMode.HoldItemList)
            {
                this.iptNoBoru.Text = string.Empty;
                SelectedMode = HoldUISelectionMode.HoldTrxnList;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        void gpItemsList_InitializeCell(WSWD.WmallPos.POS.FX.Win.UserControls.CellDataBoundEventArgs e)
        {
            Label lbl = null;

            if (e.Cell.ColumnIndex == 0)
            {
                // NO
                lbl = new Label()
                {
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                e.Cell.Controls.Add(lbl);
            }
            else if (e.Cell.ColumnIndex == 1)
            {
                // 상품명
                lbl = new Label()
                {
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                e.Cell.Controls.Add(lbl);
            }
            else if (e.Cell.ColumnIndex == 2)
            {
                // 수량
                lbl = new Label()
                {
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleRight
                };
                e.Cell.Controls.Add(lbl);
            }
            else
            {
                // 금액
                lbl = new Label()
                {
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleRight
                };
                e.Cell.Controls.Add(lbl);
            }
        }

        void gpItemsList_RowDataBound(WSWD.WmallPos.POS.FX.Win.UserControls.RowDataBoundEventArgs e)
        {
            SAT900TItemData data = e.ItemData == null ? new SAT900TItemData() : e.ItemData as SAT900TItemData;
            e.Row.Cells[0].Controls[0].Text = string.IsNullOrEmpty(data.NmItem) ? string.Empty : data.SqBoru.ToString();
            e.Row.Cells[1].Controls[0].Text = data.NmItem;
            e.Row.Cells[2].Controls[0].Text = data.QtItem.MoneyToText();
            e.Row.Cells[3].Controls[0].Text = data.AmItem.MoneyToText();
        }

        void gpHoldList_InitializeCell(WSWD.WmallPos.POS.FX.Win.UserControls.CellDataBoundEventArgs e)
        {
            Label lbl;

            if (e.Cell.ColumnIndex == 0)
            {
                // NO
                lbl = new Label()
                {
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                e.Cell.Controls.Add(lbl);
            }
            else if (e.Cell.ColumnIndex == 1)
            {
                // 시간
                lbl = new Label()
                {
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                e.Cell.Controls.Add(lbl);
            }
            else
            {
                // 금액
                lbl = new Label()
                {
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleRight,
                };
                e.Cell.Controls.Add(lbl);
            }
        }

        void gpHoldList_RowDataBound(WSWD.WmallPos.POS.FX.Win.UserControls.RowDataBoundEventArgs e)
        {
            SAT900TData data = e.ItemData != null ? (SAT900TData)e.ItemData : new SAT900TData();
            e.Row.Cells[0].Controls[0].Text = data.NoBoru;
            e.Row.Cells[1].Controls[0].Text = data.DdTime;
            e.Row.Cells[2].Controls[0].Text = data.AmSale.MoneyToText();
        }

        void POS_SL_P003_Load(object sender, EventArgs e)
        {
            m_presenter.LoadHoldList();
        }

        /// <summary>
        ///           ① BarCode 체계 : 'A' + YYMMDD+POS번호(4자리) + 거래번호(4자리) + 'A'
        /// </summary>
        /// <param name="eventData"></param>
        void Scanner_DataEvent(string eventData)
        {
            Trace.WriteLine("SL_P003_Scanner_DataEvent " + eventData + "-On: " + m_scanner.ToString(), "program");

            if (!m_scanner)
            {
                return;
            }

            iptNoBoru.Text = eventData;
            if (!m_presenter.ValidateNoBoru(eventData))
            {
                iptNoBoru.Text = string.Empty;
                return;
            }

            SelectBoruData();
        }

        void POS_SL_P003_Deactivate(object sender, EventArgs e)
        {
            m_scanner = false;
        }

        void POS_SL_P003_Activated(object sender, EventArgs e)
        {
            m_scanner = true;
        }

        void POS_SL_P003_FormClosed(object sender, FormClosedEventArgs e)
        {
            POSDeviceManager.Scanner.DataEvent -= new POSDataEventHandler(Scanner_DataEvent);
            gpHoldList.InitializeCell -= new WSWD.WmallPos.POS.FX.Win.UserControls.CellDataBoundEventHandler(gpHoldList_InitializeCell);
            gpHoldList.RowDataBound -= new WSWD.WmallPos.POS.FX.Win.UserControls.RowDataBoundEventHandler(gpHoldList_RowDataBound);
            gpItemsList.InitializeCell -= new WSWD.WmallPos.POS.FX.Win.UserControls.CellDataBoundEventHandler(gpItemsList_InitializeCell);
            gpItemsList.RowDataBound -= new WSWD.WmallPos.POS.FX.Win.UserControls.RowDataBoundEventHandler(gpItemsList_RowDataBound);

            this.KeyEvent -= new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(POS_SL_P003_KeyEvent);
            this.Activated -= new EventHandler(POS_SL_P003_Activated);
            this.Deactivate -= new EventHandler(POS_SL_P003_Deactivate);
            this.FormClosed -= new FormClosedEventHandler(POS_SL_P003_FormClosed);
            this.Load -= new EventHandler(POS_SL_P003_Load);
        
            this.btOK.Click -= new System.EventHandler(this.btOK_Click);
            this.btClose.Click -= new System.EventHandler(this.btClose_Click);
        }

        void POS_SL_P003_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_ENTER)
            {
                e.IsHandled = true;
                btOK_Click(btOK, EventArgs.Empty);
            }
            else if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_CLEAR)
            {
                if (iptNoBoru.Text.Length > 0)
                {
                    return;
                }
                e.IsHandled = true;
                btClose_Click(btClose, EventArgs.Empty);
            }
        }

        #endregion

    }

    internal enum HoldUISelectionMode
    {
        HoldTrxnList,
        HoldItemList
    }
}
