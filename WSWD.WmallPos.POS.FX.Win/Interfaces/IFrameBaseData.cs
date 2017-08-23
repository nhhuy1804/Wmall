using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.FX.Win.Interfaces
{
    public interface IFrameBaseData <T> where T: EventArgs 
    {
        void Attach(IObserver<T> observer);
        void Detach(IObserver<T> observer);
    }
}
