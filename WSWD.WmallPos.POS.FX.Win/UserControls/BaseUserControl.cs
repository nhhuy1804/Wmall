using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSWD.WmallPos.POS.FX.Win.Forms;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public partial class BaseUserControl : UserControl
    {
        public BaseUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Parent key input form
        /// </summary>
        protected KeyInputForm KeyListener
        {
            get
            {
                return this.ParentForm == null ? null : (KeyInputForm)this.ParentForm;
            }
        }

    }
}
