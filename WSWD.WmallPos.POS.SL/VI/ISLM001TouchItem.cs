using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.POS.SL.Data;

namespace WSWD.WmallPos.POS.SL.VI
{
    public interface ISLM001TouchItem
    {
        void BindItems(TouchItemData[] touchItems);
        event TouchEventHandler OnTouch;
    }
}
