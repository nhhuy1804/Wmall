using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.PY.Data
{
    public static class PYExtensions
    {
        static public Int64 ValidateMoney(this Int64 intValue)
        {
            return (Int64)Math.Ceiling((double)(intValue / 10) * 10);
        }
    }

    /// <summary>
    /// VAN결제 카드승인요청 오류결과메시지
    /// </summary>
    public enum VANRequestErrorType
    {
        None,
        /// <summary>
        /// 통신오류
        /// </summary>
        CommError,
        /// <summary>
        /// 정보없음
        /// </summary>
        NoInfoFound,
        /// <summary>
        /// 승인되지않을때
        /// </summary>
        SomeError,
    }

    public enum PayCardMode
    {
        /// <summary>
        /// 신용카드
        /// </summary>
        CreditCard,
        /// <summary>
        /// 은련카드
        /// </summary>
        ERCard,
        /// <summary>
        /// 전화승인
        /// </summary>
        TelManualCard,
        /// <summary>
        /// APP card
        /// </summary>
        AppCard,
        /// <summary>
        /// IC Card
        /// </summary>
        CashICCard
    }


    public enum PaymentState
    {
        /// <summary>
        /// 준비된상태
        /// </summary>
        Ready,
        /// <summary>
        /// 처리중
        /// </summary>
        Processing,
        /// <summary>
        /// 오류있는경우
        /// </summary>
        Errored,
        /// <summary>
        /// 결제완료
        /// </summary>
        PayCompleted
    }

}
