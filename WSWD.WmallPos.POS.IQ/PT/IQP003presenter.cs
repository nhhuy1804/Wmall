//-----------------------------------------------------------------
/*
 * 화면명   : IQP003presenter.cs
 * 화면설명 : 수표 조회
 * 개발자   : 정광호
 * 개발일자 : 2015.04.03
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using WSWD.WmallPos.POS.IQ.PI;
using WSWD.WmallPos.POS.IQ.VI;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using WSWD.WmallPos.FX.BO.Trans;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.POS.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.IQ.PT
{
    public class IQP003presenter : IIQP003presenter
    {
        private IIQP003View m_view;
        public IQP003presenter(IIQP003View view)
        {
            m_view = view;
        }
    }
}
