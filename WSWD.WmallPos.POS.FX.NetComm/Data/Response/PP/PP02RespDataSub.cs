using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.FX.NetComm.Data.Types;

namespace WSWD.WmallPos.FX.NetComm.Data.Response.PP
{
    public class PP02RespDataSub : SerializeClassBase
    {
        /// <summary>
        /// 회원카드번호
        /// </summary>
        [TypeGubun(TypeProperties.Text, 20)]
        public string CustCardNo;

        /// <summary>
        /// 회원명
        /// </summary>
        [TypeGubun(TypeProperties.Text, 50)]
        public string CustName;
    }
}
