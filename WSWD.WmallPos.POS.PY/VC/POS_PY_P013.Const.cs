//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P013.Consts.cs
 * 화면설명 : 현금IC결제
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
    partial class POS_PY_P013
    {
        static string MSG_VAN_REQ_COMM_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00043");
        static string MSG_VAN_REQ_COMM_ERROR_RETRY = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00117");

        static string LABEL_RETRY = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00045");
        static string LABEL_FORCE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00046");

        static string MSG_VAN_REQ_PROCESSING = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00180");

        static string MSG_ENTER_IC_CARD = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00181");

        static string MSG_ENTER_ORG_APPR_NO = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00112");
        static string MSG_ENTER_ORG_APPR_DATE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00113");

        static string TITLE_MANURTN = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00125");
        static string TITLE_AUTORTN = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00124");
    }
}
