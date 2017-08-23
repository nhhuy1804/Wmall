//-----------------------------------------------------------------
/*
 * 화면명   : IPYP014presenter.cs
 * 화면설명 : 현금영수증
 * 개발자   : 정광호
 * 개발일자 : 2015.04.
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PV;

namespace WSWD.WmallPos.POS.PY.PI
{
    public interface IPYP014presenter
    {
        /// <summary>
        /// 현금영수증 전문호출
        /// </summary>
        /// <param name="cashType"></param>
        /// <param name="isSwipe"></param>
        /// <param name="confirmNo"></param>
        /// <param name="cashAmt"></param>
        /// <param name="vatAmt"></param>
        /// <param name="addData"></param>
        void MakeCashRecptRequest(int cashType, bool isSwipe, string confirmNo, int cashAmt, int vatAmt,
            PV21ReqDataAdd addData);
    }
}
