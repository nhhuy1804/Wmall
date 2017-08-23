//-----------------------------------------------------------------
/*
 * 화면명   : IOM002presenter.cs
 * 화면설명 : 중간입금 설정
 * 개발자   : 정광호
 * 개발일자 : 2015.03.24
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using WSWD.WmallPos.POS.IO.PI;
using WSWD.WmallPos.POS.IO.VI;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using WSWD.WmallPos.FX.BO.Trans;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.POS.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.NetComm;

namespace WSWD.WmallPos.POS.IO.PT
{
    public class IOM002presenter : IIOM002presenter
    {
        private IIOM002View m_view;
        public IOM002presenter(IIOM002View view)
        {
            m_view = view;
        }

        #region IPresenter Members

        /// <summary>
        /// 타사 상품권명 조회
        /// </summary>
        public void GetTicketTitle()
        {
            var masterdb = MasterDbHelper.InitInstance();
            DataSet ds = null;
            try
            {
                ds = masterdb.ExecuteQuery(Extensions.LoadSqlCommand("POS_IO", "SelectGFM050T"),
                    new string[] { "@DD_SALE" },
                    new object[] {
                        ConfigData.Current.AppConfig.PosInfo.SaleDate
                    });
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                masterdb.EndInstance();
            }

            if (m_view != null)
            {
                //타사 상품권명 셋팅
                m_view.SetTicketTitle(ds);
            }
        }

        /// <summary>
        /// 중간입금 금액 조회
        /// </summary>
        public void GetMiddleDeposit()
        {
            var transdb = TranDbHelper.InitInstance();
            DataSet ds = null;
            try
            {
                ds = transdb.ExecuteQuery(Extensions.LoadSqlCommand("POS_IO", "M002SelectTotalAmtSAT300T"),
                    new string[] { "@DD_SALE", "@CD_STORE", "@NO_POS" },
                    new object[] {
                        ConfigData.Current.AppConfig.PosInfo.SaleDate,
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo
                    });
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                transdb.EndInstance();
            }

            if (m_view != null)
            {
                //중간입금 차수 및 각각의 차수 금액 셋팅
                m_view.SetMiddleDeposit(ds);
            }
        }

        /// <summary>
        /// 중간입금 차수 및 차수 합계 금액 DB저장
        /// </summary>
        /// <param name="dr">중간입금 Datarow</param>
        /// <param name="dMiddleTotalAmt">중간입금 해당회차 총금액</param>
        public void SetMiddelAmt(string strInputGubun,  string strMiddleNo, DataSet ds)
        {
            DataRow drC = ds.Tables["cash"].Rows[0];        //현금
            DataRow drT = ds.Tables["ticket"].Rows[0];      //상품권
            DataRow drTNm = ds.Tables["ticket"].Rows[3];    //상품권 명

            int iTempCnt = 0;   //총건수
            Int64 iTempAmt = 0;   //총금액

            #region Basket header

            BasketHeader header = new BasketHeader();
            header.TrxnType = NetCommConstants.TRXN_TYPE_MID_IO;
            header.CancType = NetCommConstants.CANCEL_TYPE_NORMAL;

            #endregion

            #region Basket details

            BasketMiddleDeposit middleDeposit = new BasketMiddleDeposit();
            middleDeposit.BasketType = BasketTypes.BasketMiddleDeposit;

            middleDeposit.InputGubun = strInputGubun.ToString();
            middleDeposit.MiddleDepositCnt = strMiddleNo;

            iTempCnt = 0;
            iTempCnt += GetData(drC["txtCashAmt01"]) > 0 ? GetData(drC["txtCashCnt01"]) : 0;
            iTempCnt += GetData(drC["txtCashAmt02"]) > 0 ? GetData(drC["txtCashCnt02"]) : 0;
            iTempCnt += GetData(drC["txtCashAmt03"]) > 0 ? GetData(drC["txtCashCnt03"]) : 0;
            iTempCnt += GetData(drC["txtCashAmt04"]) > 0 ? GetData(drC["txtCashCnt04"]) : 0;
            iTempCnt += GetData(drC["txtCashAmt05"]) > 0 ? GetData(drC["txtCashCnt05"]) : 0;
            iTempCnt += GetData(drC["txtCashAmt06"]) > 0 ? GetData(drC["txtCashCnt06"]) : 0;
            iTempCnt += GetData(drC["txtCashAmt07"]) > 0 ? GetData(drC["txtCashCnt07"]) : 0;
            iTempCnt += GetData(drC["txtCashAmt08"]) > 0 ? GetData(drC["txtCashCnt08"]) : 0;
            iTempCnt += GetData(drC["txtCashAmt09"]) > 0 ? GetData(drC["txtCashCnt09"]) : 0;
            iTempAmt = 0;
            iTempAmt += GetData(drC["txtCashCnt01"]) > 0 ? GetData(drC["txtCashAmt01"]) : 0;
            iTempAmt += GetData(drC["txtCashCnt02"]) > 0 ? GetData(drC["txtCashAmt02"]) : 0;
            iTempAmt += GetData(drC["txtCashCnt03"]) > 0 ? GetData(drC["txtCashAmt03"]) : 0;
            iTempAmt += GetData(drC["txtCashCnt04"]) > 0 ? GetData(drC["txtCashAmt04"]) : 0;
            iTempAmt += GetData(drC["txtCashCnt05"]) > 0 ? GetData(drC["txtCashAmt05"]) : 0;
            iTempAmt += GetData(drC["txtCashCnt06"]) > 0 ? GetData(drC["txtCashAmt06"]) : 0;
            iTempAmt += GetData(drC["txtCashCnt07"]) > 0 ? GetData(drC["txtCashAmt07"]) : 0;
            iTempAmt += GetData(drC["txtCashCnt08"]) > 0 ? GetData(drC["txtCashAmt08"]) : 0;
            iTempAmt += GetData(drC["txtCashCnt09"]) > 0 ? GetData(drC["txtCashAmt09"]) : 0;

            middleDeposit.CashTotalCnt = iTempCnt > 0 ? iTempCnt.ToString() : "";
            middleDeposit.CashTotalAmt = iTempAmt > 0 ? iTempAmt.ToString() : "";

            int ticketCnt = GetData(drT["txtTicketCnt01"]);
            int ticketAmt = GetData(drT["txtTicketAmt01"]);

            middleDeposit.TicketCnt = GetData(drT["txtTicketAmt01"]) > 0 ? GetDataString(drT["txtTicketCnt01"]) : "";
            middleDeposit.TicketAmt = GetData(drT["txtTicketCnt01"]) > 0 ? GetDataString(drT["txtTicketAmt01"]) : "";

            iTempCnt = 0;
            //iTempCnt += GetData(drT["txtTicketAmt02"]) > 0 ? GetData(drT["txtTicketCnt02"]) : 0;
            iTempCnt += GetData(drT["txtTicketAmt03"]) > 0 ? GetData(drT["txtTicketCnt03"]) : 0;
            /* Loc changed 12.08
            iTempCnt += GetData(drT["txtTicketAmt04"]) > 0 ? GetData(drT["txtTicketCnt04"]) : 0;
            iTempCnt += GetData(drT["txtTicketAmt05"]) > 0 ? GetData(drT["txtTicketCnt05"]) : 0;
            iTempCnt += GetData(drT["txtTicketAmt06"]) > 0 ? GetData(drT["txtTicketCnt06"]) : 0;
            iTempCnt += GetData(drT["txtTicketAmt07"]) > 0 ? GetData(drT["txtTicketCnt07"]) : 0;
            iTempCnt += GetData(drT["txtTicketAmt08"]) > 0 ? GetData(drT["txtTicketCnt08"]) : 0;
            iTempCnt += GetData(drT["txtTicketAmt09"]) > 0 ? GetData(drT["txtTicketCnt09"]) : 0;*/

            iTempAmt = 0;
            //iTempAmt += GetData(drT["txtTicketCnt02"]) > 0 ? GetData(drT["txtTicketAmt02"]) : 0;
            iTempAmt += GetData(drT["txtTicketCnt03"]) > 0 ? GetData(drT["txtTicketAmt03"]) : 0;
            /* Loc changed 12.08
            iTempAmt += GetData(drT["txtTicketCnt04"]) > 0 ? GetData(drT["txtTicketAmt04"]) : 0;
            iTempAmt += GetData(drT["txtTicketCnt05"]) > 0 ? GetData(drT["txtTicketAmt05"]) : 0;
            iTempAmt += GetData(drT["txtTicketCnt06"]) > 0 ? GetData(drT["txtTicketAmt06"]) : 0;
            iTempAmt += GetData(drT["txtTicketCnt07"]) > 0 ? GetData(drT["txtTicketAmt07"]) : 0;
            iTempAmt += GetData(drT["txtTicketCnt08"]) > 0 ? GetData(drT["txtTicketAmt08"]) : 0;
            iTempAmt += GetData(drT["txtTicketCnt09"]) > 0 ? GetData(drT["txtTicketAmt09"]) : 0;*/

            middleDeposit.OtherCompanyTicketTotalCnt = iTempCnt > 0 ? iTempCnt.ToString() : "";
            middleDeposit.OtherCompanyTicketTotalAmt = iTempAmt > 0 ? iTempAmt.ToString() : "";

            middleDeposit.TicketTotalCnt = ticketCnt.ToString();
            middleDeposit.TicketTotalAmt = ticketAmt.ToString();

            middleDeposit.WonCnt_1000000 = GetData(drC["txtCashAmt01"]) > 0 ? GetDataString(drC["txtCashCnt01"]) : "";
            middleDeposit.WonAmt_1000000 = GetData(drC["txtCashCnt01"]) > 0 ? GetDataString(drC["txtCashAmt01"]) : "";
            middleDeposit.WonCnt_50000 = GetData(drC["txtCashAmt02"]) > 0 ? GetDataString(drC["txtCashCnt02"]) : "";
            middleDeposit.WonAmt_50000 = GetData(drC["txtCashCnt02"]) > 0 ? GetDataString(drC["txtCashAmt02"]) : "";
            middleDeposit.WonCnt_10000 = GetData(drC["txtCashAmt03"]) > 0 ? GetDataString(drC["txtCashCnt03"]) : "";
            middleDeposit.WonAmt_10000 = GetData(drC["txtCashCnt03"]) > 0 ? GetDataString(drC["txtCashAmt03"]) : "";
            middleDeposit.WonCnt_5000 = GetData(drC["txtCashAmt04"]) > 0 ? GetDataString(drC["txtCashCnt04"]) : "";
            middleDeposit.WonAmt_5000 = GetData(drC["txtCashCnt04"]) > 0 ? GetDataString(drC["txtCashAmt04"]) : "";
            middleDeposit.WonCnt_1000 = GetData(drC["txtCashAmt05"]) > 0 ? GetDataString(drC["txtCashCnt05"]) : "";
            middleDeposit.WonAmt_1000 = GetData(drC["txtCashCnt05"]) > 0 ? GetDataString(drC["txtCashAmt05"]) : "";
            middleDeposit.WonCnt_500 = GetData(drC["txtCashAmt06"]) > 0 ? GetDataString(drC["txtCashCnt06"]) : "";
            middleDeposit.WonAmt_500 = GetData(drC["txtCashCnt06"]) > 0 ? GetDataString(drC["txtCashAmt06"]) : "";
            middleDeposit.WonCnt_100 = GetData(drC["txtCashAmt07"]) > 0 ? GetDataString(drC["txtCashCnt07"]) : "";
            middleDeposit.WonAmt_100 = GetData(drC["txtCashCnt07"]) > 0 ? GetDataString(drC["txtCashAmt07"]) : "";
            middleDeposit.WonCnt_50 = GetData(drC["txtCashAmt08"]) > 0 ? GetDataString(drC["txtCashCnt08"]) : "";
            middleDeposit.WonAmt_50 = GetData(drC["txtCashCnt08"]) > 0 ? GetDataString(drC["txtCashAmt08"]) : "";
            middleDeposit.WonCnt_10 = GetData(drC["txtCashAmt09"]) > 0 ? GetDataString(drC["txtCashCnt09"]) : "";
            middleDeposit.WonAmt_10 = GetData(drC["txtCashCnt09"]) > 0 ? GetDataString(drC["txtCashAmt09"]) : "";

            // 할인쿠폰
            //if (GetDataString(drTNm["txtTicketAmt02"]) != "")
            //{
                //middleDeposit.OtherCompanyTicketNm_01 = GetDataString(drTNm["txtTicketAmt02"]);
            middleDeposit.DiscCouponAmt = GetData(drT["txtTicketAmt02"]) > 0 ? GetDataString(drT["txtTicketAmt02"]) : "";
            middleDeposit.DiscCouponCnt = GetData(drT["txtTicketCnt02"]) > 0 ? GetDataString(drT["txtTicketCnt02"]) : "";
            //}

            // 타사상품권
            //if (GetDataString(drTNm["txtTicketAmt03"]) != "")
            //{
                middleDeposit.OtherCompanyTicketNm_01 = "타사상품권";// GetDataString(drTNm["txtTicketAmt03"]);
                middleDeposit.OtherCompanyTicketCnt_01 = GetData(drT["txtTicketAmt03"]) > 0 ? GetDataString(drT["txtTicketCnt03"]) : "";
                middleDeposit.OtherCompanyTicketAmt_01 = GetData(drT["txtTicketCnt03"]) > 0 ? GetDataString(drT["txtTicketAmt03"]) : "";
            //}

            /* LOc changed 12.08
            if (GetDataString(drTNm["txtTicketAmt04"]) != "")
            {
                middleDeposit.OtherCompanyTicketNm_03 = GetDataString(drTNm["txtTicketAmt04"]);
                middleDeposit.OtherCompanyTicketCnt_03 = GetData(drT["txtTicketAmt04"]) > 0 ? GetDataString(drT["txtTicketCnt04"]) : "";
                middleDeposit.OtherCompanyTicketAmt_03 = GetData(drT["txtTicketCnt04"]) > 0 ? GetDataString(drT["txtTicketAmt04"]) : "";
            }

            if (GetDataString(drTNm["txtTicketAmt05"]) != "")
            {
                middleDeposit.OtherCompanyTicketNm_04 = GetDataString(drTNm["txtTicketAmt05"]);
                middleDeposit.OtherCompanyTicketCnt_04 = GetData(drT["txtTicketAmt05"]) > 0 ? GetDataString(drT["txtTicketCnt05"]) : "";
                middleDeposit.OtherCompanyTicketAmt_04 = GetData(drT["txtTicketCnt05"]) > 0 ? GetDataString(drT["txtTicketAmt05"]) : "";
            }

            if (GetDataString(drTNm["txtTicketAmt06"]) != "")
            {
                middleDeposit.OtherCompanyTicketNm_05 = GetDataString(drTNm["txtTicketAmt06"]);
                middleDeposit.OtherCompanyTicketCnt_05 = GetData(drT["txtTicketAmt06"]) > 0 ? GetDataString(drT["txtTicketCnt06"]) : "";
                middleDeposit.OtherCompanyTicketAmt_05 = GetData(drT["txtTicketCnt06"]) > 0 ? GetDataString(drT["txtTicketAmt06"]) : "";
            }

            if (GetDataString(drTNm["txtTicketAmt07"]) != "")
            {
                middleDeposit.OtherCompanyTicketNm_06 = GetDataString(drTNm["txtTicketAmt07"]);
                middleDeposit.OtherCompanyTicketCnt_06 = GetData(drT["txtTicketAmt07"]) > 0 ? GetDataString(drT["txtTicketCnt07"]) : "";
                middleDeposit.OtherCompanyTicketAmt_06 = GetData(drT["txtTicketCnt07"]) > 0 ? GetDataString(drT["txtTicketAmt07"]) : "";
            }

            if (GetDataString(drTNm["txtTicketAmt08"]) != "")
            {
                middleDeposit.OtherCompanyTicketNm_07 = GetDataString(drTNm["txtTicketAmt08"]);
                middleDeposit.OtherCompanyTicketCnt_07 = GetData(drT["txtTicketAmt08"]) > 0 ? GetDataString(drT["txtTicketCnt08"]) : "";
                middleDeposit.OtherCompanyTicketAmt_07 = GetData(drT["txtTicketCnt08"]) > 0 ? GetDataString(drT["txtTicketAmt08"]) : "";
            }

            if (GetDataString(drTNm["txtTicketAmt09"]) != "")
            {
                middleDeposit.OtherCompanyTicketNm_08 = GetDataString(drTNm["txtTicketAmt09"]);
                middleDeposit.OtherCompanyTicketCnt_08 = GetData(drT["txtTicketAmt09"]) > 0 ? GetDataString(drT["txtTicketCnt09"]) : "";
                middleDeposit.OtherCompanyTicketAmt_08 = GetData(drT["txtTicketCnt09"]) > 0 ? GetDataString(drT["txtTicketAmt09"]) : "";
            }*/

            #endregion

            var transdb = TranDbHelper.InitInstance();
            var trans = transdb.BeginTransaction();

            try
            {
                TransManager.SaveTrans(header, new BasketBase[] { middleDeposit }, transdb, trans);

                trans.Commit();

                // TR 완료시
                TransManager.OnTransComplete();
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }

                LogUtils.Instance.LogException(ex);
                header = null;
            }
            finally
            {
                transdb.EndInstance();
                trans.Dispose();
            }

            if (m_view != null)
            {
                m_view.SetTran(header, middleDeposit);
            }
        }

        private string GetDataString(object data)
        {
            string sReturn = string.Empty;

            if (data == null)
            {
                sReturn = "";
            }
            else
            {
                sReturn = data.ToString();
            }

            return sReturn;
        }

        private int GetData(object data)
        {
            int dReturn = 0;


            if (data != null && data.ToString() != "")
            {
                dReturn = Convert.ToInt32(data);
            }

            return dReturn;
        }

        #endregion
    }
}
