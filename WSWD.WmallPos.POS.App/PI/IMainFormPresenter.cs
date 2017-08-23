using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.POS.FX.Win.Interfaces;

namespace WSWD.WmallPos.POS.App.PI
{
    public interface IMainFormPresenter : IBasePresenter
    {
        /// <summary>
        /// On POS Terminate
        /// </summary>
        void Terminate();
    }
}
