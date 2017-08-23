using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.SO.Data
{
    public enum LoginMessageTypes
    {
        None,
        EnterUserId,
        LoginFailed,
        NoUserInfo,
        CheckCasInfo,
        EnterPassword
    }

}
