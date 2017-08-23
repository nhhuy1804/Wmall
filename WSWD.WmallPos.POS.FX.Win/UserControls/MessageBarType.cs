using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public enum MessageBarType
    {
        /// <summary>
        /// 팝업화면에서 기본적으로 사용
        /// </summary>
        TypeStatus,

        /// <summary>
        /// 특정화면에 MessageBar 2개 있을때, 2번째 MessageBar일때 사용
        /// </summary>
        TypeMessage
    }
}
