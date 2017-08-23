//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P012.cs
 * 화면설명 : 자국통화 선택화면(DCC)
 * 개발자   : TCL
 * 개발일자 : 2015.06.17
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
using WSWD.WmallPos.FX.Shared.NetComm.Response.PV;

namespace WSWD.WmallPos.POS.PY.VC
{
    public partial class POS_PY_P012 : PopupBase01, IPYP012View
    {
        #region 변수

        //비즈니스 로직
        private IPYP012presenter m_presenter;

        /// <summary>
        /// 선택한 자국통화코드
        /// </summary>
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button m_selCUR = null;

        /// <summary>
        /// 결제할금액
        /// </summary>
        private Int64 m_payAmt = 0;

        /// <summary>
        /// 전문
        /// </summary>
        private PV21RespData m_respData = null;

        /// <summary>
        /// VISA Card 구분
        /// </summary>
        private bool m_isVisaCard = false;

        /// <summary>
        /// 기준통화코도, 금액, ...
        /// </summary>
        private string m_localCode = string.Empty;
        private string m_localAmount = string.Empty;
        private string m_homeCode = string.Empty;
        private string m_homeAmount = string.Empty;
        /// <summary>
        /// 역환율
        /// </summary>
        private string m_rvsRate = string.Empty;
        private string m_markupText = string.Empty;

        #endregion

        #region 생성자

        public POS_PY_P012(Int64 payAmt, bool isVisaCard, PV21RespData respData)
        {
            InitializeComponent();

            m_payAmt = payAmt;
            m_isVisaCard = isVisaCard;
            m_respData = respData;
            this.Height -= isVisaCard ? 0 : lblMarkup.Height;

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
            // this.KeyEvent += new OPOSKeyEventHandler(form_KeyEvent);            //KeyEvent
            this.FormClosed += new FormClosedEventHandler(form_FormClosed);

            this.btnRetry.Click += new EventHandler(btnRetry_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnClose.Click += new EventHandler(btnClose_Click);
            
            this.btnOK.Enabled = false;

            btnKRW.Click += new EventHandler(btnKRW_Click);
            btnOtherCUR.Click += new EventHandler(btnOtherCUR_Click);
            
            // 동글이 초기화
            POSDeviceManager.SignPad.DCCRespEvent += new POSDataEventHandler(SignPad_DCCRespEvent);
            POSDeviceManager.SignPad.SignPadCancelledEvent += new EventHandler(SignPad_SignPadCancelledEvent);
            InitSignPad();
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

            //정보 조회
            m_presenter = new PYP012presenter(this);

            DisplayDCCInfo();
        }

        /// <summary>
        /// Form closed event, sign pad 닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Load -= new EventHandler(form_Load);
            this.FormClosed -= new FormClosedEventHandler(form_FormClosed);

            this.btnRetry.Click -= new EventHandler(btnRetry_Click);
            this.btnOK.Click -= new EventHandler(btnOK_Click);
            this.btnClose.Click -= new EventHandler(btnClose_Click);

            btnKRW.Click -= new EventHandler(btnKRW_Click);
            btnOtherCUR.Click -= new EventHandler(btnOtherCUR_Click);

            // 동글이 초기화
            POSDeviceManager.SignPad.DCCRespEvent -= new POSDataEventHandler(SignPad_DCCRespEvent);
            POSDeviceManager.SignPad.SignPadCancelledEvent -= new EventHandler(SignPad_SignPadCancelledEvent);

            // 여전법 변경 0620
            // SignPad Donggle 하나만 사용 (PY_P001)
            // POSDeviceManager.SignPad.Close();
            POSDeviceManager.SignPad.ReInitialize(false);
            ClearSecureData();
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
            m_respData = null;
        }

        /// <summary>
        /// 동글이에서 통화코드 선택결과
        /// </summary>
        /// <param name="eventData"></param>
        void SignPad_DCCRespEvent(string eventData)
        {
            if (string.IsNullOrEmpty(eventData))
            {
                StatusMessage = MSG_SIGNPAD_RESP_ERROR;
            }
            else
            {
                if (eventData.Length == 2)
                {
                    if ("1".Equals(eventData.Substring(1)))
                    {
                        btnKRW_Click(btnKRW, EventArgs.Empty);
                    }
                    else
                    {
                        btnOtherCUR_Click(btnOtherCUR, EventArgs.Empty);
                    }
                }
            }
        }


        /// <summary>
        /// 쌰인패드 취소됨
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SignPad_SignPadCancelledEvent(object sender, EventArgs e)
        {
            StatusMessage = MSG_SIGNPAD_CANCELLED;
        }

        /// <summary>
        /// 싸인패드 재시도 재요청
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnRetry_Click(object sender, EventArgs e)
        {
            InitSignPad();
            RequestDCCSelection();
        }
               

        void btnOK_Click(object sender, EventArgs e)
        {
            if (m_selCUR == null)
            {
                StatusMessage = MSG_GUIDE_ASK_SELECTION;
                return;
            }

            btnOK.Enabled = false;

            // 선택한 옵션 0/1: 0: KRW, 1: OTHER CURRENCY
            this.ReturnResult.Add("SELECT_CUR", m_selCUR.Tag.ToString());
            this.DialogResult = DialogResult.OK;
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        void btnOtherCUR_Click(object sender, EventArgs e)
        {
            WSWD.WmallPos.POS.FX.Win.UserControls.Button btn = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)sender;
            SetSelected(btn);
        }

        void btnKRW_Click(object sender, EventArgs e)
        {
            WSWD.WmallPos.POS.FX.Win.UserControls.Button btn = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)sender;
            SetSelected(btn);
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// 싸인패드INIT
        /// </summary>
        void InitSignPad()
        {
            // 여전법 변경 0620
            // 사용안함
            // 한번만 POS_PY_P001.cs에서 POS_PY_P002.cs 사용함
            //if (POSDeviceManager.SignPad.Status != WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
            //{
            //    POSDeviceManager.SignPad.Initialize(this.axKSNet_Dongle1);
            //}

            if (POSDeviceManager.SignPad.Status != WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
            {
                StatusMessage = MSG_SIGNPAD_INIT_ERROR;
            }
        }

        /// <summary>
        /// DCC 선택
        /// </summary>
        private void RequestDCCSelection()
        {
            // Request signpad
            if (POSDeviceManager.SignPad.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
            {
                StatusMessage = MSG_GUIDE_ASK_SELECTION;
                ClearSelection();
                POSDeviceManager.SignPad.RequestDCCRateSelection(m_localCode, m_localAmount,
                    m_homeCode, m_homeAmount, m_rvsRate, m_markupText);
            }
        }

        /// <summary>
        /// 통화선택
        /// </summary>
        /// <param name="button"></param>
        void SetSelected(WSWD.WmallPos.POS.FX.Win.UserControls.Button button)
        {
            if (m_selCUR == null)
            {
                m_selCUR = button;
                button.Selected = true;
            }
            else
            {
                if (m_selCUR.Name == button.Name)
                {
                    m_selCUR.Selected = !m_selCUR.Selected;
                    if (!m_selCUR.Selected)
                        m_selCUR = null;
                }
                else
                {
                    m_selCUR.Selected = false;
                    button.Selected = true;
                    m_selCUR = button;
                }
            }

            StatusMessage = m_selCUR != null ? string.Format(MSG_SIGNPAD_CURR_SELECTED, m_selCUR.Text) : MSG_GUIDE_ASK_SELECTION;
            btnOK.Enabled = m_selCUR != null;
        }
        
        /// <summary>
        /// 선택취소
        /// </summary>
        void ClearSelection()
        {
            if (m_selCUR != null)
            {
                m_selCUR.Selected = false;
                m_selCUR = null;
            }

            btnOK.Enabled = m_selCUR != null;
        }

        void DisplayDCCInfo()
        {
            StatusMessage = MSG_GUIDE_ASK_SELECTION;

            // 기준통화금액
            m_localCode = "KRW";
            m_localAmount  = string.Format("{0:#,##0}", m_payAmt);
            lblAmount.Text = string.Format("{0} {1}", m_localCode, m_localAmount);

            // 자국통화금액
            m_homeCode = m_respData.DCCCurCode;
            m_homeAmount = MakeDecimalPoint(m_respData.DCCCurAmt, m_respData.DCCCurDecPoint);
            lblOthAmt.Text = m_homeCode + " " + m_homeAmount;
            
            // 역환율
            string revsRate = MakeDecimalPoint(m_respData.DCCRvExRate, m_respData.DCCRvExRateDecPoint);
            double unitAmt = Math.Pow(10, TypeHelper.ToInt32(m_respData.DCCRvExRateUnt));
            m_rvsRate = string.Format("{0} {1} = KRW {2}", m_respData.DCCCurCode, unitAmt, revsRate);
            lblExRate.Text = m_rvsRate;
            
            // Markup
            lblMarkup.Visible = m_isVisaCard;
            double mk = TypeHelper.ToDouble(m_respData.MarkupPerc);

            string markupPercentage = mk.ToString("N" + TypeHelper.ToInt32(m_respData.MarkupPercUnt, 1).ToString()) + "%";

            m_markupText = m_isVisaCard ? string.Format("Markup included in FX : {0}", markupPercentage) : string.Empty;
            lblMarkup.Text = m_markupText;

            btnKRW.Tag = "0";
            btnOtherCUR.Tag = "1";
            btnOtherCUR.Text = m_respData.DCCCurCode;

            // Request signpad
            if (POSDeviceManager.SignPad.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
            {
                POSDeviceManager.SignPad.RequestDCCRateSelection(m_localCode, m_localAmount,
                    m_homeCode, m_homeAmount, m_rvsRate, m_markupText);
            }

            #region DCC Offer 출력 - Print
            DCCOfferPrint(m_isVisaCard, lblAmount.Text, lblExRate.Text, lblOthAmt.Text, m_homeCode, markupPercentage);
            #endregion
        }

        /// <summary>
        /// DCC Offer 출력한다
        /// </summary>
        /// <param name="localAmount">true:VISA, false:Master&JCB</param>
        /// <param name="localAmount">예: KRW 100,000</param>
        /// <param name="exchangeRate">예: JPY100 = KRW 1048.8440</param>
        /// <param name="transCurrency">예: JPY 9,534</param>
        /// <param name="homeCode">예: JPY</param>
        /// <param name="markupPercent">예: 3.00%</param>
        void DCCOfferPrint(bool m_isVisaCard, string localAmount, string exchangeRate, string transCurrency, 
            string homeCode, string markupPercent)
        {
            //출력확인
            if (ChkPrint())
            {
                //DCC Offer메세지 조회
                DataTable dt = m_presenter.GetDccMsg(FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_07);
                if (dt != null)
                {
                    //출력
                    POSPrinterUtils.Instance.SetPrintDCCOffer(m_isVisaCard, localAmount, exchangeRate, 
                        transCurrency, homeCode, markupPercent, dt);    
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="decPointPos"></param>
        /// <returns></returns>
        static string MakeDecimalPoint(string amount, string decPointPos)
        {
            int nDecPointPos = TypeHelper.ToInt32(decPointPos);

            if (amount.Length < decPointPos.Length || nDecPointPos == 0)
            {
                return amount;
            }

            double d = Convert.ToDouble(amount.Substring(0, amount.Length - nDecPointPos) + "." +
                amount.Substring(amount.Length - nDecPointPos));

            return d.ToString("N" + decPointPos);
        }


        #region 프린트 확인

        /// <summary>
        /// 프린트 확인
        /// </summary>
        /// <returns></returns>
        public bool ChkPrint()
        {
            bool bReturn = false;
            string strErrMsg = string.Empty;

            try
            {
                if (POSDeviceManager.Printer.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
                {
                    if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.PowerClose)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_POWER;
                    }
                    else if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.CoverOpenned)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_OPENCOVER;
                    }
                    else if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.PaperEmpty)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_PAPER;
                    }
                    else if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.Closed)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_ERROR;
                    }
                    else
                    {
                        bReturn = true;
                    }
                }
                else
                {
                    strErrMsg = FXConsts.ERR_MSG_PRINTER_ERROR;
                }

                if (!bReturn)
                {
                    string[] strBtnNm = new string[2];
                    strBtnNm[0] = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00045");
                    strBtnNm[1] = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00190");

                    if (this.InvokeRequired)
                    {
                        this.Invoke((MethodInvoker)delegate()
                        {
                            if (ShowMessageBox(MessageDialogType.Question, null, strErrMsg, strBtnNm) == DialogResult.Yes)
                            {
                                POSDeviceManager.Printer.Open();
                                bReturn = ChkPrint();
                            }
                        });
                    }
                    else
                    {
                        if (ShowMessageBox(MessageDialogType.Question, null, strErrMsg, strBtnNm) == DialogResult.Yes)
                        {
                            POSDeviceManager.Printer.Open();
                            bReturn = ChkPrint();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }

            return bReturn;
        }

        #endregion

        #endregion

        #region 사용안함
                
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="payCard">카드Basket</param>
        ///// <param name="isVisaCard"></param>
        ///// <param name="localCode">예: KRW<</param>
        ///// <param name="localAmount">예: 100,000<</param>
        ///// <param name="homeCode">예: JPY</param>
        ///// <param name="homeAmount">예: 9,534</param>
        ///// <param name="exchangeRate">예: JPY100 = KRW 1048.8440</param>
        ///// <param name="markupPercent">예: 3.00%</param>
        ///// <param name="selectKRW">원화구분</param>
        //static public void ParseDCCPrintData(BasketPayCard payCard,
        //    out bool isVisaCard, out string localCode,
        //    out string localAmount, out string homeCode, out string homeAmount,
        //    out string exchangeRate, out string markupPercent, out bool selectKRW)
        //{
        //    selectKRW = "410".Equals(payCard.DCCCurNo);

        //    // 기준통화금액
        //    localCode = "KRW";
        //    localAmount = string.Format("{0:#,##0}", TypeHelper.ToInt64(payCard.PayAmt));

        //    // 자국통화금액
        //    homeCode = payCard.DCCCurCode;
        //    homeAmount = MakeDecimalPoint(payCard.DCCCurAmt, payCard.DCCCurDecPoint);
            
        //    // 역환율
        //    string revsRate = MakeDecimalPoint(payCard.DCCRvExRate, payCard.DCCRvExRateDecPoint);
        //    double unitAmt = Math.Pow(10, TypeHelper.ToInt32(payCard.DCCRvExRateUnt));
        //    exchangeRate = string.Format("{0}{1} = KRW {2}", payCard.DCCCurCode, unitAmt, revsRate);

        //    // Markup
            
        //    double mk = TypeHelper.ToDouble(payCard.MarkupPerc);
        //    isVisaCard = "03".Equals(payCard.ForeignCardFg);
        //    markupPercent = isVisaCard ? mk.ToString("N" +
        //        TypeHelper.ToInt32(payCard.MarkupPercUnt, 1).ToString()) + "%" : string.Empty;
        //}

        #endregion
    }
}