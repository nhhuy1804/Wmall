//-----------------------------------------------------------------
/*
 * 화면명   : IPYP001View.cs
 * 화면설명 : 신용카드결제
 * 개발자   : TCL
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
    public interface IPYP001View
    {
        void ShowProgressMessage(bool showProgress);
        void ShowErrorMessage(VANRequestErrorType errorType, string errorMessage, string errorCode, string viewTag);

        /// <summary>
        /// 여전법 변경 
        /// 
        /// PV01RespData > PV21RespData
        /// </summary>
        /// <param name="respData"></param>
        /// <param name="strSignData"></param>
        void OnReturnSuccess(PV21RespData respData, string strSignData);
    }
}