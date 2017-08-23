//-----------------------------------------------------------------
/*
 * 화면명   : STM002presenter.cs
 * 화면설명 : 메인 공지사항
 * 개발자   : 정광호
 * 개발일자 : 2015.04.30
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

using WSWD.WmallPos.POS.ST.PI;
using WSWD.WmallPos.POS.ST.VI;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.POS.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.Data;

namespace WSWD.WmallPos.POS.ST.PT
{
    public class STM002Presenter : ISTM002Presenter
    {
        private ISTM002View m_view;
        public STM002Presenter(ISTM002View view)
        {
            m_view = view;
        }

        #region IPresenter Members

        /// <summary>
        /// 메인 공지사항 조회
        /// </summary>
        public void GetNotice()
        {
            var db = MasterDbHelper.InitInstance();
            DataSet ds = null;
            try
            {
                ds = db.ExecuteQuery(Extensions.LoadSqlCommand("POS_ST", "M002GetNoticeBSM130T"),
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
                //메인 공지사항 셋팅
                if (m_view.InvokeRequired)
                {
                    m_view.BeginInvoke((MethodInvoker)delegate()
                    {
                        m_view.SetNotice(ds);
                    });
                }
                else
                {
                    m_view.SetNotice(ds);
                }
            }
        }

        /// <summary>
        /// 새로운공지확인
        /// </summary>
        public void CheckNewNotice()
        {
            var db = MasterDbHelper.InitInstance();
            try
            {
                var fgCnt = db.ExecuteScalar(Extensions.LoadSqlCommand("POS_ST", "M002GetNoticeYNBSM130T"),
                    new string[] {
                        "@ID_USER",
                        "@DD_SALE"
                    }, new object[]{
                        ConfigData.Current.AppConfig.PosInfo.CasNo,
                        DateTime.Today.ToString("yyyyMMdd")
                    });

                FrameBaseData.Current.OnDataChanged(FrameBaseDataItem.HasNotice, TypeHelper.ToInt32(fgCnt) > 0);
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                db.EndInstance();
            }
        }

        #endregion
    }
}
