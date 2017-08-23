//-----------------------------------------------------------------
/*
 * 화면명   : IPYP007View.cs
 * 화면설명 : 타사상품권
 * 개발자   : 정광호
 * 개발일자 : 2015.05.
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WSWD.WmallPos.POS.PY.VI
{
    public interface IPYP007View
    {
        /// <summary>
        /// 타사 상품권 권종 셋팅
        /// </summary>
        /// <param name="ds">타사 상품권 권종 내역</param>
        void SetTicket(DataSet ds);
    }
}