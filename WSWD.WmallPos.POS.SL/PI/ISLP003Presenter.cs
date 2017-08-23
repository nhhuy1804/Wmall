using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using WSWD.WmallPos.POS.SL.Data;
using WSWD.WmallPos.FX.Shared;

namespace WSWD.WmallPos.POS.SL.PI
{
    public interface ISLP003Presenter
    {
        void LoadHoldList();
        void LoadHoldItems(string ddSale, string noBoru);
        bool ValidateNoBoru(string noBoru);
        bool CheckHoldTrxnExists(string noBoru);
        SAT900TItemData[] ReleaseHoldTrxn(string noBoru);
    }
}
