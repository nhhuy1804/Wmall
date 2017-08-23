//-----------------------------------------------------------------
/*
 * 화면명   : POS_PT_P001.cs
 * 화면설명 : 포인트 조회
 * 개발자   : 정광호
 * 개발일자 : 2015.04.06
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

using WSWD.WmallPos.POS.PT.PI;
using WSWD.WmallPos.POS.PT.PT;
using WSWD.WmallPos.POS.PT.VI;
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
using System.Diagnostics;

namespace WSWD.WmallPos.POS.PT.VC
{
    public partial class POS_PT_P001 : PopupBase
    {
        #region 변수

        /// <summary>
        /// 회원번호
        /// </summary>
        private string strCustNo = string.Empty;

        /// <summary>
        /// 회원정보
        /// </summary>
        private PP01RespData _returnData = null;

        /// <summary>
        /// 입력형태
        /// </summary>
        private string strInputWcc = string.Empty;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        #endregion

        #region 생성자

        public POS_PT_P001(PP01RespData returnData)
        {
            InitializeComponent();

            //회원정보
            _returnData = returnData;

            //Form Load Event
            Load += new EventHandler(form_Load); 
        }

        public POS_PT_P001()
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
            this.FormClosed += new FormClosedEventHandler(form_FormClosed);
            this.KeyEvent += new OPOSKeyEventHandler(form_KeyEvent);                                        //Key Event
            this.btnCard.Click += new EventHandler(btnCard_Click);                                          //지류카드 button Event
            this.btnSave.Click += new EventHandler(btnSave_Click);                                          //적용 button Event
            this.btnClose.Click += new EventHandler(btnClose_Click);                                        //닫기 button Event

            if (POSDeviceManager.Scanner != null)
            {
                POSDeviceManager.Scanner.DataEvent += new POSDataEventHandler(Scanner_DataEvent);               //Scanner Event    
            }

            txtType.InputFocused += new EventHandler(txt_InputFocused);
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

            //SignPad
            if (POSDeviceManager.SignPad != null)
            {
                POSDeviceManager.SignPad.Initialize(this.axKSNet_Dongle1);
                POSDeviceManager.SignPad.PinEvent += new POSDataEventHandler(SignPad_PinEvent);
            }

            strCustNo = "";

            //구분 입력 포커스 활성
            txtType.SetFocus();

            //화면 표출 메세지 설정
            msgBar.Text = strMsg01;

            if (_returnData != null)
            {
                txtType.Text = "1";
                txtCardNo.Text = _returnData.CardNo;
                txtCustName.Text = _returnData.CustName;
                txtGradeName.Text = _returnData.GradeName;
                txtDelayPoint.Text = _returnData.DelayPoint;
                txtAbtyPoint.Text = _returnData.AbtyPoint;
                txtCltePoint.Text = _returnData.CltePoint;
                txtRemark.Text = _returnData.Remark;
                strCustNo = _returnData.CardNo;
                msgBar.Text = strMsg09;
            }
        }

        /// <summary>
        /// form close Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Load -= new EventHandler(form_Load);

            this.FormClosed -= new FormClosedEventHandler(form_FormClosed);
            this.KeyEvent -= new OPOSKeyEventHandler(form_KeyEvent);                                        //Key Event
            this.btnCard.Click -= new EventHandler(btnCard_Click);                                          //지류카드 button Event
            this.btnSave.Click -= new EventHandler(btnSave_Click);                                          //적용 button Event
            this.btnClose.Click -= new EventHandler(btnClose_Click);                                        //닫기 button Event
            this.txtType.InputFocused -= new EventHandler(txt_InputFocused);

            if (POSDeviceManager.SignPad.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
            {
                POSDeviceManager.SignPad.PinEvent -= new POSDataEventHandler(SignPad_PinEvent);
                POSDeviceManager.SignPad.Close();    
            }

            if (POSDeviceManager.Scanner != null)
            {
                POSDeviceManager.Scanner.DataEvent -= new POSDataEventHandler(Scanner_DataEvent);               //Scanner Event    
            }
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

            if (strCustNo != null && strCustNo.Length > 0)
            {
                //기존 조회결과 회원번호가 존재시 전체 클리어
                e.IsHandled = true;

                if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
                {
                    txtType.SetFocus();
                    txtType.Text = "";
                    txtCardNo.Text = "";
                    txtCustName.Text = "";
                    txtGradeName.Text = "";
                    txtDelayPoint.Text = "";
                    txtAbtyPoint.Text = "";
                    txtCltePoint.Text = "";
                    txtRemark.Text = "";
                    strCustNo = "";

                    msgBar.Text = strMsg01;
                }
                else if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
                {
                    btnSave_Click(btnSave, null);
                    return;

                }
                else
                {
                    msgBar.Text = strMsg02;
                }
            }
            else
            {
                if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
                {
                    #region KEY_ENTER

                    if (txtType.IsFocused)
                    {
                        if (txtType.Text.Length <= 0)
                        {
                            msgBar.Text = strMsg03;
                        }
                        else
                        {
                            if (txtType.Text == "1" || txtType.Text == "2")
                            {
                                txtCardNo.Text = "";
                                txtCardNo.SetFocus();
                                msgBar.Text = txtType.Text == "1" ? strMsg04 : strMsg05;

                                if (txtType.Text == "2" && POSDeviceManager.SignPad != null)
                                {
                                    POSDeviceManager.SignPad.RequestPinData(strMsg06, "", "", "", 1, 13);
                                }
                            }
                            else
                            {
                                txtType.Text = "";
                                msgBar.Text = strMsg03;
                            }
                        }
                    }
                    else if (txtCardNo.IsFocused)
                    {
                        if (txtCardNo.Text.Length <= 0)
                        {
                            txtCardNo.Text = "";
                            msgBar.Text = strMsg03;

                            if (txtType.Text == "2")
                            {
                                POSDeviceManager.SignPad.ClearPinDataRequest();
                            }
                        }
                        else
                        {
                            if (txtType.Text == "1")
                            {
                                //카드번호 입력    
                                if (txtCardNo.Text.Length != 13)
                                {
                                    txtCardNo.Text = "";
                                    msgBar.Text = strMsg03;
                                }
                                else
                                {
                                    strInputWcc = "@";

                                    //전문통신(PP01)
                                    GetServerRegister("1", txtCardNo.Text);
                                }
                            }
                            else if (txtType.Text == "2")
                            {
                                POSDeviceManager.SignPad.ClearPinDataRequest();

                                //핸폰번호 입력
                                if (txtCardNo.Text.Length > 11 || txtCardNo.Text.Length < 9)
                                {
                                    txtCardNo.Text = "";
                                    msgBar.Text = strMsg03;
                                }
                                else
                                {
                                    strInputWcc = "@";

                                    //전문통신(PP02)
                                    GetServerRegister("2", txtCardNo.Text);
                                }
                            }
                        }
                    }

                    #endregion
                }
                else if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
                {
                    #region KEY_CLEAR

                    if (txtType.IsFocused)
                    {
                        if (txtType.Text.Length <= 0)
                        {
                            e.IsHandled = true;
                            btnClose_Click(btnClose, null);
                            return;
                        }
                        else
                        {
                            msgBar.Text = strMsg01;
                        }
                    }
                    else if (txtCardNo.IsFocused)
                    {
                        if (txtCardNo.Text.Length > 0)
                        {
                            msgBar.Text = txtType.Text == "1" ? strMsg04 : strMsg05;
                        }
                        else
                        {
                            POSDeviceManager.SignPad.ClearPinDataRequest();
                            txtType.SetFocus();
                            msgBar.Text = strMsg01;
                            e.IsHandled = true;
                        }
                    }

                    #endregion
                }
            }
        }

        /// <summary>
        /// 텍스트박스 포커스
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txt_InputFocused(object sender, EventArgs e)
        {
            if (TypeHelper.ToString(txtType.Text).Length > 0 && TypeHelper.ToString(txtType.Text) == "2")
            {
                POSDeviceManager.SignPad.ClearPinDataRequest();
            }
        }

        /// <summary>
        /// 지류카드 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCard_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;

            try
            {
                ChildManager.ShowProgress(true);
                SetControlDisable(true);

                if (ChkPrint() && strCustNo != null && strCustNo.Length > 0)
                {
                    POSPrinterUtils.Instance.PrintPT_P001_CARD(strCustNo, txtCustName.Text);
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                SetControlDisable(false);
                ChildManager.ShowProgress(false);
            }
        }

        /// <summary>
        /// 적용 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;

            if (_returnData != null)
            {
                if (this.ReturnResult.ContainsKey("Cust"))
                {
                    this.ReturnResult["Cust"] = _returnData;
                }
                else
                {
                    this.ReturnResult.Add("Cust", _returnData);
                } 

                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
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
        void Scanner_DataEvent(string scannedData)
        {
            Trace.WriteLine("PT_P001_Scanner_DataEvent " + scannedData, "program");

            if (_bDisable) return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    _returnData = null;
                    txtCardNo.Text = "";
                    txtCustName.Text = "";
                    txtGradeName.Text = "";
                    txtDelayPoint.Text = "";
                    txtAbtyPoint.Text = "";
                    txtCltePoint.Text = "";
                    txtRemark.Text = "";
                    strCustNo = "";

                    if (scannedData.Length == 13 && txtType.Text != "2")
                    {
                        txtType.Text = "1";
                        txtCardNo.Text = scannedData;
                        txtCardNo.SetFocus();

                        strInputWcc = "A";

                        //전문통신(PP01)
                        GetServerRegister("1", txtCardNo.Text);
                    }
                    else
                    {
                        msgBar.Text = strMsg03;
                    }
                });
            }
            else
            {
                _returnData = null;
                txtCardNo.Text = "";
                txtCustName.Text = "";
                txtGradeName.Text = "";
                txtDelayPoint.Text = "";
                txtAbtyPoint.Text = "";
                txtCltePoint.Text = "";
                txtRemark.Text = "";
                strCustNo = "";

                if (scannedData.Length == 13 && txtType.Text != "2")
                {
                    txtType.Text = "1";
                    txtCardNo.Text = scannedData;
                    txtCardNo.SetFocus();

                    strInputWcc = "A";

                    //전문통신(PP01)
                    GetServerRegister("1", txtCardNo.Text);
                }
                else
                {
                    msgBar.Text = strMsg03;
                }
            }
        }

        /// <summary>
        /// SignPad Event
        /// </summary>
        /// <param name="msrData">전화번호 입력값</param>
        void SignPad_PinEvent(string msrData)
        {
            if (_bDisable) return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    _returnData = null;
                    txtCardNo.Text = "";
                    txtCustName.Text = "";
                    txtGradeName.Text = "";
                    txtDelayPoint.Text = "";
                    txtAbtyPoint.Text = "";
                    txtCltePoint.Text = "";
                    txtRemark.Text = "";
                    strCustNo = "";

                    txtCardNo.Text = msrData;

                    //핸폰번호 입력
                    if (txtCardNo.Text.Length > 11 || txtCardNo.Text.Length < 9)
                    {
                        txtCardNo.Text = "";
                        msgBar.Text = strMsg03;
                    }
                    else
                    {
                        strInputWcc = "@";

                        //전문통신(PP02)
                        GetServerRegister("2", txtCardNo.Text);
                    }
                });
            }
            else
            {
                _returnData = null;
                txtCardNo.Text = "";
                txtCustName.Text = "";
                txtGradeName.Text = "";
                txtDelayPoint.Text = "";
                txtAbtyPoint.Text = "";
                txtCltePoint.Text = "";
                txtRemark.Text = "";
                strCustNo = "";

                txtCardNo.Text = msrData;

                //핸폰번호 입력
                if (txtCardNo.Text.Length > 11 || txtCardNo.Text.Length < 9)
                {
                    txtCardNo.Text = "";
                    msgBar.Text = strMsg03;
                }
                else
                {
                    strInputWcc = "@";

                    //전문통신(PP02)
                    GetServerRegister("2", txtCardNo.Text);
                }
            }
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// 전문통신(PP01 or PP02)
        /// </summary>
        /// <param name="strType">전문구분</param>
        /// <param name="strNumber">입력번호</param>
        private void GetServerRegister(string strType, string strNumber)
        {
            ChildManager.ShowProgress(true);
            SetControlDisable(true);

            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        txtCustName.Text = "";
                        txtGradeName.Text = "";
                        txtDelayPoint.Text = "";
                        txtAbtyPoint.Text = "";
                        txtCltePoint.Text = "";
                        txtRemark.Text = "";
                        strCustNo = "";
                        msgBar.Text = strMsg15;
                    });
                }
                else
                {

                    txtCustName.Text = "";
                    txtGradeName.Text = "";
                    txtDelayPoint.Text = "";
                    txtAbtyPoint.Text = "";
                    txtCltePoint.Text = "";
                    txtRemark.Text = "";
                    strCustNo = "";
                    msgBar.Text = strMsg15;
                }

                _returnData = null;
                //Application.DoEvents();

                if (strType == "1")
                {
                    var pp01 = new PP01DataTask(strNumber); //TEST ->"2701900057818"
                    pp01.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pp01_TaskCompleted);
                    pp01.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pp01_Errored);
                    pp01.ExecuteTask();
                }
                else if (strType == "2")
                {
                    POSDeviceManager.SignPad.ClearPinDataRequest();

                    //전화번호
                    var pp02 = new PP02DataTask(strNumber); //TEST ->"01088673503"
                    pp02.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pp02_TaskCompleted);
                    pp02.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pp02_Errored);
                    pp02.ExecuteTask();
                }
            }
            catch (Exception)
            {
                ChildManager.ShowProgress(false);
                SetControlDisable(false);   
            }
        }

        /// <summary>
        /// PP01전문통신(카드번호) 완료 이벤트
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
                        this.BeginInvoke((MethodInvoker)delegate()
                        {
                            _returnData = responseData.DataRecords.ToDataRecords<PP01RespData>()[0];
                            txtCardNo.Text = data[0].CardNo;
                            txtCustName.Text = data[0].CustName;
                            txtGradeName.Text = data[0].GradeName;
                            txtDelayPoint.Text = data[0].DelayPoint;
                            txtAbtyPoint.Text = data[0].AbtyPoint;
                            txtCltePoint.Text = data[0].CltePoint;
                            txtRemark.Text = data[0].Remark;
                            strCustNo = data[0].CardNo;

                            if (_returnData != null)
                            {
                                _returnData.InputWcc = !string.IsNullOrEmpty(strInputWcc) ? strInputWcc : "";
                            }

                            msgBar.Text = strMsg09;
                            SetControlDisable(false);
                        });
                    }
                    else
                    {
                        _returnData = responseData.DataRecords.ToDataRecords<PP01RespData>()[0];
                        txtCardNo.Text = data[0].CardNo;
                        txtCustName.Text = data[0].CustName;
                        txtGradeName.Text = data[0].GradeName;
                        txtDelayPoint.Text = data[0].DelayPoint;
                        txtAbtyPoint.Text = data[0].AbtyPoint;
                        txtCltePoint.Text = data[0].CltePoint;
                        txtRemark.Text = data[0].Remark;
                        strCustNo = data[0].CardNo;

                        if (_returnData != null)
                        {
                            _returnData.InputWcc = !string.IsNullOrEmpty(strInputWcc) ? strInputWcc : "";
                        }

                        msgBar.Text = strMsg09;
                        SetControlDisable(false);
                    }
                }
            }
            else if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.NoInfo)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        _returnData = null;
                        txtCardNo.Text = "";
                        txtCustName.Text = "";
                        txtGradeName.Text = "";
                        txtDelayPoint.Text = "";
                        txtAbtyPoint.Text = "";
                        txtCltePoint.Text = "";
                        txtRemark.Text = "";
                        strCustNo = "";
                        txtCardNo.SetFocus();
                        msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false); 
                    });
                }
                else
                {
                    _returnData = null;
                    txtCardNo.Text = "";
                    txtCustName.Text = "";
                    txtGradeName.Text = "";
                    txtDelayPoint.Text = "";
                    txtAbtyPoint.Text = "";
                    txtCltePoint.Text = "";
                    txtRemark.Text = "";
                    strCustNo = "";
                    txtCardNo.SetFocus();
                    msgBar.Text = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false); 
                }
            }
            else
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        _returnData = null;
                        txtCardNo.Text = "";
                        txtCustName.Text = "";
                        txtGradeName.Text = "";
                        txtDelayPoint.Text = "";
                        txtAbtyPoint.Text = "";
                        txtCltePoint.Text = "";
                        txtRemark.Text = "";
                        strCustNo = "";
                        txtCardNo.SetFocus();
                        msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false);  
                    });
                }
                else
                {
                    _returnData = null;
                    txtCardNo.Text = "";
                    txtCustName.Text = "";
                    txtGradeName.Text = "";
                    txtDelayPoint.Text = "";
                    txtAbtyPoint.Text = "";
                    txtCltePoint.Text = "";
                    txtRemark.Text = "";
                    strCustNo = "";
                    txtCardNo.SetFocus();
                    msgBar.Text = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false);  
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
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    _returnData = null;
                    txtCardNo.Text = "";
                    txtCustName.Text = "";
                    txtGradeName.Text = "";
                    txtDelayPoint.Text = "";
                    txtAbtyPoint.Text = "";
                    txtCltePoint.Text = "";
                    txtRemark.Text = "";
                    strCustNo = "";

                    txtCardNo.SetFocus();
                    msgBar.Text = strMsg11;
                    SetControlDisable(false);  
                });
            }
            else
            {
                _returnData = null;
                txtCardNo.Text = "";
                txtCustName.Text = "";
                txtGradeName.Text = "";
                txtDelayPoint.Text = "";
                txtAbtyPoint.Text = "";
                txtCltePoint.Text = "";
                txtRemark.Text = "";
                strCustNo = "";
                txtCardNo.SetFocus();
                msgBar.Text = strMsg11;
                SetControlDisable(false);  
            }
        }

        /// <summary>
        /// PP02전문통신(전화번호) 완료 이벤트
        /// </summary>
        /// <param name="responseData"></param>
        void pp02_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            ChildManager.ShowProgress(false);

            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                var data = responseData.DataRecords.ToDataRecords<PP02RespData>();
                if (data.Length > 0)
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate()
                        {
                            if (TypeHelper.ToInt32(data[0].CustCount) > 1)
                            {
                                DataTable dtCust = new DataTable();
                                dtCust.Columns.Add("Col01");
                                dtCust.Columns.Add("Col02");

                                for (int i = 0; i < TypeHelper.ToInt32(data[0].CustCount); i++)
                                {
                                    dtCust.Rows.Add(new object[] { data[0].CustList[i].CustCardNo.ToString(), data[0].CustList[i].CustName.ToString() });
                                }


                                //동일 전화번호가 두명이상이면 선택팝업 open
                                if (dtCust.Rows.Count > 0)
                                {
                                    using (var pop = ChildManager.ShowPopup(strMsg12, "WSWD.WmallPos.POS.PT.dll",
                                        "WSWD.WmallPos.POS.PT.VC.POS_PT_P003", dtCust))
                                    {

                                        if (pop.ShowDialog(this) == DialogResult.OK)
                                        {
                                            if (pop.ReturnResult != null && pop.ReturnResult.Count > 0)
                                            {
                                                foreach (var item in pop.ReturnResult)
                                                {
                                                    GetServerRegister("1", item.Value.ToString());
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                SetControlDisable(false);
                            }

                            if (TypeHelper.ToInt32(data[0].CustCount) <= 1)
                            {
                                GetServerRegister("1", data[0].CustList[0].CustCardNo.ToString());
                            }
                        });
                    }
                    else
                    {
                        if (TypeHelper.ToInt32(data[0].CustCount) > 1)
                        {
                            DataTable dtCust = new DataTable();
                            dtCust.Columns.Add("Col01");
                            dtCust.Columns.Add("Col02");

                            for (int i = 0; i < TypeHelper.ToInt32(data[0].CustCount); i++)
                            {
                                dtCust.Rows.Add(new object[] { data[0].CustList[i].CustCardNo.ToString(), data[0].CustList[i].CustName.ToString() });
                            }


                            //동일 전화번호가 두명이상이면 선택팝업 open
                            if (dtCust.Rows.Count > 0)
                            {
                                using (var pop = ChildManager.ShowPopup(strMsg12, "WSWD.WmallPos.POS.PT.dll",
                                    "WSWD.WmallPos.POS.PT.VC.POS_PT_P003", dtCust))
                                {

                                    if (pop.ShowDialog(this) == DialogResult.OK)
                                    {
                                        if (pop.ReturnResult != null && pop.ReturnResult.Count > 0)
                                        {
                                            foreach (var item in pop.ReturnResult)
                                            {
                                                GetServerRegister("1", item.Value.ToString());
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            SetControlDisable(false);
                        }

                        if (TypeHelper.ToInt32(data[0].CustCount) <= 1)
                        {
                            GetServerRegister("1", data[0].CustList[0].CustCardNo.ToString());
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
                        POSDeviceManager.SignPad.RequestPinData(strMsg06, "", "", "", 1, 13);
                        _returnData = null;
                        txtCardNo.Text = "";
                        txtCustName.Text = "";
                        txtGradeName.Text = "";
                        txtDelayPoint.Text = "";
                        txtAbtyPoint.Text = "";
                        txtCltePoint.Text = "";
                        txtRemark.Text = "";
                        strCustNo = "";
                        msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false); 
                    });
                }
                else
                {
                    POSDeviceManager.SignPad.RequestPinData(strMsg06, "", "", "", 1, 13);
                    _returnData = null;
                    txtCardNo.Text = "";
                    txtCustName.Text = "";
                    txtGradeName.Text = "";
                    txtDelayPoint.Text = "";
                    txtAbtyPoint.Text = "";
                    txtCltePoint.Text = "";
                    txtRemark.Text = "";
                    strCustNo = "";
                    msgBar.Text = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false); 
                }
            }
            else
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        POSDeviceManager.SignPad.RequestPinData(strMsg06, "", "", "", 1, 13);
                        _returnData = null;
                        txtCardNo.Text = "";
                        txtCustName.Text = "";
                        txtGradeName.Text = "";
                        txtDelayPoint.Text = "";
                        txtAbtyPoint.Text = "";
                        txtCltePoint.Text = "";
                        txtRemark.Text = "";
                        strCustNo = "";
                        msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false);  
                    });
                }
                else
                {
                    POSDeviceManager.SignPad.RequestPinData(strMsg06, "", "", "", 1, 13);
                    _returnData = null;
                    txtCardNo.Text = "";
                    txtCustName.Text = "";
                    txtGradeName.Text = "";
                    txtDelayPoint.Text = "";
                    txtAbtyPoint.Text = "";
                    txtCltePoint.Text = "";
                    txtRemark.Text = "";
                    strCustNo = "";
                    msgBar.Text = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false);  
                }
            }
        }

        /// <summary>
        /// PP02전문통신 오류
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="lastException"></param>
        void pp02_Errored(string errorMessage, Exception lastException)
        {
            ChildManager.ShowProgress(false);

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    _returnData = null;
                    txtCardNo.Text = "";
                    txtCustName.Text = "";
                    txtGradeName.Text = "";
                    txtDelayPoint.Text = "";
                    txtAbtyPoint.Text = "";
                    txtCltePoint.Text = "";
                    txtRemark.Text = "";
                    strCustNo = "";
                    txtCardNo.SetFocus();
                    msgBar.Text = strMsg11;
                    SetControlDisable(false);  
                });
            }
            else
            {
                _returnData = null;
                txtCardNo.Text = "";
                txtCustName.Text = "";
                txtGradeName.Text = "";
                txtDelayPoint.Text = "";
                txtAbtyPoint.Text = "";
                txtCltePoint.Text = "";
                txtRemark.Text = "";
                strCustNo = "";
                txtCardNo.SetFocus();
                msgBar.Text = strMsg11;
                SetControlDisable(false);  
            }
        }

        #region 프린트 확인

        /// <summary>
        /// 프린트 확인
        /// </summary>
        /// <returns></returns>
        private bool ChkPrint()
        {
            bool bReturn = false;
            string strErrMsg = string.Empty;

            try
            {
                if (POSDeviceManager.Printer.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
                {
                    if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.PowerClose)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_POWER;
                    }
                    else if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.CoverOpenned)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_OPENCOVER;
                    }
                    else if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.PaperEmpty)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_PAPER;
                    }
                    else if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.Closed)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_ERROR;
                    }
                    else
                    {
                        bReturn = true;
                    }
                }
                else
                {
                    strErrMsg = FXConsts.ERR_MSG_PRINTER_ERROR;
                }

                if (!bReturn)
                {
                    string[] strBtnNm = new string[2];
                    strBtnNm[0] = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00045");
                    strBtnNm[1] = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00190");

                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate()
                        {
                            if (ShowMessageBox(MessageDialogType.Question, null, strErrMsg, strBtnNm) == DialogResult.Yes)
                            {
                                POSDeviceManager.Printer.Open();
                                bReturn = ChkPrint();
                            }
                        });
                    }
                    else
                    {
                        if (ShowMessageBox(MessageDialogType.Question, null, strErrMsg, strBtnNm) == DialogResult.Yes)
                        {
                            POSDeviceManager.Printer.Open();
                            bReturn = ChkPrint();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }

            return bReturn;
        }

        #endregion

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
                }
            }
        }

        #endregion
    }
}
