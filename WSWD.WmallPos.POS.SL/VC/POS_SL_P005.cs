//-----------------------------------------------------------------
/*
 * 화면명   : POS_SL_P005.cs
 * 화면설명 : 사은품 회수
 * 개발자   : TCL
 * 개발일자 : 2015.09.03
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PQ;

namespace WSWD.WmallPos.POS.SL.VC
{
    /// <summary>
    /// 사은품회사
    /// </summary>
    public partial class POS_SL_P005 : PopupBase02
    {
        public POS_SL_P005()
            : this(null, null)
        {

        }

        public POS_SL_P005(List<PQ11RespData> presentList, BasketHeader basketHeader)
        {
            InitializeComponent();

            this.m_prsnList = new List<PQ11RespData>();

            if (presentList != null)
            {
                foreach (var prn in presentList)
                {
                    PQ11RespData p = (PQ11RespData)PQ11RespData.Parse(typeof(PQ11RespData), prn.ToString());
                    this.m_prsnList.Add(p);
                }
            }
            
            this.m_bskHeader = basketHeader;

            // 데이타로드
            FormInitialize();
        }
    }
}
