//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P012.Consts.cs
 * 화면설명 : 자국통화 선택화면
 * 개발자   : TCL
 * 개발일자 : 2015.06.17
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.PY.VC
{
    partial class POS_PY_P012
    {
        static string MSG_GUIDE_ASK_SELECTION = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00175");
        static string MSG_SIGNPAD_RESP_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00176");
        static string MSG_SIGNPAD_INIT_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00177");

        static string MSG_SIGNPAD_CANCELLED = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00178");

        static string MSG_SIGNPAD_CURR_SELECTED = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00179");

    }
}
