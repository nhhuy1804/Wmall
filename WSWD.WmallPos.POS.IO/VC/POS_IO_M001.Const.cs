//-----------------------------------------------------------------
/*
 * 화면명   : POS_IO_M001.Consts.cs
 * 화면설명 : 준비금 설정
 * 개발자   : 정광호
 * 개발일자 : 2015.03.20
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.IO.VC
{
    partial class POS_IO_M001
    {
        static string strMsg01 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00250");
        static string strMsg02 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00069");
    }
}
