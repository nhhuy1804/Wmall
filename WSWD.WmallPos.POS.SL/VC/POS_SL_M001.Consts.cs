using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.SL.VC
{
    partial class POS_SL_M001
    {
        static string GUIDE_MSG_INTPUT_STARTED = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00007");
        static string GUIDE_MSG_PAYMENT_STARTED = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00008");
        static string GUIDE_MSG_INPUTING_CDITEM = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00009");
        static string GUIDE_MSG_INPUTING_FGCLASS = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00010");
        static string GUIDE_MSG_INPUTING_2NDBARCODE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00011");
        static string GUIDE_MSG_INPUTING_UTSPRC = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00012");

        static string GUIDE_MSG_INPUT_LENGTH_CHECK = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00013");
        static string GUIDE_MSG_INPUT_DATA_INVALID = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00014");
        static string GUIDE_MSG_INPUT_QTY_INVALID = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00015");
        static string GUIDE_MSG_INPUT_KEY_INVALID = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00016");

        static string GUIDE_MSG_INPUT_NUMBER_OVER = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00017");
        static string GUIDE_MSG_INPUT_CHANGE_PRICE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00396");
        static string GUIDE_MSG_INPUT_ITEMAMT_OVER = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00018");
        static string GUIDE_MSG_INPUT_TOTALAMT_OVER = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00019");
        static string GUIDE_MSG_ITEMCNT_OVER = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00020");

        static string GUIDE_MSG_PSTATE_INITIAL = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00021");
        static string GUIDE_MSG_PSTATE_INITIAL_AUTORTN = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00022");

        static string GUIDE_MSG_NETWORK_PROBLEM = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00023");
        static string GUIDE_MSG_NO_HOLD_TRXN = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00024");
        static string GUIDE_MSG_OVER_TENDER = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00025");

        static string GUIDE_MSG_ONLY_ONLINE_PAY = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00026");
        static string GUIDE_MSG_ONLY_OFFLINE_PAY = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00027");

        static string GUIDE_MSG_ONLY_ONLINE_ITEM = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00028");
        static string GUIDE_MSG_ONLY_OFFLINE_ITEM = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00029");

        static string GUIDE_MSG_PLEASE_WAIT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00030");

        static string GUIDE_MSG_CONFIRM_AUTORTN = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00031");
        static string GUIDE_MSG_TRXN_CANCELLED = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00032");
        static string GUIDE_MSG_RTN_TRXN = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00033");        
        static string GUIDE_MSG_TRXN_RTN_ALREADY = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00034");

        static string GUIDE_MSG_TRXN_NOTFOUND = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00035");
        static string GUIDE_MSG_NOT_SALE_TRXN = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00036");
        static string GUIDE_MSG_SALE_TRXN_SVR_CHK_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00037");

        static string GUIDE_MSG_CANT_SALE_OTHER = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00038");
        static string GUIDE_MSG_CANT_SALE_ITEM = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00039");

        static string ERR_MSG_NO_CD_CLASS = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00040");
        static string ERR_MSG_NO_CD_ITEM = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00041");
        static string ERR_MSG_NO_PRESET_ITEM = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00042");


        static string ERR_MSG_AUTORTN_VAN_COMM_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00043");
        static string ERR_MSG_AUTORTN_VAN_NOINFO = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00044");
        static string LABEL_RETRY = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00045");
        static string LABEL_FORCE_CONT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00046");
        static string LABEL_CLOSE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00190");

        static string AUTORTN_MSG_PROCESSING = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00047");
        static string AUTORTN_MSG_GEN_TR_PRINT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00048");
        
        static string AUTORTN_MSG_TKS_PRSNT_POP = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00049");
        static string AUTORTN_MSG_TKS_PRSNT_POP_CANC = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00050");
        static string AUTORTN_MSG_TKS_PRSNT_REFUND = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00397");

        static string GUIDE_MSG_AUTORTN_PROCESSING = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00051");
        static string GUIDE_MSG_AUTORTN_MSG_CANC_RTN_START = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("01388");

        /// <summary>
        /// Loc added 11/21
        /// </summary>
        static string GUIDE_MSG_CANT_PAY_2_COUPON = "ER_할인쿠폰은 중복 결제 불가.";

        static string TITLE_POINT_INQ = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00052");
        
        static string TITLE_SL_SALE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00053");
        static string TITLE_SL_RETURN_MANU = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00054");
        static string TITLE_SL_RETURN_AUTO = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00055");
        static string TITLE_SL_OTH_SALE = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00056");
        static string TITLE_SL_OTH_SALE_RETURN = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00057");

        static string LABEL_COL_NO = "NO";
        static string LABEL_COL_ITEM = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00058");
        static string LABEL_COL_QTY = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00059");
        static string LABEL_COL_UTSPRC = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00060");
        static string LABEL_COL_DPER = "%";
        static string LABEL_COL_DAMT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00061");
        static string LABEL_COL_IAMT = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00062");

        static string MSG_CDP_SALE_1 = "PRICE";
        static string MSG_CDP_SALE_TOTAL = "TOTAL";
        /// <summary>
        /// 받을돈
        /// </summary>
        static string MSG_CDP_SALE_RECEIVE = "RECEIVE";
        /// <summary>
        /// 잔액
        /// </summary>
        static string MSG_CDP_SALE_CHANGE = "CHANGE";

        static string MSG_CDP_CANCEL = "CANCEL";
        static string MSG_CDP_SUSPENSION = "SUSPENSION";
        static string MSG_CDP_TRANSACTION = "TRANSACTION !!!";
        static string MSG_DATE_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00810");
        static string GUIDE_MSG_ONLY_RETURN_SALE_ITEM = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00865");//WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("01378");
        static string GUIDE_MSG_2ND_BARCODE_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("01379");
    }
}
