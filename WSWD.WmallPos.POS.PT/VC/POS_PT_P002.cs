//-----------------------------------------------------------------
/*
 * 화면명   : POS_PT_P002.cs
 * 화면설명 : 포인트 적립
 * 개발자   : 정광호
 * 개발일자 : 2015.04.
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

using WSWD.WmallPos.POS.PT.PI;
using WSWD.WmallPos.POS.PT.PT;
using WSWD.WmallPos.POS.PT.VI;
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

namespace WSWD.WmallPos.POS.PT.VC
{
    public partial class POS_PT_P002 : PopupBase
    {
        #region 변수

        /// <summary>
        /// 회원정보
        /// </summary>
        private PP01RespData _cust = null;

        /// <summary>
        /// 결제 헤더정보
        /// </summary>
        private BasketHeader _BasketHeader = null;

        /// <summary>
        /// 결제 결제내역
        /// </summary>
        private List<BasketPay> _BasketPays = null;

        /// <summary>
        /// 결제 소계정보
        /// </summary>
        private BasketSubTotal _BasketSubTtl = null;

        /// <summary>
        /// 포인트적립정보
        /// </summary>
        private BasketPointSave _BasketPointSave = null;

        /// <summary>
        /// 프로모션 정보
        /// </summary>
        private Dictionary<string, object> _dicPromoPoint = null;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        BackgroundWorker bw = null;

        #endregion

        #region 생성자

        public POS_PT_P002(PP01RespData cust, BasketHeader BasketHeader, List<BasketPay> BasketPays, BasketSubTotal BasketSubTtl, BasketPointSave BasketPointSave, Dictionary<string, object> dicPromoPoint)
        {
            InitializeComponent();

            //회원정보
            _cust = cust;

            //결제 헤더정보`
            _BasketHeader = BasketHeader;

            //결제 결제내역
            _BasketPays = BasketPays;

            //결제 소계정보
            _BasketSubTtl = BasketSubTtl;

            if (BasketPointSave != null)
            {
                _BasketPointSave = BasketPointSave;
            }

            //프로모션정보
            if (dicPromoPoint != null && dicPromoPoint.Count > 0)
            {
                _dicPromoPoint = dicPromoPoint;
            }

            //Form Load Event
            this.Load += new EventHandler(form_Load);
            this.FormClosed += new FormClosedEventHandler(POS_PT_P002_FormClosed);
        }

        void POS_PT_P002_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Load -= new EventHandler(form_Load);
            this.FormClosed -= new FormClosedEventHandler(POS_PT_P002_FormClosed);
            this.KeyEvent -= new OPOSKeyEventHandler(form_KeyEvent);                                        //Key Event
            this.btnRetry.Click -= new EventHandler(btnRetry_Click);                                        //재시도 button Event
            this.btnClose.Click -= new EventHandler(btnClose_Click);                                        //닫기 button Event
            this.bw.DoWork -= new DoWorkEventHandler(bw_DoWork);
        }

        #endregion

        #region 이벤트 등록

        /// <summary>
        /// Form Event 정의
        /// </summary>
        private void InitEvent()
        {
            this.KeyEvent += new OPOSKeyEventHandler(form_KeyEvent);                                        //Key Event
            this.btnRetry.Click += new EventHandler(btnRetry_Click);                                        //재시도 button Event
            this.btnClose.Click += new EventHandler(btnClose_Click);                                        //닫기 button Event
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

            if (_cust != null && _cust.Length > 0)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        txtCardNo.Text = _cust.CardNo;      //카드번호
                        txtCustName.Text = _cust.CustName;  //회원명
                        msgBar.Text = strMsg01.ToString();
                    });
                }
                else
                {
                    txtCardNo.Text = _cust.CardNo;      //카드번호
                    txtCustName.Text = _cust.CustName;  //회원명
                    msgBar.Text = strMsg01.ToString();
                }
            }

            //백그라운드 이벤트
            bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            btnRetry_Click(null, null);
        }

        /// <summary>
        /// Key Event
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

            if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
            {
                e.IsHandled = true;
                btnClose_Click(btnClose, null);
            }

            if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
            {
                e.IsHandled = true;
                btnRetry_Click(btnRetry, null);
            }
        }

        /// <summary>
        /// BackgroundWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            //포인트 적립 전문통신
            GetServerRegister();
        }

        /// <summary>
        /// 재시도 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnRetry_Click(object sender, EventArgs e)
        {
            //포인트 적립 전문통신
            bw.RunWorkerAsync();
        }

        /// <summary>
        /// 닫기 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;

            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// 포인트 적립 전문통신
        /// </summary>
        private void GetServerRegister()
        {
            if (_bDisable) return;

            ChildManager.ShowProgress(true);
            SetControlDisable(true);

            try
            {
                Int64 iPayCashAmt = 0;
                Int64 iPayCardAmt = 0;
                //Int64 iPayEtcAmt = 0;
                //Int64 iEventPayCashAmt = 0;
                //Int64 iEventPayCardAmt = 0;
                //Int64 iEventPayEtcAmt = 0;
                Int64 iPayAmt = 0;                    //합계
                Int64 iBalAmt = 0;                    //잔액
                Int64 iSumPayAmt = 0;
                Int64 iSumBalAmt = 0;

                foreach (var item in _BasketPays)
                {
                    BasketPay bp = (BasketPay)item;

                    if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                    {
                        iPayAmt = (bp.PayAmt != null ? TypeHelper.ToInt64(bp.PayAmt) : 0);
                        iBalAmt = (bp.BalAmt != null ? TypeHelper.ToInt64(bp.BalAmt) : 0);
                        iSumPayAmt += iPayAmt;
                        iSumBalAmt += iBalAmt;

                        if (iPayAmt > 0)
                        {
                            if ((bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CASH &&
                                bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CASH) || //현금
                                (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CASH &&
                                bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CASH) || //수표
                                (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_TKCKET &&
                                bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_TICKET_OTHER)) // 타사상품권
                            {
                                iPayCashAmt += (iPayAmt - iBalAmt);
                            }
                            else if ((bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD &&
                                    bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CARD) || //신용카드
                                    (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD &&
                                    bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CARD_OTHER) || //타건카드
                                    (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD &&
                                    bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CARD_WELFARE) || //타건복지
                                    (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD &&
                                    bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CASH_IC)) //현금IC
                            {
                                iPayCardAmt += (iPayAmt - iBalAmt);
                            }
                        }
                    }
                }

                if (_cust != null && _cust.Length > 0)
                {
                    //ChildManager.ShowProgress(true);

                    //취소구분(0:정상 / 1:수동반품 / 2:자동반품)
                    string strCancType = string.Empty;

                    //원거래정보(반품시 적용, 매출일(8)+점(4)+포스(4)+거래번호(6))
                    string strOTApprInfo = "";

                    //원승인번호(반품시 적용)
                    string strOTApprNo = "";

                    if (_BasketHeader.CancType == null || _BasketHeader.CancType.ToString().Length <= 0 || _BasketHeader.CancType == "0")
                    {
                        //정상
                        strCancType = "0";
                    }
                    else
                    {
                        if ((_BasketHeader.OTSaleDate != null && _BasketHeader.OTSaleDate.Length >= 0) ||
                            (_BasketHeader.OTStoreNo != null && _BasketHeader.OTStoreNo.Length >= 0) ||
                            (_BasketHeader.OTPosNo != null && _BasketHeader.OTPosNo.Length >= 0) ||
                            (_BasketHeader.OTTrxnNo != null && _BasketHeader.OTTrxnNo.Length >= 0) ||
                            (_BasketHeader.OTCasNo != null && _BasketHeader.OTCasNo.Length >= 0))
                        {
                            //자동반품
                            strCancType = "2";
                            strOTApprInfo = _BasketHeader.OTSaleDate + _BasketHeader.OTStoreNo + _BasketHeader.OTPosNo + _BasketHeader.OTTrxnNo;
                            strOTApprNo = _BasketPointSave.NoAppr;
                        }
                        else
                        {
                            //수동반품
                            strCancType = "1";
                        }
                    }

                    //회원번호
                    string strCustNo = _cust.CustNo;

                    //카드번호
                    string strCardNo = _cust.CardNo;

                    //현금 계열 결제 금액
                    string strPayCashAmt = iPayCashAmt.ToString();

                    //카드 계열 결제 금액
                    string strPayCardAmt = iPayCardAmt.ToString();

                    //기타 결제 금액
                    string strPayEtcAmt = ((iSumPayAmt - iSumBalAmt) - (iPayCashAmt + iPayCardAmt)).ToString();

                    //포인트 적립 행사 적용 정보(점포코드(4) + 프로모션_년(4) + 프로모션_월(2) + 프로모션_주차(2) + 프로모션순번(3) + 프로모션허들순번(3))
                    string strPointEventInfo = _dicPromoPoint != null ? TypeHelper.ToString(_dicPromoPoint["PointEventInfo"]) : "";

                    //행사 적용 대상 현금 계열 결제 금액
                    string strEventPayCashAmt = _dicPromoPoint != null ? TypeHelper.ToString(_dicPromoPoint["EventPayCashAmt"]) : "";

                    //행사 적용 대상 카드 계열 결제 금액
                    string strEventPayCardAmt = _dicPromoPoint != null ? TypeHelper.ToString(_dicPromoPoint["EventPayCardAmt"]) : "";

                    //행사 적용 대상 기타 결제 금액
                    string strEventPayEtcAmt = _dicPromoPoint != null ? TypeHelper.ToString(_dicPromoPoint["EventPayEtcAmt"]) : "";

                    var pp03 = new PP03DataTask(strCancType, strOTApprInfo, strOTApprNo,
                        strCustNo, strCardNo, strPayCashAmt, strPayCardAmt, strPayEtcAmt,
                        strPointEventInfo, strEventPayCashAmt, strEventPayCardAmt, strEventPayEtcAmt); //TEST ->"2701900057818"
                    pp03.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pp03_TaskCompleted);
                    pp03.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pp03_Errored);
                    pp03.ExecuteTask();
                }
                else
                {
                    ChildManager.ShowProgress(false);
                    SetControlDisable(false);
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                ChildManager.ShowProgress(false);
                SetControlDisable(false);
            }
        }

        /// <summary>
        /// 포인트 적립 전문통신 완료 이벤트
        /// </summary>
        /// <param name="responseData"></param>
        void pp03_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            ChildManager.ShowProgress(false);

            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                var data = responseData.DataRecords.ToDataRecords<PP03RespData>();
                if (data.Length > 0)
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate()
                        {
                            BasketPointSave bp = new BasketPointSave();
                            bp.BasketType = BasketTypes.BasketPointSave;//구분자
                            bp.FgProgRes = "1";                         //포인트처리결과값
                            bp.NoCard = data[0].CardNo;                 //카드번호
                            bp.PointNmMember = data[0].CustName;        //포인트회원명
                            bp.AmPoint = data[0].IssuePoint;            //발생포인트
                            bp.AmMarkNotDay = data[0].AnniversaryPoint; //기념일적용점수
                            bp.AmMarkEvt = data[0].EventPoint;          //행사적용점수
                            //bp.AmPointAdd3 = data[0].AnniversaryPoint;  //추가포인트3
                            bp.AmPointUsable = data[0].AbtyPoint;       //가용점수
                            bp.AmPointAccu = data[0].CltePoint;         //누적점수
                            bp.AmPointDelay = data[0].DelayPoint;       //유예점수
                            bp.CustGrade = data[0].GradeCode;           //고객등급
                            bp.CustGradeNm = data[0].GradeName;         //고객등급명
                            bp.Remark = data[0].Remark;                 //비고
                            bp.NoAppr = data[0].ApprNo;                 //승인번호
                            bp.NoPointMember = data[0].CustNo;          //포인트회원번호
                            //bp.ClassMember = data[0].;                //회원종류
                            bp.InputWcc = _cust.InputWcc;               //입력형태
                            bp.PointEvtCode = data[0].PointEventCode;   //적용된 포인트 적립 행사 코드
                            bp.PointEvtName = data[0].PointEventName;   //적용된 포인트 적립 행사 명

                            this.ReturnResult.Clear();
                            this.ReturnResult.Add("POINT_DATA", bp);
                            this.DialogResult = DialogResult.OK;
                            SetControlDisable(false);
                        });
                    }
                    else
                    {
                        BasketPointSave bp = new BasketPointSave();
                        bp.BasketType = BasketTypes.BasketPointSave;//구분자
                        bp.FgProgRes = "1";                         //포인트처리결과값
                        bp.NoCard = data[0].CardNo;                 //카드번호
                        bp.PointNmMember = data[0].CustName;        //포인트회원명
                        bp.AmPoint = data[0].IssuePoint;            //발생포인트
                        bp.AmMarkNotDay = data[0].AnniversaryPoint; //기념일적용점수
                        bp.AmMarkEvt = data[0].EventPoint;          //행사적용점수
                        //bp.AmPointAdd3 = data[0].;                //추가포인트3
                        bp.AmPointUsable = data[0].AbtyPoint;       //가용점수
                        bp.AmPointAccu = data[0].CltePoint;         //누적점수
                        bp.AmPointDelay = data[0].DelayPoint;       //유예점수
                        bp.CustGrade = data[0].GradeCode;           //고객등급
                        bp.CustGradeNm = data[0].GradeName;         //고객등급명
                        bp.Remark = data[0].Remark;                 //비고
                        bp.NoAppr = data[0].ApprNo;                 //승인번호
                        bp.NoPointMember = data[0].CustNo;          //포인트회원번호
                        //bp.ClassMember = data[0].;                //회원종류
                        bp.InputWcc = _cust.InputWcc;               //입력형태
                        bp.PointEvtCode = data[0].PointEventCode;   //적용된 포인트 적립 행사 코드
                        bp.PointEvtName = data[0].PointEventName;   //적용된 포인트 적립 행사 명

                        this.ReturnResult.Clear();
                        this.ReturnResult.Add("POINT_DATA", bp);
                        this.DialogResult = DialogResult.OK;
                        SetControlDisable(false);
                    }
                }
            }
            else if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.NoInfo)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        txtCardNo.Text = "";
                        msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false);
                    });
                }
                else
                {
                    txtCardNo.Text = "";
                    msgBar.Text = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false);
                }
            }
            else
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false);
                    });
                }
                else
                {
                    msgBar.Text = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false);
                }
            }
        }

        /// <summary>
        /// 포인트 적립 에러 이벤트
        /// </summary>
        /// <param name="responseData"></param>
        void pp03_Errored(string errorMessage, Exception lastException)
        {
            ChildManager.ShowProgress(false);

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    msgBar.Text = strMsg03;
                    SetControlDisable(false);
                });
            }
            else
            {
                msgBar.Text = strMsg03;
                SetControlDisable(false);
            }
        }

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable/Disable
        /// </summary>
        void SetControlDisable(bool bDisable)
        {


            _bDisable = bDisable;

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    //TraceHelper.Instance.JournalWrite("포인트적립테스트", bDisable.ToString());
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
                    }
                });
            }
            else
            {
                //TraceHelper.Instance.JournalWrite("포인트적립테스트", bDisable.ToString());
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
                }
            }

            //Application.DoEvents();
        }

        #endregion
    }
}
