//-----------------------------------------------------------------
/*
 * 화면명   : IOM001presenter.cs
 * 화면설명 : 준비금 설정
 * 개발자   : 정광호
 * 개발일자 : 2015.03.20
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using WSWD.WmallPos.POS.IO.PI;
using WSWD.WmallPos.POS.IO.VI;
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
using WSWD.WmallPos.FX.Shared.NetComm;

namespace WSWD.WmallPos.POS.IO.PT
{
    public class IOM001presenter : IIOM001presenter
    {
        private IIOM001View m_view;
        public IOM001presenter(IIOM001View view)
        {
            m_view = view;
        }

        #region IPresenter Members

        /// <summary>
        /// 이전 준비금 및 회차 조회
        /// </summary>
        public void GetCash()
        {
            var transdb = TranDbHelper.InitInstance();
            DataSet ds = null;
            try
            {
                ds = transdb.ExecuteQuery(Extensions.LoadSqlCommand("POS_IO", "M001SelectSAT300T"),
                    new string[] {
                        "@DD_SALE",
                        "@CD_STORE",
                        "@NO_POS",
                        "@ID_ITEM"
                    },
                    new object[] {
                        ConfigData.Current.AppConfig.PosInfo.SaleDate,
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo,
                        NetCommConstants.ID_ITEM_D00
                    });
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
                m_view.SetBeforeCash(ds);
            }
        }

        /// <summary>
        /// 준비금 DB저장
        /// </summary>
        /// <param name="iPreReserveAmt">이전준비금</param>
        /// <param name="iReserveAmt">준비금</param>
        /// <param name="iReserveAmt">준비금회차</param>
        public void SetCash(Int32 iPreReserveAmt, Int32 iReserveAmt, Int32 iReserveNo)
        {
            #region Basket header

            BasketHeader header = new BasketHeader();
            header.TrxnType = NetCommConstants.TRXN_TYPE_PRE_IO;
            header.CancType = NetCommConstants.CANCEL_TYPE_NORMAL;

            #endregion

            #region Basket details

            BasketReserve basketReserve = new BasketReserve();
            basketReserve.BasketType = BasketTypes.BasketReserve;
            basketReserve.PayGrpCd = NetCommConstants.PAYMENT_GROUP_CASH;
            basketReserve.PayDtlCd = NetCommConstants.PAYMENT_DETAIL_CASH;
            basketReserve.ReserveAmt = iReserveAmt.ToString();
            basketReserve.PreReserveAmt = iPreReserveAmt.ToString();
            basketReserve.ReserveNo = iReserveNo.ToString();

            #endregion

            var transdb = TranDbHelper.InitInstance();
            var trans = transdb.BeginTransaction();

            try
            {
                TransManager.SaveTrans(header, new BasketBase[] { basketReserve }, transdb, trans);

                trans.Commit();

                // TR 완료시
                TransManager.OnTransComplete();
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtils.Instance.LogException(ex);
                header = null;
            }
            finally
            {
                transdb.EndInstance();
                trans.Dispose();
            }

            if (m_view != null)
            {
                m_view.SetTran(header, basketReserve);
            }
        }

        #endregion
    }
}
