//-----------------------------------------------------------------
/*
 * 화면명   : PYP011presenter.cs
 * 화면설명 : 결제할인
 * 개발자   : 정광호
 * 개발일자 : 2015.04.21
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

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

namespace WSWD.WmallPos.POS.PY.PT
{
    public class PYP011presenter : IPYP011presenter
    {
        private IPYP011View m_view;
        public PYP011presenter(IPYP011View view)
        {
            m_view = view;
        }

        #region IPresenter Members


        #endregion
    }
}
