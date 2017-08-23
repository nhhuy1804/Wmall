//-----------------------------------------------------------------
/*
 * 화면명   : ISTM002presenter.cs
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

namespace WSWD.WmallPos.POS.ST.PI
{
    public interface ISTM002Presenter
    {
        /// <summary>
        /// 메인 공지사항 조회
        /// </summary>
        void GetNotice();

        /// <summary>
        /// 새로운 공지확인
        /// </summary>
        void CheckNewNotice();
    }
}
