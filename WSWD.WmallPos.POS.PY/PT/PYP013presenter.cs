//-----------------------------------------------------------------
/*
 * 화면명   : PYP013presenter.cs
 * 화면설명 : 현금IC결제
 * 개발자   : 정광호
 * 개발일자 : 2015.05.
*/
//-----------------------------------------------------------------


using WSWD.WmallPos.POS.PY.PI;
using WSWD.WmallPos.POS.PY.VI;

using WSWD.WmallPos.FX.Shared.NetComm.Request.PV;
using WSWD.WmallPos.FX.NetComm.Tasks.PV;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PV;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.Shared;

namespace WSWD.WmallPos.POS.PY.PT
{
    public class PYP013presenter : IPYP013presenter
    {
        private IPYP013View m_view;
        public PYP013presenter(IPYP013View view)
        {
            m_view = view;
        }


        #region IPYP013presenter Members

        /// <summary>
        /// 현금IC
        /// </summary>
        /// <param name="isAppr"></param>
        /// <param name="isChipReader"></param>
        /// <param name="icCardSeqNo"></param>
        /// <param name="issuerCode"></param>
        /// <param name="encData"></param>
        /// <param name="trackIII"></param>
        /// <param name="payAmt"></param>
        /// <param name="taxAmt"></param>
        /// <param name="orgApprDate"></param>
        /// <param name="orgApprNo"></param>
        public void ProcessVANCashIC(bool isAppr, bool isChipReader,
            string fgSimpTrxn,
            string icCardSeqNo, string issuerCode,
            string issuerPosCd, 
            string encData, string trackIII, string payAmt, string taxAmt,
            string orgApprDate, string orgApprNo)
        {
            var reqData = new PV04ReqData()
            {
                InputType = isChipReader ? "C" : "R",
                CancType = isAppr ? "0" : "1",
                ICCardSeqNo = icCardSeqNo,
                IssuerRepNo = fgSimpTrxn.Equals("02") ? string.Empty : issuerCode,
                IssuerPosCd = issuerPosCd,
                EncData = fgSimpTrxn.Equals("02") ? string.Empty : encData,
                Track3Data = fgSimpTrxn.Equals("02") ? string.Empty : trackIII,
                FgSimpTrxn = fgSimpTrxn, //string.IsNullOrEmpty(icCardSeqNo) ? "01" : "00",
                PayAmt = payAmt,
                TaxAmt = taxAmt,
                OTTrxnDate = orgApprDate,
                OTApprNo = orgApprNo
            };

            m_view.ShowProgressMessage(true);

            PV04DataTask task = new PV04DataTask(reqData);
            task.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(task_TaskCompleted);
            task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(task_Errored);
            task.ExecuteTask();
        }

        void task_Errored(string errorMessage, System.Exception lastException)
        {
            m_view.ShowProgressMessage(false);
            m_view.ShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.CommError, errorMessage, string.Empty);
        }

        void task_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            m_view.ShowProgressMessage(false);
            if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.Success)
            {
                var response = responseData.DataRecords.ToDataRecords<PV04RespData>()[0];
                if ("0000".Equals(response.RespCode))
                {
                    m_view.OnReturnSuccess(response);
                }
                else
                {
                    m_view.ShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.SomeError,
                        string.Format("{0} {1}", response.RespMessage1.TrimNull(),
                        response.RespMessage2.TrimNull()), response.RespCode);
                }
            }
            else
            {
                m_view.ShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType.NoInfoFound,
                    responseData.Response.ErrorMessage.TrimNull(), string.Empty);
            }
        }


        /// <summary>
        /// 난수확인할것
        /// </summary>
        /// <param name="payAmt"></param>
        /// <param name="taxAmt"></param>
        public void ProcessGetRanNum(string payAmt, string taxAmt)
        {
            var reqData = new PV04ReqData()
            {
                TrxnType = PV04RespData.REQ_RAND_NUM,
                InputType = "C",
                CancType = "0",
                FgSimpTrxn = "00", //"01",
                PayAmt = payAmt,
                TaxAmt = taxAmt,
            };

            m_view.ShowProgressMessage(true);

            PV04DataTask task = new PV04DataTask(reqData);
            task.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(task_TaskCompleted);
            task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(task_Errored);
            task.ExecuteTask();
        }

        #endregion
    }
}
