//-----------------------------------------------------------------
/*
 * 화면명   : BasketAccount.cs
 * 화면설명 : 정산데이터
 * 개발자   : 정광호
 * 개발일자 : 2015.04.09
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.FX.NetComm.Data.Basket;
using WSWD.WmallPos.FX.NetComm.Data.Types;

namespace WSWD.WmallPos.POS.FX.NetComm.Data.Basket
{
    public class BasketAccount : BasketBase
    {
        /// <summary>
        /// 3	정산항목코드(A00:고객수) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_A00;
        /// <summary>
        /// 10	금액(A00:고객수) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_A00;
        /// <summary>
        /// 4	건수(A00:고객수) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_A00;

        /// <summary>
        /// 3	정산항목코드(A01:정상 판매 합계) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_A01;
        /// <summary>
        /// 10	금액(A01:정상 판매 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_A01;
        /// <summary>
        /// 4	건수(A01:정상 판매 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_A01;

        /// <summary>
        /// 3	정산항목코드(A02:반품 판매 합계) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_A02;
        /// <summary>
        /// 10	금액(A02:반품 판매 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_A02;
        /// <summary>
        /// 4	건수(A02:반품 판매 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_A02;

        /// <summary>
        /// 3	정산항목코드(A03:할인 합계) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_A03;
        /// <summary>
        /// 10	금액(A03:할인 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_A03;
        /// <summary>
        /// 4	건수(A03:할인 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_A03;

        /// <summary>
        /// 3	정산항목코드(A04:에누리 합계) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_A04;
        /// <summary>
        /// 10	금액(A04:에누리 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_A04;
        /// <summary>
        /// 4	건수(A04:에누리 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_A04;

        /// <summary>
        /// 3	정산항목코드(A05:지정 정정 합계) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_A05;
        /// <summary>
        /// 10	금액(A05:지정 정정 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_A05;
        /// <summary>
        /// 4	건수(A05:지정 정정 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_A05;

        /// <summary>
        /// 3	정산항목코드(A06:거래 중지 합계) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_A06;
        /// <summary>
        /// 10	금액(A06:거래 중지 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_A06;
        /// <summary>
        /// 4	건수(A06:거래 중지 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_A06;

        /// <summary>
        /// 3	정산항목코드(B00:현금 정상 판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_B00;
        /// <summary>
        /// 10	금액(B00:현금 정상 판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_B00;
        /// <summary>
        /// 4	건수(B00:현금 정상 판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_B00;

        /// <summary>
        /// 3	정산항목코드(B01:현금 반품 판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_B01;
        /// <summary>
        /// 10	금액(B01:현금 반품 판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_B01;
        /// <summary>
        /// 4	건수(B01:현금 반품 판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_B01;

        /// <summary>
        /// 3	정산항목코드(C00:타사카드 정상 판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C00;
        /// <summary>
        /// 10	금액(C00:타사카드 정상 판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C00;
        /// <summary>
        /// 4	건수(C00:타사카드 정상 판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C00;

        /// <summary>
        /// 3	정산항목코드(C01:타사카드 반품 판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C01;
        /// <summary>
        /// 10	금액(C01:타사카드 반품 판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C01;
        /// <summary>
        /// 4	건수(C01:타사카드 반품 판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C01;

        /// <summary>
        /// 3	정산항목코드(C02:타건카드 정상판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C02;
        /// <summary>
        /// 10	금액(C02:타건카드 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C02;
        /// <summary>
        /// 4	건수(C02:타건카드 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C02;

        /// <summary>
        /// 3	정산항목코드(C03:타건카드 반품판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C03;
        /// <summary>
        /// 10	금액(C03:타건카드 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C03;
        /// <summary>
        /// 4	건수(C03:타건카드 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C03;

        /// <summary>
        /// 3	정산항목코드(C04:자사복지카드 정상판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C04;
        /// <summary>
        /// 10	금액(C04:자사복지카드 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C04;
        /// <summary>
        /// 4	건수(C04:자사복지카드 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C04;

        /// <summary>
        /// 3	정산항목코드(C05:자사복지카드 반품판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C05;
        /// <summary>
        /// 10	금액(C05:자사복지카드 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C05;
        /// <summary>
        /// 4	건수(C05:자사복지카드 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C05;

        /// <summary>
        /// 3	정산항목코드(C06:타건복지카드 정상판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C06;
        /// <summary>
        /// 10	금액(C06:타건복지카드 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C06;
        /// <summary>
        /// 4	건수(C06:타건복지카드 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C06;

        /// <summary>
        /// 3	정산항목코드(C07:타건복지카드 반품판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C07;
        /// <summary>
        /// 10	금액(C07:타건복지카드 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C07;
        /// <summary>
        /// 4	건수(C07:타건복지카드 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C07;

        /// <summary>
        /// 3	정산항목코드(C08:타사상품 정상판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C08;
        /// <summary>
        /// 10	금액(C08:타사상품 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C08;
        /// <summary>
        /// 4	건수(C08:타사상품 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C08;

        /// <summary>
        /// 3	정산항목코드(C09:타사상품 반품판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C09;
        /// <summary>
        /// 10	금액(C09:타사상품 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C09;
        /// <summary>
        /// 4	건수(C09:타사상품 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C09;

        /// <summary>
        /// 3	정산항목코드(C10:상품교환권 정상판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C10;
        /// <summary>
        /// 10	금액(C10:상품교환권 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C10;
        /// <summary>
        /// 4	건수(C10:상품교환권 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C10;

        /// <summary>
        /// 3	정산항목코드(C11:상품교환권 반품판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C11;
        /// <summary>
        /// 10	금액(C11:상품교환권 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C11;
        /// <summary>
        /// 4	건수(C11:상품교환권 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C11;

        /// <summary>
        /// 3	정산항목코드(C12:구상품교환권 정상판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C12;
        /// <summary>
        /// 10	금액(C12:구상품교환권 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C12;
        /// <summary>
        /// 4	건수(C12:구상품교환권 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C12;

        /// <summary>
        /// 3	정산항목코드(C13:구상품교환권 반품판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C13;
        /// <summary>
        /// 10	금액(C13:구상품교환권 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C13;
        /// <summary>
        /// 4	건수(C13:구상품교환권 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C13;

        /// <summary>
        /// 3	정산항목코드(C14:포인트결제 정상판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C14;
        /// <summary>
        /// 10	금액(C14:포인트결제 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C14;
        /// <summary>
        /// 4	건수(C14:포인트결제 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C14;

        /// <summary>
        /// 3	정산항목코드(C15:포인트결제 반품판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C15;
        /// <summary>
        /// 10	금액(C15:포인트결제 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C15;
        /// <summary>
        /// 4	건수(C15:포인트결제 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C15;

        /// <summary>
        /// 3	정산항목코드(C16:할인쿠폰 정상판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C16;
        /// <summary>
        /// 10	금액(C16:할인쿠폰 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C16;
        /// <summary>
        /// 4	건수(C16:할인쿠폰 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C16;

        /// <summary>
        /// 3	정산항목코드(C17:할인쿠폰 반품판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C17;
        /// <summary>
        /// 10	금액(C17:할인쿠폰 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C17;
        /// <summary>
        /// 4	건수(C17:할인쿠폰 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C17;

        /// <summary>
        /// 3	정산항목코드(C18:결제할인 정상판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C18;
        /// <summary>
        /// 10	금액(C18:결제할인 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C18;
        /// <summary>
        /// 4	건수(C18:결제할인 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C18;

        /// <summary>
        /// 3	정산항목코드(C19:결제할인 반품판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C19;
        /// <summary>
        /// 10	금액(C19:결제할인 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C19;
        /// <summary>
        /// 4	건수(C19:결제할인 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C19;

        /// <summary>
        /// 3	정산항목코드(C20:현금IC 정상판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C20;
        /// <summary>
        /// 10	금액(C20:현금IC 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C20;
        /// <summary>
        /// 4	건수(C20:현금IC 정상판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C20;

        /// <summary>
        /// 3	정산항목코드(C21:현금IC 반품판매) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C21;
        /// <summary>
        /// 10	금액(C21:현금IC 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C21;
        /// <summary>
        /// 4	건수(C21:현금IC 반품판매) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C21;

        /// <summary>
        /// 3	정산항목코드(C22:온라인매출(외상대)) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C22;
        /// <summary>
        /// 10	금액(C22:온라인매출(외상대)) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C22;
        /// <summary>
        /// 4	건수(C22:온라인매출(외상대)) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C22;

        /// <summary>
        /// 3	정산항목코드(C23:온라인매출(외상대)) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_C23;
        /// <summary>
        /// 10	금액(C23:온라인매출(외상대)) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_C23;
        /// <summary>
        /// 4	건수(C23:온라인매출(외상대)) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_C23;

        /// <summary>
        /// 3	정산항목코드(D00:준 비 금) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_D00;
        /// <summary>
        /// 10	금액(D00:준 비 금) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_D00;
        /// <summary>
        /// 4	건수(D00:준 비 금) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_D00;

        /// <summary>
        /// 3	정산항목코드(E00:중간입금 횟차 누적) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_E00;
        /// <summary>
        /// 10	금액(E00:중간입금 횟차 누적) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_E00;
        /// <summary>
        /// 4	건수(E00:중간입금 횟차 누적) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_E00;

        /// <summary>
        /// 3	정산항목코드(E01:현금 중간 입급 1회) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_E01;
        /// <summary>
        /// 10	금액(E01:현금 중간 입급 1회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_E01;
        /// <summary>
        /// 4	건수(E01:현금 중간 입급 1회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_E01;

        /// <summary>
        /// 3	정산항목코드(E02:현금 중간 입급 2회) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_E02;
        /// <summary>
        /// 10	금액(E02:현금 중간 입급 2회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_E02;
        /// <summary>
        /// 4	건수(E02:현금 중간 입급 2회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_E02;

        /// <summary>
        /// 3	정산항목코드(E03:현금 중간 입급 3회) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_E03;
        /// <summary>
        /// 10	금액(E03:현금 중간 입급 3회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_E03;
        /// <summary>
        /// 4	건수(E03:현금 중간 입급 3회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_E03;

        /// <summary>
        /// 3	정산항목코드(E04:현금 중간 입급 4회) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_E04;
        /// <summary>
        /// 10	금액(E04:현금 중간 입급 4회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_E04;
        /// <summary>
        /// 4	건수(E04:현금 중간 입급 4회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_E04;

        /// <summary>
        /// 3	정산항목코드(E05:현금 중간 입급 5회) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_E05;
        /// <summary>
        /// 10	금액(E05:현금 중간 입급 5회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_E05;
        /// <summary>
        /// 4	건수(E05:현금 중간 입급 5회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_E05;

        /// <summary>
        /// 3	정산항목코드(E10:현금 마감 입금) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_E10;
        /// <summary>
        /// 10	금액(E10:현금 마감 입금) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_E10;
        /// <summary>
        /// 4	건수(E10:현금 마감 입금) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_E10;

        /// <summary>
        /// 3	정산항목코드(E21:기타 중간 입금 1회) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_E21;
        /// <summary>
        /// 10	금액(E21:기타 중간 입금 1회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_E21;
        /// <summary>
        /// 4	건수(E21:기타 중간 입금 1회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_E21;

        /// <summary>
        /// 3	정산항목코드(E22:기타 중간 입금 2회) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_E22;
        /// <summary>
        /// 10	금액(E22:기타 중간 입금 2회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_E22;
        /// <summary>
        /// 4	건수(E22:기타 중간 입금 2회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_E22;

        /// <summary>
        /// 3	정산항목코드(E23:기타 중간 입금 3회) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_E23;
        /// <summary>
        /// 10	금액(E23:기타 중간 입금 3회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_E23;
        /// <summary>
        /// 4	건수(E23:기타 중간 입금 3회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_E23;

        /// <summary>
        /// 3	정산항목코드(E24:기타 중간 입금 4회) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_E24;
        /// <summary>
        /// 10	금액(E24:기타 중간 입금 4회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_E24;
        /// <summary>
        /// 4	건수(E24:기타 중간 입금 4회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_E24;

        /// <summary>
        /// 3	정산항목코드(E25:기타 중간 입금 5회) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_E25;
        /// <summary>
        /// 10	금액(E25:기타 중간 입금 5회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_E25;
        /// <summary>
        /// 4	건수(E25:기타 중간 입금 5회) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_E25;

        /// <summary>
        /// 3	정산항목코드(E30:기타 마감 입금 합계) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_E30;
        /// <summary>
        /// 10	금액(E30:기타 마감 입금 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_E30;
        /// <summary>
        /// 4	건수(E30:기타 마감 입금 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_E30;

        /// <summary>
        /// 3	정산항목코드(E40:타상환불) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_E40;
        /// <summary>
        /// 10	금액(E40:타상환불) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_E40;
        /// <summary>
        /// 4	건수(E40:타상환불) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_E40;

        /// <summary>
        /// 3	정산항목코드(F00:저장물 정상 판매 합계) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_F00;
        /// <summary>
        /// 10	금액(F00:저장물 정상 판매 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_F00;
        /// <summary>
        /// 4	건수(F00:저장물 정상 판매 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_F00;

        /// <summary>
        /// 3	정산항목코드(F01:저장물 반품 판매 합계) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_F01;
        /// <summary>
        /// 10	금액(F01:저장물 반품 판매 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_F01;
        /// <summary>
        /// 4	건수(F01:저장물 반품 판매 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_F01;

        /// <summary>
        /// 3	정산항목코드(F02:저장물 현금 판매 합계) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_F02;
        /// <summary>
        /// 10	금액(F02:저장물 현금 판매 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_F02;
        /// <summary>
        /// 4	건수(F02:저장물 현금 판매 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_F02;

        /// <summary>
        /// 3	정산항목코드(F03:저장물 현금 반품 합계) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_F03;
        /// <summary>
        /// 10	금액(F03:저장물 현금 반품 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_F03;
        /// <summary>
        /// 4	건수(F03:저장물 현금 반품 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_F03;

        /// <summary>
        /// 3	정산항목코드(F04:저장물 카드 판매 합계) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_F04;
        /// <summary>
        /// 10	금액(F04:저장물 카드 판매 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_F04;
        /// <summary>
        /// 4	건수(F04:저장물 카드 판매 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_F04;

        /// <summary>
        /// 3	정산항목코드(F05:저장물 카드 반품 합계) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_F05;
        /// <summary>
        /// 10	금액(F05:저장물 카드 반품 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_F05;
        /// <summary>
        /// 4	건수(F05:저장물 카드 반품 합계) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_F05;

        /// <summary>
        /// 3	정산항목코드(G00:자동반품시 사은품 반납 : 사은품 반납) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_G00;
        /// <summary>
        /// 10	금액(G00:자동반품시 사은품 반납 : 사은품 반납) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_G00;
        /// <summary>
        /// 4	건수(G00:자동반품시 사은품 반납 : 사은품 반납) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_G00;

        /// <summary>
        /// 3	정산항목코드(G01:자동반품시 사은품 반납 : 현금 변제) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_G01;
        /// <summary>
        /// 10	금액(G01:자동반품시 사은품 반납 : 현금 변제) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_G01;
        /// <summary>
        /// 4	건수(G01:자동반품시 사은품 반납 : 현금 변제) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_G01;

        /// <summary>
        /// 3	정산항목코드(G02:자동반품시 사은품 반납 : 상품교환권 반납) S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 3)]
        public string AccountCode_G02;
        /// <summary>
        /// 10	금액(G02:자동반품시 사은품 반납 : 상품교환권 반납) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AccountAmt_G02;
        /// <summary>
        /// 4	건수(G02:자동반품시 사은품 반납 : 상품교환권 반납) N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 4)]
        public string AccountCnt_G02;
    }
}
