//-----------------------------------------------------------------
/*
 * 화면명   : POS_SL_P007.Consts.cs
 * 화면설명 : 
 * 개발자   : TCL
 * 개발일자 : 2015.06.08
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.SL.VC
{
    partial class POS_SL_P007
    {
        static string GP11_COL1 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("01381");
        static string GP11_COL2 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00257");
        static string GP11_COL3 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("01382");
        static string GP11_COL4 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("01383");
        static string MSG_INPUT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("01384");
        static string MSG_EXIST_GIFT_EXCHNO = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("01385");
        static string MSG_USING_GIFT_EXCHNO = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("01386");

        static string MSG_OVER_TOTAL_RET_AMT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("01387");
    }
}
