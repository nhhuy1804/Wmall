using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.ED.PT
{
    /// <summary>
    /// EOD 포스정산 할때 확인작업
    /// </summary>
    public class EDEODValidator : IMenuClickValidator
    {
        static string MSG_HOLD_COUNT_EXISTS = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00306");

        #region IMenuClickValidator Members

        /// <summary>
        /// 보류건 있는지확인
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool ValidateMenuOnClick(out string errorMessage)
        {
            var db = TranDbHelper.InitInstance();
            errorMessage = string.Empty;
            int nCnt = 0;
            try
            {
                var cnt = db.ExecuteScalar(Extensions.LoadSqlCommand("POS_ED", "P003GetSAT900T"),
                    new string[] {
                        "@DD_SALE",
                        "@CD_STORE",
                        "@NO_POS",
                    },
                    new object[] {
                        ConfigData.Current.AppConfig.PosInfo.SaleDate,
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo
                    });

                nCnt = TypeHelper.ToInt32(cnt);
                if (nCnt > 0)
                {
                    errorMessage = string.Format(MSG_HOLD_COUNT_EXISTS, nCnt);
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

            return nCnt == 0;
        }

        #endregion
    }
}
