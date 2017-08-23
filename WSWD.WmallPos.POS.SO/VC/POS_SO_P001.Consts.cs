using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.POS.FX.Win.Forms;

namespace WSWD.WmallPos.POS.SO.VC
{
    public partial class POS_SO_P001
    {
        // IN_, QS_, ER_, WN_
        static string MSG_ENTER_ID = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00210");
        static string MSG_ENTER_PASS = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00211");

        static string MSG_ERR_NO_USER = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00212");
        static string MSG_ERR_NOT_ADMIN = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00213");
        static string MSG_ERR_PASS = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00209");
    }
}
