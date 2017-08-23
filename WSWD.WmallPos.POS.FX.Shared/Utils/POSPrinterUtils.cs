//-----------------------------------------------------------------
/*
 * 화면명   : PosPrinterUtils.cs
 * 화면설명 : POS관련 프린트
 * 개발자   : 정광호
 * 개발일자 : 2015.03.30
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.FX.Shared.Utils;
using System.Text.RegularExpressions;
using System.Data;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using System.Windows.Forms;
using System.Drawing;
using WSWD.WmallPos.FX.Shared.Helper;
using System.Diagnostics;
using System.IO;

namespace WSWD.WmallPos.FX.Shared.Utils
{
    public class POSPrinterUtils
    {
        #region IPrinterUtils Members

        private static POSPrinterUtils m_instance = null;
        public static POSPrinterUtils Instance
        {
            get
            {
                return m_instance;
            }
        }

        public static void SetPrinterUtilsInstance(POSPrinterUtils printUtils)
        {
            m_instance = printUtils;
        }

        IPrinterDevice m_device;
        public POSPrinterUtils(IPrinterDevice device)
        {
            m_device = device;
        }


        #endregion

        #region 기본 타이틀

        /// <summary>
        /// 기본 판매 타이틀
        /// </summary>
        /// <param name="bPrint">true : 프린트 출력, false : 화면 출력</param>
        /// <param name="basket">헤더정보</param>
        /// <param name="trxnNo">거래번호</param>
        /// <returns></returns>
        public string ReceiptBaseTitle(bool bPrint, BasketHeader basketHeader, string trxnNo)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                string strCasName = string.Empty;
                string strTelNo = string.Empty;
                string strPosNo = string.Empty;
                string strTime = string.Empty;

                if (basketHeader != null)
                {
                    strCasName = basketHeader.CasName;
                    strTelNo = basketHeader.PosTelNo;
                    strPosNo = string.Format("{0}-{1}", basketHeader.PosNo, basketHeader.TrxnNo);
                    strTime = DateTimeUtils.GetCurrentDateTimePrint(basketHeader.SaleDate, basketHeader.OccrTime);
                }
                else
                {
                    strCasName = ConfigData.Current.AppConfig.PosInfo.CasName;
                    strTelNo = ConfigData.Current.AppConfig.PosInfo.PosTelNo;
                    strPosNo = string.Format("{0}-{1}", ConfigData.Current.AppConfig.PosInfo.PosNo,
                        trxnNo.Length <= 0 ? ConfigData.Current.AppConfig.PosInfo.TrxnNo : trxnNo);
                    strTime = DateTimeUtils.GetCurrentDateTimePrint(null);
                }

                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(FXConsts.RECEIPT_DEFAULT_CASNAME);
                sb.Append(getFixCut(true, PrintTypes.WideNormal, strCasName, 13));

                sb.Append(FXConsts.RECEIPT_DEFAULT_TEL);
                sb.Append(getFixCut(true, PrintTypes.WideNormal, strTelNo, 19));
                sb.Append(Environment.NewLine);
                sb.Append(FXConsts.RECEIPT_DEFAULT_POSNO);
                sb.Append(getFixCut(true, PrintTypes.WideNormal, strPosNo, 16));
                sb.Append(getFixCut(true, PrintTypes.WideNormal, strTime, 20));
                sb.Append(Environment.NewLine);
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }

            return sb.ToString();
        }

        #endregion

        #region SINGON

        /// <summary>
        /// 사인 On, Off
        /// </summary>
        /// <param name="bPrint">true : 프린트 출력, false : 화면 출력</param>
        /// <param name="Type">사인 On,Off 구분</param>
        /// <param name="basketHeader">헤더정보</param>
        /// <param name="dtTime">사인 On,Off 시간</param>
        /// <returns></returns>
        public string PrintReceiptSignOn(bool bPrint, bool isSignOn, string Type, BasketHeader basketHeader, DateTime dtTime)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Append(ReceiptBaseTitle(bPrint, basketHeader, string.Empty));

                string signDate = string.Format("{0}({1}) ", dtTime.ToString("yyyy/MM/dd"), dtTime.ToString("dddd"));
                string signTime = string.Format(ConfigData.Current.SysMessage.GetMessage("00303"), (dtTime.Hour > 12) ? ConfigData.Current.SysMessage.GetMessage("00304") : ConfigData.Current.SysMessage.GetMessage("00305"), dtTime.ToString("hh"), dtTime.ToString("mm"));

                sb.Append(string.Format(FXConsts.RECEIPT_DEFAULT_DATE_POS, signDate));
                sb.Append(Environment.NewLine);
                sb.Append(string.Format(FXConsts.RECEIPT_DEFAULT_TIME_POS, signTime));
                sb.Append(Environment.NewLine);
                sb.Append(string.Format(FXConsts.RECEIPT_SIGN_TRXNO, basketHeader.TrxnNo));
                sb.Append(Environment.NewLine);
                sb.Append(string.Format(FXConsts.RECEIPT_SIGN_SD, basketHeader.CasName, basketHeader.CasNo));
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append(string.Format(string.Format(Type, new string('.', 13))));
                sb.Append(FXConsts.PRINT_LAST);
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }
            finally
            {
                Print(bPrint, isSignOn ? "SIGNON" : "SIGNOFF", true, sb.ToString(), string.Empty, false);
            }

            return sb.ToString();
        }

        #endregion

        #region 개설

        /// <summary>
        /// 개설주프린트함수
        /// </summary>
        public void PrintReceiptSdOpen(BasketHeader basketHeader, string printBody)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                string strCurrentDate = string.Format("{0} {1} ({2})", DateTimeUtils.GetYearMonthKorString(DateTime.Now), DateTime.Now.ToString("dd"), DateTimeUtils.GetDayFKorString(DateTime.Now));
                string strCurrentTime = string.Format("({0}) {1}시 {2}분 ", (DateTime.Now.Hour > 12) ? "오후" : "오전", DateTime.Now.ToString("hh"), DateTime.Now.ToString("mm"));

                sb.Append(ReceiptBaseTitle(true, basketHeader, string.Empty));
                sb.Append(Environment.NewLine);
                sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(string.Format(FXConsts.RECEIPT_SD_START, new string('*', 8)));
                sb.Append(Environment.NewLine);
                sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(string.Format(FXConsts.RECEIPT_DEFAULT_DATE, strCurrentDate));
                sb.Append(Environment.NewLine);
                sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(string.Format(FXConsts.RECEIPT_DEFAULT_TIME, strCurrentTime));
                sb.Append(Environment.NewLine);
                sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(string.Format(FXConsts.RECEIPT_DEFAULT_POSNO2, ConfigData.Current.AppConfig.PosInfo.PosNo));
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append(printBody);
                sb.Append(Environment.NewLine);
                sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(string.Format(FXConsts.RECEIPT_SD_END, new string('*', 8)));
                sb.Append(Environment.NewLine);
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }
            finally
            {
                Print(true, FXConsts.RECEIPT_NAME_SDOPEN, true, sb.ToString(), string.Empty, true);
            }
        }

        /// <summary>
        /// 개설 내용
        /// </summary>
        /// <param name="type">장비타입</param> 0 - POS, 1- PDA
        /// <param name="name">항목</param>
        /// <param name="sendCnt">전송완료건수</param>
        /// <param name="send">전송대상건수</param>
        /// <param name="posNo">전송결과(OK/ER)</param>
        /// <returns></returns>
        public static string ReceiptSdOpenBody(string type, string name, int completeCnt, int targetCnt, string sendFg)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(getFixCut(true, PrintTypes.WideNormal, name, ("0".Equals(type) ? 28 : 25)));
                sb.Append(getFixCut(false, PrintTypes.WideNormal, completeCnt.ToString(), 4)).Append("/");
                sb.Append(getFixCut(true, PrintTypes.WideNormal, string.Format("  {0}", targetCnt), 6));
                sb.Append(getFixCut(true, PrintTypes.WideNormal, string.Format(" {0}", sendFg), 3));

                sb.Append(Environment.NewLine);
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }

            return sb.ToString();
        }

        #endregion

        #region 보류

        /// <summary>
        /// 보류
        /// </summary>
        /// <param name="trxnNo">거래번호</param>
        /// <param name="noBoru">보류번호</param>
        /// <param name="itemList">판매상품</param>
        /// <param name="totalAmt">총금액</param>
        /// <param name="dsMsg">점포명판 메시지</param>
        public void PrintReceiptHold(string trxnNo, string noBoru, string itemList, long totalAmt, DataSet dsMsg)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                //기본 타이틀
                sb.Append(ReceiptBaseTitle(true, null, trxnNo));

                #region 점포명판 메세지

                if (dsMsg != null && dsMsg.Tables.Count > 0 && dsMsg.Tables[0] != null && dsMsg.Tables[0].Rows.Count > 0)
                {
                    sb.Append(PrintMessage(true, dsMsg.Tables[0]));
                }

                #endregion

                // title center
                sb.Append(FXConsts.RECEIPT_BORU_TITLE_01);
                sb.Append(" " + noBoru);
                sb.Append(FXConsts.RECEIPT_BORU_TITLE_CENTER);
                sb.Append(" ");
                sb.Append(FXConsts.RECEIPT_BORU_TITLE_01);
                sb.Append(Environment.NewLine);
                sb.Append(itemList);

                // line
                sb.Append(FXConsts.PRINT_LINEFEED);
                sb.Append(Environment.NewLine);
                sb.Append(FXConsts.PRINT_BOLDWIDE_DOUBLE);
                sb.Append(getFixCut(true, PrintTypes.WideDouble, FXConsts.RECEIPT_BORU_TITLE_SUM, 14));
                sb.Append(getFixCut(false, PrintTypes.WideDouble, string.Format("{0:#,##0}", totalAmt), 25));
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }
            finally
            {
                Print(true, FXConsts.RECEIPT_NAME_HOLD, true, sb.ToString(), string.Format("A{0}{1:yyMMdd}{2}{3}A", ConfigData.Current.AppConfig.PosInfo.StoreNo, DateTime.Today, ConfigData.Current.AppConfig.PosInfo.PosNo, noBoru), true);
            }
        }

        #endregion

        #region 거래중지 영수증

        /// <summary>
        /// 거래중지 영수증
        /// </summary>
        /// <param name="bPrint">true :  프린트 출력, false : 화면 출력</param>
        /// <param name="basketHeader">헤더정보</param>
        /// <param name="itemDetails">판매상품정보</param>
        /// <param name="totalAmt">총금액</param>
        /// <param name="bRePrint">true : 재발행 , false : 발행</param>
        /// <param name="dsMsg">점포명판 메시지</param>
        /// <returns></returns>
        public string PrintReceiptNoSale(bool bPrint, BasketHeader basketHeader, string itemDetails, Int64 totalAmt, bool bRePrint, DataSet dsMsg)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                #region 무효영수증
                sb.Append(bPrint && bRePrint ? RePrint() : "");
                #endregion

                #region 점포명판 메세지

                if (dsMsg != null && dsMsg.Tables.Count > 0 && dsMsg.Tables[0] != null && dsMsg.Tables[0].Rows.Count > 0)
                {
                    sb.Append(PrintMessage(bPrint, dsMsg.Tables[0]));
                }

                #endregion

                sb.Append(ReceiptBaseTitle(bPrint, basketHeader, string.Empty));

                // title center
                sb.Append(Environment.NewLine);
                sb.Append(FXConsts.RECEIPT_CANCALL_TITLE_01);
                sb.Append(FXConsts.RECEIPT_CANCALL_TITLE_CENTER);
                sb.Append(FXConsts.RECEIPT_CANCALL_TITLE_01);
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);

                //제목
                sb.Append(FXConsts.PRINT_DLINEFEED);
                sb.Append(Environment.NewLine);
                sb.Append(FXConsts.RECEIPT_PRINT_DESC_01);
                sb.Append(Environment.NewLine);
                sb.Append(FXConsts.PRINT_LINEFEED);
                sb.Append(Environment.NewLine);

                // 상품
                sb.Append(itemDetails);

                // line
                sb.Append(new string('-', 40));
                sb.Append(Environment.NewLine);

                // 합 계
                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                sb.Append(FXConsts.RECEIPT_CANCALL_TITLE_SUM);
                sb.Append(getFixCut(false, PrintTypes.WideDouble, string.Format("{0:#,##0}", totalAmt), 24));
                sb.Append(Environment.NewLine);

                #region 무효영수증
                sb.Append(bPrint && bRePrint ? RePrint() : "");
                #endregion

                sb.Append(FXConsts.PRINT_LAST);
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }
            finally
            {
                Print(bPrint, FXConsts.RECEIPT_NAME_NOSALE, true, sb.ToString(), string.Empty, false);
            }

            return sb.ToString();
        }

        #endregion

        #region 결제

        #region 판매상품 결제

        /// <summary>
        /// 결제(현금, 신용카드, 상품교환권, 타사상품권, 타건복지,  현금IC, 포인트, 할인쿠폰, 결제할인)
        /// </summary>
        /// <param name="bPrint">true :  프린트 출력, false : 화면 출력</param>
        /// <param name="bRePrint">재발행여부(true:재발행, false:발행)</param>
        /// <param name="bOnSalePrint"></param>
        /// <param name="bExchangePrint"></param>
        /// <param name="basketHeader">헤더정보</param>
        /// <param name="basketBase">판매정보</param>
        /// <param name="bpCard">카드정보</param>
        /// <param name="dsMsg">점포명판 메세지, W-Mall 광고 메세지, 브랜드 광고 메세지, 안내 메세지</param>
        /// <param name="dsPromotion">프로모션정보</param>
        /// <param name="dtDcc">DCC 정보</param>
        /// <param name="cancType">반품여부(0:정산판매, 1:반품, 2: 반품)</param>
        /// <param name="cardPrint">수기서명 여부(0:서명, 1:서명X(청구용), 2:서명X(보관용)</param>
        /// <param name="bCash"></param>
        /// <returns></returns>
        public string SetPrintPay(bool bPrint, bool bRePrint, bool bOnSalePrint, bool bExchangePrint,
            BasketHeader basketHeader, List<BasketBase> basketBase, BasketPayCard bpCard,
            DataSet dsMsg, DataTable dtPromotion, DataTable dtDcc,
            CancelPrint cancType, CardPrint cardPrint, bool bCash)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                string strUINm = string.Empty;      //출력명(저널입력)

                int iPayAmt = 0;                    //합계
                int iBalAmt = 0;                    //잔액
                int iTempPay = 0;                   //결제금액
                int iTempBal = 0;                   //결제잔액
                Int64 iExchangeAmt = 0;             //상품교환권 금액(구상품교환권 포함)
                Int64 iExchangeBal = 0;             //상품교환권 잔액(구상품교환권 포함)

                string tempVal = string.Empty;
                string sbTemp = string.Empty;

                bool bCancType = cancType != CancelPrint.Normal ? true : false;                             //반품여부
                bool bOSale = basketHeader.TrxnType != NetCommConstants.TRXN_TYPE_OTH_SALE ? true : false;  //저장물 판매여부(true:일반상품, false:저장물)

                // 상품교환권 교환권 출력에서 재발행인 경우에는 출력 하지 않는다.
                // BY KHJ 2016.03.28
                if (bExchangePrint && bRePrint)
                    return "";

                if (CardPrint.Basic == cardPrint)
                {
                    #region 반품일시 현금만 있는지 확인

                    if (bCancType && !bCash)
                    {
                        foreach (var item in basketBase)
                        {
                            if (item.GetType().Name.ToString().Equals("BasketPayCash"))
                            {
                                #region 현금

                                BasketPayCash bp = (BasketPayCash)item;

                                if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                                {
                                    iTempPay = (bp.PayAmt != null ? TypeHelper.ToInt32(bp.PayAmt) : 0);
                                    iTempBal = (bp.BalAmt != null ? TypeHelper.ToInt32(bp.BalAmt) : 0);

                                    if (iTempPay > 0)
                                    {
                                        if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_SPECIAL && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_ONLINE)
                                        {
                                            bCash = false;
                                            break;
                                        }
                                        else
                                        {
                                            //현금    
                                            bCash = true;
                                        }

                                    }
                                }
                                #endregion
                            }
                            else if (item.GetType().Name.ToString().Equals("BasketPayCard") || item.GetType().Name.ToString().Equals("BasketExchange") || item.GetType().Name.ToString().Equals("BasketOtherTicket") ||
                                item.GetType().Name.ToString().Equals("BasketPay") || item.GetType().Name.ToString().Equals("BasketCashIC") || item.GetType().Name.ToString().Equals("BasketPoint") ||
                                item.GetType().Name.ToString().Equals("BasketCoupon") || item.GetType().Name.ToString().Equals("BasketOldExGift"))
                            {
                                bCash = false;
                                break;
                            }
                        }
                    }

                    #endregion

                    #region 신용카드 수기서명 출력

                    //신용카드 수기서명 자료가 존재할경우 우선적으로 신용카드 청구용 영수증(cardPrint = 1) 및 보관용 영수증(cardPrint = 2)을 출력한다

                    foreach (var item in basketBase)
                    {
                        if (item.GetType().Name.ToString().Equals("BasketPayCard"))
                        {
                            BasketPayCard bp = (BasketPayCard)item;

                            if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                            {
                                iTempPay = (bp.PayAmt != null ? TypeHelper.ToInt32(bp.PayAmt) : 0);
                                iTempBal = (bp.BalAmt != null ? TypeHelper.ToInt32(bp.BalAmt) : 0);

                                if (iTempPay > 0 && TypeHelper.ToString(bp.CVM).Length <= 0 || TypeHelper.ToString(bp.CVM) == "N")
                                {
                                    //카드 수기서명시 청구용 영수증
                                    sbTemp = string.Empty;
                                    sbTemp = SetPrintPay(bPrint, bRePrint, false, false, basketHeader, basketBase, bp, dsMsg, dtPromotion, dtDcc, cancType, CardPrint.Charge, bCash);
                                    sb.Append(!bPrint ? sbTemp : "");
                                    // 여전법 추가 0808(KSK)
                                    sbTemp = string.Empty;

                                    //카드 수기서명시 보관용 영수증
                                    sbTemp = string.Empty;
                                    sbTemp = SetPrintPay(bPrint, bRePrint, false, false, basketHeader, basketBase, bp, dsMsg, dtPromotion, dtDcc, cancType, CardPrint.Keep, bCash);
                                    sb.Append(!bPrint ? sbTemp : "");
                                    // 여전법 추가 0808(KSK)
                                    sbTemp = string.Empty;
                                }
                            }
                        }
                    }

                    #endregion
                }

                if (cardPrint != CardPrint.Basic || cancType != CancelPrint.Cancel || !bOSale || bCash)
                {
                    #region 결제관련 출력물 기본(로고, 광고메세지, 브랜드광고 메세지, 판매기본 타이틀, 해당UI 타이틀, 판매상품, 합계)
                    sb.Append(PayBaseStart(bPrint, bRePrint, basketHeader, basketBase, dsMsg, cancType, cardPrint, bOnSalePrint, bExchangePrint, bCash));
                    #endregion

                    #region 결제 금액

                    #region 상품교환권 반품시 상품교환권 교환권

                    if ((cardPrint == CardPrint.Basic || cardPrint == CardPrint.None) && bOSale)
                    {
                        foreach (var item in basketBase)
                        {
                            if (item.GetType().Name.ToString().Equals("BasketExchange"))
                            {
                                #region 상품교환권

                                BasketExchange bp = (BasketExchange)item;

                                if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                                {
                                    iTempPay = (bp.PayAmt != null ? TypeHelper.ToInt32(bp.PayAmt) : 0);
                                    iTempBal = (bp.BalAmt != null ? TypeHelper.ToInt32(bp.BalAmt) : 0);

                                    if (iTempPay > 0)
                                    {
                                        iExchangeAmt += iTempPay;
                                        iExchangeBal += iTempBal;
                                    }
                                }

                                #endregion
                            }
                            //else if (item.GetType().Name.ToString().Equals("BasketOldExGift"))
                            //{
                            //    #region 구상품교환권

                            //    BasketOldExGift bp = (BasketOldExGift)item;

                            //    if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                            //    {
                            //        iTempPay = (bp.PayAmt != null ? TypeHelper.ToInt32(bp.PayAmt) : 0);
                            //        iTempBal = (bp.BalAmt != null ? TypeHelper.ToInt32(bp.BalAmt) : 0);

                            //        if (iTempPay > 0)
                            //        {
                            //            iExchangeAmt += iTempPay;
                            //            iExchangeBal += iTempBal;
                            //        }
                            //    }

                            //    #endregion
                            //}                     

                        }

                        if (bExchangePrint && (iExchangeBal > 0 || iExchangeAmt > 0))
                        {
                            sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                            sb.Append(FXConsts.RECEIPT_PRINT_EXCHANGE_RETURN_05);
                            sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, string.Format("{0:#,##0}", iExchangeAmt), 24));
                            sb.Append(Environment.NewLine);
                        }
                    }

                    #endregion

                    if (!bExchangePrint)
                    {
                        foreach (var item in basketBase)
                        {
                            if (item.GetType().Name.ToString().Equals("BasketPayCash"))
                            {
                                #region 현금

                                BasketPayCash bp = (BasketPayCash)item;

                                if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                                {
                                    iTempPay = (bp.PayAmt != null ? TypeHelper.ToInt32(bp.PayAmt) : 0);
                                    iTempBal = (bp.BalAmt != null ? TypeHelper.ToInt32(bp.BalAmt) : 0);

                                    if (iTempPay > 0)
                                    {
                                        if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_SPECIAL && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_ONLINE)
                                        {
                                            #region 온라인
                                            sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                                            sb.Append(FXConsts.RECEIPT_PRINT_NM_11);
                                            sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, string.Format("{0:#,##0}", bCancType ? -iTempPay : iTempPay), bPrint ? 24 : 32));
                                            sb.Append(Environment.NewLine);

                                            iPayAmt += iTempPay;
                                            iBalAmt += iTempBal;
                                            #endregion
                                        }
                                        else
                                        {
                                            //현금    
                                            sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                                            sb.Append(FXConsts.RECEIPT_PRINT_DESC_08);

                                            if (bOnSalePrint)
                                            {
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, string.Format("{0:#,##0}", iTempPay - iTempBal), bPrint ? 24 : 32));
                                            }
                                            else
                                            {
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, string.Format("{0:#,##0}", bCancType ? -iTempPay : iTempPay), bPrint ? 24 : 32));
                                            }

                                            sb.Append(Environment.NewLine);

                                            iPayAmt += iTempPay;
                                            iBalAmt += iTempBal;
                                        }

                                    }
                                }
                                #endregion
                            }
                            else if (item.GetType().Name.ToString().Equals("BasketPayCard"))
                            {
                                #region 신용카드

                                BasketPayCard bp = (BasketPayCard)item;

                                if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                                {
                                    iTempPay = (bp.PayAmt != null ? TypeHelper.ToInt32(bp.PayAmt) : 0);
                                    iTempBal = (bp.BalAmt != null ? TypeHelper.ToInt32(bp.BalAmt) : 0);

                                    if (iTempPay > 0)
                                    {
                                        if (cardPrint == CardPrint.Basic || cardPrint == CardPrint.None || bpCard == null || (bp.CardNo != bpCard.CardNo && bp.ApprNo != bpCard.ApprNo))
                                        {
                                            //신용카드    
                                            sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                                            sb.Append(FXConsts.RECEIPT_PRINT_NM_01);
                                            sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, string.Format("{0:#,##0}", bCancType ? -iTempPay : iTempPay), bPrint ? 24 : 32));
                                            sb.Append(Environment.NewLine);

                                            if (!bOnSalePrint)
                                            {
                                                //카드번호
                                                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                                                // 여전법 변경 주석처리
                                                // 46588*******3960 [정상승인]A HANUR  → 465887******3960 로 표시 해야 함
                                                //tempVal = string.Format("  {0}{1}{2}", bp.CardNo.Substring(0, 5), "****", bp.CardNo.Substring(9, bp.CardNo.Length == 19 ? 10 : bp.CardNo.Length - 9));
                                                //tempVal = string.Format("  {0}{1}{2}", bp.CardNo.Substring(0, 6), "******", bp.CardNo.Substring(12, bp.CardNo.Length == 19 ? 13 : bp.CardNo.Length - 12));
                                                //KSK_20170403
                                                if ("Y".Equals(bp.EunCardFg))
                                                {
                                                    tempVal = string.Format("  {0}{1}{2}", bp.CardNo.Substring(0, 6), "@@@@@@", bp.CardNo.Substring(12, bp.CardNo.Length - 12));
                                                    sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, tempVal, bp.CardNo.Length == 19 ? 22 : 18));
                                                }
                                                else
                                                {
                                                    tempVal = string.Format("  {0}{1}{2}", bp.CardNo.Substring(0, 6), "******", bp.CardNo.Substring(12, bp.CardNo.Length - 12));
                                                    sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, tempVal, bp.CardNo.Length == 19 ? 22 : 18));
                                                }

                                                // 여전법 추가 0808(KSK)
                                                tempVal = string.Empty;

                                                //승인구분([정상승인])
                                                tempVal = bp.ApprState != null && bp.ApprState.Length > 0 ? (bp.ApprState == "1" ? FXConsts.RECEIPT_PRINT_CARD_01 : FXConsts.RECEIPT_PRINT_CARD_02) : "";
                                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, tempVal, 11));
                                                // 여전법 추가 0808(KSK)
                                                tempVal = string.Empty;

                                                //카드번호입력구분(A=카드 read, @=카드번호 수기입력)
                                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, bp.InputType != null && bp.InputType.Length > 0 ? bp.InputType : "", 2));

                                                //벤사ID
                                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, bp.VanId != null && bp.VanId.Length > 0 ? bp.VanId : "", bp.CardNo.Length == 19 ? 5 : 9));
                                                sb.Append(Environment.NewLine);

                                                //카드회사명
                                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, string.Format("  {0}", bp.CardNm != null && bp.CardNm.Length > 0 ? bp.CardNm : ""), 18));

                                                //유효기간(**/**)
                                                sb.Append(FXConsts.RECEIPT_PRINT_CARD_03);
                                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, bp.ExpMY != null && bp.ExpMY.Length >= 4 ? "**/**" : "", 9));
                                                sb.Append(Environment.NewLine);

                                                #region 신용카드 정상승인일 경우 출력

                                                //매입사명
                                                sb.Append(FXConsts.RECEIPT_PRINT_CARD_04);
                                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, bp.MaeipComNm != null && bp.MaeipComNm.Length > 0 ? bp.MaeipComNm : "", 27));
                                                sb.Append(Environment.NewLine);

                                                if (bp.ApprState == "1")
                                                {
                                                    //가맹점번호
                                                    sb.Append(FXConsts.RECEIPT_PRINT_CARD_05);

                                                    // sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, bp.MerchantCode != null && bp.MerchantCode.Length > 0 ? bp.MerchantCode : "", 27));
                                                    // 여전법 변경 06.02
                                                    sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, bp.MerchantCode != null && bp.MerchantCode.Length > 0 ? bp.MerchantCode : "", 13));
                                                    
                                                    // 여전법 추가 06.02
                                                    // if (!string.IsNullOrEmpty(bp.PaidCardBalance) && bp.PaidCardBalance.Length > 0)
                                                    if (!string.IsNullOrEmpty(bp.PaidCardBalance) && TypeHelper.ToInt32(bp.PaidCardBalance) > 0)
                                                    {
                                                        // 잔액:
                                                        sb.Append(FXConsts.RECEIPT_PRINT_CARD_15);

                                                        int paidAmtBalance = TypeHelper.ToInt32(bp.PaidCardBalance);
                                                        sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, paidAmtBalance > 0 ? 
                                                            string.Format("{0:#,##0}", paidAmtBalance) : string.Empty, 9));
                                                    }
                                                    sb.Append(Environment.NewLine);
                                                }

                                                #endregion

                                                //승인번호
                                                tempVal = bp.ApprNo != null && bp.ApprNo.Length > 0 ? bp.ApprNo : "";
                                                sb.Append(FXConsts.RECEIPT_PRINT_CARD_06);
                                                if (tempVal.Replace(" ", "").Length <= 0)
                                                {
                                                    sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, FXConsts.RECEIPT_PRINT_CARD_14, 8));
                                                }
                                                else
                                                {
                                                    sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, tempVal, 8));
                                                }
                                                // 여전법 추가 0808(KSK)
                                                tempVal = string.Empty;

                                                //할부개월(일시불00)
                                                tempVal = bp.Halbu != null && bp.Halbu.Length > 0 ? (bp.Halbu.Length == 1 ? "0" + bp.Halbu : bp.Halbu) : "00";
                                                sb.Append(FXConsts.RECEIPT_PRINT_CARD_07);
                                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, tempVal, 6));
                                                // 여전법 추가 0808(KSK)
                                                tempVal = string.Empty;
                                                sb.Append(Environment.NewLine);


                                                if (TypeHelper.ToString(bp.DCCCheckNo).Trim().Length > 0 || TypeHelper.ToString(bp.DCCCurNo).Trim().Length > 0)
                                                {
                                                    bool isVisaCard = false;
                                                    string localCode = string.Empty;
                                                    string localAmount = string.Empty;
                                                    string homeCode = string.Empty;
                                                    string homeAmount = string.Empty;
                                                    string exchangeRate = string.Empty;
                                                    string markupPercent = string.Empty;
                                                    bool selectKRW = false;

                                                    selectKRW = "410".Equals(bp.DCCCurNo);

                                                    // 기준통화금액
                                                    localCode = "KRW";
                                                    localAmount = string.Format("{0:#,##0}", TypeHelper.ToInt64(bp.PayAmt));

                                                    // 자국통화금액
                                                    homeCode = bp.DCCCurCode;
                                                    homeAmount = MakeDecimalPoint(bp.DCCCurAmt, bp.DCCCurDecPoint);

                                                    // 역환율
                                                    string revsRate = MakeDecimalPoint(bp.DCCRvExRate, bp.DCCRvExRateDecPoint);
                                                    double unitAmt = Math.Pow(10, TypeHelper.ToInt32(bp.DCCRvExRateUnt));
                                                    exchangeRate = string.Format("{0} {1} = KRW {2}", bp.DCCCurCode, unitAmt, revsRate);

                                                    // Markup

                                                    double mk = TypeHelper.ToDouble(bp.MarkupPerc);
                                                    isVisaCard = "03".Equals(bp.ForeignCardFg);
                                                    markupPercent = mk.ToString("N" + TypeHelper.ToInt32(bp.MarkupPercUnt, 1).ToString()) + "%";


                                                    //DCC출력물
                                                    sb.Append(SetPrintDcc(true, bCancType, isVisaCard, selectKRW, bp.ForeignCardFg, localCode, localAmount, homeCode, homeAmount, exchangeRate, markupPercent, bp.NatCurNo, bp.NatCurCode, dtDcc));
                                                }
                                            }
                                        }

                                        iPayAmt += iTempPay;
                                        iBalAmt += iTempBal;
                                    }
                                }

                                #endregion
                            }
                            else if (item.GetType().Name.ToString().Equals("BasketExchange"))
                            {
                                #region 상품교환권

                                BasketExchange bp = (BasketExchange)item;

                                if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                                {
                                    iTempPay = (bp.PayAmt != null ? TypeHelper.ToInt32(bp.PayAmt) : 0);
                                    iTempBal = (bp.BalAmt != null ? TypeHelper.ToInt32(bp.BalAmt) : 0);

                                    if (iTempPay > 0)
                                    {
                                        //상품교환권    
                                        sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                                        sb.Append(FXConsts.RECEIPT_PRINT_NM_02);
                                        sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, string.Format("{0:#,##0}", bCancType ? -iTempPay : iTempPay), bPrint ? 24 : 32));
                                        sb.Append(Environment.NewLine);

                                        if (!bCancType || (basketHeader.OTSaleDate != null && basketHeader.OTSaleDate.Length >= 0))
                                        {
                                            //자동반품 또는 판매일경우만 상품권번호 출력
                                            //상품교환권 번호
                                            sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                                            sb.Append(string.Format("  {0}", bp.ExchangeNo != null && bp.ExchangeNo.Length > 0 ? bp.ExchangeNo : ""));
                                            sb.Append(Environment.NewLine);
                                        }

                                        iPayAmt += iTempPay;
                                        iBalAmt += iTempBal;
                                    }
                                }

                                #endregion
                            }
                            else if (item.GetType().Name.ToString().Equals("BasketOtherTicket"))
                            {
                                #region 타사상품권

                                BasketOtherTicket bp = (BasketOtherTicket)item;

                                if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                                {
                                    iTempPay = (bp.PayAmt != null ? TypeHelper.ToInt32(bp.PayAmt) : 0);
                                    iTempBal = (bp.BalAmt != null ? TypeHelper.ToInt32(bp.BalAmt) : 0);

                                    if (iTempPay > 0)
                                    {
                                        //타사상품권    
                                        sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                                        sb.Append(FXConsts.RECEIPT_PRINT_NM_03);
                                        sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, string.Format("{0:#,##0}", bCancType ? -iTempPay : iTempPay), bPrint ? 24 : 32));
                                        sb.Append(Environment.NewLine);

                                        //타사상품권코드
                                        sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                                        tempVal = string.Format("  {0}", bp.OtherTicketType != null && bp.OtherTicketType.Length > 0 ? bp.OtherTicketType : "");
                                        sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, tempVal, 4));

                                        //타사상품권이름
                                        int iTemp = 18 - tempVal.Length;
                                        sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, string.Format("[{0}]", bp.OtherTicketNm != null && bp.OtherTicketNm.Length > 0 ? bp.OtherTicketNm : ""), (iTemp + 2)));

                                        //타사상품권번호
                                        sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, string.Format("  {0}", bp.OtherTicketNo != null && bp.OtherTicketNo.Length > 0 ? bp.OtherTicketNo : ""), 20));
                                        sb.Append(Environment.NewLine);

                                        iPayAmt += iTempPay;
                                        iBalAmt += iTempBal;
                                    }
                                }

                                #endregion
                            }
                            else if (item.GetType().Name.ToString().Equals("BasketPay"))
                            {
                                #region 타건복지, 결제할인, 타건카드

                                BasketPay bp = (BasketPay)item;

                                if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                                {
                                    iTempPay = (bp.PayAmt != null ? TypeHelper.ToInt32(bp.PayAmt) : 0);
                                    iTempBal = (bp.BalAmt != null ? TypeHelper.ToInt32(bp.BalAmt) : 0);

                                    if (iTempPay > 0)
                                    {
                                        if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CARD_WELFARE)
                                        {
                                            #region 타건복지
                                            sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                                            sb.Append(FXConsts.RECEIPT_PRINT_NM_04);
                                            sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, string.Format("{0:#,##0}", bCancType ? -iTempPay : iTempPay), bPrint ? 24 : 32));
                                            sb.Append(Environment.NewLine);

                                            iPayAmt += iTempPay;
                                            iBalAmt += iTempBal;
                                            #endregion
                                        }
                                        else if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_COUPON && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_DISCOUNT)
                                        {
                                            #region 결제할인
                                            sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                                            sb.Append(FXConsts.RECEIPT_PRINT_NM_08);
                                            sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, string.Format("{0:#,##0}", bCancType ? -iTempPay : iTempPay), bPrint ? 24 : 32));
                                            sb.Append(Environment.NewLine);

                                            iPayAmt += iTempPay;
                                            iBalAmt += iTempBal;
                                            #endregion
                                        }
                                        else if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CARD_OTHER)
                                        {
                                            #region 타건카드
                                            sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                                            sb.Append(FXConsts.RECEIPT_PRINT_NM_09);
                                            sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, string.Format("{0:#,##0}", bCancType ? -iTempPay : iTempPay), bPrint ? 24 : 32));
                                            sb.Append(Environment.NewLine);

                                            iPayAmt += iTempPay;
                                            iBalAmt += iTempBal;
                                            #endregion
                                        }
                                        else if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_SPECIAL && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_ONLINE)
                                        {
                                            #region 온라인
                                            sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                                            sb.Append(FXConsts.RECEIPT_PRINT_NM_11);
                                            sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, string.Format("{0:#,##0}", bCancType ? -iTempPay : iTempPay), bPrint ? 24 : 32));
                                            sb.Append(Environment.NewLine);

                                            iPayAmt += iTempPay;
                                            iBalAmt += iTempBal;
                                            #endregion
                                        }
                                    }
                                }

                                #endregion
                            }
                            else if (item.GetType().Name.ToString().Equals("BasketCashIC"))
                            {
                                #region 현금IC

                                BasketCashIC bp = (BasketCashIC)item;

                                if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                                {
                                    iTempPay = (bp.PayAmt != null ? TypeHelper.ToInt32(bp.PayAmt) : 0);
                                    iTempBal = (bp.BalAmt != null ? TypeHelper.ToInt32(bp.BalAmt) : 0);

                                    if (iTempPay > 0)
                                    {
                                        //현금IC    
                                        sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                                        sb.Append(FXConsts.RECEIPT_PRINT_NM_05);
                                        sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, string.Format("{0:#,##0}", bCancType ? -iTempPay : iTempPay), bPrint ? 24 : 32));
                                        sb.Append(Environment.NewLine);

                                        //은행명
                                        sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                                        sb.Append(FXConsts.RECEIPT_PRINT_CASH_IC_01);
                                        sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, bp.IssueComNm != null && bp.IssueComNm.Length > 0 ? bp.IssueComNm : "", 29));
                                        sb.Append(Environment.NewLine);

                                        //계좌번호
                                        sb.Append(FXConsts.RECEIPT_PRINT_CASH_IC_02);
                                        sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, bp.CashICAccountNo != null && bp.CashICAccountNo.Length > 0 ? bp.CashICAccountNo : "", 29));
                                        sb.Append(Environment.NewLine);

                                        //승인번호
                                        tempVal = bp.ApprNo != null && bp.ApprNo.Length > 0 ? bp.ApprNo : "";
                                        sb.Append(FXConsts.RECEIPT_PRINT_CASH_IC_03);
                                        if (tempVal.Replace(" ", "").Length <= 0)
                                        {
                                            sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, FXConsts.RECEIPT_PRINT_CARD_14, 29));
                                        }
                                        else
                                        {
                                            sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, tempVal, 29));
                                        }
                                        sb.Append(Environment.NewLine);

                                        iPayAmt += iTempPay;
                                        iBalAmt += iTempBal;
                                    }
                                }

                                #endregion
                            }
                            else if (item.GetType().Name.ToString().Equals("BasketPoint"))
                            {
                                #region 포인트

                                BasketPoint bp = (BasketPoint)item;

                                if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                                {
                                    iTempPay = (bp.PayAmt != null ? TypeHelper.ToInt32(bp.PayAmt) : 0);
                                    iTempBal = (bp.BalAmt != null ? TypeHelper.ToInt32(bp.BalAmt) : 0);

                                    if (iTempPay > 0)
                                    {
                                        //포인트    
                                        sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                                        sb.Append(FXConsts.RECEIPT_PRINT_NM_06);
                                        sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, string.Format("{0:#,##0}", bCancType ? -iTempPay : iTempPay), bPrint ? 24 : 32));
                                        sb.Append(Environment.NewLine);

                                        //카드번호
                                        sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                                        sb.Append(FXConsts.RECEIPT_PRINT_POINT_01);
                                        sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, bp.CardNo != null && bp.CardNo.Length > 0 ? bp.CardNo : "", 28));
                                        sb.Append(Environment.NewLine);

                                        //회원명
                                        sb.Append(FXConsts.RECEIPT_PRINT_POINT_02);
                                        sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, bp.CustNm != null && bp.CustNm.Length > 0 ? bp.CustNm : "", 28));
                                        sb.Append(Environment.NewLine);

                                        //승인번호
                                        tempVal = bp.ApprovalNo != null && bp.ApprovalNo.Length > 0 ? bp.ApprovalNo : "";
                                        sb.Append(FXConsts.RECEIPT_PRINT_POINT_03);
                                        if (tempVal.Replace(" ", "").Length <= 0)
                                        {
                                            sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, FXConsts.RECEIPT_PRINT_CARD_14, 28));
                                        }
                                        else
                                        {
                                            sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, tempVal, 28));

                                        }
                                        sb.Append(Environment.NewLine);

                                        if (tempVal.Replace(" ", "").Length > 0)
                                        {
                                            //가용포인트
                                            sb.Append(FXConsts.RECEIPT_PRINT_POINT_04);
                                            sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, string.Format("{0:#,##0}점", bp.BalanceAmt != null && bp.BalanceAmt.Length > 0 ? TypeHelper.ToInt32(bp.BalanceAmt) : 0), 28));
                                            sb.Append(Environment.NewLine);
                                        }

                                        iPayAmt += iTempPay;
                                        iBalAmt += iTempBal;
                                    }
                                }

                                #endregion
                            }
                            else if (item.GetType().Name.ToString().Equals("BasketCoupon"))
                            {
                                #region 쿠폰

                                BasketCoupon bp = (BasketCoupon)item;

                                if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                                {
                                    iTempPay = (bp.PayAmt != null ? TypeHelper.ToInt32(bp.PayAmt) : 0);
                                    iTempBal = (bp.BalAmt != null ? TypeHelper.ToInt32(bp.BalAmt) : 0);

                                    if (iTempPay > 0)
                                    {
                                        //할인쿠폰    
                                        sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                                        sb.Append(FXConsts.RECEIPT_PRINT_NM_07);
                                        sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, string.Format("{0:#,##0}", bCancType ? -iTempPay : iTempPay), bPrint ? 24 : 32));
                                        sb.Append(Environment.NewLine);

                                        //할인쿠폰코드
                                        sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                                        tempVal = string.Format("  {0}", bp.CouponCd != null && bp.CouponCd.Length > 0 ? bp.CouponCd : "");
                                        sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, tempVal, tempVal.Length));

                                        //할인쿠폰이름
                                        sb.Append(string.Format("[{0}]", getFixCut(bPrint, true, PrintTypes.WideNormal, string.Format("{0}", bp.CouponNm != null && bp.CouponNm.Length > 0 ? bp.CouponNm : ""), 34 - (tempVal.Length + 2))));

                                        //할인쿠폰매수
                                        sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format(" {0}매", bp.CouponCnt != null && bp.CouponCnt.Length > 0 ? bp.CouponCnt : ""), 6));
                                        sb.Append(Environment.NewLine);

                                        iPayAmt += iTempPay;
                                        iBalAmt += iTempBal;
                                    }
                                }

                                #endregion
                            }
                            else if (item.GetType().Name.ToString().Equals("BasketOldExGift"))
                            {
                                #region 구상품교환권

                                BasketOldExGift bp = (BasketOldExGift)item;

                                if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                                {
                                    iTempPay = (bp.PayAmt != null ? TypeHelper.ToInt32(bp.PayAmt) : 0);
                                    iTempBal = (bp.BalAmt != null ? TypeHelper.ToInt32(bp.BalAmt) : 0);

                                    if (iTempPay > 0)
                                    {
                                        sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                                        sb.Append(FXConsts.RECEIPT_PRINT_NM_10);
                                        sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, string.Format("{0:#,##0}", bCancType ? -iTempPay : iTempPay), 20));
                                        sb.Append(Environment.NewLine);

                                        iPayAmt += iTempPay;
                                        iBalAmt += iTempBal;
                                    }
                                }

                                #endregion
                            }
                        }

                        #region 수기서명 신용카드
                        if ((cardPrint == CardPrint.Charge || cardPrint == CardPrint.Keep) && bpCard != null && bpCard.CardNo.Length > 0)
                        {
                            // 여전법 추가 0808(KSK)
                            string printString = string.Empty;
                            printString = PrintCardPrint(bPrint, bpCard, bCancType);
                            sb.Append(printString);
                            printString = string.Empty;
                        }
                        #endregion

                        #region 받은돈, 거스름돈

                        // BY KHJ 2016.04.11
                        //if (iBalAmt > 0 && !bCancType && cardPrint == CardPrint.Basic)
                        //
                        if (iBalAmt > 0 && (cardPrint == CardPrint.Basic || cardPrint == CardPrint.None))
                        {
                            //받은돈(DOUBLE WIDE)
                            sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                            sb.Append(FXConsts.RECEIPT_PRINT_DESC_09);
                            sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, string.Format("{0:#,##0}", ((bCancType) ? -1 : 1) * iPayAmt), bPrint ? 24 : 32));
                            sb.Append(Environment.NewLine);

                            //거스름(DOUBLE WIDE)
                            sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                            sb.Append(FXConsts.RECEIPT_PRINT_DESC_10);
                            sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, string.Format("{0:#,##0}", ((bCancType) ? -1 : 1) * iBalAmt), bPrint ? 24 : 32));
                            sb.Append(Environment.NewLine);
                        }

                        #endregion
                    }

                    if (cardPrint == CardPrint.Basic)
                    {
                        if (cancType == CancelPrint.Cancel && bOSale)
                        {
                            #region 반품 고객기재사항

                            //고객기재사항
                            sb.Append(Environment.NewLine);
                            sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                            sb.Append(FXConsts.RECEIPT_PRINT_CUSTWRITE_01);
                            sb.Append(Environment.NewLine);

                            //고객기재사항(성명)
                            sb.Append(FXConsts.RECEIPT_PRINT_CUSTWRITE_02);
                            sb.Append(Environment.NewLine);

                            //고객기재사항(주소)
                            sb.Append(FXConsts.RECEIPT_PRINT_CUSTWRITE_03);
                            sb.Append(Environment.NewLine);

                            //고객기재사항(전화번호)
                            sb.Append(FXConsts.RECEIPT_PRINT_CUSTWRITE_04);
                            sb.Append(Environment.NewLine);

                            //고객기재사항(취소사유)
                            sb.Append(FXConsts.RECEIPT_PRINT_CUSTWRITE_05);
                            sb.Append(Environment.NewLine);

                            //고객기재사항(고객서명)
                            sb.Append(FXConsts.RECEIPT_PRINT_CUSTWRITE_06);
                            sb.Append(Environment.NewLine);
                            sb.Append(FXConsts.PRINT_ASTARFEED);
                            sb.Append(Environment.NewLine);

                            //브랜드명
                            sb.Append(FXConsts.RECEIPT_PRINT_CUSTWRITE_07);
                            sb.Append(Environment.NewLine);

                            //인수자
                            sb.Append(FXConsts.RECEIPT_PRINT_CUSTWRITE_08);
                            sb.Append(Environment.NewLine);

                            //책임자 확인(성명기재 및 날인)
                            sb.Append(FXConsts.RECEIPT_PRINT_CUSTWRITE_09);
                            sb.Append(Environment.NewLine);
                            sb.Append(FXConsts.PRINT_ASTARFEED);
                            sb.Append(Environment.NewLine);
                            #endregion
                        }

                        if ((cancType != CancelPrint.Cancel || !bOSale))
                        {
                            if (!bOnSalePrint)
                            {
                                //결제관련 출력물 기본
                                sb.Append(PayBaseEnd(bPrint, basketHeader, basketBase, dsMsg, bCancType));
                            }
                            else
                            {
                                sb.Append(FXConsts.PRINT_ASTARFEED);
                                sb.Append(Environment.NewLine);
                            }
                        }
                    }
                    else if (cardPrint == CardPrint.Charge)
                    {
                        //sb.Append(PrintCardPrint(bPrint, null, false));

                        // 여전법 추가 0808(KSK)
                        string printString = string.Empty;
                        printString = PrintCardPrint(bPrint, null, false);
                        sb.Append(printString);
                        printString = string.Empty;

                    }

                    if (cardPrint == CardPrint.None && bOSale && !bExchangePrint)
                    {
                        //반품등록 끝
                        sb.Append(PayBaseEnd(bPrint, basketHeader, basketBase, dsMsg, bCancType));
                    }


                    if (cardPrint == CardPrint.None && bOSale && bExchangePrint)
                    {
                        //상품교환권 교환권
                        sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                        sb.Append(Environment.NewLine);
                        sb.Append(FXConsts.RECEIPT_PRINT_EXCHANGE_RETURN_06);
                        sb.Append(Environment.NewLine);
                        sb.Append(FXConsts.RECEIPT_PRINT_EXCHANGE_RETURN_07);
                        sb.Append(Environment.NewLine);
                        sb.Append(Environment.NewLine);
                        sb.Append(FXConsts.RECEIPT_PRINT_EXCHANGE_RETURN_08);
                        sb.Append(Environment.NewLine);
                        sb.Append(FXConsts.RECEIPT_PRINT_EXCHANGE_RETURN_09);
                        sb.Append(Environment.NewLine);
                        sb.Append(FXConsts.RECEIPT_PRINT_EXCHANGE_RETURN_10);
                        sb.Append(Environment.NewLine);
                        sb.Append(FXConsts.RECEIPT_PRINT_EXCHANGE_RETURN_11);
                        sb.Append(Environment.NewLine);
                        sb.Append(Environment.NewLine);
                        sb.Append(FXConsts.PRINT_ASTARFEED);
                        sb.Append(Environment.NewLine);
                    }

                    #region 무효영수증
                    sb.Append(bPrint && bRePrint ? RePrint() : "");
                    #endregion

                    #endregion
                }

                if (bPrint && sb.ToString().Length > 0)
                {
                    #region 저널 입력용 명칭

                    if (!bOSale)
                    {
                        if (!bCancType)
                        {
                            strUINm = bOnSalePrint ? FXConsts.RECEIPT_NAME_PAY_BAG_02 : FXConsts.RECEIPT_NAME_PAY_BAG_01;
                        }
                        else
                        {
                            strUINm = FXConsts.RECEIPT_NAME_PAY_BAG_RETURN;
                        }
                    }
                    else
                    {
                        if (cancType == CancelPrint.Cancel)
                        {
                            if ((iExchangeAmt > 0 || iExchangeBal > 0) && bExchangePrint)
                            {
                                strUINm = FXConsts.RECEIPT_NAME_SALEITEM_RETURN_03;
                            }
                            else
                            {
                                strUINm = FXConsts.RECEIPT_NAME_SALEITEM_RETURN_01;
                            }
                        }
                        else if (cancType == CancelPrint.ReCancel)
                        {
                            strUINm = FXConsts.RECEIPT_NAME_SALEITEM_RETURN_02;
                        }
                        else
                        {
                            strUINm = FXConsts.RECEIPT_NAME_SALEITEM;
                        }
                    }

                    #endregion

                    #region 실제 출력

                    // Loc changed 11.13
                    // 바코드는 SaleDate로 출력한다 (발생일자 아님 - 원래 발생일자 OccurDate)
                    string printString = sb.ToString();
                    Print(bPrint, strUINm, false, printString, bOSale ? string.Format("A{0}{1}{2}{3}A", basketHeader.StoreNo,
                        basketHeader.SaleDate.Substring(2, 6), basketHeader.PosNo, basketHeader.TrxnNo.Substring(2, 4)) : "", true);
                    printString = string.Empty;
                    #endregion
                }

                #region 저장물 판매의 경우 저장물 교환권 출력

                if (cardPrint == CardPrint.Basic && !bOSale && !bCancType && !bOnSalePrint)
                {
                    sbTemp = string.Empty;
                    sbTemp = SetPrintPay(bPrint, bRePrint, true, false, basketHeader, basketBase, null, dsMsg, dtPromotion, dtDcc, cancType, CardPrint.Keep, bCash);
                    sb.Append(sbTemp);
                    sbTemp = string.Empty;
                }

                #endregion

                #region 프로모션 출력

                if (bPrint && cardPrint == CardPrint.Basic && !bCancType && bOSale)
                {
                    var items_BasketPointSave = basketBase.Where(p => p.BasketType.Equals(BasketTypes.BasketPointSave)).Cast<BasketPointSave>().ToArray();  //포인트적립정보

                    if (items_BasketPointSave != null && items_BasketPointSave.Length > 0)
                    {
                        PayPromotion(basketHeader, items_BasketPointSave[0], dtPromotion);
                    }
                    else
                    {
                        PayPromotion(basketHeader, null, dtPromotion);
                    }
                }

                #endregion

                //반품일경우 매장용 전표 출력후 고객용 반품영수증 출력
                if (cardPrint == CardPrint.Basic && cancType == CancelPrint.Cancel && bOSale)
                {
                    if (!bExchangePrint)
                    {
                        cancType = CancelPrint.ReCancel;
                        cardPrint = CardPrint.None;
                        sbTemp = string.Empty;
                        sbTemp = SetPrintPay(bPrint, bRePrint, bOnSalePrint, false, basketHeader, basketBase, null, dsMsg, dtPromotion, dtDcc, cancType, cardPrint, bCash);
                        sb.Append(sbTemp);
                        sbTemp = string.Empty;
                    }
                }
                else if (cardPrint == CardPrint.None && cancType == CancelPrint.ReCancel && bOSale)
                {
                    if ((iExchangeAmt > 0 || iExchangeBal > 0) && !bExchangePrint)
                    {
                        cancType = CancelPrint.Cancel;
                        cardPrint = CardPrint.None;
                        sbTemp = string.Empty;
                        sbTemp = SetPrintPay(bPrint, bRePrint, false, true, basketHeader, basketBase, null, dsMsg, dtPromotion, dtDcc, cancType, cardPrint, bCash);
                        sb.Append(sbTemp);
                        sbTemp = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }

            // 여전법 추가 0808(KSK)
            string returnString = string.Empty;
            returnString = sb.ToString();

            sb.Remove(0, sb.Length);
            sb.Length = 0;

            sb = new StringBuilder();
            
            return returnString;
        }

        #endregion

        #region 반품관련 출력물 정보

        /// <summary>
        /// 반품관련 출력물 정보
        /// </summary>
        public enum CancelPrint
        {
            Normal = 1,     //정상판매
            Cancel = 2,     //반품판매
            ReCancel = 3    //반품판매
        }

        #endregion

        #region 결제관련 출력물 기본 시작

        /// <summary>
        /// 결제관련 출력물 기본 시작
        /// </summary>
        /// <param name="bPrint">true :  프린트 출력, false : 화면 출력</param>
        /// <param name="bRePrint">재발행여부(true:재발행, false:발행)</param>
        /// <param name="basketHeader">헤더정보</param>
        /// <param name="basket">판매상품정보, 상품소계정보</param>
        /// <param name="dsMsg">W-Mall 광고 메세지, 브랜드 광고 메세지, 안내 메세지 자료</param>
        /// <param name="strCancType">취소구분</param>
        /// <param name="iCardPrint">수기서명 여부(0:서명, 1:서명X(청구용), 2:서명X(보관용)</param>
        /// <param name="bOnSalePrint"></param>
        /// <param name="bExchangePrint"></param>
        /// <returns>결제관련 출력물 기본 정보 문자열</returns>
        private string PayBaseStart(bool bPrint, bool bRePrint, BasketHeader basketHeader, List<BasketBase> basketBase, DataSet dsMsg, CancelPrint cancType, CardPrint cardPrint, bool bOnSalePrint, bool bExchangePrint, bool bCash)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                string strTemp = string.Empty;                                                                                                      //출력 임시 문자열
                var items_BasketItem = basketBase.Where(p => p.BasketType.Equals(BasketTypes.BasketItem)).Cast<BasketItem>().ToArray();             //판매상품정보
                var items_BasketSubTotal = basketBase.Where(p => p.BasketType.Equals(BasketTypes.BasketSubTotal)).Cast<BasketSubTotal>().ToArray(); //상품소계정보
                bool bOSale = basketHeader.TrxnType != NetCommConstants.TRXN_TYPE_OTH_SALE ? true : false;                                          //저장물 판매여부(true:일반상품, false:저장물)
                bool bCancType = cancType != CancelPrint.Normal ? true : false;

                #region 로고이미지
                if (bPrint)
                {
                    sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                    sb.Append(m_device.PrintLogo());
                }
                #endregion

                if (cardPrint != CardPrint.Basic || cancType != CancelPrint.Cancel || !bOSale || bCash)
                {
                    #region 무효영수증
                    sb.Append(bPrint && bRePrint ? RePrint() : "");
                    #endregion
                }

                #region 점포명판 메세지

                if (dsMsg != null && dsMsg.Tables.Count > 0 && dsMsg.Tables[0] != null && dsMsg.Tables[0].Rows.Count > 0)
                {
                    sb.Append(PrintMessage(bPrint, dsMsg.Tables[0]));
                }

                #endregion

                #region 광고,브랜드광고 메세지
                if (bOSale && !bExchangePrint && cardPrint == CardPrint.Basic && !bCancType)
                {
                    #region 광고메세지
                    if (dsMsg != null && dsMsg.Tables.Count > 1 && dsMsg.Tables[1] != null && dsMsg.Tables[1].Rows.Count > 0)
                    {
                        sb.Append(PrintMessage(bPrint, dsMsg.Tables[1]));
                    }
                    #endregion

                    #region 브랜드광고메세지
                    if (dsMsg != null && dsMsg.Tables.Count > 3 && dsMsg.Tables[3] != null && dsMsg.Tables[3].Rows.Count > 0)
                    {
                        sb.Append(PrintMessage(bPrint, dsMsg.Tables[3]));
                    }
                    #endregion
                }
                #endregion

                #region 기본타이틀
                sb.Append(ReceiptBaseTitle(bPrint, basketHeader, string.Empty));
                #endregion

                #region 카드 수기서명시 청구용,보관용 영수증 타이틀
                if ((cardPrint == CardPrint.Charge || cardPrint == CardPrint.Keep) && bOSale)
                {
                    sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                    sb.Append(FXConsts.PRINT_ASTARFEED);
                    sb.Append(Environment.NewLine);
                    sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                    sb.Append(FXConsts.RECEIPT_PRINT_CARD_08_01);
                    sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                    sb.Append(bCancType ? FXConsts.RECEIPT_PRINT_RETURN_17 : FXConsts.RECEIPT_PRINT_CARD_08_02);
                    sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                    sb.Append(cardPrint == CardPrint.Charge ? FXConsts.RECEIPT_PRINT_CARD_09 : FXConsts.RECEIPT_PRINT_CARD_10);
                    sb.Append(Environment.NewLine);
                    sb.Append(FXConsts.PRINT_ASTARFEED);
                    sb.Append(Environment.NewLine);
                }
                #endregion

                //저장물일경우의 타이틀
                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                if (!bOSale)
                {
                    #region 저장물 타이틀

                    sb.Append(FXConsts.RECEIPT_PRINT_BAG_01);
                    sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                    sb.Append(cancType != CancelPrint.Normal ? FXConsts.RECEIPT_PRINT_BAG_04 : (bOnSalePrint ? FXConsts.RECEIPT_PRINT_BAG_03 : FXConsts.RECEIPT_PRINT_BAG_02));

                    sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                    sb.Append(FXConsts.RECEIPT_PRINT_BAG_05);
                    sb.Append(Environment.NewLine);

                    //제목
                    sb.Append(FXConsts.PRINT_DLINEFEED);
                    sb.Append(Environment.NewLine);
                    sb.Append(FXConsts.RECEIPT_PRINT_DESC_01);
                    sb.Append(Environment.NewLine);
                    sb.Append(FXConsts.PRINT_LINEFEED);
                    sb.Append(Environment.NewLine);

                    #endregion
                }
                else
                {
                    if (cancType == CancelPrint.Cancel)
                    {
                        if (bExchangePrint)
                        {
                            #region 상품교환권 교환권 타이틀

                            //반품일경우
                            sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                            sb.Append(FXConsts.PRINT_ASTARFEED);
                            sb.Append(Environment.NewLine);

                            sb.Append(FXConsts.RECEIPT_PRINT_EXCHANGE_RETURN_01);
                            sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                            sb.Append(FXConsts.RECEIPT_PRINT_EXCHANGE_RETURN_02);
                            sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                            sb.Append(FXConsts.RECEIPT_PRINT_EXCHANGE_RETURN_01);
                            sb.Append(Environment.NewLine);
                            sb.Append(FXConsts.PRINT_ASTARFEED);
                            sb.Append(Environment.NewLine);
                            sb.Append(Environment.NewLine);

                            //반품거래정보
                            sb.Append(FXConsts.RECEIPT_PRINT_EXCHANGE_RETURN_03);

                            if (basketHeader.OTSaleDate != null && basketHeader.OTSaleDate.Length > 0)
                            {
                                //자동반품
                                string strTempOT = basketHeader.OTSaleDate;
                                strTempOT += string.Format("-{0}", basketHeader.OTPosNo != null && basketHeader.OTPosNo.Length > 0 ? basketHeader.OTPosNo : "    ");
                                strTempOT += string.Format("-{0}", basketHeader.OTTrxnNo != null && basketHeader.OTTrxnNo.Length > 0 ? basketHeader.OTTrxnNo : "      ");
                                sb.Append(strTempOT);
                            }
                            else
                            {
                                //수동반품
                                sb.Append(FXConsts.RECEIPT_PRINT_EXCHANGE_RETURN_04);
                            }
                            sb.Append(Environment.NewLine);
                            sb.Append(Environment.NewLine);

                            #endregion
                        }
                        else
                        {
                            #region 반품전표(매장용) 타이틀

                            if (cardPrint == CardPrint.Basic || cardPrint == CardPrint.None)
                            {
                                //반품일경우
                                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                                sb.Append(FXConsts.PRINT_ASTARFEED);
                                sb.Append(Environment.NewLine);

                                //반품전표(매장용)
                                sb.Append(FXConsts.RECEIPT_PRINT_RETURN_01);
                                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                                sb.Append(FXConsts.RECEIPT_PRINT_RETURN_02);
                                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                                sb.Append(FXConsts.RECEIPT_PRINT_RETURN_03);
                                sb.Append(Environment.NewLine);

                                sb.Append(FXConsts.PRINT_ASTARFEED);
                                sb.Append(Environment.NewLine);

                                if (basketHeader.OTSaleDate != null && basketHeader.OTSaleDate.Length >= 8 &&
                                    basketHeader.OTPosNo != null && basketHeader.OTPosNo.Length >= 4 &&
                                    basketHeader.OTTrxnNo != null && basketHeader.OTTrxnNo.Length >= 4)
                                {
                                    //원거래정보
                                    sb.Append(FXConsts.RECEIPT_PRINT_RETURN_07);
                                    sb.Append(string.Format("{0}-{1}-{2}", basketHeader.OTSaleDate, basketHeader.OTPosNo, basketHeader.OTTrxnNo));
                                    sb.Append(Environment.NewLine);
                                }
                            }

                            //제목
                            sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                            sb.Append(FXConsts.PRINT_DLINEFEED);
                            sb.Append(Environment.NewLine);
                            sb.Append(FXConsts.RECEIPT_PRINT_DESC_01);
                            sb.Append(Environment.NewLine);
                            sb.Append(FXConsts.PRINT_LINEFEED);
                            sb.Append(Environment.NewLine);

                            #endregion
                        }
                    }
                    else
                    {
                        #region 반품영수증(고객용) 타이틀

                        sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                        if (cancType == CancelPrint.ReCancel)
                        {
                            //반품일경우
                            sb.Append(FXConsts.PRINT_ASTARFEED);
                            sb.Append(Environment.NewLine);

                            //반품등록
                            sb.Append(FXConsts.RECEIPT_PRINT_RETURN_04);
                            sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                            sb.Append(FXConsts.RECEIPT_PRINT_RETURN_05);
                            sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                            sb.Append(FXConsts.RECEIPT_PRINT_RETURN_06);
                            sb.Append(Environment.NewLine);

                            if (basketHeader.OTSaleDate != null && basketHeader.OTSaleDate.Length >= 8 &&
                                basketHeader.OTPosNo != null && basketHeader.OTPosNo.Length >= 4 &&
                                basketHeader.OTTrxnNo != null && basketHeader.OTTrxnNo.Length >= 4)
                            {
                                //원거래정보
                                sb.Append(FXConsts.RECEIPT_PRINT_RETURN_07);
                                sb.Append(string.Format("{0}-{1}-{2}", basketHeader.OTSaleDate, basketHeader.OTPosNo, basketHeader.OTTrxnNo));
                                sb.Append(Environment.NewLine);
                            }
                        }

                        #endregion

                        //제목
                        sb.Append(FXConsts.PRINT_DLINEFEED);
                        sb.Append(Environment.NewLine);
                        sb.Append(FXConsts.RECEIPT_PRINT_DESC_01);
                        sb.Append(Environment.NewLine);
                        sb.Append(FXConsts.PRINT_LINEFEED);
                        sb.Append(Environment.NewLine);
                    }
                }

                if (!bExchangePrint)
                {
                    #region 판매상품 내용
                    sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                    foreach (var item in items_BasketItem)
                    {
                        BasketItem bi = (BasketItem)item;
                        sb.Append(POSPrinterUtils.ReceiptSaleItem(
                          bi.NmClass
                        , bi.NmItem
                        , bi.CdClass
                        , bi.CdDp == "0" || bi.CdDp == "4" ? bi.InCdItem : bi.CdItem
                        , bi.FgMargin
                        , bi.CdDp
                        , bi.FgNewPrcApp == "1" ? true : false
                        , bi.FgTax
                        , bi.FgCanc
                        , bi.CntItem.Length > 0 ? (bCancType ? -TypeHelper.ToInt32(bi.CntItem) : TypeHelper.ToInt32(bi.CntItem)) : 0   //반품일경우 -입력
                        , bi.UtSprc.Length > 0 ? (bCancType ? -TypeHelper.ToInt32(bi.UtSprc) : TypeHelper.ToInt32(bi.UtSprc)) : 0      //반품일경우 -입력
                        , bi.AmSale.Length > 0 ? (bCancType ? -TypeHelper.ToInt32(bi.AmSale) : TypeHelper.ToInt32(bi.AmSale)) : 0));   //반품일경우 -입력
                    }

                    #endregion

                    #region 판매상품 비고내역

                    #region 정상판매의경우, 저장물판매의 정산판매 또는 반품일경우 출력내용

                    if (!bOnSalePrint)
                    {
                        //참고내용
                        sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                        sb.Append(Environment.NewLine);
                        sb.Append(FXConsts.RECEIPT_PRINT_DESC_02);
                        sb.Append(Environment.NewLine);

                        //면세물품가액
                        sb.Append(FXConsts.RECEIPT_PRINT_DESC_03);
                        strTemp = items_BasketSubTotal[0].AmNoTaxItem != null && items_BasketSubTotal[0].AmNoTaxItem != "" ? string.Format("{0:#,##0}", bCancType ? -TypeHelper.ToInt32(items_BasketSubTotal[0].AmNoTaxItem) : TypeHelper.ToInt32(items_BasketSubTotal[0].AmNoTaxItem)) : "0";
                        sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, strTemp, 17));
                        sb.Append(Environment.NewLine);

                        //과세물품가액
                        sb.Append(FXConsts.RECEIPT_PRINT_DESC_04);
                        strTemp = items_BasketSubTotal[0].AmTaxItem != null && items_BasketSubTotal[0].AmTaxItem != "" ? string.Format("{0:#,##0}", bCancType ? -TypeHelper.ToInt32(items_BasketSubTotal[0].AmTaxItem) : TypeHelper.ToInt32(items_BasketSubTotal[0].AmTaxItem)) : "0";
                        sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, strTemp, 17));
                        sb.Append(Environment.NewLine);

                        //부가세액
                        sb.Append(FXConsts.RECEIPT_PRINT_DESC_06);
                        strTemp = items_BasketSubTotal[0].AmTax != null && items_BasketSubTotal[0].AmTax != "" ? string.Format("{0:#,##0}", bCancType ? -TypeHelper.ToInt32(items_BasketSubTotal[0].AmTax) : TypeHelper.ToInt32(items_BasketSubTotal[0].AmTax)) : "0";
                        sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, strTemp, 17));

                        sb.Append(Environment.NewLine);
                        sb.Append(FXConsts.PRINT_LINEFEED);
                        sb.Append(Environment.NewLine);
                    }

                    #endregion

                    //합계(DOUBLE WIDE)
                    sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                    sb.Append(FXConsts.RECEIPT_PRINT_DESC_07);

                    if (items_BasketSubTotal[0].AmTotal != null && items_BasketSubTotal[0].AmTotal != "")
                    {
                        strTemp = items_BasketSubTotal[0].AmTotal != null && items_BasketSubTotal[0].AmTotal != "" ?
                            string.Format("{0:#,##0}", bCancType ? -TypeHelper.ToInt32(items_BasketSubTotal[0].AmTotal) : TypeHelper.ToInt32(items_BasketSubTotal[0].AmTotal))
                            : "0";
                    }
                    else
                    {
                        strTemp = basketHeader.TrxnAmt != null && basketHeader.TrxnAmt != "" ?
                            string.Format("{0:#,##0}", bCancType ? -TypeHelper.ToInt32(basketHeader.TrxnAmt) : TypeHelper.ToInt32(basketHeader.TrxnAmt))
                            : "0";
                    }

                    sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, strTemp, bPrint ? 24 : 32));
                    sb.Append(Environment.NewLine);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }

            return sb.ToString();
        }

        #endregion

        #region 결제관련 출력물 기본 끝

        /// <summary>
        /// 결제관련 출력물 기본 끝
        /// </summary>
        /// <param name="bPrint">true :  프린트 출력, false : 화면 출력</param>
        /// <param name="basketHeader">헤더정보</param>
        /// <param name="basket">포인트적립정보, 현금영수증정보</param>
        /// <param name="dsMsg">W-Mall 광고 메세지, 브랜드 광고 메세지, 안내 메세지 자료</param>
        /// <param name="bCancType">반품여부</param>
        /// <returns></returns>
        static string PayBaseEnd(bool bPrint, BasketHeader basketHeader, List<BasketBase> basketBase, DataSet dsMsg, bool bCancType)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                var items_BasketPointSave = basketBase.Where(p => p.BasketType.Equals(BasketTypes.BasketPointSave)).Cast<BasketPointSave>().ToArray();  //포인트적립정보
                var items_BasketCashRecpt = basketBase.Where(p => p.BasketType.Equals(BasketTypes.BasketCashRecpt)).Cast<BasketCashRecpt>().ToArray();  //현금영수증정보
                bool bOSale = basketHeader.TrxnType != NetCommConstants.TRXN_TYPE_OTH_SALE ? true : false;                                              //저장물 판매여부(true:일반상품, false:저장물)

                //포인트 적립정보
                if (bOSale && items_BasketPointSave != null && items_BasketPointSave.Length > 0)
                {
                    sb.Append(PrintPoint(bPrint, basketHeader, items_BasketPointSave[0]));
                }

                //현금영수증 정보
                if (items_BasketCashRecpt != null && items_BasketCashRecpt.Length > 0)
                {
                    sb.Append(PrintCashReceipt(bPrint, items_BasketCashRecpt[0], bCancType));
                }

                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(FXConsts.PRINT_ASTARFEED);
                sb.Append(Environment.NewLine);

                //안내메세지
                if (bOSale && dsMsg != null && dsMsg.Tables.Count > 2 && dsMsg.Tables[2] != null && dsMsg.Tables[2].Rows.Count > 0)
                {
                    sb.Append(PrintMessage(bPrint, dsMsg.Tables[2]));
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }

            return sb.ToString();
        }

        #endregion

        #region 포인트

        /// <summary>
        /// 포인트적립
        /// </summary>
        /// <param name="bPrint">true :  프린트 출력, false : 화면 출력</param>
        /// <param name="basketHeader">헤더 정보</param>
        /// <param name="basketPointSave">포인트 적립 정보</param>
        /// <returns>포인트 적립 정보 출력 문자열</returns>
        static string PrintPoint(bool bPrint, BasketHeader basketHeader, BasketPointSave basketPointSave)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                if (basketPointSave != null && basketPointSave.Length > 0 && basketPointSave.NoCard != null && basketPointSave.NoCard.Length > 0)
                {
                    bool bReturnAmt = basketHeader.CancType == "2" || basketHeader.CancType == "3" ? true : false;
                    string strNoCard = basketPointSave.NoCard != null ? basketPointSave.NoCard : "";
                    string strPointNmMember = basketPointSave.PointNmMember != null ? basketPointSave.PointNmMember : "";
                    string strCustGradeNm = basketPointSave.CustGradeNm != null ? basketPointSave.CustGradeNm : "";
                    string strAmPoint = string.Format("{0:#,##0}점", basketPointSave.AmPoint != null && basketPointSave.AmPoint.ToString() != "" ? bReturnAmt ? -TypeHelper.ToInt32(basketPointSave.AmPoint) : TypeHelper.ToInt32(basketPointSave.AmPoint) : 0);                          //발생
                    string strAmPointUsable = string.Format("{0:#,##0}점", basketPointSave.AmPointUsable != null && basketPointSave.AmPointUsable.ToString() != "" ? TypeHelper.ToInt32(basketPointSave.AmPointUsable) : 0);  //가용
                    string strAmPointDelay = string.Format("{0:#,##0}점", basketPointSave.AmPointDelay != null && basketPointSave.AmPointDelay.ToString() != "" ? TypeHelper.ToInt32(basketPointSave.AmPointDelay) : 0);   //예정
                    string strAmMarkEvt = TypeHelper.ToInt32(basketPointSave.AmMarkEvt) > 0 ? string.Format("{0:#,##0}점", basketPointSave.AmMarkEvt != null && basketPointSave.AmMarkEvt.ToString() != "" ? TypeHelper.ToInt32(basketPointSave.AmMarkEvt) : 0) : "";   //행사
                    string strAmMarkNotDay = TypeHelper.ToInt32(basketPointSave.AmMarkNotDay) > 0 ? string.Format("{0:#,##0}점", basketPointSave.AmMarkNotDay != null && basketPointSave.AmMarkNotDay.ToString() != "" ? TypeHelper.ToInt32(basketPointSave.AmMarkNotDay) : 0) : "";   //추가

                    string strNoAppr = basketPointSave.NoAppr != null ? basketPointSave.NoAppr.Replace(" ", "") : "";
                    string strDesc = strNoAppr.Length <= 0 ? FXConsts.RECEIPT_PRINT_CARD_14 : basketPointSave.Remark;
                    bool bNoAppr = strNoAppr.Length <= 0 ? true : false;

                    sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                    sb.Append(FXConsts.RECEIPT_PRINT_POINTSAVE_01);
                    sb.Append(Environment.NewLine);

                    sb.Append(FXConsts.RECEIPT_PRINT_POINTSAVE_02);
                    sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, strNoCard, 25));
                    sb.Append(Environment.NewLine);

                    sb.Append(FXConsts.RECEIPT_PRINT_POINTSAVE_03);
                    sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, strPointNmMember, 25));
                    sb.Append(Environment.NewLine);

                    sb.Append(FXConsts.RECEIPT_PRINT_POINTSAVE_04);
                    sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, strCustGradeNm, 25));
                    sb.Append(Environment.NewLine);

                    sb.Append(FXConsts.RECEIPT_PRINT_POINTSAVE_05);
                    sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, strAmPoint, 25));
                    sb.Append(Environment.NewLine);

                    if (!bNoAppr)
                    {
                        if (strAmMarkEvt.Length > 0)
                        {
                            strAmMarkEvt = bReturnAmt ? "-" + strAmMarkEvt : strAmMarkEvt;
                            sb.Append(FXConsts.RECEIPT_PRINT_POINTSAVE_09);
                            sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, strAmMarkEvt, 25));
                            sb.Append(Environment.NewLine);
                        }

                        if (strAmMarkNotDay.Length > 0)
                        {
                            strAmMarkNotDay = bReturnAmt ? "-" + strAmMarkNotDay : strAmMarkNotDay;
                            sb.Append(FXConsts.RECEIPT_PRINT_POINTSAVE_10);
                            sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, strAmMarkNotDay, 25));
                            sb.Append(Environment.NewLine);
                        }

                        sb.Append(FXConsts.RECEIPT_PRINT_POINTSAVE_08);
                        sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, strAmPointDelay, 25));
                        sb.Append(Environment.NewLine);

                        sb.Append(FXConsts.RECEIPT_PRINT_POINTSAVE_06);
                        sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, strAmPointUsable, 25));
                        sb.Append(Environment.NewLine);
                    }


                    sb.Append(FXConsts.RECEIPT_PRINT_POINTSAVE_07);

                    if (strDesc.Length > 0)
                    {
                        int iTempStart = 0;
                        int iTempLen = 0;
                        List<string> listDesc = new List<string>();

                        Regex strRegex = new Regex(@"[가-힣]");
                        for (int i = 1; i <= strDesc.Length; i++)
                        {
                            if (strRegex.IsMatch(strDesc.Substring(i - 1, 1)))
                            {
                                iTempLen += 2;
                            }
                            else
                            {
                                iTempLen += 1;
                            }

                            if (iTempLen >= 25)
                            {
                                iTempLen = 0;
                                listDesc.Add(strDesc.Substring(iTempStart, i - iTempStart));
                                iTempStart = i;
                            }
                        }

                        if (listDesc != null && listDesc.Count > 0)
                        {
                            listDesc.Add(strDesc.Substring(strDesc.LastIndexOf(listDesc[listDesc.Count - 1]) + listDesc[listDesc.Count - 1].Length, strDesc.Length - strDesc.LastIndexOf(listDesc[listDesc.Count - 1]) - listDesc[listDesc.Count - 1].Length));

                            bool bFirst = true;
                            foreach (string item in listDesc)
                            {
                                if (!bFirst)
                                {
                                    sb.Append("               ");
                                }
                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, item, 25));
                                sb.Append(Environment.NewLine);
                                bFirst = false;
                            }
                        }
                        else
                        {
                            if (strDesc.Length > 0)
                            {
                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, strDesc, 25));
                                sb.Append(Environment.NewLine);
                            }
                        }
                    }
                    else
                    {
                        sb.Append(Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }

            return sb.ToString();
        }

        #endregion

        #region 현금영수증

        /// <summary>
        /// 현금영수증
        /// </summary>
        /// <param name="bPrint">true :  프린트 출력, false : 화면 출력</param>
        /// <param name="basketCashRecpt">현금영수증 정보</param>
        /// <param name="bCancType">반품 여부</param>
        /// <returns></returns>
        static string PrintCashReceipt(bool bPrint, BasketCashRecpt basketCashRecpt, bool bCancType)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                if (basketCashRecpt != null && basketCashRecpt.Length > 0 && basketCashRecpt.NoAppr != null && basketCashRecpt.AmAppr != null && basketCashRecpt.AmAppr.Length > 0)
                {
                    string strAmAppr = basketCashRecpt.AmAppr != null && basketCashRecpt.AmAppr.Length > 0 ? bCancType ? "-" + string.Format("{0:#,##0}", TypeHelper.ToInt32(basketCashRecpt.AmAppr)) : string.Format("{0:#,##0}", TypeHelper.ToInt32(basketCashRecpt.AmAppr)) : "";
                    string strNoPersonal = basketCashRecpt.NoPersonal != null && basketCashRecpt.NoPersonal.Length > 0 ? string.Format("{0}****", basketCashRecpt.NoPersonal.Substring(0, basketCashRecpt.NoPersonal.Length - 4)) : "";
                    string strNoAppr = basketCashRecpt.NoAppr != null && basketCashRecpt.NoAppr.Replace(" ", "").Length > 0 ? basketCashRecpt.NoAppr : FXConsts.RECEIPT_PRINT_CARD_14;

                    sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                    sb.Append(basketCashRecpt.FgSelf == "1" ? FXConsts.RECEIPT_PRINT_CASH_00 : (basketCashRecpt.FgTrxnType == "1" ? FXConsts.RECEIPT_PRINT_CASH_01 : FXConsts.RECEIPT_PRINT_CASH_02));
                    sb.Append(Environment.NewLine);

                    sb.Append(FXConsts.RECEIPT_PRINT_CASH_03);
                    sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, strAmAppr, 25));
                    sb.Append(Environment.NewLine);

                    sb.Append(FXConsts.RECEIPT_PRINT_CASH_04);
                    sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, strNoPersonal, 25));
                    sb.Append(Environment.NewLine);

                    sb.Append(FXConsts.RECEIPT_PRINT_CASH_05);
                    sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, strNoAppr, 25));
                    sb.Append(Environment.NewLine);

                    sb.Append(FXConsts.RECEIPT_PRINT_CASH_06);
                    sb.Append(Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }

            return sb.ToString();
        }

        #endregion

        #region 메세지

        /// <summary>
        /// 점포명판 메세지, W-Mall 광고 메세지, 브랜드 광고 메세지, 안내 메세지
        /// </summary>
        /// <param name="bPrint">true :  프린트 출력, false : 화면 출력</param>
        /// <param name="dt">메세지 데이터</param>
        /// <returns></returns>
        static string PrintMessage(bool bPrint, DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                bool bAdd = false;
                string tempVal = string.Empty;
                string FG_SIZ = string.Empty;

                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);

                foreach (DataRow drTemp in dt.Rows)
                {
                    tempVal = TypeHelper.ToString(drTemp["NM_DESC"]);
                    FG_SIZ = TypeHelper.ToString(drTemp["FG_SIZ"]);

                    if (FG_SIZ == "1" || FG_SIZ == "2" || FG_SIZ == "3")
                    {
                        //1 == 가로확대
                        //2 == 세로확대
                        //3 == 가로세로확대
                        sb.Append(!bPrint ? "" :
                            FG_SIZ == "1" ? FXConsts.PRINT_BOLDWIDE_DOUBLE :
                            FG_SIZ == "2" ? FXConsts.PRINT_HEIGHT_DOUBLE :
                            FXConsts.PRINT_WH_DOUBLE);
                        sb.Append(getFixCut(bPrint, true, PrintTypes.WideDouble, tempVal, 40));
                        sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                    }
                    else
                    {
                        //보통, 굵게
                        sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                        sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, tempVal, 40));
                    }

                    sb.Append(Environment.NewLine);
                    bAdd = true;
                }

                if (bAdd)
                {
                    sb.Append(Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }

            return sb.ToString();
        }

        #endregion

        #region 프로모션

        /// <summary>
        /// 프로모션 출력
        /// </summary>
        /// <param name="basketHeader">헤더정보</param>
        /// <param name="basketPointSave">포인트정보</param>
        /// <param name="dtPromotion">프로모션 정보</param>
        /// <returns></returns>
        private void PayPromotion(BasketHeader basketHeader, BasketPointSave basketPointSave, DataTable dtPromotion)
        {
            try
            {
                if (dtPromotion != null && dtPromotion.Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    DataSet ds = new DataSet();
                    DataTable dtClone = new DataTable();
                    dtClone.Columns.Add("CD_PRM");
                    dtClone.Columns.Add("SQ_LOC");
                    dtClone.Columns.Add("FG_TEXT");
                    dtClone.Columns.Add("FG_SIZ");
                    dtClone.Columns.Add("NM_DESC");

                    foreach (DataRow drTemp in dtPromotion.Rows)
                    {
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            bool bAdd = false;
                            foreach (DataTable dt in ds.Tables)
                            {
                                if (dt.TableName ==
                                    string.Format("{0}_{1}_{2}_{3}_{4}", TypeHelper.ToString(drTemp["CD_STORE"]), TypeHelper.ToString(drTemp["YY_PRM"]), TypeHelper.ToString(drTemp["MM_PRM"]), TypeHelper.ToString(drTemp["WE_PRM"]), TypeHelper.ToString(drTemp["SQ_PRM"])))
                                {
                                    dt.Rows.Add(new object[] { 
                                    TypeHelper.ToString(drTemp["CD_PRM"]),
                                    TypeHelper.ToString(drTemp["SQ_LOC"]),
                                    TypeHelper.ToString(drTemp["FG_TEXT"]),
                                    TypeHelper.ToString(drTemp["FG_SIZ"]),
                                    TypeHelper.ToString(drTemp["NM_DESC"])
                                    });
                                    bAdd = true;
                                    break;
                                }
                            }

                            if (!bAdd)
                            {
                                DataTable dt = dtClone.Clone();
                                dt.TableName = string.Format("{0}_{1}_{2}_{3}_{4}", TypeHelper.ToString(drTemp["CD_STORE"]), TypeHelper.ToString(drTemp["YY_PRM"]), TypeHelper.ToString(drTemp["MM_PRM"]), TypeHelper.ToString(drTemp["WE_PRM"]), TypeHelper.ToString(drTemp["SQ_PRM"]));

                                dt.Rows.Add(new object[] { 
                                TypeHelper.ToString(drTemp["CD_PRM"]),
                                TypeHelper.ToString(drTemp["SQ_LOC"]),
                                TypeHelper.ToString(drTemp["FG_TEXT"]),
                                TypeHelper.ToString(drTemp["FG_SIZ"]),
                                TypeHelper.ToString(drTemp["NM_DESC"])
                                });

                                ds.Tables.Add(dt);
                            }
                        }
                        else
                        {
                            DataTable dt = dtClone.Clone();
                            dt.TableName = string.Format("{0}_{1}_{2}_{3}_{4}", TypeHelper.ToString(drTemp["CD_STORE"]), TypeHelper.ToString(drTemp["YY_PRM"]), TypeHelper.ToString(drTemp["MM_PRM"]), TypeHelper.ToString(drTemp["WE_PRM"]), TypeHelper.ToString(drTemp["SQ_PRM"]));

                            dt.Rows.Add(new object[] { 
                            TypeHelper.ToString(drTemp["CD_PRM"]),
                            TypeHelper.ToString(drTemp["SQ_LOC"]),
                            TypeHelper.ToString(drTemp["FG_TEXT"]),
                            TypeHelper.ToString(drTemp["FG_SIZ"]),
                            TypeHelper.ToString(drTemp["NM_DESC"])
                            });

                            ds.Tables.Add(dt);
                        }
                    }


                    if (ds != null && ds.Tables.Count > 0)
                    {
                        foreach (DataTable dtTemp in ds.Tables)
                        {
                            DataView dv = dtTemp.DefaultView;
                            dv.Sort = "SQ_LOC asc";
                            DataTable dt = dv.ToTable();

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                string strUI = "판매행사";
                                string strTITLE = "";

                                if (TypeHelper.ToString(dt.Rows[0]["CD_PRM"]) == "05")
                                {
                                    //증정권
                                    strUI = FXConsts.RECEIPT_NAME_PAY_GIFT_05;
                                    strTITLE = FXConsts.RECEIPT_PRINT_NM_13;
                                }
                                else if (TypeHelper.ToString(dt.Rows[0]["CD_PRM"]) == "07")
                                {
                                    //경품권
                                    strUI = FXConsts.RECEIPT_NAME_PAY_GIFT_07;
                                    strTITLE = FXConsts.RECEIPT_PRINT_NM_14;
                                }
                                else if (TypeHelper.ToString(dt.Rows[0]["CD_PRM"]) == "06")
                                {
                                    //응모권
                                    strUI = FXConsts.RECEIPT_NAME_PAY_GIFT_06;
                                    strTITLE = FXConsts.RECEIPT_PRINT_NM_15;
                                }

                                sb = new StringBuilder();
                                m_device.StartPrint(strUI);
                                sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                                sb.Append(m_device.PrintLogo());

                                /// <summary>
                                /// KSK Delete 16.06.01
                                /// 증정권/ 응모권 / 경품권 영수증 문구 삭제
                                /// </summary>
                                //sb.Append(Environment.NewLine);                               
                                //sb.Append(FXConsts.PRINT_BOLDWIDE_DOUBLE);
                                //sb.Append(strTITLE);
                                //sb.Append(Environment.NewLine);

                                sb.Append(Environment.NewLine);
                                sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                                sb.Append(string.Format(FXConsts.RECEIPT_PRINT_NM_16, basketHeader.OccrDate, basketHeader.PosNo, basketHeader.TrxnNo));
                                sb.Append(Environment.NewLine);
                                sb.Append(Environment.NewLine);
                                m_device.PrintNormal(sb.ToString());

                                foreach (DataRow drTemp in dt.Rows)
                                {
                                    if (TypeHelper.ToString(drTemp["FG_TEXT"]) == "0")
                                    {
                                        sb = new StringBuilder();

                                        //텍스트
                                        if (TypeHelper.ToString(drTemp["FG_SIZ"]) == "1")
                                        {
                                            //가로확대

                                            sb.Append(FXConsts.PRINT_BOLDWIDE_DOUBLE);
                                            sb.Append(getFixCut(true, PrintTypes.WideDouble, TypeHelper.ToString(drTemp["NM_DESC"]), 40));
                                            sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                                        }
                                        else if (TypeHelper.ToString(drTemp["FG_SIZ"]) == "2")
                                        {
                                            //세로확대
                                            sb.Append(FXConsts.PRINT_HEIGHT_DOUBLE);
                                            sb.Append(getFixCut(true, PrintTypes.WideNormal, TypeHelper.ToString(drTemp["NM_DESC"]), 40));
                                            sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                                        }
                                        else if (TypeHelper.ToString(drTemp["FG_SIZ"]) == "3")
                                        {
                                            //가로세로확대
                                            sb.Append(FXConsts.PRINT_WH_DOUBLE);
                                            sb.Append(getFixCut(true, PrintTypes.WideDouble, TypeHelper.ToString(drTemp["NM_DESC"]), 40));
                                            sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                                        }
                                        else
                                        {
                                            //보통, 굵게
                                            sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                                            sb.Append(getFixCut(true, PrintTypes.WideNormal, TypeHelper.ToString(drTemp["NM_DESC"]), 40));
                                        }

                                        sb.Append(Environment.NewLine);
                                        m_device.PrintNormal(sb.ToString());
                                    }
                                    else if (TypeHelper.ToString(drTemp["FG_TEXT"]) == "1")
                                    {
                                        //바코드
                                        m_device.PrintBarCode(TypeHelper.ToString(drTemp["NM_DESC"]));
                                        m_device.PrintNormal(Environment.NewLine);
                                    }
                                }

                                sb = new StringBuilder();

                                if (strTITLE == FXConsts.RECEIPT_PRINT_NM_15)
                                {
                                    sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                                    sb.Append(Environment.NewLine);
                                    sb.Append(FXConsts.RECEIPT_PRINT_EXCHANGE_RETURN_08);
                                    sb.Append(Environment.NewLine);
                                    sb.Append(FXConsts.RECEIPT_PRINT_EXCHANGE_RETURN_09);
                                    sb.Append(Environment.NewLine);
                                    sb.Append(FXConsts.RECEIPT_PRINT_EXCHANGE_RETURN_10);
                                    sb.Append(Environment.NewLine);
                                    sb.Append(FXConsts.RECEIPT_PRINT_EXCHANGE_RETURN_11);
                                    sb.Append(Environment.NewLine);
                                    sb.Append(Environment.NewLine);
                                }

                                sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                                sb.Append(FXConsts.PRINT_DLINEFEED);
                                sb.Append(Environment.NewLine);
                                sb.Append(FXConsts.PRINT_LAST);
                                m_device.PrintNormal(sb.ToString());

                                //출력물 종료
                                m_device.EndPrint();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
        }

        #endregion

        #region 신용카드 관련 출력 및 정보

        /// <summary>
        /// 카드 수기서명 정보
        /// </summary>
        public enum CardPrint
        {
            Basic = 0,  //신용카드 시작
            Charge = 1, //신용카드 청구용
            Keep = 2,   //신용카드 보관용
            None = 3  //신용카드 보관용
        }

        /// <summary>
        /// 카드 수기서명시 출력물
        /// </summary>
        /// <param name="bPrint">true : 프린터 출력, false : 화면 출력</param>
        /// <param name="bpCard">카드정보</param>
        /// <param name="bCancType">반품여부</param>
        /// <returns></returns>
        static string PrintCardPrint(bool bPrint, BasketPayCard bpCard, bool bCancType)
        {
            StringBuilder sb = new StringBuilder();

            if (bpCard != null)
            {
                string tempVal = string.Empty;
                Int64 iTempPay = (bpCard.PayAmt != null ? TypeHelper.ToInt32(bpCard.PayAmt) : 0);
                Int64 iTempBal = (bpCard.BalAmt != null ? TypeHelper.ToInt32(bpCard.BalAmt) : 0);

                //신용카드    
                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                sb.Append(FXConsts.RECEIPT_PRINT_NM_12);
                sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, string.Format("{0:#,##0}", bCancType ? -iTempPay : iTempPay), 24));
                sb.Append(Environment.NewLine);

                //카드번호
                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                // 여전법 변경 주석처리
                // 46588*******3960 [정상승인]A HANUR  → 465887******3960 로 표시 해야 함
                // tempVal = string.Format("  {0}{1}{2}", bpCard.CardNo.Substring(0, 5), "****", bpCard.CardNo.Substring(9, bpCard.CardNo.Length == 19 ? 10 : bpCard.CardNo.Length - 9));
                //tempVal = string.Format("  {0}{1}{2}", bpCard.CardNo.Substring(0, 6), "******", bpCard.CardNo.Substring(12, bpCard.CardNo.Length == 19 ? 13 : bpCard.CardNo.Length - 12));
                //KSK_20170403

                //KSK_20170403
                if ("Y".Equals(bpCard.EunCardFg))
                {
                    tempVal = string.Format("  {0}{1}{2}", bpCard.CardNo.Substring(0, 6), "@@@@@@", bpCard.CardNo.Substring(12, bpCard.CardNo.Length - 12));
                    sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, tempVal, bpCard.CardNo.Length == 19 ? 22 : 18));
                }
                else
                {
                    tempVal = string.Format("  {0}{1}{2}", bpCard.CardNo.Substring(0, 6), "******", bpCard.CardNo.Substring(12, bpCard.CardNo.Length - 12));
                    sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, tempVal, bpCard.CardNo.Length == 19 ? 22 : 18));
                }

                //여전법 추가 0808(KSK)
                tempVal = string.Empty;

                //승인구분([정상승인])
                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, bpCard.ApprState != null && bpCard.ApprState.Length > 0 ? (bpCard.ApprState == "1" ? FXConsts.RECEIPT_PRINT_CARD_01 : FXConsts.RECEIPT_PRINT_CARD_02) : "", 11));

                //카드번호입력구분(A=카드 read, @=카드번호 수기입력)
                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, bpCard.InputType != null && bpCard.InputType.Length > 0 ? bpCard.InputType : "", 2));

                //벤사ID
                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, bpCard.VanId != null && bpCard.VanId.Length > 0 ? bpCard.VanId : "", bpCard.CardNo.Length == 19 ? 5 : 9));
                sb.Append(Environment.NewLine);

                //카드회사명
                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, string.Format("  {0}", bpCard.CardNm != null && bpCard.CardNm.Length > 0 ? bpCard.CardNm : ""), 18));

                //유효기간(**/**)
                sb.Append(FXConsts.RECEIPT_PRINT_CARD_03);
                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, bpCard.ExpMY != null && bpCard.ExpMY.Length >= 4 ? "**/**" : "", 9));
                sb.Append(Environment.NewLine);

                #region 신용카드 정상승인일 경우 출력

                //매입사명
                sb.Append(FXConsts.RECEIPT_PRINT_CARD_04);
                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, bpCard.MaeipComNm != null && bpCard.MaeipComNm.Length > 0 ? bpCard.MaeipComNm : "", 27));
                sb.Append(Environment.NewLine);

                if (bpCard.ApprState == "1")
                {
                    //가맹점번호
                    sb.Append(FXConsts.RECEIPT_PRINT_CARD_05);
                    sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, bpCard.MerchantCode != null && bpCard.MerchantCode.Length > 0 ? bpCard.MerchantCode : "", 27));
                    sb.Append(Environment.NewLine);
                }

                #endregion

                //승인번호
                tempVal = bpCard.ApprNo != null && bpCard.ApprNo.Length > 0 ? bpCard.ApprNo : "";
                sb.Append(FXConsts.RECEIPT_PRINT_CARD_06);
                if (tempVal.Replace(" ", "").Length <= 0)
                {
                    sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, FXConsts.RECEIPT_PRINT_CARD_14, 8));
                }
                else
                {
                    sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, tempVal, 8));
                }
                //여전법 추가 0808(KSK)
                tempVal = string.Empty;

                //할부개월(일시불00)
                sb.Append(FXConsts.RECEIPT_PRINT_CARD_07);
                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, bpCard.Halbu != null && bpCard.Halbu.Length > 0 ? (bpCard.Halbu.Length == 1 ? "0" + bpCard.Halbu : bpCard.Halbu) : "00", 6));
                sb.Append(Environment.NewLine);
            }
            else
            {
                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(FXConsts.PRINT_ASTARFEED);
                sb.Append(Environment.NewLine);
                sb.Append(FXConsts.RECEIPT_PRINT_CARD_11);
                sb.Append(Environment.NewLine);
                sb.Append(FXConsts.RECEIPT_PRINT_CARD_12);
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append(FXConsts.RECEIPT_PRINT_CARD_13);
                sb.Append(Environment.NewLine);
                sb.Append(FXConsts.PRINT_ASTARFEED);
                sb.Append(Environment.NewLine);
            }

            string returnString = string.Empty;
            returnString = sb.ToString();

            sb.Remove(0, sb.Length);

            return returnString;
        }

        #endregion

        #region 무효영수증 출력

        /// <summary>
        /// 무효영수증 출력
        /// </summary>
        /// <returns></returns>
        static string RePrint()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
            sb.Append(FXConsts.PRINT_ASTARFEED);
            sb.Append(Environment.NewLine);
            sb.Append(FXConsts.PRINT_BOLDWIDE_DOUBLE);
            sb.Append(FXConsts.RECEIPT_REPRINT);
            sb.Append(Environment.NewLine);
            sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
            sb.Append(FXConsts.PRINT_ASTARFEED);
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }

        #endregion

        #endregion

        #region 사은품 회수

        /// <summary>
        /// 사은품 회수
        /// </summary>
        /// <param name="bPrint">true : 프린터 출력, false : 화면 출력</param>
        /// <param name="bRePrint">재발행여부(true:재발행, false:발행)</param>
        /// <param name="basketHeader">판매 헤더정보</param>
        /// <param name="basketTksPresentRtn">사은품 회수 정보</param>
        /// <returns></returns>
        public string PrintTksPresentRtn(bool bPrint, bool bRePrint, BasketHeader basketHeader, List<BasketTksPresentRtn> basketTksPresentRtn)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                if (basketHeader != null && basketTksPresentRtn != null && basketTksPresentRtn.Count > 0)
                {
                    #region 사은품회수 내역에서 전체 미회수면 출력하지 않는다

                    foreach (BasketTksPresentRtn bpConfirm in basketTksPresentRtn)
                    {
                        if (bpConfirm != null)
                        {
                            if (bpConfirm.RtnCantRsn != "1" && bpConfirm.RtnCantRsn != "2")
                            {
                                #region 무효영수증
                                sb.Append(bPrint && bRePrint ? RePrint() : "");
                                #endregion

                                //판매 기본 타이틀
                                sb.Append(ReceiptBaseTitle(bPrint, basketHeader, string.Empty));

                                //사은품 반납 타이틀
                                sb.Append(FXConsts.PRINT_ASTARFEED);
                                sb.Append(Environment.NewLine);
                                sb.Append(!bPrint ? FXConsts.RECEIPT_PRINT_GIFT_RETURN_01_TEXT : FXConsts.RECEIPT_PRINT_GIFT_RETURN_01);
                                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                                sb.Append(!bPrint ? FXConsts.RECEIPT_PRINT_GIFT_RETURN_02_TEXT : FXConsts.RECEIPT_PRINT_GIFT_RETURN_02);
                                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                                sb.Append(!bPrint ? FXConsts.RECEIPT_PRINT_GIFT_RETURN_03_TEXT : FXConsts.RECEIPT_PRINT_GIFT_RETURN_03);
                                sb.Append(Environment.NewLine);
                                sb.Append(FXConsts.PRINT_ASTARFEED);
                                sb.Append(Environment.NewLine);

                                //반품 거래정보
                                sb.Append(FXConsts.RECEIPT_PRINT_GIFT_RETURN_04);
                                sb.Append(string.Format("{0}-{1}-{2}", TypeHelper.ToString(basketHeader.OTSaleDate), TypeHelper.ToString(basketHeader.OTPosNo), TypeHelper.ToString(basketHeader.OTTrxnNo)));
                                sb.Append(Environment.NewLine);

                                #region 사은품회수 내역 만큼 반복 출력

                                foreach (BasketTksPresentRtn bp in basketTksPresentRtn)
                                {
                                    if (bp != null && bp.RtnCantRsn != "1" && bp.RtnCantRsn != "2")
                                    {
                                        sb.Append(Environment.NewLine);

                                        //사은품 증정내역
                                        sb.Append(FXConsts.RECEIPT_PRINT_GIFT_RETURN_05);
                                        sb.Append(Environment.NewLine);

                                        //증정번호
                                        sb.Append(FXConsts.RECEIPT_PRINT_GIFT_RETURN_06);
                                        sb.Append(string.Format("{0}-{1}-{2}", TypeHelper.ToString(bp.PresentDate), TypeHelper.ToString(bp.PresentNo), TypeHelper.ToString(bp.PresentSeq)));
                                        sb.Append(Environment.NewLine);

                                        //증정상품
                                        sb.Append(FXConsts.RECEIPT_PRINT_GIFT_RETURN_07);
                                        sb.Append(TypeHelper.ToString(bp.PresentNm));
                                        sb.Append(Environment.NewLine);

                                        //증정금액
                                        sb.Append(FXConsts.RECEIPT_PRINT_GIFT_RETURN_08);
                                        sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}원", TypeHelper.ToInt64(bp.PresentAmt)), 11));
                                        sb.Append(Environment.NewLine);
                                        sb.Append(Environment.NewLine);

                                        //사은품 반납내역
                                        sb.Append(FXConsts.RECEIPT_PRINT_GIFT_RETURN_09);
                                        sb.Append(Environment.NewLine);

                                        if (TypeHelper.ToInt64(bp.RtnPrsAmt) > 0)
                                        {
                                            //현물회수
                                            sb.Append(FXConsts.RECEIPT_PRINT_GIFT_RETURN_10);
                                            sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}원", TypeHelper.ToInt64(bp.RtnPrsAmt)), 11));
                                            sb.Append(Environment.NewLine);
                                        }

                                        if (TypeHelper.ToInt64(bp.RtnCashAmt) + TypeHelper.ToInt64(bp.RtnGiftCashAmt) > 0)
                                        {
                                            //현금회수
                                            sb.Append(FXConsts.RECEIPT_PRINT_GIFT_RETURN_11);
                                            sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}원", TypeHelper.ToInt64(bp.RtnCashAmt) + TypeHelper.ToInt64(bp.RtnGiftCashAmt)), 11));
                                            sb.Append(Environment.NewLine);
                                        }

                                        if (TypeHelper.ToInt64(bp.RtnGiftAmt) > 0)
                                        {
                                            //상품권회수
                                            sb.Append(FXConsts.RECEIPT_PRINT_GIFT_RETURN_12);
                                            sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}원", TypeHelper.ToInt64(bp.RtnGiftAmt)), 11));
                                            sb.Append(Environment.NewLine);

                                            if (TypeHelper.ToInt64(bp.GiftCount1) > 0 && TypeHelper.ToInt64(bp.GiftAmt1) > 0 && TypeHelper.ToString(bp.GiftNo1).Length > 0)
                                            {
                                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, string.Format("  {0} ", TypeHelper.ToString(bp.GiftNo1)), 16));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}매", TypeHelper.ToInt64(bp.GiftCount1)), 5));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}원", TypeHelper.ToInt64(bp.GiftAmt1)), 12));
                                                sb.Append(Environment.NewLine);
                                            }

                                            if (TypeHelper.ToInt64(bp.GiftCount2) > 0 && TypeHelper.ToInt64(bp.GiftAmt2) > 0 && TypeHelper.ToString(bp.GiftNo2).Length > 0)
                                            {
                                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, string.Format("  {0} ", TypeHelper.ToString(bp.GiftNo2)), 16));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}매", TypeHelper.ToInt64(bp.GiftCount2)), 5));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}원", TypeHelper.ToInt64(bp.GiftAmt2)), 12));
                                                sb.Append(Environment.NewLine);
                                            }

                                            if (TypeHelper.ToInt64(bp.GiftCount3) > 0 && TypeHelper.ToInt64(bp.GiftAmt3) > 0 && TypeHelper.ToString(bp.GiftNo3).Length > 0)
                                            {
                                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, string.Format("  {0} ", TypeHelper.ToString(bp.GiftNo3)), 16));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}매", TypeHelper.ToInt64(bp.GiftCount3)), 5));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}원", TypeHelper.ToInt64(bp.GiftAmt3)), 12));
                                                sb.Append(Environment.NewLine);
                                            }

                                            if (TypeHelper.ToInt64(bp.GiftCount4) > 0 && TypeHelper.ToInt64(bp.GiftAmt4) > 0 && TypeHelper.ToString(bp.GiftNo4).Length > 0)
                                            {
                                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, string.Format("  {0} ", TypeHelper.ToString(bp.GiftNo4)), 16));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}매", TypeHelper.ToInt64(bp.GiftCount4)), 5));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}원", TypeHelper.ToInt64(bp.GiftAmt4)), 12));
                                                sb.Append(Environment.NewLine);
                                            }

                                            if (TypeHelper.ToInt64(bp.GiftCount5) > 0 && TypeHelper.ToInt64(bp.GiftAmt5) > 0 && TypeHelper.ToString(bp.GiftNo5).Length > 0)
                                            {
                                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, string.Format("  {0} ", TypeHelper.ToString(bp.GiftNo5)), 16));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}매", TypeHelper.ToInt64(bp.GiftCount5)), 5));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}원", TypeHelper.ToInt64(bp.GiftAmt5)), 12));
                                                sb.Append(Environment.NewLine);
                                            }

                                            if (TypeHelper.ToInt64(bp.GiftCount6) > 0 && TypeHelper.ToInt64(bp.GiftAmt6) > 0 && TypeHelper.ToString(bp.GiftNo6).Length > 0)
                                            {
                                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, string.Format("  {0} ", TypeHelper.ToString(bp.GiftNo6)), 16));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}매", TypeHelper.ToInt64(bp.GiftCount6)), 5));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}원", TypeHelper.ToInt64(bp.GiftAmt6)), 12));
                                                sb.Append(Environment.NewLine);
                                            }

                                            if (TypeHelper.ToInt64(bp.GiftCount7) > 0 && TypeHelper.ToInt64(bp.GiftAmt7) > 0 && TypeHelper.ToString(bp.GiftNo7).Length > 0)
                                            {
                                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, string.Format("  {0} ", TypeHelper.ToString(bp.GiftNo7)), 16));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}매", TypeHelper.ToInt64(bp.GiftCount7)), 5));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}원", TypeHelper.ToInt64(bp.GiftAmt7)), 15));
                                                sb.Append(Environment.NewLine);
                                            }

                                            if (TypeHelper.ToInt64(bp.GiftCount8) > 0 && TypeHelper.ToInt64(bp.GiftAmt8) > 0 && TypeHelper.ToString(bp.GiftNo8).Length > 0)
                                            {
                                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, string.Format("  {0} ", TypeHelper.ToString(bp.GiftNo8)), 16));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}매", TypeHelper.ToInt64(bp.GiftCount8)), 5));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}원", TypeHelper.ToInt64(bp.GiftAmt8)), 12));
                                                sb.Append(Environment.NewLine);
                                            }

                                            if (TypeHelper.ToInt64(bp.GiftCount9) > 0 && TypeHelper.ToInt64(bp.GiftAmt9) > 0 && TypeHelper.ToString(bp.GiftNo9).Length > 0)
                                            {
                                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, string.Format("  {0} ", TypeHelper.ToString(bp.GiftNo9)), 16));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}매", TypeHelper.ToInt64(bp.GiftCount9)), 5));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}원", TypeHelper.ToInt64(bp.GiftAmt9)), 12));
                                                sb.Append(Environment.NewLine);
                                            }

                                            if (TypeHelper.ToInt64(bp.GiftCount10) > 0 && TypeHelper.ToInt64(bp.GiftAmt10) > 0 && TypeHelper.ToString(bp.GiftNo10).Length > 0)
                                            {
                                                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, string.Format("  {0} ", TypeHelper.ToString(bp.GiftNo10)), 16));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}매", TypeHelper.ToInt64(bp.GiftCount10)), 5));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}원", TypeHelper.ToInt64(bp.GiftAmt10)), 12));
                                                sb.Append(Environment.NewLine);
                                            }

                                            Int64 tempCnt = 0;
                                            tempCnt += TypeHelper.ToInt64(bp.GiftCount1) > 0 && TypeHelper.ToInt64(bp.GiftAmt1) > 0 && TypeHelper.ToString(bp.GiftNo1).Length <= 0 ? TypeHelper.ToInt64(bp.GiftCount1) : 0;
                                            tempCnt += TypeHelper.ToInt64(bp.GiftCount2) > 0 && TypeHelper.ToInt64(bp.GiftAmt2) > 0 && TypeHelper.ToString(bp.GiftNo2).Length <= 0 ? TypeHelper.ToInt64(bp.GiftCount2) : 0;
                                            tempCnt += TypeHelper.ToInt64(bp.GiftCount3) > 0 && TypeHelper.ToInt64(bp.GiftAmt3) > 0 && TypeHelper.ToString(bp.GiftNo3).Length <= 0 ? TypeHelper.ToInt64(bp.GiftCount3) : 0;
                                            tempCnt += TypeHelper.ToInt64(bp.GiftCount4) > 0 && TypeHelper.ToInt64(bp.GiftAmt4) > 0 && TypeHelper.ToString(bp.GiftNo4).Length <= 0 ? TypeHelper.ToInt64(bp.GiftCount4) : 0;
                                            tempCnt += TypeHelper.ToInt64(bp.GiftCount5) > 0 && TypeHelper.ToInt64(bp.GiftAmt5) > 0 && TypeHelper.ToString(bp.GiftNo5).Length <= 0 ? TypeHelper.ToInt64(bp.GiftCount5) : 0;
                                            tempCnt += TypeHelper.ToInt64(bp.GiftCount6) > 0 && TypeHelper.ToInt64(bp.GiftAmt6) > 0 && TypeHelper.ToString(bp.GiftNo6).Length <= 0 ? TypeHelper.ToInt64(bp.GiftCount6) : 0;
                                            tempCnt += TypeHelper.ToInt64(bp.GiftCount7) > 0 && TypeHelper.ToInt64(bp.GiftAmt7) > 0 && TypeHelper.ToString(bp.GiftNo7).Length <= 0 ? TypeHelper.ToInt64(bp.GiftCount7) : 0;
                                            tempCnt += TypeHelper.ToInt64(bp.GiftCount8) > 0 && TypeHelper.ToInt64(bp.GiftAmt8) > 0 && TypeHelper.ToString(bp.GiftNo8).Length <= 0 ? TypeHelper.ToInt64(bp.GiftCount8) : 0;
                                            tempCnt += TypeHelper.ToInt64(bp.GiftCount9) > 0 && TypeHelper.ToInt64(bp.GiftAmt9) > 0 && TypeHelper.ToString(bp.GiftNo9).Length <= 0 ? TypeHelper.ToInt64(bp.GiftCount9) : 0;
                                            tempCnt += TypeHelper.ToInt64(bp.GiftCount10) > 0 && TypeHelper.ToInt64(bp.GiftAmt10) > 0 && TypeHelper.ToString(bp.GiftNo10).Length <= 0 ? TypeHelper.ToInt64(bp.GiftCount10) : 0;

                                            Int64 tempAmt = 0;
                                            tempAmt += TypeHelper.ToInt64(bp.GiftCount1) > 0 && TypeHelper.ToInt64(bp.GiftAmt1) > 0 && TypeHelper.ToString(bp.GiftNo1).Length <= 0 ? TypeHelper.ToInt64(bp.GiftAmt1) : 0;
                                            tempAmt += TypeHelper.ToInt64(bp.GiftCount2) > 0 && TypeHelper.ToInt64(bp.GiftAmt2) > 0 && TypeHelper.ToString(bp.GiftNo2).Length <= 0 ? TypeHelper.ToInt64(bp.GiftAmt2) : 0;
                                            tempAmt += TypeHelper.ToInt64(bp.GiftCount3) > 0 && TypeHelper.ToInt64(bp.GiftAmt3) > 0 && TypeHelper.ToString(bp.GiftNo3).Length <= 0 ? TypeHelper.ToInt64(bp.GiftAmt3) : 0;
                                            tempAmt += TypeHelper.ToInt64(bp.GiftCount4) > 0 && TypeHelper.ToInt64(bp.GiftAmt4) > 0 && TypeHelper.ToString(bp.GiftNo4).Length <= 0 ? TypeHelper.ToInt64(bp.GiftAmt4) : 0;
                                            tempAmt += TypeHelper.ToInt64(bp.GiftCount5) > 0 && TypeHelper.ToInt64(bp.GiftAmt5) > 0 && TypeHelper.ToString(bp.GiftNo5).Length <= 0 ? TypeHelper.ToInt64(bp.GiftAmt5) : 0;
                                            tempAmt += TypeHelper.ToInt64(bp.GiftCount6) > 0 && TypeHelper.ToInt64(bp.GiftAmt6) > 0 && TypeHelper.ToString(bp.GiftNo6).Length <= 0 ? TypeHelper.ToInt64(bp.GiftAmt6) : 0;
                                            tempAmt += TypeHelper.ToInt64(bp.GiftCount7) > 0 && TypeHelper.ToInt64(bp.GiftAmt7) > 0 && TypeHelper.ToString(bp.GiftNo7).Length <= 0 ? TypeHelper.ToInt64(bp.GiftAmt7) : 0;
                                            tempAmt += TypeHelper.ToInt64(bp.GiftCount8) > 0 && TypeHelper.ToInt64(bp.GiftAmt8) > 0 && TypeHelper.ToString(bp.GiftNo8).Length <= 0 ? TypeHelper.ToInt64(bp.GiftAmt8) : 0;
                                            tempAmt += TypeHelper.ToInt64(bp.GiftCount9) > 0 && TypeHelper.ToInt64(bp.GiftAmt9) > 0 && TypeHelper.ToString(bp.GiftNo9).Length <= 0 ? TypeHelper.ToInt64(bp.GiftAmt9) : 0;
                                            tempAmt += TypeHelper.ToInt64(bp.GiftCount10) > 0 && TypeHelper.ToInt64(bp.GiftAmt10) > 0 && TypeHelper.ToString(bp.GiftNo10).Length <= 0 ? TypeHelper.ToInt64(bp.GiftAmt10) : 0;

                                            if (TypeHelper.ToInt64(tempCnt) > 0 && TypeHelper.ToInt64(tempAmt) > 0)
                                            {
                                                //구상품교환권
                                                sb.Append(FXConsts.RECEIPT_PRINT_GIFT_RETURN_13);
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}매", tempCnt), 7));
                                                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}원", tempAmt), 12));
                                            }
                                        }

                                        sb.Append(Environment.NewLine);
                                        sb.Append(FXConsts.PRINT_ASTARFEED);
                                        sb.Append(Environment.NewLine);
                                    }
                                }

                                break;

                                #endregion

                                #region 무효영수증
                                sb.Append(bPrint && bRePrint ? RePrint() : "");
                                #endregion
                            }
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }
            finally
            {
                Print(bPrint, FXConsts.RECEIPT_NAME_PAY_GIFT_RETURN, true, sb.ToString(), string.Empty, true);
            }

            return sb.ToString();
        }

        #endregion

        #region DCC

        /// <summary>
        /// DCC Offer 출력한다
        /// </summary>
        /// <param name="m_isVisaCard">true:VISA, false:Master&JCB</param>
        /// <param name="localAmount">원화금액 예: KRW 100,000</param>
        /// <param name="exchangeRate">예: JPY100 = KRW 1048.8440</param>
        /// <param name="transCurrency">예: JPY 9,534</param>
        /// <param name="homeCode">외화코드 예: JPY</param>
        /// <param name="markupPercent">예: 3.00%</param>
        /// <param name="dtDcc">DccOffer 내용</param>
        /// <returns></returns>
        public string SetPrintDCCOffer(bool m_isVisaCard, string localAmount, string exchangeRate, string transCurrency,
            string homeCode, string markupPercent, DataTable dtDcc)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                //Title
                sb.Append("             ");
                sb.Append(FXConsts.PRINT_BOLDWIDE_DOUBLE);
                //sb.Append(FXConsts.PRINT_CENTER);
                sb.Append(FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_01);
                sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);

                //시스템 일자
                sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(getFixCut(false, PrintTypes.WideNormal, DateTime.Now.ToString("yyyy/MM/dd HH:mm:dd"), 40));
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);

                //Local Amount
                sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(getFixCut(true, PrintTypes.WideNormal, FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_02, FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_02.Length));
                sb.Append(getFixCut(false, PrintTypes.WideNormal, localAmount, 40 - FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_02.Length));
                sb.Append(Environment.NewLine);

                //Exchange Rate
                sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(getFixCut(true, PrintTypes.WideNormal, FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_03, FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_03.Length));
                sb.Append(getFixCut(false, PrintTypes.WideNormal, exchangeRate, 40 - FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_03.Length));
                sb.Append(Environment.NewLine);

                //Markup included in FX
                if (m_isVisaCard)
                {
                    //VISA
                    sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                    sb.Append(getFixCut(true, PrintTypes.WideNormal, FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_04, FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_04.Length));
                    sb.Append(getFixCut(false, PrintTypes.WideNormal, markupPercent, 40 - FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_04.Length));
                    sb.Append(Environment.NewLine);
                }
                else
                {
                    //MASTER&JCB
                    sb.Append(Environment.NewLine);
                }

                sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(FXConsts.PRINT_LINEFEED);
                sb.Append(Environment.NewLine);

                ////Total Transaction Currency
                sb.Append(FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(getFixCut(true, PrintTypes.WideNormal, FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_05, FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_05.Length));
                sb.Append(getFixCut(false, PrintTypes.WideNormal, transCurrency, 40 - FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_05.Length));
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);

                sb.Append(FXConsts.PRINT_NORMAL);
                foreach (DataRow dr in dtDcc.Rows)
                {
                    string str = TypeHelper.ToString(dr["NM_BODY"])
                        .Replace("#cur", FXConsts.PRINT_BOLDWIDE_NORMAL + homeCode + FXConsts.PRINT_NORMAL)
                        .Replace("#mku", FXConsts.PRINT_BOLDWIDE_NORMAL + markupPercent.Replace("%", "") + FXConsts.PRINT_NORMAL);
                    sb.Append(getFixCut(true, PrintTypes.WideNormal, str, 44));
                    sb.Append(Environment.NewLine);
                }

                // Loc changed 11.13
                // DCCFooter.bmp Image 출력 후 4빈 줄 출력한다
                // sb.Append(FXConsts.PRINT_LAST);
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }
            finally
            {
                Print(true, FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_01, false, sb.ToString(), string.Empty, true);
            }

            return sb.ToString();
        }

        /// <summary>
        /// DCC 출력한다
        /// </summary>
        /// <param name="bPrint">true:프린트 출력, false:화면 출력</param>
        /// <param name="bCancType">반품여부</param>
        /// <param name="isVisaCard">true:VISA, false:Master&JCB</param>
        /// <param name="selectKRW">true:원화, false:외화</param>
        /// <param name="localCode">원화코드</param>
        /// <param name="localAmount">원화금액 예: KRW 100,000</param>
        /// <param name="homeCode">외화코드 예: JPY</param>
        /// <param name="homeAmount">외화금액</param>
        /// <param name="exchangeRate">환율</param>
        /// <param name="markupPercent"></param>
        /// <param name="NatCurNo"></param>
        /// <param name="NatCurCode"></param>
        /// <param name="dtDcc"></param>
        /// <returns></returns>
        private string SetPrintDcc(bool bPrint, bool bCancType, bool isVisaCard, bool selectKRW, string ForeignCardFg, string localCode, string localAmount,
            string homeCode, string homeAmount, string exchangeRate, string markupPercent, string NatCurNo, string NatCurCode,
            DataTable dtDcc)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                //Local Amount
                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, "  " + FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_02, ("  " + FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_02).Length));
                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, localCode + " " + (bCancType ? "-" + localAmount : localAmount), 40 - ("  " + FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_02).Length));
                sb.Append(Environment.NewLine);

                if (!selectKRW)
                {
                    //Exchange Rate
                    sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                    sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, "  " + FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_03, ("  " + FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_03).Length));
                    sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, exchangeRate, 40 - ("  " + FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_03).Length));
                    sb.Append(Environment.NewLine);

                    //Markup included in FX
                    if (isVisaCard)
                    {
                        //VISA
                        sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                        sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, "  " + FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_04, ("  " + FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_04).Length));
                        sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, markupPercent, 40 - ("  " + FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_04).Length));
                        sb.Append(Environment.NewLine);
                    }

                    sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                    sb.Append(FXConsts.PRINT_LINEFEED);
                    sb.Append(Environment.NewLine);

                    ////Total Transaction Currency
                    sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                    sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_05, FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_05.Length));
                    sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, homeCode + " " + (bCancType ? "-" + homeAmount : homeAmount), 40 - FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_05.Length));
                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);
                }
                else
                {
                    //원화선택시
                    sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                    sb.Append(FXConsts.PRINT_LINEFEED);
                    sb.Append(Environment.NewLine);
                }

                if (!bCancType)
                {
                    string strFilter = selectKRW ? "DR04" : (ForeignCardFg == "03" ? "DR01" : ForeignCardFg == "04" ? "DR02" : "DR03");
                    DataRow[] drFilter = dtDcc.Select(string.Format("CD_HEAD = '{0}'", strFilter));

                    if (drFilter != null && drFilter.Length > 0)
                    {
                        sb.Append(!bPrint ? "" : FXConsts.PRINT_NORMAL);

                        foreach (DataRow dr in drFilter)
                        {
                            if (!selectKRW)
                            {
                                sb.Append(TypeHelper.ToString(dr["NM_BODY"])
                                .Replace("#cur", (!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL) + homeCode + (!bPrint ? "" : FXConsts.PRINT_NORMAL))
                                .Replace("#mku", (!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL) + markupPercent.Replace("%", "") + (!bPrint ? "" : FXConsts.PRINT_NORMAL)));
                            }
                            else
                            {
                                sb.Append(TypeHelper.ToString(dr["NM_BODY"])
                                .Replace("#cur", (!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL) + NatCurCode + (!bPrint ? "" : FXConsts.PRINT_NORMAL))
                                .Replace("#mku", (!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL) + markupPercent.Replace("%", "") + (!bPrint ? "" : FXConsts.PRINT_NORMAL)));
                            }

                            sb.Append(Environment.NewLine);
                        }
                    }

                    sb.Append(Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }

            return sb.ToString();
        }

        #endregion

        #region 준비금

        /// <summary>
        /// 준비금 출력
        /// </summary>
        /// <param name="bPrint">true :  프린트 출력, false : 화면 출력</param>
        /// <param name="basketHeader">헤더정보</param>
        /// <param name="basketReserve">준비금 정보</param>
        /// <returns></returns>
        public string PrintIO_M001(bool bPrint, BasketHeader basketHeader, BasketReserve basketReserve)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                //기본 타이틀
                sb.Append(ReceiptBaseTitle(bPrint, basketHeader, string.Empty));

                sb.Append(Environment.NewLine);
                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                sb.Append(bPrint ? getFixCut(bPrint, true, PrintTypes.WideDouble, FXConsts.RECEIPT_RESERV, 40) : FXConsts.RECEIPT_RESERV_FORM);
                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(Environment.NewLine);

                //이전준비금
                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, FXConsts.RECEIPT_RESERV_PRE, 29));
                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}", TypeHelper.ToInt64(basketReserve.PreReserveAmt)), 11));
                sb.Append(Environment.NewLine);

                //최종준비금
                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, string.Format("{0} ({1}회)", FXConsts.RECEIPT_RESERV_LAST, TypeHelper.ToInt32(basketReserve.ReserveNo)), 29));
                sb.Append(getFixCut(bPrint, false, PrintTypes.WideNormal, string.Format("{0:#,##0}", TypeHelper.ToInt64(basketReserve.ReserveAmt)), 11));

                sb.Append(FXConsts.PRINT_LAST);
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }
            finally
            {
                Print(bPrint, FXConsts.RECEIPT_NAME_POS_IO_M001, true, sb.ToString(), string.Empty, false);
            }

            return sb.ToString();
        }

        #endregion

        #region 중간입금, 마감입금

        /// <summary>
        /// 중간입금, 마감입금 출력
        /// </summary>
        /// <param name="bPrint">true : 프린터 출력, false : 화면 출력</param>
        /// <param name="basketHeader">헤더정보</param>
        /// <param name="basketMiddleDeposit">중간입금정보</param>
        /// <returns></returns>
        public string PrintIO_M002_M003(bool bPrint, string strGubun, BasketHeader basketHeader, BasketMiddleDeposit basketMiddleDeposit)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                //기본 타이틀
                sb.Append(ReceiptBaseTitle(bPrint, basketHeader, string.Empty));

                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                sb.Append(Environment.NewLine);

                if (bPrint)
                {
                    sb.Append(strGubun == FXConsts.RECEIPT_NAME_POS_IO_M002 ? FXConsts.RECEIPT_MIDDLE : FXConsts.RECEIPT_CLOSE);
                }
                else
                {
                    sb.Append(strGubun == FXConsts.RECEIPT_NAME_POS_IO_M002 ? FXConsts.RECEIPT_MIDDLE_FORM : FXConsts.RECEIPT_CLOSE_FORM);
                }

                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);

                if (strGubun == FXConsts.RECEIPT_NAME_POS_IO_M002)
                {
                    sb.Append(string.Format(FXConsts.RECEIPT_DEPOSIT_COUNT, TypeHelper.ToInt64(basketMiddleDeposit.MiddleDepositCnt)));
                    sb.Append(Environment.NewLine);
                }

                //현금
                sb.Append(FXConsts.RECEIPT_DEPOSIT_CASH_TEXT);
                sb.Append(Environment.NewLine);
                sb.Append(POSPrinterUtils.DepositList("01:" + ConfigData.Current.SysMessage.GetMessage("00259"), TypeHelper.ToInt64(basketMiddleDeposit.WonCnt_1000000), TypeHelper.ToInt64(basketMiddleDeposit.WonAmt_1000000)));
                sb.Append(POSPrinterUtils.DepositList("02:" + ConfigData.Current.SysMessage.GetMessage("00294"), TypeHelper.ToInt64(basketMiddleDeposit.WonCnt_50000), TypeHelper.ToInt64(basketMiddleDeposit.WonAmt_50000)));
                sb.Append(POSPrinterUtils.DepositList("03:" + ConfigData.Current.SysMessage.GetMessage("00295"), TypeHelper.ToInt64(basketMiddleDeposit.WonCnt_10000), TypeHelper.ToInt64(basketMiddleDeposit.WonAmt_10000)));
                sb.Append(POSPrinterUtils.DepositList("04:" + ConfigData.Current.SysMessage.GetMessage("00296"), TypeHelper.ToInt64(basketMiddleDeposit.WonCnt_5000), TypeHelper.ToInt64(basketMiddleDeposit.WonAmt_5000)));
                sb.Append(POSPrinterUtils.DepositList("05:" + ConfigData.Current.SysMessage.GetMessage("00297"), TypeHelper.ToInt64(basketMiddleDeposit.WonCnt_1000), TypeHelper.ToInt64(basketMiddleDeposit.WonAmt_1000)));
                sb.Append(POSPrinterUtils.DepositList("06:" + ConfigData.Current.SysMessage.GetMessage("00298"), TypeHelper.ToInt64(basketMiddleDeposit.WonCnt_500), TypeHelper.ToInt64(basketMiddleDeposit.WonAmt_500)));
                sb.Append(POSPrinterUtils.DepositList("07:" + ConfigData.Current.SysMessage.GetMessage("00299"), TypeHelper.ToInt64(basketMiddleDeposit.WonCnt_100), TypeHelper.ToInt64(basketMiddleDeposit.WonAmt_100)));
                sb.Append(POSPrinterUtils.DepositList("08:" + ConfigData.Current.SysMessage.GetMessage("00300"), TypeHelper.ToInt64(basketMiddleDeposit.WonCnt_50), TypeHelper.ToInt64(basketMiddleDeposit.WonAmt_50)));
                sb.Append(POSPrinterUtils.DepositList("09:" + ConfigData.Current.SysMessage.GetMessage("00301"), TypeHelper.ToInt64(basketMiddleDeposit.WonCnt_10), TypeHelper.ToInt64(basketMiddleDeposit.WonAmt_10)));

                //상품권
                sb.Append(FXConsts.RECEIPT_DEPOSIT_TICKET_TEXT);
                sb.Append(Environment.NewLine);
                sb.Append(POSPrinterUtils.DepositList("31:" + ConfigData.Current.SysMessage.GetMessage("00302"),
                    TypeHelper.ToInt64(basketMiddleDeposit.TicketTotalCnt),
                    TypeHelper.ToInt64(basketMiddleDeposit.TicketTotalAmt)));

                // Loc add 12.08 할인쿠폰
                sb.Append(POSPrinterUtils.DepositList("32:할인쿠폰",
                    TypeHelper.ToInt64(basketMiddleDeposit.DiscCouponCnt),
                    TypeHelper.ToInt64(basketMiddleDeposit.DiscCouponAmt)));


                if (TypeHelper.ToString(basketMiddleDeposit.OtherCompanyTicketNm_01).Length > 0)
                {
                    sb.Append(POSPrinterUtils.DepositList("33:" + basketMiddleDeposit.OtherCompanyTicketNm_01, TypeHelper.ToInt64(basketMiddleDeposit.OtherCompanyTicketCnt_01), TypeHelper.ToInt64(basketMiddleDeposit.OtherCompanyTicketAmt_01)));
                }
                if (TypeHelper.ToString(basketMiddleDeposit.OtherCompanyTicketNm_02).Length > 0)
                {
                    sb.Append(POSPrinterUtils.DepositList(basketMiddleDeposit.OtherCompanyTicketNm_02, TypeHelper.ToInt64(basketMiddleDeposit.OtherCompanyTicketCnt_02), TypeHelper.ToInt64(basketMiddleDeposit.OtherCompanyTicketAmt_02)));
                }
                if (TypeHelper.ToString(basketMiddleDeposit.OtherCompanyTicketNm_03).Length > 0)
                {
                    sb.Append(POSPrinterUtils.DepositList(basketMiddleDeposit.OtherCompanyTicketNm_03, TypeHelper.ToInt64(basketMiddleDeposit.OtherCompanyTicketCnt_03), TypeHelper.ToInt64(basketMiddleDeposit.OtherCompanyTicketAmt_03)));
                }
                if (TypeHelper.ToString(basketMiddleDeposit.OtherCompanyTicketNm_04).Length > 0)
                {
                    sb.Append(POSPrinterUtils.DepositList(basketMiddleDeposit.OtherCompanyTicketNm_04, TypeHelper.ToInt64(basketMiddleDeposit.OtherCompanyTicketCnt_04), TypeHelper.ToInt64(basketMiddleDeposit.OtherCompanyTicketAmt_04)));
                }
                if (TypeHelper.ToString(basketMiddleDeposit.OtherCompanyTicketNm_05).Length > 0)
                {
                    sb.Append(POSPrinterUtils.DepositList(basketMiddleDeposit.OtherCompanyTicketNm_05, TypeHelper.ToInt64(basketMiddleDeposit.OtherCompanyTicketCnt_05), TypeHelper.ToInt64(basketMiddleDeposit.OtherCompanyTicketAmt_05)));
                }
                if (TypeHelper.ToString(basketMiddleDeposit.OtherCompanyTicketNm_06).Length > 0)
                {
                    sb.Append(POSPrinterUtils.DepositList(basketMiddleDeposit.OtherCompanyTicketNm_06, TypeHelper.ToInt64(basketMiddleDeposit.OtherCompanyTicketCnt_06), TypeHelper.ToInt64(basketMiddleDeposit.OtherCompanyTicketAmt_06)));
                }
                if (TypeHelper.ToString(basketMiddleDeposit.OtherCompanyTicketNm_07).Length > 0)
                {
                    sb.Append(POSPrinterUtils.DepositList(basketMiddleDeposit.OtherCompanyTicketNm_07, TypeHelper.ToInt64(basketMiddleDeposit.OtherCompanyTicketCnt_07), TypeHelper.ToInt64(basketMiddleDeposit.OtherCompanyTicketAmt_07)));
                }
                if (TypeHelper.ToString(basketMiddleDeposit.OtherCompanyTicketNm_08).Length > 0)
                {
                    sb.Append(POSPrinterUtils.DepositList(basketMiddleDeposit.OtherCompanyTicketNm_08,
                        TypeHelper.ToInt64(basketMiddleDeposit.OtherCompanyTicketCnt_08),
                        TypeHelper.ToInt64(basketMiddleDeposit.OtherCompanyTicketAmt_08)));
                }

                string CashTotalAmt = string.Format("{0:#,##0}", TypeHelper.ToInt64(basketMiddleDeposit.CashTotalAmt));
                string TicketTotalAmt = string.Format("{0:#,##0}",
                    TypeHelper.ToInt64(basketMiddleDeposit.TicketAmt) +
                    TypeHelper.ToInt64(basketMiddleDeposit.DiscCouponAmt) +
                    TypeHelper.ToInt64(basketMiddleDeposit.OtherCompanyTicketTotalAmt));
                string TotalAmt = string.Format("{0:#,##0}",
                    TypeHelper.ToInt64(basketMiddleDeposit.CashTotalAmt) +
                    TypeHelper.ToInt64(basketMiddleDeposit.TicketAmt) +
                    TypeHelper.ToInt64(basketMiddleDeposit.DiscCouponAmt) +
                    TypeHelper.ToInt64(basketMiddleDeposit.OtherCompanyTicketTotalAmt)); ;

                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(FXConsts.PRINT_LINEFEED);
                sb.Append(Environment.NewLine);

                //현금합계
                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, FXConsts.RECEIPT_DEPOSIT_CASHTOTAL, 12));
                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, CashTotalAmt, 28));
                sb.Append(Environment.NewLine);

                //상품권합계
                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, FXConsts.RECEIPT_DEPOSIT_GIFTTOTAL, 12));
                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, TicketTotalAmt, 28));
                sb.Append(Environment.NewLine);

                //총합계
                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_NORMAL);
                sb.Append(getFixCut(bPrint, true, PrintTypes.WideNormal, FXConsts.RECEIPT_DEPOSIT_TOTALAMT, 12));
                sb.Append(!bPrint ? "" : FXConsts.PRINT_BOLDWIDE_DOUBLE);
                sb.Append(getFixCut(bPrint, false, PrintTypes.WideDouble, TotalAmt, 28));
                sb.Append(FXConsts.PRINT_LAST);
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }
            finally
            {
                Print(bPrint, strGubun, true, sb.ToString(), string.Empty, false);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 중간입금, 마감입금
        /// </summary>
        /// <param name="name">명</param>
        /// <param name="count">회차</param>
        /// <param name="amt">금액</param>
        /// <returns></returns>
        public static string DepositList(string name, Int64 count, Int64 amt)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Append(getFixCut(true, PrintTypes.WideNormal, string.Format(" {0}{1}", name.Substring(0, 2), name.Substring(2, name.Length - 2)), 20));
                sb.Append(getFixCut(false, PrintTypes.WideNormal, string.Format(FXConsts.RECEIPT_DEPOSIT_TEXT2, count), 5));
                sb.Append(getFixCut(false, PrintTypes.WideNormal, string.Format("{0:#,##0}", amt), 15));
                sb.Append(Environment.NewLine);
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }

            return sb.ToString();
        }

        #endregion

        #region 품번별 매출조회

        /// <summary>
        /// 품번별 매출조회
        /// </summary>
        /// <param name="bPrint">true:프린트 출력, false:화면 출력</param>
        /// <param name="dt">매출 내역</param>
        /// <param name="totalCnt">총건수</param>
        /// <param name="totalAmt">총금액</param>
        /// <returns></returns>
        public string PrintIQ_P001(bool bPrint, DataTable dt, Int64 totalCnt, Int64 totalAmt)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Append(ReceiptBaseTitle(bPrint, null, ""));

                sb.Append(PrinterUtils.SelectProdNoHead(bPrint ? "0" : ""));

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i = i + 3)
                    {
                        DataRow dr_01 = dt.Rows[i];
                        DataRow dr_02 = dt.Rows[i + 1];
                        DataRow dr_03 = dt.Rows[i + 2];

                        sb.Append(
                            PrinterUtils.SelectProdNoBody(bPrint ? "0" : "",
                            dr_01[2].ToString(),
                            dr_01[3].ToString(),
                            TypeHelper.ToInt64(dr_02[3].ToString()),
                            TypeHelper.ToInt64(dr_02[4].ToString()),
                            TypeHelper.ToInt64(dr_03[3].ToString()),
                            TypeHelper.ToInt64(dr_03[4].ToString())));
                    }

                    sb.Append(PrinterUtils.SelectProdNoTotal(bPrint ? "0" : "", totalCnt, totalAmt));
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }
            finally
            {
                Print(bPrint, FXConsts.RECEIPT_NAME_POS_IQ_P001, true, sb.ToString(), string.Empty, false);
            }

            return sb.ToString();
        }

        #endregion

        #region 카드사별 매출조회

        /// <summary>
        /// 카드사별 매출조회
        /// </summary>
        /// <param name="bPrint">true:프린트 출력, false:화면 출력</param>
        /// <param name="drCnt">건수</param>
        /// <param name="drAmt">금액</param>
        /// <param name="totalCntSale">총판매건수</param>
        /// <param name="totalCntRetn">총반품건수</param>
        /// <param name="totalAmtRenr">총판매금액</param>
        /// <param name="totalAmtSale">총반품금액</param>
        /// <returns></returns>
        public string PrintIQ_P002(bool bPrint, DataRow[] drCnt, DataRow[] drAmt, Int64 totalCntSale, Int64 totalCntRetn, Int64 totalAmtRenr, Int64 totalAmtSale)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Append(ReceiptBaseTitle(bPrint, null, ""));

                //타이틀
                sb.Append(PrinterUtils.SelectProdNoCardTitle(bPrint ? "0" : ""));

                //매수 타이틀
                sb.Append(PrinterUtils.SelectProdNoCardHead(bPrint ? "0" : "", "cnt"));

                //매수 내용
                if (drCnt != null && drCnt.Length > 0)
                {
                    foreach (DataRow dr in drCnt)
                    {
                        sb.Append(PrinterUtils.SelectProdNoCardBody(bPrint ? "0" : "", "cnt", dr[1].ToString(), dr[2].ToString(), TypeHelper.ToInt64(dr[3]), TypeHelper.ToInt64(dr[4])));
                    }
                }

                //매수 합계
                sb.Append(PrinterUtils.SelectProdNoCardCntTotal(bPrint ? "0" : "", totalCntSale, totalCntRetn));

                //금액 타이틀
                sb.Append(PrinterUtils.SelectProdNoCardHead(bPrint ? "0" : "", "amt"));

                //금액 내용
                if (drAmt != null && drAmt.Length > 0)
                {
                    foreach (DataRow dr in drAmt)
                    {
                        sb.Append(PrinterUtils.SelectProdNoCardBody(bPrint ? "0" : "", "amt", dr[1].ToString(), dr[2].ToString(), TypeHelper.ToInt64(dr[3]), TypeHelper.ToInt64(dr[4])));
                    }
                }

                //금액 합계
                sb.Append(PrinterUtils.SelectProdNoCardAmtTotal(bPrint ? "0" : "", totalAmtRenr, totalAmtSale));
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }
            finally
            {
                Print(bPrint, FXConsts.RECEIPT_NAME_POS_IQ_P002, true, sb.ToString(), string.Empty, false);
            }

            return sb.ToString();
        }

        #endregion

        #region 합계점검조회, 계산원정산, POS정산

        /// <summary>
        /// 합계점검조회, 계산원정산, POS정산
        /// </summary>
        /// <param name="bPrint">true : 프린트 출력, false : 화면 출력</param>
        /// <param name="strType">합계점검조회, 계산원정산, POS정산 구분</param>
        /// <param name="basketHeader">헤더정보</param>
        /// <param name="basketAccount">마감정보</param>
        /// <param name="printOpt">0: print journal only; 1: print only; 2: journal & print</param>
        /// <returns></returns>
        public string SetPrintAccount(bool bPrint, string strType, BasketHeader basketHeader, BasketAccount basketAccount,
            DataTable dtAccount, int printOpt)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                int TotalCol = 90;
                int iCol = 0;

                //기본 타이틀
                sb.Append(ReceiptBaseTitle(bPrint, basketHeader, string.Empty));

                DataTable dt = new DataTable();

                #region 데이터 만들기

                if (dtAccount != null)
                {
                    dt = dtAccount.Copy();
                }
                else
                {
                    for (int i = 1; i <= TotalCol; i++)
                    {
                        dt.Columns.Add(string.Format("colCnt_{0}", i.ToString()));
                        dt.Columns.Add(string.Format("colAmt_{0}", i.ToString()));
                    }

                    dt.Rows.Add();
                    DataRow drPrint = dt.Rows[0];

                    #region 값 대입

                    #region 중간기타
                    drPrint[iCol] =
                        TypeHelper.ToInt64(basketAccount.AccountCnt_E21) +
                        TypeHelper.ToInt64(basketAccount.AccountCnt_E22) +
                        TypeHelper.ToInt64(basketAccount.AccountCnt_E23) +
                        TypeHelper.ToInt64(basketAccount.AccountCnt_E24) +
                        TypeHelper.ToInt64(basketAccount.AccountCnt_E25);
                    iCol++;
                    drPrint[iCol] =
                        TypeHelper.ToInt64(basketAccount.AccountAmt_E21) +
                        TypeHelper.ToInt64(basketAccount.AccountAmt_E22) +
                        TypeHelper.ToInt64(basketAccount.AccountAmt_E23) +
                        TypeHelper.ToInt64(basketAccount.AccountAmt_E24) +
                        TypeHelper.ToInt64(basketAccount.AccountAmt_E25);
                    iCol++;
                    #endregion

                    #region 마감기타
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_E30);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_E30);
                    iCol++;
                    #endregion

                    #region 지정정정
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_A05);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_A05);
                    iCol++;
                    #endregion

                    #region 거래중지
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_A06);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_A06);
                    iCol++;
                    #endregion

                    #region 현금반품
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_B01) + TypeHelper.ToInt64(basketAccount.AccountCnt_F03);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_B01) + TypeHelper.ToInt64(basketAccount.AccountAmt_F03);
                    iCol++;
                    #endregion

                    #region 대체반품
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_C01) +
                        TypeHelper.ToInt64(basketAccount.AccountCnt_C03) +
                        TypeHelper.ToInt64(basketAccount.AccountCnt_C05) +
                        TypeHelper.ToInt64(basketAccount.AccountCnt_C07) +
                        TypeHelper.ToInt64(basketAccount.AccountCnt_C09) +
                        TypeHelper.ToInt64(basketAccount.AccountCnt_C11) +
                        TypeHelper.ToInt64(basketAccount.AccountCnt_C13) +
                        TypeHelper.ToInt64(basketAccount.AccountCnt_C15) +
                        TypeHelper.ToInt64(basketAccount.AccountCnt_C17) +
                        TypeHelper.ToInt64(basketAccount.AccountCnt_C19) +
                        TypeHelper.ToInt64(basketAccount.AccountCnt_C21) +
                        TypeHelper.ToInt64(basketAccount.AccountCnt_C23) +
                        TypeHelper.ToInt64(basketAccount.AccountCnt_F05);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_C01) +
                        TypeHelper.ToInt64(basketAccount.AccountAmt_C03) +
                        TypeHelper.ToInt64(basketAccount.AccountAmt_C05) +
                        TypeHelper.ToInt64(basketAccount.AccountAmt_C07) +
                        TypeHelper.ToInt64(basketAccount.AccountAmt_C09) +
                        TypeHelper.ToInt64(basketAccount.AccountAmt_C11) +
                        TypeHelper.ToInt64(basketAccount.AccountAmt_C13) +
                        TypeHelper.ToInt64(basketAccount.AccountAmt_C15) +
                        TypeHelper.ToInt64(basketAccount.AccountAmt_C17) +
                        TypeHelper.ToInt64(basketAccount.AccountAmt_C19) +
                        TypeHelper.ToInt64(basketAccount.AccountAmt_C21) +
                        TypeHelper.ToInt64(basketAccount.AccountAmt_C23) +
                        TypeHelper.ToInt64(basketAccount.AccountAmt_F05);
                    iCol++;
                    #endregion

                    #region 총판매액
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_A01) - TypeHelper.ToInt64(basketAccount.AccountCnt_A02) + TypeHelper.ToInt64(basketAccount.AccountCnt_F00) - TypeHelper.ToInt64(basketAccount.AccountCnt_F01);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_A01) - TypeHelper.ToInt64(basketAccount.AccountAmt_A02) + TypeHelper.ToInt64(basketAccount.AccountAmt_F00) - TypeHelper.ToInt64(basketAccount.AccountAmt_F01);
                    iCol++;
                    #endregion

                    #region 총매출액
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_A01) - TypeHelper.ToInt64(basketAccount.AccountCnt_A02) + TypeHelper.ToInt64(basketAccount.AccountCnt_F00) - TypeHelper.ToInt64(basketAccount.AccountCnt_F01);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_A01) - TypeHelper.ToInt64(basketAccount.AccountAmt_A02) + TypeHelper.ToInt64(basketAccount.AccountAmt_F00) - TypeHelper.ToInt64(basketAccount.AccountAmt_F01);
                    iCol++;
                    #endregion

                    #region 에누리
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_A04);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_A04);
                    iCol++;
                    #endregion

                    #region 순매출액
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_A01) - TypeHelper.ToInt64(basketAccount.AccountCnt_A02) + TypeHelper.ToInt64(basketAccount.AccountCnt_F00) - TypeHelper.ToInt64(basketAccount.AccountCnt_F01) -
                        TypeHelper.ToInt64(basketAccount.AccountCnt_A03) - TypeHelper.ToInt64(basketAccount.AccountCnt_A04);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_A01) - TypeHelper.ToInt64(basketAccount.AccountAmt_A02) + TypeHelper.ToInt64(basketAccount.AccountAmt_F00) - TypeHelper.ToInt64(basketAccount.AccountAmt_F01) -
                        TypeHelper.ToInt64(basketAccount.AccountAmt_A03) - TypeHelper.ToInt64(basketAccount.AccountAmt_A04);
                    iCol++;
                    #endregion

                    //대체 매출 현황
                    #region 타사카드
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_C00) - TypeHelper.ToInt64(basketAccount.AccountCnt_C01) + TypeHelper.ToInt64(basketAccount.AccountCnt_F04) - TypeHelper.ToInt64(basketAccount.AccountCnt_F05);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_C00) - TypeHelper.ToInt64(basketAccount.AccountAmt_C01) + TypeHelper.ToInt64(basketAccount.AccountAmt_F04) - TypeHelper.ToInt64(basketAccount.AccountAmt_F05);
                    iCol++;
                    #endregion

                    #region 타건카드
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_C02) - TypeHelper.ToInt64(basketAccount.AccountCnt_C03);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_C02) - TypeHelper.ToInt64(basketAccount.AccountAmt_C03);
                    iCol++;
                    #endregion

                    #region 자사복지
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_C04) - TypeHelper.ToInt64(basketAccount.AccountCnt_C05);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_C04) - TypeHelper.ToInt64(basketAccount.AccountAmt_C05);
                    iCol++;
                    #endregion

                    #region 타건복지
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_C06) - TypeHelper.ToInt64(basketAccount.AccountCnt_C07);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_C06) - TypeHelper.ToInt64(basketAccount.AccountAmt_C07);
                    iCol++;
                    #endregion

                    #region 타사상품
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_C08) - TypeHelper.ToInt64(basketAccount.AccountCnt_C09);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_C08) - TypeHelper.ToInt64(basketAccount.AccountAmt_C09);
                    iCol++;
                    #endregion

                    #region 상품교환
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_C10) -
                        TypeHelper.ToInt64(basketAccount.AccountCnt_C11);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_C10) -
                        TypeHelper.ToInt64(basketAccount.AccountAmt_C11);
                    iCol++;
                    #endregion

                    #region 구상품교환
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_C12) - TypeHelper.ToInt64(basketAccount.AccountCnt_C13);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_C12) - TypeHelper.ToInt64(basketAccount.AccountAmt_C13);
                    iCol++;
                    #endregion

                    #region 포인트
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_C14) - TypeHelper.ToInt64(basketAccount.AccountCnt_C15);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_C14) - TypeHelper.ToInt64(basketAccount.AccountAmt_C15);
                    iCol++;
                    #endregion

                    #region 할인쿠폰
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_C16) - TypeHelper.ToInt64(basketAccount.AccountCnt_C17);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_C16) - TypeHelper.ToInt64(basketAccount.AccountAmt_C17);
                    iCol++;
                    #endregion

                    #region 결제할인
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_C18) - TypeHelper.ToInt64(basketAccount.AccountCnt_C19);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_C18) - TypeHelper.ToInt64(basketAccount.AccountAmt_C19);
                    iCol++;
                    #endregion

                    #region 현금IC
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_C20) - TypeHelper.ToInt64(basketAccount.AccountCnt_C21);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_C20) - TypeHelper.ToInt64(basketAccount.AccountAmt_C21);
                    iCol++;
                    #endregion

                    #region 외상대
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_C22) - TypeHelper.ToInt64(basketAccount.AccountCnt_C23);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_C22) - TypeHelper.ToInt64(basketAccount.AccountAmt_C23);
                    iCol++;
                    #endregion

                    #region 대체소계
                    drPrint[iCol] =
                        (TypeHelper.ToInt64(basketAccount.AccountCnt_C00) - TypeHelper.ToInt64(basketAccount.AccountCnt_C01)) +
                        (TypeHelper.ToInt64(basketAccount.AccountCnt_C02) - TypeHelper.ToInt64(basketAccount.AccountCnt_C03)) +
                        (TypeHelper.ToInt64(basketAccount.AccountCnt_C04) - TypeHelper.ToInt64(basketAccount.AccountCnt_C05)) +
                        (TypeHelper.ToInt64(basketAccount.AccountCnt_C06) - TypeHelper.ToInt64(basketAccount.AccountCnt_C07)) +
                        (TypeHelper.ToInt64(basketAccount.AccountCnt_C08) - TypeHelper.ToInt64(basketAccount.AccountCnt_C09)) +
                        (TypeHelper.ToInt64(basketAccount.AccountCnt_C10) - TypeHelper.ToInt64(basketAccount.AccountCnt_C11)) +
                        (TypeHelper.ToInt64(basketAccount.AccountCnt_C12) - TypeHelper.ToInt64(basketAccount.AccountCnt_C13)) +
                        (TypeHelper.ToInt64(basketAccount.AccountCnt_C14) - TypeHelper.ToInt64(basketAccount.AccountCnt_C15)) +
                        (TypeHelper.ToInt64(basketAccount.AccountCnt_C16) - TypeHelper.ToInt64(basketAccount.AccountCnt_C17)) +
                        (TypeHelper.ToInt64(basketAccount.AccountCnt_C18) - TypeHelper.ToInt64(basketAccount.AccountCnt_C19)) +
                        (TypeHelper.ToInt64(basketAccount.AccountCnt_C20) - TypeHelper.ToInt64(basketAccount.AccountCnt_C21)) +
                        (TypeHelper.ToInt64(basketAccount.AccountCnt_C22) - TypeHelper.ToInt64(basketAccount.AccountCnt_C23)) +
                        (TypeHelper.ToInt64(basketAccount.AccountCnt_F04) - TypeHelper.ToInt64(basketAccount.AccountCnt_F05));
                    iCol++;
                    drPrint[iCol] =
                        (TypeHelper.ToInt64(basketAccount.AccountAmt_C00) - TypeHelper.ToInt64(basketAccount.AccountAmt_C01)) +
                        (TypeHelper.ToInt64(basketAccount.AccountAmt_C02) - TypeHelper.ToInt64(basketAccount.AccountAmt_C03)) +
                        (TypeHelper.ToInt64(basketAccount.AccountAmt_C04) - TypeHelper.ToInt64(basketAccount.AccountAmt_C05)) +
                        (TypeHelper.ToInt64(basketAccount.AccountAmt_C06) - TypeHelper.ToInt64(basketAccount.AccountAmt_C07)) +
                        (TypeHelper.ToInt64(basketAccount.AccountAmt_C08) - TypeHelper.ToInt64(basketAccount.AccountAmt_C09)) +
                        (TypeHelper.ToInt64(basketAccount.AccountAmt_C10) - TypeHelper.ToInt64(basketAccount.AccountAmt_C11)) +
                        (TypeHelper.ToInt64(basketAccount.AccountAmt_C12) - TypeHelper.ToInt64(basketAccount.AccountAmt_C13)) +
                        (TypeHelper.ToInt64(basketAccount.AccountAmt_C14) - TypeHelper.ToInt64(basketAccount.AccountAmt_C15)) +
                        (TypeHelper.ToInt64(basketAccount.AccountAmt_C16) - TypeHelper.ToInt64(basketAccount.AccountAmt_C17)) +
                        (TypeHelper.ToInt64(basketAccount.AccountAmt_C18) - TypeHelper.ToInt64(basketAccount.AccountAmt_C19)) +
                        (TypeHelper.ToInt64(basketAccount.AccountAmt_C20) - TypeHelper.ToInt64(basketAccount.AccountAmt_C21)) +
                        (TypeHelper.ToInt64(basketAccount.AccountAmt_C22) - TypeHelper.ToInt64(basketAccount.AccountAmt_C23)) +
                        (TypeHelper.ToInt64(basketAccount.AccountAmt_F04) - TypeHelper.ToInt64(basketAccount.AccountAmt_F05));
                    iCol++;
                    #endregion

                    //상품권 환불 현황
                    #region 타상환불
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_E40);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_E40);
                    iCol++;
                    #endregion

                    #region 환불소계
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_E40);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_E40);
                    iCol++;
                    #endregion

                    //상품교환권 현황
                    #region 정상판매
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_C10) + TypeHelper.ToInt64(basketAccount.AccountCnt_C12);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_C10) + TypeHelper.ToInt64(basketAccount.AccountAmt_C12);
                    iCol++;
                    #endregion

                    #region 사은품 반납
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_G02);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_G02);
                    iCol++;
                    #endregion

                    #region 회수합계
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_C10) + TypeHelper.ToInt64(basketAccount.AccountCnt_C12) + TypeHelper.ToInt64(basketAccount.AccountCnt_G02);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_C10) + TypeHelper.ToInt64(basketAccount.AccountAmt_C12) + TypeHelper.ToInt64(basketAccount.AccountAmt_G02);
                    iCol++;
                    #endregion

                    #region 반품교환

                    // Loc changed 12.07
                    // (1) “29 반품교환”  타이틀 “29 재 증 정” 으로 변경
                    // (2) 건수와 금액은 신상품 교환권의 반품 건수와 금액만 누적한 금액 출력
                    // (3) 영수증 출력,  화면 같이 수정 확인 바랍니다.
                    // (4) 포스 마감 부분과 계산원 마감 부분 같이 확인 바랍니다. (매뉴가 나눠져 있어 같이 확인 해야 될겁니다.)
                    // 현재는 정산 항목 C11 + C13 계산으로 되어 있는데 
                    // C11 항목만 반영 하면 됩니다.
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_C11);// + TypeHelper.ToInt64(basketAccount.AccountCnt_C13);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_C11);// +TypeHelper.ToInt64(basketAccount.AccountAmt_C13);
                    iCol++;
                    #endregion

                    //입출금 현황
                    #region 현금매출
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_B00) - TypeHelper.ToInt64(basketAccount.AccountCnt_B01) + TypeHelper.ToInt64(basketAccount.AccountCnt_F02) - TypeHelper.ToInt64(basketAccount.AccountCnt_F03);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_B00) - TypeHelper.ToInt64(basketAccount.AccountAmt_B01) + TypeHelper.ToInt64(basketAccount.AccountAmt_F02) - TypeHelper.ToInt64(basketAccount.AccountAmt_F03);
                    iCol++;
                    #endregion

                    #region 준비금
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_D00);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_D00);
                    iCol++;
                    #endregion

                    #region 현물변제
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_G01);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_G01);
                    iCol++;
                    #endregion

                    #region 상품교환권 변제
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_G03);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_G03);
                    iCol++;
                    #endregion

                    #region 입금계
                    drPrint[iCol] =
                        (TypeHelper.ToInt64(basketAccount.AccountCnt_B00) - TypeHelper.ToInt64(basketAccount.AccountCnt_B01)) +
                        (TypeHelper.ToInt64(basketAccount.AccountCnt_D00) + TypeHelper.ToInt64(basketAccount.AccountCnt_G01) + TypeHelper.ToInt64(basketAccount.AccountCnt_G03)) +
                        (TypeHelper.ToInt64(basketAccount.AccountCnt_F02) - TypeHelper.ToInt64(basketAccount.AccountCnt_F03));
                    iCol++;
                    drPrint[iCol] =
                        (TypeHelper.ToInt64(basketAccount.AccountAmt_B00) - TypeHelper.ToInt64(basketAccount.AccountAmt_B01)) +
                        (TypeHelper.ToInt64(basketAccount.AccountAmt_D00) + TypeHelper.ToInt64(basketAccount.AccountAmt_G01) + TypeHelper.ToInt64(basketAccount.AccountAmt_G03)) +
                        (TypeHelper.ToInt64(basketAccount.AccountAmt_F02) - TypeHelper.ToInt64(basketAccount.AccountAmt_F03));
                    iCol++;
                    #endregion

                    #region 중간입금1회
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_E01);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_E01);
                    iCol++;
                    #endregion

                    #region 중간입금2회
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_E02);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_E02);
                    iCol++;
                    #endregion

                    #region 중간입금3회
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_E03);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_E03);
                    iCol++;
                    #endregion

                    #region 중간입금4회
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_E04);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_E04);
                    iCol++;
                    #endregion

                    #region 중간입금5회
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_E05);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_E05);
                    iCol++;
                    #endregion

                    #region 중간입금 계
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_E00);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_E01) + TypeHelper.ToInt64(basketAccount.AccountAmt_E02) + TypeHelper.ToInt64(basketAccount.AccountAmt_E03) + TypeHelper.ToInt64(basketAccount.AccountAmt_E04) + TypeHelper.ToInt64(basketAccount.AccountAmt_E05);
                    iCol++;
                    #endregion

                    #region 출금계
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_E00);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_E01) + TypeHelper.ToInt64(basketAccount.AccountAmt_E02) + TypeHelper.ToInt64(basketAccount.AccountAmt_E03) + TypeHelper.ToInt64(basketAccount.AccountAmt_E04) + TypeHelper.ToInt64(basketAccount.AccountAmt_E05);
                    iCol++;
                    #endregion

                    #region 현금잔고
                    drPrint[iCol] =
                        (
                            (TypeHelper.ToInt64(basketAccount.AccountCnt_B00) - TypeHelper.ToInt64(basketAccount.AccountCnt_B01)) +
                            (TypeHelper.ToInt64(basketAccount.AccountCnt_D00) + TypeHelper.ToInt64(basketAccount.AccountCnt_G01) + TypeHelper.ToInt64(basketAccount.AccountCnt_G03)) +
                            (TypeHelper.ToInt64(basketAccount.AccountCnt_F02) - TypeHelper.ToInt64(basketAccount.AccountCnt_F03))
                        ) - TypeHelper.ToInt64(basketAccount.AccountCnt_E00) - TypeHelper.ToInt64(basketAccount.AccountCnt_E40);
                    iCol++;
                    drPrint[iCol] =
                        (
                            (TypeHelper.ToInt64(basketAccount.AccountAmt_B00) - TypeHelper.ToInt64(basketAccount.AccountAmt_B01)) +
                            (TypeHelper.ToInt64(basketAccount.AccountAmt_D00) + TypeHelper.ToInt64(basketAccount.AccountAmt_G01) + TypeHelper.ToInt64(basketAccount.AccountAmt_G03)) +
                            (TypeHelper.ToInt64(basketAccount.AccountAmt_F02) - TypeHelper.ToInt64(basketAccount.AccountAmt_F03))
                        ) -
                        (
                            TypeHelper.ToInt64(basketAccount.AccountAmt_E01) +
                            TypeHelper.ToInt64(basketAccount.AccountAmt_E02) +
                            TypeHelper.ToInt64(basketAccount.AccountAmt_E03) +
                            TypeHelper.ToInt64(basketAccount.AccountAmt_E04) +
                            TypeHelper.ToInt64(basketAccount.AccountAmt_E05)
                        ) - TypeHelper.ToInt64(basketAccount.AccountAmt_E40)
                        ;
                    iCol++;
                    #endregion

                    #region 보관
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_D00);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_D00);
                    iCol++;
                    #endregion

                    #region 마감입금
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_E10);
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_E10);
                    iCol++;
                    #endregion

                    #region 과부족
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountCnt_E10) -
                        (
                            (
                                (
                                    (TypeHelper.ToInt64(basketAccount.AccountCnt_B00) - TypeHelper.ToInt64(basketAccount.AccountCnt_B01)) +
                                    (TypeHelper.ToInt64(basketAccount.AccountCnt_D00) + TypeHelper.ToInt64(basketAccount.AccountCnt_G01) + TypeHelper.ToInt64(basketAccount.AccountCnt_G03)) +
                                    (TypeHelper.ToInt64(basketAccount.AccountCnt_F02) - TypeHelper.ToInt64(basketAccount.AccountCnt_F03))
                                ) - TypeHelper.ToInt64(basketAccount.AccountCnt_E00) - TypeHelper.ToInt64(basketAccount.AccountCnt_E40)
                            )
                            -
                            (
                                TypeHelper.ToInt64(basketAccount.AccountCnt_D00)
                            )
                        );
                    iCol++;
                    drPrint[iCol] = TypeHelper.ToInt64(basketAccount.AccountAmt_E10) -
                        (
                            (

                                (
                                (TypeHelper.ToInt64(basketAccount.AccountAmt_B00) - TypeHelper.ToInt64(basketAccount.AccountAmt_B01)) +
                                (TypeHelper.ToInt64(basketAccount.AccountAmt_D00) + TypeHelper.ToInt64(basketAccount.AccountAmt_G01) + TypeHelper.ToInt64(basketAccount.AccountAmt_G03)) +
                                (TypeHelper.ToInt64(basketAccount.AccountAmt_F02) - TypeHelper.ToInt64(basketAccount.AccountAmt_F03))
                                ) -
                                (
                                    TypeHelper.ToInt64(basketAccount.AccountAmt_E01) +
                                    TypeHelper.ToInt64(basketAccount.AccountAmt_E02) +
                                    TypeHelper.ToInt64(basketAccount.AccountAmt_E03) +
                                    TypeHelper.ToInt64(basketAccount.AccountAmt_E04) +
                                    TypeHelper.ToInt64(basketAccount.AccountAmt_E05)
                                ) - TypeHelper.ToInt64(basketAccount.AccountAmt_E40)
                            )
                            -
                            (
                                TypeHelper.ToInt64(basketAccount.AccountAmt_D00)
                            )
                        );
                    iCol++;
                    #endregion

                    #endregion
                }

                #endregion

                iCol = 1;
                for (int i = 0; i < TotalCol; i = i + 2)
                {
                    sb.Append(PrinterUtils.SelectTotalAccount(bPrint ? "0" : "2", basketHeader,
                        strType == FXConsts.RECEIPT_NAME_POS_ED_P001 ? "0" :
                        strType == FXConsts.RECEIPT_NAME_POS_ED_P002 ? "1" : "2",
                        iCol++, TypeHelper.ToInt64(dt.Rows[0][i]),
                        TypeHelper.ToInt64(dt.Rows[0][i + 1])));
                }
                sb.Append(FXConsts.PRINT_LAST);
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }
            finally
            {
                Print(bPrint, strType, true, sb.ToString(), string.Empty, false, printOpt);
            }

            return sb.ToString();
        }

        #endregion

        #region 포인트 조회(지류카드)

        /// <summary>
        /// 포인트 조회(지류카드)
        /// </summary>
        /// <param name="strCustNo">회원번호</param>
        /// <param name="strCustName">회원명</param>
        /// <returns></returns>
        public string PrintPT_P001_CARD(string strCustNo, string strCustName)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Append(ReceiptBaseTitle(true, null, ""));
                sb.Append(PrinterUtils.SelectCustCardBacode("0", strCustNo, strCustName));
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                sb = new StringBuilder();
            }
            finally
            {
                Print(true, FXConsts.RECEIPT_NAME_POS_PT_P001, true, sb.ToString(), string.Format("A{0}A", strCustNo), true);
            }

            return sb.ToString();
        }

        #endregion

        #region 사용자 정의

        #region 상품항목

        /// <summary>
        /// 상품항목 출력공통 함수
        /// </summary>
        /// <param name="nmClassName">품번명이나 상품명</param>
        /// <param name="nmItem">품목명, 단품인경우 EMPTY</param>
        /// <param name="cdClass">품번코드나 단품인경우 상품코드</param>
        /// <param name="cdItem">품번인경우 품목코드, 단품: EMPTY</param>
        /// <param name="fgClass">품번인경우 구분, 단품: EMPTY</param>
        /// <param name="cdDp">상품구분: 0:단품(PLU),1:NON_PLU,2:품번,3:외식상품,4:저장물</param>
        /// <param name="prcChange">가경변경 되었는지</param>
        /// <param name="fgTax">세구분</param>
        /// <param name="fgCanc">취소구분</param>
        /// <param name="qty"></param>
        /// <param name="price"></param>
        /// <param name="amt"></param>
        /// <returns></returns>
        public static string ReceiptSaleItem(string nmClassName,
            string nmItem, string cdClass, string cdItem,
            string fgClass, string cdDp, bool prcChange,
            string fgTax, string fgCanc,
            int qty, int price, int amt)
        {
            if (!"0".Equals(fgCanc) && !string.IsNullOrEmpty(fgCanc))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(prcChange ? "&" : " ");

            // 상품명 & 수량
            string itemName = nmItem;

            if (cdDp.Equals("2"))
            {
                if (nmClassName != null && nmClassName.Length > 0)
                {
                    if (nmItem.Length >= nmClassName.Length && nmItem.Substring(0, nmClassName.Length) == nmClassName)
                    {
                        itemName = nmItem;
                    }
                    else
                    {
                        itemName = string.Format("{0}{1}{2}", nmClassName, !string.IsNullOrEmpty(nmClassName) && !string.IsNullOrEmpty(nmItem) && !nmItem.Contains("_") ? "_" : "", nmItem);
                    }
                }
                else
                {
                    itemName = nmItem;
                }
            }

            sb.Append(getFixCut(true, PrintTypes.WideNormal, itemName, 33));
            sb.Append(getFixCut(false, PrintTypes.WideNormal, string.Format("{0:#,##0}", qty), 6));
            sb.Append(Environment.NewLine);

            //면세일경우 "*"
            sb.Append("2".Equals(fgTax) ? "*" : " ");

            string itemCode = cdDp == "2" ?
                string.Format("{0}{1}{2}{3}{4}", cdClass, cdItem, fgClass, string.IsNullOrEmpty(cdItem) ? "" : "0", cdDp == "2" ? "[M]" : (cdDp == "4" ? "[S]" : "[P]"))
                : string.Format("{0}{1}", cdItem, cdDp == "2" ? "[M]" : (cdDp == "4" ? "[S]" : "[P]"));

            sb.Append(getFixCut(true, PrintTypes.WideNormal, itemCode, 17));
            sb.Append(getFixCut(false, PrintTypes.WideNormal, string.Format("{0:#,##0}", price), 10));
            sb.Append(getFixCut(false, PrintTypes.WideNormal, string.Format("{0:#,##0}", amt), 12));
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }

        #endregion

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

        /// <summary>
        /// 좌우측 정렬
        /// </summary>
        /// <param name="bPrint">true : 프린트 출력, false : 화면 출력</param>
        /// <param name="bLeft">true : 좌측정렬, false : 우측정렬</param>
        /// <param name="type">프린트 타입</param>
        /// <param name="value">출력 값</param>
        /// <param name="len">출력 총자리수</param>
        public static string getFixCut(bool bPrint, bool bLeft, PrintTypes type, string value, int len)
        {
            string strReturn = value;
            int iData = 0;

            Regex strRegex = new Regex(@"[가-힣]");
            for (int i = 0; i < value.Length; i++)
            {
                if (strRegex.IsMatch(value.Substring(i, 1)))
                {
                    if (PrintTypes.WideNormal == type || !bPrint)
                    {
                        if (len >= iData + 2)
                        {
                            iData = iData + 2;
                            strReturn = value.Substring(0, i + 1);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (len >= iData + 3)
                        {
                            iData = iData + 3;
                            strReturn = value.Substring(0, i + 1);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    if (len >= iData + 1)
                    {
                        iData++;
                        strReturn = value.Substring(0, i + 1);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            iData = 0;

            strRegex = new Regex(@"[가-힣]");
            for (int i = 0; i < strReturn.Length; i++)
            {
                if (strRegex.IsMatch(strReturn.Substring(i, 1)))
                {
                    iData += PrintTypes.WideDouble != type || !bPrint ? 1 : 3;
                }
            }

            len = len - iData;

            if (PrintTypes.WideDouble != type || !bPrint)
            {
                strReturn = !bLeft ? strReturn.PadLeft(len, ' ') : strReturn.PadRight(len, ' ');
            }
            else
            {
                strReturn = !bLeft ? strReturn.PadLeft((len / 2), ' ') : strReturn.PadRight((len / 2), ' ');
            }

            return strReturn;
        }

        /// <summary>
        /// 좌우측 정렬
        /// </summary>
        /// <param name="bLeft">true : 좌측정렬, false : 우측정렬</param>
        /// <param name="type">프린트 타입</param>
        /// <param name="value">출력 값</param>
        /// <param name="len">출력 총자리수</param>
        public static string getFixCut(bool bLeft, PrintTypes type, string value, int len)
        {
            return getFixCut(true, bLeft, type, value, len);
        }

        #region 실제출력

        /// <summary>
        /// 실제 프린트 출력
        /// </summary>
        /// <param name="bPrint">true :  프린트 출력, false : 화면 출력</param>
        /// <param name="strUI">저널 입력명</param>
        /// <param name="bLogo">로그 출력여부</param>
        /// <param name="strDesc">프린트 내용</param>
        /// <param name="strBarCode">바코드</param>
        /// <param name="bLast">마지막 출력전 빈칸 띄우기</param>
        public void Print(bool bPrint, string strUI, bool bLogo, string strDesc, string strBarCode, bool bLast, int printOpt)
        {
            try
            {
                if (m_device != null && bPrint && strDesc.Length > 0)
                {
                    //출력 시작(저널 저장)
                    m_device.StartPrint(strUI, printOpt);

                    //로고이미지
                    if (bLogo)
                    {
                        m_device.PrintNormal(m_device.PrintLogo(), printOpt);
                    }

                    //출력 내용
                    m_device.PrintNormal(strDesc, printOpt);

                    // 여전법 추가 0808(KSK)
                    strUI = string.Empty;
                    strDesc = string.Empty;

                    //바코드 출력
                    if (strBarCode.Length > 0)
                    {
                        m_device.PrintNormal(Environment.NewLine, printOpt);
                        m_device.PrintBarCode(strBarCode, printOpt);
                    }

                    // DCCOffer 인경우, DCCFooter.bmp image 출력한다
                    // Loc added 11.13
                    // 김성근 PM, 이사님요구사항
                    // DCCOffer 마지막 줄에 이미지 인쇄 한다
                    // 이미지 없으면 무시
                    if (strUI.Equals(FXConsts.RECEIPT_PRINT_POS_DCC_OFFER_01))
                    {
                        string filePath = Path.Combine(FXConsts.FOLDER_RESOURCE.GetFolder(), FXConsts.PRINT_DCC_FOOTER_IMG);
                        if (File.Exists(filePath))
                        {
                            m_device.PrintBitmap(filePath);
                        }
                    }

                    //출력 컷팅 라인
                    if (bLast)
                    {
                        m_device.PrintNormal(FXConsts.PRINT_LAST, printOpt);
                    }

                    //출력물 종료
                    m_device.EndPrint(printOpt);

                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bPrint"></param>
        /// <param name="strUI"></param>
        /// <param name="bLogo"></param>
        /// <param name="strDesc"></param>
        /// <param name="strBarCode"></param>
        /// <param name="bLast"></param>
        public void Print(bool bPrint, string strUI, bool bLogo, string strDesc, string strBarCode, bool bLast)
        {
            Print(bPrint, strUI, bLogo, strDesc, strBarCode, bLast, 2);
        }

        #endregion

        #endregion
    }
}