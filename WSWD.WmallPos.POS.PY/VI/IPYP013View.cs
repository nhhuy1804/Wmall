﻿//-----------------------------------------------------------------
/*
 * 화면명   : IPYP013View.cs
 * 화면설명 : 현금IC결제
 * 개발자   : 정광호
 * 개발일자 : 2015.05.
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WSWD.WmallPos.POS.PY.Data;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PV;

namespace WSWD.WmallPos.POS.PY.VI
{
    public interface IPYP013View
    {
        void ShowProgressMessage(bool showProgress);
        void ShowErrorMessage(VANRequestErrorType errorType, string errorMessage, string errorCode);
        void OnReturnSuccess(PV04RespData respData);
    }
}