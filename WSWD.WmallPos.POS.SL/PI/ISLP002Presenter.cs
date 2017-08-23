//-----------------------------------------------------------------
/*
 * 화면명   : ISLP002presenter.cs
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

namespace WSWD.WmallPos.POS.SL.PI
{
    public interface ISLP002presenter
    {
        /// <summary>
        /// 공통코드 조회
        /// </summary>
        void GetCode();

        /// <summary>
        /// 단품, 품번, 품목 조회
        /// </summary>
        /// <param name="strPQ">단품(PQ05), 품번(PQ06), 품목(PQ08) 구분</param>
        /// <param name="strValue">조회 코드</param>
        void GetPQ(string strPQ, string strValue);
    }
}
