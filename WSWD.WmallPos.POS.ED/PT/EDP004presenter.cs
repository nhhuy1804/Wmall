//-----------------------------------------------------------------
/*
 * 화면명   : EDP004presenter.cs
 * 화면설명 : DATA 전송
 * 개발자   : 정광호
 * 개발일자 : 2015.04.15
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

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
    public class EDP004presenter : IEDP004presenter
    {
        private IEDP004View m_view;
        public EDP004presenter(IEDP004View view)
        {
            m_view = view;
        }

        #region IPresenter Members

        /// <summary>
        /// TR전송, 결락전송 데이터 조회
        /// </summary>
        /// <param name="strType">전송구분(0:TR전송, 1:결락전송)</param>
        public void GetTRData(string strType)
        {
            var db = TranDbHelper.InitInstance();
            DataSet ds = null;
            try
            {
                ds = db.ExecuteQuery(Extensions.LoadSqlCommand("POS_ED", strType == "0" ? "GetTRDataAll" : "GetTRDataLoss"),
                    new string[] { "@DD_SALE", "@CD_STORE", "@NO_POS" },
                    new object[] {
                        //ConfigData.Current.AppConfig.PosInfo.SaleDate,
                        m_view.TRTransDate,
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo,
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
                //TR전송, 결락전송 데이터 결과 셋팅
                m_view.SetTRData(ds);
            }
        }

        #endregion
    }
}
