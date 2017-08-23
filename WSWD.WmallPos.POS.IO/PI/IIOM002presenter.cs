//-----------------------------------------------------------------
/*
 * 화면명   : IIOM002presenter.cs
 * 화면설명 : 중간입금 설정
 * 개발자   : 정광호
 * 개발일자 : 2015.03.24
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace WSWD.WmallPos.POS.IO.PI
{
    public interface IIOM002presenter
    {
        void GetTicketTitle();      //타사 상품권명 조회
        void GetMiddleDeposit();    //중간입금 금액 조회
        void SetMiddelAmt(string strInputGubun, string strMiddleNo, DataSet ds);   //중간입금(수정회차 포함) 등록   
    }
}
