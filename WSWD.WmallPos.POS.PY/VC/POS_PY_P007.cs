//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P007.cs
 * 화면설명 : 타사상품권
 * 개발자   : 정광호
 * 개발일자 : 2015.05.
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

using WSWD.WmallPos.POS.PY.PI;
using WSWD.WmallPos.POS.PY.PT;
using WSWD.WmallPos.POS.PY.VI;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PP;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PP;
using WSWD.WmallPos.FX.NetComm.Tasks.PP;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using System.Diagnostics;

namespace WSWD.WmallPos.POS.PY.VC
{
    public partial class POS_PY_P007 : PopupBase, IPYP007View
    {
        #region 변수

        /// <summary>
        /// 비즈니스 로직
        /// </summary>
        private IPYP007presenter m_Presenter;

        /// <summary>
        /// 타사 상품권 권종 내역
        /// </summary>
        private DataSet dSTikcet = null;

        /// <summary>
        /// 추가된 타사상품권 내역
        /// </summary>
        private DataTable dtAddTicket = null;

        /// <summary>
        /// 받을돈
        /// </summary>
        private int _iGetAmt = 0;

        /// <summary>
        /// 타사상품권 내역
        /// </summary>
        private List<BasketOtherTicket> _basketOtherTickets = null;

        /// <summary>
        /// 최대 등록건수 확인
        /// </summary>
        private int iTranOverCnt = 0;

        /// <summary>
        /// 타사상품권 종류
        /// </summary>
        private string strType = string.Empty;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        /// <summary>
        /// 반품여부
        /// </summary>
        private bool modeReturn = false;

        /// <summary>
        /// 자동,수동 반품여부
        /// </summary>
        private bool _bAuto = false;

        #endregion

        #region 생성자


        /// <summary>
        /// 타사상품권
        /// </summary>
        /// <param name="iGetAmt">받을돈</param>
        /// <param name="basketOtherTickets">타사상품권 내역</param>
        /// <param name="_iOverCnt">Tran 개수</param>
        public POS_PY_P007(int iGetAmt, List<BasketPay> basket, int _iTranOverCnt, bool modeReturn, bool bAuto)
        {
            InitializeComponent();

            //현재 Tran 개수
            iTranOverCnt = _iTranOverCnt;

            //받을돈
            _iGetAmt = iGetAmt;

            this.modeReturn = modeReturn;
            this._bAuto = bAuto;

            if (bAuto)
            {
                this.Text = this.Text + (modeReturn ? strMsg09 : string.Empty);
                this.btnClose.Enabled = !modeReturn;
            }
            else
            {
                this.Text = this.Text + (modeReturn ? strMsg11 : string.Empty);
            }

            //타사상품권 내역
            _basketOtherTickets = new List<BasketOtherTicket>();

            foreach (var item in basket)
            {
                if (item.GetType().Name.ToString() == "BasketOtherTicket")
                {
                    BasketOtherTicket bp = (BasketOtherTicket)item;

                    if (bp != null)
                    {
                        _basketOtherTickets.Add(bp);
                    }
                }
            }

            //Form Load Event
            Load += new EventHandler(form_Load); 
        }

        #endregion

        #region 이벤트 등록

        /// <summary>
        /// Form Event 정의
        /// </summary>
        private void InitEvent()
        {
            this.FormClosed += new FormClosedEventHandler(POS_PY_P007_FormClosed);
            this.KeyEvent += new OPOSKeyEventHandler(form_KeyEvent);                                                                    //KeyEvent
            this.btnCancel.Click += new EventHandler(btnCancel_Click);                                                                  //한건취소 button Event
            this.btnSave.Click += new EventHandler(btnSave_Click);                                                                      //결제확정 button Event
            this.btnClose.Click += new EventHandler(btnClose_Click);                                                                    //닫기 button Event
            btnType.Click += new EventHandler(btnType_Click);                                                                           //상품권종류 선택
            grd.RowDataBound += new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanelRowDataBoundEventHandler(grd_RowDataBound);          //그리드 데이터 바인딩 Event
            if (POSDeviceManager.Scanner != null)
            {
                POSDeviceManager.Scanner.DataEvent += new POSDataEventHandler(Scanner_DataEvent);                                       //Scanner Event    
            }
        }

        void POS_PY_P007_FormClosed(object sender, FormClosedEventArgs e)
        {
            Load -= new EventHandler(form_Load);             
            this.FormClosed -= new FormClosedEventHandler(POS_PY_P007_FormClosed);
            this.KeyEvent -= new OPOSKeyEventHandler(form_KeyEvent);                                                                    //KeyEvent
            this.btnCancel.Click -= new EventHandler(btnCancel_Click);                                                                  //한건취소 button Event
            this.btnSave.Click -= new EventHandler(btnSave_Click);                                                                      //결제확정 button Event
            this.btnClose.Click -= new EventHandler(btnClose_Click);                                                                    //닫기 button Event
            btnType.Click -= new EventHandler(btnType_Click);                                                                           //상품권종류 선택
            grd.RowDataBound -= new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanelRowDataBoundEventHandler(grd_RowDataBound);          //그리드 데이터 바인딩 Event
            
            if (POSDeviceManager.Scanner != null)
            {
                POSDeviceManager.Scanner.DataEvent -= new POSDataEventHandler(Scanner_DataEvent);                                       //Scanner Event    
            }
        }

        #endregion

        #region 이벤트 정의

        /// <summary>
        /// Form Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void form_Load(object sender, EventArgs e)
        {
            //이벤트 등록
            InitEvent();

            dtAddTicket = new DataTable();
            dtAddTicket.Columns.Add("ColNo");
            dtAddTicket.Columns.Add("ColNum");
            dtAddTicket.Columns.Add("ColAmt");
            dtAddTicket.Columns.Add("ColTypeNm");
            dtAddTicket.Columns.Add("ColTypeCd");

            //그리드 컬럼 설정
            grd.AddColumn("ColNo", "NO", 50);
            grd.AddColumn("ColNum", "상품권번호", 170);
            grd.AddColumn("ColAmt", "권종금액", 100);
            grd.AddColumn("ColTypeNm", "종류");
            grd.AddColumn("ColTypeCd", "", 0);

            SetControlDisable(true);

            //타사상품권 종류 조회
            m_Presenter = new PYP007presenter(this);
            m_Presenter.GetTicket();
            
            //받을돈
            txtGetAmt.Text = _iGetAmt > 0 ? string.Format("{0}", _iGetAmt) : "0";

            //최대건수 확인
            iTranOverCnt += _basketOtherTickets != null ? _basketOtherTickets.Count : 0;

            if (iTranOverCnt >= 90)
            {
                txtType.ReadOnly = true;
                txtTicketNo.ReadOnly = true;
                txtTicketAmt.ReadOnly = true;
                btnType.Enabled = false;
                msgBar.Text = strMsg04;
            }
            else
            {
                msgBar.Text = strMsg01;     //메세지 설정
                txtType.SetFocus();
            }
        }

        /// <summary>
        /// KeyEvent
        /// </summary>
        /// <param name="e"></param>
        void form_KeyEvent(OPOSKeyEventArgs e)
        {
            if (_bDisable)
            {
                e.IsHandled = true;
                return;
            }

            if (e.Key.OPOSKey == OPOSMapKeys.KEY_NOSALE)
            {
                if (POSDeviceManager.CashDrawer != null && POSDeviceManager.CashDrawer.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened && POSDeviceManager.CashDrawer.Enabled)
                {
                    //돈통 open
                    POSDeviceManager.CashDrawer.OpenDrawer();
                }
                return;
            }

            if (TypeHelper.ToInt32(txtGetAmt.Text.Length > 0 ? txtGetAmt.Text.ToString() : "0") <= 0)
            {
                msgBar.Text = strMsg06;
                e.IsHandled = true;
                return;
            }

            if (txtType.IsFocused)
            {
                if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
                {
                    if (txtType.Text.Length > 0)
                    {
                        lblType.Text = "";
                        msgBar.Text = strMsg01;
                    }
                    else
                    {
                        e.IsHandled = true;
                        btnClose_Click(btnClose, null);
                        return;
                    }
                }
                else if (e.Key.OPOSKey == OPOSMapKeys.KEY_BKS)
                {
                    lblType.Text = "";
                    msgBar.Text = strMsg01;
                }
                else if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
                {
                    if (dSTikcet != null && dSTikcet.Tables.Count > 0 && dSTikcet.Tables[0].Rows.Count > 0)
                    {
                        DataRow[] drFilter = dSTikcet.Tables[0].Select(string.Format("KD_GIFT = '{0}'", txtType.Text));

                        if (drFilter != null && drFilter.Length > 0)
                        {
                            lblType.Text = drFilter[0]["NM_GIFT"] != null ? drFilter[0]["NM_GIFT"].ToString() : "";
                            txtTicketNo.SetFocus();
                            msgBar.Text = strMsg02;
                        }
                        else
                        {
                            txtType.Text = "";
                            lblType.Text = "";
                            btnType_Click(btnType, null);
                        }
                    }
                    else
                    {
                        txtType.Text = "";
                        lblType.Text = "";
                        btnType_Click(btnType, null);
                    }
                }
            }
            else if (txtTicketNo.IsFocused)
            {
                if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
                {
                    if (txtTicketNo.Text.Length > 0)
                    {
                        msgBar.Text = strMsg02;    
                    }
                    else
                    {
                        txtTicketNo.Text = "";
                        txtType.ReadOnly = false;
                        txtType.SetFocus();
                        txtTicketNo.ReadOnly = false;
                        btnType.Enabled = true;
                        msgBar.Text = strMsg01;
                        e.IsHandled = true;
                    }
                }
                else if (e.Key.OPOSKey == OPOSMapKeys.KEY_BKS)
                {
                    msgBar.Text = strMsg02;
                }
                else if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
                {
                    if (txtTicketNo.Text.Length > 0)
                    {
                        //상품권번호 확인
                        CheckTicket();
                    }
                    else
                    {
                        msgBar.Text = strMsg02;
                    }
                }
            }
            else if (txtTicketAmt.IsFocused)
            {
                if (txtType.Text.Length <= 0)
                {
                    txtType.SetFocus();
                    msgBar.Text = strMsg01;

                    txtType.ReadOnly = false;
                    txtTicketNo.Text = "";
                    txtTicketNo.ReadOnly = true;
                    txtTicketAmt.Text = "";
                    txtTicketAmt.ReadOnly = true;
                    btnType.Enabled = true;

                    e.IsHandled = true;
                }
                else
                {
                    if (txtTicketNo.Text.Length <= 0)
                    {
                        txtTicketNo.ReadOnly = false;
                        txtTicketNo.SetFocus();
                        msgBar.Text = strMsg02;

                        txtTicketAmt.Text = "";
                        txtTicketAmt.ReadOnly = true;

                        e.IsHandled = true;
                    }
                    else
                    {
                        if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
                        {
                            if (txtTicketAmt.Text.Length > 0)
                            {
                                msgBar.Text = strMsg03;
                            }
                            else
                            {
                                txtTicketNo.Text = "";
                                txtTicketNo.ReadOnly = false;
                                txtTicketNo.SetFocus();
                                msgBar.Text = strMsg02;
                                txtTicketAmt.ReadOnly = true;
                                e.IsHandled = true;
                            }
                        }
                        else if (e.Key.OPOSKey == OPOSMapKeys.KEY_BKS)
                        {
                            msgBar.Text = strMsg03;
                        }
                        else if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
                        {
                            if (txtTicketAmt.Text.Length > 0)
                            {
                                //권종금액 확인
                                CheckAmt();
                            }
                        }
                    }
                }
            }
        }

        void btnType_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;

            if (POSDeviceManager.Scanner != null)
            {
                POSDeviceManager.Scanner.DataEvent -= new POSDataEventHandler(Scanner_DataEvent);                                       //Scanner Event    
            }

            using (var pop = ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.PY.dll",
                "WSWD.WmallPos.POS.PY.VC.POS_PY_P020"))
            {
                if (pop.ShowDialog(this) == DialogResult.OK)
                {
                    if (pop.ReturnResult != null && pop.ReturnResult.Count > 0)
                    {
                        foreach (var item in pop.ReturnResult)
                        {
                            if (item.Key == "KD_GIFT")
                            {
                                txtType.Text = item.Value.ToString();
                            }
                            else if (item.Key == "NM_GIFT")
                            {
                                lblType.Text = item.Value.ToString();
                            }
                        }

                        if (txtType.Text.Length > 0)
                        {
                            txtTicketNo.Text = "";
                            txtTicketAmt.Text = "";
                            txtTicketNo.SetFocus();
                            msgBar.Text = strMsg02;
                        }
                    }
                }
            }

            if (POSDeviceManager.Scanner != null)
            {
                POSDeviceManager.Scanner.DataEvent += new POSDataEventHandler(Scanner_DataEvent);                                       //Scanner Event    
            }
        }

        /// <summary>
        /// 한건취소 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCancel_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;
            if (grd == null || grd.RowCount <= 0 || grd.CurrentRowIndex < 0) return;

            SetControlDisable(true);

            try
            {
                DataRow dr = (System.Data.DataRow)(grd.GetRow(grd.CurrentRowIndex).ItemData);

                //그리드 선택행 삭제
                dtAddTicket.Rows.Remove(dr);
                grd.DeleteActiveRow();

                int iRow = 1;
                Int32 iAmt = 0;

                foreach (DataRow drTemp in dtAddTicket.Rows)
                {
                    iAmt += TypeHelper.ToInt32(TypeHelper.ToString(drTemp["ColAmt"]).Replace(",", ""));
                    drTemp[0] = iRow.ToString();
                    iRow++;
                }

                for (int i = 0; i < grd.RowCount; i++)
                {
                    DataRow drTemp = (DataRow)grd.GetRow(i).ItemData;
                    grd.UpdateRow(i, drTemp);
                }

                txtGetAmt.Text = _iGetAmt - iAmt > 0 ? string.Format("{0}", _iGetAmt - iAmt) : "0";   //받을돈 원복
                txtTicketTotalAmt.Text = iAmt > 0 ? string.Format("{0}", iAmt) : "0";                 //상품권합계 원복

                if (ChkCnt(-1))
                {
                    //txtType.Text = "";
                    //lblType.Text = "";
                    //txtType.SetFocus();
                    txtType.ReadOnly = false;
                    txtTicketNo.Text = "";
                    txtTicketNo.ReadOnly = false;
                    txtTicketNo.SetFocus();
                    txtTicketAmt.Text = "";
                    txtTicketAmt.ReadOnly = true;
                    btnType.Enabled = true;
                    msgBar.Text = strMsg02;
                    iTranOverCnt--;
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);                
            }
            finally 
            {
                SetControlDisable(false);
            }
        }

        /// <summary>
        /// 적용 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;

            if ((modeReturn && !_bAuto) || !modeReturn)
            {
                if (grd == null || grd.RowCount <= 0) return;
            }

            SetControlDisable(true);

            try
            {
                var listBasket = new List<BasketPay>();
                BasketOtherTicket bp = null;

                Int32 iGetAmt = _iGetAmt;
                Int32 iPayAmt = 0;

                for (int i = 0; i < grd.RowCount; i++)
                {
                    DataRow dr = (System.Data.DataRow)(grd.GetRow(i).ItemData);

                    iPayAmt = TypeHelper.ToInt32(dr["ColAmt"].ToString().Replace(",", ""));
                    bp = new BasketOtherTicket();
                    bp.PayGrpCd = NetCommConstants.PAYMENT_GROUP_TKCKET;        //지불 수단 그룹 코드
                    bp.PayDtlCd = NetCommConstants.PAYMENT_DETAIL_TICKET_OTHER; //지불 수단 상세 코드
                    bp.PayAmt = iPayAmt.ToString();                             //지불 금액
                    bp.CancFg = "0";                                             //취소 flag
                    bp.OtherTicketType = dr["ColTypeCd"].ToString();            //타사상품권 종류
                    bp.OtherTicketNo = dr["ColNum"].ToString();                 //타사상품권 번호
                    bp.OtherTicketNm = dr["ColTypeNm"].ToString();              //타사상품권 이름
                    bp.TicketAmt = iPayAmt.ToString();                          //권종 금액
                    bp.OtherTicketDivision = "01";                              //상품권 구분	S (01:지로, 02:모바일)

                    iGetAmt -= iPayAmt;

                    if (iGetAmt < 0)
                    {
                        bp.BalAmt = (iGetAmt * -1).ToString();
                        bp.BalGrpCd = NetCommConstants.PAYMENT_GROUP_CASH;
                        bp.BalDtlCd = NetCommConstants.PAYMENT_DETAIL_CASH;
                    }
                    else
                    {
                        bp.BalGrpCd = NetCommConstants.PAYMENT_GROUP_NONE;
                        bp.BalDtlCd = NetCommConstants.PAYMENT_DETAIL_NONE;
                    }

                    listBasket.Add(bp);
                }

                if (modeReturn && _bAuto && iGetAmt > 0)
                {
                    
                    var ret = ShowMessageBox(MessageDialogType.Question, string.Empty,
                        string.Format(strMsg10, iGetAmt));
                    if (ret == DialogResult.Yes)
                    {
                        #region 현금BASKET생성
                        BasketPayCash payCash = new BasketPayCash();
                        payCash.PayAmt = iGetAmt.ToString();
                        listBasket.Add(payCash);
                        #endregion
                    }
                    else
                    {
                        return;
                    }
                }

                this.ReturnResult.Add("PAY_DATA", listBasket);
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception)
            {
                
            }
            finally
            {
                SetControlDisable(false);
            }
        }

        /// <summary>
        /// 닫기 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;

            SetControlDisable(true);

            try
            {
                if (modeReturn && _bAuto && _iGetAmt > 0)
                {
                    var listBasket = new List<BasketPay>();

                    var ret = ShowMessageBox(MessageDialogType.Question, string.Empty,
                        string.Format(strMsg10, _iGetAmt));
                    if (ret == DialogResult.Yes)
                    {
                        #region 현금BASKET생성
                        BasketPayCash payCash = new BasketPayCash();
                        payCash.PayAmt = _iGetAmt.ToString();
                        listBasket.Add(payCash);
                        #endregion
                    }
                    else
                    {
                        return;
                    }

                    this.ReturnResult.Add("PAY_DATA", listBasket);
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    this.DialogResult = DialogResult.Cancel;
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                SetControlDisable(false);
            }
        }

        /// <summary>
        /// 그리드 메인 데이터 바인딩
        /// </summary>
        /// <param name="row"></param>
        void grd_RowDataBound(WSWD.WmallPos.POS.FX.Win.UserControls.GridRow row)
        {
            if (row.RowState == GridRowState.Added)
            {
                for (int i = 0; i < 5; i++)
                {
                    row.Cells[i].Controls.Add(new Label()
                    {
                        Text = ((System.Data.DataRow)(row.ItemData)).ItemArray[i].ToString(),
                        AutoSize = false,
                        TextAlign = i == 0 || i == 2 ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft,
                        Dock = DockStyle.Fill
                    });
                }
            }
            else
            {
                DataRow[] drFilter = dtAddTicket.Select(string.Format("ColNum = '{0}'", row.Cells[1].Controls[0].Text.ToString()));

                if (drFilter != null && drFilter.Length > 0)
                {
                    row.Cells[0].Controls[0].Text = drFilter[0]["ColNo"].ToString();
                }
            }
        }

        /// <summary>
        /// Scanner Event
        /// </summary>
        /// <param name="msrData"></param>
        void Scanner_DataEvent(string msrData)
        {
            Trace.WriteLine("PY_P007_Scanner_DataEvent " + msrData, "program");

            if (_bDisable || txtTicketNo.ReadOnly || TypeHelper.ToInt32(txtGetAmt.Text) <= 0) return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    txtTicketNo.Text = "";
                    Application.DoEvents();
                    if (msrData.Length > 0)
                    {
                        txtTicketNo.Text = msrData;

                        Application.DoEvents();

                        //상품권번호 확인
                        CheckTicket();    
                    }
                    
                });
            }
            else
            {
                txtTicketNo.Text = "";
                Application.DoEvents();
                if (msrData.Length > 0)
                {
                    txtTicketNo.Text = msrData;
                    
                    Application.DoEvents();

                    //상품권번호 확인
                    CheckTicket();
                }
            }
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// 타사 상품권 권종 셋팅
        /// </summary>
        /// <param name="ds">타사 상품권 권종 내역</param>
        public void SetTicket(DataSet ds)
        {
            try
            {
                if (dSTikcet != null)
                {
                    dSTikcet.Clear();
                }

                dSTikcet = ds.Copy();
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                SetControlDisable(false);
            }
        }

        /// <summary>
        /// 상품권번호 및 권종금액 확인
        /// </summary>
        void CheckTicket()
        {
            SetControlDisable(true);

            try
            {
                bool bOver = false;
                bool bAdd = false;
                string strCD_PREF = string.Empty;   //프리픽스
                int iCD_PO_NOTE = 0;                //권종코드 시작위치
                int iCD_LN_NOTE = 0;                //권종코드 길이
                int iCD_LN_GIFT = 0;                //타사상품권 코드 길이
                bool bFG_JOIN = false;              //가맹여부
                string strCD_NOTE = string.Empty;
                int iAM_NOTE = 0;
                Int32 iGetAmt = TypeHelper.ToInt32(txtGetAmt.Text.Length > 0 ? txtGetAmt.Text.ToString() : "0");
                Int32 iTotalAmt = TypeHelper.ToInt32(txtTicketTotalAmt.Text.Length > 0 ? txtTicketTotalAmt.Text.ToString() : "0");
                string strTicket = txtTicketNo.Text.ToString();

                //상품권번호 확인
                DataRow[] drFilter = dSTikcet.Tables[1].Select(string.Format("KD_GIFT = '{0}'", txtType.Text.ToString()));

                if (drFilter != null && drFilter.Length > 0)
                {
                    //중복 및 건수확인
                    if (!ChkDup()) return;

                    foreach (DataRow dr in drFilter)
                    {
                        bAdd = false;

                        strCD_PREF = dr["CD_PREF"].ToString();
                        iCD_PO_NOTE = TypeHelper.ToInt32(dr["CD_PO_NOTE"].ToString());
                        iCD_LN_NOTE = TypeHelper.ToInt32(dr["CD_LN_NOTE"].ToString());
                        iCD_LN_GIFT = TypeHelper.ToInt32(dr["CD_LN_GIFT"].ToString());
                        bFG_JOIN = dr["FG_JOIN"] != null && dr["FG_JOIN"].ToString() != "" && dr["FG_JOIN"].ToString() == "1" ? true : false;
                        strCD_NOTE = dr["CD_NOTE"] != null ? dr["CD_NOTE"].ToString() : "";
                        iAM_NOTE = TypeHelper.ToInt32(dr["AM_NOTE"] != null && dr["AM_NOTE"].ToString() != "" ? dr["AM_NOTE"].ToString() : "0");

                        if (bFG_JOIN)
                        {
                            if (iCD_LN_GIFT == strTicket.Length)
                            {
                                if ((iCD_PO_NOTE - 1) + iCD_LN_NOTE <= strTicket.Length)
                                {
                                    if (strTicket.Substring(0, strCD_PREF.Length) == strCD_PREF)
                                    {
                                        if (strTicket.Substring(iCD_PO_NOTE - 1, iCD_LN_NOTE) == strCD_NOTE)
                                        {
                                            if (modeReturn && iAM_NOTE > TypeHelper.ToInt64(txtGetAmt.Text))
                                            {
                                                bOver = true;
                                                txtTicketNo.Text = "";
                                                txtTicketAmt.Text = "";
                                                msgBar.Text = strMsg12;
                                                txtTicketNo.SetFocus();
                                                break;
                                            }
                                            else
                                            {
                                                DataRow drAdd = dtAddTicket.NewRow();
                                                drAdd[0] = (grd.RowCount + 1).ToString();
                                                drAdd[1] = strTicket;
                                                drAdd[2] = string.Format("{0:#,##0}", iAM_NOTE);
                                                drAdd[3] = dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "";
                                                drAdd[4] = dr["KD_GIFT"].ToString();
                                                dtAddTicket.Rows.Add(drAdd);
                                                grd.AddRow(drAdd);
                                                iTranOverCnt++;

                                                txtTicketTotalAmt.Text = iTotalAmt + iAM_NOTE > 0 ? string.Format("{0}", iTotalAmt + iAM_NOTE) : "0";
                                                txtGetAmt.Text = iGetAmt - iAM_NOTE > 0 ? string.Format("{0}", iGetAmt - iAM_NOTE) : "0";

                                                //txtType.Text = "";
                                                //lblType.Text = "";
                                                txtTicketNo.Text = "";
                                                txtTicketAmt.Text = "";

                                                if (iGetAmt - iAM_NOTE <= 0)
                                                {
                                                    txtType.ReadOnly = true;
                                                    txtTicketNo.ReadOnly = true;
                                                    txtTicketAmt.ReadOnly = true;
                                                    btnType.Enabled = false;
                                                    txtType.SetFocus();
                                                    msgBar.Text = strMsg06;
                                                }
                                                else
                                                {
                                                    txtType.ReadOnly = false;
                                                    txtTicketNo.ReadOnly = false;
                                                    txtTicketAmt.ReadOnly = true;
                                                    btnType.Enabled = true;
                                                    txtTicketNo.SetFocus();
                                                    msgBar.Text = strMsg02;
                                                }

                                                bAdd = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (strTicket.Length > 0)
                            {
                                bAdd = true;
                                txtTicketAmt.Text = "";
                                txtTicketAmt.ReadOnly = false;
                                txtTicketAmt.SetFocus();
                                msgBar.Text = strMsg03;
                                break;
                            }
                        }
                    }

                    if (!bAdd)
                    {
                        txtTicketNo.Text = "";
                        txtTicketAmt.Text = "";
                        txtTicketAmt.ReadOnly = true;
                        txtTicketNo.SetFocus();
                        msgBar.Text = bOver? strMsg12 : strMsg07;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                SetControlDisable(false);
            }
        }

        /// <summary>
        /// 타사상품권 중복확인
        /// </summary>
        /// <returns></returns>
        private bool ChkDup()
        {
            //타사상품권 최대건수 확인
            if (!ChkCnt(1)) return false;

            string strTicket = txtTicketNo.Text.ToString();

            if (_basketOtherTickets != null)
            {
                foreach (BasketOtherTicket item in _basketOtherTickets)
                {
                    if (item != null)
                    {
                        if (item.OtherTicketNo == strTicket)
                        {
                            txtTicketNo.Text = "";
                            txtTicketNo.SetFocus();
                            txtTicketAmt.Text = "";
                            txtTicketAmt.ReadOnly = true;
                            msgBar.Text = strMsg05;
                            return false;
                        }
                    }
                }
            }

            if (grd != null)
            {
                for (int i = 0; i < grd.RowCount; i++)
                {
                    DataRow dr = (System.Data.DataRow)(grd.GetRow(i).ItemData);

                    if (dr["ColNum"].ToString() == strTicket)
                    {
                        txtTicketNo.Text = "";
                        txtTicketNo.SetFocus();
                        txtTicketAmt.Text = "";
                        txtTicketAmt.ReadOnly = true;
                        msgBar.Text = strMsg05;
                        return false;
                    }
                }    
            }

            return true;
        }

        /// <summary>
        /// 타사상품권 최대건수 확인
        /// </summary>
        private bool ChkCnt(int iCnt)
        {
            if (iTranOverCnt + iCnt >= 90)
            {
                txtType.Text = "";
                lblType.Text = "";
                txtType.ReadOnly = true;
                txtTicketNo.Text = "";
                txtTicketNo.ReadOnly = true;
                txtTicketAmt.Text = "";
                txtTicketAmt.ReadOnly = true;
                btnType.Enabled = false;
                msgBar.Text = strMsg04;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 권종금액 확인
        /// </summary>
        /// <param name="e"></param>
        void CheckAmt()
        {
            DataRow[] drFilter = dSTikcet.Tables[1].Select(string.Format("KD_GIFT = '{0}'", txtType.Text));

            Int32 iGetAmt = TypeHelper.ToInt32(txtGetAmt.Text.Length > 0 ? txtGetAmt.Text.ToString() : "0");
            Int32 iTotalAmt = TypeHelper.ToInt32(txtTicketTotalAmt.Text.Length > 0 ? txtTicketTotalAmt.Text.ToString() : "0");
            Int32 iPayAmt = TypeHelper.ToInt32(txtTicketAmt.Text.Length > 0 ? txtTicketAmt.Text.ToString() : "0");

            DataRow drAdd = dtAddTicket.NewRow();
            drAdd[0] = (grd.RowCount + 1).ToString();
            drAdd[1] = txtTicketNo.Text.ToString();
            drAdd[2] = string.Format("{0:#,##0}", iPayAmt);
            drAdd[3] = drFilter[0]["NM_GIFT"] != null ? drFilter[0]["NM_GIFT"].ToString() : "";
            drAdd[4] = drFilter[0]["KD_GIFT"].ToString();
            dtAddTicket.Rows.Add(drAdd);

            grd.AddRow(drAdd);
            iTranOverCnt++;

            txtTicketTotalAmt.Text = iTotalAmt + iPayAmt > 0 ? string.Format("{0}", iTotalAmt + iPayAmt) : "0";
            txtGetAmt.Text = iGetAmt - iPayAmt > 0 ? string.Format("{0}", iGetAmt - iPayAmt) : "0";

            //txtType.Text = "";
            //lblType.Text = "";
            txtTicketNo.Text = "";
            txtTicketAmt.Text = "";

            if (iGetAmt - iPayAmt <= 0)
            {
                txtType.ReadOnly = true;
                txtTicketNo.ReadOnly = true;
                txtTicketAmt.ReadOnly = true;
                btnType.Enabled = false;
                msgBar.Text = strMsg06;
            }
            else
            {
                txtType.ReadOnly = false;
                txtTicketNo.ReadOnly = false;
                txtTicketAmt.ReadOnly = true;
                btnType.Enabled = true;
                msgBar.Text = strMsg02;
                txtTicketNo.SetFocus();
            }
        }

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable/Disable
        /// </summary>
        void SetControlDisable(bool bDisable)
        {
            ChildManager.ShowProgress(bDisable);
            _bDisable = bDisable;

            if (!bDisable)
            {
                if (POSDeviceManager.Scanner != null)
                {
                    POSDeviceManager.Scanner.DataEvent += new POSDataEventHandler(Scanner_DataEvent);                                       //Scanner Event    
                }
            }
            else
            {
                if (POSDeviceManager.Scanner != null)
                {
                    POSDeviceManager.Scanner.DataEvent -= new POSDataEventHandler(Scanner_DataEvent);                                       //Scanner Event    
                }
            }

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    foreach (var item in this.ContainerPanel.Controls)
                    {
                        if (item.GetType().Name.ToString().ToLower() == "keypad")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad key = (WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad)item;
                            key.Enabled = !_bDisable;
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "inputtext")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.InputText txt = (WSWD.WmallPos.POS.FX.Win.UserControls.InputText)item;
                            txt.Enabled = !_bDisable;
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "button")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.Button btn = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)item;
                            btn.Enabled = !_bDisable;
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "gridpanel")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel grd = (WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel)item;
                            grd.Enabled = !_bDisable;
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "panel")
                        {
                            Panel pnl = (Panel)item;
                            pnl.Enabled = !_bDisable;
                        }
                    }
                });
            }
            else
            {
                foreach (var item in this.ContainerPanel.Controls)
                {
                    if (item.GetType().Name.ToString().ToLower() == "keypad")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad key = (WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad)item;
                        key.Enabled = !_bDisable;
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "inputtext")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.InputText txt = (WSWD.WmallPos.POS.FX.Win.UserControls.InputText)item;
                        txt.Enabled = !_bDisable;
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "button")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.Button btn = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)item;
                        btn.Enabled = !_bDisable;
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "gridpanel")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel grd = (WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel)item;
                        grd.Enabled = !_bDisable;
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "panel")
                    {
                        Panel pnl = (Panel)item;
                        pnl.Enabled = !_bDisable;
                    }
                }
            }

            //Application.DoEvents();
        }

        #endregion
    }
}
