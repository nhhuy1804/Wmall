using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.POS.SL.Data;

namespace WSWD.WmallPos.POS.SL.VI
{
    public interface ISLM001TouchGroup
    {
        /// <summary>
        /// Groups binding and refresh selected group
        /// </summary>
        /// <param name="touchGroups"></param>
        void BindGroups(TouchGroupData[] touchGroups);
        event TouchEventHandler OnTouch;
    }
}
