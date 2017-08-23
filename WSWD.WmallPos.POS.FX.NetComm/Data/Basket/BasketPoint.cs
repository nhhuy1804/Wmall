using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.FX.NetComm.Data.Types;

namespace WSWD.WmallPos.FX.NetComm.Data.Basket
{
    public class BasketPoint : BasketPay
    {
        /// <summary>
        /// 20	카드번호	S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 20)]
        public string CardNo;
        /// <summary>
        /// 20	회원명	S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 20)]
        public string CustNm;
        /// <summary>
        /// 8	사용점수	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 8)]
        public string UsePoint;
        /// <summary>
        /// 10	사용후 가용점수	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string BalancePoint;
        /// <summary>
        /// 10	사용후 가용금액	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string BalanceAmt;
        /// <summary>
        /// 5	1점당현금환원기준금액	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string ExchangePoint;
        /// <summary>
        /// 10	승인번호	S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 10)]
        public string ApprovalNo;
        /// <summary>
        /// 20	회원번호	S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 20)]
        public string CustNo;
        /// <summary>
        /// 10	원거래 승인번호	S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 10)]
        public string DealApprovalNo;
    }
}
