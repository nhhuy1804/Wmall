//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P003.Consts.cs
 * 화면설명 : 신용카드결제(비밀번호입력)
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
    partial class POS_PY_P003
    {
        static string MSG_PASS_WAITING = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00131");
        static string MSG_PASS_SIGN_WAIT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00132");
        static string MSG_PASS_SIGN_THANKS = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00133");
        static string MSG_SIGNPAD_INIT_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00134");       
    }
}
