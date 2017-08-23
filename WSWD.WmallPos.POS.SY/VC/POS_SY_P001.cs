//-----------------------------------------------------------------
/*
 * 화면명   : POS_SY_P001.cs
 * 화면설명 : 시스템설정_환경
 * 개발자   : 정광호
 * 개발일자 : 2015.06.15
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.POS.SY.PI;
using WSWD.WmallPos.POS.SY.PT;
using WSWD.WmallPos.POS.SY.VI;
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
using WSWD.WmallPos.POS.FX.Win;

namespace WSWD.WmallPos.POS.SY.VC
{
    public partial class POS_SY_P001 : FormBase
    {
        #region 변수

        /// <summary>
        /// 팝업 메세지 버튼
        /// </summary>
        string[] strBtnNm = new string[2];

        /// <summary>
        /// 현재 탭페이지 인덱스
        /// </summary>
        int iTabIndex = 0;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        /// <summary>
        /// AppConfig 변경 여부
        /// </summary>
        AppChange appChange;

        #endregion

        #region 생성자

        public POS_SY_P001()
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
            this.KeyEvent += new OPOSKeyEventHandler(form_KeyEvent);                                 //Key Event
            this.tc.Selecting += new TabControlCancelEventHandler(tc_Selecting);                            //탭변경 이벤트
            this.btnSave.Click += new EventHandler(btnSave_Click);                                          //저장 button Event
            this.btnClose.Click += new EventHandler(btnClose_Click);                                        //닫기 button Event

            #region 텍스트박스 포커스 이벤트

            txtStoreNo.InputFocused += new EventHandler(txt_InputFocused);
            txtStoreType.InputFocused += new EventHandler(txt_InputFocused);
            txtPosNo.InputFocused += new EventHandler(txt_InputFocused);
            txtSaleDate.InputFocused += new EventHandler(txt_InputFocused);
            txtTrxnNo.InputFocused += new EventHandler(txt_InputFocused);
            txtShiftCount.InputFocused += new EventHandler(txt_InputFocused);
            txtEodFlag.InputFocused += new EventHandler(txt_InputFocused);
            txtSodStatus.InputFocused += new EventHandler(txt_InputFocused);
            txtSlipNumber.InputFocused += new EventHandler(txt_InputFocused);
            txtFrameNumber.InputFocused += new EventHandler(txt_InputFocused);

            txtSvrIP1_01.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrIP1_02.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrIP1_03.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrIP1_04.InputFocused += new EventHandler(txt_InputFocused);
            txtComDPort1.InputFocused += new EventHandler(txt_InputFocused);
            txtComUPort1.InputFocused += new EventHandler(txt_InputFocused);
            txtComQPort1.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrIP2_01.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrIP2_02.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrIP2_03.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrIP2_04.InputFocused += new EventHandler(txt_InputFocused);
            txtComDPort2.InputFocused += new EventHandler(txt_InputFocused);
            txtComUPort2.InputFocused += new EventHandler(txt_InputFocused);
            txtComQPort2.InputFocused += new EventHandler(txt_InputFocused);
            txtComDTimeOut.InputFocused += new EventHandler(txt_InputFocused);
            txtComUTimeOut.InputFocused += new EventHandler(txt_InputFocused);
            txtComQTimeOut.InputFocused += new EventHandler(txt_InputFocused);

            txtSvrGftIP1_01.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrGftIP1_02.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrGftIP1_03.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrGftIP1_04.InputFocused += new EventHandler(txt_InputFocused);
            txtComGPort1.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrGftIP2_01.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrGftIP2_02.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrGftIP2_03.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrGftIP2_04.InputFocused += new EventHandler(txt_InputFocused);
            txtComGPort2.InputFocused += new EventHandler(txt_InputFocused);
            txtComGTimeOut.InputFocused += new EventHandler(txt_InputFocused);

            txtSvrPntIP1_01.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrPntIP1_02.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrPntIP1_03.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrPntIP1_04.InputFocused += new EventHandler(txt_InputFocused);
            txtComPPort1.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrPntIP2_01.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrPntIP2_02.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrPntIP2_03.InputFocused += new EventHandler(txt_InputFocused);
            txtSvrPntIP2_04.InputFocused += new EventHandler(txt_InputFocused);
            txtComPPort2.InputFocused += new EventHandler(txt_InputFocused);
            txtComPTimeOut.InputFocused += new EventHandler(txt_InputFocused);

            txtHqSvrIP1_01.InputFocused += new EventHandler(txt_InputFocused);
            txtHqSvrIP1_02.InputFocused += new EventHandler(txt_InputFocused);
            txtHqSvrIP1_03.InputFocused += new EventHandler(txt_InputFocused);
            txtHqSvrIP1_04.InputFocused += new EventHandler(txt_InputFocused);
            txtHqComQPort1.InputFocused += new EventHandler(txt_InputFocused);
            txtHqComUPort1.InputFocused += new EventHandler(txt_InputFocused);
            txtHqSvrIP2_01.InputFocused += new EventHandler(txt_InputFocused);
            txtHqSvrIP2_02.InputFocused += new EventHandler(txt_InputFocused);
            txtHqSvrIP2_03.InputFocused += new EventHandler(txt_InputFocused);
            txtHqSvrIP2_04.InputFocused += new EventHandler(txt_InputFocused);
            txtHqComQPort2.InputFocused += new EventHandler(txt_InputFocused);
            txtHqComUPort2.InputFocused += new EventHandler(txt_InputFocused);
            txtHqComQTimeOut.InputFocused += new EventHandler(txt_InputFocused);
            txtHqComUTimeOut.InputFocused += new EventHandler(txt_InputFocused);

            txtFtpSvrIP1_01.InputFocused += new EventHandler(txt_InputFocused);
            txtFtpSvrIP1_02.InputFocused += new EventHandler(txt_InputFocused);
            txtFtpSvrIP1_03.InputFocused += new EventHandler(txt_InputFocused);
            txtFtpSvrIP1_04.InputFocused += new EventHandler(txt_InputFocused);
            txtFtpComPort1.InputFocused += new EventHandler(txt_InputFocused);
            txtFtpSvrIP2_01.InputFocused += new EventHandler(txt_InputFocused);
            txtFtpSvrIP2_02.InputFocused += new EventHandler(txt_InputFocused);
            txtFtpSvrIP2_03.InputFocused += new EventHandler(txt_InputFocused);
            txtFtpSvrIP2_04.InputFocused += new EventHandler(txt_InputFocused);
            txtFtpComPort2.InputFocused += new EventHandler(txt_InputFocused);
            txtMode.InputFocused += new EventHandler(txt_InputFocused);
            txtUser.InputFocused += new EventHandler(txt_InputFocused);
            txtPass.InputFocused += new EventHandler(txt_InputFocused);
            txtJournalPath.InputFocused += new EventHandler(txt_InputFocused);
            txtDataFileDownloadPath.InputFocused += new EventHandler(txt_InputFocused);
            txtVersionInfoPath.InputFocused += new EventHandler(txt_InputFocused);
            txtCreateUploadPathByDate.InputFocused += new EventHandler(txt_InputFocused);

            txtVanSvrIP1_01.InputFocused += new EventHandler(txt_InputFocused);
            txtVanSvrIP1_02.InputFocused += new EventHandler(txt_InputFocused);
            txtVanSvrIP1_03.InputFocused += new EventHandler(txt_InputFocused);
            txtVanSvrIP1_04.InputFocused += new EventHandler(txt_InputFocused);
            txtVanComPort1.InputFocused += new EventHandler(txt_InputFocused);
            txtVanSvrIP2_01.InputFocused += new EventHandler(txt_InputFocused);
            txtVanSvrIP2_02.InputFocused += new EventHandler(txt_InputFocused);
            txtVanSvrIP2_03.InputFocused += new EventHandler(txt_InputFocused);
            txtVanSvrIP2_04.InputFocused += new EventHandler(txt_InputFocused);
            txtVanComPort2.InputFocused += new EventHandler(txt_InputFocused);
            txtComTimeOut.InputFocused += new EventHandler(txt_InputFocused);

            txtPointUse.InputFocused += new EventHandler(txt_InputFocused);
            txtPointSchemePrefix.InputFocused += new EventHandler(txt_InputFocused);
            txtPointPayKeyInputEnable.InputFocused += new EventHandler(txt_InputFocused);
            txtCashReceiptUse.InputFocused += new EventHandler(txt_InputFocused);
            txtCashReceiptIssue.InputFocused += new EventHandler(txt_InputFocused);
            txtCashReceiptApplAmount.InputFocused += new EventHandler(txt_InputFocused);
            txtGoodsCodePrefix1.InputFocused += new EventHandler(txt_InputFocused);
            txtGoodsCodePrefix2.InputFocused += new EventHandler(txt_InputFocused);
            txtDataKeepDays.InputFocused += new EventHandler(txt_InputFocused);
            txtSalesReturn.InputFocused += new EventHandler(txt_InputFocused);
            txtSignUploadTask.InputFocused += new EventHandler(txt_InputFocused);
            txtTransUploadTask.InputFocused += new EventHandler(txt_InputFocused);
            txtTransStatusTask.InputFocused += new EventHandler(txt_InputFocused);
            txtNoticeStatusTask.InputFocused += new EventHandler(txt_InputFocused);

            #endregion
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

            strBtnNm[0] = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00371");
            strBtnNm[1] = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00372");

            //컨트롤 초기화
            SetInitControl();
        }

        /// <summary>
        /// 컨트롤 초기화
        /// </summary>
        private void SetInitControl()
        {
            #region 텍스트박스 입력값 셋팅

            appChange = new AppChange();
            txtStoreNo.Text = ConfigData.Current.AppConfig.PosInfo.StoreNo;
            txtStoreType.Text = ConfigData.Current.AppConfig.PosInfo.StoreType;
            txtPosNo.Text = ConfigData.Current.AppConfig.PosInfo.PosNo;
            txtSaleDate.Text = ConfigData.Current.AppConfig.PosInfo.SaleDate;
            txtTrxnNo.Text = ConfigData.Current.AppConfig.PosInfo.TrxnNo;
            txtShiftCount.Text = ConfigData.Current.AppConfig.PosInfo.ShiftCount;
            txtEodFlag.Text = ConfigData.Current.AppConfig.PosInfo.EodFlag;
            txtSodStatus.Text = ConfigData.Current.AppConfig.PosInfo.SodStatus;
            txtSlipNumber.Text = ConfigData.Current.AppConfig.PosInfo.SlipNumber;
            txtFrameNumber.Text = ConfigData.Current.AppConfig.PosInfo.FrameNumber;

            SetTextIP(txtSvrIP1_01, txtSvrIP1_02, txtSvrIP1_03, txtSvrIP1_04, ConfigData.Current.AppConfig.PosComm.SvrIP1);
            txtComDPort1.Text = ConfigData.Current.AppConfig.PosComm.ComDPort1;
            txtComUPort1.Text = ConfigData.Current.AppConfig.PosComm.ComUPort1;
            txtComQPort1.Text = ConfigData.Current.AppConfig.PosComm.ComQPort1;
            SetTextIP(txtSvrIP2_01, txtSvrIP2_02, txtSvrIP2_03, txtSvrIP2_04, ConfigData.Current.AppConfig.PosComm.SvrIP2);
            txtComDPort2.Text = ConfigData.Current.AppConfig.PosComm.ComDPort2;
            txtComUPort2.Text = ConfigData.Current.AppConfig.PosComm.ComUPort2;
            txtComQPort2.Text = ConfigData.Current.AppConfig.PosComm.ComQPort2;
            txtComDTimeOut.Text = ConfigData.Current.AppConfig.PosComm.ComDTimeOut;
            txtComUTimeOut.Text = ConfigData.Current.AppConfig.PosComm.ComUTimeOut;
            txtComQTimeOut.Text = ConfigData.Current.AppConfig.PosComm.ComQTimeOut;

            SetTextIP(txtSvrGftIP1_01, txtSvrGftIP1_02, txtSvrGftIP1_03, txtSvrGftIP1_04, ConfigData.Current.AppConfig.PosComm.SvrGftIP1);
            txtComGPort1.Text = ConfigData.Current.AppConfig.PosComm.ComGPort1;
            SetTextIP(txtSvrGftIP2_01, txtSvrGftIP2_02, txtSvrGftIP2_03, txtSvrGftIP2_04, ConfigData.Current.AppConfig.PosComm.SvrGftIP2);
            txtComGPort2.Text = ConfigData.Current.AppConfig.PosComm.ComGPort2;
            txtComGTimeOut.Text = ConfigData.Current.AppConfig.PosComm.ComGTimeOut;

            SetTextIP(txtSvrPntIP1_01, txtSvrPntIP1_02, txtSvrPntIP1_03, txtSvrPntIP1_04, ConfigData.Current.AppConfig.PosComm.SvrPntIP1);
            txtComPPort1.Text = ConfigData.Current.AppConfig.PosComm.ComPPort1;
            SetTextIP(txtSvrPntIP2_01, txtSvrPntIP2_02, txtSvrPntIP2_03, txtSvrPntIP2_04, ConfigData.Current.AppConfig.PosComm.SvrPntIP2);
            txtComPPort2.Text = ConfigData.Current.AppConfig.PosComm.ComPPort2;
            txtComPTimeOut.Text = ConfigData.Current.AppConfig.PosComm.ComPTimeOut;

            SetTextIP(txtHqSvrIP1_01, txtHqSvrIP1_02, txtHqSvrIP1_03, txtHqSvrIP1_04, ConfigData.Current.AppConfig.PosComm.HqSvrIP1);
            txtHqComQPort1.Text = ConfigData.Current.AppConfig.PosComm.HqComQPort1;
            txtHqComUPort1.Text = ConfigData.Current.AppConfig.PosComm.HqComUPort1;
            SetTextIP(txtHqSvrIP2_01, txtHqSvrIP2_02, txtHqSvrIP2_03, txtHqSvrIP2_04, ConfigData.Current.AppConfig.PosComm.HqSvrIP2);
            txtHqComQPort2.Text = ConfigData.Current.AppConfig.PosComm.HqComQPort2;
            txtHqComUPort2.Text = ConfigData.Current.AppConfig.PosComm.HqComUPort2;
            txtHqComQTimeOut.Text = ConfigData.Current.AppConfig.PosComm.HqComQTimeOut;
            txtHqComUTimeOut.Text = ConfigData.Current.AppConfig.PosComm.HqComUTimeOut;

            SetTextIP(txtFtpSvrIP1_01, txtFtpSvrIP1_02, txtFtpSvrIP1_03, txtFtpSvrIP1_04, ConfigData.Current.AppConfig.PosFTP.FtpSvrIP1);
            txtFtpComPort1.Text = ConfigData.Current.AppConfig.PosFTP.FtpComPort1;
            SetTextIP(txtFtpSvrIP2_01, txtFtpSvrIP2_02, txtFtpSvrIP2_03, txtFtpSvrIP2_04, ConfigData.Current.AppConfig.PosFTP.FtpSvrIP2);
            txtFtpComPort2.Text = ConfigData.Current.AppConfig.PosFTP.FtpComPort2;
            txtMode.Text = ConfigData.Current.AppConfig.PosFTP.Mode;
            txtUser.Text = ConfigData.Current.AppConfig.PosFTP.User;
            txtPass.Text = ConfigData.Current.AppConfig.PosFTP.Pass;
            txtJournalPath.Text = ConfigData.Current.AppConfig.PosFTP.JournalPath;
            txtDataFileDownloadPath.Text = ConfigData.Current.AppConfig.PosFTP.DataFileDownloadPath;
            txtVersionInfoPath.Text = ConfigData.Current.AppConfig.PosFTP.VersionInfoPath;
            txtCreateUploadPathByDate.Text = ConfigData.Current.AppConfig.PosFTP.CreateUploadPathByDate;

            SetTextIP(txtVanSvrIP1_01, txtVanSvrIP1_02, txtVanSvrIP1_03, txtVanSvrIP1_04, ConfigData.Current.AppConfig.PosVan.VanSvrIP1);
            txtVanComPort1.Text = ConfigData.Current.AppConfig.PosVan.VanComPort1;
            SetTextIP(txtVanSvrIP2_01, txtVanSvrIP2_02, txtVanSvrIP2_03, txtVanSvrIP2_04, ConfigData.Current.AppConfig.PosVan.VanSvrIP2);
            txtVanComPort2.Text = ConfigData.Current.AppConfig.PosVan.VanComPort2;
            txtComTimeOut.Text = ConfigData.Current.AppConfig.PosVan.ComTimeOut;

            txtPointUse.Text = ConfigData.Current.AppConfig.PosOption.PointUse;
            txtPointSchemePrefix.Text = ConfigData.Current.AppConfig.PosOption.PointSchemePrefix;
            txtPointPayKeyInputEnable.Text = ConfigData.Current.AppConfig.PosOption.PointPayKeyInputEnable;
            txtCashReceiptUse.Text = ConfigData.Current.AppConfig.PosOption.CashReceiptUse;
            txtCashReceiptIssue.Text = ConfigData.Current.AppConfig.PosOption.CashReceiptIssue;
            txtCashReceiptApplAmount.Text = ConfigData.Current.AppConfig.PosOption.CashReceiptApplAmount;
            txtGoodsCodePrefix1.Text = ConfigData.Current.AppConfig.PosOption.GoodsCodePrefix1;
            txtGoodsCodePrefix2.Text = ConfigData.Current.AppConfig.PosOption.GoodsCodePrefix2;
            txtDataKeepDays.Text = ConfigData.Current.AppConfig.PosOption.DataKeepDays;
            txtSalesReturn.Text = ConfigData.Current.AppConfig.PosOption.SalesReturn;
            txtSignUploadTask.Text = ConfigData.Current.AppConfig.PosOption.SignUploadTask;
            txtTransUploadTask.Text = ConfigData.Current.AppConfig.PosOption.TransUploadTask;
            txtTransStatusTask.Text = ConfigData.Current.AppConfig.PosOption.TransStatusTask;
            txtNoticeStatusTask.Text = ConfigData.Current.AppConfig.PosOption.NoticeStatusTask;

            #endregion

            // iTabIndex = iTabIndex;
            txtStoreNo.SetFocus();
        }

        /// <summary>
        /// Key Event
        /// </summary>
        /// <param name="e"></param>
        void form_KeyEvent(OPOSKeyEventArgs e)
        {
            if (_bDisable)
            {
                e.IsHandled = true;
                return;
            }

            if (!e.IsControlKey)
            {
                if (txtStoreType.IsFocused || txtMode.IsFocused || txtPointUse.IsFocused || txtPointPayKeyInputEnable.IsFocused || txtCashReceiptUse.IsFocused || txtCashReceiptIssue.IsFocused)
                {
                    if (e.KeyCodeText != "0" && e.KeyCodeText != "1")
                    {
                        e.IsHandled = true;
                        return;
                    }
                }
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER )
            {
                #region 기본설정

                if (txtStoreNo.IsFocused)
                {
                    txtStoreType.SetFocus();
                }
                else if (txtStoreType.IsFocused)
                {
                    txtPosNo.SetFocus();
                }
                else if (txtPosNo.IsFocused)
                {
                    txtSaleDate.SetFocus();
                }
                else if (txtSaleDate.IsFocused)
                {
                    txtTrxnNo.SetFocus();
                }
                else if (txtTrxnNo.IsFocused)
                {
                    txtShiftCount.SetFocus();
                }
                else if (txtShiftCount.IsFocused)
                {
                    txtEodFlag.SetFocus();
                }
                else if (txtEodFlag.IsFocused)
                {
                    txtSodStatus.SetFocus();
                }
                else if (txtSodStatus.IsFocused)
                {
                    txtSlipNumber.SetFocus();
                }
                else if (txtSlipNumber.IsFocused)
                {
                    txtFrameNumber.SetFocus();
                }
                else if (txtFrameNumber.IsFocused)
                {
                    txtVersion.SetFocus();
                }
                else if (txtVersion.IsFocused)
                {
                    txtLastNoticeTime.SetFocus();
                }
                else if (txtLastNoticeTime.IsFocused)
                {
                    txtStoreType.SetFocus();
                }

                #endregion

                #region 점서버

                if (txtSvrIP1_01.IsFocused)
                {
                    txtSvrIP1_02.SetFocus();
                }
                else if (txtSvrIP1_02.IsFocused)
                {
                    txtSvrIP1_03.SetFocus();
                }
                else if (txtSvrIP1_03.IsFocused)
                {
                    txtSvrIP1_04.SetFocus();
                }
                else if (txtSvrIP1_04.IsFocused)
                {
                    txtComDPort1.SetFocus();
                }
                else if (txtComDPort1.IsFocused)
                {
                    txtComUPort1.SetFocus();
                }
                else if (txtComUPort1.IsFocused)
                {
                    txtComQPort1.SetFocus();
                }
                else if (txtComQPort1.IsFocused)
                {
                    txtSvrIP2_01.SetFocus();
                }
                else if (txtSvrIP2_01.IsFocused)
                {
                    txtSvrIP2_02.SetFocus();
                }
                else if (txtSvrIP2_02.IsFocused)
                {
                    txtSvrIP2_03.SetFocus();
                }
                else if (txtSvrIP2_03.IsFocused)
                {
                    txtSvrIP2_04.SetFocus();
                }
                else if (txtSvrIP2_04.IsFocused)
                {
                    txtComDPort2.SetFocus();
                }
                else if (txtComDPort2.IsFocused)
                {
                    txtComUPort2.SetFocus();
                }
                else if (txtComUPort2.IsFocused)
                {
                    txtComQPort2.SetFocus();
                }
                else if (txtComQPort2.IsFocused)
                {
                    txtComDTimeOut.SetFocus();
                }
                else if (txtComDTimeOut.IsFocused)
                {
                    txtComUTimeOut.SetFocus();
                }
                else if (txtComUTimeOut.IsFocused)
                {
                    txtComQTimeOut.SetFocus();
                }
                else if (txtComQTimeOut.IsFocused)
                {
                    txtSvrIP1_01.SetFocus();
                }

                #endregion

                #region 본부서버

                if (txtSvrGftIP1_01.IsFocused)
                {
                    txtSvrGftIP1_02.SetFocus();
                }
                else if (txtSvrGftIP1_02.IsFocused)
                {
                    txtSvrGftIP1_03.SetFocus();
                }
                else if (txtSvrGftIP1_03.IsFocused)
                {
                    txtSvrGftIP1_04.SetFocus();
                }
                else if (txtSvrGftIP1_04.IsFocused)
                {
                    txtComGPort1.SetFocus();
                }
                else if (txtComGPort1.IsFocused)
                {
                    txtSvrGftIP2_01.SetFocus();
                }
                else if (txtSvrGftIP2_01.IsFocused)
                {
                    txtSvrGftIP2_02.SetFocus();
                }
                else if (txtSvrGftIP2_02.IsFocused)
                {
                    txtSvrGftIP2_03.SetFocus();
                }
                else if (txtSvrGftIP2_03.IsFocused)
                {
                    txtSvrGftIP2_04.SetFocus();
                }
                else if (txtSvrGftIP2_04.IsFocused)
                {
                    txtComGPort2.SetFocus();
                }
                else if (txtComGPort2.IsFocused)
                {
                    txtComGTimeOut.SetFocus();
                }
                else if (txtComGTimeOut.IsFocused)
                {
                    txtSvrPntIP1_01.SetFocus();
                }
                else if (txtSvrPntIP1_01.IsFocused)
                {
                    txtSvrPntIP1_02.SetFocus();
                }
                else if (txtSvrPntIP1_02.IsFocused)
                {
                    txtSvrPntIP1_03.SetFocus();
                }
                else if (txtSvrPntIP1_03.IsFocused)
                {
                    txtSvrPntIP1_04.SetFocus();
                }
                else if (txtSvrPntIP1_04.IsFocused)
                {
                    txtComPPort1.SetFocus();
                }
                else if (txtComPPort1.IsFocused)
                {
                    txtSvrPntIP2_01.SetFocus();
                }
                else if (txtSvrPntIP2_01.IsFocused)
                {
                    txtSvrPntIP2_02.SetFocus();
                }
                else if (txtSvrPntIP2_02.IsFocused)
                {
                    txtSvrPntIP2_03.SetFocus();
                }
                else if (txtSvrPntIP2_03.IsFocused)
                {
                    txtSvrPntIP2_04.SetFocus();
                }
                else if (txtSvrPntIP2_04.IsFocused)
                {
                    txtComPPort2.SetFocus();
                }
                else if (txtComPPort2.IsFocused)
                {
                    txtComPTimeOut.SetFocus();
                }
                else if (txtComPTimeOut.IsFocused)
                {
                    txtHqSvrIP1_01.SetFocus();
                }
                else if (txtHqSvrIP1_01.IsFocused)
                {
                    txtHqSvrIP1_02.SetFocus();
                }
                else if (txtHqSvrIP1_02.IsFocused)
                {
                    txtHqSvrIP1_03.SetFocus();
                }
                else if (txtHqSvrIP1_03.IsFocused)
                {
                    txtHqSvrIP1_04.SetFocus();
                }
                else if (txtHqSvrIP1_04.IsFocused)
                {
                    txtHqComQPort1.SetFocus();
                }
                else if (txtHqComQPort1.IsFocused)
                {
                    txtHqComUPort1.SetFocus();
                }
                else if (txtHqComUPort1.IsFocused)
                {
                    txtHqSvrIP2_01.SetFocus();
                }
                else if (txtHqSvrIP2_01.IsFocused)
                {
                    txtHqSvrIP2_02.SetFocus();
                }
                else if (txtHqSvrIP2_02.IsFocused)
                {
                    txtHqSvrIP2_03.SetFocus();
                }
                else if (txtHqSvrIP2_03.IsFocused)
                {
                    txtHqSvrIP2_04.SetFocus();
                }
                else if (txtHqSvrIP2_04.IsFocused)
                {
                    txtHqComQPort2.SetFocus();
                }
                else if (txtHqComQPort2.IsFocused)
                {
                    txtHqComUPort2.SetFocus();
                }
                else if (txtHqComUPort2.IsFocused)
                {
                    txtHqComQTimeOut.SetFocus();
                }
                else if (txtHqComQTimeOut.IsFocused)
                {
                    txtHqComUTimeOut.SetFocus();
                }
                else if (txtHqComUTimeOut.IsFocused)
                {
                    txtSvrGftIP1_01.SetFocus();
                }

                #endregion

                #region FTP

                if (txtFtpSvrIP1_01.IsFocused)
                {
                    txtFtpSvrIP1_02.SetFocus();
                }
                else if (txtFtpSvrIP1_02.IsFocused)
                {
                    txtFtpSvrIP1_03.SetFocus();
                }
                else if (txtFtpSvrIP1_03.IsFocused)
                {
                    txtFtpSvrIP1_04.SetFocus();
                }
                else if (txtFtpSvrIP1_04.IsFocused)
                {
                    txtFtpComPort1.SetFocus();
                }
                else if (txtFtpComPort1.IsFocused)
                {
                    txtFtpSvrIP2_01.SetFocus();
                }
                else if (txtFtpSvrIP2_01.IsFocused)
                {
                    txtFtpSvrIP2_02.SetFocus();
                }
                else if (txtFtpSvrIP2_02.IsFocused)
                {
                    txtFtpSvrIP2_03.SetFocus();
                }
                else if (txtFtpSvrIP2_03.IsFocused)
                {
                    txtFtpSvrIP2_04.SetFocus();
                }
                else if (txtFtpSvrIP2_04.IsFocused)
                {
                    txtFtpComPort2.SetFocus();
                }
                else if (txtFtpComPort2.IsFocused)
                {
                    txtMode.SetFocus();
                }
                else if (txtMode.IsFocused)
                {
                    txtUser.SetFocus();
                }
                else if (txtUser.IsFocused)
                {
                    txtPass.SetFocus();
                }
                else if (txtPass.IsFocused)
                {
                    txtJournalPath.SetFocus();
                }
                else if (txtJournalPath.IsFocused)
                {
                    txtDataFileDownloadPath.SetFocus();
                }
                else if (txtDataFileDownloadPath.IsFocused)
                {
                    txtVersionInfoPath.SetFocus();
                }
                else if (txtVersionInfoPath.IsFocused)
                {
                    txtCreateUploadPathByDate.SetFocus();
                }
                else if (txtCreateUploadPathByDate.IsFocused)
                {
                    txtFtpSvrIP1_01.SetFocus();
                }

                #endregion

                #region VAN

                if (txtVanSvrIP1_01.IsFocused)
                {
                    txtVanSvrIP1_02.SetFocus();
                }
                else if (txtVanSvrIP1_02.IsFocused)
                {
                    txtVanSvrIP1_03.SetFocus();
                }
                else if (txtVanSvrIP1_03.IsFocused)
                {
                    txtVanSvrIP1_04.SetFocus();
                }
                else if (txtVanSvrIP1_04.IsFocused)
                {
                    txtVanComPort1.SetFocus();
                }
                else if (txtVanComPort1.IsFocused)
                {
                    txtVanSvrIP2_01.SetFocus();
                }
                else if (txtVanSvrIP2_01.IsFocused)
                {
                    txtVanSvrIP2_02.SetFocus();
                }
                else if (txtVanSvrIP2_02.IsFocused)
                {
                    txtVanSvrIP2_03.SetFocus();
                }
                else if (txtVanSvrIP2_03.IsFocused)
                {
                    txtVanSvrIP2_04.SetFocus();
                }
                else if (txtVanSvrIP2_04.IsFocused)
                {
                    txtVanComPort2.SetFocus();
                }
                else if (txtVanComPort2.IsFocused)
                {
                    txtComTimeOut.SetFocus();
                }
                else if (txtComTimeOut.IsFocused)
                {
                    txtVanSvrIP1_01.SetFocus();
                }

                #endregion

                #region 기타옵션

                if (txtPointUse.IsFocused)
                {
                    txtPointSchemePrefix.SetFocus();
                }
                else if (txtPointSchemePrefix.IsFocused)
                {
                    txtPointPayKeyInputEnable.SetFocus();
                }
                else if (txtPointPayKeyInputEnable.IsFocused)
                {
                    txtCashReceiptUse.SetFocus();
                }
                else if (txtCashReceiptUse.IsFocused)
                {
                    txtCashReceiptIssue.SetFocus();
                }
                else if (txtCashReceiptIssue.IsFocused)
                {
                    txtCashReceiptApplAmount.SetFocus();
                }
                else if (txtCashReceiptApplAmount.IsFocused)
                {
                    txtGoodsCodePrefix1.SetFocus();
                }
                else if (txtGoodsCodePrefix1.IsFocused)
                {
                    txtGoodsCodePrefix2.SetFocus();
                }
                else if (txtGoodsCodePrefix2.IsFocused)
                {
                    txtDataKeepDays.SetFocus();
                }
                else if (txtDataKeepDays.IsFocused)
                {
                    txtSalesReturn.SetFocus();
                }
                else if (txtSalesReturn.IsFocused)
                {
                    txtSignUploadTask.SetFocus();
                }
                else if (txtSignUploadTask.IsFocused)
                {
                    txtTransUploadTask.SetFocus();
                }
                else if (txtTransUploadTask.IsFocused)
                {
                    txtTransStatusTask.SetFocus();
                }
                else if (txtTransStatusTask.IsFocused)
                {
                    txtNoticeStatusTask.SetFocus();
                }
                else if (txtNoticeStatusTask.IsFocused)
                {
                    txtPointUse.SetFocus();
                }

                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
            {
                #region 기본설정

                if (txtStoreNo.IsFocused)
                {
                    if (txtStoreNo.Text.Length <= 0)
                    {

                    }
                }
                else if (txtStoreType.IsFocused)
                {
                    if (txtStoreType.Text.Length <= 0)
                    {
                        txtStoreNo.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtPosNo.IsFocused)
                {
                    if (txtPosNo.Text.Length <= 0)
                    {
                        txtStoreType.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSaleDate.IsFocused)
                {
                    if (txtSaleDate.Text.Length <= 0)
                    {
                        txtPosNo.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtTrxnNo.IsFocused)
                {
                    if (txtTrxnNo.Text.Length <= 0)
                    {
                        txtSaleDate.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtShiftCount.IsFocused)
                {
                    if (txtShiftCount.Text.Length <= 0)
                    {
                        txtTrxnNo.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtEodFlag.IsFocused)
                {
                    if (txtEodFlag.Text.Length <= 0)
                    {
                        txtShiftCount.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSodStatus.IsFocused)
                {
                    if (txtSodStatus.Text.Length <= 0)
                    {
                        txtEodFlag.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSlipNumber.IsFocused)
                {
                    if (txtSlipNumber.Text.Length <= 0)
                    {
                        txtSodStatus.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtFrameNumber.IsFocused)
                {
                    if (txtFrameNumber.Text.Length <= 0)
                    {
                        txtSlipNumber.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtVersion.IsFocused)
                {
                    if (txtVersion.Text.Length <= 0)
                    {
                        txtFrameNumber.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtLastNoticeTime.IsFocused)
                {
                    if (txtLastNoticeTime.Text.Length <= 0)
                    {
                        txtVersion.SetFocus();
                        e.IsHandled = true;
                    }
                }

                #endregion

                #region 점서버

                if (txtSvrIP1_01.IsFocused)
                {
                    if (txtSvrIP1_01.Text.Length <= 0)
                    {
                        
                    }
                }
                else if (txtSvrIP1_02.IsFocused)
                {
                    if (txtSvrIP1_02.Text.Length <= 0)
                    {
                        txtSvrIP1_01.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrIP1_03.IsFocused)
                {
                    if (txtSvrIP1_03.Text.Length <= 0)
                    {
                        txtSvrIP1_02.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrIP1_04.IsFocused)
                {
                    if (txtSvrIP1_04.Text.Length <= 0)
                    {
                        txtSvrIP1_03.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtComDPort1.IsFocused)
                {
                    if (txtComDPort1.Text.Length <= 0)
                    {
                        txtSvrIP1_04.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtComUPort1.IsFocused)
                {
                    if (txtComUPort1.Text.Length <= 0)
                    {
                        txtComDPort1.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtComQPort1.IsFocused)
                {
                    if (txtComQPort1.Text.Length <= 0)
                    {
                        txtComUPort1.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrIP2_01.IsFocused)
                {
                    if (txtSvrIP2_01.Text.Length <= 0)
                    {
                        txtComQPort1.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrIP2_02.IsFocused)
                {
                    if (txtSvrIP2_02.Text.Length <= 0)
                    {
                        txtSvrIP2_01.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrIP2_03.IsFocused)
                {
                    if (txtSvrIP2_03.Text.Length <= 0)
                    {
                        txtSvrIP2_02.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrIP2_04.IsFocused)
                {
                    if (txtSvrIP2_04.Text.Length <= 0)
                    {
                        txtSvrIP2_03.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtComDPort2.IsFocused)
                {
                    if (txtComDPort2.Text.Length <= 0)
                    {
                        txtSvrIP2_04.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtComUPort2.IsFocused)
                {
                    if (txtComUPort2.Text.Length <= 0)
                    {
                        txtComDPort2.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtComQPort2.IsFocused)
                {
                    if (txtComQPort2.Text.Length <= 0)
                    {
                        txtComUPort2.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtComDTimeOut.IsFocused)
                {
                    if (txtComDTimeOut.Text.Length <= 0)
                    {
                        txtComQPort2.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtComUTimeOut.IsFocused)
                {
                    if (txtComUTimeOut.Text.Length <= 0)
                    {
                        txtComDTimeOut.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtComQTimeOut.IsFocused)
                {
                    if (txtComQTimeOut.Text.Length <= 0)
                    {
                        txtComUTimeOut.SetFocus();
                        e.IsHandled = true;
                    }
                }

                #endregion

                #region 본부서버

                if (txtSvrGftIP1_01.IsFocused)
                {
                    
                }
                else if (txtSvrGftIP1_02.IsFocused)
                {
                    if (txtSvrGftIP1_02.Text.Length <= 0)
                    {
                        txtSvrGftIP1_01.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrGftIP1_03.IsFocused)
                {
                    if (txtSvrGftIP1_03.Text.Length <= 0)
                    {
                        txtSvrGftIP1_02.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrGftIP1_04.IsFocused)
                {
                    if (txtSvrGftIP1_04.Text.Length <= 0)
                    {
                        txtSvrGftIP1_03.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtComGPort1.IsFocused)
                {
                    if (txtComGPort1.Text.Length <= 0)
                    {
                        txtSvrGftIP1_04.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrGftIP2_01.IsFocused)
                {
                    if (txtSvrGftIP2_01.Text.Length <= 0)
                    {
                        txtComGPort1.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrGftIP2_02.IsFocused)
                {
                    if (txtSvrGftIP2_02.Text.Length <= 0)
                    {
                        txtSvrGftIP2_01.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrGftIP2_03.IsFocused)
                {
                    if (txtSvrGftIP2_03.Text.Length <= 0)
                    {
                        txtSvrGftIP2_02.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrGftIP2_04.IsFocused)
                {
                    if (txtSvrGftIP2_04.Text.Length <= 0)
                    {
                        txtSvrGftIP2_03.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtComGPort2.IsFocused)
                {
                    if (txtComGPort2.Text.Length <= 0)
                    {
                        txtSvrGftIP2_04.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtComGTimeOut.IsFocused)
                {
                    if (txtComGTimeOut.Text.Length <= 0)
                    {
                        txtComGPort2.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrPntIP1_01.IsFocused)
                {
                    if (txtSvrPntIP1_01.Text.Length <= 0)
                    {
                        txtComGTimeOut.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrPntIP1_02.IsFocused)
                {
                    if (txtSvrPntIP1_02.Text.Length <= 0)
                    {
                        txtSvrPntIP1_01.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrPntIP1_03.IsFocused)
                {
                    if (txtSvrPntIP1_03.Text.Length <= 0)
                    {
                        txtSvrPntIP1_02.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrPntIP1_04.IsFocused)
                {
                    if (txtSvrPntIP1_04.Text.Length <= 0)
                    {
                        txtSvrPntIP1_03.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtComPPort1.IsFocused)
                {
                    if (txtComPPort1.Text.Length <= 0)
                    {
                        txtSvrPntIP1_04.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrPntIP2_01.IsFocused)
                {
                    if (txtSvrPntIP2_01.Text.Length <= 0)
                    {
                        txtComPPort1.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrPntIP2_02.IsFocused)
                {
                    if (txtSvrPntIP2_02.Text.Length <= 0)
                    {
                        txtSvrPntIP2_01.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrPntIP2_03.IsFocused)
                {
                    if (txtSvrPntIP2_03.Text.Length <= 0)
                    {
                        txtSvrPntIP2_02.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSvrPntIP2_04.IsFocused)
                {
                    if (txtSvrPntIP2_04.Text.Length <= 0)
                    {
                        txtSvrPntIP2_03.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtComPPort2.IsFocused)
                {
                    if (txtComPPort2.Text.Length <= 0)
                    {
                        txtSvrPntIP2_04.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtComPTimeOut.IsFocused)
                {
                    if (txtComPTimeOut.Text.Length <= 0)
                    {
                        txtComPPort2.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtHqSvrIP1_01.IsFocused)
                {
                    if (txtHqSvrIP1_01.Text.Length <= 0)
                    {
                        txtComPTimeOut.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtHqSvrIP1_02.IsFocused)
                {
                    if (txtHqSvrIP1_02.Text.Length <= 0)
                    {
                        txtHqSvrIP1_01.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtHqSvrIP1_03.IsFocused)
                {
                    if (txtHqSvrIP1_03.Text.Length <= 0)
                    {
                        txtHqSvrIP1_02.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtHqSvrIP1_04.IsFocused)
                {
                    if (txtHqSvrIP1_04.Text.Length <= 0)
                    {
                        txtHqSvrIP1_03.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtHqComQPort1.IsFocused)
                {
                    if (txtHqComQPort1.Text.Length <= 0)
                    {
                        txtHqSvrIP1_04.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtHqComUPort1.IsFocused)
                {
                    if (txtHqComUPort1.Text.Length <= 0)
                    {
                        txtHqComQPort1.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtHqSvrIP2_01.IsFocused)
                {
                    if (txtHqSvrIP2_01.Text.Length <= 0)
                    {
                        txtHqComUPort1.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtHqSvrIP2_02.IsFocused)
                {
                    if (txtHqSvrIP2_02.Text.Length <= 0)
                    {
                        txtHqSvrIP2_01.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtHqSvrIP2_03.IsFocused)
                {
                    if (txtHqSvrIP2_03.Text.Length <= 0)
                    {
                        txtHqSvrIP2_02.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtHqSvrIP2_04.IsFocused)
                {
                    if (txtHqSvrIP2_04.Text.Length <= 0)
                    {
                        txtHqSvrIP2_03.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtHqComQPort2.IsFocused)
                {
                    if (txtHqComQPort2.Text.Length <= 0)
                    {
                        txtHqSvrIP2_04.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtHqComUPort2.IsFocused)
                {
                    if (txtHqComUPort2.Text.Length <= 0)
                    {
                        txtHqComQPort2.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtHqComQTimeOut.IsFocused)
                {
                    if (txtHqComQTimeOut.Text.Length <= 0)
                    {
                        txtHqComUPort2.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtHqComUTimeOut.IsFocused)
                {
                    if (txtHqComUTimeOut.Text.Length <= 0)
                    {
                        txtHqComQTimeOut.SetFocus();
                        e.IsHandled = true;
                    }
                }

                #endregion

                #region FTP

                if (txtFtpSvrIP1_01.IsFocused)
                {
                    if (txtFtpSvrIP1_01.Text.Length <= 0)
                    {
                        
                    }
                }
                else if (txtFtpSvrIP1_02.IsFocused)
                {
                    if (txtFtpSvrIP1_02.Text.Length <= 0)
                    {
                        txtFtpSvrIP1_01.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtFtpSvrIP1_03.IsFocused)
                {
                    if (txtFtpSvrIP1_03.Text.Length <= 0)
                    {
                        txtFtpSvrIP1_02.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtFtpSvrIP1_04.IsFocused)
                {
                    if (txtFtpSvrIP1_04.Text.Length <= 0)
                    {
                        txtFtpSvrIP1_03.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtFtpComPort1.IsFocused)
                {
                    if (txtFtpComPort1.Text.Length <= 0)
                    {
                        txtFtpSvrIP1_04.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtFtpSvrIP2_01.IsFocused)
                {
                    if (txtFtpSvrIP2_01.Text.Length <= 0)
                    {
                        txtFtpComPort1.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtFtpSvrIP2_02.IsFocused)
                {
                    if (txtFtpSvrIP2_02.Text.Length <= 0)
                    {
                        txtFtpSvrIP2_01.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtFtpSvrIP2_03.IsFocused)
                {
                    if (txtFtpSvrIP2_03.Text.Length <= 0)
                    {
                        txtFtpSvrIP2_02.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtFtpSvrIP2_04.IsFocused)
                {
                    if (txtFtpSvrIP2_04.Text.Length <= 0)
                    {
                        txtFtpSvrIP2_03.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtFtpComPort2.IsFocused)
                {
                    if (txtFtpComPort2.Text.Length <= 0)
                    {
                        txtFtpSvrIP2_04.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtMode.IsFocused)
                {
                    if (txtMode.Text.Length <= 0)
                    {
                        txtFtpComPort2.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtUser.IsFocused)
                {
                    if (txtUser.Text.Length <= 0)
                    {
                        txtMode.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtPass.IsFocused)
                {
                    if (txtPass.Text.Length <= 0)
                    {
                        txtUser.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtJournalPath.IsFocused)
                {
                    if (txtJournalPath.Text.Length <= 0)
                    {
                        txtPass.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtDataFileDownloadPath.IsFocused)
                {
                    if (txtDataFileDownloadPath.Text.Length <= 0)
                    {
                        txtJournalPath.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtVersionInfoPath.IsFocused)
                {
                    if (txtVersionInfoPath.Text.Length <= 0)
                    {
                        txtDataFileDownloadPath.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtCreateUploadPathByDate.IsFocused)
                {
                    if (txtCreateUploadPathByDate.Text.Length <= 0)
                    {
                        txtVersionInfoPath.SetFocus();
                        e.IsHandled = true;
                    }
                }

                #endregion

                #region VAN

                if (txtVanSvrIP1_01.IsFocused)
                {
                    if (txtVanSvrIP1_01.Text.Length <= 0)
                    {
                        
                    }
                }
                else if (txtVanSvrIP1_02.IsFocused)
                {
                    if (txtVanSvrIP1_02.Text.Length <= 0)
                    {
                        txtVanSvrIP1_01.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtVanSvrIP1_03.IsFocused)
                {
                    if (txtVanSvrIP1_03.Text.Length <= 0)
                    {
                        txtVanSvrIP1_02.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtVanSvrIP1_04.IsFocused)
                {
                    if (txtVanSvrIP1_04.Text.Length <= 0)
                    {
                        txtVanSvrIP1_03.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtVanComPort1.IsFocused)
                {
                    if (txtVanComPort1.Text.Length <= 0)
                    {
                        txtVanSvrIP1_04.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtVanSvrIP2_01.IsFocused)
                {
                    if (txtVanSvrIP2_01.Text.Length <= 0)
                    {
                        txtVanComPort1.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtVanSvrIP2_02.IsFocused)
                {
                    if (txtVanSvrIP2_02.Text.Length <= 0)
                    {
                        txtVanSvrIP2_01.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtVanSvrIP2_03.IsFocused)
                {
                    if (txtVanSvrIP2_03.Text.Length <= 0)
                    {
                        txtVanSvrIP2_02.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtVanSvrIP2_04.IsFocused)
                {
                    if (txtVanSvrIP2_04.Text.Length <= 0)
                    {
                        txtVanSvrIP2_03.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtVanComPort2.IsFocused)
                {
                    if (txtVanComPort2.Text.Length <= 0)
                    {
                        txtVanSvrIP2_04.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtComTimeOut.IsFocused)
                {
                    if (txtComTimeOut.Text.Length <= 0)
                    {
                        txtVanComPort2.SetFocus();
                        e.IsHandled = true;
                    }
                }

                #endregion

                #region 기타옵션

                if (txtPointUse.IsFocused)
                {
                    if (txtPointUse.Text.Length <= 0)
                    {

                    }
                }
                else if (txtPointSchemePrefix.IsFocused)
                {
                    if (txtPointSchemePrefix.Text.Length <= 0)
                    {
                        txtPointUse.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtPointPayKeyInputEnable.IsFocused)
                {
                    if (txtPointPayKeyInputEnable.Text.Length <= 0)
                    {
                        txtPointSchemePrefix.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtCashReceiptUse.IsFocused)
                {
                    if (txtCashReceiptUse.Text.Length <= 0)
                    {
                        txtPointPayKeyInputEnable.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtCashReceiptIssue.IsFocused)
                {
                    if (txtCashReceiptIssue.Text.Length <= 0)
                    {
                        txtCashReceiptUse.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtCashReceiptApplAmount.IsFocused)
                {
                    if (txtCashReceiptApplAmount.Text.Length <= 0)
                    {
                        txtCashReceiptIssue.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtGoodsCodePrefix1.IsFocused)
                {
                    if (txtGoodsCodePrefix1.Text.Length <= 0)
                    {
                        txtCashReceiptApplAmount.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtGoodsCodePrefix2.IsFocused)
                {
                    if (txtGoodsCodePrefix2.Text.Length <= 0)
                    {
                        txtGoodsCodePrefix1.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtDataKeepDays.IsFocused)
                {
                    if (txtDataKeepDays.Text.Length <= 0)
                    {
                        txtGoodsCodePrefix2.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSalesReturn.IsFocused)
                {
                    if (txtSalesReturn.Text.Length <= 0)
                    {
                        txtDataKeepDays.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtSignUploadTask.IsFocused)
                {
                    if (txtSignUploadTask.Text.Length <= 0)
                    {
                        txtSalesReturn.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtTransUploadTask.IsFocused)
                {
                    if (txtTransUploadTask.Text.Length <= 0)
                    {
                        txtSignUploadTask.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtTransStatusTask.IsFocused)
                {
                    if (txtTransStatusTask.Text.Length <= 0)
                    {
                        txtTransUploadTask.SetFocus();
                        e.IsHandled = true;
                    }
                }
                else if (txtNoticeStatusTask.IsFocused)
                {
                    if (txtNoticeStatusTask.Text.Length <= 0)
                    {
                        txtTransStatusTask.SetFocus();
                        e.IsHandled = true;
                    }
                }

                #endregion
            }
        }

        /// <summary>
        /// 탭 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tc_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (_bDisable) return;

            if (ChkData())
            {
                msgBar.Text = strMsg07;
                e.Cancel = true;
            }
            else
            {
                tc.TabPages[tc.SelectedIndex].SuspendDrawing();
                iTabIndex = tc.SelectedIndex;
                

                if (tc.TabPages[iTabIndex].Name.ToString() == "tpPosInfo")
                {
                    txtStoreNo.SetFocus();
                }
                else if (tc.TabPages[iTabIndex].Name.ToString() == "tpPosComm01")
                {
                    txtSvrIP1_01.SetFocus();
                }
                else if (tc.TabPages[iTabIndex].Name.ToString() == "tpPosComm02")
                {
                    txtSvrGftIP1_01.SetFocus();
                }
                else if (tc.TabPages[iTabIndex].Name.ToString() == "tpPosFTP")
                {
                    txtFtpSvrIP1_01.SetFocus();
                }
                else if (tc.TabPages[iTabIndex].Name.ToString() == "tpPosVan")
                {
                    txtVanSvrIP1_01.SetFocus();
                }
                else if (tc.TabPages[iTabIndex].Name.ToString() == "tpPosOption")
                {
                    txtPointUse.SetFocus();
                }

                tc.TabPages[iTabIndex].ResumeDrawing();
            }
        }

        /// <summary>
        /// 포커스 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txt_InputFocused(object sender, EventArgs e)
        {
            InputText txt = (InputText)sender;

            msgBar.Text = "";

            if (txt.Name.ToString() == txtStoreType.Name.ToString())
            {
                msgBar.Text = strMsg01;
            }
            else if (txt.Name.ToString() == txtMode.Name.ToString())
            {
                msgBar.Text = strMsg02;
            }
            else if (txt.Name.ToString() == txtPointUse.Name.ToString())
            {
                msgBar.Text = strMsg03;
            }
            else if (txt.Name.ToString() == txtPointPayKeyInputEnable.Name.ToString())
            {
                msgBar.Text = strMsg04;
            }
            else if (txt.Name.ToString() == txtCashReceiptUse.Name.ToString())
            {
                msgBar.Text = strMsg05;
            }
            else if (txt.Name.ToString() == txtCashReceiptIssue.Name.ToString())
            {
                msgBar.Text = strMsg06;
            }
            //if (txt.Name.ToString() == txtScannerUse.Name.ToString() ||
            //    txt.Name.ToString() == txtCDPUse.Name.ToString() ||
            //    txt.Name.ToString() == txtPRINTERUse.Name.ToString() ||
            //    txt.Name.ToString() == txtMSRUse.Name.ToString() ||
            //    txt.Name.ToString() == txtCASHUse.Name.ToString() ||
            //    txt.Name.ToString() == txtSignPadUse.Name.ToString())
            //{
            //    msgBar.Text = strMsg01;
            //}
            //else if (txt.Name.ToString() == txtScannerMethod.Name.ToString() ||
            //    txt.Name.ToString() == txtCDPMethod.Name.ToString() ||
            //    txt.Name.ToString() == txtPRINTERMethod.Name.ToString())
            ////txt.Name.ToString() == txtMSRMethod.Name.ToString() ||
            ////txt.Name.ToString() == txtCASHMethod.Name.ToString() ||
            ////txt.Name.ToString() == txtSignPadMethod.Name.ToString())
            //{
            //    msgBar.Text = strMsg02;
            //}
            ////else if (txt.Name.ToString() == txtScannerLogicalName.Name.ToString() ||
            ////    txt.Name.ToString() == txtCDPLogicalName.Name.ToString() ||
            ////    txt.Name.ToString() == txtPRINTERLogicalName.Name.ToString() ||
            ////    txt.Name.ToString() == txtMSRLogicalName.Name.ToString() ||
            ////    txt.Name.ToString() == txtCASHLogicalName.Name.ToString() ||
            ////    txt.Name.ToString() == txtSignPadLogicalName.Name.ToString())
            ////{
            ////    msgBar.Text = strMsg03;
            ////}
            //else if (txt.Name.ToString() == txtScannerPort.Name.ToString() ||
            //    txt.Name.ToString() == txtCDPPort.Name.ToString() ||
            //    txt.Name.ToString() == txtPRINTERPort.Name.ToString() ||
            //    txt.Name.ToString() == txtSignPadPort.Name.ToString())
            //{
            //    msgBar.Text = strMsg03;
            //}
            //else if (txt.Name.ToString() == txtScannerSpeed.Name.ToString() ||
            //    txt.Name.ToString() == txtCDPSpeed.Name.ToString() ||
            //    txt.Name.ToString() == txtPRINTERSpeed.Name.ToString() ||
            //    txt.Name.ToString() == txtSignPadSpeed.Name.ToString())
            //{
            //    msgBar.Text = strMsg04;
            //}
            //else if (txt.Name.ToString() == txtScannerDataBit.Name.ToString() ||
            //    txt.Name.ToString() == txtCDPDataBit.Name.ToString() ||
            //    txt.Name.ToString() == txtPRINTERDataBit.Name.ToString() ||
            //    txt.Name.ToString() == txtSignPadDataBit.Name.ToString())
            //{
            //    msgBar.Text = strMsg05;
            //}
            //else if (txt.Name.ToString() == txtScannerStopBit.Name.ToString() ||
            //    txt.Name.ToString() == txtCDPStopBit.Name.ToString() ||
            //    txt.Name.ToString() == txtPRINTERStopBit.Name.ToString() ||
            //    txt.Name.ToString() == txtSignPadStopBit.Name.ToString())
            //{
            //    msgBar.Text = strMsg06;
            //}
            //else if (txt.Name.ToString() == txtScannerParity.Name.ToString() ||
            //    txt.Name.ToString() == txtCDPParity.Name.ToString() ||
            //    txt.Name.ToString() == txtPRINTERParity.Name.ToString() ||
            //    txt.Name.ToString() == txtSignPadParity.Name.ToString())
            //{
            //    msgBar.Text = strMsg07;
            //}
            //else if (txt.Name.ToString() == txtScannerFlowControl.Name.ToString() ||
            //    txt.Name.ToString() == txtCDPFlowControl.Name.ToString() ||
            //    txt.Name.ToString() == txtPRINTERFlowControl.Name.ToString() ||
            //    txt.Name.ToString() == txtSignPadFlowControl.Name.ToString())
            //{
            //    msgBar.Text = strMsg08;
            //}
            //else if (txt.Name.ToString() == txtMSRMethod.Name.ToString())
            //{
            //    msgBar.Text = strMsg09;
            //}
            //else if (txt.Name.ToString() == txtCASHMethod.Name.ToString())
            //{
            //    msgBar.Text = strMsg10;
            //}
            //else if (txt.Name.ToString() == txtSignPadMethod.Name.ToString())
            //{
            //    msgBar.Text = strMsg11;
            //}
            //else if (txt.Name.ToString() == txtPRINTERLogoBMP.Name.ToString())
            //{
            //    msgBar.Text = strMsg12;
            //}
            //else if (txt.Name.ToString() == txtPRINTERCutFeedCn.Name.ToString())
            //{
            //    msgBar.Text = strMsg13;
            //}
            //else if (txt.Name.ToString() == txtPRINTERBarCodeWidth.Name.ToString())
            //{
            //    msgBar.Text = strMsg14;
            //}
            //else if (txt.Name.ToString() == txtPRINTERBarCodeHeight.Name.ToString())
            //{
            //    msgBar.Text = strMsg15;
            //}
        }

        /// <summary>
        /// 저장 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            SetControlDisable(true);

            try
            {
                if (ChkData())
                {
                    if (tc.TabPages[iTabIndex].Name.ToString() == "tpPosInfo")
                    {
                        #region PosInfo

                        ConfigData.Current.AppConfig.PosInfo.StoreNo = !appChange.bStoreNo ? ConfigData.Current.AppConfig.PosInfo.StoreNo : txtStoreNo.Text;
                        ConfigData.Current.AppConfig.PosInfo.StoreType = !appChange.bStoreType ? ConfigData.Current.AppConfig.PosInfo.StoreType : txtStoreType.Text;
                        ConfigData.Current.AppConfig.PosInfo.PosNo = !appChange.bPosNo ? ConfigData.Current.AppConfig.PosInfo.PosNo : txtPosNo.Text;

                        #endregion
                    }
                    else if (tc.TabPages[iTabIndex].Name.ToString() == "tpPosComm01")
                    {
                        #region PosComm1

                        ConfigData.Current.AppConfig.PosComm.SvrIP1 = !appChange.bSvrIP1 ? ConfigData.Current.AppConfig.PosComm.SvrIP1 : GetTextIp(txtSvrIP1_01, txtSvrIP1_02, txtSvrIP1_03, txtSvrIP1_04);
                        ConfigData.Current.AppConfig.PosComm.ComDPort1 = !appChange.bComDPort1 ? ConfigData.Current.AppConfig.PosComm.ComDPort1 : txtComDPort1.Text;
                        ConfigData.Current.AppConfig.PosComm.ComUPort1 = !appChange.bComUPort1 ? ConfigData.Current.AppConfig.PosComm.ComUPort1 : txtComUPort1.Text;
                        ConfigData.Current.AppConfig.PosComm.ComQPort1 = !appChange.bComQPort1 ? ConfigData.Current.AppConfig.PosComm.ComQPort1 : txtComQPort1.Text;
                        ConfigData.Current.AppConfig.PosComm.SvrIP2 = !appChange.bSvrIP2 ? ConfigData.Current.AppConfig.PosComm.SvrIP2 : GetTextIp(txtSvrIP2_01, txtSvrIP2_02, txtSvrIP2_03, txtSvrIP2_04);
                        ConfigData.Current.AppConfig.PosComm.ComDPort2 = !appChange.bComDPort2 ? ConfigData.Current.AppConfig.PosComm.ComDPort2 : txtComDPort2.Text;
                        ConfigData.Current.AppConfig.PosComm.ComUPort2 = !appChange.bComUPort2 ? ConfigData.Current.AppConfig.PosComm.ComUPort2 : txtComUPort2.Text;
                        ConfigData.Current.AppConfig.PosComm.ComQPort2 = !appChange.bComQPort2 ? ConfigData.Current.AppConfig.PosComm.ComQPort2 : txtComQPort2.Text;
                        ConfigData.Current.AppConfig.PosComm.ComDTimeOut = !appChange.bComDTimeOut ? ConfigData.Current.AppConfig.PosComm.ComDTimeOut : txtComDTimeOut.Text;
                        ConfigData.Current.AppConfig.PosComm.ComUTimeOut = !appChange.bComUTimeOut ? ConfigData.Current.AppConfig.PosComm.ComUTimeOut : txtComUTimeOut.Text;
                        ConfigData.Current.AppConfig.PosComm.ComQTimeOut = !appChange.bComQTimeOut ? ConfigData.Current.AppConfig.PosComm.ComQTimeOut : txtComQTimeOut.Text;

                        #endregion
                    }
                    else if (tc.TabPages[iTabIndex].Name.ToString() == "tpPosComm02")
                    {
                        #region PosComm2

                        ConfigData.Current.AppConfig.PosComm.SvrGftIP1 = !appChange.bSvrGftIP1 ? ConfigData.Current.AppConfig.PosComm.SvrGftIP1 : GetTextIp(txtSvrGftIP1_01, txtSvrGftIP1_02, txtSvrGftIP1_03, txtSvrGftIP1_04);
                        ConfigData.Current.AppConfig.PosComm.ComGPort1 = !appChange.bComGPort1 ? ConfigData.Current.AppConfig.PosComm.ComGPort1 : txtComGPort1.Text;
                        ConfigData.Current.AppConfig.PosComm.SvrGftIP2 = !appChange.bSvrGftIP2 ? ConfigData.Current.AppConfig.PosComm.SvrGftIP2 : GetTextIp(txtSvrGftIP2_01, txtSvrGftIP2_02, txtSvrGftIP2_03, txtSvrGftIP2_04);
                        ConfigData.Current.AppConfig.PosComm.ComGPort2 = !appChange.bComGPort2 ? ConfigData.Current.AppConfig.PosComm.ComGPort2 : txtComGPort2.Text;
                        ConfigData.Current.AppConfig.PosComm.ComGTimeOut = !appChange.bComGTimeOut ? ConfigData.Current.AppConfig.PosComm.ComGTimeOut : txtComGTimeOut.Text;

                        ConfigData.Current.AppConfig.PosComm.SvrPntIP1 = !appChange.bSvrPntIP1 ? ConfigData.Current.AppConfig.PosComm.SvrPntIP1 : GetTextIp(txtSvrPntIP1_01, txtSvrPntIP1_02, txtSvrPntIP1_03, txtSvrPntIP1_04);
                        ConfigData.Current.AppConfig.PosComm.ComPPort1 = !appChange.bComPPort1 ? ConfigData.Current.AppConfig.PosComm.ComPPort1 : txtComPPort1.Text;
                        ConfigData.Current.AppConfig.PosComm.SvrPntIP2 = !appChange.bSvrPntIP2 ? ConfigData.Current.AppConfig.PosComm.SvrPntIP2 : GetTextIp(txtSvrPntIP2_01, txtSvrPntIP2_02, txtSvrPntIP2_03, txtSvrPntIP2_04);
                        ConfigData.Current.AppConfig.PosComm.ComPPort2 = !appChange.bComPPort2 ? ConfigData.Current.AppConfig.PosComm.ComPPort2 : txtComPPort2.Text;
                        ConfigData.Current.AppConfig.PosComm.ComPTimeOut = !appChange.bComPTimeOut ? ConfigData.Current.AppConfig.PosComm.ComPTimeOut : txtComPTimeOut.Text;

                        ConfigData.Current.AppConfig.PosComm.HqSvrIP1 = !appChange.bHqSvrIP1 ? ConfigData.Current.AppConfig.PosComm.HqSvrIP1 : GetTextIp(txtHqSvrIP1_01, txtHqSvrIP1_02, txtHqSvrIP1_03, txtHqSvrIP1_04);
                        ConfigData.Current.AppConfig.PosComm.HqComQPort1 = !appChange.bHqComQPort1 ? ConfigData.Current.AppConfig.PosComm.HqComQPort1 : txtHqComQPort1.Text;
                        ConfigData.Current.AppConfig.PosComm.HqComUPort1 = !appChange.bHqComUPort1 ? ConfigData.Current.AppConfig.PosComm.HqComUPort1 : txtHqComUPort1.Text;
                        ConfigData.Current.AppConfig.PosComm.HqSvrIP2 = !appChange.bHqSvrIP2 ? ConfigData.Current.AppConfig.PosComm.HqSvrIP2 : GetTextIp(txtHqSvrIP2_01, txtHqSvrIP2_02, txtHqSvrIP2_03, txtHqSvrIP2_04);
                        ConfigData.Current.AppConfig.PosComm.HqComQPort2 = !appChange.bHqComQPort2 ? ConfigData.Current.AppConfig.PosComm.HqComQPort2 : txtHqComQPort2.Text;
                        ConfigData.Current.AppConfig.PosComm.HqComUPort2 = !appChange.bHqComUPort2 ? ConfigData.Current.AppConfig.PosComm.HqComUPort2 : txtHqComUPort2.Text;
                        ConfigData.Current.AppConfig.PosComm.HqComQTimeOut = !appChange.bHqComQTimeOut ? ConfigData.Current.AppConfig.PosComm.HqComQTimeOut : txtHqComQTimeOut.Text;
                        ConfigData.Current.AppConfig.PosComm.HqComUTimeOut = !appChange.bHqComUTimeOut ? ConfigData.Current.AppConfig.PosComm.HqComUTimeOut : txtHqComUTimeOut.Text;

                        #endregion
                    }
                    else if (tc.TabPages[iTabIndex].Name.ToString() == "tpPosFTP")
                    {
                        #region PosFTP

                        ConfigData.Current.AppConfig.PosFTP.FtpSvrIP1 = !appChange.bFtpSvrIP1 ? ConfigData.Current.AppConfig.PosFTP.FtpSvrIP1 : GetTextIp(txtFtpSvrIP1_01, txtFtpSvrIP1_02, txtFtpSvrIP1_03, txtFtpSvrIP1_04);
                        ConfigData.Current.AppConfig.PosFTP.FtpComPort1 = !appChange.bFtpComPort1 ? ConfigData.Current.AppConfig.PosFTP.FtpComPort1 : txtFtpComPort1.Text;
                        ConfigData.Current.AppConfig.PosFTP.FtpSvrIP2 = !appChange.bFtpSvrIP2 ? ConfigData.Current.AppConfig.PosFTP.FtpSvrIP2 : GetTextIp(txtFtpSvrIP2_01, txtFtpSvrIP2_02, txtFtpSvrIP2_03, txtFtpSvrIP2_04);
                        ConfigData.Current.AppConfig.PosFTP.FtpComPort2 = !appChange.bFtpComPort2 ? ConfigData.Current.AppConfig.PosFTP.FtpComPort2 : txtFtpComPort2.Text;
                        ConfigData.Current.AppConfig.PosFTP.Mode = !appChange.bMode ? ConfigData.Current.AppConfig.PosFTP.Mode : txtMode.Text;
                        ConfigData.Current.AppConfig.PosFTP.User = !appChange.bUser ? ConfigData.Current.AppConfig.PosFTP.User : txtUser.Text;
                        ConfigData.Current.AppConfig.PosFTP.Pass = !appChange.bPass ? ConfigData.Current.AppConfig.PosFTP.Pass : txtPass.Text;
                        ConfigData.Current.AppConfig.PosFTP.JournalPath = !appChange.bJournalPath ? ConfigData.Current.AppConfig.PosFTP.JournalPath : txtJournalPath.Text;
                        ConfigData.Current.AppConfig.PosFTP.DataFileDownloadPath = !appChange.bDataFileDownloadPath ? ConfigData.Current.AppConfig.PosFTP.DataFileDownloadPath : txtDataFileDownloadPath.Text;
                        ConfigData.Current.AppConfig.PosFTP.VersionInfoPath = !appChange.bVersionInfoPath ? ConfigData.Current.AppConfig.PosFTP.VersionInfoPath : txtVersionInfoPath.Text;
                        ConfigData.Current.AppConfig.PosFTP.CreateUploadPathByDate = !appChange.bCreateUploadPathByDate ? ConfigData.Current.AppConfig.PosFTP.CreateUploadPathByDate : txtCreateUploadPathByDate.Text;

                        #endregion
                    }
                    else if (tc.TabPages[iTabIndex].Name.ToString() == "tpPosVan")
                    {
                        #region PosVan

                        ConfigData.Current.AppConfig.PosVan.VanSvrIP1 = !appChange.bVanSvrIP1 ? ConfigData.Current.AppConfig.PosVan.VanSvrIP1 : GetTextIp(txtVanSvrIP1_01, txtVanSvrIP1_02, txtVanSvrIP1_03, txtVanSvrIP1_04);
                        ConfigData.Current.AppConfig.PosVan.VanComPort1 = !appChange.bVanComPort1 ? ConfigData.Current.AppConfig.PosVan.VanComPort1 : txtVanComPort1.Text;
                        ConfigData.Current.AppConfig.PosVan.VanSvrIP2 = !appChange.bVanSvrIP2 ? ConfigData.Current.AppConfig.PosVan.VanSvrIP2 : GetTextIp(txtVanSvrIP2_01, txtVanSvrIP2_02, txtVanSvrIP2_03, txtVanSvrIP2_04);
                        ConfigData.Current.AppConfig.PosVan.VanComPort2 = !appChange.bVanComPort2 ? ConfigData.Current.AppConfig.PosVan.VanComPort2 : txtVanComPort2.Text;
                        ConfigData.Current.AppConfig.PosVan.ComTimeOut = !appChange.bComTimeOut ? ConfigData.Current.AppConfig.PosVan.ComTimeOut : txtComTimeOut.Text;

                        #endregion
                    }
                    else if (tc.TabPages[iTabIndex].Name.ToString() == "tpPosOption")
                    {
                        #region PosOption

                        ConfigData.Current.AppConfig.PosOption.PointUse = !appChange.bPointUse ? ConfigData.Current.AppConfig.PosOption.PointUse : txtPointUse.Text;
                        ConfigData.Current.AppConfig.PosOption.PointSchemePrefix = !appChange.bPointSchemePrefix ? ConfigData.Current.AppConfig.PosOption.PointSchemePrefix : txtPointSchemePrefix.Text;
                        ConfigData.Current.AppConfig.PosOption.PointPayKeyInputEnable = !appChange.bPointPayKeyInputEnable ? ConfigData.Current.AppConfig.PosOption.PointPayKeyInputEnable : txtPointPayKeyInputEnable.Text;
                        ConfigData.Current.AppConfig.PosOption.CashReceiptUse = !appChange.bCashReceiptUse ? ConfigData.Current.AppConfig.PosOption.CashReceiptUse : txtCashReceiptUse.Text;
                        ConfigData.Current.AppConfig.PosOption.CashReceiptIssue = !appChange.bCashReceiptIssue ? ConfigData.Current.AppConfig.PosOption.CashReceiptIssue : txtCashReceiptIssue.Text;
                        ConfigData.Current.AppConfig.PosOption.CashReceiptApplAmount = !appChange.bCashReceiptApplAmount ? ConfigData.Current.AppConfig.PosOption.CashReceiptApplAmount : txtCashReceiptApplAmount.Text;
                        ConfigData.Current.AppConfig.PosOption.GoodsCodePrefix1 = !appChange.bGoodsCodePrefix1 ? ConfigData.Current.AppConfig.PosOption.GoodsCodePrefix1 : txtGoodsCodePrefix1.Text;
                        ConfigData.Current.AppConfig.PosOption.GoodsCodePrefix2 = !appChange.bGoodsCodePrefix2 ? ConfigData.Current.AppConfig.PosOption.GoodsCodePrefix2 : txtGoodsCodePrefix2.Text;
                        ConfigData.Current.AppConfig.PosOption.DataKeepDays = !appChange.bDataKeepDays ? ConfigData.Current.AppConfig.PosOption.DataKeepDays : txtDataKeepDays.Text;
                        ConfigData.Current.AppConfig.PosOption.SalesReturn = !appChange.bSalesReturn ? ConfigData.Current.AppConfig.PosOption.SalesReturn : txtSalesReturn.Text;
                        ConfigData.Current.AppConfig.PosOption.SignUploadTask = !appChange.bSignUploadTask ? ConfigData.Current.AppConfig.PosOption.SignUploadTask : txtSignUploadTask.Text;
                        ConfigData.Current.AppConfig.PosOption.TransUploadTask = !appChange.bTransUploadTask ? ConfigData.Current.AppConfig.PosOption.TransUploadTask : txtTransUploadTask.Text;
                        ConfigData.Current.AppConfig.PosOption.TransStatusTask = !appChange.bTransStatusTask ? ConfigData.Current.AppConfig.PosOption.TransStatusTask : txtTransStatusTask.Text;
                        ConfigData.Current.AppConfig.PosOption.NoticeStatusTask = !appChange.bNoticeStatusTask ? ConfigData.Current.AppConfig.PosOption.NoticeStatusTask : txtNoticeStatusTask.Text;

                        #endregion
                    }

                    //저장
                    ConfigData.Current.AppConfig.Save();
                    AppConfig.Load();

                    //컨트롤 초기화
                    SetInitControl();
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                SetControlDisable(false);
            }
        }

        /// <summary>
        /// 닫기 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;

            if (ChkData())
            {
                if (ShowMessageBox(MessageDialogType.Question, null, strMsg00, strBtnNm) == DialogResult.Yes)
                {
                    //저장
                    btnSave_Click(btnSave, null);
                }
            }

            this.Close();
        }

        #endregion

        #region 사용자 정의

        bool ChkData()
        {
            bool retData = false;

            if (tc.TabPages[iTabIndex].Name.ToString() == "tpPosInfo")
            {
                #region PosInfo

                if (ConfigData.Current.AppConfig.PosInfo.StoreNo != txtStoreNo.Text)
                {
                    appChange.bStoreNo = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosInfo.StoreType != txtStoreType.Text)
                {
                    appChange.bStoreType = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosInfo.PosNo != txtPosNo.Text)
                {
                    appChange.bPosNo = true;
                    retData = true;
                }

                #endregion
            }
            else if (tc.TabPages[iTabIndex].Name.ToString() == "tpPosComm01")
            {
                #region PosComm1

                if (ConfigData.Current.AppConfig.PosComm.SvrIP1 != GetTextIp(txtSvrIP1_01, txtSvrIP1_02, txtSvrIP1_03, txtSvrIP1_04))
                {
                    appChange.bSvrIP1 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.ComDPort1 != txtComDPort1.Text)
                {
                    appChange.bComDPort1 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.ComUPort1 != txtComUPort1.Text)
                {
                    appChange.bComUPort1 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.ComQPort1 != txtComQPort1.Text)
                {
                    appChange.bComQPort1 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.SvrIP2 != GetTextIp(txtSvrIP2_01, txtSvrIP2_02, txtSvrIP2_03, txtSvrIP2_04))
                {
                    appChange.bSvrIP2 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.ComDPort2 != txtComDPort2.Text)
                {
                    appChange.bComDPort2 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.ComUPort2 != txtComUPort2.Text)
                {
                    appChange.bComUPort2 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.ComQPort2 != txtComQPort2.Text)
                {
                    appChange.bComQPort2 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.ComDTimeOut != txtComDTimeOut.Text)
                {
                    appChange.bComDTimeOut = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.ComUTimeOut != txtComUTimeOut.Text)
                {
                    appChange.bComUTimeOut = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.ComQTimeOut != txtComQTimeOut.Text)
                {
                    appChange.bComQTimeOut = true;
                    retData = true;
                }
                #endregion
            }
            else if (tc.TabPages[iTabIndex].Name.ToString() == "tpPosComm02")
            {
                #region PosComm2

                if (ConfigData.Current.AppConfig.PosComm.SvrGftIP1 != GetTextIp(txtSvrGftIP1_01, txtSvrGftIP1_02, txtSvrGftIP1_03, txtSvrGftIP1_04))
                {
                    appChange.bSvrGftIP1 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.ComGPort1 != txtComGPort1.Text)
                {
                    appChange.bComGPort1 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.SvrGftIP2 != GetTextIp(txtSvrGftIP2_01, txtSvrGftIP2_02, txtSvrGftIP2_03, txtSvrGftIP2_04))
                {
                    appChange.bSvrGftIP2 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.ComGPort2 != txtComGPort2.Text)
                {
                    appChange.bComGPort2 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.ComGTimeOut != txtComGTimeOut.Text)
                {
                    appChange.bComGTimeOut = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.SvrPntIP1 != GetTextIp(txtSvrPntIP1_01, txtSvrPntIP1_02, txtSvrPntIP1_03, txtSvrPntIP1_04))
                {
                    appChange.bSvrPntIP1 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.ComPPort1 != txtComPPort1.Text)
                {
                    appChange.bComPPort1 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.SvrPntIP2 != GetTextIp(txtSvrPntIP2_01, txtSvrPntIP2_02, txtSvrPntIP2_03, txtSvrPntIP2_04))
                {
                    appChange.bSvrPntIP2 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.ComPPort2 != txtComPPort2.Text)
                {
                    appChange.bComPPort2 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.ComPTimeOut != txtComPTimeOut.Text)
                {
                    appChange.bComPTimeOut = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.HqSvrIP1 != GetTextIp(txtHqSvrIP1_01, txtHqSvrIP1_02, txtHqSvrIP1_03, txtHqSvrIP1_04))
                {
                    appChange.bHqSvrIP1 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.HqComQPort1 != txtHqComQPort1.Text)
                {
                    appChange.bHqComQPort1 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.HqComUPort1 != txtHqComUPort1.Text)
                {
                    appChange.bHqComUPort1 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.HqSvrIP2 != GetTextIp(txtHqSvrIP2_01, txtHqSvrIP2_02, txtHqSvrIP2_03, txtHqSvrIP2_04))
                {
                    appChange.bHqSvrIP2 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.HqComQPort2 != txtHqComQPort2.Text)
                {
                    appChange.bHqComQPort2 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.HqComUPort2 != txtHqComUPort2.Text)
                {
                    appChange.bHqComUPort2 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.HqComQTimeOut != txtHqComQTimeOut.Text)
                {
                    appChange.bHqComQTimeOut = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosComm.HqComUTimeOut != txtHqComUTimeOut.Text)
                {
                    appChange.bHqComUTimeOut = true;
                    retData = true;
                }

                #endregion
            }
            else if (tc.TabPages[iTabIndex].Name.ToString() == "tpPosFTP")
            {
                #region PosFTP

                if (ConfigData.Current.AppConfig.PosFTP.FtpSvrIP1 != GetTextIp(txtFtpSvrIP1_01, txtFtpSvrIP1_02, txtFtpSvrIP1_03, txtFtpSvrIP1_04))
                {
                    appChange.bFtpSvrIP1 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosFTP.FtpComPort1 != txtFtpComPort1.Text)
                {
                    appChange.bFtpComPort1 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosFTP.FtpSvrIP2 != GetTextIp(txtFtpSvrIP2_01, txtFtpSvrIP2_02, txtFtpSvrIP2_03, txtFtpSvrIP2_04))
                {
                    appChange.bFtpSvrIP2 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosFTP.FtpComPort2 != txtFtpComPort2.Text)
                {
                    appChange.bFtpComPort2 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosFTP.Mode != txtMode.Text)
                {
                    appChange.bMode = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosFTP.User != txtUser.Text)
                {
                    appChange.bUser = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosFTP.Pass != txtPass.Text)
                {
                    appChange.bPass = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosFTP.JournalPath != txtJournalPath.Text)
                {
                    appChange.bJournalPath = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosFTP.DataFileDownloadPath != txtDataFileDownloadPath.Text)
                {
                    appChange.bDataFileDownloadPath = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosFTP.VersionInfoPath != txtVersionInfoPath.Text)
                {
                    appChange.bVersionInfoPath = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosFTP.CreateUploadPathByDate != txtCreateUploadPathByDate.Text)
                {
                    appChange.bCreateUploadPathByDate = true;
                    retData = true;
                }

                #endregion
            }
            else if (tc.TabPages[iTabIndex].Name.ToString() == "tpPosVan")
            {
                #region PosVan

                if (ConfigData.Current.AppConfig.PosVan.VanSvrIP1 != GetTextIp(txtVanSvrIP1_01, txtVanSvrIP1_02, txtVanSvrIP1_03, txtVanSvrIP1_04))
                {
                    appChange.bVanSvrIP1 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosVan.VanComPort1 != txtVanComPort1.Text)
                {
                    appChange.bVanComPort1 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosVan.VanSvrIP2 != GetTextIp(txtVanSvrIP2_01, txtVanSvrIP2_02, txtVanSvrIP2_03, txtVanSvrIP2_04))
                {
                    appChange.bVanSvrIP2 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosVan.VanComPort2 != txtVanComPort2.Text)
                {
                    appChange.bVanComPort2 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosVan.ComTimeOut != txtComTimeOut.Text)
                {
                    appChange.bComTimeOut = true;
                    retData = true;
                }

                #endregion
            }
            else if (tc.TabPages[iTabIndex].Name.ToString() == "tpPosOption")
            {
                #region PosOption

                if (ConfigData.Current.AppConfig.PosOption.PointUse != txtPointUse.Text)
                {
                    appChange.bPointUse = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosOption.PointSchemePrefix != txtPointSchemePrefix.Text)
                {
                    appChange.bPointSchemePrefix = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosOption.PointPayKeyInputEnable != txtPointPayKeyInputEnable.Text)
                {
                    appChange.bPointPayKeyInputEnable = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosOption.CashReceiptUse != txtCashReceiptUse.Text)
                {
                    appChange.bCashReceiptUse = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosOption.CashReceiptIssue != txtCashReceiptIssue.Text)
                {
                    appChange.bCashReceiptIssue = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosOption.CashReceiptApplAmount != txtCashReceiptApplAmount.Text)
                {
                    appChange.bCashReceiptApplAmount = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosOption.GoodsCodePrefix1 != txtGoodsCodePrefix1.Text)
                {
                    appChange.bGoodsCodePrefix1 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosOption.GoodsCodePrefix2 != txtGoodsCodePrefix2.Text)
                {
                    appChange.bGoodsCodePrefix2 = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosOption.DataKeepDays != txtDataKeepDays.Text)
                {
                    appChange.bDataKeepDays = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosOption.SalesReturn != txtSalesReturn.Text)
                {
                    appChange.bSalesReturn = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosOption.SignUploadTask != txtSignUploadTask.Text)
                {
                    appChange.bSignUploadTask = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosOption.TransUploadTask != txtTransUploadTask.Text)
                {
                    appChange.bTransUploadTask = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosOption.TransStatusTask != txtTransStatusTask.Text)
                {
                    appChange.bTransStatusTask = true;
                    retData = true;
                }

                if (ConfigData.Current.AppConfig.PosOption.NoticeStatusTask != txtNoticeStatusTask.Text)
                {
                    appChange.bNoticeStatusTask = true;
                    retData = true;
                }

                #endregion
            }

            return retData;
        }

        /// <summary>
        /// 텍스트박스 IP 셋팅
        /// </summary>
        /// <param name="txt01"></param>
        /// <param name="txt02"></param>
        /// <param name="txt03"></param>
        /// <param name="txt04"></param>
        /// <param name="strIP"></param>
        void SetTextIP(InputText txt01, InputText txt02, InputText txt03, InputText txt04, string strIP)
        {
            string[] arrTempIP = strIP.Split('.');

            if (arrTempIP != null && arrTempIP.Length >= 4)
            {
                bool bInput = true;

                foreach (string strTemp in arrTempIP)
                {
                    if (strTemp.Length <= 0 || strTemp.Length > 3)
                    {
                        bInput = false;
                        break;
                    }
                }

                if (bInput)
                {
                    txt01.Text = arrTempIP[0];
                    txt02.Text = arrTempIP[1];
                    txt03.Text = arrTempIP[2];
                    txt04.Text = arrTempIP[3];
                }
            }
        }

        /// <summary>
        /// 텍스트박스 IP 반환
        /// </summary>
        /// <param name="txt01"></param>
        /// <param name="txt02"></param>
        /// <param name="txt03"></param>
        /// <param name="txt04"></param>
        /// <returns></returns>
        string GetTextIp(InputText txt01, InputText txt02, InputText txt03, InputText txt04)
        {
            string strReturn = string.Empty;

            if (txt01.Text.Length > 0 && txt02.Text.Length > 0 && txt03.Text.Length > 0 && txt04.Text.Length > 0)
            {
                strReturn = string.Format("{0}.{1}.{2}.{3}", txt01.Text, txt02.Text, txt03.Text, txt04.Text);
            }

            return strReturn;
        }

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable/Disable
        /// </summary>
        void SetControlDisable(bool bDisable)
        {
            ChildManager.ShowProgress(bDisable);
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
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "tabcontrol")
                        {
                            TabControl tab = (TabControl)item;
                            tab.Enabled = !_bDisable;
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
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "tabcontrol")
                    {
                        TabControl tab = (TabControl)item;
                        tab.Enabled = !_bDisable;
                    }
                }
            }

            //Application.DoEvents();
        }

        #endregion
    }

    public class AppChange
    {
        /// <summary>
        /// 
        /// </summary>
        public bool bStoreNo = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bStoreType = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bPosNo = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bSvrIP1 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bComDPort1 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bComUPort1 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bComQPort1 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bSvrIP2 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bComDPort2 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bComUPort2 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bComQPort2 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bComDTimeOut = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bComUTimeOut = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bComQTimeOut = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bSvrGftIP1 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bComGPort1 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bSvrGftIP2 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bComGPort2 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bComGTimeOut = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bSvrPntIP1 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bComPPort1 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bSvrPntIP2 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bComPPort2 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bComPTimeOut = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bHqSvrIP1 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bHqComQPort1 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bHqComUPort1 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bHqSvrIP2 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bHqComQPort2 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bHqComUPort2 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bHqComQTimeOut = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bHqComUTimeOut = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bFtpSvrIP1 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bFtpComPort1 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bFtpSvrIP2 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bFtpComPort2 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bMode = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bUser = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bPass = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bJournalPath = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bDataFileDownloadPath = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bVersionInfoPath = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bCreateUploadPathByDate = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bVanSvrIP1 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bVanComPort1 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bVanSvrIP2 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bVanComPort2 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bComTimeOut = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bPointUse = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bPointSchemePrefix = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bPointPayKeyInputEnable = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bCashReceiptUse = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bCashReceiptIssue = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bCashReceiptApplAmount = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bGoodsCodePrefix1 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bGoodsCodePrefix2 = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bDataKeepDays = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bAutoReturnCardRead = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bSalesReturn = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bSignUploadTask = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bTransUploadTask = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bTransStatusTask = false;
        /// <summary>
        /// 
        /// </summary>
        public bool bNoticeStatusTask = false;
    }
}
