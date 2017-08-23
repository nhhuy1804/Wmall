using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace FindWindowTest
{
    public partial class Form1 : Form
    {

        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var t = sender.GetType();
            var typeName = t.Name;
            MessageBox.Show(typeName);

            var p = t.GetProperty("Name");
            var name = p.GetValue(sender, null).ToString();
            //var btn = (Button)sender;
            MessageBox.Show(name);

            // retrieve the handler of the window  
            int iHandle = FindWindow(null, textBox1.Text);
            if (iHandle > 0)
            {
                MessageBox.Show("찾았다");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string cardNo = "12121****1212121212121";
            cardNo.Clear();
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

        public unsafe static void Clear(this string s)
        {
            fixed (char* ptr = s)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    ptr[i] = '0';
                }
            }

            fixed (char* ptr = s)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    ptr[i] = 'F';
                }
            }

            fixed (char* ptr = s)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    ptr[i] = '0';
                }
            }
        }
    }


}
