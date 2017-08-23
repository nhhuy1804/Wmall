using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

using WSWD.WmallPos.POS.FX.Win.Controls;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Data;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.POS.FX.Win.Forms;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public class InputText : CLabel, IKeyInputView, IFocusableControl
    {
        #region 생성자, PUBLIC

        public InputText()
        {
            base.AutoSize = false;
            this.Corner = 1;
            this.TextAlign = ContentAlignment.MiddleLeft;// System.Windows.Forms.HorizontalAlignment.Left;
            this.Margin = new Padding(6, 3, 3, 3);

            this.FocusedBorderWidth = 2;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BorderWidth = 1;
            this.HasBorder = true;
            this.ForeColor = Color.FromArgb(51, 51, 51);
            
            this.KeyEvent += new OPOSKeyEventHandler(InputText_KeyEvent);
            this.Disposed += new EventHandler(InputText_Disposed);
            this.Click += new EventHandler(InputText_Click);
            this.HandleCreated += new EventHandler(InputText_HandleCreated);
            this.HandleDestroyed += new EventHandler(InputText_HandleDestroyed);
        }

        void InputText_HandleDestroyed(object sender, EventArgs e)
        {
            this.KeyEvent -= new OPOSKeyEventHandler(InputText_KeyEvent);
            this.Disposed -= new EventHandler(InputText_Disposed);
            this.Click -= new EventHandler(InputText_Click);
            this.HandleCreated -= new EventHandler(InputText_HandleCreated);
            this.HandleDestroyed -= new EventHandler(InputText_HandleDestroyed);
            this.DetachKeyInput();
        }

        void InputText_HandleCreated(object sender, EventArgs e)
        {
            this.AttachKeyInput();
        }

        #endregion

        #region 이벤트처리

        void InputText_KeyEvent(OPOSKeyEventArgs e)
        {
            if (!e.IsControlKey)
            {
                if (!string.IsNullOrEmpty(this.Text) && MaxLength > 0 && this.Text.Length >= MaxLength)
                {
                    e.IsHandled = true;
                    return;
                }

                this.Text += e.KeyCodeText; 
                e.IsHandled = true;
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_BKS)
            {
                this.Text = this.Text.Length > 0 ? this.Text.Substring(0, this.Text.Length - 1) : string.Empty;
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
            {
                this.Text = string.Empty;
            }
        }

        void InputText_Click(object sender, EventArgs e)
        {
            this.SetFocus();
        }

        void InputText_Disposed(object sender, EventArgs e)
        {
            this.KeyEvent -= new OPOSKeyEventHandler(InputText_KeyEvent);
            this.Disposed -= new EventHandler(InputText_Disposed);
            this.Click -= new EventHandler(InputText_Click);
            this.HandleCreated -= new EventHandler(InputText_HandleCreated);
            this.DetachKeyInput();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            string tempText = m_text;
            if (this.PasswordMode)
            {
                m_text = new string('*', m_text.Length);
            }
            else
            {
                m_text = FormatDisplay();
            }

            base.OnPaint(e);
            m_text = tempText;
        }

        #endregion

        #region 속석

        /// <summary>
        /// 
        /// </summary>
        public override bool AutoSize
        {
            get
            {
                return false;
            }
        }

        private bool m_readOnly = false;
        public bool ReadOnly
        {
            get
            {
                return m_readOnly;
            }
            set
            {
                m_readOnly = value;
                if (value)
                {
                    this.BackColor = Color.FromArgb(246, 246, 246);
                }
                else
                {
                    this.BackColor = Color.White;
                }

                this.Focusable = !value;
            }
        }

        /// <summary>
        /// Focusable
        /// </summary>
        private bool m_isFocused = false;
        public bool IsFocused
        {
            get
            {
                return m_isFocused;
            }
            set
            {
                m_isFocused = value;
                if (value)
                {
                    this.BorderColor = Color.FromArgb(125, 109, 195);
                    this.BorderWidth = FocusedBorderWidth;
                }
                else
                {
                    if (this.InputLostFocused != null)
                    {
                        InputLostFocused(this, EventArgs.Empty);
                    }
                    this.BorderColor = Color.FromArgb(187, 187, 187);
                    this.BorderWidth = 1;
                }

                // this.HasBorder = value;
                Invalidate();

                if (value && InputFocused != null)
                {
                    InputFocused(this, EventArgs.Empty);
                }
            }
        }

        private string m_text = string.Empty;
        public override string Text
        {
            get
            {
                return m_text;
            }
            set
            {
                m_text = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 비밀번호 모드
        /// </summary>
        private bool m_passwordMode = false;
        public bool PasswordMode
        {
            get { return m_passwordMode; }
            set
            {
                m_passwordMode = value;
                Invalidate();
            }
        }

        [DefaultValue(typeof(InputTextDataType), "Text")]
        public InputTextDataType DataType { get; set; }

        /// <summary>
        /// Display format
        /// </summary>
        public string Format { get; set; }

        [Category("Appearance"), DefaultValue(0),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int MaxLength { get; set; }

        public override Font Font
        {
            get
            {
                return new Font("돋움", 12, FontStyle.Bold);                
            }            
        }

        /// <summary>
        /// Focus 일때 BorderWidth
        /// </summary>
        [DefaultValue(2)]
        public int FocusedBorderWidth { get; set; }

        /// <summary>
        /// Fired when control is focused
        /// </summary>
        public event EventHandler InputFocused;

        /// <summary>
        /// Fired when control is losing focused
        /// </summary>
        public event EventHandler InputLostFocused;

        #endregion

        #region IKeyInputView Members

        public event OPOSKeyEventHandler KeyEvent;

        public void PerformKeyEvent(OPOSKeyEventArgs e)
        {
            if (this.ReadOnly || !this.IsFocused)
            {
                return;
            }

            if (KeyEvent != null)
            {
                e.Sender = this;
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {                        
                        KeyEvent(e);
                    });
                }
                else
                {
                    KeyEvent(e);
                } 
            }
        }

        protected KeyInputForm KeyListener
        {
            get
            {
                var form = this.FindForm();
                return form != null ? (KeyInputForm)form : null;
            }
        }

        #endregion

        #region Private 함수

        /// <summary>
        /// Format text to display
        /// </summary>
        /// <returns></returns>
        private string FormatDisplay()
        {
            string text = m_text;
            if (!string.IsNullOrEmpty(Format))
            {
                try
                {
                    switch (DataType)
                    {
                        case InputTextDataType.Numeric:
                            text = string.IsNullOrEmpty(text) ? string.Empty : 
                                string.Format("{0:" + Format + "}", TypeHelper.ToDouble(text));
                            break;
                        case InputTextDataType.DateTime:
                            text = string.IsNullOrEmpty(text) ? string.Empty : 
                                string.Format("{0:" + Format + "}", TypeHelper.ToDateTime(text));
                            break;
                        default:
                            break;
                    }
                }
                catch
                {

                }
            }

            return text;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // InputText
            // 
            this.Name = "InputText";
            this.ResumeLayout(false);

        }

        #endregion

        #region IFocusableControl Members

        /// <summary>
        /// WMALL SetFocus
        /// </summary>
        public void SetFocus()
        {
            //KeyListenerManager.Instance.ActivateControl(this);
            if (this.KeyListener != null)
            {
                this.KeyListener.FocusedControl = this;
            }
        }

        [Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Browsable(true)]
        public int FocusedIndex
        {
            get;
            set;
        }

        private bool m_focusable = true;
        public bool Focusable
        {
            get
            {
                return m_focusable;
            }
            set
            {
                m_focusable = value;
            }
        }

        #endregion
    }
}
