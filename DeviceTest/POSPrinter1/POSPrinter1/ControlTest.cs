using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeyBoardTest
{
    public partial class ControlTest : Form
    {
        public ControlTest()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(ControlTest_KeyDown);
        }

        void ControlTest_KeyDown(object sender, KeyEventArgs e)
        {
            ultraLabel1.Text += (char)e.KeyCode;
        }

        void ParseTrackIIData(string trackIIData, out string cardNo, out string expYM)
        {
            trackIIData = trackIIData.Trim();
            cardNo = expYM = string.Empty;
            int idx = trackIIData.IndexOf("=");
            if (idx >= 0)
            {
                cardNo = trackIIData.Substring(0, idx);
                expYM = trackIIData.Substring(idx + 1, Math.Min(4, trackIIData.Length - cardNo.Length - 1));

                // reverse expYM                
                expYM = expYM.Length < 4 ? string.Empty : expYM.Substring(2, 2) + expYM.Substring(0, 2);
            }
            else
            {
                cardNo = trackIIData.Substring(0, Math.Max(19, trackIIData.Length));
            }
        }

        private void btnCardParse_Click(object sender, EventArgs e)
        {
            string cardNo = string.Empty;
            string expMY = string.Empty;
            ParseTrackIIData(textBox1.Text, out cardNo, out expMY);
            textBox2.Text = cardNo;
            textBox3.Text = expMY;
        }
    }
}
