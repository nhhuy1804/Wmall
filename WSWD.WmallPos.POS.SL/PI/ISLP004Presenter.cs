//-----------------------------------------------------------------
/*
 * 화면명   : ISLP004presenter.cs
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

namespace WSWD.WmallPos.POS.SL.PI
{
    public interface ISLP004presenter
    {
        /// <summary>
        /// 공지사항 조회
        /// </summary>
        void GetNotice();

        /// <summary>
        /// 공지사항 확인 저장
        /// </summary>
        /// <param name="strDD_STRAT">공지시작일</param>
        /// <param name="iNO_SEQ">순번</param>
        /// <param name="strDD_END">공지종료일</param>
        bool SetNoticeSave(string strDD_STRAT, int iNO_SEQ, string strDD_END);
    }
}
