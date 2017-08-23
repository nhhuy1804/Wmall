//-----------------------------------------------------------------
/*
 * 화면명   : IQP004presenter.cs
 * 화면설명 : 영수증 조회
 * 개발자   : 정광호
 * 개발일자 : 2015.05.
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using WSWD.WmallPos.POS.IQ.PI;
using WSWD.WmallPos.POS.IQ.VI;
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

namespace WSWD.WmallPos.POS.IQ.PT
{
    public class IQP004presenter : IIQP004presenter
    {
        private IIQP004View m_view;
        public IQP004presenter(IIQP004View view)
        {
            m_view = view;
        }

        /// <summary>
        /// 영수증 조회
        /// </summary>
        /// <param name="strSaleDate">조회일자</param>
        /// <param name="strPosNo">포스번호</param>
        /// <param name="strTrxnNo">거래번호</param>
        public void GetReceipt(string strSaleDate, string strPosNo, string strTrxnNo)
        {
            DataSet dsTrans = null;
            var transdb = TranDbHelper.InitInstance();

            try
            {
                string strTemp = string.Empty;

                if (strTrxnNo.Length > 0)
                {
                    strTemp = strTrxnNo.PadLeft(6, '0');
                }

                dsTrans = transdb.ExecuteQuery(Extensions.LoadSqlCommand("POS_IQ", "P004GetReceiptSAT010T"),
                    new string[] { "@DD_SALE", "@NO_POS", "@NO_TRXN" },
                    new object[] { strSaleDate, strPosNo, strTemp });
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                transdb.EndInstance();
            }

            if (m_view != null)
            {
                // 영수증 조회 내역 셋팅
                m_view.SetReceiptList(dsTrans);
            }
        }


        /// <summary>
        /// 영수증 메세지 조회
        /// </summary>
        /// <param name="strStoreNo">점포코드</param>
        /// <param name="strCdClass">품번코드</param>
        public DataSet GetReceiptMsg(string strStoreNo, string strCdClass)
        {
            DataSet dsMaster = null;

            var masterdb = MasterDbHelper.InitInstance();

            try
            {
                string strSql = Extensions.LoadSqlCommand("POS_SL", "M001GetPrintTitleMsg");
                strSql += Extensions.LoadSqlCommand("POS_SL", "M001GetPrintMsg");
                strSql += Environment.NewLine + " ";
                strSql += Environment.NewLine + (WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00260"));
                strSql += Environment.NewLine + ("SELECT		NM_DESC");
                strSql += Environment.NewLine + (",			FG_SIZ");
                strSql += Environment.NewLine + ("FROM		BSM042T");
                strSql += Environment.NewLine + ("WHERE		CD_STORE	=	@CD_STORE");
                strSql += Environment.NewLine + (WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00261"));
                strSql += Environment.NewLine + ("AND			CD_CLASS	IN	(" + strCdClass + ")");
                strSql += Environment.NewLine + ("ORDER BY	CD_CLASS");
                strSql += Environment.NewLine + (",			SQ_LOC");
                strSql += Environment.NewLine + (";");

                dsMaster = masterdb.ExecuteQuery(strSql,
                    new string[] { "@CD_STORE" },
                    new object[] { strStoreNo });
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                masterdb.EndInstance();
            }

            //영수증 메세지 조회 내역 셋팅
            return dsMaster;
        }

        /// <summary>
        /// 프린트 DCC내용 전체조회
        /// </summary>
        /// <returns></returns>
        public DataTable GetPrintDccMsg()
        {
            DataSet ds = new DataSet();
            var masterdb = MasterDbHelper.InitInstance();
            try
            {
                ds = masterdb.ExecuteQuery(Extensions.LoadSqlCommand("POS_SL", "M001GetDccMsgSYM051T"), null, null);
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
    }
}
