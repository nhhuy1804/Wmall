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
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
            this.Activated += new EventHandler(ProgressForm_Activated);
        }

        void ProgressForm_Activated(object sender, EventArgs e)
        {
            Form1.ActiveHandle = this.Handle;
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.Focus();
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            this.Focus();
        }

        void ProgressForm_Deactivate(object sender, EventArgs e)
        {
            this.Activate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
