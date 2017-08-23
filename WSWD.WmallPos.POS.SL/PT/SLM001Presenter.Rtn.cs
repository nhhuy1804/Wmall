using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using WSWD.WmallPos.POS.SL.Data;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using WSWD.WmallPos.FX.NetComm.Tasks.PQ;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.POS.PY.PT;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PV;
using WSWD.WmallPos.FX.NetComm.Tasks.PP;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PP;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PQ;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PV;
using WSWD.WmallPos.POS.PY.Data;

namespace WSWD.WmallPos.POS.SL.PT
{
    /// <summary>
    /// Presenter class
    /// 수동/자동반품 업무따라 뺼것
    /// 개발자     : TCL
    /// 개발일자   : 06.03
    /// </summary>
    partial class SLM001Presenter
    {
        #region 수동반품

        bool ProcessSaleManuReturn()
        {
            var res = ChangeSaleMode(SaleModes.ManuReturn, true, true, true);
            if (res == ChangeSaleModeStatus.InvalidCondition)
            {
                ReportInvalidState(InvalidDataInputState.InvalidKey);
            }

            return res == ChangeSaleModeStatus.Success;
        }

        #endregion

        #region 자동반품

        #region 자동반품용 변수

        /// <summary>
        /// 자동반품용 결제수단 basket list
        /// </summary>
        List<BasketPay> BasketPaysAutoRtn;

        /// <summary>
        /// 진행시 순서된 결제수단 리스트
        /// 
        /// </summary>
        List<BasketPay> ProcessPays;
        int m_autoRtnPayProcessIdx = -1;

        List<PQ11RespData> TksPresentList = null;

        /// <summary>
        /// 강제진행한 결제수단
        /// </summary>
        List<BasketBase> ForcePays = null;

        // 반품처리시 중간에 취소가능여부
        bool m_autoRtnCancellable = true;

        /// <summary>
        /// 첫밴승인결제수단여부
        /// </summary>
        bool m_autoRtnFirstVANPay = true;

        string m_cardCancCVM = "N"; // No pin, no sign

        #endregion

        #region 영수증번호 확인 작업

        /// <summary>
        /// 입력된번로로 거래있는지 확인
        ///        ※ 영수증 거래 정보 : 판매읻자(6) + 포스번호(4) + 거래번호(4)
        ///         'A' + YYMMDD+POS번호(4자리) + 거래번호(4자리) + 'A'
        /// </summary>
        void AutoRtnProcessOnEnter()
        {
            if (m_saleView.ProcessState == SaleProcessState.AutoRtnReady &&
                 string.IsNullOrEmpty(m_saleView.InputText))
            {
                AutoRtnConfirmStart();
                return;
            }

            string inputText = m_saleView.InputText;
            string storeNo = ConfigData.Current.AppConfig.PosInfo.StoreNo;

            if (inputText.StartsWith("A"))
            {
                inputText = inputText.Substring(1);
            }

            if (inputText.EndsWith("A"))
            {
                inputText = inputText.Substring(0, inputText.Length - 1);
            }

            if (inputText.Length != 14 && inputText.Length != 16)
            {
                ReportInvalidState(InvalidDataInputState.LengthError);
                return;
            }

            if (inputText.Length == 16)
            {
                //2015.09.11 정광호 수정
                // 바코드 스캔이 기존 14자리에서 StoreNo를 포함한 16자리로 변경
                if (inputText.Substring(0, 2) != ConfigData.Current.AppConfig.PosInfo.StoreNo)
                {
                    ReportInvalidState(InvalidDataInputState.ReturnOnlySaleItemError);
                    return;
                }

                inputText = inputText.Substring(2);
            }

            AutoRtnSearchTrxnNo(inputText, storeNo);
        }

        /// <summary>
        /// Restart AutoRtn
        /// </summary>
        void AutoRtnResetStart()
        {
            SaleIntialize();
            m_saleView.ItemsGrid_DataInitialize(true);
            m_summaryData.Update(this.BasketItems.ToArray());
            m_saleView.UpdateSummary(m_summaryData);
            m_saleView.UpdatePayList(this.BasketPays, false);

            m_saleView.ProcessState = SaleProcessState.InputStarted;
        }

        /// <summary>
        /// (1) 영수증 정보가 조회 되면 표시 되는 화면 입니다.
        /// (2) 영수증 자료 조회 방법
        ///     ① Local Tran DB에서 sat010t Table 을 조회 한다. 있으면 Tran 분석하여 화면에 표시 한다.
        ///     ② Local에 해당 Tran이 없으면 점서버와 통신(전문구분:PQ04)하여 Tran 정보를 수신하여 화면에 표시 한다.
        ///     ③ 본부 서버와 통신(전문구분:PQ11) 하여 사은품 지급 정보를 확인 한다.        /// 
        /// </summary>
        /// <param name="trxnNo">'A' + YYMMDD+POS번호(4자리) + 거래번호(4자리) + 'A'</param>
        /// 2015.09.11 정광호 수정 바코드스캔이 기존 14자리에서 StoreNo를 포함한 16자리로 변경됨으로 인한 수정
        void AutoRtnSearchTrxnNo(string trxnNo, string storeNo)
        {
            ReportInvalidState(InvalidDataInputState.Waiting);

            // 사은품증정내역
            TksPresentList = new List<PQ11RespData>();

            string saleDate = "20" + trxnNo.Substring(0, 6);
            string posNo = trxnNo.Substring(6, 4);
            string trNo = TypeHelper.ToInt32(trxnNo.Substring(10)).ToString("d6");

            // 판매거래인지 확인한다
            var db = TranDbHelper.InitInstance();
            try
            {
                var query = "GetSaleTrxnHeader".POSSLQuerySQL();
                var cont = db.ExecuteScalar(query,
                    new string[] {
                        "@CD_STORE", "@NO_POS", "@DD_SALE", "@NO_TRXN"
                    },
                    new object[] {
                        storeNo, 
                        posNo, saleDate, trNo
                    });

                if (cont != null && !string.IsNullOrEmpty(cont.ToString()))
                {
                    BasketHeader header = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), cont.ToString());

                    if (!NetCommConstants.TRXN_TYPE_SALE.Equals(header.TrxnType))
                    {
                        AutoRtnOnSearchTRResult(2, null, null, null, null, null, null);
                        return;
                    }

                    // header all baskets
                    AutoRtnLoadBaskets(saleDate, posNo, trNo);
                }
                else
                {
                    // load from server
                    var pq04 = new PQ04DataTask(new WSWD.WmallPos.FX.Shared.NetComm.Request.PQ.PQ04ReqData()
                    {
                        PosNo = posNo,
                        SaleDate = saleDate,
                        StoreNo = storeNo,
                        TrxnNo = trNo
                    });

                    // m_saleView.ShowProgress(true);
                    pq04.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(AutoRtnPQ04_Errored);
                    pq04.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(AutoRtnPQ04_TaskCompleted);
                    pq04.ExecuteTask();
                }
            }
            catch (Exception ex)
            {
                ReportInvalidState(InvalidDataInputState.NetworkError);
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                db.Dispose();
            }
        }

        void AutoRtnPQ04_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            // m_saleView.ShowProgress(false);
            if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.Success)
            {
                var records = responseData.DataRecords.ToDataRecords<PQ04RespData>();
                AutoRtnParseBaskets(records);
            }
            else
            {
                AutoRtnOnSearchTRResult(1, null, null, null, null, null, null);
            }
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="lastException"></param>
        void AutoRtnPQ04_Errored(string errorMessage, Exception lastException)
        {
            // m_saleView.ShowProgress(false);
            AutoRtnOnSearchTRResult(3, null, null, null, null, null, null);
        }

        /// <summary>
        /// 로컬db에서 가져온다
        /// </summary>
        /// <param name="saleDate"></param>
        /// <param name="posNo"></param>
        /// <param name="noTrxn"></param>
        void AutoRtnLoadBaskets(string saleDate, string posNo, string noTrxn)
        {
            var db = TranDbHelper.InitInstance();
            try
            {
                string query = "M001LoadTransByNoTrxn".POSSLQuerySQL();
                var ds = db.ExecuteQuery(query,
                    new string[] {
                        "@DD_SALE", "@CD_STORE", "@NO_POS", "@NO_TRXN"
                    },
                    new object[] {
                        saleDate,
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        posNo,
                        noTrxn
                    });

                BasketHeader bh = null;
                BasketCashRecpt bcr = null;
                BasketPointSave bps = null;
                BasketSubTotal stt = null;
                List<BasketItem> basketItems = new List<BasketItem>();
                List<BasketPay> basketPays = new List<BasketPay>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var item = ds.Tables[0].Rows[i];
                    string vcCont = item["VC_CONT"].ToString();

                    if (i == 0)
                    {
                        bh = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), vcCont);
                    }
                    else if (i == 1)
                    {
                        bcr = (BasketCashRecpt)BasketCashRecpt.Parse(typeof(BasketCashRecpt), vcCont);
                    }
                    else if (i == 2)
                    {
                        bps = (BasketPointSave)BasketPointSave.Parse(typeof(BasketPointSave), vcCont);
                    }
                    else
                    {
                        if (vcCont.StartsWith(BasketTypes.BasketSubTotal))
                        {
                            stt = (BasketSubTotal)BasketSubTotal.Parse(typeof(BasketSubTotal), vcCont);
                        }
                        else
                        {
                            if (vcCont.StartsWith(BasketTypes.BasketItem))
                            {
                                basketItems.Add((BasketItem)BasketItem.Parse(typeof(BasketItem), vcCont));
                            }
                            else if (vcCont.StartsWith(BasketTypes.BasketPay))
                            {
                                basketPays.Add((BasketPay)BasketBase.Parse(vcCont));
                            }
                        }

                    }
                }

                AutoRtnOnSearchTRResult(bh != null ? 0 : 1, bh, bcr, bps, stt, basketItems.ToArray(), basketPays.ToArray());
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
        /// 서버에서 받은 PQ04Data
        /// </summary>
        /// <param name="records"></param>
        void AutoRtnParseBaskets(PQ04RespData[] records)
        {
            // vccont to basket pay, by each type
            List<BasketBase> baskets = new List<BasketBase>();
            for (int i = 0; i < records.Length; i++)
            {
                baskets.Add(BasketBase.Parse(records[i].TrxnInfo));
            }

            AutoRtnParseBaskets(baskets.ToArray());
        }

        /// <summary>
        /// 결과서버에서 받는다
        /// </summary>
        /// <param name="baskets"></param>
        void AutoRtnParseBaskets(BasketBase[] baskets)
        {
            BasketHeader header = (BasketHeader)baskets[0];

            if (!NetCommConstants.TRXN_TYPE_SALE.Equals(header.TrxnType))
            {
                AutoRtnOnSearchTRResult(2, null, null, null, null, null, null);
                return;
            }

            // atlease header, end, cashreceipt, pointsave, 1item, 1 payment
            if (baskets.Length < 6)
            {
                AutoRtnOnSearchTRResult(1, null, null, null, null, null, null);
                return;
            }

            try
            {
                var bh = (BasketHeader)baskets[0];
                var bcr = (BasketCashRecpt)baskets[1];
                var bps = (BasketPointSave)baskets[2];
                BasketSubTotal bst = null;
                List<BasketItem> bis = new List<BasketItem>();
                List<BasketPay> bpays = new List<BasketPay>();
                for (int i = 3; i < baskets.Length; i++)
                {
                    if (BasketTypes.BasketSubTotal.Equals(baskets[i].BasketType))
                    {
                        bst = (BasketSubTotal)baskets[i];
                    }
                    else if (BasketTypes.BasketItem.Equals(baskets[i].BasketType))
                    {
                        bis.Add((BasketItem)baskets[i]);
                    }
                    else if (BasketTypes.BasketPay.Equals(baskets[i].BasketType))
                    {
                        bpays.Add((BasketPay)baskets[i]);
                    }
                }

                AutoRtnOnSearchTRResult(bh != null ? 0 : 1, bh, bcr, bps, bst, bis.ToArray(), bpays.ToArray());
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                AutoRtnOnSearchTRResult(1, null, null, null, null, null, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultCode">0:정상,1:거래없음,2:판매거래아님,3:통신오류</param>
        /// <param name="baskets"></param>
        void AutoRtnOnSearchTRResult(int resultCode, BasketHeader header,
            BasketCashRecpt cashReceipt,
            BasketPointSave basketPointSave,
            BasketSubTotal basketSubtotal,
            BasketItem[] basketItems,
            BasketPay[] basketPays)
        {
            if (resultCode != 0)
            {
                // clear current UI;
                AutoRtnClearTrxnInfo();

                InvalidDataInputState state = InvalidDataInputState.None;
                if (resultCode == 1)
                {
                    state = InvalidDataInputState.AutoRtnNoTrans;
                }
                else if (resultCode == 2)
                {
                    state = InvalidDataInputState.NotSaleTrans;
                }
                else
                {
                    state = InvalidDataInputState.TrxnSvrCheckError;
                }

                ReportInvalidState(state);
                return;
            }

            #region 반품가능한지 상태알림

            if ("0".Equals(header.CancType))
            {
                // 이미반품됨
                if ("1".Equals(header.RfProcFg))
                {
                    ReportInvalidState(InvalidDataInputState.TransReturned);
                    return;
                }
            }
            else if ("1".Equals(header.CancType))
            {
                ReportInvalidState(InvalidDataInputState.CancelledTrxn);
                return;
            }
            else
            {
                ReportInvalidState(InvalidDataInputState.ReturnedTrxn);
                return;
            }

            #endregion

            this.BasketHeader = header;
            this.BasketCashReceipt = cashReceipt;
            this.BasketPointSave = basketPointSave;
            this.BasketSubTtl = basketSubtotal;

            this.BasketItems.Clear();
            this.BasketItems.AddRange(basketItems);

            this.BasketPays.Clear();
            this.BasketPays.AddRange(basketPays);

            // 사은품확인
            AutoRtnCheckTksEvent();
        }

        /// <summary>
        /// Update status to UI view
        /// </summary>
        /// <param name="hasTksEvent"></param>
        void AutoRtnUpdateTrxnInfo(bool hasTksEvent)
        {
            #region 옆부분에 거래정보표시

            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            keyValues.Add("#SALE_DATE", DateTimeUtils.FormatDateString(this.BasketHeader.SaleDate, "/"));
            keyValues.Add("#POS_NO", this.BasketHeader.PosNo);
            keyValues.Add("#TRXN_NO", this.BasketHeader.TrxnNo);
            keyValues.Add("#TRXN_TYPE", NetCommConstants.TrxnTypeToName(this.BasketHeader.TrxnType));

            keyValues.Add("#DATE_FULL", DateTimeUtils.FormatDateString(this.BasketHeader.OccrDate, "/") + " " +
                DateTimeUtils.FormatTimeString(this.BasketHeader.OccrTime, string.Empty));

            // 카드결제
            var cardPays = this.BasketPays.Where(p => NetCommConstants.PAYMENT_DETAIL_CARD.Equals(p.PayDtlCd)).ToList();
            if (this.BasketPays != null && cardPays != null && cardPays.Count > 0)
            {
                var cardTemplate = ReadTextResource("AutoRtnTrxnInfo_Card.txt");
                StringBuilder sbCard = new StringBuilder();
                sbCard.AppendLine();

                foreach (var cardPay in cardPays)
                {
                    var cp = (BasketPayCard)BasketPayCard.Parse(typeof(BasketPayCard), cardPay.ToString());

                    string cardInfo = cardTemplate;
                    cardInfo = cardInfo.Replace("#CARD_PAY", LABEL_PAY_CARD);
                    cardInfo = cardInfo.Replace("#CARD_NO", cp.CardNo.MaskData(MaskingDataType.CardNo));
                    cardInfo = cardInfo.Replace("#EXP_MY", cp.ExpMY.MaskData(MaskingDataType.CardExpMY));
                    cardInfo = cardInfo.Replace("#APPR_NO", cp.ApprNo);
                    sbCard.Append(cardInfo);
                    sbCard.AppendLine();
                }

                keyValues.Add("#CARD_PAY_LIST", sbCard.ToString());
            }
            else
            {
                keyValues.Add("#CARD_PAY_LIST", string.Empty);
            }

            // 포인트회원
            if (this.BasketPointSave != null)
            {
                keyValues.Add("#MEM_NO", this.BasketPointSave.NoPointMember);
                keyValues.Add("#MEM_NAME", this.BasketPointSave.PointNmMember);
                keyValues.Add("#POINT", string.Format("{0:#,##0}", TypeHelper.ToInt32(this.BasketPointSave.AmPoint)));
            }
            else
            {
                keyValues.Add("#MEM_NO", string.Empty);
                keyValues.Add("#MEM_NAME", string.Empty);
                keyValues.Add("#POINT", string.Empty);
            }

            // 현금영수증
            if (this.BasketCashReceipt != null && !string.IsNullOrEmpty(this.BasketCashReceipt.NoAppr))
            {
                keyValues.Add("#CASH_RCP", LABEL_CASHRCP_YES);
            }
            else
            {
                keyValues.Add("#CASH_RCP", LABEL_CASHRCP_NO);
            }

            keyValues.Add("#THKS_FG", hasTksEvent ? LABEL_TKS_FG : string.Empty);


            #endregion

            #region 상품리스티 & 결제내역업데이트

            // trxn info
            if (m_saleView.InvokeRequired)
            {
                m_saleView.BeginInvoke((MethodInvoker)delegate()
                {
                    m_saleView.AutoRtnShowTrnxInfo(keyValues);

                    m_saleView.ItemsGrid_DataInitialize(true);
                    m_saleView.RestoreFromBaskets(this.BasketItems.ToArray());

                    m_summaryData.Update(this.BasketItems.ToArray());
                    m_saleView.UpdateSummary(m_summaryData);
                    m_saleView.UpdatePayList(this.BasketPays, true);
                    m_saleView.ProcessState = SaleProcessState.AutoRtnReady;
                    m_saleView.InputText = string.Empty;
                });
            }
            else
            {
                m_saleView.AutoRtnShowTrnxInfo(keyValues);
                m_saleView.ItemsGrid_DataInitialize(true);
                m_saleView.RestoreFromBaskets(this.BasketItems.ToArray());

                m_summaryData.Update(this.BasketItems.ToArray());
                m_saleView.UpdateSummary(m_summaryData);
                m_saleView.UpdatePayList(this.BasketPays, true);
                m_saleView.ProcessState = SaleProcessState.AutoRtnReady;
                m_saleView.InputText = string.Empty;
            }

            #endregion
        }

        /// <summary>
        /// 자동반품 거래정보 상세의 카드정보 부분 가져오기
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        string ReadTextResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            resourceName = "WSWD.WmallPos.POS.SL.Resources." + resourceName;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 반품정보 지우기
        /// </summary>
        void AutoRtnClearTrxnInfo()
        {
            if (m_saleView.InvokeRequired)
            {
                m_saleView.BeginInvoke((MethodInvoker)delegate()
                {
                    m_saleView.AutoRtnShowTrnxInfo(null);

                    m_saleView.ItemsGrid_DataInitialize(true);

                    this.BasketItems.Clear();
                    m_summaryData.Update(this.BasketItems.ToArray());

                    m_summaryData = new SaleSummaryData();
                    m_saleView.UpdateSummary(m_summaryData);

                    this.BasketPays.Clear();
                    m_saleView.UpdatePayList(this.BasketPays, true);
                    m_saleView.InputText = string.Empty;
                });
            }
            else
            {
                m_saleView.AutoRtnShowTrnxInfo(null);

                m_saleView.ItemsGrid_DataInitialize(true);

                this.BasketItems.Clear();
                m_summaryData.Update(this.BasketItems.ToArray());

                m_summaryData = new SaleSummaryData();
                m_saleView.UpdateSummary(m_summaryData);

                this.BasketPays.Clear();
                m_saleView.UpdatePayList(this.BasketPays, true);
                m_saleView.InputText = string.Empty;
            }
        }

        #region 사은품업무처리

        /// <summary>
        /// 사은품여부확인,PQ11전문
        /// </summary>
        void AutoRtnCheckTksEvent()
        {
            m_saleView.ShowProgress(true);
            var pq11 = new PQ11DataTask(this.BasketHeader.SaleDate,
                this.BasketHeader.StoreNo, this.BasketHeader.PosNo, this.BasketHeader.TrxnNo);
            pq11.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(AutoRtnPQ11_Errored);
            pq11.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(AutoRtnPQ11_TaskCompleted);
            pq11.ExecuteTask();
        }

        void AutoRtnPQ11_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                var datas = responseData.DataRecords.ToDataRecords<PQ11RespData>();
                this.TksPresentList.Clear();
                this.TksPresentList.AddRange(datas);
                AutoRtnUpdateTrxnInfo(datas.Length > 0);
            }
            else
            {
                AutoRtnUpdateTrxnInfo(false);
            }
            m_saleView.ShowProgress(false);
        }

        void AutoRtnPQ11_Errored(string errorMessage, Exception lastException)
        {
            AutoRtnUpdateTrxnInfo(false);
            m_saleView.ShowProgress(false);
        }

        #endregion

        #endregion

        #region 반품처리한다

        /// <summary>
        /// 자동반품모드전환
        /// </summary>
        /// <returns></returns>
        bool AutoRtnChangeMode()
        {
            var res = ChangeSaleMode(SaleModes.AutoReturn, true, true, true);
            if (res == ChangeSaleModeStatus.InvalidCondition)
            {
                ReportInvalidState(InvalidDataInputState.InvalidKey);
            }

            return res == ChangeSaleModeStatus.Success;
        }

        /// <summary>
        /// 자동반품확정처리
        /// </summary>
        public void AutoRtnConfirmStart()
        {
            // 소계모드처럼 전환
            m_saleView.ProcessState = SaleProcessState.AutoRtnProcessing;

            // processing, cancel all keys
            m_keyEventProcessing = true;

            // 사은품증정내역 있으면 화면 오픈한다
            if (this.TksPresentList.Count > 0)
            {
                m_saleView.AutoRtnUpdateStatusMsg(SLExtensions.PAYMENT_DETAIL_AUTORTN_TKS_PRESENT, string.Empty);

                object returnData = null;
                var ret = m_saleView.AutoRtnShowTksPresentReturnPopup(this.TksPresentList, this.BasketHeader, out returnData);
                if (ret == DialogResult.No)
                {
                    m_keyEventProcessing = false;
                    m_saleView.ProcessState = SaleProcessState.AutoRtnReady;
                    m_saleView.AutoRtnUpdateStatusMsg(SLExtensions.PAYMENT_DETAIL_AUTORTN_TKS_PRESENT, "CANC");
                    return;
                }

                this.TksPresentList.Clear();
                if (ret == DialogResult.Yes)
                {
                    var datas = (List<PQ11RespData>)returnData;
                    this.TksPresentList.AddRange(datas);
                }
            }

            // 변수생성
            // 반품 결과 basket list
            BasketPaysAutoRtn = new List<BasketPay>();
            ForcePays = new List<BasketBase>();

            #region 순서대로 처리할 결제수단리스트 생성

            this.ProcessPays = new List<BasketPay>();

            // 반품 중일 때 빠져 나갈수 있는지 확인여부
            this.m_autoRtnCancellable = true;
            this.m_autoRtnFirstVANPay = true;

            // 기타
            var otherPays = this.BasketPays.Where(p => !NetCommConstants.PAYMENT_DETAIL_TICKET_OTHER.Equals(p.PayDtlCd)).ToArray();
            this.ProcessPays.AddRange(otherPays);

            // 타사상품권
            var otherTickets = this.BasketPays.Where(p => NetCommConstants.PAYMENT_DETAIL_TICKET_OTHER.Equals(p.PayDtlCd)).ToArray();
            this.ProcessPays.AddRange(otherTickets);

            #endregion

            // ProcessPays에서 하나씩 나간다
            m_autoRtnPayProcessIdx = -1;
            AutoRtnProcessNextBasketPay(false);
        }

        /// <summary>
        /// 자동반품확정처리 하고나서
        /// 취소한다 (결제취소진행전)
        /// 첫결제 취소승인 하기전에 취소함
        /// </summary>
        void AutoRtnConfirmStop()
        {
            // 소계모드처럼 전환
            m_saleView.ProcessState = SaleProcessState.AutoRtnReady;

            // not processing, cancel all keys
            m_keyEventProcessing = false;

            // Update status
            m_saleView.AutoRtnUpdateStatusMsg(SLExtensions.PAYMENT_DETAIL_AUTORTN_CANCEL_STARTED, string.Empty);
        }

        #region 결제수단 하나씩 처리

        /// <summary>
        /// 다음결제수단 취소처리
        /// </summary>
        void AutoRtnProcessNextBasketPay(bool retry)
        {
            // 재시도 하는지?
            if (!retry)
            {
                // 재시도 안하면 다음결게수단으로 진행함
                // 다음건
                m_autoRtnPayProcessIdx++;
            }

            // 모든 결제수단 처리 완료되면, 현금영수증취소 처리
            if (m_autoRtnPayProcessIdx > this.ProcessPays.Count - 1)
            {
                if (this.ProcessPays.Count == m_autoRtnPayProcessIdx)
                {
                    // 현금영수증처리
                    AutoRtnProcessCashRcp();
                }
                else
                {
                    // 포인트적립취소
                    AutoRtnProcessPointSave();
                }
                return;
            }

            var bp = this.ProcessPays[m_autoRtnPayProcessIdx];
            m_saleView.AutoRtnUpdateStatusMsg(bp.PayGrpCd, bp.PayDtlCd);

            if (NetCommConstants.PAYMENT_DETAIL_CARD.Equals(bp.PayDtlCd))
            {
                m_saleView.BeginInvoke((MethodInvoker)delegate()
                {
                    AutoRtnProcessCardPay(bp);
                });
            }
            else if (NetCommConstants.PAYMENT_DETAIL_CASH_IC.Equals(bp.PayDtlCd))
            {
                m_saleView.BeginInvoke((MethodInvoker)delegate()
                {
                    AutoRtnProcessCashIC(bp);
                });
            }
            else if (NetCommConstants.PAYMENT_DETAIL_POINT.Equals(bp.PayDtlCd))
            {
                m_saleView.BeginInvoke((MethodInvoker)delegate()
                {
                    AutoRtnProcessPointUsePay(bp);
                });
            }
            else if (NetCommConstants.PAYMENT_DETAIL_TICKET_OTHER.Equals(bp.PayDtlCd))
            {
                m_saleView.BeginInvoke((MethodInvoker)delegate()
                {
                    AutoRtnProcessOtherTickets();
                });
            }
            else
            {
                AutoRtnProcessOtherPay(bp);
            }
        }

        #region 카드취소처리

        /// <summary>
        /// 자동반품 카드취소
        /// </summary>
        /// <param name="cardPay"></param>
        void AutoRtnProcessCardPay(BasketPay pay)
        {
            BasketPayCard cardPay = (BasketPayCard)BasketPayCard.Parse(typeof(BasketPayCard), pay.ToString());

            #region 이부분 임시 주석처리함

            // APP CARD 확인
            //if ("P".Equals(cardPay.InputType))
            //{
            //    // 카드취소 팝업 오픈
            //    m_saleView.BeginInvoke((MethodInvoker)delegate()
            //    {
            //        AutoRtnProcessCardPayAppCard(cardPay);
            //    });
            //}            
            //{

            // 신용카드, 은력카드, DCC서비스, 전화승인
            // Card Sign Popup
            // ERCard 인경우
            #endregion

            #region 여전법 변경 - 주석처리

            /*
            // 은련카드인경우비밀번호 팝업호출
            if ("Y".Equals(cardPay.EunCardFg))
            {
                m_saleView.BeginInvoke((MethodInvoker)delegate()
                {
                    string workKeyIndex = string.Empty;
                    string cardPin = string.Empty;
                    m_saleView.ShowERCardPasswordPopup(cardPay.CardNo, out workKeyIndex, out cardPin);
                    m_cardCancCVM = string.IsNullOrEmpty(cardPin) ? "N" : "P";
                    AutoRtnProcessCardPayVANRequest(cardPay,
                        GetPV11ReqDataAdd(),
                        string.Empty, workKeyIndex, cardPin);
                });
            }
            else
            {
                m_saleView.BeginInvoke((MethodInvoker)delegate()
                {
                    string signData = string.Empty;
                    signData = m_saleView.ShowCardSignPopup(TypeHelper.ToInt32(cardPay.PayAmt));
                    m_cardCancCVM = string.IsNullOrEmpty(signData) ? "N" : "S";

                    /*
                    // 전화승인
                    if ("2".Equals(cardPay.ApprState))
                    {
                        // 전화승인
                        AutoRtnProcessCardPayTelCard(cardPay);
                    }
                    else
                    {
                        AutoRtnProcessCardPayVANRequest(cardPay, signData, string.Empty, string.Empty);
                    }
                    */
            // Loc changed: 10.20수정
            // 전화승인은 밴승인함
            //AutoRtnProcessCardPayVANRequest(cardPay,
            //    GetPV11ReqDataAdd(), signData, string.Empty, string.Empty);
            //});
            //}
            //}*/

            #endregion

            #region 여전법 추가 0620, 카드결재 모두 카드결제 화면으로 표시

            // 카드취소 팝업 오픈
            m_saleView.BeginInvoke((MethodInvoker)delegate()
            {
                // AutoRtnProcessCardPayAppCard(cardPay);
                AutoRtnProcessCardPayCard(cardPay);
            });

            #endregion


        }

        /// <summary>
        /// 앱카드처리
        /// 
        /// AppCard 사용일때
        /// 
        /// </summary>
        /// <param name="cardPay"></param>
        void AutoRtnProcessCardPayAppCard(BasketPayCard cardPay)
        {
            // 카드취소 팝업 오픈
            object returnData = null;
            string errorCode = string.Empty;
            string errorMessage = string.Empty;
            var res = m_saleView.ShowCardPopup(cardPay.PayAmt, cardPay.ApprAmtIncVat,
                m_autoRtnCancellable, cardPay,
                GetPV11ReqDataAdd(), PayCardMode.AppCard,
                out returnData, out errorCode, out errorMessage);
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                // returnData=null인경우, 강제진행(취소함);
                if (returnData == null)
                {
                    AutoRtnProcessCardPayMakeBasket(null, errorCode, errorMessage);
                }
                else
                {
                    BasketPay rbpc = (BasketPay)returnData;
                    BasketPaysAutoRtn.Add(rbpc);
                    AutoRtnProcessNextBasketPay(false);
                }
            }
            else
            {
                var ret = m_saleView.AutoRtnShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.SomeError,
                    MSG_AUTORTN_APP_CARD_CANCELLED_ERROR, m_autoRtnFirstVANPay);
                if (ret == DialogResult.Yes)
                {
                    AutoRtnProcessNextBasketPay(true);
                }
                else if (ret == DialogResult.Cancel)
                {
                    AutoRtnConfirmStop();
                }
                else
                {
                    AutoRtnProcessCardPayMakeBasket(null, string.Empty, MSG_AUTORTN_APP_CARD_CANCELLED_ERROR);
                }
            }
        }

        /// <summary>
        /// 여전법 추가 0620
        /// 반품 시 카드결제 팝업
        /// </summary>
        /// <param name="cardPay"></param>
        void AutoRtnProcessCardPayCard(BasketPayCard cardPay)
        {
            PayCardMode cardMode = PayCardMode.CreditCard;
            //KSK_20170403
            //if ("Y".Equals(cardPay.EunCardFg))
            if ("P".Equals(cardPay.InputType))
            {
                cardMode = PayCardMode.AppCard;
            }
            else if ("I".Equals(cardPay.InputType))
            {
                cardMode = PayCardMode.CashICCard;
            }
            else if ((cardPay.ExpMY.Substring(0,1) == "*" && cardPay.ApprState == "2") || (cardPay.ExpMY.Substring(0,1) != "*" && cardPay.PrefixCode[5] == '2'))
            {
                cardMode = PayCardMode.TelManualCard;
            }
            else if ((cardPay.ExpMY.Substring(0, 1) == "*" && cardPay.EunCardFg == "Y") || (cardPay.ExpMY.Substring(0, 1) != "*" && cardPay.MerchantCode[8] == 'Y'))
            {
                cardMode = PayCardMode.ERCard;
            }

            // 카드취소 팝업 오픈
            object returnData = null;
            string errorCode = string.Empty;
            string errorMessage = string.Empty;
            var res = m_saleView.ShowCardPopup(cardPay.PayAmt, cardPay.ApprAmtIncVat,
                m_autoRtnCancellable, cardPay,
                GetPV11ReqDataAdd(), cardMode,
                out returnData, out errorCode, out errorMessage);
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                // returnData=null인경우, 강제진행(취소함);
                if (returnData == null)
                {
                    AutoRtnProcessCardPayMakeBasket(null, errorCode, errorMessage);
                }
                else
                {
                    BasketPay rbpc = (BasketPay)returnData;
                    BasketPaysAutoRtn.Add(rbpc);
                    AutoRtnProcessNextBasketPay(false);
                }
            }
            else
            {
                var ret = m_saleView.AutoRtnShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.SomeError,
                    MSG_AUTORTN_APP_CARD_CANCELLED_ERROR, m_autoRtnFirstVANPay);
                if (ret == DialogResult.Yes)
                {
                    AutoRtnProcessNextBasketPay(true);
                }
                else if (ret == DialogResult.Cancel)
                {
                    AutoRtnConfirmStop();
                }
                else
                {
                    // 강제취소
                    AutoRtnProcessCardPayMakeBasket(null, string.Empty, MSG_AUTORTN_APP_CARD_CANCELLED_ERROR);
                }
            }
        }

        /// <summary>
        /// 전화승인결제
        /// </summary>
        /// <param name="cardPay"></param>
        void AutoRtnProcessCardPayTelCard(BasketPayCard cardPay)
        {
            BasketPayCard pay = (BasketPayCard)BasketPayCard.Parse(typeof(BasketPayCard), cardPay.ToString());
            pay.CVM = m_cardCancCVM;
            pay.OTApprNo = cardPay.ApprNo;
            pay.OTSaleDate = cardPay.RealApprProcDate;
            pay.CurDateCancType = DateTime.Today.ToString("yyyyMMdd").Equals(this.BasketHeader.SaleDate) ? "1" : "0";
            BasketPaysAutoRtn.Add(pay);
            AutoRtnProcessNextBasketPay(false);
        }

        /// <summary>
        /// Loc added on 10.24
        /// 자동반품/판매 중인 거래의 
        /// - 점포정보
        /// - 포스정보
        /// - 첫상품정보 가져온다
        /// 이함수는 거래결제 시작시 사용함 (카드, 현금영수증)
        /// 
        /// 여전법 변경
        /// PV11ReqDataAdd => PV21ReqDataAdd
        /// </summary>
        /// <returns></returns>
        PV21ReqDataAdd GetPV11ReqDataAdd()
        {
            Encoding transferEnc = Encoding.GetEncoding(NetCommConstants.TRANFER_ENCODING);
            var storeNameBytes = transferEnc.GetBytes(ConfigData.Current.AppConfig.PosInfo.StoreName);
            var addData = new PV21ReqDataAdd()
            {
                StoreCode = ConfigData.Current.AppConfig.PosInfo.StoreNo,
                // 여전볍 변경, byte일경우 hexa로 변환함
                StoreName = ConfigData.Current.AppConfig.PosInfo.StoreName, // 20170209.ojg Hex String 에서 string으로 변경
                //StoreName = storeNameBytes.BytesToHexString(),      
                SaleDate = ConfigData.Current.AppConfig.PosInfo.SaleDate,
                PosNo = ConfigData.Current.AppConfig.PosInfo.PosNo,
                TrxnNo = ConfigData.Current.AppConfig.PosInfo.TrxnNo
            };

            // 첫상품가져오기
            /*
             * 등록된 1번째 상품 정보
                (Basket.품번(6자리) + 
                 Basket.품목(4자리) +
                 Basket.상품구분(2) +
                 '0')
                만약 취소된 상품이면 다음 상품 정보로 지정
             * */
            BasketItem firstItem = null;
            foreach (var item in this.BasketItems)
            {
                if (item.FgCanc == "0")
                {
                    firstItem = item;
                    break;
                }
            }

            if (firstItem != null)
            {
                addData.ItemCode = firstItem.CdClass + firstItem.CdItem + firstItem.FgMargin + "0";
                var itemNameBytes = transferEnc.GetBytes(firstItem.NmItem);
                
                //addData.ItemName = itemNameBytes.BytesToHexString();// firstItem.NmItem;, 여전법 변경
                addData.ItemName = firstItem.NmItem; //, 여전법 변경    20170209.ojg Hex String -> String
                
            }

            return addData;
        }

        /// <summary>
        /// 승인요청
        /// Loc changed on 10.24
        /// 
        /// 여전법 변경 0623
        /// 사용안함
        /// </summary>
        /// <param name="cardPay"></param>
        /// <param name="addData">전무추가정보</param>
        /// <param name="signData"></param>
        /// <param name="workKeyIndex"></param>
        /// <param name="cardPin"></param>
        void AutoRtnProcessCardPayVANRequest(BasketPayCard cardPay,
            PV21ReqDataAdd addData,
            string signData, string workKeyIndex, string cardPin)
        {
            // VAN사 승인요청
            var presenter = new PYP001presenter(this);
            presenter.RequestVANCardPayment(0,
                cardPay.TrackII,
                "@".Equals(cardPay.InputType) ? "K" : "S",
                cardPay.Halbu, signData,
                cardPay.PayAmt, cardPay.ApprAmtIncVat,
                cardPay.EunCardFg, string.Empty,
                workKeyIndex, cardPin, cardPay.RealApprProcDate,
                cardPay.ApprNo, null, addData);
        }

        #region 자동반품 - VAN승인결과처리

        /// <summary>
        /// 사용안함
        /// </summary>
        /// <param name="showProgress"></param>
        public void ShowProgressMessage(bool showProgress)
        {
            //m_saleView.ShowProgress(showProgress);
        }

        /// <summary>
        /// 오류발생시, 재시도 할수 있게 만들어야함
        /// 신용카드 및 현금영수증 같이 처리
        /// 
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="errorMessage"></param>
        /// <param name="errorCode"></param>
        /// <param name="viewTag"></param>
        public void ShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType errorType, string errorMessage, string errorCode, string viewTag)
        {
            if (m_saleView.InvokeRequired)
            {
                m_saleView.BeginInvoke((MethodInvoker)delegate()
                {
                    var res = m_saleView.AutoRtnShowErrorMessage(errorType, string.IsNullOrEmpty(errorCode) ? errorMessage :
                        string.Format("[{0}] {1}", errorCode, errorMessage), m_autoRtnFirstVANPay);
                    if (res == DialogResult.Yes)
                    {
                        AutoRtnProcessNextBasketPay(true);
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        AutoRtnConfirmStop();
                    }
                    else
                    {
                        if ("CARD".Equals(viewTag))
                        {
                            AutoRtnProcessCardPayMakeBasket(null, errorCode, errorMessage);
                        }
                        else
                        {
                            // 현금영수증인경우
                            // 여전법 변경 주석처리 0630
                            AutoRtnProcessCashReceiptMakeBasket(null, errorCode, errorMessage);
                        }
                    }
                });
            }
            else
            {
                var res = m_saleView.AutoRtnShowErrorMessage(errorType, string.IsNullOrEmpty(errorCode) ? errorMessage :
                        string.Format("[{0}] {1}", errorCode, errorMessage), m_autoRtnFirstVANPay);
                if (res == DialogResult.Yes)
                {
                    AutoRtnProcessNextBasketPay(true);
                }
                else if (res == DialogResult.Cancel)
                {
                    AutoRtnConfirmStop();
                }
                else
                {
                    // 현금영수증인경우
                    if (this.ProcessPays.Count == m_autoRtnPayProcessIdx)
                    {
                        AutoRtnProcessCashReceiptMakeBasket(null, errorCode, errorMessage);
                    }
                    else
                    {
                        AutoRtnProcessCardPayMakeBasket(null, errorCode, errorMessage);
                    }
                }
            }
        }

        private string _SingData = string.Empty;

        /// <summary>
        /// 카드결제추소 정상승인시
        /// 
        /// 여전법 변경
        /// PV01RespData > PV21RespData
        /// </summary>
        /// <param name="respData"></param>
        public void OnReturnSuccess(WSWD.WmallPos.FX.Shared.NetComm.Response.PV.PV21RespData respData, string strSignData)
        {
            _SingData = strSignData;
            AutoRtnProcessCardPayMakeBasket(respData, null, null);
        }

        #endregion

        /// <summary>
        /// 자동반품-카드취소승인성공
        /// 강제진행 취소/강제취소도 여기서 처리한다
        ///     - respData = null
        ///     
        /// 여전법 변경
        /// PV01RespData > PV21RespData
        /// </summary>
        /// <param name="respData">null 일때 강제취소</param>
        void AutoRtnProcessCardPayMakeBasket(PV21RespData respData, string errorCode, string errorMessage)
        {
            var bp = this.ProcessPays[m_autoRtnPayProcessIdx];
            var orgCardPay = (BasketPayCard)bp;
            BasketPayCard cardBasket = null;

            //KSK_20170403
            //cardBasket = (BasketPayCard)BasketPayCard.Parse(typeof(BasketPayCard), bp.ToString());

            String TmpTot = bp.ToString();
            String TmpSub1 = string.Empty;
            string tmpSub2 = string.Empty;


            if (TmpTot.Substring(97, 1) != "*")
            {
                TmpSub1 = TmpTot.Substring(0, 224);
                tmpSub2 = TmpTot.Substring(224, TmpTot.Length - 225);
                TmpTot = String.Empty;
                TmpTot = TmpSub1 + "          " + tmpSub2;

                TmpSub1 = TmpTot.Substring(0, 373);
                tmpSub2 = TmpTot.Substring(373, TmpTot.Length - 374);
                TmpTot = String.Empty;
                TmpTot = TmpSub1 + "000000000" + tmpSub2;

                cardBasket = (BasketPayCard)BasketPayCard.Parse(typeof(BasketPayCard), TmpTot);
            }
            else
            {
                cardBasket = (BasketPayCard)BasketPayCard.Parse(typeof(BasketPayCard), bp.ToString());
            }
  
            if (errorCode != null)
            {
                cardBasket.ApprState = "2"; // 전화승인
            }
            else
            {
                cardBasket.ApprState = "1"; // 자동승인
            }
            
            cardBasket.OTSaleDate = orgCardPay.RealApprProcDate;
            cardBasket.OTApprNo = orgCardPay.ApprNo;
            cardBasket.CurDateCancType = DateTime.Today.ToString("yyyyMMdd").Equals(this.BasketHeader.SaleDate) ? "1" : "0";
            cardBasket.CVM = m_cardCancCVM;

            if (errorCode == null && respData != null)
            {
                cardBasket.CardNm = respData.CardNm;
                cardBasket.IssueComCd = respData.IssueComCd;
                cardBasket.IssueComNm = respData.IssueComNm;
                cardBasket.MaeipComCd = respData.MaeipComCd;
                cardBasket.MaeipComNm = respData.MaeipComNm;

                cardBasket.ApprNo = respData.ApprNo;
                cardBasket.RealApprProcDate = respData.ApprDate;
                cardBasket.RealApprProcTime = respData.ApprTime;

                cardBasket.VanId = respData.ApprVanCode;
                cardBasket.MerchantCode = respData.MerchantsCode;
            }
            else
            {
                //KSK_20170403
                //cardBasket.ApprNo = string.Empty;
                cardBasket.ApprNo = orgCardPay.ApprNo;
                cardBasket.CancFg = "1";

                cardBasket.RealApprProcDate = DateTime.Today.ToString("yyyyMMdd");
                cardBasket.RealApprProcTime = DateTime.Now.ToString("HHmmss");

                cardBasket.ForceCancFg = "1";
                cardBasket.CancRcvCode = errorCode;
                cardBasket.CancRcvMsg = errorMessage;

                ForcePays.Add(cardBasket);
            }

            // 반품중인데 취소못하게
            m_autoRtnCancellable = false;
            m_autoRtnFirstVANPay = false;

            BasketPaysAutoRtn.Add(cardBasket);

            if (!string.IsNullOrEmpty(_SingData))
            {
                POSDeviceManager.SignPad.SaveSignData(cardBasket.ApprNo);
                _SingData = string.Empty;
            }

            // 다음결제수단
            AutoRtnProcessNextBasketPay(false);
        }

        #endregion

        #region 현금IC취소처리

        /// <summary>
        /// 현금IC인경우 현금IC결제 팝업에서 처리한다
        /// </summary>
        /// <param name="pay"></param>
        void AutoRtnProcessCashIC(BasketPay pay)
        {
            var cashIC = (BasketCashIC)BasketCashIC.Parse(typeof(BasketCashIC), pay.ToString());
            object returnData = null;
            string errorCode = string.Empty;
            string errorMessage = string.Empty;
            var ret = m_saleView.ShowCashICPopup(TypeHelper.ToInt32(cashIC.PayAmt),
                TypeHelper.ToInt32(cashIC.ApprAmtIncVat), cashIC, m_autoRtnFirstVANPay,
                out returnData, out errorCode, out errorMessage);

            if (ret == System.Windows.Forms.DialogResult.OK)
            {
                BasketCashIC cashICPay = (BasketCashIC)returnData;
                this.BasketPaysAutoRtn.Add(cashICPay);

                // 강제진행 오류 리스트
                if (!string.IsNullOrEmpty(errorCode) || !string.IsNullOrEmpty(errorMessage))
                {
                    ForcePays.Add(cashIC);
                }

                AutoRtnProcessNextBasketPay(false);
            }
            else
            {
                // 닫기누름 - 진행안함 - 결제
                AutoRtnConfirmStop();
            }
        }

        #endregion

        #region 자동반품 - 포인트사용

        /// <summary>
        /// 포인트사용 결제취소
        /// </summary>
        /// <param name="pay"></param>
        void AutoRtnProcessPointUsePay(BasketPay pay)
        {
            // 상태업데이트
            m_saleView.AutoRtnUpdateStatusMsg(NetCommConstants.PAYMENT_DETAIL_POINT, NetCommConstants.PAYMENT_DETAIL_POINT);

            BasketPoint bpoint = (BasketPoint)pay;

            // 반품시 적용, 매출일(8)+점(4)+포스(4)+거래번호(6)
            string otApprInfo = this.BasketHeader.SaleDate +
                 this.BasketHeader.StoreNo.PadRight(4, ' ') + this.BasketHeader.PosNo + this.BasketHeader.TrxnNo;
            var pp04 = new PP04DataTask("2", otApprInfo, bpoint.ApprovalNo,
                bpoint.CustNo, bpoint.CardNo, bpoint.PayAmt);
            pp04.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(AutoRtnPP04_TaskCompleted);
            pp04.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(AutoRtnPP04_Errored);
            pp04.ExecuteTask();
        }

        /// <summary>
        /// 포인트사용취소 결과
        /// </summary>
        /// <param name="data"></param>
        void AutoRtnProcessPointUseComplete(PP04RespData data)
        {
            // 1st Payment 아닌거로 설정
            m_autoRtnFirstVANPay = false;

            var pay = this.ProcessPays[m_autoRtnPayProcessIdx];
            BasketPoint bpoint = (BasketPoint)pay;

            // 취소basket생성
            // 포인트사용 TRAN (결제-현금 TRAN 사용)
            var pointPay = (BasketPoint)(BasketPoint.Parse(typeof(BasketPoint), bpoint.ToString()));
            pointPay.DealApprovalNo = bpoint.ApprovalNo;

            if (data != null)
            {
                pointPay.BalancePoint = data.PayAfterPoint;
                pointPay.BalanceAmt = data.PayAfterAmt;
                pointPay.ApprovalNo = data.ApprNo;
            }
            else
            {
                pointPay.ApprovalNo = string.Empty;
            }

            this.BasketPaysAutoRtn.Add(pointPay);

            // 강제진행 항목
            if (data == null)
            {
                ForcePays.Add(pointPay);
            }

            AutoRtnProcessNextBasketPay(false);
        }

        #region 포인트사용 전문

        void AutoRtnPP04_Errored(string errorMessage, Exception lastException)
        {
            if (m_saleView.InvokeRequired)
            {
                m_saleView.BeginInvoke((MethodInvoker)delegate()
                {
                    var ret = m_saleView.AutoRtnShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.SomeError, errorMessage,
                        m_autoRtnFirstVANPay);
                    if (ret == DialogResult.Yes)
                    {
                        AutoRtnProcessNextBasketPay(true);
                    }
                    else if (ret == DialogResult.Cancel)
                    {
                        AutoRtnConfirmStop();
                    }
                    else
                    {
                        // 포인트사용
                        AutoRtnProcessPointUseComplete(null);
                    }
                });
            }
            else
            {
                var ret = m_saleView.AutoRtnShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.SomeError, errorMessage,
                    m_autoRtnFirstVANPay);
                if (ret == DialogResult.Yes)
                {
                    AutoRtnProcessNextBasketPay(true);
                }
                else if (ret == DialogResult.Cancel)
                {
                    AutoRtnConfirmStop();
                }
                else
                {
                    AutoRtnProcessPointUseComplete(null);
                }
            }
        }

        void AutoRtnPP04_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                var data = responseData.DataRecords.ToDataRecords<PP04RespData>()[0];
                AutoRtnProcessPointUseComplete(data);
            }
            else
            {
                if (m_saleView.InvokeRequired)
                {
                    m_saleView.BeginInvoke((MethodInvoker)delegate()
                    {
                        var ret = m_saleView.AutoRtnShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.SomeError,
                            responseData.Response.ErrorMessage, m_autoRtnFirstVANPay);
                        if (ret == DialogResult.Yes)
                        {
                            AutoRtnProcessNextBasketPay(true);
                        }
                        else if (ret == DialogResult.Cancel)
                        {
                            AutoRtnConfirmStop();
                        }
                        else
                        {
                            AutoRtnProcessPointUseComplete(null);
                        }
                    });
                }
                else
                {
                    var ret = m_saleView.AutoRtnShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.SomeError,
                        responseData.Response.ErrorMessage, m_autoRtnFirstVANPay);
                    if (ret == DialogResult.Yes)
                    {
                        AutoRtnProcessNextBasketPay(true);
                    }
                    else if (ret == DialogResult.Cancel)
                    {
                        AutoRtnConfirmStop();
                    }
                    else
                    {
                        AutoRtnProcessPointUseComplete(null);
                    }
                }
            }
        }

        #endregion

        #endregion

        #region 타사상품권취소처리 - 한껀번에

        /// <summary>
        /// 타사상품권취소처리 - 한껀번에
        /// </summary>
        void AutoRtnProcessOtherTickets()
        {
            // 타사상품권 BASKET 전체 갖는다
            // Get from current index + until not other ticker
            int idx = m_autoRtnPayProcessIdx; // 2, 3tick
            int totalAmt = 0;
            int ticketCount = 0;
            for (int i = idx; i < this.ProcessPays.Count; i++)
            {
                var bp = this.ProcessPays[i];
                if (NetCommConstants.PAYMENT_DETAIL_TICKET_OTHER.Equals(bp.PayDtlCd))
                {
                    totalAmt += TypeHelper.ToInt32(bp.PayAmt) - TypeHelper.ToInt32(bp.BalAmt);
                    ticketCount++;
                }
            }

            object returnData = null;
            var ret = m_saleView.ShowOtherTicketPopup(totalAmt, new List<BasketPay>(), 0, true, out returnData);
            if (ret == DialogResult.OK)
            {
                m_autoRtnPayProcessIdx += ticketCount - 1;

                var pays = (List<BasketPay>)returnData;

                // add to result
                foreach (var pay in pays)
                {
                    this.BasketPaysAutoRtn.Add(pay);
                }
            }

            // next payment
            AutoRtnProcessNextBasketPay(false);
        }

        #endregion

        #region 기타결제수단취소처리 (현금, 기타카드, 포인트사용 등)

        void AutoRtnProcessOtherPay(BasketPay pay)
        {
            #region 현금결제
            if (NetCommConstants.PAYMENT_DETAIL_CASH.Equals(pay.PayDtlCd))
            {
                BasketPayCash payCash = (BasketPayCash)BasketPayCash.Parse(typeof(BasketPayCash), pay.ToString());
                payCash.PayAmt = Convert.ToString(TypeHelper.ToInt32(payCash.PayAmt) - TypeHelper.ToInt32(payCash.BalAmt));
                payCash.BalAmt = string.Empty;

                this.BasketPaysAutoRtn.Add(payCash);
                AutoRtnProcessNextBasketPay(false);
                return;
            }
            #endregion

            // 현금외에
            var opay = (BasketPay)BasketBase.Parse(pay.ToString());
            this.BasketPaysAutoRtn.Add(opay);
            AutoRtnProcessNextBasketPay(false);
        }

        #endregion

        #region 자동반품 - 현금영수증취소

        void AutoRtnProcessCashRcp()
        {
            // 현금영수증 발행 했으면, 취소처리한다
            if (!string.IsNullOrEmpty(this.BasketCashReceipt.NoAppr))
            {
                //AutoRtnProcessCashRcpB();
                AutoRtnProcessCashReceiptPopup();
                return;
            }

            // 포인트적립취소
            AutoRtnProcessNextBasketPay(false);
        }

        /// <summary>
        /// 여전법 추가 0623
        /// 
        /// 
        /// 현금영수증전문 만들어서
        /// 서버와 통신
        /// 
        /// POS_PY_P015 팝업 사용
        /// 추가 보낼 것
        /// - 
        /// 
        /// </summary>
        void AutoRtnProcessCashRcpB()
        {
            m_saleView.AutoRtnUpdateStatusMsg(NetCommConstants.PAYMENT_DETAIL_CASHRCP, NetCommConstants.PAYMENT_DETAIL_CASHRCP);
            var presenter = new PYP015presenter(this);

            // 0: 자진발급, 1: 개인소득공제, 2: 사업자(지출증빙)
            int cashType = 0;
            if (!"1".Equals(this.BasketCashReceipt.FgSelf))
            {
                // TrxnCode = cashType == 2 ? "01" : "00",
                if ("1".Equals(this.BasketCashReceipt.FgTrxnType))
                {
                    cashType = 1;
                }
                else
                {
                    cashType = 2;
                }
            }

            bool isSwipe = "A".Equals(this.BasketCashReceipt.InputWcc);

            // 전문에 추가정보
            // 점포, 거래, 포스, 첫상품
            presenter.MakeCashRecptRequest(cashType, isSwipe,
                this.BasketCashReceipt.DdAppr,
                this.BasketCashReceipt.NoAppr,
                "@".Equals(this.BasketCashReceipt.InputWcc) ? this.BasketCashReceipt.NoPersonal : this.BasketCashReceipt.NoTrack,
                "1",
                TypeHelper.ToInt32(this.BasketCashReceipt.AmAppr),
                TypeHelper.ToInt32(this.BasketCashReceipt.AmTax),
                GetPV11ReqDataAdd());
        }


        void AutoRtnProcessCashRcpA()
        {
            m_saleView.AutoRtnUpdateStatusMsg(NetCommConstants.PAYMENT_DETAIL_CASHRCP, NetCommConstants.PAYMENT_DETAIL_CASHRCP);
            var presenter = new PYP015presenter(this);

            // 0: 자진발급, 1: 개인소득공제, 2: 사업자(지출증빙)
            int cashType = 0;
            if (!"1".Equals(this.BasketCashReceipt.FgSelf))
            {
                // TrxnCode = cashType == 2 ? "01" : "00",
                if ("1".Equals(this.BasketCashReceipt.FgTrxnType))
                {
                    cashType = 1;
                }
                else
                {
                    cashType = 2;
                }
            }

            bool isSwipe = "A".Equals(this.BasketCashReceipt.InputWcc);

            // 전문에 추가정보
            // 점포, 거래, 포스, 첫상품
            presenter.MakeCashRecptRequest(cashType, isSwipe,
                this.BasketCashReceipt.DdAppr,
                this.BasketCashReceipt.NoAppr,
                "@".Equals(this.BasketCashReceipt.InputWcc) ? this.BasketCashReceipt.NoPersonal : this.BasketCashReceipt.NoTrack,
                "1",
                TypeHelper.ToInt32(this.BasketCashReceipt.AmAppr),
                TypeHelper.ToInt32(this.BasketCashReceipt.AmTax),
                GetPV11ReqDataAdd());
        }

        /// <summary>
        /// 현금영수증취소 
        /// 결과받기
        /// </summary>
        /// <param name="respData"></param>
        public void OnReturnSuccess(PV02RespData respData)
        {
            AutoRtnProcessCashReceiptMakeBasket(respData, null, null);
        }

        /// <summary>
        /// 현금영수증 Basket 생성
        /// </summary>
        /// <param name="respData">승인실패인경우 =null</param>
        /// <param name="errorCode">승인실패인경우</param>
        /// <param name="errorMessage">승인실패인경우</param>
        void AutoRtnProcessCashReceiptMakeBasket(PV02RespData respData, string errorCode, string errorMessage)
        {
            // 1st Payment 아닌거로 설정
            m_autoRtnFirstVANPay = false;

            var cashBasket = new BasketCashRecpt()
            {
                AmAppr = this.BasketCashReceipt.AmAppr,
                AmTax = this.BasketCashReceipt.AmTax,
                CdCancRsn = "1",
                CdVan = respData == null ? BasketCashReceipt.CdVan : respData.ApprVanCode,

                NoAppr = respData == null ? string.Empty : respData.ApprNo,
                DdAppr = respData == null ? DateTime.Today.ToString("yyyyMMdd") : respData.ApprDate,
                TmAppr = respData == null ? DateTime.Now.ToString("HHmmss") : respData.ApprTime,

                FgAppr = respData == null ? "0" : "1",
                FgIDCheck = this.BasketCashReceipt.FgIDCheck,
                FgSelf = this.BasketCashReceipt.FgSelf,
                FgTrxnType = this.BasketCashReceipt.FgTrxnType,
                InputWcc = this.BasketCashReceipt.InputWcc,
                NoPersonal = this.BasketCashReceipt.NoPersonal,
                NoTrack = this.BasketCashReceipt.NoTrack,

                DdApprOrg = this.BasketCashReceipt.DdAppr,
                NoApprOrg = this.BasketCashReceipt.NoAppr,

                ForceCancFg = respData == null ? "1" : string.Empty,
                CancRcvCode = respData == null ? errorCode : string.Empty,
                CancRcvMsg = respData == null ? errorMessage : string.Empty
            };

            if (!string.IsNullOrEmpty(errorCode) || !string.IsNullOrEmpty(errorMessage))
            {
                ForcePays.Add(cashBasket);
            }

            // overwrite
            this.BasketCashReceipt = cashBasket;

            // next
            AutoRtnProcessNextBasketPay(false);
        }


        /// <summary>
        /// 여전법 추가 0630
        /// 현금영수증 취소
        /// 자동반품시 사용
        /// </summary>
        void AutoRtnProcessCashReceiptPopup()
        {
            object retData = null;
            int cashAmt = Convert.ToInt32(this.BasketCashReceipt.AmAppr);
            int taxAmt = Convert.ToInt32(this.BasketCashReceipt.AmTax);
            var reqData = GetPV11ReqDataAdd();
            reqData.DdApprOrg = this.BasketCashReceipt.DdAppr;
            reqData.NoApprOrg = this.BasketCashReceipt.NoAppr;
            reqData.IsAutoReturn = true;
            var res = m_saleView.ShowCashReceiptPopup(cashAmt, taxAmt,
                reqData,
                out retData);

            // make cashReceiptBasket
            if (retData != null)
            {
                if (retData.GetType() == typeof(string[]))
                {
                    string errorCode = ((string[])retData)[0];
                    string errorMsg = ((string[])retData)[1];
                    AutoRtnProcessCashReceiptMakeBasket(null, errorCode, errorMsg);
                }
                else
                {
                    this.BasketCashReceipt = (BasketCashRecpt)retData;

                    // next
                    AutoRtnProcessNextBasketPay(false);
                }
            }
            else
            {
                AutoRtnProcessCashReceiptMakeBasket(null, string.Empty, string.Empty);
            }
        }

        #endregion

        #region 자동반품 - 포인트적립취소

        /// <summary>
        /// 포인트적립취소
        /// </summary>
        void AutoRtnProcessPointSave()
        {
            if (string.IsNullOrEmpty(this.BasketPointSave.NoAppr))
            {
                AutoRtnCompleteStep();
                return;
            }

            m_saleView.AutoRtnUpdateStatusMsg(NetCommConstants.PAYMENT_DETAIL_POINTSAVE, NetCommConstants.PAYMENT_DETAIL_POINTSAVE);

            // 원거래정보 - 반품시 적용, 매출일(8)+점(4)+포스(4)+거래번호(6)
            string otTrxnInfo = this.BasketHeader.SaleDate +
                this.BasketHeader.StoreNo.PadRight(4, ' ') +
                this.BasketHeader.PosNo + this.BasketHeader.TrxnNo;

            string payCashAmt = string.Empty;
            string payCardAmt = string.Empty;
            string payEtcAmt = string.Empty;
            CalcPointSaveCancelAmount(out payCashAmt, out payCardAmt, out payEtcAmt);

            var pp03 = new PP03DataTask("2",
                otTrxnInfo, this.BasketPointSave.NoAppr,
                this.BasketPointSave.NoPointMember,
                this.BasketPointSave.NoCard,
                payCashAmt, payCardAmt, payEtcAmt,
                string.Empty, string.Empty, string.Empty, string.Empty);

            pp03.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(AutoRtnPointPP03_TaskCompleted);
            pp03.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(AutoRtnPointPP03_Errored);
            pp03.ExecuteTask();
        }

        /// <summary>
        /// 포인트적립취소 오류
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="lastException"></param>
        void AutoRtnPointPP03_Errored(string errorMessage, Exception lastException)
        {
            if (m_saleView.InvokeRequired)
            {
                m_saleView.BeginInvoke((MethodInvoker)delegate()
                {
                    var res = m_saleView.AutoRtnShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.SomeError, errorMessage,
                        m_autoRtnFirstVANPay);
                    if (res == System.Windows.Forms.DialogResult.Yes)
                    {
                        AutoRtnProcessPointSave();
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        AutoRtnConfirmStop();
                    }
                    else
                    {
                        AutoRtnProcessPointSaveComplete(null, string.Empty, errorMessage);
                    }
                });
            }
            else
            {
                var res = m_saleView.AutoRtnShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.SomeError, errorMessage,
                    m_autoRtnFirstVANPay);
                if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    AutoRtnProcessPointSave();
                }
                else if (res == DialogResult.Cancel)
                {
                    AutoRtnConfirmStop();
                }
                else
                {
                    AutoRtnProcessPointSaveComplete(null, string.Empty, errorMessage);
                }
            }
        }

        /// <summary>
        /// 포인트적립 취소
        /// </summary>
        /// <param name="responseData"></param>
        void AutoRtnPointPP03_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                var data = responseData.DataRecords.ToDataRecords<PP03RespData>()[0];
                AutoRtnProcessPointSaveComplete(data, string.Empty, string.Empty);
            }
            else
            {
                if (m_saleView.InvokeRequired)
                {
                    m_saleView.BeginInvoke((MethodInvoker)delegate()
                    {
                        var res = m_saleView.AutoRtnShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.NoInfoFound,
                            MSG_AUTORTN_POINT_SAVE_CANCEL_NO_INFO, m_autoRtnFirstVANPay);

                        if (res == System.Windows.Forms.DialogResult.Yes)
                        {
                            AutoRtnProcessPointSave();
                        }
                        else if (res == DialogResult.Cancel)
                        {
                            AutoRtnConfirmStop();
                        }
                        else
                        {
                            AutoRtnProcessPointSaveComplete(null, string.Empty, responseData.Response.ErrorMessage);
                        }
                    });
                }
                else
                {
                    var res = m_saleView.AutoRtnShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.NoInfoFound,
                        MSG_AUTORTN_POINT_SAVE_CANCEL_NO_INFO, m_autoRtnFirstVANPay);

                    if (res == System.Windows.Forms.DialogResult.Yes)
                    {
                        AutoRtnProcessPointSave();
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        AutoRtnConfirmStop();
                    }
                    else
                    {
                        AutoRtnProcessPointSaveComplete(null, string.Empty, responseData.Response.ErrorMessage);
                    }
                }
            }
        }

        void CalcPointSaveCancelAmount(out string payCashAmt, out string payCardAmt, out string payEtcAmt)
        {
            Int64 iPayCashAmt = 0;
            Int64 iPayCardAmt = 0;
            Int64 iSumPayAmt = 0;
            Int64 iSumBalAmt = 0;

            foreach (var item in this.BasketPays)
            {
                var iPayAmt = TypeHelper.ToInt64(item.PayAmt);
                var iBalAmt = TypeHelper.ToInt64(item.BalAmt);
                iSumPayAmt += iPayAmt;
                iSumBalAmt += iBalAmt;

                if (iPayAmt > 0)
                {
                    if ((item.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CASH &&
                                item.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CASH) || //현금
                                (item.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CASH &&
                                item.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CASH) || //수표
                                (item.PayGrpCd == NetCommConstants.PAYMENT_GROUP_TKCKET &&
                                item.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_TICKET_OTHER)) // 타사상품권
                    {
                        iPayCashAmt += (iPayAmt - iBalAmt);
                    }
                    else if ((item.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD &&
                                    item.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CARD) || //신용카드
                                    (item.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD &&
                                    item.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CARD_OTHER) || //타건카드
                                    (item.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD &&
                                    item.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CARD_WELFARE) || //타건복지
                                    (item.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD &&
                                    item.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CASH_IC)) //현금IC
                    {
                        iPayCardAmt += (iPayAmt - iBalAmt);
                    }
                }
            }

            payCashAmt = iPayCashAmt.ToString();
            payCardAmt = iPayCardAmt.ToString();
            payEtcAmt = ((iSumPayAmt - iSumBalAmt) - (iPayCashAmt + iPayCardAmt)).ToString();   //기타금액 = 전체금액 - (현금금액 + 카드금액)
        }

        /// <summary>
        /// 포인트적립 BASKET생성
        /// </summary>
        /// <param name="respData"></param>
        void AutoRtnProcessPointSaveComplete(PP03RespData respData, string errorCode, string errorMessage)
        {
            // 1st Payment 아닌거로 설정
            m_autoRtnFirstVANPay = false;

            BasketPointSave bp = (BasketPointSave)(BasketPointSave.Parse(typeof(BasketPointSave), this.BasketPointSave.ToString()));
            bp.FgProgRes = respData != null ? "1" : "0";

            if (respData != null)
            {
                bp.NoCard = respData.CardNo;
                bp.PointNmMember = respData.CustName;
                bp.AmPoint = respData.IssuePoint;
                bp.AmMarkNotDay = respData.AnniversaryPoint;
                bp.AmMarkEvt = respData.EventPoint;
                bp.AmPointUsable = respData.AbtyPoint;
                bp.AmPointAccu = respData.CltePoint;
                bp.AmPointDelay = respData.DelayPoint;
                bp.CustGrade = respData.GradeCode;
                bp.CustGradeNm = respData.GradeName;
                bp.Remark = respData.Remark;
                bp.NoAppr = respData.ApprNo;
                bp.NoPointMember = respData.CustNo;
                bp.PointEvtCode = respData.PointEventCode;
                bp.PointEvtName = respData.PointEventName;
            }
            else
            {
                bp.NoAppr = string.Empty;
                bp.ForceCancFg = "1";
                bp.CancRcvCode = errorCode;
                bp.CancRcvMsg = errorMessage;

                // 강제진행항목에 추가
                ForcePays.Add(bp);
            }

            // overwrite old trans data
            this.BasketPointSave = bp;

            // next is complete
            AutoRtnCompleteStep();
        }

        #endregion

        #endregion

        #region 사은품회수 BASKET 생성

        /// <summary>
        /// 사은품회수 리스트
        /// </summary>
        /// <returns></returns>
        List<BasketTksPresentRtn> AutoRtnMakeTksPresentBasketList()
        {
            List<BasketTksPresentRtn> list = new List<BasketTksPresentRtn>();
            foreach (var item in TksPresentList)
            {
                var bk = new BasketTksPresentRtn()
                {
                    PresentDate = item.PresentDate,
                    PresentNo = item.PresentNo,
                    PresentSeq = item.PresentSeq,
                    PresentNm = item.TksGiftName,
                    PresentAmt = item.PresentAmt,
                    RtnPrsTotalAmt = item.ReturnAmt,
                    OTSaleDate = this.BasketHeader.SaleDate,
                    OTStoreNo = this.BasketHeader.StoreNo,
                    OTPosNo = this.BasketHeader.PosNo,
                    OTTrxnNo = this.BasketHeader.TrxnNo,
                    RtnCashAmt = item.RtnCashAmt.ToString(),
                    RtnGiftAmt = item.RtnGiftAmt.ToString(),
                    RtnPrsAmt = item.RtnPresentAmt.ToString(),
                    RtnGiftCashAmt = item.RtnGiftCashAmt.ToString(),
                    RtnCantRsn = item.RtnCantRsn
                };

                if (item.RtnPrsnList != null)
                {
                    int nextNo = 1;
                    //// update 반납교환권
                    for (int i = 0; i < item.RtnPrsnList.Count; i++)
                    {
                        if (item.RtnPrsnList[i].GiftAmt <= 0 ||
                            item.RtnPrsnList[i].GiftCount <= 0)
                        {
                            continue;
                        }

                        // 교환권번호
                        string fieldName = string.Format("GiftNo{0}", nextNo);
                        UpdateFieldValue(bk, fieldName, item.RtnPrsnList[i].GiftNo);

                        // 권종금액
                        fieldName = string.Format("GiftAmt{0}", nextNo);
                        UpdateFieldValue(bk, fieldName, Convert.ToString(item.RtnPrsnList[i].GiftAmt));

                        // 매수
                        fieldName = string.Format("GiftCount{0}", nextNo);
                        UpdateFieldValue(bk, fieldName, Convert.ToString(item.RtnPrsnList[i].GiftCount));

                        nextNo++;
                    }
                }

                list.Add(bk);
            }

            return list;
        }

        void UpdateFieldValue(object data, string fieldName, object value)
        {
            var fi = data.GetType().GetField(fieldName);
            if (fi != null)
            {
                fi.SetValue(data, value);
            }
        }

        #endregion

        #region 반품 Header 생성 & 반품완료

        /// <summary>
        /// 결제수단 모두 취소 완료시
        /// 이 함수를 호출한다
        /// </summary>
        void AutoRtnCompleteStep()
        {
            m_saleView.AutoRtnUpdateStatusMsg(SLExtensions.PAYMENT_DETAIL_AUTORTN_END,
                SLExtensions.PAYMENT_DETAIL_AUTORTN_END);

            if (m_saleView.InvokeRequired)
            {
                m_saleView.BeginInvoke((MethodInvoker)delegate()
                {
                    AutoRtnSaveTRComplete();
                });
            }
            else
            {
                AutoRtnSaveTRComplete();
            }
        }

        /// <summary>
        /// TR저장 및 강제취소건표시
        /// </summary>
        void AutoRtnSaveTRComplete()
        {
            #region Make BasketHeader - 생성

            this.BasketHeader.CancType = NetCommConstants.CANCEL_TYPE_RETURN;

            /*
             * 
             *  원거래등록 매출일자
                원거래등록 점포코드
                원거래등록 포스번호
                원거래등록 거래번호
                원거래등록 계산원 코드
             * */
            this.BasketHeader.OTCasNo = this.BasketHeader.CasNo;
            this.BasketHeader.OTPosNo = this.BasketHeader.PosNo;
            this.BasketHeader.OTSaleDate = this.BasketHeader.SaleDate;
            this.BasketHeader.OTStoreNo = this.BasketHeader.StoreNo;
            this.BasketHeader.OTTrxnNo = this.BasketHeader.TrxnNo;

            /*
             * 
             *  점포코드
                포스코드
                매출일자
                거래번호
                거래구분
                취소구분
                발생 일자
                발생 시간
                TRAN 버퍼 버전
                정산차수
                계산원 번호
                계산원 이름

             * 
             * */
            this.BasketHeader.StoreNo = ConfigData.Current.AppConfig.PosInfo.StoreNo;
            this.BasketHeader.PosNo = ConfigData.Current.AppConfig.PosInfo.PosNo;
            this.BasketHeader.SaleDate = ConfigData.Current.AppConfig.PosInfo.SaleDate;
            this.BasketHeader.TrxnNo = ConfigData.Current.AppConfig.PosInfo.TrxnNo;
            this.BasketHeader.TrxnType = NetCommConstants.TRXN_TYPE_SALE;
            this.BasketHeader.CancType = NetCommConstants.CANCEL_TYPE_RETURN;
            this.BasketHeader.OccrDate = DateTime.Today.ToString("yyyyMMdd");
            this.BasketHeader.OccrTime = DateTime.Now.ToString("HHmmss");
            this.BasketHeader.TxBufVer = BasketHeader.TX_BUF_VER;
            this.BasketHeader.ShiftNo = ConfigData.Current.AppConfig.PosInfo.ShiftCount;
            this.BasketHeader.CasNo = ConfigData.Current.AppConfig.PosInfo.CasNo;
            this.BasketHeader.CasName = ConfigData.Current.AppConfig.PosInfo.CasName;
            this.BasketHeader.RfProcFg = "0";

            /*
             * 
             * 
             *  반품사유코드
                할인사유코드
             * ==> 사용안함
             * */

            /*
             * 
             * 현금영수증 승인 처리 여부
             * 포인트 적립 처리 여부
             * */
            // 현금영수증여부
            this.BasketHeader.CRProcFg = this.BasketCashReceipt != null &&
                !string.IsNullOrEmpty(this.BasketCashReceipt.NoPersonal) ? "1" : "0";

            // 포인트적립여부
            this.BasketHeader.PntSaveProcFg = this.BasketPointSave != null &&
                !string.IsNullOrEmpty(this.BasketPointSave.NoPointMember) ? "1" : "0";

            // details
            this.BasketPays.Clear();
            this.BasketPays.AddRange(this.BasketPaysAutoRtn);
            this.BasketPaysAutoRtn.Clear();

            #endregion

            #region TR생성한다

            // 사은품회수
            List<BasketTksPresentRtn> pbList = null;
            if (this.TksPresentList.Count > 0)
            {
                pbList = AutoRtnMakeTksPresentBasketList();
            }

            if (ProcessTRGenerate(true, pbList))
            {
                // 강제진행, 강제취소 있을때 내역표시한다
                if (this.ForcePays != null && this.ForcePays.Count > 0)
                {
                    m_saleView.AutoRtnShowForceCancelBaskets(this.BasketHeader, this.ForcePays);
                    this.ForcePays.Clear();
                    this.ForcePays = null;
                }
            }

            #endregion

            // processing, cancel all keys
            m_keyEventProcessing = false;
        }

        #endregion

        #endregion

        #endregion

    }
}