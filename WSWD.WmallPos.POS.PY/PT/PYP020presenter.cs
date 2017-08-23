//-----------------------------------------------------------------
/*
 * 화면명   : PYP020presenter.cs
 * 화면설명 : 상품권종류 선택
 * 개발자   : 정광호
 * 개발일자 : 2015.05.
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
    public class PYP020presenter : IPYP020presenter
    {
        private IPYP020View m_view;
        public PYP020presenter(IPYP020View view)
        {
            m_view = view;
        }

        #region IPresenter Members

        new public void GetType()
        {
            var masterdb = MasterDbHelper.InitInstance();
            DataSet ds = null;
            try
            {
                ds = masterdb.ExecuteQuery(Extensions.LoadSqlCommand("POS_PY", "P007GetTicketGFM050T"),
                    new string[] { "@DD_SALE" },
                    new object[] { ConfigData.Current.AppConfig.PosInfo.SaleDate });
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
                m_view.SetType(ds);
            }
        }

        #endregion
    }
}
