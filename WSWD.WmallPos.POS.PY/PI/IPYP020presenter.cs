﻿//-----------------------------------------------------------------
/*
 * 화면명   : IPYP020presenter.cs
 * 화면설명 : 타사상품권 상품권종류 선택
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
    public interface IPYP020presenter
    {
        /// <summary>
        /// 상품권 종류 조회
        /// </summary>
        void GetType();
    }
}
