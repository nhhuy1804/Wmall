//-----------------------------------------------------------------
/*
 * 화면명   : IPYP005View.cs
 * 화면설명 : 카드사 선택
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
    public interface IPYP005View
    {
        void BindCardCompList(List<string[]> cardList);
    }
}