using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.POS.ST.PT;

namespace WSWD.WmallPos.POS.ST.PI
{
    public interface STIM001Presenter
    {
        void ValidateOnOpen();

        /// <summary>
        /// 디비에 개인정보, 카드번홀ㄹ
        /// 00으로 마스킹한다
        /// 90일
        /// </summary>
        void Clear85DaysSensData();
    }
}
