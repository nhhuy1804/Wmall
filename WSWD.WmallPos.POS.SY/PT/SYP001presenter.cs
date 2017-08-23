//-----------------------------------------------------------------
/*
 * 화면명   : SYP001presenter.cs
 * 화면설명 : 시스템설정_환경설정
 * 개발자   : 정광호
 * 개발일자 : 2015.06.15
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using WSWD.WmallPos.POS.SY.PI;
using WSWD.WmallPos.POS.SY.VI;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using WSWD.WmallPos.FX.BO.Trans;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.POS.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.SY.PT
{
    public class SYP001presenter : ISYP001presenter
    {
        private ISYP001View m_view;
        public SYP001presenter(ISYP001View view)
        {
            m_view = view;
        }

        #region IPresenter Members



        #endregion
    }
}
