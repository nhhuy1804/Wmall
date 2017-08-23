//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P013.cs
 * 화면설명 : 현금IC결제
 * 개발자   : TCL
 * 개발일자 : 2015.05.27
 * 
 * * 간소화 거래 : 취소시 고객의 카드 미지참으로 처리 할때 이용한다.
   - IC 카드 일련번호, 발급기관대표코드, 암호화정보, 트랙데이터 헝목 ALL Space
   - 간소화거래여부 '01'

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
using WSWD.WmallPos.POS.PY.Data;
using WSWD.WmallPos.POS.FX.Win.Data;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PV;

namespace WSWD.WmallPos.POS.PY.VC
{
    public partial class POS_PY_P013 : PopupBase01, IPYP013View
    {
        #region 변수

        const string MODE_NO_CARD_CANC = "02";
        const string MODE_NORMAL_TRXN = "00";
        const string MODE_SIMPLE_TRXN = "01";

        //비즈니스 로직
        private IPYP013presenter m_presenter;

        /// <summary>
        /// 대상금액
        /// </summary>
        private int m_payAmt = 0;
        private int m_taxAmt = 0;

        /// <summary>
        /// IC Card 관련받은 정보
        /// </summary>
        private string m_icCardEncData = string.Empty;
        private string m_icCardTrackII = string.Empty;
        private string m_icCardSeqNo = string.Empty;
        private string m_icCardIssuerCd = string.Empty;
        private string m_icCardIssuerPosCd = string.Empty;

        /// <summary>
        /// 반품
        /// </summary>
        private bool m_modeReturn = false;
        private bool m_modeAutoRtn = false;
        /// <summary>
        /// Loc added 10.27
        /// 첫결제수단인경우, 반품인경우, 취소가능 (닫기버튼)
        /// 
        /// </summary>
        private bool m_allowCancel = false;

        /// <summary>
        /// 원현금IC정보, 자동반품시 사용
        /// </summary>
        private BasketCashIC m_orgCashIC = null;

        /// <summary>
        /// 반품모드인지?
        /// </summary>
        private CashICPaymentState m_modeProcessing = CashICPaymentState.Ready;
        public CashICPaymentState ModeProcessing
        {
            get
            {
                return m_modeProcessing;
            }
            set
            {
                m_modeProcessing = value;
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        btnICCard.Enabled = value == CashICPaymentState.Ready;
                        btnClose.Enabled = value == CashICPaymentState.Ready && !m_modeAutoRtn;
                        //btnSimpleCancel.Enabled = value == CashICPaymentState.Ready;
                        btnCancNoCard.Enabled = value == CashICPaymentState.Ready;
                    });
                }
                else
                {
                    btnICCard.Enabled = value == CashICPaymentState.Ready;
                    btnClose.Enabled = value == CashICPaymentState.Ready && !m_modeAutoRtn;
                    //btnSimpleCancel.Enabled = value == CashICPaymentState.Ready;
                    btnCancNoCard.Enabled = value == CashICPaymentState.Ready;
                }
            }
        }

        #endregion

        #region 생성자

        /// <summary>
        /// 현금IC
        /// </summary>
        /// <param name="payAmt">대상금액</param>
        /// <param name="taxAmt">세금액</param>
        /// <param name="modeReturn">반품여부</param>
        /// <param name="orgApprNo">원거래번호</param>
        /// <param name="orgAppDate">원거래일자</param>
        public POS_PY_P013(int payAmt, int taxAmt, bool modeReturn, BasketCashIC orgCashIC, bool allowCancel)
        {
            InitializeComponent();

            //대상금액
            m_payAmt = payAmt;
            m_taxAmt = taxAmt;
            m_modeReturn = modeReturn;
            m_allowCancel = allowCancel;

            lblOrgApprDate.Visible = lblOrgApprNo.Visible =
                txtOrgApprDate.Visible = txtOrgApprNo.Visible = modeReturn;
            lblYmd.Visible = modeReturn;

            m_orgCashIC = orgCashIC;
            txtOrgApprDate.Text = m_orgCashIC != null ? m_orgCashIC.RealApprProcDate : string.Empty;
            txtOrgApprNo.Text = m_orgCashIC != null ? m_orgCashIC.ApprNo : string.Empty;
            m_modeAutoRtn = m_orgCashIC != null ?
                !string.IsNullOrEmpty(m_orgCashIC.ApprNo) && !string.IsNullOrEmpty(m_orgCashIC.RealApprProcDate) :
                false;

            txtOrgApprNo.ReadOnly = m_modeAutoRtn;
            txtOrgApprNo.Tag = 12; // min length 0981400352960
            txtOrgApprDate.ReadOnly = m_modeAutoRtn;
            txtOrgApprDate.Tag = 8; // min length

            //2015.09.18 정광호 수정-----------------------------
            // 간소화취소 사용안함
            //btnSimpleCancel.Visible = modeReturn;
            //btnSimpleCancel.Left = 183;
            //btnClose.Left = modeReturn ? 286 : 234;
            btnSimpleCancel.Visible = false;
            //---------------------------------------------------

            // Loc added 10.26
            // 무카드 취소
            btnCancNoCard.Visible = m_modeAutoRtn;

            // Allow to close popup or not
            btnClose.Enabled = m_allowCancel; // 자동반품시 닫기 비활성화
            this.Text = this.Text + (modeReturn ? (m_modeAutoRtn ? TITLE_AUTORTN : TITLE_MANURTN) : string.Empty);

            if (modeReturn && !m_modeAutoRtn)
            {
                txtOrgApprDate.SetFocus();
            }

            //대상금액
            txtPayAmt.Text = m_payAmt.ToString();

            //정보 조회
            m_presenter = new PYP013presenter(this);

            InitEvent();
        }

        #endregion

        #region 이벤트 등록

        /// <summary>
        /// Form Event 정의
        /// </summary>
        private void InitEvent()
        {
            this.btnICCard.Click += new EventHandler(btnICCard_Click);              //현금IC카드 정보 button Event
            this.btnSimpleCancel.Click += new EventHandler(btnSimpleCancel_Click);
            this.btnCancNoCard.Click += new EventHandler(btnCancNoCard_Click);
            this.btnClose.Click += new EventHandler(btnClose_Click);            //닫기 button Event
            this.KeyEvent += new OPOSKeyEventHandler(POS_PY_P013_KeyEvent);
            this.FormClosed += new FormClosedEventHandler(POS_PY_P013_FormClosed);

            if (m_modeReturn && !m_modeAutoRtn)
            {
                txtOrgApprDate.InputFocused += new EventHandler(txtOrgApprDate_InputFocused);
                txtOrgApprNo.InputFocused += new EventHandler(txtOrgApprNo_InputFocused);
            }
        }

        void POS_PY_P013_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.btnICCard.Click -= new EventHandler(btnICCard_Click);              //현금IC카드 정보 button Event
            this.btnSimpleCancel.Click -= new EventHandler(btnSimpleCancel_Click);
            this.btnCancNoCard.Click -= new EventHandler(btnCancNoCard_Click);
            this.btnClose.Click -= new EventHandler(btnClose_Click);            //닫기 button Event
            this.KeyEvent -= new OPOSKeyEventHandler(POS_PY_P013_KeyEvent);
            this.FormClosed -= new FormClosedEventHandler(POS_PY_P013_FormClosed);

            if (m_modeReturn && !m_modeAutoRtn)
            {
                txtOrgApprDate.InputFocused -= new EventHandler(txtOrgApprDate_InputFocused);
                txtOrgApprNo.InputFocused -= new EventHandler(txtOrgApprNo_InputFocused);
            }
        }

        #endregion

        #region 이벤트 정의

        void POS_PY_P013_KeyEvent(OPOSKeyEventArgs e)
        {
            if (ModeProcessing == CashICPaymentState.Processing)
            {
                e.IsHandled = true;
                return;
            }

            if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR ||
                e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
            {
                ValidateOnKeyEvent(e);
            }
        }

        /// <summary>
        /// 현금IC카드 정보 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnICCard_Click(object sender, EventArgs e)
        {
            ResetInputs();
            RequestRandNumber();
        }

        /// <summary>
        /// 간소화거래
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSimpleCancel_Click(object sender, EventArgs e)
        {
            RequestNoICCardPay();
        }

        /// <summary>
        /// 무카드취소
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCancNoCard_Click(object sender, EventArgs e)
        {
            RequestNoICCardPayReturn();
        }

        /// <summary>
        /// 닫기 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        void txtOrgApprNo_InputFocused(object sender, EventArgs e)
        {
            StatusMessage = MSG_ENTER_ORG_APPR_NO;
        }

        void txtOrgApprDate_InputFocused(object sender, EventArgs e)
        {
            StatusMessage = MSG_ENTER_ORG_APPR_DATE;
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// Reset inputs
        /// </summary>
        void ResetInputs()
        {
            txtCardNo.Text = m_icCardEncData = m_icCardIssuerCd = m_icCardIssuerPosCd = m_icCardSeqNo = m_icCardTrackII = string.Empty;
            StatusMessage = string.Empty;
        }

        /// <summary>
        /// 난수가져오기
        /// </summary>
        void RequestRandNumber()
        {
            // 처리중
            ModeProcessing = CashICPaymentState.Processing;

            // VAN승인 - 난수 가져오기
            // m_presenter.ProcessGetRanNum(m_payAmt.ToString(), m_taxAmt.ToString());

            // 임시테스트 - Random 난수
            string randNum = MakeRandomNumber();
            RequestICCardReader(randNum);
        }

        /// <summary>
        /// 난수받아서 처리
        /// </summary>
        /// <param name="randNum"></param>
        void RequestICCardReader(string randNum)
        {
            TraceHelper.Instance.TraceWrite("RequestICCardReader", "난수: -{0}-", randNum);

            // request to receice ic card no
            using (var pop = ChildManager.ShowPopup(string.Empty,
                "WSWD.WmallPos.POS.PY.dll", "WSWD.WmallPos.POS.PY.VC.POS_PY_P004", randNum))
            {
                var res = pop.ShowDialog(this);

                if (res == DialogResult.OK)
                {
                    m_icCardTrackII = pop.ReturnResult["TRACKII"].ToString();
                    m_icCardSeqNo = pop.ReturnResult["IC_CARD_SEQ_NO"].ToString();
                    m_icCardIssuerCd = pop.ReturnResult["ISSUER_CD"].ToString();
                    m_icCardIssuerPosCd = pop.ReturnResult["ISSUER_POS_NO"].ToString();
                    m_icCardEncData = pop.ReturnResult["ENC_DATA"].ToString();
                    txtCardNo.Text = m_icCardSeqNo;

                    if (ValidatePayment(false, false))
                    {
                        ProcessPayment(MODE_NORMAL_TRXN);
                        return;
                    }
                }
            }

            ModeProcessing = CashICPaymentState.Ready;
        }

        /// <summary>
        /// 간소화취소
        /// </summary>
        void RequestNoICCardPay()
        {
            if (!ValidatePayment(false, true))
            {
                return;
            }

            ProcessPayment(MODE_SIMPLE_TRXN);
        }

        /// <summary>
        /// 무카드취소
        /// </summary>
        void RequestNoICCardPayReturn()
        {
            txtCardNo.Text = m_orgCashIC != null ? m_orgCashIC.ICCardSeqNo : string.Empty;
            m_icCardSeqNo = txtCardNo.Text;
            if (!ValidatePayment(false, true))
            {
                return;
            }

            ProcessPayment(MODE_NO_CARD_CANC);
        }

        /// <summary>
        /// 
        /// </summary>
        void ProcessPayment(string simpleTrxnMode)
        {
            ShowProgressMessage(true);

            // VAN승인
            m_presenter.ProcessVANCashIC(!m_modeReturn, true,
                simpleTrxnMode,
                m_icCardSeqNo, m_icCardIssuerCd, m_icCardIssuerPosCd, m_icCardEncData, m_icCardTrackII,
                m_payAmt.ToString(), m_taxAmt.ToString(), txtOrgApprDate.Text, txtOrgApprNo.Text);

        }

        /// <summary>
        /// 결제진행 가능여부확인
        /// </summary>
        /// <returns></returns>
        bool ValidatePayment(bool onEnter, bool modeNoCard)
        {
            if (m_modeReturn)
            {
                if (txtOrgApprDate.Text.Length != 8 || !DateTimeUtils.ValidateOrgApprDateSystem(txtOrgApprDate.Text))
                {
                    txtOrgApprDate.Text = "";
                    txtOrgApprDate.SetFocus();
                    return false;
                }

                if (txtOrgApprNo.Text.Length < 6)
                {
                    txtOrgApprNo.Text = "";
                    txtOrgApprNo.SetFocus();
                    return false;
                }
            }

            // 무카드인경우 카드번호만 확인한다
            bool invalid = (modeNoCard ? false :
                            string.IsNullOrEmpty(m_icCardIssuerCd) ||
                            string.IsNullOrEmpty(m_icCardEncData) ||
                            string.IsNullOrEmpty(m_icCardTrackII))
                || string.IsNullOrEmpty(m_icCardSeqNo);

            if (invalid)
            {
                StatusMessage = MSG_ENTER_IC_CARD;
                return false;
            }

            if (m_modeAutoRtn || !m_modeReturn)
            {
                return true;
            }

            return onEnter ? txtOrgApprNo.IsFocused : true;
        }

        /// <summary>
        /// 초기상태?
        /// </summary>
        /// <returns></returns>
        bool IsInitialState()
        {
            // 자동반품
            // 수동반품 & 정상일때 텍스트 없을때
            return m_modeAutoRtn ? true : txtOrgApprDate.Text.Length == 0 && txtOrgApprNo.Text.Length == 0;
        }

        /// <summary>
        /// PROGRESS MESSAGE
        /// </summary>
        /// <param name="showProgress"></param>
        public void ShowProgressMessage(bool showProgress)
        {
            ChildManager.ShowProgress(showProgress, MSG_VAN_REQ_PROCESSING);
        }

        /// <summary>
        /// ERROR MESSAGE
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="errorMessage"></param>
        /// <param name="errorCode"></param>
        public void ShowErrorMessage(VANRequestErrorType errorType, string errorMessage, string errorCode)
        {
            ShowProgressMessage(false);

            ModeProcessing = CashICPaymentState.Errored;
            string message = string.Empty;
            switch (errorType)
            {
                case VANRequestErrorType.None:
                    break;
                case VANRequestErrorType.CommError:
                    message = MSG_VAN_REQ_COMM_ERROR;
                    message += string.Format("{0}{1}", string.IsNullOrEmpty(errorCode) ?
                        string.Empty : "[" + errorCode + "] ",
                        errorMessage);
                    break;
                case VANRequestErrorType.NoInfoFound:
                case VANRequestErrorType.SomeError:
                    message = string.Format("{0}{1}",
                        string.IsNullOrEmpty(errorCode) ? string.Empty : "[" + errorCode + "] ",
                        errorMessage);
                    break;
                default:
                    break;
            }


            StatusMessage = message;

            #region 자동반품시 통신오류, 강제진행, 강제취소

            if ((errorType == VANRequestErrorType.CommError ||
                errorType == VANRequestErrorType.SomeError) && m_modeReturn && m_modeAutoRtn)
            {
                // 자동반품시, 통신오류, 재시도이나 강제진행
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(MSG_VAN_REQ_COMM_ERROR);
                sb.AppendLine(MSG_VAN_REQ_COMM_ERROR_RETRY);
                sb.Append(Environment.NewLine);
                sb.AppendLine("[ERROR]");
                sb.AppendFormat("{0}{1}", string.IsNullOrEmpty(errorCode) ?
                        string.Empty : "[" + errorCode + "] ", errorMessage);

                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        var res = ShowMessageBox(MessageDialogType.Question, string.Empty, sb.ToString(),
                            new string[] {
                            LABEL_RETRY, LABEL_FORCE
                        });

                        // 강제진행
                        // Basket 생성 & 닫기
                        if (res == DialogResult.No)
                        {
                            OnEndPayment(null, errorCode, errorMessage);
                        }
                    });
                }
                else
                {
                    var res = ShowMessageBox(MessageDialogType.Question, string.Empty, sb.ToString(),
                            new string[] {
                            LABEL_RETRY, LABEL_FORCE
                        });

                    // 강제진행
                    // Basket 생성 & 닫기
                    if (res == DialogResult.No)
                    {
                        OnEndPayment(null, errorCode, errorMessage);
                    }
                }
            }

            #endregion

            ModeProcessing = CashICPaymentState.Ready;
        }

        /// <summary>
        /// VAN 승인결와 성공
        /// </summary>
        /// <param name="respData"></param>
        public void OnReturnSuccess(PV04RespData respData)
        {
            ShowProgressMessage(false);
            if (PV04RespData.REQ_RAND_NUM.Equals(respData.TrxnType))
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        RequestICCardReader(respData.NoticeMessage2);
                    });
                }
                else
                {
                    RequestICCardReader(respData.NoticeMessage2);
                }
                return;
            }

            StatusMessage = string.Format("[{0}] {1}", respData.ApprNo, respData.RespMessage1);
            OnEndPayment(respData, null, null);
        }

        /// <summary>
        /// 난수 32글자, hexa
        /// </summary>
        /// <returns></returns>
        private string MakeRandomNumber()
        {
            StringBuilder sb = new StringBuilder();
            var rand = new Random();
            for (int i = 0; i < 32; i++)
            {
                int rn = rand.Next(15);
                sb.Append(rn.ToString("X"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 결제완료이나 통신실패시, BASKET생성 및 완료
        /// </summary>
        /// <param name="respData"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        void OnEndPayment(PV04RespData respData, string errorCode, string errorMessage)
        {
            BasketCashIC basket = null;
            if (m_orgCashIC != null)
            {
                basket = (BasketCashIC)(BasketCashIC.Parse(typeof(BasketCashIC), m_orgCashIC.ToString()));
                basket.ForceCancFg = respData != null ? string.Empty : "1";
                basket.CancRcvCode = respData != null ? string.Empty : errorCode;
                basket.CancRcvMsg = respData != null ? string.Empty : errorMessage;
            }
            else
            {
                basket = new BasketCashIC()
               {
                   CancFg = "0",
                   InputType = "C",
                   ApprAmtIncVat = m_taxAmt.ToString(),
                   PayAmt = m_payAmt.ToString(),
               };
            }

            #region 공통속성
            basket.OTApprNo = txtOrgApprNo.Text;
            basket.OTSaleDate = txtOrgApprDate.Text;
            #endregion

            #region 승인받을때
            if (respData != null)
            {
                basket.CardNm = respData.CardName;
                basket.CashICAccountNo = respData.PayAcctNo;
                basket.IssueComCd = respData.IssuerCode;
                basket.IssueComNm = respData.IssuerName;
                basket.MaeipComCd = respData.MaeipCode;
                basket.MaeipComNm = respData.MaeipName;
                basket.VanID = respData.ApprVanCode;
                basket.ICCardSeqNo = m_icCardSeqNo;
            }

            #endregion

            basket.ApprNo = respData != null ? respData.ApprNo : string.Empty;
            basket.RealApprProcDate = respData != null ? respData.ApprDate : DateTime.Today.ToString("yyyyMMdd");
            basket.RealApprProcTime = respData != null ? respData.ApprTime : DateTime.Now.ToString("HHmmss");

            this.ReturnResult.Add("PAY_DATA", basket);
            if (!string.IsNullOrEmpty(errorCode) || !string.IsNullOrEmpty(errorMessage))
            {
                this.ReturnResult.Add("ERROR_CODE", errorCode);
                this.ReturnResult.Add("ERROR_MSG", errorMessage);
            }
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// KEY ENTER AND CLEAR
        /// </summary>
        /// <param name="e"></param>
        void ValidateOnKeyEvent(OPOSKeyEventArgs e)
        {
            InputText it = null;
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
            {
                if (ValidatePayment(true, false))
                {
                    e.IsHandled = true;
                    ProcessPayment(MODE_NORMAL_TRXN);
                    return;
                }

                if (this.FocusedControl != null)
                {
                    it = (InputText)this.FocusedControl;
                    int minLen = (int)it.Tag;
                    if (it.Text.Length < minLen)
                    {
                        e.IsHandled = true;
                        return;
                    }

                    this.NextControl();
                }

                return;
            }

            if (ModeProcessing == CashICPaymentState.Errored)
            {
                ModeProcessing = CashICPaymentState.Ready;
                e.IsHandled = true;
                return;
            }

            var initState = IsInitialState();
            if (initState)
            {
                ModeProcessing = CashICPaymentState.Ready;
            }

            if (this.FocusedControl == null || initState)
            {
                if (btnClose.Enabled)
                {
                    e.IsHandled = true;
                    btnClose_Click(btnClose, EventArgs.Empty);
                }

                return;
            }

            it = (InputText)this.FocusedControl;
            if (it.Text.Length > 0)
            {
                return;
            }

            this.PreviousControl();
        }

        #endregion
    }

    public enum CashICPaymentState
    {
        Ready,
        Processing,
        Errored
    }
}