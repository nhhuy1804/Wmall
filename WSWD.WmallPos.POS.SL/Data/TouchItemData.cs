using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.SL.Data
{
    public class TouchItemData
    {
        /// <summary>
        /// 일반품번 정산 RANGE
        /// </summary>
        public const string PB_NM_RNG_FR = "00";
        public const string PB_NM_RNG_TO = "19";
        
        /// <summary>
        /// 행사품번 RAGE
        /// </summary>
        public const string PB_EV_RNG_FR = "20";
        public const string PB_EV_RNG_TO = "29";


        //2015.09.08  정광호 추가----------------------------------------
        //***************************************************************
        /// <summary>
        /// 특가품번 RAGE
        /// </summary>
        public const string PB_SP_RNG_FR = "30";
        public const string PB_SP_RNG_TO = "49";

        /// <summary>
        /// 균일품번 RAGE
        /// </summary>
        public const string PB_EQ_RNG_FR = "50";
        public const string PB_EQ_RNG_TO = "59";

        /// <summary>
        /// 온라인품번 RAGE
        /// </summary>
        public const string PB_ON_RNG_FR = "60";
        public const string PB_ON_RNG_TO = "69";

        /// <summary>
        /// 노마진품번 RAGE
        /// </summary>
        public const string PB_NO_RNG_FR = "70";
        public const string PB_NO_RNG_TO = "79";

        /// <summary>
        /// 티켓품번 RAGE
        /// </summary>
        public const string PB_TI_RNG_FR = "80";
        public const string PB_TI_RNG_TO = "89";
        //***************************************************************
        //---------------------------------------------------------------

        public string CdGrop;
        public int SqSort;
        public string CdItem;
        public string CdDp;
        public string NmItem;
        public int UtSprc;

        /// <summary>
        /// 품번인지 확인
        /// </summary>
        public bool IsPB
        {
            get
            {
                return SLExtensions.CDDP_PB.Equals(CdDp);
            }
        }

        public static bool InRange(string value, string fromValue, string toValue)
        {
            int v = TypeHelper.ToInt32(value);
            int fv = TypeHelper.ToInt32(fromValue);
            int tv = TypeHelper.ToInt32(toValue);
            return v >= fv && v <= tv;
        }
    }
}
