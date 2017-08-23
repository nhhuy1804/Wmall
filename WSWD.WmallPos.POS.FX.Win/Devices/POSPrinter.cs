using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POS.Devices;
using System.IO;
using System.Diagnostics;

using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Exceptions;
using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.Data;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.POS.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.Helper;

namespace WSWD.WmallPos.POS.FX.Win.Devices
{
    public class POSPrinter : POSDeviceBase, IPrinterDevice
    {
        private string logoFile = Path.Combine(FXConsts.FOLDER_RESOURCE.GetFolder(), FXConsts.RESOURCE_FILE_LOGO);
        private OPOSPOSPrinter m_device = null;
        private string m_PrintSaleMode = (char)(0x1b) + "|2C";  //sale mode
        private string m_PrintBig = (char)(0x1b) + "|1C";       //Big mode
        private string m_PrintLogo = (char)(0x1b) + "|1B";      //Logo
        private string m_PrintBold = (char)(0x1b) + "|bC";           //Bold
        private string m_receiptName = string.Empty;

        public POSPrinter()
        {
            m_device = new OPOSPOSPrinter();
        }

        #region IPOSDevice Members

        public PosPrinter Config
        {
            get
            {
                return ConfigData.Current.DevConfig.Printer;
            }
        }


        public override bool UseYN
        {
            get
            {
                return "1".Equals(Config.Use);
            }
        }

        public override bool Enabled
        {
            get
            {
                return m_device.DeviceEnabled;
            }
            set
            {
                m_device.DeviceEnabled = value;
            }
        }

        public override DeviceStatus Open()
        {
            if (Status == DeviceStatus.Opened)
            {
                m_device.AsyncMode = true;
                return Status;
            }

            if (!UseYN)
            {
                throw new Exception("프린터 사용안함.");
            }

            this.Status = DeviceStatus.Closed;
            try
            {
                var rc = m_device.Open(Config.LogicalName);
                if (rc == (int)OPOS_Constants.OPOS_SUCCESS)
                {
                    rc = m_device.ClaimDevice(1000);
                    if (rc == (int)OPOS_Constants.OPOS_SUCCESS)
                    {
                        Status = DeviceStatus.Opened;
                        SetBitmap();
                    }
                    else
                    {
                        Status = DeviceStatus.InitError;
                    }
                }
                else
                {
                    this.Status = DeviceStatus.OpenError;
                }

            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                //throw new Exception("ER00004", ex);
            }

            base.Open();
            return this.Status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Close()
        {
            var rc = m_device.ReleaseDevice();
            if (rc == (int)OPOS_Constants.OPOS_SUCCESS)
            {
                rc = m_device.Close();
                if (rc == (int)OPOS_Constants.OPOS_SUCCESS)
                {
                    Status = DeviceStatus.Closed;
                }
            }

            return Status == DeviceStatus.Closed;
        }

        #endregion

        public POSPrinterStatus PrinterStatus
        {
            get
            {
                //전원
                if (m_device.ClearOutput() != (int)OPOS_Constants.OPOS_SUCCESS)
                    return POSPrinterStatus.PowerClose;

                //커버
                if (m_device.CoverOpen)
                    return POSPrinterStatus.CoverOpenned;

                //용지
                if (m_device.RecEmpty)
                    return POSPrinterStatus.PaperEmpty;

                return Status == DeviceStatus.Opened ? POSPrinterStatus.Openned : POSPrinterStatus.Closed;
            }
        }

        public void SetBitmap()
        {
            //1, PtrSReceipt, vData, PtrBmAsis, PtrBmCenter
            int res = m_device.SetBitmap(1, (int)OPOSPOSPrinterConstants.PTR_S_RECEIPT, logoFile,
                (int)OPOSPOSPrinterConstants.PTR_BM_ASIS, (int)OPOSPOSPrinterConstants.PTR_BC_CENTER);

            if (res != (int)OPOS_Constants.OPOS_SUCCESS)
            {
                Trace.WriteLine("SetBitmap error");
            }
        }

        /// <summary>
        /// 프린트시작
        /// </summary>
        /// <param name="receiptName">어떤 영수증 출력할지: 개설, 정산, 판매</param>
        public void StartPrint(string receiptName)
        {
            StartPrint(receiptName, 2);
        }

        public void StartPrint(string receiptName, int printOpt)
        {
            m_receiptName = receiptName;

            if (printOpt > 0)
            {
                m_device.TransactionPrint((int)OPOSPOSPrinterConstants.PTR_S_RECEIPT,
                    (int)OPOSPOSPrinterConstants.PTR_TP_TRANSACTION);
            }
        }

        public string PrintLogo()
        {
            return FXConsts.PRINT_LOGO;
        }

        /// <summary>
        /// Loc added 11.13
        /// DCC Offer 마지막 줄을 이미지로 인쇄
        /// 
        /// </summary>
        /// <param name="bmpFilePath"></param>
        public void PrintBitmap(string bmpFilePath)
        {
            m_device.AsyncMode = false;
            int res = m_device.PrintBitmap((int)OPOSPOSPrinterConstants.PTR_S_RECEIPT, bmpFilePath,
                (int)OPOSPOSPrinterConstants.PTR_BM_ASIS, (int)OPOSPOSPrinterConstants.PTR_BM_LEFT);

            if (res != (int)OPOS_Constants.OPOS_SUCCESS)
            {
                Trace.WriteLine("PrintBitmap error");
            }

            m_device.AsyncMode = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="printText"></param>
        /// <returns></returns>
        public int PrintNormal(string printText)
        {
            return PrintNormal(printText, 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="printText"></param>
        /// <param name="printJournal"></param>
        /// <returns></returns>
        public int PrintNormal(string printText, int printOpt)
        {
            if (printOpt == 0 || printOpt == 2)
            {
                string journalText = printText;
                journalText = journalText.Replace(FXConsts.PRINT_LOGO, string.Empty);
                journalText = journalText.Replace(FXConsts.PRINT_BOLDWIDE_DOUBLE, string.Empty);
                journalText = journalText.Replace(FXConsts.PRINT_BOLDWIDE_NORMAL, string.Empty);
                journalText = journalText.Replace(FXConsts.PRINT_CENTER, string.Empty);
                journalText = journalText.Replace(FXConsts.PRINT_HEIGHT_DOUBLE, string.Empty);
                journalText = journalText.Replace(FXConsts.PRINT_WH_DOUBLE, string.Empty);
                journalText = journalText.Replace(FXConsts.PRINT_NORMAL, string.Empty);

                string[] lines = journalText.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                foreach (var line in lines)
                {
                    TraceHelper.Instance.JournalWrite(m_receiptName, line);

                    //여전법 추가 0808(KSK)
                    m_receiptName = string.Empty;
                }
            }

            if (printOpt == 1 || printOpt == 2)
            {
                // REPLACE CONTROL CHARACTER
                printText = printText.Replace(FXConsts.PRINT_LOGO, FXConsts.PRINT_LOGO_CHAR);
                printText = printText.Replace(FXConsts.PRINT_BOLDWIDE_DOUBLE, FXConsts.PRINT_BOLDWIDE_DOUBLE_CHAR);
                printText = printText.Replace(FXConsts.PRINT_BOLDWIDE_NORMAL, FXConsts.PRINT_BOLDWIDE_NORMAL_CHAR);
                printText = printText.Replace(FXConsts.PRINT_CENTER, FXConsts.PRINT_CENTER_CHAR);

                // 정광호 추가(2015.06.26)
                printText = printText.Replace(FXConsts.PRINT_HEIGHT_DOUBLE, FXConsts.PRINT_HEIGHT_DOUBLE_CHAR); //세로확대
                printText = printText.Replace(FXConsts.PRINT_WH_DOUBLE, FXConsts.PRINT_WH_DOUBLE_CHAR);         //가로세로확대
                printText = printText.Replace(FXConsts.PRINT_NORMAL, FXConsts.PRINT_NORMAL_CHAR);               //일반

                m_device.RecLineChars = 44;
                m_device.RecLineSpacing = 30;
                m_device.PrintNormal((int)OPOSPOSPrinterConstants.PTR_S_RECEIPT, printText);

                //여전법 추가 0808(KSK)
                printText = "";
                printText = string.Empty;
            }

            return 0;
        }

        public int PrintNormal(string str, uint iTemp)
        {
            return 0;
        }

        public int PrintBarCode(string strBarCode)
        {
            return PrintBarCode(strBarCode, 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strBarCode"></param>
        /// <param name="printOpt"></param>
        /// <returns></returns>
        public int PrintBarCode(string strBarCode, int printOpt)
        {
            int result = 0;
            {
                int iStation = (int)OPOSPOSPrinterConstants.PTR_S_RECEIPT;
                int iSymbology = (int)OPOSPOSPrinterConstants.PTR_BCS_Codabar;
                int iHeight = Convert.ToInt32(Config.BarCodeHeight);
                int iWidth = Convert.ToInt32(Config.BarCodeWidth);
                int iAlignment = (int)OPOSPOSPrinterConstants.PTR_BC_CENTER;
                int iTextPosition = (int)OPOSPOSPrinterConstants.PTR_BC_TEXT_BELOW;
                result = m_device.PrintBarCode(iStation, strBarCode, iSymbology, iHeight, iWidth, iAlignment, iTextPosition);
            }

            if (printOpt == 0 || printOpt == 2)
            {
                TraceHelper.Instance.JournalWrite(m_receiptName, FXConsts.RECEIPT_BARCODE + strBarCode);
                //여전법 추가 0808(KSK)
                m_receiptName.ResetZero();
            }

            return result;
        }

        public void EndPrint()
        {
            EndPrint(2);
        }

        public void EndPrint(int printOpt)
        {
            m_receiptName = string.Empty;
            if (printOpt > 0)
            {
                m_device.CutPaper(90);
                m_device.TransactionPrint((int)OPOSPOSPrinterConstants.PTR_S_RECEIPT,
                    (int)OPOSPOSPrinterConstants.PTR_TP_NORMAL);
            }
        }

        #region IPrinterDevice Members


        public int PrintDefault(string printText)
        {
            //throw new NotImplementedException();
            return 0;
        }

        #endregion
    }

    public enum POSPrinterStatus
    {
        Openned,
        Closed,
        CoverOpenned,
        PaperEmpty,
        PowerClose
    }
}
