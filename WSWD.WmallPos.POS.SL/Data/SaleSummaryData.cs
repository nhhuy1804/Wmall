using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.SL.Data
{
    /// <summary>
    /// 결제내역 SUMMARY
    /// </summary>
    public class SaleSummaryData
    {
        public static SaleSummaryData Empty
        {
            get
            {
                return new SaleSummaryData();
            }
        }

        /// <summary>
        /// 합계금액
        /// </summary>
        public int TotalAmt
        {
            get
            {
                return TotalItemAmt - DiscAmt;
            }
        }

        public int TotalItemAmt
        {
            get;
            set;
        }

        /// <summary>
        /// 할인금액
        /// </summary>
        public int DiscAmt { get; set; }

        /// <summary>
        /// 받을돈
        /// </summary>
        public int RecvAmt
        {
            get
            {
                return Math.Max(TotalAmt - PaidAmt, 0);
            }
        }

        /// <summary>
        /// 받은돈
        /// </summary>
        public int PaidAmt { get; set; }

        /// <summary>
        /// 잔액, 거스름돈
        /// </summary>
        public int ChangeAmt
        {
            get
            {
                return PaidAmt > TotalAmt ? PaidAmt - TotalAmt : 0;
            }
        }

        /// <summary>
        /// 과세상품 합계금액
        /// </summary>
        public int TaxItemAmt { get; private set; }

        /// <summary>
        /// 면세상품 합계금액
        /// </summary>
        public int NoTaxItemAmt { get; private set; }

        /// <summary>
        /// UPdate summary data
        /// </summary>
        /// <param name="items"></param>
        public void Update(PBItemData[] items)
        {
            TotalItemAmt = items.Sum(p => p == null ? 0 : p.AmItem);
            TaxItemAmt = items.Where(p => "1".Equals(p.FgTax)).Sum(p => p.AmItem);
            NoTaxItemAmt = items.Where(p => !"1".Equals(p.FgTax)).Sum(p => p.AmItem);
        }

        /// <summary>
        /// 결제금액의 세금액 계산 (비율)
        /// - 전상품 과세합 및 면세합 비율계산
        /// - 결제금액 (현금이나 카드) * 비율
        /// 
        /// - VAN승인시 사용;
        ///     - 현금영수증 및 카드
        /// </summary>
        /// <param name="payAmt"></param>
        /// <param name="taxAmt"></param>
        /// <returns></returns>
        public int CalcTaxPerc(int payAmt, bool bCalcTaxAmt)
        {
            var dNoTaxAmt = Convert.ToDouble(TotalAmt) / 1.1;
            int nNoTaxAmt = (int)Math.Round(dNoTaxAmt);
            int nTaxAmt = TotalAmt - nNoTaxAmt;

            double percTax = Convert.ToDouble(this.TaxItemAmt) / TotalAmt;
            int payTaxAmt = (int)Math.Round(percTax * payAmt);

            return CalcTaxAmt(payTaxAmt, bCalcTaxAmt);
        }

        public int CalcTaxAmt(int amt, bool calcTax)
        {
            var dNoTaxAmt = Convert.ToDouble(amt) / 1.1;
            int nNoTaxAmt = (int)Math.Round(dNoTaxAmt);
            int nTaxAmt = amt - nNoTaxAmt;

            return calcTax ? nTaxAmt : nNoTaxAmt;
        }

        public void Update(List<BasketPay> pays)
        {
            PaidAmt = 0;
            foreach (var pay in pays)
            {
                PaidAmt += TypeHelper.ToInt32(pay.PayAmt);
            }
        }

        /// <summary>
        /// Update summary from items}
        /// </summary>
        /// <param name="items"></param>
        public void Update(BasketItem[] items)
        {
            DiscAmt = 0;
            TotalItemAmt = 0;
            foreach (var item in items)
            {
                DiscAmt += TypeHelper.ToInt32(item.AmDisc);
                TotalItemAmt += TypeHelper.ToInt32(item.AmSale);
            }
        }

        /// <summary>
        /// 결제한 금액업데이트한다
        /// </summary>
        /// <param name="paidAmt"></param>
        public void Update(int paidAmt)
        {
            PaidAmt += paidAmt;
        }
    }
}
