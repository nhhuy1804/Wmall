//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P003.cs
 * 화면설명 : 신용카드결제(비밀번호입력)
 * 개발자   : 정광호
 * 개발일자 : 2015.05.
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.POS.PY.PI;
using WSWD.WmallPos.POS.PY.PT;
using WSWD.WmallPos.POS.PY.VI;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PP;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PP;
using WSWD.WmallPos.FX.NetComm.Tasks.PP;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.FX.Shared.NetComm.Types;

namespace WSWD.WmallPos.POS.PY.VC
{
    public partial class POS_PY_P003 : PopupBase01, IPYP003View
    {
        #region 생성자 & 변수

        private string m_inputPassword = string.Empty;
        private bool m_encoding = false;
        private string m_cardNo = string.Empty;
        private short m_cardPassLength = 4;

        public POS_PY_P003()
            : this(false, string.Empty, 4, true)
        {

        }

        public POS_PY_P003(bool encoding, string cardNo, int passLength) :
            this(encoding, cardNo, passLength, true)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encoding">비밀번호암호화여부</param>
        /// <param name="cardNo"></param>
        /// <param name="passLength"></param>
        public POS_PY_P003(bool encoding, string cardNo, int passLength, bool cancellable)
        {
            InitializeComponent();

            m_encoding = encoding;
            m_cardNo = cardNo;
            m_cardPassLength = (short)passLength;
            btnClose.Enabled = cancellable;

            //Form Load Event
            Load += new EventHandler(form_Load);
        }

        #endregion

        #region 이벤트 등록

        /// <summary>
        /// Form Event 정의
        /// </summary>
        private void InitEvent()
        {
            this.FormClosed += new FormClosedEventHandler(POS_PY_P003_FormClosed);
            this.KeyEvent += new OPOSKeyEventHandler(form_KeyEvent);            //KeyEvent
            this.btnSave.Click += new EventHandler(btnSave_Click);              //확인 button Event
            this.btnClose.Click += new EventHandler(btnClose_Click);            //닫기 button Event
            this.txtPassword.InputFocused += new EventHandler(txtPassWord_InputFocused);

            POSDeviceManager.SignPad.PinEvent += new POSDataEventHandler(SignPad_PinEvent);
        }

        #endregion

        #region 이벤트 정의

        /// <summary>
        /// Key Event
        /// </summary>
        /// <param name="e"></param>
        void form_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
            {
                e.IsHandled = true;
                btnSave_Click(btnSave, EventArgs.Empty);
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
            {
                e.IsHandled = true;
                RequestInputPassword();
            }
        }

        void POS_PY_P003_FormClosed(object sender, FormClosedEventArgs e)
        {
            Load -= new EventHandler(form_Load); 

            this.FormClosed -= new FormClosedEventHandler(POS_PY_P003_FormClosed);
            this.KeyEvent -= new OPOSKeyEventHandler(form_KeyEvent);            //KeyEvent
            this.btnSave.Click -= new EventHandler(btnSave_Click);              //확인 button Event
            this.btnClose.Click -= new EventHandler(btnClose_Click);            //닫기 button Event
            this.txtPassword.InputFocused -= new EventHandler(txtPassWord_InputFocused);

            POSDeviceManager.SignPad.PinEvent -= new POSDataEventHandler(SignPad_PinEvent);
            POSDeviceManager.SignPad.ClearPinDataRequest();
        }

        void form_Load(object sender, EventArgs e)
        {
            //이벤트 등록
            InitEvent();

            //SignPad
            RequestInputPassword();
        }

        /// <summary>
        /// 비밀번호입력 들어오기
        /// </summary>
        /// <param name="eventData"></param>
        void SignPad_PinEvent(string eventData)
        {
            txtPassword.Text = eventData;
            m_inputPassword = eventData;
            StatusMessage = string.Empty;

            btnSave_Click(btnSave, EventArgs.Empty);
        }

        /// <summary>
        /// 확인 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text.Length > 0)
            {
                this.ReturnResult.Add("PASSWORD", m_inputPassword);
                this.ReturnResult.Add("WORK_INDEX", "83");
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                RequestInputPassword();
                StatusMessage = MSG_PASS_WAITING;
            }
        }

        void txtPassWord_InputFocused(object sender, EventArgs e)
        {
            StatusMessage = MSG_PASS_WAITING;
        }

        /// <summary>
        /// 닫기 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// 싸인패드명령
        /// </summary>
        void RequestInputPassword()
        {
            if (POSDeviceManager.SignPad.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
            {
                StatusMessage = MSG_PASS_WAITING;

                int ret = 0;
                if (m_encoding)
                {
                    //ret = POSDeviceManager.SignPad.RequestCardPinData(MSG_PASS_SIGN_WAIT, string.Empty, MSG_PASS_SIGN_THANKS, string.Empty, m_cardNo, 1, m_cardPassLength);
                    ret = POSDeviceManager.SignPad.RequestPinData(MSG_PASS_SIGN_WAIT, string.Empty, 
                        MSG_PASS_SIGN_THANKS, string.Empty, m_cardNo, 1, m_cardPassLength, 1);
                }
                else
                {
                    ret = POSDeviceManager.SignPad.RequestPinData(MSG_PASS_SIGN_WAIT, string.Empty, MSG_PASS_SIGN_THANKS, string.Empty, 1, m_cardPassLength);
                }

                if (ret != 0)
                {
                    StatusMessage = MSG_SIGNPAD_INIT_ERROR;
                }
            }
            else
            {
                StatusMessage = MSG_SIGNPAD_INIT_ERROR;
            }
        }

        #endregion
    }
}