using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.POS.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.FX.Shared.Exceptions;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared;
using System.Data.SQLite;
using WSWD.WmallPos.FX.BO.Trans;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.TM.Samples
{
    public class TestUtils
    {
        /// <summary>
        /// Exception 처리
        /// Trace Write
        /// Journal write
        /// </summary>
        public void ExceptionHandler()
        {
            try
            {
                // trace 한다
                TraceHelper.Instance.TraceWrite("ExceptionHandler", "Started of ExceptionHandler");

                // 저널저장한다
                TraceHelper.Instance.JournalWrite("JrnItem", "More info");

                // exception throw
                // ER00001은 PostMesg.dat에 등록 되어 있음
                throw new Exception("테스트");
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
        }

        /// <summary>
        /// Db접속예제
        /// </summary>
        public void SQLiteAccess()
        {
            /// 
            /// method 1
            /// Transaction 처리 안함
            /// 
            using (var db = TranDbHelper.InitInstance())
            {
                string queryString = Extensions.LoadSqlCommand("FileName", "CommandName");
                db.ExecuteNonQuery(queryString,
                    new string[] {
                        "@CD_STORE"
                    }, new object[] {
                        ConfigData.Current.AppConfig.PosInfo.StoreNo
                    });
            }

            /// 
            /// method 2
            /// Transacition 사용
            /// 
            TranDbHelper tranDb = null;
            SQLiteTransaction trans = null;
            try
            {
                tranDb = TranDbHelper.InitInstance();
                trans = tranDb.BeginTransaction();

                BasketHeader header = new BasketHeader();

                #region Basket header

                // header.TxBufVer...

                #endregion

                #region Basket details

                // 준비금
                BasketPay prepareAmt = new BasketPay();
                prepareAmt.BalAmt = "112121";

                #endregion

                // SAT010T, SAT011T저장
                TransManager.SaveTrans(header, new BasketBase[] {
                    prepareAmt
                }, tranDb, trans);

                #region Summary table 처리, SAT300T...

                // get query string from file
                string queryString = Extensions.LoadSqlCommand("FileName", "CommandName");
                tranDb.ExecuteNonQuery(queryString,
                    new string[] {
                        "@CD_STORE"
                    }, new object[] {
                        ConfigData.Current.AppConfig.PosInfo.StoreNo
                    }, trans); // transaction 개체준다


                #endregion

                trans.Commit();

                // TR 완료시
                TransManager.OnTransComplete();
            }
            catch (Exception ex)
            {
                trans.Rollback();

                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                if (tranDb != null)
                {
                    tranDb.Dispose();
                }
            }


        }

        /// <summary>
        /// Config data 사용법
        /// </summary>
        public void ConfigDataRead()
        {
            // 점포코드
            string storeNo = ConfigData.Current.AppConfig.PosInfo.StoreNo;

            // 해당 Config항목을 문서 참고 해서 위방법으로 이용한다.
        }
    }
}
