using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.POS.SL.Data;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;

namespace WSWD.WmallPos.POS.SL.VI
{
    public interface ISLM001BaseView
    {
        string GuideMessage { get; set; }
        //string ErrorMessage { set; }
        string StatusMessage { set; }

        /// <summary>
        /// Show last guide message;
        /// </summary>
        void RestoreGuideMessage();

        /// <summary>
        /// 상품입력 시작확인
        /// </summary>
        void ItemsGrid_StartOperation();

        /// <summary>
        /// 상품리스트 초기화할지
        /// </summary>
        /// <param name="resetItems"></param>
        void ItemsGrid_DataInitialize(bool resetItems);
        
        void ItemsGrid_UpdateItemRow(int rowIndex, PBItemData itemData);
        void ItemsGrid_UpdateItemRow(PBItemData itemData);
        void ItemsGrid_AddItemRow(PBItemData itemData, bool setSelected);
        void ItemsGrid_CancelNewRow();

        /// <summary>
        /// Show error to view
        /// </summary>
        /// <param name="errorMessage"></param>
        void ReportErrorMessage(SaleViewErrorMessage errorMessage);

        /// <summary>
        /// Printer error, show message
        /// </summary>
        void ShowPrinterError();

        void ShowRtnError();

        bool ChkPrint();

        void ShowDateErrorMsg();

        /// <summary>
        /// 현재선택 된 행의 데이타
        /// </summary>
        PBItemData ItemsGrid_CurrentItem { get; }

        /// <summary>
        /// 현재 입력중인 상품들
        /// </summary>
        PBItemData[] DataRows { get; }

        /// <summary>
        /// Update summary info to view
        /// </summary>
        /// <param name="summaryData"></param>
        void UpdateSummary(SaleSummaryData summaryData);

        /// <summary>
        /// 결제내역업데이트한다
        /// </summary>
        /// <param name="pays"></param>
        /// <param name="scrollToView">마지막항목보여주기</param>
        void UpdatePayList(List<BasketPay> pays, bool scrollToView);

        /// <summary>
        /// 결제내역업데이트한다
        /// </summary>
        /// <param name="pays"></param>
        void UpdatePayList(List<BasketPay> pays);

        /// <summary>
        /// Restore from basket item list
        /// </summary>
        /// <param name="itemBasket"></param>
        void RestoreFromBaskets(BasketItem[] itemBasket);

        /// <summary>
        /// Close UI
        /// </summary>
        void Close();
    }
}
