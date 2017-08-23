//-----------------------------------------------------------------
/*
 * 화면명   : POS_ED_P001.Consts.cs
 * 화면설명 : 합계 점검 조회
 * 개발자   : 정광호
 * 개발일자 : 2015.04.10
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.SY.VC
{
    partial class POS_SY_P002
    {
        static string strMsg00 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00366");
        static string strMsg01 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00368");
        static string strMsg02 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00373");
        static string strMsg03 = "1 = com1, 2 = com2, 3 = com3, …, L = LPT";
        static string strMsg04 = "9600, 19200, 38400, 57600, 115200";
        static string strMsg05 = "8, 7";
        static string strMsg06 = "1, 2";
        static string strMsg07 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00374");
        static string strMsg08 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00375");
        static string strMsg09 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00376");
        static string strMsg10 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00377");
        static string strMsg11 = "0 = KSNET";
        static string strMsg12 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00378");
        static string strMsg13 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00379");
        static string strMsg14 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00380");
        static string strMsg15 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00381");
        static string strMsg16 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00370");
        static string strMsg17 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00382");

        const string MSG_POS_INTG_OK = "IC리더기({0}) 무결성 검증이 정상적으로 완료되었습니다.";
        const string MSG_POS_INTG_FAILED = "IC리더기({0}) 무결설 실패하여 WMPOS를 종료합니다.";
    }
}
