//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P005.cs
 * 화면설명 : 카드사 선택
 * 개발자   : 정광호
 * 개발일자 : 2015.05
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
    public partial class POS_PY_P005 : PopupBase01, IPYP005View
    {
        #region 변수

        const int HEIGHT_12 = 341;
        const int HEIGHT_16 = 389;        

        WSWD.WmallPos.POS.FX.Win.UserControls.Button m_selecteButton = null;

        private IPYP005presenter m_presenter = null;

        #endregion

        #region 생성자

        public POS_PY_P005()
        {
            InitializeComponent();
            
            //Form Load Event
            Load += new EventHandler(form_Load);
            FormClosed += new FormClosedEventHandler(POS_PY_P005_FormClosed);
        }

        void POS_PY_P005_FormClosed(object sender, FormClosedEventArgs e)
        {
            Load -= new EventHandler(form_Load); 
            this.btnOK.Click -= new EventHandler(btnOK_Click);    //닫기 button Event
            this.btnClose.Click -= new EventHandler(btnClose_Click);
        }

        #endregion

        #region 이벤트 등록

        /// <summary>
        /// Form Event 정의
        /// </summary>
        private void InitEvent()
        {
            this.btnOK.Click += new EventHandler(btnOK_Click);    //닫기 button Event
            this.btnClose.Click += new EventHandler(btnClose_Click);

            m_presenter = new PYP005presenter(this);
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
            this.Height = HEIGHT_12;            
            m_presenter.GetCardCompList();            

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
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
            {
                btnOK_Click(btnOK, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 확인 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (m_selecteButton != null)
            {
                this.ReturnResult.Add("CD_CARD", m_selecteButton.Tag.ToString());
                this.ReturnResult.Add("NM_CARD", m_selecteButton.Text);

                this.DialogResult = DialogResult.OK;
            }
            else
            {
                StatusMessage = MSG_SELECT_CARD;
            }
        }
        
        /// <summary>
        /// 닫기버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region 사용자 정의

        public void BindCardCompList(List<string[]> cardList)
        {
            if (cardList.Count > 12)
            {
                for (int i = 12; i < cardList.Count; i++)
                {
                    string btnName = string.Format("button{0}", i + 1);
                    var cs = this.ContainerPanel.Controls.Find(btnName, true);
                    if (cs.Length > 0)
                    {
                        cs[0].Visible= true;
                    }
                }

                this.Height = HEIGHT_16;
            }
            else
            {
                this.Height = HEIGHT_12;
            }

            for (int i = 0; i < 16; i++)
            {
                string btnName = string.Format("button{0}", i + 1);
                var cs = this.ContainerPanel.Controls.Find(btnName, true);
                if (cs.Length > 0)
                {
                    if (i > cardList.Count - 1)
                    {
                        cs[0].Text = string.Empty;
                        cs[0].Tag = null;
                    }
                    else
                    {
                        cs[0].Text = cardList[i][1];
                        cs[0].Tag = cardList[i][0];
                        cs[0].MouseUp += new MouseEventHandler(CardButton_MouseUp);
                    }
                }
            }

            StatusMessage = MSG_SELECT_CARD;
        }

        void CardButton_MouseUp(object sender, MouseEventArgs e)
        {
            WSWD.WmallPos.POS.FX.Win.UserControls.Button btn = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)sender;
            if (m_selecteButton != null)
            {
                m_selecteButton.Selected = false;
            }

            btn.Selected = true;
            m_selecteButton = btn;
            StatusMessage = btn.Text;
        }

        #endregion
    }
}
