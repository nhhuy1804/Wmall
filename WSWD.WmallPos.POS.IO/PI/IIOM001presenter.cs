//-----------------------------------------------------------------
/*
 * 화면명   : IIOM001presenter.cs
 * 화면설명 : 준비금 설정
 * 개발자   : 정광호
 * 개발일자 : 2015.03.20
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace WSWD.WmallPos.POS.IO.PI
{
    public interface IIOM001presenter
    {
        /// <summary>
        /// 이전준비금 조회
        /// </summary>
        void GetCash();

        /// <summary>
        /// 준비금 등록
        /// </summary>
        /// <param name="iPreReserveAmt">이전준비금</param>
        /// <param name="iReserveAmt">준비금</param>
        /// <param name="iReserveNo">준비금회차</param>
        void SetCash(Int32 iPreReserveAmt, Int32 iReserveAmt, Int32 iReserveNo);
    }
}
