using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.FX.Win.Interfaces
{
    public interface IObserver<T> where T: EventArgs
    {
        void Update(Object sender, T e);
    }
}
