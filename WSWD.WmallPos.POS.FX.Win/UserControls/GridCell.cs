using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public class GridCell : UserControl
    {
        /// <summary>
        /// Parent GridRow element
        /// </summary>
        public GridRow Row { get; internal set; }

        /// <summary>
        /// Cell index in GridRow cells
        /// </summary>
        public int CellIndex
        {
            get
            {
                if (this.Parent != null)
                {
                    return this.Parent.Controls.GetChildIndex(this);
                }

                return -1;
            }
        }
    }
}
