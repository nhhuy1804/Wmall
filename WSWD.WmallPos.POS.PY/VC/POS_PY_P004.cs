//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P004.cs
 * 화면설명 : 신용카드결제(IC카드 정보 입력대기)
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
    public partial class POS_PY_P004 : PopupBase01, IPYP004View
    {
        #region 변수

        /// <summary>
        /// 난수
        /// </summary>
        private string m_randNum = string.Empty;

        #endregion

        #region 생성자

        /// <summary>
        /// 현금영수증 ICCard Reader 팝업
        /// </summary>
        public POS_PY_P004() :
            this(string.Empty)
        {
        }


        /// <summary>
        /// 현금영수증 ICCard Reader 팝업
        /// </summary>
        /// <param name="iGetAmt">대상금액</param>
        public POS_PY_P004(string randNum)
        {
            InitializeComponent();

            // 난수, max 32 charc
            m_randNum = string.IsNullOrEmpty(randNum) ? string.Empty : randNum;

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
            this.FormClosing += new FormClosingEventHandler(POS_PY_P004_FormClosing);
            this.KeyEvent += new OPOSKeyEventHandler(form_KeyEvent);            //KeyEvent
            this.btnRetry.Click += new EventHandler(btnRetry_Click);
            this.btnClose.Click += new EventHandler(btnClose_Click);            //닫기 button Event

            // POSDeviceManager.SignPad.ICStatusEvent += new POSICStatusCheckEventHandler(SignPad_ICStatusEvent);
            POSDeviceManager.SignPad.ICCardEvent += new POSICCardReaderEventHandler(SignPad_ICCardEvent);
            POSDeviceManager.SignPad.SignPadCancelledEvent += new EventHandler(SignPad_SignPadCancelledEvent);                
        }

        void POS_PY_P004_FormClosing(object sender, FormClosingEventArgs e)
        {
            Load -= new EventHandler(form_Load); 

            this.FormClosing -= new FormClosingEventHandler(POS_PY_P004_FormClosing);
            this.KeyEvent -= new OPOSKeyEventHandler(form_KeyEvent);            //KeyEvent
            this.btnRetry.Click -= new EventHandler(btnRetry_Click);
            this.btnClose.Click -= new EventHandler(btnClose_Click);            //닫기 button Event

            // POSDeviceManager.SignPad.ICStatusEvent -= new POSICStatusCheckEventHandler(SignPad_ICStatusEvent);
            POSDeviceManager.SignPad.ICCardEvent -= new POSICCardReaderEventHandler(SignPad_ICCardEvent);
            POSDeviceManager.SignPad.SignPadCancelledEvent -= new EventHandler(SignPad_SignPadCancelledEvent);
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
            //이벤트 등록
            InitEvent();

            lbl.Text = MSG_ASK_IC_CARD_ENTER;
            StatusMessage = MSG_WAIT_IC_CARD;

            // Init and check ICCard Status
            InitSignPad();
        }

        void SignPad_ICStatusEvent(bool statusOK)
        {
            if (!statusOK)
            {
                StatusMessage = MSG_SIGNPAD_IC_ERROR;
            }
            else
            {
                // dont check status again
                POSDeviceManager.SignPad.ICStatusEvent -= new POSICStatusCheckEventHandler(SignPad_ICStatusEvent);
                RequestICCard();
            }
        }

        void SignPad_SignPadCancelledEvent(object sender, EventArgs e)
        {
            btnRetry.Enabled = true;
            StatusMessage = MSG_WAIT_IC_CARD_CANCELLED;
        }

        void SignPad_ICCardEvent(string encData, string posEntryMode, 
            string track3Data, string icCardSeqNo, string issuerCd, string issuePosCode)
        {
            this.ReturnResult.Add("TRACKII", track3Data);
            this.ReturnResult.Add("IC_CARD_SEQ_NO", icCardSeqNo);
            this.ReturnResult.Add("ISSUER_CD", issuerCd);
            this.ReturnResult.Add("ISSUER_POS_NO", issuePosCode);
            this.ReturnResult.Add("ENC_DATA", encData);

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Key Event
        /// </summary>
        /// <param name="e"></param>
        void form_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
            {
                e.IsHandled = true;
                btnClose_Click(btnClose, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 재시도
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnRetry_Click(object sender, EventArgs e)
        {
            RequestICCard();
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
        /// 싸인패드상태확인
        /// </summary>
        void InitSignPad()
        {
            if (POSDeviceManager.SignPad.Status != WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
            {
                POSDeviceManager.SignPad.Initialize(this.axKSNet_Dongle1, true);
            }

            //if (POSDeviceManager.SignPad.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
            //{
            //    POSDeviceManager.SignPad.CheckICStatus();
            //}
            //else
            //{
            //    StatusMessage = MSG_SIGNPAD_INIT_ERROR;
            //}

            RequestICCard();
        }

        /// <summary>
        /// IC카드 읽기요청
        /// </summary>
        void RequestICCard()
        {
            btnRetry.Enabled = false;
            POSDeviceManager.SignPad.RequestICCardAppr(m_randNum, MSG_ASK_IC_CARD_READER, string.Empty, string.Empty, string.Empty, 10);
        }

        #endregion
    }
}