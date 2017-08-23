using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;

using WSWD.WmallPos.POS.SD.PI;
using WSWD.WmallPos.POS.SD.VI;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.NetComm.Tasks.PQ;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PQ;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.NetComm.Tasks.PD;
using WSWD.WmallPos.FX.NetComm.Tasks;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PD;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.BO.Trans;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using WSWD.WmallPos.POS.FX.Win.Devices;
using System.Windows.Forms;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PD;

namespace WSWD.WmallPos.POS.SD.PT
{
    public class SDP001Presenter : ISDP001Presenter
    {
        #region 변수
        private string m_saleDate = string.Empty;
        private ISDP001View m_view = null;
        private StringBuilder m_printList = null;

        #endregion

        #region 생성자
        public SDP001Presenter(ISDP001View view)
        {
            this.m_view = view;
            this.m_printList = new StringBuilder();
        }
        #endregion

        #region 속성
        public string SaleDate
        {
            get
            {
                return m_saleDate;
            }
        }
        #endregion

        #region IP001Presenter Members

        /// <summary>
        /// (1) 프로그램 기동후 다음 조건을 만족하면 자동으로 화면을 표시 하고 개설 작업 진행 한다.
        ///   (CASE 1) CFG.영업일자 < 시스템 일자 AND CFG.정산완료 = 'Y' 이면 개설 처리
        ///      - 영업일자는 시스템일자로 표시 한다.
        ///   (CASE 2) CFG.매장형태=24시간운영 AND CFG.영업일자 = 시스템 일자 AND CFG.정산완료 = 'Y' 이면 개설 처리
        ///      - 영업일자는 시스템일자 + 1일 로 표시 한다.
        /// </summary>
        public string[] PreLoading()
        {
            int sysDate = int.Parse(DateTime.Today.ToString("yyyyMMdd"));
            int saleDate = TypeHelper.ToInt32(ConfigData.Current.AppConfig.PosInfo.SaleDate);

            if (saleDate == 0 || (saleDate < sysDate && "Y".Equals(ConfigData.Current.AppConfig.PosInfo.EodFlag)))
            {
                m_saleDate = sysDate.ToString();
            }
            else if ("1".Equals(ConfigData.Current.AppConfig.PosInfo.StoreType) &&
                sysDate == saleDate &&
                "Y".Equals(ConfigData.Current.AppConfig.PosInfo.EodFlag))
            {
                m_saleDate = DateTime.Today.AddDays(1).ToString("yyyyMMdd");
            }

            return new string[] { m_saleDate, DateTimeUtils.GetCurrentDateTimeFull() };
        }

        /// <summary>
        /// 개설한다
        /// </summary>
        /// <param name="isOnline">온라인/오프라인</param>
        /// <param name="procFg">개설여부; (개설가능 0 / 기개설 1/ 개설 불가 2) </param>
        public void DoOpen(bool isOnline, string procFg)
        {
            // SOD Status
            ConfigData.Current.AppConfig.PosInfo.SodStatus = isOnline ? "1" : "0";
            ConfigData.Current.AppConfig.Save();

            if (isOnline && !procFg.Equals("0"))
            {
                // program close
                m_view.Step03_OnOpenCheckResult(procFg, string.Empty, null);
                return;
            }

            if (isOnline)
            {
                // start with PD01
                PD01();
            }
            else
            {
                // 저널파일저장한다
                TraceHelper.Instance.JournalWrite("개설", "개설 강제진행");

                // 마지막단계, 마지막거래번호 확인안함
                LastTask(0);
            }
        }

        #endregion

        #region 개설작업 - 마스터다운로드

        #region PD01 점포 마스터

        void PD01()
        {
            var pd01Task = new PD01DataTask();
            pd01Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd01Task_Errored);
            pd01Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd01Task_DownloadProgressChanged);
            pd01Task.ExecuteTask();
        }

        void pd01Task_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("점포정보 다운로드 진행합니다.", OpenItemStatus.None);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("점포정보 다운로드 {0}/{1}건.", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("점포정보 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_STORE, done, total, "OK"));
                PD02();
            }
        }

        void pd01Task_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("점포정보 다운로드 오류.", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_STORE, 0, 0, "ER"));
            m_view.ShowErrorMessage("점포정보 다운로드 오류.", new ErrorRetryDelegateHandler(PD01),
                new ErrorRetryDelegateHandler(PD02));
        }

        #endregion

        #region PD02 - 계산원 마스터

        void PD02()
        {
            var pd02 = new PD02DataTask();
            pd02.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd02_Errored);
            pd02.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd02_DownloadProgressChanged);
            pd02.ExecuteTask();
        }

        void pd02_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("계산원 마스터 다운로드 진행합니다. ", OpenItemStatus.None);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("계산원마스터 다운로드 {0}/{1}건.", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("계산원마스터 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_CAS, done, total, "OK"));
                PD03();
            }
        }

        void pd02_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("계산원 마스터 다운로드 오류. ", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_CAS, 0, 0, "ER"));
            m_view.ShowErrorMessage("계산원 마스터 다운로드 오류. ", new ErrorRetryDelegateHandler(PD02),
                new ErrorRetryDelegateHandler(PD03));
        }

        #endregion

        #region PD03 - 상품마스터

        public void PD03()
        {
            var PD03Task = new PD03DataTask();
            PD03Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(PD03Task_Errored);
            PD03Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(PD03Task_DownloadProgressChanged);
            PD03Task.ExecuteTask();
        }

        public void PD03Task_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("상품마스터 다운로드 진행합니다.", OpenItemStatus.None);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("상품마스터 다운로드 {0}/{1} 진행중...", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("상품마스터 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PROD, done, total, "OK"));
                PD04();
            }
        }

        public void PD03Task_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("상품마스터 다운로드 오류.", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PROD, 0, 0, "ER"));
            m_view.ShowErrorMessage("상품마스터 다운로드 오류.", new ErrorRetryDelegateHandler(PD03),
                new ErrorRetryDelegateHandler(PD04));
        }

        #endregion

        #region PD04 - 품번 마스터

        void PD04()
        {
            var pd04 = new PD04DataTask();
            pd04.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd04_Errored);
            pd04.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd04_DownloadProgressChanged);
            pd04.ExecuteTask();
        }

        void pd04_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("품번 마스터 다운로드 진행합니다.", OpenItemStatus.None);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("품번 마스터 다운로드 {0}/{1}건 진행중", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("품번 마스터 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PRODNO, done, total, "OK"));
                PD05();
            }
        }


        void pd04_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("품번 마스터 다운로드 오류", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PRODNO, 0, 0, "ER"));
            m_view.ShowErrorMessage("품번 마스터 다운로드 오류", new ErrorRetryDelegateHandler(PD04),
                new ErrorRetryDelegateHandler(PD05));
        }

        #endregion

        #region PD05 - 품목 마스터

        void PD05()
        {
            var pd05 = new PD05DataTask();
            pd05.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd05_Errored);
            pd05.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd05_DownloadProgressChanged);
            pd05.ExecuteTask();
        }

        void pd05_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("품목 마스터 다운로드 진행합니다.", OpenItemStatus.None);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("품목 마스터 다운로드 {0}/{1}건 진행중", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("품목 마스터 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PRODITEMM, done, total, "OK"));
                PD06();
            }
        }

        void pd05_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("품목 마스터 다운로드 오류.", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PRODITEMM, 0, 0, "ER"));
            m_view.ShowErrorMessage("품목 마스터 다운로드 오류.", new ErrorRetryDelegateHandler(PD05),
                new ErrorRetryDelegateHandler(PD06));

        }

        #endregion

        #region PD06 - TOUCH상품 그룹

        void PD06()
        {
            var pd06 = new PD06DataTask();
            pd06.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd06_Errored);
            pd06.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd06_DownloadProgressChanged);
            pd06.ExecuteTask();
        }

        void pd06_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("Touch 상품 GROUP 다운로드 진행합니다.", OpenItemStatus.None);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("Touch 상품 GROUP 다운로드 {0}/{1}건 진행중", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("Touch 상품 GROUP 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_DISPPRETOUCH, done, total, "OK"));
                // next task
                PD07();
            }
        }

        void pd06_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("Touch 상품 GROUP 다운로드 오류.", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_DISPPRETOUCH, 0, 0, "ER"));
            m_view.ShowErrorMessage("Touch 상품 GROUP 다운로드 오류.", new ErrorRetryDelegateHandler(PD06),
                new ErrorRetryDelegateHandler(PD07));

        }

        #endregion

        #region PD07 - Touch 상품 GROUP별 상품코드

        void PD07()
        {
            var pd07 = new PD07DataTask();
            pd07.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd07_Errored);
            pd07.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd07_DownloadProgressChanged);
            pd07.ExecuteTask();
        }

        void pd07_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("Touch 상품 GROUP별 상품코드 다운로드 진행합니다.", OpenItemStatus.None);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("Touch 상품 GROUP별 상품코드 다운로드 {0}/{1}건 진행중", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("Touch 상품 GROUP별 상품코드 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_DISPPRODTOUCU, done, total, "OK"));

                // 다음작업
                PD08();
            }
        }

        void pd07_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("Touch 상품 GROUP별 상품코드 해당정보없음", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_DISPPRODTOUCU, 0, 0, "ER"));
            m_view.ShowErrorMessage("Touch 상품 GROUP별 상품코드 해당정보없음", new ErrorRetryDelegateHandler(PD07), new ErrorRetryDelegateHandler(PD08));
        }

        #endregion

        #region PD08 - Preset Key 마스터

        void PD08()
        {
            var pd08 = new PD08DataTask();
            pd08.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd08Task_Errored);
            pd08.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd08_DownloadProgressChanged);
            pd08.ExecuteTask();
        }

        void pd08_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("Key마스터 다운로드 진행합니다.", OpenItemStatus.None);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("Key마스터 다운로드 {0}/{1}건 진행중", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("Key마스터 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_KEYTOUCU, done, total, "OK"));
                PD09();
            }
        }

        void pd08Task_Errored(string errorMessage, Exception lastException)
        {
            m_view.AddOpenItemStatus("Key마스터 다운로드 오류.", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_KEYTOUCU, 0, 0, "ER"));
            m_view.ShowErrorMessage("Key마스터 다운로드 오류.", new ErrorRetryDelegateHandler(PD08), new ErrorRetryDelegateHandler(PD09));
        }

        #endregion

        #region PD09 영수증명판

        void PD09()
        {
            var pd09 = new PD09DataTask();
            pd09.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd09Task_Errored);
            pd09.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd09_DownloadProgressChanged);
            pd09.ExecuteTask();
        }

        void pd09_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("영수증명판 다운로드 진행합니다.", OpenItemStatus.OK);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("영수증명판 다운로드 {0}/{1}건 진행중", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("영수증명판 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_NAMECARD, done, total, "OK"));
                PD10();
            }
        }

        void pd09Task_Errored(string errorMessage, Exception lastException)
        {
            m_view.AddOpenItemStatus("영수증명판 다운로드 오류.", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_NAMECARD, 0, 0, "ER"));
            m_view.ShowErrorMessage("영수증명판 다운로드 오류.", new ErrorRetryDelegateHandler(PD09), new ErrorRetryDelegateHandler(PD10));

        }

        #endregion

        #region PD10 - 브랜드광고

        void PD10()
        {
            var pd10 = new PD10DataTask();
            pd10.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd10Task_Errored);
            pd10.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd10_DownloadProgressChanged);
            pd10.ExecuteTask();
        }

        void pd10_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("브랜드광고 다운로드 진행합니다.", OpenItemStatus.OK);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("브랜드광고 다운로드 {0}/{1}건 진행중", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("브랜드광고 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_AD, done, total, "OK"));
                PD11();
            }
        }

        void pd10Task_Errored(string errorMessage, Exception lastException)
        {
            m_view.AddOpenItemStatus("브랜드광고 다운로드 오류.", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_AD, 0, 0, "ER"));
            m_view.ShowErrorMessage("브랜드광고 다운로드 오류.", new ErrorRetryDelegateHandler(PD10), new ErrorRetryDelegateHandler(PD11));
        }

        #endregion

        #region PD11 - 카드 회사 정보

        void PD11()
        {
            var pd11Task = new PD11DataTask();
            pd11Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd11Task_Errored);
            pd11Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd11Task_DownloadProgressChanged);
            pd11Task.ExecuteTask();
        }

        void pd11Task_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("카드회사정보 다운로드 진행합니다.", OpenItemStatus.OK);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("카드회사정보 다운로드 {0}/{1}건 진행중", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("카드회사정보 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_CARD, done, total, "OK"));
                PD12();
            }
        }

        void pd11Task_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("카드회사정보 다운로드 오류", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_CARD, 0, 0, "ER"));
            m_view.ShowErrorMessage("카드회사정보 다운로드 오류", new ErrorRetryDelegateHandler(PD11), new ErrorRetryDelegateHandler(PD12));
        }

        #endregion

        #region PD12 - 공지사항

        void PD12()
        {
            var pd12 = new PD12DataTask();
            pd12.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd12_Errored);
            pd12.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd12_DownloadProgressChanged);
            pd12.ExecuteTask();
        }

        void pd12_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("공지사항 다운로드 진행합니다.", OpenItemStatus.OK);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("공지사항 다운로드 {0}/{1}건 진행중", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("공지사항 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_NOTICE, done, total, "OK"));
                PD13();
            }
        }

        void pd12_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("공지사항 다운로드 오류.", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_NOTICE, 0, 0, "ER"));
            m_view.ShowErrorMessage("공지사항 다운로드 오류.", new ErrorRetryDelegateHandler(PD12), new ErrorRetryDelegateHandler(PD13));

        }

        #endregion

        /// <summary>
        /// 통합코드
        /// </summary>
        #region PD13

        void PD13()
        {
            var pd13Task = new PD13DataTask();
            pd13Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd13Task_Errored);
            pd13Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd13Task_DownloadProgressChanged);
            pd13Task.ExecuteTask();
        }

        void pd13Task_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("통합코드 다운로드 진행합니다.", OpenItemStatus.OK);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("통합코드 다운로드 {0}/{1}건 진행중", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("통합코드 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_CODE, done, total, "OK"));
                PD14();
            }
        }

        void pd13Task_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("통합코드 다운로드 오류.", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_CODE, 0, 0, "ER"));
            m_view.ShowErrorMessage("통합코드 다운로드 오류", new ErrorRetryDelegateHandler(PD13), new ErrorRetryDelegateHandler(PD14));
        }

        #endregion

        /// <summary>
        /// 메시지 데이타
        /// </summary>
        #region pd14

        void PD14()
        {
            PD14ReqData reqData = new PD14ReqData();
            reqData.StoreNo = ConfigData.Current.AppConfig.PosInfo.StoreNo;
            reqData.MsgType = "P";

            var pd14Task = new PD14DataTask(reqData);
            pd14Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd14Task_Errored);
            pd14Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd14Task_DownloadProgressChanged);
            pd14Task.ExecuteTask();
        }

        void pd14Task_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("메시지데이터 다운로드 진행합니다.", OpenItemStatus.OK);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("메시지데이터 다운로드 {0}/{1} 진행중.", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("메시지데이터 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_MSG, done, total, "OK"));
                PD15();
            }
        }

        void pd14Task_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("메시지데이터 다운로드 오류.", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_MSG, 0, 0, "ER"));
            m_view.ShowErrorMessage("메시지데이터 다운로드 오류", new ErrorRetryDelegateHandler(PD14), new ErrorRetryDelegateHandler(PD15));
        }

        #endregion

        /// <summary>
        /// 타사상품권마스터
        /// </summary>
        #region pd15
        void PD15()
        {
            var pd15Task = new PD15DataTask();
            pd15Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd15Task_Errored);
            pd15Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd15Task_DownloadProgressChanged);
            pd15Task.ExecuteTask();
        }

        void pd15Task_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("타사상품권 마스터 다운로드 진행합니다.", OpenItemStatus.OK);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("타사상품권 마스터 다운로드 {0}/{1}건 진행중", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("타사상품권 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_TGIFT, done, total, "OK"));
                PD16();
            }
        }

        void pd15Task_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("타사상품권 마스터 다운로드 오류.", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_TGIFT, 0, 0, "ER"));
            m_view.ShowErrorMessage("타사상품권 마스터 다운로드 오류.", new ErrorRetryDelegateHandler(PD15), new ErrorRetryDelegateHandler(PD16));
        }

        #endregion

        /// <summary>
        /// 타사상품권 권종
        /// </summary>
        #region pd16
        void PD16()
        {
            var pd16Task = new PD16DataTask();
            pd16Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd16Task_Errored);
            pd16Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd16Task_DownloadProgressChanged);
            pd16Task.ExecuteTask();
        }

        void pd16Task_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("타사상품권 권종 다운로드 진행합니다.", OpenItemStatus.OK);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("타사상품권 권종 다운로드 {0}/{1}건 진행중", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("타사상품권 권종 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_TGIFTTYPE, done, total, "OK"));
                PD17();
            }
        }

        void pd16Task_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("타사상품권 권종 다운로드 오류", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_TGIFTTYPE, 0, 0, "ER"));
            m_view.ShowErrorMessage("타사상품권 권종 다운로드 오류.", new ErrorRetryDelegateHandler(PD16), new ErrorRetryDelegateHandler(PD17));
        }

        #endregion

        /// <summary>
        /// 할인쿠폰
        /// </summary>
        #region pd17
        void PD17()
        {
            var pd17Task = new PD17DataTask();
            pd17Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd17Task_Errored);
            pd17Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd17Task_DownloadProgressChanged);
            pd17Task.ExecuteTask();
        }

        void pd17Task_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("할인쿠폰 다운로드 진행합니다.", OpenItemStatus.OK);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("할인쿠폰 권종 다운로드 {0}/{1}건 진행중", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("할인쿠폰 권종 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_COUPON, done, total, "OK"));
                PD18();
            }
        }

        void pd17Task_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("할인쿠폰 다운로드 오류.", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_COUPON, 0, 0, "ER"));
            m_view.ShowErrorMessage("할인쿠폰 다운로드 오류.", new ErrorRetryDelegateHandler(PD17), new ErrorRetryDelegateHandler(PD18));
        }

        #endregion

        /// <summary>
        /// 프로모션마스터
        /// </summary>
        #region pd18

        void PD18()
        {
            var pd18Task = new PD18DataTask();
            pd18Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd18Task_Errored);
            pd18Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd18Taskk_DownloadProgressChanged);
            pd18Task.ExecuteTask();
        }

        void pd18Taskk_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("프로모션 마스터 다운로드 진행합니다.", OpenItemStatus.OK);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("프로모션 마스터 다운로드 {0}/{1}건 진행중...", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("프로모션 마스터 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PMMST, done, total, "OK"));
                PD19();
            }
        }

        void pd18Task_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("프로모션 마스터 다운로드 오류.", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PMMST, 0, 0, "ER"));
            m_view.ShowErrorMessage("프로모션 마스터 다운로드 오류.", new ErrorRetryDelegateHandler(PD18), new ErrorRetryDelegateHandler(PD19));
        }

        #endregion

        /// <summary>
        /// 프로모션허들마스터
        /// </summary>
        #region pd19

        void PD19()
        {
            var pd19Task = new PD19DataTask();
            pd19Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd19Task_Errored);
            pd19Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd19Taskk_DownloadProgressChanged);
            pd19Task.ExecuteTask();
        }

        void pd19Taskk_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("프로모션허들마스터 다운로드 진행합니다.", OpenItemStatus.OK);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("프로모션허들마스터 다운로드 {0}/{1}건 진행중...", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("프로모션허들마스터 다운로드 완료", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PMHDMST, done, total, "OK"));
                PD20();
            }
        }

        void pd19Task_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("프로모션허들마스터 다운로드 오류.", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PMHDMST, 0, 0, "ER"));
            m_view.ShowErrorMessage("프로모션허들마스터 다운로드 오류.", new ErrorRetryDelegateHandler(PD19), new ErrorRetryDelegateHandler(PD20));
        }

        #endregion

        /// <summary>
        /// 프로모션브랜드마스터
        /// </summary>
        #region pd20

        void PD20()
        {
            var pd20Task = new PD20DataTask();
            pd20Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd20Task_Errored);
            pd20Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd20Taskk_DownloadProgressChanged);
            pd20Task.ExecuteTask();
        }

        void pd20Taskk_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("프로모션브랜드마스터 다운로드 진행합니다.", OpenItemStatus.OK);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("프로모션브랜드마스터 다운로드 {0}/{1}건 진행중...", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("프로모션브랜드마스터 다운로드 완료", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PMBRANDMST, done, total, "OK"));
                PD21();
            }
        }

        void pd20Task_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("프로모션브랜드마스터 다운로드 오류", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PMBRANDMST, 0, 0, "ER"));
            m_view.ShowErrorMessage("프로모션브랜드마스터 다운로드 오류.", new ErrorRetryDelegateHandler(PD20), new ErrorRetryDelegateHandler(PD21));

        }

        #endregion

        /// <summary>
        /// 프로모션허들별 증정권관리
        /// </summary>
        #region pd21

        void PD21()
        {
            var pd21Task = new PD21DataTask();
            pd21Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd21Task_Errored);
            pd21Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd21Taskk_DownloadProgressChanged);
            pd21Task.ExecuteTask();
        }

        void pd21Taskk_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("프로모션허들별 증정권관리 다운로드 진행합니다.", OpenItemStatus.OK);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("프로모션허들별 증정권관리 다운로드 {0}/{1}건 진행중...", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("프로모션허들별 증정권관리 다운로드 완료", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PMHDGIFTMNG, done, total, "OK"));
                PD22();
            }
        }

        void pd21Task_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("프로모션허들별 증정권관리 다운로드 오류.", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PMHDGIFTMNG, 0, 0, "ER"));
            m_view.ShowErrorMessage("프로모션허들별 증정권관리 다운로드 오류", new ErrorRetryDelegateHandler(PD21), new ErrorRetryDelegateHandler(PD22));
        }

        #endregion

        /// <summary>
        /// 프로모션허들별 경품관리
        /// </summary>
        #region pd22

        void PD22()
        {
            var pd22Task = new PD22DataTask();
            pd22Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd22Task_Errored);
            pd22Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd22Taskk_DownloadProgressChanged);
            pd22Task.ExecuteTask();
        }

        void pd22Taskk_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("프로모션허들별 경품관리  다운로드 진행합니다.", OpenItemStatus.OK);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("프로모션허들별 경품관리  다운로드 {0}/{1}건 진행중...", done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("프로모션허들별 경품관리  다운로드 완료", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PMHDGIVEMNG, done, total, "OK"));
                PD23();
            }
        }

        void pd22Task_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("프로모션허들별 경품관리 다운로드 오류.", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PMHDGIVEMNG, 0, 0, "ER"));
            m_view.ShowErrorMessage("프로모션허들별 경품관리 다운로드 오류.", new ErrorRetryDelegateHandler(PD22),
                new ErrorRetryDelegateHandler(PD23));
        }

        #endregion

        /// <summary>
        /// 프로모션 영수증 행사글 
        /// </summary>
        #region pd23

        void PD23()
        {
            var pd23Task = new PD23DataTask();
            pd23Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd23Task_Errored);
            pd23Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd23Taskk_DownloadProgressChanged);
            pd23Task.ExecuteTask();
        }

        void pd23Taskk_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                m_view.AddOpenItemStatus("프로모션 영수증 행사글 다운로드 진행합니다.", OpenItemStatus.OK);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                m_view.UpdateOpenItemStatus(string.Format("프로모션 영수증 행사글 다운로드 {0}/{1}건 진행중...",
                    done, total), OpenItemStatus.OK);
            }
            else
            {
                m_view.UpdateOpenItemStatus("프로모션 영수증 행사글 다운로드 완료.", OpenItemStatus.OK);
                m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PMEVENTTXT, done, total, "OK"));
                PQ09();
            }
        }

        void pd23Task_Errored(string errorMessage, Exception lastException)
        {
            m_view.UpdateOpenItemStatus("프로모션 영수증 행사글 다운로드 오류.", OpenItemStatus.Error);
            m_printList.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PMEVENTTXT, 0, 0, "ER"));
            m_view.ShowErrorMessage("프로모션 영수증 행사글 다운로드 오류.",
                new ErrorRetryDelegateHandler(PD23), new ErrorRetryDelegateHandler(PQ09));

        }

        #endregion

        #region Final Steps

        void LastTask(int trxnNo)
        {
            BackupTrans();

            //2015.09.01 정광호 추가--------------
            //DB자료 삭제
            DeleteDB();
            //------------------------------------

            DeleteTraceLog();
            DeleteLastNotices();
            DeleteTrans();
            UpdateAppConfig(trxnNo);
            SdTran();
            SdPrintReceipt();
            m_view.DoOpenComplete();
        }



        #endregion

        #endregion

        #region 개설작업 - TRANS DB 백업

        /// <summary>
        /// 백업한다
        /// Tran.db 파일을 backup 폴더에 복사 (tran_yyyymmdd.db)
        /// </summary>
        void BackupTrans()
        {
            string transFile = Path.Combine(FXConsts.FOLDER_DATA_TRANS.GetFolder(), FXConsts.DATABASE_FILE_TRAN);
            // file copy
            string newFileName = string.Format("{0}_{1:yyyyMMdd}.db", Path.GetFileNameWithoutExtension(FXConsts.DATABASE_FILE_TRAN), DateTime.Today);
            newFileName = Path.Combine(FXConsts.FOLDER_DATA_BACK.GetFolder(), newFileName);
            File.Copy(transFile, newFileName, true);
        }

        private void DeleteDB()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                throw;
            }
        }

        #endregion

        #region 개설작업 - 과거 TRACE, ERROR LOG, JRN, SIGN 삭제,

        /// <summary>
        /// 여전법 변경 0723
        /// log파일 삭제 (전체)
        /// 보관일수가 85보다 작으니 개설 후 삭제 된다
        /// 다른곳에 log 삭제안 해도 됨.
        /// 
        /// "AppConfig.ini" File의 자료 보관 일수에 맞춰 이전 file 삭제
        /// </summary>
        void DeleteTraceLog()
        {
            // delete file     
            int days = TypeHelper.ToInt32(ConfigData.Current.AppConfig.PosOption.DataKeepDays);
            if (days <= 0)
            {
                return;
            }

            DateTime beforeDate = DateTime.Today.AddDays(days * -1);

            #region Log files
            DeleteFilesInFolder(FXConsts.FOLDER_LOG.GetFolder(), "error", beforeDate);

            // delete png file which having date before beforeDate
            string folder = Path.Combine(FXConsts.FOLDER_LOG.GetFolder(), "error");
            var files = Directory.GetFiles(folder, "*.png");
            foreach (var file in files)
            {
                var cd = File.GetCreationTime(file);
                if (cd <= beforeDate)
                {
                    try
                    {
                        // delete
                        File.Delete(file);
                    }
                    catch
                    {
                    }
                }
            }

            #endregion

            #region Trace files
            DeleteFilesInFolder(FXConsts.FOLDER_LOG.GetFolder(), FXConsts.TRACE, beforeDate);
            #endregion

            #region Program log
            DeleteFilesInFolder(FXConsts.FOLDER_LOG.GetFolder(), "program", beforeDate);
            #endregion

            #region Socket log
            DeleteFilesInFolder(FXConsts.FOLDER_LOG.GetFolder(), "socket", beforeDate);
            #endregion

            #region SIGN files
            DeleteFilesInFolder(FXConsts.FOLDER_DATA.GetFolder(), "sign", beforeDate, "*.SIGN_");
            #endregion

            #region JRN files
            DeleteFilesInFolder(FXConsts.JOURNAL.GetFolder(), string.Empty, beforeDate, "*.jrn");
            #endregion

            #region KEYBOARD file
            DeleteFilesInFolder(FXConsts.FOLDER_LOG.GetFolder(), "keyboard", beforeDate);
            #endregion

            #region OCX Logs
            // ksdg_log
            DeleteFilesInFolder(FXConsts.FOLDER_BIN.GetFolder(), "ksdg_log", beforeDate, "*.log");
            #endregion

            #region 여전법 추가 0723 reader files
            DeleteFilesInFolder(FXConsts.FOLDER_LOG.GetFolder(), "reader", beforeDate);
            #endregion

            #region Backup trans
            //2015.09.01 정광호 수정---------------------------------------------------------------------------------
            //FXConsts.FOLDER_DATA_BACK에서 "back"로 변경
            DeleteFilesInFolder(FXConsts.FOLDER_DATA.GetFolder(), "back", beforeDate, "*.db");
            // delete old files
            //DeleteFilesInFolder(FXConsts.FOLDER_DATA.GetFolder(), FXConsts.FOLDER_DATA_BACK, beforeDate, "*.db");
            //--------------------------------------------------------------------------------------------------------
            #endregion

            #region ksdg_log, log in update folder
            DeleteFilesInFolder(FXConsts.FOLDER_UPDATE.GetFolder(), "ksdg_log", beforeDate, "*.log");
            DeleteFilesInFolder(FXConsts.FOLDER_UPDATE.GetFolder(), FXConsts.FOLDER_LOG, beforeDate, "*.log");
            #endregion
        }

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

        #region 지난 공지사항 삭제

        /// <summary>
        /// (6) 지난 공지 관련 Table bsm130t, bsm131t delete (시스템일자 > 공지시작일) 
        /// </summary>
        void DeleteLastNotices()
        {
            var db = MasterDbHelper.InitInstance();
            var trans = db.BeginTransaction();
            try
            {
                string query = Extensions.LoadSqlCommand("POS_SD", "P001DeletePastNotices_BSM130T");
                db.ExecuteNonQuery(query, new string[]{
                    "@DD_TODAY"
                }, new object[] {
                    string.Format("{0:yyyyMMdd}", DateTime.Today)
                }, trans);

                query = Extensions.LoadSqlCommand("POS_SD", "P001DeletePastNotices_BSM131T");
                db.ExecuteNonQuery(query, new string[]{
                    "@DD_TODAY"
                }, new object[] {
                    string.Format("{0:yyyyMMdd}", DateTime.Today)
                }, trans);

                trans.Commit();
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
                db.EndInstance();
            }
        }

        #endregion

        #region 개설작업 - 과거 TRANS 파일삭제 - AppConfig.ini 자료보관일수에 맞춰 삭제

        /// <summary>
        /// 과거 TRANS 파일삭제 - AppConfig.ini 자료보관일수에 맞춰 삭제
        /// </summary>
        void DeleteTrans()
        {
            int days = TypeHelper.ToInt32(ConfigData.Current.AppConfig.PosOption.DataKeepDays);
            if (days <= 0)
            {
                return;
            }

            DateTime beforeDate = DateTime.Today.AddDays(days * -1);
            string saleDate = string.Format("{0:yyyyMMdd}", beforeDate);

            string[] tables = new string[] {
                "SAT010T", "SAT011T", "SAT300T", "SAT301T", "SAT302T", "SAT303T", "SAT304T", "SAT900T"
            };

            var db = TranDbHelper.InitInstance();
            var trans = db.BeginTransaction();
            try
            {
                string sqlQuery = string.Empty;
                foreach (var table in tables)
                {
                    sqlQuery = Extensions.LoadSqlCommand("POS_SD", string.Format("P001DeletePastTrans_{0}", table));
                    db.ExecuteNonQuery(sqlQuery,
                        new string[] {
                            "@CD_STORE",
                            "@NO_POS",
                            "@DD_SALE"
                        },
                        new object[] {
                            ConfigData.Current.AppConfig.PosInfo.StoreNo,
                            ConfigData.Current.AppConfig.PosInfo.PosNo,
                            saleDate
                        }, trans);
                }

                trans.Commit();
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
        }

        #endregion

        #region 개설작업 - AppConfig.ini 업데이트한다

        /// <summary>
        ///       - "AppConfig.ini" 파일의 영업일자 항목("SaleDate")를 실 영업일자로 업데이트 한다.
        ///       - "AppConfig.ini" 파일의 마감구분 항목("EOD Flag")를 "N" 업데이트 한다.
        ///       - "AppConfig.ini" 파일의 거래번호 항목("TrxnNo")를 "000001" 업데이트 한다.
        ///       - "AppConfig.ini" 파일의 정산차수 항목("ShiftCount")를 "01" 업데이트 한다.
        /// </summary>
        void UpdateAppConfig(int trxnNo)
        {
            // 디비에서 마직막번호 확인 
            var db = TranDbHelper.InitInstance();
            try
            {
                var sql = Extensions.LoadSqlCommand("POS_SD", "GetLastTrxnNo");
                var tn = db.ExecuteScalar(sql,
                    new string[] {
                        "@DD_SALE", "@CD_STORE", "@NO_POS"
                    }, new object[] {
                        m_saleDate, ConfigData.Current.AppConfig.PosInfo.StoreNo,
                        ConfigData.Current.AppConfig.PosInfo.PosNo
                    });

                if (tn != null)
                {
                    int ntn = TypeHelper.ToInt32(tn.ToString());
                    trxnNo = Math.Max(trxnNo, ntn) + 1;
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

            ConfigData.Current.AppConfig.PosInfo.SaleDate = m_saleDate;
            ConfigData.Current.AppConfig.PosInfo.EodFlag = "N";
            ConfigData.Current.AppConfig.PosInfo.TrxnNo = string.Format("{0:d6}", trxnNo);
            ConfigData.Current.AppConfig.PosInfo.ShiftCount = "01";
            ConfigData.Current.AppConfig.PosInfo.SlipNumber = "0001";
            ConfigData.Current.AppConfig.PosInfo.FrameNumber = "0001";
            ConfigData.Current.AppConfig.Save();
        }

        #endregion

        #region 개설작업 - TRANS정보저장

        BasketHeader basketHeader = null;

        void SdTran()
        {
            #region - 해당 Tran 정보를 점서버로 전송한다

            basketHeader = new BasketHeader()
            {
                TrxnType = NetCommConstants.TRXN_TYPE_POS_OPEN,
                CancType = "0"
            };

            TranDbHelper db = null;
            SQLiteTransaction trans = null;

            db = TranDbHelper.InitInstance();
            trans = db.BeginTransaction();
            try
            {
                TransManager.SaveTrans(basketHeader, null, db, trans);
                trans.Commit();

                // 거래완료
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

            #endregion
        }

        #endregion

        #region 개설영수증출력

        private void SdPrintReceipt()
        {
            if (m_view.ChkPrint())
            {
                // 영수증시작
                POSPrinterUtils.Instance.PrintReceiptSdOpen(basketHeader, m_printList.ToString());
                m_printList = null;
            }
        }

        #endregion

        #region 개설작업 - 마지막거래번호

        void PQ09()
        {
            var pq09 = new PQ09DataTask(m_saleDate);
            pq09.TaskCompleted += new TaskCompletedHandler(pq09_TaskCompleted);
            pq09.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pq09_Errored);
            pq09.ExecuteTask();
        }

        void pq09_Errored(string errorMessage, Exception lastException)
        {
            LastTask(0);
        }

        void pq09_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                var resp = responseData.DataRecords.ToDataRecords<PQ09RespData>()[0];
                int sTrxnNo = TypeHelper.ToInt32(resp.FinalTrxnNo);
                LastTask(sTrxnNo);
            }
        }

        #endregion
    }
}
