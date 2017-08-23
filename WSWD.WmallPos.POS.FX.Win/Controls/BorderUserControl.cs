using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.FX.Win.Controls
{
    public partial class BorderUserControl : UserControl
    {
        public BorderUserControl()
        {
            InitializeComponent();

            this.SetStyle(
                System.Windows.Forms.ControlStyles.UserPaint |
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
                true);
        }

        #region Border

        [Category("Appearance")]
        [DefaultValue(1)]
        public Padding BorderWidth { get; set; }

        [Category("Appearance")]
        public Color BorderColor { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Pen p;
            // Draw Left
            if (this.BorderWidth.Left > 0)
            {
                p = new Pen(this.BorderColor, this.BorderWidth.Left);
                e.Graphics.DrawLine(p, new Point(0, 0), new Point(0, this.Size.Height - 1));
            }

            // Draw Top
            if (this.BorderWidth.Top > 0)
            {
                p = new Pen(this.BorderColor, this.BorderWidth.Top);
                e.Graphics.DrawLine(p, new Point(0, 0), new Point(this.Size.Width - 1, 0));
            }

            // Draw Right
            if (this.BorderWidth.Right > 0)
            {
                p = new Pen(this.BorderColor, this.BorderWidth.Right);
                e.Graphics.DrawLine(p, new Point(this.Size.Width - 1, 0), new Point(this.Size.Width - 1, this.Size.Height - 1));
            }

            // Draw Bottom
            if (this.BorderWidth.Bottom > 0)
            {
                p = new Pen(this.BorderColor, this.BorderWidth.Bottom);
                e.Graphics.DrawLine(p, new Point(0, this.Size.Height - 1), new Point(this.Size.Width - 1, this.Size.Height - 1));
            }
        }

        #endregion
    }
}
