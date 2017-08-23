//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P018.cs
 * 화면설명 : 기타 결제 선택
 * 개발자   : 정광호
 * 개발일자 : 2015.05
*/
//-----------------------------------------------------------------

using System;
using System.Windows.Forms;

using WSWD.WmallPos.FX.Shared;

using WSWD.WmallPos.POS.FX.Win.Forms;

namespace WSWD.WmallPos.POS.PY.VC
{
    public partial class POS_PY_P018 : PopupBase01
    {
        #region 변수

        private WSWD.WmallPos.POS.FX.Win.UserControls.Button m_selButton = null;

        #endregion

        #region 생성자

        /// <summary>
        /// 타건카드
        /// </summary>
        /// <param name="iGetAmt">받을돈</param>
        public POS_PY_P018()
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
            this.KeyEvent += new OPOSKeyEventHandler(form_KeyEvent);    //KeyEvent
            this.btnP016.Click += new EventHandler(btn_Click);   //구 상품교환권 Button Event
            this.btnP008.Click += new EventHandler(btn_Click);   // 타건복지 Button Event
            this.btnP017.Click += new EventHandler(btn_Click);   //타건카드 Button Event
            this.btnP011.Click += new EventHandler(btn_Click);   //결제할인 Button Event

            this.btnClose.Click += new EventHandler(btnClose_Click);    //닫기 button Event
            this.FormClosed += new FormClosedEventHandler(POS_PY_P018_FormClosed);
        }

        void POS_PY_P018_FormClosed(object sender, FormClosedEventArgs e)
        {
            Load -= new EventHandler(form_Load);
            this.KeyEvent -= new OPOSKeyEventHandler(form_KeyEvent);    //KeyEvent
            this.btnP016.Click -= new EventHandler(btn_Click);   //구 상품교환권 Button Event
            this.btnP008.Click -= new EventHandler(btn_Click);   // 타건복지 Button Event
            this.btnP017.Click -= new EventHandler(btn_Click);   //타건카드 Button Event
            this.btnP011.Click -= new EventHandler(btn_Click);   //결제할인 Button Event

            this.btnClose.Click -= new EventHandler(btnClose_Click);    //닫기 button Event
            this.FormClosed -= new FormClosedEventHandler(POS_PY_P018_FormClosed);
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

            //메세지 설정
            StatusMessage = MSG_SELECT_TASK;

            // 복지
            btnP008.Tag = new string[] { "POS_PY_P008", MSG_P008 };
            // 결제할인
            btnP011.Tag = new string[] { "POS_PY_P011", MSG_P011 };
            // 구상품교환권
            btnP016.Tag = new string[] { "POS_PY_P016", MSG_P016 };
            // 타건카드
            btnP017.Tag = new string[] { "POS_PY_P017", MSG_P017 };
        }

        /// <summary>
        /// KeyEvent
        /// </summary>
        /// <param name="e"></param>
        void form_KeyEvent(OPOSKeyEventArgs e)
        {
            e.IsHandled = true;
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
            {
                btnClose_Click(btnClose, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 구 상품교환권, 타건복지, 타건카드, 결제할인 Button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btn_Click(object sender, EventArgs e)
        {
            if (m_selButton != null)
            {
                m_selButton.Selected = false;
            }

            WSWD.WmallPos.POS.FX.Win.UserControls.Button btn = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)sender;
            btn.Selected = true;
            m_selButton = btn;

            string[] msgs = btn.Tag as string[];
            StatusMessage = msgs[1];


            if (m_selButton == null)
            {
                StatusMessage = MSG_SELECT_TASK;
                return;
            }

            this.ReturnResult.Clear();
            this.ReturnResult.Add("SELECT_CLASS", msgs[0]);
            this.DialogResult = DialogResult.OK;
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


        #endregion
    }
}
