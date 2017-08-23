using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.FX.Shared.NetComm.Basket;

namespace WSWD.WmallPos.POS.SL.Data
{
    public class SAT900TData
    {
        public string NoBoru;
        public string DdTime;
        public long AmSale;
    }

    public class SAT900TItemData
    {
        public string NoBoru;
        public int SqBoru;
        public string NmItem;
        public int QtItem;
        public int AmItem;
        public string VcCont;
        public BasketItem Basket;
    }

    public enum SLP003HoldErrorState
    {
        NoError,
        NoBoruNotExists,
        InvalidScanNoBoru
    }
}
