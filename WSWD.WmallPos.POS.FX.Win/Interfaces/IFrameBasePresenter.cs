using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.FX.Win.Interfaces
{
    public interface IFrameBasePresenter : IBasePresenter
    {
        void Initialize(ITopBarDataView topBar);
    }
}
