//-----------------------------------------------------------------
/*
 * 화면명   : IPYP001presenter.cs
 * 화면설명 : 신용카드결제
 * 개발자   : 정광호
 * 개발일자 : 2015.05.15
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
    public interface IPYP001Presenter
    {
        /// <summary>
        /// 여전법 변경 05.27
        /// 
        /// bool isAppr > int cancType; 0: 승인, 1: 취소, 9: 망상취소
        /// 
        /// </summary>
        /// <param name="cancType">0: 승인, 1: 취소, 9: 망상취소</param>
        /// <param name="trackII"></param>
        /// <param name="inputType"></param>
        /// <param name="installment"></param>
        /// <param name="signData"></param>
        /// <param name="payAmt"></param>
        /// <param name="vatAmt"></param>
        /// <param name="fgERCard"></param>
        /// <param name="fgDCCCheck"></param>
        /// <param name="workKeyIndex"></param>
        /// <param name="cardPinNo"></param>
        /// <param name="otApprDate">반품시 원거래일자+시간</param>
        /// <param name="otArppNo">반품시 원거래번호</param>
        /// <param name="dccData"></param>
        /// <param name="addData"></param>
        void RequestVANCardPayment(int cancType, string trackII, string inputType, string installment,
                    string signData, string payAmt, string vatAmt, string fgERCard, string fgDCCCheck,
            string workKeyIndex, string cardPinNo, string otApprDate, string otArppNo, object dccData,
            PV21ReqDataAdd addData);

        int LastFrameNo
        {
            get;
        }
    }
}
