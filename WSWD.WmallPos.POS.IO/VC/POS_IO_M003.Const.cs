//-----------------------------------------------------------------
/*
 * 화면명   : POS_IO_M003.Consts.cs
 * 화면설명 : 마감입금 설정
 * 개발자   : 정광호
 * 개발일자 : 2015.03.24
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.IO.VC
{
    partial class POS_IO_M003
    {
        static string strMsg01 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00256");
        static string strMsg02 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00257");
        static string strMsg03 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00062");
        static string strMsg04 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00258");
        static string strMsg05 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00259");
        static string strMsg06 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00302");

        static string strMsgErr_01 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00388");
        static string strMsgErr = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00386");

        /// <summary>
        /// 현금
        /// </summary>
        private const string _cash = "cash";

        /// <summary>
        /// 상품권
        /// </summary>
        private const string _ticket = "ticket";
    }
}
