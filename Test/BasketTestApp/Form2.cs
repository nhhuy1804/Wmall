using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BasketTestApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            //this.axCtrlKeyboard1.ScannerEvent += new AxKeyBoardHook.__CtrlKeyboard_ScannerEventEventHandler(axCtrlKeyboard1_ScannerEvent);
            this.FormClosed += new FormClosedEventHandler(Form2_FormClosed);
        }

        void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            // axCtrlKeyboard1.EventEnable = false;
        }

        void axCtrlKeyboard1_ScannerEvent(object sender, AxKeyBoardHook.__CtrlKeyboard_ScannerEventEvent e)
        {
            textBox1.Text = e.strData;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
