//-----------------------------------------------------------------
/*
 * 화면명   : IEDP003View.cs
 * 화면설명 : POS 정산
 * 개발자   : 정광호
 * 개발일자 : 2015.04.10
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
    public interface IEDP003View
    {
        /// <summary>
        /// 보류건 확인
        /// </summary>
        /// <param name="ds">보류건수</param>
        void SetWait(int iCnt);

        /// <summary>
        /// POS 정산 확인
        /// </summary>
        /// <param name="ds">출력데이터</param>
        void SetTranPrint(DataSet ds, BasketHeader _bpHeader);

        /// <summary>
        /// SAT011T 업데이트 확인
        /// </summary>
        /// <param name="iCnt">업데이트 실패 개수</param>
        void SetTranConfirm(int iCnt);
    }
}