//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P004.Consts.cs
 * 화면설명 : 신용카드결제(IC카드 정보 입력대기)
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
    partial class POS_PY_P004
    {
        static string MSG_ASK_IC_CARD_ENTER = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00135");
        static string MSG_ASK_IC_CARD_READER = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00136");

        static string MSG_WAIT_IC_CARD = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00137");
        static string MSG_WAIT_IC_CARD_CANCELLED = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00138");

        static string MSG_SIGNPAD_INIT_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00134");
        static string MSG_SIGNPAD_IC_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00139");
    }
}
