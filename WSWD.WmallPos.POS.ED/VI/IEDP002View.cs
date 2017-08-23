//-----------------------------------------------------------------
/*
 * 화면명   : IEDP002View.cs
 * 화면설명 : 계산원 정산
 * 개발자   : 정광호
 * 개발일자 : 2015.04.08
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;

namespace WSWD.WmallPos.POS.ED.VI
{
    public interface IEDP002View
    {
        /// <summary>
        /// 계산원 정산 프린트 데이터
        /// </summary>
        /// <param name="ds"></param>
        void SetTranPrint(DataSet ds, BasketHeader _bpHeader);
    }
}