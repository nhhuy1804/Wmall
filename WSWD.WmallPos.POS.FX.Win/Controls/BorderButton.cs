using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.ComponentModel;

namespace WSWD.WmallPos.POS.FX.Win.Controls
{
    public class BorderButton : Label
    {
        public BorderButton()
        {
            this.Resize += new EventHandler(RoundLabel_Resize);
            this.Paint += new PaintEventHandler(RoundLabel_Paint);
        }

        void RoundLabel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawPath(new Pen(new SolidBrush(this.BorderColor), 1), RoundedRectangle(e.ClipRectangle, m_corner, 1));

            // draw text
        }

        void RoundLabel_Resize(object sender, EventArgs e)
        {
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, m_corner, m_corner));
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
         );

        private int m_corner = 10;
        public int Corner
        {
            get
            {
                return m_corner;
            }
            set
            {
                m_corner = value;
                this.Invalidate();
            }
        }

        private Color m_borderColor = Color.FromArgb(108, 86, 197);
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "0x6c56c5")]
        public Color BorderColor
        {
            get
            {
                return m_borderColor;
            }
            set
            {
                m_borderColor = value;
                this.Invalidate();
            }
        }

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
    }
}
