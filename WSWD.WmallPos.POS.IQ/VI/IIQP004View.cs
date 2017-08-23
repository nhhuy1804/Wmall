//-----------------------------------------------------------------
/*
 * 화면명   : IIQP004View.cs
 * 화면설명 : 영수증 조회
 * 개발자   : 정광호
 * 개발일자 : 2015.05.
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace WSWD.WmallPos.POS.IQ.VI
{
    public interface IIQP004View
    {
        /// <summary>
        /// 영수증 조회
        /// </summary>
        /// <param name="ds">영수증 조회 내역</param>
        void SetReceiptList(DataSet ds);
    }
}
