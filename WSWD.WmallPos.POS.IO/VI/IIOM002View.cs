//-----------------------------------------------------------------
/*
 * 화면명   : IIOM002View.cs
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
using WSWD.WmallPos.FX.Shared.NetComm.Basket;

namespace WSWD.WmallPos.POS.IO.VI
{
    public interface IIOM002View
    {
        void SetMiddleDeposit(DataSet ds);      //중간입금 금액 셋팅
        void SetTicketTitle(DataSet ds);        //타사 상품권명 셋팅

        /// <summary>
        /// 중간입금
        /// </summary>
        /// <param name="_BasketHeader"></param>
        /// <param name="_BasketMiddleDeposit"></param>
        void SetTran(BasketHeader _BasketHeader, BasketMiddleDeposit _BasketMiddleDeposit);
    }
}
