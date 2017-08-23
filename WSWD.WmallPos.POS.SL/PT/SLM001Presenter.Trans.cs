using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.IO;

using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.SL.Data;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.BO.Trans;
using WSWD.WmallPos.POS.FX.Win.Devices;
using System.Windows.Forms;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PV;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PQ;
using WSWD.WmallPos.FX.NetComm.Tasks.PQ;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PQ;
using WSWD.WmallPos.POS.PY.Data;

namespace WSWD.WmallPos.POS.SL.PT
{
    /// <summary>
    /// M001 Presenter 처리
    /// TR 처리업무
    /// </summary>
    partial class SLM001Presenter
    {
        #region 공통

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if ok, false if all items are canceled, 전상품 취소됨</returns>
        bool MakeBasketItems()
        {
            this.BasketItems.Clear();
            int cancCount = 0;
            foreach (var item in m_saleView.DataRows)
            {
                var bi = item.ToBasketItem();
                this.BasketItems.Add(bi);

                if (string.IsNullOrEmpty(bi.FgCanc) || "0".Equals(bi.FgCanc))
                {
                    continue;
                }

                cancCount += !"0".Equals(bi.FgCanc) ? 1 : 0;
            }

            return cancCount != this.BasketItems.Count;
        }

        #endregion

        #region 보류

        /// <summary>
        /// 보류
        /// /// (2) 보류 데이터를 생성 한다.
        ///    - 보류 데이터 관리 Table(sat900t)에 보류 정보를 저장한다.
        ///    - 보류 영수증 번호는 해당 일자 Max(보류번호) + 1로 생성하여 처리한다.
        ///    - 저장시 발생시간, 총판매금액은 저장되는 Row에 동일하게 적용한다. 
        ///    (보류해지시 정보를 조회하기 편하게 하기 위해)
        ///    - VC_CONT는 바스켓 버퍼 내용을 기준으로 저장 한다.
        ///    - 보류 해지 구분(VC_CANCEL)은 'N'올 저장 한다.
        /// (3) 판매화면을 초기화 하고 보류건수를 증가해서 표시 한다.
        /// (4) 보류 영수증을 출력 한다.
        /// (5) 보류 발생 정보를 저널에 저장 한다.
        /// </summary>
        bool ProcessHold()
        {
            if (!MakeBasketItems())
                return false;

            bool transSaveOK = false;

            #region TRXN 저장

            string noBoru = string.Empty;
            string ddTime = string.Empty;
            string trxnNo = ConfigData.Current.AppConfig.PosInfo.TrxnNo;
            var db = TranDbHelper.InitInstance();
            var trans = db.BeginTransaction();
            try
            {
                string query = "M001HoldGetMaxNo".POSSLQuerySQL();
                var maxNo = db.ExecuteScalar(query, new string[] {
                    "@DD_SALE", "@CD_STORE", "@NO_POS"
                }, new object[] {
                    ConfigData.Current.AppConfig.PosInfo.SaleDate, 
                    ConfigData.Current.AppConfig.PosInfo.StoreNo, 
                    ConfigData.Current.AppConfig.PosInfo.PosNo
                }, trans);

                #region 보류 상품 등록

                noBoru = maxNo == null ? "0001" :
                                    string.Format("{0:d4}",
                                    TypeHelper.ToInt32(maxNo) + 1);
                ddTime = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);

                var biList = this.BasketItems.Where(p => "0".Equals(p.FgCanc)).ToArray();

                int sqBoru = 1;
                foreach (var bitem in biList)
                {
                    string insQuery = "M001HoldInsert".POSSLQuerySQL();
                    db.ExecuteNonQuery(insQuery,
                        new string[] {
                            "@DD_SALE", "@CD_STORE", "@NO_POS", "@NO_BORU", "@SQ_BORU", "@VC_CONT",
                            "@ID_USER", "@FG_CANCEL", "@DD_TIME", "@AM_SALE"
                        }, new object[] {
                            ConfigData.Current.AppConfig.PosInfo.SaleDate,
                            ConfigData.Current.AppConfig.PosInfo.StoreNo,
                            ConfigData.Current.AppConfig.PosInfo.PosNo,
                            noBoru,
                            sqBoru,
                            bitem.ToString(),
                            ConfigData.Current.AppConfig.PosInfo.CasNo,
                            "N", // 보류상태,
                            ddTime,
                            m_summaryData.TotalAmt
                        }, trans);

                    sqBoru++;

                }
                #endregion

                trans.Commit();
                transSaveOK = true;
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                trans.Dispose();
                db.Dispose();
            }
            #endregion

            if (transSaveOK)
            {
                if (m_saleView.ChkPrint())
                {
                    #region 보류영수증출력 & 저널저장

                    StringBuilder sbPrint = new StringBuilder();

                    foreach (var item in m_saleView.DataRows)
                    {
                        var line = POSPrinterUtils.ReceiptSaleItem(item.NmClass, item.NmItem, item.CdClass,
                            item.CdItem, item.FgClass, item.CdDp,
                            item.FgUtSprcChanged, item.FgTax, item.FgCanc,
                            TypeHelper.ToInt32(item.Qty), TypeHelper.ToInt32(item.UtSprc), (int)item.AmItem);
                        sbPrint.Append(line);
                    }

                    //프린트 메세지 조회
                    DataSet dsPrint = GetPrintMsg("");

                    POSPrinterUtils.Instance.PrintReceiptHold(trxnNo, noBoru, sbPrint.ToString(), m_summaryData.TotalAmt, dsPrint);
                    sbPrint = null;

                    #endregion
                }

                #region 고객용표시기
                m_saleView.ShowCDPMessage(CDPMessageType.TransHold, string.Empty, string.Empty);
                #endregion

                ChangeSaleMode(SaleModes.Sale, false, false, false);
            }

            return transSaveOK;
        }

        #endregion

        #region 거래중지

        /// <summary>
        /// (5) 거래중지 TRAN 정보를 저장 한다. (sat010t, sat011t)
        ///     -  Header Basket + 상품 Basket + End Basket
        /// (6) 계산웝별 부분 합계 Table(sat301t)의 거래중지 항목(A06)의 건수, 금액을 반영하여  INSERT(or Update) 한다.
        ///     - 거래중지금액 = 상품 판매가격 총합계 - 할인합계 - 에누리합계
        ///     - 거래중지건수 = sat301t Table 거래중지 항목 (A06)의 건수 + 1로 Update
        /// (7) POS 합계 Table(sat300t)의 거래중지 항목(A06) 건수, 금액을 반영하여  INSERT(or Update) 한다.
        ///     - 거래중지금액 = 상품 판매가격 총합계 - 할인합계 - 에누리합계
        ///     - 거래중지건수 = sat300t Table 거래중지 항목 (A06)의 건수 + 1로 Update
        ///     ※ (5) ~ (7)작업은 Transaction 묶어서 처리 한다.
        /// (8) 거래중지 영수증을 출력 한다.
        /// (9) 전자저널에 거래중지 프린트 정보를 저장 한다.
        /// </summary>
        bool ProcessCancelAll()
        {
            if (!m_saleView.HasItems)
            {
                ReportInvalidState(InvalidDataInputState.InvalidKey);
                return false;
            }

            bool transSaveOK = true;

            #region TR 저장, SAT010T, SAT011T

            // Update header to cancel
            this.BasketHeader.CancType = NetCommConstants.CANCEL_TYPE_CANCEL;
            this.BasketHeader.TrxnAmt = m_summaryData.RecvAmt.ToString();
            this.BasketHeader.OccrTime = DateTime.Now.ToString("HHmmss");
            MakeBasketItems();

            TranDbHelper db = null;
            SQLiteTransaction trans = null;
            db = TranDbHelper.InitInstance();
            trans = db.BeginTransaction();

            try
            {
                // TR저장 & 집계한다.
                TransManager.SaveTrans(this.BasketHeader, BasketItems.ToArray(), db, trans);
                trans.Commit();
            }
            catch (Exception ex)
            {
                transSaveOK = false;
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                trans.Dispose();
                db.Dispose();
            }
            #endregion

            if (transSaveOK)
            {
                if (m_saleView.ChkPrint())
                {
                    #region 거래중지 영수증출력 & 저널저장

                    StringBuilder sbPrint = new StringBuilder();

                    foreach (var item in m_saleView.DataRows)
                    {
                        var line = POSPrinterUtils.ReceiptSaleItem(item.NmClass, item.NmItem, item.CdClass,
                            item.CdItem, item.FgClass, item.CdDp,
                            item.FgUtSprcChanged, item.FgTax, item.FgCanc,
                            TypeHelper.ToInt32(item.Qty), TypeHelper.ToInt32(item.UtSprc), (int)item.AmItem);
                        sbPrint.Append(line);
                    }

                    //프린트 메세지 조회
                    DataSet dsPrint = GetPrintMsg("");

                    POSPrinterUtils.Instance.PrintReceiptNoSale(true, this.BasketHeader, sbPrint.ToString(), m_summaryData.TotalAmt, false, dsPrint);
                    sbPrint = null;

                    #endregion
                }

                // 거래완료
                TransManager.OnTransComplete();

                #region ⑪ 거래복원 작업을 위한 파일(BackTran.tmp)을 저장한다.
                // BACKTRAN SAVE
                SaveBackTran(this.BasketHeader.TrxnNo);
                #endregion

                #region 고객용표시기
                m_saleView.ShowCDPMessage(CDPMessageType.TransCancel, string.Empty, string.Empty);
                #endregion

                // 정상판매모드로
                ChangeSaleMode(SaleModes.Sale, false, false, false);
            }

            return transSaveOK;
        }

        #endregion

        #region 소계

        bool ProcessSubTotal(bool subTotal)
        {
            if (subTotal)
            {
                if (!m_saleView.HasItems || m_summaryData.TotalAmt <= 0)
                {
                    return false;
                }

                m_saleView.ProcessState = SaleProcessState.SubTotal;

                #region 상품 Basket 생성
                MakeBasketItems();
                #endregion

                #region 소계 Basket 만들기

                int disc = 0;
                int enuri = 0;

                int discPAmt = 0;
                int discPCnt = 0;
                int discAAmt = 0;
                int discACnt = 0;

                int enuriPAmt = 0;
                int enuriPCnt = 0;
                int enuriAAmt = 0;
                int enuriACnt = 0;

                int qtyTotal = 0;
                int itemCnt = this.BasketItems.Select(p => p.SourceCode).Distinct().Count();

                foreach (var bi in this.BasketItems)
                {
                    disc += TypeHelper.ToInt32(bi.AmDisc);
                    enuri += TypeHelper.ToInt32(bi.AmEnuri);

                    discPAmt += "1".Equals(bi.FgDiscProc) ? TypeHelper.ToInt32(bi.AmDisc) : 0;
                    discPCnt += "1".Equals(bi.FgDiscProc) ? 1 : 0;

                    discAAmt += "2".Equals(bi.FgDiscProc) ? TypeHelper.ToInt32(bi.AmDisc) : 0;
                    discACnt += "2".Equals(bi.FgDiscProc) ? 1 : 0;

                    enuriPAmt += "1".Equals(bi.FgEnuriProc) ? TypeHelper.ToInt32(bi.AmEnuri) : 0;
                    enuriPCnt += "1".Equals(bi.FgEnuriProc) ? 1 : 0;

                    enuriAAmt += "2".Equals(bi.FgEnuriProc) ? TypeHelper.ToInt32(bi.AmEnuri) : 0;
                    enuriACnt += "2".Equals(bi.FgEnuriProc) ? 1 : 0;

                    qtyTotal += TypeHelper.ToInt32(bi.CntItem);
                }

                long amTotal = m_summaryData.TotalAmt;

                // 세금미포함 과세상품 합계
                long amWoTax = (long)Math.Round(m_summaryData.TaxItemAmt / 1.1);

                // 과세상품 세금합계
                long amTax = m_summaryData.TaxItemAmt - amWoTax;

                this.BasketSubTtl.AmItem = amTotal.ToString();
                this.BasketSubTtl.AmTotal = amTotal.ToString();
                this.BasketSubTtl.AmSubTtl = Convert.ToString(amTotal - disc - enuri);
                this.BasketSubTtl.AmNoTaxItem = m_summaryData.NoTaxItemAmt.ToString();
                this.BasketSubTtl.AmTaxItem = amWoTax.ToString();
                this.BasketSubTtl.AmTax = amTax.ToString();

                this.BasketSubTtl.AmPcDiscSum = discPAmt.ToString();
                this.BasketSubTtl.CtPcDiscSum = discPCnt.ToString();
                this.BasketSubTtl.AmAmDiscSum = discAAmt.ToString();
                this.BasketSubTtl.CtAmDiscSum = discACnt.ToString();

                this.BasketSubTtl.AmPcEnuriSum = enuriPAmt.ToString();
                this.BasketSubTtl.CtPcEnuriSum = enuriPCnt.ToString();
                this.BasketSubTtl.AmAmEnuriSum = enuriAAmt.ToString();
                this.BasketSubTtl.CtAmEnuriSum = enuriACnt.ToString();

                this.BasketSubTtl.QtItem = qtyTotal.ToString();
                this.BasketSubTtl.CtItem = itemCnt.ToString();

                #endregion

                m_summaryData.Update(this.BasketPays);
                m_saleView.UpdateSummary(this.m_summaryData);
                m_saleView.ShowCDPMessage(CDPMessageType.SubTotal, string.Empty, string.Empty);

                return true;
            }
            else
            {
                m_saleView.ProcessState = SaleProcessState.InputStarted;
                m_saleView.UpdateSummary(this.m_summaryData);

                this.BasketSubTtl = null;
                this.BasketSubTtl = new BasketSubTotal();
                this.BasketItems = null;
                this.BasketItems = new List<BasketItem>();

                this.BasketPays.Clear();
                m_summaryData.Update(this.BasketPays);
                m_saleView.UpdatePayList(this.BasketPays);

                return true;
            }
        }

        #endregion

        #region 결제수단처리 공통

        bool ValidatePayMethod(PayDetailType payType)
        {
            var valid = m_saleItemType == SaleItemType.OnlineItem ?
                payType == PayDetailType.Online :
                payType == PayDetailType.Offline;

            if (!valid)
            {
                ReportInvalidState(payType == PayDetailType.Online ?
                    InvalidDataInputState.OnlyOfflinePay : InvalidDataInputState.OnlyOnlinePay);
            }

            return valid;
        }

        #endregion

        #region 현금결제

        /// <summary>
        // 현금결제 누를때
        ///     ① 입력한 굼액 받은돈에 누적 계산 한다.
        ///     ② 받을돈을 계산 한다. (소계금액 - 받은돈합)
        ///        - 만약 받을돈이 <= 0 이면 결제 등록을 끝내고 판매 작업을 끝낸다.
        ///        '-' 금액은 잔돈 금액으로 처리 한다.
        /// </summary>
        void ProcessPayCash()
        {
            // 모든결제 수단 들어갈때 이함수를 무조건 호출해야함
            if (!ValidatePayMethod(PayDetailType.Offline))
            {
                return;
            }

            bool valid = true;
            m_saleView.InputText.ValidateMoney(2, out valid);
            if (!valid)
            {
                ReportInvalidState(InvalidDataInputState.InvalidData);
                return;
            }

            if (string.IsNullOrEmpty(m_saleView.InputText))
            {
                m_saleView.InputText = m_summaryData.RecvAmt.ToString();
            }

            int amt = 0;
            if (!ValidateOverTender(NetCommConstants.PAYMENT_GROUP_CASH,
                NetCommConstants.PAYMENT_DETAIL_CASH, m_saleView.InputText, out amt))
            {
                return;
            }

            MakePayCash(amt);

            m_saleView.InputText = string.Empty;
            m_saleView.UpdatePayList(this.BasketPays);

            /*
             (4) 더 이상 받을돈이 없어 판매가 왼료되면
                 ① 조회된 회원정보가 있으면 포인트 적립 처리한다.
                 ② 결제 금액중 현금 결제 금액이 있으면 현금 영수증 승인 처리 한다.
                 ③ 매출 Tran을 정보(sat010t, sat011t)를 저장한다.
                 ④ 계산웝별 부분 합계 Table(sat301t) 관련 항목 INSERT(or Update) 한다.
                 ⑤ POS 합계 Table(sat300t) 관련 항목 INSERT(or Update) 한다.
                 ⑥ 품번 집계 Table(sat303t)에 금액, 건수 Insert( Or Update) 한다.
                 ⑦ 카드 결재금액이 있으면 카드사별 집계 Table(sat304t)에 금액, 건수 Insert(Or Update) 한다.
                     ※ ③ ~ ⑦작업은 Transaction 묶어서 처리 한다.
                 ⑧ TRAN 정보를 점서버로 전송한다. 전송 결과를 SAT011T Table에 Update 한다.
                 ⑨ 판매 내역 영수증을 출력 한다.
                 ⑩ 전자저널에 판매내역 프린트 정보를 저장 한다.
                 ⑪ 거래복원 작업을 위한 파일(BackTran.tmp)을 저장한다.
                 ⑫ 판매 대기 초기 MODE로 돌아간다.
            */

            ProcessSaleComplete();
        }

        void MakePayCash(int amt)
        {
            var cashPay = BasketPays.FirstOrDefault(p => NetCommConstants.PAYMENT_DETAIL_CASH.Equals(p.PayDtlCd));
            BasketPayCash pay = null;

            if (cashPay != null)
            {
                pay = (BasketPayCash)cashPay;
            }
            else
            {
                pay = new BasketPayCash();
            }

            pay.PayAmt = Convert.ToString(TypeHelper.ToInt32(pay.PayAmt) + amt);

            m_summaryData.Update(amt);
            m_saleView.UpdateSummary(m_summaryData);

            // 잔액?
            pay.BalAmt += Convert.ToString(TypeHelper.ToInt32(pay.BalAmt) + m_summaryData.ChangeAmt);
            m_saleView.ShowCDPMessage(CDPMessageType.PayCash, amt.ToString(), pay.BalAmt);

            if (cashPay == null)
            {
                BasketPays.Add(pay);
            }
        }

        /// <summary>
        /// 현금영수증처리한다
        /// </summary>
        void MakeCashReceipt()
        {
            // 현금영수증안한다
            if (ConfigData.Current.AppConfig.PosOption.CashReceiptUse == null ||
                !"1".Equals(ConfigData.Current.AppConfig.PosOption.CashReceiptUse))
            {
                return;
            }

            int cashAmt = 0;
            foreach (var item in this.BasketPays)
            {
                if (item.PayGrpCd != null && item.PayDtlCd != null)
                {
                    if (NetCommConstants.PAYMENT_DETAIL_CASH.Equals(item.PayDtlCd) ||
                    NetCommConstants.PAYMENT_DETAIL_TICKET_OTHER.Equals(item.PayDtlCd) ||
                    NetCommConstants.PAYMENT_DETAIL_TICKET.Equals(item.PayDtlCd))
                    {
                        cashAmt += TypeHelper.ToInt32(item.PayAmt) - TypeHelper.ToInt32(item.BalAmt);
                    }
                }
            }

            // 최소금액아니면 현금영수증 필요없다
            int minAmt = TypeHelper.ToInt32(ConfigData.Current.AppConfig.PosOption.CashReceiptApplAmount);
            if (cashAmt < minAmt)
            {
                return;
            }

            // 현금영수증 화면보여줌
            object retData = null;
            var res = m_saleView.ShowCashReceiptPopup(cashAmt, m_summaryData.CalcTaxPerc(cashAmt, true),
                GetPV11ReqDataAdd(),
                out retData);
            if (res != DialogResult.OK)
            {
                return;
            }

            // make cashReceiptBasket
            if (retData != null)
            {
                this.BasketCashReceipt = (BasketCashRecpt)retData;
            }
        }

        #endregion

        #region 카드결제

        void ProcessPayCard()
        {
            // 모든결제 수단 들어갈때 이함수를 무조건 호출해야함
            if (!ValidatePayMethod(PayDetailType.Offline))
            {
                return;
            }

            bool valid = true;
            m_saleView.InputText.ValidateMoney(2, out valid);
            if (!valid)
            {
                ReportInvalidState(InvalidDataInputState.InvalidData);
                return;
            }

            // check input amount
            if (string.IsNullOrEmpty(m_saleView.InputText))
            {
                m_saleView.InputText = m_summaryData.RecvAmt.ToString();
            }

            int amt = 0;
            if (!ValidateOverTender(NetCommConstants.PAYMENT_GROUP_CARD,
                NetCommConstants.PAYMENT_DETAIL_CARD, m_saleView.InputText, out amt))
            {
                return;
            }

            // show card popupz
            object retData = null;
            string errorCode = string.Empty;
            string errorMessage = string.Empty;
            var dlgRes = m_saleView.ShowCardPopup(amt.ToString(), string.Empty, true, null,
                GetPV11ReqDataAdd(), PayCardMode.CreditCard,
                out retData, out errorCode, out errorMessage);

            if (dlgRes != DialogResult.OK)
            {
                m_saleView.InputText = string.Empty;
            }
            else
            {
                // PROCESS CARD PAYMENT, GET DATA FROM POPUP
                BasketPayCard pay = (BasketPayCard)retData;

                m_summaryData.Update(TypeHelper.ToInt32(pay.PayAmt));
                m_saleView.UpdateSummary(m_summaryData);

                m_saleView.ShowCDPMessage(CDPMessageType.PayCard, pay.PayAmt, pay.BalAmt);
                BasketPays.Add(pay);
                m_saleView.UpdatePayList(this.BasketPays);

                m_saleView.InputText = string.Empty;
                ProcessSaleComplete();
            }
        }


        #endregion

        #region 현금IC

        /// <summary>
        /// 현금ID Popup처리
        /// </summary>
        void ProcessPayCashIC()
        {
            // 모든결제 수단 들어갈때 이함수를 무조건 호출해야함
            if (!ValidatePayMethod(PayDetailType.Offline))
            {
                return;
            }

            bool valid = true;
            m_saleView.InputText.ValidateMoney(2, out valid);
            if (!valid)
            {
                ReportInvalidState(InvalidDataInputState.InvalidData);
                return;
            }

            // check input amount
            if (string.IsNullOrEmpty(m_saleView.InputText))
            {
                m_saleView.InputText = m_summaryData.RecvAmt.ToString();
            }

            // show card popup
            int cashAmt = 0;
            if (!ValidateOverTender(NetCommConstants.PAYMENT_GROUP_CASH,
                NetCommConstants.PAYMENT_DETAIL_CASH_IC, m_saleView.InputText, out cashAmt))
            {
                return;
            }

            int taxAmt = m_summaryData.CalcTaxPerc(cashAmt, true);

            object retData = null;
            string errorCode = string.Empty;
            string errorMessage = string.Empty;

            // 정산판매일경우, 항상 결제팝업 닫기가능하게.
            var res = m_saleView.ShowCashICPopup(cashAmt, taxAmt, null, true, out retData, out errorCode, out errorMessage);

            if (res != DialogResult.OK)
            {
                m_saleView.InputText = string.Empty;
            }
            else
            {
                // PROCESS CARD PAYMENT, GET DATA FROM POPUP
                BasketCashIC pay = (BasketCashIC)retData;

                m_summaryData.Update(TypeHelper.ToInt32(pay.PayAmt));
                m_saleView.UpdateSummary(m_summaryData);

                m_saleView.ShowCDPMessage(CDPMessageType.PayOther, pay.PayAmt, pay.BalAmt);
                BasketPays.Add(pay);
                m_saleView.UpdatePayList(this.BasketPays);

                m_saleView.InputText = string.Empty;
                ProcessSaleComplete();
            }
        }

        #endregion

        #region 상품교환권

        void ProcessExchange()
        {
            if (!ValidatePayMethod(PayDetailType.Offline))
            {
                return;
            }

            int iTranOverCnt = 0;

            iTranOverCnt += BasketItems.Count;
            iTranOverCnt += BasketPays.Count;

            if (iTranOverCnt >= 90)
            {
                return;
            }

            object retData = null;
            var res = m_saleView.ShowExchangePopup(m_summaryData.RecvAmt, BasketPays, iTranOverCnt, out retData);
            if (res != DialogResult.OK)
            {
                m_saleView.InputText = string.Empty;
            }
            else
            {
                List<BasketExchange> pay = (List<BasketExchange>)retData;
                Int32 iPay = 0;
                foreach (BasketExchange item in pay)
                {
                    iPay += TypeHelper.ToInt32(item.PayAmt);
                }

                // 결재내역보옂기
                m_summaryData.Update(iPay);
                m_saleView.UpdateSummary(m_summaryData);

                // basket pay list추가
                foreach (BasketExchange item in pay)
                {
                    // basket pay list추가
                    BasketPays.Add(item);
                }

                m_saleView.ShowCDPMessage(CDPMessageType.PayOther, iPay.ToString(), string.Empty);
                m_saleView.UpdatePayList(this.BasketPays);

                // 결제완료확인 및 진행
                ProcessSaleComplete();
            }
        }

        /// <summary>
        /// 반품시 상품교환권
        /// </summary>
        void ProcessExchangeRtn()
        {
            // 모든결제 수단 들어갈때 이함수를 무조건 호출해야함
            if (!ValidatePayMethod(PayDetailType.Offline))
            {
                return;
            }

            if (string.IsNullOrEmpty(m_saleView.InputText))
            {
                m_saleView.InputText = m_summaryData.RecvAmt.ToString();
            }

            object retData = null;
            int cashAmt = TypeHelper.ToInt32(m_saleView.InputText);
            var res = m_saleView.ShowExchangeRtnPopup(cashAmt, out retData);

            if (res != DialogResult.OK)
            {
                m_saleView.InputText = string.Empty;
            }
            else
            {
                // PROCESS CARD PAYMENT, GET DATA FROM POPUP
                BasketExchange pay = (BasketExchange)retData;

                m_summaryData.Update(TypeHelper.ToInt32(pay.PayAmt));
                m_saleView.UpdateSummary(m_summaryData);

                m_saleView.ShowCDPMessage(CDPMessageType.PayOther, pay.PayAmt, pay.BalAmt);
                BasketPays.Add(pay);
                m_saleView.UpdatePayList(this.BasketPays);

                m_saleView.InputText = string.Empty;
                ProcessSaleComplete();
            }
        }

        #endregion

        #region 타사상품권

        void ProcessOtherTicket()
        {
            if (!ValidatePayMethod(PayDetailType.Offline))
            {
                return;
            }

            int iTranOverCnt = 0;

            iTranOverCnt += BasketItems.Count;
            iTranOverCnt += BasketPays.Count;

            if (iTranOverCnt >= 90)
            {
                return;
            }

            object retData = null;
            var res = m_saleView.ShowOtherTicketPopup(m_summaryData.RecvAmt, BasketPays, iTranOverCnt, BasketHeader.CancType != "1" ? false : true, out retData);
            if (res != DialogResult.OK)
            {
                m_saleView.InputText = string.Empty;
            }
            else
            {
                var pays = (List<BasketPay>)retData;

                Int32 iPay = 0;

                foreach (BasketPay item in pays)
                {
                    iPay += TypeHelper.ToInt32(item.PayAmt);
                }

                // 결재내역보옂기
                m_summaryData.Update(iPay);
                m_saleView.UpdateSummary(m_summaryData);
                BasketPays.AddRange(pays);

                m_saleView.ShowCDPMessage(CDPMessageType.PayGift, iPay.ToString(), string.Empty);
                m_saleView.UpdatePayList(this.BasketPays);

                // 결제완료확인 및 진행
                ProcessSaleComplete();
            }
        }

        #endregion

        #region 포인트사용

        /// <summary>
        /// 
        /// </summary>
        void ProcessPointUse()
        {
            // 모든결제 수단 들어갈때 이함수를 무조건 호출해야함
            if (!ValidatePayMethod(PayDetailType.Offline))
            {
                return;
            }

            object retData = null;
            var res = m_saleView.ShowPointUsePopup(m_summaryData.RecvAmt, this.CustInfo, out retData);
            if (res != DialogResult.OK)
            {
                m_saleView.InputText = string.Empty;
            }
            else
            {
                BasketPoint pay = (BasketPoint)retData;

                // 결재내역보옂기
                m_summaryData.Update(TypeHelper.ToInt32(pay.PayAmt));
                m_saleView.UpdateSummary(m_summaryData);

                // basket pay list추가
                m_saleView.ShowCDPMessage(CDPMessageType.PayPoint, pay.PayAmt, pay.BalAmt);
                BasketPays.Add(pay);
                m_saleView.UpdatePayList(this.BasketPays);

                // 결제완료확인 및 진행
                ProcessSaleComplete();
            }
        }

        #endregion

        #region 쿠폰할인

        /// <summary>
        /// 할인쿠폰
        /// </summary>
        void ProcessCoupon()
        {
            if (!ValidatePayMethod(PayDetailType.Offline))
            {
                return;
            }

            #region Loc added 11.13 - 주석처리
            
            
            // Loc added 11.13
            // 할인쿠폰이 한번만 가능함
            var couponPayCount = BasketPays.Count(p => p.PayGrpCd.Equals(NetCommConstants.PAYMENT_GROUP_COUPON) &&
                p.PayDtlCd.Equals(NetCommConstants.PAYMENT_DETAIL_COUPON));
            if (couponPayCount > 0)
            {
                m_saleView.ReportInvalidState(InvalidDataInputState.TwoCouponInputError);
                return;
            }

            #endregion
            
            bool bType = false;
            string strType = string.Empty;
            string strTemp = string.Empty;

            //단일브랜드 확인
            foreach (var item in m_saleView.DataRows)
            {
                var bi = item.ToBasketItem();

                if (bi != null)
                {
                    strTemp = bi.CdClass != null ? bi.CdClass : "";

                    if (strType.Length <= 0)
                    {
                        strType = strTemp;
                    }
                    else
                    {
                        if (strType != strTemp)
                        {
                            bType = true;
                            break;
                        }
                    }
                }
            }

            object retData = null;
            var res = m_saleView.ShowCouponPopup(m_summaryData.RecvAmt, BasketItems, BasketPays, bType, out retData);
            if (res != DialogResult.OK)
            {
                m_saleView.InputText = string.Empty;
            }
            else
            {
                BasketCoupon pay = (BasketCoupon)retData;

                // 결재내역보옂기
                m_summaryData.Update(TypeHelper.ToInt32(pay.PayAmt));
                m_saleView.UpdateSummary(m_summaryData);

                // basket pay list추가
                m_saleView.ShowCDPMessage(CDPMessageType.PayCoupon, pay.PayAmt, pay.BalAmt);
                BasketPays.Add(pay);
                m_saleView.UpdatePayList(this.BasketPays);

                // 결제완료확인 및 진행
                ProcessSaleComplete();
            }
        }

        #endregion

        #region 기타결제

        /// <summary>
        /// 기타결제팝업
        /// </summary>
        void ProcessOtherPay()
        {
            // 모든결제 수단 들어갈때 이함수를 무조건 호출해야함
            if (!ValidatePayMethod(PayDetailType.Offline))
            {
                return;
            }

            // check input amount
            m_saleView.InputText = m_summaryData.RecvAmt.ToString();

            // show 기타결제 popup
            int cashAmt = 0;
            bool valid = false;
            cashAmt = m_saleView.InputText.ValidateMoney(2, out valid);
            if (!valid)
            {
                return;
            }

            int taxAmt = m_summaryData.CalcTaxPerc(cashAmt, true);

            object retData = null;
            var res = m_saleView.ShowOtherPayMethod(cashAmt, taxAmt, out retData);

            if (res != DialogResult.OK)
            {
                m_saleView.InputText = string.Empty;
            }
            else
            {
                // PROCESS CARD PAYMENT, GET DATA FROM POPUP
                BasketPay pay = (BasketPay)retData;

                m_summaryData.Update(TypeHelper.ToInt32(pay.PayAmt));
                m_saleView.UpdateSummary(m_summaryData);

                m_saleView.ShowCDPMessage(CDPMessageType.PayOther, pay.PayAmt, pay.BalAmt);
                BasketPays.Add(pay);
                m_saleView.UpdatePayList(this.BasketPays);

                m_saleView.InputText = string.Empty;
                ProcessSaleComplete();
            }
        }

        #endregion

        #region 온라인매출

        void ProcessOnlinePay()
        {
            // 모든결제 수단 들어갈때 이함수를 무조건 호출해야함
            if (!ValidatePayMethod(PayDetailType.Online))
            {
                return;
            }

            /* 
             * 단위테스트계획서_POS_V1.0_20150622.xlsx
             * • 받을돈 > 입력한 금액인 경우 "결제 금액 초과 되어습니다." 표시 하지 말고 그냥 
                온라인 매출 금액으로 입력 처리 할것
             */
            /*
            bool valid = true;
            m_saleView.InputText.ValidateMoney(2, out valid);
            if (!valid)
            {
                ReportInvalidState(InvalidDataInputState.InvalidData);
                return;
            }

            if (string.IsNullOrEmpty(m_saleView.InputText))
            {
                m_saleView.InputText = m_summaryData.RecvAmt.ToString();
            }*/

            // show card popup
            int cashAmt = 0;
            if (!ValidateOverTender(NetCommConstants.PAYMENT_GROUP_SPECIAL,
                NetCommConstants.PAYMENT_DETAIL_ONLINE, m_summaryData.RecvAmt.ToString(), out cashAmt))
            {
                return;
            }

            BasketPayCash pay = new BasketPayCash()
            {
                BalDtlCd = NetCommConstants.PAYMENT_DETAIL_NONE,
                BalGrpCd = NetCommConstants.PAYMENT_GROUP_NONE,
                CancFg = "0",
                PayAmt = m_summaryData.RecvAmt.ToString(),
                PayDtlCd = NetCommConstants.PAYMENT_DETAIL_ONLINE,
                PayGrpCd = NetCommConstants.PAYMENT_GROUP_SPECIAL
            };

            // 결재내역보옂기
            m_summaryData.Update(TypeHelper.ToInt32(pay.PayAmt));
            m_saleView.UpdateSummary(m_summaryData);

            // basket pay list추가
            m_saleView.ShowCDPMessage(CDPMessageType.PayOnline, pay.PayAmt, pay.BalAmt);
            BasketPays.Add(pay);
            m_saleView.UpdatePayList(this.BasketPays);

            // 결제완료확인 및 진행
            ProcessSaleComplete();
        }

        /// <summary>
        /// 온라인 매출 여부확인
        /// </summary>
        /// <returns></returns>
        bool ValidateOnlinePay()
        {
            bool valid = true;
            // 상품리스트
            foreach (var item in m_saleView.DataRows)
            {
                int nFgClass = TypeHelper.ToInt32(item.FgClass);
                if (nFgClass < 60 || nFgClass > 69)
                {
                    valid = false;
                    break;
                }
            }

            // 결제수단확인
            if (valid)
            {
                foreach (var pay in this.BasketPays)
                {
                    if (!NetCommConstants.PAYMENT_DETAIL_ONLINE.Equals(pay.PayDtlCd))
                    {
                        valid = false;
                        break;
                    }
                }
            }

            if (!valid)
            {
                ReportInvalidState(InvalidDataInputState.OnlyOnlinePay);
            }

            return valid;
        }

        #endregion

        #region 결제진행하기 - 지불입력완료 시

        /// <summary>
        /// ///	(4) 더 이상 받을돈이 없어 판매가 왼료되면
        ///		① 조회된 회원정보가 있으면 포인트 적립 처리한다.
        ///           ② 결제 금액중 현금 결제 금액이 있으면 현금 영수증 승인 처리 한다.
        ///           ③ 매출 Tran을 정보(sat010t, sat011t)를 저장한다.
        ///           ④ 계산웝별 부분 합계 Table(sat301t) 관련 항목 INSERT(or Update) 한다.
        ///           ⑤ POS 합계 Table(sat300t) 관련 항목 INSERT(or Update) 한다.
        ///           ⑥ 품번 집계 Table(sat303t)에 금액, 건수 Insert( Or Update) 한다.
        ///           ⑦ 카드 결재금액이 있으면 카드사별 집계 Table(sat304t)에 금액, 건수 Insert(Or Update) 한다.
        ///             ※ ③ ~ ⑦작업은 Transaction 묶어서 처리 한다.
        ///           ⑧ TRAN 정보를 점서버로 전송한다. 전송 결과를 SAT011T Table에 Update 한다.
        ///           ⑨ 판매 내역 영수증을 출력 한다.
        ///           ⑩ 전자저널에 판매내역 프린트 정보를 저장 한다.
        ///           ⑪ 거래복원 작업을 위한 파일(BackTran.tmp)을 저장한다.
        ///           ⑫ 판매 대기 초기 MODE로 돌아간다.
        /// </summary>
        void ProcessSaleComplete()
        {
            // END OF PAYMENT
            // 받을금액 없을때
            if (m_summaryData.RecvAmt > 0)
            {
                return;
            }

            #region 프로모션 처리

            CheckPromotion();

            #endregion

            #region 현금영수증처리

            MakeCashReceipt();

            #endregion

            #region 포인트적립 정보 처리

            MakePointSave();

            #endregion

            #region 포인트 적립후 프로모션 행사글 출력여부 확인

            if (CheckPromotionPrint())
            {
                // Promotion Check Process 있을경우, 기다림
                while (m_promotionCount > 0)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }

            #endregion

            // TR생성 및 출력
            ProcessSaleCompleteFinal();
        }

        /// <summary>
        /// 마지막 작업
        /// </summary>
        private void ProcessSaleCompleteFinal()
        {
            #region TR생성, 출력 및 백업

            #region Basket Header
            this.BasketHeader.TrxnNo = ConfigData.Current.AppConfig.PosInfo.TrxnNo;
            this.BasketHeader.TrxnAmt = m_summaryData.TotalAmt.ToString();
            this.BasketHeader.OccrDate = DateTime.Today.ToString("yyyyMMdd");
            this.BasketHeader.OccrTime = DateTime.Now.ToString("HHmmss");

            // 현금영수증여부
            this.BasketHeader.CRProcFg = this.BasketCashReceipt != null &&
                !string.IsNullOrEmpty(this.BasketCashReceipt.NoAppr) ? "1" : "0";

            // 포인트적립여부
            this.BasketHeader.PntSaveProcFg = this.BasketPointSave != null &&
                !string.IsNullOrEmpty(this.BasketPointSave.NoAppr) ? "1" : "0";
            #endregion

            var beforeState = m_saleView.ProcessState;

            // 결제중모드전환
            m_saleView.ProcessState = SaleProcessState.Payment;
            if (!ProcessTRGenerate(true, null))
            {
                m_saleView.ProcessState = beforeState;
            }

            #endregion
        }

        /// <summary>
        /// 현재 BASKET으로 TR생성 및 출력
        /// </summary>
        bool ProcessTRGenerate(bool showProgress, List<BasketTksPresentRtn> tksPsnList)
        {
            if (showProgress)
                m_saleView.ShowProgress(true);
            m_saleView.GuideMessage = GUIDE_MSG_TR_GENERATING;
            Application.DoEvents();

            bool bResultFlag = true;
            List<BasketBase> details = new List<BasketBase>();

            TranDbHelper db = null;
            SQLiteTransaction trans = null;
            db = TranDbHelper.InitInstance();
            trans = db.BeginTransaction();

            try
            {
                #region TR저장 & 집계한다.

                // 현금영수증
                details.Add(this.BasketCashReceipt);
                // 포인트
                details.Add(this.BasketPointSave);
                // 상품리스트
                details.AddRange(this.BasketItems.ToArray());
                // 소계
                details.Add(this.BasketSubTtl);
                // 결제수단
                details.AddRange(this.BasketPays.ToArray());
                // 거래정보 저장함
                TransManager.SaveTrans(BasketHeader, details.ToArray(), db, trans);

                #endregion

                #region 사은품회수 - 반품일때
                BasketHeaderTKS = null;
                if (m_saleView.SaleMode == SaleModes.AutoReturn && tksPsnList != null &&
                    tksPsnList.Count > 0)
                {
                    // SaveTrans 함수는 한TRANS에 여러번호출하면 
                    // TrxnNo증가해줘야한다
                    int trxnNo = TypeHelper.ToInt32(ConfigData.Current.AppConfig.PosInfo.TrxnNo);
                    trxnNo++;
                    ConfigData.Current.AppConfig.PosInfo.TrxnNo = trxnNo.ToString("d6");

                    BasketHeaderTKS = new BasketHeader()
                    {
                        TrxnType = NetCommConstants.TRXN_TYPE_TKS_PRS,
                        CancType = "0",
                        OTCasNo = this.BasketHeader.CasNo,
                        OTPosNo = this.BasketHeader.PosNo,
                        OTSaleDate = this.BasketHeader.SaleDate,
                        OTStoreNo = this.BasketHeader.StoreNo,
                        OTTrxnNo = this.BasketHeader.TrxnNo,
                        RfProcFg = "0",
                        CRProcFg = "0",
                        PntSaveProcFg = "0"
                    };

                    // 저장
                    TransManager.SaveTrans(BasketHeaderTKS, tksPsnList.ToArray(), db, trans);
                }

                #endregion

                #region TRANSACTION 완료
                trans.Commit();
                #endregion
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                bResultFlag = false;
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                if (showProgress)
                    m_saleView.ShowProgress(false);
                db.Dispose();
                trans.Dispose();
            }

            // 성공시
            if (bResultFlag)
            {
                #region 돈통 오픈
                //2015.09.12 정광호 추가-----------------------------------------------------------------------------------
                //결제가 완료된 시점에서 현금,상품교환권,할인쿠폰, 구상품교환권, 타사상품권 결제가 존재한다면 돈통 open
                if (POSDeviceManager.CashDrawer != null && POSDeviceManager.CashDrawer.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened && POSDeviceManager.CashDrawer.Enabled)
                {
                    bool bOpenCash = false;

                    foreach (var item in details)
                    {
                        if (item.GetType().Name.ToString().Equals("BasketPayCash"))
                        {
                            #region 현금

                            BasketPayCash bp = (BasketPayCash)item;

                            if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0 &&
                                (bp.PayGrpCd != NetCommConstants.PAYMENT_GROUP_SPECIAL && bp.PayDtlCd != NetCommConstants.PAYMENT_DETAIL_ONLINE))
                            {
                                bOpenCash = true;
                                break;
                            }

                            #endregion
                        }
                        else if (item.GetType().Name.ToString().Equals("BasketExchange"))
                        {
                            #region 상품교환권

                            BasketExchange bp = (BasketExchange)item;

                            if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                            {
                                bOpenCash = true;
                                break;
                            }

                            #endregion
                        }
                        else if (item.GetType().Name.ToString().Equals("BasketOtherTicket"))
                        {
                            #region 타사상품권

                            BasketOtherTicket bp = (BasketOtherTicket)item;

                            if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                            {
                                bOpenCash = true;
                                break;
                            }

                            #endregion
                        }
                        else if (item.GetType().Name.ToString().Equals("BasketCoupon"))
                        {
                            #region 쿠폰

                            BasketCoupon bp = (BasketCoupon)item;

                            if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                            {
                                bOpenCash = true;
                                break;
                            }

                            #endregion
                        }
                        else if (item.GetType().Name.ToString().Equals("BasketOldExGift"))
                        {
                            #region 구상품교환권

                            BasketOldExGift bp = (BasketOldExGift)item;

                            if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                            {
                                bOpenCash = true;
                                break;
                            }

                            #endregion
                        }
                    }

                    if (bOpenCash)
                    {
                        try
                        {
                            //돈통 open
                            POSDeviceManager.CashDrawer.OpenDrawer();
                        }
                        catch (Exception ex)
                        {
                            LogUtils.Instance.LogException(ex);
                        }
                    }
                }
                //---------------------------------------------------------------------------------------------------------
                #endregion

                #region 출력확인

                if (m_saleView.ChkPrint())
                {
                    m_saleView.GuideMessage = GUIDE_MSG_PRINTING;
                    Application.DoEvents();

                    #region 판매 내역 영수증을 출력 한다.

                    //프린트 메세지 조회
                    DataSet dsPrint = GetPrintMsg(this.strGetCD_CLASS);

                    //프린트 DCC내용 전체조회
                    DataTable dtDcc = GetPrintDccMsg();

                    POSPrinterUtils.CancelPrint cancelPrint = TypeHelper.ToString(BasketHeader.CancType) == "2" ? POSPrinterUtils.CancelPrint.Cancel : POSPrinterUtils.CancelPrint.Normal;

                    //결제내용 출력
                    POSPrinterUtils.Instance.SetPrintPay(true, false, false, false, BasketHeader, details, null,
                        dsPrint, this.dtPromoPrint, dtDcc, cancelPrint, POSPrinterUtils.CardPrint.Basic, false);

                    // 2015.08.28 정광호 추가
                    //사은품 회수 출력물 발행
                    if (m_saleView.SaleMode == SaleModes.AutoReturn && tksPsnList != null && tksPsnList.Count > 0)
                    {
                        POSPrinterUtils.Instance.PrintTksPresentRtn(true, false, BasketHeaderTKS, tksPsnList);
                    }

                    #endregion
                }

                #endregion

                // 거래완료
                TransManager.OnTransComplete();

                //-------------------------------------------------------------------------
                //2015.09.01 정광호 추가
                //판매완료후 영업일자와 시스템일자가 틀린경우 메세지박스 호출
                int sysDate = int.Parse(DateTime.Today.ToString("yyyyMMdd"));
                int saleDate = TypeHelper.ToInt32(ConfigData.Current.AppConfig.PosInfo.SaleDate);

                if (sysDate > saleDate)
                {
                    if ("1".Equals(ConfigData.Current.AppConfig.PosInfo.StoreType))
                    {
                        // 24시간 운영매장인경우
                        int nowDate = TypeHelper.ToInt32(DateTime.Now.Hour);
                        int eodBaseDate = TypeHelper.ToInt32(ConfigData.Current.AppConfig.PosInfo.EodBaseHour.Length <= 0 || ConfigData.Current.AppConfig.PosInfo.EodBaseHour == "0" ?
                            "1" : ConfigData.Current.AppConfig.PosInfo.EodBaseHour);

                        if (nowDate > eodBaseDate)
                        {
                            m_saleView.ShowDateErrorMsg();
                        }
                    }
                    else
                    {
                        //24시간 운영매장이 아닌경우
                        m_saleView.ShowDateErrorMsg();
                    }
                }

                //-------------------------------------------------------------------------

                #region ⑪ 거래복원 작업을 위한 파일(BackTran.tmp)을 저장한다.

                m_saleView.GuideMessage = GUIDE_SAVING_BACKUP;
                Application.DoEvents();

                // BACKTRAN SAVE
                SaveBackTran(this.BasketHeader.TrxnNo);

                #endregion

                #region 여전법 추가 0617
                ClearSecureData();
                #endregion

                // 결재완료모드전황 => 대기모드전환
                ChangeSaleMode(SaleModes.Sale, false, false, false);

                // Free Memory
                GC.Collect();
            }

            return bResultFlag;
        }
        
        /// <summary>
        /// 프린트 광고메세지 조회
        /// </summary>
        /// <param name="strCD_CLASS">품번코드</param>
        /// <returns></returns>
        DataSet GetPrintMsg(string strCD_CLASS)
        {
            DataSet ds = null;
            var masterdb = MasterDbHelper.InitInstance();

            try
            {
                string strSql = Extensions.LoadSqlCommand("POS_SL", "M001GetPrintTitleMsg");
                strSql += Extensions.LoadSqlCommand("POS_SL", "M001GetPrintMsg");
                strSql += Environment.NewLine + " ";
                strSql += Environment.NewLine + (WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00260"));
                strSql += Environment.NewLine + ("SELECT		NM_DESC");
                strSql += Environment.NewLine + (",			FG_SIZ");
                strSql += Environment.NewLine + ("FROM		BSM042T");
                strSql += Environment.NewLine + ("WHERE		CD_STORE	=	@CD_STORE");
                strSql += Environment.NewLine + (WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00261"));
                strSql += Environment.NewLine + ("AND			CD_CLASS	IN	(" + strCD_CLASS + ")");
                strSql += Environment.NewLine + ("ORDER BY	CD_CLASS");
                strSql += Environment.NewLine + (",			SQ_LOC");
                strSql += Environment.NewLine + (";");

                ds = masterdb.ExecuteQuery(strSql,
                    new string[] { "@CD_STORE" },
                    new object[] { ConfigData.Current.AppConfig.PosInfo.StoreNo });
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                masterdb.EndInstance();
            }

            return ds;
        }

        /// <summary>
        /// 프린트 DCC내용 전체조회
        /// </summary>
        /// <returns></returns>
        DataTable GetPrintDccMsg()
        {
            DataSet ds = new DataSet();
            var masterdb = MasterDbHelper.InitInstance();
            try
            {
                ds = masterdb.ExecuteQuery(Extensions.LoadSqlCommand("POS_SL", "M001GetDccMsgSYM051T"), null, null);
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                masterdb.EndInstance();
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 여전법 추가 0617
        /// BasketCard CardNo 메모리삭제
        /// </summary>
        void ClearSecureData()
        {
            // 여전법 추가 0617
            // 메모리 삭제
            this.BasketCashReceipt.NoTrack.ResetZero();
            this.BasketCashReceipt.NoPersonal.ResetZero();

            foreach (var bp in this.BasketPays)
            {                
                if (bp is BasketPayCard)
                {
                    var bpc = (BasketPayCard)bp;
                    bpc.CardNo.ResetZero();
                    bpc.TrackII.ResetZero();                    
                }
            }
        }

        #endregion

        #region BackTran.tmp 파일 저장

        void SaveBackTran(string noTrxn)
        {
            string backTranFile = Path.Combine(FXConsts.FOLDER_DATA_TRANS.GetFolder(), "BackTran.tmp");
            var db = TranDbHelper.InitInstance();
            try
            {
                string query = "M001LoadTransByNoTrxn".POSSLQuerySQL();
                var ds = db.ExecuteQuery(query,
                    new string[] {
                        "@DD_SALE", "@CD_STORE", "@NO_POS", "@NO_TRXN"
                    },
                    new object[] {
                        ConfigData.Current.AppConfig.PosInfo.SaleDate,
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo,
                        noTrxn
                    });

                StringBuilder sbTmp = new StringBuilder();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var item = ds.Tables[0].Rows[i];

                    var bs = new BasketTrans()
                    {
                        DdSale = item["DD_SALE"].ToString(),
                        CdStore = item["CD_STORE"].ToString(),
                        NoPos = item["NO_POS"].ToString(),
                        NoTrxn = item["NO_TRXN"].ToString(),
                        SqTrxn = item["SQ_TRXN"].ToString(),
                        VcCont = item["VC_CONT"].ToString(),
                    };

                    sbTmp.Append(bs.ToString());
                    if (i < ds.Tables[0].Rows.Count - 1)
                    {
                        // 0x1C
                        sbTmp.Append((char)NetCommConstants.RECORD_SEP);
                    }
                }

                // save overwrite to file
                File.WriteAllText(backTranFile, sbTmp.ToString());
                sbTmp = null;
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                db.Dispose();

            }
        }

        /// <summary>
        /// 거래복원
        /// </summary>
        /// <returns></returns>
        bool LoadBackTran()
        {
            if (m_saleView.HasItems)
            {
                return false;
            }

            string backTranFile = Path.Combine(FXConsts.FOLDER_DATA_TRANS.GetFolder(), "BackTran.tmp");
            if (!File.Exists(backTranFile))
            {
                return false;
            }

            string fileContent = File.ReadAllText(backTranFile);
            if (string.IsNullOrEmpty(fileContent))
            {
                return false;
            }

            string[] records = fileContent.Split(new char[] {
                (char)NetCommConstants.RECORD_SEP
            });

            List<BasketItem> list = new List<BasketItem>();
            foreach (var r in records)
            {
                BasketTrans bt = null;

                try
                {
                    bt = (BasketTrans)BasketTrans.Parse(typeof(BasketTrans), r);
                }
                catch
                {

                }

                if (bt != null && bt.VcCont.StartsWith("200"))
                {
                    BasketItem bi = (BasketItem)BasketItem.Parse(typeof(BasketItem), bt.VcCont);
                    if (bi != null && (string.IsNullOrEmpty(bi.FgCanc) || "0".Equals(bi.FgCanc)))
                    {
                        list.Add(bi);
                    }
                }
            }

            // 상품있을때만처리
            if (list.Count > 0)
            {
                // if 저장물
                var isOtherSale = list.Where(p => SLExtensions.CDDP_OTHER.Equals(p.CdDp)).Count() > 0;

                if (m_saleView.SaleMode.ToString().Contains("Return"))
                {
                    bool valid = (isOtherSale && m_saleView.SaleMode.ToString().Contains("Other"))
                    || (!isOtherSale && !m_saleView.SaleMode.ToString().Contains("Other"));
                    if (!valid)
                    {
                        return false;
                    }
                }
                else
                {
                    if (isOtherSale)
                    {
                        ChangeSaleMode(SaleModes.OtherSale, false, false, true);
                        //this.BasketHeader.TrxnType = NetCommConstants.TRXN_TYPE_OTH_SALE;
                        //m_saleView.SaleMode = SaleModes.OtherSale;
                    }
                    else
                    {
                        ChangeSaleMode(SaleModes.Sale, false, false, true);
                    }
                }

                // 상품리스트 표시
                LoadItems(list.ToArray());
            }

            // clear file content
            File.WriteAllText(backTranFile, string.Empty);

            return list.Count > 0;
        }

        #endregion

        #region 저장물판매 - 쇼핑팩판매, 반품

        /// <summary>
        /// 저장물판매
        /// </summary>
        /// <returns></returns>
        bool ProcessOthSale()
        {
            var res = ChangeSaleMode(SaleModes.OtherSale, true, false, true);
            if (res == ChangeSaleModeStatus.InvalidCondition)
            {
                ReportInvalidState(InvalidDataInputState.InvalidKey);
            }

            return res == ChangeSaleModeStatus.Success;
        }

        /// <summary>
        /// 저장물 반품
        /// </summary>
        /// <returns></returns>
        bool ProcessOthSaleReturn()
        {
            var res = ChangeSaleMode(SaleModes.OtherSaleReturn, true, false, true);
            if (res == ChangeSaleModeStatus.InvalidCondition)
            {
                ReportInvalidState(InvalidDataInputState.InvalidKey);
            }

            return res == ChangeSaleModeStatus.Success;
        }

        #endregion

        #region 포인트 적립처리

        /// <summary>
        /// 포인트 적립처리
        /// </summary>
        void MakePointSave()
        {
            if (CustInfo == null) return;

            if (BasketHeader.TrxnType != NetCommConstants.TRXN_TYPE_SALE) return;

            //2015.09.15 정광호 추가-------------------------------------------------------------------------------------------------------------------------------
            //*****************************************************************************************************************************************************
            //포인트 적립금액이 있는경우만 팝업을 띄운다
            bool bOpenPopUp = false;
            if (BasketPays != null && BasketPays.Count > 0)
            {
                foreach (var item in BasketPays)
                {
                    if (item.GetType().Name.ToString().Equals("BasketPayCash"))
                    {
                        #region 현금

                        BasketPayCash bp = (BasketPayCash)item;

                        if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0 && (bp.PayAmt != null ? TypeHelper.ToInt32(bp.PayAmt) : 0) > 0)
                        {
                            if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_SPECIAL && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_ONLINE)
                            {

                            }
                            else
                            {
                                #region 현금
                                bOpenPopUp = true;
                                break;
                                #endregion
                            }
                        }
                        #endregion
                    }
                    else if (item.GetType().Name.ToString().Equals("BasketPayCard"))
                    {
                        #region 신용카드

                        BasketPayCard bp = (BasketPayCard)item;

                        if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0 && (bp.PayAmt != null ? TypeHelper.ToInt32(bp.PayAmt) : 0) > 0)
                        {
                            bOpenPopUp = true;
                            break;
                        }

                        #endregion
                    }
                    else if (item.GetType().Name.ToString().Equals("BasketOtherTicket"))
                    {
                        #region 타사상품권

                        BasketOtherTicket bp = (BasketOtherTicket)item;

                        if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0 && (bp.PayAmt != null ? TypeHelper.ToInt32(bp.PayAmt) : 0) > 0)
                        {
                            bOpenPopUp = true;
                            break;
                        }

                        #endregion
                    }
                    else if (item.GetType().Name.ToString().Equals("BasketPay"))
                    {
                        #region 타건복지, 결제할인, 타건카드

                        BasketPay bp = (BasketPay)item;

                        if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0 && (bp.PayAmt != null ? TypeHelper.ToInt32(bp.PayAmt) : 0) > 0)
                        {
                            if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CARD_WELFARE)
                            {
                                #region 타건복지
                                bOpenPopUp = true;
                                break;
                                #endregion
                            }
                            else if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CARD_OTHER)
                            {
                                #region 타건카드
                                bOpenPopUp = true;
                                break;
                                #endregion
                            }
                        }

                        #endregion
                    }
                    else if (item.GetType().Name.ToString().Equals("BasketCashIC"))
                    {
                        #region 현금IC

                        BasketCashIC bp = (BasketCashIC)item;

                        if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0 && (bp.PayAmt != null ? TypeHelper.ToInt32(bp.PayAmt) : 0) > 0)
                        {
                            bOpenPopUp = true;
                            break;
                        }

                        #endregion
                    }
                }
            }
            //*****************************************************************************************************************************************************
            //-----------------------------------------------------------------------------------------------------------------------------------------------------

            if (bOpenPopUp)
            {
                //포인트 적립화면 오픈
                object retData = null;
                var res = m_saleView.ShowPointSavePopup(CustInfo, BasketHeader, BasketPays, BasketSubTtl, BasketPointSave, this.dicPromoPoint, out retData);
                if (res != DialogResult.OK)
                {
                    return;
                }

                // make cashReceiptBasket
                this.BasketPointSave = (BasketPointSave)retData;
            }
        }

        #endregion
    }
}
