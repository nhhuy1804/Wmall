//-----------------------------------------------------------------
/*
 * 화면명   : POS_ED_P003.cs
 * 화면설명 : POS 정산
 * 개발자   : 정광호
 * 개발일자 : 2015.04.10
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
using WSWD.WmallPos.POS.FX.Shared.Utils;
using WSWD.WmallPos.POS.FX.Shared;
using System.Runtime.InteropServices;

namespace WSWD.WmallPos.POS.ED.VC
{
    /// <summary>
    /// 마감관리 - POS정산
    /// </summary>
    public partial class POS_ED_P003 : FormBase, IEDP003View
    {
        #region 변수

        /// <summary>
        /// 시스템 강제종료/재시작
        /// </summary>
        /// <param name="lpMachineName">컴퓨터 이름</param>
        /// <param name="lpMessage">종료 전 사용자에게 알릴 메시지</param>
        /// <param name="dwTimeout">종료까지 대기 시간</param>
        /// <param name="bForceAppsClosed">프로그램 강제 종료 여부(false > 강제 종료)</param>
        /// <param name="bRebootAfterShutdown">시스템 종료 후 다시 시작 여부(true > 다시 시작)</param>
        [DllImport("advapi32.dll")]
        public static extern void InitiateSystemShutdown(string lpMachineName, string lpMessage, int dwTimeout, bool bForceAppsClosed, bool bRebootAfterShutdown);

        //POS 정산 비즈니스 로직
        private IEDP003presenter m_Presenter;

        /// <summary>
        /// 포스합계 매출 자료
        /// </summary>
        private DataTable _dtPrint = null;

        /// <summary>
        /// POS정산 자동처리
        /// </summary>
        private bool _bAuto = false;

        /// <summary>
        /// POS정산 성공여부
        /// </summary>
        private bool _bComplete = false;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        BasketHeader _bpHeader;

        /// <summary>
        /// 보류건수 존재 유무
        /// </summary>
        // private bool bCnt = false;

        private int m_ftpFailedCount = 0;
        private int MAX_FTP_COUNT = 3;

        #endregion

        #region 생성자

        /// <summary>
        /// POS 정산
        /// </summary>
        public POS_ED_P003()
        {
            InitializeComponent();

            //Form Load Event
            Load += new EventHandler(form_Load);
        }

        /// <summary>
        /// POS 정산
        /// </summary>
        /// <param name="_bAuto">갤서에서 넘어오는 자동실행 여부</param>
        public POS_ED_P003(bool bAuto)
        {
            InitializeComponent();

            //POS정산 자동처리
            _bAuto = bAuto;

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

            //POS 정산 비즈니스 로직
            m_Presenter = new EDP003presenter(this);

            txtSaleDate.Text = DateTimeUtils.FormatDateString(ConfigData.Current.AppConfig.PosInfo.SaleDate, "/");

            //화면 표출 메세지 설정
            msgBar.Text = strMsg01;

            // show CDP 고객용표시기
            if (POSDeviceManager.LineDisplay.Status == DeviceStatus.Opened)
            {
                POSDeviceManager.LineDisplay.DisplayText(MSG_CDP_CLOSE, string.Empty);
            }

            if (_bAuto)
            {
                //
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                bw.RunWorkerAsync();
            }
            //else
            //{
            //    //보류건수확인
            //    m_Presenter.GetWait();
            //}
        }

        /// <summary>
        /// BackgroundWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_bAuto)
            {
                //POS정산 자동처리
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        ProcessPOSClose();
                    });
                }
                else
                {
                    ProcessPOSClose();
                }

            }
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
                ProcessPOSClose();
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
            ProcessPOSClose();
        }

        /// <summary>
        /// POS Close
        /// </summary>
        void ProcessPOSClose()
        {
            if (_bDisable) return;
            if (txtSaleDate.Text.Length <= 0) return;

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

            if (_bAuto)
            {
                //화면 표출 메세지 설정
                msgBar.Text = strMsg03;

                ChildManager.ShowProgress(true);
                SetControlDisable(true);
                Application.DoEvents();

                SetMsgBar(osiMsgBar01, OpenItemStatus.None, strMsg04);
                SetMsgBar(osiMsgBar02, OpenItemStatus.None, strMsg05);

                m_Presenter.SetTran(osiMsgBar01, osiMsgBar02);
            }
            else
            {
                if (ShowMessageBox(MessageDialogType.Question, null, string.Format("{0} {1}", txtSaleDate.Text, strMsg02)) == DialogResult.Yes)
                {
                    ChildManager.ShowProgress(true);
                    SetControlDisable(true);

                    //화면 표출 메세지 설정
                    msgBar.Text = strMsg03;
                    Application.DoEvents();

                    SetMsgBar(osiMsgBar01, OpenItemStatus.None, strMsg04);
                    SetMsgBar(osiMsgBar02, OpenItemStatus.None, strMsg05);

                    m_Presenter.SetTran(osiMsgBar01, osiMsgBar02);
                }
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

            if (_bAuto)
            {
                this.DialogResult = _bComplete ? DialogResult.OK : DialogResult.Cancel;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// 보류건수 확인
        /// </summary>
        /// <param name="iCnt">보류건수 확인결과값</param>
        public void SetWait(int iCnt)
        {
            //if (iCnt > 0)
            //{
            //    ShowMessageBox(WSWD.WmallPos.FX.Shared.MessageDialogType.Warning, "", string.Format(strMsg23, iCnt));

            //    msgBar.Text = string.Format(strMsg23, iCnt);

            //    //화면 종료
            //    this.btnRun.Enabled = false;

            //    bCnt = true;
            //}

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerAsync();
        }

        /// <summary>
        /// 합계 점검 조회 내역 셋팅
        /// </summary>
        /// <param name="ds">합계 점검 조회 내역 결과</param>
        public void SetTranPrint(DataSet ds, BasketHeader bpHeader)
        {
            if (osiMsgBar01.ItemStatus != OpenItemStatus.OK || osiMsgBar02.ItemStatus != OpenItemStatus.OK)
            {
                ChildManager.ShowProgress(false);
                SetControlDisable(false);
                return;
            }

            _bpHeader = bpHeader;
            _dtPrint = null;
            _dtPrint = ds.Tables[0].Copy();

            timerChk.Tag = "0";
            timerChk.Interval = 3000; //3초 간격으로 10번 확인
            timerChk.Tick += new EventHandler(timerChk_Tick);
            timerChk.Enabled = true;
        }

        /// <summary>
        /// SAT011 업데이트 확인
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timerChk_Tick(object sender, EventArgs e)
        {
            int iUpdateConfirm = TypeHelper.ToInt32(timerChk.Tag.ToString()) + 1;

            SetMsgBar(osiMsgBar03, OpenItemStatus.None, string.Format("{0} ({1}회)", strMsg21, iUpdateConfirm.ToString()));

            timerChk.Enabled = false;
            timerChk.Tag = iUpdateConfirm.ToString();
            m_Presenter.GetTranConfirm();
        }

        /// <summary>
        /// SAT011 업데이트 확인완료
        /// </summary>
        /// <param name="iCnt">업데이트 실패 개수</param>
        public void SetTranConfirm(int iCnt)
        {
            string strMsg = string.Empty;

            if (iCnt == 0)
            {
                _bComplete = true;

                SetMsgBar(osiMsgBar03, OpenItemStatus.OK, strMsg20);

                if (_bAuto)
                {
                    //종료메시지
                    strMsg = string.Format(WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00322"),
                        txtSaleDate.Text, strMsg25);

                    // 정산지 출력 - Journal Print
                    SetPrint(0);

                    //저널파일 FTP 전송
                    if (!SetFTP())
                    {
                        _bDisable = false;
                        btnRun.Enabled = true;
                        btnClose.Enabled = true;
                        ChildManager.ShowProgress(false);
                        SetControlDisable(false);
                        //this.DialogResult = DialogResult.Cancel;
                        return;
                    }

                    // 정산지 출력
                    SetPrint(1);

                    //AppConfig.ini 파일 초기화
                    if (!SetInitConfig())
                    {
                        _bDisable = false;
                        btnRun.Enabled = true;
                        btnClose.Enabled = true;
                        ShowMessageBox(WSWD.WmallPos.FX.Shared.MessageDialogType.Information, "", strMsg12);
                        ChildManager.ShowProgress(false);
                        SetControlDisable(false);
                        // this.DialogResult = DialogResult.Cancel;
                    }
                    else
                    {
                        ShowMessageBox(WSWD.WmallPos.FX.Shared.MessageDialogType.Information, "", strMsg);
                        ChildManager.ShowProgress(false);
                        SetControlDisable(false);
                        this.DialogResult = DialogResult.OK;
                    }

                    _bDisable = false;
                    btnRun.Enabled = true;
                    btnClose.Enabled = true;
                }
                else
                {
                    //종료메시지
                    strMsg = string.Format(WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00322"),
                        txtSaleDate.Text, strMsg24);

                    //정산지 출력 - Journal Print
                    SetPrint(0);

                    //저널파일 FTP 전송
                    if (!SetFTP())
                    {
                        msgBar.Text = strMsg01;
                        _bDisable = false;
                        btnRun.Enabled = true;
                        btnClose.Enabled = true;
                        ChildManager.ShowProgress(false);
                        SetControlDisable(false);
                        return;
                    }

                    //정산지 출력
                    SetPrint(1);

                    //AppConfig.ini 파일 초기화
                    if (!SetInitConfig())
                    {
                        msgBar.Text = strMsg01;
                        ShowMessageBox(WSWD.WmallPos.FX.Shared.MessageDialogType.Information, "", strMsg12);
                        _bDisable = false;
                        btnRun.Enabled = true;
                        btnClose.Enabled = true;
                        ChildManager.ShowProgress(false);
                        SetControlDisable(false);
                        return;
                    }
                    else
                    {
                        ShowMessageBox(WSWD.WmallPos.FX.Shared.MessageDialogType.Information, "", strMsg);

                        #region 포스 종료

                        ChildManager.ShowProgress(false);
                        SetControlDisable(false);

                        //화면 종료
                        this.DialogResult = DialogResult.OK;

                        //시스템 종료
                        InitiateSystemShutdown("\\\\127.0.0.1", null, 0, false, false);

                        #endregion
                    }
                }
            }
            else
            {
                //SAT011 업데이트 확인실패

                if (TypeHelper.ToInt32(timerChk.Tag.ToString()) > 10)
                {
                    SetMsgBar(osiMsgBar03, OpenItemStatus.Error, strMsg19);

                    //에러메시지 확인
                    strMsg = string.Format(WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00393"), strMsg07, iCnt.ToString(), Environment.NewLine, strMsg08);

                    string[] strBtnNm = new string[1];
                    strBtnNm[0] = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00045");
                    //strBtnNm[1] = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00190");

                    ShowMessageBox(MessageDialogType.Warning, null, strMsg, strBtnNm);

                    timerChk.Tag = "0";
                    timerChk.Enabled = true;

                    //if (ShowMessageBox(MessageDialogType.Question, null, strMsg, strBtnNm) == DialogResult.Yes)
                    //{
                    //    //m_Presenter.GetTranConfirm();

                    //    timerChk.Tag = "0";
                    //    timerChk.Interval = 3000; //3초 간격으로 10번 확인
                    //    timerChk.Tick += new EventHandler(timerChk_Tick);
                    //    timerChk.Enabled = true;
                    //}
                    //else
                    //{
                    //    _bDisable = false;
                    //    btnRun.Enabled = true;
                    //    btnClose.Enabled = true;

                    //    ChildManager.ShowProgress(false);
                    //    SetControlDisable(false);

                    //    //화면 종료
                    //    this.DialogResult = DialogResult.Cancel;

                    //    if (!_bAuto)
                    //    {
                    //        //시스템 종료
                    //        InitiateSystemShutdown("\\\\127.0.0.1", null, 0, false, false);
                    //    }
                    //}
                }
                else
                {
                    timerChk.Enabled = true;
                }
            }
        }

        /// <summary>
        /// POS 정산지 출력
        /// </summary>
        /// <param name="printOpt">0: journal print; 1: only print</param>
        /// <returns></returns>
        private bool SetPrint(int printOpt)
        {
            bool bReturn = false;

            if (printOpt != 0)
            {
                SetMsgBar(osiMsgBar05, OpenItemStatus.None, strMsg09);
            }
            try
            {
                bool chkPrint = printOpt == 0 ? true : ChkPrint();
                if (chkPrint)
                {
                    if (_dtPrint != null && _dtPrint.Rows.Count > 0)
                    {
                        if (POSPrinterUtils.Instance.SetPrintAccount(true, FXConsts.RECEIPT_NAME_POS_ED_P003, _bpHeader, null,
                            _dtPrint, printOpt).Length > 0)
                        {
                            bReturn = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                _bComplete = false;
            }
            finally
            {
                if (printOpt != 0)
                    SetMsgBar(osiMsgBar05, bReturn ? OpenItemStatus.OK : OpenItemStatus.Error, bReturn ? strMsg14 : strMsg15);
            }

            return bReturn;
        }

        /// <summary>
        /// 저널파일 FTP 전송
        /// </summary>
        /// <returns></returns>
        private bool SetFTP()
        {
            bool bReturn = false;
            string strFtpMsg = string.Empty;

            SetMsgBar(osiMsgBar04, OpenItemStatus.None, strMsg16);

            try
            {
                string strServer = ConfigData.Current.AppConfig.PosFTP.FtpSvrIP1;
                string strUser = ConfigData.Current.AppConfig.PosFTP.User;
                string strPass = ConfigData.Current.AppConfig.PosFTP.Pass;
                string strPort = ConfigData.Current.AppConfig.PosFTP.FtpComPort1;
                string strPath = Path.Combine(
                    FXConsts.JOURNAL.GetFolder(),
                    string.Format("{0}-{1}-{2}.jrn", ConfigData.Current.AppConfig.PosInfo.SaleDate, ConfigData.Current.AppConfig.PosInfo.StoreNo, ConfigData.Current.AppConfig.PosInfo.PosNo)
                    );
                FileInfo fi = new FileInfo(strPath);

                if (fi.Exists)
                {
                    if (strServer.Length > 0 && strUser.Length > 0 && strPass.Length > 0 && strPort.Length > 0 && strPath.Length > 0)
                    {
                        FtpUtils ftp = new FtpUtils(strServer, strUser, strPass, 10, TypeHelper.ToInt32(strPort));

                        if (ftp.Login(out strFtpMsg))
                        {
                            //폴더 검사
                            string[] arrDir = ftp.GetFileList(string.Format("{0}", ConfigData.Current.AppConfig.PosFTP.JournalPath), out bReturn, out strFtpMsg);

                            if (bReturn)
                            {
                                if (arrDir.Length > 0)
                                {
                                    string strDir = string.Format("{0}/{1}", ConfigData.Current.AppConfig.PosFTP.JournalPath, ConfigData.Current.AppConfig.PosInfo.SaleDate);
                                    bool bMake = false;
                                    foreach (string strTemp in arrDir)
                                    {
                                        if (strTemp == strDir)
                                        {
                                            bMake = true;
                                            break;
                                        }
                                    }

                                    if (bMake)
                                    {
                                        //폴더존재시 이동
                                        bReturn = ftp.ChangeDir(strDir, out strFtpMsg);
                                    }
                                    else
                                    {
                                        bReturn = ftp.MakeDir(strDir, out strFtpMsg);
                                    }
                                }
                            }
                        }

                        if (bReturn)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (ftp.Upload(strPath, out strFtpMsg))
                                {
                                    ftp.Close();
                                    bReturn = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    bReturn = true;
                }

                if (!bReturn)
                {
                    string[] strBtnNm = new string[1];
                    strBtnNm[0] = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00045");
                    //strBtnNm[1] = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00190");

                    //LogUtils.Instance.LogException(strFtpMsg);

                    //if (ShowMessageBox(MessageDialogType.Warning, null, strMsg22, strBtnNm) == DialogResult.Yes)
                    //{

                    //}

                    ShowMessageBox(MessageDialogType.Warning, null, strMsg22, strBtnNm);

#if DEBUG
                    m_ftpFailedCount++;
                    if (m_ftpFailedCount >= MAX_FTP_COUNT)
                    {
                        bReturn = true;
                    }
                    else
                    {
                        bReturn = SetFTP();
                    }
#else   
                    bReturn = SetFTP();
#endif
                    //else
                    //{
                    //    bReturn = false;
                    //    //ChildManager.ShowProgress(false);
                    //    //SetControlDisable(false);

                    //    ////화면 종료
                    //    //this.DialogResult = DialogResult.Cancel;

                    //    //if (!_bAuto)
                    //    //{
                    //    //    //시스템 종료
                    //    //    InitiateSystemShutdown("\\\\127.0.0.1", null, 0, false, false);
                    //    //}
                    //}
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                SetMsgBar(osiMsgBar04, bReturn ? OpenItemStatus.OK : OpenItemStatus.Error, bReturn ? strMsg17 : strMsg18);
            }

            return bReturn;
        }

        /// <summary>
        /// AppConfig.ini 파일 초기화
        /// </summary>
        /// <returns></returns>
        private bool SetInitConfig()
        {
            bool bReturn = false;

            SetMsgBar(osiMsgBar06, OpenItemStatus.None, strMsg10);

            try
            {
                ConfigData.Current.AppConfig.PosInfo.EodFlag = "Y";           //마감구분
                ConfigData.Current.AppConfig.Save();

                bReturn = true;
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                _bComplete = false;
            }
            finally
            {
                SetMsgBar(osiMsgBar06, bReturn ? OpenItemStatus.OK : OpenItemStatus.Error, bReturn ? strMsg11 : strMsg12);
            }

            return bReturn;
        }

        /// <summary>
        /// 진행 메세지바 설정
        /// </summary>
        /// <param name="osiMsgBar04"></param>
        /// <param name="openItemStatus"></param>
        /// <param name="p"></param>
        private void SetMsgBar(OpenStatusItem osiMsgBar, OpenItemStatus openItemStatus, string strMsgBar)
        {
            osiMsgBar.MessageText = strMsgBar;
            osiMsgBar.ItemStatus = openItemStatus;
            Application.DoEvents();
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
