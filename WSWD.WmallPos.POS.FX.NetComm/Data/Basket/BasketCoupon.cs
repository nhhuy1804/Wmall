using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.FX.NetComm.Data.Types;

namespace WSWD.WmallPos.FX.NetComm.Data.Basket
{
    public class BasketCoupon : BasketPay
    {
        /// <summary>
        /// 10	쿠폰코드	S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 10)]
        public string CouponCd;
        /// <summary>
        /// 20	쿠폰명칭	S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 20)]
        public string CouponNm;
        /// <summary>
        /// 3	쿠폰수량	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 3)]
        public string CouponCnt;
    }
}
