//-----------------------------------------------------------------
/*
 * 화면명   : IQP002presenter.cs
 * 화면설명 : 카드사별 매출 조회
 * 개발자   : 정광호
 * 개발일자 : 2015.04.01
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using WSWD.WmallPos.POS.IQ.PI;
using WSWD.WmallPos.POS.IQ.VI;
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

namespace WSWD.WmallPos.POS.IQ.PT
{
    public class IQP002presenter : IIQP002presenter
    {
        private IIQP002View m_view;
        public IQP002presenter(IIQP002View view)
        {
            m_view = view;
        }

        #region IPresenter Members

        /// <summary>
        /// 카드사별 매출조회
        /// </summary>
        public void GetCardSaleResult()
        {
            string strSql = Extensions.LoadSqlCommand("POS_IQ", "P002GetCardSaleSAT304T");

            // Master db 이용합니다.
            var db = TranDbHelper.InitInstance();
            DataSet ds = null;

            try
            {
                ds = db.ExecuteQuery(strSql,
                    new string[] {
                        "@DD_SALE",
                        "@CD_STORE",
                        "@NO_POS"
                    },
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
                db.EndInstance();
            }

            if (m_view != null)
            {
                m_view.SetCardSaleResult(ds);
            }
        }

        #endregion
    }
}
