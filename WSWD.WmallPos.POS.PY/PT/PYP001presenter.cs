//-----------------------------------------------------------------
/*
 * 화면명   : PYP001presenter.cs
 * 화면설명 : 신용카드결제
 * 개발자   : TCL
 * 개발일자 : 2015.05.14
*/
//-----------------------------------------------------------------

using System.Text;

using WSWD.WmallPos.POS.PY.PI;
using WSWD.WmallPos.POS.PY.VI;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PV;
using WSWD.WmallPos.FX.NetComm.Tasks.PV;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PV;
using WSWD.WmallPos.FX.Shared.Utils;
using System.Diagnostics;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.FX.Shared;
using System;

namespace WSWD.WmallPos.POS.PY.PT
{
    public partial class PYP001presenter : IPYP001Presenter
    {
        private IPYP001View m_view;

        /// <summary>
        /// 여전법 추가 0729
        /// </summary>
        private int m_lastFrameNo = 0;
        private int m_cancType = 0;

        public PYP001presenter(IPYP001View view)
        {
            m_view = view;
        }

        //PV21ReqData reqData = null;
        private string sign_data = string.Empty;

        /// <summary>
        /// 신용카드요청
        /// 
        /// 여전법 변경 
        /// PV21ReqDataAdd > PV21ReqDataAdd
        /// PV21ReqData > PV21ReqData
        /// </summary>
        /// <param name="cancType">취소구분 0:승인,1:취소, 9:망취소(EncReaderCom2ndGen)</param>
        /// <param name="trackII"></param>
        /// <param name="installment"></param>
        /// <param name="signData"></param>
        /// <param name="payAmt"></param>
        public void RequestVANCardPayment(
            int cancType,
            string trackII,
            string inputType,
            string installment,
            string signData,
            string payAmt,
            string vatAmt,
            string fgERCard,
            string fgDCCCheck,
            string workKeyIndex,
            string cardPinNo,
            string otApprDate,
            string otApprNo,
            object dccData,
            PV21ReqDataAdd addData)
        {
            int signLength = Encoding.Default.GetByteCount(signData);
            PV21ReqData reqData = new PV21ReqData()
            {
                TrackData = trackII,
                FgERCard = fgERCard,
                PayAmt = payAmt,
                Vat = vatAmt,
                InputType = inputType,
                SignDataType = string.IsNullOrEmpty(signData) ? string.Empty : "K1",
                WorkKeyIndex = string.IsNullOrEmpty(signData) ? "00" : "83",
                Halbu = TypeHelper.ToInt32(installment).ToString("d2"),
                CancType = cancType.ToString(),
                SignData = signData,
                SignMethod = string.IsNullOrEmpty(signData) ? "0" : "1",
                SignDataLength = signLength.ToString().PadLeft(4, '0'),
                OTSaleDate = otApprDate,
                OTApprNo = otApprNo,

                // Loc added on 10.24
                // 전문추가정보
                StoreCode = addData.StoreCode,
                StoreName = addData.StoreName,
                //StoreName = string.Format("{0,-20}",addData.StoreName),   // 20170208.ojg 변경
                SaleDate = addData.SaleDate,
                PosNo = addData.PosNo,
                TrxnNo = addData.TrxnNo,
                ItemCode = addData.ItemCode,
                ItemName = addData.ItemName,
                //ItemName = string.Format("{0,-80}",addData.ItemName),   // 20170208.ojg 변경


                // 여전법 추가 05.27
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

            if ("Y".Equals(fgERCard))
            {
                reqData.ECWorkKeyIndex = workKeyIndex;
                reqData.ERCardPin = cardPinNo;
            }

            if (dccData != null)
            {
                PV21ReqData dccReqData = (PV21ReqData)dccData;
                reqData.FgDCCCheck = fgDCCCheck;
                reqData.DCCCheckNo = dccReqData.DCCCheckNo;
                reqData.BaseCurCodeNo = dccReqData.BaseCurCodeNo;
                reqData.BaseCurAmt = dccReqData.BaseCurAmt;
                reqData.BaseCurAmtDecPoint = dccReqData.BaseCurAmtDecPoint;
                reqData.ExRate = dccReqData.ExRate;
                reqData.ExRateDecPoint = dccReqData.ExRateDecPoint;
                reqData.TrxnCurNo = dccReqData.TrxnCurNo;
                reqData.TrxnCurCode = dccReqData.TrxnCurCode;
                reqData.TrxnCurAmt = dccReqData.TrxnCurAmt;
                reqData.TrxnCurAmtDecPoint = dccReqData.TrxnCurAmtDecPoint;
                reqData.TrxnWonAmt = dccReqData.TrxnWonAmt;
                reqData.RateId = dccReqData.RateId;

                // Loc added on 10.24
                // 전문추가정보
                reqData.StoreCode = addData.StoreCode;
                //20170208.osj 공백 추가
                reqData.StoreName = addData.StoreName;
                //reqData.StoreName = string.Format("{0,-20}", addData.StoreName);//      20170208.ojg 변경 대기
                reqData.SaleDate = addData.SaleDate;
                reqData.PosNo = addData.PosNo;
                reqData.TrxnNo = addData.TrxnNo;
                reqData.ItemCode = addData.ItemCode;
                //20170208.osj 공백 추가
                reqData.ItemName = addData.ItemName;
                //reqData.ItemName = string.Format("{0,-80}", addData.ItemName);//      20170208.ojg 변경 대기
            }

            m_view.ShowProgressMessage(true);

            sign_data = signData;
            PV21DataTask task = new PV21DataTask(reqData);
            //var task = new PV01DataTask(reqData);

            // 망취소용
            m_cancType = cancType;
            if (cancType == 9)
            {
                task.ReqFrameNo = addData.OrgFrameNo;
            }
            task.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(task_TaskCompleted);
            task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(task_Errored);
            task.ExecuteTask();

 
            reqData = null;

            // 여전법 추가 0803
            //reqData.TrackData.ResetZero();
            //reqData.MaskCardNo.ResetZero();
            //reqData.ENCCardNo.ResetZero();
            //reqData.ENCData.ResetZero();
            
            GC.Collect();
        }


        void task_Errored(string errorMessage, System.Exception lastException)
        {
            m_view.ShowProgressMessage(false);
            m_view.ShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.CommError, errorMessage,
                string.Empty, "CARD");
        }

        void task_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            m_view.ShowProgressMessage(false);
            if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.Success)
            {
                var response = responseData.DataRecords.ToDataRecords<PV21RespData>()[0];

                // 여전법 추가0729
                // request frameNo 저장
                this.m_lastFrameNo = responseData.Response.ReqFrameNo;

                // 여전법 변경 
                // PV01RespData > PV21RespData
                // var response = responseData.DataRecords.ToDataRecords<PV01RespData>()[0];
                if ("0000".Equals(response.RespCode))
                {
                    m_view.OnReturnSuccess(response, sign_data);
                }
                else
                {                    
                    m_view.ShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.SomeError,
                        string.Format("{0} {1}", response.RespMessage1.TrimNull(),
                        response.RespMessage2.TrimNull()), response.RespCode, "CARD");
                }
            }
            else
            {
                m_view.ShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.NoInfoFound,
                    responseData.Response.ErrorMessage.TrimNull(), string.Empty, "CARD");
            }
        }
        
        #region IPYP001Presenter 멤버

        public int LastFrameNo
        {
            get { return m_lastFrameNo; }
        }

        #endregion
    }
}
