using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSWD.WmallPos.POS.FX.Win.Controls;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public partial class OpenStatusItem : BorderPanel
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

                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        MessageLabel.Text = value;
                    });
                }
                else
                {
                    MessageLabel.Text = value;
                }
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

                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        switch (value)
                        {
                            case OpenItemStatus.None:
                                StatusLabel.Icon = null;
                                StatusLabel.Text = string.Empty;
                                break;
                            case OpenItemStatus.OK:
                                StatusLabel.Icon = Properties.Resources.ico_ok;
                                StatusLabel.Text = "OK";
                                break;
                            case OpenItemStatus.Error:
                                StatusLabel.Icon = Properties.Resources.ico_error;
                                StatusLabel.Text = "ERROR";
                                break;
                            default:
                                break;
                        }
                    });
                }
                else
                {
                    switch (value)
                    {
                        case OpenItemStatus.None:
                            StatusLabel.Icon = null;
                            StatusLabel.Text = string.Empty;
                            break;
                        case OpenItemStatus.OK:
                            StatusLabel.Icon = Properties.Resources.ico_ok;
                            StatusLabel.Text = "OK";
                            break;
                        case OpenItemStatus.Error:
                            StatusLabel.Icon = Properties.Resources.ico_error;
                            StatusLabel.Text = "ERROR";
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 기본배경색
        /// </summary>
        public override Color BackColor
        {
            get
            {
                if (this.Parent != null)
                {
                    try
                    {
                        int idx = this.Parent.Controls.GetChildIndex(this);
                        return idx % 2 == 0 ? Color.White : Color.FromArgb(246);
                    }
                    catch (Exception)
                    {
                        return base.BackColor;
                    }
                }

                return base.BackColor;
            }
        }

        /// <summary>
        /// 기본태투리색갈
        /// </summary>
        public override Color BorderColor
        {
            get
            {
                if (this.Parent != null)
                {
                    int idx = this.Parent.Controls.GetChildIndex(this);
                    if (idx % 2 == 1)
                    {
                        return Color.FromArgb(204);
                    }
                }

                return BackColor;
            }
        }


        /// <summary>
        /// 태투리사이즈
        /// </summary>
        public override Padding BorderWidth
        {
            get
            {
                if (this.Parent != null)
                {
                    int idx = this.Parent.Controls.GetChildIndex(this);
                    if (idx % 2 == 1)
                    {
                        return new Padding(0, 1, 0, 1);
                    }
                }

                return new Padding(0);
            }
        }
    }

    public enum OpenItemStatus
    {
        None,
        OK,
        Error
    }
}
