using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.POS.FX.Win.Interfaces;

namespace WSWD.WmallPos.POS.FX.Win.Interfaces
{
    public interface IMainFormView : IBaseView
    {        
        void OnLoggedIn();
        void OnLoggedOut();
    }
}
