using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public class SectionLabel : IconLabel
    {
        public SectionLabel()
        {
            this.Icon = Properties.Resources.bullet;
        }

        public override Font Font
        {
            get
            {
                return new Font("돋움", 12, FontStyle.Bold);               
            }
        }
    }
}
