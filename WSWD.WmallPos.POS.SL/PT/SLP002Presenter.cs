//-----------------------------------------------------------------
/*
 * 화면명   : SLP002presenter.cs
 * 화면설명 : 가격조회
 * 개발자   : 정광호
 * 개발일자 : 2015.04.28
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

using WSWD.WmallPos.POS.SL.PI;
using WSWD.WmallPos.POS.SL.VI;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.POS.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.SL.PT
{
    public class SLP002presenter : ISLP002presenter
    {
        private ISLP002View m_view;
        public SLP002presenter(ISLP002View view)
        {
            m_view = view;
        }

        #region IPresenter Members

        /// <summary>
        /// 공통코드 조회
        /// </summary>
        public void GetCode()
        {
            var db = MasterDbHelper.InitInstance();
            DataSet ds = null;
            try
            {
                string strSql = string.Empty;

                ds = db.ExecuteQuery(Extensions.LoadSqlCommand("POS_SL", "P002GetCD"), null, null);
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
                //조회결과 셋팅
                m_view.SetCode(ds);
            }
        }

        /// <summary>
        /// 단품, 품번, 품목 조회
        /// </summary>
        /// <param name="strPQ">단품(PQ05), 품번(PQ06), 품목(PQ08) 구분</param>
        /// <param name="strValue">조회 코드</param>
        public void GetPQ(string strPQ, string strValue)
        {
            var db = MasterDbHelper.InitInstance();
            DataSet ds = null;
            try
            {
                string strSql = string.Empty;

                if (strPQ == "PQ05")
                {
                    ds = db.ExecuteQuery(Extensions.LoadSqlCommand("POS_SL", "P002GetCD_ITEMBSM079T"), new string[] { "@CD_ITEM" }, new object[] { strValue });
                }
                else if (strPQ == "PQ06")
                {
                    ds = db.ExecuteQuery(Extensions.LoadSqlCommand("POS_SL", "P002GetCD_CLASSBSM061T"), new string[] { "@CD_CLASS" }, new object[] { strValue });
                }
                else if (strPQ == "PQ08")
                {
                    ds = db.ExecuteQuery(Extensions.LoadSqlCommand("POS_SL", "P002GetCD_CTGYBSM100T"), new string[] { "@CD_CTGY" }, new object[] { strValue });    
                }
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
                //조회결과 셋팅
                m_view.SetPQ(strPQ, strValue, ds);
            }
        }

        #endregion
    }
}
