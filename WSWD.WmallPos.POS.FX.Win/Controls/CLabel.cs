using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace WSWD.WmallPos.POS.FX.Win.Controls
{
    public partial class CLabel : Label
    {
        public CLabel()
        {
            this.TextAlign = ContentAlignment.MiddleLeft;
            this.BorderStyle = BorderStyle.None;
        }

        private int m_corner = 5;

        [Category("Appearance")]
        public int Corner
        {
            get
            {
                return m_corner;
            }
            set
            {
                m_corner = value;
            }
        }

        [Category("Appearance")]
        public bool HasBorder { get; set; }

        [Category("Appearance")]
        public Color BorderColor { get; set; }

        [Category("Appearance")]
        public int BorderWidth { get; set; }

        public override ContentAlignment TextAlign
        {
            get
            {
                return base.TextAlign;
            }
            set
            {
                base.TextAlign = value;
                if (value.ToString().Contains("Right"))
                {
                    this.Padding = new Padding(this.Padding.Left, this.Padding.Top, 5, this.Padding.Bottom);
                }
                else if (value.ToString().Contains("Left"))
                {
                    this.Padding = new Padding(4, this.Padding.Top, this.Padding.Right, this.Padding.Bottom);
                }
                else if (value.ToString().Contains("Center"))
                {
                    this.Padding = new Padding(0, this.Padding.Top, this.Padding.Right, this.Padding.Bottom);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // Paint the outer rounded rectangle
            if (HasBorder)
            {
                Rectangle outerRect = new Rectangle(ClientRectangle.X + BorderWidth / 2, ClientRectangle.Y + BorderWidth / 2,
                    ClientRectangle.Width - BorderWidth, ClientRectangle.Height - BorderWidth);
                //using (GraphicsPath outerPath = RoundedRectangle(outerRect, this.Corner, BorderWidth))
                //{
                //    using (Pen outlinePen = new Pen(BorderColor, BorderWidth))
                //    {
                //        g.DrawPath(outlinePen, outerPath);
                //    }
                //}                
                using (Pen outlinePen = new Pen(BorderColor, BorderWidth))
                {
                    g.DrawRectangle(outlinePen, outerRect);
                }
            }

        }

        private GraphicsPath RoundedRectangle(Rectangle boundingRect, int cornerRadius, int margin)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddArc(boundingRect.X + margin, boundingRect.Y + margin, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddArc(boundingRect.X + boundingRect.Width - margin - cornerRadius * 2,
                boundingRect.Y + margin, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddArc(boundingRect.X + boundingRect.Width - margin - cornerRadius * 2,
                boundingRect.Y + boundingRect.Height - margin - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddArc(boundingRect.X + margin, boundingRect.Y + boundingRect.Height - margin - cornerRadius * 2,
                cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.CloseFigure();
            return roundedRect;
        }
    }
}
