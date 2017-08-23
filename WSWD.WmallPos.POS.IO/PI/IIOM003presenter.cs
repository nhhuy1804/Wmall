//-----------------------------------------------------------------
/*
 * 화면명   : IIOM003presenter.cs
 * 화면설명 : 마감입금 설정
 * 개발자   : 정광호
 * 개발일자 : 2015.03.27
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace WSWD.WmallPos.POS.IO.PI
{
    public interface IIOM003presenter
    {
        void GetTicketTitle();                              //타사 상품권명 조회
        void SetCloseAmt(DataSet ds);    //마감입금 등록   
    }
}
