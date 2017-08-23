using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.POS.SL.Data;
using WSWD.WmallPos.FX.Shared;

namespace WSWD.WmallPos.POS.SL.VI
{
    public interface IM001KeyView : IM001BaseView
    {
        string InputText { get; set; }
        int InputLength { set; }

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

        void ProcessKeyEvent(OPOSKeyEventArgs e);

        /// <summary>
        /// 오류시 어떤오류인지 표시
        /// </summary>
        void ReportInvalidState(InvalidDataInputState invalidState);
    }
}
