//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P019.cs
 * 화면설명 : 상품교환권 - 수동반품
 * 개발자   : TCL
 * 개발일자 : 2015.06.01
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
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.FX.Win.Data;
using WSWD.WmallPos.POS.PY.Data;

namespace WSWD.WmallPos.POS.PY.VC
{
    public partial class POS_PY_P019 : PopupBase01
    {
        #region 변수

        /// <summary>
        /// 받을돈
        /// </summary>
        private int m_payAmt = 0;

        #endregion

        #region 생성자

        /// <summary>
        /// 타건카드
        /// </summary>
        /// <param name="iGetAmt">받을돈</param>
        public POS_PY_P019(int payAmt)
        {
            InitializeComponent();

            //받을돈
            m_payAmt = payAmt;

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
            this.txtGiftAmt.InputFocused += new EventHandler(txtGiftAmt_InputFocused);
            this.FormClosed += new FormClosedEventHandler(POS_PY_P019_FormClosed);
        }

        void POS_PY_P019_FormClosed(object sender, FormClosedEventArgs e)
        {
            Load -= new EventHandler(form_Load);
            this.KeyEvent -= new OPOSKeyEventHandler(form_KeyEvent);    //KeyEvent
            this.btnSave.Click -= new EventHandler(btnSave_Click);      //적용 button Event
            this.btnClose.Click -= new EventHandler(btnClose_Click);    //닫기 button Event

            this.txtPaymentCnt.InputFocused -= new EventHandler(txtPaymentCnt_InputFocused);
            this.txtGiftAmt.InputFocused -= new EventHandler(txtGiftAmt_InputFocused);
            this.FormClosed -= new FormClosedEventHandler(POS_PY_P019_FormClosed);
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
            StatusMessage = strMsg01;
        }

        void txtGiftAmt_InputFocused(object sender, EventArgs e)
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

        /// <summary>
        /// Validate input and continue
        /// </summary>
        /// <param name="e"></param>
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
        /// ON ENTER AND OK BUTTON CLICK
        /// </summary>
        void ProcessSave()
        {
            btnSave.Enabled = btnClose.Enabled = false;

            // 상품교환권
            var bp = new BasketExchange();
            bp.PayGrpCd = NetCommConstants.PAYMENT_GROUP_COUPON;        //지불 수단 그룹 코드
            bp.PayDtlCd = NetCommConstants.PAYMENT_DETAIL_EXCHANGE;     //지불 수단 상세 코드

            bp.BalGrpCd = NetCommConstants.PAYMENT_GROUP_NONE;        //지불 수단 그룹 코드
            bp.BalDtlCd = NetCommConstants.PAYMENT_DETAIL_NONE;     //지불 수단 상세 코드

            bp.PayAmt = txtPaymentAmt.Text;                             //지불 금액
            bp.CancFg = "0";                                             //취소 flag                        
            bp.ExchangeAmt = txtGiftAmt.Text;                        //권종 금액                        
            bp.ExchangeDivision = "01";
            bp.ExchangeType = "";

            this.ReturnResult.Add("PAY_DATA", bp);
            this.DialogResult = DialogResult.OK;
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
            }

            if (txtGiftAmt.IsFocused)
            {
                var amt = TypeHelper.ToInt64(txtPaymentCnt.Text) * TypeHelper.ToInt64(txtGiftAmt.Text);
                amt = amt.ValidateMoney();
                txtPaymentAmt.Text = amt.ToString();

                if (amt <= 0)
                {
                    StatusMessage = MSG_INPUT_AMT;
                    txtGiftAmt.Text = string.Empty;
                    return;
                }

                if (amt > m_payAmt)
                {
                    StatusMessage = ERR_MSG_OVER_AMT;
                    txtGiftAmt.Text = string.Empty;
                    return;
                }

                ProcessSave();

                return;
            }

            this.NextControl();

        }

        /// <summary>
        /// 초기상태확인
        /// </summary>
        /// <returns></returns>
        bool IsInitialState()
        {
            return txtPaymentCnt.Text.Length == 0 && txtGiftAmt.Text.Length == 0;
        }

        #endregion
    }
}
