using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


using WSWD.WmallPos.POS.TM.PI;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.POS.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.TM.VI;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.TM.PT
{
    /// <summary>
    /// Test 업무에 대해 처리하는 Business Logic
    /// 
    /// </summary>
    public class TestPresenter : ITestPresenter
    {
        private static string QUERY_TEST = string.Empty;
        static TestPresenter()
        {
            QUERY_TEST = Extensions.LoadSqlCommand("TestPresenter", "TestCommand");
        }

        private IM001TestView m_view1;
        private IM001SummaryView m_view2;
        public TestPresenter(IM001TestView view1, IM001SummaryView view2)
        {
            m_view1 = view1;
            m_view2 = view2;
        }

        #region ITestPresenter Members

        /// <summary>
        /// Connect to db,
        /// get data from BSM043T
        /// Transaction 처리 한다
        /// </summary>
        /// <returns></returns>
        public void ProcessBSM043T()
        {
            // Master db 이용합니다.
            var db = MasterDbHelper.InitInstance();
            var trans = db.BeginTransaction();

            DataSet ds = null;
            try
            {
                ds = db.ExecuteQuery(QUERY_TEST,
                    new string[] {
                        "@CD_STORE",
                        "@NO_POS"
                    },
                    new object[] {
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo
                    }, trans);

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                db.EndInstance();
            }

            if (m_view1 != null)
            {
                m_view1.UpdateBSM043TData(ds);
            }

            if (m_view2 != null)
            {
                m_view2.UpdateTotalCount(ds.Tables[0].Rows.Count);
            }
        }

        #endregion
    }
}
