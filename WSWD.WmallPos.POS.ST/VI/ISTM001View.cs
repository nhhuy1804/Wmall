using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.POS.ST.PT;

namespace WSWD.WmallPos.POS.ST.VI
{
    /// <summary>
    /// M001, 개시화면 뷰
    /// </summary>
    public interface ISTM001View
    {
        void OnValidateOpen(ValidateOpenStatus validateStatus);
    }
}
