//-----------------------------------------------------------------
/*
 * 화면명   : PTP002presenter.cs
 * 화면설명 : 포인트 적립
 * 개발자   : 정광호
 * 개발일자 : 2015.04.08
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using WSWD.WmallPos.POS.PT.PI;
using WSWD.WmallPos.POS.PT.VI;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using WSWD.WmallPos.FX.BO.Trans;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.POS.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.PT.PT
{
    public class PTP002presenter : IPTP002presenter
    {
        private IPTP002View m_view;
        public PTP002presenter(IPTP002View view)
        {
            m_view = view;
        }

        #region IPresenter Members

        

        #endregion
    }
}
