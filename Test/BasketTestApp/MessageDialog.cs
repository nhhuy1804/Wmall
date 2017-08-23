using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.POS.FX.Win.Forms;

namespace BasketTestApp
{
    public partial class MessageDialog : InputDialogBase
    {
        private MessageDialogType m_messageType = MessageDialogType.Information;
        private string m_message = string.Empty;
        private object[] m_buttonLabels = null;

        public MessageDialog(MessageDialogType messageType, string messageCode, string strMsg, params object[] buttonLabels)
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

        public MessageDialog(Exception exception)
            : this()
        {
            m_messageType = MessageDialogType.Error;
            m_message = exception.Message;
        }

        public MessageDialog()
        {
            InitializeComponent();
            this.Load += new EventHandler(MessageDialog_Load);

            this.Text = string.Empty;
        }

        void MessageDialog_Load(object sender, EventArgs e)
        {
        }

    }
}
