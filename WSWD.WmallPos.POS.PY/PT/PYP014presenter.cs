//-----------------------------------------------------------------
/*
 * 화면명   : PYP014presenter.cs
 * 화면설명 : 현금영수증
 * 개발자   : 정광호 / TCL
 * 개발일자 : 2015.05.15
*/
//-----------------------------------------------------------------

using System;

using WSWD.WmallPos.POS.PY.PI;
using WSWD.WmallPos.POS.PY.VI;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PV;
using WSWD.WmallPos.FX.NetComm.Tasks.PV;
using WSWD.WmallPos.POS.PY.Data;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PV;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.POS.PY.VC;
using WSWD.WmallPos.FX.Shared;

namespace WSWD.WmallPos.POS.PY.PT
{
    public class PYP014presenter : IPYP014presenter
    {
        private IPYP014View m_view;
        public PYP014presenter(IPYP014View view)
        {
            m_view = view;
        }

        #region IPYP014presenter Members

        /// <summary>
        /// Loc added on 10.24
        /// 전문에 점포, 포스, 거래, 첫상품정보 추가
        /// </summary>
        /// <param name="cashType"></param>
        /// <param name="isSwipe"></param>
        /// <param name="confirmNo"></param>
        /// <param name="cashAmt"></param>
        /// <param name="vatAmt"></param>
        /// <param name="addData">추가전문정보: 점포, 첫상품 등</param>
        public void MakeCashRecptRequest(int cashType, bool isSwipe, string confirmNo, int cashAmt, int vatAmt, PV21ReqDataAdd addData)
        {
            PV12ReqData reqData = new PV12ReqData()
            {
                CancType ="0",
                CashPayAmt = cashAmt.ToString(),
                IDCheckNo = confirmNo,                
                InputType = isSwipe ? "S" : "K",
                TrxnCode = cashType == POS_PY_P014.CASHRCP_TYPE_EVIDENCE ? "01" : "00",
                Vat = vatAmt.ToString(),

                // Loc added on 10.24
                StoreCode = addData.StoreCode,
                StoreName = addData.StoreName,
                SaleDate = addData.SaleDate,
                PosNo = addData.PosNo,
                TrxnNo = addData.TrxnNo,
                ItemCode = addData.ItemCode,
                ItemName = addData.ItemName,

                // 여전법 추가 0621
                MaskCardNo = addData.MaskCardNo,
                ENCCardNo = addData.ENCCardNo,
                ENCData = addData.ENCData,
                ENCDataLen = addData.ENCDataLen,
                ENCDataType = addData.ENCDataType,
                ENCUseGbn = addData.ENCUseGbn,
                CATTmlAuthNo = ConfigData.Current.AppConfig.PosOption.CATTmlAuthNo,
                SoftAuthNo = ConfigData.Current.AppConfig.PosOption.SoftAuthNo,
                TMLSerialNo = addData.TMLSerialNo
            };

            m_view.ShowProgressMessage(true);

            PV12DataTask task = new PV12DataTask(reqData);
            //PV02DataTask task = new PV02DataTask(reqData);
            task.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(task_TaskCompleted);
            task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(task_Errored);
            task.ExecuteTask();
        }

        void task_Errored(string errorMessage, Exception lastException)
        {
            m_view.ShowErrorMessage(VANRequestErrorType.CommError, errorMessage, string.Empty, "CASHRCP");
        }

        void task_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            m_view.ShowProgressMessage(false);
            if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.Success)
            {
                var response = responseData.DataRecords.ToDataRecords<PV02RespData>()[0];
                if ("0000".Equals(response.RespCode))
                {
                    m_view.OnReturnSuccess(response);
                }
                else
                {
                    m_view.ShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.SomeError,
                        string.Format("{0} {1}", response.RespMessage1.Trim(), response.RespMessage2.Trim()),
                        response.RespCode, "CASHRCP");
                }
            }
            else
            {
                m_view.ShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.NoInfoFound,
                    responseData.Response.ErrorMessage.Trim(), string.Empty, "CASHRCP");
            }
        }

        #endregion
    }
}
