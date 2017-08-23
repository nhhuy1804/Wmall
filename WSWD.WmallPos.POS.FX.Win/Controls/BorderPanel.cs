using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace WSWD.WmallPos.POS.FX.Win.Controls
{
    public class BorderPanel : UserControl
    {
        [Category("Appearance")]
        [DefaultValue(1)]
        public virtual Padding BorderWidth { get; set; }

        [Category("Appearance")]
        public virtual Color BorderColor { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int border = this.BorderWidth.Left;
            border = Math.Max(this.BorderWidth.Top, border);
            border = Math.Max(this.BorderWidth.Bottom, border);
            border = Math.Max(this.BorderWidth.Right, border);

            Rectangle outerRect = new Rectangle(ClientRectangle.X + border / 2, ClientRectangle.Y + border / 2,
                    ClientRectangle.Width - border, ClientRectangle.Height - border);

            if (this.BorderWidth.Left > 0 && 
                this.BorderWidth.Top > 0 && 
                this.BorderWidth.Bottom > 0 && 
                this.BorderWidth.Right > 0)
            {
                using (var pen = new Pen(this.BorderColor, border))
                {
                    e.Graphics.DrawRectangle(pen, outerRect);
                }

                return;
            }

            var bw = this.BorderWidth;
            Pen p;
            // Draw Left
            if (bw.Left > 0)
            {
                p = new Pen(this.BorderColor, bw.Left);
                e.Graphics.DrawLine(p, new Point(outerRect.Left, outerRect.Top), 
                    new Point(outerRect.Left, outerRect.Height));
            }

            // Draw Top
            if (bw.Top > 0)
            {
                p = new Pen(this.BorderColor, bw.Top);
                e.Graphics.DrawLine(p, new Point(outerRect.Left, outerRect.Top), 
                    new Point(outerRect.Width, outerRect.Top));
            }

            // Draw Right
            if (bw.Right > 0)
            {
                p = new Pen(this.BorderColor, bw.Right);
                e.Graphics.DrawLine(p, new Point(outerRect.Width, outerRect.Top), 
                    new Point(outerRect.Width, outerRect.Height));
            }

            // Draw Bottom
            if (bw.Bottom > 0)
            {
                p = new Pen(this.BorderColor, bw.Bottom);
                e.Graphics.DrawLine(p, new Point(outerRect.Left, outerRect.Height),
                    new Point(outerRect.Width, outerRect.Height));
            }
        }
    }
}
