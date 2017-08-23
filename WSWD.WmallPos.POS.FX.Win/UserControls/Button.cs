using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.POS.FX.Win.Controls;
using System.Drawing;
using System.ComponentModel;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public class Button : RoundedButton
    {
        public Button()
        {
            this.Height = 42;
            this.Width = 90;
        }

        public override Font Font
        {
            get
            {
                return new Font("돋움", 11, FontStyle.Bold);                
            }
        }

        [DefaultValue(typeof(KeyButtonTypes), "Normal")]
        public KeyButtonTypes KeyType { get; set; }

        public void FireClick()
        {
            this.OnClick(EventArgs.Empty);
        }
    }
}
