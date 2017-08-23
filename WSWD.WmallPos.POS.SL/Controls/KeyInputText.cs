using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using WSWD.WmallPos.POS.FX.Win;
using WSWD.WmallPos.POS.FX.Win.UserControls;

namespace WSWD.WmallPos.POS.SL.Controls
{
    public class KeyInputText : InputText
    {
        public KeyInputText()
        {
            FocusedBorderWidth = 1;
        }

        public override Font Font
        {
            get
            {
                return new Font("돋움", 14, FontStyle.Bold);                
            }
        }
    }
}
