using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;

namespace WSWD.WmallPos.POS.SL.VI
{
    public interface ISLM001TransView
    {
        void LoadItems(BasketItem[] holdItems);
    }
}
