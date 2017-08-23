//-----------------------------------------------------------------
/*
 * 화면명   : POS_IQ_P003.cs
 * 화면설명 : 수표 조회
 * 개발자   : 정광호
 * 개발일자 : 2015.04.03
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

using WSWD.WmallPos.POS.IQ.PI;
using WSWD.WmallPos.POS.IQ.PT;
using WSWD.WmallPos.POS.IQ.VI;
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
using WSWD.WmallPos.FX.Shared.NetComm.Request.PV;
using WSWD.WmallPos.FX.NetComm.Tasks.PV;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PV;

namespace WSWD.WmallPos.POS.IQ.VC
{
    public partial class POS_IQ_P003 : FormBase
    {
        #region 변수

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        #endregion

        #region 생성자

        public POS_IQ_P003()
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
            this.btnClose.Click += new EventHandler(btnClose_Click);                                        //닫기 button Event
            this.KeyEvent += new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(form_KeyEvent);         //Key Event


            txt01.InputFocused += new EventHandler(txt_InputFocused);
            txt02.InputFocused += new EventHandler(txt_InputFocused);
            txt03.InputFocused += new EventHandler(txt_InputFocused);
            txt04.InputFocused += new EventHandler(txt_InputFocused);
            txt05.InputFocused += new EventHandler(txt_InputFocused);
            txt06.InputFocused += new EventHandler(txt_InputFocused);
            txt07.InputFocused += new EventHandler(txt_InputFocused);
            txt08.InputFocused += new EventHandler(txt_InputFocused);
            txt09.InputFocused += new EventHandler(txt_InputFocused);
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

            //화면 표출 메세지 설정
            msgBar.Text = strMsg02;

            //수표번호 포커스
            txt01.SetFocus();
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
                #region Enter

                if (txt04.IsFocused || txt06.IsFocused || txt09.IsFocused)
                {
                    msgBar.Text = "";
                    msgBar.ForeColor = Color.FromArgb(51, 51, 51);

                    if (txt04.IsFocused)
                    {
                        if (txt04.Text.Length > 0)
                        {
                            if (txt04.Text.Length != txt04.MaxLength)
                            {
                                txt04.SetFocus();
                                e.IsHandled = true;
                            }
                            else
                            {
                                txt05.SetFocus();
                            }
                        }
                        else
                        {
                            txt05.SetFocus();
                        }
                    }
                    else if (txt06.IsFocused)
                    {
                        if (txt06.Text.Length >= 0)
                        {
                            if (TypeHelper.ToInt32(txt06.Text) > 0 && TypeHelper.ToInt32(txt06.Text) <= 999999999)
                            {
                                txt07.SetFocus();
                            }
                            else
                            {
                                msgBar.Text = strMsg01;
                            }
                        }
                        else
                        {
                            msgBar.Text = strMsg01;
                        }
                    }
                    else if (txt09.IsFocused)
                    {
                        if (txt09.Text.Length == txt09.MaxLength)
                        {
                            if (txt01.Text.Length != txt01.MaxLength)
                            {
                                txt01.SetFocus();
                                return;
                            }

                            if (txt02.Text.Length != txt02.MaxLength)
                            {
                                txt02.SetFocus();
                                return;
                            }

                            if (txt03.Text.Length != txt03.MaxLength)
                            {
                                txt03.SetFocus();
                                return;
                            }

                            if (txt04.Text.Length > 0)
                            {
                                if (txt04.Text.Length != txt04.MaxLength)
                                {
                                    txt04.SetFocus();
                                    return;
                                }
                            }

                            if (txt05.Text.Length != txt05.MaxLength)
                            {
                                txt05.SetFocus();
                                return;
                            }

                            if (TypeHelper.ToInt32(txt06.Text) <= 0 || TypeHelper.ToInt32(txt06.Text) > 999999999)
                            {
                                if (txt05.Text == "13" || txt05.Text == "14" || txt05.Text == "15" || txt05.Text == "16")
                                {
                                    txt05.Text = "";
                                    txt05.SetFocus();
                                }
                                else
                                {
                                    txt06.SetFocus();
                                }
                                
                                return;
                            }

                            if (txt07.Text.Length != txt07.MaxLength)
                            {
                                txt07.SetFocus();
                                return;
                            }

                            if (txt08.Text.Length != txt08.MaxLength)
                            {
                                txt08.SetFocus();
                                return;
                            }

                            DateTime dateTime = DateTime.Now;

                            string strTemp = string.Format("{0}-{1}-{2}", txt07.Text, txt08.Text, txt09.Text);
                            if (!DateTime.TryParse(strTemp, out dateTime))
                            {
                                txt07.Text = "";
                                txt08.Text = "";
                                txt09.Text = "";
                                txt07.SetFocus();
                                msgBar.Text = strMsg01;
                                return;
                            }
                            else
                            {
                                if (DateTime.Today < dateTime)
                                {
                                    txt07.Text = "";
                                    txt08.Text = "";
                                    txt09.Text = "";
                                    txt07.SetFocus();
                                    msgBar.Text = strMsg01;
                                    return;
                                }
                            }

                            //수표 조회
                            GetServerPV03();
                        }
                        else
                        {
                            msgBar.Text = strMsg01;
                        }
                    }
                }
                else if (txt01.Focused)
                {
                    if (txt01.Text.Length == txt01.MaxLength)
                    {
                        txt02.SetFocus();
                    }
                }
                else if (txt02.Focused)
                {
                    if (txt02.Text.Length == txt02.MaxLength)
                    {
                        txt03.SetFocus();
                    }
                }
                else if (txt03.Focused)
                {
                    if (txt03.Text.Length == txt03.MaxLength)
                    {
                        txt04.SetFocus();
                    }
                }
                else if (txt05.Focused)
                {
                    if (txt05.Text.Length == txt05.MaxLength &&
                        (txt05.Text == "13" || txt05.Text == "14" || txt05.Text == "15" || txt05.Text == "16" || txt05.Text == "19"))
                    {
                        if (txt05.Text == "19")
                        {
                            txt06.SetFocus();
                        }
                        else
                        {
                            txt07.SetFocus();
                        }
                    }
                }
                else if (txt07.Focused)
                {
                    if (txt07.Text.Length == txt07.MaxLength)
                    {
                        txt08.SetFocus();
                    }
                }
                else if (txt08.Focused)
                {
                    if (txt08.Text.Length == txt08.MaxLength)
                    {
                        txt09.SetFocus();
                    }
                }

                #endregion
            }
            else if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_CLEAR)
            {
                msgBar.Text = "";
                msgBar.ForeColor = Color.FromArgb(51, 51, 51);

                #region Clear

                if (txt01.IsFocused)
                {
                    txt01.SetFocus();
                    SetClearControl(e, txt01, null);
                }
                else if (txt02.IsFocused)
                {
                    txt02.SetFocus();
                    SetClearControl(e, txt02, txt01);
                }
                else if (txt03.IsFocused)
                {
                    txt03.SetFocus();
                    SetClearControl(e, txt03, txt02);
                }
                else if (txt04.IsFocused)
                {
                    txt04.SetFocus();
                    SetClearControl(e, txt04, txt03);
                }
                else if (txt05.IsFocused)
                {
                    txt05.SetFocus();
                    SetClearControl(e, txt05, txt04);
                }
                else if (txt06.IsFocused)
                {
                    if (txt05.Text.Length > 0 && txt05.Text == "19")
                    {
                        txt06.SetFocus();
                        SetClearControl(e, txt06, txt05);
                    }
                    else
                    {
                        e.IsHandled = true;
                    }
                }
                else if (txt07.IsFocused)
                {
                    txt07.SetFocus();
                    SetClearControl(e, txt07, null);
                }
                else if (txt08.IsFocused)
                {
                    txt08.SetFocus();
                    SetClearControl(e, txt08, txt07);
                }
                else if (txt09.IsFocused)
                {
                    txt09.SetFocus();
                    SetClearControl(e, txt09, txt08);
                }

                #endregion
            }
            else if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_BKS)
            {
                msgBar.Text = "";
                msgBar.ForeColor = Color.FromArgb(51, 51, 51);

                if (txt01.IsFocused)
                {
                    txt01.SetFocus();
                }
                else if (txt02.IsFocused)
                {
                    txt02.SetFocus();
                }
                else if (txt03.IsFocused)
                {
                    txt03.SetFocus();
                }
                else if (txt04.IsFocused)
                {
                    txt04.SetFocus();
                }
                else if (txt05.IsFocused)
                {
                    txt06.Text = string.Empty;
                    txt05.SetFocus();
                }
                else if (txt06.IsFocused)
                {
                    if (txt05.Text.Length > 0 && txt05.Text == "19")
                    {
                        txt06.SetFocus();
                    }
                    else
                    {
                        e.IsHandled = true;
                    }
                }
                else if (txt07.IsFocused)
                {
                    txt07.SetFocus();
                }
                else if (txt08.IsFocused)
                {
                    txt08.SetFocus();
                }
                else if (txt09.IsFocused)
                {
                    txt09.SetFocus();
                }
            }
            else if (!e.IsControlKey)
            {
                #region 숫자 입력

                if (txt01.IsFocused)
                {
                    ValidationLength(e, txt01, txt02);
                }
                else if (txt02.IsFocused)
                {
                    ValidationLength(e, txt02, txt03);
                }
                else if (txt03.IsFocused)
                {
                    ValidationLength(e, txt03, txt04);
                }
                else if (txt04.IsFocused)
                {
                    ValidationLength(e, txt04, txt05);
                }
                else if (txt05.IsFocused)
                {
                    ValidationLength(e, txt05, null);
                }
                else if (txt06.IsFocused)
                {
                    if (txt05.Text.Length > 0 && (txt05.Text == "13" || txt05.Text == "14" || txt05.Text == "15" || txt05.Text == "16" || txt05.Text == "19"))
                    {
                        ValidationLength(e, txt06, txt07);    
                    }
                    else
                    {
                        e.IsHandled = true;
                    }
                }
                else if (txt07.IsFocused)
                {
                    ValidationLength(e, txt07, txt08);
                }
                else if (txt08.IsFocused)
                {
                    ValidationLength(e, txt08, txt09);
                }
                else if (txt09.IsFocused)
                {
                    ValidationLength(e, txt09, null);
                }

                #endregion
            }
        }

        /// <summary>
        /// 텍스트박스 포커스 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txt_InputFocused(object sender, EventArgs e)
        {
            InputText txt = (InputText)sender;

            msgBar.Text = "";
            msgBar.ForeColor = Color.FromArgb(51, 51, 51);

            if (txt.Name.ToString() == txt01.Name.ToString())
            {
                msgBar.Text = strMsg02;
            }
            else if (txt.Name.ToString() == txt02.Name.ToString())
            {
                msgBar.Text = strMsg03;
            }
            else if (txt.Name.ToString() == txt03.Name.ToString())
            {
                msgBar.Text = strMsg04;
            }
            else if (txt.Name.ToString() == txt04.Name.ToString())
            {
                msgBar.Text = strMsg05;
            }
            else if (txt.Name.ToString() == txt05.Name.ToString())
            {
                msgBar.Text = strMsg06;
            }
            else if (txt.Name.ToString() == txt06.Name.ToString())
            {
                msgBar.Text = strMsg07;
            }
            else if (txt.Name.ToString() == txt07.Name.ToString() ||
                txt.Name.ToString() == txt08.Name.ToString() ||
                txt.Name.ToString() == txt09.Name.ToString())
            {
                msgBar.Text = strMsg08;
            }
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// InputControl Clear 클릭
        /// </summary>
        /// <param name="txt">현재 포커스 inputControl</param>
        /// <param name="txtFocus">이전 포커스 inputControl</param>
        /// <param name="strMsg">이전 포커스 이동시 메세지창 내용</param>
        private void SetClearControl(OPOSKeyEventArgs e, InputText txt, InputText txtFocus)
        {
            if (txt.Text.Length <= 0)
            {
                if (txt.Name.ToString() == txt07.Name.ToString())
                {
                    if (txt05.Text.ToString() == "19")
                    {
                        txtFocus = txt06;
                    }
                    else
                    {
                        txtFocus = txt05;
                    }
                }
                else if (txt.Name.ToString() == txt06.Name.ToString())
                {
                    txt07.Text = string.Empty;
                }

                if (txtFocus != null)
                {
                    txtFocus.SetFocus();
                }

                e.IsHandled = true;
            }
            else
            {
                if (txt.Name.ToString() == txt05.Name.ToString())
                {
                    txt06.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// InputControl 자릿수 유효성 검사 및 포커스 지정과 메시지 설정
        /// </summary>
        /// <param name="e"></param>
        /// <param name="txt">현재 포커스 inputControl</param>
        /// <param name="txtFocus">다음 포커스 inputControl</param>
        private void ValidationLength(OPOSKeyEventArgs e, InputText txt, InputText txtFocus)
        {
            if (txt.Text.Length + 1 == txt.MaxLength)
            {
                txt.Text = txt.Text + e.KeyCodeText.ToString();

                if (txt.Name.ToString() == txt05.Name.ToString())
                {
                    if (txt.Text == "13" || txt.Text == "14" || txt.Text == "15" || txt.Text == "16")
                    {
                        switch (txt.Text.ToString())
                        {
                            case "13":
                                txt06.Text = strMsg10;
                                break;
                            case "14":
                                txt06.Text = strMsg11;
                                break;
                            case "15":
                                txt06.Text = strMsg12;
                                break;
                            case "16":
                                txt06.Text = strMsg13;
                                break;
                            default:
                                break;
                        }

                        txt07.SetFocus();
                    }
                    else if (txt.Text == "19")
                    {
                        txt06.SetFocus();
                    }
                }

                if (txtFocus != null)
                {
                    txtFocus.SetFocus();
                }

                e.IsHandled = true;
            }
        }

        /// <summary>
        /// 전문통신(PV03)
        /// </summary>
        private void GetServerPV03()
        {
            if (_bDisable) return;

            SetControlDisable(true);
            //ChildManager.ShowProgress(true);

            PV03ReqData reqData = new PV03ReqData();
            reqData.CancType = "0";                                                     //취소구분 0:승인
            reqData.ChequeType = "00";                                                  //수표구분(00:자기앞 . 01:가계수표, 02:당좌수표)
            reqData.ChequeNo = txt01.Text.ToString();                                   //수표번호
            reqData.IssBankCode = txt02.Text.ToString();                                //발행은행코드
            reqData.IssBranCode = txt03.Text.ToString();                                //발행지점코드
            reqData.CheqTypeCode = txt05.Text.ToString();                               //권종코드(13:10만원 14:30만원,15:50만원,16:100만원,19:기타)
            reqData.ChequeAmt = txt06.Text.ToString();                                  //수표금액
            reqData.IssueDate = string.Format("{0}{1}{2}", 
                txt07.Text.ToString(), txt08.Text.ToString(), txt09.Text.ToString());   //발행일
            reqData.AccSeqNo = txt04.Text.ToString().Length <= 0 ? "000000" : txt04.Text.ToString();    //계좌일련번호(단위 농협/수협일 경우, 그외 Space)

            var pv03 = new PV03DataTask(reqData);
            pv03.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pv03_TaskCompleted);
            pv03.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pv03_Errored);
            pv03.ExecuteTask();
        }

        #region 전문조회

        /// <summary>
        /// PV03전문통신 완료 이벤트
        /// </summary>
        /// <param name="responseData"></param>
        void pv03_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            SetControlDisable(false);
            //ChildManager.ShowProgress(false);

            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                var data = responseData.DataRecords.ToDataRecords<PV03RespData>();

                if (data.Length > 0)
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate()
                        {
                            if (data[0].RespCode != null && data[0].RespCode.Length > 0 && data[0].RespCode.ToString() == "0000")
                            {
                                //정상수표
                                msgBar.Text = strMsg09;
                                msgBar.ForeColor = Color.FromArgb(51, 51, 51);
                            }
                            else
                            {
                                string strTemp01 = data[0].RespMessage1 != null && data[0].RespMessage1.Length > 0 ? data[0].RespMessage1.Trim() : "";
                                string strTemp02 = data[0].RespMessage2 != null && data[0].RespMessage2.Length > 0 ? data[0].RespMessage2.Trim() : "";
                                msgBar.Text = strTemp01.Length > 0 ? (strTemp02.Length > 0 ? string.Format("{0}\n{1}", strTemp01, strTemp02) : strTemp01) : strTemp02;
                                msgBar.ForeColor = Color.FromArgb(215, 58, 58);
                            }
                        });
                    }
                    else
                    {
                        if (data[0].RespCode != null && data[0].RespCode.Length > 0 && data[0].RespCode.ToString() == "0000")
                        {
                            //정상수표
                            msgBar.Text = strMsg09;
                            msgBar.ForeColor = Color.FromArgb(51, 51, 51);
                        }
                        else
                        {
                            string strTemp01 = data[0].RespMessage1 != null && data[0].RespMessage1.Length > 0 ? data[0].RespMessage1.Trim() : "";
                            string strTemp02 = data[0].RespMessage2 != null && data[0].RespMessage2.Length > 0 ? data[0].RespMessage2.Trim() : "";

                            msgBar.Text = strTemp01.Length > 0 ? (strTemp02.Length > 0 ? string.Format("{0}\n{1}", strTemp01, strTemp02) : strTemp01) : strTemp02;
                            msgBar.ForeColor = Color.FromArgb(215, 58, 58);
                        }
                    }
                }
            }
            else if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.NoInfo)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {

                        msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        msgBar.ForeColor = Color.FromArgb(215, 58, 58);
                        
                    });
                }
                else
                {
                    msgBar.Text = responseData.Response.ErrorMessage.ToString();
                    msgBar.ForeColor = Color.FromArgb(215, 58, 58);
                }
            }
            else
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        msgBar.ForeColor = Color.FromArgb(215, 58, 58);
                    });
                }
                else
                {
                    msgBar.Text = responseData.Response.ErrorMessage.ToString();
                    msgBar.ForeColor = Color.FromArgb(215, 58, 58);
                }
            }
        }

        /// <summary>
        /// PV03전문통신 오류 이벤트
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="lastException"></param>
        void pv03_Errored(string errorMessage, Exception lastException)
        {
            SetControlDisable(false);
            
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    msgBar.Text = strMsg15;
                    msgBar.ForeColor = Color.FromArgb(215, 58, 58);
                });
            }
            else
            {
                msgBar.Text = strMsg15;
                msgBar.ForeColor = Color.FromArgb(215, 58, 58);
            }
        }

        #endregion

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable/Disable
        /// </summary>
        void SetControlDisable(bool bDisable)
        {
            ChildManager.ShowProgress(bDisable);
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

                //Application.DoEvents();
            }

            
        }

        #endregion
    }
}
