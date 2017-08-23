using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.Data;
using WSWD.WmallPos.POS.FX.Win.Interfaces;

namespace WSWD.WmallPos.POS.BO.VC
{
    /// <summary>
    /// 공통 기능키 처리
    /// 
    /// </summary>
    public static class MainFormExtensions
    {
        static public void ProcessFuncKey(this IChildFormManager form, OPOSKeyEventArgs e)
        {
            switch (e.Key.OPOSKey)
            {
                case OPOSMapKeys.KEY_SIGNOFF: // 싸인오프
                    e.IsHandled = true;
                    form.OnLoggedOut();
                    break;
                case OPOSMapKeys.KEY_EOD: // 정산
                    e.IsHandled = true;
                    form.ShowMenu("MNU_CLOSE");
                    break;
                case OPOSMapKeys.KEY_INOUT: // 입출금등록
                    e.IsHandled = true;
                    form.ShowMenu("MNU_IO");
                    break;
                case OPOSMapKeys.KEY_INQUIRY: // 점검조회
                    e.IsHandled = true;
                    form.ShowMenu("MNU_CHECK");
                    break;
                case OPOSMapKeys.KEY_INQCHK: // 수표조회
                    e.IsHandled = true;
                    form.ShowForm(string.Empty, "WSWD.WmallPos.POS.IQ.dll",
                        "WSWD.WmallPos.POS.IQ.VC.POS_IQ_P003");
                    break;
                case OPOSMapKeys.KEY_INQRECP: // 영수증조회
                    e.IsHandled = true;
                    form.ShowForm(string.Empty, "WSWD.WmallPos.POS.IQ.dll",
                        "WSWD.WmallPos.POS.IQ.VC.POS_IQ_P004");
                    break;
                case OPOSMapKeys.KEY_NOSALE: // 돈통열기
                    e.IsHandled = true;
                    try
                    {
                        POSDeviceManager.CashDrawer.OpenDrawer();
                    }
                    catch
                    {
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
