//-----------------------------------------------------------------
/*
 * 화면명   : EDP001presenter.cs
 * 화면설명 : 합계 점검 조회
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
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.ED.PT
{
    public class EDP001presenter : IEDP001presenter
    {
        private IEDP001View m_view;
        public EDP001presenter(IEDP001View view)
        {
            m_view = view;
        }

        #region IPresenter Members

        /// <summary>
        /// 합계 점검 조회
        /// </summary>
        public void GetTotalChkResult()
        {
            string strSql = Extensions.LoadSqlCommand("POS_ED", "GetTotal") + Extensions.LoadSqlCommand("POS_ED", "P001GetTotalChkSAT300T");

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
                m_view.SetTotalChkResult(ds);
            }
        }

        #endregion
    }
}
