using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.SL.PT
{
    partial class SLM001Presenter
    {
        static string LABEL_PAY_CARD = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00001");
        static string LABEL_CASHRCP_YES = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00002");
        static string LABEL_CASHRCP_NO = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00003");
        static string LABEL_TKS_FG = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00004");

        static string MSG_AUTORTN_APP_CARD_CANCELLED_ERROR = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00005");
        static string MSG_AUTORTN_POINT_SAVE_CANCEL_NO_INFO = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00006");

        static string GUIDE_MSG_TR_GENERATING = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00405");
        static string GUIDE_MSG_PRINTING = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00406");
        static string GUIDE_SAVING_BACKUP = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00407");
    }
}
