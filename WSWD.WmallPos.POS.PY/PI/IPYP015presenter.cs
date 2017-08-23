//-----------------------------------------------------------------
/*
 * 화면명   : IPYP015presenter.cs
 * 화면설명 : 현금영수증 취소
 * 개발자   : TCL
 * 개발일자 : 2015.05.28
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PV;

namespace WSWD.WmallPos.POS.PY.PI
{
    public interface IPYP015presenter
    {
        /// <summary>
        /// 현금영수증취소요청
        /// Loc changed 10.24
        /// 전문 추가정보 같이보내기
        /// 점포, 포스, 거래, 첫상품
        /// 
        /// 여전볍 변경 05.27
        /// PV11ReqDataAdd > PV21ReqDataAdd 
        /// </summary>
        /// <param name="cashType">0: 자진발급, 1: 개인소득공제, 2: 사업자(지출증비)</param>
        /// <param name="isSwipe"></param>
        /// <param name="otApprDate"></param>
        /// <param name="otApprNo"></param>
        /// <param name="confirmNo"></param>
        /// <param name="cancReason">취소사유</param>
        /// <param name="cashAmt">금액</param>
        /// <param name="vatAmt">세액</param>
        /// <param name="addData">전문추가정보</param>
        void MakeCashRecptRequest(int cashType, bool isSwipe, string otApprDate, string otApprNo, string confirmNo,
            string cancReason, int cashAmt, int vatAmt, PV21ReqDataAdd addData);
    }
}
