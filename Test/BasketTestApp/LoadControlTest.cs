using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using WSWD.WmallPos.POS.TM.VC;

namespace BasketTestApp
{
    public partial class LoadControlTest : Form
    {
        public LoadControlTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            POS_TM_M003 uc = new POS_TM_M003();
            uc.Dock = DockStyle.Fill;
            this.pnlContainer.Controls.Add(uc);
            this.pnlContainer.Controls[this.pnlContainer.Controls.Count - 1].BringToFront();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            POS_TM_M004 uc = new POS_TM_M004();
            uc.Dock = DockStyle.Fill;
            this.pnlContainer.Controls.Add(uc);
            this.pnlContainer.Controls[this.pnlContainer.Controls.Count - 1].BringToFront();
        }
    }
}
