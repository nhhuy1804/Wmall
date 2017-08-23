//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P015.cs
 * 화면설명 : 현금영수증 취소
 * 개발자   : TCL
 * 개발일자 : 2015.05.28
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
using WSWD.WmallPos.POS.FX.Win.Controls;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using WSWD.WmallPos.POS.PY.Data;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PV;
using WSWD.WmallPos.POS.FX.Win.Data;
using System.Globalization;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PV;

namespace WSWD.WmallPos.POS.PY.VC
{
    public partial class POS_PY_P015 : PopupBase01, IPYP015View
    {
        #region 변수

        //비즈니스 로직
        private IPYP015presenter m_presenter;

        WSWD.WmallPos.POS.FX.Win.UserControls.Button m_selButton = null;

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
        /// 
        /// 여전볍 변경 05.27
        /// PV11ReqDataAdd > PV21ReqDataAdd 
        /// </summary>
        private PV21ReqDataAdd m_addData = null;

        /// <summary>
        /// 0: 자진발급
        /// 1: 개인소득공제
        /// 2: 사업자(지출증비)
        /// </summary>
        private int m_crPayType = POS_PY_P014.CASHRCP_TYPE_NONE;

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
                return m_crPayType == POS_PY_P014.CASHRCP_TYPE_SELF ? "0100001234" : string.Copy(m_confirmNo);
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

        #region 생성자, 초기화

        /// <summary>
        /// 현금영수증
        /// Loc changed on 10.24
        /// 전문추가정보, 생성자에 추가
        /// 
        /// 
        /// 여전볍 변경 05.27
        /// PV11ReqDataAdd > PV21ReqDataAdd 
        /// </summary>
        /// <param name="cashAmt"></param>
        /// <param name="taxAmt"></param>
        /// <param name="addData"></param>
        public POS_PY_P015(int cashAmt, int taxAmt, PV21ReqDataAdd addData)
        {
            InitializeComponent();

            //대상금액
            m_cashAmt = cashAmt;
            m_taxAmt = taxAmt;
            m_addData = addData;

            // 여전법 추가 0621
            if (m_addData == null)
            {
                m_addData = new PV21ReqDataAdd();
            }

            // 자동반품시, 원거래정보를 설정해주고 
            // 수정불가
            if (m_addData.IsAutoReturn)
            {
                txtOTApprDate.Text = m_addData.DdApprOrg;
                txtOTApprNo.Text = m_addData.NoApprOrg;
                txtOTApprDate.ReadOnly = txtOTApprNo.ReadOnly = true;
            }

            // Form Events
            this.Load += new EventHandler(POS_PY_P015_Load);
            this.FormClosed += new FormClosedEventHandler(POS_PY_P015_FormClosed);

            // 초기화
            InitControls();
        }

        /// <summary>
        /// Controls 값설정
        /// </summary>
        private void InitControls()
        {
            btnSelf.Tag = POS_PY_P014.CASHRCP_TYPE_SELF;
            btnDeduction.Tag = POS_PY_P014.CASHRCP_TYPE_DEDUCTION;
            btnEvidence.Tag = POS_PY_P014.CASHRCP_TYPE_EVIDENCE;

            txtType.Text = MSG_CASH;

            //대상금액
            txtAmt.Text = m_cashAmt.ToString();

            //정보 조회
            m_presenter = new PYP015presenter(this);

            // 소득공제선택
            CRPayType_Click(btnDeduction, EventArgs.Empty);
        }

        /// <summary>
        /// Form Event 정의
        /// </summary>
        private void InitEvent()
        {
            this.KeyEvent += new OPOSKeyEventHandler(POS_PY_P015_KeyEvent);     //KeyEvent
            this.txtConfirmNo.KeyEvent += new OPOSKeyEventHandler(txtConfirmNo_KeyEvent);
            this.btnDeduction.Click += new EventHandler(CRPayType_Click);
            this.btnEvidence.Click += new EventHandler(CRPayType_Click);

            this.btnSelf.Click += new EventHandler(btnSelf_Click);
            this.btnClose.Click += new EventHandler(btnClose_Click);

            this.txtConfirmNo.InputFocused += new EventHandler(txtConfirmNo_InputFocused);
            this.txtOTApprDate.InputFocused += new EventHandler(txtOTApprDate_InputFocused);
            this.txtOTApprNo.InputFocused += new EventHandler(txtOTApprNo_InputFocused);
            this.txtCancReasn.InputFocused += new EventHandler(txtCancReasn_InputFocused);
            
            // 여전법 추가 0617
            // 신용카드 & 식별번호 일기
            this.btnRdICCard.Click += new EventHandler(ConfirmNoRdBtn_Click);
            this.btnRdConfirmNo.Click += new EventHandler(ConfirmNoRdBtn_Click);

            POSDeviceManager.SignPad.CardICReaderEvent += new POSCardICOnEncCardReader(SignPad_CardICReaderEvent);
            POSDeviceManager.SignPad.SetICTransAmount(m_cashAmt);
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
                    string.Empty, string.Empty, string.Empty,
                    1, 13);
            }
            else
            {
                StatusMessage = MSG_SIGNPAD_INIT_ERROR;
            }

            // POSDeviceManager.Msr.DataEvent += new POSMsrDataEventHandler(Msr_DataEvent);
        }

        #endregion

        #region 이벤트정의

        void POS_PY_P015_Load(object sender, EventArgs e)
        {
            // 자진발급
            // 설정에 자진발급
            btnSelf.Visible = !"0".Equals(ConfigData.Current.AppConfig.PosOption.CashReceiptIssue.Trim());
            btnClose.Tag = POS_PY_P014.CASHRCP_TYPE_NONE;
            if (!btnSelf.Visible)
            {
                btnClose.Left = 240;
            }
            else
            {
                btnClose.Left = 307;
            }

            //이벤트 등록
            InitEvent();

            // SIGNPAD
            InitDevices();
        }

        void POS_PY_P015_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Load -= new EventHandler(POS_PY_P015_Load);
            this.FormClosed -= new FormClosedEventHandler(POS_PY_P015_FormClosed);

            this.KeyEvent -= new OPOSKeyEventHandler(POS_PY_P015_KeyEvent);     //KeyEvent
            this.txtConfirmNo.KeyEvent -= new OPOSKeyEventHandler(txtConfirmNo_KeyEvent);
            this.btnDeduction.Click -= new EventHandler(CRPayType_Click);
            this.btnEvidence.Click -= new EventHandler(CRPayType_Click);

            this.btnSelf.Click -= new EventHandler(btnSelf_Click);
            this.btnClose.Click -= new EventHandler(btnClose_Click);

            this.txtConfirmNo.InputFocused -= new EventHandler(txtConfirmNo_InputFocused);
            this.txtOTApprDate.InputFocused -= new EventHandler(txtOTApprDate_InputFocused);
            this.txtOTApprNo.InputFocused -= new EventHandler(txtOTApprNo_InputFocused);
            this.txtCancReasn.InputFocused -= new EventHandler(txtCancReasn_InputFocused);

            POSDeviceManager.SignPad.PinEvent -= new POSDataEventHandler(SignPad_PinEvent);

            // POSDeviceManager.Msr.DataEvent -= new POSMsrDataEventHandler(Msr_DataEvent);
            POSDeviceManager.SignPad.Close();

            // 여전법 추가 0617
            // 신용카드 & 식별번호 일기
            this.btnRdICCard.Click -= new EventHandler(ConfirmNoRdBtn_Click);
            this.btnRdConfirmNo.Click -= new EventHandler(ConfirmNoRdBtn_Click);

            // ICCard reading 
            POSDeviceManager.SignPad.CardICReaderEvent -= new POSCardICOnEncCardReader(SignPad_CardICReaderEvent);

            // 여전법 추가 0621
            ClearSecureData();
        }

        /// <summary>
        /// Key Event
        /// </summary>
        /// <param name="e"></param>
        void POS_PY_P015_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (ModeProcessing)
            {
                e.IsHandled = true;
                return;
            }

            if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER ||
                e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
            {
                ValidateOnKeyEvent(e);
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

        /// <summary>
        /// 자진발급
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSelf_Click(object sender, EventArgs e)
        {
            m_crPayType = (int)((Control)sender).Tag;

            // ENTER key인경우
            if (!ValidateReadyForPayment())
            {
                ValidateInputAndNext();
                return;
            }

            if (CanStartPaymentProcess(false))
            {
                ProcessCashReceipt();
            }
        }

        /// <summary>
        /// 닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;            
        }

        /// <summary>
        /// 소득공제/지출증빙
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CRPayType_Click(object sender, EventArgs e)
        {
            if (m_selButton != null)
            {
                m_selButton.Selected = false;
            }

            var c = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)sender;
            m_selButton = c;
            c.Selected = true;

            m_crPayType = (int)c.Tag;
            txtBusinessNo.Text = (m_crPayType == POS_PY_P014.CASHRCP_TYPE_DEDUCTION ? MSG_ID_TYPE_IND : MSG_ID_TYPE_BIZ) + "(" + c.Text + ")";
            txtConfirmNo.SetFocus();
        }

        void txtConfirmNo_InputFocused(object sender, EventArgs e)
        {
            messageBar1.Text = MSG_INPUT_CONFIRM_NO;
        }

        void txtOTApprDate_InputFocused(object sender, EventArgs e)
        {
            messageBar1.Text = MSG_INPUT_OT_APPR_DATE;
        }

        void txtOTApprNo_InputFocused(object sender, EventArgs e)
        {
            messageBar1.Text = MSG_INPUT_OT_APPR_NO;
        }

        void txtCancReasn_InputFocused(object sender, EventArgs e)
        {
            messageBar1.Text = MSG_INPUT_CANC_REASON;
        }

        #endregion

        #region 사용자정의

        void ProcessCashReceipt()
        {
            if (ModeProcessing)
            {
                return;
            }

            //2015.09.09 정광호 추가
            //카드를 읽고 카드번호가 존재하는 상태에서 자진발급 버튼을 누르면 자진발급이 아닌 카드번호로 전문통신하기때문에 추가
            if (m_crPayType == POS_PY_P014.CASHRCP_TYPE_SELF && m_swipe)
            {
                m_swipe = false;
                m_readCardTrack = "";
            }

            ModeProcessing = true;

            // 밴사연동          
            StatusMessage = string.Empty;
            m_presenter.MakeCashRecptRequest(m_crPayType, m_swipe, txtOTApprDate.Text,
                txtOTApprNo.Text, ReadCardTrack, txtCancReasn.Text, m_cashAmt, m_taxAmt,
                m_addData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="showProgress"></param>
        public void ShowProgressMessage(bool showProgress)
        {
            ChildManager.ShowProgress(showProgress, MSG_VAN_REQ_PROCESSING);

            if (!showProgress)
            {
                ModeProcessing = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="errorMessage"></param>
        /// <param name="errorCode"></param>
        public void ShowErrorMessage(VANRequestErrorType errorType, string errorMessage, string errorCode, string viewTag)
        {
            ModeProcessing = false;
            bool showErrorMsg = false;
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
                    showErrorMsg = true;
                    break;
                default:
                    break;
            }

            StatusMessage = message;
            m_crPayType = btnDeduction.Selected ? POS_PY_P014.CASHRCP_TYPE_DEDUCTION : POS_PY_P014.CASHRCP_TYPE_EVIDENCE;

            // 수동반품시
            // 승인안되면 그냥닫가할 수도 있으면 basket생성안함

            // 여전법 추가 0630
            // 자동반품 시,
            // 재시도 해야 하고
            // 강제 취소 가능함

            if (showErrorMsg)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(MSG_VAN_REQ_COMM_ERROR);
                sb.AppendLine(MSG_VAN_REQ_COMM_ERROR_RETRY);
                sb.Append(Environment.NewLine);
                sb.AppendLine("[ERROR]");
                sb.AppendFormat("{0}{1}", string.IsNullOrEmpty(errorCode) ?
                        string.Empty : "[" + errorCode + "] ", errorMessage);

                this.InvokeIfNeeded(() =>
                {
                    if (m_addData.IsAutoReturn)
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
                    else
                    {
                        ShowMessageBox(MessageDialogType.Error, string.Empty,
                            message, new string[] { LABEL_CLOSE });
                    }                    
                });
                
                /*
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        var res = ShowMessageBox(MessageDialogType.Error, string.Empty,
                            message, new string[] {
                        LABEL_CLOSE
                    });
                        //if (res != DialogResult.Yes)
                        //{
                        //    // basket null으로 return함
                        //    this.DialogResult = DialogResult.OK;
                        //}
                    });
                }
                else
                {
                    var res = ShowMessageBox(MessageDialogType.Error, string.Empty,
                        message, new string[] {
                        LABEL_CLOSE
                    });
                    //if (res != DialogResult.Yes)
                    //{
                    //    // basket null으로 return함
                    //    this.DialogResult = DialogResult.OK;
                    //}
                }      */          
            }
        }

        /// <summary>
        /// 승인정산일때
        /// </summary>
        /// <param name="respData"></param>
        public void OnReturnSuccess(PV02RespData respData)
        {
            StatusMessage = respData.RespMessage1;
            
            string fgIDCheck = "2";
            if (m_crPayType != POS_PY_P014.CASHRCP_TYPE_SELF)
            {
                if (m_swipe)
                {
                    fgIDCheck = "1";
                }
                else if (txtConfirmNo.Text.Length == 10 || txtConfirmNo.Text.Length == 11)
                {
                    fgIDCheck = "2";
                }
                else if (txtConfirmNo.Text.Length == 13)
                {
                    fgIDCheck = "3";
                }
                else
                {
                    fgIDCheck = m_crPayType == POS_PY_P014.CASHRCP_TYPE_DEDUCTION ? "2" : "4";
                }
            }

            var cashBasket = new BasketCashRecpt()
            {
                AmAppr = m_cashAmt.ToString(),
                AmTax = m_taxAmt.ToString(),
                CdCancRsn = txtCancReasn.Text,
                CdVan = respData.ApprVanCode,
                DdAppr = respData.ApprDate,
                TmAppr = respData.ApprTime,
                FgAppr = "1",
                FgIDCheck = fgIDCheck,
                FgSelf = m_crPayType == POS_PY_P014.CASHRCP_TYPE_SELF ? "1" : "0",
                FgTrxnType = (m_crPayType == POS_PY_P014.CASHRCP_TYPE_SELF || 
                            m_crPayType == POS_PY_P014.CASHRCP_TYPE_DEDUCTION) ? "1" : "2",
                InputWcc = m_swipe ? "A" : "@",
                NoAppr = respData.ApprNo,
                NoPersonal = ConfirmNo,
                NoTrack = m_readCardTrack,
                DdApprOrg = txtOTApprDate.Text,
                NoApprOrg = txtOTApprNo.Text
            };

            this.ReturnResult.Add("PAY_DATA", cashBasket);
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// CLEAR key 처리
        /// </summary>
        /// <returns></returns>
        bool ProcessClearKey()
        {
            if (this.FocusedControl != null)
            {
                InputText it = (InputText)this.FocusedControl;
                if (!string.IsNullOrEmpty(it.Text))
                {
                    return false;
                }
            }

            if (IsInitialState())
            {
                btnClose_Click(btnClose, EventArgs.Empty);
                return true;
            }
            
            this.PreviousControl();
            return true;
        }

        #endregion

        #region Validation Logic

        bool ValidateReadyForPayment()
        {
            if (m_crPayType == POS_PY_P014.CASHRCP_TYPE_NONE)
            {
                return true;
            }

            // 자진발급시 확인번호필요 없음
            //var res = ((txtConfirmNo.Text.Length >= 10 ||
            //    m_crPayType == POS_PY_P014.CASHRCP_TYPE_SELF) &&
            //    txtOTApprDate.Text.Length >= 6 &&
            //    txtOTApprNo.Text.Length >= 8 &&
            //    txtCancReasn.Text.Length == 1);

            if (m_crPayType != POS_PY_P014.CASHRCP_TYPE_SELF && txtConfirmNo.Text.Length < 10)
            {
                txtConfirmNo.Text = "";
                txtConfirmNo.SetFocus();
                return false;
            }

            if (txtOTApprDate.Text.Length != 8 || !DateTimeUtils.ValidateOrgApprDateSystem(txtOTApprDate.Text))
            {
                txtOTApprDate.Text = "";
                txtOTApprDate.SetFocus();
                return false;
            }

            if (txtOTApprNo.Text.Length < 6)
            {
                txtOTApprNo.Text = "";
                txtOTApprNo.SetFocus();
                return false;
            }

            if (txtCancReasn.Text.Length != 1 || (txtCancReasn.Text != "1" && txtCancReasn.Text != "2" && txtCancReasn.Text != "3"))
            {
                txtCancReasn.Text = "";
                txtCancReasn.SetFocus();
                return false;
            }
            //if (res)
            //{
            //    if (txtOTApprDate.IsFocused)
            //    {
            //        if (!DateTimeUtils.ValidateOrgApprDate(txtOTApprDate.Text))
            //        {
            //            StatusMessage = MSG_INPUT_OT_APPR_DATE_INVALID;
            //            res = false;
            //        }
            //        else
            //        {
            //            StatusMessage = string.Empty;
            //        }
            //    }
            //}

            return true;
        }

        /// <summary>
        /// Loc added 10.27
        /// 사업자지출증빙일경우 
        /// 확인번호는 WMALL 사업자등록번호이면 안 됨
        /// </summary>
        /// <returns></returns>
        bool ValidateValidBusinessNo()
        {
            if (m_crPayType == POS_PY_P014.CASHRCP_TYPE_EVIDENCE)
            {
                return !ConfirmNo.Equals("1198179493");
            }

            return true;
        }

        bool CanStartPaymentProcess(bool onEnter)
        {
            if (!ValidateValidBusinessNo())
            {
                StatusMessage = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("01390");
                return false;
            }
            else
            {
                StatusMessage = string.Empty;
            }
            return onEnter ? txtCancReasn.IsFocused : true;
        }

        void ValidateOnKeyEvent(OPOSKeyEventArgs e)
        {
            // ENTER KEY
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
            {
                e.IsHandled = true;

                if (m_crPayType != POS_PY_P014.CASHRCP_TYPE_NONE)
                {
                    if (txtConfirmNo.IsFocused)
                    {
                        if (txtConfirmNo.Text.Length < 10)
                        {
                            txtConfirmNo.Text = "";
                            txtConfirmNo.SetFocus();
                            return;
                        }
                    }
                    else if (txtOTApprDate.IsFocused)
                    {
                        if (txtOTApprDate.Text.Length != 8 || !DateTimeUtils.ValidateOrgApprDateSystem(txtOTApprDate.Text))
                        {
                            txtOTApprDate.Text = "";
                            txtOTApprDate.SetFocus();
                            return;
                        }
                    }
                    else if (txtOTApprNo.IsFocused)
                    {
                        if (txtOTApprNo.Text.Length < 6)
                        {
                            txtOTApprNo.Text = "";
                            txtOTApprNo.SetFocus();
                            return;
                        }
                    }
                    else if (txtCancReasn.IsFocused)
                    {
                        if (txtCancReasn.Text.Length != 1 || (txtCancReasn.Text != "1" && txtCancReasn.Text != "2" && txtCancReasn.Text != "3"))
                        {
                            txtCancReasn.Text = "";
                            txtCancReasn.SetFocus();
                            return;
                        }
                    }
                }

                //// ENTER key인경우
                //if (!ValidateReadyForPayment())
                //{
                //    ValidateInputAndNext();
                //    return;
                //}

                if (CanStartPaymentProcess(true))
                {
                    if (m_crPayType != POS_PY_P014.CASHRCP_TYPE_SELF)
                    {
                        if (txtConfirmNo.Text.Length < 10)
                        {
                            txtConfirmNo.Text = "";
                            txtConfirmNo.SetFocus();
                            return;
                        }

                        if (txtOTApprDate.Text.Length != 8 || !DateTimeUtils.ValidateOrgApprDateSystem(txtOTApprDate.Text))
                        {
                            txtOTApprDate.Text = "";
                            txtOTApprDate.SetFocus();
                            return;
                        }

                        if (txtOTApprNo.Text.Length < 6)
                        {
                            txtOTApprNo.Text = "";
                            txtOTApprNo.SetFocus();
                            return;
                        }

                        if (txtCancReasn.Text.Length != 1 || (txtCancReasn.Text != "1" && txtCancReasn.Text != "2" && txtCancReasn.Text != "3"))
                        {
                            txtCancReasn.Text = "";
                            txtCancReasn.SetFocus();
                            return;
                        }
                    }
                    
                    ProcessCashReceipt();
                }
                else
                {
                    this.NextControl();
                }

                return;
            }

            // CLEAR KEY
            #region CLEAR KEY

            // Navigate back or close form (close button)
            e.IsHandled = ProcessClearKey();
            #endregion
        }

        /// <summary>
        /// 초기상태확인
        /// </summary>
        /// <returns></returns>
        bool IsInitialState()
        {
            return string.IsNullOrEmpty(txtConfirmNo.Text) &&
                string.IsNullOrEmpty(txtOTApprDate.Text) &&
                string.IsNullOrEmpty(txtOTApprNo.Text) &&
                string.IsNullOrEmpty(txtCancReasn.Text) && 
                txtConfirmNo.IsFocused;
        }

        /// <summary>
        /// 입력확인 및 다음 컨트롤로 이동
        /// </summary>
        void ValidateInputAndNext()
        {
            // 그냥닫기
            if (m_crPayType == POS_PY_P014.CASHRCP_TYPE_NONE)
            {
                return;
            }

            if (txtConfirmNo.Text.Length < 10)
            {
                txtConfirmNo.Text = "";
                txtConfirmNo.SetFocus();
                return;
            }

            #region 원거래정보
            if (txtOTApprDate.IsFocused)
            {
                if (txtOTApprDate.Text.Length < 6)
                {
                    txtOTApprDate.Text = "";
                    return;
                }
                else
                {
                    if (txtOTApprDate.IsFocused)
                    {
                        if (!DateTimeUtils.ValidateOrgApprDateSystem(txtOTApprDate.Text))
                        {
                            txtOTApprDate.Text = "";
                            //StatusMessage = MSG_INPUT_OT_APPR_DATE_INVALID;
                            return;
                        }
                        else
                        {
                            StatusMessage = string.Empty;
                        }
                    }
                }
            }
            if (txtOTApprNo.IsFocused)
            {
                if (txtOTApprNo.Text.Length < 8)
                {
                    txtOTApprNo.Text = "";
                    return;
                }
            }
            #endregion

            #region 승인번호
            if (txtCancReasn.IsFocused)
            {
                if (txtCancReasn.Text.Length == 0)
                {
                    return;
                }
            }
            #endregion

            this.NextControl();
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