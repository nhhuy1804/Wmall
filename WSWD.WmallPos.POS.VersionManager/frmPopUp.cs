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
    public partial class frmPopUp : Form
    {
        #region 생성자

        public frmPopUp(string strDate, string strMsg)
        {
            InitializeComponent();

            //컨트롤 설정
            InitControl(strDate, strMsg);

            //컨트롤 이벤트 셋팅
            InitEvent();
        }

        #endregion

        #region 컨트롤 설정 및 컨트롤 이벤트 셋팅

        /// <summary>
        /// 컨트롤 설정
        /// </summary>
        private void InitControl(string strDate, string strMsg)
        {
            try
            {
                lbl.Text = strDate;
                label1.Text = strMsg.Length <= 0 ? "서버 적용하시겠습니까?" : strMsg;
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
                    this.DialogResult = DialogResult.OK;
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

        #endregion

    }
}
