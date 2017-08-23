using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Win;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.POS.ST.VI;
using WSWD.WmallPos.POS.ST.PI;
using WSWD.WmallPos.POS.ST.PT;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.FX.NetComm.Tasks.PQ;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PQ;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.POS.FX.Shared.Utils;
using WSWD.WmallPos.FX.NetComm.Tasks;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.DB;

namespace WSWD.WmallPos.POS.ST.VC
{
    /// <summary>
    /// 개시
    /// </summary>
    public partial class POS_ST_M001 : FormBase, ISTM001View
    {
        private STIM001Presenter m_presenter;
        private PQ01DataTask m_sysTimeTask = null;
        private List<string> m_errorList = null;

         public POS_ST_M001()
        {
            InitializeComponent();

            this.IsModal = true;
            this.Text = LBL_TITLE_START;

            this.tmCheckSysTime.Tick += new EventHandler(tmCheckSysTime_Tick);

            m_sysTimeTask = new PQ01DataTask();
            m_sysTimeTask.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pq01_TaskCompleted);
            m_sysTimeTask.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pq01_Errored);

            m_presenter = new STM001Presenter(this);

            // 여전법 추가 0621
            // 90일 지난 개인정보, 카드번호, TRACK II 데이터 등을 삭제처리한다
            m_presenter.Clear85DaysSensData();

            tmCheckSysTime.Enabled = true;
         }

        void InitEvents()
        {
            osiLastTrxnNo.MessageText = string.Empty;
        }

        #region 이벤트처리

        void pq01_Errored(string errorMessage, Exception lastException)
        {
            ShowSystemTimeSyncError(MSG_TIME_SVR_CHECK_ERROR);
        }

        void pq01_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                var data = responseData.DataRecords.ToDataRecords<PQ01RespData>()[0];

                this.BeginInvoke((MethodInvoker)delegate()
                {
                    #region 시스템시간

                    if (!ValidateSystemTime(data.SystemTime))
                    {
                        return;
                    }

                    if (!ValidateSaleDate())
                    {
                        return;
                    }

                    #endregion

                    // 포스, 점포존재여부 확인
                    if (!CheckStorePosNoExists(data.StoreNoExistFg, data.PosNoExistFg))
                        return;

                    // Update POS 전화번호
                    ConfigData.Current.AppConfig.PosInfo.PosTelNo = data.POSTelNo;
                    ConfigData.Current.AppConfig.Save();

                    // 마지막거래번호
                    CheckLastTrxnNo();
                });
            }
            else
            {
                ShowSystemTimeSyncError(MSG_TIME_SVR_CHECK_ERROR);
            }
        }

        void tmCheckSysTime_Tick(object sender, EventArgs e)
        {
            tmCheckSysTime.Enabled = false;
            osiSystemTime.MessageText = MSG_TIME_SYNC_START;
            osiSystemTime.ItemStatus = OpenItemStatus.None;
            m_sysTimeTask.ExecuteTask();
        }

        #endregion

        #region 사용자정의

        void ShowSystemTimeSyncError(string errorMessage)
        {
            osiSystemTime.MessageText = MSG_TIME_SYNC_ERROR;
            osiSystemTime.ItemStatus = OpenItemStatus.Error;

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    if (ShowMessageBox(MessageDialogType.Question, "ERRORCONNECTIONSERVER", errorMessage,
                        new string[] { LBL_RETRY, LBL_FORCE_CONT }) == DialogResult.Yes)
                    {
                        // retry
                        this.tmCheckSysTime.Enabled = true;
                    }
                    else
                    {
                        // 마지막거래번호
                        CheckLastTrxnNo();
                    }
                });
            }
            else
            {
                if (ShowMessageBox(MessageDialogType.Question, "ERRORCONNECTIONSERVER", errorMessage,
                        new string[] { LBL_RETRY, LBL_FORCE_CONT }) == DialogResult.Yes)
                {
                    // retry
                    this.tmCheckSysTime.Enabled = true;
                }
                else
                {
                    // 마지막거래번호
                    CheckLastTrxnNo();
                }
            }
        }

        /// <summary>
        /// Init all devices
        /// </summary>
        void InitDevices()
        {
            m_errorList = new List<string>();
            POSDeviceManager.Initialize(OnDeviceInitialize);
        }

        /// <summary>
        /// 장비초기화 완료 시 상태표시
        /// </summary>
        /// <param name="deviceType"></param>
        void OnDeviceInitialize(POSDeviceTypes deviceType, bool initializeCompleted)
        {
            switch (deviceType)
            {
                case POSDeviceTypes.CashDrawer:
                    osiCashDrawer.MessageText = !initializeCompleted ? MSG_CD_INITING : MSG_CD_INIT_COMPLETE;
                    osiCashDrawer.ItemStatus = !initializeCompleted ? OpenItemStatus.None :
                        POSDeviceManager.CashDrawer.Status == DeviceStatus.OpenError ? OpenItemStatus.Error : OpenItemStatus.OK;
                    if (initializeCompleted && POSDeviceManager.CashDrawer.Status == DeviceStatus.OpenError)
                    {
                        m_errorList.Add(MSG_CD_INIT_ERROR);
                    }
                    Application.DoEvents();
                    break;
                case POSDeviceTypes.LineDisplay:
                    osiCDP.MessageText = !initializeCompleted ? MSG_CDP_INITING : MSG_CDP_INIT_COMPLETE;
                    osiCDP.ItemStatus = !initializeCompleted ? OpenItemStatus.None :
                        POSDeviceManager.LineDisplay.Status == DeviceStatus.OpenError ? OpenItemStatus.Error : OpenItemStatus.OK;
                    if (initializeCompleted && POSDeviceManager.LineDisplay.Status == DeviceStatus.OpenError)
                    {
                        m_errorList.Add(MSG_CDP_INIT_ERROR);
                    }
                    Application.DoEvents();
                    break;
                case POSDeviceTypes.Msr:
                    osiMSR.MessageText = !initializeCompleted ? MSG_MSR_INITING : MSG_MSR_INIT_COMPLETE;

                    // 여전법 변경 0618
                    osiMSR.ItemStatus = OpenItemStatus.OK;

                    // 여전법 변경 0618
                    //osiMSR.ItemStatus = !initializeCompleted ? OpenItemStatus.None :
                    //    POSDeviceManager.Msr.Status == DeviceStatus.OpenError ? OpenItemStatus.Error : OpenItemStatus.OK;
                    /*
                    if (initializeCompleted && POSDeviceManager.Msr.Status == DeviceStatus.OpenError)
                    {
                        m_errorList.Add(MSG_MSR_INIT_COMPLETE);
                    }*/
                    Application.DoEvents();
                    break;
                case POSDeviceTypes.Printer:
                    osiPrinter.MessageText = !initializeCompleted ? MSG_PRINTER_INITING : MSG_PRINTER_INIT_COMPLETE;
                    osiPrinter.ItemStatus = !initializeCompleted ? OpenItemStatus.None :
                        POSDeviceManager.Printer.Status == DeviceStatus.OpenError ? OpenItemStatus.Error : OpenItemStatus.OK;
                    if (initializeCompleted && POSDeviceManager.Printer.PrinterStatus != POSPrinterStatus.Openned)
                    {
                        m_errorList.Add(POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.Closed ?
                            MSG_PRINTER_INIT_ERROR : MSG_PRINTER_COVER_OPEN);
                    }
                    Application.DoEvents();
                    break;
                case POSDeviceTypes.ScannerGun:
                    osiScanner.MessageText = !initializeCompleted ? MSG_SCANNER_INITING : MSG_SCANNER_INIT_COMPLETE;
                    osiScanner.ItemStatus = !initializeCompleted ? OpenItemStatus.None :
                        POSDeviceManager.Scanner.Status == DeviceStatus.OpenError ? OpenItemStatus.Error : OpenItemStatus.OK;
                    if (initializeCompleted && POSDeviceManager.Scanner.Status == DeviceStatus.OpenError)
                    {
                        m_errorList.Add(MSG_SCANNER_INIT_ERROR);
                    }
                    Application.DoEvents();
                    break;
                case POSDeviceTypes.SignPad:
                    break;
                default:
                    osiKeyboard.MessageText = MSG_KEYBOARD_INIT_ERROR;
                    osiKeyboard.ItemStatus = OpenItemStatus.OK;
                    Application.DoEvents();
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        FormLoading();
                    });
                    break;
            }
        }

        /// <summary>
        /// 테스트용
        /// Printer Error Check, not proceed to Login
        /// </summary>
        void FormLoading()
        {
            string errorMessage = GetDeviceErrorMessageList();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                errorMessage += Environment.NewLine;
                errorMessage += Environment.NewLine;
                errorMessage += "[ERROR]";
                errorMessage += Environment.NewLine;
                errorMessage += string.Join(Environment.NewLine, m_errorList.ToArray());

                ShowMessageBox(MessageDialogType.Error, "DEVICEERROR", errorMessage);
                m_errorList.Clear();

//#if !DEBUG

//                // 프린터 오류시 프로그램 종료
//                if (!Debugger.IsAttached)
//                {
//                    if (POSDeviceManager.Printer.PrinterStatus != POSPrinterStatus.Openned)
//                    {
//                        Trace.WriteLine(errorMessage, "error");
//                        this.DialogResult = DialogResult.Ignore;
//                        return;
//                    }
//                }
//#endif
                //2015.09.01 정광호 수정----------------------------------------------------------
                // 프린터 오류시 프로그램 종료
                //if (!Debugger.IsAttached)
                //{
                //    if (POSDeviceManager.Printer.PrinterStatus != POSPrinterStatus.Openned)
                //    {
                //        ShowMessageBox(MessageDialogType.Error, "", "POS프린트를 확인하십시오.");
                //        this.DialogResult = DialogResult.Ignore;
                //        return;
                //    }
                //}
                //--------------------------------------------------------------------------------
            }
            // InitSignPad
            // 여전법 추가 0722
//#if !DEBUG
            InitSignPad();
//#endif

            m_presenter.ValidateOnOpen();
        }

        /// <summary>
        /// Return list of error message
        /// </summary>
        /// <returns></returns>
        string GetDeviceErrorMessageList()
        {
            bool hasError = (POSDeviceManager.Printer.Status != DeviceStatus.Opened) ||
                    (POSDeviceManager.Scanner.Status != DeviceStatus.Opened) ||
                    (POSDeviceManager.LineDisplay.Status != DeviceStatus.Opened) ||
                    //(POSDeviceManager.Msr.Status != DeviceStatus.Opened) ||
                    (POSDeviceManager.CashDrawer.Status != DeviceStatus.Opened);

            return hasError ? MSG_DEVICE_INIT_ERROR : string.Empty;
        }

        /// <summary>
        /// 시간확인
        /// </summary>
        /// <param name="serverTime"></param>
        /// <returns></returns>
        bool ValidateSystemTime(string serverTime)
        {
            DateTime svTime = DateTime.ParseExact(serverTime, "yyyyMMddHHmmss", new System.Globalization.CultureInfo("ko-KR", true));
            var ts = svTime.Subtract(DateTime.Now);
            if (Math.Abs(ts.Days) > 0)
            {
                ShowMessageBox(MessageDialogType.Error, string.Empty, 
                    string.Format(ERR_MSG_SVR_POS_TIME_ERROR, svTime, DateTime.Now));
                this.DialogResult = DialogResult.Ignore;
                return false;
            }

            POSDateTimeUtils.SetTime(serverTime);
            osiSystemTime.ItemStatus = OpenItemStatus.OK;
            osiSystemTime.MessageText = MSG_TIME_SYNC_COMPLETE;

            return true;
        }

        /// <summary>
        /// 24시간 매장아닌경우, 영업일자가 2일 많으면 포스종료
        /// </summary>
        /// <returns></returns>
        bool ValidateSaleDate()
        {
            string saleDate = ConfigData.Current.AppConfig.PosInfo.SaleDate;
            saleDate = string.IsNullOrEmpty(saleDate) ? DateTime.Today.ToString("yyyyMMdd") : saleDate;
              
            var sd = DateTime.ParseExact(saleDate, "yyyyMMdd", 
                new System.Globalization.CultureInfo("ko-KR", true));
            var ts = sd.Subtract(DateTime.Today);

            if (ts.Days >= 1 && !"1".Equals(ConfigData.Current.AppConfig.PosInfo.StoreType))
            {
                ShowMessageBox(MessageDialogType.Error, string.Empty,
                    string.Format(ERR_MSG_SALE_DATE_ERROR, DateTime.Today, sd));
                this.DialogResult = DialogResult.Ignore;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 점포, 포스존재여부확인
        /// </summary>
        /// <param name="storeFg"></param>
        /// <param name="posFg"></param>
        bool CheckStorePosNoExists(string storeFg, string posFg)
        {
            string errorMessage = string.Empty;
            if ("N".Equals(storeFg))
            {
                errorMessage = ERR_MSG_STORE_NOT_EXISTS;
            }
            
            if (string.IsNullOrEmpty(errorMessage) && "N".Equals(posFg))
            {
                errorMessage = ERR_MSG_POS_NOT_EXISTS;
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ShowMessageBox(MessageDialogType.Error, string.Empty, errorMessage);
                this.DialogResult = DialogResult.Ignore;
                return false;
            }

            return true;
        }

        #endregion

        #region 시스템 거래번호 확인 및 날짜확인

        /// <summary>
        /// config에 SaleDatge & PosNo를 가지고 마지막 거래번호 가져온다
        /// 오류나면 재시도 / 강제진행
        ///     - 강제진행은: config에 있는 설정으로 사용한다
        ///     -
        /// 받으면 설정을 업데이트한다
        /// </summary>
        void CheckLastTrxnNo()
        {
            osiLastTrxnNo.MessageText = MSG_GET_LAST_TRXN_NO;

            var pq09 = new PQ09DataTask(ConfigData.Current.AppConfig.PosInfo.SaleDate);
            pq09.TaskCompleted += new TaskCompletedHandler(pq09_TaskCompleted);
            pq09.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pq09_Errored);
            pq09.ExecuteTask();
        }

        void pq09_Errored(string errorMessage, Exception lastException)
        {
            osiLastTrxnNo.ItemStatus = OpenItemStatus.Error;

            this.BeginInvoke((MethodInvoker)delegate()
            {
                var res = ShowMessageBox(MessageDialogType.Question, string.Empty,
                    string.Format(MSG_GET_LAST_TRXN_NO_ERROR, 
                        DateTimeUtils.FormatDateString(ConfigData.Current.AppConfig.PosInfo.SaleDate, "/")),
                    new string[] {
                        LABEL_RETRY, LABEL_CLOSE
                    });
                if (res == DialogResult.Yes)
                {
                    CheckLastTrxnNo();
                }
                else
                {
                    InitDevices();
                }
            });
        }

        /// <summary>
        /// 마지막번호가져올때 성공
        /// </summary>
        /// <param name="responseData"></param>
        void pq09_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                osiLastTrxnNo.ItemStatus = OpenItemStatus.OK;

                var resp = responseData.DataRecords.ToDataRecords<PQ09RespData>()[0];
                int sTrxnNo = TypeHelper.ToInt32(resp.FinalTrxnNo);
                int cfgTrxnNo = TypeHelper.ToInt32(ConfigData.Current.AppConfig.PosInfo.TrxnNo) - 1;
                int dbTrxnNo = GetLastTrxnNoDb();

                sTrxnNo = Math.Max(cfgTrxnNo, sTrxnNo);
                sTrxnNo = Math.Max(dbTrxnNo, sTrxnNo);

                // 증가1
                sTrxnNo++;
                ConfigData.Current.AppConfig.PosInfo.TrxnNo = sTrxnNo.ToString("d6");
                ConfigData.Current.AppConfig.Save();

                this.BeginInvoke((MethodInvoker)delegate()
                {
                    // 장비초기화
                    InitDevices();
                });
            }
        }

        /// <summary>
        /// DB에서 마지막거래번호를 가져온다
        /// </summary>
        /// <returns></returns>
        int GetLastTrxnNoDb()
        {
            int lastTrxnNo = 0;

            // 디비에서 마직막번호 확인 
            var db = TranDbHelper.InitInstance();
            try
            {
                var sql = Extensions.LoadSqlCommand("POS_SD", "GetLastTrxnNo");
                var tn = db.ExecuteScalar(sql,
                    new string[] {
                        "@DD_SALE", "@CD_STORE", "@NO_POS"
                    }, new object[] {
                        ConfigData.Current.AppConfig.PosInfo.SaleDate, 
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo
                    });

                if (tn != null)
                {
                    lastTrxnNo = TypeHelper.ToInt32(tn.ToString());
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                db.Dispose();
            }

            return lastTrxnNo;
        }

        #endregion

        #region IM001View Members

        public void OnValidateOpen(ValidateOpenStatus validateStatus)
        {
            if (validateStatus == ValidateOpenStatus.LastDateNotClosed)
            {
                var dres = ShowMessageBox(MessageDialogType.Question, "WN0001", MSG_ED_CLOSE_ASK);
                if (dres == DialogResult.Yes)
                {
                    // goto 마감처리화면
                    // show Popup
                    var edForm = ChildManager.ShowForm(string.Empty, "WSWD.WmallPos.POS.ED.dll",
                        "WSWD.WmallPos.POS.ED.VC.POS_ED_P003", false, true);
                    edForm.Unload += new EventHandler(edForm_Unload);
                }
                else
                {
                    // 프로그램 종료
                    this.DialogResult = DialogResult.Ignore;
                }
            }
            else if (validateStatus == ValidateOpenStatus.NeedOpen)
            {
                // 개설한다
                var openForm = ChildManager.ShowForm(string.Empty, "WSWD.WmallPos.POS.SD.dll",
                    "WSWD.WmallPos.POS.SD.VC.POS_SD_P001");
                openForm.Unload += new EventHandler(openForm_Unload);
            }
            else
            {
                if (validateStatus == ValidateOpenStatus.UpdateEodFlagLogin)
                {
                    var res = ShowMessageBox(MessageDialogType.Question, "CODE",
                        string.Format(MSG_OPENNED_CONT_ASK, DateTime.Today));
                    if (res == DialogResult.No)
                    {
                        // 프로그램 종료
                        this.DialogResult = DialogResult.Ignore;
                        return;
                    }

                    ConfigData.Current.AppConfig.PosInfo.EodFlag = "N";
                    ConfigData.Current.AppConfig.Save();
                }

                // 로그인한다
                GotoLogin();
            }
        }

        /// <summary>
        /// 개설화면닫을때
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void openForm_Unload(object sender, EventArgs e)
        {
            FormBase form = (FormBase)sender;
            if (form.DialogResult == DialogResult.Cancel)
            {
                this.DialogResult = DialogResult.Ignore;
            }
            else
            {
                // 개설완료 후
                // 로그인한다
                GotoLogin();
            }
        }

        /// <summary>
        /// Login화면으로 이동
        /// </summary>
        private void GotoLogin()
        {
            this.DialogResult = DialogResult.OK;

            // 로그인 페이지이동
            ChildManager.ShowForm(string.Empty, "WSWD.WmallPos.POS.SO.dll", "WSWD.WmallPos.POS.SO.VC.POS_SO_M001");
        }

        void edForm_Unload(object sender, EventArgs e)
        {
            FormBase form = (FormBase)sender;
            if (form.DialogResult == DialogResult.OK)
            {
                // 개설한다
                var openForm = ChildManager.ShowForm(string.Empty, "WSWD.WmallPos.POS.SD.dll",
                    "WSWD.WmallPos.POS.SD.VC.POS_SD_P001");
                openForm.Unload += new EventHandler(openForm_Unload);
            }
            else
            {
                // close app
                this.DialogResult = DialogResult.Ignore;
            }
        }

        #endregion

        /// <summary>
        /// 여전법 추가 0722
        /// </summary>
        void InitSignPad()
        {
            // const string INTG_LOG_FILE_NAME = "integrity";
            const string MSG_POS_INTG_OK = "IC리더기({0}) 무결성 검증이 정상적으로 완료되었습니다.";
            const string MSG_POS_INTG_FAILED = "IC리더기({0}) 무결설 실패하여 WMPOS를 종료합니다.";

            if (POSDeviceManager.SignPad.Status != WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
            {
                POSDeviceManager.SignPad.Initialize(this.axKSNet_Dongle1);

                var result = POSDeviceManager.SignPad.CheckPOSIntegrity();
                string alertMsg = string.Empty;

                if (result)
                {
                    alertMsg = string.Format(MSG_POS_INTG_OK, "###KSP-6000S1201");
                }
                else
                {
                    alertMsg = string.Format(MSG_POS_INTG_FAILED, "###KSP-6000S1201");
                    ShowMessageBox(MessageDialogType.Warning, string.Empty, alertMsg);
                }

                //KSK_20170403
                // log 저장
                //string logMsg = string.Format("[{0:yyMMdd.HHmmss}] {1}", DateTime.Now, alertMsg);
                //var encAlertMsg = DataUtils.DamoEncrypt(logMsg, 256);
                //LogUtils.Instance.LogByType("reader", encAlertMsg, new string[] { INTG_LOG_FILE_NAME, "false" });


                POSDeviceManager.SignPad.Close();
                if (!result)
                {
                    //KSK_20170403
                    //this.Close();
                    //Application.Exit();
                }
            }
        }
    }
}
