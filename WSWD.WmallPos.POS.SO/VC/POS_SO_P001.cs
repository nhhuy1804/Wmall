using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Data;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using System.Data;

namespace WSWD.WmallPos.POS.SO.VC
{
    public partial class POS_SO_P001 : PopupBase01
    {
        /// <summary>
        /// 오류인상태
        /// </summary>
        private bool m_isError = false;
        private DataRow m_casDataRow = null;
        private List<string> m_checkRoles = null;

        public POS_SO_P001()
        {
            InitializeComponent();

            //Form Load Event
            Load += new EventHandler(form_Load);
            m_checkRoles = new List<string>();
        }

        public POS_SO_P001(string fgUsers) : this()
        {
            var fgs = fgUsers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            m_checkRoles.AddRange(fgs);
        }

        #region 이벤트등록

        void InitEvent()
        {
            this.KeyEvent += new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(POS_SO_P001_KeyEvent);

            this.txtCasNo.InputFocused += new EventHandler(txtCasNo_InputFocused);
            this.txtCasNo.MaxLength = 7;

            this.txtPassword.InputFocused += new EventHandler(txtPassword_InputFocused);
            this.txtPassword.MaxLength = 4;

            this.btnClose.Click += new EventHandler(btnClose_Click);
            this.btnSave.Click += new EventHandler(btnSave_Click);
        }

        #endregion

        #region 이벤트정의

        void POS_SO_P001_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (!e.IsControlKey)
            {
                m_isError = false;
            }

            if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_CLEAR)
            {
                if (txtPassword.Text.Length == 0 && txtCasNo.Text.Length == 0 && txtCasNo.IsFocused)
                {
                    e.IsHandled = true;
                    btnClose_Click(btnClose, EventArgs.Empty);
                }
                else
                {
                    if (this.FocusedControl != null)
                    {
                        InputText it = (InputText)this.FocusedControl;
                        if (it.Text.Length > 0)
                        {
                            it.SetFocus();
                            m_casDataRow = null;
                            return;
                        }
                    }

                    e.IsHandled = true;
                    this.PreviousControl();
                }
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
            {
                ValidateOnEnter();
            }
        }

        void form_Load(object sender, EventArgs e)
        {
            //이벤트 등록
            InitEvent();
            txtCasNo.SetFocus();
        }

        void txtPassword_InputFocused(object sender, EventArgs e)
        {
            if (!m_isError)
            {
                StatusMessage = MSG_ENTER_PASS;
                m_isError = false;
            }
        }

        void txtCasNo_InputFocused(object sender, EventArgs e)
        {
            if (!m_isError)
            {
                StatusMessage = MSG_ENTER_ID;
                m_isError = false;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            ValidateOnEnter();
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        #endregion

        #region 상용자저의

        /// <summary>
        /// 계산원확인
        /// </summary>
        /// <returns></returns>
        bool CheckCasData()
        {
            if (txtCasNo.Text.Length == 0)
            {
                txtCasNo.SetFocus();
                return false;
            }

            var db = MasterDbHelper.InitInstance();
            try
            {
                string query = Extensions.LoadSqlCommand("POS_SO", "ValidateAdminCas");
                m_casDataRow = db.ExecuteQueryDataRow(query,
                    new string[] {
                        "@ID_USER"
                    },
                    new object[] {
                        txtCasNo.Text
                    });


                if (m_casDataRow == null)
                {
                    m_isError = true;
                    txtCasNo.Text = string.Empty;
                    StatusMessage = MSG_ERR_NO_USER;
                    txtCasNo.SetFocus();
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                db.Dispose();
            }

            return m_casDataRow != null;
        }

        /// <summary>
        /// ENTER key, 확인버튼 클릭시
        /// </summary>
        void ValidateOnEnter()
        {
            if (m_casDataRow == null || (
                m_casDataRow != null && m_casDataRow["ID_USER"].ToString() != txtCasNo.Text))
            {
                if (!CheckCasData())
                {
                    return;
                }
            }

            if (txtCasNo.IsFocused)
            {
                this.NextControl();
            }

            if (m_casDataRow != null)
            {
                if (txtPassword.Text.Length > 0)
                {
                    ProcessLogin();
                    return;
                }
            }
        }

        /// <summary>
        /// Returns
        /// 0: 정상
        /// 1: 암호틀림
        /// 2: 관리자아님
        /// </summary>
        /// <returns></returns>
        int ValidateAdmin()
        {
            if (m_casDataRow == null)
            {
                if (!CheckCasData())
                {
                    return 3;
                }
            }

            int loginValid = 0;
            string adminFg = m_casDataRow["FG_USER"].ToString();
            string pass = m_casDataRow["NO_PASS"].ToString();
            loginValid = txtPassword.Text.Equals(pass) ? 0 : 1; // 암호틀림

            if (loginValid == 0)
            {
                if (!m_checkRoles.Contains(adminFg))
                {
                    loginValid = 2; // 관리자아님
                }
            }

            return loginValid;
        }

        /// <summary>
        /// 로그인처리
        /// </summary>
        void ProcessLogin()
        {
            int nValid = ValidateAdmin();
            if (nValid == 0)
            {
                this.ReturnResult.Add("CASNO", txtCasNo.Text);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                m_isError = true;

                if (nValid == 1)
                {
                    StatusMessage = MSG_ERR_PASS;
                    txtPassword.Text = string.Empty;
                }
                else if (nValid == 2)
                {
                    StatusMessage = MSG_ERR_NOT_ADMIN;
                    txtCasNo.Text = string.Empty;
                    txtPassword.Text = string.Empty;
                    txtCasNo.SetFocus();
                }
                else
                {
                    StatusMessage = MSG_ERR_NO_USER;
                    txtCasNo.Text = string.Empty;
                    txtPassword.Text = string.Empty;
                    txtCasNo.SetFocus();
                }
            }
        }

        #endregion
    }
}
