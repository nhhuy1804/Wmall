//-----------------------------------------------------------------
/*
 * 화면명   : IEDP003presenter.cs
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

namespace WSWD.WmallPos.POS.ED.PI
{
    public interface IEDP003presenter
    {
        /// <summary>
        /// SAT900 보류건수 확인
        /// </summary>
        void GetWait();

        /// <summary>
        /// POS 정산 저장
        /// </summary>
        /// <param name="osiMsgBar01"></param>
        /// <param name="osiMsgBar02"></param>
        void SetTran(WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiMsgBar01, WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiMsgBar02);

        /// <summary>
        /// SAT011 업데이트 확인
        /// </summary>
        void GetTranConfirm();
    }
}
