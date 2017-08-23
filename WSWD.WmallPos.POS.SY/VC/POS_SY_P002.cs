//-----------------------------------------------------------------
/*
 * 화면명   : POS_SY_P002.cs
 * 화면설명 : 시스템설정_연동장비설정
 * 개발자   : 정광호
 * 개발일자 : 2015.06.15
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

using WSWD.WmallPos.POS.SY.PI;
using WSWD.WmallPos.POS.SY.PT;
using WSWD.WmallPos.POS.SY.VI;
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

namespace WSWD.WmallPos.POS.SY.VC
{
    public partial class POS_SY_P002 : FormBase
    {
        #region 변수

        /// <summary>
        /// 여전법 추가 0722
        /// 무결성 로그 파일이름
        /// </summary>
        const string INTG_LOG_FILE_NAME = "integrity";

        /// <summary>
        /// 팝업 메세지 버튼
        /// </summary>
        string[] strBtnNm = new string[2];

        /// <summary>
        /// 현재 탭페이지 인덱스
        /// </summary>
        int iTabIndex = 0;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        /// <summary>
        /// AppConfig 변경 여부
        /// </summary>
        DevChangeSCANNER devChangeSCANNER;
        /// <summary>
        /// AppConfig 변경 여부
        /// </summary>
        DevChangeCDP devChangeCDP;
        /// <summary>
        /// AppConfig 변경 여부
        /// </summary>
        DevChangePRINTER devChangePRINTER;
        /// <summary>
        /// AppConfig 변경 여부
        /// </summary>
        DevChangeMSR devChangeMSR;
        /// <summary>
        /// AppConfig 변경 여부
        /// </summary>
        DevChangeCASHDRAWER devChangeCASHDRAWER;
        /// <summary>
        /// AppConfig 변경 여부
        /// </summary>
        DevChangeSIGNPAD devChangeSIGNPAD;        

        #endregion

        #region 생성자

        public POS_SY_P002()
        {
            InitializeComponent();

            //Form Load Event
            Load += new EventHandler(form_Load);
            this.Unload += new EventHandler(POS_SY_P002_Unload);
        }

        /// <summary>
        /// Unload, clear event handlers
        /// and reset sign pad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void POS_SY_P002_Unload(object sender, EventArgs e)
        {
            this.Unload -= new EventHandler(POS_SY_P002_Unload);
            this.KeyEvent -= new OPOSKeyEventHandler(form_KeyEvent);                                 //Key Event
            this.tc.Selecting -= new TabControlCancelEventHandler(tc_Selecting);                            //탭변경 이벤트
            this.btnSave.Click -= new EventHandler(btnSave_Click);                                          //저장 button Event
            this.btnClose.Click -= new EventHandler(btnClose_Click);                                        //닫기 button Event

            // 여전법 추가 0722
            this.btnIntgCheck.Click -= new EventHandler(btnIntgCheck_Click);
            this.btnIntgView.Click -= new EventHandler(btnIntgView_Click);

            #region 텍스트박스 포커스 이벤트

            txtScannerUse.InputFocused -= new EventHandler(txt_InputFocused);
            txtScannerMethod.InputFocused -= new EventHandler(txt_InputFocused);
            txtScannerLogicalName.InputFocused -= new EventHandler(txt_InputFocused);
            txtScannerPort.InputFocused -= new EventHandler(txt_InputFocused);
            txtScannerSpeed.InputFocused -= new EventHandler(txt_InputFocused);
            txtScannerDataBit.InputFocused -= new EventHandler(txt_InputFocused);
            txtScannerStopBit.InputFocused -= new EventHandler(txt_InputFocused);
            txtScannerParity.InputFocused -= new EventHandler(txt_InputFocused);
            txtScannerFlowControl.InputFocused -= new EventHandler(txt_InputFocused);

            txtCDPUse.InputFocused -= new EventHandler(txt_InputFocused);
            txtCDPMethod.InputFocused -= new EventHandler(txt_InputFocused);
            txtCDPLogicalName.InputFocused -= new EventHandler(txt_InputFocused);
            txtCDPPort.InputFocused -= new EventHandler(txt_InputFocused);
            txtCDPSpeed.InputFocused -= new EventHandler(txt_InputFocused);
            txtCDPDataBit.InputFocused -= new EventHandler(txt_InputFocused);
            txtCDPStopBit.InputFocused -= new EventHandler(txt_InputFocused);
            txtCDPParity.InputFocused -= new EventHandler(txt_InputFocused);
            txtCDPFlowControl.InputFocused -= new EventHandler(txt_InputFocused);

            txtPRINTERUse.InputFocused -= new EventHandler(txt_InputFocused);
            txtPRINTERMethod.InputFocused -= new EventHandler(txt_InputFocused);
            txtPRINTERLogicalName.InputFocused -= new EventHandler(txt_InputFocused);
            txtPRINTERPort.InputFocused -= new EventHandler(txt_InputFocused);
            txtPRINTERSpeed.InputFocused -= new EventHandler(txt_InputFocused);
            txtPRINTERDataBit.InputFocused -= new EventHandler(txt_InputFocused);
            txtPRINTERStopBit.InputFocused -= new EventHandler(txt_InputFocused);
            txtPRINTERParity.InputFocused -= new EventHandler(txt_InputFocused);
            txtPRINTERFlowControl.InputFocused -= new EventHandler(txt_InputFocused);
            txtPRINTERLogoBMP.InputFocused -= new EventHandler(txt_InputFocused);
            txtPRINTERCutFeedCn.InputFocused -= new EventHandler(txt_InputFocused);
            txtPRINTERBarCodeWidth.InputFocused -= new EventHandler(txt_InputFocused);
            txtPRINTERBarCodeHeight.InputFocused -= new EventHandler(txt_InputFocused);

            //여전법 추가 KSK(0923)
            //txtMSRUse.InputFocused -= new EventHandler(txt_InputFocused);
            //txtMSRMethod.InputFocused -= new EventHandler(txt_InputFocused);
            //txtMSRLogicalName.InputFocused -= new EventHandler(txt_InputFocused);

            txtCASHUse.InputFocused -= new EventHandler(txt_InputFocused);
            txtCASHMethod.InputFocused -= new EventHandler(txt_InputFocused);
            txtCASHLogicalName.InputFocused -= new EventHandler(txt_InputFocused);

            txtSignPadUse.InputFocused -= new EventHandler(txt_InputFocused);
            txtSignPadMethod.InputFocused -= new EventHandler(txt_InputFocused);
            txtSignPadLogicalName.InputFocused -= new EventHandler(txt_InputFocused);
            txtSignPadPort.InputFocused -= new EventHandler(txt_InputFocused);
            txtSignPadSpeed.InputFocused -= new EventHandler(txt_InputFocused);
            txtSignPadDataBit.InputFocused -= new EventHandler(txt_InputFocused);
            txtSignPadStopBit.InputFocused -= new EventHandler(txt_InputFocused);
            txtSignPadParity.InputFocused -= new EventHandler(txt_InputFocused);
            txtSignPadFlowControl.InputFocused -= new EventHandler(txt_InputFocused);

            #endregion
            
            POSDeviceManager.SignPad.Close();
        }

        #endregion

        #region 이벤트 등록

        /// <summary>
        /// Form Event 정의
        /// </summary>
        private void InitEvent()
        {
            this.KeyEvent += new OPOSKeyEventHandler(form_KeyEvent);                                 //Key Event
            this.tc.Selecting += new TabControlCancelEventHandler(tc_Selecting);                            //탭변경 이벤트
            this.btnSave.Click += new EventHandler(btnSave_Click);                                          //저장 button Event
            this.btnClose.Click += new EventHandler(btnClose_Click);                                        //닫기 button Event

            // 여전법 추가 0722
            this.btnIntgCheck.Click += new EventHandler(btnIntgCheck_Click);
            this.btnIntgView.Click += new EventHandler(btnIntgView_Click);

            #region 텍스트박스 포커스 이벤트

            txtScannerUse.InputFocused += new EventHandler(txt_InputFocused);
            txtScannerMethod.InputFocused += new EventHandler(txt_InputFocused);
            txtScannerLogicalName.InputFocused += new EventHandler(txt_InputFocused);
            txtScannerPort.InputFocused += new EventHandler(txt_InputFocused);
            txtScannerSpeed.InputFocused += new EventHandler(txt_InputFocused);
            txtScannerDataBit.InputFocused += new EventHandler(txt_InputFocused);
            txtScannerStopBit.InputFocused += new EventHandler(txt_InputFocused);
            txtScannerParity.InputFocused += new EventHandler(txt_InputFocused);
            txtScannerFlowControl.InputFocused += new EventHandler(txt_InputFocused);

            txtCDPUse.InputFocused += new EventHandler(txt_InputFocused);
            txtCDPMethod.InputFocused += new EventHandler(txt_InputFocused);
            txtCDPLogicalName.InputFocused += new EventHandler(txt_InputFocused);
            txtCDPPort.InputFocused += new EventHandler(txt_InputFocused);
            txtCDPSpeed.InputFocused += new EventHandler(txt_InputFocused);
            txtCDPDataBit.InputFocused += new EventHandler(txt_InputFocused);
            txtCDPStopBit.InputFocused += new EventHandler(txt_InputFocused);
            txtCDPParity.InputFocused += new EventHandler(txt_InputFocused);
            txtCDPFlowControl.InputFocused += new EventHandler(txt_InputFocused);

            txtPRINTERUse.InputFocused += new EventHandler(txt_InputFocused);
            txtPRINTERMethod.InputFocused += new EventHandler(txt_InputFocused);
            txtPRINTERLogicalName.InputFocused += new EventHandler(txt_InputFocused);
            txtPRINTERPort.InputFocused += new EventHandler(txt_InputFocused);
            txtPRINTERSpeed.InputFocused += new EventHandler(txt_InputFocused);
            txtPRINTERDataBit.InputFocused += new EventHandler(txt_InputFocused);
            txtPRINTERStopBit.InputFocused += new EventHandler(txt_InputFocused);
            txtPRINTERParity.InputFocused += new EventHandler(txt_InputFocused);
            txtPRINTERFlowControl.InputFocused += new EventHandler(txt_InputFocused);
            txtPRINTERLogoBMP.InputFocused += new EventHandler(txt_InputFocused);
            txtPRINTERCutFeedCn.InputFocused += new EventHandler(txt_InputFocused);
            txtPRINTERBarCodeWidth.InputFocused += new EventHandler(txt_InputFocused);
            txtPRINTERBarCodeHeight.InputFocused += new EventHandler(txt_InputFocused);

            //여전법 추가 KSK(0923)
            //txtMSRUse.InputFocused += new EventHandler(txt_InputFocused);
            //txtMSRMethod.InputFocused += new EventHandler(txt_InputFocused);
            //txtMSRLogicalName.InputFocused += new EventHandler(txt_InputFocused);

            txtCASHUse.InputFocused += new EventHandler(txt_InputFocused);
            txtCASHMethod.InputFocused += new EventHandler(txt_InputFocused);
            txtCASHLogicalName.InputFocused += new EventHandler(txt_InputFocused);

            txtSignPadUse.InputFocused += new EventHandler(txt_InputFocused);
            txtSignPadMethod.InputFocused += new EventHandler(txt_InputFocused);
            txtSignPadLogicalName.InputFocused += new EventHandler(txt_InputFocused);
            txtSignPadPort.InputFocused += new EventHandler(txt_InputFocused);
            txtSignPadSpeed.InputFocused += new EventHandler(txt_InputFocused);
            txtSignPadDataBit.InputFocused += new EventHandler(txt_InputFocused);
            txtSignPadStopBit.InputFocused += new EventHandler(txt_InputFocused);
            txtSignPadParity.InputFocused += new EventHandler(txt_InputFocused);
            txtSignPadFlowControl.InputFocused += new EventHandler(txt_InputFocused);

            #endregion
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

            strBtnNm[0] = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00371");
            strBtnNm[1] = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00372");

            //컨트롤 초기화
            SetInitControl();

            // InitSignPad
            // 여전법 추가 0722
            InitSignPad();
        }

        /// <summary>
        /// 컨트롤 초기화
        /// </summary>
        private void SetInitControl()
        {
            devChangeSCANNER = new DevChangeSCANNER();
            devChangeCDP = new DevChangeCDP();
            devChangePRINTER = new DevChangePRINTER();
            devChangeMSR = new DevChangeMSR();
            devChangeCASHDRAWER = new DevChangeCASHDRAWER();
            devChangeSIGNPAD = new DevChangeSIGNPAD();

            txtScannerUse.Text = ConfigData.Current.DevConfig.ScannerGun.Use;
            txtScannerMethod.Text = ConfigData.Current.DevConfig.ScannerGun.Method;
            txtScannerLogicalName.Text = ConfigData.Current.DevConfig.ScannerGun.LogicalName;
            txtScannerPort.Text = ConfigData.Current.DevConfig.ScannerGun.Port;
            txtScannerSpeed.Text = ConfigData.Current.DevConfig.ScannerGun.Speed;
            txtScannerDataBit.Text = ConfigData.Current.DevConfig.ScannerGun.DataBit;
            txtScannerStopBit.Text = ConfigData.Current.DevConfig.ScannerGun.StopBit;
            txtScannerParity.Text = ConfigData.Current.DevConfig.ScannerGun.Parity;
            txtScannerFlowControl.Text = ConfigData.Current.DevConfig.ScannerGun.FlowControl;

            txtCDPUse.Text = ConfigData.Current.DevConfig.LineDisplay.Use;
            txtCDPMethod.Text = ConfigData.Current.DevConfig.LineDisplay.Method;
            txtCDPLogicalName.Text = ConfigData.Current.DevConfig.LineDisplay.LogicalName;
            txtCDPPort.Text = ConfigData.Current.DevConfig.LineDisplay.Port;
            txtCDPSpeed.Text = ConfigData.Current.DevConfig.LineDisplay.Speed;
            txtCDPDataBit.Text = ConfigData.Current.DevConfig.LineDisplay.DataBit;
            txtCDPStopBit.Text = ConfigData.Current.DevConfig.LineDisplay.StopBit;
            txtCDPParity.Text = ConfigData.Current.DevConfig.LineDisplay.Parity;
            txtCDPFlowControl.Text = ConfigData.Current.DevConfig.LineDisplay.FlowControl;

            txtPRINTERUse.Text = ConfigData.Current.DevConfig.Printer.Use;
            txtPRINTERMethod.Text = ConfigData.Current.DevConfig.Printer.Method;
            txtPRINTERLogicalName.Text = ConfigData.Current.DevConfig.Printer.LogicalName;
            txtPRINTERPort.Text = ConfigData.Current.DevConfig.Printer.Port;
            txtPRINTERSpeed.Text = ConfigData.Current.DevConfig.Printer.Speed;
            txtPRINTERDataBit.Text = ConfigData.Current.DevConfig.Printer.DataBit;
            txtPRINTERStopBit.Text = ConfigData.Current.DevConfig.Printer.StopBit;
            txtPRINTERParity.Text = ConfigData.Current.DevConfig.Printer.Parity;
            txtPRINTERFlowControl.Text = ConfigData.Current.DevConfig.Printer.FlowControl;
            txtPRINTERLogoBMP.Text = ConfigData.Current.DevConfig.Printer.LogoBMP;
            txtPRINTERCutFeedCn.Text = ConfigData.Current.DevConfig.Printer.CutFeedCn;
            txtPRINTERBarCodeWidth.Text = ConfigData.Current.DevConfig.Printer.BarCodeWidth;
            txtPRINTERBarCodeHeight.Text = ConfigData.Current.DevConfig.Printer.BarCodeHeight;

            //여전법 추가 KSK(0923)
            //txtMSRUse.Text = ConfigData.Current.DevConfig.MSR.Use;
            //txtMSRMethod.Text = ConfigData.Current.DevConfig.MSR.Method;
            //txtMSRLogicalName.Text = ConfigData.Current.DevConfig.MSR.LogicalName;

            txtCASHUse.Text = ConfigData.Current.DevConfig.CashDrawer.Use;
            txtCASHMethod.Text = ConfigData.Current.DevConfig.CashDrawer.Method;
            txtCASHLogicalName.Text = ConfigData.Current.DevConfig.CashDrawer.LogicalName;

            txtSignPadUse.Text = ConfigData.Current.DevConfig.SignPad.Use;
            txtSignPadMethod.Text = ConfigData.Current.DevConfig.SignPad.Method;
            txtSignPadLogicalName.Text = ConfigData.Current.DevConfig.SignPad.LogicalName;
            txtSignPadPort.Text = ConfigData.Current.DevConfig.SignPad.Port;
            txtSignPadSpeed.Text = ConfigData.Current.DevConfig.SignPad.Speed;
            txtSignPadDataBit.Text = ConfigData.Current.DevConfig.SignPad.DataBit;
            txtSignPadStopBit.Text = ConfigData.Current.DevConfig.SignPad.StopBit;
            txtSignPadParity.Text = ConfigData.Current.DevConfig.SignPad.Parity;
            txtSignPadFlowControl.Text = ConfigData.Current.DevConfig.SignPad.FlowControl;

            iTabIndex = 0;
            txtScannerUse.SetFocus();
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

            if (!e.IsControlKey)
            {
                if (txtScannerUse.IsFocused || txtScannerMethod.IsFocused ||
                    txtCDPUse.IsFocused || txtCDPMethod.IsFocused ||
                    txtPRINTERUse.IsFocused || txtPRINTERMethod.IsFocused ||
                    //여전법 추가 KSK(0923)
                    //txtMSRUse.IsFocused || txtMSRMethod.IsFocused ||
                    txtCASHUse.IsFocused || txtCASHMethod.IsFocused ||
                    txtSignPadUse.IsFocused)
                {
                    if (e.KeyCodeText != "0" && e.KeyCodeText != "1")
                    {
                        e.IsHandled = true;
                        return;
                    }
                }
                else if (txtSignPadMethod.IsFocused)
                {
                    if (e.KeyCodeText != "0")
                    {
                        e.IsHandled = true;
                        return;
                    }
                }
                else if (txtScannerDataBit.IsFocused || txtCDPDataBit.IsFocused || txtPRINTERDataBit.IsFocused || txtSignPadDataBit.IsFocused)
                {
                    if (e.KeyCodeText != "7" && e.KeyCodeText != "8")
                    {
                        e.IsHandled = true;
                        return;
                    }
                }
                else if (txtScannerStopBit.IsFocused || txtCDPStopBit.IsFocused || txtPRINTERStopBit.IsFocused || txtSignPadStopBit.IsFocused)
                {
                    if (e.KeyCodeText != "1" && e.KeyCodeText != "2")
                    {
                        e.IsHandled = true;
                        return;
                    }
                }
                else if (txtScannerParity.IsFocused || txtCDPParity.IsFocused || txtPRINTERParity.IsFocused || txtSignPadParity.IsFocused)
                {
                    if (e.KeyCodeText != "0" && e.KeyCodeText != "1" && e.KeyCodeText != "2" && e.KeyCodeText != "3" && e.KeyCodeText != "4")
                    {
                        e.IsHandled = true;
                        return;
                    }
                }
                else if (txtScannerFlowControl.IsFocused || txtCDPFlowControl.IsFocused || txtPRINTERFlowControl.IsFocused || txtSignPadFlowControl.IsFocused)
                {
                    if (e.KeyCodeText != "0" && e.KeyCodeText != "1" && e.KeyCodeText != "2")
                    {
                        e.IsHandled = true;
                        return;
                    }
                }
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
            {
                #region SCANNER

                if (txtScannerUse.IsFocused)
                {
                    txtScannerMethod.SetFocus();
                }
                else if (txtScannerMethod.IsFocused)
                {
                    txtScannerLogicalName.SetFocus();
                }
                else if (txtScannerLogicalName.IsFocused)
                {
                    txtScannerPort.SetFocus();
                }
                else if (txtScannerPort.IsFocused)
                {
                    txtScannerSpeed.SetFocus();
                }
                else if (txtScannerSpeed.IsFocused)
                {
                    txtScannerDataBit.SetFocus();
                }
                else if (txtScannerDataBit.IsFocused)
                {
                    txtScannerStopBit.SetFocus();
                }
                else if (txtScannerStopBit.IsFocused)
                {
                    txtScannerParity.SetFocus();
                }
                else if (txtScannerParity.IsFocused)
                {
                    txtScannerFlowControl.SetFocus();
                }
                else if (txtScannerFlowControl.IsFocused)
                {
                    txtScannerUse.SetFocus();
                }

                #endregion

                #region CDP

                if (txtCDPUse.IsFocused)
                {
                    txtCDPMethod.SetFocus();
                }
                else if (txtCDPMethod.IsFocused)
                {
                    txtCDPLogicalName.SetFocus();
                }
                else if (txtCDPLogicalName.IsFocused)
                {
                    txtCDPPort.SetFocus();
                }
                else if (txtCDPPort.IsFocused)
                {
                    txtCDPSpeed.SetFocus();
                }
                else if (txtCDPSpeed.IsFocused)
                {
                    txtCDPDataBit.SetFocus();
                }
                else if (txtCDPDataBit.IsFocused)
                {
                    txtCDPStopBit.SetFocus();
                }
                else if (txtCDPStopBit.IsFocused)
                {
                    txtCDPParity.SetFocus();
                }
                else if (txtCDPParity.IsFocused)
                {
                    txtCDPFlowControl.SetFocus();
                }
                else if (txtCDPFlowControl.IsFocused)
                {
                    txtCDPUse.SetFocus();
                }

                #endregion

                #region PRINTER

                if (txtPRINTERUse.IsFocused)
                {
                    txtPRINTERMethod.SetFocus();
                }
                else if (txtPRINTERMethod.IsFocused)
                {
                    txtPRINTERLogicalName.SetFocus();
                }
                else if (txtPRINTERLogicalName.IsFocused)
                {
                    txtPRINTERPort.SetFocus();
                }
                else if (txtPRINTERPort.IsFocused)
                {
                    txtPRINTERSpeed.SetFocus();
                }
                else if (txtPRINTERSpeed.IsFocused)
                {
                    txtPRINTERDataBit.SetFocus();
                }
                else if (txtPRINTERDataBit.IsFocused)
                {
                    txtPRINTERStopBit.SetFocus();
                }
                else if (txtPRINTERStopBit.IsFocused)
                {
                    txtPRINTERParity.SetFocus();
                }
                else if (txtPRINTERParity.IsFocused)
                {
                    txtPRINTERFlowControl.SetFocus();
                }
                else if (txtPRINTERFlowControl.IsFocused)
                {
                    txtPRINTERLogoBMP.SetFocus();
                }
                else if (txtPRINTERLogoBMP.IsFocused)
                {
                    txtPRINTERCutFeedCn.SetFocus();
                }
                else if (txtPRINTERCutFeedCn.IsFocused)
                {
                    txtPRINTERBarCodeWidth.SetFocus();
                }
                else if (txtPRINTERBarCodeWidth.IsFocused)
                {
                    txtPRINTERBarCodeHeight.SetFocus();
                }
                else if (txtPRINTERBarCodeHeight.IsFocused)
                {
                    txtPRINTERUse.SetFocus();
                }

                #endregion

                #region MSR

                //여전법 추가 KSK(0923)
                //if (txtMSRUse.IsFocused)
                //{
                //    txtMSRMethod.SetFocus();
                //}
                //else if (txtMSRMethod.IsFocused)
                //{
                //    txtMSRLogicalName.SetFocus();
                //}
                //else if (txtMSRLogicalName.IsFocused)
                //{
                //    txtMSRUse.SetFocus();
                //}

                #endregion

                #region CASH DRAWER

                if (txtCASHUse.IsFocused)
                {
                    txtCASHMethod.SetFocus();
                }
                else if (txtCASHMethod.IsFocused)
                {
                    txtCASHLogicalName.SetFocus();
                }
                else if (txtCASHLogicalName.IsFocused)
                {
                    txtCASHUse.SetFocus();
                }

                #endregion

                #region SignPad

                if (txtSignPadUse.IsFocused)
                {
                    txtSignPadMethod.SetFocus();
                }
                else if (txtSignPadMethod.IsFocused)
                {
                    txtSignPadLogicalName.SetFocus();
                }
                else if (txtSignPadLogicalName.IsFocused)
                {
                    txtSignPadPort.SetFocus();
                }
                else if (txtSignPadPort.IsFocused)
                {
                    txtSignPadSpeed.SetFocus();
                }
                else if (txtSignPadSpeed.IsFocused)
                {
                    txtSignPadDataBit.SetFocus();
                }
                else if (txtSignPadDataBit.IsFocused)
                {
                    txtSignPadStopBit.SetFocus();
                }
                else if (txtSignPadStopBit.IsFocused)
                {
                    txtSignPadParity.SetFocus();
                }
                else if (txtSignPadParity.IsFocused)
                {
                    txtSignPadFlowControl.SetFocus();
                }
                else if (txtSignPadFlowControl.IsFocused)
                {
                    txtSignPadUse.SetFocus();
                }

                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
            {
                #region SCANNER

                if (txtScannerUse.IsFocused)
                {
                    if (txtScannerUse.Text.Length <= 0)
                    {
                        
                    }
                }
                else if (txtScannerMethod.IsFocused)
                {
                    if (txtScannerMethod.Text.Length <= 0)
                    {
                        txtScannerUse.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtScannerLogicalName.IsFocused)
                {
                    if (txtScannerLogicalName.Text.Length <= 0)
                    {
                        txtScannerMethod.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtScannerPort.IsFocused)
                {
                    if (txtScannerPort.Text.Length <= 0)
                    {
                        txtScannerLogicalName.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtScannerSpeed.IsFocused)
                {
                    if (txtScannerSpeed.Text.Length <= 0)
                    {
                        txtScannerPort.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtScannerDataBit.IsFocused)
                {
                    if (txtScannerDataBit.Text.Length <= 0)
                    {
                        txtScannerSpeed.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtScannerStopBit.IsFocused)
                {
                    if (txtScannerStopBit.Text.Length <= 0)
                    {
                        txtScannerDataBit.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtScannerParity.IsFocused)
                {
                    if (txtScannerParity.Text.Length <= 0)
                    {
                        txtScannerStopBit.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtScannerFlowControl.IsFocused)
                {
                    if (txtScannerFlowControl.Text.Length <= 0)
                    {
                        txtScannerParity.SetFocus();
                        e.IsHandled = true;
                    }
                }

                #endregion

                #region CDP

                if (txtCDPUse.IsFocused)
                {
                    if (txtCDPUse.Text.Length <= 0)
                    {
                        
                    }
                }
                else if (txtCDPMethod.IsFocused)
                {
                    if (txtCDPMethod.Text.Length <= 0)
                    {
                        txtCDPUse.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtCDPLogicalName.IsFocused)
                {
                    if (txtCDPLogicalName.Text.Length <= 0)
                    {
                        txtCDPMethod.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtCDPPort.IsFocused)
                {
                    if (txtCDPPort.Text.Length <= 0)
                    {
                        txtCDPLogicalName.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtCDPSpeed.IsFocused)
                {
                    if (txtCDPSpeed.Text.Length <= 0)
                    {
                        txtCDPPort.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtCDPDataBit.IsFocused)
                {
                    if (txtCDPDataBit.Text.Length <= 0)
                    {
                        txtCDPSpeed.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtCDPStopBit.IsFocused)
                {
                    if (txtCDPStopBit.Text.Length <= 0)
                    {
                        txtCDPDataBit.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtCDPParity.IsFocused)
                {
                    if (txtCDPParity.Text.Length <= 0)
                    {
                        txtCDPStopBit.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtCDPFlowControl.IsFocused)
                {
                    if (txtCDPFlowControl.Text.Length <= 0)
                    {
                        txtCDPParity.SetFocus();
                        e.IsHandled = true;
                    }
                }

                #endregion

                #region PRINTER

                if (txtPRINTERUse.IsFocused)
                {
                    if (txtPRINTERUse.Text.Length <= 0)
                    {
                        
                    }
                }
                else if (txtPRINTERMethod.IsFocused)
                {
                    if (txtPRINTERMethod.Text.Length <= 0)
                    {
                        txtPRINTERUse.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtPRINTERLogicalName.IsFocused)
                {
                    if (txtPRINTERLogicalName.Text.Length <= 0)
                    {
                        txtPRINTERMethod.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtPRINTERPort.IsFocused)
                {
                    if (txtPRINTERPort.Text.Length <= 0)
                    {
                        txtPRINTERLogicalName.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtPRINTERSpeed.IsFocused)
                {
                    if (txtPRINTERSpeed.Text.Length <= 0)
                    {
                        txtPRINTERPort.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtPRINTERDataBit.IsFocused)
                {
                    if (txtPRINTERDataBit.Text.Length <= 0)
                    {
                        txtPRINTERSpeed.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtPRINTERStopBit.IsFocused)
                {
                    if (txtPRINTERStopBit.Text.Length <= 0)
                    {
                        txtPRINTERDataBit.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtPRINTERParity.IsFocused)
                {
                    if (txtPRINTERParity.Text.Length <= 0)
                    {
                        txtPRINTERStopBit.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtPRINTERFlowControl.IsFocused)
                {
                    if (txtPRINTERFlowControl.Text.Length <= 0)
                    {
                        txtPRINTERParity.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtPRINTERLogoBMP.IsFocused)
                {
                    if (txtPRINTERLogoBMP.Text.Length <= 0)
                    {
                        txtPRINTERFlowControl.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtPRINTERCutFeedCn.IsFocused)
                {
                    if (txtPRINTERCutFeedCn.Text.Length <= 0)
                    {
                        txtPRINTERLogoBMP.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtPRINTERBarCodeWidth.IsFocused)
                {
                    if (txtPRINTERBarCodeWidth.Text.Length <= 0)
                    {
                        txtPRINTERCutFeedCn.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtPRINTERBarCodeHeight.IsFocused)
                {
                    if (txtPRINTERBarCodeHeight.Text.Length <= 0)
                    {
                        txtPRINTERBarCodeWidth.SetFocus();
                        e.IsHandled = true;
                    }
                }

                #endregion

                #region MSR

                //여전법 추가 KSK(0923)
                //if (txtMSRUse.IsFocused)
                //{
                //    if (txtMSRUse.Text.Length <= 0)
                //    {
                        
                //    }
                //}
                //else if (txtMSRMethod.IsFocused)
                //{
                //    if (txtMSRMethod.Text.Length <= 0)
                //    {
                //        txtMSRUse.SetFocus();
                //        e.IsHandled = true;
                //    }
                //}
                //else if (txtMSRLogicalName.IsFocused)
                //{
                //    if (txtMSRLogicalName.Text.Length <= 0)
                //    {
                //        txtMSRMethod.SetFocus();
                //        e.IsHandled = true;
                //    }
                //}

                #endregion

                #region CASH DRAWER

                if (txtCASHUse.IsFocused)
                {
                    if (txtCASHUse.Text.Length <= 0)
                    {
                        
                    }
                }
                else if (txtCASHMethod.IsFocused)
                {
                    if (txtCASHMethod.Text.Length <= 0)
                    {
                        txtCASHUse.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtCASHLogicalName.IsFocused)
                {
                    if (txtCASHLogicalName.Text.Length <= 0)
                    {
                        txtCASHMethod.SetFocus();
                        e.IsHandled = true;
                    }
                }

                #endregion

                #region SignPad

                if (txtSignPadUse.IsFocused)
                {
                    if (txtSignPadUse.Text.Length <= 0)
                    {
                        
                    }
                }
                else if (txtSignPadMethod.IsFocused)
                {
                    if (txtSignPadMethod.Text.Length <= 0)
                    {
                        txtSignPadUse.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSignPadLogicalName.IsFocused)
                {
                    if (txtSignPadLogicalName.Text.Length <= 0)
                    {
                        txtSignPadMethod.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSignPadPort.IsFocused)
                {
                    if (txtSignPadPort.Text.Length <= 0)
                    {
                        txtSignPadLogicalName.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSignPadSpeed.IsFocused)
                {
                    if (txtSignPadSpeed.Text.Length <= 0)
                    {
                        txtSignPadPort.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSignPadDataBit.IsFocused)
                {
                    if (txtSignPadDataBit.Text.Length <= 0)
                    {
                        txtSignPadSpeed.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSignPadStopBit.IsFocused)
                {
                    if (txtSignPadStopBit.Text.Length <= 0)
                    {
                        txtSignPadDataBit.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSignPadParity.IsFocused)
                {
                    if (txtSignPadParity.Text.Length <= 0)
                    {
                        txtSignPadStopBit.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSignPadFlowControl.IsFocused)
                {
                    if (txtSignPadFlowControl.Text.Length <= 0)
                    {
                        txtSignPadParity.SetFocus();
                        e.IsHandled = true;
                    }
                }

                #endregion
            }
        }

        /// <summary>
        /// 탭 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tc_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (_bDisable) return;

            if (ChkData())
            {
                msgBar.Text = strMsg16;
                e.Cancel = true;
            }
            else
            {
                iTabIndex = tc.SelectedIndex;

                if (tc.TabPages[iTabIndex].Name.ToString() == "tpSCANNER")
                {
                    txtScannerUse.SetFocus();
                }
                else if (tc.TabPages[iTabIndex].Name.ToString() == "tpCDP")
                {
                    txtCDPUse.SetFocus();
                }
                else if (tc.TabPages[iTabIndex].Name.ToString() == "tpPRINTER")
                {
                    txtPRINTERUse.SetFocus();
                }
                //여전법 추가 KSK(0923)
                //else if (tc.TabPages[iTabIndex].Name.ToString() == "tpMSR")
                //{
                //    txtMSRUse.SetFocus();
                //}
                else if (tc.TabPages[iTabIndex].Name.ToString() == "tpCASHDRAWER")
                {
                    txtCASHUse.SetFocus();
                }
                else if (tc.TabPages[iTabIndex].Name.ToString() == "tpSIGNPAD")
                {
                    txtSignPadUse.SetFocus();
                }
            }
        }

        /// <summary>
        /// 포커스 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txt_InputFocused(object sender, EventArgs e)
        {
            InputText txt = (InputText)sender;

            msgBar.Text = "";

            //여전법 추가 KSK(0923)
            //if (txt.Name.ToString() == txtScannerUse.Name.ToString() ||
            //    txt.Name.ToString() == txtCDPUse.Name.ToString() ||
            //    txt.Name.ToString() == txtPRINTERUse.Name.ToString() ||
            //    txt.Name.ToString() == txtMSRUse.Name.ToString() ||
            //    txt.Name.ToString() == txtCASHUse.Name.ToString() ||
            //    txt.Name.ToString() == txtSignPadUse.Name.ToString())
            if (txt.Name.ToString() == txtScannerUse.Name.ToString() ||
                txt.Name.ToString() == txtCDPUse.Name.ToString() ||
                txt.Name.ToString() == txtPRINTERUse.Name.ToString() ||
                txt.Name.ToString() == txtCASHUse.Name.ToString() ||
                txt.Name.ToString() == txtSignPadUse.Name.ToString())
            {
                msgBar.Text = strMsg01;
            }
            else if (txt.Name.ToString() == txtScannerMethod.Name.ToString() ||
                txt.Name.ToString() == txtCDPMethod.Name.ToString())
                //txt.Name.ToString() == txtPRINTERMethod.Name.ToString() ||
                //txt.Name.ToString() == txtMSRMethod.Name.ToString() ||
                //txt.Name.ToString() == txtCASHMethod.Name.ToString() ||
                //txt.Name.ToString() == txtSignPadMethod.Name.ToString())
            {
                msgBar.Text = strMsg02;
            }
            //else if (txt.Name.ToString() == txtScannerLogicalName.Name.ToString() ||
            //    txt.Name.ToString() == txtCDPLogicalName.Name.ToString() ||
            //    txt.Name.ToString() == txtPRINTERLogicalName.Name.ToString() ||
            //    txt.Name.ToString() == txtMSRLogicalName.Name.ToString() ||
            //    txt.Name.ToString() == txtCASHLogicalName.Name.ToString() ||
            //    txt.Name.ToString() == txtSignPadLogicalName.Name.ToString())
            //{
            //    msgBar.Text = strMsg03;
            //}
            else if (txt.Name.ToString() == txtScannerPort.Name.ToString() ||
                txt.Name.ToString() == txtCDPPort.Name.ToString() ||
                txt.Name.ToString() == txtPRINTERPort.Name.ToString() ||
                txt.Name.ToString() == txtSignPadPort.Name.ToString())
            {
                msgBar.Text = strMsg03;
            }
            else if (txt.Name.ToString() == txtScannerSpeed.Name.ToString() ||
                txt.Name.ToString() == txtCDPSpeed.Name.ToString() ||
                txt.Name.ToString() == txtPRINTERSpeed.Name.ToString() ||
                txt.Name.ToString() == txtSignPadSpeed.Name.ToString())
            {
                msgBar.Text = strMsg04;
            }
            else if (txt.Name.ToString() == txtScannerDataBit.Name.ToString() ||
                txt.Name.ToString() == txtCDPDataBit.Name.ToString() ||
                txt.Name.ToString() == txtPRINTERDataBit.Name.ToString() ||
                txt.Name.ToString() == txtSignPadDataBit.Name.ToString())
            {
                msgBar.Text = strMsg05;
            }
            else if (txt.Name.ToString() == txtScannerStopBit.Name.ToString() ||
                txt.Name.ToString() == txtCDPStopBit.Name.ToString() ||
                txt.Name.ToString() == txtPRINTERStopBit.Name.ToString() ||
                txt.Name.ToString() == txtSignPadStopBit.Name.ToString())
            {
                msgBar.Text = strMsg06;
            }
            else if (txt.Name.ToString() == txtScannerParity.Name.ToString() ||
                txt.Name.ToString() == txtCDPParity.Name.ToString() ||
                txt.Name.ToString() == txtPRINTERParity.Name.ToString() ||
                txt.Name.ToString() == txtSignPadParity.Name.ToString())
            {
                msgBar.Text = strMsg07;
            }
            else if (txt.Name.ToString() == txtScannerFlowControl.Name.ToString() ||
                txt.Name.ToString() == txtCDPFlowControl.Name.ToString() ||
                txt.Name.ToString() == txtPRINTERFlowControl.Name.ToString() ||
                txt.Name.ToString() == txtSignPadFlowControl.Name.ToString())
            {
                msgBar.Text = strMsg08;
            }
            //여전법 추가 KSK(0923)
            //else if (txt.Name.ToString() == txtMSRMethod.Name.ToString())
            //{
            //    msgBar.Text = strMsg09;
            //}
            else if (txt.Name.ToString() == txtCASHMethod.Name.ToString())
            {
                msgBar.Text = strMsg10;
            }
            else if (txt.Name.ToString() == txtSignPadMethod.Name.ToString())
            {
                msgBar.Text = strMsg11;
            }
            else if (txt.Name.ToString() == txtPRINTERLogoBMP.Name.ToString())
            {
                msgBar.Text = strMsg12;
            }
            else if (txt.Name.ToString() == txtPRINTERCutFeedCn.Name.ToString())
            {
                msgBar.Text = strMsg13;
            }
            else if (txt.Name.ToString() == txtPRINTERBarCodeWidth.Name.ToString())
            {
                msgBar.Text = strMsg14;
            }
            else if (txt.Name.ToString() == txtPRINTERBarCodeHeight.Name.ToString())
            {
                msgBar.Text = strMsg15;
            }
            else if (txt.Name.ToString() == txtPRINTERMethod.Name.ToString())
            {
                msgBar.Text = strMsg17;
            }
        }

        /// <summary>
        /// 저장 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            SetControlDisable(true);

            try
            {
                if (ChkData())
                {
                    if (tc.TabPages[iTabIndex].Name.ToString() == "tpSCANNER")
                    {
                        #region SCANNER

                        ConfigData.Current.DevConfig.ScannerGun.Use = !devChangeSCANNER.bUse ? ConfigData.Current.DevConfig.ScannerGun.Use : txtScannerUse.Text;
                        ConfigData.Current.DevConfig.ScannerGun.Method = !devChangeSCANNER.bMethod ? ConfigData.Current.DevConfig.ScannerGun.Method : txtScannerMethod.Text;
                        ConfigData.Current.DevConfig.ScannerGun.LogicalName = !devChangeSCANNER.bLogicalName ? ConfigData.Current.DevConfig.ScannerGun.LogicalName : txtScannerLogicalName.Text;
                        ConfigData.Current.DevConfig.ScannerGun.Port = !devChangeSCANNER.bPort ? ConfigData.Current.DevConfig.ScannerGun.Port : txtScannerPort.Text;
                        ConfigData.Current.DevConfig.ScannerGun.Speed = !devChangeSCANNER.bSpeed ? ConfigData.Current.DevConfig.ScannerGun.Speed : txtScannerSpeed.Text;
                        ConfigData.Current.DevConfig.ScannerGun.DataBit = !devChangeSCANNER.bDataBit ? ConfigData.Current.DevConfig.ScannerGun.DataBit : txtScannerDataBit.Text;
                        ConfigData.Current.DevConfig.ScannerGun.StopBit = !devChangeSCANNER.bStopBit ? ConfigData.Current.DevConfig.ScannerGun.StopBit : txtScannerStopBit.Text;
                        ConfigData.Current.DevConfig.ScannerGun.Parity = !devChangeSCANNER.bParity ? ConfigData.Current.DevConfig.ScannerGun.Parity : txtScannerParity.Text;
                        ConfigData.Current.DevConfig.ScannerGun.FlowControl = !devChangeSCANNER.bFlowControl ? ConfigData.Current.DevConfig.ScannerGun.FlowControl : txtScannerFlowControl.Text;

                        #endregion
                    }
                    else if (tc.TabPages[iTabIndex].Name.ToString() == "tpCDP")
                    {
                        #region CDP

                        ConfigData.Current.DevConfig.LineDisplay.Use = !devChangeCDP.bUse ? ConfigData.Current.DevConfig.LineDisplay.Use : txtCDPUse.Text;
                        ConfigData.Current.DevConfig.LineDisplay.Method = !devChangeCDP.bMethod ? ConfigData.Current.DevConfig.LineDisplay.Method : txtCDPMethod.Text;
                        ConfigData.Current.DevConfig.LineDisplay.LogicalName = !devChangeCDP.bLogicalName ? ConfigData.Current.DevConfig.LineDisplay.LogicalName : txtCDPLogicalName.Text;
                        ConfigData.Current.DevConfig.LineDisplay.Port = !devChangeCDP.bPort ? ConfigData.Current.DevConfig.LineDisplay.Port : txtCDPPort.Text;
                        ConfigData.Current.DevConfig.LineDisplay.Speed = !devChangeCDP.bSpeed ? ConfigData.Current.DevConfig.LineDisplay.Speed : txtCDPSpeed.Text;
                        ConfigData.Current.DevConfig.LineDisplay.DataBit = !devChangeCDP.bDataBit ? ConfigData.Current.DevConfig.LineDisplay.DataBit : txtCDPDataBit.Text;
                        ConfigData.Current.DevConfig.LineDisplay.StopBit = !devChangeCDP.bStopBit ? ConfigData.Current.DevConfig.LineDisplay.StopBit : txtCDPStopBit.Text;
                        ConfigData.Current.DevConfig.LineDisplay.Parity = !devChangeCDP.bParity ? ConfigData.Current.DevConfig.LineDisplay.Parity : txtCDPParity.Text;
                        ConfigData.Current.DevConfig.LineDisplay.FlowControl = !devChangeCDP.bFlowControl ? ConfigData.Current.DevConfig.LineDisplay.FlowControl : txtCDPFlowControl.Text;

                        #endregion
                    }
                    else if (tc.TabPages[iTabIndex].Name.ToString() == "tpPRINTER")
                    {
                        #region PRINTER

                        ConfigData.Current.DevConfig.Printer.Use = !devChangePRINTER.bUse ? ConfigData.Current.DevConfig.Printer.Use : txtPRINTERUse.Text;
                        ConfigData.Current.DevConfig.Printer.Method = !devChangePRINTER.bMethod ? ConfigData.Current.DevConfig.Printer.Method : txtPRINTERMethod.Text;
                        ConfigData.Current.DevConfig.Printer.LogicalName = !devChangePRINTER.bLogicalName ? ConfigData.Current.DevConfig.Printer.LogicalName : txtPRINTERLogicalName.Text;
                        ConfigData.Current.DevConfig.Printer.Port = !devChangePRINTER.bPort ? ConfigData.Current.DevConfig.Printer.Port : txtPRINTERPort.Text;
                        ConfigData.Current.DevConfig.Printer.Speed = !devChangePRINTER.bSpeed ? ConfigData.Current.DevConfig.Printer.Speed : txtPRINTERSpeed.Text;
                        ConfigData.Current.DevConfig.Printer.DataBit = !devChangePRINTER.bDataBit ? ConfigData.Current.DevConfig.Printer.DataBit : txtPRINTERDataBit.Text;
                        ConfigData.Current.DevConfig.Printer.StopBit = !devChangePRINTER.bStopBit ? ConfigData.Current.DevConfig.Printer.StopBit : txtPRINTERStopBit.Text;
                        ConfigData.Current.DevConfig.Printer.Parity = !devChangePRINTER.bParity ? ConfigData.Current.DevConfig.Printer.Parity : txtPRINTERParity.Text;
                        ConfigData.Current.DevConfig.Printer.FlowControl = !devChangePRINTER.bFlowControl ? ConfigData.Current.DevConfig.Printer.FlowControl : txtPRINTERFlowControl.Text;
                        ConfigData.Current.DevConfig.Printer.LogoBMP = !devChangePRINTER.bLogoBMP ? ConfigData.Current.DevConfig.Printer.LogoBMP : txtPRINTERLogoBMP.Text;
                        ConfigData.Current.DevConfig.Printer.CutFeedCn = !devChangePRINTER.bCutFeedCn ? ConfigData.Current.DevConfig.Printer.CutFeedCn : txtPRINTERCutFeedCn.Text;
                        ConfigData.Current.DevConfig.Printer.BarCodeWidth = !devChangePRINTER.bBarCodeWidth ? ConfigData.Current.DevConfig.Printer.BarCodeWidth : txtPRINTERBarCodeWidth.Text;
                        ConfigData.Current.DevConfig.Printer.BarCodeHeight = !devChangePRINTER.bBarCodeHeight ? ConfigData.Current.DevConfig.Printer.BarCodeHeight : txtPRINTERBarCodeHeight.Text;

                        #endregion
                    }
                    else if (tc.TabPages[iTabIndex].Name.ToString() == "tpMSR")
                    {
                        #region MSR
                        //여전법 추가 KSK(0923)
                        //ConfigData.Current.DevConfig.MSR.Use = !devChangeMSR.bUse ? ConfigData.Current.DevConfig.MSR.Use : txtMSRUse.Text;
                        //ConfigData.Current.DevConfig.MSR.Method = !devChangeMSR.bMethod ? ConfigData.Current.DevConfig.MSR.Method : txtMSRMethod.Text;
                        //ConfigData.Current.DevConfig.MSR.LogicalName = !devChangeMSR.bLogicalName ? ConfigData.Current.DevConfig.MSR.LogicalName : txtMSRLogicalName.Text;

                        #endregion
                    }
                    else if (tc.TabPages[iTabIndex].Name.ToString() == "tpCASHDRAWER")
                    {
                        #region CASHDRAWER

                        ConfigData.Current.DevConfig.CashDrawer.Use = !devChangeCASHDRAWER.bUse ? ConfigData.Current.DevConfig.CashDrawer.Use : txtCASHUse.Text;
                        ConfigData.Current.DevConfig.CashDrawer.Method = !devChangeCASHDRAWER.bMethod ? ConfigData.Current.DevConfig.CashDrawer.Method : txtCASHMethod.Text;
                        ConfigData.Current.DevConfig.CashDrawer.LogicalName = !devChangeCASHDRAWER.bLogicalName ? ConfigData.Current.DevConfig.CashDrawer.LogicalName : txtCASHLogicalName.Text;

                        #endregion
                    }
                    else if (tc.TabPages[iTabIndex].Name.ToString() == "tpSIGNPAD")
                    {
                        #region SIGNPAD

                        ConfigData.Current.DevConfig.SignPad.Use = !devChangeSIGNPAD.bUse ? ConfigData.Current.DevConfig.SignPad.Use : txtSignPadUse.Text;
                        ConfigData.Current.DevConfig.SignPad.Method = !devChangeSIGNPAD.bMethod ? ConfigData.Current.DevConfig.SignPad.Method : txtSignPadMethod.Text;
                        ConfigData.Current.DevConfig.SignPad.LogicalName = !devChangeSIGNPAD.bLogicalName ? ConfigData.Current.DevConfig.SignPad.LogicalName : txtSignPadLogicalName.Text;
                        ConfigData.Current.DevConfig.SignPad.Port = !devChangeSIGNPAD.bPort ? ConfigData.Current.DevConfig.SignPad.Port : txtSignPadPort.Text;
                        ConfigData.Current.DevConfig.SignPad.Speed = !devChangeSIGNPAD.bSpeed ? ConfigData.Current.DevConfig.SignPad.Speed : txtSignPadSpeed.Text;
                        ConfigData.Current.DevConfig.SignPad.DataBit = !devChangeSIGNPAD.bDataBit ? ConfigData.Current.DevConfig.SignPad.DataBit : txtSignPadDataBit.Text;
                        ConfigData.Current.DevConfig.SignPad.StopBit = !devChangeSIGNPAD.bStopBit ? ConfigData.Current.DevConfig.SignPad.StopBit : txtSignPadStopBit.Text;
                        ConfigData.Current.DevConfig.SignPad.Parity = !devChangeSIGNPAD.bParity ? ConfigData.Current.DevConfig.SignPad.Parity : txtSignPadParity.Text;
                        ConfigData.Current.DevConfig.SignPad.FlowControl = !devChangeSIGNPAD.bFlowControl ? ConfigData.Current.DevConfig.SignPad.FlowControl : txtSignPadFlowControl.Text;

                        #endregion
                    }

                    //저장
                    ConfigData.Current.DevConfig.Save();
                    DevConfig.Load();

                    //컨트롤 초기화
                    SetInitControl();
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
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

            if (ChkData())
            {
                if (ShowMessageBox(MessageDialogType.Question, null, strMsg00, strBtnNm) == DialogResult.Yes)
                {
                    //저장
                    btnSave_Click(btnSave, null);
                }
            }

            this.Close();
        }

        #endregion

        #region 사용자 정의

        bool ChkData()
        {
            bool retData = false;

            if (tc.TabPages[iTabIndex].Name.ToString() == "tpSCANNER")
            {
                #region SCANNER

                if (ConfigData.Current.DevConfig.ScannerGun.Use != txtScannerUse.Text)
                {
                    devChangeSCANNER.bUse = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.ScannerGun.Method != txtScannerMethod.Text)
                {
                    devChangeSCANNER.bMethod = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.ScannerGun.LogicalName != txtScannerLogicalName.Text)
                {
                    devChangeSCANNER.bLogicalName = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.ScannerGun.Port != txtScannerPort.Text)
                {
                    devChangeSCANNER.bPort = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.ScannerGun.Speed != txtScannerSpeed.Text)
                {
                    devChangeSCANNER.bSpeed = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.ScannerGun.DataBit != txtScannerDataBit.Text)
                {
                    devChangeSCANNER.bDataBit = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.ScannerGun.StopBit != txtScannerStopBit.Text)
                {
                    devChangeSCANNER.bStopBit = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.ScannerGun.Parity != txtScannerParity.Text)
                {
                    devChangeSCANNER.bParity = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.ScannerGun.FlowControl != txtScannerFlowControl.Text)
                {
                    devChangeSCANNER.bFlowControl = true;
                    retData = true;
                }

                #endregion
            }
            else if (tc.TabPages[iTabIndex].Name.ToString() == "tpCDP")
            {
                #region CDP

                if (ConfigData.Current.DevConfig.LineDisplay.Use != txtCDPUse.Text)
                {
                    devChangeCDP.bUse = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.LineDisplay.Method != txtCDPMethod.Text)
                {
                    devChangeCDP.bMethod = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.LineDisplay.LogicalName != txtCDPLogicalName.Text)
                {
                    devChangeCDP.bLogicalName = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.LineDisplay.Port != txtCDPPort.Text)
                {
                    devChangeCDP.bPort = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.LineDisplay.Speed != txtCDPSpeed.Text)
                {
                    devChangeCDP.bSpeed = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.LineDisplay.DataBit != txtCDPDataBit.Text)
                {
                    devChangeCDP.bDataBit = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.LineDisplay.StopBit != txtCDPStopBit.Text)
                {
                    devChangeCDP.bStopBit = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.LineDisplay.Parity != txtCDPParity.Text)
                {
                    devChangeCDP.bParity = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.LineDisplay.FlowControl != txtCDPFlowControl.Text)
                {
                    devChangeCDP.bFlowControl = true;
                    retData = true;
                }

                #endregion
            }
            else if (tc.TabPages[iTabIndex].Name.ToString() == "tpPRINTER")
            {
                #region PRINTER

                if (ConfigData.Current.DevConfig.Printer.Use != txtPRINTERUse.Text)
                {
                    devChangePRINTER.bUse = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.Printer.Method != txtPRINTERMethod.Text)
                {
                    devChangePRINTER.bMethod = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.Printer.LogicalName != txtPRINTERLogicalName.Text)
                {
                    devChangePRINTER.bLogicalName = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.Printer.Port != txtPRINTERPort.Text)
                {
                    devChangePRINTER.bPort = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.Printer.Speed != txtPRINTERSpeed.Text)
                {
                    devChangePRINTER.bSpeed = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.Printer.DataBit != txtPRINTERDataBit.Text)
                {
                    devChangePRINTER.bDataBit = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.Printer.StopBit != txtPRINTERStopBit.Text)
                {
                    devChangePRINTER.bStopBit = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.Printer.Parity != txtPRINTERParity.Text)
                {
                    devChangePRINTER.bParity = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.Printer.FlowControl != txtPRINTERFlowControl.Text)
                {
                    devChangePRINTER.bFlowControl = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.Printer.LogoBMP != txtPRINTERLogoBMP.Text)
                {
                    devChangePRINTER.bLogoBMP = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.Printer.CutFeedCn != txtPRINTERCutFeedCn.Text)
                {
                    devChangePRINTER.bCutFeedCn = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.Printer.BarCodeWidth != txtPRINTERBarCodeWidth.Text)
                {
                    devChangePRINTER.bBarCodeWidth = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.Printer.BarCodeHeight != txtPRINTERBarCodeHeight.Text)
                {
                    devChangePRINTER.bBarCodeHeight = true;
                    retData = true;
                }

                #endregion
            }
            else if (tc.TabPages[iTabIndex].Name.ToString() == "tpMSR")
            {
                #region MSR
                //여전법 추가 KSK(0923)
                //if (ConfigData.Current.DevConfig.MSR.Use != txtMSRUse.Text)
                //{
                //    devChangeMSR.bUse = true;
                //    retData = true;
                //}

                //if (ConfigData.Current.DevConfig.MSR.Method != txtMSRMethod.Text)
                //{
                //    devChangeMSR.bMethod = true;
                //    retData = true;
                //}

                //if (ConfigData.Current.DevConfig.MSR.LogicalName != txtMSRLogicalName.Text)
                //{
                //    devChangeMSR.bLogicalName = true;
                //    retData = true;
                //}

                #endregion
            }
            else if (tc.TabPages[iTabIndex].Name.ToString() == "tpCASHDRAWER")
            {
                #region CASHDRAWER

                if (ConfigData.Current.DevConfig.CashDrawer.Use != txtCASHUse.Text)
                {
                    devChangeCASHDRAWER.bUse = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.CashDrawer.Method != txtCASHMethod.Text)
                {
                    devChangeCASHDRAWER.bMethod = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.CashDrawer.LogicalName != txtCASHLogicalName.Text)
                {
                    devChangeCASHDRAWER.bLogicalName = true;
                    retData = true;
                }

                #endregion
            }
            else if (tc.TabPages[iTabIndex].Name.ToString() == "tpSIGNPAD")
            {
                #region SIGNPAD

                if (ConfigData.Current.DevConfig.SignPad.Use != txtSignPadUse.Text)
                {
                    devChangeSIGNPAD.bUse = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.SignPad.Method != txtSignPadMethod.Text)
                {
                    devChangeSIGNPAD.bMethod = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.SignPad.LogicalName != txtSignPadLogicalName.Text)
                {
                    devChangeSIGNPAD.bLogicalName = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.SignPad.Port != txtSignPadPort.Text)
                {
                    devChangeSIGNPAD.bPort = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.SignPad.Speed != txtSignPadSpeed.Text)
                {
                    devChangeSIGNPAD.bSpeed = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.SignPad.DataBit != txtSignPadDataBit.Text)
                {
                    devChangeSIGNPAD.bDataBit = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.SignPad.StopBit != txtSignPadStopBit.Text)
                {
                    devChangeSIGNPAD.bStopBit = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.SignPad.Parity != txtSignPadParity.Text)
                {
                    devChangeSIGNPAD.bParity = true;
                    retData = true;
                }

                if (ConfigData.Current.DevConfig.SignPad.FlowControl != txtSignPadFlowControl.Text)
                {
                    devChangeSIGNPAD.bFlowControl = true;
                    retData = true;
                }

                #endregion
            }

            return retData;
        }

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
                        else if (item.GetType().Name.ToString().ToLower() == "tabcontrol")
                        {
                            TabControl tab = (TabControl)item;
                            tab.Enabled = !_bDisable;
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
                    else if (item.GetType().Name.ToString().ToLower() == "tabcontrol")
                    {
                        TabControl tab = (TabControl)item;
                        tab.Enabled = !_bDisable;
                    }
                }
            }

            //Application.DoEvents();
        }

        #endregion

        #region 여전법 추가 - 무결성 체크 & 조회 0722

        /*
             ① 무결성 체크 버튼을 눌르게 되면 무결성 검사(API : EncReaderComCheck → Key 검증, S/W검증)를 한 후 POPUP으로 결과 표시 한다.
         * "    ② 성공일 경우 : ""IC리더기(###KSP-6000S1001) 무결성 검증이 정상적으로 완료되었습니다."" Message 처리
         * "    ③ 실패일 경우 : "IC리더기(###KSP-6000S1001) 무결설 실패하여 WMPOS를 종료합니다" Message 처리 후 POS Program 종료
         * ④ 무결성 체크에 대해서는 LOG FILE로 보관을 해야 한다.
         * - Log File Path : "C:\WMallPOS\log\Reader"
         * - Log 저장 방식 : POP UP 당 1Line 암호화(D'AMO API 사용) 처리를 하여 Log File 저장을 하여야 한다.
         * ⑤ [무결성 조회] 버튼을 이용하여 Log를 화면에 표시를 할 때는 복호화(D'AMO API 사용) 처리를 하여 화면에 표시를 한다.
         * - 화면 표시 할 때는 POP UP으로 표시를 한다.
         * ⑥ Log File 의 저장 기간은 기타 Log File 저장일과 동일하게 한다 (단, 최대 85일 이상은 안된다)          
         */

        /// <summary>
        /// 여전법 추가 0722
        /// </summary>
        void InitSignPad()
        {
            if (POSDeviceManager.SignPad.Status != WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
            {
                POSDeviceManager.SignPad.Initialize(this.axKSNet_Dongle1);
            }
            else
            {
                POSDeviceManager.SignPad.Close();
                POSDeviceManager.SignPad.Initialize(this.axKSNet_Dongle1);
            }
        }

        /// <summary>
        /// 무결성 체크 킬릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnIntgView_Click(object sender, EventArgs e)
        {
            bool show = btnIntgView.Tag == null ? false : (bool)btnIntgView.Tag;
            show = !show;
            btnIntgView.Tag = show;
            ShowIntegList(show);
        }

        /// <summary>
        /// Load log file and decrypt to show
        /// </summary>
        private void ShowIntegList(bool show)
        {
            if (show)
            {
                txtIntgLog.Clear();

                // load and decrypt
                var lines = LogUtils.Instance.ReadLogFile("reader", INTG_LOG_FILE_NAME);
                foreach (var line in lines)
                {
                    var decLine = DataUtils.DamoDecrypt(line, 256);
                    txtIntgLog.AppendText(decLine);
                    txtIntgLog.AppendText(Environment.NewLine);
                }
            }

            bpnlIntgList.Visible = txtIntgLog.Visible = show;
        }

        void btnIntgCheck_Click(object sender, EventArgs e)
        {
            var result = POSDeviceManager.SignPad.CheckPOSIntegrity();            
            string alertMsg = string.Empty;

            if (result)
            {
                alertMsg = string.Format(MSG_POS_INTG_OK, "###KSP-6000S1201");
                ShowMessageBox(MessageDialogType.Information, string.Empty, alertMsg);
            }
            else
            {
                alertMsg = string.Format(MSG_POS_INTG_FAILED, "###KSP-6000S1201");
                ShowMessageBox(MessageDialogType.Warning, string.Empty, alertMsg);
            }

            // log 저장
            string logMsg = string.Format("[{0:yyMMdd.HHmmss}] {1}", DateTime.Now, alertMsg);
            var encAlertMsg = DataUtils.DamoEncrypt(logMsg, 256);
            LogUtils.Instance.LogByType("reader", encAlertMsg, new string[] { INTG_LOG_FILE_NAME, "false" });

            if (!result)
            {
                this.Close();
                Application.Exit();
            }
        }

        #endregion
    }

    public class DevChangeSCANNER
    {
        /// <summary>
        /// 
        /// </summary>
        public bool bUse = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bMethod = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bLogicalName = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bPort = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bSpeed = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bDataBit = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bStopBit = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bParity = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bFlowControl = false;
    }

    public class DevChangeCDP
    {
        /// <summary>
        /// 
        /// </summary>
        public bool bUse = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bMethod = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bLogicalName = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bPort = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bSpeed = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bDataBit = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bStopBit = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bParity = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bFlowControl = false;
    }

    public class DevChangePRINTER
    {
        /// <summary>
        /// 
        /// </summary>
        public bool bUse = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bMethod = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bLogicalName = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bPort = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bSpeed = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bDataBit = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bStopBit = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bParity = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bFlowControl = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bLogoBMP = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bCutFeedCn = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bBarCodeWidth = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bBarCodeHeight = false;
    }

    public class DevChangeMSR
    {
        /// <summary>
        /// 
        /// </summary>
        public bool bUse = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bMethod = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bLogicalName = false;
    }

    public class DevChangeCASHDRAWER
    {
        /// <summary>
        /// 
        /// </summary>
        public bool bUse = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bMethod = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bLogicalName = false;
    }

    public class DevChangeSIGNPAD
    {
        /// <summary>
        /// 
        /// </summary>
        public bool bUse = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bMethod = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bLogicalName = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bPort = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bSpeed = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bDataBit = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bStopBit = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bParity = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bFlowControl = false;
    }
}
