//-----------------------------------------------------------------
/*
 * 화면명   : POS_ED_P002.cs
 * 화면설명 : 계산원 정산
 * 개발자   : 정광호
 * 개발일자 : 2015.04.08
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

namespace WSWD.WmallPos.POS.ED.VC
{
    /// <summary>
    /// 마감관리 - 계산원정산
    /// </summary>
    public partial class POS_ED_P002 : FormBase, IEDP002View
    {
        #region 변수

        //계산원 정산 비즈니스 로직
        private IEDP002presenter m_Presenter;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        #endregion

        #region 생성자

        public POS_ED_P002()
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
            this.btnRun.Click += new EventHandler(btnRun_Click);                                            //실행 button Event
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

            //계산원 정산 비즈니스 로직
            m_Presenter = new EDP002presenter(this);

            txtSaleDate.Text = DateTimeUtils.FormatDateString(ConfigData.Current.AppConfig.PosInfo.SaleDate, "/");  //영업일자
            txtCasNm.Text = ConfigData.Current.AppConfig.PosInfo.CasName;                                           //계산원
            txtCasNm.Tag = ConfigData.Current.AppConfig.PosInfo.CasNo;                                              //계산원 코드

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
                btnRun_Click(btnRun, null);
            }
            else if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_CLEAR)
            {
                this.Close();
            }
        }

        /// <summary>
        /// 실행 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnRun_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;
            if (txtCasNm.Tag == null || txtCasNm.Tag.ToString().Length <= 0 || txtSaleDate.Text.Length <= 0 || m_Presenter == null) return;

            try
            {
                osiMsgBar01.MessageText = "";
                osiMsgBar02.MessageText = "";
                osiMsgBar03.MessageText = "";
                osiMsgBar04.MessageText = "";
                osiMsgBar05.MessageText = "";
                osiMsgBar06.MessageText = "";
                osiMsgBar07.MessageText = "";
                osiMsgBar01.ItemStatus = OpenItemStatus.None;
                osiMsgBar02.ItemStatus = OpenItemStatus.None;
                osiMsgBar03.ItemStatus = OpenItemStatus.None;
                osiMsgBar04.ItemStatus = OpenItemStatus.None;
                osiMsgBar05.ItemStatus = OpenItemStatus.None;
                osiMsgBar06.ItemStatus = OpenItemStatus.None;
                osiMsgBar07.ItemStatus = OpenItemStatus.None;

                if (ShowMessageBox(MessageDialogType.Question, null, string.Format("{0} {1}", txtCasNm.Text, strMsg02)) == DialogResult.Yes)
                {
                    ChildManager.ShowProgress(true);
                    SetControlDisable(true);   

                    //화면 표출 메세지 설정
                    msgBar.Text = strMsg03;
                    Application.DoEvents();

                    m_Presenter.SetTran(osiMsgBar01, osiMsgBar02);
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
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

            this.Close();
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// 합계 점검 조회 내역 셋팅
        /// </summary>
        /// <param name="ds">합계 점검 조회 내역 결과</param>
        public void SetTranPrint(DataSet ds, BasketHeader _bpHeader)
        {
            if (osiMsgBar01.ItemStatus != OpenItemStatus.OK || osiMsgBar02.ItemStatus != OpenItemStatus.OK)
            {
                ChildManager.ShowProgress(false);
                SetControlDisable(false);
                return;
            } 

            try
            {
                if (ChkPrint())
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                    {
                        if (POSPrinterUtils.Instance.SetPrintAccount(true, FXConsts.RECEIPT_NAME_POS_ED_P002, _bpHeader, null, ds.Tables[0], 2).ToString().Length > 0)
                        {
                            osiMsgBar03.MessageText = strMsg05;
                            osiMsgBar03.ItemStatus = OpenItemStatus.OK;
                            Application.DoEvents();
                        }
                        else
                        {
                            osiMsgBar03.MessageText = strMsg06;
                            osiMsgBar03.ItemStatus = OpenItemStatus.Error;
                            Application.DoEvents();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                osiMsgBar03.MessageText = strMsg06;
                osiMsgBar03.ItemStatus = OpenItemStatus.Error;
                Application.DoEvents();
            }

            if (osiMsgBar03.ItemStatus == OpenItemStatus.OK)
            {
                //AppConfig.ini 파일 초기화
                SetInitConfig();
            }
            else
            {
                ChildManager.ShowProgress(false);
                SetControlDisable(false);
                return;
            }
        }

        /// <summary>
        /// AppConfig.ini 파일 초기화
        /// </summary>
        private void SetInitConfig()
        {
            try
            {
                ConfigData.Current.AppConfig.PosInfo.CasNo = string.Empty;    //계산원 코드
                ConfigData.Current.AppConfig.PosInfo.CasName = string.Empty;  //계산원명
                ConfigData.Current.AppConfig.PosInfo.CasPass = string.Empty;  //계산원 비밀번호

                int iShiftCount = TypeHelper.ToInt32(ConfigData.Current.AppConfig.PosInfo.ShiftCount) + 1;
                ConfigData.Current.AppConfig.PosInfo.ShiftCount = iShiftCount > 9 ? iShiftCount.ToString() : "0" + iShiftCount.ToString();  //정산차수
                ConfigData.Current.AppConfig.Save();

                osiMsgBar04.MessageText = strMsg07;
                osiMsgBar04.ItemStatus = OpenItemStatus.OK;
                Application.DoEvents();

                FrameBaseData.Current.OnDataChanged(FrameBaseDataItem.ConfigChanged, null);

                ChildManager.OnLoggedOut();
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                osiMsgBar04.MessageText = strMsg08;
                osiMsgBar04.ItemStatus = OpenItemStatus.Error;
                Application.DoEvents();
            }
            finally
            {
                ChildManager.ShowProgress(false);
                SetControlDisable(false);

                //화면 종료
                this.Close();
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
                        this.Invoke((MethodInvoker)delegate()
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
