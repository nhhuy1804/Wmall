using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using WSWD.WmallPos.FX.NetComm.Data.Types;

/*
 * 상품등록
 * 
*/
namespace WSWD.WmallPos.FX.NetComm.Data.Basket
{
    /// <summary>
    /// 상품등록 basket
    /// </summary>
    public class BasketItem : BasketBase
    {
        /// <summary>
        /// SourceCode 20
        /// </summary>
        [TypeGubun(TypeProperties.Text, 20)]
        public string SourceCode;
        /// <summary>
        /// 상품명 40
        /// </summary>
        [TypeGubun(TypeProperties.Text, 40)]
        public string NmItem;
        /// <summary>
        /// 내부상품코드 20
        /// </summary>
        [TypeGubun(TypeProperties.Text, 20)]
        public string InCdItem;
        /// <summary>
        /// 품번상품 품번코드
        /// </summary>
        [TypeGubun(TypeProperties.Text, 6)]
        public string CdClass;
        /// <summary>
        /// 품번상품코드 4
        /// </summary>
        [TypeGubun(TypeProperties.Text, 4)]
        public string CdItem;
        /// <summary>
        /// 품번상품 마진구분 2
        /// </summary>
        [TypeGubun(TypeProperties.Text, 2)]
        public string FgMargin;
        /// <summary>
        /// 분류코드 8
        /// </summary>
        [TypeGubun(TypeProperties.Text, 8)]
        public string CdType;
        /// <summary>
        /// 면과세구분 1
        /// </summary>
        [TypeGubun(TypeProperties.Text, 1)]
        public string FgTax;
        /// <summary>
        /// 등록건수 5
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string CntItem;
        /// <summary>
        /// 취소건수 5
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string CntCancel;
        /// <summary>
        /// 원가 10
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string UtCprc;
        /// <summary>
        /// 판매단가
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string UtSprc;
        /// <summary>
        /// 공병 공박스 코드 2
        /// </summary>
        [TypeGubun(TypeProperties.Text, 2)]
        public string CdBox;
        /// <summary>
        /// 공병.공박스 단가 10
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string PrcBox;
        /// <summary>
        /// 추가적립포인트 5
        /// </summary>
        [TypeGubun(TypeProperties.Number, 5)]
        public string AddPnt;
        /// <summary>
        /// 판매금액 10
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AmSale;
        /// <summary>
        /// 공병금액 10
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AmMiner;
        /// <summary>
        /// 상품할인율 3
        /// </summary>
        [TypeGubun(TypeProperties.Number, 3)]
        public string PcDisc;
        /// <summary>
        /// 상품할인금액 10
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AmDisc;
        /// <summary>
        /// 상품에누리율 3
        /// </summary>
        [TypeGubun(TypeProperties.Number, 3)]
        public string PcEnuri;
        /// <summary>
        /// 상품에누리금액 10
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AmtEnuri;
        /// <summary>
        /// 할인처리구분자 1
        /// 0:없음 / 1:% / 2:금액	
        /// </summary>
        [TypeGubun(TypeProperties.Text, 1)]
        public string FgDiscProc;
        /// <summary>
        /// 에누리처리구분자 1
        /// 0:없음 / 1:% / 2:금액	
        /// </summary>
        [TypeGubun(TypeProperties.Text, 1)]
        public string FgEnuriProc;
        /// <summary>
        /// 취소처리구분자 1
        /// 0:없음 / 1:직전  / 2:지정	
        /// </summary>
        [TypeGubun(TypeProperties.Text, 1)]
        public string FgCancProc;
        /// <summary>
        /// 판매처리구분자 1
        /// </summary>
        [TypeGubun(TypeProperties.Text, 1)]
        public string FgSaleProc;
        /// <summary>
        /// 포인트적립 계산 포함여부 1
        /// 0:포인트 계산에 포함 안됨 / 1:포함됨	
        /// </summary>
        [TypeGubun(TypeProperties.Text, 1)]
        public string FgIncPntAp;
        /// <summary>
        /// 상품별추가 적립포인트합계 10
        /// </summary>
        [TypeGubun(TypeProperties.Number, 10)]
        public string AmSumItemPnt;
        /// <summary>
        /// 에누리처리구분 2
        /// </summary>
        [TypeGubun(TypeProperties.Text, 2)]
        public string FgEnuri;
        /// <summary>
        /// 에누리적용코드 2
        /// </summary>
        [TypeGubun(TypeProperties.Text, 2)]
        public string CdEnuriAp;
        /// <summary>
        /// 업체코드 10
        /// </summary>
        [TypeGubun(TypeProperties.Text, 10)]
        public string CdVendor;
        /// <summary>
        /// 업체명 40
        /// </summary>
        [TypeGubun(TypeProperties.Text, 40)]
        public string NmVendor;
        /// <summary>
        /// 상품속성 1
        /// 0:PLU,1:Non-PLU,2:품번,3:외식,4:저장물	
        /// </summary>
        [TypeGubun(TypeProperties.Text, 1)]
        public string CdDp;
        /// <summary>
        /// 신가격적용여부 1
        /// </summary>
        [TypeGubun(TypeProperties.Text, 1)]
        public string FgNewPrcApp;
        /// <summary>
        /// 주방주문번호 4
        /// </summary>
        [TypeGubun(TypeProperties.Text, 4)]
        public string NoKitOrder;
    }
}
