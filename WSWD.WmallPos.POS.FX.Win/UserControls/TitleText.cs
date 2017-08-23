using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.POS.FX.Win.Controls;
using System.Drawing;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public class TitleText : RoundedLabel
    {
        public TitleText()
        {
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BorderWidth = new System.Windows.Forms.Padding(0);
            this.ForeColor = this.ColorProp("TitleText", "ForeColor", SystemColors.ControlText);
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
