using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Data;
using WSWD.WmallPos.POS.FX.Shared;
using System.Diagnostics;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public partial class InputControl : UserControl, IFocusableControl
    {
        public InputControl()
        {
            InitializeComponent();
            this.TextControl.ReadOnly = false;
            this.TextControl.IsFocused = false;
            this.InputType = InputControlType.Editable;
            this.TextControl.InputFocused += new EventHandler(TextControl_InputFocused);
        }

        #region Override

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Pen p;
            // Draw Left
            if (this.BorderWidth.Left > 0)
            {
                p = new Pen(this.BorderColor, this.BorderWidth.Left);
                e.Graphics.DrawLine(p, new Point(0, 0), new Point(0, this.Size.Height - 1));
            }

            // Draw Top
            if (this.BorderWidth.Top > 0)
            {
                p = new Pen(this.BorderColor, this.BorderWidth.Top);
                e.Graphics.DrawLine(p, new Point(0, 0), new Point(this.Size.Width - 1, 0));
            }

            // Draw Right
            if (this.BorderWidth.Right > 0)
            {
                p = new Pen(this.BorderColor, this.BorderWidth.Right);
                e.Graphics.DrawLine(p, new Point(this.Size.Width - 1, 0), new Point(this.Size.Width - 1, this.Size.Height - 1));
            }

            // Draw Bottom
            if (this.BorderWidth.Bottom > 0)
            {
                p = new Pen(this.BorderColor, this.BorderWidth.Bottom);
                e.Graphics.DrawLine(p, new Point(0, this.Size.Height - 1), new Point(this.Size.Width - 1, this.Size.Height - 1));
            }
        }

        private void RendenControlByType()
        {
            this.SuspendLayout();
            //this.BorderColor = Color.LightGray;
            this.MinimumSize = new Size(this.Width, 34);
            this.BorderWidth = new Padding(0);
            TextControl.PasswordMode = false;

            switch (m_inputType)
            {
                case InputControlType.ReadOnly:
                    TextControl.ReadOnly = true;
                    break;
                case InputControlType.Editable:
                    TextControl.ReadOnly = false;
                    break;
                case InputControlType.Password:
                    TextControl.ReadOnly = false;
                    TextControl.PasswordMode = true;
                    break;
                case InputControlType.SubTotal:
                    TextControl.ReadOnly = false;
                    this.BackColor = Color.FromArgb(242, 210, 211);
                    this.TextControl.ForeColor = Color.FromArgb(215, 58, 58);
                    this.BorderColor = Color.FromArgb(232, 179, 175);
                    this.MinimumSize = new Size(this.Width, 36);
                    this.BorderWidth = new Padding(1);
                    break;
                default:
                    break;
            }

            this.ResumeLayout();
        }

        void TextControl_InputFocused(object sender, EventArgs e)
        {
            if (InputFocused != null)
            {
                InputFocused(this, EventArgs.Empty);
            }
        }

        #endregion

        #region 속성

        private InputControlType m_inputType = InputControlType.ReadOnly;
        public InputControlType InputType
        {
            get
            {
                return m_inputType;
            }
            set
            {
                m_inputType = value;
                RendenControlByType();
            }
        }

        /// <summary>
        /// Set focus
        /// </summary>
        public bool IsFocused
        {
            get
            {
                return TextControl.IsFocused;
            }
            set
            {
                TextControl.IsFocused = value;

                if (InputFocused != null && value)
                {
                    InputFocused(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Padding BorderWidth { get; set; }

        /// <summary>
        /// 라벨넓이
        /// </summary>
        public int TitleWidth
        {
            get
            {
                return TitleControl.Width;
            }
            set
            {
                TitleControl.Width = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get
            {
                return TitleControl.Text;
            }
            set
            {
                TitleControl.Text = value;
            }
        }

        /// <summary>
        /// Text value
        /// </summary>
        [Category("Appearance"),
        EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                return TextControl.Text;
            }
            set
            {
                TextControl.Text = value;
            }
        }

        public string Format
        {
            get
            {
                return TextControl.Format;
            }
            set
            {
                TextControl.Format = value;
            }
        }

        [DefaultValue(typeof(InputTextDataType), "Text")]
        public InputTextDataType DataType
        {
            get
            {
                return TextControl.DataType;
            }
            set
            {
                TextControl.DataType = value;
            }
        }

        /// <summary>
        /// 타이틀정렬
        /// </summary>
        public ContentAlignment TitleAlign
        {
            get
            {
                return TitleControl.TextAlign;
            }
            set
            {
                TitleControl.TextAlign = value;
            }
        }

        /// <summary>
        /// 내용, 텍스트부분 정렬
        /// </summary>
        public ContentAlignment TextAlign
        {
            get
            {
                return TextControl.TextAlign;
            }
            set
            {
                TextControl.TextAlign = value;
            }
        }

        /// <summary>
        /// Maximum length of text can input
        /// </summary>
        public int MaxLength
        {
            get
            {
                return TextControl.MaxLength;
            }
            set
            {
                TextControl.MaxLength = value;
            }
        }

        public event EventHandler InputFocused;

        #endregion

        #region IFocusableControl Members

        public void SetFocus()
        {
            //KeyListenerManager.Instance.ActivateControl(this);
        }

        [Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Browsable(true)]
        public int FocusedIndex
        {
            get
            {
                return TextControl.FocusedIndex;
            }
            set
            {
                TextControl.FocusedIndex = value;
            }
        }

        /// <summary>
        /// Focus가능여부
        /// </summary>
        public bool Focusable
        {
            get
            {
                return TextControl.Focusable;
            }
            set
            {
                TextControl.Focusable = value;
            }
        }
        
        #endregion
    }
}
