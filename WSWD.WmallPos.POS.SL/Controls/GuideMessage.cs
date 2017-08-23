using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.POS.FX.Win;

namespace WSWD.WmallPos.POS.SL.Controls
{
    public class GuideMessage : Label
    {
        public override System.Drawing.Font Font
        {
            get
            {
                return new Font("돋움", 14, FontStyle.Bold);
            }
        }

        public override bool AutoSize
        {
            get
            {
                return false;
            }
        }

        public override ContentAlignment TextAlign
        {
            get
            {
                return ContentAlignment.MiddleLeft;
            }
        }

        public override Color ForeColor
        {
            get
            {
                return base.Text.StartsWith("ER_") ? Color.Red : Color.FromArgb(51, 51, 51);
            }
        }

        /// <summary>
        /// Error message
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text.StartsWith("ER_") ? base.Text.Substring(3) : base.Text;
            }
            set
            {
                base.Text = value;
            }
        }
    }
}
