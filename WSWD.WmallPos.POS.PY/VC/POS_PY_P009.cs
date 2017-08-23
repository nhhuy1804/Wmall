//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P009.cs
 * 화면설명 : 포인트 사용
 * 개발자   : 정광호
 * 개발일자 : 2015.04.21
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
using System.Diagnostics;

namespace WSWD.WmallPos.POS.PY.VC
{
    public partial class POS_PY_P009 : PopupBase01
    {
        #region 변수

        /// <summary>
        /// 받을돈
        /// </summary>
        private int _iGetAmt = 0;

        /// <summary>
        /// 반품모드
        /// </summary>
        private bool _modeReturn = false;

        /// <summary>
        /// 회원정보
        /// </summary>
        private PP01RespData _PP01RespData = null;

        /// <summary>
        /// 회원정보 유무
        /// </summary>
        private bool bCust = false;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        #endregion

        #region 생성자

        /// <summary>
        /// 포인트 사용
        /// </summary>
        /// <param name="iGetAmt">받을돈</param>
        /// <param name="PP01RespData">회원정보</param>
        public POS_PY_P009(int iGetAmt, PP01RespData PP01RespData, bool modeReturn)
        {
            InitializeComponent();

            //받을돈
            _iGetAmt = iGetAmt;

            // 반품여부
            _modeReturn = modeReturn;
            this.Text = this.Text + (modeReturn ? TITLE_CANCEL : string.Empty);

            //회원정보

            // BY KHJ 2016/04/11
            //_PP01RespData = PP01RespData;
            //

            // BY KHJ 2016/04/11
            _PP01RespData = null;
            //

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
            this.FormClosed += new FormClosedEventHandler(POS_PY_P009_FormClosed);
            this.KeyEvent += new OPOSKeyEventHandler(form_KeyEvent);                                 //Key Event
            this.btnSave.Click += new EventHandler(btnSave_Click);                                          //적용 button Event
            this.btnClose.Click += new EventHandler(btnClose_Click);                                        //닫기 button Event

            if (POSDeviceManager.Scanner != null)
            {
                POSDeviceManager.Scanner.DataEvent += new POSDataEventHandler(Scanner_DataEvent);               //Scanner Event    
            }
        }

        void POS_PY_P009_FormClosed(object sender, FormClosedEventArgs e)
        {
            Load -= new EventHandler(form_Load);
            this.KeyEvent -= new OPOSKeyEventHandler(form_KeyEvent);                                 //Key Event
            this.btnSave.Click -= new EventHandler(btnSave_Click);                                          //적용 button Event
            this.btnClose.Click -= new EventHandler(btnClose_Click);   
            if (POSDeviceManager.Scanner != null)
            {
                POSDeviceManager.Scanner.DataEvent -= new POSDataEventHandler(Scanner_DataEvent);               //Scanner Event    
            }
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
            txtGetAmt.Text = _iGetAmt.ToString();

            if (_PP01RespData != null && _PP01RespData.Length > 0 && _PP01RespData.CardNo.Length > 0)
            {
                bCust = true;
                txtCardNo.Text = _PP01RespData.CardNo;
                txtCustName.Text = _PP01RespData.CustName;
                txtGradeName.Text = _PP01RespData.GradeName;
                txtAbtyPoint.Text = _PP01RespData.AbtyPoint;
                txtPaymentAmt.SetFocus();
            }
            else
            {
                txtCardNo.SetFocus();
            }

            //화면 표출 메세지 설정
            StatusMessage = txtCardNo.Text.Length <= 0 ? strMsg01 : strMsg02;
        }

        /// <summary>
        /// Key Event
        /// </summary>
        /// <param name="e"></param>
        void form_KeyEvent(OPOSKeyEventArgs e)
        {
            if (_bDisable)
            {
                e.IsHandled = true;
                return;
            }

            if (e.Key.OPOSKey == OPOSMapKeys.KEY_NOSALE)
            {
                if (POSDeviceManager.CashDrawer != null && POSDeviceManager.CashDrawer.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened && POSDeviceManager.CashDrawer.Enabled)
                {
                    //돈통 open
                    POSDeviceManager.CashDrawer.OpenDrawer();
                }
                return;
            }

            if (txtCardNo.IsFocused)
            {
                if (bCust)
                {
                    //기존 조회결과 회원번호가 존재시 전체 클리어

                    if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
                    {
                        bCust = false;
                        txtCardNo.Text = "";
                        txtCustName.Text = "";
                        txtGradeName.Text = "";
                        txtAbtyPoint.Text = "";
                        txtPaymentAmt.Text = "";

                        StatusMessage = strMsg01;
                    }
                    else
                    {
                        StatusMessage = strMsg04;
                    }

                    e.IsHandled = true;
                }
                else
                {
                    if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
                    {
                        if (txtCardNo.Text.Length <= 0)
                        {
                            txtCardNo.Text = "";
                            StatusMessage = strMsg03;
                        }
                        else
                        {
                            if (txtCardNo.Text.Length != 13)
                            {
                                txtCardNo.Text = "";
                                StatusMessage = strMsg03;
                            }
                            else
                            {
                                //전문통신(PP01)
                                GetServerRegister();
                            }
                        }
                    }
                    else if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
                    {
                        if (txtCardNo.Text.Length <= 0)
                        {
                            e.IsHandled = true;
                            this.DialogResult = DialogResult.Cancel;
                        }
                        else
                        {
                            txtCustName.Text = "";
                            txtGradeName.Text = "";
                            txtAbtyPoint.Text = "";
                            StatusMessage = strMsg01;
                        }
                    }
                    else if (e.Key.OPOSKey == OPOSMapKeys.KEY_BKS)
                    {
                        txtCustName.Text = "";
                        txtGradeName.Text = "";
                        txtAbtyPoint.Text = "";
                        StatusMessage = strMsg01;
                    }
                }
            }
            else if (txtPaymentAmt.IsFocused)
            {
                if (!bCust)
                {
                    txtCardNo.SetFocus();
                    e.IsHandled = true;
                }
                else
                {
                    if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
                    {
                        if (txtPaymentAmt.Text.Length <= 0)
                        {
                            txtCardNo.SetFocus();
                            StatusMessage = strMsg01;
                            e.IsHandled = true;
                        }
                        else
                        {
                            StatusMessage = strMsg02;
                        }
                    }
                    else if (e.Key.OPOSKey == OPOSMapKeys.KEY_BKS)
                    {
                        StatusMessage = strMsg02;
                    }
                    else if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
                    {
                        e.IsHandled = true;

                        btnSave_Click(btnSave, null);
                    }
                    else if (!e.IsControlKey)
                    {
                        StatusMessage = strMsg02;
                    }
                    else
                    {
                        StatusMessage = strMsg04;
                        e.IsHandled = true;
                    }
                }
            }
        }

        /// <summary>
        /// 적용 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (_bDisable || !bCust || txtCardNo.Text.Length <= 0 || txtPaymentAmt.Text.Length <= 0) return;

            txtPaymentAmt.Text = TypeHelper.RoundDown32(Convert.ToDecimal(txtPaymentAmt.Text)).ToString();    //원단위절사
            Application.DoEvents();

            if (TypeHelper.ToInt32(txtPaymentAmt.Text) <= 0)
            {
                StatusMessage = strMsg02;
                txtPaymentAmt.Text = "";
                txtPaymentAmt.SetFocus();
                return;
            }

            ChildManager.ShowProgress(true);
            SetControlDisable(true);

            try
            {
                //if (TypeHelper.ToInt32(txtPaymentAmt.Text) < 1000)
                //{
                //    StatusMessage = strMsg07;
                //    txtPaymentAmt.Text = "";
                //    txtPaymentAmt.SetFocus();
                //    ChildManager.ShowProgress(false);
                //    SetControlDisable(false);
                //    return;
                //}

                //받을돈 비교
                if (_iGetAmt < TypeHelper.ToInt32(txtPaymentAmt.Text))
                {
                    StatusMessage = strMsg05;
                    txtPaymentAmt.Text = "";
                    txtPaymentAmt.SetFocus();
                    ChildManager.ShowProgress(false);
                    SetControlDisable(false);
                    return;
                }

                ////가용점수 비교
                //if (TypeHelper.ToInt32(txtAbtyPoint.Text) < TypeHelper.ToInt32(txtPaymentAmt.Text))
                //{
                //    StatusMessage = strMsg06;
                //    txtPaymentAmt.Text = "";
                //    txtPaymentAmt.SetFocus();
                //    ChildManager.ShowProgress(false);
                //    SetControlDisable(false);
                //    return;
                //}

                ////1000점 단위 비교
                //if (TypeHelper.ToInt32(txtPaymentAmt.Text) % 1000 > 0)
                //{
                //    StatusMessage = strMsg07;
                //    txtPaymentAmt.Text = "";
                //    txtPaymentAmt.SetFocus();
                //    ChildManager.ShowProgress(false);
                //    SetControlDisable(false);
                //    return;
                //}

                var pp04 = new PP04DataTask(_modeReturn ? "1" : "0", "", "", _PP01RespData.CustNo, txtCardNo.Text, txtPaymentAmt.Text); //TEST ->"2701900057818"
                pp04.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pp04_TaskCompleted);
                pp04.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pp04_Errored);
                pp04.ExecuteTask();
            }
            catch (Exception)
            {
                ChildManager.ShowProgress(false);
                SetControlDisable(false);
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

            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Scanner Event
        /// </summary>
        /// <param name="msrData"></param>
        void Scanner_DataEvent(string msrData)
        {
            Trace.WriteLine("PY_P009_Scanner_DataEvent " + msrData, "program");

            if (_bDisable) return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    _PP01RespData = null;
                    bCust = false;
                    txtCardNo.Text = "";
                    txtCustName.Text = "";
                    txtGradeName.Text = "";
                    txtAbtyPoint.Text = "";

                    txtCardNo.Text = msrData;
                    txtPaymentAmt.Text = "";
                    Application.DoEvents();

                    //회원번호 입력
                    if (txtCardNo.Text.Length != 13)
                    {
                        txtCardNo.Text = "";
                        StatusMessage = strMsg03;
                    }
                    else
                    {
                        //전문통신(PP01)
                        GetServerRegister();
                    }
                });
            }
            else
            {
                _PP01RespData = null;
                bCust = false;
                txtCardNo.Text = "";
                txtCustName.Text = "";
                txtGradeName.Text = "";
                txtAbtyPoint.Text = "";

                txtCardNo.Text = msrData;
                txtPaymentAmt.Text = "";
                Application.DoEvents();

                //회원번호 입력
                if (txtCardNo.Text.Length != 13)
                {
                    txtCardNo.Text = "";
                    StatusMessage = strMsg03;
                }
                else
                {
                    //전문통신(PP01)
                    GetServerRegister();
                }
            }
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// 전문통신(PP01)
        /// </summary>
        private void GetServerRegister()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    if (_bDisable || txtCardNo.Text.Length != 13) return;

                    ChildManager.ShowProgress(true);
                    SetControlDisable(true);

                    bCust = false;
                    _PP01RespData = null;

                    var pp01 = new PP01DataTask(txtCardNo.Text); //TEST ->"2701900057818"
                    pp01.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pp01_TaskCompleted);
                    pp01.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pp01_Errored);
                    pp01.ExecuteTask();
                });
            }
            else
            {
                if (_bDisable || txtCardNo.Text.Length != 13) return;

                ChildManager.ShowProgress(true);
                SetControlDisable(true);

                bCust = false;
                _PP01RespData = null;

                var pp01 = new PP01DataTask(txtCardNo.Text); //TEST ->"2701900057818"
                pp01.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pp01_TaskCompleted);
                pp01.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pp01_Errored);
                pp01.ExecuteTask();
            }
        }

        /// <summary>
        /// PP01전문통신 완료 이벤트
        /// </summary>
        /// <param name="responseData"></param>
        void pp01_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            ChildManager.ShowProgress(false);

            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                var data = responseData.DataRecords.ToDataRecords<PP01RespData>();
                if (data.Length > 0)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke((MethodInvoker)delegate()
                        {
                            _PP01RespData = data[0];
                            txtCardNo.Text = data[0].CardNo;
                            txtCustName.Text = data[0].CustName;
                            txtGradeName.Text = data[0].GradeName;
                            txtAbtyPoint.Text = data[0].AbtyPoint;
                            bCust = true;

                            StatusMessage = strMsg02;
                            txtPaymentAmt.SetFocus();
                            txtPaymentAmt.Focus();
                            SetControlDisable(false);
                            Application.DoEvents();
                        });
                    }
                    else
                    {
                        _PP01RespData = data[0];
                        txtCardNo.Text = data[0].CardNo;
                        txtCustName.Text = data[0].CustName;
                        txtGradeName.Text = data[0].GradeName;
                        txtAbtyPoint.Text = data[0].AbtyPoint;
                        bCust = true;

                        StatusMessage = strMsg02;
                        txtPaymentAmt.SetFocus();
                        txtPaymentAmt.Focus();
                        SetControlDisable(false);
                        Application.DoEvents();
                    }
                }
            }
            else if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.NoInfo)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        _PP01RespData = null;
                        bCust = false;
                        txtCardNo.Text = "";
                        txtCustName.Text = "";
                        txtGradeName.Text = "";
                        txtAbtyPoint.Text = "";
                        txtCardNo.SetFocus();
                        StatusMessage = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false);
                        Application.DoEvents();
                    });
                }
                else
                {
                    _PP01RespData = null;
                    bCust = false;
                    txtCardNo.Text = "";
                    txtCustName.Text = "";
                    txtGradeName.Text = "";
                    txtAbtyPoint.Text = "";
                    txtCardNo.SetFocus();
                    StatusMessage = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false);
                    Application.DoEvents();
                }
            }
            else
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        _PP01RespData = null;
                        bCust = false;
                        txtCardNo.Text = "";
                        txtCustName.Text = "";
                        txtGradeName.Text = "";
                        txtAbtyPoint.Text = "";
                        txtCardNo.SetFocus();
                        StatusMessage = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false);
                        Application.DoEvents();
                    });
                }
                else
                {
                    _PP01RespData = null;
                    bCust = false;
                    txtCardNo.Text = "";
                    txtCustName.Text = "";
                    txtGradeName.Text = "";
                    txtAbtyPoint.Text = "";
                    txtCardNo.SetFocus();
                    StatusMessage = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false);
                    Application.DoEvents();
                }
            }
        }

        /// <summary>
        /// PP01전문통신 오류
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="lastException"></param>
        void pp01_Errored(string errorMessage, Exception lastException)
        {
            ChildManager.ShowProgress(false);
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    _PP01RespData = null;
                    bCust = false;
                    txtCardNo.Text = "";
                    txtCustName.Text = "";
                    txtGradeName.Text = "";
                    txtAbtyPoint.Text = "";
                    txtCardNo.SetFocus();
                    StatusMessage = strMsg11;
                    SetControlDisable(false);
                    Application.DoEvents();
                });
            }
            else
            {
                _PP01RespData = null;
                bCust = false;
                txtCardNo.Text = "";
                txtCustName.Text = "";
                txtGradeName.Text = "";
                txtAbtyPoint.Text = "";
                txtCardNo.SetFocus();
                StatusMessage = strMsg11;
                SetControlDisable(false);
                Application.DoEvents();
            }
        }

        /// <summary>
        /// PP04전문통신 완료 이벤트
        /// </summary>
        /// <param name="responseData"></param>
        void pp04_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            ChildManager.ShowProgress(false);

            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                var data = responseData.DataRecords.ToDataRecords<PP04RespData>();
                if (data.Length > 0)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke((MethodInvoker)delegate()
                        {
                            // 포인트사용 TRAN (결제-현금 TRAN 사용)
                            BasketPoint pointAmt = new BasketPoint();
                            pointAmt.BasketType = BasketTypes.BasketPay;
                            pointAmt.PayGrpCd = NetCommConstants.PAYMENT_GROUP_POINT;
                            pointAmt.PayDtlCd = NetCommConstants.PAYMENT_DETAIL_POINT;
                            pointAmt.PayAmt = txtPaymentAmt.Text.ToString();
                            pointAmt.BalGrpCd = NetCommConstants.PAYMENT_GROUP_NONE;
                            pointAmt.BalDtlCd = NetCommConstants.PAYMENT_DETAIL_NONE;
                            pointAmt.CancFg = _modeReturn ? NetCommConstants.CANCEL_TYPE_CANCEL : NetCommConstants.CANCEL_TYPE_NORMAL;
                            pointAmt.CardNo = txtCardNo.Text.ToString();
                            pointAmt.CustNm = txtCustName.Text.ToString();
                            pointAmt.UsePoint = txtPaymentAmt.Text.ToString();
                            pointAmt.BalancePoint = data[0].PayAfterPoint;
                            pointAmt.BalanceAmt = data[0].PayAfterAmt;
                            pointAmt.ApprovalNo = data[0].ApprNo;
                            pointAmt.CustNo = _PP01RespData.CustNo;

                            this.ReturnResult.Add("PAY_DATA", pointAmt);
                            this.DialogResult = DialogResult.OK;
                            SetControlDisable(false);
                            Application.DoEvents();
                        });
                    }
                    else
                    {
                        // 포인트사용 TRAN (결제-현금 TRAN 사용)
                        BasketPoint pointAmt = new BasketPoint();
                        pointAmt.BasketType = BasketTypes.BasketPay;
                        pointAmt.PayGrpCd = NetCommConstants.PAYMENT_GROUP_POINT;
                        pointAmt.PayDtlCd = NetCommConstants.PAYMENT_DETAIL_POINT;
                        pointAmt.PayAmt = txtPaymentAmt.Text.ToString();
                        pointAmt.BalGrpCd = NetCommConstants.PAYMENT_GROUP_NONE;
                        pointAmt.BalDtlCd = NetCommConstants.PAYMENT_DETAIL_NONE;
                        //_modeReturn ? NetCommConstants.CANCEL_TYPE_CANCEL : NetCommConstants.CANCEL_TYPE_NORMAL;
                        pointAmt.CancFg = NetCommConstants.CANCEL_TYPE_NORMAL;  
                        pointAmt.CardNo = txtCardNo.Text.ToString();
                        pointAmt.CustNm = txtCustName.Text.ToString();
                        pointAmt.UsePoint = txtPaymentAmt.Text.ToString();
                        pointAmt.BalancePoint = data[0].PayAfterPoint;
                        pointAmt.BalanceAmt = data[0].PayAfterAmt;
                        pointAmt.ApprovalNo = data[0].ApprNo;
                        pointAmt.CustNo = _PP01RespData.CustNo;

                        this.ReturnResult.Add("PAY_DATA", pointAmt);
                        this.DialogResult = DialogResult.OK;
                        SetControlDisable(false);
                        Application.DoEvents();
                    }
                }
            }
            else if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.NoInfo)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        StatusMessage = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false);
                        Application.DoEvents();
                    });
                }
                else
                {
                    StatusMessage = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false);
                    Application.DoEvents();
                }
            }
            else
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        StatusMessage = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false);
                        Application.DoEvents();
                    });
                }
                else
                {
                    StatusMessage = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false);
                    Application.DoEvents();
                }
            }
        }

        /// <summary>
        /// PP04전문통신 오류
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="lastException"></param>
        void pp04_Errored(string errorMessage, Exception lastException)
        {
            ChildManager.ShowProgress(false);

            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    StatusMessage = strMsg11;
                    SetControlDisable(false);
                    Application.DoEvents();
                });
            }
            else
            {
                StatusMessage = strMsg11;
                SetControlDisable(false);
                Application.DoEvents();
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
                    foreach (var item in this.ContainerPanel.Controls)
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
                        else if (item.GetType().Name.ToString().ToLower() == "panel")
                        {
                            Panel pnl = (Panel)item;
                            pnl.Enabled = !_bDisable;
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "gridpanel")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel grd = (WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel)item;
                            grd.Enabled = !_bDisable;
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "panel")
                        {
                            Panel pnl = (Panel)item;
                            pnl.Enabled = !_bDisable;
                        }
                    }
                });
            }
            else
            {
                foreach (var item in this.ContainerPanel.Controls)
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
                    else if (item.GetType().Name.ToString().ToLower() == "panel")
                    {
                        Panel pnl = (Panel)item;
                        pnl.Enabled = !_bDisable;
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "gridpanel")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel grd = (WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel)item;
                        grd.Enabled = !_bDisable;
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "panel")
                    {
                        Panel pnl = (Panel)item;
                        pnl.Enabled = !_bDisable;
                    }
                }
            }

            //Application.DoEvents();
        }

        #endregion
    }
}
