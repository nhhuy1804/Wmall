//-----------------------------------------------------------------
/*
 * 화면명   : IEDP002presenter.cs
 * 화면설명 : 계산원 정산
 * 개발자   : 정광호
 * 개발일자 : 2015.04.08
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WSWD.WmallPos.POS.ED.PI
{
    public interface IEDP002presenter
    {
        /// <summary>
        /// 계산원 정산 저장
        /// </summary>
        /// <param name="osiMsgBar01"></param>
        /// <param name="osiMsgBar02"></param>
        void SetTran(WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiMsgBar01, WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiMsgBar02);  
    }
}
