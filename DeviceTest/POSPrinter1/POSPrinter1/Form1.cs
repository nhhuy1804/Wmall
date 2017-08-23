using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace KeyBoardTest
{
    public partial class Form1 : Form
    {
        public static IntPtr ActiveHandle = IntPtr.Zero;

        public Form1()
        {
            InitializeComponent();

            this.Activated += new EventHandler(Form1_Activated);
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            label1.Text += e.KeyCode.ToString();
            label1.Text += "";
        }

        void Form1_Activated(object sender, EventArgs e)
        {
            ActiveHandle = this.Handle;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Form1().ShowDialog(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text.GetCommand();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new ProgressForm().Show(this);
        }
    }

    public static class StringHelper
    {
        /// <summary>
        /// LOAD SQL COMMAND TEXT FROM FILE
        /// </summary>
        /// <param name="commandId"></param>
        /// <returns></returns>
        static public string GetCommand(this string commandId)
        {
            string filePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),
                "test.sql"); //HttpContext.Current.Server.MapPath("~/App_Data/ServiceCommands.sql");
            string fileContent = File.ReadAllText(filePath);
            // \n:Start\n([^\n\/]+)\n:End
            // (?s)(?<=(?<!/+\s*)<{0}>\s+)(?!//).+\s(?=</{0}>)
            Match m = Regex.Match(fileContent, string.Format(@"(?s)(?<=(?<!/+\s*)<{0}>\s+)(?!//).+\s(?=</{0}>)", commandId));
            if (m.Success)
            {
                return m.Groups[0].Value;
            }

            return string.Empty;
        }
    }
}
