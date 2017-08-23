//-----------------------------------------------------------------
/*
 * 화면명   : PYP010presenter.cs
 * 화면설명 : 할인쿠폰
 * 개발자   : 정광호
 * 개발일자 : 2015.04.21
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using WSWD.WmallPos.POS.PY.PI;
using WSWD.WmallPos.POS.PY.VI;
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
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.PY.PT
{
    public class PYP010presenter : IPYP010presenter
    {
        private IPYP010View m_view;
        public PYP010presenter(IPYP010View view)
        {
            m_view = view;
        }

        #region IPresenter Members

        /// <summary>
        /// 쿠폰정보 조회
        /// </summary>
        public void GetSaleCoupon()
        {
            var db = MasterDbHelper.InitInstance();
            DataSet ds = null;

            try
            {
                ds = db.ExecuteQuery(Extensions.LoadSqlCommand("POS_PY", "P012GetCouponBSM200T"),
                    new string[] { "@DD_SALE" },
                    new object[] { ConfigData.Current.AppConfig.PosInfo.SaleDate });
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
                m_view.SetSaleCoupon(ds);
            }
        }

        #endregion
    }
}
