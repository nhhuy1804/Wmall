using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.POS.SL.Data;

namespace WSWD.WmallPos.POS.SL.VI
{
    public interface ISLP003View
    {
        void BindHoldList(SAT900TData[] holdList);
        void BindHoldItems(SAT900TItemData[] itemList);
        void ReportError(SLP003HoldErrorState errorState);
    }
}
