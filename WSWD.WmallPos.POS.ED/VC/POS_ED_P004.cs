//-----------------------------------------------------------------
/*
 * 화면명   : POS_ED_P004.cs
 * 화면설명 : DATA 전송
 * 개발자   : 정광호
 * 개발일자 : 2015.04.15
*/
//-----------------------------------------------------------------

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.POS.ED.PI;
using WSWD.WmallPos.POS.ED.PT;
using WSWD.WmallPos.POS.ED.VI;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.POS.FX.Win.Data;
using WSWD.WmallPos.FX.Shared.Data;
using WSWD.WmallPos.FX.NetComm.Tasks.PU;
using WSWD.WmallPos.FX.Shared.NetComm.Response;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PU;
using WSWD.WmallPos.FX.Shared.NetComm;

namespace WSWD.WmallPos.POS.ED.VC
{
    /// <summary>
    /// 마감관리 - DATA 전송
    /// </summary>
    public partial class POS_ED_P004 : FormBase, IEDP004View
    {
        #region 변수

        //DATA 전송 비즈니스 로직
        private IEDP004presenter m_Presenter;

        /// <summary>
        /// TR전송, 결락전송 자료
        /// </summary>
        private DataSet _dsTR = null;

        /// <summary>
        /// TR전송, 결락전송 자료 카운트
        /// </summary>
        private int iRow = 0;

        /// <summary>
        /// TR전송, 결락전송 구분(0:TR전송, 1:결락전송)
        /// </summary>
        private string strType = string.Empty;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        #endregion

        #region 생성자

        public POS_ED_P004()
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
            
            this.KeyEvent += new OPOSKeyEventHandler(form_KeyEvent);                                 //Key Event
            this.btnTransTr.Click += new EventHandler(btnTrans_Click);                                      //TR전송 Click Event
            this.btnTransLoss.Click += new EventHandler(btnTrans_Click);                                    //결락전송 Click Event
            this.btnClose.Click += new EventHandler(btnClose_Click);                                        //닫기 button Event
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

            this.IsModal = true;

            //DATA 전송 비즈니스 로직
            m_Presenter = new EDP004presenter(this);

            //전송일자
            txtTransDate.Text = DateTimeUtils.FormatDateString(ConfigData.Current.AppConfig.PosInfo.SaleDate, "/");
            txtTransDate.SetFocus();

            //화면 표출 메세지 설정
            msgBar.Text = strMsg01;
        }

        /// <summary>
        /// Key Event
        /// </summary>
        /// <param name="e"></param>
        void form_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (_bDisable)
            {
                e.IsHandled = true;
                return;
            }

            if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_ENTER)
            {
                //실행
                
            }
            else if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_CLEAR)
            {
                if (!txtTransDate.IsFocused)
                {
                    e.IsHandled = true;
                    this.Close();
                }
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_BKS)
            {
                e.IsHandled = true;
            }
            else if (!e.IsControlKey && txtTransDate.IsFocused)
            {
                if (txtTransDate.Text.Length == 3 || txtTransDate.Text.Length == 6)
                {
                    txtTransDate.Text += (e.KeyCodeText.ToString() + "/");
                    e.IsHandled = true;
                }
                else if (txtTransDate.Text.Length == 9)
                {
                    DateTime dtTime = DateTime.Now;
                    if (!DateTime.TryParse(txtTransDate.Text + e.KeyCodeText.ToString(), out dtTime))
                    {
                        txtTransDate.Text = "";
                        msgBar.Text = strMsg02;
                        e.IsHandled = true;
                    }
                }
            }
        }

        /// <summary>
        /// TR전송, 결락전송 Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnTrans_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;

            _dsTR = null;
            iRow = 0;
            strType = "";

            if (txtTransDate.Text.Length != 10 || m_Presenter == null) return;

            DateTime dtTime = DateTime.Now;
            if (!DateTime.TryParse(txtTransDate.Text, out dtTime)) return;

            WSWD.WmallPos.POS.FX.Win.UserControls.Button btn = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)sender;

            strType = btn == btnTransTr ? "0" : "1";
            string strMsg_01 = strType == "0" ? strMsg05 : strMsg06;
            string strMsg_02 = strType == "0" ? strMsg03 : strMsg04;

            if (ShowMessageBox(MessageDialogType.Question, null, string.Format("{0} {1}", txtTransDate.Text, strMsg_01)) == DialogResult.Yes)
            {
                ChildManager.ShowProgress(true);
                SetControlDisable(true);

                //화면 표출 메세지 설정
                msgBarProgress.Text = string.Format("0/0");
                colorProgressBar1.Percentage = 0;
                msgBar.Text = strMsg_02;
                Application.DoEvents();

                //TR전송, 결락전송 데이터 조회
                m_Presenter.GetTRData(strType);
            }
        }

        /// <summary>
        /// 닫기 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;

            this.Close();
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// TR전송, 결락전송 데이터 결과
        /// </summary>
        /// <param name="ds"></param>
        public void SetTRData(DataSet ds)
        {
            if (ds == null || ds.Tables.Count != 2 || ds.Tables[0] == null || ds.Tables[1] == null)
            {
                ChildManager.ShowProgress(false);
                msgBar.Text = strMsg09;
                SetControlDisable(false);
                return;
            }

            _dsTR = ds.Copy();

            msgBarProgress.Text = string.Format("0/{0}", ds.Tables[0].Rows.Count.ToString());

            if (ds.Tables[0].Rows.Count <= 0)
            {
                ChildManager.ShowProgress(false);
                msgBar.Text = strMsg09;
                SetControlDisable(false);
            }
            else
            {
                TransData();
            }

            Application.DoEvents();
        }

        public string TRTransDate
        {
            get
            {
                return txtTransDate.Text.Replace("/", "");
            }
        }

        /// <summary>
        /// TR전송, 결락전송 전문Upload
        /// </summary>
        private void TransData()
        {
            if (_dsTR.Tables[0].Rows.Count < iRow + 1)
            {
                ChildManager.ShowProgress(false);
                msgBar.Text = strMsg07;
                SetControlDisable(false);
                return;
            } 

            DataRow[] drFilter = _dsTR.Tables[1].Select(string.Format("NO_TRXN = '{0}'", _dsTR.Tables[0].Rows[iRow][0].ToString()));

            if (drFilter != null && drFilter.Length > 0)
            {
                DataTable dt = _dsTR.Tables[1].Clone();

                DataRow NewDr;
                foreach (DataRow drTemp in drFilter)
                {
                    NewDr = dt.NewRow();

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        NewDr[i] = drTemp[i] != null ? drTemp[i].ToString() : "";
                    }

                    dt.Rows.Add(NewDr);
                }

                iRow++;

                var pu01Task = new PU01DataTaskManual(strType, dt);
                pu01Task.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pu01Task_TaskCompleted);
                pu01Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pu01Task_Errored);
                pu01Task.ExecuteTask();
            }
        }

        /// <summary>
        /// 전문(TR, 결락) 전송완료
        /// </summary>
        /// <param name="responseData"></param>
        void pu01Task_TaskCompleted(TaskResponseData responseData)
        {
            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        if (_dsTR.Tables[0].Rows.Count > 0)
                        {
                            colorProgressBar1.Percentage = (int)(Convert.ToDouble(iRow) / Convert.ToDouble(_dsTR.Tables[0].Rows.Count) * 100);
                        }
                        else
                        {
                            colorProgressBar1.Percentage = 0;
                        }

                        msgBarProgress.Text = string.Format("{0}/{1}", iRow, _dsTR.Tables[0].Rows.Count.ToString());

                        TransData();
                    });
                }
                else
                {
                    if (_dsTR.Tables[0].Rows.Count > 0)
                    {
                        colorProgressBar1.Percentage = (int)(Convert.ToDouble(iRow) / Convert.ToDouble(_dsTR.Tables[0].Rows.Count) * 100);
                    }
                    else
                    {
                        colorProgressBar1.Percentage = 0;
                    }

                    msgBarProgress.Text = string.Format("{0}/{1}", iRow, _dsTR.Tables[0].Rows.Count.ToString());

                    TransData();
                }
            }
            else if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.NoInfo)
            {
                ChildManager.ShowProgress(false);

                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false);
                    });
                }
                else
                {
                    msgBar.Text = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false);
                }
            }
            else
            {
                ChildManager.ShowProgress(false);

                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false);
                    });
                }
                else
                {
                    msgBar.Text = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false);
                }
            }
        }

        void pu01Task_Errored(string errorMessage, Exception lastException)
        {
            ChildManager.ShowProgress(false);

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    msgBar.Text = strMsg08;
                    SetControlDisable(false);
                });
            }
            else
            {
                msgBar.Text = strMsg08;
                SetControlDisable(false);
            }
        }

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable/Disable
        /// </summary>
        void SetControlDisable(bool bDisable)
        {
            _bDisable = bDisable;

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    foreach (var item in this.Controls)
                    {
                        if (item.GetType().Name.ToString().ToLower() == "keypad")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad key = (WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad)item;
                            key.Enabled = !_bDisable;
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "inputtext")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.InputText txt = (WSWD.WmallPos.POS.FX.Win.UserControls.InputText)item;
                            txt.Enabled = !_bDisable;
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "button")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.Button btn = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)item;
                            btn.Enabled = !_bDisable;
                        }
                    }
                });
            }
            else
            {
                foreach (var item in this.Controls)
                {
                    if (item.GetType().Name.ToString().ToLower() == "keypad")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad key = (WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad)item;
                        key.Enabled = !_bDisable;
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "inputtext")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.InputText txt = (WSWD.WmallPos.POS.FX.Win.UserControls.InputText)item;
                        txt.Enabled = !_bDisable;
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "button")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.Button btn = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)item;
                        btn.Enabled = !_bDisable;
                    }
                }
            }

            //Application.DoEvents();
        }

        #endregion
    }
}
