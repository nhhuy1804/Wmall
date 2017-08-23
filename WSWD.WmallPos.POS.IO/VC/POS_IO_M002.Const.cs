//-----------------------------------------------------------------
/*
 * 화면명   : POS_IO_M002.Consts.cs
 * 화면설명 : 중간입금 설정
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
    partial class POS_IO_M002
    {
        static string strMsg01 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00251");
        static string strMsg02 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00252");
        static string strMsg03 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00253");
        static string strMsg05 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00259");
        static string strMsg06 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00302");

        static string strMsgErr_01 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00384");
        static string strMsgErr_02 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00385");
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
