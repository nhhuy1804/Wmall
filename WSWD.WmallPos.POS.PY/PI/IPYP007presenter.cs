//-----------------------------------------------------------------
/*
 * 화면명   : IPYP007presenter.cs
 * 화면설명 : 타사상품권
 * 개발자   : 정광호
 * 개발일자 : 2015.05.
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WSWD.WmallPos.POS.PY.PI
{
    public interface IPYP007presenter
    {
        /// <summary>
        /// 타사 상품권 권종마스터 조회
        /// </summary>
        void GetTicket();
    }
}
