using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;

using WSWD.WmallPos.POS.SO.PI;
using WSWD.WmallPos.POS.SO.VI;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.NetComm.Tasks.PQ;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PQ;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.BO.Trans;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.POS.SO.Data;
using System.Windows.Forms;
using WSWD.WmallPos.FX.Shared.Data;

namespace WSWD.WmallPos.POS.SO.PT
{
    /// <summary>
    /// 
    /// </summary>
    public class SOPresenter : ISOPresenter
    {
        private PQ03RespData m_casData = null;
        private IM001View m_view = null;
        public SOPresenter(IM001View view)
        {
            m_view = view;
            Initialize();
        }

        public SOPresenter()
        {
        }

        public void Initialize()
        {
            //if (!string.IsNullOrEmpty(ConfigData.Current.AppConfig.PosInfo.CasNo))
            //{
            //    m_casData = new PQ03RespData()
            //    {
            //        UserNo = ConfigData.Current.AppConfig.PosInfo.CasNo,
            //        UserName = ConfigData.Current.AppConfig.PosInfo.CasName,
            //        Password = ConfigData.Current.AppConfig.PosInfo.CasPass
            //    };
            //}

            // update view
            m_view.Ready();
        }

        /// <summary>
        ///     ① 입력된 비밀번호가 맞는지 확인 한다.
        ///     ② 비밀번호가 맞지 않으면 (참고5 Guid)항목에 오류 메시지 표시
        ///     ③ 비밀번호가 맞으면 
        ///      - Config 파일 "AppConfig.ini" 항목에 값 업데이트
        ///         (CasNo, CasName, CasPass)
        ///      - 해당 Tran 정보를 점서버로 전송한다.
        ///      - 전송 성공하면 SAT011T.FG_PRS = 'Y'로 업데이트
        ///      - 전송 실패하면 SAT011T.FG_PRS = 'E'로 업데이트
        ///      - SignOn 영수증을 출력한다.
        ///     - 전자저널에 SignOn 프린트 정보를 저장 한다.
        ///     - 작업이 완료되면 메인화면으로 전환한다.        
        /// </summary>
        /// <param name="userLoggedIn">사용자로그인 된상태인지</param>
        public void ValidateLogin(bool userLoggedIn)
        {
            #region Validate login

            if (m_casData == null)
            {
                m_view.UpdateStatusMessage(LoginMessageTypes.CheckCasInfo);
                m_view.SetFocus(0);
                return;
            }

            if (!m_casData.Password.Equals(m_view.CasPass))
            {
                m_view.UpdateStatusMessage(LoginMessageTypes.LoginFailed);
                return;
            }

            // 이미 그전에 로그인 된상태이면 
            // 같은사람 로그인 하면
            //  - Login하고 trans는 점서버로 보내지 않음
            // 다른 사람이 로그인 하면
            //  - SIGNOFF
            //  - SIGNON
            if (userLoggedIn)
            {
                // SIGNOFF last User
                // 점서버로 발송
                DoSignOff();
            }

            var basketHeader = MakeTrans(true, m_view.CasNo, m_view.CasName);

            #region 전자저널에 SignOn 프린트 정보를 저장 한다.

            TraceHelper.Instance.JournalWrite("SIGNON", "계산원 : {0} {1}",
                m_view.CasNo,
                m_view.CasName);

            #endregion

            #endregion

            #region Config Update 한다

            ConfigData.Current.AppConfig.PosInfo.CasNo = m_casData.UserNo;
            ConfigData.Current.AppConfig.PosInfo.CasName = m_casData.UserName;
            ConfigData.Current.AppConfig.PosInfo.CasPass = m_casData.Password;
            ConfigData.Current.AppConfig.Save();
            FrameBaseData.Current.OnDataChanged(FrameBaseDataItem.ConfigChanged, null);

            #endregion

            #region SignOn 영수증을 출력한다.

            if (m_view.InvokeRequired)
            {
                m_view.BeginInvoke((MethodInvoker)delegate()
                {
                    if (m_view.ChkPrint())
                    {
                        POSPrinterUtils.Instance.PrintReceiptSignOn(true, true,
                            FXConsts.RECEIPT_SIGN_OK, 
                            basketHeader, DateTime.Now);
                    }

                    m_view.LoginSuccess();
                });
            }
            else
            {
                if (m_view.ChkPrint())
                {
                    POSPrinterUtils.Instance.PrintReceiptSignOn(true, true,
                        FXConsts.RECEIPT_SIGN_OK, 
                        basketHeader, DateTime.Now);
                }

                m_view.LoginSuccess();
            }
            
            #endregion

        }

        /// <summary>
        /// TR만들고 보냄
        /// Journal저장
        /// </summary>
        /// <param name="signOn"></param>
        /// <param name="casNo"></param>
        /// <param name="casName"></param>
        /// <param name="printJournal">저널저장여부</param>
        BasketHeader MakeTrans(bool signOn, string casNo, string casName)
        {
            BasketHeader header = new BasketHeader()
            {
                CasNo = casNo,
                CasName = casName,
                TrxnType = signOn ? NetCommConstants.TRXN_TYPE_SIGNON : NetCommConstants.TRXN_TYPE_SIGNOFF,
                CancType = "0"
            };

            TranDbHelper db = null;
            SQLiteTransaction trans = null;
            try
            {
                db = TranDbHelper.InitInstance();
                trans = db.BeginTransaction();
                TransManager.SaveTrans(header, null, db, trans);
                trans.Commit();
                TransManager.OnTransComplete();
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                trans.Dispose();
                db.Dispose();
            }

            return header;
        }

        /// <summary>
        /// ① Local Table bms030t에서 계산원 정보 확인 하고 없으면 서버와 통신(전문:PQ03 )하여 계산원 확인 한다.
        ///       - 미등록 계산원인경우 오류 메시지 (참고5 Guid) 항목에 표시
        /// ② 조회된 계산원명을 표시한다.
        /// </summary>
        /// <returns></returns>
        public void ValidateCasNo()
        {
            m_casData = null;
            if (string.IsNullOrEmpty(m_view.CasNo))
            {
                return;
            }

            m_view.UpdateStatusMessage(string.Empty);

            using (var db = MasterDbHelper.InitInstance())
            {
                string sql = Extensions.LoadSqlCommand("POS_SO", "ValidateCasNo");

                var ds = db.ExecuteQuery(sql,
                    new string[] {
                        "@ID_USER"
                    },
                    new object[] {
                        m_view.CasNo
                    });

                var list = new List<PQ03RespData>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new PQ03RespData()
                    {
                        UserNo = dr["ID_USER"].ToString(),
                        UserName = dr["NM_USER"].ToString(),
                        EmpNo = dr["NO_EMP"].ToString(),
                        Password = dr["NO_PASS"].ToString(),
                        UserType = dr["FG_USER"].ToString(),
                        ProcFg = dr["FG_USE"].ToString(),
                    });
                }

                if (list.Count == 0)
                {
                    //// not found, check in server, usign task, socket
                    //var pq03Task = new PQ03DataTask(m_view.CasNo);
                    //pq03Task.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pq03Task_TaskCompleted);
                    //pq03Task.ExecuteTask();
                    m_view.UpdateStatusMessage(LoginMessageTypes.NoUserInfo);
                }
                else
                {
                    m_casData = list[0];
                    m_view.CasName = m_casData.UserName;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseData"></param>
        void pq03Task_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.Success)
            {
                // get casName;       
                var data = responseData.DataRecords.ToDataRecords<PQ03RespData>();
                if (data.Length > 0)
                {
                    m_casData = data[0];
                    m_view.CasName = m_casData.UserName;
                }
            }
            else if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.NoInfo)
            {
                m_view.UpdateStatusMessage(LoginMessageTypes.NoUserInfo);
            }
            else
            {
                m_view.UpdateStatusMessage(responseData.Response.ErrorMessage);
            }
        }

        public void DoSignOff()
        {
            MakeTrans(false, ConfigData.Current.AppConfig.PosInfo.CasNo, ConfigData.Current.AppConfig.PosInfo.CasName);
            
            TraceHelper.Instance.JournalWrite("SIGNOFF", "계산원 : {0} {1}",
                ConfigData.Current.AppConfig.PosInfo.CasNo,
                ConfigData.Current.AppConfig.PosInfo.CasName);

            ConfigData.Current.AppConfig.PosInfo.CasNo = string.Empty;
            ConfigData.Current.AppConfig.PosInfo.CasName = string.Empty;
            ConfigData.Current.AppConfig.PosInfo.CasPass = string.Empty;
            ConfigData.Current.AppConfig.Save();
        }
    }
}
