using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.POS.SL.Data;
using WSWD.WmallPos.POS.FX.Win.UserControls;

namespace WSWD.WmallPos.POS.SL.VI
{
    public interface IM001BaseView
    {
        string GuideMessage { get; set; }
        string ErrorMessage { set; }
        
        void UpdateItemRow(PBItemData itemData);
        void AddItemRow(PBItemData itemData);
        void CancelNewRow();

        /// <summary>
        /// 현재 입력중인 상품들
        /// </summary>
        PBItemData[] DataRows { get; }
    }
}
