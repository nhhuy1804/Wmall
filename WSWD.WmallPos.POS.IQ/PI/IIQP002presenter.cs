//-----------------------------------------------------------------
/*
 * 화면명   : IIQP002presenter.cs
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

namespace WSWD.WmallPos.POS.IQ.PI
{
    public interface IIQP002presenter
    {
        void GetCardSaleResult(); //카드사별 매출조회
    }
}
