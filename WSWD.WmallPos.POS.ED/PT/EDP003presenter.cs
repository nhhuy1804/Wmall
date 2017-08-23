//-----------------------------------------------------------------
/*
 * 화면명   : EDP003presenter.cs
 * 화면설명 : POS 정산
 * 개발자   : 정광호
 * 개발일자 : 2015.04.10
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using WSWD.WmallPos.POS.ED.PI;
using WSWD.WmallPos.POS.ED.VI;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using WSWD.WmallPos.FX.BO.Trans;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.POS.FX.Shared.Utils;

using System.Windows.Forms;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.ED.PT
{
    public class EDP003presenter : IEDP003presenter
    {
        private IEDP003View m_view;
        public EDP003presenter(IEDP003View view)
        {
            m_view = view;
        }

        #region IPresenter Members

        /// <summary>
        /// SAT900 보류확인
        /// </summary>
        public void GetWait()
        {
            var db = TranDbHelper.InitInstance();
            string strCnt = "0";

            try
            {
                strCnt = Convert.ToString(db.ExecuteScalar(Extensions.LoadSqlCommand("POS_ED", "P003GetSAT900T"),
                    new string[] {
                        "@DD_SALE",
                        "@CD_STORE",
                        "@NO_POS",
                    },
                    new object[] {
                        ConfigData.Current.AppConfig.PosInfo.SaleDate,
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo
                    }));
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                db.EndInstance();
            }

            if (m_view != null)
            {
                m_view.SetWait(TypeHelper.ToInt32(strCnt));
            }
        }

        /// <summary>
        /// POS 정산
        /// </summary>
        /// <param name="strCasNm"></param>
        /// <param name="strSaleDate"></param>
        public void SetTran(WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiMsgBar01, WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiMsgBar02)
        {
            bool b301 = false;
            bool b302 = false;

            DataSet ds = null;
            var transdb = TranDbHelper.InitInstance();
            var trans = transdb.BeginTransaction();
            BasketHeader header = new BasketHeader();

            try
            {
                #region SAT301T 삭제

                string strSqlP002SetDeleteSAT301T = Extensions.LoadSqlCommand("POS_ED", "P002SetDeleteSAT301T");

                //SAT301T 삭제
                transdb.ExecuteNonQuery(strSqlP002SetDeleteSAT301T,
                        new string[] { "@DD_SALE", "@CD_STORE", "@NO_POS", "@ID_USER" },
                        new object[] {
                                ConfigData.Current.AppConfig.PosInfo.SaleDate,
                                ConfigData.Current.AppConfig.PosInfo.StoreNo,
                                ConfigData.Current.AppConfig.PosInfo.PosNo,
                                ConfigData.Current.AppConfig.PosInfo.CasNo
                            }, trans);


                osiMsgBar01.MessageText = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00308");
                osiMsgBar01.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.OK;
                Application.DoEvents();

                b301 = true;

                #endregion

                #region SAT302T 조회

                string strSqlP003GetSelectSAT302T = Extensions.LoadSqlCommand("POS_ED", "GetTotal") + 
                    Extensions.LoadSqlCommand("POS_ED", "GetTotalItem") + 
                    Extensions.LoadSqlCommand("POS_ED", "P003GetSelectSAT300T");

                ds = transdb.ExecuteQuery(strSqlP003GetSelectSAT302T,
                        new string[] { "@DD_SALE", "@CD_STORE", "@NO_POS" },
                        new object[] {
                                ConfigData.Current.AppConfig.PosInfo.SaleDate,
                                ConfigData.Current.AppConfig.PosInfo.StoreNo,
                                ConfigData.Current.AppConfig.PosInfo.PosNo
                            }, trans);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    b302 = true;

                    #region SAT010T, SAT011T저장

                    //Sign Off Tran 먼저 발생후 마감 Tran
                    if (ConfigData.Current.AppConfig.PosInfo.CasNo.Length > 0)
                    {
                        //Sign Off
                        BasketHeader headerSingOff = new BasketHeader()
                        {
                            CasNo = ConfigData.Current.AppConfig.PosInfo.CasNo,
                            CasName = ConfigData.Current.AppConfig.PosInfo.CasName,
                            TrxnType = NetCommConstants.TRXN_TYPE_SIGNOFF,
                            CancType = "0"
                        };

                        TransManager.SaveTrans(headerSingOff, null, transdb, trans);

                        // SaveTrans 함수는 한TRANS에 여러번호출하면 
                        // TrxnNo증가해줘야한다
                        TransManager.OnTransComplete();
                    }

                    #region Basket 생성

                    #region Basket header

                    header = new BasketHeader();
                    header.TrxnType = NetCommConstants.TRXN_TYPE_POS_CLOSE;
                    header.CancType = NetCommConstants.CANCEL_TYPE_NORMAL;

                    #endregion

                    #region Basket details

                    DataRow dr = ds.Tables[0].Rows[0];
                    BasketAccount basketAccount = new BasketAccount();
                    basketAccount.BasketType = BasketTypes.BasketAccount;
                    basketAccount.AccountCode_A00 = NetCommConstants.ID_ITEM_A00;
                    basketAccount.AccountAmt_A00 = dr["AMT_" + NetCommConstants.ID_ITEM_A00] != null && dr["AMT_" + NetCommConstants.ID_ITEM_A00].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_A00].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_A00].ToString() : "";
                    basketAccount.AccountCnt_A00 = dr["CNT_" + NetCommConstants.ID_ITEM_A00] != null && dr["CNT_" + NetCommConstants.ID_ITEM_A00].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_A00].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_A00].ToString() : "";
                    basketAccount.AccountCode_A01 = NetCommConstants.ID_ITEM_A01;
                    basketAccount.AccountAmt_A01 = dr["AMT_" + NetCommConstants.ID_ITEM_A01] != null && dr["AMT_" + NetCommConstants.ID_ITEM_A01].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_A01].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_A01].ToString() : "";
                    basketAccount.AccountCnt_A01 = dr["CNT_" + NetCommConstants.ID_ITEM_A01] != null && dr["CNT_" + NetCommConstants.ID_ITEM_A01].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_A01].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_A01].ToString() : "";
                    basketAccount.AccountCode_A02 = NetCommConstants.ID_ITEM_A02;
                    basketAccount.AccountAmt_A02 = dr["AMT_" + NetCommConstants.ID_ITEM_A02] != null && dr["AMT_" + NetCommConstants.ID_ITEM_A02].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_A02].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_A02].ToString() : "";
                    basketAccount.AccountCnt_A02 = dr["CNT_" + NetCommConstants.ID_ITEM_A02] != null && dr["CNT_" + NetCommConstants.ID_ITEM_A02].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_A02].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_A02].ToString() : "";
                    basketAccount.AccountCode_A03 = NetCommConstants.ID_ITEM_A03;
                    basketAccount.AccountAmt_A03 = dr["AMT_" + NetCommConstants.ID_ITEM_A03] != null && dr["AMT_" + NetCommConstants.ID_ITEM_A03].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_A03].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_A03].ToString() : "";
                    basketAccount.AccountCnt_A03 = dr["CNT_" + NetCommConstants.ID_ITEM_A03] != null && dr["CNT_" + NetCommConstants.ID_ITEM_A03].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_A03].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_A03].ToString() : "";
                    basketAccount.AccountCode_A04 = NetCommConstants.ID_ITEM_A04;
                    basketAccount.AccountAmt_A04 = dr["AMT_" + NetCommConstants.ID_ITEM_A04] != null && dr["AMT_" + NetCommConstants.ID_ITEM_A04].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_A04].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_A04].ToString() : "";
                    basketAccount.AccountCnt_A04 = dr["CNT_" + NetCommConstants.ID_ITEM_A04] != null && dr["CNT_" + NetCommConstants.ID_ITEM_A04].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_A04].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_A04].ToString() : "";
                    basketAccount.AccountCode_A05 = NetCommConstants.ID_ITEM_A05;
                    basketAccount.AccountAmt_A05 = dr["AMT_" + NetCommConstants.ID_ITEM_A05] != null && dr["AMT_" + NetCommConstants.ID_ITEM_A05].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_A05].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_A05].ToString() : "";
                    basketAccount.AccountCnt_A05 = dr["CNT_" + NetCommConstants.ID_ITEM_A05] != null && dr["CNT_" + NetCommConstants.ID_ITEM_A05].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_A05].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_A05].ToString() : "";
                    basketAccount.AccountCode_A06 = NetCommConstants.ID_ITEM_A06;
                    basketAccount.AccountAmt_A06 = dr["AMT_" + NetCommConstants.ID_ITEM_A06] != null && dr["AMT_" + NetCommConstants.ID_ITEM_A06].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_A06].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_A06].ToString() : "";
                    basketAccount.AccountCnt_A06 = dr["CNT_" + NetCommConstants.ID_ITEM_A06] != null && dr["CNT_" + NetCommConstants.ID_ITEM_A06].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_A06].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_A06].ToString() : "";

                    basketAccount.AccountCode_B00 = NetCommConstants.ID_ITEM_B00;
                    basketAccount.AccountAmt_B00 = dr["AMT_" + NetCommConstants.ID_ITEM_B00] != null && dr["AMT_" + NetCommConstants.ID_ITEM_B00].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_B00].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_B00].ToString() : "";
                    basketAccount.AccountCnt_B00 = dr["CNT_" + NetCommConstants.ID_ITEM_B00] != null && dr["CNT_" + NetCommConstants.ID_ITEM_B00].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_B00].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_B00].ToString() : "";
                    basketAccount.AccountCode_B01 = NetCommConstants.ID_ITEM_B01;
                    basketAccount.AccountAmt_B01 = dr["AMT_" + NetCommConstants.ID_ITEM_B01] != null && dr["AMT_" + NetCommConstants.ID_ITEM_B01].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_B01].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_B01].ToString() : "";
                    basketAccount.AccountCnt_B01 = dr["CNT_" + NetCommConstants.ID_ITEM_B01] != null && dr["CNT_" + NetCommConstants.ID_ITEM_B01].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_B01].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_B01].ToString() : "";

                    basketAccount.AccountCode_C00 = NetCommConstants.ID_ITEM_C00;
                    basketAccount.AccountAmt_C00 = dr["AMT_" + NetCommConstants.ID_ITEM_C00] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C00].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C00].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C00].ToString() : "";
                    basketAccount.AccountCnt_C00 = dr["CNT_" + NetCommConstants.ID_ITEM_C00] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C00].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C00].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C00].ToString() : "";
                    basketAccount.AccountCode_C01 = NetCommConstants.ID_ITEM_C01;
                    basketAccount.AccountAmt_C01 = dr["AMT_" + NetCommConstants.ID_ITEM_C01] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C01].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C01].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C01].ToString() : "";
                    basketAccount.AccountCnt_C01 = dr["CNT_" + NetCommConstants.ID_ITEM_C01] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C01].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C01].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C01].ToString() : "";
                    basketAccount.AccountCode_C02 = NetCommConstants.ID_ITEM_C02;
                    basketAccount.AccountAmt_C02 = dr["AMT_" + NetCommConstants.ID_ITEM_C02] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C02].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C02].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C02].ToString() : "";
                    basketAccount.AccountCnt_C02 = dr["CNT_" + NetCommConstants.ID_ITEM_C02] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C02].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C02].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C02].ToString() : "";
                    basketAccount.AccountCode_C03 = NetCommConstants.ID_ITEM_C03;
                    basketAccount.AccountAmt_C03 = dr["AMT_" + NetCommConstants.ID_ITEM_C03] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C03].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C03].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C03].ToString() : "";
                    basketAccount.AccountCnt_C03 = dr["CNT_" + NetCommConstants.ID_ITEM_C03] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C03].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C03].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C03].ToString() : "";
                    basketAccount.AccountCode_C04 = NetCommConstants.ID_ITEM_C04;
                    basketAccount.AccountAmt_C04 = dr["AMT_" + NetCommConstants.ID_ITEM_C04] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C04].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C04].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C04].ToString() : "";
                    basketAccount.AccountCnt_C04 = dr["CNT_" + NetCommConstants.ID_ITEM_C04] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C04].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C04].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C04].ToString() : "";
                    basketAccount.AccountCode_C05 = NetCommConstants.ID_ITEM_C05;
                    basketAccount.AccountAmt_C05 = dr["AMT_" + NetCommConstants.ID_ITEM_C05] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C05].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C05].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C05].ToString() : "";
                    basketAccount.AccountCnt_C05 = dr["CNT_" + NetCommConstants.ID_ITEM_C05] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C05].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C05].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C05].ToString() : "";
                    basketAccount.AccountCode_C06 = NetCommConstants.ID_ITEM_C06;
                    basketAccount.AccountAmt_C06 = dr["AMT_" + NetCommConstants.ID_ITEM_C06] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C06].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C06].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C06].ToString() : "";
                    basketAccount.AccountCnt_C06 = dr["CNT_" + NetCommConstants.ID_ITEM_C06] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C06].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C06].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C06].ToString() : "";
                    basketAccount.AccountCode_C07 = NetCommConstants.ID_ITEM_C07;
                    basketAccount.AccountAmt_C07 = dr["AMT_" + NetCommConstants.ID_ITEM_C07] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C07].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C07].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C07].ToString() : "";
                    basketAccount.AccountCnt_C07 = dr["CNT_" + NetCommConstants.ID_ITEM_C07] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C07].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C07].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C07].ToString() : "";
                    basketAccount.AccountCode_C08 = NetCommConstants.ID_ITEM_C08;
                    basketAccount.AccountAmt_C08 = dr["AMT_" + NetCommConstants.ID_ITEM_C08] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C08].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C08].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C08].ToString() : "";
                    basketAccount.AccountCnt_C08 = dr["CNT_" + NetCommConstants.ID_ITEM_C08] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C08].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C08].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C08].ToString() : "";
                    basketAccount.AccountCode_C09 = NetCommConstants.ID_ITEM_C09;
                    basketAccount.AccountAmt_C09 = dr["AMT_" + NetCommConstants.ID_ITEM_C09] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C09].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C09].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C09].ToString() : "";
                    basketAccount.AccountCnt_C09 = dr["CNT_" + NetCommConstants.ID_ITEM_C09] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C09].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C09].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C09].ToString() : "";
                    basketAccount.AccountCode_C10 = NetCommConstants.ID_ITEM_C10;
                    basketAccount.AccountAmt_C10 = dr["AMT_" + NetCommConstants.ID_ITEM_C10] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C10].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C10].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C10].ToString() : "";
                    basketAccount.AccountCnt_C10 = dr["CNT_" + NetCommConstants.ID_ITEM_C10] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C10].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C10].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C10].ToString() : "";
                    basketAccount.AccountCode_C11 = NetCommConstants.ID_ITEM_C11;
                    basketAccount.AccountAmt_C11 = dr["AMT_" + NetCommConstants.ID_ITEM_C11] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C11].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C11].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C11].ToString() : "";
                    basketAccount.AccountCnt_C11 = dr["CNT_" + NetCommConstants.ID_ITEM_C11] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C11].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C11].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C11].ToString() : "";
                    basketAccount.AccountCode_C12 = NetCommConstants.ID_ITEM_C12;
                    basketAccount.AccountAmt_C12 = dr["AMT_" + NetCommConstants.ID_ITEM_C12] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C12].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C12].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C12].ToString() : "";
                    basketAccount.AccountCnt_C12 = dr["CNT_" + NetCommConstants.ID_ITEM_C12] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C12].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C12].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C12].ToString() : "";
                    basketAccount.AccountCode_C13 = NetCommConstants.ID_ITEM_C13;
                    basketAccount.AccountAmt_C13 = dr["AMT_" + NetCommConstants.ID_ITEM_C13] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C13].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C13].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C13].ToString() : "";
                    basketAccount.AccountCnt_C13 = dr["CNT_" + NetCommConstants.ID_ITEM_C13] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C13].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C13].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C13].ToString() : "";
                    basketAccount.AccountCode_C14 = NetCommConstants.ID_ITEM_C14;
                    basketAccount.AccountAmt_C14 = dr["AMT_" + NetCommConstants.ID_ITEM_C14] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C14].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C14].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C14].ToString() : "";
                    basketAccount.AccountCnt_C14 = dr["CNT_" + NetCommConstants.ID_ITEM_C14] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C14].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C14].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C14].ToString() : "";
                    basketAccount.AccountCode_C15 = NetCommConstants.ID_ITEM_C15;
                    basketAccount.AccountAmt_C15 = dr["AMT_" + NetCommConstants.ID_ITEM_C15] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C15].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C15].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C15].ToString() : "";
                    basketAccount.AccountCnt_C15 = dr["CNT_" + NetCommConstants.ID_ITEM_C15] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C15].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C15].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C15].ToString() : "";
                    basketAccount.AccountCode_C16 = NetCommConstants.ID_ITEM_C16;
                    basketAccount.AccountAmt_C16 = dr["AMT_" + NetCommConstants.ID_ITEM_C16] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C16].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C16].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C16].ToString() : "";
                    basketAccount.AccountCnt_C16 = dr["CNT_" + NetCommConstants.ID_ITEM_C16] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C16].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C16].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C16].ToString() : "";
                    basketAccount.AccountCode_C17 = NetCommConstants.ID_ITEM_C17;
                    basketAccount.AccountAmt_C17 = dr["AMT_" + NetCommConstants.ID_ITEM_C17] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C17].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C17].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C17].ToString() : "";
                    basketAccount.AccountCnt_C17 = dr["CNT_" + NetCommConstants.ID_ITEM_C17] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C17].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C17].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C17].ToString() : "";
                    basketAccount.AccountCode_C18 = NetCommConstants.ID_ITEM_C18;
                    basketAccount.AccountAmt_C18 = dr["AMT_" + NetCommConstants.ID_ITEM_C18] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C18].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C18].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C18].ToString() : "";
                    basketAccount.AccountCnt_C18 = dr["CNT_" + NetCommConstants.ID_ITEM_C18] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C18].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C18].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C18].ToString() : "";
                    basketAccount.AccountCode_C19 = NetCommConstants.ID_ITEM_C19;
                    basketAccount.AccountAmt_C19 = dr["AMT_" + NetCommConstants.ID_ITEM_C19] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C19].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C19].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C19].ToString() : "";
                    basketAccount.AccountCnt_C19 = dr["CNT_" + NetCommConstants.ID_ITEM_C19] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C19].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C19].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C19].ToString() : "";
                    basketAccount.AccountCode_C20 = NetCommConstants.ID_ITEM_C20;
                    basketAccount.AccountAmt_C20 = dr["AMT_" + NetCommConstants.ID_ITEM_C20] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C20].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C20].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C20].ToString() : "";
                    basketAccount.AccountCnt_C20 = dr["CNT_" + NetCommConstants.ID_ITEM_C20] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C20].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C20].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C20].ToString() : "";
                    basketAccount.AccountCode_C21 = NetCommConstants.ID_ITEM_C21;
                    basketAccount.AccountAmt_C21 = dr["AMT_" + NetCommConstants.ID_ITEM_C21] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C21].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C21].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C21].ToString() : "";
                    basketAccount.AccountCnt_C21 = dr["CNT_" + NetCommConstants.ID_ITEM_C21] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C21].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C21].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C21].ToString() : "";
                    basketAccount.AccountCode_C22 = NetCommConstants.ID_ITEM_C22;
                    basketAccount.AccountAmt_C22 = dr["AMT_" + NetCommConstants.ID_ITEM_C22] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C22].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C22].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C22].ToString() : "";
                    basketAccount.AccountCnt_C22 = dr["CNT_" + NetCommConstants.ID_ITEM_C22] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C22].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C22].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C22].ToString() : "";
                    basketAccount.AccountCode_C23 = NetCommConstants.ID_ITEM_C23;
                    basketAccount.AccountAmt_C23 = dr["AMT_" + NetCommConstants.ID_ITEM_C23] != null && dr["AMT_" + NetCommConstants.ID_ITEM_C23].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_C23].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_C23].ToString() : "";
                    basketAccount.AccountCnt_C23 = dr["CNT_" + NetCommConstants.ID_ITEM_C23] != null && dr["CNT_" + NetCommConstants.ID_ITEM_C23].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_C23].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_C23].ToString() : "";

                    basketAccount.AccountCode_D00 = NetCommConstants.ID_ITEM_D00;
                    basketAccount.AccountAmt_D00 = dr["AMT_" + NetCommConstants.ID_ITEM_D00] != null && dr["AMT_" + NetCommConstants.ID_ITEM_D00].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_D00].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_D00].ToString() : "";
                    basketAccount.AccountCnt_D00 = dr["CNT_" + NetCommConstants.ID_ITEM_D00] != null && dr["CNT_" + NetCommConstants.ID_ITEM_D00].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_D00].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_D00].ToString() : "";

                    basketAccount.AccountCode_E00 = NetCommConstants.ID_ITEM_E00;
                    basketAccount.AccountAmt_E00 = dr["AMT_" + NetCommConstants.ID_ITEM_E00] != null && dr["AMT_" + NetCommConstants.ID_ITEM_E00].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_E00].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_E00].ToString() : "";
                    basketAccount.AccountCnt_E00 = dr["CNT_" + NetCommConstants.ID_ITEM_E00] != null && dr["CNT_" + NetCommConstants.ID_ITEM_E00].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_E00].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_E00].ToString() : "";
                    basketAccount.AccountCode_E01 = NetCommConstants.ID_ITEM_E01;
                    basketAccount.AccountAmt_E01 = dr["AMT_" + NetCommConstants.ID_ITEM_E01] != null && dr["AMT_" + NetCommConstants.ID_ITEM_E01].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_E01].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_E01].ToString() : "";
                    basketAccount.AccountCnt_E01 = dr["CNT_" + NetCommConstants.ID_ITEM_E01] != null && dr["CNT_" + NetCommConstants.ID_ITEM_E01].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_E01].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_E01].ToString() : "";
                    basketAccount.AccountCode_E02 = NetCommConstants.ID_ITEM_E02;
                    basketAccount.AccountAmt_E02 = dr["AMT_" + NetCommConstants.ID_ITEM_E02] != null && dr["AMT_" + NetCommConstants.ID_ITEM_E02].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_E02].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_E02].ToString() : "";
                    basketAccount.AccountCnt_E02 = dr["CNT_" + NetCommConstants.ID_ITEM_E02] != null && dr["CNT_" + NetCommConstants.ID_ITEM_E02].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_E02].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_E02].ToString() : "";
                    basketAccount.AccountCode_E03 = NetCommConstants.ID_ITEM_E03;
                    basketAccount.AccountAmt_E03 = dr["AMT_" + NetCommConstants.ID_ITEM_E03] != null && dr["AMT_" + NetCommConstants.ID_ITEM_E03].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_E03].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_E03].ToString() : "";
                    basketAccount.AccountCnt_E03 = dr["CNT_" + NetCommConstants.ID_ITEM_E03] != null && dr["CNT_" + NetCommConstants.ID_ITEM_E03].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_E03].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_E03].ToString() : "";
                    basketAccount.AccountCode_E04 = NetCommConstants.ID_ITEM_E04;
                    basketAccount.AccountAmt_E04 = dr["AMT_" + NetCommConstants.ID_ITEM_E04] != null && dr["AMT_" + NetCommConstants.ID_ITEM_E04].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_E04].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_E04].ToString() : "";
                    basketAccount.AccountCnt_E04 = dr["CNT_" + NetCommConstants.ID_ITEM_E04] != null && dr["CNT_" + NetCommConstants.ID_ITEM_E04].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_E04].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_E04].ToString() : "";
                    basketAccount.AccountCode_E05 = NetCommConstants.ID_ITEM_E05;
                    basketAccount.AccountAmt_E05 = dr["AMT_" + NetCommConstants.ID_ITEM_E05] != null && dr["AMT_" + NetCommConstants.ID_ITEM_E05].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_E05].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_E05].ToString() : "";
                    basketAccount.AccountCnt_E05 = dr["CNT_" + NetCommConstants.ID_ITEM_E05] != null && dr["CNT_" + NetCommConstants.ID_ITEM_E05].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_E05].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_E05].ToString() : "";
                    basketAccount.AccountCode_E10 = NetCommConstants.ID_ITEM_E10;
                    basketAccount.AccountAmt_E10 = dr["AMT_" + NetCommConstants.ID_ITEM_E10] != null && dr["AMT_" + NetCommConstants.ID_ITEM_E10].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_E10].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_E10].ToString() : "";
                    basketAccount.AccountCnt_E10 = dr["CNT_" + NetCommConstants.ID_ITEM_E10] != null && dr["CNT_" + NetCommConstants.ID_ITEM_E10].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_E10].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_E10].ToString() : "";
                    basketAccount.AccountCode_E21 = NetCommConstants.ID_ITEM_E21;
                    basketAccount.AccountAmt_E21 = dr["AMT_" + NetCommConstants.ID_ITEM_E21] != null && dr["AMT_" + NetCommConstants.ID_ITEM_E21].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_E21].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_E21].ToString() : "";
                    basketAccount.AccountCnt_E21 = dr["CNT_" + NetCommConstants.ID_ITEM_E21] != null && dr["CNT_" + NetCommConstants.ID_ITEM_E21].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_E21].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_E21].ToString() : "";
                    basketAccount.AccountCode_E22 = NetCommConstants.ID_ITEM_E22;
                    basketAccount.AccountAmt_E22 = dr["AMT_" + NetCommConstants.ID_ITEM_E22] != null && dr["AMT_" + NetCommConstants.ID_ITEM_E22].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_E22].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_E22].ToString() : "";
                    basketAccount.AccountCnt_E22 = dr["CNT_" + NetCommConstants.ID_ITEM_E22] != null && dr["CNT_" + NetCommConstants.ID_ITEM_E22].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_E22].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_E22].ToString() : "";
                    basketAccount.AccountCode_E23 = NetCommConstants.ID_ITEM_E23;
                    basketAccount.AccountAmt_E23 = dr["AMT_" + NetCommConstants.ID_ITEM_E23] != null && dr["AMT_" + NetCommConstants.ID_ITEM_E23].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_E23].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_E23].ToString() : "";
                    basketAccount.AccountCnt_E23 = dr["CNT_" + NetCommConstants.ID_ITEM_E23] != null && dr["CNT_" + NetCommConstants.ID_ITEM_E23].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_E23].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_E23].ToString() : "";
                    basketAccount.AccountCode_E24 = NetCommConstants.ID_ITEM_E24;
                    basketAccount.AccountAmt_E24 = dr["AMT_" + NetCommConstants.ID_ITEM_E24] != null && dr["AMT_" + NetCommConstants.ID_ITEM_E24].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_E24].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_E24].ToString() : "";
                    basketAccount.AccountCnt_E24 = dr["CNT_" + NetCommConstants.ID_ITEM_E24] != null && dr["CNT_" + NetCommConstants.ID_ITEM_E24].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_E24].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_E24].ToString() : "";
                    basketAccount.AccountCode_E25 = NetCommConstants.ID_ITEM_E25;
                    basketAccount.AccountAmt_E25 = dr["AMT_" + NetCommConstants.ID_ITEM_E25] != null && dr["AMT_" + NetCommConstants.ID_ITEM_E25].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_E25].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_E25].ToString() : "";
                    basketAccount.AccountCnt_E25 = dr["CNT_" + NetCommConstants.ID_ITEM_E25] != null && dr["CNT_" + NetCommConstants.ID_ITEM_E25].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_E25].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_E25].ToString() : "";
                    basketAccount.AccountCode_E30 = NetCommConstants.ID_ITEM_E30;
                    basketAccount.AccountAmt_E30 = dr["AMT_" + NetCommConstants.ID_ITEM_E30] != null && dr["AMT_" + NetCommConstants.ID_ITEM_E30].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_E30].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_E30].ToString() : "";
                    basketAccount.AccountCnt_E30 = dr["CNT_" + NetCommConstants.ID_ITEM_E30] != null && dr["CNT_" + NetCommConstants.ID_ITEM_E30].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_E30].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_E30].ToString() : "";
                    basketAccount.AccountCode_E40 = NetCommConstants.ID_ITEM_E40;
                    basketAccount.AccountAmt_E40 = dr["AMT_" + NetCommConstants.ID_ITEM_E40] != null && dr["AMT_" + NetCommConstants.ID_ITEM_E40].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_E40].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_E40].ToString() : "";
                    basketAccount.AccountCnt_E40 = dr["CNT_" + NetCommConstants.ID_ITEM_E40] != null && dr["CNT_" + NetCommConstants.ID_ITEM_E40].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_E40].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_E40].ToString() : "";

                    basketAccount.AccountCode_F00 = NetCommConstants.ID_ITEM_F00;
                    basketAccount.AccountAmt_F00 = dr["AMT_" + NetCommConstants.ID_ITEM_F00] != null && dr["AMT_" + NetCommConstants.ID_ITEM_F00].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_F00].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_F00].ToString() : "";
                    basketAccount.AccountCnt_F00 = dr["CNT_" + NetCommConstants.ID_ITEM_F00] != null && dr["CNT_" + NetCommConstants.ID_ITEM_F00].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_F00].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_F00].ToString() : "";
                    basketAccount.AccountCode_F01 = NetCommConstants.ID_ITEM_F01;
                    basketAccount.AccountAmt_F01 = dr["AMT_" + NetCommConstants.ID_ITEM_F01] != null && dr["AMT_" + NetCommConstants.ID_ITEM_F01].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_F01].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_F01].ToString() : "";
                    basketAccount.AccountCnt_F01 = dr["CNT_" + NetCommConstants.ID_ITEM_F01] != null && dr["CNT_" + NetCommConstants.ID_ITEM_F01].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_F01].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_F01].ToString() : "";
                    basketAccount.AccountCode_F02 = NetCommConstants.ID_ITEM_F02;
                    basketAccount.AccountAmt_F02 = dr["AMT_" + NetCommConstants.ID_ITEM_F02] != null && dr["AMT_" + NetCommConstants.ID_ITEM_F02].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_F02].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_F02].ToString() : "";
                    basketAccount.AccountCnt_F02 = dr["CNT_" + NetCommConstants.ID_ITEM_F02] != null && dr["CNT_" + NetCommConstants.ID_ITEM_F02].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_F02].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_F02].ToString() : "";
                    basketAccount.AccountCode_F03 = NetCommConstants.ID_ITEM_F03;
                    basketAccount.AccountAmt_F03 = dr["AMT_" + NetCommConstants.ID_ITEM_F03] != null && dr["AMT_" + NetCommConstants.ID_ITEM_F03].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_F03].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_F03].ToString() : "";
                    basketAccount.AccountCnt_F03 = dr["CNT_" + NetCommConstants.ID_ITEM_F03] != null && dr["CNT_" + NetCommConstants.ID_ITEM_F03].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_F03].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_F03].ToString() : "";
                    basketAccount.AccountCode_F04 = NetCommConstants.ID_ITEM_F04;
                    basketAccount.AccountAmt_F04 = dr["AMT_" + NetCommConstants.ID_ITEM_F04] != null && dr["AMT_" + NetCommConstants.ID_ITEM_F04].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_F04].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_F04].ToString() : "";
                    basketAccount.AccountCnt_F04 = dr["CNT_" + NetCommConstants.ID_ITEM_F04] != null && dr["CNT_" + NetCommConstants.ID_ITEM_F04].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_F04].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_F04].ToString() : "";
                    basketAccount.AccountCode_F05 = NetCommConstants.ID_ITEM_F05;
                    basketAccount.AccountAmt_F05 = dr["AMT_" + NetCommConstants.ID_ITEM_F05] != null && dr["AMT_" + NetCommConstants.ID_ITEM_F05].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_F05].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_F05].ToString() : "";
                    basketAccount.AccountCnt_F05 = dr["CNT_" + NetCommConstants.ID_ITEM_F05] != null && dr["CNT_" + NetCommConstants.ID_ITEM_F05].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_F05].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_F05].ToString() : "";

                    basketAccount.AccountCode_G00 = NetCommConstants.ID_ITEM_G00;
                    basketAccount.AccountAmt_G00 = dr["AMT_" + NetCommConstants.ID_ITEM_G00] != null && dr["AMT_" + NetCommConstants.ID_ITEM_G00].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_G00].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_G00].ToString() : "";
                    basketAccount.AccountCnt_G00 = dr["CNT_" + NetCommConstants.ID_ITEM_G00] != null && dr["CNT_" + NetCommConstants.ID_ITEM_G00].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_G00].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_G00].ToString() : "";
                    basketAccount.AccountCode_G01 = NetCommConstants.ID_ITEM_G01;
                    basketAccount.AccountAmt_G01 = dr["AMT_" + NetCommConstants.ID_ITEM_G01] != null && dr["AMT_" + NetCommConstants.ID_ITEM_G01].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_G01].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_G01].ToString() : "";
                    basketAccount.AccountCnt_G01 = dr["CNT_" + NetCommConstants.ID_ITEM_G01] != null && dr["CNT_" + NetCommConstants.ID_ITEM_G01].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_G01].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_G01].ToString() : "";
                    basketAccount.AccountCode_G02 = NetCommConstants.ID_ITEM_G02;
                    basketAccount.AccountAmt_G02 = dr["AMT_" + NetCommConstants.ID_ITEM_G02] != null && dr["AMT_" + NetCommConstants.ID_ITEM_G02].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_G02].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_G02].ToString() : "";
                    basketAccount.AccountCnt_G02 = dr["CNT_" + NetCommConstants.ID_ITEM_G02] != null && dr["CNT_" + NetCommConstants.ID_ITEM_G02].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_G02].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_G02].ToString() : "";
                    basketAccount.AccountCode_G03 = NetCommConstants.ID_ITEM_G03;
                    basketAccount.AccountAmt_G03 = dr["AMT_" + NetCommConstants.ID_ITEM_G03] != null && dr["AMT_" + NetCommConstants.ID_ITEM_G03].ToString() != "" && dr["AMT_" + NetCommConstants.ID_ITEM_G03].ToString() != "0" ? dr["AMT_" + NetCommConstants.ID_ITEM_G03].ToString() : "";
                    basketAccount.AccountCnt_G03 = dr["CNT_" + NetCommConstants.ID_ITEM_G03] != null && dr["CNT_" + NetCommConstants.ID_ITEM_G03].ToString() != "" && dr["CNT_" + NetCommConstants.ID_ITEM_G03].ToString() != "0" ? dr["CNT_" + NetCommConstants.ID_ITEM_G03].ToString() : "";

                    #endregion

                    #endregion

                    // Loc수정 06.04            
                    TransManager.SaveTrans(header, new BasketBase[] { basketAccount }, transdb, trans);

                    osiMsgBar02.MessageText = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00390");
                    osiMsgBar02.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.OK;
                    Application.DoEvents();

                    #endregion
                }
                else
                {
                    osiMsgBar02.MessageText = "POS 정산정보 저장 실패";
                    osiMsgBar02.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.Error;
                    Application.DoEvents();
                }

                #endregion

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
            }
            finally
            {
                transdb.EndInstance();

                if (!b301)
                {
                    osiMsgBar01.MessageText = "POS 부분매출정보 삭제 실패";
                    osiMsgBar01.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.Error;
                    Application.DoEvents();
                }
                else
                {
                    if (b302)
                    {
                        ConfigData.Current.AppConfig.PosInfo.CasNo = string.Empty;    //계산원 코드
                        ConfigData.Current.AppConfig.PosInfo.CasName = string.Empty;  //계산원명
                        ConfigData.Current.AppConfig.PosInfo.CasPass = string.Empty;  //계산원 비밀번호
                        ConfigData.Current.AppConfig.Save();
                    }
                }
            }

            if (m_view != null)
            {
                m_view.SetTranPrint(ds, header);
            }
        }

        /// <summary>
        /// SAT011 업데이트 확인
        /// </summary>
        public void GetTranConfirm()
        {
            string strSqlP003GetSelectSAT011T = Extensions.LoadSqlCommand("POS_ED", "P003GetSelectSAT011T");

            var db = TranDbHelper.InitInstance();
            string strCnt = "0";

            try
            {
                strCnt = Convert.ToString(db.ExecuteScalar(strSqlP003GetSelectSAT011T,
                    new string[] {
                        "@DD_SALE",
                        "@CD_STORE",
                        "@NO_POS",
                    },
                    new object[] {
                        ConfigData.Current.AppConfig.PosInfo.SaleDate,
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo
                    }));
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                db.EndInstance();
            }

            if (m_view != null)
            {
                m_view.SetTranConfirm(TypeHelper.ToInt32(strCnt));
            }
        }

        #endregion
    }
}
