using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;

using WSWD.WmallPos.POS.FX.Win;

namespace WSWD.WmallPos.POS.FX.Win.Controls
{
    public class RoundedButtonStateStyle
    {
        public RoundedButtonStateStyle()
        {
            this.Corner = 3;
            this.Selected = false;
            this.BorderSize = 1;
        }

        private bool _Selected = false;
        public bool Selected { get { return _Selected; } set { _Selected = value; } }

        private int _BorderSize = 1;
        public int BorderSize { get { return _BorderSize; } set { _BorderSize = value; } }

        private int _Corner = 3;
        public int Corner { get { return _Corner; } set { _Corner = value; } }

        private Color _NormTopColor = "#ffffff".FromHtmlColor();
        [DefaultValue(typeof(Color), "#ffffff")]
        public Color NormTopColor { get { return _NormTopColor; } set { _NormTopColor = value; } }

        private Color _NormBottomColor = "#e9e5fe".FromHtmlColor();
        [DefaultValue(typeof(Color), "#e9e5fe")]
        public Color NormBottomColor { get { return _NormBottomColor; } set { _NormBottomColor = value; } }

        private Color _NormBorderColor = "#6b56c3".FromHtmlColor();
        [DefaultValue(typeof(Color), "#6b56c3")]
        public Color NormBorderColor { get { return _NormBorderColor; } set { _NormBorderColor = value; } }

        private Color _NormForeColor = "#5c3b98".FromHtmlColor();
        [DefaultValue(typeof(Color), "#5c3b98")]
        public Color NormForeColor { get { return _NormForeColor; } set { _NormForeColor = value; } }


        private Color _DisabledTopColor = "#ffffff".FromHtmlColor();
        [DefaultValue(typeof(Color), "#ffffff")]
        public Color DisabledTopColor { get { return _DisabledTopColor; } set { _DisabledTopColor = value; } }

        private Color _DisabledBottomColor = "#f4f3fb".FromHtmlColor();
        [DefaultValue(typeof(Color), "#f4f3fb")]
        public Color DisabledBottomColor { get { return _DisabledBottomColor; } set { _DisabledBottomColor = value; } }

        private Color _DisabledBorderColor = "#aea7ce".FromHtmlColor();
        [DefaultValue(typeof(Color), "#aea7ce")]
        public Color DisabledBorderColor { get { return _DisabledBorderColor; } set { _DisabledBorderColor = value; } }

        private Color _DisabledForeColor = "#a094b6".FromHtmlColor();
        [DefaultValue(typeof(Color), "#a094b6")]
        public Color DisabledForeColor { get { return _DisabledForeColor; } set { _DisabledForeColor = value; } }



        private Color _PressedTopColor = "#afa5de".FromHtmlColor();
        [DefaultValue(typeof(Color), "#afa5de")]
        public Color PressedTopColor { get { return _PressedTopColor; } set { _PressedTopColor = value; } }

        private Color _PressedBottomColor = "#faf9ff".FromHtmlColor();
        [DefaultValue(typeof(Color), "#faf9ff")]
        public Color PressedBottomColor { get { return _PressedBottomColor; } set { _PressedBottomColor = value; } }

        private Color _PressedBorderColor = "#5c3b98".FromHtmlColor();
        [DefaultValue(typeof(Color), "#5c3b98")]
        public Color PressedBorderColor { get { return _PressedBorderColor; } set { _PressedBorderColor = value; } }

        private Color _PressedForeColor = "#4d2e83".FromHtmlColor();
        [DefaultValue(typeof(Color), "#4d2e83")]
        public Color PressedForeColor { get { return _PressedForeColor; } set { _PressedForeColor = value; } }


        private Color _SelectedTopColor = "#542f95".FromHtmlColor();
        [DefaultValue(typeof(Color), "#542f95")]
        public Color SelectedTopColor { get { return _SelectedTopColor; } set { _SelectedTopColor = value; } }

        private Color _SelectedBottomColor = "#7353b4".FromHtmlColor();
        [DefaultValue(typeof(Color), "#7353b4")]
        public Color SelectedBottomColor { get { return _SelectedBottomColor; } set { _SelectedBottomColor = value; } }

        private Color _SelectedBorderColor = "#3d276f".FromHtmlColor();
        [DefaultValue(typeof(Color), "#3d276f")]
        public Color SelectedBorderColor { get { return _SelectedBorderColor; } set { _SelectedBorderColor = value; } }

        private Color _SelectedForeColor = "#ffffff".FromHtmlColor();
        [DefaultValue(typeof(Color), "#ffffff")]
        public Color SelectedForeColor { get { return _SelectedForeColor; } set { _SelectedForeColor = value; } }
    }
}
