using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace WSWD.WmallPos.POS.FX.Win.Controls
{
    public class ImageButton : Control
    {
        private bool m_isMouseDown = false;
        public ImageButton()
        {
            this.MouseDown += new MouseEventHandler(Button_MouseDown);
            this.MouseUp += new MouseEventHandler(Button_MouseUp);
            this.TextAlign = ContentAlignment.MiddleCenter;
        }


        #region Properties

        private ContentAlignment m_textAlign = ContentAlignment.MiddleCenter;
        [Bindable(true), DefaultValue(typeof(ContentAlignment), "MiddleCenter"), Category("Appearance")]
        public ContentAlignment TextAlign { get { return m_textAlign; } set { m_textAlign = value; } }

        /// <summary>
        /// 일반상태 이미지
        /// </summary>
        [Category("Appearance")]
        public Image NormImage { get; set; }

        /// <summary>
        /// 누른상태 이미지
        /// </summary>
        [Category("Appearance")]
        public Image PressedImage { get; set; }

        /// <summary>
        /// Disable image
        /// </summary>
        [Category("Appearance")]
        public Image DisaImage { get; set; }

        [Category("Appearance")]
        public Image Image { get; set; }

        private bool m_autoResize = true;
        [DefaultValue(true), Bindable(true)]
        public bool AutoResize
        {
            get
            {
                return m_autoResize;
            }
            set
            {
                m_autoResize = value;
                if (this.NormImage != null && value)
                {
                    this.Size = this.NormImage.Size;
                }
            }
        }

        #endregion

        #region privates
        
        void Button_MouseUp(object sender, MouseEventArgs e)
        {
            m_isMouseDown = false;
            Invalidate();
        }

        void Button_MouseDown(object sender, MouseEventArgs e)
        {
            m_isMouseDown = true;
            Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DisaImage == null)
            {
                this.DisaImage = this.NormImage;
            }

            Image img = this.Enabled ? this.NormImage : this.DisaImage;

            if (this.Enabled)
            {
                // paing image
                if (m_isMouseDown)
                {
                    img = this.PressedImage == null ? NormImage : PressedImage;
                }
            }
            
            var g = e.Graphics;
            if (img != null)
            {
                g.DrawImage(img, new Point(0, 0));
            }

            if (this.Image != null)
            {
                // draw center
                g.DrawImage(this.Image, new Point((this.Width - this.Image.Width) / 2,
                    (this.Height - this.Image.Height) / 2));
            }


            // Paint the text
            using (SolidBrush backgroundBrush = new SolidBrush(this.ForeColor))
            {
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                StringFormat drawFormat = new StringFormat();
                switch (this.TextAlign)
                {
                    case ContentAlignment.BottomCenter:
                        drawFormat.Alignment = StringAlignment.Center;
                        drawFormat.LineAlignment = StringAlignment.Far;
                        break;
                    case ContentAlignment.BottomLeft:
                        drawFormat.Alignment = StringAlignment.Near;
                        drawFormat.LineAlignment = StringAlignment.Far;
                        break;
                    case ContentAlignment.BottomRight:
                        drawFormat.Alignment = StringAlignment.Far;
                        drawFormat.LineAlignment = StringAlignment.Far;
                        break;
                    case ContentAlignment.MiddleCenter:
                        drawFormat.Alignment = StringAlignment.Center;
                        drawFormat.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.MiddleLeft:
                        drawFormat.Alignment = StringAlignment.Near;
                        drawFormat.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.MiddleRight:
                        drawFormat.Alignment = StringAlignment.Far;
                        drawFormat.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.TopCenter:
                        drawFormat.Alignment = StringAlignment.Center;
                        drawFormat.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.TopLeft:
                        drawFormat.Alignment = StringAlignment.Near;
                        drawFormat.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.TopRight:
                        drawFormat.Alignment = StringAlignment.Far;
                        drawFormat.LineAlignment = StringAlignment.Center;
                        break;
                    default:
                        break;
                }

                g.DrawString(this.Text, this.Font, backgroundBrush, new RectangleF(0F + this.Margin.Left + this.Padding.Left,
                    0F + this.Margin.Top + this.Padding.Top,
                    this.Width - this.Margin.Left - this.Margin.Right - this.Padding.Left - this.Padding.Right,
                    this.Height - this.Margin.Top - this.Margin.Bottom - this.Padding.Top - this.Padding.Bottom),
                    drawFormat);
            }
        }

        #endregion

    }
}
