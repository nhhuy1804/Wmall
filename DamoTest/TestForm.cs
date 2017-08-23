using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DamoTest
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(GetEncString(textBox1.Text, 20));
        }

        private string GetEncString(string textValue, int length)
        {
            Encoding transferEnc = Encoding.GetEncoding(949);
            byte[] allData = Encoding.Default.GetBytes(textValue);

            var textData = transferEnc.GetString(allData);
            var encData = transferEnc.GetBytes(textData);

            byte[] data = new byte[length];
            for (int i = 0; i < length; i++)
            {
                data[i] = (byte)' ';
            }

            System.Buffer.BlockCopy(encData, 0, data, 0, encData.Length);
            return transferEnc.GetString(encData);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string cardNo = "12121****1212121212121";
            cardNo.ResetZero();
            MessageBox.Show(cardNo);
        }
    }


    public static class Extensions
    {
        static public void ResetZero(this string textData)
        {
            if (string.IsNullOrEmpty(textData))
            {
                return;
            }

            var tmpString = new string('0', textData.Length);
            textData = tmpString;

            tmpString = new string('F', textData.Length);
            textData = tmpString;

            tmpString = new string('0', textData.Length);
            textData = tmpString;
        }
    }
}
