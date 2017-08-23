//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P018.Consts.cs
 * 화면설명 : 기타 결제 선택
 * 개발자   : 정광호
 * 개발일자 : 2015.05.
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.PY.VC
{
    partial class POS_PY_P018
    {
        static string MSG_SELECT_TASK = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00199");

        static string MSG_P008 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00200");
        static string MSG_P011 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00201");
        static string MSG_P016 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00202");
        static string MSG_P017 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00203");
    }
}
