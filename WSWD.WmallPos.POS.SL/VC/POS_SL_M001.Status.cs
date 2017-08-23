using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.POS.SL.Data;
using WSWD.WmallPos.FX.Shared.Data;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.SL.Controls;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.POS.FX.Win;
using System.Drawing;

namespace WSWD.WmallPos.POS.SL.VC
{
    partial class POS_SL_M001
    {
        #region 상태업데이트

        private string m_lastGuideMessage = string.Empty;

        /// <summary>
        /// 계산원 가이드 메시지
        /// </summary>
        public string GuideMessage
        {
            get
            {
                return guideMessage1.Text;
            }
            set
            {
                m_lastGuideMessage = guideMessage1.Text;

                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        guideMessage1.Text = value;
                    });
                }
                else
                {
                    guideMessage1.Text = value;
                }
            }
        }

        /// <summary>
        /// 마지막 메시지 보여주기
        /// </summary>
        public void RestoreGuideMessage()
        {
            if (!string.IsNullOrEmpty(m_lastGuideMessage))
            {
                GuideMessage = m_lastGuideMessage;
            }
        }

        public void ReportErrorMessage(SaleViewErrorMessage errorMessage)
        {
            InputText = string.Empty;

            switch (errorMessage)
            {
                case SaleViewErrorMessage.None:
                    this.ErrorMessage = string.Empty;
                    break;
                case SaleViewErrorMessage.NoCdClass:
                    this.ErrorMessage = ERR_MSG_NO_CD_CLASS;
                    break;
                case SaleViewErrorMessage.NoCdItem:
                    this.ErrorMessage = ERR_MSG_NO_CD_ITEM;
                    break;
                case SaleViewErrorMessage.NoPresetItem:
                    this.ErrorMessage = ERR_MSG_NO_PRESET_ITEM;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 오류메시지 표시
        /// </summary>
        string ErrorMessage
        {
            set
            {
                if (string.IsNullOrEmpty(value.Trim()))
                {
                    return;
                }

                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        ShowMessageBox(MessageDialogType.Error, string.Empty, value);
                        Application.DoEvents();
                    });
                }
                else
                {
                    ShowMessageBox(MessageDialogType.Error, string.Empty, value);
                    Application.DoEvents();
                }
            }
        }

        public void ReportInvalidState(InvalidDataInputState invalidState)
        {
            m_scannerOn = true;
            switch (invalidState)
            {
                case InvalidDataInputState.Waiting:
                    GuideMessage = GUIDE_MSG_PLEASE_WAIT;
                    break;
                case InvalidDataInputState.ConfirmAutoRtn:
                    InputText = string.Empty;
                    GuideMessage = GUIDE_MSG_CONFIRM_AUTORTN;
                    m_lastGuideMessage = GUIDE_MSG_PSTATE_INITIAL_AUTORTN;
                    autoRtnPanel1.SetEnableButton(0, true);
                    autoRtnPanel1.SetEnableButton(1, true);
                    break;

                case InvalidDataInputState.CancelledTrxn:
                    InputText = string.Empty;
                    GuideMessage = GUIDE_MSG_TRXN_CANCELLED;
                    autoRtnPanel1.SetEnableButton(0, false);
                    autoRtnPanel1.SetEnableButton(1, true);
                    break;

                case InvalidDataInputState.ReturnedTrxn:
                    InputText = string.Empty;
                    GuideMessage = GUIDE_MSG_RTN_TRXN;
                    autoRtnPanel1.SetEnableButton(0, false);
                    autoRtnPanel1.SetEnableButton(1, true);
                    break;

                case InvalidDataInputState.AutoRtnNoTrans:
                    InputText = string.Empty;
                    GuideMessage = GUIDE_MSG_TRXN_NOTFOUND;
                    autoRtnPanel1.SetEnableButton(0, false);
                    autoRtnPanel1.SetEnableButton(1, true);
                    break;

                case InvalidDataInputState.TransReturned:
                    InputText = string.Empty;
                    GuideMessage = GUIDE_MSG_TRXN_RTN_ALREADY;
                    autoRtnPanel1.SetEnableButton(0, false);
                    autoRtnPanel1.SetEnableButton(1, true);
                    break;

                case InvalidDataInputState.NotSaleTrans:
                    InputText = string.Empty;
                    GuideMessage = GUIDE_MSG_NOT_SALE_TRXN;
                    autoRtnPanel1.SetEnableButton(0, false);
                    autoRtnPanel1.SetEnableButton(1, true);
                    //m_processState = SaleProcessState.AutoRtnReady;
                    break;

                case InvalidDataInputState.TrxnSvrCheckError:
                    InputText = string.Empty;
                    GuideMessage = GUIDE_MSG_SALE_TRXN_SVR_CHK_ERROR;
                    autoRtnPanel1.SetEnableButton(0, false);
                    autoRtnPanel1.SetEnableButton(1, true);
                    break;

                case InvalidDataInputState.LengthError:
                    GuideMessage = GUIDE_MSG_INPUT_LENGTH_CHECK;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.ReturnOnlySaleItemError:
                    GuideMessage = GUIDE_MSG_ONLY_RETURN_SALE_ITEM;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.SecBarcodeError:
                    GuideMessage = GUIDE_MSG_2ND_BARCODE_ERROR;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.InvalidQty:
                    GuideMessage = GUIDE_MSG_INPUT_QTY_INVALID;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.InvalidData:
                    GuideMessage = GUIDE_MSG_INPUT_DATA_INVALID;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.InvalidKey:
                    GuideMessage = GUIDE_MSG_INPUT_KEY_INVALID;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.NetworkError:
                    GuideMessage = GUIDE_MSG_NETWORK_PROBLEM;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.NumberOverflow:
                    GuideMessage = GUIDE_MSG_INPUT_NUMBER_OVER;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.InputChangePrice:
                    GuideMessage = GUIDE_MSG_INPUT_CHANGE_PRICE;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.ItemAmtOverflow:
                    GuideMessage = GUIDE_MSG_INPUT_ITEMAMT_OVER;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.TotalAmtOverflow:
                    GuideMessage = GUIDE_MSG_INPUT_TOTALAMT_OVER;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.OverItemCount:
                    GuideMessage = GUIDE_MSG_ITEMCNT_OVER;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.NoHoldTrxn:
                    GuideMessage = GUIDE_MSG_NO_HOLD_TRXN;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.OverPayAmount:
                    GuideMessage = GUIDE_MSG_OVER_TENDER;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.OnlyOnlinePay:
                    GuideMessage = GUIDE_MSG_ONLY_ONLINE_PAY;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.OnlyOfflinePay:
                    GuideMessage = GUIDE_MSG_ONLY_OFFLINE_PAY;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.OnlyOnlineItem:
                    GuideMessage = GUIDE_MSG_ONLY_ONLINE_ITEM;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.OnlyOfflineItem:
                    GuideMessage = GUIDE_MSG_ONLY_OFFLINE_ITEM;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.CantSaleOther:
                    GuideMessage = GUIDE_MSG_CANT_SALE_OTHER;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.CantSaleItem:
                    GuideMessage = GUIDE_MSG_CANT_SALE_ITEM;
                    InputText = string.Empty;
                    break;
                case InvalidDataInputState.TwoCouponInputError:
                    GuideMessage = GUIDE_MSG_CANT_PAY_2_COUPON;
                    InputText = string.Empty;
                    break;
                default:
                    break;
            }
        }

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

        //2015.09.01 정광호 추가-----------------------------------------------------
        public void ShowDateErrorMsg()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    ShowMessageBox(MessageDialogType.Warning, string.Empty, MSG_DATE_ERROR);
                });
            }
            else
            {
                ShowMessageBox(MessageDialogType.Warning, string.Empty, MSG_DATE_ERROR);
            }
        }
        //---------------------------------------------------------------------------

        public string StatusMessage
        {
            set
            {
                FrameBaseData.Current.OnDataChanged(FrameBaseDataItem.StatusBarMessage, value);
            }
        }

        public void ShowProgress(bool showProgress)
        {
            ChildManager.ShowProgress(showProgress);
        }

        /// <summary>
        /// CDP 메시지
        /// </summary>
        /// <param name="payAmt"></param>
        /// <param name="balAmt"></param>
        /// <param name="messageType"></param>
        public void ShowCDPMessage(CDPMessageType messageType, string payAmt, string balAmt)
        {
            int CDP_LENGTH = 20;
            

            if (POSDeviceManager.LineDisplay.Status == DeviceStatus.Opened)
            {
                string msg1 = string.Empty;
                string msg2 = string.Empty;
                string sTotalAmt = string.Empty;
                Int64 totalAmt = 0;
                int nPayAmt = TypeHelper.ToInt32(payAmt);
                int nBalAmt = TypeHelper.ToInt32(balAmt);

                switch (messageType)
                {
                    case CDPMessageType.ItemChanged:
                        var row = gpItems.GetSelectedRow();
                        var rowData = row != null ? row.ItemData : null;

                        if (rowData != null)
                        {
                            string sItemAmt = string.Format("{0:#,##0}", ((PBItemData)rowData).AmItem) != "0" ? (StateRefund ? "-" : "") +
                                string.Format("{0:#,##0}", Math.Abs(TypeHelper.ToInt64(((PBItemData)rowData).AmItem))) : "0";
                            msg1 = MSG_CDP_SALE_1 + sItemAmt.PadLeft(CDP_LENGTH - MSG_CDP_SALE_1.Length, ' ');
                        }

                        totalAmt = TypeHelper.ToInt64(saleSummaryControl1.TotalAmt);
                        sTotalAmt = totalAmt != 0 ? (StateRefund ? "-" : "") +
                            string.Format("{0:#,##0}", Math.Abs(totalAmt)) : "0";
                        msg2 = MSG_CDP_SALE_TOTAL + sTotalAmt.PadLeft(CDP_LENGTH - MSG_CDP_SALE_TOTAL.Length, ' ');

                        break;
                    case CDPMessageType.TransCancel:
                        msg1 = MSG_CDP_CANCEL;
                        msg2 = MSG_CDP_TRANSACTION;
                        break;
                    case CDPMessageType.TransHold:
                        msg1 = MSG_CDP_SUSPENSION;
                        msg2 = MSG_CDP_TRANSACTION;
                        break;
                    case CDPMessageType.TransHoldRelease:
                        msg1 = MSG_CDP_CANCEL + " " + MSG_CDP_SUSPENSION;
                        msg2 = MSG_CDP_TRANSACTION;
                        break;
                    case CDPMessageType.SubTotal:
                        totalAmt = TypeHelper.ToInt64(saleSummaryControl1.TotalAmt);
                        sTotalAmt = totalAmt != 0 ? (StateRefund ? "-" : "") + string.Format("{0:#,##0}", Math.Abs(totalAmt)) : "0";
                        msg1 = MSG_CDP_SALE_TOTAL + sTotalAmt.PadLeft(CDP_LENGTH - MSG_CDP_SALE_TOTAL.Length, ' ');
                        break;
                    default:
                        string sPayAmt = nPayAmt != 0 ? (StateRefund ? "-" : "") + string.Format("{0:#,##0}", Math.Abs(nPayAmt)) : "0";
                        string sPayPrefix = CDPTitleByType(messageType);
                        msg1 = sPayPrefix + sPayAmt.PadLeft(CDP_LENGTH - sPayPrefix.Length, ' ');

                        // 잔액있으면, CHANGE 표시
                        if (nBalAmt > 0)
                        {
                            sPayAmt = nBalAmt != 0 ? (StateRefund ? "-" : "") + string.Format("{0:#,##0}", Math.Abs(nBalAmt)) : "0";
                            sPayPrefix = MSG_CDP_SALE_CHANGE;
                            msg2 = sPayPrefix + sPayAmt.PadLeft(CDP_LENGTH - sPayPrefix.Length, ' ');
                        }
                        else
                        {
                            Int64 recvMoney = TypeHelper.ToInt64(saleSummaryControl1.RecvMoney);
                            if (recvMoney > 0)
                            {
                                sPayAmt = recvMoney != 0 ? (StateRefund ? "-" : "") + string.Format("{0:#,##0}", Math.Abs(recvMoney)) : "0";
                                sPayPrefix = MSG_CDP_SALE_RECEIVE;
                            }
                            msg2 = sPayPrefix + sPayAmt.PadLeft(CDP_LENGTH - sPayPrefix.Length, ' ');
                        }

                        break;
                }

                POSDeviceManager.LineDisplay.DisplayText(msg1, msg2);
            }
        }

        string CDPTitleByType(CDPMessageType messagType)
        {
            switch (messagType)
            {
                case CDPMessageType.PayCash:
                    return "CASH";                    
                case CDPMessageType.PayGift:
                    return "GIFT";                    
                case CDPMessageType.PayCard:
                    return "CARD";                    
                case CDPMessageType.PayCoupon:
                    return "COUPON";                    
                case CDPMessageType.PayPoint:
                    return "POINT";                    
                case CDPMessageType.PayOnline:
                    return "ON-LINE";
                case CDPMessageType.PayOther:
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 상품있는지?
        /// </summary>
        public bool HasItems
        {
            get
            {
                return DataRows.Length > 0 && !m_retainItems;
            }
        }

        #endregion

        #region Summary 부분

        /// <summary>
        /// 하단부분업데이트한다
        /// - ItemChanged
        ///     => update total
        ///     => update presenter total
        ///     => update view summary
        /// </summary>
        public void UpdateSummary(SaleSummaryData summaryData)
        {
            switch (ProcessState)
            {
                case SaleProcessState.Completed:
                    break;
                case SaleProcessState.AutoRtnReady:
                case SaleProcessState.InputStarted:
                    // clear all
                    saleSummaryControl1.RecvMoney = saleSummaryControl1.PaidMoney = saleSummaryControl1.DiscAmt =
                        saleSummaryControl1.ChangeAmt = "0";
                    saleSummaryControl1.TotalAmt = summaryData.TotalAmt.ToString();
                    break;                     
                case SaleProcessState.ItemInputing:                    
                    // clear all
                    saleSummaryControl1.RecvMoney = saleSummaryControl1.PaidMoney = saleSummaryControl1.DiscAmt =
                        saleSummaryControl1.ChangeAmt = "0";
                    saleSummaryControl1.TotalAmt = summaryData.TotalAmt.ToString();
                    break;
                case SaleProcessState.SubTotal:
                case SaleProcessState.Payment:
                    // clear all
                    saleSummaryControl1.TotalAmt = summaryData.TotalAmt.ToString();
                    saleSummaryControl1.RecvMoney = summaryData.RecvAmt.ToString();
                    saleSummaryControl1.PaidMoney = summaryData.PaidAmt.ToString();
                    saleSummaryControl1.DiscAmt = summaryData.DiscAmt.ToString();
                    saleSummaryControl1.ChangeAmt = summaryData.ChangeAmt.ToString();
                    break;
                default:
                    break;
            }

            // CDP업데이트
            ShowCDPMessage(CDPMessageType.ItemChanged, string.Empty, string.Empty); 
        }

        /// <summary>
        /// 결재내역
        /// </summary>
        /// <param name="pays"></param>
        public void UpdatePayList(List<BasketPay> pays)
        {
            saleSummaryControl1.UpdatePayList(pays, true);
        }

        public void UpdatePayList(List<BasketPay> pays, bool scrollToView)
        {
            saleSummaryControl1.UpdatePayList(pays, scrollToView);
        }

        #endregion

        #region 자동반품/정상판매 화면전환

        void ChangeLayoutByMode()
        {
            pnlRightTop.SuspendDrawing();
            pnFuncKeyGroup.SuspendDrawing();
            autoRtnPanel1.SuspendDrawing();

            pnlKeyPad.Visible = pnlRightTop.Visible = pnFuncKeyGroup.Visible = false;
            pnlRightTop.BackColor = StateRefund ? Color.FromArgb(242, 210, 211) : Color.FromArgb(207, 201, 239);
            pnlKeyPad.BackColor = StateRefund ? Color.FromArgb(232, 179, 175) : Color.FromArgb(182, 171, 231);
            saleKeyPad1.BackColor = StateRefund ? Color.FromArgb(232, 179, 175) : Color.FromArgb(207, 201, 239);
            pnFuncKeyGroup.BackColor = StateRefund ? Color.FromArgb(246, 232, 231) : Color.FromArgb(239, 233, 247);

            if (this.SaleMode == SaleModes.AutoReturn)
            {
                pnlRightTop.SendToBack();
                pnFuncKeyGroup.SendToBack();
                autoRtnPanel1.BringToFront();
                autoRtnPanel1.ResetState();
            }
            else
            {
                autoRtnPanel1.SendToBack();
                pnlRightTop.BringToFront();
                pnFuncKeyGroup.BringToFront();
            }

            pnlKeyPad.Visible = pnlRightTop.Visible = pnFuncKeyGroup.Visible = true;
            autoRtnPanel1.ResumeDrawing();
            pnlRightTop.ResumeDrawing();
            pnFuncKeyGroup.ResumeDrawing();
        }

        /// <summary>
        /// Update data to right parth of Autoreturn
        /// </summary>
        /// <param name="keyValues"></param>
        public void AutoRtnShowTrnxInfo(Dictionary<string, string> keyValues)
        {
            autoRtnPanel1.BindTrxnInfo(keyValues);
        }

        private string m_lastStatusMessage;

        /// <summary>
        /// 반품진행메시지표시
        /// </summary>
        /// <param name="payGrpCd"></param>
        /// <param name="payDtlCd"></param>
        public void AutoRtnUpdateStatusMsg(string payGrpCd, string payDtlCd)
        {
            if (SLExtensions.PAYMENT_DETAIL_AUTORTN_END.Equals(payGrpCd))
            {
                autoRtnPanel1.ShowProgressMessage(AUTORTN_MSG_GEN_TR_PRINT);
            }
            else if (SLExtensions.PAYMENT_DETAIL_AUTORTN_TKS_PRESENT.Equals(payGrpCd))
            {
                m_lastStatusMessage = AUTORTN_MSG_TKS_PRSNT_REFUND;
                autoRtnPanel1.ShowProgressMessage(string.IsNullOrEmpty(payDtlCd) ? AUTORTN_MSG_TKS_PRSNT_POP : AUTORTN_MSG_TKS_PRSNT_POP_CANC);
            }
            else if (SLExtensions.PAYMENT_DETAIL_AUTORTN_CANCEL_STARTED.Equals(payGrpCd))
            {
                m_lastStatusMessage = string.Empty;
                autoRtnPanel1.ShowProgressMessage(GUIDE_MSG_AUTORTN_MSG_CANC_RTN_START);
            }
            else
            {
                m_lastStatusMessage = NetCommConstants.PaymentTypeName(payGrpCd, payDtlCd) + WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00408");
                autoRtnPanel1.ShowProgressMessage(NetCommConstants.PaymentTypeName(payGrpCd, payDtlCd) + AUTORTN_MSG_PROCESSING);
            }
        }

        /// <summary>
        /// 메시지표시
        /// Returns:
        /// DialogResult.Yes: 재시도
        /// DialogResult.No: 강제진행
        /// DialogResult.Cancel: 닫기
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="errorMessage"></param>
        /// <param name="allowCancel">닫기버튼</param>
        /// <returns></returns>
        public DialogResult AutoRtnShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType errorType, 
            string errorMessage, bool allowCancel)
        {
            string sErrorMessage = string.Empty;
            if (!string.IsNullOrEmpty(m_lastStatusMessage))
            {
                sErrorMessage = "[" + m_lastStatusMessage + "]";
                sErrorMessage += Environment.NewLine;
            }
            
            switch (errorType)
            {
                case WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.None:
                    break;
                case WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.CommError:
                    sErrorMessage += ERR_MSG_AUTORTN_VAN_COMM_ERROR;
                    sErrorMessage += Environment.NewLine;
                    sErrorMessage += "[ERROR]";
                    sErrorMessage += Environment.NewLine;
                    sErrorMessage += errorMessage;
                    break;
                case WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.NoInfoFound:
                    sErrorMessage += ERR_MSG_AUTORTN_VAN_NOINFO;
                    sErrorMessage += Environment.NewLine;
                    sErrorMessage += "[ERROR]";
                    sErrorMessage += Environment.NewLine;
                    sErrorMessage += errorMessage;
                    break;
                case WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.SomeError:
                default:
                    sErrorMessage += errorMessage;
                    break;
            }

            if (allowCancel)
            {
                return ShowMessageBox(MessageDialogType.YesNoCancel, string.Empty, sErrorMessage, new string[]
                {
                    LABEL_RETRY, LABEL_FORCE_CONT, LABEL_CLOSE
                });
            }

            return ShowMessageBox(MessageDialogType.Question, string.Empty, sErrorMessage, new string[]
                {
                    LABEL_RETRY, LABEL_FORCE_CONT
                });

        }



        /// <summary>
        /// 2015.09.23 정광호 수정
        /// 반품가능여부 확인 메세지
        /// </summary>
        public void ShowRtnError()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    ShowMessageBox(MessageDialogType.Warning, string.Empty, FXConsts.MSG_RTN_NOT);
                });
            }
            else
            {
                ShowMessageBox(MessageDialogType.Warning, string.Empty, FXConsts.MSG_RTN_NOT);
            }
        }

        #endregion
    }
}
