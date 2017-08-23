using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;


using WSWD.WmallPos.POS.FX.Win;
using WSWD.WmallPos.POS.FX.Win.Controls;

namespace WSWD.WmallPos.POS.BO.VC
{
    public class MenuButton : ImageButton
    {
        public MenuButton()
        {
            this.ButtonType = MenuButtonType.TypeNorm;
        }

        private MenuButtonType m_buttonType = MenuButtonType.TypeNorm;

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public MenuButtonType ButtonType
        {
            get
            {
                return m_buttonType;
            }
            set
            {
                m_buttonType = value;
                SetImageByType();
                Invalidate();
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                Invalidate();
            }
        }

        private void SetImageByType()
        {
            if (m_buttonType == MenuButtonType.TypeNorm)
            {
                this.NormImage = Properties.Resources.btn_main_01;
                this.PressedImage = Properties.Resources.btn_main_01_DN;
                this.DisaImage = Properties.Resources.btn_main_01_DA;
                this.ForeColor = Color.White;
            }
            else
            {
                this.NormImage = Properties.Resources.btn_main_02;
                this.PressedImage = Properties.Resources.btn_main_02_DN;
                this.DisaImage = Properties.Resources.btn_main_02_DA;
                this.ForeColor = this.ColorProp("TYPE01", "NormForeColor", "#5c3b98".FromHtmlColor());
            }
        }
    }

    public enum MenuButtonType
    {
        TypeNorm,
        TypeBack
    }
}
