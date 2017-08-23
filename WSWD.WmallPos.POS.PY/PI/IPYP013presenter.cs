//-----------------------------------------------------------------
/*
 * 화면명   : IPYP013presenter.cs
 * 화면설명 : 현금IC결제
 * 개발자   : TCL
 * 개발일자 : 2015.05.28 / 06.04
 *  
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WSWD.WmallPos.POS.PY.PI
{
    public interface IPYP013presenter
    {
        /// <summary>
        /// 현금IC결제
        /// </summary>
        /// <param name="isAppr"></param>
        /// <param name="isChipReader"></param>
        /// <param name="fgSimpTrxn">간소화거래여부</param>
        /// <param name="icCardSeqNo"></param>
        /// <param name="issuerCode"></param>
        /// <param name="encData"></param>
        /// <param name="trackIII"></param>
        /// <param name="payAmt"></param>
        /// <param name="taxAmt"></param>
        /// <param name="orgApprDate"></param>
        /// <param name="orgApprNo"></param>
        void ProcessVANCashIC(bool isAppr, bool isChipReader,
            string fgSimpTrxn,
            string icCardSeqNo, string issuerCode,
            string issuerPosCd, 
            string encData, string trackIII,
            string payAmt, string taxAmt, string orgApprDate, string orgApprNo);

        /// <summary>
        /// 난수확인할것
        /// </summary>
        /// <param name="payAmt"></param>
        /// <param name="taxAmt"></param>
        void ProcessGetRanNum(string payAmt, string taxAmt);
    }
}
