using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;

using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.POS.FX.Shared.Data;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.FX.Win.Controls
{
    public class RoundedButton : Control
    {
        private bool m_isMouseDown = false;

        public RoundedButton()
        {
            this.SetStyle(
                System.Windows.Forms.ControlStyles.UserPaint |
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
                true);

            this.MouseDown += new MouseEventHandler(RoundedButton_MouseDown);
            this.MouseUp += new MouseEventHandler(RoundedButton_MouseUp);
            this.StateStyle = new RoundedButtonStateStyle();
            this.TextAlign = ContentAlignment.MiddleCenter;
        }

        private RoundedButtonStateStyle StateStyle = null;

        [Category("Appearance")]
        public int Corner
        {
            get
            {
                return this.StateStyle.Corner;
            }
            set
            {
                this.StateStyle.Corner = value;
            }
        }

        [Category("Appearance")]
        public int BorderSize
        {
            get
            {
                return this.StateStyle.BorderSize;
            }
            set
            {
                this.StateStyle.BorderSize = value;
            }
        }

        [Category("Operation")]
        public bool Selected
        {
            get
            {
                return this.StateStyle.Selected;
            }
            set
            {
                this.StateStyle.Selected = value;
                Invalidate();
            }
        }

        private bool m_isHighlight = false;
        [Category("Operation")]
        public bool IsHighlight
        {
            get
            {
                return m_isHighlight;
            }
            set
            {
                m_isHighlight = value;
                Invalidate();
            }
        }

        private ButtonTypes m_buttonType = ButtonTypes.Type01;

        [Category("Appearance"), DefaultValue(typeof(ButtonTypes), "Type01")]
        public ButtonTypes ButtonType
        {
            get
            {
                return m_buttonType;
            }
            set
            {
                m_buttonType = value;
                this.StateStyle = WinExtensions.GetButtonStateStyleByType(value);
                Invalidate();
            }
        }

        [Bindable(true),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Browsable(true)]
        public ContentAlignment TextAlign
        {
            get;
            set;
        }

        [Bindable(true),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public Image Image { get; set; }

        [Category("Appearance"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Browsable(true)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                if (!DesignMode)
                {
                    Invalidate();
                }
            }
        }

        void RoundedButton_MouseUp(object sender, MouseEventArgs e)
        {
            m_isMouseDown = false;
            Invalidate();
        }

        void RoundedButton_MouseDown(object sender, MouseEventArgs e)
        {
            m_isMouseDown = true;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            //base.OnPaint(pevent);
            //DateTimeUtils.PrintTime(string.Format("{0}-OnPaint Started", this.Name));

            Color topColor = this.StateStyle.NormTopColor;
            Color bottomColor = this.StateStyle.NormBottomColor;
            Color borderColor = this.StateStyle.NormBorderColor;
            Color foreColor = this.StateStyle.NormForeColor;

            if (!Enabled)
            {
                topColor = this.StateStyle.DisabledTopColor;
                bottomColor = this.StateStyle.DisabledBottomColor;
                borderColor = this.StateStyle.DisabledBorderColor;
                foreColor = this.StateStyle.DisabledForeColor;
            }
            else if (this.StateStyle.Selected)
            {
                topColor = this.StateStyle.SelectedTopColor;
                bottomColor = this.StateStyle.SelectedBottomColor;
                borderColor = this.StateStyle.SelectedBorderColor;
                foreColor = this.StateStyle.SelectedForeColor;
            }
            else if (m_isMouseDown)
            {
                topColor = this.StateStyle.PressedTopColor;
                bottomColor = this.StateStyle.PressedBottomColor;
                borderColor = this.StateStyle.PressedBorderColor;
                foreColor = this.StateStyle.PressedForeColor;
            }

            if (!m_isMouseDown && !this.Selected && IsHighlight)
            {
                topColor = Color.FromArgb(168, 140, 215);
            }

            Graphics g = pevent.Graphics;
            this.ForeColor = foreColor;

            // Paint the outer rounded rectangle
            Rectangle outerRect = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);

            if (this.StateStyle.Corner == 0)
            {
                using (LinearGradientBrush outerBrush = new LinearGradientBrush(outerRect, topColor, bottomColor,
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(outerBrush, outerRect);
                }

                using (Pen outlinePen = new Pen(borderColor))
                {
                    g.DrawRectangle(outlinePen, outerRect);
                }
            }
            else
            {
                using (GraphicsPath outerPath = RoundedRectangle(outerRect, this.StateStyle.Corner, 0))
                {
                    using (LinearGradientBrush outerBrush = new LinearGradientBrush(outerRect, topColor, bottomColor,
                        LinearGradientMode.Vertical))
                    {
                        g.FillPath(outerBrush, outerPath);
                    }

                    using (Pen outlinePen = new Pen(borderColor))
                    {
                        g.DrawPath(outlinePen, outerPath);
                    }
                }
            }


            // draw image in center
            if (this.Image != null)
            {
                var rect = new Rectangle()
                {
                    X = (this.ClientSize.Width - this.Image.Width) / 2,
                    Y = (this.ClientSize.Height - this.Image.Height) / 2,
                    Width = this.Image.Width,
                    Height = this.Image.Height
                };
                g.DrawImage(this.Image, rect);
            }
            //// Paint the text
            using (SolidBrush backgroundBrush = new SolidBrush(foreColor))
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

            //DateTimeUtils.PrintTime(string.Format("{0}-OnPaint Ended", this.Name));

            //string tmp = this.Text;
            //base.Text = string.Empty;
            //base.OnPaint(pevent);
            //base.Text = tmp;
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
