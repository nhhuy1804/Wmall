using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using WSWD.WmallPos.POS.FX.Win;
using WSWD.WmallPos.FX.Shared.Utils;
using System.ComponentModel;
using WSWD.WmallPos.POS.FX.Win.Forms;

namespace WSWD.WmallPos.POS.SL.Controls
{
    /// <summary>
    /// 하단항목
    /// </summary>
    public class SummaryItemText : Label
    {
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
                return ContentAlignment.MiddleRight;
            }
        }

        public override System.Drawing.Font Font
        {
            get
            {                
                return new Font("돋움", 16, FontStyle.Bold);
            }
        }

        bool IsRefund
        {
            get
            {
                var fb = (FrameBase)this.FindForm();
                return fb != null ? fb.StateRefund : false;
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int val = TypeHelper.ToInt32(value);
                    base.Text = string.Format("{1}{0:#,##0}", val, IsRefund && val > 0 ? "-" : "");
                }
                else
                {
                    base.Text = value;
                }
            }
        }

        public override Color BackColor
        {
            get
            {
                return EvenPosition ? Color.White : "#f6f6f6".FromHtmlColor();
            }
        }

        [Category("Appearance"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Browsable(true)]
        public bool EvenPosition { get; set; }
    }
}
