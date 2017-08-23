//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P014.static s.cs
 * 화면설명 : 현금영수증
 * 개발자   : 정광호
 * 개발일자 : 2015.04.21
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.PY.VC
{
    partial class POS_PY_P014
    {
        static string MSG_CASH = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00091");
        static string MSG_ID_TYPE_IND = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00183");
        static string MSG_ID_TYPE_BIZ = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00184");

        static string MSG_INPUT_CONFIRM_NO_SIGNPAD = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00185");
        static string MSG_INPUT_CONFIRM_NO = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00186");
        static string MSG_SIGNPAD_INIT_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00187");

        static string MSG_VAN_REQ_COMM_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00043");
        static string MSG_VAN_REQ_PROCESSING = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00188");

        static string MSG_COMM_ERROR_RETRY = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00117");

        static string LABEL_SELF = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00189");
        static string LABEL_CLOSE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00190");

        static string LABEL_RETRY = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00045");
    }
}
