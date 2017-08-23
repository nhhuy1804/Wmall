using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.FX.Win.Interfaces
{
    public interface IFocusableControl 
    {
        bool IsFocused { get; set; }
        int FocusedIndex { get; set; }
        int TabIndex { get; set; }

        bool Focusable { get; set; }
        bool Visible { get; set; }
        bool Enabled { get; set; }

        void SetFocus();
    }
}
