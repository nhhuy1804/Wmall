using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.ST.VC
{
    partial class POS_ST_M001
    {
        static string MSG_TIME_SYNC_START = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00218");
        static string MSG_TIME_SYNC_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00219");
        static string MSG_TIME_SYNC_COMPLETE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00220");
        static string MSG_TIME_SVR_CHECK_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00221");

        static string MSG_DEVICE_INIT_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00222");
        static string MSG_KEYBOARD_INIT_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00223");

        static string MSG_SCANNER_INITING = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00224");
        static string MSG_SCANNER_INIT_COMPLETE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00225");
        static string MSG_SCANNER_INIT_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00226");

        static string MSG_PRINTER_INITING = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00227");
        static string MSG_PRINTER_INIT_COMPLETE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00228");
        static string MSG_PRINTER_INIT_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00229");
        static string MSG_PRINTER_COVER_OPEN = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00230");

        static string MSG_MSR_INITING = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00231");
        static string MSG_MSR_INIT_COMPLETE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00232");

        static string MSG_CDP_INITING = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00233");
        static string MSG_CDP_INIT_COMPLETE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00234");
        static string MSG_CDP_INIT_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00235");

        static string MSG_CD_INITING = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00236");
        static string MSG_CD_INIT_COMPLETE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00236");
        static string MSG_CD_INIT_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00237");

        static string LBL_RETRY = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00045");
        static string LBL_FORCE_CONT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00046");
        static string LBL_TITLE_START = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00238");

        static string MSG_ED_CLOSE_ASK = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00239");
        static string MSG_OPENNED_CONT_ASK = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00240");

        static string MSG_GET_LAST_TRXN_NO = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00398");
        static string MSG_GET_LAST_TRXN_NO_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00399");
        static string LABEL_RETRY = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00045");
        static string LABEL_CLOSE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00190");

        static string ERR_MSG_STORE_NOT_EXISTS = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00400");
        static string ERR_MSG_POS_NOT_EXISTS = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00401");

        static string ERR_MSG_SVR_POS_TIME_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00402");
        static string ERR_MSG_SALE_DATE_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00403");

    }
}
