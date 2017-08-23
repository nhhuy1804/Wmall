//-----------------------------------------------------------------
/*
 * 화면명   : POS_SL_P007.cs
 * 화면설명 : 반납 상품교환권 등록
 * 개발자   : TCL
 * 개발일자 : 2015.11.03
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.POS.SL.Data;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.FX.NetComm.Tasks.PG;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PG;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PG;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using System.Diagnostics;

namespace WSWD.WmallPos.POS.SL.VC
{
    /// <summary>
    /// TODO
    /// 1) bind list
    /// - maximum 10rows
    /// - fill with existing data;
    /// 2) set total
    /// 3) provide list of enterred gift list; (check existings)
    /// 4) key event
    /// - form prev, next;
    /// - clear key for GiftNo
    ///     - clear all text, enable 매수, 권종금액
    /// - ENTER key form GitNo
    ///     - 전문
    /// - Scanner Event
    ///     - Focus control check;
    ///     - Process like ENTER key
    /// - Amount, Qty check number,>10won (key event for 2 inputs)
    /// - Validate on Save
    ///     - Total (Count * Amt) <= Return Amt
    /// - Check using GiftNo khac.. (echange gift no kahc).
    /// - Make to call to this UI; check logic amount, change inputtext to button (P005 grid)
    /// </summary>
    public partial class POS_SL_P007 : PopupBase01
    {
        #region 변수

        private Dictionary<string, List<RtnPrsGiftData>> m_retGiftList;
        private string m_exchGiftNo;
        /// <summary>
        /// 회수금액
        /// </summary>
        private Int64 m_retAmt;
        /// <summary>
        /// 현재입력중인 교환권번호
        /// </summary>
        private string m_inputGiftNo = string.Empty;
        private bool m_scannedGiftNo = false;

        #endregion

        #region 생성자

        /// <summary>
        /// 
        /// </summary>
        /// <param name="giftList"></param>
        /// <param name="exchGiftNo">상품교환권</param>
        public POS_SL_P007(Dictionary<string, List<RtnPrsGiftData>> giftList, string exchGiftNo, Int64 retAmt)
        {
            InitializeComponent();

            m_retGiftList = giftList;
            if (giftList == null)
            {
                m_retGiftList = new Dictionary<string, List<RtnPrsGiftData>>();
            }

            m_exchGiftNo = exchGiftNo;
            m_retAmt = retAmt;

            FormInitialize();
        }
        #endregion

        #region 초기화

        void FormInitialize()
        {
            #region Grid Columns

            gpPQ11.UnSelectable = true;
            gpPQ11.ColumnCount = 5;
            gpPQ11.SetColumn(0, "NO", 70); // center
            gpPQ11.SetColumn(1, GP11_COL1, 140); // 교환권번호
            gpPQ11.SetColumn(2, GP11_COL2, 70); // 매수
            gpPQ11.SetColumn(3, GP11_COL3, 95); // 권종금액
            gpPQ11.SetColumn(4, GP11_COL4); // 용도

            gpPQ11.UnSelectable = true;
            gpPQ11.AutoFillRows = true;
            gpPQ11.ShowPageNo = true;
            gpPQ11.ScrollType = ScrollTypes.PageChanged;
            gpPQ11.PageIndex = -1;

            #endregion


            ((System.Windows.Forms.TableLayoutPanel)(gpPQ11.Controls[2])).RowStyles[1] = new RowStyle(SizeType.Absolute, 60);
            ((System.Windows.Forms.Label)(gpPQ11.Controls[2].Controls[1])).AutoSize = false;
            ((System.Windows.Forms.Label)(gpPQ11.Controls[2].Controls[1])).Font = new Font("돋움체", 9, FontStyle.Bold);

            InitEvents();

            this.StatusMessage = MSG_INPUT;
            this.txtRetAmt.Text = m_retAmt.ToString();
            if (m_retGiftList.ContainsKey(m_exchGiftNo))
            {
                var list = m_retGiftList[m_exchGiftNo];
                var totalGiftAmt = list.Sum(P => P.GiftCount * P.GiftAmt);
                this.txtTotalGiftAmt.Text = totalGiftAmt.ToString();
            }

            #region Scanner Event
            POSDeviceManager.Scanner.DataEvent += new POSDataEventHandler(Scanner_DataEvent);
            #endregion
        }


        /// <summary>
        /// 
        /// </summary>
        void InitEvents()
        {
            btnClose.Click += new EventHandler(btnClose_Click);
            btnSave.Click += new EventHandler(btnSave_Click);

            this.Load += new EventHandler(POS_SL_P007_Load);
            this.KeyEvent += new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(POS_SL_P007_KeyEvent);

            this.gpPQ11.InitializeCell +=
                new WSWD.WmallPos.POS.FX.Win.UserControls.CellDataBoundEventHandler(gpPQ11_InitializeCell);
            this.gpPQ11.RowDataBound += new WSWD.WmallPos.POS.FX.Win.UserControls.RowDataBoundEventHandler(gpPQ11_RowDataBound);

            this.FormClosed += new FormClosedEventHandler(POS_SL_P007_FormClosed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void POS_SL_P007_FormClosed(object sender, FormClosedEventArgs e)
        {
            btnClose.Click -= new EventHandler(btnClose_Click);
            btnSave.Click -= new EventHandler(btnSave_Click);

            this.Load -= new EventHandler(POS_SL_P007_Load);
            this.KeyEvent -= new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(POS_SL_P007_KeyEvent);

            this.gpPQ11.InitializeCell -=
                new WSWD.WmallPos.POS.FX.Win.UserControls.CellDataBoundEventHandler(gpPQ11_InitializeCell);
            this.gpPQ11.RowDataBound -= new WSWD.WmallPos.POS.FX.Win.UserControls.RowDataBoundEventHandler(gpPQ11_RowDataBound);

            this.FormClosed -= new FormClosedEventHandler(POS_SL_P007_FormClosed);
            POSDeviceManager.Scanner.DataEvent -= new POSDataEventHandler(Scanner_DataEvent);        
        }

        #endregion

        #region 이벤트정의

        void gpPQ11_InitializeCell(WSWD.WmallPos.POS.FX.Win.UserControls.CellDataBoundEventArgs e)
        {
            PQ11Grid_BuildCells(e);
        }

        void gpPQ11_RowDataBound(WSWD.WmallPos.POS.FX.Win.UserControls.RowDataBoundEventArgs e)
        {
            RtnPrsGiftData data = (RtnPrsGiftData)e.ItemData;
            e.Row.Cells[0].Controls[0].Text = (e.Row.RowIndex + 1).ToString("d2");
            e.Row.Cells[1].Controls[0].Text = data != null ? data.GiftNo : string.Empty;
            e.Row.Cells[1].Controls[0].Tag = true; // no changes
            e.Row.Cells[2].Controls[0].Text = data == null ? string.Empty : string.Format("{0:#,###}", TypeHelper.ToInt32(data.GiftCount));
            e.Row.Cells[3].Controls[0].Text = data == null ? string.Empty : string.Format("{0:#,###}", TypeHelper.ToInt32(data.GiftAmt));
            e.Row.Cells[4].Controls[0].Text = data == null ? string.Empty : data.GiftUsage;
        }

        void POS_SL_P007_Load(object sender, EventArgs e)
        {
            // Load gift list;
            BindGiftList();
            FocusNextRow();
        }

        /// <summary>
        /// Process ENTER, CLEAR: next previous (clear)
        /// ENTER KEy for GiftNo; process PG01
        /// ENTER key for Amt: proces Korean won amount
        /// CLEAR key for GiftNo: Reset input current row
        /// 
        /// </summary>
        /// <param name="e"></param>
        void POS_SL_P007_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (this.FocusedControl != null)
            {
                InputText it = (InputText)this.FocusedControl;
                SaleGridCell cell = (SaleGridCell)it.Parent;

                // 교환권번호컬럼
                if (cell.ColumnIndex == 1)
                {
                    // GiftNo, clear, reset input, invalid
                    if (it.Text.Length > 0 && e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_BKS)
                    {
                        //2015.08.26 정광호 수정
                        ResetInputs(true);
                        //ResetInputs();
                        return;
                    }

                    if (it.Text.Length > 0 && e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_CLEAR)
                    {
                        it.Text = string.Empty;
                        e.IsHandled = true;
                        //2015.08.26 정광호 수정
                        ResetInputs(true);
                        //ResetInputs();
                        return;
                    }

                    if (it.Text.Length > 0 && e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_ENTER)
                    {
                        ValidateGiftNo(false, it.Text);
                        e.IsHandled = true;
                        return;
                    }

                    if (!e.IsControlKey)
                    {
                        StatusMessage = MSG_INPUT;
                    }
                }

                if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_CLEAR)
                {
                    SaleGridRow row = (SaleGridRow)cell.Parent;
                    var itNo = (InputText)row.Cells[1].Controls[0];

                    if (it.Text.Length > 0 && !it.ReadOnly && itNo.Text.Length == 0)
                    {
                        e.IsHandled = true;
                        it.Text = string.Empty;
                        UpdateTotalGiftAmt();
                        return;
                    }

                    e.IsHandled = true;
                    this.PreviousControl();
                }
                else if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_ENTER)
                {
                    // validate number enterred
                    if (cell.ColumnIndex == 3 && !it.ReadOnly)
                    {
                        var val = TypeHelper.ToInt64(it.Text.Replace(",", ""));
                        val = ValidateMoney(val);
                        it.Text = val.ToString();
                    }

                    // update total git amount
                    UpdateTotalGiftAmt();
                    e.IsHandled = true;
                    this.NextControl();
                }
            }
        }

        /// <summary>
        /// TODO
        /// - Read and check from server
        /// </summary>
        /// <param name="eventData"></param>
        void Scanner_DataEvent(string eventData)
        {
            Trace.WriteLine("SL_P007_Scanner_DataEvent " + eventData, "program");

            ValidateGiftNo(true, eventData);
        }

        /// <summary>
        /// 저장한다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            ValidateAndSave();
        }

        /// <summary>
        /// 취소
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region 사용자정의
        private void BindGiftList()
        {
            List<RtnPrsGiftData> list = null;
            if (m_retGiftList.ContainsKey(m_exchGiftNo))
            {
                list = m_retGiftList[m_exchGiftNo];
            }

            for (int i = 0; i < 10; i++)
            {
                RtnPrsGiftData item = null;
                if (list != null && i < list.Count)
                {
                    item = list[i];
                }

                gpPQ11.AddRow(item);
            }

            gpPQ11.PageIndex = 0;
        }

        /// <summary>
        /// Make grid with 4 cells/row
        /// </summary>
        /// <param name="e"></param>
        private void PQ11Grid_BuildCells(CellDataBoundEventArgs e)
        {
            Label lbl = null;
            InputText input = null;
            switch (e.Cell.ColumnIndex)
            {
                case 0:
                case 4:
                    // label for indexing
                    lbl = new Label()
                    {
                        AutoSize = false,
                        Left = 1,
                        Top = 1,
                        Width = e.Cell.Width - 2,
                        Height = e.Cell.Height - 2,
                        TextAlign = e.Cell.ColumnIndex == 0 ? ContentAlignment.MiddleCenter : ContentAlignment.MiddleLeft,
                        //BackColor = Color.Transparent
                    };
                    e.Cell.Controls.Add(lbl);
                    break;
                default:
                    input = new InputText()
                    {
                        DataType = e.Cell.ColumnIndex == 1 ? InputTextDataType.Text : InputTextDataType.Numeric,
                        Format = e.Cell.ColumnIndex == 1 ? string.Empty : "#,###",
                        TextAlign = e.Cell.ColumnIndex == 1 ? ContentAlignment.MiddleCenter : ContentAlignment.MiddleRight,
                        ReadOnly = false,
                        HasBorder = true,
                        Left = 1,
                        Top = 1,
                        Width = e.Cell.Width - 2,
                        Height = e.Cell.Height - 2,
                        BorderWidth = 1,
                        Corner = 1,
                        MaxLength = e.Cell.ColumnIndex == 1 ? 13 : 9,
                        Padding = new Padding(0, 0, 2, 0),
                        BorderColor = System.Drawing.Color.FromArgb(187, 187, 187),
                        Focusable = true,
                        //BackColor = Color.Transparent
                    };

                    if (e.Cell.ColumnIndex == 2 ||
                        e.Cell.ColumnIndex == 3)
                    {
                        input.Format = "#,##0";
                    }

                    e.Cell.Controls.Add(input);

                    break;
            }
        }

        /// <summary>
        /// For input 구상품교환권
        /// </summary>
        /// <param name="bMsg"></param>
        /// 2015.08.26 정광호 수정 bMsg 구분자 추가하여 메시지관련 변경 확인
        private void ResetInputs(bool bMsg)
        {
            // reset flag
            m_scannedGiftNo = false;

            //2015.08.26 정광호 수정--------------------
            //******************************************
            if (bMsg)
            {
                // Guide Message
                StatusMessage = MSG_INPUT;
            }
            //StatusMessage = MSG_INPUT;
            //******************************************
            //2015.08.26 정광호 수정--------------------

            if (this.FocusedControl != null)
            {
                InputText itNo = (InputText)this.FocusedControl;
                itNo.Text = string.Empty;
                itNo.Tag = null;

                SaleGridRow row = (SaleGridRow)itNo.Parent.Parent;
                var itCnt = (InputText)row.Cells[2].Controls[0];
                var itAmt = (InputText)row.Cells[3].Controls[0];
                var lblEtc = (Label)row.Cells[4].Controls[0];

                itCnt.Text = itAmt.Text = lblEtc.Text = string.Empty;
                itCnt.ReadOnly = itAmt.ReadOnly = false;


                //2015.08.26 정광호 수정--------------------
                //클리어시 itemdata null로 처리안해주면 상품권 스캐닝시에 하위에 입력됨
                row.ItemData = null;
            }

            // Update total
            UpdateTotalGiftAmt();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="giftNo"></param>
        /// <returns></returns>
        private bool ValidateInputGiftNo(bool scanned, string giftNo)
        {
            // check for existings
            int count = 0;
            Int64 totalGiftAmt = 0;
            // check in grid first
            foreach (SaleGridRow row in gpPQ11.Rows)
            {
                string gn = row.Cells[1].Controls[0].Text;
                if (giftNo.Equals(gn))
                {
                    count++;
                }
                string gc = row.Cells[2].Controls[0].Text.Replace(",", "");
                string ga = row.Cells[3].Controls[0].Text.Replace(",", "");
                totalGiftAmt += TypeHelper.ToInt64(gc) * TypeHelper.ToInt64(ga);
            }

            if (scanned)
            {
                if (count != 0)
                {
                    StatusMessage = MSG_EXIST_GIFT_EXCHNO;
                    //ResetInputs();
                    return false;
                }
            }
            else
            {
                if (count > 1)
                {
                    StatusMessage = MSG_EXIST_GIFT_EXCHNO;
                    //2015.08.26 정광호 수정
                    ResetInputs(false);
                    //ResetInputs();
                    return false;
                }
            }

            // validate other GiftNno
            foreach (var key in m_retGiftList.Keys)
            {
                if (key.Equals(m_exchGiftNo))
                {
                    continue;
                }

                var giftList = m_retGiftList[key];
                if (giftList == null)
                {
                    continue;
                }

                foreach (var data in giftList)
                {
                    if (data == null || string.IsNullOrEmpty(data.GiftNo))
                    {
                        continue;
                    }

                    if (data.GiftNo.Equals(giftNo))
                    {
                        StatusMessage = MSG_EXIST_GIFT_EXCHNO;
                        //2015.08.26 정광호 수정
                        ResetInputs(false);
                        //ResetInputs();
                        return false;
                    }
                }
            }

            //2015.08.26 정광호 수정----------------------------------------------------
            //**************************************************************************
            // 정합성 검사는 적용버튼을 누를때 확인
            //if (totalGiftAmt >= TypeHelper.ToInt64(txtRetAmt.Text.Replace(",", "")))
            //{
            //    StatusMessage = MSG_OVER_TOTAL_RET_AMT;
            //    ResetInputs(false);
            //    return false;
            //}
            //**************************************************************************
            //2015.08.26 정광호 수정----------------------------------------------------

            return true;
        }

        /// <summary>
        /// 합계금액확인 & 저장
        /// </summary>
        /// <returns></returns>
        private void ValidateAndSave()
        {
            // make result
            var list = new List<RtnPrsGiftData>();

            Int64 totalGiftAmt = 0;
            foreach (SaleGridRow row in gpPQ11.Rows)
            {
                string gc = row.Cells[2].Controls[0].Text.Replace(",", "");
                string ga = row.Cells[3].Controls[0].Text.Replace(",", "");
                totalGiftAmt += TypeHelper.ToInt64(gc) * TypeHelper.ToInt64(ga);

                if (TypeHelper.ToInt64(gc) == 0 ||
                    TypeHelper.ToInt64(ga) == 0)
                {
                    continue;
                }

                string gn = row.Cells[1].Controls[0].Text;
                list.Add(new RtnPrsGiftData()
                {
                    GiftNo = gn,
                    GiftCount = TypeHelper.ToInt32(gc),
                    GiftAmt = TypeHelper.ToInt64(ga),
                    GiftUsage = row.Cells[4].Controls[0].Text
                });
            }

            if (totalGiftAmt > TypeHelper.ToInt64(txtRetAmt.Text.Replace(",", "")))
            {
                StatusMessage = MSG_OVER_TOTAL_RET_AMT;
                return;
            }

            this.ReturnResult.Add("GIFT_LIST", list);
            this.ReturnResult.Add("GIFT_TOTAL", totalGiftAmt);
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 전문에서 확인성공
        /// </summary>
        /// <param name="giftAmt"></param>
        /// <param name="giftOtherData"></param>
        private void ConfirmNewGiftData(string giftAmt, string giftOtherData, string strGiftNo)
        {
            InputText itNo = null;
            InputText itCnt = null;
            InputText itAmt = null;
            Label lblEtc = null;
            SaleGridRow curRow = null;

            if (m_scannedGiftNo)
            {
                //  get next empty
                foreach (SaleGridRow row in gpPQ11.Rows)
                {
                    //2015.08.27 정광호 수정---------------------------------------
                    //*************************************************************
                    //상품권번호없이 직접 입력한 row에 스캐닝 값이 들어가는것을 방지

                    //if (row.ItemData == null)
                    //{
                    //    itNo = (InputText)row.Cells[1].Controls[0];
                    //    itCnt = (InputText)row.Cells[2].Controls[0];
                    //    itAmt = (InputText)row.Cells[3].Controls[0];
                    //    lblEtc = (Label)row.Cells[4].Controls[0];

                    //    curRow = row;
                    //    break;
                    //}

                    if (row.Cells[2].Controls[0].Text.Length <= 0 && row.Cells[3].Controls[0].Text.Length <= 0)
                    {
                        itNo = (InputText)row.Cells[1].Controls[0];
                        itCnt = (InputText)row.Cells[2].Controls[0];
                        itAmt = (InputText)row.Cells[3].Controls[0];
                        lblEtc = (Label)row.Cells[4].Controls[0];

                        curRow = row;
                        break;
                    }
                    //*************************************************************
                    //2015.08.27 정광호 수정---------------------------------------
                }
            }
            else
            {
                //2015.08.26 정광호 수정---------------------------------------
                //*************************************************************
                itNo = (InputText)this.FocusedControl;
                curRow = (SaleGridRow)itNo.Parent.Parent;
                itCnt = (InputText)curRow.Cells[2].Controls[0];
                itAmt = (InputText)curRow.Cells[3].Controls[0];
                lblEtc = (Label)curRow.Cells[4].Controls[0];
            }

            if (itCnt != null)
            {
                //2015.08.26 정광호 수정---------------------------------------
                //*************************************************************
                itCnt.ReadOnly = itAmt.ReadOnly = true;
                itNo.Text = m_inputGiftNo;
                itCnt.Text = "1";
                itAmt.Text = giftAmt;
                lblEtc.Text = giftOtherData;
                curRow.ItemData = new RtnPrsGiftData()
                {
                    GiftAmt = TypeHelper.ToInt64(giftAmt),
                    GiftCount = 1,
                    GiftNo = m_inputGiftNo,
                    GiftUsage = giftOtherData
                };

                // focus to next row
                if (curRow.RowIndex < gpPQ11.Rows.Length - 1)
                {
                    InputText it = (InputText)gpPQ11.Rows[curRow.RowIndex + 1].Cells[1].Controls[0];
                    it.SetFocus();
                }

                //*************************************************************
                //2015.08.26 정광호 수정---------------------------------------
            }

            // update total
            UpdateTotalGiftAmt();

            m_inputGiftNo = string.Empty;
            m_scannedGiftNo = false;
        }

        /// <summary>
        /// Focus to next inputing row
        /// </summary>
        private void FocusNextRow()
        {
            InputText itNo = null;
            //  get next empty
            foreach (SaleGridRow row in gpPQ11.Rows)
            {
                if (row.ItemData == null)
                {
                    itNo = (InputText)row.Cells[1].Controls[0];
                    break;
                }
            }

            if (itNo != null)
            {
                itNo.SetFocus();
            }
        }

        /// <summary>
        /// Update all grid data to total
        /// </summary>
        private void UpdateTotalGiftAmt()
        {
            Int64 totalGiftAmt = 0;
            foreach (SaleGridRow row in gpPQ11.Rows)
            {
                string gc = row.Cells[2].Controls[0].Text.Replace(",", "");
                string ga = row.Cells[3].Controls[0].Text.Replace(",", "");
                totalGiftAmt += TypeHelper.ToInt64(gc) * TypeHelper.ToInt64(ga);
            }

            this.txtTotalGiftAmt.Text = totalGiftAmt.ToString();
        }

        #region PG01전문 교환권확인
        /// <summary>
        /// 
        /// </summary>
        /// <param name="giftNo"></param>
        private void ValidateGiftNo(bool scanned, string giftNo)
        {
            // reset status message
            StatusMessage = MSG_INPUT;

            m_scannedGiftNo = scanned;
            if (!ValidateInputGiftNo(scanned, giftNo))
            {
                return;
            }

            m_inputGiftNo = giftNo;
            var task = new PG01DataTask(new PG01ReqData()
            {
                GiftChangeNo = giftNo
            });

            task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(task_Errored);
            task.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(task_TaskCompleted);
            ChildManager.ShowProgress(true);
            task.ExecuteTask();
        }

        /// <summary>
        /// 상품교환권확인결과
        /// </summary>
        /// <param name="responseData"></param>
        void task_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            ChildManager.ShowProgress(false);
            if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.Success)
            {
                // confirm input giftno;
                var resp = responseData.DataRecords.ToDataRecords<PG01RespData>()[0];
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        ConfirmNewGiftData(resp.PayAmt, resp.EtcData, m_inputGiftNo);
                    });
                }
                else
                {
                    ConfirmNewGiftData(resp.PayAmt, resp.EtcData, m_inputGiftNo);
                }
            }
            else
            {
                this.StatusMessage = responseData.Response.ErrorMessage;
                if (!m_scannedGiftNo)
                {
                    //2015.08.26 정광호 수정
                    ResetInputs(false);
                    //ResetInputs();
                }
            }
        }

        /// <summary>
        /// 전문확인 오류
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="lastException"></param>
        void task_Errored(string errorMessage, Exception lastException)
        {
            ChildManager.ShowProgress(false);
            this.BeginInvoke((MethodInvoker)delegate()
            {
                this.StatusMessage = errorMessage;
                if (!m_scannedGiftNo)
                {
                    //2015.08.26 정광호 수정
                    ResetInputs(false);
                    //ResetInputs();
                }
            });
        }
        #endregion

        #endregion

        #region 기타
        Int64 ValidateMoney(Int64 intValue)
        {
            return (Int64)Math.Ceiling((double)(intValue / 10) * 10);
        }

        #endregion
    }
}
