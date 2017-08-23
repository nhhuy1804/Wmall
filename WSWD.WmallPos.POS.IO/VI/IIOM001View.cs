//-----------------------------------------------------------------
/*
 * 화면명   : IIOM001View.cs
 * 화면설명 : 준비금 설정
 * 개발자   : 정광호
 * 개발일자 : 2015.03.20
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
    public interface IIOM001View
    {
        /// <summary>
        /// 이전 준비금 및 회차 셋팅
        /// </summary>
        /// <param name="ds">이전 준비금 및 회차 내역</param>
        void SetBeforeCash(DataSet ds);

        /// <summary>
        /// 준비금
        /// </summary>
        /// <param name="_bpHeader"></param>
        /// <param name="basketReserve"></param>
        void SetTran(BasketHeader _bpHeader, BasketReserve basketReserve);
    }
}
