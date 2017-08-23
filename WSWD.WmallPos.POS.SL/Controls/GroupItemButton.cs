using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using WSWD.WmallPos.POS.FX.Win;

namespace WSWD.WmallPos.POS.SL.Controls
{
    public class GroupItemButton : WSWD.WmallPos.POS.FX.Win.UserControls.Button
    {
        public override System.Drawing.Font Font
        {
            get
            {
                return new Font("돋움", 10, FontStyle.Bold);
            }
        }
    }
}
