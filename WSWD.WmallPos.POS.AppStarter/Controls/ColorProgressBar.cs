using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.AppStarter.Controls
{
    public partial class ColorProgressBar : UserControl
    {
        private Color borderColor1 = Color.FromArgb(77, 46, 131);
        private Color backColor1 = Color.FromArgb(111, 83, 180);

        private Color borderColorDefault = Color.FromArgb(202, 202, 202);
        private Color backColorDefault = Color.FromArgb(246, 246, 246);

        public ColorProgressBar()
        {
            InitializeComponent();            
        }

        private int _Percentage = 0;

        public int Percentage
        {
            get { return _Percentage; }
            set
            {
                _Percentage = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {         
            Pen p;
            int width = Percentage * this.Width / 100;

            if (Percentage == 0)
            {
                p = new Pen(borderColorDefault, 1);
                e.Graphics.DrawLine(p, new Point(0, 0), new Point(0, this.Size.Height - 1));
                e.Graphics.DrawLine(p, new Point(0, 0), new Point(this.Size.Width - 1, 0));
                e.Graphics.DrawLine(p, new Point(this.Size.Width - 1, 0), new Point(this.Size.Width - 1, this.Size.Height - 1));
                e.Graphics.DrawLine(p, new Point(0, this.Size.Height - 1), new Point(this.Size.Width - 1, this.Size.Height - 1));

                // Fill with color
                Rectangle rect = new Rectangle(1, 1, this.Width - 2, this.Height - 2);
                e.Graphics.FillRectangle(new SolidBrush(this.backColorDefault), rect);
            }
            else if (Percentage < 100)
            {
                p = new Pen(borderColor1, 1);

                // vertical left
                e.Graphics.DrawLine(p, new Point(0, 0), new Point(0, this.Height - 1));
                e.Graphics.DrawLine(p, new Point(0, 0), new Point(width, 0));
                e.Graphics.DrawLine(p, new Point(0, this.Height - 1), new Point(width, this.Height - 1));

                Rectangle rect = new Rectangle(1, 1, width, this.Height - 2);
                e.Graphics.FillRectangle(new SolidBrush(this.backColor1), rect);

                p = new Pen(borderColorDefault, 1);
                e.Graphics.DrawLine(p, new Point(width, 0), new Point(this.Width - 1, 0));
                e.Graphics.DrawLine(p, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height - 1));
                e.Graphics.DrawLine(p, new Point(width, this.Height - 1), new Point(this.Width - 1, this.Height - 1));

                rect = new Rectangle(width + 1 , 1, this.Width - width - 2, this.Height - 2);
                e.Graphics.FillRectangle(new SolidBrush(this.backColorDefault), rect);
            }
            else
            {
                p = new Pen(borderColor1, 1);
                e.Graphics.DrawLine(p, new Point(0, 0), new Point(0, this.Size.Height - 1));
                e.Graphics.DrawLine(p, new Point(0, 0), new Point(this.Size.Width - 1, 0));
                e.Graphics.DrawLine(p, new Point(this.Size.Width - 1, 0), new Point(this.Size.Width - 1, this.Size.Height - 1));
                e.Graphics.DrawLine(p, new Point(0, this.Size.Height - 1), new Point(this.Size.Width - 1, this.Size.Height - 1));

                // Fill with color
                Rectangle rect = new Rectangle(1, 1, this.Width - 2, this.Height - 2);
                e.Graphics.FillRectangle(new SolidBrush(this.backColor1), rect);
            }


        }
    }
}
