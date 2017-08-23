//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P002.cs
 * 화면설명 : 전자서명
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
using System.Diagnostics;

namespace WSWD.WmallPos.POS.PY.VC
{
    public partial class POS_PY_P002 : PopupBase01, IPYP002View
    {
        #region 변수

        //비즈니스 로직
        //private IPYP002presenter m_Presenter;

        /// <summary>
        /// 대상금액
        /// </summary>
        private int _iAmt = 0;

        private bool _cancellable = true;

        private bool m_modeProcessing = false;
        private bool ModeProcessing
        {
            get
            {
                return m_modeProcessing;
            }
            set
            {
                m_modeProcessing = value;
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        btnSave.Enabled = !m_modeProcessing;
                        btnClose.Enabled = !m_modeProcessing;
                    });
                }
                else
                {
                    btnSave.Enabled = !m_modeProcessing;
                    btnClose.Enabled = !m_modeProcessing;
                }
            }
        }

        #endregion

        #region 생성자

        public POS_PY_P002(int iAmt)
            : this(iAmt, true)
        {
        }

        /// <summary>
        /// 현금영수증
        /// </summary>
        /// <param name="iGetAmt">대상금액</param>
        public POS_PY_P002(int iAmt, bool cancellable)
        {
            InitializeComponent();

            //대상금액
            _iAmt = iAmt;
            _cancellable = cancellable;
            this.btnClose.Enabled = cancellable;
            this.txtAmt.Text = iAmt.ToString();

            //SignPad
            if (POSDeviceManager.SignPad.Status != WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
            {
                POSDeviceManager.SignPad.Initialize(this.axKSNet_Dongle1);
            }

            //이벤트 등록
            InitEvent();
        }

        #endregion

        #region 이벤트 등록

        /// <summary>
        /// Form Event 정의
        /// </summary>
        private void InitEvent()
        {
            // 완전 Closed
            this.Disposed += new EventHandler(POS_PY_P002_Disposed);
            this.Load += new EventHandler(form_Load);
            // receive key event
            this.KeyEvent += new OPOSKeyEventHandler(form_KeyEvent);            //KeyEvent
            this.btnSave.Click += new EventHandler(btnSave_Click);              //확인 button Event
            this.btnClose.Click += new EventHandler(btnClose_Click);            //닫기 button Event
        }

        void POS_PY_P002_Disposed(object sender, EventArgs e)
        {
            this.Disposed -= new EventHandler(POS_PY_P002_Disposed);
            this.Load -= new EventHandler(form_Load);

            // receive key event            
            this.KeyEvent -= new OPOSKeyEventHandler(form_KeyEvent);            //KeyEvent
            this.btnSave.Click -= new EventHandler(btnSave_Click);              //확인 button Event
            this.btnClose.Click -= new EventHandler(btnClose_Click);            //닫기 button Event                

            POSDeviceManager.SignPad.Close();
        }
        #endregion

        #region 이벤트 정의

        /// <summary>
        /// Form Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void form_Load(object sender, EventArgs e)
        {
            ModeProcessing = false;
            this.FormClosed += new FormClosedEventHandler(POS_PY_P002_FormClosed);

            //정보 조회
            //m_Presenter = new PYP002presenter(this);
            if (POSDeviceManager.SignPad.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
            {
                RequestSignPad();
            }
            else
            {
                StatusMessage = MSG_SIGNPAD_INIT_ERROR;
            }
        }

        void POS_PY_P002_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.FormClosed -= new FormClosedEventHandler(POS_PY_P002_FormClosed);
            POSDeviceManager.SignPad.RequestResetSignPad();
        }

        /// <summary>
        /// Key Event
        /// </summary>
        /// <param name="e"></param>
        void form_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
            {
                //전자서명 재시도 요청
                RequestSignPad();
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
            {
                btnSave_Click(btnSave, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 확인 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    ConfirmCloseSign();
                });
            }
            else
            {
                ConfirmCloseSign();
            }
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

        void ConfirmCloseSign()
        {
            if (ModeProcessing)
            {
                return;
            }

            ModeProcessing = true;

            if (!POSDeviceManager.SignPad.CloseSign())
            {
                if (ShowMessageBox(MessageDialogType.Question, string.Empty, MSG_NO_SIGN_CONFIRM_OK) == DialogResult.Yes)
                {
                    this.ReturnResult.Clear();
                    this.ReturnResult.Add("SIGN_DATA", string.Empty);
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    RequestSignPad();
                    ModeProcessing = false;
                }
            }
            else
            {
                this.ReturnResult.Clear();
                this.ReturnResult.Add("SIGN_DATA", POSDeviceManager.SignPad.LastSignData);
                this.DialogResult = DialogResult.OK;
            }
        }

        void RequestSignPad()
        {
            StatusMessage = MSG_SIGN_REQ_WAITING;
            //전자서명 요청
            POSDeviceManager.SignPad.RequestSign("서명 해주세요", string.Format("금액: {0:#,##0}", _iAmt));
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetSignPad()
        {
            POSDeviceManager.SignPad.ReInitialize(true);
        }

        #endregion
    }
}