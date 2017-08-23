using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using WSWD.WmallPos.FX.Shared;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public class CheckBox : Control
    {
        public CheckBox()
        {
            this.MaximumSize = new System.Drawing.Size(16, 16);
            this.Click += new EventHandler(WCheckBox_Click);
        }

        void WCheckBox_Click(object sender, EventArgs e)
        {
            this.Checked = !this.Checked;
        }

        private bool m_checked = false;
        [Category("Operation"), DefaultValue(false)]
        public bool Checked
        {
            get { return m_checked; }
            set
            {
                m_checked = value; 
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(this.Checked ? Properties.Resources.check_box_ON :
                Properties.Resources.check_box_OFF, new System.Drawing.Point(0, 0));
        }
    }
}
