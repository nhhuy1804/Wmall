using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.FX.Win.Interfaces
{
    /// <summary>
    /// View for all child (FormBase, DialogBase, PopupBase...)
    /// </summary>
    public interface IChildFormView
    {
        /// <summary>
        /// Manage child forms
        /// </summary>
        IChildFormManager ChildManager { get; set; }
    }
}
