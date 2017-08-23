//-----------------------------------------------------------------
/*
 * 화면명   : POS_SL_P005.cs
 * 화면설명 : 사은품 회수
 * 개발자   : TCL
 * 개발일자 : 2015.08.03
 * 
 * 11.02:
 *      1) 한줄만 확인 돼서 (validation) 확정 가능하게 수정
 *      2) 미회수 사유 디자인 변경 (위 디자인)
 *      3) 교환권 입력 팝업에서 중복 확인 문제 해결
 *      
 * 11.11
 *      1) 미사유 리스트: 공통코드에서 가져온다
 *      
 *
 * 11.13
 *      1) 등록버튼 => 확정
 *      2) 반납취소 버튼 추가
 *      3) 로직 변경
 * 
 *  (3-1) [확정] 버튼 	
       ① 입력된 회수 정보 저장 (임시 저장한후 실제 반품 확정 작업시 TRAN 반영)	
       ② 만약 미회수 사유가 입력된 경우라면 관리자 ID/PASSWORD PopUp 창을 띄어 관리자가 확인 할수 있도록 한다.	
       ③ 입력된 금액 확인 작업	
        - 입력된 금액은 회수금액 보다 작거나 같아야된다.	
        - 현물금액과 상품권금액은 같이 입력 할수 없다	
    (3-2) [반납취소] 버튼	
       ① 반품 작업을 진행하지 않고 화면을 종료 한다.	
       ② 2-3) 화면으로 복귀 하고 반품 확정 Step은 더 이상 진행하지 않고 종료 한다.	
    (3-3) [닫기] 버튼	
       ① 회수 처리 등록 하지 않고 자동 반품 처리 한다.	
       ② 관리자 ID/PASSWORD PopUp 창을 띄어 관리자가 확인 할수 있도록 한다.	
 * 
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PQ;

using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.NetComm.Tasks.PQ;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.POS.FX.Win.Data;
using WSWD.WmallPos.POS.FX.Win;
using WSWD.WmallPos.FX.Shared.DB;

namespace WSWD.WmallPos.POS.SL.VC
{
    /// <summary>
    /// 사은품회수
    /// 
    /// TODO
    /// 1) Change 상품교환권번호 컬럼 ==> button with text;
    /// 2) How to clear amount for 교환권번호?
    /// 3) Update amount from P007 to button text and data item
    /// 
    /// SLCaller:
    /// - If Gift is ok, amount is ok, and no 미회수, 교환권번호리스트 적용한다
    /// 
    /// 
    /// </summary>
    partial class POS_SL_P005
    {
        #region 변수, 속성

        private List<PQ11RespData> m_prsnList = null;
        private BasketHeader m_bskHeader = null;

        /// <summary>
        /// 반납교환훤리스트
        /// </summary>
        private Dictionary<string, List<RtnPrsGiftData>> m_rtnPrsGiftList = null;

        /// <summary>
        /// 미회수 사유 리스트
        /// </summary>
        private Dictionary<string, string> m_nonRtnReasons = null;

        #endregion

        #region 초기화

        void FormInitialize()
        {
            #region Grid Columns

            gpPQ11.UnSelectable = true;
            gpPQ11.ColumnCount = 9;
            gpPQ11.SetColumn(0, GP11_COL1, 140); // center
            gpPQ11.SetColumn(1, GP11_COL2); // left
            gpPQ11.SetColumn(2, GP11_COL3, 98); // right
            gpPQ11.SetColumn(3, GP11_COL4, 130); // center, 상품교환권
            gpPQ11.SetColumn(4, GP11_COL5, 98); // right
            gpPQ11.SetColumn(5, GP11_COL6, 98); // right
            gpPQ11.SetColumn(6, GP11_COL7, 98); // right
            gpPQ11.SetColumn(7, GP11_COL8, 98); // right
            gpPQ11.SetColumn(8, GP11_COL9, 50);// center

            gpPQ11.UnSelectable = true;
            gpPQ11.AutoFillRows = true;
            gpPQ11.ShowPageNo = true;
            gpPQ11.ScrollType = ScrollTypes.PageChanged;
            gpPQ11.PageIndex = -1;

            ((System.Windows.Forms.TableLayoutPanel)(gpPQ11.Controls[2])).RowStyles[1] = new RowStyle(SizeType.Absolute, 60);
            ((System.Windows.Forms.Label)(gpPQ11.Controls[2].Controls[1])).AutoSize = false;
            ((System.Windows.Forms.Label)(gpPQ11.Controls[2].Controls[1])).Font = new Font("돋움체", 9, FontStyle.Bold);

            gpPQ12.ColumnCount = 8;
            gpPQ12.SetColumn(0, GP12_COL1, 130); // center
            gpPQ12.SetColumn(1, GP12_COL2); // left
            gpPQ12.SetColumn(2, GP12_COL3, 120); // center
            gpPQ12.SetColumn(3, GP12_COL4, 90); // center
            gpPQ12.SetColumn(4, GP12_COL5, 110); // center
            gpPQ12.SetColumn(5, GP12_COL6, 70); // center
            gpPQ12.SetColumn(6, GP12_COL7, 80); // center
            gpPQ12.SetColumn(7, GP12_COL8, 105); // right
            gpPQ12.AutoFillRows = true;
            gpPQ12.ShowPageNo = true;
            gpPQ12.ScrollType = ScrollTypes.PageChanged;

            ((System.Windows.Forms.TableLayoutPanel)(gpPQ12.Controls[2])).RowStyles[1] = new RowStyle(SizeType.Absolute, 60);
            ((System.Windows.Forms.Label)(gpPQ12.Controls[2].Controls[1])).AutoSize = false;
            ((System.Windows.Forms.Label)(gpPQ12.Controls[2].Controls[1])).Font = new Font("돋움체", 9, FontStyle.Bold);

            #endregion

            #region 미회수 사유

            LoadNonRtnReasons();

            #endregion

            InitEvents();

            this.m_rtnPrsGiftList = new Dictionary<string, List<RtnPrsGiftData>>();
            this.StatusMessage = MSG_INPUT;
        }

        /// <summary>
        /// 이벤트등록
        /// </summary>
        void InitEvents()
        {
            btnClose.Click += new EventHandler(btnClose_Click);
            btnConfirm.Click += new EventHandler(btnConfirm_Click);
            btnRefundCanc.Click += new EventHandler(btnRefundCanc_Click);

            this.Load += new EventHandler(POS_SL_P005_Load);
            this.KeyEvent += new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(POS_SL_P005_KeyEvent);
            this.gpPQ11.InitializeCell +=
                new WSWD.WmallPos.POS.FX.Win.UserControls.CellDataBoundEventHandler(gpPQ11_InitializeCell);
            this.gpPQ11.RowDataBound += new WSWD.WmallPos.POS.FX.Win.UserControls.RowDataBoundEventHandler(gpPQ11_RowDataBound);
            this.gpPQ12.InitializeCell +=
                new WSWD.WmallPos.POS.FX.Win.UserControls.CellDataBoundEventHandler(gpPQ12_InitializeCell);
            this.gpPQ12.RowDataBound += new WSWD.WmallPos.POS.FX.Win.UserControls.RowDataBoundEventHandler(gpPQ12_RowDataBound);

            this.FormClosed += new FormClosedEventHandler(POS_SL_P005_FormClosed);
        }

        /// <summary>
        /// Form Closed Event
        /// 이벤트 제거
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void POS_SL_P005_FormClosed(object sender, FormClosedEventArgs e)
        {
            btnClose.Click -= new EventHandler(btnClose_Click);
            btnConfirm.Click -= new EventHandler(btnConfirm_Click);

            this.Load -= new EventHandler(POS_SL_P005_Load);
            this.KeyEvent -= new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(POS_SL_P005_KeyEvent);
            this.gpPQ11.InitializeCell -=
                new WSWD.WmallPos.POS.FX.Win.UserControls.CellDataBoundEventHandler(gpPQ11_InitializeCell);
            this.gpPQ11.RowDataBound -= new WSWD.WmallPos.POS.FX.Win.UserControls.RowDataBoundEventHandler(gpPQ11_RowDataBound);
            this.gpPQ12.InitializeCell -=
                new WSWD.WmallPos.POS.FX.Win.UserControls.CellDataBoundEventHandler(gpPQ12_InitializeCell);
            this.gpPQ12.RowDataBound -= new WSWD.WmallPos.POS.FX.Win.UserControls.RowDataBoundEventHandler(gpPQ12_RowDataBound);

            this.FormClosed -= new FormClosedEventHandler(POS_SL_P005_FormClosed);

            // Grid control event delete
            ClearEvents();
        }

        #endregion

        #region 이벤트정의

        /// <summary>
        /// 저장한다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnConfirm_Click(object sender, EventArgs e)
        {
            // apply current inputing text to data
            ValidateCurrentInput();

            string errorMessage = string.Empty;
            if (!ValidateInputDataOnSave(out errorMessage))
            {
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ShowMessageBox(WSWD.WmallPos.FX.Shared.MessageDialogType.Warning, string.Empty, errorMessage);
                }
                return;
            }

            var res = ShowMessageBox(WSWD.WmallPos.FX.Shared.MessageDialogType.Question, string.Empty,
                MSG_CONFIRM_TKS_PRS_RET);
            if (res != DialogResult.Yes)
            {
                return;
            }


            this.ReturnResult.Add("PRESENT_LIST", this.m_prsnList);
            this.DialogResult = DialogResult.Yes;
        }

        /// <summary>
        /// 취소
        /// 11.13 변경사항
        /// ① 회수 처리 등록 하지 않고 자동 반품 처리 한다.	
        /// ② 관리자 ID/PASSWORD PopUp 창을 띄어 관리자가 확인 할수 있도록 한다.	
        /// Return: DialogResult.Cancel, Cancel the Refund but proceed with AutoRtn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnClose_Click(object sender, EventArgs e)
        {
            // Loc changed 11.13 주석처리
            // this.DialogResult = DialogResult.Cancel;
            if (ValidateAdmin())
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// 11.13
        /// 
        /// ① 반품 작업을 진행하지 않고 화면을 종료 한다.
        /// ② 2-3) 화면으로 복귀 하고 반품 확정 Step은 더 이상 진행하지 않고 종료 한다.
        /// 
        /// Return: DialogResult.No
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnRefundCanc_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        /// <summary>
        /// 영수증 리스트 셀만들기
        /// </summary>
        /// <param name="e"></param>
        void gpPQ12_InitializeCell(WSWD.WmallPos.POS.FX.Win.UserControls.CellDataBoundEventArgs e)
        {
            PQ12Grid_BuildCells(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void gpPQ12_RowDataBound(WSWD.WmallPos.POS.FX.Win.UserControls.RowDataBoundEventArgs e)
        {
            PQ12RespData data = (PQ12RespData)e.ItemData;
            if (data == null)
            {
                for (int i = 0; i < e.Row.Cells.Length; i++)
                {
                    e.Row.Cells[i].Controls[0].Text = string.Empty;
                    e.Row.Cells[i].Controls[0].ForeColor = Color.Black;
                }
                return;
            }
            e.Row.Cells[0].Controls[0].Text = data.PresentDate.Substring(2) + "-" + data.PresentNo;// +"-" + data.PresentSeq;
            e.Row.Cells[1].Controls[0].Text = data.PrmName;
            e.Row.Cells[2].Controls[0].Text = data.EventGbn;
            e.Row.Cells[3].Controls[0].Text = data.SumFg;
            e.Row.Cells[4].Controls[0].Text = DateTimeUtils.FormatDateString(data.SaleDate, "/"); ;
            e.Row.Cells[5].Controls[0].Text = data.PosNo;
            e.Row.Cells[6].Controls[0].Text = data.TrxnNo;

            // Loc changed 11.4
            // 반품처리완료여부 = R, 금액 (-)처리
            e.Row.Cells[7].Controls[0].Text = string.Format("{1}{0:#,##0}",
                TypeHelper.ToInt32(data.SaleAmt), data.RtnProcFg.Equals("R") ? "-" : "");
            e.Row.Cells[7].Controls[0].ForeColor = data.RtnProcFg.Equals("R") ? Color.Red : Color.Black;
        }

        void gpPQ11_InitializeCell(WSWD.WmallPos.POS.FX.Win.UserControls.CellDataBoundEventArgs e)
        {
            PQ11Grid_BuildCells(e);
        }

        void gpPQ11_RowDataBound(WSWD.WmallPos.POS.FX.Win.UserControls.RowDataBoundEventArgs e)
        {
            PQ11RespData data = (PQ11RespData)e.ItemData;
            if (data == null)
            {
                for (int i = 0; i < e.Row.Cells.Length; i++)
                {
                    e.Row.Cells[i].Controls[0].Text = string.Empty;
                    e.Row.Cells[3].Controls[0].Visible = false;

                    if (i >= 5)
                    {
                        InputText it = (InputText)e.Row.Cells[i].Controls[0];
                        it.Focusable = false;
                        it.HasBorder = false;
                    }
                }
                return;
            }

            e.Row.Cells[0].Controls[0].Text = data.PresentDate.Substring(2) + "-" + data.PresentNo + "-" + data.PresentSeq;
            e.Row.Cells[1].Controls[0].Text = data.TksGiftName;
            int amt = TypeHelper.ToInt32(data.PresentAmt);
            e.Row.Cells[2].Controls[0].Text = string.Format("{0:#,###}", TypeHelper.ToInt32(data.PresentAmt));
            e.Row.Cells[3].Controls[0].Text = data.ExchGiftNo;
            e.Row.Cells[3].Controls[0].Visible = !string.IsNullOrEmpty(data.ExchGiftNo);
            e.Row.Cells[4].Controls[0].Text = string.Format("{0:#,###}", TypeHelper.ToInt32(data.ReturnAmt));

            e.Row.Cells[5].Controls[0].Text = string.IsNullOrEmpty(data.ExchGiftNo) ? data.RtnCashAmt.ToString() : data.RtnGiftCashAmt.ToString();
            e.Row.Cells[6].Controls[0].Text = data.RtnGiftAmt.ToString();
            e.Row.Cells[7].Controls[0].Text = data.RtnPresentAmt.ToString();

            e.Row.Cells[8].Controls[0].Text = data.RtnCantRsn;

            // 상품권 있을때 현물입력 못하게
            if (!string.IsNullOrEmpty(data.ExchGiftNo))
            {
                e.Row.Cells[7].Controls[0].Tag = true;
            }
            else
            {
                // 상품권 아닐때, 상품권금액 입력 불가
                e.Row.Cells[6].Controls[0].Tag = true;
            }


            for (int i = 5; i < 9; i++)
            {
                InputText it = (InputText)e.Row.Cells[i].Controls[0];
                it.Focusable = true;
                it.HasBorder = true;
            }

            if (e.Row.RowIndex == 0)
            {
                ((InputText)e.Row.Cells[5].Controls[0]).SetFocus();
            }
        }

        /// <summary>
        /// Apply value in current InputText to data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void input_InputLostFocused(object sender, EventArgs e)
        {
            ValidateCurrentInput();
        }

        /// <summary>
        /// 입력된 금액이 CLEAR되면 처리
        /// </summary>
        /// <param name="e"></param>
        void input_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_CLEAR)
            {
                ValidateCurrentInput();
            }
            else
            {
                InputText input = (InputText)this.FocusedControl;
                SaleGridCell cell = (SaleGridCell)input.Parent;
                if (cell.ColumnIndex == 8)
                {
                    ValidateCurrentInput();
                }
            }
        }

        /// <summary>
        /// 상품교환권컬럼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GiftNoBtn_Click(object sender, EventArgs e)
        {
            ShowDetailExchGiftList((WSWD.WmallPos.POS.FX.Win.UserControls.Button)sender);
        }

        void task_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            this.BeginInvoke((MethodInvoker)delegate()
            {
                this.ResumeDrawing();
            });

            if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.Success)
            {
                var datas = responseData.DataRecords.ToDataRecords<PQ12RespData>();
                // bind to grid
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    foreach (var item in datas)
                    {
                        this.gpPQ12.AddRow(item);
                    }
                });
            }
        }

        void task_Errored(string errorMessage, Exception lastException)
        {
            this.BeginInvoke((MethodInvoker)delegate()
            {
                this.ResumeDrawing();
            });
        }

        void POS_SL_P005_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            // readonly control, just pass
            if (this.FocusedControl != null)
            {
                var it = (InputText)this.FocusedControl;
                if ((it.Tag == null ? false : (bool)it.Tag))
                {
                    if (!e.IsControlKey)
                    {
                        e.IsHandled = true;
                        return;
                    }
                    else if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_CLEAR)
                    {
                        e.IsHandled = true;
                        this.PreviousControl();
                        return;
                    }
                }
            }

            if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_CLEAR)
            {
                if (this.FocusedControl != null)
                {
                    var it = (InputText)this.FocusedControl;
                    var val = TypeHelper.ToInt64(it.Text);
                    if (val > 0)
                    {
                        return;
                    }
                }

                e.IsHandled = true;
                this.PreviousControl();
            }
            else if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_ENTER)
            {
                e.IsHandled = true;
                this.NextControl();
            }
        }

        void POS_SL_P005_Load(object sender, EventArgs e)
        {
            this.SuspendDrawing();
            LoadData();

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.RunWorkerAsync();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // backgreound to get PQ12
            if (this.m_bskHeader != null)
            {
                LoadPQ12Data();
            }
        }

        #endregion

        #region 그리드 BINDING

        void PQ11Grid_BuildCells(CellDataBoundEventArgs e)
        {
            Label lbl = null;
            InputText input = null;
            WSWD.WmallPos.POS.FX.Win.UserControls.Button btn = null;
            switch (e.Cell.ColumnIndex)
            {
                case 0:
                    lbl = new Label()
                    {
                        TextAlign = ContentAlignment.MiddleCenter,
                        AutoSize = false,
                        Left = 0,
                        Top = 0,
                        Width = e.Cell.Width,
                        Height = e.Cell.Height,
                        BackColor = Color.Transparent
                    };
                    e.Cell.Controls.Add(lbl);
                    break;
                case 1:
                    lbl = new Label()
                    {
                        TextAlign = ContentAlignment.MiddleLeft,
                        AutoSize = false,
                        Left = 0,
                        Top = 0,
                        Width = e.Cell.Width,
                        Height = e.Cell.Height,
                        BackColor = Color.Transparent
                    };
                    e.Cell.Controls.Add(lbl);
                    break;
                case 3:
                    btn = new WSWD.WmallPos.POS.FX.Win.UserControls.Button()
                    {
                        TextAlign = ContentAlignment.MiddleCenter,
                        Left = 1,
                        Top = 1,
                        Width = e.Cell.Width - 2,
                        Height = e.Cell.Height - 2,
                        Visible = false
                    };
                    btn.Click += new EventHandler(GiftNoBtn_Click);
                    e.Cell.Controls.Add(btn);
                    break;
                case 2:
                case 4:
                    lbl = new Label()
                    {
                        TextAlign = ContentAlignment.MiddleRight,
                        AutoSize = false,
                        Left = 0,
                        Top = 0,
                        Width = e.Cell.Width,
                        Height = e.Cell.Height,
                        BackColor = Color.Transparent
                    };
                    e.Cell.Controls.Add(lbl);
                    break;
                case 8:
                    input = new InputText()
                    {
                        DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric,
                        Format = "#,###",
                        TextAlign = ContentAlignment.MiddleCenter,
                        ReadOnly = false,
                        HasBorder = true,
                        Left = 1,
                        Top = 1,
                        Width = e.Cell.Width - 2,
                        Height = e.Cell.Height - 2,
                        BorderWidth = 1,
                        Corner = 1,
                        MaxLength = 1,
                        Padding = new Padding(0, 0, 2, 0),
                        BorderColor = System.Drawing.Color.FromArgb(187, 187, 187),
                        Focusable = true,
                        BackColor = Color.White
                    };
                    input.InputLostFocused += new EventHandler(input_InputLostFocused);
                    input.KeyEvent += new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(input_KeyEvent);
                    e.Cell.Controls.Add(input);
                    break;
                default:
                    input = new InputText()
                    {
                        Left = 1,
                        Top = 1,
                        Width = e.Cell.Width - 2,
                        Height = e.Cell.Height - 2,
                        DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric,
                        Format = "#,###",
                        TextAlign = ContentAlignment.MiddleRight,
                        ReadOnly = e.Cell.ColumnIndex == 6,
                        HasBorder = true,
                        BorderWidth = 1,
                        Padding = new Padding(0, 0, 2, 0),
                        Corner = 1,
                        MaxLength = 8,
                        BorderColor = System.Drawing.Color.FromArgb(187, 187, 187),
                        Focusable = true,
                        BackColor = Color.White
                    };
                    input.InputLostFocused += new EventHandler(input_InputLostFocused);
                    e.Cell.Controls.Add(input);
                    break;
            }

        }

        void PQ12Grid_BuildCells(CellDataBoundEventArgs e)
        {
            Label lbl = null;
            switch (e.Cell.ColumnIndex)
            {
                case 1:
                    lbl = new Label()
                    {
                        TextAlign = ContentAlignment.MiddleLeft,
                        AutoSize = false,
                        Dock = DockStyle.Fill
                    };
                    e.Cell.Controls.Add(lbl);
                    break;
                case 7:
                    lbl = new Label()
                    {
                        TextAlign = ContentAlignment.MiddleRight,
                        AutoSize = false,
                        Dock = DockStyle.Fill
                    };
                    e.Cell.Controls.Add(lbl);
                    break;
                default:
                    lbl = new Label()
                    {
                        TextAlign = ContentAlignment.MiddleCenter,
                        AutoSize = false,
                        Dock = DockStyle.Fill
                    };
                    e.Cell.Controls.Add(lbl);
                    break;
            }
        }

        /// <summary>
        /// Remove all events handler
        /// </summary>
        void ClearEvents()
        {
            foreach (var item in gpPQ11.Rows)
            {
                var btn = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)item.Cells[3].Controls[0];
                btn.Click -= new EventHandler(GiftNoBtn_Click);

                var input = (InputText)item.Cells[8].Controls[0];
                input.InputLostFocused -= new EventHandler(input_InputLostFocused);
                input.KeyEvent -= new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(input_KeyEvent);

                input = (InputText)item.Cells[5].Controls[0];
                input.InputLostFocused -= new EventHandler(input_InputLostFocused);

                input = (InputText)item.Cells[6].Controls[0];
                input.InputLostFocused -= new EventHandler(input_InputLostFocused);

                input = (InputText)item.Cells[7].Controls[0];
                input.InputLostFocused -= new EventHandler(input_InputLostFocused);
            }
        }

        #endregion

        #region 사용자정의

        /// <summary>
        /// 미회수 사유를 디비에서 가져오기
        /// </summary>
        void LoadNonRtnReasons()
        {
            m_nonRtnReasons = new Dictionary<string, string>();

            #region 미회수 사유 리스트 가져오기

            using (var db = MasterDbHelper.InitInstance())
            {
                var dsCodes = db.ExecuteQuery("SELECT CD_BODY, NM_BODY FROM SYM051T WHERE CD_HEAD = 'GC14' ORDER BY CD_BODY", null, null);

                foreach (DataRow dr in dsCodes.Tables[0].Rows)
                {
                    m_nonRtnReasons.Add(dr["CD_BODY"].ToString(), dr["NM_BODY"].ToString());
                }
            }

            #endregion

            //m_nonRtnReasons.Add("1", "불응");
            //m_nonRtnReasons.Add("2", "기타");

            StringBuilder sb = new StringBuilder();
            foreach (var key in m_nonRtnReasons.Keys)
            {
                sb.AppendFormat("{0}:{1}", key, m_nonRtnReasons[key]);
                sb.Append(" / ");
            }

            lblRtnReason.Text = GP11_RTN_REASON;
            string rsn = sb.ToString();
            if (rsn.Length > 0)
            {
                rsn = rsn.Substring(0, rsn.Length - 3);
                lblRtnReason.Text += rsn;
            }

            rsn = null;
            sb = null;
        }

        void LoadData()
        {
            foreach (var item in m_prsnList)
            {
                gpPQ11.AddRow(item);

                // init m_rtnPrsGiftList
                if (!string.IsNullOrEmpty(item.ExchGiftNo))
                {
                    m_rtnPrsGiftList.Add(item.ExchGiftNo, new List<RtnPrsGiftData>());
                }
            }

            gpPQ11.PageIndex = 0;
        }

        void LoadPQ12Data()
        {
            PQ12DataTask task = new PQ12DataTask(this.m_bskHeader.SaleDate,
                this.m_bskHeader.StoreNo, this.m_bskHeader.PosNo, this.m_bskHeader.TrxnNo);
            task.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(task_TaskCompleted);
            task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(task_Errored);
            task.ExecuteTask();
        }

        /// <summary>
        /// 등록시 확인한다
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        bool ValidateInputDataOnSave(out string errorMessage)
        {
            int validCount = 0;
            errorMessage = string.Empty;

            // 관리자 확인 필요여부
            bool needValidateAdmin = false;

            foreach (var item in this.m_prsnList)
            {
                // ② 만약 미회수 사유가 입력된 경우라면 관리자 
                // ID/PASSWORD PopUp 창을 띄어 관리자가 확인 할수 있도록 한다.
                if (!string.IsNullOrEmpty(item.RtnCantRsn))
                {
                    // 금액이 입력 된 상태에는 미회수 사유 입력 불가
                    if (item.RtnGiftCashAmt > 0 || item.RtnCashAmt > 0 
                        || item.RtnGiftAmt > 0 || item.RtnPresentAmt > 0)
                    {
                        errorMessage = ERR_MSG_1;
                        validCount = 0;
                        break;
                    }

                    needValidateAdmin = true;
                    validCount++;
                    continue;
                }

                if (item.RtnCashAmt > 0 && item.RtnGiftAmt > 0)
                {
                    errorMessage = ERR_MSG_2;
                    break;
                }

                if (item.RtnPresentAmt > 0 && item.RtnGiftAmt > 0)
                {
                    errorMessage = ERR_MSG_3;
                    break;
                }

                // 금액확인
                Int64 totalAmt = item.RtnPresentAmt + item.RtnGiftAmt + item.RtnCashAmt + item.RtnGiftCashAmt;
                if (totalAmt > TypeHelper.ToInt64(item.ReturnAmt))
                {
                    errorMessage = ERR_MSG_4;
                    break;
                }
                else if (totalAmt > 0)
                {
                    validCount++;
                }
            }

            if (validCount > 0 && string.IsNullOrEmpty(errorMessage))
            {
                var validDatas = this.m_prsnList.Where(p => !string.IsNullOrEmpty(p.RtnCantRsn)
                    || (p.RtnCashAmt + p.RtnGiftAmt + p.RtnPresentAmt + p.RtnGiftCashAmt) > 0).ToArray();
                List<PQ11RespData> retDatas = new List<PQ11RespData>();
                retDatas.AddRange(validDatas);

                this.m_prsnList = retDatas;
            }
            else
            {
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = ERR_MSG_5;
                }
            }

            if (validCount > 0 && string.IsNullOrEmpty(errorMessage) && needValidateAdmin)
            {
                // validate admin
                if (!ValidateAdmin())
                {
                    validCount = 0;
                }
            }

            return validCount > 0 && string.IsNullOrEmpty(errorMessage);
        }

        /// <summary>
        /// 관리자확인 팝업
        /// </summary>
        /// <returns></returns>
        bool ValidateAdmin()
        {
            using (var pop = ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.SO.dll",
                "WSWD.WmallPos.POS.SO.VC.POS_SO_P001", "03,04"))
            {
                var res = pop.ShowDialog(this);
                return res != DialogResult.Cancel;
            }
        }

        /// <summary>
        /// 현재입력중인 데이타를 확인한다
        /// </summary>
        void ValidateCurrentInput()
        {
            if (this.FocusedControl == null)
            {
                return;
            }

            InputText input = (InputText)this.FocusedControl;
            if ((input.Tag == null ? false : (bool)input.Tag))
            {
                return;
            }

            SaleGridCell cell = (SaleGridCell)input.Parent;

            Int64 val = 0;
            if (cell.ColumnIndex != 8 &
                input.DataType == WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric)
            {
                val = TypeHelper.ToInt64(input.Text);
                val = ValidateMoney(val);
                input.Text = val.ToString();
            }

            PQ11RespData data = (PQ11RespData)cell.Row.ItemData;
            if (data == null)
            {
                return;
            }

            // todo
            // 1.aappl value to data item;

            switch (cell.ColumnIndex)
            {
                case 5:
                    // if cash column; it ExchGiftNo empty, only accept cash
                    if (string.IsNullOrEmpty(data.ExchGiftNo))
                    {
                        data.RtnCashAmt = val;
                        data.RtnGiftCashAmt = 0;
                    }
                    else
                    {
                        // if not, accept cash stands for gift
                        data.RtnGiftCashAmt = val;
                        data.RtnCashAmt = 0;
                    }
                    break;
                case 6:
                    data.RtnGiftAmt = val;
                    break;
                case 7:
                    // 현물금액
                    data.RtnPresentAmt = val;
                    break;
                default:
                    //if (!"1".Equals(input.Text) && !string.IsNullOrEmpty(input.Text))
                    //{
                    //    input.Text = "2";
                    //}

                    if (!string.IsNullOrEmpty(input.Text))
                    {
                        if (!m_nonRtnReasons.ContainsKey(input.Text))
                        {
                            input.Text = string.Empty;
                        }
                    }

                    data.RtnCantRsn = input.Text;
                    break;
            }
        }

        Int64 ValidateMoney(Int64 intValue)
        {
            return (Int64)Math.Ceiling((double)(intValue / 10) * 10);
        }

        /// <summary>
        /// 반납상품교환권 등록화면
        /// <param name="giftButton">해당행의 교환권번호버튼</param>
        /// </summary>
        void ShowDetailExchGiftList(WSWD.WmallPos.POS.FX.Win.UserControls.Button giftButton)
        {
            // TODO
            // 1. Check amount, remain 회수금액 > 0
            // 2. Check 미회수입력
            SaleGridCell cell = giftButton.Parent as SaleGridCell;
            SaleGridRow row = cell.Row;
            string noRtnReason = row.Cells[8].Controls[0].Text;
            if (!string.IsNullOrEmpty(noRtnReason))
            {
                ShowMessageBox(WSWD.WmallPos.FX.Shared.MessageDialogType.Warning, string.Empty,
                    ERR_MSG_NORTN_RSN_ENTER_INVALID);
                return;
            }

            if (string.IsNullOrEmpty(giftButton.Text))
            {
                return;
            }

            string exchGiftNo = giftButton.Text;

            Int64 cashAmt = TypeHelper.ToInt64(row.Cells[5].Controls[0].Text.Replace(",", ""));
            //Int64 giftAmt = TypeHelper.ToInt64(row.Cells[6].Controls[0].Text.Replace(",", ""));
            Int64 retAmt = TypeHelper.ToInt64(row.Cells[4].Controls[0].Text.Replace(",", ""));

            // SHOW POUPP
            using (var pop = ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.SL.dll",
                "WSWD.WmallPos.POS.SL.VC.POS_SL_P007", m_rtnPrsGiftList, giftButton.Text, retAmt - cashAmt))
            {
                var res = pop.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    // Update back to row data from P007 pop
                    var retList = (List<RtnPrsGiftData>)pop.ReturnResult["GIFT_LIST"];
                    m_rtnPrsGiftList[giftButton.Text] = retList;

                    // update total input gift amt;
                    PQ11RespData data = (PQ11RespData)row.ItemData;
                    data.RtnGiftAmt = (long)pop.ReturnResult["GIFT_TOTAL"];
                    row.Cells[6].Controls[0].Text = data.RtnGiftAmt.ToString();
                    data.RtnPrsnList = new List<RtnPrsGiftData>();
                    data.RtnPrsnList.AddRange(retList);
                }
            }
        }

        #endregion

    }
}
