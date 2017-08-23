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
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.NetComm.Types;

namespace WSWD.WmallPos.POS.SL.VC
{
    /// <summary>
    /// 개발자   : TCL
    /// 개발일자 : 2015.06.22
    /// 자동반품시, 카드, 현금IC, 현금영수증 취소 오류시,
    /// 강제진행 한 결제 basket을 모아서 게산원한테 보여준 화면
    /// </summary>
    public partial class POS_SL_P006 : PopupBase03
    {
        public POS_SL_P006() : this(null, null)
        {
        }

        public POS_SL_P006(BasketHeader header, List<BasketBase> baskets)
        {
            InitializeComponent();

            #region 이벤트등록

            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.FormClosed += new FormClosedEventHandler(POS_SL_P006_FormClosed);

            #endregion

            if (baskets != null)
            {
                // display error messages
                StringBuilder sb = new StringBuilder();
                foreach (var bk in baskets)
                {
                    string basketTypeTypeName = string.Empty;
                    string noAppr = string.Empty;
                    //string ddAppr = string.Empty;
                    Int64 payAmt = 0;

                    if (BasketTypes.BasketPay.Equals(bk.BasketType))
                    {
                        BasketPay bp = (BasketPay)bk;

                        if (NetCommConstants.PAYMENT_DETAIL_CARD.Equals(bp.PayDtlCd))
                        {
                            noAppr = ((BasketPayCard)bp).OTApprNo;
                        }
                        else if (NetCommConstants.PAYMENT_DETAIL_CASH_IC.Equals(bp.PayDtlCd))
                        {
                            noAppr = ((BasketCashIC)bp).OTApprNo;
                        }
                        else if (NetCommConstants.PAYMENT_DETAIL_POINT.Equals(bp.PayDtlCd))
                        {
                            noAppr = ((BasketPoint)bp).DealApprovalNo;
                        }

                        basketTypeTypeName = NetCommConstants.PaymentTypeName(bp.PayGrpCd, bp.PayDtlCd);
                        payAmt = TypeHelper.ToInt64(bp.PayAmt);
                    }
                    else if (BasketTypes.BasketCashRecpt.Equals(bk.BasketType))
                    {
                        basketTypeTypeName = NetCommConstants.PaymentTypeName(NetCommConstants.PAYMENT_DETAIL_CASHRCP, 
                            NetCommConstants.PAYMENT_DETAIL_CASHRCP);
                        noAppr = ((BasketCashRecpt)bk).NoApprOrg;                        
                        payAmt = TypeHelper.ToInt64(((BasketCashRecpt)bk).AmAppr);
                    }
                    else if (BasketTypes.BasketPointSave.Equals(bk.BasketType))
                    {
                        basketTypeTypeName = NetCommConstants.PaymentTypeName(NetCommConstants.PAYMENT_DETAIL_POINTSAVE, 
                            NetCommConstants.PAYMENT_DETAIL_POINTSAVE);
                        payAmt = 0;
                    }

                    sb.AppendFormat("- {0}", basketTypeTypeName);
                    sb.Append(payAmt > 0 ? string.Format(": {0:#,##0}", payAmt) : string.Empty);
                    sb.Append(string.IsNullOrEmpty(noAppr) ? string.Empty : string.Format(" [원승인번호:{0}]", noAppr));
                    sb.AppendLine();
                }

                sb.AppendLine();
                sb.Append("- 전산실에 확인 요망.");
                lblMessage.Text = sb.ToString();
            }
        }

        void POS_SL_P006_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.btnOK.Click -= new EventHandler(btnOK_Click);
            this.FormClosed -= new FormClosedEventHandler(POS_SL_P006_FormClosed);
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
