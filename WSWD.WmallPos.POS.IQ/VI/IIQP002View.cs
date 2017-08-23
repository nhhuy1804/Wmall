//-----------------------------------------------------------------
/*
 * 화면명   : IIQP002View.cs
 * 화면설명 : 카드사별 매출 조회
 * 개발자   : 정광호
 * 개발일자 : 2015.04.01
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace WSWD.WmallPos.POS.IQ.VI
{
    public interface IIQP002View
    {
        void SetCardSaleResult(DataSet ds); //카드사별 매출조회
    }
}
