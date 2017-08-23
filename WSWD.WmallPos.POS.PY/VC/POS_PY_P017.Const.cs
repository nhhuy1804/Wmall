//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P017.Consts.cs
 * 화면설명 : 타건카드
 * 개발자   : TCL
 * 개발일자 : 2015.05.25
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.PY.VC
{
    partial class POS_PY_P017
    {
        static string TITLE_CANCEL = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00125");
        static string MSG_INPUT_AMT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00152");
        static string ERR_MSG_OVER_AMT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00153");
    }
}
