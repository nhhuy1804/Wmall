using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.POS.FX.Win;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Win.Devices;
using System.Diagnostics;
using WSWD.WmallPos.FX.Shared;

namespace WSWD.WmallPos.POS.TM.VC
{
    /// <summary>
    /// 장비테스트
    /// </summary>
    public partial class POS_TM_M002 : FormBase
    {
        public POS_TM_M002()
        {
            InitializeComponent();

            this.KeyEvent += new OPOSKeyEventHandler(POS_TM_M002_KeyEvent);
            this.Unload += new EventHandler(POS_TM_M002_Unload);

            POSDeviceManager.SignPad.Initialize(this.axKSNet_Dongle1);
            POSDeviceManager.SignPad.RFCardEvent += new POSDataEventHandler(SignPad_RFCardEvent);

            POSDeviceManager.Msr.DataEvent += new POSMsrDataEventHandler(Msr_DataEvent);
            POSDeviceManager.Scanner.DataEvent += new POSDataEventHandler(Scanner_DataEvent);
        }

        void SignPad_RFCardEvent(string msrData)
        {
            inputControl5.Text = msrData;
        }

        void POS_TM_M002_KeyEvent(OPOSKeyEventArgs e)
        {
            
        }

        void Msr_DataEvent(string msrData, string cardNo, string expMY)
        {
            inputControl4.Text = msrData;
        }

        void POS_TM_M002_Unload(object sender, EventArgs e)
        {
            POSDeviceManager.SignPad.Close();   
        }

        private void button3_Click(object sender, EventArgs e)
        {            
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {   
            POSDeviceManager.LineDisplay.DisplayText(inputControl2.Text, inputControl3.Text);            
        }
                
        private void button1_Click(object sender, EventArgs e)
        {
            POSDeviceManager.SignPad.RequestSign("싸인해주세요", "금액: 100,000");
        }

        private void POS_TM_M002_Load(object sender, EventArgs e)
        {
            label1.Text += string.Format("{0} {1}\n", POSDeviceManager.CashDrawer.Config.LogicalName, POSDeviceManager.CashDrawer.Status.ToString());
            label1.Text += string.Format("{0} {1}\n", POSDeviceManager.LineDisplay.Config.LogicalName, POSDeviceManager.LineDisplay.Status.ToString());
            label1.Text += string.Format("{0} {1}\n", POSDeviceManager.Msr.Config.LogicalName, POSDeviceManager.Msr.Status.ToString());
            label1.Text += string.Format("{0} {1}\n", POSDeviceManager.Printer.Config.LogicalName, POSDeviceManager.Printer.Status.ToString());
            label1.Text += string.Format("{0} {1}\n", POSDeviceManager.Scanner.Config.LogicalName, POSDeviceManager.Scanner.Status.ToString());

            inputPrinter.SetFocus();
        }

        void Scanner_DataEvent(string msrData)
        {
            inputControl1.Text = msrData;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!POSDeviceManager.SignPad.CloseSign())
            {
                if (ShowMessageBox(MessageDialogType.Question, "QS00001", null) == DialogResult.Yes)
                {
                    this.Close();
                }
                else
                {
                    POSDeviceManager.SignPad.RequestSign("싸인해주세요", "금액: 100,000");
                }
            }
            else
            {
                // show sign image
                Process.Start(new ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = POSDeviceManager.SignPad.TempSignFile
                }).Start();

                POSDeviceManager.SignPad.SaveSignData("22351111");
                this.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            POSDeviceManager.Printer.PrintNormal(inputPrinter.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            POSDeviceManager.SignPad.RequestRFCardRead("카드대기해주세요", "1212121", string.Empty,
                string.Empty);
        }

        private void btnCD_Click(object sender, EventArgs e)
        {
            POSDeviceManager.CashDrawer.OpenDrawer();
        }

    }
}
