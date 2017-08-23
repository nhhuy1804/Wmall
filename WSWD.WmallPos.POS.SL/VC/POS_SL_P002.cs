//-----------------------------------------------------------------
/*
 * 화면명   : POS_SL_P002.cs
 * 화면설명 : 가격조회
 * 개발자   : 정광호
 * 개발일자 : 2015.04.28
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

using WSWD.WmallPos.POS.SL.PI;
using WSWD.WmallPos.POS.SL.PT;
using WSWD.WmallPos.POS.SL.VI;
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
using WSWD.WmallPos.FX.NetComm.Tasks.PQ;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PQ;
using WSWD.WmallPos.FX.Shared.NetComm;
using System.Diagnostics;

namespace WSWD.WmallPos.POS.SL.VC
{
    public partial class POS_SL_P002 : PopupBase, ISLP002View
    {
        #region 변수

        //비즈니스 로직
        private ISLP002presenter m_Presenter;

        /// <summary>
        /// 화면구분(true : 단품상품 가격조회 화면, false : 품번상품 가격조회 화면)
        /// </summary>
        private bool bType = true;
        /// <summary>
        /// 스캔 여부
        /// </summary>
        private bool _bScan = false;
        /// <summary>
        /// 입력값 완료여부
        /// </summary>
        private bool _bComplete = false;

        /// <summary>
        /// 공통코드 데이터(FG_TAX:BS14, CD_DP:BS21)
        /// </summary>
        private DataSet dsCode = null;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        #endregion

        #region 생성자

        public POS_SL_P002()
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
            this.FormClosed += new FormClosedEventHandler(form_FormClosed);
            this.KeyEvent += new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(form_KeyEvent);        //Key Event
            this.btnClose.Click += new EventHandler(btnClose_Click);                                //닫기 button Event

            if (POSDeviceManager.Scanner != null)
            {
                POSDeviceManager.Scanner.DataEvent += new POSDataEventHandler(Scanner_DataEvent);   //Scanner Event    
            }
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

            //가격조회------------------------------
            m_Presenter = new SLP002presenter(this);
            m_Presenter.GetCode();
            //--------------------------------------

            //컨트롤 초기화(첫 시작은 단품 상품 가격조회 화면)
            InitControl("", true);

            //메세지
            msgBar.Text = strMSG01;

            //상품코드 포커스
            txtCD_ITEM.SetFocus();
        }

        void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Load -= new EventHandler(form_Load);
            this.FormClosed -= new FormClosedEventHandler(form_FormClosed);
            this.KeyEvent -= new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(form_KeyEvent);        //Key Event
            this.btnClose.Click -= new EventHandler(btnClose_Click);                                //닫기 button Event

            if (POSDeviceManager.Scanner != null)
            {
                POSDeviceManager.Scanner.DataEvent -= new POSDataEventHandler(Scanner_DataEvent);               //Scanner Event    
            }
        }

        /// <summary>
        /// Key Event
        /// </summary>
        /// <param name="e"></param>
        void form_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (_bDisable) return;

            if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_ENTER)
            {
                if (txtCD_ITEM.IsFocused)
                {
                    //화면 설정
                    SetInit();
                }
            }
            else if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_PLU)
            {
                if (txtCD_ITEM.IsFocused)
                {
                    //화면 설정
                    SetInit();
                }
            }
            else if (!e.IsControlKey)
            {
                if (_bComplete)
                {
                    _bComplete = false;

                    InitControl(txtCD_ITEM.Text.ToString(), true);
                }
                else
                {
                    //string strTemp = msgBar.Tag != null ? msgBar.Tag.ToString() : "";

                    //if (strTemp.Length > 0)
                    //{
                    //    msgBar.Text = strTemp;
                    //}
                }
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

            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Scanner Event
        /// </summary>
        /// <param name="msrData"></param>
        void Scanner_DataEvent(string msrData)
        {
            Trace.WriteLine("SL_P002_Scanner_DataEvent " + msrData, "program");

            if (_bDisable) return;

            if (msrData.Length > 0)
            {
                _bScan = true;

                txtCD_ITEM.Text = msrData;

                if (_bComplete)
                {
                    _bComplete = false;

                    InitControl(txtCD_ITEM.Text.ToString(), true);
                }

                //화면 설정
                SetInit();
            }
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// 컨트롤 초기화
        /// </summary>
        /// <param name="strValue">초기화 입력값</param>
        /// <param name="bInitTextBox">텍스트박스 컨트롤 입력값 초기화여부(true:입력값만 초기화, false:입력값 초기화 및 위치변경</param>
        private void InitControl(string strValue, bool bInitTextBox)
        {
            if (bInitTextBox)
            {
                txtBarcode01.Text = "";
                txtBarcode02.Text = "";
                txtCD_ITEM.Text = strValue;
                txtScanCode.Text = "";
                txtNM_ITEM.Text = "";
                txtCD_CLASS.Text = "";
                txtNM_CLASS.Text = "";
                txtCD_CTGY.Text = "";
                txtNM_CTGY.Text = "";
                txtFG_TAX.Text = "";
                txtCD_DP.Text = "";
                txtAmt.Text = "";
                txtFG_CLASS.Text = "";
                txtCD_DEPT.Text = "";
            }
            else
            {
                //Label X 
                int iLblX = 13;

                //Textbox X
                int iTxtX = 123;

                //Label & TextBox Y Point
                int iPointY = 13;

                //1단바코드
                txtBarcode01.Text = "";
                txtBarcode01.Visible = true;
                txtBarcode01.Location = new Point(iTxtX, iPointY);
                lblBarcode01.Visible = true;
                lblBarcode01.Location = new Point(iLblX, iPointY);
                iPointY += 36;

                //2단바코드
                txtBarcode02.Text = "";
                txtBarcode02.Visible = true;
                txtBarcode02.Location = new Point(iTxtX, iPointY);
                lblBarcode02.Visible = true;
                lblBarcode02.Location = new Point(iLblX, iPointY);
                iPointY += 36;

                //상품코드
                txtCD_ITEM.Text = strValue;

                if (bType)
                {
                    #region 단품상품가격조회화면

                    //SCAN CODE
                    txtScanCode.Text = "";
                    txtScanCode.Visible = true;
                    txtScanCode.Location = new Point(iTxtX, iPointY);
                    lblScanCode.Visible = true;
                    lblScanCode.Location = new Point(iLblX, iPointY);
                    iPointY += 36;

                    //상품명
                    txtNM_ITEM.Text = "";
                    txtNM_ITEM.Visible = true;
                    txtNM_ITEM.Location = new Point(iTxtX, iPointY);
                    lblNM_ITEM.Visible = true;
                    lblNM_ITEM.Location = new Point(iLblX, iPointY);
                    iPointY += 36;

                    //품번코드
                    txtCD_CLASS.Text = "";
                    txtCD_CLASS.Visible = true;
                    txtCD_CLASS.Location = new Point(iTxtX, iPointY);
                    lblCD_CLASS.Visible = true;
                    lblCD_CLASS.Location = new Point(iLblX, iPointY);
                    iPointY += 36;

                    //품번명
                    txtNM_CLASS.Text = "";
                    txtNM_CLASS.Visible = true;
                    txtNM_CLASS.Location = new Point(iTxtX, iPointY);
                    lblNM_CLASS.Visible = true;
                    lblNM_CLASS.Location = new Point(iLblX, iPointY);
                    iPointY += 36;

                    //품목코드
                    txtCD_CTGY.Text = "";
                    txtCD_CTGY.Visible = true;
                    txtCD_CTGY.Location = new Point(iTxtX, iPointY);
                    lblCD_CTGY.Visible = true;
                    lblCD_CTGY.Location = new Point(iLblX, iPointY);
                    iPointY += 36;

                    //품목명
                    txtNM_CTGY.Text = "";
                    txtNM_CTGY.Visible = true;
                    txtNM_CTGY.Location = new Point(iTxtX, iPointY);
                    lblNM_CTGY.Visible = true;
                    lblNM_CTGY.Location = new Point(iLblX, iPointY);
                    iPointY += 36;

                    //세구분
                    txtFG_TAX.Text = "";
                    txtFG_TAX.Visible = true;
                    txtFG_TAX.Location = new Point(iTxtX, iPointY);
                    lblFG_TAX.Visible = true;
                    lblFG_TAX.Location = new Point(iLblX, iPointY);
                    iPointY += 36;

                    //단품구분
                    txtCD_DP.Text = "";
                    txtCD_DP.Visible = true;
                    txtCD_DP.Location = new Point(iTxtX, iPointY);
                    lblCD_DP.Visible = true;
                    lblCD_DP.Location = new Point(iLblX, iPointY);
                    iPointY += 36;

                    //가격
                    txtAmt.Text = "";
                    txtAmt.Visible = true;
                    txtAmt.Location = new Point(iTxtX, iPointY);
                    lblAmt.Visible = true;
                    lblAmt.Location = new Point(iLblX, iPointY);

                    //Visible False-----------------------------------------
                    //상품구분
                    txtFG_CLASS.Text = "";
                    txtFG_CLASS.Visible = false;
                    txtFG_CLASS.Location = new Point(0, 0);
                    lblFG_CLASS.Visible = false;
                    lblFG_CLASS.Location = new Point(0, 0);

                    //코너코드
                    txtCD_DEPT.Text = "";
                    txtCD_DEPT.Visible = false;
                    txtCD_DEPT.Location = new Point(0, 0);
                    lblCD_DEPT.Visible = false;
                    lblCD_DEPT.Location = new Point(0, 0);
                    //Visible False-----------------------------------------

                    #endregion
                }
                else
                {
                    #region 품번상품가격조회화면

                    //품번코드
                    txtCD_CLASS.Text = "";
                    txtCD_CLASS.Visible = true;
                    txtCD_CLASS.Location = new Point(iTxtX, iPointY);
                    lblCD_CLASS.Visible = true;
                    lblCD_CLASS.Location = new Point(iLblX, iPointY);
                    iPointY += 36;

                    //품번명
                    txtNM_CLASS.Text = "";
                    txtNM_CLASS.Visible = true;
                    txtNM_CLASS.Location = new Point(iTxtX, iPointY);
                    lblNM_CLASS.Visible = true;
                    lblNM_CLASS.Location = new Point(iLblX, iPointY);
                    iPointY += 36;

                    //품목코드
                    txtCD_CTGY.Text = "";
                    txtCD_CTGY.Visible = true;
                    txtCD_CTGY.Location = new Point(iTxtX, iPointY);
                    lblCD_CTGY.Visible = true;
                    lblCD_CTGY.Location = new Point(iLblX, iPointY);
                    iPointY += 36;

                    //품목명
                    txtNM_CTGY.Text = "";
                    txtNM_CTGY.Visible = true;
                    txtNM_CTGY.Location = new Point(iTxtX, iPointY);
                    lblNM_CTGY.Visible = true;
                    lblNM_CTGY.Location = new Point(iLblX, iPointY);
                    iPointY += 36;

                    //상품구분
                    txtFG_CLASS.Text = "";
                    txtFG_CLASS.Visible = true;
                    txtFG_CLASS.Location = new Point(iTxtX, iPointY);
                    lblFG_CLASS.Visible = true;
                    lblFG_CLASS.Location = new Point(iLblX, iPointY);
                    iPointY += 36;

                    //코너코드
                    txtCD_DEPT.Text = "";
                    txtCD_DEPT.Visible = true;
                    txtCD_DEPT.Location = new Point(iTxtX, iPointY);
                    lblCD_DEPT.Visible = true;
                    lblCD_DEPT.Location = new Point(iLblX, iPointY);
                    iPointY += 36;

                    //세구분
                    txtFG_TAX.Text = "";
                    txtFG_TAX.Visible = true;
                    txtFG_TAX.Location = new Point(iTxtX, iPointY);
                    lblFG_TAX.Visible = true;
                    lblFG_TAX.Location = new Point(iLblX, iPointY);
                    iPointY += 36;

                    //가격
                    txtAmt.Text = "";
                    txtAmt.Visible = true;
                    txtAmt.Location = new Point(iTxtX, iPointY);
                    lblAmt.Visible = true;
                    lblAmt.Location = new Point(iLblX, iPointY);

                    //Visible False-----------------------------------------

                    //SCAN CODE
                    txtScanCode.Text = "";
                    txtScanCode.Visible = false;
                    txtScanCode.Location = new Point(0, 0);
                    lblScanCode.Visible = false;
                    lblScanCode.Location = new Point(0, 0);

                    //상품명
                    txtNM_ITEM.Text = "";
                    txtNM_ITEM.Visible = false;
                    txtNM_ITEM.Location = new Point(0, 0);
                    lblNM_ITEM.Visible = false;
                    lblNM_ITEM.Location = new Point(0, 0);

                    //단품구분
                    txtCD_DP.Text = "";
                    txtCD_DP.Visible = false;
                    txtCD_DP.Location = new Point(0, 0);
                    lblCD_DP.Visible = false;
                    lblCD_DP.Location = new Point(0, 0);
                    iPointY += 36;

                    //Visible False-----------------------------------------

                    #endregion
                }
            }
        }

        /// <summary>
        /// 화면 설정
        /// </summary>
        private void SetInit()
        {
            if (_bDisable) return;

            if (_bScan)
            {
                if (txtCD_ITEM.Text.Length == 13)
                {
                    if (txtCD_ITEM.Text.Substring(0,2) == "22" || txtCD_ITEM.Text.Substring(0,2) == "29")
                    {
                        if (bType)
                        {
                            if (txtCD_ITEM.Text.Substring(0,2) == "29")
                            {
                                _bScan = false;
                                txtCD_ITEM.Text = "";
                                msgBar.Tag = msgBar.Text.ToString();
                                msgBar.Text = strMSG07;
                            }
                            else
                            {
                                //품번(PQ06) DB조회
                                GetServerPQ("DB", "PQ06", txtCD_ITEM.Text.ToString().Substring(2,6));
                            }
                        }
                        else
                        {
                            if (txtCD_ITEM.Text.Substring(0,2) == "22")
                            {
                                GetServerPQ("DB", "PQ06", txtCD_ITEM.Text.ToString().Substring(2, 6));
                            }
                            else
                            {
                                if (txtBarcode01.Text.Length <= 0)
                                {
                                    _bScan = false;
                                    txtCD_ITEM.Text = "";
                                    msgBar.Tag = msgBar.Text.ToString();
                                    msgBar.Text = strMSG07;
                                }
                                else
                                {
                                    _bComplete = true;
                                    txtBarcode02.Text = txtCD_ITEM.Text.ToString();
                                    txtCD_ITEM.Text = "";
                                    msgBar.Text = strMSG01;
                                }
                            }
                        }
                    }
                    else
                    {
                        //단품(PQ05) DB조회
                        GetServerPQ("DB", "PQ05", txtCD_ITEM.Text.ToString());
                    }
                }
                else
                {
                    _bScan = false;
                    txtCD_ITEM.Text = "";
                    msgBar.Tag = msgBar.Text.ToString();
                    msgBar.Text = strMSG07;
                }
            }
            else
            {
                if (txtCD_ITEM.Text.Length > 13 || txtCD_ITEM.Text.Length <= 0)
                {
                    txtCD_ITEM.Text = "";
                    msgBar.Text = strMSG07;
                }
                else
                {
                    if (txtBarcode01.Text.Length > 0)
                    {
                        if (!bType && txtCD_CLASS.Text.Length > 0 && txtCD_CTGY.Text.Length > 0 && txtCD_ITEM.Text.Length == 2)
                        {
                            _bComplete = true;
                            txtFG_CLASS.Text = txtCD_ITEM.Text.ToString();
                            txtCD_ITEM.Text = "";
                            msgBar.Text = strMSG01;
                        }
                        else
                        {
                            txtCD_ITEM.Text = "";
                            msgBar.Text = strMSG07;
                        }
                    }
                    else
                    {
                        if (!bType && txtCD_CLASS.Text.Length > 0 && txtCD_ITEM.Text.Length == 4)
                        {
                            GetServerPQ("DB", "PQ08", txtCD_ITEM.Text.ToString());
                        }
                        else if (!bType && txtCD_CLASS.Text.Length > 0 && txtCD_CTGY.Text.Length > 0 && txtCD_ITEM.Text.Length == 2)
                        {
                            _bComplete = true;
                            txtFG_CLASS.Text = txtCD_ITEM.Text.ToString();
                            txtCD_ITEM.Text = "";
                            msgBar.Text = strMSG01;
                        }
                        else
                        {
                            GetServerPQ("DB", "PQ05", txtCD_ITEM.Text.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 단품(PQ05), 품번(PQ06), 품목(PQ08) 전문 또는 DB조회
        /// </summary>
        /// <param name="strType">DB:DB조회, TASK:전문조회</param>
        /// <param name="strPQ">조회구분 단품(PQ05), 품번(PQ06), 품목(PQ08)</param>
        /// <param name="strValue">조회파라미터값</param>
        private void GetServerPQ(string strType, string strPQ, string strValue)
        {
            if (strType == "DB")
            {
                //단품, 품번, 품목 DB 조회
                m_Presenter.GetPQ(strPQ, strValue);
            }
            else if (strType == "TASK")
            {
                ChildManager.ShowProgress(true);
                SetControlDisable(true);

                if (strPQ == "PQ05")
                {
                    //단품조회
                    var pq05 = new PQ05DataTask(strValue);
                    pq05.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pq05_TaskCompleted);
                    pq05.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pq05_Errored);
                    pq05.ExecuteTask();
                }
                else if (strPQ == "PQ06")
                {
                    //품번조회
                    var pq06 = new PQ06DataTask(strValue);
                    pq06.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pq06_TaskCompleted);
                    pq06.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pq06_Errored);
                    pq06.ExecuteTask();

                    ////품번조회
                    //var pq07 = new PQ07DataTask(strValue, "");
                    //pq07.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pq06_TaskCompleted);
                    //pq07.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pq07_Errored);
                    //pq07.ExecuteTask();
                }
                else if (strPQ == "PQ08")
                {
                    //품목조회
                    var pq08 = new PQ08DataTask(strValue);
                    pq08.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pq08_TaskCompleted);
                    pq08.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pq08_Errored);
                    pq08.ExecuteTask();
                }
            }
        }

        /// <summary>
        /// 단품(PQ05), 품번(PQ06), 품목(PQ08) DB조회 내역 셋팅
        /// </summary>
        /// <param name="strPQ">단품(PQ05), 품번(PQ06), 품목(PQ08) 구분</param>
        /// <param name="strPQ">조회파라미터값</param>
        /// <param name="ds">조회 내역</param>
        public void SetPQ(string strPQ, string strValue, DataSet ds)
        {
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    if (strPQ == "PQ05")
                    {
                        if (!bType)
                        {
                            bType = true;
                            InitControl(txtCD_ITEM.Text.ToString(), false);
                        }
                        else
                        {
                            InitControl(txtCD_ITEM.Text.ToString(), true);
                        }

                        //단품조회
                        txtCD_ITEM.Text = "";
                        //txtCD_ITEM.Text = dr["CD_ITEM"] != null ? dr["CD_ITEM"].ToString() : "";
                        txtNM_ITEM.Text = dr["NM_ITEM"] != null ? dr["NM_ITEM"].ToString() : "";
                        txtCD_CLASS.Text = dr["CD_CLASS"] != null ? dr["CD_CLASS"].ToString() : "";
                        txtNM_CLASS.Text = dr["NM_CLASS"] != null ? dr["NM_CLASS"].ToString() : "";
                        txtFG_TAX.Text = dr["NM_TAX"] != null ? dr["NM_TAX"].ToString() : "";
                        txtCD_DP.Text = dr["NM_DP"] != null ? dr["NM_DP"].ToString() : "";
                        txtAmt.Text = dr["UT_SPRC"] != null && dr["UT_SPRC"].ToString() != "" ? string.Format("{0:#,##0}", Convert.ToInt32(dr["UT_SPRC"])) : "";

                        _bScan = false;
                        _bComplete = true;
                        msgBar.Text = strMSG01;
                    }
                    else if (strPQ == "PQ06")
                    {
                        if (bType)
                        {
                            bType = false;
                            InitControl(txtCD_ITEM.Text.ToString(), false);
                        }
                        else
                        {
                            InitControl(txtCD_ITEM.Text.ToString(), true);
                        }

                        //품번조회
                        txtCD_CLASS.Text = dr["CD_CLASS"] != null ? dr["CD_CLASS"].ToString() : "";
                        txtNM_CLASS.Text = dr["NM_CLASS"] != null ? dr["NM_CLASS"].ToString() : "";
                        //txtFG_CLASS.Text = dr["FG_CLASS"] != null ? dr["FG_CLASS"].ToString() : "";
                        txtCD_DEPT.Text = dr["CD_DEPT"] != null ? dr["CD_DEPT"].ToString() : "";
                        txtFG_TAX.Text = dr["NM_TAX"] != null ? dr["NM_TAX"].ToString() : "";

                        if (_bScan)
                        {
                            txtBarcode01.Text = txtCD_ITEM.Text.ToString();
                            msgBar.Text = strMSG02;

                            if (txtCD_ITEM.Text.Length >= 12)
                            {
                                //품번(PQ06) DB조회
                                GetServerPQ("DB", "PQ08", txtCD_ITEM.Text.ToString().Substring(8, 4));
                            }
                        }
                        else
                        {
                            txtCD_ITEM.Text = "";
                            msgBar.Text = strMSG03;
                        }
                    }
                    else if (strPQ == "PQ08")
                    {
                        //품목조회
                        txtCD_CTGY.Text = dr["CD_CTGY"] != null ? dr["CD_CTGY"].ToString() : "";
                        txtNM_CTGY.Text = dr["NM_CTGY"] != null ? dr["NM_CTGY"].ToString() : "";

                        if (_bScan)
                        {
                            _bScan = false;
                            txtCD_ITEM.Text = "";
                            msgBar.Text = strMSG02;
                        }
                        else
                        {
                            txtCD_ITEM.Text = "";
                            msgBar.Text = strMSG09;
                        }
                    }
                }
                else
                {
                    //단품(PQ05), 품번(PQ06), 품목(PQ08) 전문조회
                    GetServerPQ("TASK", strPQ, strValue);
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);                
            }
            
        }

        /// <summary>
        /// 공통코드 셋팅
        /// </summary>
        /// <param name="ds">table01:FG_TAX, table02:CD_DP</param>
        public void SetCode(DataSet ds)
        {
            if (ds != null)
            {
                dsCode = ds.Copy();    
            }
        }

        /// <summary>
        /// 단품조회
        /// </summary>
        /// <param name="responseData"></param>
        void pq05_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            ChildManager.ShowProgress(false);

            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                var data = responseData.DataRecords.ToDataRecords<PQ05RespData>();

                if (data.Length > 0)
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate()
                        {
                            if (!bType)
                            {
                                bType = true;
                                InitControl(txtCD_ITEM.Text.ToString(), false);
                            }
                            else
                            {
                                InitControl(txtCD_ITEM.Text.ToString(), true);
                            }

                            txtCD_ITEM.Text = "";
                            //txtCD_ITEM.Text = data[0].CdItem != null ? data[0].CdItem.ToString() : "";
                            txtNM_ITEM.Text = data[0].NmItem != null ? data[0].NmItem.ToString() : "";
                            txtCD_CLASS.Text = data[0].CdClass != null ? data[0].CdClass.ToString() : "";
                            txtNM_CLASS.Text = data[0].NmClass != null ? data[0].NmClass.ToString() : "";
                            txtAmt.Text = data[0].UtSprc != null ? data[0].UtSprc.ToString() : "";
                            txtAmt.Text = data[0].UtSprc != null && data[0].UtSprc.ToString() != "" ? string.Format("{0:#,##0}", Convert.ToInt32(data[0].UtSprc)) : "";

                            txtFG_TAX.Text = data[0].FgTax != null ? data[0].FgTax.ToString() : "";
                            txtCD_DP.Text = data[0].CdDp != null ? data[0].CdDp.ToString() : "";

                            if (dsCode != null && dsCode.Tables.Count > 0 && dsCode.Tables[0] != null && dsCode.Tables[0].Rows.Count > 0)
                            {
                                DataRow[] drFilter = dsCode.Tables[0].Select(string.Format("FG_TAX = '{0}'", data[0].FgTax != null ? data[0].FgTax.ToString() : ""));

                                if (drFilter != null && drFilter.Length > 0)
                                {
                                    txtFG_TAX.Text = drFilter[0]["NM_TAX"] != null ? drFilter[0]["NM_TAX"].ToString() : "";
                                }
                            }

                            if (dsCode != null && dsCode.Tables.Count > 1 && dsCode.Tables[1] != null && dsCode.Tables[1].Rows.Count > 0)
                            {
                                DataRow[] drFilter = dsCode.Tables[1].Select(string.Format("CD_DP = '{0}'", data[0].CdDp != null ? data[0].CdDp.ToString() : ""));

                                if (drFilter != null && drFilter.Length > 0)
                                {
                                    txtFG_TAX.Text = drFilter[0]["NM_TAX"] != null ? drFilter[0]["NM_TAX"].ToString() : "";
                                }
                            }

                            _bScan = false;
                            _bComplete = true;
                            msgBar.Text = strMSG01;
                            SetControlDisable(false);
                        });
                    }
                    else
                    {
                        if (!bType)
                        {
                            bType = true;
                            InitControl(txtCD_ITEM.Text.ToString(), false);
                        }
                        else
                        {
                            InitControl(txtCD_ITEM.Text.ToString(), true);
                        }

                        txtCD_ITEM.Text = "";
                        //txtCD_ITEM.Text = data[0].CdItem != null ? data[0].CdItem.ToString() : "";
                        txtNM_ITEM.Text = data[0].NmItem != null ? data[0].NmItem.ToString() : "";
                        txtCD_CLASS.Text = data[0].CdClass != null ? data[0].CdClass.ToString() : "";
                        txtNM_CLASS.Text = data[0].NmClass != null ? data[0].NmClass.ToString() : "";
                        txtAmt.Text = data[0].UtSprc != null ? data[0].UtSprc.ToString() : "";
                        txtAmt.Text = data[0].UtSprc != null && data[0].UtSprc.ToString() != "" ? string.Format("{0:#,##0}", Convert.ToInt32(data[0].UtSprc)) : "";

                        txtFG_TAX.Text = data[0].FgTax != null ? data[0].FgTax.ToString() : "";
                        txtCD_DP.Text = data[0].CdDp != null ? data[0].CdDp.ToString() : "";

                        if (dsCode != null && dsCode.Tables.Count > 0 && dsCode.Tables[0] != null && dsCode.Tables[0].Rows.Count > 0)
                        {
                            DataRow[] drFilter = dsCode.Tables[0].Select(string.Format("FG_TAX = '{0}'", data[0].FgTax != null ? data[0].FgTax.ToString() : ""));

                            if (drFilter != null && drFilter.Length > 0)
                            {
                                txtFG_TAX.Text = drFilter[0]["NM_TAX"] != null ? drFilter[0]["NM_TAX"].ToString() : "";
                            }
                        }

                        if (dsCode != null && dsCode.Tables.Count > 1 && dsCode.Tables[1] != null && dsCode.Tables[1].Rows.Count > 0)
                        {
                            DataRow[] drFilter = dsCode.Tables[1].Select(string.Format("CD_DP = '{0}'", data[0].CdDp != null ? data[0].CdDp.ToString() : ""));

                            if (drFilter != null && drFilter.Length > 0)
                            {
                                txtFG_TAX.Text = drFilter[0]["NM_TAX"] != null ? drFilter[0]["NM_TAX"].ToString() : "";
                            }
                        }

                        _bScan = false;
                        _bComplete = true;
                        msgBar.Text = strMSG01;
                        SetControlDisable(false);
                    }
                }
            }
            else
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        if (_bScan)
                        {
                            _bScan = false;
                            txtCD_ITEM.Text = "";
                            msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        }
                        else
                        {
                            if (txtCD_ITEM.Text.Length == 6)
                            {
                                GetServerPQ("DB", "PQ06", txtCD_ITEM.Text.ToString());
                            }
                            else
                            {
                                txtCD_ITEM.Text = "";
                                msgBar.Text = responseData.Response.ErrorMessage.ToString();
                            }
                        }
                        SetControlDisable(false);
                    });
                }
                else
                {
                    if (_bScan)
                    {
                        _bScan = false;
                        txtCD_ITEM.Text = "";
                        msgBar.Text = responseData.Response.ErrorMessage.ToString();
                    }
                    else
                    {
                        if (txtCD_ITEM.Text.Length == 6)
                        {
                            GetServerPQ("DB", "PQ06", txtCD_ITEM.Text.ToString());
                        }
                        else
                        {
                            txtCD_ITEM.Text = "";
                            msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        }
                    }
                    SetControlDisable(false);
                }
            }
        }

        void pq05_Errored(string errorMessage, Exception lastException)
        {
            ChildManager.ShowProgress(false);

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    if (_bScan)
                    {
                        _bScan = false;
                    }

                    msgBar.Text = strMSG10;
                    SetControlDisable(false);
                });
            }
            else
            {
                if (_bScan)
                {
                    _bScan = false;
                }

                msgBar.Text = strMSG10;
                SetControlDisable(false);
            }
        }

        /// <summary>
        /// 품번조회
        /// </summary>
        /// <param name="responseData"></param>
        void pq06_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            ChildManager.ShowProgress(false);

            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                var data = responseData.DataRecords.ToDataRecords<PQ06RespData>();

                if (data.Length > 0)
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate()
                        {
                            if (bType)
                            {
                                bType = false;
                                InitControl(txtCD_ITEM.Text.ToString(), false);
                            }
                            else
                            {
                                InitControl(txtCD_ITEM.Text.ToString(), true);
                            }

                            txtCD_CLASS.Text = data[0].cdClass != null ? data[0].cdClass.ToString() : "";
                            txtNM_CLASS.Text = data[0].nmClass != null ? data[0].nmClass.ToString() : "";
                            //txtFG_CLASS.Text = data[0].fgcl != null ? data[0].CdItem.ToString() : "";
                            txtCD_DEPT.Text = data[0].cdDept != null ? data[0].cdDept.ToString() : "";

                            txtFG_TAX.Text = data[0].fgTax != null ? data[0].fgTax.ToString() : "";

                            if (dsCode != null && dsCode.Tables.Count > 0 && dsCode.Tables[0] != null && dsCode.Tables[0].Rows.Count > 0)
                            {
                                DataRow[] drFilter = dsCode.Tables[0].Select(string.Format("FG_TAX = '{0}'", data[0].fgTax != null ? data[0].fgTax.ToString() : ""));

                                if (drFilter != null && drFilter.Length > 0)
                                {
                                    txtFG_TAX.Text = drFilter[0]["NM_TAX"] != null ? drFilter[0]["NM_TAX"].ToString() : "";
                                }
                            }

                            if (_bScan)
                            {
                                txtBarcode01.Text = txtCD_ITEM.Text.ToString();
                                msgBar.Text = strMSG02;

                                if (txtCD_ITEM.Text.Length >= 12)
                                {
                                    //품번(PQ06) DB조회
                                    GetServerPQ("DB", "PQ08", txtCD_ITEM.Text.ToString().Substring(8, 4));
                                }
                            }
                            else
                            {
                                txtCD_ITEM.Text = "";
                                msgBar.Text = strMSG03;
                            }

                            SetControlDisable(false);
                        });
                    }
                    else
                    {
                        if (bType)
                        {
                            bType = false;
                            InitControl(txtCD_ITEM.Text.ToString(), false);
                        }
                        else
                        {
                            InitControl(txtCD_ITEM.Text.ToString(), true);
                        }

                        txtCD_CLASS.Text = data[0].cdClass != null ? data[0].cdClass.ToString() : "";
                        txtNM_CLASS.Text = data[0].nmClass != null ? data[0].nmClass.ToString() : "";
                        //txtFG_CLASS.Text = data[0].fgcl != null ? data[0].CdItem.ToString() : "";
                        txtCD_DEPT.Text = data[0].cdDept != null ? data[0].cdDept.ToString() : "";

                        txtFG_TAX.Text = data[0].fgTax != null ? data[0].fgTax.ToString() : "";

                        if (dsCode != null && dsCode.Tables.Count > 0 && dsCode.Tables[0] != null && dsCode.Tables[0].Rows.Count > 0)
                        {
                            DataRow[] drFilter = dsCode.Tables[0].Select(string.Format("FG_TAX = '{0}'", data[0].fgTax != null ? data[0].fgTax.ToString() : ""));

                            if (drFilter != null && drFilter.Length > 0)
                            {
                                txtFG_TAX.Text = drFilter[0]["NM_TAX"] != null ? drFilter[0]["NM_TAX"].ToString() : "";
                            }
                        }

                        if (_bScan)
                        {
                            txtBarcode01.Text = txtCD_ITEM.Text.ToString();
                            msgBar.Text = strMSG02;

                            if (txtCD_ITEM.Text.Length >= 12)
                            {
                                //품번(PQ06) DB조회
                                GetServerPQ("DB", "PQ08", txtCD_ITEM.Text.ToString().Substring(8, 4));
                            }
                        }
                        else
                        {
                            txtCD_ITEM.Text = "";
                            msgBar.Text = strMSG03;
                        }
                        SetControlDisable(false);
                    }
                }
            }
            else
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        txtCD_ITEM.Text = "";
                        msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false);
                    });
                }
                else
                {
                    txtCD_ITEM.Text = "";
                    msgBar.Text = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false);
                }
            }
        }

        void pq06_Errored(string errorMessage, Exception lastException)
        {
            ChildManager.ShowProgress(false);

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    txtCD_ITEM.Text = "";
                    msgBar.Text = strMSG10;
                    SetControlDisable(false);
                });
            }
            else
            {
                txtCD_ITEM.Text = "";
                msgBar.Text = strMSG10;
                SetControlDisable(false);
            }
        }

        /// <summary>
        /// 품번조회
        /// </summary>
        /// <param name="responseData"></param>
        void pq07_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            ChildManager.ShowProgress(false);

            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                var data = responseData.DataRecords.ToDataRecords<PQ07RespData>();

                if (data.Length > 0)
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate()
                        {
                            txtCD_CLASS.Text = data[0].CdClass != null ? data[0].CdClass.ToString() : "";
                            txtNM_CLASS.Text = data[0].NmClass != null ? data[0].NmClass.ToString() : "";
                            txtFG_CLASS.Text = data[0].FgClass != null ? data[0].FgClass.ToString() : "";
                            txtCD_DEPT.Text = data[0].CdDept != null ? data[0].CdDept.ToString() : "";

                            txtFG_TAX.Text = data[0].FgTax != null ? data[0].FgTax.ToString() : "";

                            if (dsCode != null && dsCode.Tables.Count > 0 && dsCode.Tables[0] != null && dsCode.Tables[0].Rows.Count > 0)
                            {
                                DataRow[] drFilter = dsCode.Tables[0].Select(string.Format("FG_TAX = '{0}'", data[0].FgTax != null ? data[0].FgTax.ToString() : ""));

                                if (drFilter != null && drFilter.Length > 0)
                                {
                                    txtFG_TAX.Text = drFilter[0]["NM_TAX"] != null ? drFilter[0]["NM_TAX"].ToString() : "";
                                }
                            }
                            SetControlDisable(false);
                        });
                    }
                    else
                    {
                        txtCD_CLASS.Text = data[0].CdClass != null ? data[0].CdClass.ToString() : "";
                        txtNM_CLASS.Text = data[0].NmClass != null ? data[0].NmClass.ToString() : "";
                        txtFG_CLASS.Text = data[0].FgClass != null ? data[0].FgClass.ToString() : "";
                        txtCD_DEPT.Text = data[0].CdDept != null ? data[0].CdDept.ToString() : "";

                        txtFG_TAX.Text = data[0].FgTax != null ? data[0].FgTax.ToString() : "";

                        if (dsCode != null && dsCode.Tables.Count > 0 && dsCode.Tables[0] != null && dsCode.Tables[0].Rows.Count > 0)
                        {
                            DataRow[] drFilter = dsCode.Tables[0].Select(string.Format("FG_TAX = '{0}'", data[0].FgTax != null ? data[0].FgTax.ToString() : ""));

                            if (drFilter != null && drFilter.Length > 0)
                            {
                                txtFG_TAX.Text = drFilter[0]["NM_TAX"] != null ? drFilter[0]["NM_TAX"].ToString() : "";
                            }
                        }
                        SetControlDisable(false);
                    }

                    //품번조회
                    GetServerPQ("DB", "PQ08", txtCD_ITEM.Text.Substring(8, 4));
                }
            }
            else if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.NoInfo)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        txtCD_ITEM.Text = "";
                        msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false);
                    });
                }
                else
                {
                    txtCD_ITEM.Text = "";
                    msgBar.Text = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false);
                }
            }
            else
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        txtCD_ITEM.Text = "";
                        msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false);
                    });
                }
                else
                {
                    txtCD_ITEM.Text = "";
                    msgBar.Text = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false);
                }
            }
        }

        void pq07_Errored(string errorMessage, Exception lastException)
        {
            ChildManager.ShowProgress(false);

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    txtCD_ITEM.Text = "";
                    msgBar.Text = strMSG10;
                    SetControlDisable(false);
                });
            }
            else
            {
                txtCD_ITEM.Text = "";
                msgBar.Text = strMSG10;
                SetControlDisable(false);
            }
        }

        /// <summary>
        /// 품목조회
        /// </summary>
        /// <param name="responseData"></param>
        void pq08_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            ChildManager.ShowProgress(false);

            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                var data = responseData.DataRecords.ToDataRecords<PQ08RespData>();

                if (data.Length > 0)
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate()
                        {
                            txtCD_CTGY.Text = data[0].cdCtgy != null ? data[0].cdCtgy.ToString() : "";
                            txtNM_CTGY.Text = data[0].nmCtgy != null ? data[0].nmCtgy.ToString() : "";

                            if (_bScan)
                            {
                                _bScan = false;
                                txtCD_ITEM.Text = "";
                                msgBar.Text = strMSG02;
                            }
                            else
                            {
                                txtCD_ITEM.Text = "";
                                msgBar.Text = strMSG09;
                            }
                            SetControlDisable(false);
                        });
                    }
                    else
                    {
                        txtCD_CTGY.Text = data[0].cdCtgy != null ? data[0].cdCtgy.ToString() : "";
                        txtNM_CTGY.Text = data[0].nmCtgy != null ? data[0].nmCtgy.ToString() : "";

                        if (_bScan)
                        {
                            _bScan = false;
                            txtCD_ITEM.Text = "";
                            msgBar.Text = strMSG02;
                        }
                        else
                        {
                            txtCD_ITEM.Text = "";
                            msgBar.Text = strMSG09;
                        }
                        SetControlDisable(false);
                    }
                }
            }
            else
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        txtCD_ITEM.Text = "";
                        msgBar.Text = strMSG06;
                        SetControlDisable(false);
                    });
                }
                else
                {
                    txtCD_ITEM.Text = "";
                    msgBar.Text = strMSG06;
                    SetControlDisable(false);
                }
            }
        }

        void pq08_Errored(string errorMessage, Exception lastException)
        {
            ChildManager.ShowProgress(false);

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    msgBar.Text = strMSG10;
                    SetControlDisable(false);
                });
            }
            else
            {
                msgBar.Text = strMSG10;
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
                    foreach (var item in this.ContainerPanel.Controls)
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
                    }
                });
            }
            else
            {
                foreach (var item in this.ContainerPanel.Controls)
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
                }
            }

            //Application.DoEvents();
        }

        #endregion
    }
}
