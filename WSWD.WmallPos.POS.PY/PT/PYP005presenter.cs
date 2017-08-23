//-----------------------------------------------------------------
/*
 * 화면명   : PYP005presenter.cs
 * 화면설명 : 카드사 선택
 * 개발자   : 정광호
 * 개발일자 : 2015.05.
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

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
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.PY.PT
{
    public class PYP005presenter : IPYP005presenter
    {
        private IPYP005View m_view;
        public PYP005presenter(IPYP005View view)
        {
            m_view = view;
        }

        #region IPresenter Members

        public void GetCardCompList()
        {
            List<string[]> list = new List<string[]>();
            var db = MasterDbHelper.InitInstance();
            try
            {
                string query = Extensions.LoadSqlCommand("POS_PY", "P005GetCardCompList");
                var ds = db.ExecuteQuery(query, null, null);
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    list.Add(new string[] {
                        item["CD_CARD"].ToString(),
                        item["NM_CARD"].ToString()
                    });
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

            m_view.BindCardCompList(list);
            list = null;
        }

        #endregion
    }
}
