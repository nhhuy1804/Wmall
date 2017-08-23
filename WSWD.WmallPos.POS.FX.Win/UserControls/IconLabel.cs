using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public class IconLabel : UserControl
    {
        private Label lblIcon;
        private Label lblContent;

        public IconLabel()
        {
            lblIcon = new Label();
            lblContent = new Label();
        }

        private string m_text = string.Empty;
        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Browsable(true), EditorBrowsable(EditorBrowsableState.Advanced)]
        public override string Text
        {
            get
            {
                return m_text;
            }
            set
            {
                m_text = value;
                if (lblContent != null)
                {
                    lblContent.Text = value;
                }
            }
        }

        private ContentAlignment m_textAlign = ContentAlignment.MiddleLeft;
        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Browsable(true), EditorBrowsable(EditorBrowsableState.Advanced)]
        public ContentAlignment TextAlign
        {
            get
            {
                return m_textAlign;
            }
            set
            {
                m_textAlign = value;
                if (lblContent != null)
                {
                    lblContent.TextAlign = value;
                }
            }
        }

        private Image m_icon = null;
        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Browsable(true), EditorBrowsable(EditorBrowsableState.Advanced)]
        public Image Icon
        {
            get
            {
                return m_icon;
            }
            set
            {
                m_icon = value;
                if (lblIcon != null)
                {
                    lblIcon.Image = value;
                }
            }
        }


        private IconLabelPosition m_iconPosition = IconLabelPosition.Left;
        public IconLabelPosition IconPosition
        {
            get
            {
                return m_iconPosition;
            }
            set
            {
                m_iconPosition = value;
                RenderControls();
            }
        }

        private void RenderControls()
        {
            this.SuspendLayout();
            lblContent.Dock = DockStyle.Fill;
            lblIcon.Image = this.Icon;
            lblIcon.Width = this.Icon != null ? this.Icon.Width + 2 : 20;
            if (m_iconPosition == IconLabelPosition.Left)
            {
                lblIcon.Dock = DockStyle.Left;

            }
            else
            {
                lblIcon.Dock = DockStyle.Right;
            }

            this.Controls.Add(lblContent);
            this.Controls.Add(lblIcon);
            this.ResumeLayout();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // IconLabel
            // 
            this.Name = "IconLabel";
            this.Size = new System.Drawing.Size(243, 51);
            this.ResumeLayout(false);

        }
    }

    public enum IconLabelPosition
    {
        Left,
        Right
    }
}
