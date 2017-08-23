using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.POS.FX.Win.Controls;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public class MessageBar : Control
    {
        public MessageBar()
        {
            this.SetStyle(
                System.Windows.Forms.ControlStyles.UserPaint |
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
                true);

            this.TextAlign = ContentAlignment.MiddleLeft;
            this.BorderWidth = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.ControlType = MessageBarType.TypeStatus;
            this.Corner = 3;
        }

        [Category("Appearance"), DefaultValue(3)]
        public int Corner { get; set; }

        [Category("Appearance")]
        [DefaultValue(1)]
        public Padding BorderWidth { get; set; }

        [Category("Appearance")]
        public Color BorderColor { get; set; }

        [Category("Appearance")]
        public ContentAlignment TextAlign { get; set; }

        private MessageBarType m_controlType = MessageBarType.TypeStatus;
        [Category("Appearance")]
        public MessageBarType ControlType
        {
            get { return m_controlType; }
            set
            {
                m_controlType = value;
            
                this.MinimumSize = new Size(0, 35);
                this.BackColor = Color.FromArgb(255, 253, 212);
                this.BorderColor = Color.FromArgb(202, 202, 202);
                this.ForeColor = Color.FromArgb(51, 51, 51);
            
                this.ForeColor = value == MessageBarType.TypeStatus ?
                    Color.FromArgb(51, 51, 51) : Color.FromArgb(215, 58, 58);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
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

            // Paint the highlight rounded rectangle
            /*
            Rectangle innerRect = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height / 2 - 1);
            using (GraphicsPath innerPath = RoundedRectangle(innerRect, this.StateStyle.Corner, 0))
            {
                using (LinearGradientBrush innerBrush = new LinearGradientBrush(innerRect, Color.FromArgb(255, Color.White),
                    Color.FromArgb(0, Color.White), LinearGradientMode.Vertical))
                {
                    g.FillPath(innerBrush, innerPath);
                }
            }

             * */
            // Paint the text
            //TextRenderer.DrawText(g, this.Text, this.Font, outerRect, this.ForeColor, Color.Transparent,
            //    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            // Paint the text

            using (SolidBrush backgroundBrush = new SolidBrush(ForeColor))
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

        /// <summary>
        /// 메시지
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        base.Text = value;
                    });
                }
                else
                {
                    base.Text = value;
                }
                Invalidate();
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
