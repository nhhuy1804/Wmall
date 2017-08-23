//-----------------------------------------------------------------
/*
 * 화면명   : EDP005presenter.cs
 * 화면설명 : 마스터 수신
 * 개발자   : 정광호
 * 개발일자 : 2015.04.20
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

using WSWD.WmallPos.POS.ED.PI;
using WSWD.WmallPos.POS.ED.VI;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using WSWD.WmallPos.FX.BO.Trans;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.POS.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.ED.PT
{
    public class EDP005presenter : IEDP005presenter
    {
        private IEDP005View m_view;
        public EDP005presenter(IEDP005View view)
        {
            m_view = view;
        }

        #region IPresenter Members

        
        #endregion
    }
}
