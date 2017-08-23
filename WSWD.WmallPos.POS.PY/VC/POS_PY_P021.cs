//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P021.cs
 * 화면설명 : 신용 IC Card, FallbAck 일경우 알림 창, 10초 기다린
 * 개발자   : TCL
 * 개발일자 : 2016.06.
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
    public partial class POS_PY_P021 : PopupBase03
    {
        #region 변수

        /// <summary>
        /// 10SECONDS
        /// </summary>
        public const int WAIT_TIMER = 20;

        /// <summary>
        /// Counting seconds
        /// </summary>
        private int m_tmCount = 0;


        #endregion

        #region 생성자

        /// <summary>
        /// 
        /// </summary>
        public POS_PY_P021()
        {
            InitializeComponent();

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
            this.btnClose.Click += new EventHandler(btnClose_Click);            //닫기 button Event
        }

        void POS_PY_P004_FormClosing(object sender, FormClosingEventArgs e)
        {
            Load -= new EventHandler(form_Load);

            this.FormClosing -= new FormClosingEventHandler(POS_PY_P004_FormClosing);
            this.KeyEvent -= new OPOSKeyEventHandler(form_KeyEvent);            //KeyEvent
            this.btnClose.Click -= new EventHandler(btnClose_Click);            //닫기 button Event
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

            this.lblRemTime.Text = string.Format("{0}초", WAIT_TIMER - m_tmCount);
        }

        /// <summary>
        /// Key Event
        /// </summary>
        /// <param name="e"></param>
        void form_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
            {
                e.IsHandled = true;
                btnClose_Click(btnClose, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 닫기 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            SetEncCardReaderCompleted(false);
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// MSR 카드 읽기 이벤트 발생 된다
        /// </summary>
        public void SetEncCardReaderCompleted(bool received)
        {
            this.m_tmCount = 0;
            this.DialogResult = received ? DialogResult.OK : DialogResult.Cancel;
        }

        /// <summary>
        /// Inc timeout
        /// </summary>
        public bool SetIncTimeOut()
        {
            m_tmCount++;
            this.lblRemTime.Text = string.Format("{0}초", WAIT_TIMER - m_tmCount);
            this.cpbWaiting.Percentage = Convert.ToInt32(Convert.ToDouble(m_tmCount * 100) / WAIT_TIMER);

            if (m_tmCount >= WAIT_TIMER)
            {
                SetEncCardReaderCompleted(false);
            }

            return m_tmCount >= WAIT_TIMER;
        }

        #endregion
    }
}