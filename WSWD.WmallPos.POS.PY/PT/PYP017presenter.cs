//-----------------------------------------------------------------
/*
 * 화면명   : PYP017presenter.cs
 * 화면설명 : 타건카드
 * 개발자   : 정광호
 * 개발일자 : 2015.05.
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using WSWD.WmallPos.POS.PY.PI;
using WSWD.WmallPos.POS.PY.VI;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using WSWD.WmallPos.FX.BO.Trans;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.POS.FX.Shared.Utils;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.PY.PT
{
    public class PYP017presenter : IPYP017presenter
    {
        private IPYP017View m_view;
        public PYP017presenter(IPYP017View view)
        {
            m_view = view;
        }

        #region IPresenter Members


        #endregion
    }
}
