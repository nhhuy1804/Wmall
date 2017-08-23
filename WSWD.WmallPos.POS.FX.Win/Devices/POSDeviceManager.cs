using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.FX.Win.Devices
{
    public delegate void POSDeviceInitializeEventHandler(POSDeviceTypes deviceType, bool initializeCompleted);

    /// <summary>
    /// 포스장비 전체관리자
    /// </summary>
    public class POSDeviceManager
    {
        public static POSCashDrawer CashDrawer;
        public static POSLineDisplay LineDisplay;
        public static POSMsr Msr;
        public static POSPrinter Printer;
        public static POSScannerGun Scanner;
        public static POSSignPad SignPad;

        private static POSDeviceInitializeEventHandler s_initializeHandler;

        /// <summary>
        /// POSDevice 초기화
        /// </summary>
        /// <param name="initializeHandler"></param>
        public static void Initialize(POSDeviceInitializeEventHandler initializeHandler)
        {
            s_initializeHandler = initializeHandler;

            // 순서대로 오픈한다
            s_initializeHandler(POSDeviceTypes.CashDrawer, false);
            CashDrawer = new POSCashDrawer();
            CashDrawer.OnOpened += new EventHandler(CashDrawer_OnOpened);
            CashDrawer.Open();
        }

        #region Device Opened Complete Event

        static void CashDrawer_OnOpened(object sender, EventArgs e)
        {
            CashDrawer.OnOpened -= new EventHandler(CashDrawer_OnOpened);

            s_initializeHandler(POSDeviceTypes.CashDrawer, true);

            s_initializeHandler(POSDeviceTypes.LineDisplay, false);
            LineDisplay = new POSLineDisplay();
            LineDisplay.OnOpened += new EventHandler(LineDisplay_OnOpened);
            LineDisplay.Open();
        }

        static void LineDisplay_OnOpened(object sender, EventArgs e)
        {
            LineDisplay.OnOpened -= new EventHandler(LineDisplay_OnOpened);
            s_initializeHandler(POSDeviceTypes.LineDisplay, true);

            // 주석처리 Msr 비활성화 05.24
            s_initializeHandler(POSDeviceTypes.Msr, true);
            //Msr = new POSMsr();
            //Msr.OnOpened += new EventHandler(Msr_OnOpened);
            //Msr.Open();

            s_initializeHandler(POSDeviceTypes.Printer, false);
            Printer = new POSPrinter();
            Printer.OnOpened += new EventHandler(Printer_OnOpened);
            Printer.Open();
        }

        /// <summary>
        /// 사용안함
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Msr_OnOpened(object sender, EventArgs e)
        {
            Msr.OnOpened -= new EventHandler(Msr_OnOpened);
            s_initializeHandler(POSDeviceTypes.Msr, true);

            s_initializeHandler(POSDeviceTypes.Printer, false);
            Printer = new POSPrinter();
            Printer.OnOpened += new EventHandler(Printer_OnOpened);
            Printer.Open();
        }

        static void Printer_OnOpened(object sender, EventArgs e)
        {
            Printer.OnOpened -= new EventHandler(Printer_OnOpened);

            s_initializeHandler(POSDeviceTypes.Printer, true);
            POSPrinterUtils.SetPrinterUtilsInstance(new POSPrinterUtils(POSDeviceManager.Printer));

            s_initializeHandler(POSDeviceTypes.ScannerGun, false);
            Scanner = new POSScannerGun();
            Scanner.OnOpened += new EventHandler(Scanner_OnOpened);
            Scanner.Open();
        }

        static void Scanner_OnOpened(object sender, EventArgs e)
        {
            Scanner.OnOpened -= new EventHandler(Scanner_OnOpened);

            s_initializeHandler(POSDeviceTypes.ScannerGun, true);

            // signpad 생성
            SignPad = new POSSignPad();

            // 키보드
            s_initializeHandler(POSDeviceTypes.Keyboard, true);
            s_initializeHandler = null;
        }
        #endregion


        /// <summary>
        /// Initialize w/o keyboard
        /// </summary>
        public static void Initialize()
        {
            CashDrawer = new POSCashDrawer();
            CashDrawer.Open();

            LineDisplay = new POSLineDisplay();
            LineDisplay.Open();

            Printer = new POSPrinter();
            Printer.Open();

            SignPad = new POSSignPad();
        }

        public static void Terminate()
        {
            if (CashDrawer != null) CashDrawer.Close();
            if (LineDisplay != null) LineDisplay.Close();
            if (Msr != null) Msr.Close();
            if (Printer != null) Printer.Close();
            if (Scanner != null) Scanner.Close();
            if (SignPad != null) SignPad.Close();
        }
    }
}
