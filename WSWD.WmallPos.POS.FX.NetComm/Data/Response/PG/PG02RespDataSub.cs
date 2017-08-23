using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.FX.NetComm.Data.Types;

namespace WSWD.WmallPos.FX.NetComm.Data.Response.PG
{
    public class PG02RespDataSub : SerializeClassBase
    {
        /// <summary>
        /// 상품교환권 번호 20
        /// </summary>
        [TypeGubun(TypeProperties.Text, 20)]
        public string GiftNo;

        /// <summary>
        /// 상품교환권 번호 1 가회수 처리 여부
        /// </summary>
        [TypeGubun(TypeProperties.Text, 1)]
        public string ProcFg;
    }
}
