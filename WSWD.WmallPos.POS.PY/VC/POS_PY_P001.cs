//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P001.cs
 * 화면설명 : 신용카드결제
 * 개발자   : TCL
 * 개발일자 : 2015.05.15
 * 
 * 수정사항
 *  1) 11.21
 *      신용카드 승인 처리 하는 POPUP 창에서 카드 SWIP 하거나 AppCard Scan한 후 카드번호 입력 항목을 사용자가 클릭하거나 화살표로 항목 이동 해서 
        Focus 이동 시킨 다음 번호만 수정되지 않게 (추가입력이나 번호 삭제) 막아야 합니다.

        CLEAR 기능 : 유지
        <- 기능 : 번호 삭제 되므로 막아야 함
        화살표 기능 : 유지
        둥록 기능 : 유지

        단,  번호 KeyIN 상태는 지금처럼 추가 입력 가능하게 한다.

 * 여전법 추가 0620
 * btnForceClose 추가
 *  - 강제취소 (자동반품시)
 *  - 자동밥품 시작하게 되면 첫결제겨거래만 취소 (진행안함)가능히며, 2번째 결제취소 시 btnForceClose표시
 * 
 * 
 * 06/20
 * btnReadIC, btnRdKEYIN 추가
 * 
 * 07/29
 * - keyin 적용
 * - key input 불가하게 (카드번호, 유효기긴 박스에)
*/
//-----------------------------------------------------------------

using System;
using System.Windows.Forms;

using WSWD.WmallPos.POS.PY.PI;
using WSWD.WmallPos.POS.PY.PT;
using WSWD.WmallPos.POS.PY.VI;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.POS.PY.Data;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PV;
using WSWD.WmallPos.POS.FX.Win.Data;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PV;
using System.Text;
using System.Diagnostics;
using WSWD.WmallPos.FX.Shared.Helper;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;

namespace WSWD.WmallPos.POS.PY.VC
{
    public partial class POS_PY_P001 : PopupBase01, IPYP001View
    {
        #region 변수 & 속성

        /// <summary>
        /// 카드번호 최소 길이
        /// </summary>
        private const int MIN_CARD_NO_LEN = 14;
        private const int MAX_CARD_NO_LEN = 19;

        //비즈니스 로직
        private IPYP001Presenter m_presenter;

        //KSK_20170403
        private int m_itempKEY = 1;

        /// <summary>
        /// 싸인데이타
        /// </summary>
        public string m_signData = string.Empty;
        private string m_tempSignFileName = string.Empty;

        private string _track2Data = string.Empty;
        private string Track2Data
        {
            get
            {
                string trackII = string.IsNullOrEmpty(_track2Data) ?
                    txtCardNo.Text + "=" + CardExpDate : _track2Data;
                return string.Copy(trackII);
            }
            set
            {
                _track2Data = value;
            }
        }

        private string CardNo
        {
            get
            {
                return string.Copy(txtCardNo.Text);
                // return txtCardNo.Text;
            }
        }

        private string CardExpDate
        {
            get
            {
                string ym = txtExpiryDate.Text;
                if (ym.Length == 4)
                {
                    ym = ym.Substring(2) + ym.Substring(0, 2);
                }
                return this.CardMode == PayCardMode.AppCard ? EXP_YM_APP_CARD : ym;
            }
        }

        /// <summary>
        /// 대상금액
        /// </summary>
        private int m_payAmt = 0;
        private int m_taxAmt = 0;
        private bool m_keyInMode = false;
        private VANRequestErrorType m_lastError = VANRequestErrorType.None;

        /// <summary>
        /// 은련카드비밀번호
        /// </summary>
        private string m_erCardPin = string.Empty;
        private string m_erCardWorkIndex = string.Empty;

        /// <summary>
        /// 받는 DCC 정보
        /// 
        /// 여전법 변경 05.27
        /// 
        /// PV21ReqData -> PV21ReqData
        /// </summary>
        private PV21ReqData dccReqData = null;

        // 반품모드
        private bool _modeReturn = false;
        bool ModeReturn
        {
            get
            {
                return _modeReturn;
            }
            set
            {
                _modeReturn = value;
                label8.Visible = label6.Visible = txtOTApprNo.Visible = txtOTApprDate.Visible = value;
                lblyMd.Visible = value;

                txtOTApprNo.ReadOnly = m_cardBasketOrg != null;
                txtOTApprDate.ReadOnly = m_cardBasketOrg != null;
                txtInstallment.ReadOnly = m_cardBasketOrg != null;

                if (value)
                {
                    this.Text = this.Text + (m_cardBasketOrg != null ? TITLE_AUTORTN : TITLE_MANURTN);
                }
            }
        }

        /// <summary>
        /// 자동반품
        /// </summary>
        private bool m_modeAutoRtn = false;

        /// <summary>
        /// 데이타 변경여부
        /// </summary>
        private bool m_dataChanged = false;
        public bool DataChanged
        {
            get
            {
                return m_dataChanged;
            }
            set
            {
                m_dataChanged = value;
                btnERCard.Enabled = btnTelSave.Enabled = !value && CardMode == m_initialCardMode;
            }
        }

        /// <summary>
        /// 처리중인?
        /// </summary>        
        private PaymentState _modeProcessing = PaymentState.Ready;
        private PaymentState ModeProcessing
        {
            get
            {
                return _modeProcessing;
            }
            set
            {
                _modeProcessing = value;

                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        UIStateByPaymentState(_modeProcessing);
                    });
                }
                else
                {
                    UIStateByPaymentState(_modeProcessing);
                }
            }
        }

        /// <summary>
        /// 기존카드모드
        /// 보통/수동반품: 신용카드
        /// 자동반품: 앱카드
        /// </summary>
        private PayCardMode m_initialCardMode = PayCardMode.CreditCard;

        /// <summary>
        /// 카드결제모드
        /// </summary>
        private PayCardMode m_cardMode = PayCardMode.CreditCard;
        private PayCardMode CardMode
        {
            get
            {
                return m_cardMode;
            }
            set
            {
                m_cardMode = value;
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        UIStateByPayCardMode(value);
                    });
                }
                else
                {
                    UIStateByPayCardMode(value);
                }
            }
        }

        public string GuideMessage
        {
            set
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        msgBar01.Text = value;
                    });
                }
                else
                {
                    msgBar01.Text = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool EnableClose
        {
            set
            {
                if (InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        this.btnClose.Enabled = value;
                    });
                }
                else
                {
                    this.btnClose.Enabled = value;
                }
            }
        }

        /// <summary>
        /// 여전법 추가 0620
        /// 강제취소 가능여부 설정
        /// </summary>
        private bool EnableForceClose
        {
            set
            {
                if (InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        this.btnForceClose.Visible = value;
                        this.btnClose.Visible = !value;
                    });
                }
                else
                {
                    this.btnForceClose.Visible = value;
                    this.btnClose.Visible = !value;
                }
            }
        }

        private BasketPayCard m_cardBasket = null;

        /// <summary>
        /// 전화승인 받은 정보
        /// </summary>
        private string m_telCardCdCard = string.Empty;
        private string m_telCardNmCard = string.Empty;

        /// <summary>
        /// BasketPayCard
        /// </summary>
        private BasketPayCard m_cardBasketOrg = null;

        /// <summary>
        /// 취소가능여부
        /// </summary>
        private bool m_cancellable = false;

        /// <summary>
        /// Card basket CVM
        /// </summary>
        private string m_cardCVM = "N";

        /// <summary>
        /// 전문추가정보
        /// </summary>
        private PV21ReqDataAdd m_addData = null;

        /// <summary>
        /// 할부개월
        /// </summary>
        private string Installment
        {
            get
            {
                return TypeHelper.ToInt32(txtInstallment.Text).ToString("d2");
            }
        }

        /// <summary>
        /// 마지막 승인요청 FrameNo
        /// </summary>
        private int m_orgApprFrameNo;

        #region 여전법 05.25

        /// <summary>
        /// FallBack Popup MSR 카드 읽기 대기팝업
        /// </summary>
        private POS_PY_P021 m_waitFallBackReadPop = null;
        private bool m_isFallBackMode = false;
        private SignPadCardInfo m_icCardInfo = null;
        private SignPadCardInfo CardICInfo
        {
            get
            {
                return m_icCardInfo;
            }
            set
            {
                m_icCardInfo = value;
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        UpdateCardICInfo(value);
                    });
                }
                else
                {
                    UpdateCardICInfo(value);
                }
            }
        }

        /// <summary>
        /// VAN 결재 완료시, 임시정보
        /// 2nd Generate 실행후 사용 및 종료
        /// </summary>
        private PV21RespData m_tempRespData = null;

        private POS_PY_P002 m_signPopup = null;

        /// <summary>
        /// 여전법 추가 0620
        /// KEYIN / IC Reading Type
        /// 
        /// </summary>
        WSWD.WmallPos.POS.FX.Win.UserControls.Button btnRdType = null;



        // 20170314.osj 전화승인(반품) 추가
        private POS_PY_P005 m_telPopup = null;


        /// <summary>
        /// 2번째 망취소 후
        /// 2nd Generate 호출 여부
        /// 0: 정상
        /// 1: 망취소
        /// 2: 망취소 후
        /// 
        /// </summary>
        private VANCancelReq2ndGenerateType m_vanCancelReq2ndGen = VANCancelReq2ndGenerateType.VANNormal;

        /// <summary>
        /// 기본 승인요청
        /// </summary>
        private CardVANReqType m_cardVANReqType = CardVANReqType.ReqAppr;

        #endregion

        #endregion

        #region 생성자

        /// <summary>
        /// 카드결제/취소
        /// 
        /// 여전법 변경 0620
        /// PayCardMode 파마메터 추가
        /// </summary>
        /// <param name="payAmt">결제금액</param>
        /// <param name="taxAmt"></param>
        /// <param name="modeReturn"></param>
        /// <param name="cancellable">취소가능?</param>
        /// <param name="cardPay">원거래카드결제BASKET</param>
        /// <param name="addData">추가정보 부가정보, added on 10.24</param>
        /// <param name="cardMode">여전법 추가 0620</param>
        public POS_PY_P001(int payAmt, int taxAmt, bool modeReturn, bool cancellable,
            BasketPayCard cardPay, PV21ReqDataAdd addData, PayCardMode cardMode)
        {
            InitializeComponent();

            // 대상금액
            m_payAmt = payAmt;
            m_taxAmt = taxAmt;

            m_cardBasketOrg = cardPay;

            // 반품시 첫 결제취소 일때만 닫기버튼 허용
            m_cancellable = cancellable;

            ModeReturn = modeReturn;
            m_modeAutoRtn = m_cardBasketOrg != null;
            m_addData = addData;

            // 여전법 변경 0616
            if (m_addData == null)
            {
                m_addData = new PV21ReqDataAdd();
            }

            // 여전법 추가 0620
            // 2번 째 결제수단 취소 시 강제쉬소 가능하게 
            if (m_modeAutoRtn)
            {
                EnableForceClose = !cancellable;
            }

            // 여전법 추가 0620
            btnRdType = btnReadIC;

            //if (modeReturn && cardPay != null)
            //{
            //    m_initialCardMode = PayCardMode.AppCard;
            //    CardMode = PayCardMode.AppCard;

            //    txtOTApprNo.Text = cardPay.ApprNo;
            //    txtOTApprDate.Text = cardPay.RealApprProcDate;
            //    txtInstallment.Text = cardPay.Halbu;
            //}

            // 여전법 변경 
            // 신용카드 자동 반품시


            //20170310.osj
            //20170314.osj  cardPay.PrefixCode[0] == ' ' 추가
            // 여전법
            // 기존 전문과 구분
            if (cardPay != null && (cardPay.PrefixCode[0] == 'H' || cardPay.PrefixCode[0] == ' '))
            {

                if (modeReturn && cardPay != null)
                {
                    //KSK_20170403
                    m_initialCardMode = cardMode;
                    CardMode = cardMode;

                    //m_initialCardMode = PayCardMode.CreditCard;
                    //CardMode = PayCardMode.CreditCard;

//                     txtOTApprNo.Text = cardPay.ApprNo;
//                     txtOTApprDate.Text = cardPay.RealApprProcDate;
//                     txtInstallment.Text = cardPay.Halbu;

                    string a1 = m_cardBasketOrg.ToLogString();
                    txtOTApprNo.Text = a1.Substring(244, 10);
                    txtOTApprDate.Text = a1.Substring(230, 8);
                    txtInstallment.Text = a1.Substring(101, 2);
                    //KSK_20170403
                    if (a1.Substring(98,1) != "*") {
                        m_telCardCdCard = a1.Substring(278, 4);
                        m_telCardNmCard = a1.Substring(282, 20);
                    }
                    else
                    {
                        m_telCardCdCard = a1.Substring(288, 4);
                        m_telCardNmCard = a1.Substring(292, 20);
                    }

                } 
            }
            else
            {
                if (modeReturn && cardPay != null)
                {
                    m_initialCardMode = cardMode;
                    CardMode = cardMode;

                    txtOTApprNo.Text = cardPay.ApprNo;
                    txtOTApprDate.Text = cardPay.RealApprProcDate;
                    txtInstallment.Text = cardPay.Halbu;
                    //KSK_20170403
                    m_telCardCdCard = cardPay.MaeipComCd;
                    m_telCardNmCard = cardPay.MaeipComNm;
                }

            }

            //Form Load Event
            Load += new EventHandler(form_Load);
            FormClosed += new FormClosedEventHandler(POS_PY_P001_FormClosed);
        }

        void POS_PY_P001_FormClosed(object sender, FormClosedEventArgs e)
        {
            // MSR
            // 여접법 05.24 비활성화
            // POSDeviceManager.Msr.DataEvent -= new POSMsrDataEventHandler(Msr_DataEvent);

            // 여접법 추가, 05.24
            POSDeviceManager.SignPad.CardICReaderEvent -= new POSCardICOnEncCardReader(SignPad_CardICReaderEvent);
            POSDeviceManager.SignPad.CardICApproveResult -= new POSCardICOnRequestCom2ndGenResult(SignPad_CardICApproveResult);

            // Close signpad
            InitSignPopup(true);

            // FallBack 일경우 MSR 읽기 대기시간 타임아웃            
            tmFallBackRead.Tick -= new EventHandler(tmFallBackRead_Tick);

            // barcode
            if (POSDeviceManager.Scanner.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
                POSDeviceManager.Scanner.DataEvent -= new POSDataEventHandler(Scanner_DataEvent);

            Load -= new EventHandler(form_Load);
            FormClosed -= new FormClosedEventHandler(POS_PY_P001_FormClosed);

            this.KeyEvent -= new OPOSKeyEventHandler(form_KeyEvent);                            //KeyEvent
            this.btnICCard.Click -= new EventHandler(btnICCard_Click);                          //현금IC카드 정보 button Event
            this.btnERCard.Click -= new EventHandler(btnERCard_Click);                          //은련카드 button Event
            this.btnTelSave.Click -= new EventHandler(btnTelSave_Click);                        //전화승인 button Event
            this.btnClose.Click -= new EventHandler(btnClose_Click);                            //닫기 button Event

            // 여전법 추가 0620
            // 강제취소 버튼
            this.btnForceClose.Click -= new EventHandler(btnForceClose_Click);                  //닫기 - 강제취소용 button Event            

            txtCardNo.InputFocused -= new EventHandler(txtCardNo_InputFocused);
            txtExpiryDate.InputFocused -= new EventHandler(txtExpiryDate_InputFocused);
            txtInstallment.InputFocused -= new EventHandler(txtInstallment_InputFocused);
            txtOTApprDate.InputFocused -= new EventHandler(txtOTApprDate_InputFocused);
            txtOTApprNo.InputFocused -= new EventHandler(txtOTApprNo_InputFocused);
            txtApprovalNo.InputFocused -= new EventHandler(txtApprovalNo_InputFocused);

            txtCardNo.KeyEvent -= new OPOSKeyEventHandler(Input_KeyEvent);
            txtExpiryDate.KeyEvent -= new OPOSKeyEventHandler(Input_KeyEvent);
            txtInstallment.KeyEvent -= new OPOSKeyEventHandler(Input_KeyEvent);
            txtOTApprDate.KeyEvent -= new OPOSKeyEventHandler(Input_KeyEvent);
            txtOTApprNo.KeyEvent -= new OPOSKeyEventHandler(Input_KeyEvent);
            txtApprovalNo.KeyEvent -= new OPOSKeyEventHandler(Input_KeyEvent);

            // 여전법 추가 0617
            // 메모리 삭제
            ClearSecureData();
        }

        #endregion

        #region 이벤트 등록

        /// <summary>
        /// Form Event 정의
        /// </summary>
        private void InitEvents()
        {
            this.KeyEvent += new OPOSKeyEventHandler(form_KeyEvent);            //KeyEvent
            this.btnICCard.Click += new EventHandler(btnICCard_Click);              //현금IC카드 정보 button Event
            this.btnERCard.Click += new EventHandler(btnERCard_Click);      //은련카드 button Event
            this.btnTelSave.Click += new EventHandler(btnTelSave_Click);              //전화승인 button Event
            this.btnClose.Click += new EventHandler(btnClose_Click);            //닫기 button Event

            // 여전법 추가 0620
            // 강제취소 버튼
            this.btnForceClose.Click += new EventHandler(btnForceClose_Click);            //닫기 - 강제취소용 button Event            

            // 여전법 추가 0617
            // 신용카드 & 식별번호 일기
            this.btnReadIC.Click += new EventHandler(KeyInRead_Click);
            this.btnRdKEYIN.Click += new EventHandler(KeyInRead_Click);

            txtCardNo.InputFocused += new EventHandler(txtCardNo_InputFocused);
            txtExpiryDate.InputFocused += new EventHandler(txtExpiryDate_InputFocused);
            txtInstallment.InputFocused += new EventHandler(txtInstallment_InputFocused);
            txtOTApprDate.InputFocused += new EventHandler(txtOTApprDate_InputFocused);
            txtOTApprNo.InputFocused += new EventHandler(txtOTApprNo_InputFocused);
            txtApprovalNo.InputFocused += new EventHandler(txtApprovalNo_InputFocused);

            txtCardNo.KeyEvent += new OPOSKeyEventHandler(Input_KeyEvent);
            txtExpiryDate.KeyEvent += new OPOSKeyEventHandler(Input_KeyEvent);
            txtInstallment.KeyEvent += new OPOSKeyEventHandler(Input_KeyEvent);
            txtOTApprDate.KeyEvent += new OPOSKeyEventHandler(Input_KeyEvent);
            txtOTApprNo.KeyEvent += new OPOSKeyEventHandler(Input_KeyEvent);
            txtApprovalNo.KeyEvent += new OPOSKeyEventHandler(Input_KeyEvent);

            // MSR
            // 여접법 05.24 비활성화
            //POSDeviceManager.Msr.Close();
            //POSDeviceManager.Msr.Open();
            //POSDeviceManager.Msr.DataEvent += new POSMsrDataEventHandler(Msr_DataEvent);

            // barcode
            if (POSDeviceManager.Scanner.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
            {
                POSDeviceManager.Scanner.DataEvent += new POSDataEventHandler(Scanner_DataEvent);
            }

            //KSK_20170403
            // 여접법 추가, 05.24
            InitSignPopup(false);

            POSDeviceManager.SignPad.CardICReaderEvent += new POSCardICOnEncCardReader(SignPad_CardICReaderEvent);
            POSDeviceManager.SignPad.CardICApproveResult += new POSCardICOnRequestCom2ndGenResult(SignPad_CardICApproveResult);
            POSDeviceManager.SignPad.SetICTransAmount(m_payAmt);

            // FallBack 일경우 MSR 읽기 대기시간 타임아웃
            tmFallBackRead.Tick += new EventHandler(tmFallBackRead_Tick);
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
            InitEvents();

            //결제금액
            txtGetAmt.Text = m_payAmt.ToString();

            /// set focus to enter card no
            txtCardNo.SetFocus();

            //정보 조회
            m_presenter = new PYP001presenter(this);
        }

        /// <summary>
        /// Key Event
        /// </summary>
        /// <param name="e"></param>
        void form_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (ModeProcessing == PaymentState.Processing ||
                ModeProcessing == PaymentState.PayCompleted)
            {
                e.IsHandled = true;
                return;
            }

            DataChanged = !IsInitialState();

            if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER ||
                e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
            {
                ValidateOnKeyEvent(e);
            }
            else
            {
                // 11.21 
                // Loc added 11.21
                ValidateChangeCardNo(e);
            }
        }

        /// <summary>
        /// Loc added 11.21
        /// 11.21 이사님 요청
        /// 신용카드 승인 처리 하는 POPUP 창에서 카드 SWIP 하거나 AppCard Scan한 후 카드번호 입력 항목을 사용자가 클릭하거나 화살표로 항목 이동 
        /// Focus 이동 시킨 다음 번호만 수정되지 않게 (추가입력이나 번호 삭제) 막아야 합니다.
        /// CLEAR 기능 : 유지
        /// 기능 : 번호 삭제 되므로 막아야 함
        /// 화살표 기능 : 유지
        /// 둥록 기능 : 유지
        /// 단,  번호 KeyIN 상태는 지금처럼 추가 입력 가능하게 한다.
        /// 
        /// 0729 - 여전법 추가, KEYBOARD입력 못 하게
        /// 
        /// </summary>
        private void ValidateChangeCardNo(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            //if (!txtCardNo.IsFocused || m_keyInMode)
            // 여전법 추가 0729
            // keyin mode이여도 키보드로 못 하게
            if (!txtCardNo.IsFocused)
            {
                return;
            }

            // allow only UP/DOWN key
            // 여전법 변경 0729
            // 모든 키 입력 못 하게 (up/down 제외)
            if (e.Key.OPOSKey != OPOSMapKeys.KEY_UP && e.Key.OPOSKey != OPOSMapKeys.KEY_DOWN)
            {
                e.IsHandled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void Input_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            DataChanged = !IsInitialState();

            // 여전법 주석처리 0729
            //if (!e.IsControlKey && txtCardNo.IsFocused)
            //{
            //    m_keyInMode = true;
            //}

            // 여전법 추가 0729
            // 유효기안 입력불가
            if (!e.IsControlKey && txtExpiryDate.IsFocused)
            {
                e.IsHandled = true;
            }
        }

        /// <summary>
        /// 카드번호처리
        /// 
        /// 여접법 05.24 비활성화
        /// 사용안함
        /// </summary>
        /// <param name="eventData"></param>
        void Msr_DataEvent(string eventData, string cardNo, string expYYmm)
        {
            TraceHelper.Instance.TraceWrite("POS_PY_P001::Msr_DataEvent", "MSR:ModeProcessing:" + ModeProcessing.ToString());
            if (ModeProcessing == PaymentState.PayCompleted ||
                ModeProcessing == PaymentState.Processing)
            {
                return;
            }

            m_keyInMode = false;
            DataChanged = true;
            Track2Data = eventData;
            txtCardNo.Text = cardNo;
            txtExpiryDate.Text = expYYmm;

            //2015.09.08 정광호 추가--------
            txtInstallment.Text = "";
            StatusMessage = string.Empty;
            //------------------------------

            txtInstallment.SetFocus();
        }

        /// <summary>
        /// APP Card 확인
        /// </summary>
        /// <param name="eventData"></param>
        void Scanner_DataEvent(string eventData)
        {
            Trace.WriteLine("PY_P001_Scanner_DataEvent " + eventData, "program");

            if (ModeProcessing == PaymentState.PayCompleted ||
                ModeProcessing == PaymentState.Processing)
            {
                return;
            }

            DataChanged = true;
            string cardNo = string.Empty;
            string corpCode = string.Empty;
            if (POSMsr.ParseAppCard(eventData, out cardNo, out corpCode))
            {
                CardMode = PayCardMode.AppCard;
                m_keyInMode = false;

                Track2Data = cardNo + "=" + EXP_YM_APP_CARD + corpCode;
                txtCardNo.Text = cardNo;
                txtExpiryDate.Text = EXP_YM_APP_CARD;

                if (m_cardBasketOrg != null)
                {
                    txtExpiryDate.SetFocus();

                    // auto process
                    StartPaymentOperation();
                }
                else
                {
                    txtInstallment.SetFocus();
                }
            }
        }

        /// <summary>
        /// 현금IC카드 정보 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnICCard_Click(object sender, EventArgs e)
        {
            this.CardMode = PayCardMode.CashICCard;
        }

        /// <summary>
        /// 은련카드 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnERCard_Click(object sender, EventArgs e)
        {
            this.CardMode = PayCardMode.ERCard;
        }

        /// <summary>
        /// 전화승인 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnTelSave_Click(object sender, EventArgs e)
        {
            this.CardMode = PayCardMode.TelManualCard;
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

        void txtInstallment_InputFocused(object sender, EventArgs e)
        {
            if (ModeProcessing == PaymentState.PayCompleted)
            {
                return;
            }
            GuideMessage = MSG_ENTER_INSTALLMENT;
        }

        void txtExpiryDate_InputFocused(object sender, EventArgs e)
        {
            if (ModeProcessing == PaymentState.PayCompleted)
            {
                return;
            }
            GuideMessage = MSG_ENTER_CARD_YM;
        }

        void txtCardNo_InputFocused(object sender, EventArgs e)
        {
            if (ModeProcessing == PaymentState.PayCompleted)
            {
                return;
            }

            GuideMessage = CardMode == PayCardMode.AppCard ||
                m_cardBasketOrg != null ? MSG_SCAN_APP_CARD : MSG_ENTER_CARD_NO;
        }

        void txtApprovalNo_InputFocused(object sender, EventArgs e)
        {
            if (ModeProcessing == PaymentState.PayCompleted)
            {
                return;
            }

            //GuideMessage = MSG_ENTER_APPR_NO;
            GuideMessage = MSG_ENTER_APPR_NO_INPUT;
        }

        void txtOTApprNo_InputFocused(object sender, EventArgs e)
        {
            GuideMessage = MSG_ENTER_OT_APPR_NO;
        }

        void txtOTApprDate_InputFocused(object sender, EventArgs e)
        {
            GuideMessage = MSG_ENTER_OT_APPR_DATE;
        }

        #endregion

        #region 사용자 정의

        void UIStateByPaymentState(PaymentState state)
        {
            btnICCard.Enabled = state == PaymentState.Ready;
            btnClose.Enabled = state == PaymentState.Ready;
            if (state == PaymentState.Ready)
            {
                StatusMessage = string.Empty;
            }
        }

        /// <summary>
        /// 결제모드에 따라 컨트롤들 활성화/비활성화
        /// </summary>
        /// <param name="mode"></param>
        void UIStateByPayCardMode(PayCardMode mode)
        {
            if (mode == PayCardMode.ERCard)
            {
                this.Text = TITLE_CARD_ER;
            }
            else if (mode == PayCardMode.TelManualCard)
            {
                this.Text = TITLE_CARD_TEL;
                // 20170314.osj 전화승인(반품) 추가
                if (!TLCard_SelectCardComp2())
                //if (!TLCard_SelectCardComp())
                {
                    CardMode = m_initialCardMode;
                    return;
                }
            }
            else
            {
                this.Text = TITLE_CARD;
            }

            lblApprNo.Visible = mode == PayCardMode.TelManualCard;
            txtApprovalNo.Visible = mode == PayCardMode.TelManualCard;
            txtApprovalNo.ReadOnly = mode != PayCardMode.TelManualCard;
            btnERCard.Enabled = m_cardBasketOrg == null && mode == m_initialCardMode;

            // 자동반품시
            // 전화승인불가능            
            btnTelSave.Enabled = m_cardBasketOrg == null && mode == m_initialCardMode;

            // 취소불가능6
            btnClose.Enabled = m_cancellable;

            if (mode == m_initialCardMode)
            {
                StatusMessage = string.Empty;
                txtCardNo.SetFocus();
            }
        }

        #endregion

        #region 결제진행 함수

        /// <summary>
        /// Start payment
        /// </summary>
        void StartPaymentOperation()
        {
            if (!(ModeProcessing == PaymentState.Ready ||
                ModeProcessing == PaymentState.Errored))
            {
                return;
            }

            if (this.CardMode == PayCardMode.CreditCard
                || this.CardMode == PayCardMode.AppCard
                || this.CardMode == PayCardMode.CashICCard
                || this.CardMode == PayCardMode.TelManualCard)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        // show 전저서명팝업
                        CRCard_ProcessCardSign();
                    });
                }
                else
                {
                    // show 전저서명팝업
                    CRCard_ProcessCardSign();
                }
            }
            else if (this.CardMode == PayCardMode.ERCard)
            {
                ERCard_ShowPasswordPop();
            }
        }

        /// <summary>
        /// 결제완료, 마지막처리
        /// </summary>
        void EndPaymentOperation()
        {
            this.ReturnResult.Add("PAY_DATA", m_cardBasket.Clone());

            // 신용카드
            if (ModeProcessing == PaymentState.PayCompleted || !string.IsNullOrEmpty(m_signData))
            {
                // SIGN FILE 새엉
                // 20150110-01-0121-000120-12345678.SIGN
                //   영업일자-점코드-포스번호-거래번호-승인번호.SIGN                        
                if (string.IsNullOrEmpty(m_tempSignFileName))
                {
                    POSDeviceManager.SignPad.SaveSignData(m_cardBasket.ApprNo);
                }
                else
                {
                    // DCC인경우에는 temp sign file reset 되는 바람에..
                    POSDeviceManager.SignPad.SaveSignData(m_cardBasket.ApprNo, m_tempSignFileName);
                }
            }

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Progress Message
        /// </summary>
        /// <param name="showProgress"></param>
        public void ShowProgressMessage(bool showProgress)
        {
            ChildManager.ShowProgress(showProgress,
                this.CardMode == PayCardMode.ERCard ?
                MSG_VAN_REQ_PROCESSING_ERCARD : (this.CardMode == PayCardMode.TelManualCard ?
                        MSG_VAN_REQ_PROCESSING_TLCARD :
                MSG_VAN_REQ_PROCESSING_CRCARD));
        }

        /// <summary>
        /// VAN승인 오류시
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="errorMessage"></param>
        /// <param name="errorCode"></param>
        /// <param name="viewTag"></param>
        public void ShowErrorMessage(VANRequestErrorType errorType, string errorMessage, string errorCode, string viewTag)
        {
            m_lastError = errorType;
            string message = string.Empty;
            bool showErrorMessage = m_modeAutoRtn;
            switch (errorType)
            {
                case VANRequestErrorType.None:
                    break;
                case VANRequestErrorType.CommError:
                    message = MSG_VAN_REQ_COMM_ERROR;
                    message += string.Format("{0}{1}", string.IsNullOrEmpty(errorCode) ?
                        string.Empty : "[" + errorCode + "] ",
                        errorMessage);
                    showErrorMessage &= true;
                    break;
                case VANRequestErrorType.NoInfoFound:
                case VANRequestErrorType.SomeError:
                    message = string.Format("{0}{1}",
                        string.IsNullOrEmpty(errorCode) ? string.Empty : "[" + errorCode + "] ",
                        errorMessage);
                    showErrorMessage &= true;
                    break;
                default:
                    break;
            }

            StatusMessage = message;
            ModeProcessing = PaymentState.Errored;

            // 자동반품시, VAN,오류시 오류메시지 보여주면서 재시도 및 강제진행할수 있게
            // 강제진행한경우에 DialogResult = OK, ReturnDAta없음
            if (showErrorMessage)
            {
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
                        var res = ShowMessageBox(MessageDialogType.Question, string.Empty,
                            message, new string[] { LABEL_RETRY, LABEL_FORCE });

                        if (res != DialogResult.Yes)
                        {
                            // 강제취소 일때
                            // basket null으로 return함
                            this.ReturnResult.Add("ERROR_CODE", errorCode);
                            this.ReturnResult.Add("ERROR_MSG", errorMessage);
                            this.DialogResult = DialogResult.OK;
                        }
                    });
                }
                else
                {
                    var res = ShowMessageBox(MessageDialogType.Question, string.Empty,
                            message, new string[] { LABEL_RETRY, LABEL_FORCE });

                    if (res != DialogResult.Yes)
                    {
                        // 강제취소 일때
                        // basket null으로 return함
                        this.ReturnResult.Add("ERROR_CODE", errorCode);
                        this.ReturnResult.Add("ERROR_MSG", errorMessage);
                        this.DialogResult = DialogResult.OK;
                    }
                }
            }

            #region 여전법 변경 05.28

            // TODO
            // IF CANCEL 요청.. 취소 요청시 ERROR            
            if (m_cardVANReqType == CardVANReqType.ReqCanc && this.CardICInfo.CardType.Equals("IC"))
            {
                // 추소요청시 결과 상관 없이 
                // RequestCom2ndGenerate 2번째 호출한다
                ProcessResetPaymentAfterCancel();
            }
            else
            {
                // IIF 정상 승인요청 
                // ICCard Reading Reset
                // 입력한 화면 유지
                ResetICCardReader(false);

                // 닫기 버튼 활성화
                EnableClose = true;
            }

            #endregion
        }

        /// <summary>
        /// VAN에서 결과 받는다
        /// </summary>
        /// <param name="respData"></param>
        public void OnReturnSuccess(PV21RespData respData, string strSignData)
        {
            // 정상승인시, ReqFrameNo 저장함
            this.m_orgApprFrameNo = "0".Equals(respData.CancType) ? m_presenter.LastFrameNo : 0;

            respData.ENCData.ResetZero();

            if ("R".Equals(respData.DCCTrxnTypeRes))
            {
                // DCC거래,
                // SignFile backup 후 나중에 사용
                m_tempSignFileName = POSDeviceManager.SignPad.BackupSignFile();

                this.BeginInvoke((MethodInvoker)delegate()
                {
                    RequestDCCExRateSelection(respData);
                });

                return;
            }
            // 여전법 추가 0808(KSK)
            //else if (respData.DCCTrxnTypeRes != "T")
            //{
            //    StatusMessage = string.Empty;
            //    StatusMessage = "[" + respData.DCCTrxnTypeRes + "]" + "DCC 조회 오류 입니다.";
            //    ModeProcessing = PaymentState.Errored;
            //    return;
            //}

            #region 여전법 추가

            // Check if CancelRequest NETCancel
            // 망취소 성공 후
            // IC 일경우만 2ndGen 호출
            if (respData.CancType == "9" || m_cardVANReqType == CardVANReqType.ReqCanc)
            {
                if (this.CardICInfo.CardType.Equals("IC"))
                {
                    ProcessResetPaymentAfterCancel();
                }
                return;
            }
            else
            {
                //KSK_20170403
                //// VAN 결재 호출 후
                //// 신용 IC 결재 시
                //// FALLBACK 아닌경우
                //// EncReaderCom2ndGen 호출
                //if (this.CardMode == PayCardMode.CreditCard &&
                //    this.CardICInfo != null && CardICInfo.CardType.Equals("IC"))
                //{
                //    m_tempRespData = respData;
                //    ProcessRequestReaderCom2ndGen();
                //    return;
                //}
            }


            #endregion

            ProcessCompletePayment(respData);
        }

        /// <summary>
        /// 
        /// </summary>
        void ProcessRequestReaderCom2ndGen()
        {
            string s2ndErrorMsg = string.Empty;
            m_vanCancelReq2ndGen = VANCancelReq2ndGenerateType.ReqRdCom2ndGen_1st;

            // 여전법 변경 2ndGen 호출시, 받은 encData 보냄(KSK_0902)
            string resCode = POSDeviceManager.SignPad.RequestReaderCom2ndGen(this.CardICInfo, m_tempRespData.ENCData, out s2ndErrorMsg);

            // 정상일때, 오류아닐 때 망취소
            if (resCode != "00")
            {
                // 정상승인 요청 시만 망취소
                s2ndErrorMsg += Environment.NewLine;
                s2ndErrorMsg += "망취소 요청 합니다.";
                ShowMessageBox(MessageDialogType.Error, string.Empty,
                    s2ndErrorMsg);

                ProcessVANNetCancel();
            }

            // 여전법 변경 2ndGen 호출시, 받은 encData 보냄(KSK_0902)
            m_tempRespData.ENCData.ResetZero();
        }

        /// <summary>
        /// 여전법 변경 05.27
        /// 
        /// 결제 마지막 단계
        /// 
        /// 07.13: 선불카드일경우 잔액표시
        /// </summary>
        /// <param name="respData"></param>
        void ProcessCompletePayment(PV21RespData respData)
        {
            string cardBalance = (respData.PaidCardBalance ?? string.Empty).Trim();

            StatusMessage = string.Format("{0}{1}", MSG_PAY_COMPLETED,
                            string.IsNullOrEmpty(cardBalance) ? string.Empty :
                            string.Format(" (잔액: {0:#,##0})", Convert.ToInt32(respData.PaidCardBalance)));

#if DEBUG
            if (!string.IsNullOrEmpty(cardBalance) &&
                Convert.ToInt32(respData.PaidCardBalance) > 0)
            {
                Thread.Sleep(3000);
            }
#endif

            txtApprovalNo.Text = respData.ApprNo;
            txtCardNm.Text = respData.CardNm;
            ModeProcessing = PaymentState.PayCompleted;

            m_cardBasket = new BasketPayCard()
            {
                ApprState = CardMode == PayCardMode.TelManualCard ? "2" : "1",
                CancFg = ModeReturn ? "1" : "0", 
                CardNm = respData.CardNm,
                CardNo = CardNo,
                ExpMY = CardExpDate,
                Halbu = Installment,
                CVM = m_cardCVM,
                ForeignCardFg = respData.CardType2,
                // 반품시, 당일취소건인지 확인
                CurDateCancType = ModeReturn ?
                    (DateTime.Today.ToString("yyyyMMdd").Equals(respData.ApprDate) ? "1" : "0") : "0",
                EunCardFg = CardMode == PayCardMode.ERCard ? "Y" : "N",
                IssueComCd = respData.IssueComCd,
                IssueComNm = respData.IssueComNm,
                MaeipComCd = respData.MaeipComCd,
                MaeipComNm = respData.MaeipComNm,
                PayAmt = m_payAmt.ToString(),
                ApprNo = respData.ApprNo,
                RealApprProcDate = respData.ApprDate,
                RealApprProcTime = respData.ApprTime,
                OTApprNo = txtOTApprNo.Text,
                OTSaleDate = txtOTApprDate.Text,
                TrackII = Track2Data,
                VanId = respData.ApprVanCode,
                MerchantCode = respData.MerchantsCode,
                ApprAmtIncVat = m_taxAmt.ToString(),

                // 여전법 추가 05.27
                PrefixCode = CardNo.Length >= 6 ? CardNo.Substring(0, 6) : CardNo,
                PaidCardBalance = respData.PaidCardBalance,
            };

            if (dccReqData != null)
            {
                m_cardBasket.DCCCheckNo = dccReqData.DCCCheckNo;
                m_cardBasket.DCCCurNo = dccReqData.TrxnCurNo;
                m_cardBasket.DCCCurCode = dccReqData.TrxnCurCode;
                m_cardBasket.DCCCurAmt = dccReqData.TrxnCurAmt;
                m_cardBasket.DCCCurDecPoint = dccReqData.TrxnCurAmtDecPoint;
                m_cardBasket.DCCExRate = dccReqData.ExRate;
                m_cardBasket.DCCExRateDecPoint = dccReqData.ExRateDecPoint;
                m_cardBasket.DCCRvExRate = dccReqData.DCCRvExRate;
                m_cardBasket.DCCRvExRateDecPoint = dccReqData.DCCRvExRateDecPoint;
                m_cardBasket.DCCRvExRateUnt = dccReqData.DCCRvExRateUnt;
                m_cardBasket.MarkupPerc = dccReqData.MarkupPerc;
                m_cardBasket.MarkupPercUnt = dccReqData.MarkupPercUnt;
                m_cardBasket.ComsPercVal = dccReqData.ComsPercVal;
                m_cardBasket.ComsPercDecPoint = dccReqData.ComsPercDecPoint;
                m_cardBasket.ComsValCurNo = dccReqData.ComsValCurNo;
                m_cardBasket.ComsValCurAmt = dccReqData.ComsValCurAmt;
                m_cardBasket.ComsValCurMinUnt = dccReqData.ComsValCurMinUnt;
                m_cardBasket.ComsPrtYN = dccReqData.ComsPercVal;

                m_cardBasket.NatCurNo = dccReqData.NatCurNo;
                m_cardBasket.NatCurCode = dccReqData.NatCurCode;

                //m_cardBasket.RateId = dccReqData.RateId;
                //m_cardBasket.ExRateExpTime = dccReqData.ExRateExpTime;
            }

            if (!m_keyInMode)
            {
                switch (this.CardMode)
                {
                    case PayCardMode.CreditCard:
                    case PayCardMode.ERCard:
                    case PayCardMode.TelManualCard:
                        m_cardBasket.InputType = "A";
                        break;
                    case PayCardMode.AppCard:
                        m_cardBasket.InputType = "P";
                        break;
                    case PayCardMode.CashICCard:
                        m_cardBasket.InputType = "I";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // 수기등록
                m_cardBasket.InputType = "@";
            }

            // Clear memory
            respData.MaskCardNo.ResetZero();
            respData.ENCCardNo.ResetZero();
            respData.ENCData.ResetZero();
                
            // 자동닫기
            EndPaymentOperation();
        }

        /// <summary>
        /// 여전법 추가 05.27
        /// 망상취소
        /// </summary>
        void ProcessVANNetCancel()
        {
            // 취소 후 화면 reset 한다
            RequestVANCardPayment(false, string.Empty, null, true);
        }

        /// <summary>
        /// 여전법 추가 05.27
        /// 취소 후 화면 reset 한다
        /// </summary>
        void ProcessResetPaymentAfterCancel()
        {
            ShowProgressMessage(true);
            string errorMsg = string.Empty;
            m_vanCancelReq2ndGen = VANCancelReq2ndGenerateType.ReqRdCom2ndGen_2stVANCancReq;
            //POSDeviceManager.SignPad.RequestReaderCom2ndGen(CardICInfo, out errorMsg);
            POSDeviceManager.SignPad.RequestReaderCom2ndGen(CardICInfo, m_tempRespData.ENCData, out errorMsg);

        }

        /// <summary>
        /// 전자서명팝업
        /// </summary>
        void CRCard_ProcessCardSign()
        {
            if (ModeProcessing == PaymentState.Processing)
            {
                return;
            }

            ModeProcessing = PaymentState.Processing;
            GuideMessage = string.Empty;
            StatusMessage = string.Empty;

            // 싸인데이티 초기화
            m_signData = string.Empty;
            m_tempSignFileName = string.Empty;

            // show 전저서명팝업
            var res = m_signPopup.ShowDialog();

            if (res == DialogResult.OK)
            {
                // 카드결제정보 받기 완료
                m_signData = m_signPopup.ReturnResult["SIGN_DATA"].ToString();
                //#if DEBUG

                //                    if (System.IO.File.Exists("C:\\Tmp\\SignData.sign"))
                //                    {
                //                        m_signData = System.IO.File.ReadAllText("C:\\Tmp\\SignData.sign");
                //                    }
                //                    else
                //                    {
                //                        System.IO.File.WriteAllText("C:\\Tmp\\SignData.sign", m_signData);
                //                    }
                //#endif

                // 본인확인방법
                m_cardCVM = string.IsNullOrEmpty(m_signData) ? "N" : "S";

                if (CardMode == PayCardMode.TelManualCard)
                {
                    TLCard_MakeBasket();
                }
                else
                {
                    RequestVANCardPayment(false, string.Empty, null, false);
                }
            }
            else
            {
                ModeProcessing = PaymentState.Ready;
            }

        }

        /// <summary>
        /// VAN연동
        /// </summary>
        /// <param name="isERCard"></param>
        void RequestVANCardPayment(bool isERCard, string fgDCCCheck, object dccReqData, bool isNetCancel)
        {
            //KSK_20170403
            Trace.WriteLine("Installment [" + txtInstallment.Text + "] ExpiryDate [" + txtExpiryDate.Text + "] txtOTApprDate ["
                + txtOTApprDate.Text + "] txtOTApprNo [" + txtOTApprNo.Text + "]", "program");

            m_cardVANReqType = isNetCancel ? CardVANReqType.ReqCanc : CardVANReqType.ReqAppr;

            string inputType = "S";
            if (m_keyInMode)
            {
                inputType = "K";
            }
            else
            {
                if (this.CardICInfo != null)
                {
                    inputType = this.CardICInfo.CardType.Equals("IC") ? "C" : "S";
                }
                else if (CardMode == PayCardMode.AppCard)
                {
                    inputType = "A";
                }
            }

            // 망취소시,
            // 마지막 성공한 승인요청의 FrameNo으로 보냄
            if (isNetCancel)
            {
                m_addData.OrgFrameNo = m_orgApprFrameNo;
            }

            int cancType = ModeReturn ? 1 : (isNetCancel ? 9 : 0);
            m_presenter.RequestVANCardPayment(cancType,
                Track2Data,
                inputType,
                Installment, m_signData, m_payAmt.ToString(), m_taxAmt.ToString(), isERCard ? "Y" : "N",
                fgDCCCheck, m_erCardWorkIndex, m_erCardPin,
                m_tempRespData != null ? m_tempRespData.ApprDate : txtOTApprDate.Text,
                m_tempRespData != null ? m_tempRespData.ApprNo : txtOTApprNo.Text,
                    dccReqData,
                    m_addData); // changed on 10.24 전문추가정보
        }

        /// <summary>
        /// 카드사선택
        /// </summary>
        bool TLCard_SelectCardComp()
        {
            using (var pop = this.ChildManager.ShowPopup(string.Empty,
                "WSWD.WmallPos.POS.PY.dll", "WSWD.WmallPos.POS.PY.VC.POS_PY_P005")) // 20170314.osj ChildManager 에 this 추가
            {
                var res = pop.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    StatusMessage = string.Format(MSG_TL_CARD_SEL, pop.ReturnResult["NM_CARD"]);
                    m_telCardCdCard = pop.ReturnResult["CD_CARD"].ToString();
                    m_telCardNmCard = pop.ReturnResult["NM_CARD"].ToString();
                    m_cardCVM = "N";

                    txtCardNo.SetFocus();
                }

                return res == DialogResult.OK;
            }
        }


        // 20170314.osj 전화승인(반품) 추가
        bool TLCard_SelectCardComp2()
        {
            DialogResult res = DialogResult.OK;
            if (ChildManager != null)
            {
            m_telPopup = (POS_PY_P005)this.ChildManager.ShowPopup(string.Empty,
                "WSWD.WmallPos.POS.PY.dll", "WSWD.WmallPos.POS.PY.VC.POS_PY_P005");

            res = m_telPopup.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    StatusMessage = string.Format(MSG_TL_CARD_SEL, m_telPopup.ReturnResult["NM_CARD"]);
                    m_telCardCdCard = m_telPopup.ReturnResult["CD_CARD"].ToString();
                    m_telCardNmCard = m_telPopup.ReturnResult["NM_CARD"].ToString();
                    m_cardCVM = "N";

                    txtCardNo.SetFocus();
                }
            }
            return res == DialogResult.OK;
        }

        /// <summary>
        /// Basket 생성
        /// </summary>
        void TLCard_MakeBasket()
        {
            ModeProcessing = PaymentState.PayCompleted;

            m_cardBasket = new BasketPayCard()
            {
                ApprNo = txtApprovalNo.Text,
                ApprState = "2",
                CancFg = ModeReturn ? "1" : "0", 
                CardNm = string.Empty,
                CardNo = CardNo,
                ExpMY = CardExpDate,
                Halbu = Installment,
                InputType = m_keyInMode ? "@" : "A",
                CVM = m_cardCVM,
                IssueComCd = string.Empty,
                IssueComNm = string.Empty,
                MaeipComCd = m_telCardCdCard,
                MaeipComNm = m_telCardNmCard,
                PayAmt = m_payAmt.ToString(),
                RealApprProcDate = DateTime.Today.ToString("yyyyMMdd"),
                RealApprProcTime = DateTime.Now.ToString("HHmmss"),
                OTApprNo = txtOTApprNo.Text,
                OTSaleDate = txtOTApprDate.Text,
                TrackII = Track2Data,
                VanId = string.Empty,

                //KSK_20170403
                // 반품시, 당일취소건인지 확인
                CurDateCancType = ModeReturn ?
                    (DateTime.Today.ToString("yyyyMMdd").Equals(txtOTApprDate.Text) ? "1" : "0") : "0",

                // 여전법 추가 05.27
                PrefixCode = CardNo.Length >= 6 ? CardNo.Substring(0, 6) : CardNo,

                ApprAmtIncVat = m_taxAmt.ToString()
            };

            // 자동닫기
            EndPaymentOperation();
        }

        /// <summary>
        /// 은련카드비밀번호팝업
        /// </summary>
        void ERCard_ShowPasswordPop()
        {
            ModeProcessing = PaymentState.Processing;

            // 여전법 변경 0622
            // IC로 읽으면 encoding 사용, encCardNo 사용
            // KEY-IN으로 하면 ENCODING안 함, 일단 카드번호
            using (var pop = ChildManager.ShowPopup(string.Empty,
                "WSWD.WmallPos.POS.PY.dll", "WSWD.WmallPos.POS.PY.VC.POS_PY_P003",
                m_icCardInfo != null, m_icCardInfo != null ? m_icCardInfo.EncCardNo : txtCardNo.Text, 6))
            {
                var res = pop.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    m_erCardPin = pop.ReturnResult["PASSWORD"].ToString();
                    m_erCardWorkIndex = pop.ReturnResult["WORK_INDEX"].ToString();
                    m_cardCVM = "P";
                    m_signData = string.Empty;
                    m_tempSignFileName = string.Empty;
                    RequestVANCardPayment(true, string.Empty, null, false);
                }
                else
                {
                    ModeProcessing = PaymentState.Ready;
                }
            }
        }

        #endregion

        #region Validation Logic

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mmYY"></param>
        /// <returns>0:정상,1:입력함,2:자리수,3:잘못됨,4:시스템날짜보다작다</returns>
        int ValidateYearMonth(string mmYY)
        {

            if (this.CardMode == PayCardMode.AppCard)
            {
                return 0;
            }
            else if (this.CardMode == PayCardMode.ERCard)
            {
                //2015.10.14 정광호 수정
                //은련카드는 유효기간 정합성 제외(카드를 읽어올때 '=' 뒤 4자리가 '0000'으로 넘어옴)
                return 0;
            }

            if (string.IsNullOrEmpty(mmYY))
            {
                return 1;
            }

            if (mmYY.Length != 4)
            {
                return 2;
            }

            if (mmYY == "****")
            {
                return 0;
            }


            int y = int.Parse(mmYY.Substring(2, 2));
            int m = int.Parse(mmYY.Substring(0, 2));
            int my = int.Parse(string.Format("{0:d2}{1:d2}", y, m));

            if (m == 0 || m > 12 || y == 0)
            {
                return 3;
            }

            if (my < int.Parse(DateTime.Today.ToString("yyMM")))
            {
                return 4;
            }

            return 0;
        }

        /// <summary>
        /// Validation before payment
        /// </summary>
        /// <returns></returns>
        bool ValidateReadyForPayment()
        {
            //var res = (txtCardNo.Text.Length >= 15
            //        && txtCardNo.Text.Length <= 19) &&
            //        ValidateYearMonth(txtExpiryDate.Text) == 0;

            if (txtCardNo.Text.Length < MIN_CARD_NO_LEN || txtCardNo.Text.Length > MAX_CARD_NO_LEN)
            {
                //txtCardNo.SetFocus();
                return false;
            }

            if (ValidateYearMonth(txtExpiryDate.Text) != 0)
            {
                //txtExpiryDate.SetFocus();
                return false;
            }

            if (this.CardMode == PayCardMode.TelManualCard)
            {
                //res = res && txtApprovalNo.Text.Length >= 6;


                if (txtApprovalNo.Text.Length < 6)
                {
                    txtApprovalNo.Text = "";
                    //txtApprovalNo.SetFocus();   
                    return false;
                }
            }

            if (this.ModeReturn)
            {
                //res &= txtOTApprDate.Text.Length == 8 &&
                //    txtOTApprNo.Text.Length >= 6;

                if (txtOTApprDate.Text.Length != 8 || !DateTimeUtils.ValidateOrgApprDateSystem(txtOTApprDate.Text))
                {
                    txtOTApprDate.Text = "";
                    //txtOTApprDate.SetFocus();
                    return false;
                }

                if (txtOTApprNo.Text.Length < 6)
                {
                    txtOTApprNo.Text = "";
                    //txtOTApprNo.SetFocus();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 해당 선택된 결제수단
        /// 해단Control Focus되면 결제진행확인
        /// </summary>
        /// <returns></returns>
        bool CanStartPaymentProcess()
        {
            if (CardMode == PayCardMode.TelManualCard)
            {
                return txtApprovalNo.IsFocused;
            }

            if (ModeReturn)
            {
                if (m_cardBasketOrg != null)
                {
                    return txtExpiryDate.IsFocused;
                }

                return txtOTApprNo.IsFocused;
            }

            return txtInstallment.IsFocused;
        }

        /// <summary>
        /// ENTER key 처리
        /// </summary>
        /// <param name="e"></param>
        void ValidateOnKeyEvent(OPOSKeyEventArgs e)
        {
            // ENTER KEY
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
            {
                e.IsHandled = true;

                if (m_cardMode != PayCardMode.TelManualCard)
                {
                    StatusMessage = string.Empty;
                }
                else
                {   // TEST.osj 전화 승인 이면 승인번호 확인 추가
                    if (txtApprovalNo.Text.Length < 8)
                    {
                        txtApprovalNo.Text = "";
                        txtApprovalNo.SetFocus();
                        return;
                    }
                }

                // ENTER key인경우
                //if (!ValidateReadyForPayment())
                //{
                //    ValidateInputAndNext();
                //    return;
                //}

                if (txtCardNo.IsFocused)
                {
                    if (txtCardNo.Text.Length < MIN_CARD_NO_LEN || txtCardNo.Text.Length > MAX_CARD_NO_LEN)
                    {
                        txtCardNo.Text = "";
                        txtCardNo.SetFocus();
                        return;
                    }
                }
                else if (txtExpiryDate.IsFocused)
                {
                    var ret = ValidateYearMonth(txtExpiryDate.Text);
                    if (ret == 1)
                    {
                        txtExpiryDate.Text = "";
                        txtExpiryDate.SetFocus();
                        GuideMessage = MSG_ENTER_CARD_YM;
                        return;
                    }
                    else if (ret == 2)
                    {
                        txtExpiryDate.Text = "";
                        txtExpiryDate.SetFocus();
                        GuideMessage = MSG_CHECK_TEXT_LENGTH;
                        return;
                    }
                    else if (ret == 3)
                    {
                        txtExpiryDate.Text = "";
                        txtExpiryDate.SetFocus();
                        GuideMessage = MSG_CHECK_YM_INVALID;
                        return;
                    }
                    else if (ret == 4)
                    {
                        txtExpiryDate.Text = "";
                        txtExpiryDate.SetFocus();
                        GuideMessage = MSG_CHECK_YM_SMALL;
                        return;
                    }
                }
                else if (txtOTApprDate.IsFocused)
                {
                    if (this.ModeReturn && (txtOTApprDate.Text.Length != 8 || !DateTimeUtils.ValidateOrgApprDateSystem(txtOTApprDate.Text)))
                    {
                        txtOTApprDate.Text = "";
                        txtOTApprDate.SetFocus();
                        return;
                    }
                }
                else if (txtOTApprNo.IsFocused)
                {
                    if (this.ModeReturn && txtOTApprNo.Text.Length < 6)
                    {
                        txtOTApprNo.Text = "";
                        txtOTApprNo.SetFocus();
                        return;
                    }
                }
                else if (txtApprovalNo.IsFocused)
                {
                    if (CardMode == PayCardMode.TelManualCard && txtApprovalNo.Text.Length < 6)
                    {
                        txtApprovalNo.Text = "";
                        txtApprovalNo.SetFocus();
                        return;
                    }
                }

                if (CanStartPaymentProcess())
                {
                    if (txtCardNo.Text.Length < MIN_CARD_NO_LEN || txtCardNo.Text.Length > MAX_CARD_NO_LEN)
                    {
                        txtCardNo.Text = "";
                        txtCardNo.SetFocus();
                        return;
                    }

                    var ret = ValidateYearMonth(txtExpiryDate.Text);
                    if (ret == 1)
                    {
                        txtExpiryDate.Text = "";
                        txtExpiryDate.SetFocus();
                        GuideMessage = MSG_ENTER_CARD_YM;
                        return;
                    }
                    else if (ret == 2)
                    {
                        txtExpiryDate.Text = "";
                        txtExpiryDate.SetFocus();
                        GuideMessage = MSG_CHECK_TEXT_LENGTH;
                        return;
                    }
                    else if (ret == 3)
                    {
                        txtExpiryDate.Text = "";
                        txtExpiryDate.SetFocus();
                        GuideMessage = MSG_CHECK_YM_INVALID;
                        return;
                    }
                    else if (ret == 4)
                    {
                        txtExpiryDate.Text = "";
                        txtExpiryDate.SetFocus();
                        GuideMessage = MSG_CHECK_YM_SMALL;
                        return;
                    }

                    if (this.ModeReturn && (txtOTApprDate.Text.Length != 8 || !DateTimeUtils.ValidateOrgApprDateSystem(txtOTApprDate.Text)))
                    {
                        txtOTApprDate.Text = "";
                        txtOTApprDate.SetFocus();
                        return;
                    }

                    if (this.ModeReturn && txtOTApprNo.Text.Length < 6)
                    {
                        txtOTApprNo.Text = "";
                        txtOTApprNo.SetFocus();
                        return;
                    }

                    if (CardMode == PayCardMode.TelManualCard && txtApprovalNo.Text.Length < 6)
                    {
                        txtApprovalNo.Text = "";
                        txtApprovalNo.SetFocus();
                        return;
                    }

                    StartPaymentOperation();
                }
                else
                {
                    this.NextControl();
                }

                return;
            }

            // CLEAR KEY
            #region CLEAR KEY

            if (ModeProcessing == PaymentState.Errored)
            {
                // 오류있는경우 오류CLEAR
                if (m_lastError != VANRequestErrorType.None)
                {
                    m_lastError = VANRequestErrorType.None;
                }

                ModeProcessing = PaymentState.Ready;
            }

            // Navigate back or close form (close button)
            e.IsHandled = ProcessClearKey();
            #endregion
        }

        void ValidateInputAndNext()
        {
            #region 카드번호확인
            if (txtCardNo.IsFocused)
            {
                if (txtCardNo.Text.Length < MIN_CARD_NO_LEN)
                {
                    return;
                }
            }
            #endregion

            #region 유효기간확인
            if (txtExpiryDate.IsFocused)
            {
                var ret = ValidateYearMonth(txtExpiryDate.Text);
                if (ret == 1)
                {
                    GuideMessage = MSG_ENTER_CARD_YM;
                    return;
                }
                else if (ret == 2)
                {
                    GuideMessage = MSG_CHECK_TEXT_LENGTH;
                    return;
                }
                else if (ret == 3)
                {
                    GuideMessage = MSG_CHECK_YM_INVALID;
                    txtExpiryDate.Text = string.Empty;
                    return;
                }
                else if (ret == 4)
                {
                    GuideMessage = MSG_CHECK_YM_SMALL;
                    txtExpiryDate.Text = string.Empty;
                    return;
                }
            }
            #endregion

            #region 할부개월확인
            //if (txtInstallment.IsFocused)
            //{
            //    if (txtInstallment.Text.Length == 1)
            //    {
            //        return;
            //    }
            //}
            #endregion

            #region 원거래정보
            if (txtOTApprDate.IsFocused)
            {
                if (txtOTApprDate.Text.Length < 6)
                {
                    return;
                }

                if (!DateTimeUtils.ValidateOrgApprDateSystem(txtOTApprDate.Text))
                {
                    //StatusMessage = MSG_INPUT_OT_APPR_DATE_INVALID;
                    return;
                }
                else
                {
                    StatusMessage = string.Empty;
                }
            }
            if (txtOTApprNo.IsFocused)
            {
                if (txtOTApprNo.Text.Length < 6)
                {
                    return;
                }
            }
            #endregion

            #region 승인번호
            if (txtApprovalNo.IsFocused)
            {
                if (txtApprovalNo.Text.Length < 6)
                {
                    return;
                }
            }
            #endregion

            this.NextControl();
        }

        /// <summary>
        /// 초기상태인지?
        /// </summary>
        /// <returns></returns>
        bool IsInitialState()
        {
            return string.IsNullOrEmpty(txtCardNo.Text) &&
                string.IsNullOrEmpty(txtExpiryDate.Text) &&
                string.IsNullOrEmpty(txtOTApprDate.Text) &&
                string.IsNullOrEmpty(txtOTApprNo.Text) &&
                string.IsNullOrEmpty(txtApprovalNo.Text) && txtCardNo.IsFocused;
        }

        /// <summary>
        /// CLR키, 이전컨트롤이동
        /// </summary>
        /// <returns></returns>
        bool ProcessClearKey()
        {
            bool clearText = false;

            if (!m_keyInMode &&
                (txtCardNo.IsFocused || txtExpiryDate.IsFocused))
            {
                // clear all
                ModeProcessing = PaymentState.Ready;
                txtCardNo.Text = txtCardNm.Text = txtApprovalNo.Text = txtExpiryDate.Text =
                    txtInstallment.Text = string.Empty;
                _track2Data = string.Empty;
                txtCardNo.SetFocus();
                m_keyInMode = true;
                clearText = true;
            }

            if (this.FocusedControl != null)
            {
                InputText it = (InputText)this.FocusedControl;
                if (!string.IsNullOrEmpty(it.Text))
                {
                    it.Text = string.Empty;
                    clearText = true;
                }

                if (it.Name == txtCardNo.Name)
                {
                    txtCardNm.Text = txtExpiryDate.Text = txtInstallment.Text = string.Empty;
                    _track2Data = string.Empty;
                }
            }

            DataChanged = !IsInitialState();
            if (!DataChanged)
            {
                if (CardMode != m_initialCardMode || clearText)
                {
                    CardMode = m_initialCardMode;
                    return true;
                }

                // 자동반품시
                // 반품모드
                // 취소불가능일때
                // enter key 외에 못 함
                if (!m_cancellable)
                {
                    return true;
                }

                btnClose_Click(btnClose, EventArgs.Empty);
                return true;
            }

            if (!clearText)
            {
                this.PreviousControl();
            }

            return true;
        }

        #endregion

        #region DCC 자국통화 선택화면

        /// <summary>
        /// DCC 자국통화 선택화면
        /// </summary>
        /// <param name="respData"></param>
        void RequestDCCExRateSelection(PV21RespData respData)
        {
            // TEST
            m_signPopup.ResetSignPad();

            using (var pop = ChildManager.ShowPopup(string.Empty,
                "WSWD.WmallPos.POS.PY.dll", "WSWD.WmallPos.POS.PY.VC.POS_PY_P012",
                m_payAmt, ForeignCardTypeVisa(respData), respData))
            {
                var ret = pop.ShowDialog(this);

                if (ret == DialogResult.OK)
                {
                    bool selOtherCUR = !pop.ReturnResult.ContainsKey("SELECT_CUR") ? false :
                        "1".Equals(pop.ReturnResult["SELECT_CUR"].ToString());

                    // process DCC request;
                    dccReqData = new PV21ReqData();

                    #region DCC선택결과 생성

                    dccReqData.DCCCheckNo = respData.DCCCheckNo;
                    dccReqData.BaseCurCodeNo = "410"; // KRW 고정
                    dccReqData.BaseCurAmt = m_payAmt.ToString();
                    dccReqData.BaseCurAmtDecPoint = "0";
                    dccReqData.ExRate = respData.DCCExRate;
                    dccReqData.ExRateDecPoint = respData.DCCExRateDecPoint;
                    dccReqData.TrxnCurNo = selOtherCUR ? respData.DCCCurNo : "410";
                    dccReqData.TrxnCurCode = selOtherCUR ? respData.DCCCurCode : "KRW";
                    dccReqData.TrxnCurAmt = selOtherCUR ? respData.DCCCurAmt : m_payAmt.ToString();
                    dccReqData.TrxnCurAmtDecPoint = selOtherCUR ? respData.DCCCurDecPoint : "0";
                    dccReqData.TrxnWonAmt = m_payAmt.ToString();

                    dccReqData.NatCurNo = respData.DCCCurNo;
                    dccReqData.NatCurCode = respData.DCCCurCode;

                    dccReqData.DCCRvExRate = respData.DCCRvExRate;
                    dccReqData.DCCRvExRateDecPoint = respData.DCCRvExRateDecPoint;
                    dccReqData.DCCRvExRateUnt = respData.DCCRvExRateUnt;
                    dccReqData.MarkupPerc = respData.MarkupPerc;
                    dccReqData.MarkupPercUnt = respData.MarkupPercUnt;
                    dccReqData.ComsPercVal = respData.ComsPercVal;
                    dccReqData.ComsPercDecPoint = respData.ComsPercDecPoint;
                    dccReqData.ComsValCurNo = respData.ComsValCurNo;
                    dccReqData.ComsValCurAmt = respData.ComsValCurAmt;
                    dccReqData.ComsValCurMinUnt = respData.ComsValCurMinUnt;
                    dccReqData.ComsPrtYN = respData.ComsPercVal;
                    //dccReqData.RateId = respData.RateId;
                    //dccReqData.ExRateExpTime = respData.ExRateExpTime;

                    #endregion

                    RequestVANCardPayment(false, selOtherCUR ? "0" : "2", dccReqData, false);
                }
                else
                {
                    ModeProcessing = PaymentState.Ready;
                }
            }
        }

        /// <summary>
        /// 
        /// 여전법 변경 05.27
        /// PV01RespData > PV21RespData
        /// </summary>
        /// <param name="respData"></param>
        /// <returns></returns>
        bool ForeignCardTypeVisa(PV21RespData respData)
        {
            return CardNo.StartsWith("4");
            // return "03".Equals(respData.CardType2);
        }

        #endregion

        #region 여전법 2016.05 ~ 추가 이벤트처리

        /// <summary>
        /// 신용IC카드 삽입 시
        /// </summary>
        /// <param name="resCode"></param>
        /// <param name="resErrorMsg"></param>
        /// <param name="retCardInfo"></param>
        void SignPad_CardICReaderEvent(string resCode, string resErrorMsg, SignPadCardInfo retCardInfo)
        {

            //KSK_20170403
            if (m_itempKEY == 2)
            {
                m_itempKEY = 1;
                return;
            }


            // 준비 된 상태아니면 처리안함
            if (ModeProcessing == PaymentState.Processing ||
                ModeProcessing == PaymentState.PayCompleted)
            {
                return;
            }

            if (!resCode.Equals("00"))
            {
                LogUtils.Instance.LogByType("error", resErrorMsg);

                if (resCode.Equals("30") ||
                    resCode.Equals("31") ||
                    resCode.Equals("32"))
                {
                    // IC칩 카드 인식에 실패하였습니다.\r\n카드제거 후 신용카드 결제를 다시\r\n시도하여 주십시오.
                    ShowMessageBox(MessageDialogType.Error, string.Empty,
                        "IC칩 카드 인식에 실패하였습니다.\r\n카드제거 후 신용카드 결제를 다시\r\n시도하여 주십시오.");
                }
                else if ((resCode == "01" ||
                    resCode == "02" ||
                    resCode == "03" ||
                    resCode == "04" ||
                    resCode == "05" ||
                    resCode == "06" ||
                    resCode == "07"))
                {
                    #region 정상 FALLBACK 처리

                    // Fallback
                    // m_sRtn = m_dksNetdongle.EncReaderComCard("FB", l_sDate, string.Format("{0:d9}", m_icCardTransAmt), "10");
                    // 호출후 10초 기다림
                    string errorMsg = string.Empty;
                    string rtn = POSDeviceManager.SignPad.RequestFallBackComCard(out errorMsg, POS_PY_P021.WAIT_TIMER);
                    if (rtn != "00")
                    {
                        ShowMessageBox(MessageDialogType.Error, string.Empty, errorMsg);
                        return;
                    }

                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        // Showing waiting form
                        m_waitFallBackReadPop = (POS_PY_P021)ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.PY.dll",
                                    "WSWD.WmallPos.POS.PY.VC.POS_PY_P021");
                        m_isFallBackMode = true;
                        tmFallBackRead.Enabled = true;
                        var res = m_waitFallBackReadPop.ShowDialog();
                        if (res == DialogResult.Cancel)
                        {
                            ResetICCardReader(true);
                        }

                        m_waitFallBackReadPop.Dispose();
                        m_waitFallBackReadPop = null;

                        tmFallBackRead.Enabled = false;
                        m_isFallBackMode = false;
                    });

                    return;

                    #endregion
                }

                // Otherwise
                // ShowMessageBox(MessageDialogType.Error, string.Empty, resErrorMsg);
                // IC칩 카드 인식에 실패하였습니다.\r\n카드 제거 후 사인패드에 신용카드를\r\n리딩하여 주십시오.
                ShowMessageBox(MessageDialogType.Error, string.Empty,
                    "IC칩 카드 인식에 실패하였습니다.\r\n카드 제거 후 신용카드 결제를\r\n다시 시도하여 주십시오.");
                return;
            }

            /// SWIPE 일경우
            if (retCardInfo.CardType.Equals("MS"))
            {
                // IC카드 결제가 가능한 카드 입니다.\r\nIC카드 결제를 진행해 주세요.
                if (retCardInfo.ServiceCode.StartsWith("2") ||
                    retCardInfo.ServiceCode.StartsWith("6"))
                {
                    ShowMessageBox(MessageDialogType.Warning, string.Empty,
                        "IC카드 결제가 가능한 카드입니다.\r\nIC카드 결제를 진행해 주세요.");
                    return;
                }
            }


            #region 정상 FALLBACK MSR으로 카드 읽기

            // 정상 FALLBACK MSR으로 카드 읽기
            if (m_isFallBackMode || retCardInfo.CardType.Equals("FB"))
            {
                m_waitFallBackReadPop.SetEncCardReaderCompleted(true);
            }

            #endregion

            // 1. Show CardInfo on UI
            // 2. Change mode to MS or IC
            // this.CardMode = PayCardMode.CreditCard;
            CardICInfo = retCardInfo;
            Track2Data = retCardInfo.NoEncCardNo; // retCardInfo.EncCardNo;
            txtExpiryDate.Text = "****";

            m_addData.ENCCardNo = retCardInfo.EncCardNo;
            m_addData.ENCData = retCardInfo.EncData;
            m_addData.MaskCardNo = retCardInfo.NoEncCardNo;
            m_addData.TMLSerialNo = retCardInfo.Reader;

            // LOC사용 하기 위해 테스트 카드번호 넣는다
            // SetSampleCardData();

            // CHANGED KEYIN  0728
            m_keyInMode = retCardInfo.CardType == "KI";
            DataChanged = true;

            if (m_modeAutoRtn)
            {
                txtExpiryDate.SetFocus();
            }
            else
            {
                txtInstallment.SetFocus();
            }
        }

        /// <summary>
        /// RequestReaderCom2ndGen 호출 후
        /// 
        /// </summary>
        /// <param name="resCode"></param>
        /// <param name="resErrorMsg"></param>
        void SignPad_CardICApproveResult(string resCode, string resErrorMsg)
        {
            ShowProgressMessage(false);

            if (m_vanCancelReq2ndGen == VANCancelReq2ndGenerateType.ReqRdCom2ndGen_2stVANCancReq)
            {
                // 입력 reset
                ResetICCardReader(true);

                ModeProcessing = PaymentState.Ready;
                StatusMessage = "망취소가 되었습니다. \r\n신용카드 거래를 다시 시도하여 주세요.";
                m_vanCancelReq2ndGen = VANCancelReq2ndGenerateType.VANNormal;
            }
            else
            {
                if (resCode == "00")
                {
                    ProcessCompletePayment(m_tempRespData);
                }
                else
                {
                    // RequestReaderCom2ndGe 호출후 
                    // 정상 아닐 때
                    // 승인 요청 일때만
                    // 망취소 요청한다

                    resErrorMsg += Environment.NewLine;
                    resErrorMsg += "망취소 요청 합니다.";
                    ShowMessageBox(MessageDialogType.Error, string.Empty,
                        resErrorMsg);

                    // 또 취소 승인 요청 및 RequestReaderCom2ndGen
                    ProcessVANNetCancel();
                }
            }

        }

        /// <summary>
        /// FALLBACK 일경우 
        /// MSR 읽기 대기 타임아웃
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tmFallBackRead_Tick(object sender, EventArgs e)
        {
            if (m_waitFallBackReadPop != null)
            {
                var endRes = m_waitFallBackReadPop.SetIncTimeOut();
                if (endRes)
                {
                    tmFallBackRead.Enabled = false;
                    m_isFallBackMode = false;
                }
            }
        }

        /// <summary>
        /// 여전법 추가 0620
        /// 강제취소 버튼
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnForceClose_Click(object sender, EventArgs e)
        {
            var res = ShowMessageBox(MessageDialogType.Question, string.Empty,
                            MSG_CONFIRM_FORCE_CANC, new string[] { LABEL_FORCE, LABEL_CLOSE });

            if (res == DialogResult.Yes)
            {
                // 강제취소 일때
                // basket null으로 return함
                this.ReturnResult.Add("ERROR_CODE", "FORCE_CANC");
                this.ReturnResult.Add("ERROR_MSG", "강제취소");
                this.DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// 여전법 추가 0620
        /// 전화승인 시
        ///     KEYIN일경우
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void KeyInRead_Click(object sender, EventArgs e)
        {
            if (btnRdType != null)
            {
                btnRdType.Selected = false;
            }

            //KSK_20170403
            if (btnRdType == btnRdKEYIN)
            {
                m_itempKEY = 2;
                btnRdType.Selected = false;

                POSDeviceManager.SignPad.ResetICCardReader();

                btnRdType = btnReadIC;
                btnRdType.Name = "IC";

            }
            else
            {
                btnRdType.Name = "KI";

                // 신용카드모드
                POSDeviceManager.SignPad.ClearPinDataRequest();

                var c = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)sender;
                btnRdType = c;
                c.Selected = true;

                // 여전법 추가 0728
                // Send request to Dongle
                // Added 0728
                POSDeviceManager.SignPad.RequestEncReaderCard(btnReadIC.Name.Equals(btnRdType.Name) ? "IC" : "KI");

            }

        }

        /// <summary>
        /// 신용 IC 카드정보 표시
        /// </summary>
        /// <param name="value"></param>
        private void UpdateCardICInfo(SignPadCardInfo value)
        {
            txtCardNo.Text = value.NoEncCardNo;
            // txtCardNm
        }

        /// <summary>
        /// 여전법 추가 0529
        /// </summary>
        private void InitSignPopup(bool close)
        {
            if (m_signPopup != null)
            {
                m_signPopup.Dispose();
                m_signPopup = null;
            }

            if (!close)
            {
                m_signPopup = (POS_PY_P002)ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.PY.dll",
                    "WSWD.WmallPos.POS.PY.VC.POS_PY_P002", m_payAmt);
            }

        }

        /// <summary>
        /// 여전법 추가0620
        /// ICCard Reader reset한다
        /// </summary>
        /// <param name="resetData"></param>
        private void ResetICCardReader(bool resetData)
        {
            // 입력 reset
            POSDeviceManager.SignPad.ResetICCardReader();
            if (resetData)
            {
                m_icCardInfo = null;
                Track2Data = string.Empty;
                txtCardNo.Text = string.Empty;
                txtExpiryDate.Text = string.Empty;
                if (!m_modeAutoRtn)
                {
                    txtInstallment.Text = string.Empty;
                }

                m_keyInMode = false;
                CardMode = m_initialCardMode;
                DataChanged = false;

                txtCardNo.SetFocus();
            }
        }

        /// <summary>
        /// 여전법 추가 0615
        /// 개인정보 삭제, 메모리 삭제..등
        /// 
        /// 1. 삭제 대상 : 암호화된 카드 번호와 EMV Data는 신용카드 화면 
        /// Close 시점에 삭제 처리 해야 한다.
        /// 2. LOG 삭제 : 모든 LOG에는 Masking된 카드번호만 사용 할 수 있다
        /// (암호화된 카드번호 및 EMV DATA LOG 저장 안됨)
        /// 3. 암호화된 카드번호 및 EMV Data 등의 메모리 삭제 시점은 신용카드 창이 
        /// Close 되는 시점이다.
        /// 4. Memory 초기화 방법은 "1" → "0" → NULL로 세팅하여 초기화 한다.
        /// </summary>
        void ClearSecureData()
        {
            /*
            _track2Data
            txtCardNo.Text
            m_erCardPin
            m_cardBasket
            m_cardBasketOrg
            m_addData
            m_icCardInfo
            m_tempRespData
             * 결제 거래 완료 시,
             * basket도 지움
             * 
            */
            _track2Data.ResetZero();
            txtCardNo.Text.ResetZero();
            m_erCardPin.ResetZero();

            if (m_addData != null)
            {
                m_addData.ENCCardNo.ResetZero();
                m_addData.ENCData.ResetZero();
                m_addData.MaskCardNo.ResetZero();
                m_addData = null;
            }

            if (m_icCardInfo != null)
            {
                m_icCardInfo.EncCardNo.ResetZero();
                m_icCardInfo.NoEncCardNo.ResetZero();
                m_icCardInfo.EncData.ResetZero();
                m_icCardInfo = null;
            }

            if (m_tempRespData != null)
            {
                m_tempRespData.ENCCardNo.ResetZero();
                m_tempRespData.ENCData.ResetZero();
                m_tempRespData.MaskCardNo.ResetZero();
                m_tempRespData = null;
            }

            if (m_cardBasket != null)
            {
                m_cardBasket.CardNo.ResetZero();
                m_cardBasket.TrackII.ResetZero();
                m_cardBasket = null;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #endregion

    }
}

public enum VANCancelReq2ndGenerateType
{
    /// <summary>
    /// 정상일때
    /// </summary>
    VANNormal,
    /// <summary>
    /// 1st 2ndGenerate 거절일때
    /// </summary>
    ReqRdCom2ndGen_1st,
    /// <summary>
    /// 망취소 요청 후,
    /// 2ndGenerate 호출
    /// </summary>
    ReqRdCom2ndGen_2stVANCancReq
}

/// <summary>
/// VAN사 요청종류
/// </summary>
public enum CardVANReqType
{
    /// <summary>
    /// 승인 요청
    /// </summary>
    ReqAppr,

    /// <summary>
    /// 취소요청
    /// </summary>
    ReqCanc
}


