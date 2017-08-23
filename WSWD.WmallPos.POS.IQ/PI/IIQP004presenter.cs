//-----------------------------------------------------------------
/*
 * 화면명   : IIQP004presenter.cs
 * 화면설명 : 영수증 조회
 * 개발자   : 정광호
 * 개발일자 : 2015.05.
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace WSWD.WmallPos.POS.IQ.PI
{
    public interface IIQP004presenter
    {
        /// <summary>
        /// 영수증 조회
        /// </summary>
        /// <param name="strSaleDate">조회일자</param>
        /// <param name="strPosNo">포스번호</param>
        /// <param name="strTrxnNo">거래번호</param>
        void GetReceipt(string strSaleDate, string strPosNo, string strTrxnNo);

        /// <summary>
        /// 영수증 메세지 조회
        /// </summary>
        /// <param name="strStoreNo">점포코드</param>
        /// <param name="strCdClass">품번코드</param>
        DataSet GetReceiptMsg(string strStoreNo, string strCdClass);

        /// <summary>
        /// 프린트 DCC내용 전체조회
        /// </summary>
        /// <returns></returns>
        DataTable GetPrintDccMsg();
    }
}
