using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.FX.Shared;
using System.Drawing;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    partial class MessageBoxDialog
    {
        private MessageDialogType m_messageType = MessageDialogType.Information;
        private string m_message = string.Empty;
        private object[] m_buttonLabels = null;

        public MessageBoxDialog(MessageDialogType messageType, string messageCode, string strMsg, params object[] buttonLabels)
            : this()
        {
            m_messageType = messageType;
            m_buttonLabels = buttonLabels;

            if (!string.IsNullOrEmpty(strMsg))
            {
                m_message = strMsg;
            }
            else
            {
                m_message = ConfigData.Current.SysMessage.GetMessage(messageCode);
            }
        }

        public MessageBoxDialog(Exception exception)
            : this()
        {
            m_messageType = MessageDialogType.Error;
            m_message = exception.Message;
        }

        public MessageBoxDialog()
        {
            InitializeComponent();
            DialogInit();
        }
        
        private void DialogInit()
        {
            this.btnYes.Click += new EventHandler(MessageDialog_Button_Click);
            this.btnNoOK.Click += new EventHandler(MessageDialog_Button_Click);
            this.btnCancelClose.Click += new EventHandler(MessageDialog_Button_Click);
            this.Load += new EventHandler(MessageDialog_Load);
            this.KeyEvent += new OPOSKeyEventHandler(MessageDialog_KeyEvent);
        }


        /// <summary>
        /// 팝업타이틀
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                if (lblPopupTitle != null)
                {
                    lblPopupTitle.Text = value;
                }
            }
        }

        private void InitDialogByType(MessageDialogType messageType)
        {
            var msgType = messageType == MessageDialogType.YesNoCancel ? MessageDialogType.Question : messageType;
            if (messageType != MessageDialogType.None)
            {
                picMsgIcon.Image = Extensions.ResourceLoad(string.Format("messagedialog_{0}.png", msgType.ToString().ToLower()));
            }

            switch (messageType)
            {
                case MessageDialogType.Question:
                    btnYes.Visible = false;
                    btnNoOK.Visible = true;
                    btnCancelClose.Visible = true;

                    btnNoOK.Text = m_buttonLabels != null && m_buttonLabels.Length > 0 ?
                            m_buttonLabels[0].ToString() : Properties.Resources.MessageDialog_Button_Yes;
                    btnNoOK.Tag = "Yes";

                    btnCancelClose.Text = m_buttonLabels != null && m_buttonLabels.Length > 0 ? 
                        m_buttonLabels[1].ToString() : Properties.Resources.MessageDialog_Button_No;
                    btnCancelClose.Tag = "No";
                    Text = Properties.Resources.MessageDialog_Title_Question;
                    break;
                case MessageDialogType.YesNoCancel:
                    btnYes.Visible = true;
                    btnNoOK.Visible = true;
                    btnCancelClose.Visible = true;

                    btnYes.Text = m_buttonLabels != null && m_buttonLabels.Length > 0 ?
                        m_buttonLabels[0].ToString() : Properties.Resources.MessageDialog_Button_Yes;
                    btnYes.Tag = "Yes";

                    btnNoOK.Text = m_buttonLabels != null && m_buttonLabels.Length > 0 ?
                        m_buttonLabels[1].ToString() : Properties.Resources.MessageDialog_Button_No;
                    btnNoOK.Tag = "No";

                    btnCancelClose.Text = m_buttonLabels != null && m_buttonLabels.Length > 0 ? 
                        m_buttonLabels[2].ToString() :
                        Properties.Resources.MessageDialog_Button_Cancel;
                    btnCancelClose.Tag = "Cancel";
                    Text = Properties.Resources.MessageDialog_Title_YesNoCancel;
                    break;
                case MessageDialogType.Error:
                    btnYes.Visible = false;
                    btnNoOK.Visible = false;
                    btnCancelClose.Visible = true;

                    btnCancelClose.Text = m_buttonLabels != null && m_buttonLabels.Length > 0 ? 
                        m_buttonLabels[0].ToString() : Properties.Resources.MessageDialog_Button_OK;
                    btnCancelClose.Tag = "OK";
                    Text = Properties.Resources.MessageDialog_Title_Error;
                    break;
                case MessageDialogType.Warning:
                    btnYes.Visible = false;
                    btnNoOK.Visible = false;
                    btnCancelClose.Visible = true;

                    btnCancelClose.Text = m_buttonLabels != null && m_buttonLabels.Length > 0 ? 
                        m_buttonLabels[0].ToString() : Properties.Resources.MessageDialog_Button_OK;
                    btnCancelClose.Tag = "OK";
                    Text = Properties.Resources.MessageDialog_Title_Warning;
                    break;
                case MessageDialogType.Information:
                    btnYes.Visible = false;
                    btnNoOK.Visible = false;
                    btnCancelClose.Visible = true;

                    btnCancelClose.Text = m_buttonLabels != null && m_buttonLabels.Length > 0 ?
                        m_buttonLabels[0].ToString() : Properties.Resources.MessageDialog_Button_OK;
                    btnCancelClose.Tag = "OK";
                    Text = Properties.Resources.MessageDialog_Title_Information;
                    break;
                default:
                    break;
            }
        }

        void MessageDialog_Load(object sender, EventArgs e)
        {
            #region message label font settings
            string fontName = this.StringProp("FontSettings", "FontName", "돋움");
            string fontBold = this.StringProp("FontSettings", "FontBold", "true");
            int fontSize = this.IntProp("FontSettings", "MessageFontSize", 12);
            this.lblMessage.Font = new Font(fontName, fontSize, "true".Equals(fontBold) ? FontStyle.Bold : FontStyle.Regular);
            #endregion

            lblMessage.Text = m_message;
            InitDialogByType(m_messageType);
        }

        void MessageDialog_KeyEvent(OPOSKeyEventArgs e)
        {
            e.IsHandled = true;
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
            {
                switch (m_messageType)
                {
                    case MessageDialogType.Question:
                        MessageDialog_Button_Click(btnNoOK, EventArgs.Empty);
                        break;
                    case MessageDialogType.YesNoCancel:
                        MessageDialog_Button_Click(btnYes, EventArgs.Empty);
                        break;
                    case MessageDialogType.Error:
                    case MessageDialogType.Warning:
                    case MessageDialogType.Information:
                    default:
                        MessageDialog_Button_Click(btnCancelClose, EventArgs.Empty);
                        break;
                }
            }
        }

        void MessageDialog_Button_Click(object sender, EventArgs e)
        {
            Control btn = (Control)sender;
            string cmd = (string)btn.Tag;
            if ("Yes".Equals(cmd))
            {
                this.DialogResult = DialogResult.Yes;
            }
            else if ("No".Equals(cmd))
            {
                this.DialogResult = DialogResult.No;
            }
            else if ("Cancel".Equals(cmd))
            {
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
