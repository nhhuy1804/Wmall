//-----------------------------------------------------------------
/*
 * 화면명   : PYP015presenter.cs
 * 화면설명 : 현금영수증 취소
 * 개발자   : 정광호
 * 개발일자 : 2015.05.
*/
//-----------------------------------------------------------------

using System;

using WSWD.WmallPos.POS.PY.PI;
using WSWD.WmallPos.POS.PY.VI;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PV;
using WSWD.WmallPos.FX.NetComm.Tasks.PV;
using WSWD.WmallPos.POS.PY.Data;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PV;
using WSWD.WmallPos.POS.PY.VC;
using WSWD.WmallPos.FX.Shared;

namespace WSWD.WmallPos.POS.PY.PT
{
    public class PYP015presenter : IPYP015presenter
    {
        private IPYP015View m_view;
        public PYP015presenter(IPYP015View view)
        {
            m_view = view;
        }

        #region IPYP015presenter Members

        /// <summary>
        /// 현금영수증취소요청
        /// Loc changed 10.24
        /// 전문 추가정보 같이보내기
        /// 점포, 포스, 거래, 첫상품
        /// 
        /// 여전볍 변경 05.27
        /// PV11ReqDataAdd > PV21ReqDataAdd 
        /// </summary>
        /// <param name="cashType">0: 자진발급, 1: 개인소득공제, 2: 사업자(지출증빙)</param>
        /// <param name="isSwipe"></param>
        /// <param name="otApprDate"></param>
        /// <param name="otApprNo"></param>
        /// <param name="confirmNo"></param>
        /// <param name="cancReason">취소사유</param>
        /// <param name="cashAmt">금액</param>
        /// <param name="vatAmt">세액</param>
        public void MakeCashRecptRequest(int cashType, bool isSwipe, 
            string otApprDate, string otApprNo, string confirmNo, string cancReason, int cashAmt, 
            int vatAmt, PV21ReqDataAdd addData)
        {
            PV12ReqData reqData = new PV12ReqData()
            {
                CancType = "1", // 취소
                CashPayAmt = cashAmt.ToString(),
                Vat = vatAmt.ToString(),
                IDCheckNo = confirmNo,
                InputType = isSwipe ? "S" : "K",
                TrxnCode = cashType == POS_PY_P014.CASHRCP_TYPE_EVIDENCE ? "01" : "00",
                OTApprNo = otApprNo,
                OTSaleDate = otApprDate,
                CancRsn = cancReason,

                // Loc added on 10.24
                // 전문추가정보
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

            //PV02DataTask task = new PV02DataTask(reqData);
            PV12DataTask task = new PV12DataTask(reqData);
            task.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(task_TaskCompleted);
            task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(task_Errored);
            task.ExecuteTask();
        }

        void task_Errored(string errorMessage, Exception lastException)
        {
            m_view.ShowProgressMessage(false);
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
                        string.Format("{0} {1}", response.RespMessage1.Trim(),
                        response.RespMessage2.Trim()), response.RespCode, "CASHRCP");
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
