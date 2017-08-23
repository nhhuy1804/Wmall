using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.FX.NetComm.Data.Basket;
using WSWD.WmallPos.FX.NetComm.Data.Types;

namespace WSWD.WmallPos.POS.FX.NetComm.Data.Basket
{
    public class BasketMiddleDeposit : BasketBase
    {
        /// <summary>
        /// 1	입력구분자	S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 1)]
        public string InputGubun;
        /// <summary>
        /// 2	중간입금차수	S
        /// </summary>
        [TypeGubun(TypeProperties.Number, 2)]
        public string MiddleDepositCnt;
        
        /// <summary>
        /// 5	현금 총건수	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string CashTotalCnt;
        /// <summary>
        /// 10	현금 총금액	S
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string CashTotalAmt;
        
        /// <summary>
        /// 5	상품교환권  총건수	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string TicketTotalCnt;
        /// <summary>
        /// 10	상품교환권  총금액	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string TicketTotalAmt;
        /// <summary>
        /// 5	자사상품권  총건수	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string OurCompanyTicketTotalCnt;
        /// <summary>
        /// 10	자사상품권  총금액	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string OurCompanyTicketTotalAmt;
        /// <summary>
        /// 5	타사상품권  총건수	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string OtherCompanyTicketTotalCnt;
        /// <summary>
        /// 10	타사상품권  총금액	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string OtherCompanyTicketTotalAmt;
        
        /// <summary>
        /// 5	수표 건수	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string WonCnt_1000000;
        /// <summary>
        /// 10	수표 금액	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string WonAmt_1000000;
        /// <summary>
        /// 5	오만원권 건수	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string WonCnt_50000;
        /// <summary>
        /// 10	오만원권 금액	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string WonAmt_50000;
        /// <summary>
        /// 5	만원권   건수	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string WonCnt_10000;
        /// <summary>
        /// 10	만원권   금액	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string WonAmt_10000;
        /// <summary>
        /// 5	오천원권 건수	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string WonCnt_5000;
        /// <summary>
        /// 10	오천원권 금액	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string WonAmt_5000;
        /// <summary>
        /// 5	천원권   건수	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string WonCnt_1000;
        /// <summary>
        /// 10	천원권   금액	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string WonAmt_1000;
        /// <summary>
        /// 5	오백원권 건수	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string WonCnt_500;
        /// <summary>
        /// 10	오백원권 금액	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string WonAmt_500;
        /// <summary>
        /// 5	백원권   건수	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string WonCnt_100;
        /// <summary>
        /// 10	백원권   금액	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string WonAmt_100;
        /// <summary>
        /// 5	오십원권 건수	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string WonCnt_50;
        /// <summary>
        /// 10	오십원권 금액	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string WonAmt_50;
        /// <summary>
        /// 5	십원권   건수	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string WonCnt_10;
        /// <summary>
        /// 10	십원권   금액	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string WonAmt_10;

        /// <summary>
        /// 5	상품교환권 건수	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string TicketCnt;
        /// <summary>
        /// 10	상품교환권 금액	N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string TicketAmt;

        /// <summary>
        /// 20	타사상품권 명               (01)	S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 20)]
        public string OtherCompanyTicketNm_01;
        /// <summary>
        /// 5	타사상품권 건수            (01)	    N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string OtherCompanyTicketCnt_01;
        /// <summary>
        /// 10	타사상품권 금액            (01) 	S
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string OtherCompanyTicketAmt_01;
        /// <summary>
        /// 20	타사상품권 명               (02)	S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 20)]
        public string OtherCompanyTicketNm_02;
        /// <summary>
        /// 5	타사상품권 건수            (02)	    N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string OtherCompanyTicketCnt_02;
        /// <summary>
        /// 10	타사상품권 금액            (02) 	S
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string OtherCompanyTicketAmt_02;
        /// <summary>
        /// 20	타사상품권 명               (03)	S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 20)]
        public string OtherCompanyTicketNm_03;
        /// <summary>
        /// 5	타사상품권 건수            (03)	    N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string OtherCompanyTicketCnt_03;
        /// <summary>
        /// 10	타사상품권 금액            (03) 	S
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string OtherCompanyTicketAmt_03;
        /// <summary>
        /// 20	타사상품권 명               (04)	S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 20)]
        public string OtherCompanyTicketNm_04;
        /// <summary>
        /// 5	타사상품권 건수            (04)	    N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string OtherCompanyTicketCnt_04;
        /// <summary>
        /// 10	타사상품권 금액            (04) 	S
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string OtherCompanyTicketAmt_04;
        /// <summary>
        /// 20	타사상품권 명               (05)	S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 20)]
        public string OtherCompanyTicketNm_05;
        /// <summary>
        /// 5	타사상품권 건수            (05)	    N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string OtherCompanyTicketCnt_05;
        /// <summary>
        /// 10	타사상품권 금액            (05) 	S
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string OtherCompanyTicketAmt_05;
        /// <summary>
        /// 20	타사상품권 명               (06)	S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 20)]
        public string OtherCompanyTicketNm_06;
        /// <summary>
        /// 5	타사상품권 건수            (06)	    N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string OtherCompanyTicketCnt_06;
        /// <summary>
        /// 10	타사상품권 금액            (06) 	S
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string OtherCompanyTicketAmt_06;
        /// <summary>
        /// 20	타사상품권 명               (07)	S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 20)]
        public string OtherCompanyTicketNm_07;
        /// <summary>
        /// 5	타사상품권 건수            (07)	    N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string OtherCompanyTicketCnt_07;
        /// <summary>
        /// 10	타사상품권 금액            (07) 	S
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string OtherCompanyTicketAmt_07;
        /// <summary>
        /// 20	타사상품권 명               (08)	S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 20)]
        public string OtherCompanyTicketNm_08;
        /// <summary>
        /// 5	타사상품권 건수            (08)	    N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string OtherCompanyTicketCnt_08;
        /// <summary>
        /// 10	타사상품권 금액            (08) 	S
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string OtherCompanyTicketAmt_08;
        /// <summary>
        /// 20	타사상품권 명               (09)	S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 20)]
        public string OtherCompanyTicketNm_09;
        /// <summary>
        /// 5	타사상품권 건수            (09)	    N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string OtherCompanyTicketCnt_09;
        /// <summary>
        /// 10	타사상품권 금액            (09) 	S
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string OtherCompanyTicketAmt_09;
        /// <summary>
        /// 20	타사상품권 명               (10)	S
        /// </summary>
        [TypeGubun(TypeProperties.Text, 20)]
        public string OtherCompanyTicketNm_10;
        /// <summary>
        /// 5	타사상품권 건수            (10)	    N
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string OtherCompanyTicketCnt_10;
        /// <summary>
        /// 10	타사상품권 금액            (10) 	S
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string OtherCompanyTicketAmt_10;
    }
}
