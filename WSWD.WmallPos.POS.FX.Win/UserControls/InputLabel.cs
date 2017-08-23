using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using WSWD.WmallPos.POS.FX.Win.Controls;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public class InputLabel : Label
    {
        public InputLabel()
        {
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        public override Color ForeColor
        {
            get
            {
                return Color.FromArgb(69, 69, 69);
            }            
        }

        public override Font Font
        {
            get
            {
                return new Font("돋움", 10, FontStyle.Bold);                
            }
        }

        public override bool AutoSize
        {
            get
            {
                return false;
            }
        }
    }
}
