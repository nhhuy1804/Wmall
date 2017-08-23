//---------------------------------*--------------------------------
/*
 * 화면명   : POS_PY_P014.cs
 * 화면설명 : 현금영수증
 * 개발자   : TCL
 * 개발일자 : 2015.04.21
*/
//-----------------------------------------------------------------

using System;
using System.Windows.Forms;

using WSWD.WmallPos.POS.PY.PI;
using WSWD.WmallPos.POS.PY.PT;
using WSWD.WmallPos.POS.PY.VI;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.POS.PY.Data;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PV;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PV;

namespace WSWD.WmallPos.POS.PY.VC
{
    public partial class POS_PY_P014 : PopupBase01, IPYP014View
    {
        #region 변수

        /// <summary>
        /// 발급안함
        /// </summary>
        public const int CASHRCP_TYPE_NONE = -1;
        /// <summary>
        /// 자진발금
        /// </summary>
        public const int CASHRCP_TYPE_SELF = 0;
        /// <summary>
        /// 소득공제
        /// </summary>
        public const int CASHRCP_TYPE_DEDUCTION = 1;
        /// <summary>
        /// 지출증빙
        /// </summary>
        public const int CASHRCP_TYPE_EVIDENCE = 2;

        //비즈니스 로직
        private IPYP014presenter m_presenter;

        /// <summary>
        /// 공제방법
        /// </summary>
        WSWD.WmallPos.POS.FX.Win.UserControls.Button m_cashTypeSelButton = null;

        /// <summary>
        /// 대상금액
        /// </summary>
        private int m_cashAmt;

        /// <summary>
        /// 세금
        /// </summary>
        private int m_taxAmt;

        /// <summary>
        /// Loc added on 10.24
        /// 전문추가정보
        /// </summary>
        private PV21ReqDataAdd m_addData = null;

        /// <summary>
        /// 0: 자진발급
        /// 1: 개인소득공제
        /// 2: 사업자(지출증비)
        /// </summary>
        private int m_crPayType = CASHRCP_TYPE_SELF;

        /// <summary>
        /// Swipe
        /// </summary>
        private bool m_swipe = false;

        private string m_readCardTrack = string.Empty;
        private string ReadCardTrack
        {
            get
            {
                return string.IsNullOrEmpty(m_readCardTrack) ? ConfirmNo : m_readCardTrack;
            }
        }

        private string m_confirmNo = string.Empty;
        private string ConfirmNo
        {
            get
            {
                return m_crPayType == CASHRCP_TYPE_SELF ? "0100001234" : string.Copy(m_confirmNo);
            }
        }

        /// <summary>
        /// 처리중인상태
        /// </summary>
        private bool m_modeProcessing = false;
        private bool ModeProcessing
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
                        btnClose.Enabled = !value;
                        btnSelf.Enabled = !value;
                    });
                }
                else
                {
                    btnClose.Enabled = !value;
                    btnSelf.Enabled = !value;
                }
            }
        }

        /// <summary>
        /// 여전법 추가 0618
        /// m_rdConfNoType
        /// </summary>
        WSWD.WmallPos.POS.FX.Win.UserControls.Button m_rdConfNoType = null;

        #endregion

        #region 생성자

        /// <summary>
        /// Loc added on 10.24
        /// 전문추가정보
        /// 현금영수증
        /// </summary>
        /// <param name="cashAmt"></param>
        /// <param name="taxAmt"></param>
        /// <param name="addData"></param>
        public POS_PY_P014(int cashAmt, int taxAmt, PV21ReqDataAdd addData)
        {
            InitializeComponent();

            //대상금액
            m_cashAmt = cashAmt;
            m_taxAmt = taxAmt;
            m_addData = addData;

            if (m_addData == null)
            {
                m_addData = new PV21ReqDataAdd();
            }

            //Form Load Event
            Load += new EventHandler(form_Load);
            FormClosed += new FormClosedEventHandler(POS_PY_P014_FormClosed);

            // 초기화
            InitControls();
        }

        void POS_PY_P014_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Load -= new EventHandler(form_Load);

            if (POSDeviceManager.SignPad.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
            {
                POSDeviceManager.SignPad.PinEvent -= new POSDataEventHandler(SignPad_PinEvent);
            }
            POSDeviceManager.SignPad.Close();

            // 여전법 변경 주석처리 0605
            // 사용안함
            // POSDeviceManager.Msr.DataEvent -= new POSMsrDataEventHandler(Msr_DataEvent);

            this.KeyEvent -= new OPOSKeyEventHandler(POS_PY_P014_KeyEvent);     //KeyEvent
            this.txtConfirmNo.KeyEvent -= new OPOSKeyEventHandler(txtConfirmNo_KeyEvent);
            this.btnDeduction.Click -= new EventHandler(CRPayType_Click);
            this.btnEvidence.Click -= new EventHandler(CRPayType_Click);

            this.btnSelf.Click -= new EventHandler(btnSelf_Click);
            this.btnClose.Click -= new EventHandler(btnClose_Click);

            this.txtConfirmNo.InputFocused -= new EventHandler(txtConfirmNo_InputFocused);

            this.FormClosed -= new FormClosedEventHandler(POS_PY_P014_FormClosed);

            // 여전법 추가 0617
            // 신용카드 & 식별번호 일기
            this.btnRdICCard.Click -= new EventHandler(ConfirmNoRdBtn_Click);
            this.btnRdConfirmNo.Click -= new EventHandler(ConfirmNoRdBtn_Click);

            // ICCard reading 
            POSDeviceManager.SignPad.CardICReaderEvent -= new POSCardICOnEncCardReader(SignPad_CardICReaderEvent);

            // 여전법 추가 0621
            ClearSecureData();
        }

        #endregion

        #region 이벤트 등록

        /// <summary>
        /// Form Event 정의
        /// </summary>
        private void InitEvent()
        {
            this.KeyEvent += new OPOSKeyEventHandler(POS_PY_P014_KeyEvent);     //KeyEvent
            this.txtConfirmNo.KeyEvent += new OPOSKeyEventHandler(txtConfirmNo_KeyEvent);
            this.btnDeduction.Click += new EventHandler(CRPayType_Click);
            this.btnEvidence.Click += new EventHandler(CRPayType_Click);

            this.btnSelf.Click += new EventHandler(btnSelf_Click);
            this.btnClose.Click += new EventHandler(btnClose_Click);

            this.txtConfirmNo.InputFocused += new EventHandler(txtConfirmNo_InputFocused);

            // 여전법 추가 0617
            // 신용카드 & 식별번호 일기
            this.btnRdICCard.Click += new EventHandler(ConfirmNoRdBtn_Click);
            this.btnRdConfirmNo.Click += new EventHandler(ConfirmNoRdBtn_Click);

            POSDeviceManager.SignPad.CardICReaderEvent += new POSCardICOnEncCardReader(SignPad_CardICReaderEvent);
            POSDeviceManager.SignPad.SetICTransAmount(m_cashAmt);
        }

        /// <summary>
        /// Controls 값설정
        /// </summary>
        private void InitControls()
        {
            // btnSelf.Tag = 0;

            btnSelf.Tag = "0".Equals(ConfigData.Current.AppConfig.PosOption.CashReceiptIssue) ? CASHRCP_TYPE_NONE : CASHRCP_TYPE_SELF;
            btnSelf.Text = "0".Equals(ConfigData.Current.AppConfig.PosOption.CashReceiptIssue) ? LABEL_CLOSE : LABEL_SELF;

            //btnClose.Tag = "0".Equals(ConfigData.Current.AppConfig.PosOption.CashReceiptIssue) ? CASHRCP_TYPE_NONE : CASHRCP_TYPE_SELF;
            //btnClose.Text = "0".Equals(ConfigData.Current.AppConfig.PosOption.CashReceiptIssue) ? LABEL_CLOSE : LABEL_SELF;

            btnDeduction.Tag = CASHRCP_TYPE_DEDUCTION;
            btnEvidence.Tag = CASHRCP_TYPE_EVIDENCE;

            txtType.Text = MSG_CASH;

            //대상금액
            txtAmt.Text = m_cashAmt.ToString();

            //정보 조회
            m_presenter = new PYP014presenter(this);

            // 소득공제선택
            CRPayType_Click(btnDeduction, EventArgs.Empty);

            // 자진발급
            // 설정에 자진발급
            //btnSelf.Visible = "1".Equals(ConfigData.Current.AppConfig.PosOption.CashReceiptIssue);
        }

        /// <summary>
        /// 장비초기화
        /// - SIGNPAD (빈법입력)
        /// - MSR
        /// 
        /// </summary>
        void InitDevices()
        {
            POSDeviceManager.SignPad.Initialize(axKSNet_Dongle1);
            if (POSDeviceManager.SignPad.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
            {
                StatusMessage = string.Empty;
                POSDeviceManager.SignPad.PinEvent += new POSDataEventHandler(SignPad_PinEvent);

                // 여전법 추가 0617
                // 기본선택이 일반식별번호 버튼
                m_rdConfNoType = btnRdConfirmNo;
                POSDeviceManager.SignPad.RequestPinData(MSG_INPUT_CONFIRM_NO_SIGNPAD,
                    string.Empty, string.Empty, string.Empty, 1, 13);
            }
            else
            {
                StatusMessage = MSG_SIGNPAD_INIT_ERROR;
            }

            // 여전법 변경 주석처리 0605
            // POSDeviceManager.Msr.DataEvent += new POSMsrDataEventHandler(Msr_DataEvent);
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

            InitDevices();
        }


        /// <summary>
        /// Key Event
        /// </summary>
        /// <param name="e"></param>
        void POS_PY_P014_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (ModeProcessing)
            {
                e.IsHandled = true;
                return;
            }

            if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
            {
                if (ValidateValidBusinessNo())
                {
                    e.IsHandled = true;
                    ProcessCashReceipt();
                }
                else
                {
                    StatusMessage = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("01390");
                }
            }
        }

        void txtConfirmNo_KeyEvent(OPOSKeyEventArgs e)
        {
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
            {
                POSDeviceManager.SignPad.RequestPinData(MSG_INPUT_CONFIRM_NO_SIGNPAD,
                    string.Empty, string.Empty, string.Empty,
                    1, 13);
                m_readCardTrack = string.Empty;
                m_confirmNo = string.Empty;
            }

            if (!e.IsControlKey)
            {
                m_swipe = false;
                m_readCardTrack = string.Empty;
                m_confirmNo = txtConfirmNo.Text;
            }

            if (txtConfirmNo.Text.Length <= 0)
            {
                m_swipe = false;
            }
        }

        void SignPad_PinEvent(string eventData)
        {
            if (ModeProcessing)
            {
                return;
            }

            txtConfirmNo.Text = MaskPrivateData(eventData);
            m_confirmNo = eventData;
            m_readCardTrack = string.Empty;
            m_swipe = false;
        }

        /// <summary>
        /// 여전법 변경 
        /// 주석처리 0605
        /// 사용 안함
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="cardNo"></param>
        /// <param name="expMY"></param>
        void Msr_DataEvent(string eventData, string cardNo, string expMY)
        {
            if (ModeProcessing)
            {
                return;
            }

            m_readCardTrack = eventData;
            txtConfirmNo.Text = MaskPrivateData(cardNo);
            m_confirmNo = cardNo;
            m_swipe = true;
        }

        #region 사용안함

        /// <summary>
        /// 자진발급
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSelf_Click(object sender, EventArgs e)
        {
            m_crPayType = (int)((Control)sender).Tag;
            ProcessCashReceipt();
        }

        #endregion

        /// <summary>
        /// 닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            //m_crPayType = (int)((Control)sender).Tag;
            //ProcessCashReceipt();
        }

        /// <summary>
        /// 소득공제/지출증빙
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CRPayType_Click(object sender, EventArgs e)
        {
            if (m_cashTypeSelButton != null)
            {
                m_cashTypeSelButton.Selected = false;
            }

            var c = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)sender;
            m_cashTypeSelButton = c;
            c.Selected = true;

            m_crPayType = (int)c.Tag;
            txtBusinessNo.Text = (m_crPayType == CASHRCP_TYPE_DEDUCTION ? MSG_ID_TYPE_IND : MSG_ID_TYPE_BIZ) + "(" + c.Text + ")";
            txtType.Text = MSG_CASH + "(" + c.Text + ")";
            txtConfirmNo.SetFocus();
        }

        void txtConfirmNo_InputFocused(object sender, EventArgs e)
        {
            messageBar1.Text = MSG_INPUT_CONFIRM_NO;
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// Loc added 10.27
        /// 사업자지출증빙일경우 
        /// 확인번호는 WMALL 사업자등록번호이면 안 됨
        /// </summary>
        /// <returns></returns>
        bool ValidateValidBusinessNo()
        {
            if (m_crPayType == CASHRCP_TYPE_EVIDENCE)
            {
                return !ConfirmNo.Equals("1198179493");
            }

            return true;
        }

        /// <summary>
        /// 현금영수증처리
        /// </summary>
        void ProcessCashReceipt()
        {
            if (m_crPayType == CASHRCP_TYPE_NONE)
            {
                this.DialogResult = DialogResult.OK;
                return;
            }

            if (m_crPayType != CASHRCP_TYPE_SELF)
            {
                // validation
                if (txtConfirmNo.Text.Length < 10)
                {
                    txtConfirmNo.SetFocus();
                    return;
                }
            }
            else
            {
                //2015.09.09 정광호 추가
                //카드를 읽고 카드번호가 존재하는 상태에서 
                //자진발급 버튼을 누르면 자진발급이 아닌 카드번호로 전문통신하기때문에 추가
                if (m_swipe)
                {
                    m_swipe = false;
                    m_readCardTrack = "";
                }

            }

            if (ModeProcessing)
            {
                return;
            }

            btnClose.Enabled = btnSelf.Enabled = false;
            ModeProcessing = true;

            // 밴사연동          
            StatusMessage = string.Empty;

            // Loc changed on 10.24
            m_presenter.MakeCashRecptRequest(m_crPayType, m_swipe, ReadCardTrack, m_cashAmt, m_taxAmt, m_addData);
        }

        /// <summary>
        /// Progress form
        /// </summary>
        /// <param name="showProgress"></param>
        public void ShowProgressMessage(bool showProgress)
        {
            ChildManager.ShowProgress(showProgress, MSG_VAN_REQ_PROCESSING);
        }

        /// <summary>
        /// 오류메시지
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="errorMessage"></param>
        /// <param name="errorCode"></param>
        /// <param name="viewTag"></param>
        public void ShowErrorMessage(VANRequestErrorType errorType, string errorMessage, string errorCode, string viewTag)
        {
            ShowProgressMessage(false);

            //2015.09.30 정광호 수정---------------------------------
            //모든 에러에 대해서 메세지 가이드대신 팝업 활성화
            //bool showErrorMsg = false;
            bool showErrorMsg = true;
            //-------------------------------------------------------

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
                    showErrorMsg = true;
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

            m_crPayType = btnDeduction.Selected ? CASHRCP_TYPE_DEDUCTION : CASHRCP_TYPE_EVIDENCE;
            StatusMessage = message;

            //2015.09.30 정광호 수정---------------------------------
            //모든 에러에 대해서 메세지 가이드대신 팝업 활성화
            //StatusMessage = message;
            //-------------------------------------------------------

            if (showErrorMsg)
            {
                message += Environment.NewLine;
                message += MSG_COMM_ERROR_RETRY;

                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        var res = ShowMessageBox(MessageDialogType.Question, string.Empty,
                    message, new string[] {
                        LABEL_RETRY, LABEL_CLOSE
                    });
                        if (res != DialogResult.Yes)
                        {
                            this.DialogResult = DialogResult.OK;
                        }
                    });
                }
                else
                {
                    var res = ShowMessageBox(MessageDialogType.Question, string.Empty,
                    message, new string[] {
                        LABEL_RETRY, LABEL_CLOSE
                    });
                    if (res != DialogResult.Yes)
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                }

            }

            ModeProcessing = false;

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    btnSelf.Enabled = btnClose.Enabled = true;
                });
            }
            else
            {
                btnSelf.Enabled = btnClose.Enabled = true;
            }
            //btnSelf.Enabled = btnClose.Enabled = true;
        }
        public void OnReturnSuccess(PV02RespData respData)
        {
            StatusMessage = respData.RespMessage1;

            // 기본 자진발금, 휴대폰
            string fgIDCheck = "2";
            if (m_crPayType != CASHRCP_TYPE_SELF)
            {
                if (m_swipe)
                {
                    fgIDCheck = "1";
                }
                else if (m_confirmNo.Length == 11)
                {
                    fgIDCheck = "2";
                }
                else if (m_confirmNo.Length == 13)
                {
                    fgIDCheck = "3";
                }
                else
                {
                    fgIDCheck = m_crPayType == CASHRCP_TYPE_DEDUCTION || IsMobilePhone(m_confirmNo) ? "2" : "4";
                }
            }

            var cashBasket = new BasketCashRecpt()
            {
                AmAppr = m_cashAmt.ToString(),
                AmTax = m_taxAmt.ToString(),
                CdCancRsn = string.Empty,
                CdVan = respData.ApprVanCode,
                DdAppr = respData.ApprDate,
                FgAppr = "1",
                FgIDCheck = fgIDCheck,
                FgSelf = m_crPayType == CASHRCP_TYPE_SELF ? "1" : "0",
                FgTrxnType = (m_crPayType == CASHRCP_TYPE_SELF || m_crPayType == CASHRCP_TYPE_DEDUCTION) ? "1" : "2",
                InputWcc = m_swipe ? "A" : "@",
                NoAppr = respData.ApprNo,
                NoPersonal = ConfirmNo,
                NoTrack = m_readCardTrack,
                TmAppr = respData.ApprTime
            };

            if (!this.ReturnResult.ContainsKey("PAY_DATA"))
            {
                this.ReturnResult.Add("PAY_DATA", cashBasket);
            }

            this.DialogResult = DialogResult.OK;
        }

        bool IsMobilePhone(string confirmNo)
        {
            return confirmNo.StartsWith("010") ||
                confirmNo.StartsWith("011") ||
                confirmNo.StartsWith("016") ||
                confirmNo.StartsWith("017") ||
                confirmNo.StartsWith("019");
        }

        /// <summary>
        /// 개인정보 masking
        /// </summary>
        /// <param name="privateData"></param>
        /// <returns></returns>
        string MaskPrivateData(string privateData)
        {
            //if (privateData.Length > 4)
            //{
            //    return new string('*', privateData.Length - 4) + privateData.Substring(privateData.Length - 4);
            //}

            return privateData;
        }

        #endregion

        #region 여전법 추가 0617

        /// <summary>
        /// ConfirmNo reading type
        /// - 신용카드 방법 (IC / MSR)
        /// - 식별번호입력, KEYBOARD 또는 동글
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ConfirmNoRdBtn_Click(object sender, EventArgs e)
        {
            var c = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)sender;
            if (m_rdConfNoType != null)
            {
                if (m_rdConfNoType.Name.Equals(c.Name))
                {
                    return;
                }

                m_rdConfNoType.Selected = false;
            }

            m_rdConfNoType = c;
            c.Selected = true;
            txtConfirmNo.Text = string.Empty;

            // 신용카드모드
            POSDeviceManager.SignPad.ClearPinDataRequest();
            // POSDeviceManager.SignPad.ResetICCardReader();

            if (!m_rdConfNoType.Name.Equals(btnRdICCard.Name))
            {
                POSDeviceManager.SignPad.RequestPinData(MSG_INPUT_CONFIRM_NO_SIGNPAD,
                    string.Empty, string.Empty, string.Empty, 1, 13);
            }
        }

        void SignPad_CardICReaderEvent(string resCode, string resErrorMsg, SignPadCardInfo retCardInfo)
        {
            if (!resCode.Equals("00"))
            {
                LogUtils.Instance.LogByType("error", resErrorMsg);

                // Otherwise
                // ShowMessageBox(MessageDialogType.Error, string.Empty, resErrorMsg);
                // IC칩 카드 인식에 실패하였습니다.\r\n카드 제거 후 사인패드에 신용카드를\r\n리딩하여 주십시오.
                ShowMessageBox(MessageDialogType.Error, string.Empty,
                    "IC칩 카드 인식에 실패하였습니다.\r\n카드 제거 후 신용카드를\r\n다시 리딩하여 주십시오.");
                return;
            }

            /// SWIPE 일경우
            if (!retCardInfo.CardType.Equals("MS"))
            {
                // 카드를 MSR Reading 하세요
                ShowMessageBox(MessageDialogType.Error, string.Empty,
                    "카드를 MSR Reading 하세요");
                return;
            }

            // 1. Show CardInfo on UI
            // 2. Store Read Card Info
            txtConfirmNo.Text = retCardInfo.NoEncCardNo;
            m_confirmNo = retCardInfo.NoEncCardNo;
            m_swipe = true;

            m_addData.ENCCardNo = retCardInfo.EncCardNo;
            m_addData.ENCData = retCardInfo.EncData;
            m_addData.MaskCardNo = retCardInfo.NoEncCardNo;
            m_addData.TMLSerialNo = retCardInfo.Reader;
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

            if (m_addData != null)
            {
                m_addData.ENCCardNo.ResetZero();
                m_addData.ENCData.ResetZero();
                m_addData.MaskCardNo.ResetZero();
            }

            m_confirmNo.ResetZero();
            txtConfirmNo.Text.ResetZero();
        }

        #endregion
    }
}