using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Config;

namespace WSWD.WmallPos.POS.Config
{
    public partial class AdminPassPop : PopupBase01
    {
        public AdminPassPop()
        {
            InitializeComponent();

            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnClose.Click += new EventHandler(btnClose_Click);

            this.Load += new EventHandler(AdminPassPop_Load);
            this.intPassword.InputFocused += new EventHandler(intPassword_InputFocused);
            this.KeyEvent += new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(AdminPassPop_KeyEvent);            
        }

        void AdminPassPop_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_ENTER)
            {
                e.IsHandled = true;
                btnOK_Click(btnOK, EventArgs.Empty);
            }
            else if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_CLEAR)
            {
                if (intPassword.Text.Length == 0)
                {
                    e.IsHandled = true;
                    btnClose_Click(btnClose, EventArgs.Empty);
                }
            }
        }

        void AdminPassPop_Load(object sender, EventArgs e)
        {
            this.intPassword.SetFocus();
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            CheckPassword();
        }

        void intPassword_InputFocused(object sender, EventArgs e)
        {
            StatusMessage = "관리자 비밀번호를 입력하십시오.(6자리)";
        }

        void CheckPassword()
        {
            if (intPassword.Text == ConfigData.Current.AppConfig.PosOption.DefAdminPass)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                ShowMessageBox(WSWD.WmallPos.FX.Shared.MessageDialogType.Warning, string.Empty,
                    "비밀번호 일지하지 않습니다.");
                intPassword.Text = string.Empty;
            }
        }
    }
}
