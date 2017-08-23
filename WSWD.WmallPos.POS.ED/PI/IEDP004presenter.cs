//-----------------------------------------------------------------
/*
 * 화면명   : IEDP004presenter.cs
 * 화면설명 : DATA 전송
 * 개발자   : 정광호
 * 개발일자 : 2015.04.15
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WSWD.WmallPos.POS.ED.PI
{
    public interface IEDP004presenter
    {
        /// <summary>
        /// TR전송, 결락전송 데이터 조회
        /// </summary>
        /// <param name="strSQL">전송구분(0:TR전송, 1:결락전송)</param>
        void GetTRData(string strType);
    }
}
