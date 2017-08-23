using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

using WSWD.WmallPos.FX.NetComm.Data.Basket;
using WSWD.WmallPos.FX.NetComm.Data;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.NetComm.Data.Types;

namespace WSWD.WmallPos.FX.BO.Trans
{
    /// <summary>
    /// 매출TRANS 관리하는 Class
    /// </summary>
    public class TransManager
    {
        /// <summary>
        /// 매출 SAT010T, SAT011T저장 공통함수
        /// </summary>
        /// <param name="header"></param>
        /// <param name="details"></param>
        /// <param name="trans"></param>
        public static void SaveTrans(BasketHeader header, BasketBase[] details, TranDbHelper db, SQLiteTransaction trans)
        {
            string queryStringSAT010T = Extensions.LoadSqlCommand("TransManager", "SaveTrans");
            string queryStringSAT011T = Extensions.LoadSqlCommand("TransManager", "SaveTransState");

            // buffer sequence
            int sqTrxn = 0;

            #region SAT010T - Header

            // save to SAT010T

            // add header
            db.ExecuteNonQuery(queryStringSAT010T,
                new string[] {
                        "@DD_SALE",
                        "@CD_STORE",
                        "@NO_POS",
                        "@NO_TRXN",
                        "@SQ_TRXN",
                        "@VC_CONT",
                        "@ID_USER",
                        "@SQ_SHIFT"                        
                    }, new object[] {
                        ConfigData.Current.AppConfig.PosInfo.SaleDate,
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo,
                        ConfigData.Current.AppConfig.PosInfo.TrxnNo,
                        sqTrxn.ToString().ToString(3, TypeProperties.Number),
                        header.ToString(),
                        ConfigData.Current.AppConfig.PosInfo.CasNo,
                        ConfigData.Current.AppConfig.PosInfo.ShiftCount
                    }, trans);

            sqTrxn++;

            #endregion

            #region 상세내역, BasketBase list

            if (details != null)
            {
                for (int i = 0; i < details.Length; i++)
                {
                    db.ExecuteNonQuery(queryStringSAT010T,
                    new string[] {
                        "@DD_SALE",
                        "@CD_STORE",
                        "@NO_POS",
                        "@NO_TRXN",
                        "@SQ_TRXN",
                        "@VC_CONT",
                        "@ID_USER",
                        "@SQ_SHIFT"                        
                    }, new object[] {
                        ConfigData.Current.AppConfig.PosInfo.SaleDate,
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo,
                        ConfigData.Current.AppConfig.PosInfo.TrxnNo,
                        sqTrxn.ToString().ToString(3, TypeProperties.Number),
                        details[i].ToString(),
                        ConfigData.Current.AppConfig.PosInfo.CasNo,
                        ConfigData.Current.AppConfig.PosInfo.ShiftCount
                    }, trans);

                    sqTrxn++;
                }
            }

            #endregion

            #region Basket end

            db.ExecuteNonQuery(queryStringSAT010T,
                new string[] {
                        "@DD_SALE",
                        "@CD_STORE",
                        "@NO_POS",
                        "@NO_TRXN",
                        "@SQ_TRXN",
                        "@VC_CONT",
                        "@ID_USER",
                        "@SQ_SHIFT"                        
                    }, new object[] {
                        ConfigData.Current.AppConfig.PosInfo.SaleDate,
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo,
                        ConfigData.Current.AppConfig.PosInfo.TrxnNo,
                        sqTrxn.ToString().ToString(3, TypeProperties.Number),
                        new BasketEnd().ToString(),
                        ConfigData.Current.AppConfig.PosInfo.CasNo,
                        ConfigData.Current.AppConfig.PosInfo.ShiftCount
                    }, trans);

            #endregion

            #region SAT011T

            db.ExecuteNonQuery(queryStringSAT011T,
                new string[] {
                        "@DD_SALE",
                        "@CD_STORE",
                        "@NO_POS",
                        "@NO_TRXN",
                        "@FG_TRXN",
                        "@FG_CANC",
                        "@ID_USER",
                        "@DD_TIME"                        
                    }, new object[] {
                        ConfigData.Current.AppConfig.PosInfo.SaleDate,
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo,
                        ConfigData.Current.AppConfig.PosInfo.TrxnNo,
                        header.TrxnType,
                        header.CancType,
                        ConfigData.Current.AppConfig.PosInfo.CasNo,
                        DateTimeUtils.GetYearMonthDayTimeString(DateTime.Now),
                    }, trans);

            #endregion
        }

        /// <summary>
        /// 매출등록 성공시 무조건 이함수 호출
        /// </summary>
        public static void OnTransComplete()
        {
            int trxnNo = TypeHelper.ToInt32(ConfigData.Current.AppConfig.PosInfo.TrxnNo);
            trxnNo++;
            ConfigData.Current.AppConfig.PosInfo.TrxnNo = trxnNo.ToString().ToString(6, TypeProperties.Number);
            ConfigData.Current.AppConfig.Save();
        }
    }
}
