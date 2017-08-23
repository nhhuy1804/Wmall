//-----------------------------------------------------------------
/*
 * 화면명   : IEDP004View.cs
 * 화면설명 : DATA 전송
 * 개발자   : 정광호
 * 개발일자 : 2015.04.15
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WSWD.WmallPos.POS.ED.VI
{
    public interface IEDP004View
    {
        /// <summary>
        /// TR전송, 결락전송 데이터 조회값
        /// </summary>
        /// <param name="ds"></param>
        void SetTRData(DataSet ds);

        string TRTransDate { get; }
    }
}