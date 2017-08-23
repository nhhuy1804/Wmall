using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using WSWD.WmallPos.POS.SL.PI;
using WSWD.WmallPos.POS.SL.VI;
using WSWD.WmallPos.POS.SL.Data;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Data;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;

namespace WSWD.WmallPos.POS.SL.PT
{
    /// <summary>
    /// 보류/보류해제
    /// </summary>
    public class SLP003Presenter : ISLP003Presenter
    {
        private ISLP003View m_view;
        private ISLM001HoldView m_mainView;
        public SLP003Presenter(ISLP003View view)
        {
            m_view = view;
        }

        public SLP003Presenter(ISLM001HoldView view)
        {
            m_mainView = view;
        }

        #region ISLP003Presenter Members

        public void LoadHoldList()
        {
            var db = TranDbHelper.InitInstance();
            try
            {
                string query = "P003HoldList".POSSLQuerySQL();
                var ds = db.ExecuteQuery(query,
                    new string[] {
                        "@DD_SALE", "@CD_STORE", "@NO_POS"
                    },
                    new object[] {
                        ConfigData.Current.AppConfig.PosInfo.SaleDate,
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo
                    });

                List<SAT900TData> holdList = new List<SAT900TData>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string time = TypeHelper.ToString(dr["DD_TIME"]);
                    time = time.Length == 14 ? time.Substring(8, 2) + ":" + time.Substring(10, 2) : string.Empty;
                    holdList.Add(new SAT900TData()
                        {
                            NoBoru = TypeHelper.ToString(dr["NO_BORU"]),
                            AmSale = (long)TypeHelper.ToDouble(dr["AM_SALE"]),
                            DdTime = time
                        });
                }

                m_view.BindHoldList(holdList.ToArray());
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
        /// Return null of there is more than 2 hold trxn
        /// Else return full data
        /// </summary>
        /// <param name="noBoru"></param>
        /// <returns></returns>
        public bool CheckHoldTrxnExists(string noBoru)
        {
            var db = TranDbHelper.InitInstance();
            try
            {
                string query = "P003HoldCount".POSSLQuerySQL();
                var count = db.ExecuteScalar(query,
                    new string[] {
                        "@DD_SALE", "@CD_STORE", 
                        "@NO_POS", "@NO_BORU"
                    },
                    new object[] {
                        ConfigData.Current.AppConfig.PosInfo.SaleDate,
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo,
                        noBoru
                    });

                int cCount = Convert.ToInt32(count);

                if (cCount == 0)
                {
                    return false;
                }

                if (cCount > 1)
                {
                    m_mainView.ShowHoldList();
                }
                else
                {
                    var holdItems = ReleaseHoldTrxn(noBoru);
                    m_mainView.LoadItems(holdItems.Select(p => p.Basket).ToArray());
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                db.Dispose();
            }

            return true;
        }

        /// <summary>
        /// 보류진짜 해제한다
        /// </summary>
        /// <param name="noBoru"></param>
        /// <returns></returns>
        public SAT900TItemData[] ReleaseHoldTrxn(string noBoru)
        {
            var db = TranDbHelper.InitInstance();
            var trans = db.BeginTransaction();
            try
            {

                #region 보류상품항목 가져오기

                string query = "P003HoldSelectTop".POSSLQuerySQL();
                var ds = db.ExecuteQuery(query,
                    new string[] {
                        "@DD_SALE", "@CD_STORE", "@NO_POS", "@NO_BORU"
                    },
                    new object[] {
                        ConfigData.Current.AppConfig.PosInfo.SaleDate,
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo,
                        noBoru
                    }, trans);

                List<SAT900TItemData> holdList = new List<SAT900TItemData>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string vcCont = TypeHelper.ToString(dr["VC_CONT"]);
                    BasketItem bi = (BasketItem)BasketItem.Parse(typeof(BasketItem), vcCont);
                    holdList.Add(new SAT900TItemData()
                    {
                        NoBoru = TypeHelper.ToString(dr["NO_BORU"]),
                        NmItem = bi.NmItem,
                        QtItem = TypeHelper.ToInt32(bi.CntItem),
                        AmItem = TypeHelper.ToInt32(bi.AmSale),
                        SqBoru = TypeHelper.ToInt32(dr["SQ_BORU"]),
                        Basket = bi,
                        VcCont = vcCont
                    });
                }

                #endregion

                #region 보류항목해제 상태저장

                if (holdList.Count > 0)
                {
                    query = "P003HoldTrxnRelease".POSSLQuerySQL();
                    db.ExecuteNonQuery(query,
                        new string[] {
                        "@DD_SALE", "@CD_STORE", "@NO_POS", "@NO_BORU"
                    },
                        new object[] {
                        ConfigData.Current.AppConfig.PosInfo.SaleDate,
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo,
                        holdList[0].NoBoru
                    }, trans);
                }

                #endregion

                trans.Commit();
                return holdList.ToArray();
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtils.Instance.LogException(ex);
                return null;
            }
            finally
            {
                db.Dispose();
                trans.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ddSale"></param>
        /// <param name="noBoru"></param>
        public void LoadHoldItems(string ddSale, string noBoru)
        {
            var db = TranDbHelper.InitInstance();
            try
            {
                #region 보류상품항목 가져오기

                string query = "P003HoldSelectTop".POSSLQuerySQL();
                var ds = db.ExecuteQuery(query,
                    new string[] {
                        "@DD_SALE", "@CD_STORE", "@NO_POS", "@NO_BORU"
                    },
                    new object[] {
                        ddSale,
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo,
                        noBoru
                    });

                if (ds.Tables[0].Rows.Count == 0)
                {
                    m_view.ReportError(SLP003HoldErrorState.NoBoruNotExists);
                }
                else
                {
                    List<SAT900TItemData> holdList = new List<SAT900TItemData>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string vcCont = TypeHelper.ToString(dr["VC_CONT"]);
                        BasketItem bi = (BasketItem)BasketItem.Parse(typeof(BasketItem), vcCont);
                        holdList.Add(new SAT900TItemData()
                        {
                            NoBoru = TypeHelper.ToString(dr["NO_BORU"]),
                            NmItem = bi.NmItem,
                            QtItem = TypeHelper.ToInt32(bi.CntItem),
                            AmItem = TypeHelper.ToInt32(bi.AmSale),
                            SqBoru = TypeHelper.ToInt32(dr["SQ_BORU"]),
                            Basket = bi,
                            VcCont = vcCont
                        });
                    }

                    m_view.ReportError(SLP003HoldErrorState.NoError);
                    m_view.BindHoldItems(holdList.ToArray());
                }

                #endregion
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

        public bool ValidateNoBoru(string noBoru)
        {
            //2015.09.12 정광호 수정---------------------------------------------------------------------------------------
            //Barcode길이가 기존 14자리에서 StoreNo를 포함한 16자리로 변경됨으로 인한 수정(기존 바코드 14자리도 동시 사용)

            //if (string.IsNullOrEmpty(noBoru) || noBoru.Length != 14)
            //{
            //    m_view.ReportError(SLP003HoldErrorState.InvalidScanNoBoru);
            //    return false;
            //}

            //string posNo = noBoru.Substring(6, 4);

            if (string.IsNullOrEmpty(noBoru) || (noBoru.Length != 14 && noBoru.Length != 16))
            {
                m_view.ReportError(SLP003HoldErrorState.InvalidScanNoBoru);
                return false;
            }

            //보류해제시 자신의 StoreNo에서만 가능
            string posNo = noBoru.Length == 14 ? noBoru.Substring(6, 4) : noBoru.Substring(8, 4);

            if (noBoru.Length == 16)
	        {
                if (noBoru.Substring(0, 2) != ConfigData.Current.AppConfig.PosInfo.StoreNo)
                {
                    m_view.ReportError(SLP003HoldErrorState.InvalidScanNoBoru);
                    return false;
                }
	        }
            //----------------------------------------------------------------------------------------------------------------

            if (!ConfigData.Current.AppConfig.PosInfo.PosNo.Equals(posNo))
            {
                m_view.ReportError(SLP003HoldErrorState.InvalidScanNoBoru);
                return false;
            }

            return true;
        }

        #endregion
    }
}
