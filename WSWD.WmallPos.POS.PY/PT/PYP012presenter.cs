//-----------------------------------------------------------------
/*
 * 화면명   : PYP012presenter.cs
 * 화면설명 : 자국 통화 결제 확인(DCC)
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
    public class PYP012presenter : IPYP012presenter
    {
        private IPYP012View m_view;
        public PYP012presenter(IPYP012View view)
        {
            m_view = view;
        }

        #region IPresenter Members

        public DataTable GetDccMsg(string strCD_HEAD)
        {
            DataSet ds = new DataSet();
            var masterdb = MasterDbHelper.InitInstance();
            try
            {
                ds = masterdb.ExecuteQuery(Extensions.LoadSqlCommand("POS_PY", "P012GetDccMsgSYM051T"),
                    new string[] { "@CD_HEAD" },
                    new object[] { strCD_HEAD });
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                masterdb.EndInstance();
            }

            return ds.Tables[0];
        }

        #endregion
    }
}
