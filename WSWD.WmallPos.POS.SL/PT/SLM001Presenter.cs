using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Diagnostics;

using WSWD.WmallPos.POS.SL.PI;
using WSWD.WmallPos.POS.SL.VI;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PQ;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.SL.Data;
using WSWD.WmallPos.FX.NetComm.Tasks.PQ;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PP;
using WSWD.WmallPos.POS.PY.VI;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.SL.PT
{
    /// <summary>
    /// M001 Presenter 처리
    /// 개발자: TCL
    /// 날 짜 : 2015/05/03~
    /// Email  : locitt@gmail.com
    /// </summary>
    public partial class SLM001Presenter : ISLM001Presenter, IPYP001View, IPYP015View
    {
        #region Privates, 변수

        #region Views

        /// <summary>
        /// VIEWs
        /// </summary>
        ISLM001SaleView m_saleView = null;
        ISLM001TouchGroup m_touchGroupView = null;
        ISLM001TouchItem m_touchItemView = null;
        #endregion

        #region 임시변수, Current Operations

        /// <summary>
        /// 상품정보
        /// </summary>
        PBItemData m_currPBItemData = null;
        TouchItemData m_lastTouchItem = null;

        /// <summary>
        /// 합계 summary data
        /// </summary>
        SaleSummaryData m_summaryData = null;

        /// <summary>
        /// 현재상품 종류
        /// </summary>
        SaleItemType m_saleItemType = SaleItemType.NoItem;

        /// <summary>
        /// KEY 이벤트처리중인상태
        /// </summary>
        bool m_keyEventProcessing = false;

        #endregion

        #region Basket List, 결제위한 데이타모음

        private PP01RespData m_custInfo = null;
        public PP01RespData CustInfo
        {
            get
            {
                return m_custInfo;
            }
            set
            {
                m_custInfo = value;
                m_saleView.StatusMessage = value != null ? string.Format("{0}: {1}", value.CustNo, value.CustName) : string.Empty;
            }
        }


        /// <summary>
        /// 헤더
        /// </summary>
        BasketHeader BasketHeader;

        /// <summary>
        /// 헤더
        /// 2015.09.07 추가 사은품회수 헤더 추가
        /// </summary>
        BasketHeader BasketHeaderTKS;

        /// <summary>
        /// 현금영수증 basket
        /// </summary>
        BasketCashRecpt BasketCashReceipt;

        /// <summary>
        /// 포인트적립 basket
        /// </summary>
        BasketPointSave BasketPointSave;

        /// <summary>
        /// 상품목록
        /// </summary   >
        List<BasketItem> BasketItems;

        /// <summary>
        /// 소계
        /// </summary>
        BasketSubTotal BasketSubTtl;

        /// <summary>
        /// 결재수단리스트
        /// </summary>
        List<BasketPay> BasketPays;

        #endregion

        #endregion

        #region 상태 & 키 MAPPING

        /// <summary>
        /// 입력대기모드
        /// </summary>
        private const string InputState_Ready = @"SCANNING,TOUCH,PRESET,NUM0,NUM00,NUM000,NUM1,NUM2,NUM3,NUM4,NUM5,NUM6,NUM7,NUM8,NUM9,
                        CLEAR,ENTER,BKS,UP,DOWN,MANWON,SIGNOFF,SUBTOTAL,
                        PLU,PRCCHG,DISCPN,QTY,
                        LINEVOID,ALLVOID,HOLD,
                        AUTORTN,MANRTN,OTHSALE,OTHSRTN,
                        OTHRGIFT,CASHIC,CARD,CASH,PNTUSE,OTHRPAY,CASH,ONLPAY,
                        PRDEXCG,INQPNT,INQPRC,INOUT,DATASET,INQUIRY,INQCHK,INQRECP,EOD,
                        BACKUP,NOSALE,MENU";

        /// <summary>
        /// 수동입력만
        /// </summary>
        private const string InputState_ItemPumNoEntered = @"NUM0,NUM00,NUM000,NUM1,NUM2,NUM3,NUM4,NUM5,NUM6,NUM7,NUM8,NUM9,
                        CLEAR,ENTER,BKS";

        /// <summary>
        /// BARCODE SCAN 2단, 수동입력
        /// </summary>
        private const string InputState_ItemCodeEntered = @"SCANNING,PLU,NUM0,NUM00,NUM000,NUM1,NUM2,NUM3,NUM4,NUM5,NUM6,NUM7,NUM8,NUM9,CLEAR,ENTER,BKS";

        /// <summary>
        /// 금액입력만
        /// </summary>
        private const string InputState_ItemGubunEntered = @"NUM0,NUM00,NUM000,NUM1,NUM2,NUM3,NUM4,NUM5,NUM6,NUM7,NUM8,NUM9,CLEAR,ENTER,BKS,MANWON";

        /// <summary>
        /// 저장물판매모드
        /// - 금액입력가능
        /// - 결제수단가능
        /// </summary>
        private const string ProcessState_OSInputStarted = @"TOUCH,PLU,SCANNING,NUM0,NUM00,NUM000,NUM1,NUM2,NUM3,NUM4,NUM5,NUM6,NUM7,NUM8,NUM9,CLEAR,
                    BKS,QTY,PRCCHG,LINEVOID,ALLVOID,SUBTOTAL,OTHSALE,OTHSRTN,MENU,CARD,CASH,INQPRC,INOUT,DATASET,INQUIRY,INQCHK,INQRECP,EOD,
                        BACKUP,NOSALE,MENU";

        /// <summary>
        /// 자동반품
        /// </summary>
        private const string ProcessState_AutoReturnInputStarted = @"SCANNING,NUM0,NUM00,NUM000,NUM1,NUM2,NUM3,NUM4,NUM5,NUM6,NUM7,NUM8,NUM9,ENTER,CLEAR,BKS";

        /// <summary>
        /// 소계상태
        /// </summary>
        private const string ProcessState_SubTotal = @"NUM0,NUM00,NUM000,NUM1,NUM2,NUM3,
                        NUM4,NUM5,NUM6,NUM7,NUM8,NUM9,
                        CLEAR,BKS,MANWON,
                        OTHRGIFT,CASHIC,CARD,CASH,INQCHK,PNTUSE,INQPNT,OTHRPAY,ONLPAY,DISCPN,PRDEXCG";

        /// <summary>
        /// 소계시, 저장물판매
        /// </summary>
        private const string ProcessState_OSSubTotal = @"NUM0,NUM00,NUM000,NUM1,NUM2,NUM3,
                        NUM4,NUM5,NUM6,NUM7,NUM8,NUM9,
                        CLEAR,BKS,MANWON,CARD,CASH,INQCHK";

        /// <summary>
        /// 자동반품 확정가능한 상태
        /// </summary>
        private const string ProcessState_AutoReturnAutoRtnReady = @"SCANNING,NUM0,NUM00,NUM000,NUM1,NUM2,NUM3,NUM4,NUM5,NUM6,NUM7,NUM8,NUM9,ENTER,CLEAR,BKS";

        /// <summary>
        /// 소계시, 자동반품상태
        /// 아무것도 키사용 불가능하게
        /// </summary>
        private const string ProcessState_AutoReturnAutoRtnProcessing = @" ";

        /// <summary>
        /// 결제중인상태
        /// </summary>
        private const string ProcessState_Payment = @"NUM0,NUM00,NUM000,NUM1,NUM2,NUM3,
                        NUM4,NUM5,NUM6,NUM7,NUM8,NUM9,
                        CLEAR,BKS,MANWON,
                        OTHRGIFT,CASHIC,CARD,CASH,INQCHK,PNTUSE,INQPNT,OTHRPAY,CASH,ONLPAY,DISCPN,PRDEXCG";

        /// <summary>
        /// 결제중인상태, 저장물판매
        /// </summary>
        private const string ProcessState_OSPayment = @"NUM0,NUM00,NUM000,NUM1,NUM2,NUM3,
                        NUM4,NUM5,NUM6,NUM7,NUM8,NUM9,
                        CLEAR,BKS,MANWON,CARD,CASH,INQCHK";

        /// <summary>
        /// Validate key by state
        /// </summary>
        /// <param name="key"></param>
        /// <param name="scan"></param>
        /// <param name="touch"></param>
        /// <returns></returns>
        public bool ValidateKeyInput(OPOSMapKeys key, bool scan, bool touch)
        {
            var ps = m_saleView.ProcessState.ToString();

            // 자동반품확인
            string ks = string.Empty;
            if (m_saleView.SaleMode == SaleModes.AutoReturn)
            {
                ks = this.GetPrivateConstField("ProcessState_AutoReturn" + ps);
            }
            else
            {
                ks = this.GetPrivateConstField("ProcessState_" +
                    (m_saleView.SaleMode.ToString().StartsWith("Other") ? "OS" : string.Empty) + ps);
            }

            if (string.IsNullOrEmpty(ks))
            {
                var state = m_saleView.InputState == ItemInputState.None ? ItemInputState.Ready : m_saleView.InputState;
                string instate = state.ToString();
                ks = this.GetPrivateConstField("InputState_" + instate);
            }

            if (string.IsNullOrEmpty(ks))
            {
                return true;
            }

            if (scan)
            {
                return ks.Contains("SCANNING");
            }

            if (touch)
            {
                return ks.Contains("TOUCH");
            }

            if (key.ToString().Contains("PRESET"))
            {
                return ks.Contains("PRESET");
            }

            ks = ks.Replace("SCANNING,", string.Empty);
            ks = ks.Replace("TOUCH,", string.Empty);
            string[] keys = ks.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var kn in keys)
            {
                string keyName = "KEY_" + kn.Trim();
                try
                {
                    if (key.ToString().Equals(keyName))
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.Instance.LogException(ex);
                }
            }

            return false;
        }

        #endregion

        #region 생성자

        public SLM001Presenter(ISLM001SaleView saleView, ISLM001TouchGroup touchGroupView,
                ISLM001TouchItem touchItemView)
        {
            m_saleView = saleView;
            m_touchGroupView = touchGroupView;
            m_touchItemView = touchItemView;

            #region TOUCH VIEW INIT

            m_touchGroupView.OnTouch += new TouchEventHandler(m_touchGroupView_OnTouch);
            m_touchItemView.OnTouch += new TouchEventHandler(m_touchItemView_OnTouch);

            //BindTouchGroups();

            #endregion

            #region DATA 초기화

            // 정상판매모드로
            ChangeSaleMode(SaleModes.Sale, false, false, true);

            #endregion
        }

        #endregion

        #region KEY EVENT

        /// <summary>
        /// Key event 처리
        /// </summary>
        /// <param name="e"></param>
        public bool ProcessKeyEvent(OPOSKeyEventArgs e)
        {
            // 처리중이니 무시
            if (m_keyEventProcessing ||
                m_saleView.ProcessState == SaleProcessState.AutoRtnProcessing)
            {
                e.IsHandled = true;
                return false;
            }

            m_keyEventProcessing = true;
            StartOperation();

            if (!ValidateKeyInput(e.Key.OPOSKey, false, false))
            {
                ReportInvalidState(InvalidDataInputState.InvalidKey);
                e.IsHandled = true;
                return OnProcessKeyEventReturn(e);
            }

            if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
            {
                // 20161227.osj 분기 추가
                //if (PBItemData.GetCdDp(m_saleView.InputText) == CdDpTypes.PB
                //|| (m_currPBItemData != null &&
                //    m_currPBItemData.CompletedStep >= PBItemParseStep.CdClass))
                //{
                    ProcessKeyOnEnter();
                //}
                //else
               // {
                //    ProcessPLUKey();
                //}
                e.IsHandled = true;
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_PLU)
            {
                ProcessPLUKey();
                e.IsHandled = true;
            }
            else if (e.Key.OPOSKey.ToString().Contains("PRESET"))
            {
                ProcessPresetKey(e);
                e.IsHandled = true;
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
            {
                // Clear to reset state
                ProcessOnClear();
                e.IsHandled = true;
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_QTY)
            {
                e.IsHandled = true;
                #region 수량변경
                ProcessChangeQty();
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_PRCCHG)
            {
                e.IsHandled = true;
                #region 가격변경
                ProcessChangePrice();
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_LINEVOID)
            {
                e.IsHandled = true;
                #region 지정취소
                ProcessLineVoid();
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_MANWON)
            {
                e.IsHandled = true;

                // 만원처리
                #region 만원처리
                ProcessManWonKey();
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_HOLD)
            {
                // no more proccess
                #region 보류
                ProcessHoldKey(e);
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_ALLVOID)
            {
                // no more proccess
                e.IsHandled = true;
                #region 거래중지
                ProcessCancelAll();
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_BACKUP)
            {
                e.IsHandled = true;
                #region 거래복원
                if (!LoadBackTran())
                {
                    ReportInvalidState(InvalidDataInputState.InvalidKey);
                }
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_CASH)
            {
                #region 현금결제
                e.IsHandled = true;
                if (m_saleView.ProcessState != SaleProcessState.SubTotal)
                {
                    ReportInvalidState(InvalidDataInputState.InvalidKey);
                    return OnProcessKeyEventReturn(e);
                }
                ProcessPayCash();
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_CARD)
            {
                #region 카드결제
                e.IsHandled = true;
                if (m_saleView.ProcessState != SaleProcessState.SubTotal)
                {
                    ReportInvalidState(InvalidDataInputState.InvalidKey);
                    return OnProcessKeyEventReturn(e);
                }
                ProcessPayCard();
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_CASHIC)
            {
                #region 현금IC
                e.IsHandled = true;
                if (m_saleView.ProcessState != SaleProcessState.SubTotal)
                {
                    ReportInvalidState(InvalidDataInputState.InvalidKey);
                    return OnProcessKeyEventReturn(e);
                }
                ProcessPayCashIC();
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_OTHRPAY)
            {
                #region 기타결제
                e.IsHandled = true;
                if (m_saleView.ProcessState != SaleProcessState.SubTotal)
                {
                    ReportInvalidState(InvalidDataInputState.InvalidKey);
                    return OnProcessKeyEventReturn(e);
                }
                ProcessOtherPay();
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_PRDEXCG)
            {
                #region 상품교환권
                e.IsHandled = true;
                if (m_saleView.ProcessState != SaleProcessState.SubTotal)
                {
                    ReportInvalidState(InvalidDataInputState.InvalidKey);
                    return OnProcessKeyEventReturn(e);
                }

                if (m_saleView.StateRefund)
                {
                    ProcessExchangeRtn();
                }
                else
                {
                    ProcessExchange();
                }
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_OTHRGIFT)
            {
                #region 타사상품권
                e.IsHandled = true;
                if (m_saleView.ProcessState != SaleProcessState.SubTotal)
                {
                    ReportInvalidState(InvalidDataInputState.InvalidKey);
                    return OnProcessKeyEventReturn(e);
                }
                ProcessOtherTicket();
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_PNTUSE)
            {
                #region 포인트사용
                e.IsHandled = true;
                if (m_saleView.ProcessState != SaleProcessState.SubTotal)
                {
                    ReportInvalidState(InvalidDataInputState.InvalidKey);
                    return OnProcessKeyEventReturn(e);
                }
                ProcessPointUse();
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_ONLPAY)
            {
                #region 온라인매출
                e.IsHandled = true;
                if (m_saleView.ProcessState != SaleProcessState.SubTotal)
                {
                    ReportInvalidState(InvalidDataInputState.InvalidKey);
                    return OnProcessKeyEventReturn(e);
                }
                ProcessOnlinePay();
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_DISCPN)
            {
                #region 할인쿠폰
                e.IsHandled = true;
                if (m_saleView.ProcessState != SaleProcessState.SubTotal)
                {
                    ReportInvalidState(InvalidDataInputState.InvalidKey);
                    return OnProcessKeyEventReturn(e);
                }
                ProcessCoupon();
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_MENU)
            {
                #region 메인화면으로이동
                e.IsHandled = true;
                if (m_saleView.HasItems)
                {
                    ReportInvalidState(InvalidDataInputState.InvalidKey);
                    return OnProcessKeyEventReturn(e);
                }
                m_saleView.Close();
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_OTHSALE)
            {
                #region 저장물판매
                e.IsHandled = true;
                ProcessOthSale();
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_OTHSRTN)
            {
                #region 저장물반품
                e.IsHandled = true;
                ProcessOthSaleReturn();
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_INQPNT)
            {
                #region 포인트 조회
                // 포인트 조회
                e.IsHandled = true;
                if (m_saleView.SaleMode.ToString().StartsWith("Other"))
                {
                    this.ReportInvalidState(InvalidDataInputState.InvalidKey);
                    return OnProcessKeyEventReturn(e);
                }

                m_saleView.ShowCustPointPopup();
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_SUBTOTAL)
            {
                #region 소계
                if (!ProcessSubTotal(true))
                {
                    ReportInvalidState(InvalidDataInputState.InvalidKey);
                    e.IsHandled = true;
                    return OnProcessKeyEventReturn(e);
                }

                if (!m_saleView.SaleMode.ToString().StartsWith("Other") &&
                    m_saleItemType != SaleItemType.OnlineItem)
                {
                    m_saleView.ShowCustPointPopup();
                }

                m_saleView.ProcessKeyEvent(e);
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_SIGNOFF)
            {
                #region 싸인오프
                if (m_saleView.HasItems)
                {
                    e.IsHandled = true;
                    ReportInvalidState(InvalidDataInputState.InvalidKey);
                    return OnProcessKeyEventReturn(e);
                }
                m_saleView.Close();
                m_saleView.ProcessKeyEvent(e);
                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_MANRTN)
            {
                //2015.09.23 정광호 수정
                //반품가능여부 확인 추가
                if (ConfigData.Current.AppConfig.PosOption.SalesReturn == "1")
                {
                    e.IsHandled = true;
                    #region 수동반품
                    ProcessSaleManuReturn();
                    #endregion
                }
                else
                {
                    m_saleView.ShowRtnError();
                }
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_AUTORTN)
            {
                //2015.09.23 정광호 수정
                //반품가능여부 확인 추가
                if (ConfigData.Current.AppConfig.PosOption.SalesReturn == "1")
                {
                    e.IsHandled = true;
                    #region 자동반품
                    AutoRtnChangeMode();
                    #endregion
                }
                else
                {
                    m_saleView.ShowRtnError();
                }
            }
            else
            {
                return m_saleView.ProcessKeyEvent(e);
            }

            return OnProcessKeyEventReturn(e);
        }

        /// <summary>
        /// End of processing key eventg
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnProcessKeyEventReturn(OPOSKeyEventArgs e)
        {
            m_keyEventProcessing = false;
            return true;
        }

        #endregion

        #region CLEAR Key

        /// <summary>
        /// CLEAR key
        /// </summary>
        void ProcessOnClear()
        {
            if (!string.IsNullOrEmpty(m_saleView.InputText))
            {
                m_saleView.InputText = string.Empty;
                return;
            }

            // 자동반품시
            if (m_saleView.SaleMode == SaleModes.AutoReturn)
            {
                // 거래번호 확인된상태이면, 다시reset해서 새로운 번호 받는다
                if (m_saleView.ProcessState == SaleProcessState.AutoRtnReady)
                {
                    AutoRtnResetStart();
                }
                else
                {
                    if (m_saleView.InvokeRequired)
                    {
                        m_saleView.BeginInvoke((MethodInvoker)delegate()
                        {
                            // 판매모드로 전환
                            ChangeSaleMode(SaleModes.Sale, false, false, true);
                        });
                    }
                    else
                    {
                        // 판매모드로 전환
                        ChangeSaleMode(SaleModes.Sale, false, false, true);
                    }
                }
                return;
            }

            if (m_saleView.ProcessState == SaleProcessState.SubTotal)
            {
                if (!CanCancelSubTotal())
                {
                    ReportInvalidState(InvalidDataInputState.InvalidKey);
                    return;
                }

                // 소계 CLEAR
                ProcessSubTotal(false);
                return;
            }

            CancelCurrentInput();
            ResetTempItemData();
        }

        #endregion

        #region 공통

        /// <summary>
        /// 
        /// </summary>
        void _ProcessPBBarCode()
        {
            if (m_currPBItemData.ScannedStep == PBItemParseStep.Empty)
            {
                // 여기서 이런경우 안생긴다
                return;
            }

            if (m_currPBItemData.ScannedStep > PBItemParseStep.FgClass)
            {
                if (m_currPBItemData.CompletedStep == PBItemParseStep.Empty)
                {
                    CheckPumBeon();
                    return;
                }

                // 상품금액 입력되거나 검색됨
                CheckItemAmount();
            }
            else if (m_currPBItemData.ScannedStep > PBItemParseStep.CdItem)
            {
                if (m_currPBItemData.CompletedStep == PBItemParseStep.Empty)
                {
                    CheckPumBeon();
                    return;
                }

                // FgClass 검색됨
                CheckItemGubun();
            }
            else if (m_currPBItemData.ScannedStep > PBItemParseStep.CdClass)
            {
                if (m_currPBItemData.CompletedStep == PBItemParseStep.Empty)
                {
                    CheckPumBeon();
                    return;
                }

                // 품목코드까지 검색됨
                CheckItemCode();
            }
            else
            {
                // 품번코드 검색됨
                CheckPumBeon();
            }
        }

        /// <summary>
        /// Parse 품번
        /// </summary>
        /// <param name="parseType"></param>
        void ProcessPBBarCode(string cdItem, string nmClass, string cdDp, int utSprc)
        {
            if (!ValidateOtherSale(cdItem))
            {
                ReportInvalidState(InvalidDataInputState.CantSaleItem);
                return;
            }

            if (m_currPBItemData == null)
            {
                m_currPBItemData = new PBItemData()
                {
                    Qty = "1",
                    AmountValidator = ValidateOverAmt,
                    NmClass = nmClass,
                    CdDp = cdDp,
                    UtSprc = utSprc.ToString()
                };
                m_currPBItemData.ParseType = m_saleView.InputOperation == ItemInputOperation.Preset ||
                                                m_saleView.InputOperation == ItemInputOperation.Touch ?
                                                PBItemParseType.FullCode : PBItemParseType.Manual;

                // 완료후 금액처리
                m_currPBItemData.CompletedStepEvent += new PBItemScannedEventHandler(m_currPBItemData_CompletedStepEvent);
                m_currPBItemData.ScannedEvent += new PBItemScannedEventHandler(m_currPBItemData_ScannedEvent);
            }

            var pr = m_currPBItemData.Parse(cdItem, m_saleView.InputOperation);
            if (pr != PBItemParseResult.Success)
            {
                InvalidDataInputState rs = InvalidDataInputState.LengthError;
                if (pr == PBItemParseResult.InvalidData)
                {
                    rs = InvalidDataInputState.InvalidData;
                }
                else if (pr == PBItemParseResult.NumberOverflow)
                {
                    rs = InvalidDataInputState.NumberOverflow;
                }
                else if (pr == PBItemParseResult.TotalAmountOver)
                {
                    rs = InvalidDataInputState.TotalAmtOverflow;
                }
                else if (pr == PBItemParseResult.SecBarCodeInput)
                {
                    rs = InvalidDataInputState.SecBarcodeError;
                }

                ReportInvalidState(rs);

                if (m_currPBItemData.CompletedStep == PBItemParseStep.Empty)
                {
                    CancelCurrentInput();
                    ResetTempItemData();
                }
            }
        }

        /// <summary>
        /// 마자막 단계의 품번/단품 데이타 들어오면
        /// 상품 금액 확인 하여
        /// 그리드에 추가한다
        /// TOUCH 상품인경우도 이렇게 처리한다
        ///     - 금액있거나 없거나 나머지는 정보 다 있다.
        /// </summary>
        /// <param name="itemData"></param>
        void m_currPBItemData_CompletedStepEvent(PBItemData itemData)
        {
            if ((m_saleView.InputOperation == ItemInputOperation.Touch ||
                m_saleView.InputOperation == ItemInputOperation.Preset) &&
                m_lastTouchItem != null)
            {
                if (m_lastTouchItem.UtSprc > 0)
                {
                    CheckItemAmount();
                    m_lastTouchItem = null;
                }
            }
        }

        void m_currPBItemData_ScannedEvent(PBItemData itemData)
        {
            try
            {
                _ProcessPBBarCode();
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
        }

        void CancelCurrentInput()
        {
            // CLEAR CURRENT ROW ITEM IF ENTERING
            if (m_saleView.ProcessState == SaleProcessState.ItemInputing)
            {
                m_saleView.ItemsGrid_CancelNewRow();
            }

            if (m_saleView.DataRows.Length == 0)
            {
                m_saleItemType = SaleItemType.NoItem;
            }

            // RestartInputState();
        }

        #endregion

        #region 품번처리

        #region 품번확인

        /// <summary>
        /// 품번확인
        /// </summary>
        void CheckPumBeon()
        {
            var db = MasterDbHelper.InitInstance();
            try
            {
                var row = db.ExecuteQueryDataRow("BSM061T_CDCLASS_Exists".POSSLQuerySQL(),
                new string[] { "@CD_CLASS" }, new object[] { m_currPBItemData.CdClass });
                if (row == null)
                {
                    m_saleView.ShowProgress(true);
                    PQ06DataTask pq06Task = new PQ06DataTask(m_currPBItemData.CdClass);
                    pq06Task.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pq06Task_TaskCompleted);
                    pq06Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pq06Task_Errored);
                    pq06Task.ExecuteTask();
                }
                else
                {
                    PQ06RespData item = new PQ06RespData()
                    {
                        cdClass = row["CD_CLASS"].ToString(),
                        nmClass = row["NM_CLASS"].ToString(),
                        fgTax = row["FG_TAX"].ToString(),
                    };

                    OnCheckPumBeonResult(item);
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                db.EndInstance();
            }
        }

        void pq06Task_Errored(string errorMessage, Exception lastException)
        {
            // hide progress
            m_saleView.ShowProgress(false);
            LogUtils.Instance.LogException(lastException);
            ReportInvalidState(InvalidDataInputState.NetworkError);
        }

        void pq06Task_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            // hide progress
            m_saleView.ShowProgress(false);
            if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.Success)
            {
                var item = responseData.DataRecords.ToDataRecords<PQ06RespData>()[0];
                OnCheckPumBeonResult(item);
            }
            else
            {
                m_saleView.ReportErrorMessage(SaleViewErrorMessage.NoCdClass);
                ProcessOnClear();
            }
        }

        void OnCheckPumBeonResult(PQ06RespData cdClassData)
        {
            try
            {
                m_currPBItemData.NmClass = cdClassData.nmClass;
                m_currPBItemData.FgTax = cdClassData.fgTax;

                PBItemProperties disp = PBItemProperties.CdClass | PBItemProperties.NmClass;
                if (m_saleView.InputOperation == ItemInputOperation.Touch ||
                    m_saleView.InputOperation == ItemInputOperation.Preset)
                {
                    disp |= PBItemProperties.CdItem | PBItemProperties.FgClass;
                }
                else if (m_saleView.InputOperation == ItemInputOperation.Scan ||
                    m_saleView.InputOperation == ItemInputOperation.PLU)
                {
                    disp |= PBItemProperties.CdItem;
                }

                m_currPBItemData.Properties = disp;

                if (!AddNewItem(m_currPBItemData))
                {
                    //CancelCurrentInput();                    
                    ResetTempItemData();
                    return;
                }

                m_currPBItemData.CompleteStep();
                m_saleView.ProcessState = SaleProcessState.ItemInputing;

                if (m_currPBItemData.ScannedStep >= PBItemParseStep.CdItem)
                {
                    CheckItemCode();
                }
                else
                {
                    m_saleView.InputState = ItemInputState.ItemPumNoEntered;
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
        }

        #endregion

        #region 품목마스터 확인

        /// <summary>
        /// 품목코드 확인
        /// 품목명표시
        /// </summary>
        void CheckItemCode()
        {
            var db = MasterDbHelper.InitInstance();
            try
            {
                var row = db.ExecuteQueryDataRow("BSM100T_CD_Exists".POSSLQuerySQL(),
                new string[] { "@CD_CTGY" }, new object[] { m_currPBItemData.CdItem });
                if (row == null)
                {
                    m_saleView.ShowProgress(true);
                    PQ08DataTask pq08Task = new PQ08DataTask(m_currPBItemData.CdItem);
                    pq08Task.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pq08Task_TaskCompleted);
                    pq08Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pq08Task_Errored);
                    pq08Task.ExecuteTask();
                }
                else
                {
                    PQ08RespData itemCode = new PQ08RespData()
                    {
                        cdCtgy = row["CD_CTGY"].ToString(),
                        nmCtgy = row["NM_CTGY"].ToString()
                    };

                    OnCheckItemCodeResult(itemCode.nmCtgy);
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                db.EndInstance();
            }
        }

        void pq08Task_Errored(string errorMessage, Exception lastException)
        {
            m_saleView.ShowProgress(false);
            LogUtils.Instance.LogException(lastException);
            ReportInvalidState(InvalidDataInputState.NetworkError);
        }

        void pq08Task_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            m_saleView.ShowProgress(false);
            if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.Success)
            {
                var itemCode = responseData.DataRecords.ToDataRecords<PQ08RespData>()[0];
                OnCheckItemCodeResult(itemCode.nmCtgy);
            }
            else
            {
                m_saleView.InputText = string.Empty;
                m_saleView.ReportErrorMessage(SaleViewErrorMessage.NoCdItem);

                if (m_saleView.InputOperation != ItemInputOperation.ManualEnter)
                {
                    CancelCurrentInput();
                    ResetTempItemData();
                }
            }
        }

        /// <summary>
        /// 상품코드 확인결과 받아서 처리 한다
        /// 현재 입력 중인 행에 업데이트 한다
        /// </summary>
        /// <param name="nmCtgy"></param>
        void OnCheckItemCodeResult(string nmCtgy)
        {
            m_currPBItemData.NmItem = nmCtgy;
            m_currPBItemData.Properties = PBItemProperties.CdClass | PBItemProperties.CdItem | PBItemProperties.NmClass |
                                    PBItemProperties.NmItem;
            m_saleView.ItemsGrid_UpdateItemRow(m_currPBItemData);
            m_currPBItemData.CompleteStep();

            if (m_currPBItemData.CompletedStep >= PBItemParseStep.FgClass)
            {
                CheckItemGubun();
            }
            else
            {
                m_saleView.InputState = ItemInputState.ItemCodeEntered;
            }
        }

        #endregion

        #region 품번 상품구분확인

        /// <summary>
        /// 상품구분확인
        /// </summary>
        void CheckItemGubun()
        {
            int onlineFg = ValidateFgClass(m_currPBItemData.FgClass);
            if (onlineFg != 0)
            {
                ReportInvalidState(onlineFg == 1 ? InvalidDataInputState.OnlyOnlineItem : InvalidDataInputState.OnlyOfflineItem);
                return;
            }

            var db = MasterDbHelper.InitInstance();
            try
            {
                var cnt = db.ExecuteScalar("BSM061T_CDCLASS_FG_Exists".POSSLQuerySQL(),
                new string[] { "@CD_CLASS", "@FG_CLASS" },
                new object[] { m_currPBItemData.CdClass, m_currPBItemData.FgClass });

                int nCnt = TypeHelper.ToInt32(cnt.ToString());
                if (nCnt == 0)
                {
                    m_saleView.ShowProgress(true);
                    PQ07DataTask pq07Task = new PQ07DataTask(m_currPBItemData.CdClass, m_currPBItemData.FgClass);
                    pq07Task.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pq07Task_TaskCompleted);
                    pq07Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pq07Task_Errored);
                    pq07Task.ExecuteTask();
                }
                else
                {
                    OnCheckItemGubun();
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                db.EndInstance();
            }
        }

        void pq07Task_Errored(string errorMessage, Exception lastException)
        {
            m_saleView.ShowProgress(false);
            LogUtils.Instance.LogException(lastException);
            ReportInvalidState(InvalidDataInputState.NetworkError);
        }

        void pq07Task_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            m_saleView.ShowProgress(false);
            if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.Success)
            {
                OnCheckItemGubun();
            }
            else
            {
                m_saleView.ReportErrorMessage(SaleViewErrorMessage.NoCdClass);
                ProcessOnClear();
            }
        }

        /// <summary>
        /// 상품구분 받고 행에 업데이트 한다
        /// 그 다음에 혹시 다음 단계까지 읽었는지 확인
        /// 상품 금액을 확인한다 (구분 받고 또는 금액도 같이 받음)
        /// </summary>
        void OnCheckItemGubun()
        {
            m_currPBItemData.Properties = PBItemProperties.All;
            m_saleView.ItemsGrid_UpdateItemRow(m_currPBItemData);
            m_currPBItemData.CompleteStep();

            if (m_currPBItemData.ScannedStep > PBItemParseStep.FgClass)
            {
                CheckItemAmount();
            }
            else
            {
                m_saleView.InputState = ItemInputState.ItemGubunEntered;
            }
        }

        #endregion

        #region 상품금액 입력

        /// <summary>
        /// 상품금액 입력 후, 
        ///     - 품번 & 구분으로 확인해서 존재 한다면, 상품금액 표시
        ///     - 아니면 오류
        /// </summary>
        void CheckItemAmount()
        {
            // 상품구분입력 시 미리 확인 했으니 여기서 할필요 없음
            OnCheckItemAmountResult();
        }

        /// <summary>
        /// 상품금액까지 업력 된다, 그리드 행에 업데이트 한다.
        /// </summary>
        void OnCheckItemAmountResult()
        {
            m_currPBItemData.Properties = PBItemProperties.All;
            if (m_currPBItemData.CompletedStep == PBItemParseStep.Empty)
            {
                if (!AddNewItem(m_currPBItemData))
                {
                    CancelCurrentInput();
                    return;
                }
            }
            else
            {
                m_saleView.ItemsGrid_UpdateItemRow(m_currPBItemData);
            }

            m_currPBItemData.CompleteStep();
            RestartInputState();
        }

        #endregion

        #endregion

        #region ENTER KEY

        /// <summary>
        /// ENTER KEY
        /// </summary>
        void ProcessKeyOnEnter()
        {
            // 거래번호 SCANN
            if (m_saleView.SaleMode == SaleModes.AutoReturn)
            {
                if (m_saleView.InvokeRequired)
                {
                    m_saleView.BeginInvoke((MethodInvoker)delegate()
                    {
                        AutoRtnProcessOnEnter();
                    });
                }
                else
                {
                    AutoRtnProcessOnEnter();
                }
                return;
            }

            if (!ValidateInput(true))
            {
                return;
            }

            m_saleView.InputOperation = ItemInputOperation.ManualEnter;
            ProcessPBBarCode(m_saleView.InputText, string.Empty, string.Empty, 0);
        }

        #endregion

        #region 전체상품코드 처리 단품 / PLU

        /// <summary>
        /// /// (2) 해당 바코드의 상품 정보를 확인 한다.
        ///    ① Local Database 상품마스터 Table (bsm079t)에서 Select 한다.
        ///    ② 만약 없으면 점서버와 통신(전문구분:PQ05)하여 상품정보를 확인한다.
        ///      - 상품정보가 없으면 오류 메시지 박스 표시 ("해당 상품정보가 없습니다.")
        /// </summary>
        /// <param name="cdItem"></param>
        void ProcessPLUCode(string cdItem, string nmItem, string cdDp, int utSprc)
        {
            if (!ValidateOtherSale(cdItem))
            {
                ReportInvalidState(InvalidDataInputState.CantSaleItem);
                return;
            }

            var db = MasterDbHelper.InitInstance();
            try
            {
                var row = db.ExecuteQueryDataRow("BSM079T_CDITEM_Exists".POSSLQuerySQL(),
                new string[] { "@CD_ITEM" }, new object[] { cdItem });
                if (row == null)
                {
                    m_saleView.ShowProgress(true);
                    PQ05DataTask pq05Task = new PQ05DataTask(cdItem);
                    pq05Task.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pq05Task_TaskCompleted);
                    pq05Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pq05Task_Errored);
                    pq05Task.ExecuteTask();
                }
                else
                {
                    PQ05RespData itemData = new PQ05RespData()
                    {
                        CdItem = row["CD_ITEM"].ToString(),
                        NmItem = row["NM_ITEM"].ToString(),
                        CdClass = row["CD_CLASS"].ToString(),

                        //정광호 단품일경우 지금 현재 303T 테이블 nm_calss에 값이 안들어옴(2015.08.20)
                        //NmClass = SLExtensions.CDDP_PB.Equals(row["CD_DP"].ToString()) ?
                        //    row["NM_CLASS"].ToString() : string.Empty,

                        NmClass = row["NM_CLASS"].ToString(),
                        FgTax = row["FG_TAX"].ToString(),
                        UtCprc = row["UT_CPRC"].ToString(),
                        UtSprc = row["UT_SPRC"].ToString(),
                        FgUse = row["FG_USE"].ToString(),
                        CdDp = row["CD_DP"].ToString(),
                        FgPoint = row["FG_POINT"].ToString(),
                        AmPoint = row["AM_APNT"].ToString(),
                        TtIudt = row["TT_IUDT"].ToString(),
                    };

                    OnProcessPLUCodeResult(itemData);
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                db.EndInstance();
            }
        }

        void pq05Task_Errored(string errorMessage, Exception lastException)
        {
            m_saleView.ShowProgress(false);
            LogUtils.Instance.LogException(lastException);
            ReportInvalidState(InvalidDataInputState.NetworkError);
        }

        void pq05Task_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            m_saleView.ShowProgress(false);
            if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.Success)
            {
                var itemData = responseData.DataRecords.ToDataRecords<PQ05RespData>()[0];
                OnProcessPLUCodeResult(itemData);
            }
            else
            {
                m_saleView.ReportErrorMessage(SaleViewErrorMessage.NoCdClass);
                ProcessOnClear();
            }
        }

        /// <summary>
        /// 상품검색됐으니 화면으로 표시한다
        /// </summary>
        /// <param name="itemData"></param>
        void OnProcessPLUCodeResult(PQ05RespData itemData)
        {
            
            if (!m_saleView.SaleMode.ToString().StartsWith("Other") &&
                SLExtensions.CDDP_OTHER.Equals(itemData.CdDp))
            {
                m_saleView.ReportInvalidState(InvalidDataInputState.CantSaleOther);
                //return;
            }
            else if (m_saleView.SaleMode.ToString().StartsWith("Other") &&
                !SLExtensions.CDDP_OTHER.Equals(itemData.CdDp))
            {
                m_saleView.ReportInvalidState(InvalidDataInputState.CantSaleItem);
                //return;
            }

            if (m_currPBItemData == null)
            {
                m_currPBItemData = new PBItemData()
                {
                    ParseType = PBItemParseType.FullCode,
                    Barcode = itemData.CdItem,
                    CdClass = itemData.CdClass,
                    NmClass = itemData.NmClass,
                    CdItem = itemData.CdItem,
                    NmItem = itemData.NmItem,
                    UtSprc = itemData.UtSprc,
                    Qty = "1",
                    FgTax = itemData.FgTax,
                    CdDp = itemData.CdDp,
                    Properties = PBItemProperties.All
                };

                m_currPBItemData.Parse();
                if (!AddNewItem(m_currPBItemData))
                {
                    CancelCurrentInput();
                    ResetTempItemData();
                    return;
                }
            }
            else
            {
                m_saleView.ItemsGrid_UpdateItemRow(m_currPBItemData);
            }

            if (m_currPBItemData.ScannedStep == PBItemParseStep.FgClass)
            {
                m_saleView.InputState = ItemInputState.ItemGubunEntered;
                m_saleView.ProcessState = SaleProcessState.ItemInputing;
                m_currPBItemData.CompleteStep();
            }
            else
            {
                RestartInputState();
            }
        }

        #endregion

        #region SCAN BARCODE

        /// <summary>
        /// 품번SCAN 처리
        /// </summary>
        /// <param name="scannedText"></param>
        public void ProcessScanCode(string scannedText)
        {
            // Restart or start operation
            StartOperation();

            m_saleView.InputText = scannedText;

            // 거래번호 SCANN
            #region 자동반품, 거래번호 스캔

            if (m_saleView.SaleMode == SaleModes.AutoReturn)
            {
                if (m_saleView.ProcessState != SaleProcessState.AutoRtnProcessing)
                {
                    if (m_saleView.InvokeRequired)
                    {
                        m_saleView.BeginInvoke((MethodInvoker)delegate()
                        {
                            AutoRtnProcessOnEnter();
                        });
                    }
                    else
                    {
                        AutoRtnProcessOnEnter();
                    }
                }
                else
                {
                    m_saleView.InputText = string.Empty;
                }

                return;
            }

            #endregion

            if (!ValidateInput(true))
            {
                return;
            }

            if (m_saleView.InputState == ItemInputState.ItemGubunEntered)
            {
                ReportInvalidState(InvalidDataInputState.InvalidData);
                return;
            }

            try
            {
                m_saleView.InputOperation = ItemInputOperation.Scan;

                if (PBItemData.GetCdDp(scannedText) == CdDpTypes.PB ||
                    (m_currPBItemData != null && m_currPBItemData.CompletedStep >= PBItemParseStep.CdClass))
                {
                    ProcessPBBarCode(scannedText, string.Empty, SLExtensions.CDDP_PB, 0);
                }
                else
                {
                    ProcessPLUCode(scannedText, string.Empty, string.Empty, 0);
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }

        }

        #endregion

        #region PLU

        /// <summary>
        /// PLU키 입력시
        /// </summary>
        void ProcessPLUKey()
        {
            if (!ValidateInput(true))
            {
                return;
            }

            string inputText = m_saleView.InputText;
            m_saleView.InputOperation = ItemInputOperation.PLU;

            // 품번인경우
            if (PBItemData.GetCdDp(inputText) == CdDpTypes.PB
                || (m_currPBItemData != null &&
                    m_currPBItemData.CompletedStep >= PBItemParseStep.CdClass))
            {
                ProcessPBBarCode(inputText, string.Empty, SLExtensions.CDDP_PB, 0);
            }
            else
            {
                // 단품인경우
                ProcessPLUCode(inputText, string.Empty, SLExtensions.CDDP_PLU, 0);
            }
        }

        #endregion

        #region TOUCH GROUPS & ITEMS

        /// <summary>
        /// TOUCH 상품그룹 
        /// </summary>
        void BindTouchGroups()
        {
            List<TouchGroupData> groups = new List<TouchGroupData>();
            var db = MasterDbHelper.InitInstance();
            try
            {
                string query = "M001TouchGroupSelect".POSSLQuerySQL();
                var ds = db.ExecuteQuery(query,
                    new string[] {
                        "@CD_STORE",
                        "@NO_POS",
                        "@OTS_MODE"
                    }, new object[] {
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo,
                        m_saleView.SaleMode == SaleModes.OtherSale || 
                        m_saleView.SaleMode == SaleModes.OtherSaleReturn ? "Y" : "N"
                    });

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    groups.Add(new TouchGroupData()
                    {
                        CdGrop = TypeHelper.ToString(row["CD_GROP"]),
                        NmGrop = TypeHelper.ToString(row["NM_GROP"])
                    });
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                db.EndInstance();
            }

            // bind
            m_touchGroupView.BindGroups(groups.ToArray());
        }

        /// <summary>
        /// TOUCH ITEM 클릭시, 처리
        /// </summary>
        /// <param name="touchItem"></param>
        void ProcessTouchPresetItem(TouchItemData touchItem)
        {
            // Restart or start operation
            StartOperation();

            if (!ValidateInput(false))
            {
                return;
            }

            m_lastTouchItem = touchItem;

            if (touchItem.IsPB)
            {
                ProcessPBBarCode(touchItem.CdItem, touchItem.NmItem, touchItem.CdDp, touchItem.UtSprc);
            }
            else
            {
                // 단품처리
                ProcessPLUCode(touchItem.CdItem, touchItem.NmItem, touchItem.CdDp, touchItem.UtSprc);
            }
        }

        /// <summary>
        /// TOUCH ITEM BY TOUCH GROUP
        /// </summary>
        /// <param name="cdGrop"></param>
        void GetTouchItemsByGroup(string cdGrop)
        {
            List<TouchItemData> items = new List<TouchItemData>();
            var db = MasterDbHelper.InitInstance();
            try
            {
                string query = "M001TouchItemsByGroupSelect".POSSLQuerySQL();
                var ds = db.ExecuteQuery(query,
                    new string[] {
                        "@CD_STORE",
                        "@NO_POS",
                        "@CD_GROP",
                        "@CD_DP"
                    }, new object[] {
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo,
                        cdGrop,
                        m_saleView.SaleMode.ToString().StartsWith("Other")  ? "4" : String.Empty
                    });

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    items.Add(new TouchItemData()
                    {
                        CdGrop = TypeHelper.ToString(row["CD_GROP"]),
                        SqSort = TypeHelper.ToInt32(row["SQ_SORT"]),
                        CdItem = TypeHelper.ToString(row["CD_ITEM"]),
                        CdDp = TypeHelper.ToString(row["CD_DP"]),
                        NmItem = TypeHelper.ToString(row["NM_ITEM"]),
                        UtSprc = TypeHelper.ToInt32(row["UT_SPRC"]),
                    });
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                db.EndInstance();
            }

            m_touchItemView.BindItems(items.ToArray());
        }

        /// <summary>
        /// TOUCH 상품선택
        /// </summary>
        /// <param name="e"></param>
        void m_touchItemView_OnTouch(TouchEventArgs e)
        {
            if (!ValidateKeyInput(OPOSMapKeys.INVALID, false, true))
            {
                return;
            }

            TouchItemData itemData = e.ItemData as TouchItemData;
            m_saleView.InputOperation = ItemInputOperation.Touch;
            ProcessTouchPresetItem(itemData);
        }

        /// <summary>
        /// TOUCH 그룹선택
        /// </summary>
        /// <param name="e"></param>
        void m_touchGroupView_OnTouch(TouchEventArgs e)
        {
            // group selected, bind item list
            if (e.Target == TouchTarget.Group)
            {
                string cdGrop = e.ItemData == null ? string.Empty : (e.ItemData as string);
                GetTouchItemsByGroup(cdGrop);
            }
        }

        #endregion

        #region PRESET KEY

        /// <summary>
        ///  입력된 POS 키보드의 Preset 번호로 Preset 마스터(bsm045t)를 확인한다.
        /// </summary>
        /// <param name="keyCode"></param>
        void ProcessPresetKey(OPOSKeyEventArgs e)
        {
            if (!ValidateInput(false))
            {
                return;
            }

            m_saleView.InputOperation = ItemInputOperation.Preset;

            // PRESET 확인            
            string keyCode = e.Key.OPOSKey.ToString();
            keyCode = keyCode.Substring(keyCode.IndexOf("PRESET") + 6);

            TouchItemData itemData = null;
            var db = MasterDbHelper.InitInstance();
            try
            {
                string query = "M001PRESETItemCheck".POSSLQuerySQL();
                var dataRow = db.ExecuteQueryDataRow(query,
                    new string[] {
                        "@CD_STORE",
                        "@NO_POS",
                        "@NO_KEY"
                    },
                    new object[] {
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo,
                        TypeHelper.ToInt32(keyCode).ToString()
                    });

                if (dataRow != null)
                {
                    itemData = new TouchItemData()
                    {
                        CdDp = dataRow["CD_DP"].ToString(),
                        CdItem = dataRow["CD_ITEM"].ToString(),
                        UtSprc = TypeHelper.ToInt32(dataRow["UT_SPRC"]),
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                db.EndInstance();
            }

            if (itemData == null)
            {
                m_saleView.ReportErrorMessage(SaleViewErrorMessage.NoPresetItem);
            }
            else
            {
                ProcessTouchPresetItem(itemData);
            }
        }

        #endregion

        #region 초기화, 상태업데이트

        /// <summary>
        /// 거래중지때, 결제완료때
        /// </summary>
        void SaleIntialize()
        {
            m_summaryData = new SaleSummaryData();
            BasketHeader = new BasketHeader()
            {
                TrxnType = m_saleView.SaleMode.ToString().StartsWith("Other") ? NetCommConstants.TRXN_TYPE_OTH_SALE :
                    NetCommConstants.TRXN_TYPE_SALE,
                CancType = m_saleView.SaleMode.ToString().EndsWith("Return") ? NetCommConstants.CANCEL_TYPE_RETURN :
                    NetCommConstants.CANCEL_TYPE_NORMAL
            };

            BasketCashReceipt = new BasketCashRecpt();
            BasketPointSave = new BasketPointSave();
            BasketItems = new List<BasketItem>();
            BasketSubTtl = new BasketSubTotal();
            BasketPays = new List<BasketPay>();

            // TOUCH 상품 그룹 로드
            BindTouchGroups();

            RestartInputState();
            CustInfo = null;
            m_saleItemType = SaleItemType.NoItem;
            m_lastTouchItem = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateSummary()
        {
            m_summaryData.Update(m_saleView.DataRows);
            m_saleView.UpdateSummary(m_summaryData);
            m_saleView.UpdatePayList(this.BasketPays);
        }

        void RestartInputState()
        {
            m_saleView.InputState = ItemInputState.Ready;
            m_saleView.ProcessState = SaleProcessState.InputStarted;

            ResetTempItemData();
        }

        /// <summary>
        /// 현재 임시 관리 하고 있는 품번객체 RESET한다
        /// 처음 품번 입력/스캔을 시작한다
        /// </summary>
        void ResetTempItemData()
        {
            if (m_currPBItemData != null)
            {
                m_currPBItemData.CompletedStepEvent -= new PBItemScannedEventHandler(m_currPBItemData_CompletedStepEvent);
                m_currPBItemData.ScannedEvent -= new PBItemScannedEventHandler(m_currPBItemData_ScannedEvent);
                m_currPBItemData = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saleMode"></param>
        /// <param name="checkCondition">상품있으면 안 됨</param>
        /// <param name="validateAdmin">관리자확인여부</param>
        /// <param name="resetSummary">결제내역reset여부</param>
        /// <returns></returns>
        public ChangeSaleModeStatus ChangeSaleMode(SaleModes saleMode, bool checkCondition, bool validateAdmin, bool resetItems)
        {
            // autoRtnTrxnInfo1.BackColor = pnlRightTop.BackColor;
            if (m_saleView.SaleMode != SaleModes.Sale && m_saleView.SaleMode == saleMode)
            {
                // back to normal
                return ChangeSaleMode(saleMode.GetReverseMode(), true, false, true);
            }

            if (checkCondition)
            {
                if (m_saleView.HasItems)
                {
                    return ChangeSaleModeStatus.InvalidCondition;
                }
            }

            if (validateAdmin)
            {
                if (string.IsNullOrEmpty(m_saleView.ValidateAdmin()))
                {
                    return ChangeSaleModeStatus.NoPermission;
                }
            }

            // 결재완료모드전황 => 대기모드전환
            m_saleView.SaleMode = saleMode;
            SaleIntialize();
            m_saleView.ItemsGrid_DataInitialize(resetItems);

            if (resetItems)
            {
                m_summaryData.Update(m_saleView.DataRows);
                m_saleView.UpdateSummary(m_summaryData);
                m_saleView.UpdatePayList(null);
            }

            m_saleView.ProcessState = SaleProcessState.Completed;
            return ChangeSaleModeStatus.Success;
        }

        /// <summary>
        /// 화면State
        /// </summary>
        /// <param name="inputState"></param>
        public void ReportInvalidState(InvalidDataInputState inputState)
        {
            switch (inputState)
            {
                case InvalidDataInputState.InvalidQty:
                case InvalidDataInputState.InvalidData:
                case InvalidDataInputState.NumberOverflow:
                case InvalidDataInputState.InputChangePrice:
                case InvalidDataInputState.TotalAmtOverflow:
                    ProcessOnClear();
                    break;
                case InvalidDataInputState.OnlyOfflineItem:
                case InvalidDataInputState.OnlyOnlineItem:
                    m_saleView.InputText = string.Empty;
                    break;
                default:
                    break;
            }

            if (m_saleView.InvokeRequired)
            {
                m_saleView.BeginInvoke((MethodInvoker)delegate()
                {
                    m_saleView.ReportInvalidState(inputState);
                });
            }
            else
            {
                m_saleView.ReportInvalidState(inputState);
            }

            m_lastInputState = inputState;
        }

        #endregion

        #region 상품관리

        public void LoadItems(BasketItem[] items)
        {
            SaleIntialize();
            m_saleItemType = items.Length > 0 ? SLExtensions.GetSaleItemType(items[0].FgMargin) : SaleItemType.NoItem;
            m_saleView.ItemsGrid_DataInitialize(true);
            m_saleView.RestoreFromBaskets(items);

            m_summaryData.Update(items);
            m_saleView.UpdateSummary(m_summaryData);
            m_saleView.ProcessState = SaleProcessState.Completed;
        }

        /// <summary>
        /// 추가가능한지 확인
        /// </summary>
        /// <param name="itemData"></param>
        /// <returns></returns>
        bool AddNewItem(PBItemData itemData)
        {
            var si = SLExtensions.GetSaleItemType(itemData);
            if (si != SaleItemType.NoItem &&
                m_saleItemType != SaleItemType.NoItem &&
                si != m_saleItemType)
            {
                ReportInvalidState(m_saleItemType == SaleItemType.OnlineItem ?
                    InvalidDataInputState.OnlyOnlineItem :
                    InvalidDataInputState.OnlyOfflineItem);
                return false;
            }

            if (si != SaleItemType.NoItem)
            {
                m_saleItemType = si;
            }

            if (!SLExtensions.CDDP_PB.Equals(itemData.CdDp))
            {
                // 단품
                var existItem = m_saleView.DataRows.FirstOrDefault(p => p.Barcode == itemData.Barcode);
                if (existItem == null)
                {
                    m_saleView.ItemsGrid_AddItemRow(m_currPBItemData, true);
                }
                else
                {
                    // add 1 qty
                    var nItem = itemData.Copy();
                    nItem.Properties = PBItemProperties.Qty;
                    nItem.Qty = "-1"; // add 1

                    int rowIndex = m_saleView.DataRows.ToList().IndexOf(existItem);
                    m_saleView.ItemsGrid_UpdateItemRow(rowIndex, nItem);
                }
            }
            else
            {
                // 품번
                m_saleView.ItemsGrid_AddItemRow(m_currPBItemData, true);
            }

            return true;
        }

        #endregion

        #region 지정취소
        void ProcessLineVoid()
        {
            PBItemData itemData = null;
            PBItemData curItemData = null;

            int lineIndex = -1; // current row
            string inputText = m_saleView.InputText;
            m_saleView.InputText = string.Empty;

            if (string.IsNullOrEmpty(inputText))
            {
                curItemData = m_saleView.ItemsGrid_CurrentItem;
            }
            else
            {
                lineIndex = TypeHelper.ToInt32(inputText);
                if (lineIndex.InRange(1, m_saleView.DataRows.Length, true))
                {
                    curItemData = m_saleView.DataRows[lineIndex - 1];
                }
            }

            if (curItemData == null)
            {
                ReportInvalidState(InvalidDataInputState.InvalidData);
                return;
            }

            if (!"0".Equals(curItemData.FgCanc) ||
                string.IsNullOrEmpty(curItemData.CdClass))
            {
                ReportInvalidState(!"0".Equals(curItemData.FgCanc) ?
                    InvalidDataInputState.InvalidData :
                    InvalidDataInputState.InvalidKey);
                return;
            }

            itemData = new PBItemData()
            {
                Properties = PBItemProperties.FgCanc,
                FgCanc = "2", // 지정
                Qty = "0",
                QtyCanc = curItemData.Qty
            };

            if (lineIndex == -1)
            {
                m_saleView.ItemsGrid_UpdateItemRow(itemData);
            }
            else
            {
                m_saleView.ItemsGrid_UpdateItemRow(lineIndex - 1, itemData);
            }

        }
        #endregion

        #region 수량변경
        void ProcessChangeQty()
        {
            if (string.IsNullOrEmpty(m_saleView.ItemsGrid_CurrentItem.CdClass) ||
                   string.IsNullOrEmpty(m_saleView.ItemsGrid_CurrentItem.CdDp))
            {
                ReportInvalidState(InvalidDataInputState.InvalidKey);
                return;
            }

            // 수량변경한다
            string iQty = string.IsNullOrEmpty(m_saleView.InputText) ? "-1" : m_saleView.InputText;
            bool valid = false;
            int qty = iQty.ValidateNumber(out valid);
            if (!valid)
            {
                ReportInvalidState(InvalidDataInputState.InvalidQty);
                return;
            }

            if (!ValidateOverQty(qty))
            {
                ReportInvalidState(InvalidDataInputState.InvalidQty);
                return;
            }

            #region Amount validation

            int validate = ValidateOverAmt(true, qty);
            if (validate != 0)
            {
                ReportInvalidState(validate == 1 ? InvalidDataInputState.ItemAmtOverflow
                    : InvalidDataInputState.TotalAmtOverflow);
                return;
            }
            #endregion

            var itemData = new PBItemData()
            {
                Properties = PBItemProperties.Qty | PBItemProperties.FgCanc,
                Qty = qty.ToString(),
                QtyCanc = "0",
                FgCanc = "0"
            };

            m_saleView.ItemsGrid_UpdateItemRow(itemData);
            m_saleView.InputText = string.Empty;
        }
        #endregion

        #region 가격변경
        void ProcessChangePrice()
        {
            if (string.IsNullOrEmpty(m_saleView.ItemsGrid_CurrentItem.CdClass) ||
                    string.IsNullOrEmpty(m_saleView.ItemsGrid_CurrentItem.FgClass))
            {
                ReportInvalidState(InvalidDataInputState.InvalidKey);
                return;
            }

            if (!"0".Equals(m_saleView.ItemsGrid_CurrentItem.FgCanc) ||
                string.IsNullOrEmpty(m_saleView.InputText))
            {
                ReportInvalidState(InvalidDataInputState.InvalidData);
                return;
            }

            // 가격변경
            #region Validate price and total amount

            bool valid = false;
            var price = m_saleView.InputText.ValidateMoney(0, out valid);
            if (!valid)
            {
                ReportInvalidState(InvalidDataInputState.NumberOverflow);
                return;
            }

            if (price <= 0)
            {
                ReportInvalidState(InvalidDataInputState.InputChangePrice);
                return;
            }

            int validate = ValidateOverAmt(false, price);
            if (validate != 0)
            {
                ReportInvalidState(validate == 1 ? InvalidDataInputState.ItemAmtOverflow :
                    InvalidDataInputState.TotalAmtOverflow);
                return;
            }
            #endregion

            var itemData = new PBItemData()
            {
                Properties = PBItemProperties.Price,
                UtSprc = price.ToString()
            };

            m_saleView.ItemsGrid_UpdateItemRow(itemData);
            m_saleView.InputText = string.Empty;
        }
        #endregion

        #region 보류키이벤트
        void ProcessHoldKey(OPOSKeyEventArgs e)
        {
            e.IsHandled = true;

            // if 반품, 안됨
            if (m_saleView.StateRefund)
            {
                ReportInvalidState(InvalidDataInputState.InvalidKey);
                return;
            }

            // 상품 을때, 보류건있는지확인 및 표시
            if (!m_saleView.HasItems)
            {
                m_saleView.ProcessKeyEvent(e);
            }
            else
            {
                if (!ProcessHold())
                {
                    ReportInvalidState(InvalidDataInputState.InvalidKey);
                }
            }
        }
        #endregion

        #region 만원키
        void ProcessManWonKey()
        {
            var number = TypeHelper.ToInt64(m_saleView.InputText);
            if (number == 0)
            {
                number = 1;
            }

            number *= 10000;
            if (number.ToString().Length < m_saleView.InputLength)
            {
                m_saleView.InputText = number.ToString();
            }

            // 만원키 사용시 현금결제로 처리
            if (m_saleView.ProcessState == SaleProcessState.SubTotal)
            {
                ProcessPayCash();
            }
        }
        #endregion

        #region Validation Logic

        InvalidDataInputState m_lastInputState = InvalidDataInputState.None;
        void StartOperation()
        {
            m_saleView.ItemsGrid_StartOperation();
            if (m_lastInputState != InvalidDataInputState.None)
            {
                m_saleView.RestoreGuideMessage();
                m_saleView.InputText = string.Empty;
                m_lastInputState = InvalidDataInputState.None;
            }
        }

        /// <summary>
        /// 수량OVER 확인
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool ValidateOverQty(int value)
        {
            if (value == 0)
            {
                return false;
            }

            int oQty = value;
            if (value == -1)
            {
                oQty = TypeHelper.ToInt32(m_saleView.ItemsGrid_CurrentItem.Qty) + 1;
            }

            return oQty <= 999;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isQty"></param>
        /// <param name="value"></param>
        /// <returns>0: valid, 1: item amout over, 2: total amt over</returns>
        int ValidateOverAmt(bool isQty, int value)
        {
            // current item
            int cQty = TypeHelper.ToInt32(m_saleView.ItemsGrid_CurrentItem.Qty);
            int oval = isQty ? TypeHelper.ToInt32(m_saleView.ItemsGrid_CurrentItem.UtSprc) :
                cQty == 0 ? 1 : cQty;

            int oQty = value;
            if (isQty && value == -1)
            {
                oQty = TypeHelper.ToInt32(m_saleView.ItemsGrid_CurrentItem.Qty) + 1;
            }

            Int64 intValue = oQty * oval;
            bool valid = false;
            var amt = intValue.ToString().ValidateMoney(1, out valid);
            if (!valid)
            {
                return 1;
            }

            //  total sum
            Int64 total = m_summaryData.TotalAmt + amt;
            total.ToString().ValidateMoney(2, out valid);
            return valid ? 0 : 2;
        }

        public int GetTaxAmt(int payAmt)
        {
            return Convert.ToInt32(m_summaryData.CalcTaxPerc(Convert.ToInt32(payAmt), true));
        }

        /// <summary>
        /// Validate input text over tender
        /// </summary>
        /// <param name="payGrpCd"></param>
        /// <param name="payDtlCd"></param>
        /// <returns></returns>
        bool ValidateOverTender(string payGrpCd, string payDtlCd, string inputText, out int rPayAmt)
        {
            bool valid = false;
            int recvAmt = m_summaryData.RecvAmt;
            rPayAmt = inputText.ValidateMoney(2, out valid);
            if (!valid)
            {
                ReportInvalidState(InvalidDataInputState.OverPayAmount);
                return false;
            }

            // 현금/수표
            // 타사상품권	
            // 자사상품권
            // 상품교환권					
            // 구상품교환권
            if (NetCommConstants.PAYMENT_DETAIL_CASH.Equals(payDtlCd) ||
                NetCommConstants.PAYMENT_DETAIL_CASH_CHECK.Equals(payDtlCd) ||
                NetCommConstants.PAYMENT_DETAIL_TICKET_OTHER.Equals(payDtlCd) ||
                NetCommConstants.PAYMENT_DETAIL_TICKET.Equals(payDtlCd) ||
                NetCommConstants.PAYMENT_DETAIL_EXCHANGE.Equals(payDtlCd) ||
                NetCommConstants.PAYMENT_DETAIL_EXCHANGE_OLD.Equals(payDtlCd))
            {
                return true;
            }

            // 신용카드					
            // 타건카드					
            // 타건복지
            // 현금IC
            // 할인쿠폰
            // 결제할인
            // 포인트지불					
            if (NetCommConstants.PAYMENT_GROUP_CARD.Equals(payGrpCd) ||
                NetCommConstants.PAYMENT_DETAIL_CASH_IC.Equals(payDtlCd) ||
                NetCommConstants.PAYMENT_DETAIL_COUPON.Equals(payDtlCd) ||
                NetCommConstants.PAYMENT_DETAIL_DISCOUNT.Equals(payDtlCd) ||
                NetCommConstants.PAYMENT_DETAIL_POINT.Equals(payDtlCd))
            {
                valid = rPayAmt <= recvAmt;
            }
            else if (NetCommConstants.PAYMENT_DETAIL_ONLINE.Equals(payDtlCd))
            {
                // 특수/통신판매/F0/On-Line
                valid = rPayAmt == recvAmt;
            }

            if (!valid)
            {
                ReportInvalidState(InvalidDataInputState.OverPayAmount);
            }

            return valid;
        }

        /// <summary>
        /// 소계취소 가능한지
        /// </summary>
        /// <returns></returns>
        bool CanCancelSubTotal()
        {
            // 신용카드
            // 현금IC
            // 현금영수증
            if (this.BasketPays.Count == 0)
            {
                return true;
            }

            int cnt = this.BasketPays.Where(p =>
                NetCommConstants.PAYMENT_DETAIL_CARD.Equals(p.PayDtlCd) ||
                NetCommConstants.PAYMENT_DETAIL_CASH_IC.Equals(p.PayDtlCd) ||
                NetCommConstants.PAYMENT_DETAIL_POINT.Equals(p.PayDtlCd) ||
                NetCommConstants.PAYMENT_DETAIL_EXCHANGE.Equals(p.PayDtlCd)
                ).ToArray().Length;

            // check 포인트사용 basket

            return cnt == 0;
        }

        bool ValidateInput(bool checkInputLength)
        {
            if (checkInputLength)
            {
                if (m_saleView.InputState == ItemInputState.Ready ||
                    m_saleView.ProcessState == SaleProcessState.InputStarted)
                {
                    if (m_saleView.InputText.Length < 6)
                    {
                        ReportInvalidState(InvalidDataInputState.LengthError);
                        return false;
                    }
                }

                if (string.IsNullOrEmpty(m_saleView.InputText))
                {
                    return false;
                }
            }

            // 상품갯수확인
            if (m_saleView.ProcessState == SaleProcessState.InputStarted
                && m_saleView.DataRows.Length == SLExtensions.MAX_TOTAL_ITEM)
            {
                ReportInvalidState(InvalidDataInputState.OverItemCount);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 품번구분확인
        /// 0: 성공
        /// 1: 온라인상품만
        /// 2: 일반상품만
        /// </summary>
        /// <param name="fgClass"></param>
        /// <returns></returns>
        int ValidateFgClass(string fgClass)
        {
            var si = SLExtensions.GetSaleItemType(fgClass);
            if (si == SaleItemType.NoItem)
            {
                return 0;
            }

            if (m_saleItemType == SaleItemType.NoItem ||
                m_saleItemType == si)
            {
                m_saleItemType = si;
                return 0;
            }
            else
            {
                return m_saleItemType == SaleItemType.OnlineItem ? 1 : 2;
            }
        }

        /// <summary>
        /// 저장물판매시 barcode가 22, 29제외
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        bool ValidateOtherSale(string barCode)
        {
            if (m_saleView.SaleMode == SaleModes.OtherSale || m_saleView.SaleMode == SaleModes.OtherSaleReturn)
            {
                if (barCode.StartsWith(PBItemData.GoodsCodePrefix1) || barCode.StartsWith(PBItemData.GoodsCodePrefix2))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
