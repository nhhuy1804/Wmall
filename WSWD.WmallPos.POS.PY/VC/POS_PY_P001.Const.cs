//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P001.Consts.cs
 * 화면설명 : 신용카드결제
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
    partial class POS_PY_P001
    {
        const string EXP_YM_APP_CARD = "8911";
        static string MSG_ENTER_CARD_NO = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00107");
        static string MSG_ENTER_CARD_YM = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00108");
        static string MSG_ENTER_INSTALLMENT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00109");

        static string MSG_ENTER_APPR_NO_INPUT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00867");//WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("01389");
        // 자동반품시, SCAN ONLY
        static string MSG_SCAN_APP_CARD = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00110");
        static string MSG_ENTER_APPR_NO = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00111");
        static string MSG_ENTER_OT_APPR_NO = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00112");
        static string MSG_ENTER_OT_APPR_DATE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00113");

        static string MSG_CHECK_TEXT_LENGTH = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00114");

        static string MSG_CHECK_YM_INVALID = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00115");
        static string MSG_CHECK_YM_SMALL = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00116");
        static string MSG_INPUT_OT_APPR_DATE_INVALID = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00193");

        static string MSG_VAN_REQ_COMM_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00043");
        static string MSG_VAN_REQ_COMM_ERROR_RETRY = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00117");
        
        static string LABEL_RETRY = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00045");
        static string LABEL_FORCE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00046");

        /// <summary>
        /// 여전법 추가 0620
        /// </summary>
        static string LABEL_CLOSE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00190");

        static string MSG_VAN_REQ_PROCESSING_CRCARD = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00118");
        static string MSG_VAN_REQ_PROCESSING_ERCARD = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00119");
        static string MSG_VAN_REQ_PROCESSING_TLCARD = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00120");

        static string TITLE_CARD = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00121");
        static string TITLE_CARD_ER = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00122");
        static string TITLE_CARD_TEL = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00123");
        static string TITLE_AUTORTN = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00124");
        static string TITLE_MANURTN = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00125");

        static string MSG_TL_CARD_SEL = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00126");

        static string MSG_PAY_COMPLETED = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00127");

        /// <summary>
        /// 여전법 추가 0620
        /// </summary>
        static string MSG_CONFIRM_FORCE_CANC = "강제취소 하시겠습니까?";

    }
}
