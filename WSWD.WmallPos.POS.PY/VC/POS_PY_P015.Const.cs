﻿//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P015.Consts.cs
 * 화면설명 : 현금영수증 취소
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
    partial class POS_PY_P015
    {
        static string MSG_CASH = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00191");
        static string MSG_ID_TYPE_IND = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00183");
        static string MSG_ID_TYPE_BIZ = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00184");

        static string LABEL_RETRY = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00045");
        static string LABEL_CLOSE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00190");
        static string LABEL_FORCE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00046");

        static string MSG_INPUT_CONFIRM_NO_SIGNPAD = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00185");

        static string MSG_INPUT_CONFIRM_NO = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00186");

        static string MSG_INPUT_OT_APPR_DATE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00192");
        static string MSG_INPUT_OT_APPR_DATE_INVALID = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00193");

        static string MSG_INPUT_OT_APPR_NO = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00112");
        static string MSG_INPUT_CANC_REASON = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00194");
        
        static string MSG_SIGNPAD_INIT_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00187");

        static string MSG_VAN_REQ_COMM_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00195");
        static string MSG_VAN_REQ_COMM_ERROR_RETRY = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00117");
        static string MSG_VAN_REQ_PROCESSING = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00196");

        static string MSG_COMM_ERROR_RETRY = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00117");
    }
}
