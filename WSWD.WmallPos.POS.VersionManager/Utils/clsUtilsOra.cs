using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;

namespace WSWD.WmallPos.POS.VersionManager.Utils
{
    public class clsUtilsOra
    {
        private static clsUtilsOra m_instance = new clsUtilsOra();
        public static clsUtilsOra Instance
        {
            get
            {
                return m_instance;
            }
        }

        /// <summary>
        /// DB연결정보
        /// </summary>
        public string _strCon = string.Empty;
        public string _db_ip = string.Empty;
        public string _db_port = string.Empty;
        public string _db_nm = string.Empty;
        public string _db_id = string.Empty;
        public string _db_pass = string.Empty;

        public DataSet GetOra(string strSql)
        {
            DataSet ds = new DataSet();

            try
            {
                //오라클 DB 접속정보 셋팅
                _strCon = string.Format("data source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1})))(CONNECT_DATA = (SRVR = DEDICATED)(SERVICE_NAME = {2})));User Id={3};Password={4};", 
                    _db_ip, _db_port, _db_nm, _db_id, _db_pass);
                Environment.SetEnvironmentVariable("NLS_LANG", "KOREAN_KOREA.KO16MSWIN949");

                using (OracleConnection conn = new OracleConnection(_strCon))
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = strSql;

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(ds);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
            }

            return ds;
        }
    }
}
