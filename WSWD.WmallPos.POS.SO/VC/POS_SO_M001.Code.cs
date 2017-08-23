using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.POS.FX.Win;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.SO.VI;
using WSWD.WmallPos.POS.SO.PI;
using WSWD.WmallPos.POS.SO.PT;
using WSWD.WmallPos.POS.SO.Data;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.SO.VC
{
    /// <summary>
    /// SIGNON 
    /// 개발자 : TCL
    /// 개발일자: 05..
    /// 
    /// </summary>
    partial class POS_SO_M001 : FormBase, IM001View
    {
        #region 변수
        private ISOPresenter presenter;

        #endregion

        #region 초기로드
        void FormInit()
        {
            this.Text = "SignOn";
            this.HideMainMenu = true;
            this.KeyEvent += new OPOSKeyEventHandler(POS_SO_M001_KeyEvent);
            this.Load += new EventHandler(POS_SO_M001_Load);
            this.Unload += new EventHandler(POS_SO_M001_Unload);

            this.txtCasNo.KeyEvent += new OPOSKeyEventHandler(intCasNo_KeyEvent);
            this.txtCasNo.InputFocused += new EventHandler(intCasNo_InputFocused);
            this.txtPassword.InputFocused += new EventHandler(intPassword_InputFocused);

            presenter = new SOPresenter(this);
        }

        void POS_SO_M001_Unload(object sender, EventArgs e)
        {
            this.KeyEvent -= new OPOSKeyEventHandler(POS_SO_M001_KeyEvent);
            this.Load -= new EventHandler(POS_SO_M001_Load);
            this.Unload -= new EventHandler(POS_SO_M001_Unload);

            this.txtCasNo.KeyEvent -= new OPOSKeyEventHandler(intCasNo_KeyEvent);
            this.txtCasNo.InputFocused -= new EventHandler(intCasNo_InputFocused);
            this.txtPassword.InputFocused -= new EventHandler(intPassword_InputFocused);

            this.BackgroundImage.Dispose();
        }


        #endregion

        #region 이벤트정의

        void intPassword_InputFocused(object sender, EventArgs e)
        {
            UpdateStatusMessage(LoginMessageTypes.EnterPassword);
        }

        void intCasNo_InputFocused(object sender, EventArgs e)
        {
            UpdateStatusMessage(LoginMessageTypes.EnterUserId);
        }

        void intCasNo_KeyEvent(OPOSKeyEventArgs e)
        {
            if (e.IsControlKey)
            {
                if (txtCasNo.Text.Length != 7)
                {
                    txtCasName.Text = string.Empty;
                    txtPassword.Text = string.Empty;
                    presenter.ValidateCasNo();
                }
            }
        }

        void POS_SO_M001_Load(object sender, EventArgs e)
        {
            if (txtCasName.Text.Length > 0)
            {
                txtPassword.SetFocus();
            }
            else
            {
                txtCasNo.SetFocus();
            }
        }

        void POS_SO_M001_KeyEvent(OPOSKeyEventArgs e)
        {
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
            {
                e.IsHandled = true;

                if (txtCasNo.IsFocused)
                {
                    presenter.ValidateCasNo();
                }
                else
                {
                    presenter.ValidateLogin(!string.IsNullOrEmpty(ConfigData.Current.AppConfig.PosInfo.CasNo));
                }
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
            {
                if (this.KeyListener.FocusedControl != null)
                {
                    InputText it = (InputText)this.KeyListener.FocusedControl;
                    if (it.Text.Length > 0)
                    {
                        return;
                    }
                }

                e.IsHandled = true;
                this.KeyListener.PreviousControl();
            }
        }

        #endregion

        #region IM001View Members

        /// <summary>
        /// Set focus when ready
        /// </summary>
        public void Ready()
        {
            //intCasNo.Text = ConfigData.Current.AppConfig.PosInfo.CasNo;
            //intCasName.Text = ConfigData.Current.AppConfig.PosInfo.CasName;

            if (!string.IsNullOrEmpty(CasNo) && !string.IsNullOrEmpty(CasName))
            {
                UpdateStatusMessage(LoginMessageTypes.EnterPassword);
                txtPassword.SetFocus();
            }
            else
            {
                txtCasNo.SetFocus();
            }

            txtPassword.Text = string.Empty;
            messageBar1.Text = string.Empty;

            // show CDP
            if (POSDeviceManager.LineDisplay.Status == DeviceStatus.Opened)
            {
                POSDeviceManager.LineDisplay.DisplayText(MSG_CDP_WELCOME, string.Empty);
            }
        }

        public string CasNo
        {
            get
            {
                return txtCasNo.Text;
            }
            set
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    txtCasNo.Text = value;

                    if (string.IsNullOrEmpty(value))
                    {
                        txtCasName.Text = string.Empty;
                    }
                });
            }
        }

        public string CasName
        {
            get
            {
                return txtCasName.Text;
            }
            set
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    txtCasName.Text = value;

                    UpdateStatusMessage(LoginMessageTypes.EnterPassword);
                    txtPassword.SetFocus();
                });
            }
        }

        public void UpdateStatusMessage(LoginMessageTypes msgType)
        {
            string message = string.Empty;

            switch (msgType)
            {
                case LoginMessageTypes.None:
                    break;
                case LoginMessageTypes.EnterUserId:
                    message = MSG_ENTER_ID;
                    break;
                case LoginMessageTypes.LoginFailed:
                    message = MSG_LOGIN_FAILED;
                    CasPass = string.Empty;
                    break;
                case LoginMessageTypes.NoUserInfo:
                    message = MSG_NO_USER;
                    break;
                case LoginMessageTypes.CheckCasInfo:
                    message = MSG_CHECK_CAS_INFO;
                    break;
                case LoginMessageTypes.EnterPassword:
                    message = MSG_ENTER_PASS;
                    break;
                default:
                    break;
            }

            UpdateStatusMessage(message);
        }

        public void UpdateStatusMessage(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    messageBar1.Text = message;
                });
            }
            else
            {
                messageBar1.Text = message;
            }
        }

        public void SetFocus(int focusIndex)
        {
            if (focusIndex == 0)
            {
                txtCasNo.SetFocus();
            }
            else
            {
                txtPassword.SetFocus();
            }
        }


        #region 프린트 확인

        /// <summary>
        /// 프린트 확인
        /// </summary>
        /// <returns></returns>
        public bool ChkPrint()
        {
            bool bReturn = false;
            string strErrMsg = string.Empty;

            try
            {
                if (POSDeviceManager.Printer.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
                {
                    if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.PowerClose)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_POWER;
                    }
                    else if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.CoverOpenned)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_OPENCOVER;
                    }
                    else if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.PaperEmpty)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_PAPER;
                    }
                    else if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.Closed)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_ERROR;
                    }
                    else
                    {
                        bReturn = true;
                    }
                }
                else
                {
                    strErrMsg = FXConsts.ERR_MSG_PRINTER_ERROR;
                }

                if (!bReturn)
                {
                    string[] strBtnNm = new string[2];
                    strBtnNm[0] = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00045");
                    strBtnNm[1] = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00190");

                    if (ShowMessageBox(MessageDialogType.Question, null, strErrMsg, strBtnNm) == DialogResult.Yes)
                    {
                        POSDeviceManager.Printer.Open();
                        bReturn = ChkPrint();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }

            return bReturn;
        }

        #endregion

        public void LoginSuccess()
        {
            this.DialogResult = DialogResult.OK;
            ChildManager.OnLoggedIn();
        }

        public string CasPass
        {
            get
            {
                return txtPassword.Text;
            }
            set
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    txtPassword.Text = value;
                });
            }
        }

        #endregion
    }
}
