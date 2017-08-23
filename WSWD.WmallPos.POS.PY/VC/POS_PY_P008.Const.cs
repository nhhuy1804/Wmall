﻿//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P008.Consts.cs
 * 화면설명 : 복지카드
 * 개발자   : 정광호
 * 개발일자 : 2015.04.21
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.PY.VC
{
    partial class POS_PY_P008
    {
        static string TITLE_CANCEL = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00125");
        static string MSG_INPUT_AMT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00152");
        static string ERR_MSG_OVER_AMT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00153");
    }
}
