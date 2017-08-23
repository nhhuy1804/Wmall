//-----------------------------------------------------------------
/*
 * 화면명   : ISTM002View.cs
 * 화면설명 : 메인 공지사항
 * 개발자   : 정광호
 * 개발일자 : 2015.04.30
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace WSWD.WmallPos.POS.ST.VI
{
    public interface ISTM002View
    {
        /// <summary>
        /// 메인 공지사항 셋팅
        /// </summary>
        /// <param name="ds">메인 공지사항 조회내역</param>
        void SetNotice(DataSet ds);

        /// <summary>
        /// Invoke ?
        /// </summary>
        bool InvokeRequired { get; }

        /// <summary>
        /// Invoke method
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IAsyncResult BeginInvoke(Delegate method);
    }
}
