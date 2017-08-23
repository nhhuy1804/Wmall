using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using WSWD.WmallPos.POS.App.PI;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.POS.FX.Win.Data;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.Data;
using WSWD.WmallPos.FX.Shared.Utils;
using System.Diagnostics;
using WSWD.WmallPos.POS.App.VI;

namespace WSWD.WmallPos.POS.App.PT
{
    public class MainFormPresenter : IMainFormPresenter
    {
        private IMainView m_view;

        #region IMainFormPresenter Members

        public MainFormPresenter(IMainView view)
        {
            this.m_view = view;
        }

        /// <summary>
        /// POS closing
        /// </summary>
        public void Terminate()
        {
        }

        #endregion
    }
}
