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
    public partial class SaleGridCell : UserControl
    {
        public SaleGridCell()
        {
            InitializeComponent();
        }

        public SaleGridRow Row
        {
            get
            {
                return this.Parent as SaleGridRow;
            }
        }

        public override Color BackColor
        {
            get
            {
                return Color.Transparent;// this.Parent == null ? Color.Empty : this.Parent.BackColor;
            }
        }

        public int ColumnIndex
        {
            get
            {
                return this.Parent.Controls.GetChildIndex(this);
            }
        }
    }
}
