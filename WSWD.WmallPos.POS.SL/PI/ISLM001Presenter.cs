using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.POS.SL.Data;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PP;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;

namespace WSWD.WmallPos.POS.SL.PI
{
    public interface ISLM001Presenter
    {
        /// <summary>
        /// 1-2단 품목입력하거나 상품스캔
        /// </summary>
        /// <param name="scannedText"></param>
        void ProcessScanCode(string scannedText);

        /// <summary>
        /// Process key event
        /// </summary>
        /// <param name="e"></param>
        bool ProcessKeyEvent(OPOSKeyEventArgs e);
        
        /// <summary>
        /// Process key event completed
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        bool OnProcessKeyEventReturn(OPOSKeyEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        void UpdateSummary();

        /// <summary>
        /// 회원정보
        /// </summary>
        PP01RespData CustInfo { get; set; }

        /// <summary>
        /// Validate event
        /// </summary>
        /// <param name="mapKey"></param>
        /// <param name="scan"></param>
        /// <param name="touch"></param>
        /// <returns></returns>
        bool ValidateKeyInput(OPOSMapKeys mapKey, bool scan, bool touch);

        /// <summary>
        /// 상태업데이트
        /// </summary>
        /// <param name="inputState"></param>
        void ReportInvalidState(InvalidDataInputState inputState);

        /// <summary>
        /// Load items from list
        /// </summary>
        /// <param name="items"></param>
        void LoadItems(BasketItem[] items);

        /// <summary>
        /// Get tax amt in payamt
        /// </summary>
        /// <param name="payAmt"></param>
        /// <returns></returns>
        int GetTaxAmt(int payAmt);

        /// <summary>
        /// Change sale mode
        /// </summary>
        /// <param name="saleMode"></param>
        /// <param name="checkCondition">상품있는지 확인여부</param>
        /// <param name="validateAdmin">관리자확인여부</param>
        /// <param name="resetSummary">모드전환시 결제내역,금액내역 reset 하는지 확인여부</param>
        ChangeSaleModeStatus ChangeSaleMode(SaleModes saleMode, bool checkCondition, bool validateAdmin, bool resetSummary);

        /// <summary>
        /// 자동반품확정한다
        /// </summary>
        void AutoRtnConfirmStart();
    }
}
