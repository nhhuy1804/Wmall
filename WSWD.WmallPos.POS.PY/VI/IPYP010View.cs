//-----------------------------------------------------------------
/*
 * 화면명   : IPYP000View.cs
 * 화면설명 : 할인쿠폰
 * 개발자   : 정광호
 * 개발일자 : 2015.04.21
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WSWD.WmallPos.POS.PY.VI
{
    public interface IPYP010View
    {
        /// <summary>
        /// 쿠폰조회 결과
        /// </summary>
        /// <param name="ds"></param>
        void SetSaleCoupon(DataSet ds);
    }
}