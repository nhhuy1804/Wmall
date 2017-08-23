using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public partial class TopBarItem : UserControl
    {
        public TopBarItem()
        {
            InitializeComponent();

            this.DataLabel.ForeColor = Color.FromArgb(247, 239, 170);
            this.TitleLabel.ForeColor = Color.FromArgb(197, 191, 212);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HideTitle
        {
            get
            {
                return !this.TitleLabel.Visible;
            }
            set
            {
                this.TitleLabel.Visible = !value;
            }
        }

        /// <summary>
        /// Set title
        /// </summary>
        public string Title
        {
            get
            {
                return this.TitleLabel.Text;
            }
            set
            {
                this.TitleLabel.Text = value;
            }
        }

        public int TitleWidth
        {
            get
            {
                return TitleLabel.Width;
            }
            set
            {
                TitleLabel.Width = value;
                DataLabel.Width = this.Width - value;
            }
        }
    }
}
