using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.POS.ST.PI;
using WSWD.WmallPos.POS.ST.VI;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.DB;
using System.Data;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using WSWD.WmallPos.FX.Shared.NetComm;
using System.IO;

namespace WSWD.WmallPos.POS.ST.PT
{
    public class STM001Presenter : STIM001Presenter
    {
        private ISTM001View m_view = null;
        public STM001Presenter(ISTM001View view)
        {
            m_view = view;
        }

        #region IM001Presenter Members

        /// <summary>
        /// /// (4) 당일 개설 여부를 확인하여 개설 작업 자동 처리 한다.
        ///   - 프로그램 기동후 다음 조건을 만족하면 자동으로 개설 작업 진행 한다.
        ///      (CASE 1) CFG.영업일자 < 시스템 일자 AND CFG.정산완료 = 'Y' 이면 개설 처리
        ///      (CASE 2) CFG.매장형태=24시간운영 AND CFG.영업일자 = 시스템 일자 AND CFG.정산완료 = 'Y' 이면 개설 처리
        ///   - 만약 이전 영업일 마감정산이 안되었으면 
        ///      IF 현재일자 > CFG.영업일자 AND CFG.정산여부 = 'N' THEN 
        ///         ① 이전 영업일자 마감여부 확인 MessageBox 표시 (메시지:"이전 일자 마감 미처리 !!", 버튼: 예(정산) 아니오:(종료))
        ///         ② 예(정산) 이면 : 마감처리 회면 호출하여 처리
        ///         ③ 아니오(종료) 이면 : 프로그램 종료
        ///      END IF
        /// </summary>
        public void ValidateOnOpen()
        {
            ValidateOpenStatus validateStatus = ValidateOpenStatus.GotoLogin;

            int sysDate = int.Parse(DateTime.Today.ToString("yyyyMMdd"));
            int saleDate = TypeHelper.ToInt32(ConfigData.Current.AppConfig.PosInfo.SaleDate);

            if (saleDate == 0)
            {
                validateStatus = ValidateOpenStatus.NeedOpen;
            }
            else
            {
                // 24시간운영일때
                if ("1".Equals(ConfigData.Current.AppConfig.PosInfo.StoreType))
                {
                    //2015.09.01정광호 수정----------------------------------------
                    //로직 변경
                    //if (!"Y".Equals(ConfigData.Current.AppConfig.PosInfo.EodFlag))
                    //{
                    //    validateStatus = ValidateOpenStatus.GotoLogin;
                    //}
                    //else
                    //{
                    //    validateStatus = ValidateOpenStatus.NeedOpen;
                    //}

                    if (!"Y".Equals(ConfigData.Current.AppConfig.PosInfo.EodFlag))
                    {
                        if (sysDate == saleDate)
                        {
                            validateStatus = ValidateOpenStatus.GotoLogin;
                        }
                        else if (sysDate > saleDate)
                        {
                            int nowDate = TypeHelper.ToInt32(DateTime.Now.Hour);
                            int eodBaseDate = TypeHelper.ToInt32(ConfigData.Current.AppConfig.PosInfo.EodBaseHour.Length <= 0 || ConfigData.Current.AppConfig.PosInfo.EodBaseHour == "0" ?
                                "1" : ConfigData.Current.AppConfig.PosInfo.EodBaseHour);

                            if (nowDate < eodBaseDate)
                            {
                                validateStatus = ValidateOpenStatus.GotoLogin;
                            }
                            else
                            {
                                validateStatus = ValidateOpenStatus.LastDateNotClosed;
                            }
                        }
                    }
                    else
                    {
                        validateStatus = ValidateOpenStatus.NeedOpen;
                    }
                    //-----------------------------------------------------------------
                }
                else
                {
                    if (!"Y".Equals(ConfigData.Current.AppConfig.PosInfo.EodFlag))
                    {
                        ///
                        /// IF CFG.SaleDate < 시스템알자
                        /// • 전일자 마감 처리 여부 확인 메시지 창에서 [아니오] 클릭하면 프로그램 종료한다.
                        /// • [마감] 클릭하면 전일자 마감 처리 실행 후 개설 작업 처리 한다.
                        ///
                        if (sysDate > saleDate)
                        {
                            validateStatus = ValidateOpenStatus.LastDateNotClosed;
                        }
                        else
                        {
                            validateStatus = ValidateOpenStatus.GotoLogin;
                        }
                    }
                    else
                    {
                        // 개설처리한다
                        if (sysDate > saleDate)
                        {
                            validateStatus = ValidateOpenStatus.NeedOpen;
                        }
                        else
                        {
                            /*
                             "이미 yyyy/mm/dd 마감 처리하였습니다." "매출 등록 하시겠습니까?" 메시지 표시
				                [예] 버튼 : AppConfig.EodFlag 'N'으로 업데이트 하고 SignOn 화면 호출
				                [아니오] 버튼 : 프로그램 종료
                             * */
                            validateStatus = ValidateOpenStatus.UpdateEodFlagLogin;
                        }
                    }
                }
            }

            m_view.OnValidateOpen(validateStatus);
        }

        #endregion

        #region STIM001Presenter 멤버

        /// <summary>
        /// 여전법 추가 0621
        /// 
        /// 디비에 개인정보, 카드번호를
        /// 00으로 마스킹한다
        /// 85일
        /// 
        /// 
        /// </summary>
        public void Clear85DaysSensData()
        {
            int lastDate = Convert.ToInt32(string.IsNullOrEmpty(ConfigData.Current.AppConfig.PosOption.LastDeleteSensData) ? "0" : ConfigData.Current.AppConfig.PosOption.LastDeleteSensData);

            string keepDates = ConfigData.Current.AppConfig.PosOption.SensDataKeepDays;

            if (string.IsNullOrEmpty(keepDates))
            {
                keepDates = "85";
                ConfigData.Current.AppConfig.PosOption.SensDataKeepDays = keepDates;
            }            

            var dToDate = DateTime.Today.AddDays(Convert.ToInt32(keepDates) * -1);
            int toDate = Convert.ToInt32(dToDate.ToString("yyyyMMdd"));

            var db = TranDbHelper.InitInstance();
            var trans = db.BeginTransaction();
            try
            {
                string selectDataSql = Extensions.LoadSqlCommand("POS_ST", "POSClearSensData90DaysSelect");
                string updateDataSql = Extensions.LoadSqlCommand("POS_ST", "POSClearSensData90DaysUpdate");

                var data = db.ExecuteQuery(selectDataSql,
                    new string[] { 
                        "@CD_STORE", 
                        "@NO_POS", 
                        "@BSK_TYPE1", 
                        "@BSK_TYPE2",
                        "@FR_DATE", 
                        "@TO_DATE" },
                    new object[] { 
                        ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo,
                        "101",
                        "400",
                        lastDate,
                        toDate
                    }, trans);

                foreach (DataRow item in data.Tables[0].Rows)
                {
                    bool changed = false;

                    // Parse BasketCardPay
                    string vcCont = item["VC_CONT"].ToString();

                    // BasketBase bb = (BasketBase)BasketBase.Parse(typeof(BasketBase), vcCont);
                    string basketType = vcCont.Substring(0, 3);

                    if (basketType.Equals(BasketTypes.BasketCashRecpt))
                    {
                        BasketCashRecpt bcrcp = (BasketCashRecpt)BasketCashRecpt.Parse(typeof(BasketCashRecpt), vcCont);
                        if (bcrcp.InputWcc.Equals("A"))
                        {
                            changed = true;
                            bcrcp.NoPersonal.ResetZero();
                            bcrcp.NoTrack.ResetZero();
                            vcCont = bcrcp.ToString();
                        }
                    }
                    else if (basketType.Equals(BasketTypes.BasketPay))
                    {
                        BasketPay bp = (BasketPay)BasketPay.Parse(typeof(BasketPay), vcCont);

                        if (bp.PayDtlCd.Equals(NetCommConstants.PAYMENT_DETAIL_CARD) &&
                            bp.PayGrpCd.Equals(NetCommConstants.PAYMENT_GROUP_CARD))
                        {
                            BasketPayCard bpc = (BasketPayCard)BasketPayCard.Parse(typeof(BasketPayCard), vcCont);

                            // Masking
                            changed = true;
                            bpc.CardNo.ResetZero();

                            // 여전법 추가 0623
                            // 저장 하지 않아서 reset zero 필요 없음
                            // bpc.TrackII.ResetZero();
                            vcCont = bpc.ToString();
                        }
                    }

                    // 변경 없음
                    if (!changed)
                    {
                        continue;
                    }


                    // 다시 업데이트
                    db.ExecuteNonQuery(updateDataSql,
                        new string[] {
                            "@CD_STORE",
                            "@NO_POS",
                            "@DD_SALE",
                            "@NO_TRXN",
                            "@SQ_TRXN",
                            "@VC_CONT"
                        },
                        new object[] {
                            ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo,
                        item["DD_SALE"],
                        item["NO_TRXN"],
                        item["SQ_TRXN"],
                        vcCont
                        }, trans);
                }

                // Update Config
                ConfigData.Current.AppConfig.PosOption.LastDeleteSensData = toDate.ToString();
                ConfigData.Current.AppConfig.Save();

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                trans.Dispose();
                db.EndInstance();
            }
        }

        #region Utilities

        void DeleteFilesInFolder(string parentFolder, string prefix, DateTime beforeDate)
        {
            DeleteFilesInFolder(parentFolder, prefix, beforeDate, string.Empty);
        }

        /// <summary>
        /// Delete files
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="beforeDate"></param>
        void DeleteFilesInFolder(string parentFolder, string prefix, DateTime beforeDate, string ext)
        {
            string folder = Path.Combine(parentFolder, prefix);
            if (!Directory.Exists(folder))
            {
                return;
            }

            DirectoryInfo di = new DirectoryInfo(folder);

            // delete log file
            var dir = new DirectoryInfo(folder);
            var files = string.IsNullOrEmpty(ext) ? dir.GetFiles() : dir.GetFiles(ext);
            try
            {
                foreach (FileInfo file in files)
                {
                    if (file.CreationTime < beforeDate)
                    {
                        file.Delete();
                    }
                }
            }
            catch
            {
            }
        }

        #endregion

        #endregion
    }

    public enum ValidateOpenStatus
    {
        LastDateNotClosed,
        NeedOpen,
        UpdateEodFlagLogin,
        GotoLogin
    }
}
