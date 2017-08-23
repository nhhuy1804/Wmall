using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.FX.Shared;
using System.Drawing;
using System.Windows.Forms;
using WSWD.WmallPos.POS.FX.Win.UserControls;

namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    partial class MessageDialog
    {
        private MessageDialogType m_messageType = MessageDialogType.Information;
        private string m_message = string.Empty;
        private object[] m_buttonLabels = null;

        public MessageDialog(MessageDialogType messageType, 
            Exception exception,
            string messageCode, 
            string strMsg, 
            params object[] buttonLabels)            
        {
            InitializeComponent();

            if (exception != null)
            {
                m_messageType = MessageDialogType.Error;
                m_message = exception.Message;
            }
            else
            {
                m_messageType = messageType;
            }

            m_buttonLabels = buttonLabels;

            if (!string.IsNullOrEmpty(strMsg))
            {
                m_message = strMsg;
            }
            else
            {
                m_message = string.IsNullOrEmpty(messageCode) ? string.Empty : ConfigData.Current.SysMessage.GetMessage(messageCode);
            }

            DialogInit();
        }

        public MessageDialog(Exception exception)
            : this(MessageDialogType.Error, exception, string.Empty, 
            string.Empty)
        {
        }

        public MessageDialog() : this(MessageDialogType.Information, null, 
            string.Empty, string.Empty)
        {
        }
        
        private void DialogInit()
        {
            this.btnYes.Click += new EventHandler(MessageDialog_Button_Click);
            this.btnNoOK.Click += new EventHandler(MessageDialog_Button_Click);
            this.btnCancelClose.Click += new EventHandler(MessageDialog_Button_Click);
            this.FormClosed += new FormClosedEventHandler(MessageDialog_FormClosed);
            lblMessage.Text = m_message;
            InitDialogByType(m_messageType);
        }

        void MessageDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.btnYes.Click -= new EventHandler(MessageDialog_Button_Click);
            this.btnNoOK.Click -= new EventHandler(MessageDialog_Button_Click);
            this.btnCancelClose.Click -= new EventHandler(MessageDialog_Button_Click);
            this.FormClosed -= new FormClosedEventHandler(MessageDialog_FormClosed);

            if (picMsgIcon.Image != null)
            {
                picMsgIcon.Image.Dispose();
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
                    btnNoOK.KeyType = KeyButtonTypes.Enter;

                    btnCancelClose.Visible = true;
                    btnCancelClose.KeyType = KeyButtonTypes.Clear;

                    btnNoOK.Text = m_buttonLabels != null && m_buttonLabels.Length > 0 ?
                            m_buttonLabels[0].ToString() : Properties.Resources.MessageDialog_Button_Yes;
                    btnNoOK.Tag = "Yes";

                    btnCancelClose.Text = m_buttonLabels != null && m_buttonLabels.Length > 0 ? m_buttonLabels[1].ToString() :
                        Properties.Resources.MessageDialog_Button_No;
                    btnCancelClose.Tag = "No";
                    Text = Properties.Resources.MessageDialog_Title_Question;
                    break;
                case MessageDialogType.YesNoCancel:
                    btnYes.Visible = true;
                    btnYes.KeyType = KeyButtonTypes.Enter;

                    btnNoOK.Visible = true;
                    btnCancelClose.Visible = true;
                    btnCancelClose.KeyType = KeyButtonTypes.Clear;

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
                    btnCancelClose.KeyType = KeyButtonTypes.EnterOrClear;

                    btnCancelClose.Text = m_buttonLabels != null && m_buttonLabels.Length > 0 ? 
                        m_buttonLabels[0].ToString() : Properties.Resources.MessageDialog_Button_OK;
                    btnCancelClose.Tag = "OK";
                    Text = Properties.Resources.MessageDialog_Title_Error;
                    break;
                case MessageDialogType.Warning:
                    btnYes.Visible = false;
                    btnNoOK.Visible = false;

                    btnCancelClose.Visible = true;
                    btnCancelClose.KeyType = KeyButtonTypes.EnterOrClear;

                    btnCancelClose.Text = m_buttonLabels != null && m_buttonLabels.Length > 0 ? 
                        m_buttonLabels[0].ToString() : Properties.Resources.MessageDialog_Button_OK;
                    btnCancelClose.Tag = "OK";
                    Text = Properties.Resources.MessageDialog_Title_Warning;
                    break;
                case MessageDialogType.Information:
                    btnYes.Visible = false;
                    btnNoOK.Visible = false;
                    btnCancelClose.Visible = true;
                    btnCancelClose.KeyType = KeyButtonTypes.EnterOrClear;

                    btnCancelClose.Text = m_buttonLabels != null && m_buttonLabels.Length > 0 ? 
                        m_buttonLabels[0].ToString() : Properties.Resources.MessageDialog_Button_OK;
                    btnCancelClose.Tag = "OK";
                    Text = Properties.Resources.MessageDialog_Title_Information;
                    break;
                default:
                    break;
            }
        }

        void MessageDialog_KeyEvent(OPOSKeyEventArgs e)
        {
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

                e.IsHandled = true;
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)            
            {
                switch (m_messageType)
                {
                    case MessageDialogType.Question:
                        MessageDialog_Button_Click(btnCancelClose, EventArgs.Empty);
                        break;
                    case MessageDialogType.YesNoCancel:
                        MessageDialog_Button_Click(btnCancelClose, EventArgs.Empty);
                        break;
                    case MessageDialogType.Error:
                    case MessageDialogType.Warning:
                    case MessageDialogType.Information:
                    default:
                        MessageDialog_Button_Click(btnCancelClose, EventArgs.Empty);
                        break;
                }

                e.IsHandled = true;
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
