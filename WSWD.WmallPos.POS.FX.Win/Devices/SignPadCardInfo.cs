using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.FX.Win.Devices
{
    public class SignPadCardInfo
    {        

        /// <summary>
        /// Response Code
        /// </summary>
        public string ResCode { get; set; }        

        /// <summary>
        /// MS or IC, FB, KI - KEYIN
        /// </summary>
        public string CardType { get; set; }

        public string Count { get; set; }

        public string Reader { get; set; }

        public string EncData { get; set; }

        public string EncCardNo { get; set; }

        public string TransAmt { get; set; }
        
        /// <summary>
        /// 암호하지 않은 카드번호
        /// 원본 데이터
        /// </summary>
        public string NoEncCardNo { get; set; }

        /// <summary>
        /// S: MSR, C: IC Card
        /// POS Entry Model
        /// </summary>
        public string CardGubun { get; set; }


        /// <summary>
        /// 분리 된 카드번호
        /// </summary>
        public string NoEncExtCardNo { get; set; }

        /// <summary>
        /// ServiceCode
        /// 서비스 코드가 코드가 ‘2’ 또는 ‘6’일 경우 IC 가 삽입 
        /// 되어 있는 카드이므로 카드이므로 MS MS 거래를 하지 못하 도록 해야함 .
        /// </summary>
        public string ServiceCode { get; set; }
    }
}
