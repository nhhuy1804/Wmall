//-----------------------------------------------------------------
/*
 * 화면명   : POS_ED_P005.cs
 * 화면설명 : 마스터 수신
 * 개발자   : 정광호
 * 개발일자 : 2015.04.20
*/
//-----------------------------------------------------------------

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.POS.ED.PI;
using WSWD.WmallPos.POS.ED.PT;
using WSWD.WmallPos.POS.ED.VI;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.POS.FX.Win.Data;
using WSWD.WmallPos.FX.Shared.Data;
using WSWD.WmallPos.FX.NetComm.Tasks.PU;
using WSWD.WmallPos.FX.Shared.NetComm.Response;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PU;
using WSWD.WmallPos.FX.NetComm.Tasks.PD;
using WSWD.WmallPos.FX.NetComm.Tasks;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PD;

namespace WSWD.WmallPos.POS.ED.VC
{
    /// <summary>
    /// 마감관리 - 마스터 수신
    /// </summary>
    public partial class POS_ED_P005 : FormBase, IEDP005View
    {
        #region 변수

        //마스터 수신 비즈니스 로직
        private IEDP005presenter m_Presenter;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        /// <summary>
        /// 전문통신시 상황팝업
        /// </summary>
        private bool bShowPop = false;

        #endregion

        #region 생성자

        public POS_ED_P005()
        {
            InitializeComponent();

            //Form Load Event
            Load += new EventHandler(form_Load); 
        }

        #endregion

        #region 이벤트 등록

        /// <summary>
        /// Form Event 정의
        /// </summary>
        private void InitEvent()
        {
            
            this.KeyEvent += new OPOSKeyEventHandler(POS_ED_P002_KeyEvent);                                 //Key Event
            this.btnChk.Click += new EventHandler(btnChk_Click);                                            //전체마스터선택 Click Event
            this.btnTrans.Click += new EventHandler(btnTrans_Click);                                        //수신시작 Click Event
            this.btnClose.Click += new EventHandler(btnClose_Click);                                        //닫기 button Event

            this.btnPD01.Click += new EventHandler(btnPD_Click);                                            //점마스터
            this.btnPD02.Click += new EventHandler(btnPD_Click);                                            //계산원
            this.btnPD03.Click += new EventHandler(btnPD_Click);                                            //단품
            this.btnPD04.Click += new EventHandler(btnPD_Click);                                            //품번
            this.btnPD05.Click += new EventHandler(btnPD_Click);                                            //품목
            this.btnPD06.Click += new EventHandler(btnPD_Click);                                            //터치상품그룹
            this.btnPD07.Click += new EventHandler(btnPD_Click);                                            //터치상풉
            this.btnPD08.Click += new EventHandler(btnPD_Click);                                            //Preset
            this.btnPD09.Click += new EventHandler(btnPD_Click);                                            //영수증명판
            this.btnPD10.Click += new EventHandler(btnPD_Click);                                            //브랜드광고
            this.btnPD11.Click += new EventHandler(btnPD_Click);                                            //카드회사정보
            this.btnPD12.Click += new EventHandler(btnPD_Click);                                            //공지사항
            this.btnPD13.Click += new EventHandler(btnPD_Click);                                            //통합코드
            this.btnPD14.Click += new EventHandler(btnPD_Click);                                            //메세지정보
            this.btnPD15.Click += new EventHandler(btnPD_Click);                                            //타사상품권
            this.btnPD16.Click += new EventHandler(btnPD_Click);                                            //타사상품권권종
            this.btnPD17.Click += new EventHandler(btnPD_Click);                                            //할인쿠폰
            this.btnPD18.Click += new EventHandler(btnPD_Click);                                            //프로모션
            this.btnPD19.Click += new EventHandler(btnPD_Click);                                            //프로모션허들
            this.btnPD20.Click += new EventHandler(btnPD_Click);                                            //프로모션브랜드
            this.btnPD21.Click += new EventHandler(btnPD_Click);                                            //프로모션증정권
            this.btnPD22.Click += new EventHandler(btnPD_Click);                                            //프로모션경품
            this.btnPD23.Click += new EventHandler(btnPD_Click);                                            //프로모션영수증
        }

        #endregion

        #region 이벤트 정의

        /// <summary>
        /// Form Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void form_Load(object sender, EventArgs e)
        {
            //이벤트 등록
            InitEvent();

            this.IsModal = true;

            //마스터 수신 비즈니스 로직
            m_Presenter = new EDP005presenter(this);

            this.btnChk.Selected = false;
            this.btnTrans.Enabled = false;
        }

        /// <summary>
        /// Key Event
        /// </summary>
        /// <param name="e"></param>
        void POS_ED_P002_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (_bDisable)
            {
                e.IsHandled = true;
                return;
            }

            if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_ENTER)
            {
                //수신시작

            }
            else if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_CLEAR)
            {
                //닫기
            }
        }

        /// <summary>
        /// 전체마스터선택 Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnChk_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;

            btnChk.Selected = !btnChk.Selected;

            this.btnPD01.Selected = btnChk.Selected ? true : false;      //점마스터
            this.btnPD02.Selected = btnChk.Selected ? true : false;      //계산원
            this.btnPD03.Selected = btnChk.Selected ? true : false;      //단품
            this.btnPD04.Selected = btnChk.Selected ? true : false;      //품번
            this.btnPD05.Selected = btnChk.Selected ? true : false;      //품목
            this.btnPD06.Selected = btnChk.Selected ? true : false;      //터치상품그룹
            this.btnPD07.Selected = btnChk.Selected ? true : false;      //터치상풉
            this.btnPD08.Selected = btnChk.Selected ? true : false;      //Preset
            this.btnPD09.Selected = btnChk.Selected ? true : false;      //영수증명판
            this.btnPD10.Selected = btnChk.Selected ? true : false;      //브랜드광고
            this.btnPD11.Selected = btnChk.Selected ? true : false;      //카드회사정보
            this.btnPD12.Selected = btnChk.Selected ? true : false;      //공지사항
            this.btnPD13.Selected = btnChk.Selected ? true : false;      //통합코드
            this.btnPD14.Selected = btnChk.Selected ? true : false;      //메세지정보
            this.btnPD15.Selected = btnChk.Selected ? true : false;      //타사상품권
            this.btnPD16.Selected = btnChk.Selected ? true : false;      //타사상품권권종
            this.btnPD17.Selected = btnChk.Selected ? true : false;      //할인쿠폰
            this.btnPD18.Selected = btnChk.Selected ? true : false;      //프로모션
            this.btnPD19.Selected = btnChk.Selected ? true : false;      //프로모션허들
            this.btnPD20.Selected = btnChk.Selected ? true : false;      //프로모션브랜드
            this.btnPD21.Selected = btnChk.Selected ? true : false;      //프로모션증정권
            this.btnPD22.Selected = btnChk.Selected ? true : false;      //프로모션경품
            this.btnPD23.Selected = btnChk.Selected ? true : false;      //프로모션영수증

            btnChk.Text = btnChk.Selected ? strMsg05 : strMsg06;
            btnTrans.Enabled = btnChk.Selected ? true : false;
        }

        /// <summary>
        /// 수신시작 Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnTrans_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;

            if (!btnPD01.Selected && !btnPD02.Selected && !btnPD03.Selected && !btnPD04.Selected && !btnPD05.Selected &&
                !btnPD06.Selected && !btnPD07.Selected && !btnPD08.Selected && !btnPD09.Selected && !btnPD10.Selected &&
                !btnPD11.Selected && !btnPD12.Selected && !btnPD13.Selected && !btnPD14.Selected && !btnPD15.Selected &&
                !btnPD16.Selected && !btnPD17.Selected && !btnPD18.Selected && !btnPD19.Selected && !btnPD20.Selected &&
                !btnPD21.Selected && !btnPD22.Selected && !btnPD23.Selected && m_Presenter == null) return;

            SetControlDisable(true);

            //수신전 초기화
            this.pnlMsg.ContainerZone.Controls.Clear();
            this.pnlMsg.ScrollPosition = 0;

            //마스터 수신정보 실행
            ChkNextTran(null);
        }

        /// <summary>
        /// 닫기 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnClose_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;

            this.Close();
        }

        /// <summary>
        /// 각 전문 버튼 Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnPD_Click(object sender, EventArgs e)
        {
            WSWD.WmallPos.POS.FX.Win.UserControls.Button btn = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)sender;

            btn.Selected = !btn.Selected;

            if (!btnPD01.Selected && !btnPD02.Selected && !btnPD03.Selected && !btnPD04.Selected && !btnPD05.Selected &&
                !btnPD06.Selected && !btnPD07.Selected && !btnPD08.Selected && !btnPD09.Selected && !btnPD10.Selected &&
                !btnPD11.Selected && !btnPD12.Selected && !btnPD13.Selected && !btnPD14.Selected && !btnPD15.Selected &&
                !btnPD16.Selected && !btnPD17.Selected && !btnPD18.Selected && !btnPD19.Selected && !btnPD20.Selected &&
                !btnPD21.Selected && !btnPD22.Selected && !btnPD23.Selected)
            {
                btnTrans.Enabled = false;
            }
            else
            {
                btnTrans.Enabled = true;
            }
        }

        #endregion

        #region 사용자 정의

        #region 전문

        #region PD01 - 점마스터

        /// <summary>
        /// PD01 - 점마스터
        /// </summary>
        void PD01()
        {
            if (!bShowPop)
            {
                ChildManager.ShowProgress(true);
                bShowPop = true;
            }

            var pd01Task = new PD01DataTask();
            pd01Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd01Task_Errored);
            pd01Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd01Task_DownloadProgressChanged);
            pd01Task.ExecuteTask();
        }

        void pd01Task_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD01, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD01, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD01, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD01);
            }
        }

        void pd01Task_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD01, OpenItemStatus.Error);
        }

        #endregion

        #region PD02 - 계산원

        /// <summary>
        /// PD02 - 계산원
        /// </summary>
        void PD02()
        {
            if (!bShowPop)
            {
                ChildManager.ShowProgress(true);
                bShowPop = true;
            }

            var pd02 = new PD02DataTask();
            pd02.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd02_Errored);
            pd02.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd02_DownloadProgressChanged);
            pd02.ExecuteTask();
        }

        void pd02_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD02, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD02, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD02, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD02);
            }
        }

        void pd02_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD02, OpenItemStatus.Error);
        }


        #endregion

        #region PD03 - 단품

        /// <summary>
        /// PD03 - 단품
        /// </summary>
        void PD03()
        {
            if (!bShowPop)
            {
                ChildManager.ShowProgress(true);
                bShowPop = true;
            }

            var pd03 = new PD03DataTask();
            pd03.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd03_Errored);
            pd03.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd03_DownloadProgressChanged);
            pd03.ExecuteTask();
        }

        void pd03_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD03, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD03, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD03, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD03);
            }
        }

        void pd03_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD03, OpenItemStatus.Error);
        }


        #endregion

        #region PD04 - 품번

        /// <summary>
        /// PD04 - 품번
        /// </summary>
        void PD04()
        {
            if (!bShowPop)
            {
                ChildManager.ShowProgress(true);
                bShowPop = true;
            }

            var pd04 = new PD04DataTask();
            pd04.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd04_Errored);
            pd04.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd04_DownloadProgressChanged);
            pd04.ExecuteTask();
        }

        void pd04_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD04, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD04, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD04, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD04);
            }
        }


        void pd04_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD04, OpenItemStatus.Error);
        }

        #endregion

        #region PD05 - 품목

        /// <summary>
        /// PD05 - 품목
        /// </summary>
        void PD05()
        {
            if (!bShowPop)
            {
                ChildManager.ShowProgress(true);
                bShowPop = true;
            }

            var pd05 = new PD05DataTask();
            pd05.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd05_Errored);
            pd05.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd05_DownloadProgressChanged);
            pd05.ExecuteTask();
        }

        void pd05_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD05, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD05, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD05, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD05);
            }
        }

        void pd05_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD05, OpenItemStatus.Error);
        }

        #endregion

        #region PD06 - 터치상품그룹

        /// <summary>
        /// PD06 - 터치상품그룹
        /// </summary>
        void PD06()
        {
            if (!bShowPop)
            {
                ChildManager.ShowProgress(true);
                bShowPop = true;
            }

            var pd06 = new PD06DataTask();
            pd06.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd06_Errored);
            pd06.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd06_DownloadProgressChanged);
            pd06.ExecuteTask();
        }

        void pd06_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD06, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD06, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD06, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD06);
            }
        }

        void pd06_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD06, OpenItemStatus.Error);
        }

        #endregion

        #region PD07 - 터치상품

        /// <summary>
        /// PD07 - 터치상품
        /// </summary>
        void PD07()
        {
            if (!bShowPop)
            {
                ChildManager.ShowProgress(true);
                bShowPop = true;
            }

            var pd07 = new PD07DataTask();
            pd07.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd07_Errored);
            pd07.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd07_DownloadProgressChanged);
            pd07.ExecuteTask();
        }

        void pd07_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD07, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD07, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD07, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD07);
            }
        }

        void pd07_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD07, OpenItemStatus.Error);
        }

        #endregion

        #region PD08 - Preset

        /// <summary>
        /// PD08 - Preset
        /// </summary>
        void PD08()
        {
            if (!bShowPop)
            {
                ChildManager.ShowProgress(true);
                bShowPop = true;
            }

            var pd08 = new PD08DataTask();
            pd08.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd08_Errored);
            pd08.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd08_DownloadProgressChanged);
            pd08.ExecuteTask();
        }

        void pd08_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD08, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD08, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD08, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD08);
            }
        }

        void pd08_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD08, OpenItemStatus.Error);
        }

        #endregion

        #region PD09 - 영수증명판

        /// <summary>
        /// PD09 - 영수증명판
        /// </summary>
        void PD09()
        {
            if (!bShowPop)
            {
                ChildManager.ShowProgress(true);
                bShowPop = true;
            }

            var pd09 = new PD09DataTask();
            pd09.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd09Task_Errored);
            pd09.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd09_DownloadProgressChanged);
            pd09.ExecuteTask();
        }

        void pd09_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD09, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD09, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD09, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD09);
            }
        }

        void pd09Task_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD09, OpenItemStatus.Error);
        }

        #endregion

        #region PD10 - 브랜드광고

        /// <summary>
        /// PD10 - 브랜드광고
        /// </summary>
        void PD10()
        {
            var pd10 = new PD10DataTask();
            pd10.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd10Task_Errored);
            pd10.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd10_DownloadProgressChanged);
            pd10.ExecuteTask();
        }

        void pd10_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD10, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD10, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD10, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD10);
            }
        }

        void pd10Task_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD10, OpenItemStatus.Error);
        }

        #endregion

        #region PD11 - 카드회사정보

        /// <summary>
        /// PD11 - 카드회사정보
        /// </summary>
        void PD11()
        {
            var pd11Task = new PD11DataTask();
            pd11Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd11Task_Errored);
            pd11Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd11Task_DownloadProgressChanged);
            pd11Task.ExecuteTask();
        }

        void pd11Task_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD11, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD11, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD11, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD11);
            }
        }

        void pd11Task_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD11, OpenItemStatus.Error);
        }

        #endregion

        #region PD12 - 공지사항

        /// <summary>
        /// PD12 - 공지사항
        /// </summary>
        void PD12()
        {
            var pd12 = new PD12DataTask();
            pd12.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd12_Errored);
            pd12.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd12_DownloadProgressChanged);
            pd12.ExecuteTask();
        }

        void pd12_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD12, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD12, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD12, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD12);
            }
        }


        void pd12_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD12, OpenItemStatus.Error);
        }

        #endregion

        #region PD13 - 통합코드

        /// <summary>
        /// PD13 - 통합코드
        /// </summary>
        void PD13()
        {
            var pd13Task = new PD13DataTask();
            pd13Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd13Task_Errored);
            pd13Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd13Task_DownloadProgressChanged);
            pd13Task.ExecuteTask();
        }

        void pd13Task_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD13, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD13, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD13, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD13);
            }
        }

        void pd13Task_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD13, OpenItemStatus.Error);
        }

        #endregion

        #region PD14 - 메시지정보

        /// <summary>
        /// PD14 - 메시지정보
        /// </summary>
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

        void pd14Task_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD14, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD14, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD14, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //메세지를 수신했다면 메세지 내용을 다시 로드한다
                SysMessage.Load();

                //다음 마스터 수신정보
                ChkNextTran(btnPD14);
            }
        }

        void pd14Task_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD14, OpenItemStatus.Error);
        }

        #endregion

        #region PD15 - 타사상품권

        /// <summary>
        /// PD15 - 타사상품권
        /// </summary>
        void PD15()
        {
            var pd15Task = new PD15DataTask();
            pd15Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd15Task_Errored);
            pd15Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd15Task_DownloadProgressChanged);
            pd15Task.ExecuteTask();
        }

        void pd15Task_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD15, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD15, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD15, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD15);
            }
        }

        void pd15Task_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD15, OpenItemStatus.Error);
        }

        #endregion

        #region PD16 - 타사상품권권종

        /// <summary>
        /// PD16 - 타사상품권권종
        /// </summary>
        void PD16()
        {
            var pd16Task = new PD16DataTask();
            pd16Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd16Task_Errored);
            pd16Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd16Task_DownloadProgressChanged);
            pd16Task.ExecuteTask();
        }

        void pd16Task_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD16, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD16, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD16, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD16);
            }
        }

        void pd16Task_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD16, OpenItemStatus.Error);
        }

        #endregion

        #region PD17 - 할인쿠폰

        /// <summary>
        /// PD17 - 할인쿠폰
        /// </summary>
        void PD17()
        {
            var pd17Task = new PD17DataTask();
            pd17Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd17Task_Errored);
            pd17Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd17Task_DownloadProgressChanged);
            pd17Task.ExecuteTask();
        }

        void pd17Task_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD17, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD17, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD17, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD17);
            }
        }

        void pd17Task_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD17, OpenItemStatus.Error);
        }

        #endregion

        #region PD18 - 프로모션

        /// <summary>
        /// PD18 - 프로모션
        /// </summary>
        void PD18()
        {
            var pd18Task = new PD18DataTask();
            pd18Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd18Task_Errored);
            pd18Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd18Task_DownloadProgressChanged);
            pd18Task.ExecuteTask();
        }

        void pd18Task_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD18, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD18, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD18, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD18);
            }
        }

        void pd18Task_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD18, OpenItemStatus.Error);
        }

        #endregion

        #region PD19 - 프로모션허들

        /// <summary>
        /// PD19 - 프로모션허들
        /// </summary>
        void PD19()
        {
            var pd19Task = new PD19DataTask();
            pd19Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd19Task_Errored);
            pd19Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd19Task_DownloadProgressChanged);
            pd19Task.ExecuteTask();
        }

        void pd19Task_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD19, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD19, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD19, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD19);
            }
        }

        void pd19Task_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD19, OpenItemStatus.Error);
        }

        #endregion

        #region PD20 - 프로모션브랜드

        /// <summary>
        /// PD20 - 프로모션브랜드
        /// </summary>
        void PD20()
        {
            var pd20Task = new PD20DataTask();
            pd20Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd20Task_Errored);
            pd20Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd20Task_DownloadProgressChanged);
            pd20Task.ExecuteTask();
        }

        void pd20Task_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD20, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD20, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD20, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD20);
            }
        }

        void pd20Task_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD20, OpenItemStatus.Error);
        }

        #endregion

        #region PD21 - 프로모션증정권

        /// <summary>
        /// PD21 - 프로모션증정권
        /// </summary>
        void PD21()
        {
            var pd21Task = new PD21DataTask();
            pd21Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd21Task_Errored);
            pd21Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd21Task_DownloadProgressChanged);
            pd21Task.ExecuteTask();
        }

        void pd21Task_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD21, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD21, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD21, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD21);
            }
        }

        void pd21Task_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD21, OpenItemStatus.Error);
        }

        #endregion

        #region PD22 - 프로모션경품

        /// <summary>
        /// PD22 - 프로모션경품
        /// </summary>
        void PD22()
        {
            var pd22Task = new PD22DataTask();
            pd22Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd22Task_Errored);
            pd22Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd22Task_DownloadProgressChanged);
            pd22Task.ExecuteTask();
        }

        void pd22Task_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD22, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD22, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD22, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD22);
            }
        }

        void pd22Task_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD22, OpenItemStatus.Error);
        }

        #endregion

        #region PD23 - 프로모션영수증

        /// <summary>
        /// PD23 - 프로모션영수증
        /// </summary>
        void PD23()
        {
            var pd23Task = new PD23DataTask();
            pd23Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd23Task_Errored);
            pd23Task.DownloadProgressChanged += new DownloadProgressChangedEventHandler(pd23Task_DownloadProgressChanged);
            pd23Task.ExecuteTask();
        }

        void pd23Task_DownloadProgressChanged(DownloadProgressState processState, int iUpdateCnt, int iUpdateTotalCnt)
        {
            if (processState == DownloadProgressState.Started)
            {
                AddOpenItemStatus(btnPD23, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else if (processState == DownloadProgressState.Processing)
            {
                UpdateOpenItemStatus(btnPD23, OpenItemStatus.None, iUpdateCnt, iUpdateTotalCnt);
            }
            else
            {
                UpdateOpenItemStatus(btnPD23, OpenItemStatus.OK, iUpdateCnt, iUpdateTotalCnt);

                //다음 마스터 수신정보
                ChkNextTran(btnPD23);
            }
        }

        void pd23Task_Errored(string errorMessage, Exception lastException)
        {
            UpdateErrorItemStatus(btnPD23, OpenItemStatus.Error);
        }

        #endregion

        #endregion

        /// <summary>
        /// 다음 마스터 수신정보
        /// </summary>
        /// <param name="btn">현재 종료된 마스터 수신정보</param>
        private void ChkNextTran(WSWD.WmallPos.POS.FX.Win.UserControls.Button btn)
        {
            if (!bShowPop)
            {
                ChildManager.ShowProgress(true);
                bShowPop = true;
            }

            if (btn == null)
            {
                if (btnPD01.Selected)
                {
                    //점마스터
                    PD01();
                }
                else
                {
                    ChkNextTran(btnPD01);
                }
            }
            else if (btn.Name.ToString() == btnPD01.Name.ToString())
            {
                if (btnPD02.Selected)
                {
                    //계산원
                    PD02();
                }
                else
                {
                    ChkNextTran(btnPD02);
                }
            }
            else if (btn.Name.ToString() == btnPD02.Name.ToString())
            {
                if (btnPD03.Selected)
                {
                    //단품
                    PD03();
                }
                else
                {
                    ChkNextTran(btnPD03);
                }
            }
            else if (btn.Name.ToString() == btnPD03.Name.ToString())
            {
                if (btnPD04.Selected)
                {
                    //품번
                    PD04();
                }
                else
                {
                    ChkNextTran(btnPD04);
                }
            }
            else if (btn.Name.ToString() == btnPD04.Name.ToString())
            {
                if (btnPD05.Selected)
                {
                    //품목
                    PD05();
                }
                else
                {
                    ChkNextTran(btnPD05);
                }
            }
            else if (btn.Name.ToString() == btnPD05.Name.ToString())
            {
                if (btnPD06.Selected)
                {
                    //화면터치부문마스터
                    PD06();
                }
                else
                {
                    ChkNextTran(btnPD06);
                }
            }
            else if (btn.Name.ToString() == btnPD06.Name.ToString())
            {
                if (btnPD07.Selected)
                {
                    //화면터치상품마스터
                    PD07();
                }
                else
                {
                    ChkNextTran(btnPD07);
                }
            }
            else if (btn.Name.ToString() == btnPD07.Name.ToString())
            {
                if (btnPD08.Selected)
                {
                    //Preset
                    PD08();
                }
                else
                {
                    ChkNextTran(btnPD08);
                }
            }
            else if (btn.Name.ToString() == btnPD08.Name.ToString())
            {
                if (btnPD09.Selected)
                {
                    //영수증명판
                    PD09();
                }
                else
                {
                    ChkNextTran(btnPD09);
                }
            }
            else if (btn.Name.ToString() == btnPD09.Name.ToString())
            {
                if (btnPD10.Selected)
                {
                    //영수증브랜드광고마스터
                    PD10();
                }
                else
                {
                    ChkNextTran(btnPD10);
                }
            }
            else if (btn.Name.ToString() == btnPD10.Name.ToString())
            {
                if (btnPD11.Selected)
                {
                    //카드회사마스터
                    PD11();
                }
                else
                {
                    ChkNextTran(btnPD11);
                }
            }
            else if (btn.Name.ToString() == btnPD11.Name.ToString())
            {
                if (btnPD12.Selected)
                {
                    //공지사항
                    PD12();
                }
                else
                {
                    ChkNextTran(btnPD12);
                }
            }
            else if (btn.Name.ToString() == btnPD12.Name.ToString())
            {
                if (btnPD13.Selected)
                {
                    //통합코드
                    PD13();
                }
                else
                {
                    ChkNextTran(btnPD13);
                }
            }
            else if (btn.Name.ToString() == btnPD13.Name.ToString())
            {
                if (btnPD14.Selected)
                {
                    //메시지정보마스터
                    PD14();
                }
                else
                {
                    ChkNextTran(btnPD14);
                }
            }

            else if (btn.Name.ToString() == btnPD14.Name.ToString())
            {
                if (btnPD15.Selected)
                {
                    //타사상품권
                    PD15();
                }
                else
                {
                    ChkNextTran(btnPD15);
                }
            }
            else if (btn.Name.ToString() == btnPD15.Name.ToString())
            {
                if (btnPD16.Selected)
                {
                    //타사상품권권종
                    PD16();
                }
                else
                {
                    ChkNextTran(btnPD16);
                }
            }
            else if (btn.Name.ToString() == btnPD16.Name.ToString())
            {
                if (btnPD17.Selected)
                {
                    //할인쿠폰
                    PD17();
                }
                else
                {
                    ChkNextTran(btnPD17);
                }
            }
            else if (btn.Name.ToString() == btnPD17.Name.ToString())
            {
                if (btnPD18.Selected)
                {
                    //프로모션
                    PD18();
                }
                else
                {
                    ChkNextTran(btnPD18);
                }
            }
            else if (btn.Name.ToString() == btnPD18.Name.ToString())
            {
                if (btnPD19.Selected)
                {
                    //프로모션허들
                    PD19();
                }
                else
                {
                    ChkNextTran(btnPD19);
                }
            }
            else if (btn.Name.ToString() == btnPD19.Name.ToString())
            {
                if (btnPD20.Selected)
                {
                    //프로모션브랜드
                    PD20();
                }
                else
                {
                    ChkNextTran(btnPD20);
                }
            }
            else if (btn.Name.ToString() == btnPD20.Name.ToString())
            {
                if (btnPD21.Selected)
                {
                    //프로모션증정권
                    PD21();
                }
                else
                {
                    ChkNextTran(btnPD21);
                }
            }
            else if (btn.Name.ToString() == btnPD21.Name.ToString())
            {
                if (btnPD22.Selected)
                {
                    //프로모션경품
                    PD22();
                }
                else
                {
                    ChkNextTran(btnPD22);
                }
            }
            else if (btn.Name.ToString() == btnPD22.Name.ToString())
            {
                if (btnPD23.Selected)
                {
                    //프로모션영수증
                    PD23();
                }
                else
                {
                    ChkNextTran(btnPD23);
                }
            }
            else if (btn.Name.ToString() == btnPD23.Name.ToString())
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        if (bShowPop)
                        {
                            ChildManager.ShowProgress(false);
                            bShowPop = false;
                            SetControlDisable(false);
                        }
                    });
                }
                else
                {
                    if (bShowPop)
                    {
                        ChildManager.ShowProgress(false);
                        bShowPop = false;
                        SetControlDisable(false);
                    }
                }
            }

            
        }

        /// <summary>
        /// 각마스터정보 수신 시작 상태변경
        /// </summary>
        /// <param name="message"></param>
        /// <param name="status"></param>
        public void AddOpenItemStatus(WSWD.WmallPos.POS.FX.Win.UserControls.Button btn, WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus status, int iUpdateCnt, int iUpdateAllCnt)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    OpenStatusItem osi = new OpenStatusItem();
                    osi.Height = 31;
                    osi.ItemStatus = status;
                    osi.MessageText = string.Format("{0} {1}", btn.Text.ToString(), strMsg01);
                    this.pnlMsg.ContainerZone.Controls.Add(osi);
                    this.pnlMsg.ScrollDown();
                });
            }
            else
            {
                OpenStatusItem osi = new OpenStatusItem();
                osi.Height = 31;
                osi.ItemStatus = status;
                osi.MessageText = string.Format("{0} {1}", btn.Text.ToString(), strMsg01);
                this.pnlMsg.ContainerZone.Controls.Add(osi);
            }
        }

        /// <summary>
        /// 각마스터정보 수신 정보 상태변경
        /// </summary>
        /// <param name="message"></param>
        /// <param name="status"></param>
        public void UpdateOpenItemStatus(WSWD.WmallPos.POS.FX.Win.UserControls.Button btn, WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus status, int iUpdateCnt, int iUpdateAllCnt)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    if (this.pnlMsg.ContainerZone.Controls.Count == 0)
                    {
                        return;
                    }

                    OpenStatusItem osi = (OpenStatusItem)this.pnlMsg.ContainerZone.Controls[this.pnlMsg.ContainerZone.Controls.Count - 1];
                    osi.MessageText = status == OpenItemStatus.None ?
                        string.Format("{0} {1}", btn.Text.ToString(), strMsg02) :
                        string.Format("{0} {1}", btn.Text.ToString(), strMsg03);
                    osi.ItemStatus = status;

                    msgBarProgress.Text = string.Format("{0}/{1}", iUpdateCnt, iUpdateAllCnt);
                    if (iUpdateAllCnt > 0)
                    {
                        colorProgressBar1.Percentage = (int)(Convert.ToDouble(iUpdateCnt) / Convert.ToDouble(iUpdateAllCnt) * 100); 
                    }
                    else
                    {
                        colorProgressBar1.Percentage = 0;
                    }
                });
            }
            else
            {
                if (this.pnlMsg.ContainerZone.Controls.Count == 0)
                {
                    return;
                }

                OpenStatusItem osi = (OpenStatusItem)this.pnlMsg.ContainerZone.Controls[this.pnlMsg.ContainerZone.Controls.Count - 1];
                osi.MessageText = status == OpenItemStatus.None ?
                        string.Format("{0} {1}", strMsg02) :
                        string.Format("{0} {1}", btn.Text.ToString(), strMsg03);
                osi.ItemStatus = status;

                msgBarProgress.Text = string.Format("{0}/{1}", iUpdateCnt, iUpdateAllCnt);

                if (iUpdateAllCnt > 0)
                {
                    colorProgressBar1.Percentage = (int)(Convert.ToDouble(iUpdateCnt) / Convert.ToDouble(iUpdateAllCnt) * 100);
                }
                else
                {
                    colorProgressBar1.Percentage = 0;
                }
            }
        }

        /// <summary>
        /// 각마스터정보 수신 에러 상태변경
        /// </summary>
        /// <param name="message"></param>
        /// <param name="status"></param>
        public void UpdateErrorItemStatus(WSWD.WmallPos.POS.FX.Win.UserControls.Button btn, WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus status)
        {
            if (bShowPop)
            {
                ChildManager.ShowProgress(false);
                bShowPop = false;
            }

            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    if (this.pnlMsg.ContainerZone.Controls.Count == 0)
                    {
                        return;
                    }

                    OpenStatusItem osi = (OpenStatusItem)this.pnlMsg.ContainerZone.Controls[this.pnlMsg.ContainerZone.Controls.Count - 1];
                    osi.MessageText = string.Format("{0} {1}", btn.Text.ToString(), strMsg07);
                    osi.ItemStatus = status;

                    SetControlDisable(false);
                });
            }
            else
            {
                if (this.pnlMsg.ContainerZone.Controls.Count == 0)
                {
                    return;
                }

                OpenStatusItem osi = (OpenStatusItem)this.pnlMsg.ContainerZone.Controls[this.pnlMsg.ContainerZone.Controls.Count - 1];
                osi.MessageText = string.Format("{0} {1}", btn.Text.ToString(), strMsg07);
                osi.ItemStatus = status;

                SetControlDisable(false);
            }
        }

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable/Disable
        /// </summary>
        void SetControlDisable(bool bDisable)
        {
            _bDisable = bDisable;

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    foreach (var item in this.Controls)
                    {
                        if (item.GetType().Name.ToString().ToLower() == "keypad")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad key = (WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad)item;
                            key.Enabled = !_bDisable;
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "inputtext")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.InputText txt = (WSWD.WmallPos.POS.FX.Win.UserControls.InputText)item;
                            txt.Enabled = !_bDisable;
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "button")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.Button btn = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)item;
                            btn.Enabled = !_bDisable;

                            if (!_bDisable)
                            {
                                if (btn.Name.ToString() == btnTrans.Name.ToString())
                                {
                                    if (!btnPD01.Selected && !btnPD02.Selected && !btnPD03.Selected && !btnPD04.Selected && !btnPD05.Selected &&
                                        !btnPD06.Selected && !btnPD07.Selected && !btnPD08.Selected && !btnPD09.Selected && !btnPD10.Selected &&
                                        !btnPD11.Selected && !btnPD12.Selected && !btnPD13.Selected && !btnPD14.Selected && !btnPD15.Selected &&
                                        !btnPD16.Selected && !btnPD17.Selected && !btnPD18.Selected && !btnPD19.Selected && !btnPD20.Selected &&
                                        !btnPD21.Selected && !btnPD22.Selected && !btnPD23.Selected)
                                    {
                                        btn.Enabled = false;
                                    }
                                }
                            }
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "scrollablepanel")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.ScrollablePanel pnl = (WSWD.WmallPos.POS.FX.Win.UserControls.ScrollablePanel)item;
                            pnl.Enabled = !_bDisable;
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "panel")
                        {
                            Panel pnl = (Panel)item;
                            pnl.Enabled = !_bDisable;
                        }
                    }
                });
            }
            else
            {
                foreach (var item in this.Controls)
                {
                    if (item.GetType().Name.ToString().ToLower() == "keypad")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad key = (WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad)item;
                        key.Enabled = !_bDisable;
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "inputtext")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.InputText txt = (WSWD.WmallPos.POS.FX.Win.UserControls.InputText)item;
                        txt.Enabled = !_bDisable;
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "button")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.Button btn = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)item;
                        btn.Enabled = !_bDisable;

                        if (!_bDisable)
                        {
                            if (btn.Name.ToString() == btnTrans.Name.ToString())
                            {
                                if (!btnPD01.Selected && !btnPD02.Selected && !btnPD03.Selected && !btnPD04.Selected && !btnPD05.Selected &&
                                    !btnPD06.Selected && !btnPD07.Selected && !btnPD08.Selected && !btnPD09.Selected && !btnPD10.Selected &&
                                    !btnPD11.Selected && !btnPD12.Selected && !btnPD13.Selected && !btnPD14.Selected && !btnPD15.Selected &&
                                    !btnPD16.Selected && !btnPD17.Selected && !btnPD18.Selected && !btnPD19.Selected && !btnPD20.Selected &&
                                    !btnPD21.Selected && !btnPD22.Selected && !btnPD23.Selected)
                                {
                                    btn.Enabled = false;
                                }
                            }
                        }
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "scrollablepanel")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.ScrollablePanel pnl = (WSWD.WmallPos.POS.FX.Win.UserControls.ScrollablePanel)item;
                        pnl.Enabled = !_bDisable;
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "panel")
                    {
                        Panel pnl = (Panel)item;
                        pnl.Enabled = !_bDisable;
                    }
                }
            }

            //Application.DoEvents();
        }

        #endregion
    }
}
