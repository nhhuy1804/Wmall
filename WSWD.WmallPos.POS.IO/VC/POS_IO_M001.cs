//-----------------------------------------------------------------
/*
 * 화면명   : POS_IO_M001.cs
 * 화면설명 : 준비금 설정
 * 개발자   : 정광호
 * 개발일자 : 2015.03.20
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

using WSWD.WmallPos.POS.IO.PI;
using WSWD.WmallPos.POS.IO.PT;
using WSWD.WmallPos.POS.IO.VI;
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

namespace WSWD.WmallPos.POS.IO.VC
{
    public partial class POS_IO_M001 : FormBase, IIOM001View
    {
        #region 변수

        //준비금 비즈니스 로직
        private IIOM001presenter m_Presenter;

        /// <summary>
        /// 준비금 현재 입금회차
        /// </summary>
        private string _strCT_ITEM = string.Empty;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        #endregion

        #region 생성자

        public POS_IO_M001()
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
            
            this.btnClose.Click += new EventHandler(btnClose_Click);                            //닫기 button Event
            this.KeyEvent += new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(form_KeyEvent);    //Key Event
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
            msgBar.Text = strMsg01;

            //준비금 포커스
            txtReserveAmt.SetFocus();

            if (POSDeviceManager.CashDrawer != null && POSDeviceManager.CashDrawer.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened && POSDeviceManager.CashDrawer.Enabled)
            {
                //돈통 open
                POSDeviceManager.CashDrawer.OpenDrawer();
            }

            //준비금 비즈니스 로직
            m_Presenter = new IOM001presenter(this);

            //이전 준비금 조회
            m_Presenter.GetCash();
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

            if (e.Key.OPOSKey == OPOSMapKeys.KEY_NOSALE)
            {
                if (POSDeviceManager.CashDrawer != null && POSDeviceManager.CashDrawer.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened && POSDeviceManager.CashDrawer.Enabled)
                {
                    //돈통 open
                    POSDeviceManager.CashDrawer.OpenDrawer();
                }
            }
            else if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_ENTER)
            {
                e.IsHandled = true;

                //등록
                SetCash();
            }
            else if (!e.IsControlKey)
            {
                msgBar.Text = strMsg01;
            }
        }

        /// <summary>
        /// 닫기 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (_bDisable)
            {
                return;
            }

            this.Close();
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// 이전 준비금 설정
        /// </summary>
        /// <param name="ds">이전 준비금 및 회차 내역</param>
        public void SetBeforeCash(DataSet ds)
        {
            txtPreReserveAmt.Text = "";
            _strCT_ITEM = "1";

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                txtPreReserveAmt.Text = ds.Tables[0].Rows[0]["AM_ITEM"] != null && ds.Tables[0].Rows[0]["AM_ITEM"].ToString() != "0" ? ds.Tables[0].Rows[0]["AM_ITEM"].ToString() : "";
                _strCT_ITEM = ds.Tables[0].Rows[0]["CT_ITEM"] != null && ds.Tables[0].Rows[0]["CT_ITEM"].ToString() != "" && ds.Tables[0].Rows[0]["CT_ITEM"].ToString() != "0" ? ds.Tables[0].Rows[0]["CT_ITEM"].ToString() : "1";
            }
        }

        /// <summary>
        /// 준비금 등록
        /// </summary>
        public void SetCash()
        {
            #region 준비금 TRAN정보 저장
            Int32 iPreReserveAmt = TypeHelper.ToInt32(txtPreReserveAmt.Text.ToString().Length > 0 ? txtPreReserveAmt.Text : "0");   //이전준비금
            Int32 iReserveAmt = TypeHelper.ToInt32(txtReserveAmt.Text.ToString().Length > 0 ? txtReserveAmt.Text : "0");            //최종준비금
            Int32 iReserveNo = TypeHelper.ToInt32(_strCT_ITEM);                                                                     //준비금 차수

            //준비금 확인
            if ((iReserveAmt > 0 && iReserveAmt < 10) || iReserveAmt <= 0)
            {
                txtReserveAmt.Text = "";
                msgBar.Text = strMsg02;
                return;
            }

            iReserveAmt = TypeHelper.RoundDown32(iReserveAmt);  //원단위절사
            txtReserveAmt.Text = iReserveAmt.ToString();        //절사된 준비금 화면에 표시
            Application.DoEvents();

            //준비금 저장
            SetControlDisable(true);
            m_Presenter.SetCash(iPreReserveAmt, iReserveAmt, iReserveNo);
            #endregion
        }

        /// <summary>
        /// 준비금 저장 확인후 프린팅
        /// </summary>
        /// <param name="basketHeader">준비금 헤더정보</param>
        /// <param name="basketReserve">준비금 정보</param>
        public void SetTran(BasketHeader basketHeader, BasketReserve basketReserve)
        {
            try
            {
                if (basketHeader != null)
                {
                    if (ChkPrint())
                    {
                        POSPrinterUtils.Instance.PrintIO_M001(true, basketHeader, basketReserve);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                SetControlDisable(false);
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
        /// <param name="bDisable">true:프로세스 진행중, false :프로세스 종료</param>
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
            }
        }

        #endregion
    }
}
