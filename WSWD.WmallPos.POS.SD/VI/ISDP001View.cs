using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.POS.FX.Win.UserControls;

namespace WSWD.WmallPos.POS.SD.VI
{
    public delegate void ErrorRetryDelegateHandler();

    public interface ISDP001View
    {        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="procFg">개설가능여부 ProcFg</param>
        /// <param name="lastErrorMessage"></param>
        /// <param name="lastException"></param>
        void Step03_OnOpenCheckResult(string procFg, string lastErrorMessage, Exception lastException);
        /// <summary>
        /// 개설작업항목 상태업데이트한다
        /// </summary>
        /// <param name="message"></param>
        /// <param name="status"></param>
        void AddOpenItemStatus(string message, OpenItemStatus status);

        /// <summary>
        /// 진행상태 업데이트, 현재항목
        /// </summary>
        /// <param name="message"></param>
        /// <param name="status"></param>
        void UpdateOpenItemStatus(string message, OpenItemStatus status);

        /// <summary>
        /// 개설완료
        /// </summary>
        void DoOpenComplete();

        /// <summary>
        /// ERROR MESSAGE  SHOW,RETRY OR NEXT
        /// </summary>
        /// <param name="message"></param>
        /// <param name="retryCallBack"></param>
        /// <param name="nextCallBack"></param>
        void ShowErrorMessage(string message, ErrorRetryDelegateHandler retryCallBack, ErrorRetryDelegateHandler nextCallBack);

        /// <summary>
        /// Printer error
        /// </summary>
        void ShowPrinterError();


        bool ChkPrint();
    }
}
