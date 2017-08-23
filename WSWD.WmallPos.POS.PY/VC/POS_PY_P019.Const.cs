//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P019.Consts.cs
 * 화면설명 : 상품교환권 - 수동반품
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
    partial class POS_PY_P019
    {
        static string strMsg01 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00197");
        static string ERR_MSG_OVER_AMT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00153");
        static string MSG_INPUT_AMT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00204");
    }
}
