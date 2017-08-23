using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.SL.Data
{
    public static class SLExtensions
    {
        public const int CELL_INDEX_NO = 0;
        public const int CELL_INDEX_ITEM = 1;
        public const int CELL_INDEX_QTY = 2;
        public const int CELL_INDEX_PRICE = 3;
        public const int CELL_INDEX_DISCP = 4;
        public const int CELL_INDEX_DISCA = 5;
        public const int CELL_INDEX_AMT = 6;

        public const string CDDP_PLU = "0";
        public const string CDDP_NON_PLU = "1";
        public const string CDDP_PB = "2";
        public const string CDDP_OTHER = "4";

        public const int MAX_PRC = 99999999;
        public const int MAX_AMT = 99999999;
        public const int MAX_TOTAL_AMT = 999999999;
        public const int MAX_TOTAL_ITEM = 90;
        
        /// <summary>
        /// 반품완료상태
        /// </summary>
        public const string PAYMENT_DETAIL_AUTORTN_END = "~~";

        /// <summary>
        /// 사은품회수팝업
        /// </summary>
        public const string PAYMENT_DETAIL_AUTORTN_TKS_PRESENT = "^^";

        /// <summary>
        /// 자동반품시작하고 취소됨(닫기)
        /// </summary>
        public const string PAYMENT_DETAIL_AUTORTN_CANCEL_STARTED = ">>";

        static public string POSSLQuerySQL(this string queryId)
        {
            return Extensions.LoadSqlCommand("POS_SL", queryId);
        }

        static public string MoneyToText(this int value)
        {
            return value > 0 ? string.Format("{0:#,##0}", value) : string.Empty;
        }

        static public string MoneyToText(this long value)
        {
            return value.MoneyToText(false);
        }

        static public string MoneyToText(this long value, bool showZero)
        {
            if (showZero)
            {
                return value >= 0 ? string.Format("{0:#,##0}", value) : string.Empty;
            }
            else
            {
                return value > 0 ? string.Format("{0:#,##0}", value) : string.Empty;
            }
        }

        static public string GetPrivateConstField(this object obj, string fieldName)
        {
            var t = obj.GetType();
            var f = t.GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static |
                 System.Reflection.BindingFlags.FlattenHierarchy);
            if (f == null)
            {
                return string.Empty;
            }
            var ov = f.GetValue(obj);
            return ov == null ? string.Empty : (string)f.GetValue(obj);
        }

        static public int ValidateNumber(this string value, out bool valid)
        {
            Int64 intValue = TypeHelper.ToInt64(value);

            valid = true;
            if (intValue > MAX_PRC)
            {
                valid = false;
                return 0;
            }

            return Convert.ToInt32(intValue);
        }

        static public int ValidateMoney(this string value, int prc, out bool valid)
        {
            Int64 intValue = TypeHelper.ToInt64(value);

            valid = true;
            if (prc == 0)
            {
                if (intValue > MAX_PRC)
                {
                    valid = false;
                }
            }
            else if (prc == 1)
            {
                if (intValue > MAX_AMT)
                {
                    valid = false;
                }
            }
            else
            {
                if (intValue > MAX_TOTAL_AMT || "0".Equals(value))
                {
                    valid = false;
                }
            }

            if (!valid)
            {
                return 0;
            }

            // round to 10 won
            var n = (int)Math.Ceiling((double)(intValue / 10) * 10);            
            return n;
        }

        static public SaleItemType GetSaleItemType(PBItemData itemData)
        {
            if (string.IsNullOrEmpty(itemData.FgClass))
            {
                return SaleItemType.NoItem;
            }

            int nFgClass = TypeHelper.ToInt32(itemData.FgClass);
            if (nFgClass < 60 || nFgClass > 69)
            {
                return SaleItemType.NormalItem;
            }
            else
            {
                return SaleItemType.OnlineItem;
            }
        }

        static public SaleItemType GetSaleItemType(string fgClass)
        {
            if (string.IsNullOrEmpty(fgClass))
            {
                return SaleItemType.NoItem;
            }

            int nFgClass = TypeHelper.ToInt32(fgClass);
            if (nFgClass < 60 || nFgClass > 69)
            {
                return SaleItemType.NormalItem;
            }
            else
            {
                return SaleItemType.OnlineItem;
            }
        }

        static public SaleModes GetReverseMode(this SaleModes mode)
        {
            SaleModes saleMode = mode;
            switch (mode)
            {
                case SaleModes.Sale:
                    break;
                case SaleModes.ManuReturn:
                    saleMode = SaleModes.Sale;
                    break;
                case SaleModes.AutoReturn:
                    saleMode = SaleModes.Sale;
                    break;
                case SaleModes.OtherSale:
                    saleMode = SaleModes.Sale;
                    break;
                case SaleModes.OtherSaleReturn:
                    saleMode = SaleModes.Sale;
                    break;
                default:
                    break;
            }

            return saleMode;
        }

        static public string MaskData(this string value, MaskingDataType maskType)
        {
            switch (maskType)
            {
                case MaskingDataType.CardNo:
                    return value.Length >= 8 ? 
                        (value.Substring(0, 4) + "-" + value.Substring(4, 4) + "****-****") : string.Empty;
                case MaskingDataType.PersonalId:
                    return value.Substring(0, value.Length - 4) + "****";
                case MaskingDataType.CardExpMY:
                    return value.Length != 4 ? string.Empty : "**/**";
                default:
                    return value;
            }
        }
    }

    [Flags]
    public enum PBItemProperties
    {
        None = 0,
        CdClass = 1,
        CdItem = 2,
        FgClass = 4,
        CheckDigit = 8,
        NmClass = 16,
        NmItem = 32,
        Qty = 64,
        Price = 128,
        DiscPerc = 256,
        DiscAmt = 512,
        FgCanc = 1024,
        All = CdClass | CdItem | FgClass | CheckDigit | NmClass | NmItem | Qty | Price | DiscPerc | DiscAmt
    }

    /// <summary>
    /// 판매상태 처리상태
    /// </summary>
    public enum SaleProcessState
    {
        /// <summary>
        /// 상품입력대기
        /// </summary>
        InputStarted,
        /// <summary>
        /// 상품입력중
        /// </summary>
        ItemInputing,        
        /// <summary>
        /// 소계
        /// </summary>
        SubTotal,
        /// <summary>
        /// 자동반품준비된상태. 확정하념됨
        /// </summary>
        AutoRtnReady,
        /// <summary>
        /// 자동반품처리중
        /// </summary>
        AutoRtnProcessing,
        /// <summary>
        /// 결제중
        /// </summary>
        Payment,
        /// <summary>
        /// 완료
        /// </summary>
        Completed
    }

    /// <summary>
    /// 상품입력 단계
    /// </summary>
    public enum ItemInputState
    {
        None,
        /// <summary>
        /// 상품입력준비된상태
        /// </summary>
        Ready,
        /// <summary>
        /// 처리중, socket
        /// </summary>
        Processing,
        /// <summary>
        /// 품번입력됨
        /// </summary>
        ItemPumNoEntered,
        /// <summary>
        /// 품목코드입력된상태
        /// </summary>
        ItemCodeEntered,
        /// <summary>
        /// 상품구분입력된상태
        /// 금액대기중
        /// </summary>
        ItemGubunEntered
    }

    /// <summary>
    /// 상품입력방법
    /// </summary>
    public enum ItemInputOperation
    {
        /// <summary>
        /// 초기상태
        /// </summary>
        None,
        /// <summary>
        /// 메뉴얼
        /// 품번수동입력
        ///     - KeyInput Sequence: 6,4,2,8
        ///     - 품번 PARSE TYPE: MANUAL
        /// </summary>
        ManualEnter,
        /// <summary>
        /// PLU Key
        /// 1) 단품검색
        /// </summary>
        PLU,
        /// <summary>
        /// Barcode scan
        /// 1) 2단
        ///     - 품번PARSE TYPE = TwoStepBarCode
        /// 2) 단품
        /// </summary>
        Scan,
        /// <summary>
        /// Preset key
        /// 1) PRESET 검색
        /// 2) SCAN - 단품
        /// </summary>
        Preset,
        /// <summary>
        /// Touch item
        /// 1) 품번이면
        ///     - 품번 PARSE TYPE: FULLBARCODE
        /// </summary>
        Touch
    }

    /// <summary>
    /// 품번PARSE TYPE
    /// </summary>
    public enum PBItemParseType
    {
        /// <summary>
        /// 수동입력 6,4,2
        /// </summary>
        Manual,

        /// <summary>
        /// 전체PB품번
        /// </summary>
        FullCode,

        /// <summary>
        /// 2단바코드
        /// </summary>
        TwoStep
    }

    /// <summary>
    /// 푼번PARSE STEP
    /// </summary>
    public enum PBItemParseStep
    {
        /// <summary>
        /// 초기상태
        /// </summary>
        Empty,
        /// <summary>
        /// 품번코드입력 된 상태
        /// </summary>
        CdClass,
        /// <summary>
        /// 상품코드 입력 된 상태
        /// </summary>
        CdItem,
        /// <summary>
        /// 상품구분 입력 된 상태
        /// </summary>
        FgClass,
        /// <summary>
        /// 상품단가 입력 된 상태
        /// </summary>
        UtSprc
    }

    /// <summary>
    /// 상품종류
    /// </summary>
    public enum CdDpTypes
    {
        /// <summary>
        /// 단품
        /// </summary>
        PLU,

        /// <summary>
        /// 품번
        /// </summary>
        PB
    }

    /// <summary>
    /// 오류시 어떤오류인지 표시
    /// Error Input Status
    /// </summary>
    public enum InvalidDataInputState
    {
        None,
        /// <summary>
        /// 기다리라, 처리중
        /// </summary>
        Waiting,
        /// <summary>
        /// 자동반품확정 알림
        /// </summary>
        ConfirmAutoRtn,
        /// <summary>
        /// 취소거래
        /// </summary>
        CancelledTrxn,
        /// <summary>
        /// 반품거래
        /// </summary>
        ReturnedTrxn,
        /// <summary>
        /// 거래없음
        /// </summary>
        AutoRtnNoTrans,
        /// <summary>
        /// 판매거래아님
        /// </summary>
        NotSaleTrans,
        /// <summary>
        /// 정상거래인 반품된거래
        /// </summary>
        TransReturned,
        /// <summary>
        /// 서버에서 매출정보확인실패
        /// </summary>
        TrxnSvrCheckError,
        /// <summary>
        /// 상품갯수초기화
        /// </summary>
        OverItemCount,
        /// <summary>
        /// 길확인
        /// </summary>
        LengthError,
        /// <summary>
        /// 판매중인 상품만 반품가능함
        /// </summary>
        ReturnOnlySaleItemError,
        /// <summary>
        /// 수량이상함
        /// </summary>
        InvalidQty,
        InvalidData,
        InvalidKey,
        /// <summary>
        /// 수량,단가너무큼
        /// </summary>
        NumberOverflow,
        /// <summary>
        /// 변경할가격입력
        /// </summary>
        InputChangePrice,
        /// <summary>
        /// 상품금액너무큼
        /// </summary>
        ItemAmtOverflow,
        /// <summary>
        /// 합계금액 너무큼
        /// </summary>
        TotalAmtOverflow,
        /// <summary>
        /// 통신오류
        /// </summary>
        NetworkError,
        /// <summary>
        /// 보류거래없음
        /// </summary>
        NoHoldTrxn,
        /// <summary>
        /// OVER TENDER
        /// </summary>
        OverPayAmount,
        /// <summary>
        /// 온라인매출만
        /// </summary>
        OnlyOnlinePay,
        /// <summary>
        /// 온라인매출결제외만
        /// </summary>
        OnlyOfflinePay,
        /// <summary>
        /// 온라인상품만
        /// </summary>
        OnlyOnlineItem,
        /// <summary>
        /// 일반상품만
        /// </summary>
        OnlyOfflineItem,
        /// <summary>
        /// 저장물판매불가
        /// </summary>
        CantSaleOther,
        /// <summary>
        /// 저장물판매시 일반상품 판매불가
        /// </summary>
        CantSaleItem,
        /// <summary>
        /// 2단 바코드를 입력 요청
        /// </summary>
        SecBarcodeError,
        /// <summary>
        /// 할인쿠폰 한번만 입력가능
        /// </summary>
        TwoCouponInputError
    }

    public enum SaleViewErrorMessage
    {
        None,
        NoCdClass,
        NoCdItem,
        NoPresetItem
    }

    /// <summary>
    /// 품번, 상품 스캔, 입력 시
    /// 확인 결과
    /// </summary>
    public enum PBItemParseResult
    {
        /// <summary>
        /// 잘못된 데이터
        /// </summary>
        InvalidData,
        /// <summary>
        /// 길이 오류
        /// </summary>
        InvalidLength,
        /// <summary>
        /// 수량단가 over
        /// </summary>
        NumberOverflow,
        /// <summary>
        /// 합계금액
        /// </summary>
        TotalAmountOver,
        /// <summary>
        /// 2단바코드
        /// </summary>
        SecBarCodeInput,
        /// <summary>
        /// 성공
        /// </summary>
        Success,
    }

    public enum SaleModes
    {
        Sale,
        ManuReturn,
        AutoReturn,
        OtherSale,
        OtherSaleReturn,
    }

    /// <summary>
    /// 판매중인 상품종류
    /// </summary>
    public enum SaleItemType
    {
        NoItem,
        NormalItem,
        OnlineItem
    }

    public enum PayDetailType
    {
        /// <summary>
        /// 온라인매출
        /// </summary>
        Online,
        /// <summary>
        /// 현금,카드,포인트등
        /// </summary>
        Offline
    }

    /// <summary>
    /// 판매모드 전환 시
    /// - 판매 > 자동반품, 수동반품
    /// 확인 하는 작업이 있어
    /// - 현재 입력 중이면 전환 못 함
    /// - 반품 모드로 전환 시 관리자 권한 확인 한 결과 (성공/실패)
    /// </summary>
    public enum ChangeSaleModeStatus
    {
        /// <summary>
        /// 성공
        /// </summary>
        Success,
        /// <summary>
        /// 작업조건 안 맞음
        /// </summary>
        InvalidCondition,
        /// <summary>
        /// 권한확인실패
        /// </summary>
        NoPermission
    }

    public enum MaskingDataType
    {
        CardNo,
        PersonalId,
        CardExpMY
    }

    /// <summary>
    /// 고객용표시기
    /// </summary>
    public enum CDPMessageType
    {
        /// <summary>
        /// 2.5 상품등록 후, 가격변경 후, 지정취소 후, 수량변경 후
        /// </summary>
        ItemChanged,
        /// <summary>
        /// 2.6 거래 중지 후
        /// </summary>
        TransCancel,
        /// <summary>
        /// 2.7 보류 등록 후
        /// </summary>
        TransHold,
        /// <summary>
        /// 2.8 보류 헤제 후
        /// </summary>
        TransHoldRelease,
        /// <summary>
        /// 2.9 소계 후
        /// </summary>
        SubTotal,
        /// <summary>
        /// 현금결제
        /// </summary>
        PayCash,
        /// <summary>
        /// 타사상품권
        /// </summary>
        PayGift,
        /// <summary>
        /// 카드결제: (신용, App카드, 신용IC, 은련,타건복지,티건카드)
        /// </summary>
        PayCard,
        /// <summary>
        /// 쿠폰
        /// </summary>
        PayCoupon,
        /// <summary>
        /// 포인트결제
        /// </summary>
        PayPoint,
        /// <summary>
        /// 온라인결제
        /// </summary>
        PayOnline,
        /// <summary>
        /// 기타
        /// </summary>
        PayOther
    }


    #region TOUCH GROUPS, ITEMS

    public delegate void TouchEventHandler(TouchEventArgs e);

    public class TouchEventArgs : EventArgs
    {
        public TouchTarget Target;
        public object ItemData;
    }

    public enum TouchTarget
    {
        Group,
        Item
    }

    #endregion
}
