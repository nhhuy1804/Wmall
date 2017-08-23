using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;

namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    public partial class PopupBase : InputDialogBase
    {
        public PopupBase()
        {
            InitializeComponent();
            this.Load += new EventHandler(PopupBase_Load);
            this.FormClosed += new FormClosedEventHandler(PopupBase_FormClosed);
        }

        void PopupBase_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Load -= new EventHandler(PopupBase_Load);
            this.FormClosed -= new FormClosedEventHandler(PopupBase_FormClosed);
        }

        void PopupBase_Load(object sender, EventArgs e)
        {
            if (this.Owner != null)
            {
                this.Left = this.Owner.Left + (this.Owner.Width - this.Width) / 2;
                this.Top = this.Owner.Top + (this.Owner.Height - this.Height) / 2;
            }
        }

        #region Properties

        /// <summary>
        /// Show status message
        /// </summary>
        public virtual string StatusMessage { get; set; }

        #endregion
    }
}
