using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.FX.Shared
{
    public enum ButtonTypes
    {
        Type01,
        Type02,
        Type03,
        Type04,
    }

    public enum DeviceStatus
    {
        OpenError,
        InitError,
        Opened,
        Closed
    }

    public enum InputTextDataType
    {
        Text,
        Numeric,
        DateTime
    }
}
