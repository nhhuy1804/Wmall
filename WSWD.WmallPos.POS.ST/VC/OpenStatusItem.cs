using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.ST.VC
{
    public partial class OpenStatusItem : UserControl
    {
        public OpenStatusItem()
        {
            InitializeComponent();
        }

        private string m_messageTex = string.Empty;
        public string MessageText
        {
            get
            {
                return m_messageTex;
            }
            set
            {
                m_messageTex = value;
                MessageLabel.Text = value;
            }
        }

        private OpenItemStatus m_status = OpenItemStatus.None;
        public OpenItemStatus ItemStatus
        {
            get
            {
                return m_status;
            }
            set
            {
                m_status = value;
                switch (value)
                {
                    case OpenItemStatus.None:
                        StatusLabel.Image = null;
                        StatusLabel.Text = string.Empty;
                        break;
                    case OpenItemStatus.OK:
                        StatusLabel.Image = Properties.Resources.ico_ok;
                        StatusLabel.Text = "OK";
                        break;
                    case OpenItemStatus.Error:
                        StatusLabel.Image = Properties.Resources.ico_error;
                        StatusLabel.Text = "ERROR";
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
