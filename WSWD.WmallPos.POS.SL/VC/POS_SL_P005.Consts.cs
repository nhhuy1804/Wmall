//-----------------------------------------------------------------
/*
 * 화면명   : POS_SL_P005.Consts.cs
 * 화면설명 : 공지사항
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
    partial class POS_SL_P005
    {
        static string GP11_COL1 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00086");
        static string GP11_COL2 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00087");
        static string GP11_COL3 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00088");
        static string GP11_COL4 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00089");
        static string GP11_COL5 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00090");
        static string GP11_COL6 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00091");
        static string GP11_COL7 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00092");
        static string GP11_COL8 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00093");
        static string GP11_COL9 = "미회수\n사유"; //WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00094");
        static string GP11_RTN_REASON = "미회수 사유:";

        static string GP12_COL1 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00086");
        static string GP12_COL2 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00095");
        static string GP12_COL3 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00096");
        static string GP12_COL4 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00097");
        static string GP12_COL5 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00098");
        static string GP12_COL6 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00099");
        static string GP12_COL7 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00100");
        static string GP12_COL8 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00062");

        static string ERR_MSG_1 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00101");
        static string ERR_MSG_2 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00102");
        static string ERR_MSG_3 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00103");
        static string ERR_MSG_4 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00104");
        static string ERR_MSG_5 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00105");

        static string ERR_MSG_NORTN_RSN_ENTER_INVALID = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("01380");

        static string MSG_INPUT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00106");

        static string MSG_CONFIRM_TKS_PRS_RET = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00409");
    }
}
