using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.FX.NetComm.Data.Types;

namespace WSWD.WmallPos.FX.NetComm.Data.Request.PG
{
    public class PG02ReqDataSub : SerializeClassBase
    {
        [TypeGubun(TypeProperties.Text, 20)]
        public string GiftNo;
    }
}
