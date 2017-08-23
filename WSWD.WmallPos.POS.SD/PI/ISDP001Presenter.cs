using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.SD.PI
{
    public interface ISDP001Presenter
    {
        /// <summary>
        /// 현재영업일자
        /// </summary>
        string SaleDate { get; }

        /// <summary>
        /// 개설조건 맞지 않음?
        /// </summary>
        /// <returns></returns>
        string[] PreLoading();

        /// <summary>
        /// 개설한다
        /// </summary>
        void DoOpen(bool isOnline, string procFg);        
    }
}
