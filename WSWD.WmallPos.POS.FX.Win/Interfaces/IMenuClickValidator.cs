using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.FX.Win.Interfaces
{
    /// <summary>
    /// Validator for menu click event
    /// </summary>
    public interface IMenuClickValidator
    {
        bool ValidateMenuOnClick(out string errorMessage);
    }
}
