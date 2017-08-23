using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSWD.WmallPos.POS.FX.Win.Forms;

namespace WSWD.WmallPos.POS.TM.VC
{
    public partial class POS_TM_P002 : PopupBase02
    {
        public POS_TM_P002()
        {
            InitializeComponent();
        }

        private void roundedButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
