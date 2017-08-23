//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P002.Consts.cs
 * 화면설명 : 전자서명
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
    partial class POS_PY_P002
    {
        static string MSG_SIGN_REQ_WAITING = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00128");
        static string MSG_SIGNPAD_INIT_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00129");
        static string MSG_NO_SIGN_CONFIRM_OK = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00130");
    }
}
