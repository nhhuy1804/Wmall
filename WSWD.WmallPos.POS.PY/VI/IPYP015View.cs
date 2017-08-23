//-----------------------------------------------------------------
/*
 * 화면명   : IPYP015View.cs
 * 화면설명 : 현금영수증 취소
 * 개발자   : TCL
 * 개발일자 : 2015.05.28
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
    public interface IPYP015View
    {
        void ShowProgressMessage(bool showProgress);
        void ShowErrorMessage(VANRequestErrorType errorType, string errorMessage, string errorCode, string viewTag);
        void OnReturnSuccess(PV02RespData respData);
    }
}