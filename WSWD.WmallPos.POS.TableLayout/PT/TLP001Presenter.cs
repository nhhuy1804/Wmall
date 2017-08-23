using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using WSWD.WmallPos.POS.TableLayout.PI;
using WSWD.WmallPos.POS.TableLayout.VI;
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

namespace WSWD.WmallPos.POS.TableLayout.PT
{
    public class TLP001Presenter : ITLP001Presenter
    {
        private ITLP001View m_view;
        public TLP001Presenter(ITLP001View view)
        {
            m_view = view;
        }

        public int MaxIndexTable(int floor)
        {
            DataSet dsMts = null;
            int max = 0;
            var mtsdb = MasterDbHelper.InitInstance();

            try
            {
                dsMts = mtsdb.ExecuteQuery(Extensions.LoadSqlCommand("CmdTable", "MaxIndexTable"),
                    new string[] { "@Floor" },
                    new object[] { floor });
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                mtsdb.EndInstance();
            }

            if (m_view != null)
            {
                if (dsMts != null && dsMts.Tables.Count > 0 && dsMts.Tables[0] != null && dsMts.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsMts.Tables[0].Rows[0];
                    try
                    {
                        max = Convert.ToInt32(dr["MAX([Index])"]);
                    }
                    catch
                    {

                    }
                    
                }
            }
            return max;
        }

        public void GetTable(int floor)
        {
            DataSet dsMts = null;
            var mtsdb = MasterDbHelper.InitInstance();

            try
            {
                dsMts = mtsdb.ExecuteQuery(Extensions.LoadSqlCommand("CmdTable", "GetTable"),
                    new string[] { "@Floor" },
                    new object[] { floor });
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                mtsdb.EndInstance();
            }

            if (m_view != null)
            {
                m_view.SetTableList(dsMts);
            }
        }

        public void InsertTable(int index, int x, int y, int floor)
        {
            var mtsdb = MasterDbHelper.InitInstance();
            var trans = mtsdb.BeginTransaction();

            try
            {
                string insQuery = Extensions.LoadSqlCommand("CmdTable", "InsertTable");
                mtsdb.ExecuteNonQuery(insQuery,
                    new string[] { "@Index", "@X", "@Y", "@Floor" }, new object[] { index, x, y, floor }, trans);
                trans.Commit();
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                trans.Dispose();
                mtsdb.Dispose();
            }
        }

        public void UpdateTable(int id, int x, int y)
        {
            var mtsdb = MasterDbHelper.InitInstance();
            var trans = mtsdb.BeginTransaction();

            try
            {
                string insQuery = Extensions.LoadSqlCommand("CmdTable", "UpdateTable");
                mtsdb.ExecuteNonQuery(insQuery,
                    new string[] { "@Id", "@X", "@Y" },
                    new object[] { id, x, y }, trans);
                trans.Commit();
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                trans.Dispose();
                mtsdb.Dispose();
            }
        }

        public void DeleteTable(int id)
        {
            var mtsdb = MasterDbHelper.InitInstance();
            var trans = mtsdb.BeginTransaction();

            try
            {
                string insQuery = Extensions.LoadSqlCommand("CmdTable", "DeleteTable");
                mtsdb.ExecuteNonQuery(insQuery, 
                    new string[] { "@id" }, 
                    new object[] { id }, trans);
                trans.Commit();
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                trans.Dispose();
                mtsdb.Dispose();
            }
        }
    }
}
