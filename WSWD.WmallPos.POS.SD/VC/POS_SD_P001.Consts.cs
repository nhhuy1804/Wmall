using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.SD.VC
{
    partial class POS_SD_P001
    {
        static string ERR_MSG_SD_COMM_FAILED = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00242");
        static string ERR_MSG_OFFLINE_SD = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00243");
        static string ERR_MSG_CANT_SD = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00244");

        static string ERR_MSG_SD_OPENNED = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00245");
        static string ERR_MSG_SD_CANT_OPEN = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00246");

        static string MSG_SD_CHECK_FG = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00247");

        static string LBL_TITLE_SD_P001 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00248");
        static string LBL_BTN_RETRY = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00045");
        static string LBL_BTN_NEXT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00249");
        static string LBL_BTN_FORCE_OFFLINE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00046");

        static string MSG_CDP_OPEN = "***** O P E N *****";
    }
}
