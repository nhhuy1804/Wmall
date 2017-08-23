//-----------------------------------------------------------------
/*
 * 화면명   : ISLP002View.cs
 * 화면설명 : 가격조회
 * 개발자   : 정광호
 * 개발일자 : 2015.04.28
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace WSWD.WmallPos.POS.SL.VI
{
    public interface ISLP002View
    {
        /// <summary>
        /// 공통코드 조회내역 셋팅
        /// </summary>
        /// <param name="ds"></param>
        void SetCode(DataSet ds);

        /// <summary>
        /// 단품, 품번, 품목 조회내역 셋팅
        /// </summary>
        /// <param name="strPQ">단품(PQ05), 품번(PQ06), 품목(PQ08) 구분</param>
        /// <param name="strValue">조회 파라미터 값</param>
        /// <param name="ds">조회내역</param>
        void SetPQ(string strPQ, string strValue, DataSet ds);
    }
}
