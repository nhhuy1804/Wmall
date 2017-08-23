using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Data;

namespace WSWD.WmallPos.POS.FX.Win.Interfaces
{
    public interface IPostMessage<T> where T : OPOSKeyEventArgs
    {
        void Attach(IObserver<T> observer);
        void Detach(IObserver<T> observer);
    }
}
