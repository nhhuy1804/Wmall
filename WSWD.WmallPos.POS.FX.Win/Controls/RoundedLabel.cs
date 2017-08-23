using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Drawing.Text;

using WSWD.WmallPos.POS.FX.Win;

namespace WSWD.WmallPos.POS.FX.Win.Controls
{
    public class RoundedLabel : Label
    {
        public RoundedLabel()
        {
            this.SetStyle(
                   System.Windows.Forms.ControlStyles.UserPaint |
                   System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                   System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
                   true);   
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
        [DefaultValue(1)]
        public Padding BorderWidth { get; set; }

        [Category("Appearance")]
        public Color BorderColor { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            // Paint the outer rounded rectangle
            Rectangle outerRect = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            using (GraphicsPath outerPath = RoundedRectangle(outerRect, this.Corner, 0))
            {
                using (Pen outlinePen = new Pen(BorderColor))
                {
                    g.DrawPath(outlinePen, outerPath);
                }
            }
        }

        #region Utilities

        private GraphicsPath RoundedRectangle(Rectangle boundingRect, int cornerRadius, int margin)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddArc(boundingRect.X + margin, boundingRect.Y + margin, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddArc(boundingRect.X + boundingRect.Width - margin - cornerRadius * 2, boundingRect.Y + margin, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddArc(boundingRect.X + boundingRect.Width - margin - cornerRadius * 2, boundingRect.Y + boundingRect.Height - margin - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddArc(boundingRect.X + margin, boundingRect.Y + boundingRect.Height - margin - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.CloseFigure();
            return roundedRect;
        }

        #endregion

    }
}
