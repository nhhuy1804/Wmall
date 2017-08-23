using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PV;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;

namespace BasketTestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);
            this.Activated += new EventHandler(Form1_Activated);
            this.Deactivate += new EventHandler(Form1_Deactivate);

            this.axCtrlKeyboard1.ScannerEvent += new AxKeyBoardHook.__CtrlKeyboard_ScannerEventEventHandler(axCtrlKeyboard1_ScannerEvent);
            this.axCtrlKeyboard1.KeyboardEvent += new AxKeyBoardHook.__CtrlKeyboard_KeyboardEventEventHandler(axCtrlKeyboard1_KeyboardEvent);
        }

        void axCtrlKeyboard1_KeyboardEvent(object sender, AxKeyBoardHook.__CtrlKeyboard_KeyboardEventEvent e)
        {
            textBox2.Text += e.strData;
        }

        void Form1_Deactivate(object sender, EventArgs e)
        {
            //this.axCtrlKeyboard1.ScannerEvent -= new AxKeyBoardHook.__CtrlKeyboard_ScannerEventEventHandler(axCtrlKeyboard1_ScannerEvent);
            //this.axCtrlKeyboard1.EventEnable = false;
        }

        void Form1_Activated(object sender, EventArgs e)
        {
            //this.axCtrlKeyboard1.EventEnable = true;
        }

        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.axCtrlKeyboard1.ScannerEvent -= new AxKeyBoardHook.__CtrlKeyboard_ScannerEventEventHandler(axCtrlKeyboard1_ScannerEvent);
            this.axCtrlKeyboard1.EventEnable = false;
        }

        void axCtrlKeyboard1_ScannerEvent(object sender, AxKeyBoardHook.__CtrlKeyboard_ScannerEventEvent e)
        {
            textBox1.Text = e.strData;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //new Form1().ShowDialog(this);
            new MessageDialog().ShowDialog(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.FormBorderStyle = FormBorderStyle.None;
            TraceHelper.Instance.JournalWrite("SIGNON", "계산원 : 홍길동 1111");
            TraceHelper.Instance.JournalWrite("보류영수증", "계산원 : 홍길동 1111");

            TraceHelper.Instance.TraceWrite("보류영수증 TRACE MESSAGE");

            BasketPayCard card = new BasketPayCard();
            card.BalAmt = "12121";
            card.CardNm = "testing";

            var newCard = card;
        }

        static void CopyProperties(object dest, object src)
        {
            foreach (PropertyDescriptor item in TypeDescriptor.GetProperties(src))
            {
                item.SetValue(dest, item.GetValue(src));
            }
        }

        private void axCtrlKeyboard1_ErrorEvent(object sender, AxKeyBoardHook.__CtrlKeyboard_ErrorEventEvent e)
        {
            label1.Text = e.strData;
        }

        private void axCtrlKeyboard1_TraceLogEvent(object sender, AxKeyBoardHook.__CtrlKeyboard_TraceLogEventEvent e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var n = Math.Ceiling((double)(9990 / 10) * 10);
            MessageBox.Show(n.ToString());

            n = Math.Ceiling((double)(9999999990 / 10) * 10);
            MessageBox.Show(n.ToString());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string s = textBox1.Text.SubstringBytes(81, 30);
            MessageBox.Show(s);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var req = new PV01ReqData();
            textBox2.Text = req.ToString();

            int d = int.Parse("0D", System.Globalization.NumberStyles.HexNumber);
            MessageBox.Show(d.ToString());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var form = new FontTestForm();
            //form.FormBorderStyle = FormBorderStyle.None;
            form.ShowDialog(this);
        }
    }
}
