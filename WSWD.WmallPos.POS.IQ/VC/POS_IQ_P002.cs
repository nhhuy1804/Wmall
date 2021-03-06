﻿//-----------------------------------------------------------------
/*
 * 화면명   : POS_IQ_P002.cs
 * 화면설명 : 카드사별 매출 조회
 * 개발자   : 정광호
 * 개발일자 : 2015.04.01
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
using WSWD.WmallPos.POS.FX.Win.Controls;

namespace WSWD.WmallPos.POS.IQ.VC
{
    public partial class POS_IQ_P002 : FormBase, IIQP002View
    {
        #region 변수

        //카드사별 매출조회 비즈니스 로직
        private IIQP002presenter m_Presenter;

        /// <summary>
        /// 카드사별 매출조회 저장 테이블
        /// </summary>
        private DataTable dtPrint = null;

        /// <summary>
        /// 총 청구액
        /// </summary>
        private int _totalAmtSale = 0;
        /// <summary>
        /// 총 에누리금액
        /// </summary>
        private int _totalAmtRenr = 0;
        /// <summary>
        /// 총 판매건수
        /// </summary>
        private int _totalCntSale = 0;
        /// <summary>
        /// 총 취소건수
        /// </summary>
        private int _totalCntRetn = 0;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        StringBuilder sbPrint = new StringBuilder();

        #endregion

        #region 생성자

        public POS_IQ_P002()
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
            this.btnPrint.Click += new EventHandler(btnPrint_Click);                                        //발행 button Event
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

            SetControlDisable(true);

            //카드사별 매출조회 설정---------------------------
            m_Presenter = new IQP002presenter(this);
            m_Presenter.GetCardSaleResult();
            //-----------------------------------------------
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
                btnPrint_Click(btnPrint, null);
            }
            else if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_CLEAR)
            {
                this.Close();
            }
        }

        /// <summary>
        /// 발행 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnPrint_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;

            ChildManager.ShowProgress(true);
            SetControlDisable(true);

            try
            {
                if (ChkPrint())
                {
                    DataRow[] drCnt = null;
                    DataRow[] drAmt = null;

                    if (dtPrint != null)
                    {
                        drCnt = dtPrint.Select("col01 = 'cnt'");
                        drAmt = dtPrint.Select("col01 = 'amt'");
                    }

                    POSPrinterUtils.Instance.PrintIQ_P002(true, drCnt, drAmt, _totalCntSale, _totalCntRetn, _totalAmtRenr, _totalAmtSale);
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

                //출력후 폼종료
                this.Close();
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
        /// 카드사별 매출조회 내역 셋팅
        /// </summary>
        /// <param name="ds">카드사별 매출조회 내역 결과</param>
        public void SetCardSaleResult(DataSet ds)
        {
            try
            {
                sbPrint = new StringBuilder();
                txtPrint.Clear();
                Application.DoEvents();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    #region 카드사별 매출조회 내역 Copy
                    DataRow NewDr = null;
                    string strCD_CARD = string.Empty;                   //카드회사코드
                    string strNM_CARD = string.Empty;                   //카드회사명
                    string strCT_SALE = string.Empty;                   //정상매출 매수
                    string strAM_SALE = string.Empty;                   //정상매출 금액
                    string strAM_SENR = string.Empty;                   //정상에누리 금액
                    string strCT_RETN = string.Empty;                   //반품매출 매수
                    string strAM_RETN = string.Empty;                   //반품매출 금액
                    string strAM_RENR = string.Empty;                   //반품에누리 금액

                    dtPrint = new DataTable();
                    dtPrint.Columns.Add("col01");
                    dtPrint.Columns.Add("col02");
                    dtPrint.Columns.Add("col03");
                    dtPrint.Columns.Add("col04");
                    dtPrint.Columns.Add("col05");

                    foreach (DataRow drTemp in ds.Tables[0].Rows)
                    {
                        strCD_CARD = drTemp["CD_CARD"].ToString();
                        strNM_CARD = drTemp["NM_CARD"] != null ? drTemp["NM_CARD"].ToString() : "";
                        strCT_SALE = Convert.ToDecimal(string.Format("{0:F0}", drTemp["CT_SALE"] != null && drTemp["CT_SALE"].ToString() != "" ? drTemp["CT_SALE"] : "0")).ToString();
                        strAM_SALE = Convert.ToDecimal(string.Format("{0:F0}", drTemp["AM_SALE"] != null && drTemp["AM_SALE"].ToString() != "" ? drTemp["AM_SALE"] : "0")).ToString();
                        strAM_SENR = Convert.ToDecimal(string.Format("{0:F0}", drTemp["AM_SENR"] != null && drTemp["AM_SENR"].ToString() != "" ? drTemp["AM_SENR"] : "0")).ToString();
                        strCT_RETN = Convert.ToDecimal(string.Format("{0:F0}", drTemp["CT_RETN"] != null && drTemp["CT_RETN"].ToString() != "" ? drTemp["CT_RETN"] : "0")).ToString();
                        strAM_RETN = Convert.ToDecimal(string.Format("{0:F0}", drTemp["AM_RETN"] != null && drTemp["AM_RETN"].ToString() != "" ? drTemp["AM_RETN"] : "0")).ToString();
                        strAM_RENR = Convert.ToDecimal(string.Format("{0:F0}", drTemp["AM_RENR"] != null && drTemp["AM_RENR"].ToString() != "" ? drTemp["AM_RENR"] : "0")).ToString();

                        NewDr = dtPrint.NewRow();
                        NewDr[0] = "cnt";
                        NewDr[1] = strCD_CARD;
                        NewDr[2] = strNM_CARD;
                        NewDr[3] = strCT_SALE;
                        NewDr[4] = strCT_RETN;
                        dtPrint.Rows.Add(NewDr);

                        NewDr = dtPrint.NewRow();
                        NewDr[0] = "amt";
                        NewDr[1] = strCD_CARD;
                        NewDr[2] = strNM_CARD;
                        NewDr[3] = (TypeHelper.ToInt32(strAM_SENR) - TypeHelper.ToInt32(strAM_RENR)).ToString();
                        NewDr[4] = (TypeHelper.ToInt32(strAM_SALE) - TypeHelper.ToInt32(strAM_RETN)).ToString();
                        dtPrint.Rows.Add(NewDr);

                        #region 매수 및 금액 합계
                        _totalCntSale += TypeHelper.ToInt32(strCT_SALE); //정상매출 건수
                        _totalCntRetn += TypeHelper.ToInt32(strCT_RETN); //반품매출 건수
                        _totalAmtSale += TypeHelper.ToInt32(strAM_SALE); //정상매출 금액
                        _totalAmtSale -= TypeHelper.ToInt32(strAM_RETN); //반품매출 금액
                        _totalAmtRenr += TypeHelper.ToInt32(strAM_SENR); //정상매출 에누리 금액
                        _totalAmtRenr -= TypeHelper.ToInt32(strAM_RENR); //반품매출 에누리 금액 
                        #endregion
                    }
                    #endregion

                    #region 화면 표출

                    DataRow[] drCnt = null;
                    DataRow[] drAmt = null;

                    if (dtPrint != null)
                    {
                        drCnt = dtPrint.Select("col01 = 'cnt'");
                        drAmt = dtPrint.Select("col01 = 'amt'");
                    }

                    txtPrint.BindNoticeInfo(POSPrinterUtils.Instance.PrintIQ_P002(false, drCnt, drAmt, _totalCntSale, _totalCntRetn, _totalAmtRenr, _totalAmtSale));

                    #endregion
                }

                //화면 표출 메세지 설정
                msgBar.Text = strMsg02;
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
                        else if (item.GetType().Name.ToString().ToLower() == "printlabelinfo")
                        {
                            PrintLabelInfo lbl = (PrintLabelInfo)item;
                            lbl.Enabled = !_bDisable;
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "salegridpanel")
                        {
                            SaleGridPanel grd = (SaleGridPanel)item;
                            grd.Enabled = !_bDisable;
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
                    else if (item.GetType().Name.ToString().ToLower() == "printlabelinfo")
                    {
                        PrintLabelInfo lbl = (PrintLabelInfo)item;
                        lbl.Enabled = !_bDisable;
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "salegridpanel")
                    {
                        SaleGridPanel grd = (SaleGridPanel)item;
                        grd.Enabled = !_bDisable;
                    }
                }
            }

            //Application.DoEvents();
        }

        #endregion
    }
}
