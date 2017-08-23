using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.SD.PI;
using WSWD.WmallPos.POS.SD.PT;
using WSWD.WmallPos.POS.SD.VI;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.FX.NetComm.Tasks.PQ;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PQ;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Devices;

namespace WSWD.WmallPos.POS.SD.VC
{
    /// <summary>
    /// 개설화면
    /// </summary>
    public partial class POS_SD_P001 : FormBase, ISDP001View
    {
        private ISDP001Presenter m_presenter = null;
        private BackgroundWorker m_canOpenCheckWorker = null;
        public POS_SD_P001()
        {
            InitializeComponent();

            this.Text = LBL_TITLE_SD_P001;
            this.IsModal = true;

            m_presenter = new SDP001Presenter(this);
            Step01_SdDateCheckFg();

            // show CDP
            if (POSDeviceManager.LineDisplay.Status == DeviceStatus.Opened)
            {
                POSDeviceManager.LineDisplay.DisplayText(MSG_CDP_OPEN, string.Empty);
            }
        }

        #region 개설여부확인

        #region 개설날자 확인 이벤트

        /// <summary>
        /// 개설여부확인
        /// 
        /// </summary>
        void Step01_SdDateCheckFg()
        {
            AddOpenItemStatus(MSG_SD_CHECK_FG, OpenItemStatus.None);

            var result = m_presenter.PreLoading();
            string saleDate = string.Empty;

            var dates = (string[])result;
            saleDate = dates[0];

            incSaleDate.Text = DateTimeUtils.FormatDateString(saleDate, "-");
            incCurDate.Text = dates[1];

            if (string.IsNullOrEmpty(saleDate))
            {
                // 개설처리 불가능
                messageBar1.Text = ERR_MSG_CANT_SD;
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        ShowMessageBox(MessageDialogType.Warning, "CANTOPEN", ERR_MSG_CANT_SD);
                    });
                }
                else
                {
                    ShowMessageBox(MessageDialogType.Warning, "CANTOPEN", ERR_MSG_CANT_SD);
                }

                this.DialogResult = DialogResult.Cancel;
                return;
            }

            m_canOpenCheckWorker = new BackgroundWorker();
            m_canOpenCheckWorker.DoWork += new DoWorkEventHandler(Step02_CanOpenCheckWorker_DoWork);
            m_canOpenCheckWorker.RunWorkerAsync();
        }

        #endregion

        #region 개설여부확인 통신 이벤트

        void Step02_CanOpenCheckWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            #region 점서버로 개설 승인 확인 전문(전문구분:PQ02)을 Socket으로 송수신 한다

            var pq02 = new PQ02DataTask(m_presenter.SaleDate);
            pq02.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pq02_TaskCompleted);
            pq02.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pq02_Errored);
            pq02.ExecuteTask();

            #endregion
        }

        /// <summary>
        /// 점서버로 개설 승인 오류
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="lastException"></param>
        void pq02_Errored(string errorMessage, Exception lastException)
        {
            Step03_OnOpenCheckResult(string.Empty, errorMessage, lastException);
        }

        /// <summary>
        /// 점서버로 개설 승인완료
        /// </summary>
        /// <param name="responseData"></param>
        void pq02_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                UpdateOpenItemStatus(MSG_SD_CHECK_FG, OpenItemStatus.OK);
                var data = responseData.DataRecords.ToDataRecords<PQ02RespData>()[0];
                m_presenter.DoOpen(true, data.ProcFg);
            }
        }

        #endregion

        #region 개설여부확인 재시도

        public void Step03_OnOpenCheckResult(string procFg, string lastErrorMessage, Exception lastException)
        {
            if (string.IsNullOrEmpty(procFg))
            {
                UpdateOpenItemStatus(MSG_SD_CHECK_FG, OpenItemStatus.Error);
                Step03_RetrySDOpen(lastErrorMessage);
            }
            else
            {
                string errorMessage = "1".Equals(procFg) ? ERR_MSG_SD_OPENNED : ERR_MSG_SD_CANT_OPEN;
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        ShowMessageBox(MessageDialogType.Warning, "FORCECOPEN", errorMessage);
                        this.DialogResult = DialogResult.Cancel;
                    });
                }
                else
                {
                    ShowMessageBox(MessageDialogType.Warning, "FORCECOPEN", errorMessage);
                    this.DialogResult = DialogResult.Cancel;
                }
            }
        }

        /// <summary>
        /// 개설여부확인오류, 통신오류
        /// </summary>
        /// <param name="lastErrorMessage"></param>
        void Step03_RetrySDOpen(string lastErrorMessage)
        {
            StringBuilder sbError = new StringBuilder();
            sbError.AppendLine(ERR_MSG_SD_COMM_FAILED);
            sbError.AppendLine("[ERROR]");
            sbError.AppendLine(lastErrorMessage);

            sbError.AppendLine(ERR_MSG_OFFLINE_SD);
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    var res = ShowMessageBox(MessageDialogType.Question, "FORCECOPEN", sbError.ToString(),
                        new string[] {
                            LBL_BTN_RETRY,
                            LBL_BTN_FORCE_OFFLINE
                        });
                    if (res == DialogResult.No)
                    {
                        // close form,
                        // end app
                        m_presenter.DoOpen(false, string.Empty);
                    }
                    else
                    {
                        UpdateOpenItemStatus(MSG_SD_CHECK_FG, OpenItemStatus.None);
                        m_canOpenCheckWorker.RunWorkerAsync();
                    }
                });
            }
            else
            {
                var res = ShowMessageBox(MessageDialogType.Question, "FORCECOPEN", sbError.ToString(),
                        new string[] {
                            LBL_BTN_RETRY,
                            LBL_BTN_FORCE_OFFLINE
                        });
                if (res == DialogResult.No)
                {
                    // close form,
                    // end app
                    m_presenter.DoOpen(false, string.Empty);
                }
                else
                {
                    UpdateOpenItemStatus(MSG_SD_CHECK_FG, OpenItemStatus.None);
                    m_canOpenCheckWorker.RunWorkerAsync();
                }
            }
        }

        #endregion

        #endregion

        #region IP001View Members

        public void AddOpenItemStatus(string message, WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus status)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    AddStatus(message, status);
                });
            }
            else
            {
                AddStatus(message, status);
            }
        }

        int rowCount = 0;
        void AddStatus(string message, OpenItemStatus status)
        {
            rowCount++;
            if (rowCount > this.borderPanel1.Controls.Count)
            {
                int i = 0;
                while (i < this.borderPanel1.Controls.Count - 1)
                {
                    var osi = (OpenStatusItem)this.borderPanel1.Controls[i];
                    var osi2 = (OpenStatusItem)this.borderPanel1.Controls[i + 1];
                    osi.MessageText = osi2.MessageText;
                    osi.ItemStatus = osi2.ItemStatus;
                    i++;
                }
            }

            var last = (OpenStatusItem)this.borderPanel1.Controls[Math.Min(rowCount - 1, this.borderPanel1.Controls.Count - 1)];
            last.MessageText = message;
            last.ItemStatus = status;
        }

        /// <summary>
        /// Show status text list below
        /// 진행상태 업데이트
        /// </summary>
        /// <param name="message"></param>
        /// <param name="status"></param>
        public void UpdateOpenItemStatus(string message, WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus status)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    OpenStatusItem osi = (OpenStatusItem)this.borderPanel1.Controls[Math.Min(rowCount - 1,
                        this.borderPanel1.Controls.Count - 1)];
                    osi.MessageText = message;
                    osi.ItemStatus = status;
                });
            }
            else
            {
                OpenStatusItem osi = (OpenStatusItem)this.borderPanel1.Controls[Math.Min(rowCount - 1,
                    this.borderPanel1.Controls.Count - 1)];
                osi.MessageText = message;
                osi.ItemStatus = status;
            }
        }

        /// <summary>
        /// 개설완료
        /// </summary>
        public void DoOpenComplete()
        {
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Show Error Message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="retryCallBack"></param>
        /// <param name="nextCallBack"></param>
        public void ShowErrorMessage(string message, ErrorRetryDelegateHandler retryCallBack,
             ErrorRetryDelegateHandler nextCallBack)
        {
            /*
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    if (ShowMessageBox(MessageDialogType.Question, string.Empty, message, new string[] { 
                LBL_BTN_RETRY, LBL_BTN_NEXT}) == DialogResult.Yes)
                    {
                        retryCallBack.Invoke();
                    }
                    else
                    {
                        nextCallBack.Invoke();
                    }
                });
            }
            else
            {
                if (ShowMessageBox(MessageDialogType.Question, string.Empty, message, new string[] { 
                LBL_BTN_RETRY, LBL_BTN_NEXT}) == DialogResult.Yes)
                {
                    retryCallBack.Invoke();
                }
                else
                {
                    nextCallBack.Invoke();
                }
            }
             * */

            // Loc changed 10.19
            // 항상 재시도
            // 
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    ShowMessageBox(MessageDialogType.Warning, string.Empty, message, new string[] { LBL_BTN_RETRY});
                    retryCallBack.Invoke();
                });
            }
            else
            {
                ShowMessageBox(MessageDialogType.Warning, string.Empty, message, new string[] { LBL_BTN_RETRY });
                retryCallBack.Invoke();
            }
        }

        /// <summary>
        /// Printer error 
        /// </summary>
        public void ShowPrinterError()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    ShowMessageBox(MessageDialogType.Warning, string.Empty, FXConsts.ERR_MSG_PRINTER_ERROR);
                });
            }
            else
            {
                ShowMessageBox(MessageDialogType.Warning, string.Empty, FXConsts.ERR_MSG_PRINTER_ERROR);

            }
        }

        #region 프린트 확인

        /// <summary>
        /// 프린트 확인
        /// </summary>
        /// <returns></returns>
        public bool ChkPrint()
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

        #endregion
    }

}
