//-----------------------------------------------------------------
/*
 * 화면명   : SLP004presenter.cs
 * 화면설명 : 공지사항
 * 개발자   : 정광호
 * 개발일자 : 2015.04.27
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
using WSWD.WmallPos.FX.Shared.Data;
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.SL.PT
{
    public class SLP004presenter : ISLP004presenter
    {
        private ISLP004View m_view;
        public SLP004presenter(ISLP004View view)
        {
            m_view = view;
        }

        #region IPresenter Members

        /// <summary>
        /// 공지사항 조회
        /// </summary>
        public void GetNotice()
        {
            var db = MasterDbHelper.InitInstance();
            DataSet ds = null;
            try
            {
                ds = db.ExecuteQuery(Extensions.LoadSqlCommand("POS_SL", "P004GetNoticeBSM130T"),
                    new string[] { "@DD_SALE", "@ID_USER" },
                    new object[] { ConfigData.Current.AppConfig.PosInfo.SaleDate, ConfigData.Current.AppConfig.PosInfo.CasNo });
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
                //공지사항 셋팅
                m_view.SetNotice(ds);
            }
        }

        /// <summary>
        /// 공지사항 확인 저장
        /// </summary>
        /// <param name="strDD_STRAT">공지시작일</param>
        /// <param name="iNO_SEQ">순번</param>
        /// <param name="strDD_END">공지종료일</param>
        public bool SetNoticeSave(string strDD_STRAT, int iNO_SEQ, string strDD_END)
        {
            bool bReturn = true;

            var transdb = MasterDbHelper.InitInstance();
            var trans = transdb.BeginTransaction();

            try
            {
                transdb.ExecuteNonQuery(Extensions.LoadSqlCommand("POS_SL", "P004SetNoticeBSM131T"),
                    new string[] { "@DD_START", "@NO_SEQ", "@ID_USER", "@DD_END", "@DD_CONF" },
                    new object[] {
                        strDD_STRAT,
                        iNO_SEQ,
                        ConfigData.Current.AppConfig.PosInfo.CasNo,
                        strDD_END,
                        ConfigData.Current.AppConfig.PosInfo.SaleDate + string.Format("{0:HHmmss}", DateTime.Now)
                    }, trans);

                trans.Commit();

                // UPdate 공지사항상태
                UpdateNewNotice();
            }
            catch (Exception ex)
            {
                bReturn = false;
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                transdb.EndInstance();
            }

            return bReturn;
        }

        /// <summary>
        /// 공지사항 상태업데이트
        /// </summary>
        void UpdateNewNotice()
        {
            var db = MasterDbHelper.InitInstance();
            try
            {
                string query = Extensions.LoadSqlCommand("POS_ST", "M002GetNoticeYNBSM130T");
                var fgCnt = db.ExecuteScalar(query,
                    new string[] {
                        "@ID_USER",
                        "@DD_SALE"
                    }, new object[]{
                        ConfigData.Current.AppConfig.PosInfo.CasNo,
                        DateTime.Today.ToString("yyyyMMdd")
                    });

                FrameBaseData.Current.OnDataChanged(FrameBaseDataItem.HasNotice, Convert.ToInt32(fgCnt) > 0);
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

        #endregion
    }
}
