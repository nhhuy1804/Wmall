//-----------------------------------------------------------------
/*
 * 화면명   : ISLP004View.cs
 * 화면설명 : 공지사항
 * 개발자   : 정광호
 * 개발일자 : 2015.04.27
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace WSWD.WmallPos.POS.SL.VI
{
    public interface ISLP004View
    {
        /// <summary>
        /// 공지사항 셋팅
        /// </summary>
        /// <param name="ds">table01 : 공지사항 메인, table02 : 공지사항 상세내역</param>
        void SetNotice(DataSet ds);
    }
}
