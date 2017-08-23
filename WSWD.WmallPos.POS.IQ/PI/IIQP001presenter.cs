//-----------------------------------------------------------------
/*
 * 화면명   : IIQP001presenter.cs
 * 화면설명 : 품번별 매출 조회
 * 개발자   : 정광호
 * 개발일자 : 2015.03.30
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace WSWD.WmallPos.POS.IQ.PI
{
    public interface IIQP001presenter
    {
        void GetItemSaleResult(); //품번별 매출조회
    }
}
