//-----------------------------------------------------------------
/*
 * 화면명   : IQP001presenter.cs
 * 화면설명 : 품번별 매출 조회
 * 개발자   : 정광호
 * 개발일자 : 2015.03.30
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
    public class IQP001presenter : IIQP001presenter
    {
        private IIQP001View m_view;
        public IQP001presenter(IIQP001View view)
        {
            m_view = view;
        }

        #region IPresenter Members

        /// <summary>
        /// 품번별 매출조회
        /// </summary>
        public void GetItemSaleResult()
        {
            string strSql = Extensions.LoadSqlCommand("POS_IQ", "P001GetItemSaleSAT303T");

            // Master db 이용합니다.
            var db = TranDbHelper.InitInstance();
            DataSet ds = null;

            try
            {
                ds = db.ExecuteQuery(strSql,
                    new string[] {
                        "@DD_SALE",
                        "@CD_STORE",
                        "@NO_POS",
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
                m_view.SetItemSaleResult(ds);
            }
        }

        #endregion
    }
}
