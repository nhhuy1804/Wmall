//-----------------------------------------------------------------
/*
 * 화면명   : IPYP012presenter.cs
 * 화면설명 : 자국 통화 결제 확인(DCC)
 * 개발자   : 정광호
 * 개발일자 : 2015.05.
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WSWD.WmallPos.POS.PY.PI
{
    public interface IPYP012presenter
    {
        /// <summary>
        /// DCC 메세지 조회
        /// </summary>
        /// <param name="strCD_HEAD">헤더</param>
        DataTable GetDccMsg(string strCD_HEAD);
    }
}
