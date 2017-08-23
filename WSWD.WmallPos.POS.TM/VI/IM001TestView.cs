using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WSWD.WmallPos.POS.TM.VI
{
    /// <summary>
    /// POS_TM_M001화면에서 일부화면기능의
    /// ITestView으로 빼낸다.
    /// 
    /// </summary>
    public interface IM001TestView
    {
        void UpdateBSM043TData(DataSet ds);
    }
}
