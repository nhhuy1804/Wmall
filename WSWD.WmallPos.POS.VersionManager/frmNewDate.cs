using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.VersionManager
{
    public partial class frmNewDate : Form
    {
        /// <summary>
        /// 신규생성일자 + 번호
        /// </summary>
        private string _NewDate;
        public string NewDate
        {
            get { return this._NewDate; }
        }

        #region 생성자

        public frmNewDate()
        {
            InitializeComponent();

            //컨트롤 설정
            InitControl();

            //컨트롤 이벤트 셋팅
            InitEvent();
        }

        #endregion

        #region 컨트롤 설정 및 컨트롤 이벤트 셋팅

        /// <summary>
        /// 컨트롤 설정
        /// </summary>
        private void InitControl()
        {
            try
            {
                txtDate.Text = DateTime.Now.ToShortDateString();
                txtNo.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 컨트롤 이벤트 셋팅
        /// </summary>
        private void InitEvent()
        {
            try
            {
                //폼로드 이벤트
                Load += new EventHandler(form_Load);

                //버튼 클릭 이벤트
                btnOK.Click += new EventHandler(btn_Click);
                btnCancel.Click += new EventHandler(btn_Click);

                txtNo.KeyPress += new KeyPressEventHandler(txtNo_KeyPress);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }



        #endregion

        #region 이벤트 정의

        /// <summary>
        /// 폼 로드 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void form_Load(object sender, EventArgs e)
        {
            try
            {
                txtNo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            try
            {
                if (btn.Name.ToString() == btnOK.Name.ToString())
                {
                    if (txtDate.Value.ToString().Length <= 0)
                    {
                        txtDate.Focus();
                        MessageBox.Show("신규생성일자는 필수입력항목입니다.");
                    }
                    else
                    {
                        if (txtNo.Text.Length > 0)
                        {
                            txtNo.Text = Convert.ToInt16(txtNo.Text) < 10 ? "00" + Convert.ToInt16(txtNo.Text).ToString() : Convert.ToInt16(txtNo.Text) < 100 ? "0" + Convert.ToInt16(txtNo.Text).ToString() : Convert.ToInt16(txtNo.Text).ToString();

                            _NewDate = txtDate.Value.ToString("yyyyMMdd") + txtNo.Text;
                            this.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            txtNo.Focus();
                            MessageBox.Show("번호는 필수입력항목입니다.");
                        }
                    }
                }
                else if (btn.Name.ToString() == btnCancel.Name.ToString())
                {
                    this.DialogResult = DialogResult.Cancel;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 번호 Keypress 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //숫자,백스페이스만 입력받는다.
                if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != 8) //8:백스페이스,45:마이너스,46:소수점
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        #endregion
    }
}
