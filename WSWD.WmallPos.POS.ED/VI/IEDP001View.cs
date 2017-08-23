//-----------------------------------------------------------------
/*
 * 화면명   : IEDP001View.cs
 * 화면설명 : 합계 점검 조회
 * 개발자   : 정광호
 * 개발일자 : 2015.04.10
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace WSWD.WmallPos.POS.ED.VI
{
    public interface IEDP001View
    {
        void SetTotalChkResult(DataSet ds); //합계 점검 조회
    }
}