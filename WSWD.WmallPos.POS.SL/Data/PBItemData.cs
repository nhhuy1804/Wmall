using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.FX.Shared.Utils;
using System.Diagnostics;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared;

namespace WSWD.WmallPos.POS.SL.Data
{
    public delegate void PBItemScannedEventHandler(PBItemData itemData);

    /// <summary>
    /// 품번PARSER
    /// </summary>
    public class PBItemData
    {
        public PBItemData()
        {
            this.CompletedStep = PBItemParseStep.Empty;
            this.ScannedStep = PBItemParseStep.Empty;
            this.Barcode = this.CdClass = this.NmClass = this.CdItem = this.NmItem =
                this.AmDisc = this.PercDisc =
                this.FgClass = this.Qty = this.UtSprc = string.Empty;
            this.Properties = PBItemProperties.None;
            this.FgCanc = "0";
        }

        /// <summary>
        /// LENGTH = 13
        /// </summary>
        public string Barcode
        {
            get;
            set;
        }

        /// <summary>
        /// 품번코드 6자리
        /// </summary>
        public string CdClass { get; set; }
        /// <summary>
        /// 품번명 
        /// </summary>
        public string NmClass { get; set; }
        /// <summary>
        /// 푸목코드 4자리
        /// </summary>
        public string CdItem { get; set; }
        /// <summary>
        /// 품목명칭 
        /// </summary>
        public string NmItem { get; set; }
        /// <summary>
        /// 상품구분 2자리
        /// </summary>
        public string FgClass { get; set; }
        /// <summary>
        /// 수량
        /// </summary>
        public string Qty { get; set; }
        /// <summary>
        /// 단가금액 8자리
        /// </summary>
        public string UtSprc { get; set; }

        /// <summary>
        /// 할인율
        /// </summary>
        public string PercDisc { get; set; }

        /// <summary>
        /// 할인금액
        /// </summary>
        public string AmDisc { get; set; }

        /// <summary>
        /// 에누리금액
        /// </summary>
        public string AmEnuri { get; set; }

        /// <summary>
        /// 상품가격 변경 여부
        /// </summary>
        public bool FgUtSprcChanged { get; set; }

        /// <summary>
        /// 1:과세, 2:면세, 3:영세율	
        /// </summary>
        public string FgTax
        {
            get;
            set;
        }

        /// <summary>
        /// 0:없음 / 1:직전  / 2:지정
        /// 취소여부
        /// </summary>
        public string FgCanc
        {
            get;
            set;
        }

        /// <summary>
        /// 마지막 취소하기전에 수량
        /// </summary>
        public string QtyCanc { get; set; }

        /// <summary>
        /// 금액
        /// </summary>
        public int AmItem
        {
            get
            {
                int q = TypeHelper.ToInt32(Qty);
                int p = TypeHelper.ToInt32(UtSprc);

                int disc = TypeHelper.ToInt32(AmDisc);
                int enr = TypeHelper.ToInt32(AmEnuri);

                return p * q - disc - enr;
            }
        }

        /// <summary>
        /// 처리 완료 단계
        /// </summary>
        public PBItemParseStep CompletedStep { get; set; }

        /// <summary>
        /// Parsing 된 단계
        /// </summary>
        public PBItemParseStep ScannedStep { get; set; }

        /// <summary>
        /// 처리방식 PARSE
        /// </summary>
        public PBItemParseType ParseType { get; set; }

        /// <summary>
        /// 상품구분
        /// 0:단품(PLU),1:NON_PLU,2:품번,3:외식상품,4:저장물	
        /// </summary>
        public string CdDp { get; set; }

        /// <summary>
        /// Show properties
        /// </summary>
        public PBItemProperties Properties
        {
            get;
            set;
        }

        public ItemAmountValidatorHandler AmountValidator;

        /// <summary>
        /// On scanned complete
        /// </summary>
        public event PBItemScannedEventHandler ScannedEvent;

        /// <summary>
        /// When processing and validate input data complete
        /// Advanced to next step
        /// </summary>
        public event PBItemScannedEventHandler CompletedStepEvent;

        /// <summary>
        /// Parse ItemCode
        /// ONLY WHEN
        /// </summary>
        /// <param name="inputCode"></param>
        /// <returns></returns>
        public PBItemParseResult Parse(string inputCode, ItemInputOperation inputOperation)
        {
            // 전체품번 6 + 4 + 2
            if (CompletedStep == PBItemParseStep.Empty && this.ParseType == PBItemParseType.FullCode)
            {
                return ParseFullCode(inputCode);
            }

            int validResult = 0;

            /// 수동입력이거나 2단바코드 SCAN
            PBItemParseResult parseResult = PBItemParseResult.Success;
            switch (CompletedStep)
            {
                case PBItemParseStep.Empty:
                    if (IsBarCode(inputCode))
                    {
                        CdClass = inputCode.Substring(2, Math.Min(6, inputCode.Length - 2));
                        CdItem = inputCode.Length >= 8 ? inputCode.Substring(8, Math.Min(4, inputCode.Length - 8)) : string.Empty;
                        ParseType = PBItemParseType.TwoStep;
                    }
                    else
                    {
                        CdClass = inputCode.Length != 6 ? string.Empty : inputCode.Substring(0, Math.Min(6, inputCode.Length));
                        ParseType = PBItemParseType.Manual;
                    }

                    CdDp = SLExtensions.CDDP_PB;
                    ScannedStep = string.IsNullOrEmpty(CdItem) || CdItem.Length != 4 ?
                        (CdClass.Length != 6 ? PBItemParseStep.Empty : PBItemParseStep.CdClass) : PBItemParseStep.CdItem;
                    parseResult = ScannedStep > PBItemParseStep.Empty ? PBItemParseResult.Success : PBItemParseResult.InvalidLength;
                    break;
                case PBItemParseStep.CdClass:
                    // reading CdItem
                    // 품번코드만 확인 된상태이니 품목코드만 받는다 4자리

                    // 수동입력아니면 잘못 된값
                    if (ParseType != PBItemParseType.Manual)
                    {
                        parseResult = PBItemParseResult.InvalidData;
                        break;
                    }

                    CdItem = inputCode.Length >= 6 ? inputCode.Substring(6, Math.Min(4, inputCode.Length - 6)) :
                        inputCode.Substring(0, Math.Min(4, inputCode.Length));
                    if (!string.IsNullOrEmpty(CdItem) && CdItem.Length == 4)
                    {
                        ScannedStep = PBItemParseStep.CdItem;
                        break;
                    }

                    parseResult = PBItemParseResult.InvalidLength;
                    break;
                case PBItemParseStep.CdItem:
                    // 품번코드, 품목코드 받은상태
                    // 구분자 & 상품금액 받는다
                    if ((inputCode.Length == 2 &&
                        inputOperation == ItemInputOperation.ManualEnter)
                        || (inputCode.Length >= 12 && (
                        inputOperation == ItemInputOperation.PLU ||
                        inputOperation == ItemInputOperation.Scan)))
                    {
                        bool isBarCode = IsBarCode(inputCode);
                        int startIndex = isBarCode ? 2 : 0;

                        if (inputCode.Length >= 12)
                        {
                            if (isBarCode)
                            {
                                FgClass = inputCode.Substring(startIndex, Math.Min(2, inputCode.Length - startIndex));
                                UtSprc = inputCode.Length > 6 ? inputCode.Substring(6, Math.Min(6, inputCode.Length - 6)) :
                                    string.Empty;
                            }
                            else
                            {
                                parseResult = PBItemParseResult.SecBarCodeInput;
                                break;
                            }
                        }
                        else
                        {
                            FgClass = inputCode.Substring(startIndex, Math.Min(2, inputCode.Length - startIndex));
                            UtSprc = inputCode.Length > 6 ? inputCode.Substring(6, Math.Min(6, inputCode.Length - 6)) :
                                string.Empty;
                        }

                        if (string.IsNullOrEmpty(FgClass) || FgClass.Length != 2)
                        {
                            parseResult = PBItemParseResult.InvalidLength;
                            break;
                        }

                        ScannedStep = PBItemParseStep.FgClass;
                        if (ValidatePrice(UtSprc, true, out validResult))
                        {
                            ScannedStep = PBItemParseStep.UtSprc;
                        }

                        break;
                    }

                    parseResult = PBItemParseResult.InvalidLength;
                    break;

                case PBItemParseStep.FgClass:
                    // 금액확인
                    if (ValidatePrice(inputCode, true, out validResult))
                    {
                        parseResult = PBItemParseResult.Success;
                    }
                    else
                    {
                        if (validResult == 0)
                        {
                            parseResult = PBItemParseResult.InvalidData;
                        }
                        else if (validResult == 1)
                        {
                            parseResult = PBItemParseResult.NumberOverflow;
                        }
                        else
                        {
                            parseResult = PBItemParseResult.TotalAmountOver;
                        }
                    }
                    break;
                case PBItemParseStep.UtSprc:
                    break;
                default:
                    break;
            }

            if (parseResult == PBItemParseResult.Success)
            {
                if (ScannedEvent != null)
                {
                    ScannedEvent(this);
                }
            }
            return parseResult;
        }

        public void Parse()
        {
            if (!string.IsNullOrEmpty(CdClass))
            {
                ScannedStep = PBItemParseStep.CdClass;
            }

            if (!string.IsNullOrEmpty(CdItem))
            {
                ScannedStep = PBItemParseStep.CdItem;
            }

            if (!string.IsNullOrEmpty(FgClass))
            {
                ScannedStep = PBItemParseStep.FgClass;
            }

            if (!string.IsNullOrEmpty(UtSprc) && TypeHelper.ToInt32(UtSprc) > 0)
            {
                ScannedStep = PBItemParseStep.UtSprc;
            }
        }

        /// <summary>
        /// Complete scanned step to completed step
        /// </summary>
        public void CompleteStep()
        {
            this.CompletedStep = ScannedStep;
            if (CompletedStepEvent != null)
            {
                CompletedStepEvent(this);
            }
        }

        public void Merge(PBItemData itemData)
        {
            if ((itemData.Properties & PBItemProperties.CdClass) == PBItemProperties.CdClass)
            {
                this.CdClass = itemData.CdClass;
            }
            if ((itemData.Properties & PBItemProperties.NmClass) == PBItemProperties.NmClass)
            {
                this.NmClass = itemData.NmClass;
            }
            if ((itemData.Properties & PBItemProperties.CdItem) == PBItemProperties.CdItem)
            {
                this.CdItem = itemData.CdItem;
            }
            if ((itemData.Properties & PBItemProperties.NmItem) == PBItemProperties.NmItem)
            {
                this.NmItem = itemData.NmItem;
            }
            if ((itemData.Properties & PBItemProperties.FgClass) == PBItemProperties.FgClass)
            {
                this.FgClass = itemData.FgClass;
            }
            if ((itemData.Properties & PBItemProperties.Qty) == PBItemProperties.Qty)
            {
                this.Qty = itemData.Qty;
            }
            if ((itemData.Properties & PBItemProperties.FgCanc) == PBItemProperties.FgCanc)
            {
                this.FgCanc = itemData.FgCanc;
                this.QtyCanc = itemData.QtyCanc;
                this.Qty = itemData.Qty;
            }
            if ((itemData.Properties & PBItemProperties.Price) == PBItemProperties.Price)
            {
                this.UtSprc = itemData.UtSprc;
            }

            if (!string.IsNullOrEmpty(itemData.CdDp))
            {
                this.CdDp = itemData.CdDp;
            }

            if (!string.IsNullOrEmpty(itemData.FgTax))
            {
                this.FgTax = itemData.FgTax;
            }

            this.Barcode = string.IsNullOrEmpty(itemData.Barcode) ? this.Barcode : itemData.Barcode;
            this.FgUtSprcChanged = itemData.FgUtSprcChanged;
            this.PercDisc = itemData.PercDisc;
            this.AmDisc = itemData.AmDisc;
            this.AmEnuri = itemData.AmEnuri;

            this.Properties |= itemData.Properties;
        }

        public PBItemData Copy()
        {
            return new PBItemData()
            {
                CdClass = this.CdClass,
                NmClass = this.NmClass,
                CdItem = this.CdItem,
                NmItem = this.NmItem,
                FgClass = this.FgClass,
                Qty = this.Qty,
                UtSprc = this.UtSprc,
                FgCanc = this.FgCanc,
                Barcode = this.Barcode,
                AmDisc = this.AmDisc,
                AmEnuri = this.AmEnuri,
                CdDp = this.CdDp,
                CompletedStep = this.CompletedStep,
                FgTax = this.FgTax,
                FgUtSprcChanged = this.FgUtSprcChanged,
                PercDisc = this.PercDisc,
                QtyCanc = this.QtyCanc,
                ParseType = this.ParseType,
                Properties = this.Properties,
                ScannedStep = this.ScannedStep
            };

        }

        /// <summary>
        /// 2단바코드인지 확인
        /// </summary>
        /// <param name="inputCode"></param>
        /// <returns></returns>
        bool IsBarCode(string inputCode)
        {
            if (ScannedStep == PBItemParseStep.Empty)
            {
                return !string.IsNullOrEmpty(inputCode) && inputCode.StartsWith(GoodsCodePrefix1) && inputCode.Length > 12;
            }
            else if (ScannedStep >= PBItemParseStep.CdClass)
            {
                return !string.IsNullOrEmpty(inputCode) && inputCode.StartsWith(GoodsCodePrefix2) && inputCode.Length > 12;
            }
            else
            {
                return !string.IsNullOrEmpty(inputCode) && inputCode.Length > 12;
            }
        }

        /// <summary>
        /// BARCODE
        /// </summary>
        /// <param name="scannedCode">Scanned된 바코드</param>
        /// <returns></returns>
        static public CdDpTypes GetCdDp(string scannedCode)
        {
            return !string.IsNullOrEmpty(scannedCode) && (scannedCode.StartsWith(GoodsCodePrefix1) ||
                scannedCode.StartsWith(GoodsCodePrefix2))
                && scannedCode.Length > 12 ? CdDpTypes.PB : CdDpTypes.PLU;
        }

        /// <summary>
        /// 품번1단바코드 PREFIX
        /// </summary>
        static public string GoodsCodePrefix1
        {
            get
            {
                return string.IsNullOrEmpty(ConfigData.Current.AppConfig.PosOption.GoodsCodePrefix1) ? "22" :
                    ConfigData.Current.AppConfig.PosOption.GoodsCodePrefix1;
            }
        }

        /// <summary>
        /// 품번2단바코드 PREFIX
        /// </summary>
        static public string GoodsCodePrefix2
        {
            get
            {
                return string.IsNullOrEmpty(ConfigData.Current.AppConfig.PosOption.GoodsCodePrefix2) ? "29" :
                    ConfigData.Current.AppConfig.PosOption.GoodsCodePrefix2;
            }
        }

        /// <summary>
        /// CDDP Print
        /// </summary>
        /// <param name="cdDp"></param>
        /// <returns></returns>
        static public string GetCdDpPrint(string cdDp)
        {
            if ("2".Equals(cdDp))
            {
                return "M";
            }
            else if ("4".Equals(cdDp))
            {
                return "S";
            }
            else
            {
                return "P";
            }
        }

        static public PBItemData Empty
        {
            get
            {
                return new PBItemData();
            }
        }

        /// <summary>
        /// 전체품번 Parsing 하기
        /// </summary>
        /// <param name="inputCode"></param>
        /// <returns></returns>
        private PBItemParseResult ParseFullCode(string inputCode)
        {
            CdClass = inputCode.Substring(0, Math.Min(6, inputCode.Length));
            if (!string.IsNullOrEmpty(CdClass))
            {
                this.ScannedStep = PBItemParseStep.CdClass;
            }

            CdItem = inputCode.Length > 6 ?
                inputCode.Substring(6, Math.Min(4, inputCode.Length - 6)) :
                string.Empty;
            if (!string.IsNullOrEmpty(CdItem))
            {
                this.ScannedStep = PBItemParseStep.CdItem;
            }

            FgClass = inputCode.Length > 10 ?
                inputCode.Substring(10, Math.Min(2, inputCode.Length - 10)) :
                string.Empty;
            if (!string.IsNullOrEmpty(FgClass))
            {
                this.ScannedStep = PBItemParseStep.FgClass;
            }

            this.Barcode = inputCode;
            UtSprc = inputCode.Length > 14 ?
                inputCode.Substring(14, Math.Min(1, inputCode.Length - 12)) :
                string.Empty;

            int validResult = 0;
            bool valid = ValidatePrice(UtSprc, !string.IsNullOrEmpty(UtSprc), out validResult);
            if (ScannedStep != PBItemParseStep.Empty)
            {
                if (ScannedEvent != null)
                {
                    ScannedEvent(this);
                }
            }

            return valid ? PBItemParseResult.Success : (validResult == 0 ? PBItemParseResult.InvalidData :
                (validResult == 1 ? PBItemParseResult.NumberOverflow : PBItemParseResult.TotalAmountOver));
        }

        /// <summary>
        /// BasketIte 생성
        /// </summary>
        /// <returns></returns>
        public BasketItem ToBasketItem()
        {
            string sourceCode = Barcode;
            if (SLExtensions.CDDP_PB.Equals(CdDp))
            {
                sourceCode = GoodsCodePrefix1 + CdClass + CdItem;
                sourceCode = sourceCode + DataUtils.fnGetBarCodeCheckDigit(sourceCode);
            }

            string inCdItem = Barcode;
            if (SLExtensions.CDDP_PB.Equals(CdDp))
            {
                inCdItem = "26" + CdClass + CdItem;
                inCdItem = inCdItem + DataUtils.fnGetBarCodeCheckDigit(inCdItem);
            }

            return new BasketItem()
            {
                SourceCode = sourceCode,
                InCdItem = inCdItem,
                NmItem = SLExtensions.CDDP_PB.Equals(CdDp) ?
                        string.Format("{0}{1}", string.IsNullOrEmpty(NmClass) ? string.Empty : NmClass + "_", NmItem)
                         : NmItem,
                CdClass = CdClass,
                NmClass = NmClass,
                CdItem = CdItem,
                CntItem = Qty,
                CntCancel = QtyCanc,
                FgCanc = FgCanc,
                FgTax = FgTax,
                FgMargin = FgClass,
                UtSprc = UtSprc,
                AmSale = AmItem.ToString(),
                AmDisc = AmDisc,
                FgDiscProc = TypeHelper.ToInt32(AmDisc) == 0 ? "0" : (TypeHelper.ToInt32(this.PercDisc) > 0 ? "1" : "2"),
                PcDisc = PercDisc.ToString(),
                CdDp = CdDp,
                FgNewPrcApp = FgUtSprcChanged ? "1" : "0"
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="price"></param>
        /// <param name="validateInput">validate enter amouint > 0</param>
        /// <returns></returns>
        bool ValidatePrice(string price, bool validateInput, out int validResult)
        {
            if (price.Length > 8)
            {
                price = price.Substring(0, 8);
            }

            validResult = 0;
            if (price.Length < 2 && validateInput)
            {
                return false;
            }

            bool valid = false;
            int prc = price.ValidateMoney(0, out valid);

            if (!valid)
            {
                return valid;
            }

            if (validateInput && prc <= 0)
            {
                valid = false;
            }
            else
            {
                if (prc > 0)
                {
                    if (AmountValidator != null)
                    {
                        validResult = AmountValidator(false, prc);

                        if (validResult == 0)
                        {
                            UtSprc = prc.ToString();
                            ScannedStep = PBItemParseStep.UtSprc;
                        }

                        if (validateInput)
                        {
                            valid = validResult == 0;
                        }
                    }
                }
            }

            return valid;
        }
    }

    public delegate int ItemAmountValidatorHandler(bool isQty, int utsPrc);

}
