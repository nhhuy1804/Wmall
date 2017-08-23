﻿//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P016.cs
 * 화면설명 : 구 상품교환권
 * 개발자   : 정광호
 * 개발일자 : 2015.05
*/
//-----------------------------------------------------------------

using System;
using System.Windows.Forms;

using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.POS.FX.Win.Data;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.PY.Data;

namespace WSWD.WmallPos.POS.PY.VC
{
    public partial class POS_PY_P016 : PopupBase01
    {
        #region 변수

        /// <summary>
        /// 받을돈
        /// </summary>
        private int m_payAmt = 0;
        private int m_taxAmt = 0;

        #endregion

        #region 생성자

        /// <summary>
        /// 타건카드
        /// </summary>
        /// <param name="iGetAmt">받을돈</param>
        public POS_PY_P016(int payAmt, int taxAmt, bool modeReturn)
        {
            InitializeComponent();

            //받을돈
            m_payAmt = payAmt;
            m_taxAmt = taxAmt;
            this.Text = this.Text + (modeReturn ? TITLE_CANCEL : string.Empty);

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
            this.KeyEvent += new OPOSKeyEventHandler(form_KeyEvent);    //KeyEvent
            this.btnSave.Click += new EventHandler(btnSave_Click);      //적용 button Event
            this.btnClose.Click += new EventHandler(btnClose_Click);    //닫기 button Event

            this.txtPaymentCnt.InputFocused += new EventHandler(txtPaymentCnt_InputFocused);
            this.txtPaymentAmt.InputFocused += new EventHandler(txtPaymentAmt_InputFocused);

            this.FormClosed += new FormClosedEventHandler(POS_PY_P016_FormClosed);
        }

        void POS_PY_P016_FormClosed(object sender, FormClosedEventArgs e)
        {
            Load -= new EventHandler(form_Load);
            this.KeyEvent -= new OPOSKeyEventHandler(form_KeyEvent);    //KeyEvent
            this.btnSave.Click -= new EventHandler(btnSave_Click);      //적용 button Event
            this.btnClose.Click -= new EventHandler(btnClose_Click);    //닫기 button Event

            this.txtPaymentCnt.InputFocused -= new EventHandler(txtPaymentCnt_InputFocused);
            this.txtPaymentAmt.InputFocused -= new EventHandler(txtPaymentAmt_InputFocused);

            this.FormClosed -= new FormClosedEventHandler(POS_PY_P016_FormClosed);
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

            //받을돈
            txtGetAmt.Text = m_payAmt.ToString();

            //결제금액
            txtPaymentCnt.SetFocus();
        }

        void txtPaymentCnt_InputFocused(object sender, EventArgs e)
        {
            StatusMessage = MSG_INPUT_QTY;
        }

        void txtPaymentAmt_InputFocused(object sender, EventArgs e)
        {
            StatusMessage = MSG_INPUT_AMT;
        }

        /// <summary>
        /// KeyEvent
        /// </summary>
        /// <param name="e"></param>
        void form_KeyEvent(OPOSKeyEventArgs e)
        {
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR ||
                e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
            {
                ValidateInputTextOnKeyEvent(e);
            }
        }

        /// <summary>
        /// 적용 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            ValidateEnterOnControl();
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

        void ValidateInputTextOnKeyEvent(OPOSKeyEventArgs e)
        {
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
            {
                e.IsHandled = true;
                InputText it = (InputText)this.FocusedControl;
                if (!string.IsNullOrEmpty(it.Text))
                {
                    it.Text = string.Empty;
                    return;
                }

                if (IsInitialState())
                {
                    btnClose_Click(btnClose, EventArgs.Empty);
                }
                else
                {
                    this.PreviousControl();
                }

                return;
            }

            ValidateEnterOnControl();
        }

        /// <summary>
        /// ENTER KEY 확인
        /// </summary>
        void ValidateEnterOnControl()
        {
            if (this.FocusedControl != null)
            {
                InputText it = (InputText)this.FocusedControl;
                if (it.Text.Length == 0)
                {
                    return;
                }

                var amt = TypeHelper.ToInt64(it.Text);
                if (amt <= 0)
                {
                    it.Text = string.Empty;
                    return;
                }
            }

            if (txtPaymentAmt.IsFocused)
            {
                var amt = TypeHelper.ToInt64(txtPaymentAmt.Text);
                amt = amt.ValidateMoney();
                txtPaymentAmt.Text = amt.ToString();

                if (amt <= 0)
                {
                    txtPaymentAmt.Text = string.Empty;
                    return;
                }

                if (amt > m_payAmt)
                {
                    StatusMessage = ERR_MSG_OVER_AMT;
                    txtPaymentAmt.Text = string.Empty;
                    return;
                }

                ProcessSave();

                return;
            }

            this.NextControl();

        }

        /// <summary>
        /// 초기상티인지 확인
        /// </summary>
        /// <returns></returns>
        bool IsInitialState()
        {
            return txtPaymentCnt.Text.Length == 0 && txtPaymentAmt.Text.Length == 0;
        }

        /// <summary>
        /// ON ENTER AND OK BUTTON CLICK
        /// </summary>
        void ProcessSave()
        {
            btnClose.Enabled = btnSave.Enabled = false;

            //// 구상품권
            BasketOldExGift prepareAmt = new BasketOldExGift();
            prepareAmt.BasketType = BasketTypes.BasketPay;
            prepareAmt.PayGrpCd = NetCommConstants.PAYMENT_GROUP_COUPON;
            prepareAmt.PayDtlCd = NetCommConstants.PAYMENT_DETAIL_EXCHANGE_OLD;
            prepareAmt.BalGrpCd = NetCommConstants.PAYMENT_GROUP_NONE;
            prepareAmt.BalDtlCd = NetCommConstants.PAYMENT_DETAIL_NONE;
            prepareAmt.CancFg = NetCommConstants.CANCEL_TYPE_NORMAL;
            prepareAmt.PayAmt = txtPaymentAmt.Text;
            prepareAmt.CntGift = txtPaymentCnt.Text;

            this.ReturnResult.Add("PAY_DATA", prepareAmt);
            this.DialogResult = DialogResult.OK;
        }

        #endregion
    }
}
