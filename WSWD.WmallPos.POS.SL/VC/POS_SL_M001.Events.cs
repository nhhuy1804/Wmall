using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.POS.FX.Win;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.Data;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.FX.Shared.Data;
using WSWD.WmallPos.POS.FX.Win.UserControls;

using WSWD.WmallPos.POS.SL.VI;
using WSWD.WmallPos.POS.SL.PT;
using WSWD.WmallPos.POS.SL.PI;
using WSWD.WmallPos.POS.SL.Data;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PP;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PQ;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PV;
using WSWD.WmallPos.POS.PY.Data;

namespace WSWD.WmallPos.POS.SL.VC
{
    partial class POS_SL_M001 : ISLM001SaleView, ISLM001HoldView
    {
        #region Privates, 변수

        ISLM001Presenter m_presenter = null;
        ISLP003Presenter m_holdPresenter = null;
        private bool m_scannerOn = false;

        /// <summary>
        /// 상품리스트 및 결제내역 남는다
        /// </summary>
        private bool m_retainItems = false;
        private bool m_itemsLoading = false;
        #endregion

        #region 생성자의 로드

        void FormInit()
        {
            #region 화면속성
            this.HideMainMenu = true;
            #endregion

            ((System.Windows.Forms.TableLayoutPanel)(gpItems.Controls[2])).RowStyles[1] = new RowStyle(SizeType.Absolute, 60);
            ((System.Windows.Forms.Label)(gpItems.Controls[2].Controls[1])).AutoSize = false;
            ((System.Windows.Forms.Label)(gpItems.Controls[2].Controls[1])).Font = new Font("돋움체", 9, FontStyle.Bold);

            #region 이벤트등록 - EventRegistration

            this.Load += new EventHandler(POS_SL_M001_Load);
            this.Activated += new EventHandler(POS_SL_M001_Activated);
            this.Deactivated += new EventHandler(POS_SL_M001_Deactivate);
            this.Unload += new EventHandler(POS_SL_M001_Unload);
            this.KeyEvent += new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(POS_SL_M001_KeyEvent);
            POSDeviceManager.Scanner.DataEvent += new POSDataEventHandler(Scanner_DataEvent);

            autoRtnPanel1.Confirmed += new EventHandler(autoRtnPanel1_Confirmed);
            autoRtnPanel1.Cancelled += new EventHandler(autoRtnPanel1_Cancelled);

            #endregion

            #region Loading - 초기로드

            // 상품목록
            ItemsGrid_InitLayout();
            FormInitialize();
            InitFuncKeyGroup();

            #endregion

            #region 비지니스로긱생성
            m_presenter = new SLM001Presenter(this, this.saleItemGroup1, this.groupItemList1);
            m_holdPresenter = new SLP003Presenter(this);
            #endregion
        }

        #endregion

        #region Form Events, Form 함수, 초기화

        void POS_SL_M001_Load(object sender, EventArgs e)
        {
            KeyInputText.SetFocus();
        }

        void POS_SL_M001_Deactivate(object sender, EventArgs e)
        {
            m_scannerOn = false;
        }

        void POS_SL_M001_Activated(object sender, EventArgs e)
        {
            m_scannerOn = true;
            this.KeyInputText.SetFocus();
        }

        void POS_SL_M001_Unload(object sender, EventArgs e)
        {
            this.Load -= new EventHandler(POS_SL_M001_Load);
            this.Activated -= new EventHandler(POS_SL_M001_Activated);
            this.Deactivated -= new EventHandler(POS_SL_M001_Deactivate);
            this.Unload -= new EventHandler(POS_SL_M001_Unload);
            this.KeyEvent -= new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(POS_SL_M001_KeyEvent);
            funcKeyGroup1.KeyEvent -= new OPOSKeyEventHandler(funcKeyGroup1_KeyEvent);
            gpItems.InitializeCell -= new CellDataBoundEventHandler(ItemsGrid_InitializeCell);
            gpItems.RowSelected -= new RowSelectedEventHandler(ItemsGrid_RowSelected);
            gpItems.RowDataBound -= new RowDataBoundEventHandler(ItemsGrid_RowDataBound);
            gpItems.DataBoundCompleted -= new EventHandler(ItemsGrid_DataBoundCompleted);
            POSDeviceManager.Scanner.DataEvent -= new POSDataEventHandler(Scanner_DataEvent);

            // reset back
            FrameBaseData.Current.OnDataChanged(FrameBaseDataItem.StateRefund, false);
        }

        private void FormInitialize()
        {
            ProcessState = SaleProcessState.InputStarted;
        }

        #endregion

        #region 기능키 이벤트, 함수

        void InitFuncKeyGroup()
        {
            funcKeyGroup1.KeyEvent += new OPOSKeyEventHandler(funcKeyGroup1_KeyEvent);
        }

        void funcKeyGroup1_KeyEvent(OPOSKeyEventArgs e)
        {
            this.POS_SL_M001_KeyEvent(e);
        }

        #endregion

        #region 품번SCAN

        /// <summary>
        /// 품번SCAN EVENT
        /// </summary>
        /// <param name="eventData"></param>
        void Scanner_DataEvent(string eventData)
        {
            Trace.WriteLine("SL_M001_Scanner_DataEvent " + eventData + "-ON: " + m_scannerOn.ToString(), "program");

            if (!m_scannerOn)
            {
                return;
            }

            if (!m_presenter.ValidateKeyInput(OPOSMapKeys.INVALID, true, false))
            {
                ReportInvalidState(InvalidDataInputState.InvalidData);
                m_scannerOn = false;
                return;
            }

            m_presenter.ProcessScanCode(eventData);
            m_scannerOn = true;
        }

        #endregion

        #region Item Grid - 상품목록

        /// <summary>
        /// 그리도 이벤트 초기화, 처음로
        /// 할때만 함
        /// </summary>
        void ItemsGrid_InitLayout()
        {
            gpItems.InitializeCell += new CellDataBoundEventHandler(ItemsGrid_InitializeCell);
            gpItems.RowSelected += new RowSelectedEventHandler(ItemsGrid_RowSelected);
            gpItems.RowDataBound += new RowDataBoundEventHandler(ItemsGrid_RowDataBound);
            gpItems.DataBoundCompleted += new EventHandler(ItemsGrid_DataBoundCompleted);

            gpItems.ScrollType = ScrollTypes.PageChanged;
            gpItems.SetColumn(0, LABEL_COL_NO, 30);
            gpItems.SetColumn(1, LABEL_COL_ITEM);
            gpItems.SetColumn(2, LABEL_COL_QTY, 40);
            gpItems.SetColumn(3, LABEL_COL_UTSPRC, 100);
            gpItems.SetColumn(4, LABEL_COL_DPER, 60);
            gpItems.SetColumn(5, LABEL_COL_DAMT, 60);
            gpItems.SetColumn(6, LABEL_COL_IAMT, 110);

            gpItems.PageIndex = 0;
            gpItems.SelectedRowIndex = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ItemsGrid_DataBoundCompleted(object sender, EventArgs e)
        {
            if (!m_retainItems)
            {
                if (m_presenter != null)
                {
                    m_presenter.UpdateSummary();
                }
            }
        }

        /// <summary>
        /// 사직확인
        /// </summary>
        public void ItemsGrid_StartOperation()
        {
            if (m_retainItems)
            {
                m_retainItems = false;
                gpItems.ClearAll();
                gpItems.PageIndex = -1;
                currentItemInfo1.ResetState();
            }
        }

        /// <summary>
        /// 그리드 데이타 초기화
        /// </summary>
        public void ItemsGrid_DataInitialize(bool resetItems)
        {
            m_retainItems = !resetItems;
            if (resetItems)
            {
                gpItems.ClearAll();
                gpItems.PageIndex = -1;
                ItemsGrid_UpdateCurrentItemStatus(PBItemData.Empty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void ItemsGrid_RowDataBound(RowDataBoundEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    e.CancelSet = true;
                    ItemsGrid_UpdateItemRow(e.Row, e.ItemData as PBItemData);
                });
            }
            else
            {
                e.CancelSet = true;
                ItemsGrid_UpdateItemRow(e.Row, e.ItemData as PBItemData);
            }
        }

        /// <summary>
        /// 상품등록중 다른 row 클릭 방지 pageindex도 동일하게 클릭 방지용 변수생성(2015.08.26 정광호 수정)
        /// 
        /// 09.10 Loc 주석처리.
        /// </summary>
        //private bool bSelectItem = false;

        /// <summary>
        /// 행선택
        /// </summary>
        /// <param name="e"></param>
        void ItemsGrid_RowSelected(RowChangingEventArgs e)
        {
            /// 
            /// todo
            /// 1) if InputState is Inputing, cant change row
            /// 
            /// 

            //2015.08.26 정광호 수정-------------------------------------------
            //*****************************************************************
            //상품등록중 다른 row 클릭 방지 pageindex도 동일하게 클릭 방지
            //bSelectItem 변수를 생성하여 밑에 gpItems.SelectedRowIndex = gpItems.RowItems.Length - 1; 부분에서 무한루프 도는현상 해결
            /*
             * 09.10 Loc주석처리
             * 그리드의 속성으로 처리함.
             *
            if (bSelectItem) return;

            if (ProcessState == SaleProcessState.ItemInputing)
            {
                //2015.08.26 정광호 수정
                bSelectItem = true;
                gpItems.SelectedRowIndex = gpItems.RowItems.Length - 1;
                bSelectItem = false;
                e.Cancel = true;
                return;
            }

             * */

            // 그대로 에전소스 유지
            if (ProcessState == SaleProcessState.ItemInputing)
            {
                //2015.08.26 정광호 수정
                //gpItems.SelectedRowIndex = gpItems.RowItems.Length - 1;
                e.Cancel = true;
                return;
            }
            //*****************************************************************
            //-----------------------------------------------------------------

            if (!m_itemsLoading)
            {
                ItemsGrid_UpdateCurrentItemStatus(e.Row.ItemData as PBItemData);
            }
        }

        /// <summary>
        /// GRID내용만들기
        /// </summary>
        /// <param name="e"></param>
        void ItemsGrid_InitializeCell(CellDataBoundEventArgs e)
        {
            ItemsGrid_BuildRowCells(e.Cell);
        }

        /// <summary>
        /// 행만들기
        /// </summary>
        /// <param name="row"></param>
        void ItemsGrid_BuildRowCells(SaleGridCell cell)
        {
            Label lbl = null;

            switch (cell.ColumnIndex)
            {
                case SLExtensions.CELL_INDEX_NO:
                    #region ITEM NO

                    lbl = new Label();
                    lbl.Left = 0;
                    lbl.Top = 0;
                    lbl.AutoSize = false;
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.Height = cell.Height;
                    lbl.Width = cell.Width;
                    cell.Controls.Add(lbl);

                    #endregion
                    break;
                case SLExtensions.CELL_INDEX_ITEM:
                    #region 상품코드 & 명

                    // ITEMCODE
                    lbl = new Label();
                    lbl.Left = 0;
                    lbl.Top = 0;
                    lbl.AutoSize = false;
                    lbl.TextAlign = ContentAlignment.MiddleLeft;
                    lbl.Height = cell.Height / 2;
                    lbl.ForeColor = Color.FromArgb(110, 56, 180);
                    lbl.Width = cell.Width;
                    cell.Controls.Add(lbl);

                    // ITEMNAME
                    int h = lbl.Height;
                    lbl = new Label();
                    lbl.Left = 0;
                    lbl.Top = cell.Height - h;
                    lbl.AutoSize = false;
                    lbl.Height = cell.Height / 2;
                    lbl.Width = cell.Width;
                    lbl.ForeColor = Color.FromArgb(110, 56, 180);
                    cell.Controls.Add(lbl);

                    #endregion
                    break;
                case SLExtensions.CELL_INDEX_QTY:
                    #region 수량

                    lbl = new Label()
                    {
                        Left = 0,
                        Top = 0,
                        Width = cell.Width,
                        Height = cell.Height,
                        AutoSize = false,
                        TextAlign = ContentAlignment.MiddleRight
                    };

                    cell.Controls.Add(lbl);
                    #endregion
                    break;

                case SLExtensions.CELL_INDEX_PRICE:
                    #region 단가

                    lbl = new Label()
                    {
                        Left = 0,
                        Top = 0,
                        Width = cell.Width,
                        Height = cell.Height,
                        AutoSize = false,
                        TextAlign = ContentAlignment.MiddleRight
                    };
                    cell.Controls.Add(lbl);

                    #endregion

                    break;

                case SLExtensions.CELL_INDEX_DISCP:
                    #region % columns

                    lbl = new Label()
                    {
                        Left = 0,
                        Top = 0,
                        Width = cell.Width,
                        Height = cell.Height,
                        AutoSize = false,
                        TextAlign = ContentAlignment.MiddleRight
                    };
                    cell.Controls.Add(lbl);

                    #endregion
                    break;

                case SLExtensions.CELL_INDEX_DISCA:
                    #region Discount - 할인

                    lbl = new Label()
                    {
                        Left = 0,
                        Top = 0,
                        Width = cell.Width,
                        Height = cell.Height,
                        AutoSize = false,
                        TextAlign = ContentAlignment.MiddleRight
                    };
                    cell.Controls.Add(lbl);

                    #endregion

                    break;
                case SLExtensions.CELL_INDEX_AMT:
                    #region 금액 - Amount

                    lbl = new Label()
                    {
                        Left = 0,
                        Top = 0,
                        Width = cell.Width,
                        Height = cell.Height,
                        AutoSize = false,
                        TextAlign = ContentAlignment.MiddleRight
                    };
                    cell.Controls.Add(lbl);

                    #endregion

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Update 수량, 할인, 단가..등
        /// </summary>
        /// <param name="value"></param>
        public void ItemsGrid_UpdateItemRow(PBItemData value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    gpItems.UpdateCurrentRow(value);
                });
            }
            else
            {
                gpItems.UpdateCurrentRow(value);
            }
        }

        /// <summary>
        /// 현대행 업데이트
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="value"></param>
        public void ItemsGrid_UpdateItemRow(int rowIndex, PBItemData value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    gpItems.SelectedRowIndex = rowIndex;
                    gpItems.UpdateRow(rowIndex, value);
                });
            }
            else
            {
                gpItems.SelectedRowIndex = rowIndex;
                gpItems.UpdateRow(rowIndex, value);
            }
        }

        /// <summary>
        /// Next Row to be ready to input
        /// </summary>
        public void ItemsGrid_AddItemRow(PBItemData itemData, bool setSelected)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    gpItems.AddRow(itemData, setSelected);
                });
            }
            else
            {
                gpItems.AddRow(itemData, setSelected);
            }
        }

        /// <summary>
        /// Cancel editing on new row
        /// </summary>
        public void ItemsGrid_CancelNewRow()
        {
            gpItems.DeleteSelectedRow();

            currentItemInfo1.NmClass = string.Empty;
            currentItemInfo1.Qty = 0;
            currentItemInfo1.Price = 0;

            this.ProcessState = SaleProcessState.InputStarted;
            this.InputOperation = ItemInputOperation.None;

            GuideMessage = GUIDE_MSG_INTPUT_STARTED;
        }

        /// <summary>
        /// SubFunc for UpdateItemRow
        /// to be called in thread or async
        /// </summary>
        /// <param name="value"></param>
        void ItemsGrid_UpdateItemRow(SaleGridRow row, PBItemData value)
        {
            if (row.Cells.Length == 0)
            {
                return;
            }

            PBItemData curItemData = row.ItemData as PBItemData;
            // update Amount - 금액다시 계산
            if (curItemData == null)
            {
                curItemData = PBItemData.Empty;
            }

            bool setNull = false;
            if (value == null)
            {
                setNull = true;
                value = PBItemData.Empty;
                value.Properties = PBItemProperties.All;
            }

            // value.CompleteStep == Empty
            // 처음 Scan 된 상품이라 모든 셀의 값이 재설정한다
            bool isFirstParse = (value.ScannedStep > PBItemParseStep.Empty &&
                value.ScannedStep <= PBItemParseStep.CdItem) && value.CompletedStep == PBItemParseStep.Empty;
            if (isFirstParse ||
                (value.Properties & PBItemProperties.CdClass) == PBItemProperties.CdClass)
            {
                currentItemInfo1.CdDp = value.CdDp;
                row.Cells[SLExtensions.CELL_INDEX_ITEM].Controls[0].Text = SLExtensions.CDDP_PB.Equals(value.CdDp) ?
                    value.CdClass : string.Empty;
                row.Cells[SLExtensions.CELL_INDEX_QTY].Controls[0].Text = string.IsNullOrEmpty(value.CdClass) ?
                    string.Empty : 1.ToString();

                // Update row index
                row.Cells[SLExtensions.CELL_INDEX_NO].Controls[0].Text = string.IsNullOrEmpty(value.CdClass) ?
                    string.Empty : string.Format("{0:d2}", row.RowIndex + 1);
            }

            if (isFirstParse ||
                (value.Properties & PBItemProperties.NmClass) == PBItemProperties.NmClass)
            {
                row.Cells[SLExtensions.CELL_INDEX_ITEM].Controls[1].Text = SLExtensions.CDDP_PB.Equals(value.CdDp) ?
                    value.NmClass : string.Empty;
                if (row.VisibleIndex == gpItems.SelectedVisibleIndex)
                    currentItemInfo1.NmClass = SLExtensions.CDDP_PB.Equals(value.CdDp) ? value.NmClass : string.Empty;
            }

            if (isFirstParse ||
                (value.Properties & PBItemProperties.CdItem) == PBItemProperties.CdItem)
            {
                row.Cells[SLExtensions.CELL_INDEX_ITEM].Controls[0].Text += SLExtensions.CDDP_PB.Equals(value.CdDp) ?
                    value.CdItem : value.Barcode;
            }

            if (isFirstParse ||
                (value.Properties & PBItemProperties.NmItem) == PBItemProperties.NmItem)
            {
                if (SLExtensions.CDDP_PB.Equals(value.CdDp))
                {
                    string name = row.Cells[SLExtensions.CELL_INDEX_ITEM].Controls[1].Text;

                    row.Cells[SLExtensions.CELL_INDEX_ITEM].Controls[1].Text = string.Format("{0}{1}{2}",
                        name, (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value.NmItem) ?
                        "_" : string.Empty), value.NmItem);

                }
                else
                {
                    row.Cells[SLExtensions.CELL_INDEX_ITEM].Controls[1].Text = value.NmItem;
                }

                if (row.VisibleIndex == gpItems.SelectedVisibleIndex)
                {
                    currentItemInfo1.NmItem = value.NmItem;
                }
            }

            if (isFirstParse ||
                (value.Properties & PBItemProperties.FgClass) == PBItemProperties.FgClass)
            {
                row.Cells[SLExtensions.CELL_INDEX_ITEM].Controls[0].Text += value.FgClass;
            }

            if (isFirstParse ||
                (value.Properties & PBItemProperties.Qty) == PBItemProperties.Qty)
            {
                var qty = TypeHelper.ToInt64(value.Qty);
                if (qty == -1)
                {
                    // 수량정정
                    qty = TypeHelper.ToInt64(curItemData.Qty);
                    qty++;
                }

                /// update last value
                value.Qty = qty.ToString();
                string qtyText = qty.MoneyToText(!string.IsNullOrEmpty(value.CdClass));
                row.Cells[SLExtensions.CELL_INDEX_QTY].Controls[0].Text =
                    string.Format("{0}{1}", StateRefund && !string.IsNullOrEmpty(qtyText) ? "-" : string.Empty,
                    qtyText);
                row.Cells[SLExtensions.CELL_INDEX_QTY].Controls[0].ForeColor = Color.Empty;
                if (row.VisibleIndex == gpItems.SelectedVisibleIndex)
                    currentItemInfo1.Qty = qty;
            }

            // 지정취소
            if (isFirstParse ||
                (value.Properties & PBItemProperties.FgCanc) == PBItemProperties.FgCanc)
            {
                if ("0".Equals(value.FgCanc))
                {
                    row.Cells[SLExtensions.CELL_INDEX_QTY].Controls[0].ForeColor = Color.Empty;
                }
                else
                {
                    row.Cells[SLExtensions.CELL_INDEX_QTY].Controls[0].Text = "0";
                    row.Cells[SLExtensions.CELL_INDEX_QTY].Controls[0].ForeColor = Color.Red;
                    if (row.VisibleIndex == gpItems.SelectedVisibleIndex)
                        currentItemInfo1.Qty = 0;
                }
            }

            if (isFirstParse ||
                (value.Properties & PBItemProperties.Price) == PBItemProperties.Price)
            {
                var nPrice = TypeHelper.ToInt64(value.UtSprc);
                var oPrice = TypeHelper.ToInt64(curItemData.UtSprc);
                if (nPrice != oPrice && oPrice > 0)
                {
                    curItemData.FgUtSprcChanged = true;
                    if (row.VisibleIndex == gpItems.SelectedVisibleIndex)
                        currentItemInfo1.NmClass = "&" + currentItemInfo1.NmClass;
                }

                row.Cells[SLExtensions.CELL_INDEX_PRICE].Controls[0].Text = nPrice.MoneyToText();
                if (row.VisibleIndex == gpItems.SelectedVisibleIndex)
                    currentItemInfo1.Price = nPrice;
            }

            //curItemData.Merge(value);
            PBItemData copyItem = null;
            if (setNull)
            {
                copyItem = value;
                gpItems.SetRowData(row, value);
            }
            else
            {
                copyItem = curItemData.Copy();
                copyItem.Merge(value);
                gpItems.SetRowData(row, copyItem);
            }

            string amtText = copyItem.AmItem.MoneyToText();
            row.Cells[SLExtensions.CELL_INDEX_AMT].Controls[0].Text =
                string.Format("{0}{1}", StateRefund && !string.IsNullOrEmpty(amtText) ? "-" : string.Empty,
                amtText);
        }

        /// <summary>
        /// 상단바 업데이트
        /// </summary>
        /// <param name="itemData"></param>
        void ItemsGrid_UpdateCurrentItemStatus(PBItemData itemData)
        {
            // 상단바에 표시
            currentItemInfo1.NmClass = itemData == null ? string.Empty : itemData.NmClass;
            currentItemInfo1.NmItem = itemData == null ? string.Empty : itemData.NmItem;
            currentItemInfo1.Qty = itemData == null ? 0 : TypeHelper.ToInt32(itemData.Qty);
            currentItemInfo1.Price = itemData == null ? 0 : TypeHelper.ToInt32(itemData.UtSprc);
        }

        /// <summary>
        /// Current row data
        /// </summary>
        public PBItemData ItemsGrid_CurrentItem
        {
            get
            {
                if (gpItems.SelectedRowIndex.InRange(0, gpItems.RowItems.Length))
                {
                    return (PBItemData)gpItems.RowItems[gpItems.SelectedRowIndex];
                }
                else
                {
                    return PBItemData.Empty;
                }
            }
        }

        #endregion

        #region KeyEvent 처리

        /// <summary>
        /// KeyEvent 처리
        /// </summary>
        /// <param name="e"></param>
        void POS_SL_M001_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    m_presenter.ProcessKeyEvent(e);
                });
            }
            else
            {
                m_presenter.ProcessKeyEvent(e);
            }
        }

        /// <summary>
        /// Process key event in View
        /// 특별한인경우만 뷰에서 키이벤트처리한다
        /// </summary>
        /// <param name="e"></param>
        public bool ProcessKeyEvent(OPOSKeyEventArgs e)
        {
            if (InputState == ItemInputState.Processing)
            {
                e.IsHandled = true;
                return false;
            }

            if (e.Key.OPOSKey == OPOSMapKeys.KEY_INQPRC)
            {
                //AutoRtnTksPresentTest();

                // 가격조회
                e.IsHandled = true;
                using (var pop = ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.SL.dll",
                    "WSWD.WmallPos.POS.SL.VC.POS_SL_P002"))
                {
                    pop.ShowDialog(this);
                    Application.DoEvents();
                }
            }
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_HOLD)
            {
                e.IsHandled = true;
                if (!m_holdPresenter.CheckHoldTrxnExists(InputText))
                {
                    m_presenter.ReportInvalidState(InvalidDataInputState.NoHoldTrxn);
                }
            }
            else
            {
                if (!this.ChildManager.ProcessFuncKey(e, true, HasItems))
                {
                    m_presenter.ReportInvalidState(InvalidDataInputState.InvalidKey);
                }
            }

            return m_presenter.OnProcessKeyEventReturn(e);
        }


        #endregion

        #region 보류화면처리

        /// <summary>
        /// 보류리스트 보기
        /// </summary>
        public void ShowHoldList()
        {
            var form = new POS_SL_P003();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                Application.DoEvents();

                var holdItems = form.ReturnResult["RETURN_DATA"];
                if (holdItems == null)
                {
                    return;
                }

                // Bind Hold Items to grid
                BasketItem[] list = ((SAT900TItemData[])holdItems).Select(p => p.Basket).ToArray();
                LoadItems(list);
            }
        }

        /// <summary>
        /// 보류해제
        /// </summary>
        /// <param name="holdItems"></param>
        public void LoadItems(BasketItem[] holdItems)
        {
            m_itemsLoading = true;
            m_presenter.LoadItems(holdItems);
            m_itemsLoading = false;

            // CDP Display
            ShowCDPMessage(CDPMessageType.TransHoldRelease, string.Empty, string.Empty);
        }

        /// <summary>
        /// 보류해제
        /// </summary>
        /// <param name="holdItems"></param>
        public void RestoreFromBaskets(BasketItem[] holdItems)
        {
            foreach (var item in holdItems)
            {
                // 지정취소된상품추가 안함
                if (!string.IsNullOrEmpty(item.FgCanc) && !"0".Equals(item.FgCanc))
                {
                    continue;
                }

                ItemsGrid_AddItemRow(new PBItemData()
                {
                    Barcode = item.SourceCode,
                    CdClass = item.CdClass,
                    NmItem = item.NmItem,
                    FgClass = item.FgMargin,
                    CdItem = item.CdItem,
                    CdDp = item.CdDp,
                    Qty = item.CntItem,
                    FgTax = item.FgTax,
                    FgCanc = item.FgCanc,
                    QtyCanc = item.CntCancel,
                    UtSprc = item.UtSprc,
                    Properties = PBItemProperties.All
                }, false);
            }

            if (gpItems.PageIndex != 0)
            {
                gpItems.PageIndex = 0;
            }
            else
            {
                gpItems.RefreshPageRows();
            }

            gpItems.SelectedRowIndex = 0;
        }

        #endregion

        #region 포인트조회

        /// <summary>
        /// 고객포인트조회
        /// </summary>
        public void ShowCustPointPopup()
        {
            if (SaleMode.ToString().Contains("Other"))
            {
                return;
            }

            using (var pop = ChildManager.ShowPopup(TITLE_POINT_INQ, "WSWD.WmallPos.POS.PT.dll",
                    "WSWD.WmallPos.POS.PT.VC.POS_PT_P001", m_presenter.CustInfo))
            {
                if (pop.ShowDialog(this) == DialogResult.OK)
                {
                    Application.DoEvents();

                    if (!pop.ReturnResult.ContainsKey("Cust"))
                    {
                        return;
                    }

                    if (this.HasItems)
                    {
                        // update customer info
                        m_presenter.CustInfo = (PP01RespData)pop.ReturnResult["Cust"];
                    }
                }
            }
        }

        #endregion

        #region 포인트사용

        /// <summary>
        /// 포인트사용
        /// 수정: TCL,
        /// 이유: 반품처리
        /// </summary>
        /// <param name="payAmt"></param>
        /// <param name="custInfo"></param>
        /// <param name="returnData"></param>
        /// <returns></returns>
        public DialogResult ShowPointUsePopup(int payAmt, PP01RespData custInfo, out object returnData)
        {
            returnData = null;
            using (var pop = ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.PY.dll",
                "WSWD.WmallPos.POS.PY.VC.POS_PY_P009", payAmt, custInfo, StateRefund))
            {
                var res = pop.ShowDialog(this);
                Application.DoEvents();

                returnData = pop.ReturnResult.ContainsKey("PAY_DATA") ? pop.ReturnResult["PAY_DATA"] : null;
                return res;
            }
        }

        /// <summary>
        /// 사은품반납테스트
        /// </summary>
        void AutoRtnTksPresentTest()
        {
            var TksPresentList = new List<PQ11RespData>();
            TksPresentList.Add(new PQ11RespData()
            {
                ExchGiftNo = "121212121",
                PresentAmt = "12000",
                PresentNo = "343434343",
                ReturnAmt = "12000",
                PresentDate = "20150606"

            });

            TksPresentList.Add(new PQ11RespData()
            {
                PresentAmt = "10000",
                PresentNo = "343434343",
                ReturnAmt = "12000",
                PresentDate = "20150606"

            });
            TksPresentList.Add(new PQ11RespData()
            {
                ExchGiftNo = "222222222",
                PresentAmt = "10000",
                PresentNo = "343434343",
                ReturnAmt = "12000",
                PresentDate = "20150606"

            });

            object returnData = null;
            var ret = AutoRtnShowTksPresentReturnPopup(TksPresentList, null, out returnData);
            if (ret != DialogResult.OK)
            {
            }
        }

        #endregion

        #region 카드결제

        /// <summary>
        /// CARD결제/취소 팝업
        /// Loc changed on 10.24, 
        /// 전문 추가정보 팝업으로 보내기
        /// 
        /// 여전볍 변경 05.27
        /// PV11ReqDataAdd > PV21ReqDataAdd 
        /// </summary>
        /// <param name="inputAmt"></param>
        /// <param name="cancellable">이팝업 취소가능한지</param>
        /// <param name="cardPay">원거래카드 BASKET</param>
        /// <param name="returnData"></param>
        /// <returns></returns>
        public DialogResult ShowCardPopup(string inputAmt, string taxAmt, bool cancellable, BasketPayCard cardPay,
            PV21ReqDataAdd addData, PayCardMode payCardMode,
            out object returnData, out string errorCode, out string errorMessage)
        {
            returnData = null;
                        
            //using (var pop = ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.PY.dll",
            //    "WSWD.WmallPos.POS.PY.VC.POS_PY_P001", TypeHelper.ToInt32(inputAmt),
            //    string.IsNullOrEmpty(taxAmt) ?
            //    m_presenter.GetTaxAmt(TypeHelper.ToInt32(inputAmt)) :
            //    TypeHelper.ToInt32(taxAmt),
            //    StateRefund, cancellable,
            //    cardPay, addData))

            // 여전법 변경 0620
            // PayCardMode 추가
            using (var pop = ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.PY.dll",
                "WSWD.WmallPos.POS.PY.VC.POS_PY_P001", TypeHelper.ToInt32(inputAmt),
                string.IsNullOrEmpty(taxAmt) ?
                m_presenter.GetTaxAmt(TypeHelper.ToInt32(inputAmt)) :
                TypeHelper.ToInt32(taxAmt),
                StateRefund, cancellable,
                cardPay, addData, payCardMode))
            {
                var retResult = pop.ShowDialog(this);

                Application.DoEvents();

                returnData = pop.ReturnResult.ContainsKey("PAY_DATA") ? pop.ReturnResult["PAY_DATA"] : null;
                errorCode = pop.ReturnResult.ContainsKey("ERROR_CODE") ? pop.ReturnResult["ERROR_CODE"].ToString() : string.Empty;
                errorMessage = pop.ReturnResult.ContainsKey("ERROR_MSG") ? pop.ReturnResult["ERROR_MSG"].ToString() : string.Empty;

                return retResult;
            }
        }

        #endregion

        #region 현금영수증, 현금결제

        /// <summary>
        /// Loc added, changed 10.24
        /// 전문추가정보
        /// 
        /// 여전볍 변경 05.27
        /// PV11ReqDataAdd > PV21ReqDataAdd 
        /// </summary>
        /// <param name="cashAmt"></param>
        /// <param name="taxAmt"></param>
        /// <param name="addData"></param>
        /// <param name="returnData"></param>
        /// <returns></returns>
        public DialogResult ShowCashReceiptPopup(int cashAmt, int taxAmt, PV21ReqDataAdd addData, out object returnData)
        {
            returnData = null;
            using (var pop = ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.PY.dll",
                this.StateRefund ? "WSWD.WmallPos.POS.PY.VC.POS_PY_P015" :
                "WSWD.WmallPos.POS.PY.VC.POS_PY_P014", cashAmt, taxAmt, addData))
            {
                var res = pop.ShowDialog(this);
                Application.DoEvents();

                returnData = pop.ReturnResult.ContainsKey("PAY_DATA") ? pop.ReturnResult["PAY_DATA"] : null;

                // 여전법 추가 0630
                // 오류코드 확인
                if (addData.IsAutoReturn)
                {
                    // ERROR_CODE
                    if (pop.ReturnResult.ContainsKey("ERROR_CODE"))
                    {
                        returnData = new string[] { 
                            (string)pop.ReturnResult["ERROR_CODE"],
                            (string)pop.ReturnResult["ERROR_MSG"]
                        };
                    }
                }
                return res;
            }
        }

        #endregion

        #region 포인트 적립정보

        /// <summary>
        /// 포인트적립화면
        /// </summary>
        /// <param name="cust">고객정보</param>
        /// <param name="BasketHeader">결제 헤더정보</param>
        /// <param name="BasketPays">결제 결제내역</param>
        /// <param name="BasketSubTtl">결제 소계정보</param>
        /// <param name="dtPromotion">프로모션 정보</param>
        /// <param name="returnData">포인트 적립정보</param>
        /// <returns></returns>
        public DialogResult ShowPointSavePopup(PP01RespData cust, BasketHeader BasketHeader, List<BasketPay> BasketPays, BasketSubTotal BasketSubTtl, BasketPointSave BasketPointSave, Dictionary<string, object> dicPromoPoint, out object returnData)
        {
            returnData = null;
            using (var pop = ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.PT.dll",
                "WSWD.WmallPos.POS.PT.VC.POS_PT_P002", cust, BasketHeader, BasketPays,
                BasketSubTtl, BasketPointSave, dicPromoPoint))
            {
                var res = pop.ShowDialog(this);
                Application.DoEvents();

                returnData = pop.ReturnResult.ContainsKey("POINT_DATA") ? pop.ReturnResult["POINT_DATA"] : null;
                return res;
            }
        }

        #endregion

        #region 현금IC

        /// <summary>
        /// 현금IC 결제화면
        /// </summary>
        /// <param name="cashAmt">결제금액</param>
        /// <param name="taxAmt"></param>
        /// <param name="orgCashIC">원거래현금IC Basket</param>
        /// <param name="allowCancel">닫기버튼활성화여부</param>
        /// <param name="returnData"></param>
        /// <param name="errorCode">오류코드</param>
        /// <param name="errorMessage">오류메시지</param>
        /// <returns>DialogResult.OK:성공</returns>
        public DialogResult ShowCashICPopup(int cashAmt, int taxAmt, BasketCashIC orgCashIC,
            bool allowCancel, out object returnData, out string errorCode, out string errorMessage)
        {
            returnData = null;
            using (var pop = ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.PY.dll",
                "WSWD.WmallPos.POS.PY.VC.POS_PY_P013", cashAmt, taxAmt, StateRefund, orgCashIC, allowCancel))
            {
                var res = pop.ShowDialog(this);
                Application.DoEvents();

                returnData = pop.ReturnResult.ContainsKey("PAY_DATA") ? pop.ReturnResult["PAY_DATA"] : null;
                errorCode = pop.ReturnResult.ContainsKey("ERROR_CODE") ? pop.ReturnResult["ERROR_CODE"].ToString() : null;
                errorMessage = pop.ReturnResult.ContainsKey("ERROR_MSG") ? pop.ReturnResult["ERROR_MSG"].ToString() : null;
                return res;
            }
        }

        #endregion

        #region 기타결제수단

        /// <summary>
        /// 기타결제수단
        /// </summary>
        /// <param name="payAmt"></param>
        /// <param name="taxAmt"></param>
        /// <param name="returnData"></param>
        /// <returns></returns>
        public DialogResult ShowOtherPayMethod(int payAmt, int taxAmt, out object returnData)
        {
            returnData = null;
            using (var pop = ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.PY.dll",
                "WSWD.WmallPos.POS.PY.VC.POS_PY_P018"))
            {
                var res = pop.ShowDialog(this);
                Application.DoEvents();

                if (res == DialogResult.OK)
                {
                    string className = pop.ReturnResult.ContainsKey("SELECT_CLASS") ?
                        pop.ReturnResult["SELECT_CLASS"].ToString() : string.Empty;

                    if (!string.IsNullOrEmpty(className))
                    {
                        className = "WSWD.WmallPos.POS.PY.VC." + className;
                        string dllName = "WSWD.WmallPos.POS.PY.dll";

                        var payPop = ChildManager.ShowPopup(string.Empty, dllName, className, payAmt, taxAmt, StateRefund);
                        res = payPop.ShowDialog(this);
                        Application.DoEvents();

                        if (res == DialogResult.OK)
                        {
                            returnData = payPop.ReturnResult.ContainsKey("PAY_DATA") ?
                                payPop.ReturnResult["PAY_DATA"] : null;
                        }
                    }
                    else
                    {
                        res = DialogResult.Cancel;
                    }
                }

                return res;
            }
        }

        #endregion

        #region 상품교환권 팝업

        public DialogResult ShowExchangePopup(int payAmt, List<BasketPay> BasketPays, int iTranOverCnt, out object returnData)
        {
            returnData = null;
            using (var pop = ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.PY.dll",
                "WSWD.WmallPos.POS.PY.VC.POS_PY_P006", payAmt, BasketPays, iTranOverCnt))
            {
                var res = pop.ShowDialog(this);
                Application.DoEvents();

                returnData = pop.ReturnResult.ContainsKey("PAY_DATA") ? pop.ReturnResult["PAY_DATA"] : null;
                return res;
            }
        }

        #endregion

        #region 타사상품권 팝업

        public DialogResult ShowOtherTicketPopup(int payAmt, List<BasketPay> BasketPays,
            int iTranOverCnt, bool bAuto, out object returnData)
        {
            returnData = null;
            using (var pop = ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.PY.dll",
                "WSWD.WmallPos.POS.PY.VC.POS_PY_P007", payAmt, BasketPays, iTranOverCnt, StateRefund, bAuto))
            {
                var res = pop.ShowDialog(this);
                Application.DoEvents();

                returnData = pop.ReturnResult.ContainsKey("PAY_DATA") ? pop.ReturnResult["PAY_DATA"] : null;
                return res;
            }
        }

        #endregion

        #region 할인쿠폰 팝업

        /// <summary>
        /// 할인쿠폰
        /// TCL수정 
        /// 06.01: 반품처리, StateRefund 파람추가
        /// </summary>
        /// <param name="payAmt"></param>
        /// <param name="bType"></param>
        /// <param name="returnData"></param>
        /// <returns></returns>
        public DialogResult ShowCouponPopup(int payAmt, List<BasketItem> BasketItems, List<BasketPay> BasketPays, bool bType, out object returnData)
        {
            returnData = null;
            using (var pop = ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.PY.dll",
                "WSWD.WmallPos.POS.PY.VC.POS_PY_P010", payAmt, BasketItems, BasketPays, bType, StateRefund))
            {
                var res = pop.ShowDialog(this);
                Application.DoEvents();

                returnData = pop.ReturnResult.ContainsKey("PAY_DATA") ? pop.ReturnResult["PAY_DATA"] : null;
                return res;
            }
        }

        #endregion

        #region 관리자확인
        /// <summary>
        /// 관리자인지확인
        /// </summary>
        /// <returns></returns>
        public string ValidateAdmin()
        {
            using (var pop = ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.SO.dll",
                "WSWD.WmallPos.POS.SO.VC.POS_SO_P001", "01,03,04"))
            {
                var res = pop.ShowDialog(this);
                Application.DoEvents();
                return res == DialogResult.Cancel ? string.Empty : pop.ReturnResult["CASNO"].ToString();
            }
        }

        #endregion

        #region 자동반품

        /// <summary>
        /// 반품취소
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void autoRtnPanel1_Cancelled(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    m_presenter.ChangeSaleMode(SaleModes.Sale, false, false, true);
                });
            }
            else
            {
                m_presenter.ChangeSaleMode(SaleModes.Sale, false, false, true);
            }
        }

        /// <summary>
        /// 반품확정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void autoRtnPanel1_Confirmed(object sender, EventArgs e)
        {
            m_presenter.AutoRtnConfirmStart();
        }

        /// <summary>
        /// 전자서명 받기
        /// </summary>
        /// <returns></returns>
        public string ShowCardSignPopup(int payAmt)
        {
            using (var pop = ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.PY.dll",
                "WSWD.WmallPos.POS.PY.VC.POS_PY_P002", payAmt, false))
            {
                var res = pop.ShowDialog(this);
                Application.DoEvents();

                if (res == DialogResult.OK)
                {
                    // 카드결제정보 받기 완료
                    return pop.ReturnResult["SIGN_DATA"].ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// 은련카드 비밀번호팝업
        /// 
        /// 여전법 변경 0622
        /// 사용안함
        /// 은련카드일 경우 신용카드 팝업 동일
        /// </summary>
        /// <param name="workKeyIndex"></param>
        /// <param name="cardPin"></param>
        public void ShowERCardPasswordPopup(string cardNo, out string workKeyIndex, out string cardPin)
        {
            workKeyIndex = string.Empty;
            cardPin = string.Empty;

            using (var pop = ChildManager.ShowPopup(string.Empty,
                "WSWD.WmallPos.POS.PY.dll", "WSWD.WmallPos.POS.PY.VC.POS_PY_P003",
                true, cardNo, 6, false))
            {
                var res = pop.ShowDialog(this);
                Application.DoEvents();
                if (res == DialogResult.OK)
                {
                    cardPin = pop.ReturnResult["PASSWORD"].ToString();
                    workKeyIndex = pop.ReturnResult["WORK_INDEX"].ToString();
                }
            }
        }

        /// <summary>
        /// 자동반품 - 사은품회수팝업
        /// </summary>
        /// <param name="presentList"></param>
        /// <param name="basketHeader"></param>
        /// <param name="returnData">리턴결과</param>
        /// <returns></returns>
        public DialogResult AutoRtnShowTksPresentReturnPopup(List<PQ11RespData> presentList, BasketHeader basketHeader,
            out object returnData)
        {
            using (var pop = ChildManager.ShowPopup(string.Empty,
                "WSWD.WmallPos.POS.SL.dll", "WSWD.WmallPos.POS.SL.VC.POS_SL_P005",
                presentList, basketHeader))
            {
                returnData = null;
                var res = pop.ShowDialog(this);
                Application.DoEvents();

                if (res == DialogResult.Yes)
                {
                    returnData = pop.ReturnResult.ContainsKey("PRESENT_LIST") ?
                        pop.ReturnResult["PRESENT_LIST"] : null;
                }

                return res;
            }
        }

        /// <summary>
        /// 강제취소, 강제진행 한 basket을 요약정보 보여준다
        /// </summary>
        /// <param name="header">원거래 HEADER BASKET</param>
        /// <param name="baskets">강제처리 된 결제BASKET</param>
        public void AutoRtnShowForceCancelBaskets(BasketHeader header, List<BasketBase> baskets)
        {
            using (var pop = ChildManager.ShowPopup(string.Empty,
                "WSWD.WmallPos.POS.SL.dll", "WSWD.WmallPos.POS.SL.VC.POS_SL_P006", header, baskets))
            {
                pop.ShowDialog(this);
                Application.DoEvents();
            }
        }

        #endregion

        #region 상품교환권 - 반품

        public DialogResult ShowExchangeRtnPopup(int payAmt, out object returnData)
        {
            returnData = null;
            using (var pop = ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.PY.dll",
                "WSWD.WmallPos.POS.PY.VC.POS_PY_P019", payAmt))
            {
                var res = pop.ShowDialog(this);
                Application.DoEvents();
                returnData = pop.ReturnResult.ContainsKey("PAY_DATA") ? pop.ReturnResult["PAY_DATA"] : null;
                return res;
            }
        }

        #endregion
    }
}
