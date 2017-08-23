//-----------------------------------------------------------------
/*
 * 화면명   : POS_IQ_P004.cs
 * 화면설명 : 영수증 조회
 * 개발자   : 정광호
 * 개발일자 : 2015.05.
 * 수정일자 : 2015.11.07 TCL
 *      1) 소스 구조 재수정
 *      2) 컨트롤 비활성화 단순화 함
 *      3) 저장물 거래 표시 추가
 *      4) 현금 IC 카드 읽기 및 조회
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
using System.Globalization;

using WSWD.WmallPos.POS.IQ.PI;
using WSWD.WmallPos.POS.IQ.PT;
using WSWD.WmallPos.POS.IQ.VI;
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
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PU;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PQ;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PQ;
using WSWD.WmallPos.FX.NetComm.Tasks.PQ;
using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.POS.FX.Win.Controls;
using WSWD.WmallPos.POS.FX.Win;
using System.Diagnostics;
using WSWD.WmallPos.POS.PY.PI;
using WSWD.WmallPos.POS.PY.VI;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PV;
using WSWD.WmallPos.POS.PY.VC;


namespace WSWD.WmallPos.POS.IQ.VC
{
    public partial class POS_IQ_P004 : FormBase, IIQP004View, IPYP013View
    {
        #region 변수

        //영수증 조회 비즈니스 로직
        private IIQP004presenter m_p004Presenter;


        /// <summary>
        /// 영수증 조회 저장 테이블
        /// </summary>
        private DataSet dsPrint = null;

        /// <summary>
        /// 전문통신 여부
        /// </summary>
        bool bGetPQData = false;

        StringBuilder sbPrint = null;

        bool[] bDesc = new bool[4];

        /// <summary>
        /// Loc added 10.30
        /// 현금IC인경우 카드번호 암호화안하고 보냄.
        /// 
        /// 여전법 변경 0617
        /// 사용안함
        /// 
        /// </summary>
        // bool bEncCardNo = true;

        //비즈니스 로직
        private IPYP013presenter m_p013Presenter;

        private RunModes m_runMode = RunModes.ModeNone;
        private RunModes RunMode
        {
            get
            {
                return m_runMode;
            }
            set
            {
                m_runMode = value;
                ChildManager.ShowProgress(value != RunModes.ModeNone);

                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        btnClose.Enabled = value == RunModes.ModeNone;
                        btnPrint.Enabled = value == RunModes.ModeNone;
                        btnSearch.Enabled = value == RunModes.ModeNone;
                        //btnICCardNo.Enabled = value == RunModes.ModeNone;
                    });
                }
                else
                {
                    btnClose.Enabled = value == RunModes.ModeNone;
                    btnPrint.Enabled = value == RunModes.ModeNone;
                    btnSearch.Enabled = value == RunModes.ModeNone;
                    //btnICCardNo.Enabled = value == RunModes.ModeNone;
                }
            }
        }

        /// <summary>
        /// Set current message
        /// </summary>
        private string MessageBar
        {
            set
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        msgBar.Text = value;
                    });
                }
                else
                {
                    msgBar.Text = value;
                }
            }
        }

        #region 여전법 추가 0604

        //private POS_PY_P021 m_waitFallBackReadPop = null;
        //private bool m_isFallBackMode = false;
        //private SignPadCardInfo m_cardICInfo;

        #endregion

        #endregion

        #region 생성자

        public POS_IQ_P004()
        {
            InitializeComponent();

            bDesc[0] = false;
            bDesc[1] = false;
            bDesc[2] = false;
            bDesc[3] = false;

            //그리드 설정
            SetGrd();

            //이벤트 등록
            InitEvent();

            //Form Load Event
            Load += new EventHandler(form_Load);
        }

        #endregion

        #region 그리드 설정

        private void SetGrd()
        {
            grd.InitializeCell += new CellDataBoundEventHandler(grd_InitializeCell);
            grd.RowDataBound += new RowDataBoundEventHandler(grd_RowDataBound);
            grd.RowClicked += new EventHandler(grd_RowClicked);
            grd.PageIndexChanged += new EventHandler(grd_PageIndexChanged);
            grd.ColumnClicked += new GridHeaderColumnClickedHandler(grd_ColumnClicked);

            //그리드 컬럼 설정
            grd.ColumnCount = 6;
            grd.SetColumn(0, "No", 60);
            grd.SetColumn(1, "시간", 60);
            grd.SetColumn(2, "금액");
            grd.SetColumn(3, "거래구분", 90);
            grd.SetColumn(4, "", 0);
            grd.SetColumn(5, "", 0);
            grd.AutoFillRows = true;
            grd.ShowPageNo = true;
            grd.ScrollType = ScrollTypes.PageChanged;
            grd.PageIndex = -1;

            ((System.Windows.Forms.TableLayoutPanel)(grd.Controls[2])).RowStyles[1] = new RowStyle(SizeType.Absolute, 60);
            ((System.Windows.Forms.Label)(grd.Controls[2].Controls[1])).AutoSize = false;
            ((System.Windows.Forms.Label)(grd.Controls[2].Controls[1])).Font = new Font("돋움체", 9, FontStyle.Bold);
        }

        #endregion

        #region 이벤트 등록

        /// <summary>
        /// Form Event 정의
        /// </summary>
        private void InitEvent()
        {
            txtSaleDate.InputFocused += new EventHandler(txt_InputFocused);
            txtPosNo.InputFocused += new EventHandler(txt_InputFocused);
            txtTrxnNo.InputFocused += new EventHandler(txt_InputFocused);

            txtPrefixCode.InputFocused += new EventHandler(txt_InputFocused);
            
            // 여전법 추가 0603            
            txtApprNo.InputFocused += new EventHandler(txt_InputFocused);
            txtPBNo.InputFocused += new EventHandler(txt_InputFocused);

            this.KeyEvent += new OPOSKeyEventHandler(form_KeyEvent);                                                             //Key Event
            this.btnSearch.Click += new EventHandler(btnSearch_Click);                                                                  //검색 button Event
            this.btnPrint.Click += new EventHandler(btnPrint_Click);                                                                    //발행 button Event
            this.btnClose.Click += new EventHandler(btnClose_Click);                                                                    //닫기 button Event
            this.Unload += new EventHandler(form_Unload);

            // Loc added 10.30
            // ICCard 정보 읽어온다
            // 카드번호 읽어와서 조회한다
            //this.btnICCardNo.Click += new EventHandler(btnICCardNo_Click);

            // 여전법 변경, 주석처리
            // 사용안함
            // POSDeviceManager.Msr.DataEvent += new POSMsrDataEventHandler(Msr_DataEvent);                                                //카드 리더기
            // POSDeviceManager.SignPad.CardICReaderEvent += new POSCardICOnEncCardReader(SignPad_CardICReaderEvent);
            // POSDeviceManager.SignPad.SetICTransAmount(1004);

            if (POSDeviceManager.Scanner != null)
            {
                POSDeviceManager.Scanner.DataEvent += new POSDataEventHandler(Scanner_DataEvent);               //Scanner Event    
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
            //영수증 조회 설정---------------------------
            m_p004Presenter = new IQP004presenter(this);

            // Loc added 10.30
            // 현금IC
            m_p013Presenter = new PY.PT.PYP013presenter(this);

            //-------------------------------------------
            dsPrint = new DataSet();

            DataTable dt = new DataTable("Desc");
            dt.Columns.Add(strColNo);
            dt.Columns.Add(strColTime);
            dt.Columns.Add(strColAmt, typeof(string));
            dt.Columns.Add(strColType);
            dt.Columns.Add(strColDesc);
            dt.Columns.Add(strColRealAmt, typeof(Int64));
            dsPrint.Tables.Add(dt);

            txtSaleDate.Text = ConfigData.Current.AppConfig.PosInfo.SaleDate;   //거래일자
            txtPosNo.Text = ConfigData.Current.AppConfig.PosInfo.PosNo;         //포스번호

            //거래번호 포커스
            txtTrxnNo.SetFocus();

            MessageBar = strMsg13;
        }

        /// <summary>
        /// Form UnLoad Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void form_Unload(object sender, EventArgs e)
        {
            grd.InitializeCell -= new CellDataBoundEventHandler(grd_InitializeCell);
            grd.RowDataBound -= new RowDataBoundEventHandler(grd_RowDataBound);
            grd.RowClicked -= new EventHandler(grd_RowClicked);
            grd.PageIndexChanged -= new EventHandler(grd_PageIndexChanged);
            grd.ColumnClicked -= new GridHeaderColumnClickedHandler(grd_ColumnClicked);

            txtSaleDate.InputFocused -= new EventHandler(txt_InputFocused);
            txtPosNo.InputFocused -= new EventHandler(txt_InputFocused);
            txtTrxnNo.InputFocused -= new EventHandler(txt_InputFocused);
            txtPrefixCode.InputFocused -= new EventHandler(txt_InputFocused);

            // 여전법 추가 0603            
            txtApprNo.InputFocused -= new EventHandler(txt_InputFocused);
            txtPBNo.InputFocused -= new EventHandler(txt_InputFocused);

            this.KeyEvent -= new OPOSKeyEventHandler(form_KeyEvent);                                                             //Key Event
            this.btnSearch.Click -= new EventHandler(btnSearch_Click);                                                                  //검색 button Event
            this.btnPrint.Click -= new EventHandler(btnPrint_Click);                                                                    //발행 button Event
            this.btnClose.Click -= new EventHandler(btnClose_Click);                                                                    //닫기 button Event
            this.Unload -= new EventHandler(form_Unload);

            // Loc added 10.30
            // ICCard 정보 읽어온다
            // 카드번호 읽어와서 조회한다
            // this.btnICCardNo.Click -= new EventHandler(btnICCardNo_Click);

            Load -= new EventHandler(form_Load);
            Unload -= new EventHandler(form_Unload);

            // 여전법 변경
            // 사용 안 함            
            // POSDeviceManager.Msr.DataEvent -= new POSMsrDataEventHandler(Msr_DataEvent);
            //카드 리더기
            // 여전법 추가 0603
            // POSDeviceManager.SignPad.CardICReaderEvent -= new POSCardICOnEncCardReader(SignPad_CardICReaderEvent);
            // POSDeviceManager.SignPad.Close();

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
            if (this.RunMode != RunModes.ModeNone)
            {
                e.IsHandled = true;
                return;
            }

            InputText it = (InputText)this.KeyListener.FocusedControl;
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
            {
                if (it.Text.Length == 0)
                {
                    e.IsHandled = true;
                    this.KeyListener.PreviousControl();
                }

                // need encrypt card no
                // 여번법 변경 06178
                // 사용안함 
                //if (it.Name.Equals(txtPrefixCode.Name))
                //{
                //    bEncCardNo = true;
                //}
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
            {
                if (it.Name.Equals(txtSaleDate.Name))
                {
                    if (!DateTimeUtils.ValidateDateFormat(it.Text))
                    {
                        e.IsHandled = true;
                        txtSaleDate.Text = "";
                        MessageBar = strMsg17;
                    }
                    else
                    {
                        e.IsHandled = true;
                        this.KeyListener.NextControl();
                    }
                }
                else
                {
                    e.IsHandled = true;
                    if (it.Text.Length > 0)
                    {
                        this.KeyListener.NextControl();
                    }
                }
            }
        }

        #region GRID Events


        /// <summary>
        /// 그리드 초기화
        /// </summary>
        /// <param name="e"></param>
        void grd_InitializeCell(CellDataBoundEventArgs e)
        {
            Label lbl = null;
            switch (e.Cell.ColumnIndex)
            {
                case 0:
                case 2:
                    lbl = new Label()
                    {
                        TextAlign = ContentAlignment.MiddleRight,
                        AutoSize = false,
                        Left = 0,
                        Top = 0,
                        Width = e.Cell.Width,
                        Height = e.Cell.Height,
                        BackColor = Color.Transparent
                    };
                    e.Cell.Controls.Add(lbl);
                    break;
                case 1:
                    lbl = new Label()
                    {
                        TextAlign = ContentAlignment.MiddleCenter,
                        AutoSize = false,
                        Left = 0,
                        Top = 0,
                        Width = e.Cell.Width,
                        Height = e.Cell.Height,
                        BackColor = Color.Transparent
                    };
                    e.Cell.Controls.Add(lbl);
                    break;
                default:
                    lbl = new Label()
                    {
                        TextAlign = ContentAlignment.MiddleLeft,
                        AutoSize = false,
                        Left = 0,
                        Top = 0,
                        Width = e.Cell.Width,
                        Height = e.Cell.Height,
                        BackColor = Color.Transparent
                    };
                    e.Cell.Controls.Add(lbl);
                    break;
            }
        }

        /// <summary>
        /// 그리드 데이터 바인딩
        /// </summary>
        /// <param name="row"></param>
        void grd_RowDataBound(RowDataBoundEventArgs e)
        {
            DataRow dr = (DataRow)e.ItemData;

            if (dr == null)
            {
                for (int i = 0; i < e.Row.Cells.Length; i++)
                {
                    e.Row.Cells[i].Controls[0].Text = string.Empty;
                }
                return;
            }

            e.Row.Cells[0].Controls[0].Text = TypeHelper.ToString(dr[0]);
            e.Row.Cells[1].Controls[0].Text = TypeHelper.ToString(dr[1]);

            string strMsg = TypeHelper.ToString(dr[3]);
            if (strMsg == strMsg01 || strMsg == strMsg02 || strMsg == strMsg03 || strMsg == strMsg07 || strMsg == strMsg23 || strMsg == strMsg21 || strMsg == strMsg22)
            {
                e.Row.Cells[2].Controls[0].Text = "";
            }
            else
            {
                e.Row.Cells[2].Controls[0].Text = TypeHelper.ToString(dr[2]).Length > 0 ? string.Format("{0:#,##0}", TypeHelper.ToInt64(dr[2])) : "";
            }
            e.Row.Cells[3].Controls[0].Text = TypeHelper.ToString(dr[3]);
            e.Row.Cells[4].Controls[0].Text = TypeHelper.ToString(dr[4]);
            e.Row.Cells[5].Controls[0].Text = TypeHelper.ToString(dr[5]);
        }

        /// <summary>
        /// 그리드 페이지 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void grd_PageIndexChanged(object sender, EventArgs e)
        {
            DoSelectTrxnRow();
        }

        /// <summary>
        /// 그리드 row 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void grd_RowClicked(object sender, EventArgs e)
        {
            DoSelectTrxnRow();
        }

        /// <summary>
        /// 그리드 정렬
        /// </summary>
        /// <param name="columnIndex"></param>
        void grd_ColumnClicked(int columnIndex)
        {
            if (this.RunMode != RunModes.ModeNone || grd.RowCount == 0) return;

            bool bSort = false;
            string strColNm = string.Empty;

            switch (columnIndex)
            {
                case 0:
                    strColNm = "colNo";
                    bSort = !bDesc[0];
                    bDesc[0] = !bDesc[0];
                    break;
                case 1:
                    strColNm = "colTime";
                    bSort = !bDesc[1];
                    bDesc[1] = !bDesc[1];
                    break;
                case 2:
                    strColNm = "colRealAmt";
                    bSort = !bDesc[2];
                    bDesc[2] = !bDesc[2];
                    break;
                case 3:
                    strColNm = "colType";
                    bSort = !bDesc[3];
                    bDesc[3] = !bDesc[3];
                    break;
                default:
                    break;
            }

            if (string.IsNullOrEmpty(strColNm))
            {
                return;
            }

            grd.ClearAll();
            grd.PageIndex = 0;

            DataView dv = dsPrint.Tables[0].DefaultView;
            dv.Sort = string.Format("{0} {1}", strColNm, bSort ? "DESC" : "ASC");

            foreach (var item in dv)
            {
                var dr = ((DataRowView)item).Row;
                grd.AddRow(dr, false);
            }

            grd.SelectedRowIndex = 0;
            grd.RefreshPageRows();
        }


        #endregion

        /// <summary>
        /// 텍스트 박스 포커스
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txt_InputFocused(object sender, EventArgs e)
        {
            InputText txt = (InputText)sender;

            if (txt.Name.Equals(txtSaleDate.Name))
            {
                MessageBar = strMsg11;
            }
            else if (txt.Name.Equals(txtPosNo.Name))
            {
                MessageBar = strMsg12;
            }
            else if (txt.Name.Equals(txtTrxnNo.Name))
            {
                MessageBar = strMsg13;
            }
            else if (txt.Name.Equals(txtPrefixCode.Name))
            {
                MessageBar = MSG_INPUT_PREFIX;
            }
            else if (txt.Name.Equals(txtApprNo.Name))
            {
                MessageBar = MSG_INPUT_APPRNO;
            }
            else if (txt.Name.Equals(txtPBNo.Name))
            {
                MessageBar = MSG_INPUT_PBNO;
            }
        }

        /// <summary>
        /// 검색 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            if (this.RunMode != RunModes.ModeNone) return;

            //조회전 필수입력사항 확인
            if (!ValidationOnSearch())
            {
                return;
            }

            // Set runing
            this.RunMode = RunModes.ModeSearchPrint;

            grd.ClearAll();
            dsPrint.Clear();
            txtPrint.Clear();
            sbPrint = new StringBuilder();

            //KSK_20170403
            if (txtPrefixCode.Text.Length > 0 || txtApprNo.Text.Length > 0 || txtPBNo.Text.Length > 0)
            {
                //카드번호가 존재시 로컬DB 조회하지 않고 바로 전문통신
                GetPQ10SaleTrxnHeader();
            }
            else
            {
                //영수증 조회
                m_p004Presenter.GetReceipt(txtSaleDate.Text.ToString(), txtPosNo.Text.ToString(), txtTrxnNo.Text.ToString());
            }
        }

        /// <summary>
        /// Loc added 10.30
        /// ICCARdNo 읽어온다
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnICCardNo_Click(object sender, EventArgs e)
        {
            RequestRandNumber();
        }

        /// <summary>
        /// 발행 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnPrint_Click(object sender, EventArgs e)
        {
            if (this.RunMode != RunModes.ModeNone) return;

            if (grd == null || grd.RowCount <= 0 || grd.SelectedRowIndex < 0 ||
                grd.GetSelectedRow() == null) return;

            this.RunMode = RunModes.ModeSearchPrint;

            try
            {
                DataRow dr = (DataRow)grd.GetSelectedRow().ItemData;
                if (dr != null)
                {
                    if (ChkPrint())
                    {
                        //프린트 발행
                        PrintOneReceipt(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                this.RunMode = RunModes.ModeNone;
            }
        }

        /// <summary>
        /// 닫기 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnClose_Click(object sender, EventArgs e)
        {
            if (this.RunMode != RunModes.ModeNone) return;
            this.Close();
        }

        #region 장비 이벤트


        /// <summary>
        /// 카드번호처리
        /// 
        /// 여전법 변경 06.05
        /// 사용 안함
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="cardNo"></param>
        /// <param name="expMY"></param>
        void Msr_DataEvent(string eventData, string cardNo, string expMY)
        {
            if (this.RunMode != RunModes.ModeNone) return;

            // Loc add 11.05
            // 신용카드인경우 암호화
            // 여전법 변경 0617
            // 사용 안함
            // bEncCardNo = true;

            if (this.InvokeRequired)
            {

                this.BeginInvoke((MethodInvoker)delegate()
                {
                    txtPrefixCode.Text = "";
                    //카드번호 입력
                    txtPrefixCode.Text = cardNo;
                    //Application.DoEvents();
                });
            }
            else
            {
                txtPrefixCode.Text = "";
                //카드번호 입력
                txtPrefixCode.Text = cardNo;
                //Application.DoEvents();
            }
        }

        /// <summary>
        /// Scanner Event
        /// </summary>
        /// <param name="msrData"></param>
        void Scanner_DataEvent(string msrData)
        {
            if (this.RunMode != RunModes.ModeNone) return;

            Trace.WriteLine("IQ_P004_Scanner_DataEvent " + msrData, "program");
            // bEncCardNo = true;

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    OnScannedData(msrData);
                });
            }
            else
            {
                OnScannedData(msrData);
            }
        }

        void OnScannedData(string msrData)
        {
            grd.ClearAll();
            dsPrint.Clear();
            txtPrint.Clear();
            txtSaleDate.Text = "";
            txtPosNo.Text = "";
            txtTrxnNo.Text = "";
            txtPrefixCode.Text = "";

            string strData = msrData.Replace("A", "");

            if (strData.Length == 14)
            {
                txtSaleDate.Text = ConfigData.Current.AppConfig.PosInfo.SaleDate.Substring(0, 2) + strData.Substring(0, 6);
                txtPosNo.Text = strData.Substring(6, 4);
                txtTrxnNo.Text = TypeHelper.ToInt32(strData.Substring(10)).ToString("d6");
                //Application.DoEvents();

                btnSearch_Click(btnSearch, null);
            }
            else if (strData.Length == 16)
            {
                if (strData.Substring(0, 2) == ConfigData.Current.AppConfig.PosInfo.StoreNo)
                {
                    txtSaleDate.Text = ConfigData.Current.AppConfig.PosInfo.SaleDate.Substring(0, 2) + strData.Substring(2, 6);
                    txtPosNo.Text = strData.Substring(8, 4);
                    txtTrxnNo.Text = TypeHelper.ToInt32(strData.Substring(12)).ToString("d6");
                    //Application.DoEvents();

                    btnSearch_Click(btnSearch, null);
                }
                else
                {
                    MessageBar = strMsg26;
                    txtSaleDate.SetFocus();
                    //Application.DoEvents();
                }
            }
            else
            {
                MessageBar = strMsg15;
                txtSaleDate.SetFocus();
                //Application.DoEvents();
            }
        }

        #endregion

        #endregion

        #region 사용자 정의

        /// <summary>
        /// 조회전 필수요건 확인
        /// </summary>
        /// <returns></returns>
        private bool ValidationOnSearch()
        {
            string strSaleDate = txtSaleDate.Text;
            string strPosNo = txtPosNo.Text;

            if (strSaleDate.Length == 8)
            {
                DateTime dtTime = DateTime.Now;
                if (!DateTime.TryParse(string.Format("{0}-{1}-{2}", strSaleDate.Substring(0, 4),
                    strSaleDate.Substring(4, 2), strSaleDate.Substring(6, 2)), out dtTime))
                {
                    txtSaleDate.Text = "";
                    MessageBar = strMsg17;
                    txtSaleDate.SetFocus();
                    return false;
                }
                else
                {
                    if (txtPrefixCode.Text.Length <= 0 && strPosNo.Length <= 0)
                    {
                        MessageBar = strMsg16;
                        txtPosNo.SetFocus();
                        return false;
                    }
                }
            }
            else
            {
                txtSaleDate.Text = "";
                MessageBar = strMsg15;
                txtSaleDate.SetFocus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Select a trxn row
        /// </summary>
        private void DoSelectTrxnRow()
        {
            if (this.RunMode != RunModes.ModeNone)
            {
                return;
            }


            if (grd.RowCount == 0 || grd.SelectedRowIndex < 0 ||
                grd.GetSelectedRow() == null)
            {
                return;
            }

            DataRow dr = (DataRow)grd.GetSelectedRow().ItemData;
            if (dr == null)
            {
                return;
            }

            Trace.WriteLine("DoSelectTrxnRow " + dr[0].ToString(), "program");

            //화면 표출 영수증 초기화
            txtPrint.Clear();

            //화면 표출 텍스트박스 셋팅
            // 전문통싱으로 데이터 받기
            if (bGetPQData)
            {
                //전문 통신
                GetPQ04SaleTrxn(dr);
            }
            else
            {
                SetPrintTextBox(dr);
            }
        }

        #region 화면표시 텍스트박스

        /// <summary>
        /// 화면 표출 텍스트박스 셋팅
        /// </summary>
        /// <param name="dr">영수증 내용</param>
        private void SetPrintTextBox(DataRow dr)
        {
            txtPrint.Clear();
            sbPrint = new StringBuilder();

            if (dr != null)
            {
                string strLossNm = string.Empty;

                string[] arrDesc;

                BasketHeader basketHeader = null;
                BasketAccount basketAccount = null;
                BasketReserve basketReserve = null;
                BasketMiddleDeposit basketMid = null;

                if (TypeHelper.ToString(dr[strColType]) == strMsg01)
                {
                    #region 개설
                    basketHeader = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), TypeHelper.ToString(dr[strColDesc]));
                    #endregion
                }
                else if (TypeHelper.ToString(dr[strColType]) == strMsg07 || TypeHelper.ToString(dr[strColType]) == strMsg23)
                {
                    #region 마감
                    arrDesc = TypeHelper.ToString(dr[strColDesc]).Split(';');
                    if (arrDesc.Length >= 2)
                    {
                        basketHeader = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), arrDesc[0]);
                        basketAccount = (BasketAccount)BasketAccount.Parse(typeof(BasketAccount), arrDesc[1]);
                        sbPrint.Append(POSPrinterUtils.Instance.SetPrintAccount(false,
                                TypeHelper.ToString(dr[strColType]) == "0" ? FXConsts.RECEIPT_NAME_POS_ED_P001 : (TypeHelper.ToString(dr[strColType]) == "1" ? FXConsts.RECEIPT_NAME_POS_ED_P002 : FXConsts.RECEIPT_NAME_POS_ED_P003),
                                basketHeader, basketAccount, null, 2));
                    }
                    #endregion
                }
                else if (TypeHelper.ToString(dr[strColType]) == strMsg02 || TypeHelper.ToString(dr[strColType]) == strMsg03)
                {
                    #region 사인ON,OFF
                    basketHeader = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), TypeHelper.ToString(dr[strColDesc]));
                    if (basketHeader != null)
                    {
                        bool isSignOn = TypeHelper.ToString(dr[strColType]) == strMsg02;
                        sbPrint.Append(POSPrinterUtils.Instance.PrintReceiptSignOn(false, isSignOn,
                            isSignOn ?
                            FXConsts.RECEIPT_SIGN_OK :
                            FXConsts.RECEIPT_SIGN_OFF,
                            basketHeader, DateTime.ParseExact(basketHeader.OccrDate + basketHeader.OccrTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture)));
                    }
                    #endregion
                }
                else if (TypeHelper.ToString(dr[strColType]) == strMsg04)
                {
                    #region 준비금
                    arrDesc = TypeHelper.ToString(dr[strColDesc]).Split(';');
                    if (arrDesc.Length >= 2)
                    {
                        basketHeader = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), arrDesc[0]);
                        basketReserve = (BasketReserve)BasketReserve.Parse(typeof(BasketReserve), arrDesc[1]);

                        sbPrint.Append(POSPrinterUtils.Instance.PrintIO_M001(false, basketHeader, basketReserve));
                    }
                    #endregion
                }
                else if (TypeHelper.ToString(dr[strColType]) == strMsg05)
                {
                    #region 중간입금
                    arrDesc = TypeHelper.ToString(dr[strColDesc]).Split(';');
                    if (arrDesc.Length >= 2)
                    {
                        basketHeader = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), arrDesc[0]);
                        basketMid = (BasketMiddleDeposit)BasketMiddleDeposit.Parse(typeof(BasketMiddleDeposit), arrDesc[1]);

                        sbPrint.Append(POSPrinterUtils.Instance.PrintIO_M002_M003(false, FXConsts.RECEIPT_NAME_POS_IO_M002, basketHeader, basketMid));
                    }
                    #endregion
                }
                else if (TypeHelper.ToString(dr[strColType]) == strMsg06)
                {
                    #region 마감입금
                    arrDesc = TypeHelper.ToString(dr[strColDesc]).Split(';');
                    if (arrDesc.Length >= 2)
                    {
                        basketHeader = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), arrDesc[0]);
                        basketMid = (BasketMiddleDeposit)BasketMiddleDeposit.Parse(typeof(BasketMiddleDeposit), arrDesc[1]);

                        sbPrint.Append(POSPrinterUtils.Instance.PrintIO_M002_M003(false, FXConsts.RECEIPT_NAME_POS_IO_M003, basketHeader, basketMid));
                    }
                    #endregion
                }
                else if (TypeHelper.ToString(dr[strColType]) == strMsg08 || TypeHelper.ToString(dr[strColType]) == strMsg09 ||
                    TypeHelper.ToString(dr[strColType]) == strMsg21 || TypeHelper.ToString(dr[strColType]) == strMsg22
                    || TypeHelper.ToString(dr[strColType]).Contains(strMsg27))
                {
                    #region 상품판매, 저장물 판매
                    arrDesc = TypeHelper.ToString(dr[strColDesc]).Split(';');
                    SetTextBoxSale(false, arrDesc);
                    #endregion
                }
                else if (TypeHelper.ToString(dr[strColType]) == strMsg25)
                {
                    #region 사은품 회수
                    arrDesc = TypeHelper.ToString(dr[strColDesc]).Split(';');
                    SetTextBoxTks(false, arrDesc);
                    #endregion
                }

                txtPrint.BindNoticeInfo(sbPrint.ToString());
            }
        }

        /// <summary>
        /// 판매
        /// </summary>
        /// <param name="bpH"></param>
        /// <param name="bpB"></param>
        void SetTextBoxSale(bool bPrint, string[] arrDesc)
        {
            List<BasketBase> basketBases = new List<BasketBase>();
            List<BasketItem> basketItems = new List<BasketItem>();
            List<BasketPay> basketPays = new List<BasketPay>();
            BasketHeader basketHeader = new BasketHeader();
            BasketCashRecpt basketCashReceipt = new BasketCashRecpt();
            BasketPointSave basketPointSave = new BasketPointSave();
            BasketSubTotal basketSubTtl = new BasketSubTotal();

            foreach (string strDesc in arrDesc)
            {
                if (strDesc.Length > 0)
                {
                    if (strDesc.Substring(0, 3) == BasketTypes.BasketHeader)
                    {
                        //헤더
                        basketHeader = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), strDesc);
                    }
                    else if (strDesc.Substring(0, 3) == BasketTypes.BasketSubTotal)
                    {
                        //소계
                        basketSubTtl = (BasketSubTotal)BasketSubTotal.Parse(typeof(BasketSubTotal), strDesc);
                    }
                    else if (strDesc.Substring(0, 3) == BasketTypes.BasketCashRecpt)
                    {
                        //현금영수증
                        basketCashReceipt = (BasketCashRecpt)BasketCashRecpt.Parse(typeof(BasketCashRecpt), strDesc);
                    }
                    else if (strDesc.Substring(0, 3) == BasketTypes.BasketPointSave)
                    {
                        //포인트적립
                        basketPointSave = (BasketPointSave)BasketPointSave.Parse(typeof(BasketPointSave), strDesc);
                    }
                    else if (strDesc.Substring(0, 3) == BasketTypes.BasketItem)
                    {
                        //상품
                        BasketItem basketItem = (BasketItem)BasketItem.Parse(typeof(BasketItem), strDesc);
                        basketItems.Add(basketItem);
                    }
                    else if (strDesc.Substring(0, 3) == BasketTypes.BasketPay)
                    {
                        //결제
                        BasketPay bp = (BasketPay)BasketPay.Parse(typeof(BasketPay), strDesc);
                        if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                        {
                            if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_SPECIAL && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_ONLINE)
                            {
                                //온라인
                                BasketPayCash basketPayCash = new BasketPayCash();
                                basketPayCash = (BasketPayCash)BasketPayCash.Parse(typeof(BasketPayCash), strDesc);
                                basketPays.Add(basketPayCash);
                            }
                            else if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CASH && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CASH)
                            {
                                //현금
                                BasketPayCash basketPayCash = new BasketPayCash();
                                basketPayCash = (BasketPayCash)BasketPayCash.Parse(typeof(BasketPayCash), strDesc);
                                basketPays.Add(basketPayCash);
                            }
                            else if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CARD)
                            {
                                //신용카드

                                //KSK_20170403
                                String TmpTot = strDesc;
                                String TmpSub1 = string.Empty;
                                string tmpSub2 = string.Empty;

                                if (TmpTot.Substring(97, 1) != "*")
                                {
                                    TmpSub1 = TmpTot.Substring(0, 224);
                                    tmpSub2 = TmpTot.Substring(224, TmpTot.Length - 225);
                                    TmpTot = String.Empty;
                                    TmpTot = TmpSub1 + "          " + tmpSub2;

                                    TmpSub1 = TmpTot.Substring(0, 373);
                                    tmpSub2 = TmpTot.Substring(373, TmpTot.Length - 374);
                                    TmpTot = String.Empty;
                                    TmpTot = TmpSub1 + "000000000" + tmpSub2;

                                    BasketPayCard basketPayCard = new BasketPayCard();
                                    basketPayCard = (BasketPayCard)BasketPayCard.Parse(typeof(BasketPayCard), TmpTot);
                                    basketPays.Add(basketPayCard);
                                }
                                else
                                {
                                    BasketPayCard basketPayCard = new BasketPayCard();
                                    basketPayCard = (BasketPayCard)BasketPayCard.Parse(typeof(BasketPayCard), strDesc);
                                    basketPays.Add(basketPayCard);

                                }

                            }
                            else if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_COUPON && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_EXCHANGE)
                            {
                                //상품교환권
                                BasketExchange basketExchange = new BasketExchange();
                                basketExchange = (BasketExchange)BasketExchange.Parse(typeof(BasketExchange), strDesc);
                                basketPays.Add(basketExchange);
                            }
                            else if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_TKCKET && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_TICKET_OTHER)
                            {
                                //타사상품권
                                BasketOtherTicket basketOtherTicket = new BasketOtherTicket();
                                basketOtherTicket = (BasketOtherTicket)BasketOtherTicket.Parse(typeof(BasketOtherTicket), strDesc);
                                basketPays.Add(basketOtherTicket);
                            }
                            else if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CARD_WELFARE)
                            {
                                //타건복지
                                basketPays.Add(bp);
                            }
                            else if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_COUPON && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_DISCOUNT)
                            {
                                //결제할인
                                basketPays.Add(bp);
                            }
                            else if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CARD_OTHER)
                            {
                                //타건카드
                                basketPays.Add(bp);
                            }
                            else if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CASH_IC)
                            {
                                //현금IC
                                BasketCashIC basketCashIC = new BasketCashIC();
                                basketCashIC = (BasketCashIC)BasketCashIC.Parse(typeof(BasketCashIC), strDesc);
                                basketPays.Add(basketCashIC);
                            }
                            else if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_POINT && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_POINT)
                            {
                                //포인트
                                BasketPoint basketPoint = new BasketPoint();
                                basketPoint = (BasketPoint)BasketPoint.Parse(typeof(BasketPoint), strDesc);
                                basketPays.Add(basketPoint);
                            }
                            else if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_COUPON && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_COUPON)
                            {
                                //쿠폰
                                BasketCoupon basketCoupon = new BasketCoupon();
                                basketCoupon = (BasketCoupon)BasketCoupon.Parse(typeof(BasketCoupon), strDesc);
                                basketPays.Add(basketCoupon);
                            }
                            else if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_COUPON && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_EXCHANGE_OLD)
                            {
                                //구상품교환권
                                BasketOldExGift basketOldExGift = new BasketOldExGift();
                                basketOldExGift = (BasketOldExGift)BasketOldExGift.Parse(typeof(BasketOldExGift), strDesc);
                                basketPays.Add(basketOldExGift);
                            }
                        }
                    }
                }
            }

            basketBases.Add(basketCashReceipt);
            basketBases.Add(basketPointSave);
            basketBases.AddRange(basketItems.ToArray());
            basketBases.Add(basketSubTtl);
            basketBases.AddRange(basketPays.ToArray());

            if (basketHeader.CancType != NetCommConstants.CANCEL_TYPE_CANCEL)
            {
                string strCD_CLASS = string.Empty;
                for (int i = 0; i < basketItems.Count; i++)
                {
                    if (basketItems[i].CdClass != null && basketItems[i].CdClass.Length > 0)
                    {
                        strCD_CLASS += strCD_CLASS.Length <= 0 ?
                            string.Format("'{0}'", basketItems[i].CdClass) :
                            string.Format(",'{0}'", basketItems[i].CdClass);
                    }
                }

                //프린트 메세지 조회
                var dsMsgTmp = m_p004Presenter.GetReceiptMsg(basketHeader.StoreNo, strCD_CLASS);

                DataTable dtPromtion = new DataTable();

                //프린트 DCC내용 전체조회
                DataTable dtDcc = m_p004Presenter.GetPrintDccMsg();

                POSPrinterUtils.CancelPrint cancelPrint = TypeHelper.ToString(basketHeader.CancType) == "2" ?
                    POSPrinterUtils.CancelPrint.Cancel : POSPrinterUtils.CancelPrint.Normal;

                if (bPrint)
                {
                    //결제내용 출력
                    POSPrinterUtils.Instance.SetPrintPay(bPrint, true, false, false, basketHeader,
                        basketBases, null, dsMsgTmp, dtPromtion, dtDcc, cancelPrint, POSPrinterUtils.CardPrint.Basic, false);
                }
                else
                {
                    //결제내용 출력
                    sbPrint.Append(POSPrinterUtils.Instance.SetPrintPay(bPrint, false, false, false,
                        basketHeader, basketBases, null, dsMsgTmp, dtPromtion, dtDcc, cancelPrint,
                        POSPrinterUtils.CardPrint.Basic, false));
                }
            }
            else
            {
                //거래중지일때

                StringBuilder sbTemp = new StringBuilder();

                foreach (BasketItem item in basketItems)
                {
                    var line = POSPrinterUtils.ReceiptSaleItem(
                        item.NmClass
                        , item.NmItem
                        , item.CdClass
                        , item.CdDp == "0" || item.CdDp == "4" ? item.InCdItem : item.CdItem
                        , item.FgMargin
                        , item.CdDp
                        , item.FgNewPrcApp == "1" ? true : false
                        , item.FgTax
                        , item.FgCanc
                        , item.CntItem.Length >= 0 ? TypeHelper.ToInt32(item.CntItem) : -TypeHelper.ToInt32(item.CntItem)   //반품일경우 -입력
                        , item.UtSprc.Length >= 0 ? TypeHelper.ToInt32(item.UtSprc) : -TypeHelper.ToInt32(item.UtSprc)      //반품일경우 -입력
                        , item.AmSale.Length >= 0 ? TypeHelper.ToInt32(item.AmSale) : -TypeHelper.ToInt32(item.AmSale));   //반품일경우 -입력
                    sbTemp.Append(line);
                }

                //프린트 메세지 조회
                var dsMsgTmp = m_p004Presenter.GetReceiptMsg(basketHeader.StoreNo, "");

                if (bPrint)
                {
                    POSPrinterUtils.Instance.PrintReceiptNoSale(bPrint, basketHeader, sbTemp.ToString(),
                        TypeHelper.ToInt64(basketHeader.TrxnAmt), true, dsMsgTmp);
                }
                else
                {
                    sbPrint.Append(POSPrinterUtils.Instance.PrintReceiptNoSale(bPrint, basketHeader,
                        sbTemp.ToString(), TypeHelper.ToInt64(basketHeader.TrxnAmt), false, dsMsgTmp));
                }
            }
        }

        /// <summary>
        /// 사은품 회수
        /// </summary>
        /// <param name="bPrint"></param>
        /// <param name="arrDesc"></param>
        void SetTextBoxTks(bool bPrint, string[] arrDesc)
        {
            BasketHeader basketHeader = new BasketHeader();
            List<BasketTksPresentRtn> basketTksPresentRtn = new List<BasketTksPresentRtn>();

            foreach (string strDesc in arrDesc)
            {
                if (strDesc.Length > 0)
                {
                    if (strDesc.Substring(0, 3) == BasketTypes.BasketHeader)
                    {
                        //헤더
                        basketHeader = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), strDesc);
                    }
                    else if (strDesc.Substring(0, 3) == BasketTypes.BasketTksPresentRtn)
                    {
                        //사은품 회수
                        basketTksPresentRtn.Add((BasketTksPresentRtn)BasketTksPresentRtn.Parse(typeof(BasketTksPresentRtn), strDesc));
                    }
                }
            }

            if (bPrint)
            {
                POSPrinterUtils.Instance.PrintTksPresentRtn(bPrint, true, basketHeader, basketTksPresentRtn);
            }
            else
            {
                sbPrint.Append(POSPrinterUtils.Instance.PrintTksPresentRtn(bPrint, true, basketHeader, basketTksPresentRtn));
            }
        }

        #endregion

        /// <summary>
        /// 프린트 발행
        /// </summary>
        /// <param name="dr">그리드 선택 자료</param>
        void PrintOneReceipt(DataRow dr)
        {
            string[] arrDesc;

            BasketHeader basketHeader = null;
            BasketAccount bpA = null;
            BasketReserve basketReserve = null;
            BasketMiddleDeposit basketMid = null;
            //List<BasketTksPresentRtn> basketTksPresentRtn = null;

            if (TypeHelper.ToString(dr[strColType]) == strMsg01)
            {
                #region 개설
                basketHeader = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), TypeHelper.ToString(dr[strColDesc]));

                int done = 0;
                int total = 0;
                StringBuilder sb = new StringBuilder();
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_STORE, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_CAS, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PROD, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PRODNO, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PRODITEMM, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_DISPPRETOUCH, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_DISPPRODTOUCU, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_KEYTOUCU, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_NAMECARD, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_AD, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_CARD, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_NOTICE, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_CODE, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_MSG, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_TGIFT, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_TGIFTTYPE, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_COUPON, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PMMST, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PMHDMST, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PMBRANDMST, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PMHDGIFTMNG, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PMHDGIVEMNG, done, total, ""));
                sb.Append(POSPrinterUtils.ReceiptSdOpenBody("0", FXConsts.RECEIPT_SD_PMEVENTTXT, done, total, ""));
                POSPrinterUtils.Instance.PrintReceiptSdOpen(basketHeader, sb.ToString());
                #endregion
            }
            else if (TypeHelper.ToString(dr[strColType]) == strMsg07 || TypeHelper.ToString(dr[strColType]) == strMsg23)
            {
                #region 마감
                arrDesc = TypeHelper.ToString(dr[strColDesc]).Split(';');
                basketHeader = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), arrDesc[0]);
                bpA = (BasketAccount)BasketAccount.Parse(typeof(BasketAccount), arrDesc[1]);

                if (TypeHelper.ToString(dr[strColType]) == strMsg23)
                {
                    POSPrinterUtils.Instance.SetPrintAccount(true, FXConsts.RECEIPT_NAME_POS_ED_P002, basketHeader,
                        bpA, null, 2);
                }
                else
                {
                    POSPrinterUtils.Instance.SetPrintAccount(true, FXConsts.RECEIPT_NAME_POS_ED_P003, basketHeader,
                        bpA, null, 2);
                }

                #endregion
            }
            else if (TypeHelper.ToString(dr[strColType]) == strMsg02)
            {
                #region 사인ON
                basketHeader = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), TypeHelper.ToString(dr[strColDesc]));
                if (basketHeader != null)
                {
                    POSPrinterUtils.Instance.PrintReceiptSignOn(true, true, FXConsts.RECEIPT_SIGN_OK,
                        basketHeader, DateTime.ParseExact(basketHeader.OccrDate + basketHeader.OccrTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture));
                }
                #endregion
            }
            else if (TypeHelper.ToString(dr[strColType]) == strMsg03)
            {
                #region 사인OFF
                basketHeader = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), TypeHelper.ToString(dr[strColDesc]));
                if (basketHeader != null)
                {
                    POSPrinterUtils.Instance.PrintReceiptSignOn(true, false, FXConsts.RECEIPT_SIGN_OFF, basketHeader, DateTime.ParseExact(basketHeader.OccrDate + basketHeader.OccrTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture));
                }
                #endregion
            }
            else if (TypeHelper.ToString(dr[strColType]) == strMsg04)
            {
                #region 준비금
                arrDesc = TypeHelper.ToString(dr[strColDesc]).Split(';');
                if (arrDesc.Length >= 2)
                {
                    basketHeader = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), arrDesc[0]);
                    basketReserve = (BasketReserve)BasketReserve.Parse(typeof(BasketReserve), arrDesc[1]);
                    POSPrinterUtils.Instance.PrintIO_M001(true, basketHeader, basketReserve);
                }
                #endregion
            }
            else if (TypeHelper.ToString(dr[strColType]) == strMsg05)
            {
                #region 중간입금
                arrDesc = TypeHelper.ToString(dr[strColDesc]).Split(';');
                if (arrDesc.Length >= 2)
                {
                    basketHeader = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), arrDesc[0]);
                    basketMid = (BasketMiddleDeposit)BasketMiddleDeposit.Parse(typeof(BasketMiddleDeposit), arrDesc[1]);

                    POSPrinterUtils.Instance.PrintIO_M002_M003(true, FXConsts.RECEIPT_NAME_POS_IO_M002, basketHeader, basketMid);
                }
                #endregion
            }
            else if (TypeHelper.ToString(dr[strColType]) == strMsg06)
            {
                #region 마감입금
                arrDesc = TypeHelper.ToString(dr[strColDesc]).Split(';');
                if (arrDesc.Length >= 2)
                {
                    basketHeader = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), arrDesc[0]);
                    basketMid = (BasketMiddleDeposit)BasketMiddleDeposit.Parse(typeof(BasketMiddleDeposit), arrDesc[1]);

                    POSPrinterUtils.Instance.PrintIO_M002_M003(true, FXConsts.RECEIPT_NAME_POS_IO_M003, basketHeader, basketMid);
                }
                #endregion
            }
            else if (TypeHelper.ToString(dr[strColType]).Contains(strMsg08) || TypeHelper.ToString(dr[strColType]).Contains(strMsg09) ||
                TypeHelper.ToString(dr[strColType]).Contains(strMsg21) || TypeHelper.ToString(dr[strColType]).Contains(strMsg22)
                || TypeHelper.ToString(dr[strColType]).Contains(strMsg27))
            {
                #region 상품판매, 저장물 판매
                arrDesc = TypeHelper.ToString(dr[strColDesc]).Split(';');
                SetTextBoxSale(true, arrDesc);
                #endregion
            }
            else if (TypeHelper.ToString(dr[strColType]) == strMsg25)
            {
                #region 사은품 회수
                arrDesc = TypeHelper.ToString(dr[strColDesc]).Split(';');
                SetTextBoxTks(true, arrDesc);
                #endregion
            }
        }

        #region 그리드 데이터 테이블

        /// <summary>
        /// 영수증 리스트 테이블 Row 추가
        /// </summary>
        /// <param name="bp"></param>
        /// <param name="strMsg"></param>
        void GridAddRow(string strTrxn, BasketHeader bp, string strMsg)
        {
            DataRow NewDr = dsPrint.Tables[strDTNm].NewRow();
            NewDr[0] = strTrxn;
            NewDr[1] = string.Format("{0}:{1}", bp.OccrTime.ToString().Substring(0, 2), bp.OccrTime.ToString().Substring(2, 2));

            if (strMsg == strMsg01 || strMsg == strMsg02 || strMsg == strMsg03 || strMsg == strMsg07 || strMsg == strMsg23 || strMsg == strMsg21 || strMsg == strMsg22 || strMsg == strMsg25)
            {
                NewDr[2] = "";
            }
            else
            {
                NewDr[2] = TypeHelper.ToString(bp.TrxnAmt);
            }

            NewDr[3] = strMsg;
            NewDr[4] = bp.ToString();
            if (strMsg == strMsg01 || strMsg == strMsg02 || strMsg == strMsg03 || strMsg == strMsg07 || strMsg == strMsg23 || strMsg == strMsg21 || strMsg == strMsg22 || strMsg == strMsg25)
            {
                NewDr[5] = DBNull.Value;
            }
            else
            {
                NewDr[5] = TypeHelper.ToInt64(bp.TrxnAmt);
            }
            dsPrint.Tables[strDTNm].Rows.Add(NewDr);
        }

        /// <summary>
        /// 영수증 리스트 테이블 Row 추가
        /// </summary>
        /// <param name="strOccrTime"></param>
        /// <param name="strAmt"></param>
        /// <param name="strDesc"></param>
        void GridAddRow(string strTrxn, BasketHeader bp, Int64 iAmt, string strDesc, string strMsg)
        {
            DataRow NewDr = dsPrint.Tables[strDTNm].NewRow();
            NewDr[0] = strTrxn;
            NewDr[1] = string.Format("{0}:{1}", bp.OccrTime.ToString().Substring(0, 2), bp.OccrTime.ToString().Substring(2, 2));
            if (strMsg == strMsg01 || strMsg == strMsg02 || strMsg == strMsg03 || strMsg == strMsg07 || strMsg == strMsg23 || strMsg == strMsg21 || strMsg == strMsg22 || strMsg == strMsg25)
            {
                NewDr[2] = "";
            }
            else
            {
                NewDr[2] = iAmt.ToString();
            }
            NewDr[3] = strMsg;
            NewDr[4] = strDesc;
            if (strMsg == strMsg01 || strMsg == strMsg02 || strMsg == strMsg03 || strMsg == strMsg07 || strMsg == strMsg23 || strMsg == strMsg21 || strMsg == strMsg22 || strMsg == strMsg25)
            {
                NewDr[5] = DBNull.Value;
            }
            else
            {
                NewDr[5] = iAmt;
            }
            dsPrint.Tables[strDTNm].Rows.Add(NewDr);
        }

        #endregion

        #region DB 조회
        /// <summary>
        /// 영수증 조회 내역 셋팅
        /// </summary>
        /// <param name="ds">영수증 조회 내역 결과</param>
        public void SetReceiptList(DataSet ds)
        {
            try
            {
                //테이블 초기화

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    BasketHeader basketHeader = null;
                    BasketAccount basketAccount = null;
                    BasketReserve basketReserve = null;
                    BasketMiddleDeposit basketMid = null;
                    BasketSubTotal basketSubTotal = null;
                    Int64 iAmt = 0;
                    Int64 iBal = 0;

                    //전문통신 여부
                    bGetPQData = false;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = ds.Tables[0].Rows[i];

                        if (dr[strDTColNm] != null && dr[strDTColNm].ToString().Length > 0 && dr[strDTColNm].ToString().Substring(0, 3) == BasketTypes.BasketHeader)
                        {
                            basketHeader = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), dr[strDTColNm].ToString());

                            if (basketHeader != null && basketHeader.Length > 0)
                            {
                                if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_POS_OPEN)
                                {
                                    #region 개설
                                    GridAddRow(dr[strDTColTrxn].ToString(), basketHeader, strMsg01);
                                    #endregion
                                }
                                else if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_POS_CLOSE || basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_CAS_CALC)
                                {
                                    #region 마감, 계산원마감
                                    basketAccount = (BasketAccount)BasketAccount.Parse(typeof(BasketAccount), ds.Tables[0].Rows[i + 1][strDTColNm].ToString());
                                    GridAddRow(dr[strDTColTrxn].ToString(), basketHeader, 0, basketHeader.ToString() + ";" + basketAccount.ToString(), basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_CAS_CALC ? strMsg23 : strMsg07);
                                    #endregion
                                }
                                else if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_SIGNON)
                                {
                                    #region 사인ON
                                    GridAddRow(dr[strDTColTrxn].ToString(), basketHeader, strMsg02);
                                    #endregion
                                }
                                else if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_SIGNOFF)
                                {
                                    #region 사인OFF
                                    GridAddRow(dr[strDTColTrxn].ToString(), basketHeader, strMsg03);
                                    #endregion
                                }
                                else if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_PRE_IO)
                                {
                                    #region 준비금
                                    basketReserve = (BasketReserve)BasketReserve.Parse(typeof(BasketReserve), ds.Tables[0].Rows[i + 1][strDTColNm].ToString());
                                    GridAddRow(dr[strDTColTrxn].ToString(), basketHeader, TypeHelper.ToInt64(basketReserve.ReserveAmt), basketHeader.ToString() + ";" + basketReserve.ToString(), strMsg04);
                                    #endregion
                                }
                                else if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_MID_IO)
                                {
                                    #region 중간입금
                                    basketMid = (BasketMiddleDeposit)BasketMiddleDeposit.Parse(typeof(BasketMiddleDeposit), ds.Tables[0].Rows[i + 1][strDTColNm].ToString());
                                    GridAddRow(dr[strDTColTrxn].ToString(), basketHeader, 
                                        TypeHelper.ToInt64(basketMid.CashTotalAmt) + 
                                        TypeHelper.ToInt64(basketMid.TicketTotalAmt) +
                                        TypeHelper.ToInt64(basketMid.OtherCompanyTicketTotalAmt) +
                                        TypeHelper.ToInt64(basketMid.DiscCouponAmt), 
                                        basketHeader.ToString() + ";" + basketMid.ToString(), strMsg05);
                                    #endregion
                                }
                                else if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_CLO_IO)
                                {
                                    #region 마감입금
                                    basketMid = (BasketMiddleDeposit)BasketMiddleDeposit.Parse(typeof(BasketMiddleDeposit), ds.Tables[0].Rows[i + 1][strDTColNm].ToString());
                                    GridAddRow(dr[strDTColTrxn].ToString(), basketHeader, 
                                        TypeHelper.ToInt64(basketMid.CashTotalAmt) + 
                                        TypeHelper.ToInt64(basketMid.TicketTotalAmt) +
                                        TypeHelper.ToInt64(basketMid.OtherCompanyTicketTotalAmt) +
                                        TypeHelper.ToInt64(basketMid.DiscCouponAmt), 
                                        basketHeader.ToString() + ";" + basketMid.ToString(), strMsg06);
                                    #endregion
                                }
                                else if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_SALE || basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_OTH_SALE)
                                {
                                    #region 상품판매, 저장물판매

                                    iAmt = 0;
                                    string strVC_CONT = string.Empty;
                                    string strMsgAdd = "";
                                    for (int j = i + 1; j < ds.Tables[0].Rows.Count; j++)
                                    {
                                        DataRow drTemp = ds.Tables[0].Rows[j];
                                        if (drTemp[strDTColNm] != null && drTemp[strDTColNm].ToString().Length > 0 && drTemp[strDTColNm].ToString().Substring(0, 3) == BasketTypes.BasketEnd &&
                                            dr["NO_TRXN"].ToString() == drTemp["NO_TRXN"].ToString())
                                        {
                                            break;
                                        }
                                        else if (dr["NO_TRXN"].ToString() == drTemp["NO_TRXN"].ToString())
                                        {
                                            strVC_CONT += (drTemp[strDTColNm].ToString() + ";");

                                            if (drTemp[strDTColNm].ToString().Substring(0, 3) == BasketTypes.BasketSubTotal)
                                            {
                                                iAmt += 0;
                                                iBal += 0;

                                                basketSubTotal = (BasketSubTotal)BasketSubTotal.Parse(typeof(BasketSubTotal), drTemp[strDTColNm].ToString());

                                                iAmt = TypeHelper.ToInt64(basketSubTotal.AmSubTtl);
                                            }
                                        }
                                    }

                                    if (basketHeader.CancType == "0")
                                    {
                                        strMsgAdd = basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_SALE ?
                                                        strMsg08 : "+" + strMsg27;
                                    }
                                    else if (basketHeader.CancType == "1")
                                    {
                                        strMsgAdd = strMsg21;
                                    }
                                    else if (basketHeader.CancType == "2")
                                    {
                                        strMsgAdd = basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_OTH_SALE ?
                                            "-" + strMsg27 : strMsg09;
                                    }

                                    GridAddRow(dr[strDTColTrxn].ToString(), basketHeader, iAmt <= 0 ? TypeHelper.ToInt64(basketHeader.TrxnAmt) : iAmt, strVC_CONT.Length > 0 ? basketHeader.ToString() + ";" + strVC_CONT : basketHeader.ToString(), strMsgAdd);

                                    #endregion
                                }
                                else if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_TKS_PRS)
                                {
                                    #region 사은품 회수

                                    iAmt = 0;
                                    string strVC_CONT = string.Empty;
                                    for (int j = i + 1; j < ds.Tables[0].Rows.Count; j++)
                                    {
                                        DataRow drTemp = ds.Tables[0].Rows[j];
                                        if (drTemp[strDTColNm] != null && drTemp[strDTColNm].ToString().Length > 0 &&
                                            drTemp[strDTColNm].ToString().Substring(0, 3) == BasketTypes.BasketEnd &&
                                            dr["NO_TRXN"].ToString() == drTemp["NO_TRXN"].ToString())
                                        {
                                            break;
                                        }
                                        else if (dr["NO_TRXN"].ToString() == drTemp["NO_TRXN"].ToString())
                                        {
                                            strVC_CONT += (drTemp[strDTColNm].ToString() + ";");
                                        }
                                    }

                                    GridAddRow(dr[strDTColTrxn].ToString(), basketHeader, iAmt, strVC_CONT.Length > 0 ? basketHeader.ToString() + ";" + strVC_CONT : basketHeader.ToString(), strMsg25);
                                    #endregion
                                }
                            }
                        }
                    }

                    grd.ClearAll();

                    foreach (DataRow dr in dsPrint.Tables[strDTNm].Rows)
                    {
                        grd.AddRow(dr);
                    }

                    //조회완료
                    this.RunMode = RunModes.ModeNone;

                    // Load selected row;
                    if (grd.RowCount > 0)
                    {
                        if (grd.SelectedRowIndex != 0)
                        {
                            grd.SelectedRowIndex = 0;
                        }
                    }
                }
                else
                {
                    //로컬DB에 자료가 없으면 전문통신으로 자료 조회
                    GetPQ10SaleTrxnHeader();
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
        }

        #endregion

        #region 전문 조회

        /// <summary>
        /// 전문통신 - 영수증조회
        /// </summary>
        void GetPQ04SaleTrxn(DataRow dr)
        {
            // Start processing
            this.RunMode = RunModes.ModeSearchPrint;

            BasketHeader bp = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), TypeHelper.ToString(dr[strColDesc]));

            string strSaleDate = bp.SaleDate;
            string strStoreNo = bp.StoreNo;
            string strPosNo = bp.PosNo;
            string strTrxnNo = bp.TrxnNo;

            // 매출 TRAN 조회 전문
            PQ04ReqData reqData = new PQ04ReqData();
            reqData.SaleDate = strSaleDate;
            reqData.StoreNo = strStoreNo;
            reqData.PosNo = strPosNo;
            reqData.TrxnNo = strTrxnNo;

            PQ04DataTask pq04 = new PQ04DataTask(reqData);
            pq04.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pq04_Errored);
            pq04.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pq04_TaskCompleted);
            pq04.ExecuteTask();
        }

        /// <summary>
        /// 카드번호 존재 시 전문 통신한다 (헤더 가져온다)
        /// 로컬DB에 자료가 없으면 전문통신으로 자료 조회
        /// </summary>
        void GetPQ10SaleTrxnHeader()
        {
            this.RunMode = RunModes.ModeSearchPrint;

            //전문통신 여부
            bGetPQData = true;

            //영수증 재발행 리스트용 헤더정보 조회 전문
            PQ10ReqData reqData = new PQ10ReqData();
            reqData.SaleDate = txtSaleDate.Text.ToString();
            reqData.StoreNo = ConfigData.Current.AppConfig.PosInfo.StoreNo;
            reqData.PosNo = txtPosNo.Text.ToString();
            reqData.TrxnNo = txtTrxnNo.Text.ToString();

            // 여전법 변경, 사용 안함
            // reqData.CardNo = bEncCardNo ? DataUtils.DamoEncrypt(txtPrefixCode.Text) : txtPrefixCode.Text;

            // 여전법 추가 0605
            reqData.PrefixCode = txtPrefixCode.Text.Length > 6 ? txtPrefixCode.Text.Substring(0, 6) : txtPrefixCode.Text;
            reqData.ApprNo = txtApprNo.Text;

            // 여전법 추가 0617
            // 품번
            reqData.PBCode = txtPBNo.Text;

            PQ10DataTask pq10 = new PQ10DataTask(reqData);
            pq10.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pq10_Errored);
            pq10.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pq10_TaskCompleted);
            pq10.ExecuteTask();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRecords"></param>
        void OnPQ04ReturnSuccess(PQ04RespData[] dataRecords)
        {
            this.RunMode = RunModes.ModeNone;

            DataRow dr = (DataRow)grd.GetSelectedRow().ItemData;
            if (dr == null)
            {
                return;
            }

            BasketHeader bp = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), TypeHelper.ToString(dr[strColDesc]));

            string strTemp = string.Empty;
            if (bp.TrxnType == NetCommConstants.TRXN_TYPE_POS_CLOSE || bp.TrxnType == NetCommConstants.TRXN_TYPE_CAS_CALC)
            {
                #region 마감
                BasketAccount basketAccount = (BasketAccount)BasketAccount.Parse(typeof(BasketAccount), dataRecords[1].TrxnInfo);
                strTemp = basketAccount.ToString();
                #endregion
            }
            else if (bp.TrxnType == NetCommConstants.TRXN_TYPE_PRE_IO)
            {
                #region 준비금
                BasketReserve basketReserve = (BasketReserve)BasketReserve.Parse(typeof(BasketReserve), dataRecords[1].TrxnInfo);
                strTemp = basketReserve.ToString();
                #endregion
            }
            else if (bp.TrxnType == NetCommConstants.TRXN_TYPE_MID_IO)
            {
                #region 중간입금
                BasketMiddleDeposit basketMid = (BasketMiddleDeposit)BasketMiddleDeposit.Parse(typeof(BasketMiddleDeposit), dataRecords[1].TrxnInfo);
                strTemp = basketMid.ToString();
                #endregion
            }
            else if (bp.TrxnType == NetCommConstants.TRXN_TYPE_CLO_IO)
            {
                #region 마감입금
                BasketMiddleDeposit basketMid = (BasketMiddleDeposit)BasketMiddleDeposit.Parse(typeof(BasketMiddleDeposit), dataRecords[1].TrxnInfo);
                strTemp = basketMid.ToString();
                #endregion
            }
            else if (bp.TrxnType == NetCommConstants.TRXN_TYPE_SALE || bp.TrxnType == NetCommConstants.TRXN_TYPE_OTH_SALE)
            {
                #region 상품판매, 저장물판매
                for (int i = 0; i < dataRecords.Length; i++)
                {
                    strTemp += (dataRecords[i].TrxnInfo + ";");
                }
                #endregion
            }
            else if (bp.TrxnType == NetCommConstants.TRXN_TYPE_TKS_PRS)
            {
                #region 사은품 회수

                for (int i = 0; i < dataRecords.Length; i++)
                {
                    strTemp += (dataRecords[i].TrxnInfo + ";");
                }

                #endregion
            }

            DataRow[] drFilter = dsPrint.Tables[0].Select(string.Format("{0} = '{1}' and {2} = '{3}'", strColNo, TypeHelper.ToString(dr[strColNo]), strColDesc, TypeHelper.ToString(dr[strColDesc])));

            if (drFilter != null && drFilter.Length > 0)
            {
                foreach (DataRow drTemp in drFilter)
                {
                    drTemp[strColDesc] = strTemp.Length > 0 ? bp.ToString() + ";" + strTemp : bp.ToString();
                }
                //drFilter[0][strColDesc] = strTemp.Length > 0 ? bp.ToString() + ";" + strTemp : bp.ToString();
            }

            SetPrintTextBox(dr);
        }

        /// <summary>
        /// 매출 TRAN 조회 전문
        /// </summary>
        /// <param name="responseData"></param>
        void pq04_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    var dataRecords = responseData.DataRecords.ToDataRecords<PQ04RespData>();
                    OnPQ04ReturnSuccess(dataRecords);
                });
            }
            else
            {
                MessageBar = responseData.Response.ErrorMessage;
                this.RunMode = RunModes.ModeNone;
            }
        }

        /// <summary>
        /// 매출 TRAN 조회 전문 에러
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="lastException"></param>
        void pq04_Errored(string errorMessage, Exception lastException)
        {
            MessageBar = strMsg24;
            this.RunMode = RunModes.ModeNone;
        }

        /// <summary>
        /// 영수증 리스트 조회 전문
        /// </summary>
        /// <param name="responseData"></param>
        void pq10_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    var data = responseData.DataRecords.ToDataRecords<PQ10RespData>();
                    OnPQ10ReturnSuccess(data);
                });
            }
            else
            {
                MessageBar = responseData.Response.ErrorMessage;
                this.RunMode = RunModes.ModeNone;
            }
        }

        /// <summary>
        /// 영수증 리스트 조회 전문 에러
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="lastException"></param>
        void pq10_Errored(string errorMessage, Exception lastException)
        {
            MessageBar = strMsg24;
            this.RunMode = RunModes.ModeNone;
        }

        /// <summary>
        /// PQ10 전문통신 성공
        /// </summary>
        /// <param name="data"></param>
        void OnPQ10ReturnSuccess(PQ10RespData[] data)
        {
            BasketHeader basketHeader = null;

            for (int i = 0; i < data.Length; i++)
            {
                basketHeader = (BasketHeader)BasketHeader.Parse(typeof(BasketHeader), data[i].HeaderDesc);

                if (basketHeader != null && basketHeader.Length > 0)
                {
                    if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_POS_OPEN)
                    {
                        #region 개설
                        GridAddRow(data[i].TrxnNo.Length > 6 ? data[i].TrxnNo.Substring(2, 4) : data[i].TrxnNo.ToString(4, TypeProperties.Number), basketHeader, strMsg01);
                        #endregion
                    }
                    else if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_POS_CLOSE)
                    {
                        #region 마감
                        GridAddRow(data[i].TrxnNo.Length > 6 ? data[i].TrxnNo.Substring(2, 4) : data[i].TrxnNo.ToString(4, TypeProperties.Number), basketHeader, strMsg07);
                        #endregion
                    }
                    else if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_SIGNON)
                    {
                        #region 사인ON
                        GridAddRow(data[i].TrxnNo.Length > 6 ? data[i].TrxnNo.Substring(2, 4) : data[i].TrxnNo.ToString(4, TypeProperties.Number), basketHeader, strMsg02);
                        #endregion
                    }
                    else if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_SIGNOFF)
                    {
                        #region 사인OFF
                        GridAddRow(data[i].TrxnNo.Length > 6 ? data[i].TrxnNo.Substring(2, 4) : data[i].TrxnNo.ToString(4, TypeProperties.Number), basketHeader, strMsg03);
                        #endregion
                    }
                    else if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_PRE_IO)
                    {
                        #region 준비금
                        GridAddRow(data[i].TrxnNo.Length > 6 ? data[i].TrxnNo.Substring(2, 4) : data[i].TrxnNo.ToString(4, TypeProperties.Number), basketHeader, strMsg04);
                        #endregion
                    }
                    else if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_MID_IO)
                    {
                        #region 중간입금
                        GridAddRow(data[i].TrxnNo.Length > 6 ? data[i].TrxnNo.Substring(2, 4) : data[i].TrxnNo.ToString(4, TypeProperties.Number), basketHeader, strMsg05);
                        #endregion
                    }
                    else if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_CLO_IO)
                    {
                        #region 마감입금
                        GridAddRow(data[i].TrxnNo.Length > 6 ? data[i].TrxnNo.Substring(2, 4) : data[i].TrxnNo.ToString(4, TypeProperties.Number), basketHeader, strMsg06);
                        #endregion
                    }
                    else if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_CAS_CALC)
                    {
                        #region 계산원정산
                        GridAddRow(data[i].TrxnNo.Length > 6 ? data[i].TrxnNo.Substring(2, 4) : data[i].TrxnNo.ToString(4, TypeProperties.Number), basketHeader, strMsg23);
                        #endregion
                    }
                    else if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_SALE || basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_OTH_SALE)
                    {
                        #region 상품판매, 저장물판매

                        string strMsgAdd = "";

                        //if (strMsgAdd.Length <= 0)
                        //{
                        //    strMsgAdd = strMsg20;
                        //}

                        if (basketHeader.CancType == "0")
                        {
                            strMsgAdd = strMsg08;
                        }
                        else if (basketHeader.CancType == "1")
                        {
                            strMsgAdd = strMsg21;
                        }
                        else if (basketHeader.CancType == "2")
                        {
                            strMsgAdd = strMsg09;
                        }

                        GridAddRow(data[i].TrxnNo.Length > 6 ? data[i].TrxnNo.Substring(2, 4) : data[i].TrxnNo.ToString(4, TypeProperties.Number), basketHeader, strMsgAdd);
                        #endregion
                    }
                    else if (basketHeader.TrxnType == NetCommConstants.TRXN_TYPE_TKS_PRS)
                    {
                        #region 사은품 회수

                        GridAddRow(data[i].TrxnNo.Length > 6 ? data[i].TrxnNo.Substring(2, 4) : data[i].TrxnNo.ToString(4, TypeProperties.Number), basketHeader, strMsg25);

                        #endregion
                    }
                }
            }

            // delete all existing data
            grd.ClearAll();

            //grd.SuspendDrawing();
            //그리드 추가
            foreach (DataRow dr in dsPrint.Tables[strDTNm].Rows)
            {
                grd.AddRow(dr);
            }

            // Complete loading list
            this.RunMode = RunModes.ModeNone;

            // select one trxn
            if (grd.RowCount > 0)
            {
                grd.SelectedRowIndex = 0;
                DoSelectTrxnRow();
            }
        }

        #endregion

        #region 프린트 확인

        /// <summary>
        /// 프린트 확인
        /// </summary>
        /// <returns></returns>
        private bool ChkPrint()
        {
            bool bReturn = false;
            string strErrMsg = string.Empty;

            try
            {
                if (POSDeviceManager.Printer.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
                {
                    if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.PowerClose)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_POWER;
                    }
                    else if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.CoverOpenned)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_OPENCOVER;
                    }
                    else if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.PaperEmpty)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_PAPER;
                    }
                    else if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.Closed)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_ERROR;
                    }
                    else
                    {
                        bReturn = true;
                    }
                }
                else
                {
                    strErrMsg = FXConsts.ERR_MSG_PRINTER_ERROR;
                }

                if (!bReturn)
                {
                    string[] strBtnNm = new string[2];
                    strBtnNm[0] = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00045");
                    strBtnNm[1] = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00190");

                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate()
                        {
                            if (ShowMessageBox(MessageDialogType.Question, null, strErrMsg, strBtnNm) == DialogResult.Yes)
                            {
                                POSDeviceManager.Printer.Open();
                                bReturn = ChkPrint();
                            }
                        });
                    }
                    else
                    {
                        if (ShowMessageBox(MessageDialogType.Question, null, strErrMsg, strBtnNm) == DialogResult.Yes)
                        {
                            POSDeviceManager.Printer.Open();
                            bReturn = ChkPrint();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }

            return bReturn;
        }

        #endregion

        #region 현금IC

        /// <summary>
        /// Loc added 10.30
        /// 난수 가져오기
        /// 여전법 변경 0617
        /// 
        /// 현금 IC 나중에 개발할것
        /// </summary>
        void RequestRandNumber()
        {
            // bEncCardNo = true;
            txtPrefixCode.Text = string.Empty;

            // 샘틀데이타
            // m_p013Presenter.ProcessGetRanNum("1000", "100");

            // 난수 받지 않고 진행
            string randNum = MakeRandomNumber();
            RequestICCardReader(randNum);
        }

        /// <summary>
        /// 난수 32글자, hexa
        /// </summary>
        /// <returns></returns>
        private string MakeRandomNumber()
        {
            StringBuilder sb = new StringBuilder();
            var rand = new Random();
            for (int i = 0; i < 32; i++)
            {
                int rn = rand.Next(15);
                sb.Append(rn.ToString("X"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 난수받아서 처리
        /// </summary>
        /// <param name="randNum"></param>
        void RequestICCardReader(string randNum)
        {
            // request to receice ic card no
            using (var pop = ChildManager.ShowPopup(string.Empty,
                "WSWD.WmallPos.POS.PY.dll", "WSWD.WmallPos.POS.PY.VC.POS_PY_P004", randNum))
            {
                var res = pop.ShowDialog(this);

                if (res == DialogResult.OK)
                {
                    txtPrefixCode.Text = pop.ReturnResult["IC_CARD_SEQ_NO"].ToString();
                    // 여전법 변경 0618
                    // 나중에 수정 할것
                    // bEncCardNo = false;
                }
            }
        }

        #region IPYP013View 멤버

        /// <summary>
        /// Loc added 10.30
        /// 난수 요청 중에 Progress 표시
        /// </summary>
        /// <param name="showProgress"></param>
        public void ShowProgressMessage(bool showProgress)
        {
            ChildManager.ShowProgress(showProgress, MSG_VAN_REQ_PROCESSING);
        }

        /// <summary>
        /// Loc added 10.30
        /// 난수 요청 할때 오류
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="errorMessage"></param>
        /// <param name="errorCode"></param>
        public void ShowErrorMessage(WSWD.WmallPos.POS.PY.Data.VANRequestErrorType errorType, string errorMessage, string errorCode)
        {
            // 여전법 변경 0618
            // 나중에 변경 할것
            // bEncCardNo = true;
            ShowProgressMessage(false);
            ShowMessageBox(MessageDialogType.Error, string.Empty, errorMessage);
        }

        /// <summary>
        /// Loc added 10.30
        /// 난수 받음
        /// </summary>
        /// <param name="respData"></param>
        public void OnReturnSuccess(WSWD.WmallPos.FX.Shared.NetComm.Response.PV.PV04RespData respData)
        {
            // 여전법 변경 0618
            // 나중에 변경 할것
            // bEncCardNo = true;
            ShowProgressMessage(false);
            if (PV04RespData.REQ_RAND_NUM.Equals(respData.TrxnType))
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        RequestICCardReader(respData.NoticeMessage2);
                    });
                }
                else
                {
                    RequestICCardReader(respData.NoticeMessage2);
                }
            }
        }

        #endregion

        #endregion

        #endregion

    }
}

public enum RunModes
{
    ModeNone,
    ModeSearchPrint,
    ModeSeletTrxn
}
