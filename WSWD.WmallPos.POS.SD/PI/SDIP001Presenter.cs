using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.SD.PI
{
    public interface SDIP001Presenter
    {
        /// <summary>
        /// 개설조건 맞지 않음?
        /// </summary>
        /// <returns></returns>
        void PreLoading();

        /// <summary>
        /// 개설확인한다, 점서버하고
        /// </summary>
        void OpenCheck();

        /// <summary>
        /// 개설한다
        /// </summary>
        void DoOpen(bool isOnline, string procFg);
    }
}
