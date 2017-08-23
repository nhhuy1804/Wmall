using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.POS.SL.Data;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Forms;
using System.Windows.Forms;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PP;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PQ;
using System.Data;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PV;
using WSWD.WmallPos.POS.PY.Data;

namespace WSWD.WmallPos.POS.SL.VI
{
    public interface ISLM001SaleView : ISLM001BaseView
    {
        string InputText { get; set; }
        int InputLength { get; set; }

        /// <summary>
        /// 판매등록상태
        /// </summary>
        SaleProcessState ProcessState { get; set; }
        
        /// <summary>
        /// 상품입력상태
        /// </summary>
        ItemInputState InputState { get; set; }

        /// <summary>
        /// 입력행위
        /// </summary>
        ItemInputOperation InputOperation { get; set; }

        /// <summary>
        /// 반품/정상상태
        /// </summary>
        bool StateRefund { get; }

        /// <summary>
        /// Invoke ?
        /// </summary>
        bool InvokeRequired { get; }

        /// <summary>
        /// Invoke method
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IAsyncResult BeginInvoke(Delegate method);

        /// <summary>
        /// 판매모드
        /// </summary>
        SaleModes SaleMode { get; set; }

        /// <summary>
        /// 상품처리중인상태
        /// </summary>
        bool HasItems { get; }

        /// <summary>
        /// Key event처리
        /// </summary>
        /// <param name="e"></param>
        bool ProcessKeyEvent(OPOSKeyEventArgs e);

        /// <summary>
        /// 오류시 어떤오류인지 표시
        /// </summary>
        void ReportInvalidState(InvalidDataInputState invalidState);

        /// <summary>
        /// Show progress form or hide
        /// </summary>
        /// <param name="showProgress"></param>
        void ShowProgress(bool showProgress);

        /// <summary>
        /// Show CDP Message
        /// </summary>
        /// <param name="messageType"></param>
        void ShowCDPMessage(CDPMessageType messageType, string payAmt, string balAmt);

        /// <summary>
        /// 카드결제팝업
        /// 
        /// 여전볍 변경 05.27
        /// PV11ReqDataAdd > PV21ReqDataAdd 
        /// 
        /// 여전볍 변경 0620
        /// payCardMode 파람 추가
        /// </summary>
        /// <param name="inputAmt"></param>
        /// <param name="cancellable">취소가능한지?</param>
        /// <param name="cardPay">원거래 CARD Basket</param>
        /// <param name="returnData">Basket 결과</param>
        /// <param name="addData">전문 추가정보 added on 10.24</param>
        /// <param name="payCardMode">여전볍 추가 0620</param>
        /// <param name="errorCode">오류시 ERRORCODE</param>
        /// <param name="errorMessage">ERROR Message</param>
        /// <param name="taxAmt">세금액</param>
        /// <returns></returns>
        DialogResult ShowCardPopup(string inputAmt, string taxAmt, bool cancellable, BasketPayCard cardPay,
            PV21ReqDataAdd addData, PayCardMode payCardMode,
            out object returnData, out string errorCode, out string errorMessage);

        /// <summary>
        /// 현금영수증화면
        /// Loc added on 10.24
        /// 전문추가정보
        /// 
        /// 여전볍 변경 05.27
        /// PV11ReqDataAdd > PV21ReqDataAdd 
        /// </summary>
        /// <param name="cashAmt"></param>
        /// <param name="taxAmt"></param>
        /// <param name="addData"></param>
        /// <param name="returnData"></param>
        /// <returns></returns>
        DialogResult ShowCashReceiptPopup(int cashAmt, int taxAmt, PV21ReqDataAdd addData, out object returnData);

        /// <summary>
        /// 포인트적립화면
        /// </summary>
        /// <param name="cust">고객정보</param>
        /// <param name="BasketHeader">결제 헤더정보</param>
        /// <param name="BasketPays">결제 결제내역</param>
        /// <param name="BasketSubTtl">결제 소계정보</param>
        /// <param name="dicPromoPoint">프로모션정보</param>
        /// <param name="returnData"></param>
        /// <returns></returns>
        DialogResult ShowPointSavePopup(PP01RespData cust, BasketHeader BasketHeader,
            List<BasketPay> BasketPays, BasketSubTotal BasketSubTtl, BasketPointSave BasketPointSave, Dictionary<string, object> dicPromoPoint, out object returnData);

        /// <summary>
        /// 현금IC
        /// </summary>
        /// <param name="cashAmt">결제금액</param>
        /// <param name="taxAmt"></param>
        /// <param name="orgCashIC">원거래현금IC Basket</param>
        /// <param name="allowCancel">닫기버튼활성화여부</param>
        /// <param name="returnData"></param>
        /// <param name="errorCode">오류코드</param>
        /// <param name="errorMessage">오류메시지</param>
        /// <returns></returns>
        DialogResult ShowCashICPopup(int cashAmt, int taxAmt, BasketCashIC orgCashIC,
            bool allowCancel, out object returnData, out string errorCode, out string errorMessage);

        /// <summary>
        /// 기타결제팝업
        /// </summary>
        /// <param name="payAmt"></param>
        /// <param name="taxAmt"></param>
        /// <param name="returnData"></param>
        /// <returns></returns>
        DialogResult ShowOtherPayMethod(int payAmt, int taxAmt, out object returnData);

        /// <summary>
        /// 포인트조회
        /// </summary>
        void ShowCustPointPopup();

        /// <summary>
        /// 포인트사용팝업
        /// </summary>
        /// <param name="payAmt"></param>
        /// <param name="custInfo"></param>
        /// <param name="returnData"></param>
        DialogResult ShowPointUsePopup(int payAmt, PP01RespData custInfo, out object returnData);
      /// <summary>
        /// 상품교환권 팝업
        /// </summary>
        /// <param name="payAmt"></param>
        /// <param name="BasketPays"></param>
        /// <param name="iTranOverCnt"></param>
        /// <param name="returnData"></param>
        DialogResult ShowExchangePopup(int payAmt, List<BasketPay> BasketPays, int iTranOverCnt, out object returnData);

        /// <summary>
        /// 타사상품권 팝업
        /// </summary>
        /// <param name="payAmt"></param>
        /// <param name="BasketPays"></param>
        /// <param name="iTranOverCnt"></param>
        /// <param name="returnData"></param>
        DialogResult ShowOtherTicketPopup(int payAmt, List<BasketPay> BasketPays, int iTranOverCnt, bool bAuto, out object returnData);

        /// <summary>
        /// 할인쿠폰 팝업
        /// </summary>
        /// <param name="payAmt"></param>
        /// <param name="strType"></param>
        /// <param name="returnData"></param>
        DialogResult ShowCouponPopup(int payAmt, List<BasketItem> BasketItems, List<BasketPay> BasketPays, bool bType, out object returnData);

        /// <summary>
        /// 관리자확인
        /// </summary>
        /// <returns>실패: Empty; 성공: 관리자번호</returns>
        string ValidateAdmin();

        /// <summary>
        /// 상품교환권 - 반품
        /// </summary>
        /// <param name="payAmt"></param>
        /// <param name="returnData"></param>
        /// <returns></returns>
        DialogResult ShowExchangeRtnPopup(int payAmt, out object returnData);

        /// <summary>
        /// 거래정보표시
        /// </summary>
        /// <param name="keyValues"></param>
        void AutoRtnShowTrnxInfo(Dictionary<string, string> keyValues);

        /// <summary>
        /// 카드전자서명 팝업
        /// </summary>
        /// <param name="payAmt">결제금액</param>
        /// <returns></returns>
        string ShowCardSignPopup(int payAmt);

        /// <summary>
        /// 자동반품 진행중인 메시지표시
        /// </summary>
        /// <param name="payGrpCd">결제수단그룹코드</param>
        /// <param name="payDtlCd">PayDetail Code - 결제수단코드</param>
        void AutoRtnUpdateStatusMsg(string payGrpCd, string payDtlCd);

        /// <summary>
        /// 은련카드 비밀번호 입력창
        /// </summary>
        /// <param name="cardNo">카드번호</param>
        /// <param name="workKeyIndedx"></param>
        /// <param name="cardPin"></param>
        void ShowERCardPasswordPopup(string cardNo, out string workKeyIndedx, out string cardPin);

        /// <summary>
        /// 자동반품 VAN 승인오류처리
        /// Loc changed on 10.19
        /// - DialogResult: YesNoCancel
        /// - Cancel Return 하게 옵션처리
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="errorMessage"></param>
        /// <param name="allowCancel">Dialog취소가능하게</param>
        //DialogResult AutoRtnShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType errorType, string errorMessage);
        DialogResult AutoRtnShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType errorType,
            string errorMessage, bool allowCancel);

        /// <summary>
        /// 사은품 회수팝업
        /// </summary>
        /// <param name="presentList"></param>
        /// <param name="basketHeader"></param>
        /// <returns></returns>
        DialogResult AutoRtnShowTksPresentReturnPopup(List<PQ11RespData> presentList, BasketHeader basketHeader,
            out object returnData);

        /// <summary>
        /// 강제취소, 강제진행 한 basket을 요약정보 보여준다
        /// </summary>
        /// <param name="header">원거래 HEADER BASKET</param>
        /// <param name="baskets">강제처리 된 결제BASKET</param>
        void AutoRtnShowForceCancelBaskets(BasketHeader header, List<BasketBase> baskets);
    }
}
