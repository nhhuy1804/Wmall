//-----------------------------------------------------------------
/*
 * 화면명   : IPYP020View.cs
 * 화면설명 : 타사상품권 상품권종류 선택
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
    public interface IPYP020View
    {
        /// <summary>
        /// 상품권종류 내역 셋팅
        /// </summary>
        /// <param name="ds"></param>
        void SetType(DataSet ds);
    }
}