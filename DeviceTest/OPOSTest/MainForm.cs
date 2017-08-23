using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using POS.Devices;

namespace OPOSTest
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnOpenCD_Click(object sender, EventArgs e)
        {
            OPOSCashDrawer cd = new OPOSCashDrawer();
            int nRC = cd.Open("DefaultCashDrawer");
            if (nRC == (int)OPOS_Constants.OPOS_SUCCESS)
            {
                cd.DeviceEnabled = true;
                nRC = cd.ClaimDevice(5000);
                if (nRC == (int)OPOS_Constants.OPOS_SUCCESS)
                {
                    cd.DeviceEnabled = true;
                    cd.OpenDrawer();
                }
            }
            else
            {
                MessageBox.Show("Open failed");
            }

            cd.ReleaseDevice();
            cd.Close();
        }

        private void btnLineDisplay_Click(object sender, EventArgs e)
        {
            OPOSLineDisplay ld = new OPOSLineDisplay();
            int nRC = ld.Open("DefaultPOSLineDisplay");
            if (nRC == (int)OPOS_Constants.OPOS_SUCCESS)
            {
                nRC = ld.ClaimDevice(5000);
                if (nRC == (int)OPOS_Constants.OPOS_SUCCESS)
                {
                    ld.DeviceEnabled = true;

                    ld.DisplayTextAt(0, 0, "WELCOME To W-MALL",
                        (int)POS.Devices.OPOSLineDisplayConstants.DISP_DT_NORMAL);

                    ld.DisplayTextAt(1, 0, "TOTAL" + "30,000".PadLeft(26 - "TOTAL".Length - 6, ' '),
                        (int)POS.Devices.OPOSLineDisplayConstants.DISP_DT_NORMAL);

                    ld.DeviceEnabled = false;
                    ld.ReleaseDevice();
                    ld.Close();
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            var Printer = new OPOSPOSPrinter();
            // Open the printer.
            int nRC = Printer.Open("DefaultPOSPrinter");
            // If succeeded, then claim.
            if (nRC == (int)OPOS_Constants.OPOS_SUCCESS)
            {
                nRC = Printer.ClaimDevice(1000);

                // If succeeded, then enable.
                if (nRC == (int)OPOS_Constants.OPOS_SUCCESS)
                {
                    Printer.DeviceEnabled = true;
                    nRC = Printer.ResultCode;
                }

                nRC = Printer.PrintNormal(2, "\x1B|cA\x1B|2COPOS POSPrinter\x1B|1C\nvia Microsoft.NET\n\n");
            }

            Printer.ReleaseDevice();
            nRC = Printer.Close();
        }

        private OPOSMSR msr;
        private void btnMSR_Click(object sender, EventArgs e)
        {
            msr = new OPOSMSR();
            int nRC = msr.Open("DefaultPOSMSR");
            if (nRC == (int)OPOS_Constants.OPOS_SUCCESS)
            {
                msr.TracksToRead = (int)OPOSMSRConstants.MSR_TR_2;

                nRC = msr.ClaimDevice(5000);

                msr.DeviceEnabled = true;
                msr.DataEventEnabled = true;
                msr.DataEvent += new _IOPOSMSREvents_DataEventEventHandler(msr_DataEvent);
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        void msr_DataEvent(int Status)
        {
            textBox1.Text = msr.Track2Data;
            msr.DataEventEnabled = true;
        }

        private void btnMSRClose_Click(object sender, EventArgs e)
        {
            msr.ReleaseDevice();
            msr.Close();
        }

        private void axCtrlKeyboard1_KeyboardEvent(object sender, AxKeyBoardHook.__CtrlKeyboard_KeyboardEventEvent e)
        {
            label1.Text = "Key: " + e.strData;
        }

        private void axCtrlKeyboard1_ScannerEvent(object sender, AxKeyBoardHook.__CtrlKeyboard_ScannerEventEvent e)
        {
            label2.Text = "Scanned: " + e.strData;
        }

        private void axCtrlKeyboard1_MsrEvent(object sender, AxKeyBoardHook.__CtrlKeyboard_MsrEventEvent e)
        {
            label3.Text = e.strTrack2;
        }
    }
}
