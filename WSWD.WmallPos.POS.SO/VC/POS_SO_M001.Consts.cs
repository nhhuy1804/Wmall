using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.SO.VC
{
    partial class POS_SO_M001
    {
        static string MSG_NONE = string.Empty;
        static string MSG_NO_USER = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00205");
        static string MSG_CHECK_CAS_INFO = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00206");
        static string MSG_ENTER_PASS = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00207");
        static string MSG_ENTER_ID = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00208");
        static string MSG_LOGIN_FAILED = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00209");

        static string MSG_CDP_WELCOME = "Welcome To W-MALL";
    }
}
