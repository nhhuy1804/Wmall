using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.FX.Win.Interfaces
{
    public interface ITopBarDataView : IBaseView
    {
        bool ServerConnected { get; set; }
        bool HasNotice { get; set; }
        int SaleHoldCount { set; }
        int UploadedTransCount { get; set; }
        int TotalTransCount { get; set; }
        bool StateRefund { set; }
        void RefreshGlobalConfig();
    }
}
