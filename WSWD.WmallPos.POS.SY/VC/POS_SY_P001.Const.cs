//-----------------------------------------------------------------
/*
 * 화면명   : POS_SY_P001.Consts.cs
 * 화면설명 : 시스템설정_환경설정
 * 개발자   : 정광호
 * 개발일자 : 2015.06.15
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.SY.VC
{
    partial class POS_SY_P001
    {
        static string strMsg00 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00366");
        static string strMsg01 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00367");
        static string strMsg02 = "0:Active / 1:Passive";
        static string strMsg03 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00368");
        static string strMsg04 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00369");
        static string strMsg05 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00368");
        static string strMsg06 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00368");
        static string strMsg07 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00370");
    }
}
