using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSWD.WmallPos.POS.VersionManager.Utils;
using System.IO;
using System.Xml;
using WSWD.WmallPos.POS.VersionManager.Control;
using System.Data.OracleClient;

namespace WSWD.WmallPos.POS.VersionManager
{
    public partial class frmMain : Form
    {
        #region 변수 설정

        /// <summary>
        /// Version.xml 로컬경로
        /// </summary>
        public string _strXmlPath = string.Empty;

        /// <summary>
        /// 설정파일들을 임시로 다운받을 로컬경로
        /// </summary>
        public string _strTempLocalPath = Application.StartupPath;

        /// <summary>
        /// 서버 경로
        /// </summary>
        public string _strServerRoot = string.Empty;

        /// <summary>
        /// 로컬 경로
        /// </summary>
        public string _strLocalRoot = string.Empty;

        /// <summary>
        /// POS,PDA
        /// </summary>
        public string _strProgram = string.Empty;

        /// <summary>
        /// 서버 리스트
        /// </summary>
        public string _strListKey = string.Empty;

        /// <summary>
        /// 서버 리스트
        /// </summary>
        public string _strListText = string.Empty;

        #endregion

        #region 생성자

        public frmMain()
        {
            InitializeComponent();

            //컨트롤 설정
            InitControl();

            //컨트롤 이벤트 셋팅
            InitEvent();
        }

        #endregion

        #region 컨트롤 설정 및 컨트롤 이벤트 셋팅

        /// <summary>
        /// 컨트롤 설정
        /// </summary>
        private void InitControl()
        {
            try
            {
                //각종 데이터 테이블 셋팅
                clsUtilsData.Instance.SetDataTable();

                //UserControl 설정
                SetUserControl(true, false, false, false, false, false);

                #region 업그레이드 프로그램 설정파일 확인

                bool bXmlAdd = false;
                string XMLPath = Path.Combine(_strTempLocalPath, @"XML\setup.xml");

                //XML확인
                if (!SetXml("setup", XMLPath, out bXmlAdd)) this.Close();

                DataSet dsSetXML = new DataSet();
                dsSetXML.ReadXml(XMLPath);

                if (dsSetXML != null && dsSetXML.Tables.Count > 0 && dsSetXML.Tables[0] != null && dsSetXML.Tables.Count > 0)
                {
                    //FTP설정
                    clsUtilsFTP.Instance.strServerIP = dsSetXML.Tables[0].Rows[0]["ip"].ToString();
                    clsUtilsFTP.Instance.strServerPort = dsSetXML.Tables[0].Rows[0]["port"].ToString();
                    clsUtilsFTP.Instance.strUserID = dsSetXML.Tables[0].Rows[0]["id"].ToString();
                    clsUtilsFTP.Instance.strUserPass = dsSetXML.Tables[0].Rows[0]["pass"].ToString();
                    _strServerRoot = dsSetXML.Tables[0].Rows[0]["serverroot"].ToString();
                    _strLocalRoot = dsSetXML.Tables[0].Rows[0]["localroot"].ToString();

                    //DB설정
                    clsUtilsOra.Instance._db_ip = dsSetXML.Tables[0].Rows[0]["db_ip"].ToString();
                    clsUtilsOra.Instance._db_port = dsSetXML.Tables[0].Rows[0]["db_port"].ToString();
                    clsUtilsOra.Instance._db_nm = dsSetXML.Tables[0].Rows[0]["db_nm"].ToString();
                    clsUtilsOra.Instance._db_id = dsSetXML.Tables[0].Rows[0]["db_id"].ToString();
                    clsUtilsOra.Instance._db_pass = dsSetXML.Tables[0].Rows[0]["db_pass"].ToString();
                }

                #endregion

                //업그레이드 프로그램 콤보박스 셋팅
                cboProgram.DropDownStyle = ComboBoxStyle.DropDownList;
                cboProgram.Items.AddRange(new object[] { clsUtilsSTRING.conServerPOS, clsUtilsSTRING.conServerPDA });
                cboProgram.SelectedIndex = 0;

                //업그레이드 일자 콤보박스 셋팅
                cboList.DropDownStyle = ComboBoxStyle.DropDownList;
                cboList.DisplayMember = "VersionText";
                cboList.ValueMember = "VersionNm";

                //업그레이드 일자 콤보박스 데이터 바인딩
                cboList.DataSource = clsUtilsData.Instance.dtList;

                //파일 관련 그리드 바인딩
                grdFile.DataSource = clsUtilsData.Instance.dtUpList;
                clsUtilsData.Instance.dtUpList.DefaultView.RowFilter = "VersionNm = '{0}'";

                SetInitControlConfig(cboAppSection, clsUtilsData.Instance.dtApp, grdApp);
                SetInitControlConfig(cboDevSection, clsUtilsData.Instance.dtDev, grdDev);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 컨트롤 이벤트 셋팅
        /// </summary>
        private void InitEvent()
        {
            try
            {
                //폼로드 이벤트
                Load += new EventHandler(form_Load);
                this.FormClosing += new FormClosingEventHandler(frmMain_FormClosing);

                //업그레이드 프로그램 콤보박스 변경 이벤트
                cboProgram.SelectedIndexChanged += new EventHandler(cboProgram_SelectedIndexChanged);

                //서버목록 리스트 콤보박스 변경 이벤트
                cboList.SelectedIndexChanged += new EventHandler(cboList_SelectedIndexChanged);

                //서버목록 트리뷰 선택 이벤트
                tv.AfterSelect += new TreeViewEventHandler(tv_AfterSelect);

                //조회, 저장, 다운로드
                btnSelect.Click += new EventHandler(btnSelect_Click);
                btnSave.Click += new EventHandler(btnSave_Click);
                btnDelete.Click += new EventHandler(btnDelete_Click);
                btnDownload.Click += new EventHandler(btnDownload_Click);

                //목록 관련 버튼
                btnTreeNew.Click += new EventHandler(btnTree_Click);            //버전 신규 발생

                //상세목록 관련 버튼
                btnFileAdd.Click += new EventHandler(btnFile_Click);            //버전별 각 폴더의 신규내용
                btnFileDelete.Click += new EventHandler(btnFile_Click);         //버전별 각 폴더의 내용삭제

                //SQL관련 유효성 검사
                cMst.txtMst.Validated += new EventHandler(txtSQL_Validated);
                cTran.txtTran.Validated += new EventHandler(txtSQL_Validated);

                //AppConfig.ini 관련 이벤트
                btnAddAppConfig.Click += new EventHandler(btnConfigAdd_Click);
                btnDeleteAppConfig.Click += new EventHandler(btnConfigDelete_Click);
                cboAppSection.SelectedIndexChanged += new EventHandler(cboConfigSection_SelectedIndexChanged);
                cboAppKey.SelectedIndexChanged += new EventHandler(cboConfigKey_SelectedIndexChanged);
                cboAppStore.SelectedIndexChanged += new EventHandler(cboConfigStore_SelectedIndexChanged);
                grdApp.CellContentClick += new DataGridViewCellEventHandler(grdConfig_CellContentClick);

                //DevConfig.ini 관련 이벤트
                btnAddDevConfig.Click += new EventHandler(btnConfigAdd_Click);
                btnDeleteDevConfig.Click += new EventHandler(btnConfigDelete_Click);
                cboDevSection.SelectedIndexChanged += new EventHandler(cboConfigSection_SelectedIndexChanged);
                cboDevKey.SelectedIndexChanged += new EventHandler(cboConfigKey_SelectedIndexChanged);
                cboDevStore.SelectedIndexChanged += new EventHandler(cboConfigStore_SelectedIndexChanged);
                grdDev.CellContentClick += new DataGridViewCellEventHandler(grdConfig_CellContentClick);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        #endregion

        #region 이벤트 정의

        /// <summary>
        /// 폼 로드 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void form_Load(object sender, EventArgs e)
        {
            try
            {
                //FTP 서버에서 목록 조회
                cboProgram_SelectedIndexChanged(cboProgram, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 폼종료 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ChkSaveList())
            {
                frmPopUp frmPop = new frmPopUp(_strListText + " 변경 내역이 존재합니다.", "");

                if (frmPop.ShowDialog() == DialogResult.OK)
                {
                    SaveData(false, "close", "");
                    return;
                }
                else
                {
                    DeleteSaveList();
                }
            }
        }

        #region 콤보박스 이벤트

        /// <summary>
        /// 업그레이드 프로그램 콤보박스 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cboProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChkSaveList())
            {
                frmPopUp frmPop = new frmPopUp(_strListText + " 변경 내역이 존재합니다.", "");

                if (frmPop.ShowDialog() == DialogResult.OK)
                {
                    SaveData(false, "program", cboProgram.SelectedItem.ToString());
                    return;
                }
                else
                {
                    DeleteSaveList();
                }
            }

            if (cboProgram == null || cboProgram.Items.Count <= 0) return;

            _strProgram = cboProgram.Text;

            //FTP 서버에서 목록 조회
            btnSelect_Click(btnSelect, null);
        }

        /// <summary>
        /// 서버목록 리스트 콤보박스 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cboList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChkSaveList())
            {
                frmPopUp frmPop = new frmPopUp(_strListText + " 변경 내역이 존재합니다.", "");

                if (frmPop.ShowDialog() == DialogResult.OK)
                {
                    SaveData(false, "list", cboList.SelectedValue != null ? cboList.SelectedValue.ToString() : "");
                    return;
                }
                else
                {
                    DeleteSaveList();
                }
            }

            if (_strProgram.Length < 0 || cboList == null || cboList.Items.Count < 0) return;

            //변경내역 조회
            //if (ChkSaveList()) return;
            SetControlEnable(false);

            try
            {
                //컨트롤 초기화
                SetControl(true);

                _strListKey = cboList.SelectedValue != null ? cboList.SelectedValue.ToString() : "";
                _strListText = cboList.Text;

                if (_strListKey.Length <= 0) return;

                SetUserControl(true);
                cStatus.ChangeStatus(_strListText, clsUtilsEnum.eProcess.eSelect);

                DataRow[] drFilter = clsUtilsData.Instance.dtList.Select(string.Format("VersionNm = '{0}'", _strListKey));

                bool bExists = false;

                if (drFilter != null && drFilter.Length > 0)
                {
                    //xml에 데이터 있는지 확인
                    string strTempServerPath = string.Format("{0}/{1}/{2}", _strServerRoot, _strProgram, clsUtilsSTRING.conVersion);
                    string strTempLocalPath = string.Format("{0}/{1}/{2}/{3}", Application.StartupPath, _strLocalRoot, _strServerRoot, _strProgram);

                    if (clsUtilsFTP.Instance.FtpIsExistsFile(strTempServerPath))
                    {
                        if (clsUtilsFTP.Instance.FtpDownload(strTempServerPath, strTempLocalPath, clsUtilsSTRING.conVersionCopy))
                        {
                            DataSet _dsXML = new DataSet();
                            _dsXML.ReadXml(string.Format("{0}/{1}{2}/{3}/{4}", Application.StartupPath, _strLocalRoot, _strServerRoot, _strProgram, clsUtilsSTRING.conVersionCopy));

                            if (_dsXML != null && _dsXML.Tables.Count > 0 && _dsXML.Tables[0] != null)
                            {
                                DataRow[] drXMLFilter = _dsXML.Tables[0].Select(string.Format("{0} = '{1}'", _dsXML.Tables[0].Columns[0].ColumnName.ToString(), _strListKey));

                                if (drXMLFilter != null && drXMLFilter.Length > 0)
                                {
                                    bExists = true;

                                    //FTP 서버에서 목록 조회
                                    List<clsDirectoryItem> sublist = clsUtilsFTP.Instance.FtpGetList(string.Format("{0}/{1}/{2}", _strServerRoot, _strProgram, _strListKey), true);

                                    if (sublist != null && sublist.Count > 0)
                                    {
                                        foreach (clsDirectoryItem item in sublist)
                                        {
                                            string[] arrDepth = item.ParentNm.Split('/');
                                            DataRow NewDr = clsUtilsData.Instance.dtUpList.NewRow();
                                            NewDr["DeleteYN"] = clsUtilsSTRING.conN;
                                            NewDr["VersionNm"] = _strListKey;
                                            NewDr["ServerDirectory"] = item.ParentNm;
                                            NewDr["LocalDirectory"] = "";
                                            NewDr["DirectoryDepth01"] = arrDepth != null && arrDepth.Length >= 2 ? arrDepth[1] : "";
                                            NewDr["DirectoryDepth02"] = arrDepth != null && arrDepth.Length >= 3 ? arrDepth[2] : "";
                                            NewDr["DirectoryDepth03"] = arrDepth != null && arrDepth.Length >= 4 ? arrDepth[3] : "";
                                            NewDr["DirectoryDepth04"] = arrDepth != null && arrDepth.Length >= 5 ? arrDepth[4] : "";
                                            NewDr["DirectoryDepth05"] = arrDepth != null && arrDepth.Length >= 6 ? arrDepth[5] : "";
                                            NewDr["DirectoryDepth06"] = arrDepth != null && arrDepth.Length >= 7 ? arrDepth[6] : "";
                                            NewDr["FileNm"] = item.Name;
                                            NewDr["FileYN"] = item.IsDirectory ? clsUtilsSTRING.conN : clsUtilsSTRING.conY;
                                            NewDr["DateCreated"] = item.DateCreated;
                                            NewDr["FileSize"] = item.Size.Length > 0 ? string.Format("{0:#,##0}KB", item.Size) : "";
                                            NewDr["ChangeYN"] = clsUtilsEnum.eChange.Normal;

                                            if (!item.IsDirectory &&
                                                (item.Name == clsUtilsSTRING.conAppConfig || item.Name == clsUtilsSTRING.conDevConfig ||
                                                item.Name == clsUtilsSTRING.conMstChanges || item.Name == clsUtilsSTRING.conTranChanges))
                                            {
                                                //설정파일들은 임시폴더에 다운로드
                                                if (!clsUtilsFTP.Instance.FtpDownload(
                                                    string.Format("{0}/{1}", item.ParentNm, item.Name),
                                                    string.Format("{0}/{1}{2}", _strTempLocalPath, _strLocalRoot, item.ParentNm),
                                                    item.Name))
                                                {
                                                    MessageBox.Show("다운로드 실패");
                                                }
                                                else
                                                {
                                                    string strLocalPath = string.Format("{0}/{1}{2}", _strTempLocalPath, _strLocalRoot, item.ParentNm);
                                                    NewDr["LocalDirectory"] = strLocalPath;

                                                    if (item.Name == clsUtilsSTRING.conAppConfig || item.Name == clsUtilsSTRING.conDevConfig)
                                                    {
                                                        string line = string.Empty;
                                                        string strTempSection = string.Empty;
                                                        string strTempKey = string.Empty;
                                                        string strTemp = string.Empty;

                                                        if (item.Name == clsUtilsSTRING.conAppConfig)
                                                        {
                                                            if (clsUtilsData.Instance.dtApp != null)
                                                            {
                                                                StreamReader file = new StreamReader(Path.Combine(strLocalPath, item.Name));
                                                                while ((line = file.ReadLine()) != null)
                                                                {
                                                                    strTempKey = string.Empty;
                                                                    strTemp = string.Empty;

                                                                    if (line == string.Format("[{0}]", clsUtilsSTRING.conINIPosComm))
                                                                    {
                                                                        strTempSection = clsUtilsSTRING.conINIPosComm;
                                                                    }
                                                                    else if (line == string.Format("[{0}]", clsUtilsSTRING.conINIPosFTP))
                                                                    {
                                                                        strTempSection = clsUtilsSTRING.conINIPosFTP;
                                                                    }
                                                                    else if (line == string.Format("[{0}]", clsUtilsSTRING.conINIPosVan))
                                                                    {
                                                                        strTempSection = clsUtilsSTRING.conINIPosVan;
                                                                    }
                                                                    else if (line == string.Format("[{0}]", clsUtilsSTRING.conINIPosOption))
                                                                    {
                                                                        strTempSection = clsUtilsSTRING.conINIPosOption;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (!line.Contains('='))
                                                                        {
                                                                            strTempSection = string.Empty;
                                                                        }
                                                                        else
                                                                        {
                                                                            strTempKey = line.ToString().Split('=')[0];
                                                                            strTemp = line.ToString().Split('=')[1];
                                                                        }
                                                                    }

                                                                    if (strTempSection.Length > 0 && strTempKey.Length > 0 && strTemp.Length > 0)
                                                                    {
                                                                        string[] strNmValue = strTemp.ToString().Split(']');
                                                                        string strTempStroeNo = strNmValue != null && strNmValue.Length > 0 ? strNmValue[0].Substring(strNmValue[0].IndexOf('[') + 1, strNmValue[0].Length - strNmValue[0].IndexOf('[') - 1) : ""; //점포구분
                                                                        string strTempPosNo = strNmValue != null && strNmValue.Length > 1 ? strNmValue[1].Substring(strNmValue[1].IndexOf('[') + 1, strNmValue[1].Length - strNmValue[1].IndexOf('[') - 1) : "";   //포스구분
                                                                        string strTempValue = strNmValue != null && strNmValue.Length > 2 ? strNmValue[2] : "";

                                                                        if (strTempStroeNo.Length > 0 && strTempPosNo.Length > 0 && strTempValue.Length > 0)
                                                                        {
                                                                            clsUtilsData.Instance.ConfigAddrow(clsUtilsData.Instance.dtApp, strTempSection, strTempKey, strTempValue, strTempStroeNo, strTempPosNo);
                                                                        }
                                                                    }
                                                                }

                                                                file.Close();
                                                            }
                                                        }
                                                        else if (item.Name == clsUtilsSTRING.conDevConfig)
                                                        {
                                                            if (clsUtilsData.Instance.dtApp != null)
                                                            {
                                                                StreamReader file = new StreamReader(Path.Combine(strLocalPath, item.Name));
                                                                while ((line = file.ReadLine()) != null)
                                                                {
                                                                    strTempKey = string.Empty;
                                                                    strTemp = string.Empty;

                                                                    if (line == string.Format("[{0}]", clsUtilsSTRING.conINIScannerGun))
                                                                    {
                                                                        strTempSection = clsUtilsSTRING.conINIScannerGun;
                                                                    }
                                                                    else if (line == string.Format("[{0}]", clsUtilsSTRING.conINILineDisplay))
                                                                    {
                                                                        strTempSection = clsUtilsSTRING.conINILineDisplay;
                                                                    }
                                                                    else if (line == string.Format("[{0}]", clsUtilsSTRING.conINIPrinter))
                                                                    {
                                                                        strTempSection = clsUtilsSTRING.conINIPrinter;
                                                                    }
                                                                    else if (line == string.Format("[{0}]", clsUtilsSTRING.conINIMSR))
                                                                    {
                                                                        strTempSection = clsUtilsSTRING.conINIMSR;
                                                                    }
                                                                    else if (line == string.Format("[{0}]", clsUtilsSTRING.conINICashDrawer))
                                                                    {
                                                                        strTempSection = clsUtilsSTRING.conINICashDrawer;
                                                                    }
                                                                    else if (line == string.Format("[{0}]", clsUtilsSTRING.conINISignPad))
                                                                    {
                                                                        strTempSection = clsUtilsSTRING.conINISignPad;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (!line.Contains('='))
                                                                        {
                                                                            strTempSection = string.Empty;
                                                                        }
                                                                        else
                                                                        {
                                                                            strTempKey = line.ToString().Split('=')[0];
                                                                            strTemp = line.ToString().Split('=')[1];
                                                                        }
                                                                    }

                                                                    if (strTempSection.Length > 0 && strTempKey.Length > 0 && strTemp.Length > 0)
                                                                    {
                                                                        string[] strNmValue = strTemp.ToString().Split(']');
                                                                        string strTempStroeNo = strNmValue != null && strNmValue.Length > 0 ? strNmValue[0].Substring(strNmValue[0].IndexOf('[') + 1, strNmValue[0].Length - strNmValue[0].IndexOf('[') - 1) : ""; //점포구분
                                                                        string strTempPosNo = strNmValue != null && strNmValue.Length > 1 ? strNmValue[1].Substring(strNmValue[1].IndexOf('[') + 1, strNmValue[1].Length - strNmValue[1].IndexOf('[') - 1) : "";   //포스구분
                                                                        string strTempValue = strNmValue != null && strNmValue.Length > 2 ? strNmValue[2] : "";

                                                                        if (strTempStroeNo.Length > 0 && strTempPosNo.Length > 0 && strTempValue.Length > 0)
                                                                        {
                                                                            clsUtilsData.Instance.ConfigAddrow(clsUtilsData.Instance.dtDev, strTempSection, strTempKey, strTempValue, strTempStroeNo, strTempPosNo);
                                                                        }
                                                                    }
                                                                }

                                                                file.Close();
                                                            }
                                                        }
                                                    }
                                                    else if (item.Name == clsUtilsSTRING.conMstChanges || item.Name == clsUtilsSTRING.conTranChanges)
                                                    {
                                                        DataTable dt = clsUtilsData.Instance.dsSQL.Tables[item.Name];
                                                        strLocalPath = string.Format("{0}/{1}", strLocalPath, item.Name);
                                                        GetSQLValue(strLocalPath, dt);
                                                    }
                                                }
                                            }

                                            clsUtilsData.Instance.dtUpList.Rows.Add(NewDr);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //트리뷰 기본 셋팅
                    SetTreeView(_strListKey, _strListText, drFilter[0]["ServerPath"].ToString());

                    //그리드 바인딩


                    if (tv != null && tv.Nodes.Count > 0)
                    {
                        tv.SelectedNode = tv.Nodes[0].Nodes[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                SetControlEnable(true);
                cStatus.ChangeStatus(_strListText, clsUtilsEnum.eProcess.eNormal);
            }
        }

        /// <summary>
        /// AppConfig.ini 및 DevConfig.ini 분류 콤보박스 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cboConfigSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBox cbo = (ComboBox)sender;
                ComboBox cboKey = cbo.Name.ToString() == cboAppSection.Name.ToString() ? cboAppKey : cboDevKey;

                DataTable dtKey = new DataTable();
                dtKey.Columns.Add("Id");
                dtKey.Columns.Add("Name");
                dtKey.Rows.Add(new object[] { "", "" });

                if (cbo.SelectedValue != null && cbo.SelectedValue.ToString().Length > 0)
                {
                    if (cbo.Name.ToString() == cboAppSection.Name.ToString())
                    {
                        switch (cbo.SelectedValue.ToString())
                        {
                            case clsUtilsSTRING.conINIPosComm01:
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommSvrIP1, clsUtilsSTRING.conPosCommSvrIP1Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComDPort1, clsUtilsSTRING.conPosCommComDPort1Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComUPort1, clsUtilsSTRING.conPosCommComUPort1Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComQPort1, clsUtilsSTRING.conPosCommComQPort1Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommSvrIP2, clsUtilsSTRING.conPosCommSvrIP2Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComDPort2, clsUtilsSTRING.conPosCommComDPort2Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComUPort2, clsUtilsSTRING.conPosCommComUPort2Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComQPort2, clsUtilsSTRING.conPosCommComQPort2Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComDTimeOut, clsUtilsSTRING.conPosCommComDTimeOutNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComUTimeOut, clsUtilsSTRING.conPosCommComUTimeOutNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComQTimeOut, clsUtilsSTRING.conPosCommComQTimeOutNm });
                                break;
                            case clsUtilsSTRING.conINIPosComm02:
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommSvrGftIP1, clsUtilsSTRING.conPosCommSvrGftIP1Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComGPort1, clsUtilsSTRING.conPosCommComGPort1Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommSvrGftIP2, clsUtilsSTRING.conPosCommSvrGftIP2Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComGPort2, clsUtilsSTRING.conPosCommComGPort2Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComGTimeOut, clsUtilsSTRING.conPosCommComGTimeOutNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommSvrPntIP1, clsUtilsSTRING.conPosCommSvrPntIP1Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComPPort1, clsUtilsSTRING.conPosCommComPPort1Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommSvrPntIP2, clsUtilsSTRING.conPosCommSvrPntIP2Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComPPort2, clsUtilsSTRING.conPosCommComPPort2Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComPTimeOut, clsUtilsSTRING.conPosCommComPTimeOutNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommHqSvrIP1, clsUtilsSTRING.conPosCommHqSvrIP1Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommHqComQPort1, clsUtilsSTRING.conPosCommHqComQPort1Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommHqComUPort1, clsUtilsSTRING.conPosCommHqComUPort1Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommHqSvrIP2, clsUtilsSTRING.conPosCommHqSvrIP2Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommHqComQPort2, clsUtilsSTRING.conPosCommHqComQPort2Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommHqComUPort2, clsUtilsSTRING.conPosCommHqComUPort2Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommHqComQTimeOut, clsUtilsSTRING.conPosCommHqComQTimeOutNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommHqComUTimeOut, clsUtilsSTRING.conPosCommHqComUTimeOutNm });

                                break;
                            case clsUtilsSTRING.conINIPosFTP:
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPFtpSvrIP1, clsUtilsSTRING.conPosFTPFtpSvrIP1Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPFtpComPort1, clsUtilsSTRING.conPosFTPFtpComPort1Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPFtpSvrIP2, clsUtilsSTRING.conPosFTPFtpSvrIP2Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPFtpComPort2, clsUtilsSTRING.conPosFTPFtpComPort2Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPMode, clsUtilsSTRING.conPosFTPModeNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPUser, clsUtilsSTRING.conPosFTPUserNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPPass, clsUtilsSTRING.conPosFTPPassNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPJournalPath, clsUtilsSTRING.conPosFTPJournalPathNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPVersionInfoPath, clsUtilsSTRING.conPosFTPVersionInfoPathNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPDataFileDownloadPath, clsUtilsSTRING.conPosFTPDataFileDownloadPathNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPCreateUploadPathByDate, clsUtilsSTRING.conPosFTPCreateUploadPathByDateNm });
                                break;
                            case clsUtilsSTRING.conINIPosVan:
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosVANVanSvrIP1, clsUtilsSTRING.conPosVANVanSvrIP1Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosVANVanComPort1, clsUtilsSTRING.conPosVANVanComPort1Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosVANVanSvrIP2, clsUtilsSTRING.conPosVANVanSvrIP2Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosVANVanComPort2, clsUtilsSTRING.conPosVANVanComPort2Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosVANComTimeOut, clsUtilsSTRING.conPosVANComTimeOutNm });
                                break;
                            case clsUtilsSTRING.conINIPosOption:
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionPointUse, clsUtilsSTRING.conPosOptionPointUseNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionPointSchemePrefix, clsUtilsSTRING.conPosOptionPointSchemePrefixNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionPointPayKeyInputEnable, clsUtilsSTRING.conPosOptionPointPayKeyInputEnableNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionCashReceiptUse, clsUtilsSTRING.conPosOptionCashReceiptUseNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionCashReceiptIssue, clsUtilsSTRING.conPosOptionCashReceiptIssueNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionCashReceiptApplAmount, clsUtilsSTRING.conPosOptionCashReceiptApplAmountNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionGoodsCodePrefix1, clsUtilsSTRING.conPosOptionGoodsCodePrefix1Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionGoodsCodePrefix2, clsUtilsSTRING.conPosOptionGoodsCodePrefix2Nm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionDataKeepDays, clsUtilsSTRING.conPosOptionDataKeepDaysNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionAutoReturnCardRead, clsUtilsSTRING.conPosOptionAutoReturnCardReadNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionSalesReturn, clsUtilsSTRING.conPosOptionSalesReturnNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionSignUploadTask, clsUtilsSTRING.conPosOptionSignUploadTaskNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionTransUploadTask, clsUtilsSTRING.conPosOptionTransUploadTaskNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionTransStatusTask, clsUtilsSTRING.conPosOptionTransStatusTaskNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionNoticeStatusTask, clsUtilsSTRING.conPosOptionNoticeStatusTaskNm });
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        dtKey.Rows.Add(new object[] { clsUtilsSTRING.conUse, clsUtilsSTRING.conUseNm });
                        dtKey.Rows.Add(new object[] { clsUtilsSTRING.conMethod, 
                        cbo.SelectedValue.ToString() == clsUtilsSTRING.conINIMSR || cbo.SelectedValue.ToString() == clsUtilsSTRING.conINICashDrawer ? clsUtilsSTRING.conMethodNm02 : clsUtilsSTRING.conMethodNm01 });
                        dtKey.Rows.Add(new object[] { clsUtilsSTRING.conLogicalName, clsUtilsSTRING.conLogicalNameNm });

                        if (cbo.SelectedValue.ToString() == clsUtilsSTRING.conINIScannerGun ||
                            cbo.SelectedValue.ToString() == clsUtilsSTRING.conINILineDisplay ||
                            cbo.SelectedValue.ToString() == clsUtilsSTRING.conINIPrinter ||
                            cbo.SelectedValue.ToString() == clsUtilsSTRING.conINISignPad)
                        {
                            dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPort, clsUtilsSTRING.conPortNm });
                            dtKey.Rows.Add(new object[] { clsUtilsSTRING.conSpeed, clsUtilsSTRING.conSpeedNm });
                            dtKey.Rows.Add(new object[] { clsUtilsSTRING.conDataBit, clsUtilsSTRING.conDataBitNm });
                            dtKey.Rows.Add(new object[] { clsUtilsSTRING.conStopBit, clsUtilsSTRING.conStopBitNm });
                            dtKey.Rows.Add(new object[] { clsUtilsSTRING.conParity, clsUtilsSTRING.conParityNm });
                            dtKey.Rows.Add(new object[] { clsUtilsSTRING.conFlowControl, clsUtilsSTRING.conFlowControlNm });

                            if (cbo.SelectedValue.ToString() == clsUtilsSTRING.conINIPrinter)
                            {
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conLogoBMP, clsUtilsSTRING.conLogoBMPNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conCutFeedCn, clsUtilsSTRING.conCutFeedCnNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conBarCodeWidth, clsUtilsSTRING.conBarCodeWidthNm });
                                dtKey.Rows.Add(new object[] { clsUtilsSTRING.conBarCodeHeight, clsUtilsSTRING.conBarCodeHeightNm });
                            }
                        }
                    }
                }

                if (cboKey != null && cboKey.DataSource != null)
                {
                    cboKey.DataSource = null;
                }

                cboKey.DataSource = dtKey;
                cboKey.DisplayMember = "Name";
                cboKey.ValueMember = "Id";
                cboKey.SelectedIndex = 0;
                cboConfigKey_SelectedIndexChanged(cboKey, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// AppConfig.ini 및 DevConfig.ini 항목 콤보박스 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cboConfigKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBox cbo = (ComboBox)sender;
                ComboBox cboTempStore = null;

                TextBox txt = null;

                if (cbo.Name.ToString() == cboAppKey.Name.ToString())
                {
                    txt = txtAppInput;
                    cboTempStore = cboAppStore;
                }
                else
                {
                    txt = txtDevInput;
                    cboTempStore = cboDevStore;
                }

                txt.Text = "";

                if (cboTempStore == null || cboTempStore.Items.Count <= 0)
                {
                    InitControlConfig(cboTempStore);
                }

                cboTempStore.SelectedIndex = 0;
                cboConfigStore_SelectedIndexChanged(cboTempStore, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cboConfigStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBox cbo = (ComboBox)sender;
                InitControlConfig(cbo.Name.ToString() == cboAppStore.Name.ToString() ? cboAppPos : cboDevPos);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        #endregion

        #region 트리뷰 이벤트

        /// <summary>
        /// 서버목록 트리뷰 선택 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {
            groupList.Text = clsUtilsSTRING.conGroupListNm;

            if (_strProgram.Length < 0 || _strListKey.Length < 0 || e.Node == null) return;

            try
            {
                TreeNode node = e.Node;
                TreeNode ParentNode = ParentNodes(node);

                string strTempNm = node.Name;

                if (strTempNm == clsUtilsSTRING.conBin || strTempNm == clsUtilsSTRING.conLib || strTempNm == clsUtilsSTRING.conDamo || strTempNm == clsUtilsSTRING.conOcx ||
                    strTempNm == clsUtilsSTRING.conConfig || strTempNm == clsUtilsSTRING.conMst ||
                    strTempNm == clsUtilsSTRING.conSchema || strTempNm == clsUtilsSTRING.conTran || strTempNm == clsUtilsSTRING.conResource ||
                    strTempNm == clsUtilsSTRING.conUpdate)
                {
                    //UserControl 설정
                    SetUserControl(true, true, false, false, false, false);

                    string strFilter =
                        node == ParentNode ? "VersionNm = ''" :
                            string.Format("ServerDirectory = '{0}' and FileYN = '{1}' and ChangeYN <> '{2}'",
                            string.Format("{0}/{1}", node.Tag.ToString(), node.Name),
                            clsUtilsSTRING.conY,
                            clsUtilsEnum.eChange.Delete.ToString());

                    ((DataTable)grdFile.DataSource).DefaultView.RowFilter = strFilter;
                }
                else
                {
                    //UserControl 설정
                    SetUserControl(true, false,
                        strTempNm == clsUtilsSTRING.conMstChanges ? true : false,
                        strTempNm == clsUtilsSTRING.conTranChanges ? true : false,
                        strTempNm == clsUtilsSTRING.conAppConfig ? true : false,
                        strTempNm == clsUtilsSTRING.conDevConfig ? true : false);
                }

                groupList.Text = strTempNm.Length > 0 ? string.Format("[{0}] {1}", strTempNm, clsUtilsSTRING.conGroupListNm) : clsUtilsSTRING.conGroupListNm;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        #endregion

        #region 버튼 이벤트

        /// <summary>
        /// 업그레이드 일자별 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSelect_Click(object sender, EventArgs e)
        {
            if (ChkSaveList())
            {
                frmPopUp frmPop = new frmPopUp(_strListText + " 변경 내역이 존재합니다.", "");

                if (frmPop.ShowDialog() == DialogResult.OK)
                {
                    SaveData(false, "select", "");
                    return;
                }
                else
                {
                    DeleteSaveList();
                }
            }

            //컨트롤 초기화
            SetControl(true);

            if (_strProgram.Length <= 0) return;

            try
            {
                SetUserControl(true, false, false, false, false, false);

                //서버 리스트 초기화
                if (clsUtilsData.Instance.dtList != null)
                {
                    cboList.SelectedIndexChanged -= new EventHandler(cboList_SelectedIndexChanged);
                    clsUtilsData.Instance.dtList.Clear();
                    cboList.SelectedIndexChanged += new EventHandler(cboList_SelectedIndexChanged);
                }

                //로컬 version.xml다운로드 임시저장 경로
                _strXmlPath = string.Format("{0}/{1}{2}/{3}/{4}", Application.StartupPath, _strLocalRoot, _strServerRoot, _strProgram, clsUtilsSTRING.conVersion);

                //서버디렉토리
                string strTempServerPath = string.Format("{0}/{1}/{2}", _strServerRoot, _strProgram, clsUtilsSTRING.conVersion);

                //로컬디렉토리
                string strTempLocalPath = string.Format("{0}/{1}/{2}/{3}", Application.StartupPath, _strLocalRoot, _strServerRoot, _strProgram);

                //version.xml다운로드
                if (clsUtilsFTP.Instance.FtpDownload(strTempServerPath, strTempLocalPath, clsUtilsSTRING.conVersion))
                {
                    //version.xml읽기
                    DataSet _dsXML = new DataSet();
                    _dsXML.ReadXml(_strXmlPath);

                    if (_dsXML != null && _dsXML.Tables.Count > 0 && _dsXML.Tables[0] != null && _dsXML.Tables.Count > 0)
                    {
                        foreach (DataRow drXML in _dsXML.Tables[0].Rows)
                        {
                            clsUtilsData.Instance.AddRowList(
                                drXML[0].ToString(),
                                string.Format("{0}/{1}/{2} [{3}]",
                                drXML[0].ToString().Substring(0, 4),
                                drXML[0].ToString().Substring(4, 2),
                                drXML[0].ToString().Substring(6, 2),
                                drXML[0].ToString().Substring(8, 3)),
                                string.Format("{0}/{1}", _strServerRoot, _strProgram),
                                clsUtilsEnum.eChange.Normal.ToString()
                                );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                if (cboList != null && cboList.Items.Count > 0)
                {
                    cboList.SelectedIndex = 0;
                    cboList_SelectedIndexChanged(cboList, null);
                }
                else
                {
                    _strListKey = "";
                    _strListText = "";
                }
            }
        }

        /// <summary>
        /// 업그레이드 일자별 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            SaveData(true, "", "");
        }

        /// <summary>
        /// 업그레이드 정보 삭제 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnDelete_Click(object sender, EventArgs e)
        {
            bool bSuccess = false;
            string strTempText = string.Empty;

            SetControlEnable(false);

            try
            {
                DataRow[] drFilter = clsUtilsData.Instance.dtList.Select(string.Format("VersionNm = '{0}'", _strListKey));

                if (tv == null || tv.Nodes.Count <= 0 || clsUtilsData.Instance.dtList == null || clsUtilsData.Instance.dtList.Rows.Count <= 0 || drFilter != null && drFilter.Length <= 0)
                {
                    MessageBox.Show("삭제할 자료가 존재하지 않습니다.");
                    return;
                }

                if (drFilter != null && drFilter.Length > 0)
                {
                    frmPopUp frm = new frmPopUp(_strListText, "업그레이드 정보 서버에서 삭제 하겠습니까?");

                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    SetUserControl(true);
                    cStatus.ChangeStatus(_strListText, clsUtilsEnum.eProcess.eDelete);

                    strTempText = _strListText;

                    if (drFilter[0]["ChangeYN"].ToString() == clsUtilsEnum.eChange.New.ToString())
                    {
                        clsUtilsData.Instance.dtUpList.Clear();

                        cboList.SelectedIndexChanged -= new EventHandler(cboList_SelectedIndexChanged);
                        clsUtilsData.Instance.dtList.Rows.Remove(drFilter[0]);
                        cboList.SelectedIndexChanged += new EventHandler(cboList_SelectedIndexChanged);

                        _strListKey = "";
                        _strListText = "";

                        if (cboList.Items.Count > 0)
                        {
                            cboList.SelectedIndex = 0;
                            cboList_SelectedIndexChanged(cboList, null);
                        }
                        else
                        {
                            SetControl(true);
                        }

                        bSuccess = true;
                    }
                    else
                    {
                        if (!clsUtilsFTP.Instance.FtpDeleteAll(string.Format("{0}/{1}", drFilter[0]["ServerPath"].ToString(), _strListKey)))
                        {
                            MessageBox.Show(_strListKey + clsUtilsSTRING.conFTPDeleteFail);
                            return;
                        }
                        else
                        {
                            bool bAdd = false;
                            drFilter[0]["ChangeYN"] = clsUtilsEnum.eChange.Delete.ToString();
                            if (SetXml("versions", _strXmlPath, out bAdd))
                            {
                                if (bAdd)
                                {
                                    if (!clsUtilsFTP.Instance.FtpUpload(_strXmlPath, string.Format("{0}/{1}/{2}", _strServerRoot, _strProgram, clsUtilsSTRING.conVersion)))
                                    {
                                        MessageBox.Show(clsUtilsSTRING.conFTPUploadFail);
                                        return;
                                    }
                                }
                                else
                                {
                                    if (!clsUtilsFTP.Instance.FtpDelete(string.Format("{0}/{1}/{2}", _strServerRoot, _strProgram, clsUtilsSTRING.conVersion), false))
                                    {
                                        MessageBox.Show(clsUtilsSTRING.conFTPUploadFail);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                return;
                            }

                            cboList.SelectedIndexChanged -= new EventHandler(cboList_SelectedIndexChanged);
                            clsUtilsData.Instance.dtList.Rows.Remove(drFilter[0]);
                            cboList.SelectedIndexChanged += new EventHandler(cboList_SelectedIndexChanged);

                            _strListKey = "";
                            _strListText = "";

                            if (cboList.Items.Count > 0)
                            {
                                cboList.SelectedIndex = 0;
                                cboList_SelectedIndexChanged(cboList, null);
                            }
                            else
                            {
                                SetControl(true);
                            }

                            bSuccess = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                SetControlEnable(true);

                if (bSuccess)
                {
                    MessageBox.Show(strTempText + " 업그레이드 서버 정보 삭제 완료.");
                }
            }
        }

        /// <summary>
        /// 다운로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnDownload_Click(object sender, EventArgs e)
        {
            if (cboProgram == null || cboProgram.Items.Count <= 0 || cboList == null || cboList.Items.Count <= 0)
            {
                return;
            }

            SetControlEnable(false);

            bool bSuccess = false;

            try
            {
                int iSaveCnt = 0;
                int iTotalCnt = 0;
                string strSaveNm = string.Empty;

                SetUserControl(false);
                cProgress.ChangeProgress(clsUtilsSTRING.conProgressDownloadStatus, iSaveCnt, iTotalCnt, strSaveNm, clsUtilsEnum.eProcess.eNormal, false);

                //FTP 서버에서 목록에 해당하는 파일 다운
                if (cboList.SelectedValue != null && cboList.SelectedValue.ToString().Length > 0)
                {
                    List<clsDirectoryItem> sublist = clsUtilsFTP.Instance.FtpGetList(string.Format("{0}/{1}/{2}", _strServerRoot, _strProgram, _strListKey), true);

                    if (sublist != null && sublist.Count > 0)
                    {
                        foreach (clsDirectoryItem item in sublist)
                        {
                            if (!item.IsDirectory)
                            {
                                iTotalCnt++;
                            }
                        }

                        if (iTotalCnt > 0)
                        {
                            cProgress.ChangeProgress(clsUtilsSTRING.conProgressDownloadStatus, iSaveCnt, iTotalCnt, strSaveNm, clsUtilsEnum.eProcess.eDownload, false);

                            foreach (clsDirectoryItem item in sublist)
                            {
                                if (!item.IsDirectory)
                                {
                                    if (!clsUtilsFTP.Instance.FtpDownload(string.Format("{0}/{1}", item.ParentNm, item.Name), string.Format("{0}/{1}{2}", _strTempLocalPath, _strLocalRoot, item.ParentNm), item.Name))
                                    {
                                        MessageBox.Show("다운로드 실패");
                                        break;
                                    }
                                    else
                                    {
                                        iSaveCnt++;
                                        cProgress.ChangeProgress(clsUtilsSTRING.conProgressDownloadStatus, iSaveCnt, iTotalCnt, strSaveNm, clsUtilsEnum.eProcess.eDownload, true);
                                    }
                                }
                            }

                            bSuccess = true;
                        }
                        else
                        {
                            MessageBox.Show("다운로드할 자료가 존재하지 않습니다.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("다운로드할 자료가 존재하지 않습니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                if (bSuccess)
                {
                    MessageBox.Show(_strListText + "\r\n다운로드 완료", "저장성공");
                }

                cProgress.ChangeProgress(clsUtilsSTRING.conProgressNormalStatus, 0, 0, string.Empty, clsUtilsEnum.eProcess.eNormal, false);

                SetControlEnable(true);
            }
        }

        /// <summary>
        /// 목록관련 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnTree_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;

                if (ChkSaveList())
                {
                    frmPopUp frmPop = new frmPopUp(_strListText + " 변경 내역이 존재합니다.", "");

                    if (frmPop.ShowDialog() == DialogResult.OK)
                    {
                        SaveData(false, "new", "");
                        return;
                    }
                    else
                    {
                        DeleteSaveList();
                    }
                }
                else
                {
                    DataRow[] drFilter = clsUtilsData.Instance.dtList.Select(string.Format("ChangeYN = '{0}'", clsUtilsEnum.eChange.New.ToString()));

                    if (drFilter != null && drFilter.Length > 0)
                    {
                        cboList.SelectedIndexChanged -= new EventHandler(cboList_SelectedIndexChanged);

                        foreach (DataRow dr in drFilter)
                        {
                            clsUtilsData.Instance.dtList.Rows.Remove(dr);
                        }

                        _strListKey = "";
                        _strListText = "";
                        cboList.SelectedIndexChanged += new EventHandler(cboList_SelectedIndexChanged);
                    }
                }

                frmNewDate frm = new frmNewDate();

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    string strVersionNm = frm.NewDate;

                    DataRow[] drVersionNm = clsUtilsData.Instance.dtList.Select(string.Format("VersionNm = '{0}'", strVersionNm));

                    if (drVersionNm != null && drVersionNm.Length <= 0)
                    {
                        clsUtilsData.Instance.AddRowList(
                            strVersionNm,
                            string.Format("{0}/{1}/{2} [{3}]",
                                strVersionNm.Substring(0, 4),
                                strVersionNm.Substring(4, 2),
                                strVersionNm.Substring(6, 2),
                                strVersionNm.Substring(8, 3)),
                            string.Format("{0}/{1}", _strServerRoot, _strProgram),
                            clsUtilsEnum.eChange.New.ToString()
                            );


                        string ServerDirectory = string.Format("{0}/{1}/{2}", _strServerRoot, _strProgram, strVersionNm);
                        clsUtilsData.Instance.AddRowUpList(clsUtilsSTRING.conN, strVersionNm, clsUtilsEnum.eChange.New, clsUtilsSTRING.conN, ServerDirectory, "", "", "", DateTime.Now, true);

                        //콤보박스에 목록 바인딩
                        cboList.Refresh();
                        cboList.SelectedIndexChanged -= new EventHandler(cboList_SelectedIndexChanged);
                        cboList.SelectedIndex = cboList.Items.Count - 1;
                        cboList.SelectedIndexChanged += new EventHandler(cboList_SelectedIndexChanged);
                        cboList_SelectedIndexChanged(cboList, null);
                    }
                    else
                    {
                        MessageBox.Show("동일한 서버목록 존재");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                //
            }
        }

        /// <summary>
        /// 상세목록관련 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnFile_Click(object sender, EventArgs e)
        {
            if (_strProgram.Length <= 0 || _strListKey.Length <= 0 || tv == null || tv.Nodes.Count <= 0) return;

            try
            {
                Button btn = (Button)sender;

                TreeNode node = tv.SelectedNode == null ? tv.Nodes[0] : tv.SelectedNode;
                TreeNode ParentNode = ParentNodes(node);
                string strTempNm = node.Name;
                string strTempLocalPath = string.Empty;

                if (btn.Name.ToString() == btnFileAdd.Name.ToString())
                {
                    #region 신규

                    if (strTempNm == clsUtilsSTRING.conBin || strTempNm == clsUtilsSTRING.conDamo || strTempNm == clsUtilsSTRING.conOcx ||
                        strTempNm == clsUtilsSTRING.conConfig || strTempNm == clsUtilsSTRING.conMst ||
                        strTempNm == clsUtilsSTRING.conSchema || strTempNm == clsUtilsSTRING.conTran || strTempNm == clsUtilsSTRING.conResource ||
                        strTempNm == clsUtilsSTRING.conUpdate)
                    {
                        OpenFileDialog openFile = new OpenFileDialog();
                        openFile.Multiselect = true;
                        if (openFile.ShowDialog() == DialogResult.OK)
                        {
                            if (openFile.FileNames.Length > 0)
                            {
                                DateTime dtNow = DateTime.Now;
                                foreach (string strFile in openFile.FileNames)
                                {
                                    FileInfo file = new FileInfo(strFile);
                                    clsUtilsData.Instance.AddRowUpList(
                                        clsUtilsSTRING.conN,
                                        _strListKey,
                                        clsUtilsEnum.eChange.New,
                                        clsUtilsSTRING.conY,
                                        string.Format("{0}/{1}", node.Tag.ToString(), strTempNm),
                                        file.Name,
                                        file.DirectoryName,
                                        file.Length.ToString(),
                                        dtNow,
                                        false);
                                }
                            }
                        }
                    }

                    #endregion
                }
                else if (btn.Name.ToString() == btnFileDelete.Name.ToString())
                {
                    #region 삭제

                    if (clsUtilsData.Instance.dtUpList != null && clsUtilsData.Instance.dtUpList.Rows.Count > 0)
                    {
                        if (strTempNm == clsUtilsSTRING.conBin || strTempNm == clsUtilsSTRING.conDamo || strTempNm == clsUtilsSTRING.conOcx || 
                            strTempNm == clsUtilsSTRING.conConfig || strTempNm == clsUtilsSTRING.conMst ||
                                strTempNm == clsUtilsSTRING.conSchema || strTempNm == clsUtilsSTRING.conTran || strTempNm == clsUtilsSTRING.conResource ||
                                strTempNm == clsUtilsSTRING.conUpdate)
                        {
                            DataRow[] drFilter = clsUtilsData.Instance.dtUpList.Select(
                                string.Format("VersionNm = '{0}' and FileYN = '{1}' and ChangeYN <> '{2}' and ServerDirectory = '{3}/{4}' and DeleteYN = '{5}'",
                                _strListKey, clsUtilsSTRING.conY, clsUtilsEnum.eChange.Delete.ToString(), node.Tag.ToString(), strTempNm, clsUtilsSTRING.conY));

                            if (drFilter != null && drFilter.Length > 0)
                            {
                                foreach (DataRow dr in drFilter)
                                {
                                    if (dr["ChangeYN"].ToString() == clsUtilsEnum.eChange.New.ToString())
                                    {
                                        clsUtilsData.Instance.dtUpList.Rows.Remove(dr);
                                    }
                                    else
                                    {
                                        dr["ChangeYN"] = clsUtilsEnum.eChange.Delete.ToString();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("삭제할 자료가 존재하지 않습니다.");
                                return;
                            }
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// AppConfig.ini 및 DevConfig.ini 등록 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnConfigAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                ComboBox cboTempSection = null;
                ComboBox cboTempKey = null;
                TextBox txtTemp = null;
                ComboBox cboTempStore = null;
                ctrlCheckedComboBox cboTempPos = null;
                DataTable dtTemp = null;

                string strSection = string.Empty;
                string strKey = string.Empty;
                string strValue = string.Empty;
                string strStore = string.Empty;
                string strPos = string.Empty;
                string strSelect = string.Empty;
                string strConfig = string.Empty;
                string strServerDirectory = string.Empty;
                string strLocalDirectory = string.Empty;

                TreeNode node = tv.SelectedNode;
                TreeNode ParentNode = ParentNodes(node);
                string strTempNm = node.Name;

                if (btn.Name.ToString() == btnAddAppConfig.Name.ToString())
                {
                    cboTempSection = cboAppSection;
                    cboTempKey = cboAppKey;
                    txtTemp = txtAppInput;
                    cboTempStore = cboAppStore;
                    cboTempPos = cboAppPos;
                    dtTemp = clsUtilsData.Instance.dtApp;
                    strConfig = clsUtilsSTRING.conAppConfig;
                }
                else
                {
                    cboTempSection = cboDevSection;
                    cboTempKey = cboDevKey;
                    txtTemp = txtDevInput;
                    cboTempStore = cboDevStore;
                    cboTempPos = cboDevPos;
                    dtTemp = clsUtilsData.Instance.dtDev;
                    strConfig = clsUtilsSTRING.conDevConfig;
                }

                if (cboTempSection == null || cboTempSection.Items.Count <= 0 || cboTempSection.SelectedValue == null || cboTempSection.SelectedValue.ToString().Length <= 0 ||
                    cboTempKey == null || cboTempKey.Items.Count <= 0 || cboTempKey.SelectedValue == null || cboTempKey.SelectedValue.ToString().Length <= 0 ||
                    txtTemp.Text.Length <= 0 ||
                    cboTempStore == null || cboTempStore.Text.Length <= 0 ||
                    cboTempPos == null || cboTempPos.Text.Length <= 0) return;

                strSection = cboTempSection.SelectedValue.ToString();
                strKey = cboTempKey.SelectedValue.ToString();
                strValue = txtTemp.Text;
                strStore = cboTempStore.Text == "전체" ? "**" : cboTempStore.Text;
                strPos = cboTempPos.Text.Contains("전체") ? "****" : cboTempPos.Text;


                if (btn.Name.ToString() == btnAddAppConfig.Name.ToString())
                {
                    strSection = strSection == clsUtilsSTRING.conINIPosComm01 || strSection == clsUtilsSTRING.conINIPosComm02 ? clsUtilsSTRING.conINIPosComm : strSection;
                    strSelect = string.Format("colAppRealSection = '{0}' and colAppKey = '{1}' and colAppValue = '{2}' and colAppStore = '{3}' and colAppPos = '{4}' and colAppChangeYN <> '{5}'",
                    strSection, strKey, strValue, strStore, strPos, clsUtilsEnum.eChange.Delete.ToString());
                }
                else
                {
                    strSelect = string.Format("colDevRealSection = '{0}' and colDevKey = '{1}' and colDevValue = '{2}' and colDevStore = '{3}' and colDevPos = '{4}' and colDevChangeYN <> '{5}'",
                    strSection, strKey, strValue, strStore, strPos, clsUtilsEnum.eChange.Delete.ToString());
                }

                DataRow[] drFilter = dtTemp.Select(strSelect);

                if (drFilter != null && drFilter.Length > 0)
                {
                    MessageBox.Show("존재");
                    return;
                }
                clsUtilsData.Instance.ConfigAddrow(dtTemp, strSection, strKey, strValue, strStore, strPos);

                string strTempLocalPath = string.Format("{0}/{1}{2}/{3}/{4}", Application.StartupPath, _strLocalRoot, _strServerRoot, _strProgram, ParentNode.Name.ToString());

                drFilter = clsUtilsData.Instance.dtUpList.Select(
                    string.Format("FileNm = '{0}'", strConfig));

                if (drFilter != null && drFilter.Length > 0)
                {
                    drFilter[0]["ChangeYN"] = clsUtilsEnum.eChange.Modify.ToString();
                }
                else
                {
                    clsUtilsData.Instance.AddRowUpList(clsUtilsSTRING.conN, ParentNode.Name.ToString(), clsUtilsEnum.eChange.New, clsUtilsSTRING.conY, node.Tag.ToString(), strConfig, strTempLocalPath, "0", DateTime.Now, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// AppConfig.ini 및 DevConfig.ini 삭제 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnConfigDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                DataTable dtTemp = null;
                DataGridView grdTemp = null;
                string strFileNm = string.Empty;
                string strChangeYN = string.Empty;

                if (btn.Name.ToString() == btnDeleteAppConfig.Name.ToString())
                {
                    dtTemp = clsUtilsData.Instance.dtApp;
                    grdTemp = grdApp;
                    strChangeYN = "colAppChangeYN";
                }
                else
                {
                    dtTemp = clsUtilsData.Instance.dtDev;
                    grdTemp = grdDev;
                    strChangeYN = "colDevChangeYN";
                }


                if (dtTemp == null || dtTemp.Rows.Count <= 0 || grdTemp.SelectedRows == null || grdTemp.SelectedRows.Count <= 0) return;

                if (grdTemp.SelectedRows[0].Cells[strChangeYN].Value.ToString() == clsUtilsEnum.eChange.New.ToString())
                {
                    grdTemp.Rows.Remove(grdTemp.SelectedRows[0]);

                    DataRow[] drFilter = dtTemp.Select(string.Format("{0} <> '{1}'", strChangeYN, clsUtilsEnum.eChange.Normal.ToString()));

                    if (drFilter.Length <= 0)
                    {
                        if (dtTemp.Rows.Count <= 0)
                        {
                            drFilter = clsUtilsData.Instance.dtUpList.Select(
                                string.Format("FileNm = '{0}'", btn.Name.ToString() == btnDeleteAppConfig.Name.ToString() ? clsUtilsSTRING.conAppConfig : clsUtilsSTRING.conDevConfig));

                            if (drFilter != null && drFilter.Length > 0)
                            {
                                clsUtilsData.Instance.dtUpList.Rows.Remove(drFilter[0]);
                            }
                        }
                    }
                }
                else
                {
                    grdTemp.SelectedRows[0].Cells[strChangeYN].Value = clsUtilsEnum.eChange.Delete.ToString();

                    DataRow[] drFilter = dtTemp.Select(string.Format("{0} <> '{1}'", strChangeYN, clsUtilsEnum.eChange.Delete.ToString()));

                    if (drFilter.Length <= 0)
                    {
                        drFilter = clsUtilsData.Instance.dtUpList.Select(
                                string.Format("FileNm = '{0}'", btn.Name.ToString() == btnDeleteAppConfig.Name.ToString() ? clsUtilsSTRING.conAppConfig : clsUtilsSTRING.conDevConfig));

                        if (drFilter != null && drFilter.Length > 0)
                        {
                            drFilter[0]["ChangeYN"] = clsUtilsEnum.eChange.Delete.ToString();
                        }
                    }
                    else
                    {
                        drFilter = clsUtilsData.Instance.dtUpList.Select(
                                string.Format("FileNm = '{0}'", btn.Name.ToString() == btnDeleteAppConfig.Name.ToString() ? clsUtilsSTRING.conAppConfig : clsUtilsSTRING.conDevConfig));

                        if (drFilter != null && drFilter.Length > 0)
                        {
                            drFilter[0]["ChangeYN"] = clsUtilsEnum.eChange.Modify.ToString();
                        }
                    }
                }

                dtTemp.AcceptChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 그리드 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void grdConfig_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView grd = (DataGridView)sender;

                if (grd.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
                {
                    string strPosNo = string.Empty;

                    if (grd.Name.ToString() == grdApp.Name.ToString())
                    {
                        strPosNo = grd.Rows[e.RowIndex].Cells["colAppPos"].Value != null ? grd.Rows[e.RowIndex].Cells["colAppPos"].Value.ToString() : "";
                    }
                    else
                    {
                        strPosNo = grd.Rows[e.RowIndex].Cells["colDevPos"].Value != null ? grd.Rows[e.RowIndex].Cells["colDevPos"].Value.ToString() : "";
                    }

                    if (strPosNo.Length > 0)
                    {
                        frmPosList frm = new frmPosList(strPosNo);
                        frm.TopMost = true;
                        frm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        #endregion

        /// <summary>
        /// SQL 파일관련 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtSQL_Validated(object sender, EventArgs e)
        {
            try
            {
                RichTextBox txt = (RichTextBox)sender;

                if (clsUtilsData.Instance.dtUpList == null || clsUtilsData.Instance.dsSQL == null || clsUtilsData.Instance.dsSQL.Tables.Count <= 0) return;

                DataTable dt = null;

                if (txt.Name.ToString() == cMst.txtMst.Name.ToString())
                {
                    dt = clsUtilsData.Instance.dsSQL.Tables[clsUtilsSTRING.conMstChanges];
                }
                else if (txt.Name.ToString() == cTran.txtTran.Name.ToString())
                {
                    dt = clsUtilsData.Instance.dsSQL.Tables[clsUtilsSTRING.conTranChanges];
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    dr["Value02"] = "";
                    dr["ChangeYN"] = clsUtilsEnum.eChange.Normal.ToString();

                    if (txt.Text.Length > 0)
                    {
                        if (dr["Value01"].ToString().Length > 0)
                        {
                            if (dr["Value01"].ToString() != txt.Text)
                            {
                                dr["Value02"] = txt.Text;
                                dr["ChangeYN"] = clsUtilsEnum.eChange.Modify.ToString();
                            }
                        }
                        else
                        {
                            dr["Value02"] = txt.Text;
                            dr["ChangeYN"] = clsUtilsEnum.eChange.New.ToString();
                        }
                    }
                    else
                    {
                        if (dr["Value01"].ToString().Length > 0)
                        {
                            dr["Value02"] = "";
                            dr["ChangeYN"] = clsUtilsEnum.eChange.Delete.ToString();
                        }
                    }

                    if (dr["ChangeYN"].ToString() != clsUtilsEnum.eChange.Normal.ToString())
                    {
                        DataRow[] drFilter = clsUtilsData.Instance.dtUpList.Select(string.Format("FileNm = '{0}' and DirectoryDepth06 = '{1}'", dt.TableName.ToString(), dt.TableName == clsUtilsSTRING.conMstChanges ? clsUtilsSTRING.conMst : clsUtilsSTRING.conTran));

                        if (drFilter != null && drFilter.Length > 0)
                        {
                            drFilter[0]["ChangeYN"] = dt.Rows[0]["ChangeYN"].ToString();
                        }
                        else
                        {
                            TreeNode node = tv.SelectedNode == null ? tv.Nodes[0] : tv.SelectedNode;

                            clsUtilsData.Instance.AddRowUpList(clsUtilsSTRING.conN,
                                _strListKey,
                                clsUtilsEnum.eChange.New,
                                clsUtilsSTRING.conY,
                                node.Tag.ToString(),
                                dt.TableName,
                                string.Format("{0}/{1}{2}", _strTempLocalPath, _strLocalRoot, node.Tag.ToString()),
                                "",
                                DateTime.Now,
                                false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        #endregion

        #region 사용자정의

        /// <summary>
        /// User Control 설정
        /// </summary>
        /// <param name="bStatus">진행상황 Visible 여부</param>
        /// <param name="bFile">파일 UserControl Visible 여부</param>
        /// <param name="bSqlMst">Mst UserControl Visible 여부</param>
        /// <param name="bSqlTran">Tran UserControl Visible 여부</param>
        /// <param name="bAppConfig">AppConfig UserControl Visible 여부</param>
        /// <param name="bDevConfig">DevConfig UserControl Visible 여부</param>
        private void SetUserControl(bool bStatus, bool bFile, bool bMst, bool bTran, bool bApp, bool bDev)
        {
            SetUserControl(bStatus);

            tlpFile.Dock = DockStyle.Fill;
            tlpFile.Visible = bFile;

            cMst.Dock = DockStyle.Fill;
            cMst.Visible = bMst;

            cTran.Dock = DockStyle.Fill;
            cTran.Visible = bTran;

            tlpApp.Dock = DockStyle.Fill;
            tlpApp.Visible = bApp;

            tlpDev.Dock = DockStyle.Fill;
            tlpDev.Visible = bDev;
        }

        /// <summary>
        /// User Control 설정
        /// </summary>
        /// <param name="bStatus">진행상황 Visible 여부</param>
        private void SetUserControl(bool bStatus)
        {
            cStatus.Dock = DockStyle.Fill;
            cStatus.Visible = bStatus;

            cProgress.Dock = DockStyle.Fill;
            cProgress.Visible = !bStatus;
        }

        /// <summary>
        /// 컨트롤 초기화
        /// </summary>
        void SetControl(bool bAll)
        {
            try
            {
                if (bAll)
                {
                    if (tv != null)
                    {
                        tv.Nodes.Clear();
                    }
                }

                //UserControl 설정
                SetUserControl(true, false, false, false, false, false);

                //트리뷰 초기화
                if (tv != null && tv.Nodes.Count > 0)
                {
                    tv.Nodes.Clear();
                }

                //파일 초기화
                if (grdFile != null && grdFile.DataSource != null)
                {
                    //데이터 테이블 삭제
                    ((DataTable)grdFile.DataSource).Clear();
                }

                //MstChanges.sql 초기화
                cMst.Clear();

                //TranChanges.sql 초기화
                cTran.Clear();

                //AppConfig.ini 파일 초기화
                if (clsUtilsData.Instance.dtApp != null)
                {
                    clsUtilsData.Instance.dtApp.Clear();
                }

                //DevConfig.ini 파일 초기화
                if (clsUtilsData.Instance.dtDev != null)
                {
                    clsUtilsData.Instance.dtDev.Clear();
                }

                cboAppSection.SelectedIndex = 0;
                cboDevSection.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 컨트롤 활성/비활성화
        /// </summary>
        /// <param name="bEnable"></param>
        void SetControlEnable(bool bEnable)
        {
            tv.Enabled = bEnable;
            btnTreeNew.Enabled = bEnable;

            cboProgram.Enabled = bEnable;
            cboList.Enabled = bEnable;

            btnSelect.Enabled = bEnable;
            btnSave.Enabled = bEnable;
            btnDelete.Enabled = bEnable;
            btnDownload.Enabled = bEnable;

            cMst.Enabled = bEnable;
            cTran.Enabled = bEnable;

            tlpFile.Enabled = bEnable;
            tlpApp.Enabled = bEnable;
            tlpDev.Enabled = bEnable;

            Application.DoEvents();
        }

        #region 트리뷰 관련 사용자 정의

        /// <summary>
        /// 트리뷰 기본 셋팅
        /// </summary>
        /// <param name="strKey">업그레이드 일자</param>
        /// <param name="strValue">업그레이드 일자 (표현방식변경)</param>
        /// <param name="strValue">상위 디렉토리</param>
        void SetTreeView(string strKey, string strValue, string strServerPath)
        {
            //최상위 노드 생성
            TreeNode nodeMaster = new TreeNode(strValue);
            nodeMaster.Name = strKey;
            nodeMaster.Tag = strServerPath;
            tv.Nodes.Add(nodeMaster);

            #region 트리뷰 생성

            string strTag = string.Format("{0}/{1}", strServerPath, strKey);

            TreeNode nodeBin = new TreeNode(clsUtilsSTRING.conBin, 0, 0);
            nodeBin.Name = clsUtilsSTRING.conBin;
            nodeBin.Tag = strTag;
            nodeMaster.Nodes.Add(nodeBin);

            TreeNode nodeLib = new TreeNode(clsUtilsSTRING.conLib, 0, 0);
            nodeLib.Name = clsUtilsSTRING.conLib;
            nodeLib.Tag = string.Format("{0}/{1}", strTag, clsUtilsSTRING.conBin);
            nodeBin.Nodes.Add(nodeLib);

            TreeNode nodeDamo = new TreeNode(clsUtilsSTRING.conDamo, 0, 0);
            nodeDamo.Name = clsUtilsSTRING.conDamo;
            nodeDamo.Tag = string.Format("{0}/{1}/{2}", strTag, clsUtilsSTRING.conBin, clsUtilsSTRING.conLib);
            nodeLib.Nodes.Add(nodeDamo);

            if (_strProgram != clsUtilsSTRING.conServerPDA)
            {
                TreeNode nodeOcx = new TreeNode(clsUtilsSTRING.conOcx, 0, 0);
                nodeOcx.Name = clsUtilsSTRING.conOcx;
                nodeOcx.Tag = string.Format("{0}/{1}/{2}", strTag, clsUtilsSTRING.conBin, clsUtilsSTRING.conLib);
                nodeLib.Nodes.Add(nodeOcx);
            }

            TreeNode nodeConfig = new TreeNode(clsUtilsSTRING.conConfig, 0, 0);
            nodeConfig.Name = clsUtilsSTRING.conConfig;
            nodeConfig.Tag = strTag;
            nodeMaster.Nodes.Add(nodeConfig);

            TreeNode nodeData = new TreeNode(clsUtilsSTRING.conData, 0, 0);
            nodeData.Name = clsUtilsSTRING.conData;
            nodeData.Tag = strTag;
            nodeMaster.Nodes.Add(nodeData);

            TreeNode nodeMst = new TreeNode(clsUtilsSTRING.conMst, 0, 0);
            nodeMst.Name = clsUtilsSTRING.conMst;
            nodeMst.Tag = string.Format("{0}/{1}", strTag, clsUtilsSTRING.conData);
            nodeData.Nodes.Add(nodeMst);

            TreeNode nodeSchema = new TreeNode(clsUtilsSTRING.conSchema, 0, 0);
            nodeSchema.Name = clsUtilsSTRING.conSchema;
            nodeSchema.Tag = string.Format("{0}/{1}", strTag, clsUtilsSTRING.conData);
            nodeData.Nodes.Add(nodeSchema);

            TreeNode nodeTran = new TreeNode(clsUtilsSTRING.conTran, 0, 0);
            nodeTran.Name = clsUtilsSTRING.conTran;
            nodeTran.Tag = string.Format("{0}/{1}", strTag, clsUtilsSTRING.conData);
            nodeData.Nodes.Add(nodeTran);

            TreeNode nodePatch = new TreeNode(clsUtilsSTRING.conPatch, 0, 0);
            nodePatch.Name = clsUtilsSTRING.conPatch;
            nodePatch.Tag = string.Format("{0}/{1}", strTag, clsUtilsSTRING.conData);
            nodeData.Nodes.Add(nodePatch);

            TreeNode nodeMstChange = new TreeNode(clsUtilsSTRING.conMst, 0, 0);
            nodeMstChange.Name = clsUtilsSTRING.conMstChange;
            nodeMstChange.Tag = string.Format("{0}/{1}/{2}", strTag, clsUtilsSTRING.conData, clsUtilsSTRING.conPatch);
            nodePatch.Nodes.Add(nodeMstChange);

            TreeNode nodeMstChangeSql = new TreeNode(clsUtilsSTRING.conMstChanges);
            nodeMstChangeSql.Name = clsUtilsSTRING.conMstChanges;
            nodeMstChangeSql.Tag = string.Format("{0}/{1}/{2}/{3}", strTag, clsUtilsSTRING.conData, clsUtilsSTRING.conPatch, clsUtilsSTRING.conMst);
            nodeMstChange.Nodes.Add(nodeMstChangeSql);

            TreeNode nodeTranChange = new TreeNode(clsUtilsSTRING.conTran, 0, 0);
            nodeTranChange.Name = clsUtilsSTRING.conTranChange;
            nodeTranChange.Tag = string.Format("{0}/{1}/{2}", strTag, clsUtilsSTRING.conData, clsUtilsSTRING.conPatch);
            nodePatch.Nodes.Add(nodeTranChange);

            TreeNode nodeTranChangeSql = new TreeNode(clsUtilsSTRING.conTranChanges);
            nodeTranChangeSql.Name = clsUtilsSTRING.conTranChanges;
            nodeTranChangeSql.Tag = string.Format("{0}/{1}/{2}/{3}", strTag, clsUtilsSTRING.conData, clsUtilsSTRING.conPatch, clsUtilsSTRING.conTran);
            nodeTranChange.Nodes.Add(nodeTranChangeSql);

            TreeNode nodeResource = new TreeNode(clsUtilsSTRING.conResource, 0, 0);
            nodeResource.Name = clsUtilsSTRING.conResource;
            nodeResource.Tag = strTag;
            nodeMaster.Nodes.Add(nodeResource);

            TreeNode nodeUpdate = new TreeNode(clsUtilsSTRING.conUpdate, 0, 0);
            nodeUpdate.Name = clsUtilsSTRING.conUpdate;
            nodeUpdate.Tag = strTag;
            nodeMaster.Nodes.Add(nodeUpdate);

            TreeNode nodeAppConfig = new TreeNode(clsUtilsSTRING.conAppConfig);
            nodeAppConfig.Name = clsUtilsSTRING.conAppConfig;
            nodeAppConfig.Tag = strTag;
            nodeMaster.Nodes.Add(nodeAppConfig);

            if (_strProgram != clsUtilsSTRING.conServerPDA)
            {
                TreeNode nodeDevConfig = new TreeNode(clsUtilsSTRING.conDevConfig);
                nodeDevConfig.Name = clsUtilsSTRING.conDevConfig;
                nodeDevConfig.Tag = strTag;
                nodeMaster.Nodes.Add(nodeDevConfig);    
            }

            tv.ExpandAll();
            #endregion
        }

        /// <summary>
        /// 부모노드 반환
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        TreeNode ParentNodes(TreeNode node)
        {
            TreeNode ParentNode = node;

            if (ParentNode.Parent == null)
            {
                ParentNode = node;
            }
            else
            {
                while (ParentNode.Parent != null)
                {
                    ParentNode = ParentNode.Parent;
                }
            }

            return ParentNode;
        }

        #endregion

        #region AppConfig, DevConfig

        /// <summary>
        /// Config 관련 컨트롤 초기화
        /// </summary>
        /// <param name="cboTemp"></param>
        /// <param name="dtTemp"></param>
        /// <param name="grdTemp"></param>
        private void SetInitControlConfig(ComboBox cboTemp, DataTable dtTemp, DataGridView grdTemp)
        {
            try
            {
                if (cboTemp == null || cboTemp.Items.Count <= 0)
                {
                    DataTable dtSection = new DataTable();
                    dtSection.Columns.Add("Id");
                    dtSection.Columns.Add("Name");

                    dtSection.Rows.Add(new object[] { "", "" });

                    if (cboTemp.Name.ToString() == cboAppSection.Name.ToString())
                    {
                        dtSection.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm01, clsUtilsSTRING.conINIPosComm01Nm });
                        dtSection.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conINIPosComm02Nm });
                        dtSection.Rows.Add(new object[] { clsUtilsSTRING.conINIPosFTP, clsUtilsSTRING.conINIPosFTPNm });
                        dtSection.Rows.Add(new object[] { clsUtilsSTRING.conINIPosVan, clsUtilsSTRING.conINIPosVanNm });
                        dtSection.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, clsUtilsSTRING.conINIPosOptionNm });
                    }
                    else
                    {
                        dtSection.Rows.Add(new object[] { clsUtilsSTRING.conINIScannerGun, clsUtilsSTRING.conINIScannerGun });
                        dtSection.Rows.Add(new object[] { clsUtilsSTRING.conINILineDisplay, clsUtilsSTRING.conINILineDisplay });
                        dtSection.Rows.Add(new object[] { clsUtilsSTRING.conINIPrinter, clsUtilsSTRING.conINIPrinter });
                        dtSection.Rows.Add(new object[] { clsUtilsSTRING.conINIMSR, clsUtilsSTRING.conINIMSR });
                        dtSection.Rows.Add(new object[] { clsUtilsSTRING.conINICashDrawer, clsUtilsSTRING.conINICashDrawer });
                        dtSection.Rows.Add(new object[] { clsUtilsSTRING.conINISignPad, clsUtilsSTRING.conINISignPad });
                    }

                    cboTemp.DataSource = dtSection;
                    cboTemp.DisplayMember = "Name";
                    cboTemp.ValueMember = "Id";
                }
                cboTemp.SelectedIndex = 0;
                cboConfigSection_SelectedIndexChanged(cboTemp, null);

                if (cboTemp.Name.ToString() == cboAppSection.Name.ToString())
                {
                    grdApp.DataSource = clsUtilsData.Instance.dtApp;
                    clsUtilsData.Instance.dtApp.DefaultView.RowFilter = string.Format("colAppChangeYN <> '{0}'", clsUtilsEnum.eChange.Delete.ToString());    
                }
                else
                {
                    grdDev.DataSource = clsUtilsData.Instance.dtDev;
                    clsUtilsData.Instance.dtDev.DefaultView.RowFilter = string.Format("colDevChangeYN <> '{0}'", clsUtilsEnum.eChange.Delete.ToString());
                }
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 점포 및 포스 체크콤보박스 셋팅
        /// </summary>
        /// <param name="cbo">점포 및 포스 체크콤보박스</param>
        void InitControlConfig(ComboBox cbo)
        {
            try
            {
                if (cbo == null || cbo.Items.Count <= 0)
                {
                    DataSet ds = clsUtilsOra.Instance.GetOra("select distinct CD_STORE from BSM010T where fg_use = '1' order by CD_STORE ASC");

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Id");
                    dt.Columns.Add("Name");

                    dt.Rows.Add(new object[] { "", "" });

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            dt.Rows.Add(new object[] { "**", "전체" });
                        }

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            dt.Rows.Add(new object[] { dr[0].ToString(), dr[0].ToString() });
                        }
                    }

                    cbo.DataSource = dt;
                    cbo.ValueMember = "Id";
                    cbo.DisplayMember = "Name";
                }
                else
                {
                    cbo.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 점포 및 포스 체크콤보박스 셋팅
        /// </summary>
        /// <param name="cbo">점포 및 포스 체크콤보박스</param>
        void InitControlConfig(ctrlCheckedComboBox cbo)
        {
            try
            {
                cbo.MaxDropDownItems = 5;
                cbo.DisplayMember = "Name";
                cbo.ValueSeparator = ",";
                cbo.Items.Clear();
                cbo.Text = "";

                ComboBox cboTempStore = cbo.Name.ToString() == cboAppPos.Name.ToString() ? cboAppStore : cboDevStore;

                if (cboTempStore.SelectedValue != null && cboTempStore.SelectedValue.ToString().Length > 0)
                {
                    int iRow = 0;
                    ctrlCCBoxItem cItem = new ctrlCCBoxItem("전체", iRow);
                    cbo.Items.Add(cItem);
                    iRow++;

                    if (cboTempStore.SelectedValue.ToString() != "**")
                    {
                        string posFg = cboProgram.SelectedIndex == 0 ? "'1','2'" : "'3','4'";

                        DataSet ds = clsUtilsOra.Instance.GetOra(string.Format("select distinct NO_POS from BSM040T where fg_pos in ({1}) and cd_store in ({0}) order by NO_POS ASC", 
                            cboTempStore.SelectedValue.ToString(), posFg));


                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                cItem = new ctrlCCBoxItem(dr[0].ToString(), iRow);
                                cbo.Items.Add(cItem);
                                iRow++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// AppConfig.ini 및 DevConfig.ini 변경내역 검사후 변경내역 존재시 ini 로컬에 저장
        /// </summary>
        bool SaveConfigValue(bool bDelete, string strConfigNm, string strTempLocalDirectory, string strTempLocalPath)
        {
            bool bReturn = false;

            try
            {
                if (bDelete)
                {
                    FileInfo fi = new FileInfo(strTempLocalPath);

                    if (fi.Exists)
                    {
                        fi.Delete();
                    }

                    return true;
                }

                //폴더 검사
                DirectoryInfo di = new DirectoryInfo(strTempLocalDirectory);

                if (!di.Exists)
                {
                    di.Create();
                }

                if (strConfigNm == clsUtilsSTRING.conAppConfig)
                {
                    using (StreamWriter file = new StreamWriter(strTempLocalPath))
                    {
                        SetWriteConfig(true, file, clsUtilsData.Instance.dtApp, strTempLocalPath, clsUtilsSTRING.conINIPosComm);
                        SetWriteConfig(true, file, clsUtilsData.Instance.dtApp, strTempLocalPath, clsUtilsSTRING.conINIPosFTP);
                        SetWriteConfig(true, file, clsUtilsData.Instance.dtApp, strTempLocalPath, clsUtilsSTRING.conINIPosVan);
                        SetWriteConfig(true, file, clsUtilsData.Instance.dtApp, strTempLocalPath, clsUtilsSTRING.conINIPosOption);
                    }
                }
                else
                {
                    using (StreamWriter file = new StreamWriter(strTempLocalPath))
                    {
                        SetWriteConfig(false, file, clsUtilsData.Instance.dtDev, strTempLocalPath, clsUtilsSTRING.conINIScannerGun);
                        SetWriteConfig(false, file, clsUtilsData.Instance.dtDev, strTempLocalPath, clsUtilsSTRING.conINILineDisplay);
                        SetWriteConfig(false, file, clsUtilsData.Instance.dtDev, strTempLocalPath, clsUtilsSTRING.conINIPrinter);
                        SetWriteConfig(false, file, clsUtilsData.Instance.dtDev, strTempLocalPath, clsUtilsSTRING.conINIMSR);
                        SetWriteConfig(false, file, clsUtilsData.Instance.dtDev, strTempLocalPath, clsUtilsSTRING.conINICashDrawer);
                        SetWriteConfig(false, file, clsUtilsData.Instance.dtDev, strTempLocalPath, clsUtilsSTRING.conINISignPad);
                    }
                }

                bReturn = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            return bReturn;
        }

        /// <summary>
        /// ini 파일 쓰기
        /// </summary>
        /// <param name="bTempApp">true : AppConfig.ini , false : DevConfig.ini</param>
        /// <param name="file">파일쓰기 스티림</param>
        /// <param name="dtTemp">데이터 테이블</param>
        /// <param name="strTempLocalPath">파일 경로</param>
        /// <param name="strFilter">필터</param>
        private void SetWriteConfig(bool bTempApp, StreamWriter file, DataTable dtTemp, string strTempLocalPath, string strFilter)
        {
            try
            {
                DataRow[] drFilter = bTempApp ?
                    dtTemp.Select(string.Format("colAppRealSection = '{0}' and colAppKey <> '' and colAppValue <> '' and colAppStore <> '' and colAppPos <> '' and colAppChangeYN <> '{1}'", strFilter, clsUtilsEnum.eChange.Delete.ToString())) :
                    dtTemp.Select(string.Format("colDevRealSection = '{0}' and colDevKey <> '' and colDevValue <> '' and colDevStore <> '' and colDevPos <> '' and colDevChangeYN <> '{1}'", strFilter, clsUtilsEnum.eChange.Delete.ToString()));

                if (drFilter != null && drFilter.Length > 0)
                {
                    file.WriteLine(string.Format("[{0}]", strFilter));

                    for (int i = drFilter.Length - 1; i >= 0; i--)
                    {
                        DataRow dr = drFilter[i];
                        file.WriteLine(string.Format("{0}=[{1}][{2}]{3}", dr[3].ToString(), dr[6].ToString(), dr[8].ToString(), dr[5].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        #endregion

        #region mstchange.sql, tranchange.sql

        /// <summary>
        /// mstchanges.sql 및 tranchanges.sql 변경내역 검사후 변경내역 존재시 SQL 로컬에 저장
        /// </summary>
        bool SaveSQLValue(bool bDelete, RichTextBox txt)
        {
            bool bReturn = false;

            try
            {
                DataTable dt = null;

                if (txt.Name.ToString() == cMst.txtMst.Name.ToString())
                {
                    dt = clsUtilsData.Instance.dsSQL.Tables[clsUtilsSTRING.conMstChanges];
                }
                else if (txt.Name.ToString() == cTran.txtTran.Name.ToString())
                {
                    dt = clsUtilsData.Instance.dsSQL.Tables[clsUtilsSTRING.conTranChanges];
                }

                string strTemp01 = string.Format("{0}/{1}/{2}/{3}/{4}/{5}", _strServerRoot, _strProgram, _strListKey, clsUtilsSTRING.conData, clsUtilsSTRING.conPatch, txt.Name == cMst.txtMst.Name ? clsUtilsSTRING.conMst : clsUtilsSTRING.conTran);
                string strTemp02 = string.Format("{0}/{1}{2}/{3}", Application.StartupPath, _strLocalRoot, strTemp01, dt.TableName);
                string strTempValue = string.Empty;

                if (bDelete)
                {
                    FileInfo fi = new FileInfo(string.Format("{0}/{1}{2}", Application.StartupPath, _strLocalRoot, strTemp01));

                    if (fi.Exists)
                    {
                        fi.Delete();
                    }

                    return true;
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    if (dt.Rows[0]["ChangeYN"].ToString() == clsUtilsEnum.eChange.New.ToString() || dt.Rows[0]["ChangeYN"].ToString() == clsUtilsEnum.eChange.Modify.ToString())
                    {
                        DirectoryInfo di = new DirectoryInfo(string.Format("{0}/{1}{2}", Application.StartupPath, _strLocalRoot, strTemp01));
                        if (!di.Exists)
                        {
                            di.Create();
                        }

                        for (int i = 0; i < txt.Lines.Length; i++)
                        {
                            if (i != 0)
                            {
                                strTempValue += ("\r\n" + txt.Lines[i].ToString());
                            }
                            else
                            {
                                strTempValue += txt.Lines[i].ToString();
                            }
                        }

                        File.WriteAllText(strTemp02, strTempValue, Encoding.Default);

                        bReturn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            return bReturn;
        }

        /// <summary>
        /// SQL 파일 읽기
        /// </summary>
        /// <param name="strLocalPath">로컬경로</param>
        /// <param name="dt">sql파일내용 임시저장할 데이터테이블</param>
        private void GetSQLValue(string strLocalPath, DataTable dt)
        {
            try
            {
                FileInfo file = new FileInfo(strLocalPath);

                //파일유무 확인
                if (file.Exists)
                {
                    StringBuilder sb = null;

                    using (StreamReader sr = new StreamReader(strLocalPath, Encoding.Default))
                    {
                        String line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (dt.TableName == clsUtilsSTRING.conMstChanges)
                            {
                                if (sb == null)
                                {
                                    sb = new StringBuilder();
                                }

                                sb.AppendLine(line);
                            }
                            else if (dt.TableName == clsUtilsSTRING.conTranChanges)
                            {
                                if (sb == null)
                                {
                                    sb = new StringBuilder();
                                }

                                sb.AppendLine(line);
                            }
                        }
                    }

                    if (sb != null && sb.ToString().Length > 0)
                    {
                        dt.Rows[0]["Value01"] = sb.ToString();
                        dt.Rows[0]["LocalPath"] = strLocalPath;

                        if (dt.TableName == clsUtilsSTRING.conMstChanges)
                        {
                            cMst.txtMst.AppendText(sb.ToString());
                        }
                        else if (dt.TableName == clsUtilsSTRING.conTranChanges)
                        {
                            cTran.txtTran.AppendText(sb.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        #endregion

        #region 데이터테이블 검사, 추가, 삭제

        /// <summary>
        /// 변경내역 조회
        /// </summary>
        bool ChkSaveList()
        {
            bool bReturn = false;

            try
            {
                DataRow[] drSub = clsUtilsData.Instance.dtUpList.Select(string.Format("VersionNm = '{0}' and ChangeYN <> '{1}'", _strListKey, clsUtilsEnum.eChange.Normal.ToString()));

                if (drSub != null && drSub.Length > 0)
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            return bReturn;
        }

        /// <summary>
        /// 기존내용 전체 원상태로 복구
        /// </summary>
        /// <returns></returns>
        bool DeleteSaveList()
        {
            bool bReturn = true;

            try
            {
                DataRow[] drFilter = clsUtilsData.Instance.dtList.Select(string.Format("VersionNm = '{0}' and ChangeYN = '{1}'", _strListKey, clsUtilsEnum.eChange.New.ToString()));

                if (drFilter != null && drFilter.Length > 0)
                {
                    if (drFilter[0]["ChangeYN"].ToString() == clsUtilsEnum.eChange.New.ToString())
                    {
                        clsUtilsData.Instance.dtUpList.Clear();
                        clsUtilsData.Instance.dtList.Rows.Remove(drFilter[0]);
                    }
                    else
                    {
                        for (int i = clsUtilsData.Instance.dtUpList.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow drSub = clsUtilsData.Instance.dtUpList.Rows[i];
                            if (drSub["FileYN"].ToString() == clsUtilsSTRING.conY.ToString())
                            {
                                if (drSub["ChangeYN"].ToString() == clsUtilsEnum.eChange.New.ToString())
                                {
                                    drSub.Delete();
                                }
                                else
                                {
                                    drSub["ChangeYN"] = clsUtilsEnum.eChange.Normal.ToString();
                                }
                            }
                        }

                        drFilter[0]["ChangeYN"] = clsUtilsEnum.eChange.Normal.ToString();
                    }
                }
                else
                {
                    for (int i = clsUtilsData.Instance.dtUpList.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow drSub = clsUtilsData.Instance.dtUpList.Rows[i];
                        if (drSub["FileYN"].ToString() == clsUtilsSTRING.conY.ToString())
                        {
                            if (drSub["ChangeYN"].ToString() == clsUtilsEnum.eChange.New.ToString())
                            {
                                clsUtilsData.Instance.dtUpList.Rows.Remove(drSub);
                            }
                            else
                            {
                                drSub["ChangeYN"] = clsUtilsEnum.eChange.Normal.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bReturn = false;
                MessageBox.Show(ex.Message.ToString());
            }

            return bReturn;
        }

        /// <summary>
        /// 데이터 저장
        /// </summary>
        /// <param name="bSaveConfirm"></param>
        private void SaveData(bool bSaveConfirm, string strGubun, string strValue)
        {
            if (_strListKey.Length <= 0 || clsUtilsData.Instance.dtUpList == null || clsUtilsData.Instance.dtUpList.Rows.Count <= 0) return;

            bool bSuccess = false;

            try
            {
                SetControlEnable(false);

                int iTotalCnt = 0;
                int iSaveCnt = 0;
                string strSaveNm = string.Empty;

                SetUserControl(false);
                cProgress.ChangeProgress(clsUtilsSTRING.conProgressSaveStatus, iSaveCnt, iTotalCnt, strSaveNm, clsUtilsEnum.eProcess.eSave, false);

                DataRow[] drMain = clsUtilsData.Instance.dtList.Select(string.Format("VersionNm = '{0}'", _strListKey));

                if (drMain != null && drMain.Length > 0)
                {
                    DataRow[] drFilter = clsUtilsData.Instance.dtUpList.Select(string.Format("ChangeYN <> '{0}'", clsUtilsEnum.eChange.Normal.ToString()));

                    if (drFilter != null && drFilter.Length > 0)
                    {
                        if (bSaveConfirm)
                        {
                            frmPopUp frm = new frmPopUp(_strListText, "서버 적용 하겠습니까?");

                            if (frm.ShowDialog() != DialogResult.OK)
                            {
                                return;
                            }
                        }

                        iTotalCnt = drFilter.Length;

                        foreach (DataRow dr in drFilter)
                        {
                            string strTempFileNm = dr["FileNm"].ToString();
                            string strTempServerDirectory = dr["ServerDirectory"].ToString();
                            string strTempServerPath = string.Format("{0}/{1}", strTempServerDirectory, strTempFileNm);
                            string strTempLocalDirectory = dr["LocalDirectory"].ToString();
                            string strTempLocalPath = string.Format("{0}/{1}", strTempLocalDirectory, strTempFileNm);

                            cProgress.ChangeProgress(clsUtilsSTRING.conProgressSaveStatus, iSaveCnt, iTotalCnt, strTempFileNm, clsUtilsEnum.eProcess.eSave, false);

                            if (dr["ChangeYN"].ToString() == clsUtilsEnum.eChange.Delete.ToString())
                            {
                                if (strTempFileNm == clsUtilsSTRING.conMstChanges || strTempFileNm == clsUtilsSTRING.conTranChanges)
                                {
                                    //신규, 수정
                                    if (!SaveSQLValue(true, strTempFileNm == clsUtilsSTRING.conMstChanges ? cMst.txtMst : cTran.txtTran))
                                    {
                                        MessageBox.Show(strTempFileNm + "파일의 저장에 실패하였습니다.");
                                        return;
                                    }
                                }
                                else if (strTempFileNm == clsUtilsSTRING.conAppConfig || strTempFileNm == clsUtilsSTRING.conDevConfig)
                                {
                                    //신규, 수정
                                    if (!SaveConfigValue(true, strTempFileNm, strTempLocalDirectory, strTempLocalPath))
                                    {
                                        MessageBox.Show(strTempFileNm + "파일의 저장에 실패하였습니다.");
                                        return;
                                    }
                                }

                                //삭제
                                if (clsUtilsFTP.Instance.FtpIsExistsFile(strTempServerPath))
                                {
                                    if (!clsUtilsFTP.Instance.FtpDelete(strTempServerPath, false))
                                    {
                                        MessageBox.Show(clsUtilsSTRING.conMstChanges + "파일의" + clsUtilsSTRING.conFTPDeleteFail);
                                        return;
                                    }

                                    iSaveCnt++;
                                    cProgress.ChangeProgress(clsUtilsSTRING.conProgressSaveStatus, iSaveCnt, iTotalCnt, strTempFileNm, clsUtilsEnum.eProcess.eSave, true);
                                }
                            }
                            else
                            {
                                if (strTempFileNm == clsUtilsSTRING.conMstChanges || strTempFileNm == clsUtilsSTRING.conTranChanges)
                                {
                                    //신규, 수정
                                    if (!SaveSQLValue(false, strTempFileNm == clsUtilsSTRING.conMstChanges ? cMst.txtMst : cTran.txtTran))
                                    {
                                        MessageBox.Show(strTempFileNm + "파일의 저장에 실패하였습니다.");
                                        return;
                                    }
                                }
                                else if (strTempFileNm == clsUtilsSTRING.conAppConfig || strTempFileNm == clsUtilsSTRING.conDevConfig)
                                {
                                    //신규, 수정
                                    if (!SaveConfigValue(false, strTempFileNm, strTempLocalDirectory, strTempLocalPath))
                                    {
                                        MessageBox.Show(strTempFileNm + "파일의 저장에 실패하였습니다.");
                                        return;
                                    }
                                }

                                FileInfo fi = new FileInfo(strTempLocalPath);

                                if (fi.Exists)
                                {
                                    //로컬파일 서버에 업로드
                                    if (!clsUtilsFTP.Instance.FtpUpload(strTempLocalPath, strTempServerPath))
                                    {
                                        MessageBox.Show(strTempFileNm + "파일의" + clsUtilsSTRING.conFTPUploadFail);
                                        return;
                                    }

                                    iSaveCnt++;
                                    cProgress.ChangeProgress(clsUtilsSTRING.conProgressSaveStatus, iSaveCnt, iTotalCnt, strTempFileNm, clsUtilsEnum.eProcess.eSave, true);
                                }
                            }

                            bSuccess = true;
                        }

                        foreach (DataRow dr in drFilter)
                        {
                            string strTempFileNm = dr["FileNm"].ToString();

                            if (dr["ChangeYN"].ToString() == clsUtilsEnum.eChange.Delete.ToString())
                            {
                                if (strTempFileNm != clsUtilsSTRING.conMstChanges && strTempFileNm != clsUtilsSTRING.conTranChanges &&
                                    strTempFileNm != clsUtilsSTRING.conAppConfig && strTempFileNm != clsUtilsSTRING.conDevConfig)
                                {
                                    clsUtilsData.Instance.dtUpList.Rows.Remove(dr);
                                }
                                else
                                {
                                    dr["ChangeYN"] = clsUtilsEnum.eChange.Normal.ToString();
                                }
                            }

                            if (strTempFileNm == clsUtilsSTRING.conMstChanges || strTempFileNm == clsUtilsSTRING.conTranChanges)
                            {
                                dr["ChangeYN"] = clsUtilsEnum.eChange.Normal.ToString();
                                clsUtilsData.Instance.dsSQL.Tables[strTempFileNm].Rows[0]["ChangeYN"] = clsUtilsEnum.eChange.Normal.ToString();
                                clsUtilsData.Instance.dsSQL.Tables[strTempFileNm].Rows[0]["Value01"] = clsUtilsData.Instance.dsSQL.Tables[strTempFileNm].Rows[0]["Value02"].ToString();
                                clsUtilsData.Instance.dsSQL.Tables[strTempFileNm].Rows[0]["Value02"] = "";
                            }
                            else if (strTempFileNm == clsUtilsSTRING.conAppConfig || strTempFileNm == clsUtilsSTRING.conDevConfig)
                            {
                                //신규, 수정
                                dr["ChangeYN"] = clsUtilsEnum.eChange.Normal.ToString();
                            }
                            else
                            {
                                dr["ChangeYN"] = clsUtilsEnum.eChange.Normal.ToString();
                                dr["LocalDirectory"] = "";
                            }

                            bSuccess = true;
                        }

                        for (int i = clsUtilsData.Instance.dtApp.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow drSub = clsUtilsData.Instance.dtApp.Rows[i];
                            if (drSub["colAppChangeYN"].ToString() == clsUtilsEnum.eChange.Delete.ToString())
                            {
                                clsUtilsData.Instance.dtApp.Rows.Remove(drSub);
                            }
                            else
                            {
                                drSub["colAppChangeYN"] = clsUtilsEnum.eChange.Normal.ToString();
                            }
                        }

                        for (int i = clsUtilsData.Instance.dtDev.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow drSub = clsUtilsData.Instance.dtDev.Rows[i];
                            if (drSub["colDevChangeYN"].ToString() == clsUtilsEnum.eChange.Delete.ToString())
                            {
                                clsUtilsData.Instance.dtDev.Rows.Remove(drSub);
                            }
                            else
                            {
                                drSub["colDevChangeYN"] = clsUtilsEnum.eChange.Normal.ToString();
                            }
                        }
                    }

                    DataRow[] drVersion = clsUtilsData.Instance.dtList.Select(string.Format("VersionNm = '{0}' and ChangeYN = '{1}'", _strListKey, clsUtilsEnum.eChange.New.ToString()));

                    if (drVersion != null && drVersion.Length > 0)
                    {


                        bool bXmlAdd = false;
                        if (!SetXml("versions", _strXmlPath, out bXmlAdd)) return;

                        if (!clsUtilsFTP.Instance.FtpUpload(_strXmlPath, string.Format("{0}/{1}/{2}", _strServerRoot, _strProgram, clsUtilsSTRING.conVersion)))
                        {
                            MessageBox.Show(clsUtilsSTRING.conFTPUploadFail);
                            return;
                        }

                        cboList.SelectedIndexChanged -= new EventHandler(cboList_SelectedIndexChanged);

                        DataView dv = clsUtilsData.Instance.dtList.DefaultView;
                        dv.Sort = "VersionNm asc";
                        DataTable dtCopy = dv.ToTable().Copy();
                        clsUtilsData.Instance.dtList.Clear();

                        foreach (DataRow drTemp in dtCopy.Rows)
                        {
                            clsUtilsData.Instance.AddRowList(
                                drTemp["VersionNm"].ToString(),
                                drTemp["VersionText"].ToString(),
                                drTemp["ServerPath"].ToString(),
                                clsUtilsEnum.eChange.Normal.ToString());
                        }

                        int iRow = 0;
                        for (int i = 0; i < cboList.Items.Count; i++)
                        {
                            if (((DataRowView)(cboList.Items[i])).Row.ItemArray[0].ToString() == _strListKey)
                            {
                                iRow = i;
                                break;
                            }
                        }
                        cboList.SelectedIndex = iRow;
                        cboList.SelectedIndexChanged += new EventHandler(cboList_SelectedIndexChanged);

                        bSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                if (bSuccess)
                {
                    MessageBox.Show(_strListText + "\r\n서버 적용 완료", "저장성공");
                }

                cProgress.ChangeProgress(clsUtilsSTRING.conProgressNormalStatus, 0, 0, string.Empty, clsUtilsEnum.eProcess.eNormal, false);

                SetControlEnable(true);

                if (!bSaveConfirm)
                {
                    if (strGubun == "new")
                    {
                        btnTree_Click(btnTreeNew, null);
                    }
                    else if (strGubun == "program")
                    {
                        for (int i = 0; i < cboProgram.Items.Count; i++)
                        {
                            if (cboProgram.Items[i].ToString() == strValue)
                            {
                                cboProgram.SelectedIndex = i;
                                cboProgram_SelectedIndexChanged(cboProgram, null);
                                break;
                            }
                        }
                    }
                    else if (strGubun == "list")
                    {
                        for (int i = 0; i < cboList.Items.Count; i++)
                        {
                            if (((System.Data.DataRowView)(cboList.Items[i])).Row.ItemArray[0].ToString() == strValue)
                            {
                                cboList.SelectedIndex = i;
                                cboList_SelectedIndexChanged(cboList, null);
                                break;
                            }
                        }
                    }
                    else if (strGubun == "select")
                    {
                        btnSelect_Click(btnSelect, null);
                    }
                    else if (strGubun == "close")
                    {

                    }
                }
            }
        }

        /// <summary>
        /// XML 저장
        /// </summary>
        /// <param name="strGubun">구분</param>
        /// <param name="XmlPath">XML경로</param>
        /// <param name="dt">XML세부내용</param>
        /// <returns></returns>
        private bool SetXml(string strGubun, string XmlPath, out bool bAdd)
        {
            bool bReturn = false;
            bAdd = false;

            try
            {
                DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(XmlPath));

                if (!di.Exists)
                {
                    di.Create();
                }

                XmlDocument doc = new XmlDocument();
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", "no");
                XmlElement ele = doc.CreateElement(strGubun);
                doc.AppendChild(dec);
                doc.AppendChild(ele);
                XmlElement child = null;

                if (strGubun == "versions")
                {
                    DataView dv = clsUtilsData.Instance.dtList.DefaultView;
                    dv.Sort = "VersionNm asc";

                    foreach (DataRow drTemp in dv.ToTable().Rows)
                    {
                        if (drTemp["ChangeYN"].ToString() != clsUtilsEnum.eChange.Delete.ToString())
                        {
                            child = doc.CreateElement("version");
                            child.InnerText = drTemp["VersionNm"].ToString();
                            ele.AppendChild(child);
                            bAdd = true;
                        }
                    }

                    doc.Save(XmlPath);
                }
                else if (strGubun == "setup")
                {
                    child = doc.CreateElement("ip");
                    child.InnerText = "124.137.10.25";
                    ele.AppendChild(child);

                    child = doc.CreateElement("port");
                    child.InnerText = "21";
                    ele.AppendChild(child);

                    child = doc.CreateElement("id");
                    child.InnerText = "pos";
                    ele.AppendChild(child);

                    child = doc.CreateElement("pass");
                    child.InnerText = "wmalldev1!";
                    ele.AppendChild(child);

                    child = doc.CreateElement("serverroot");
                    child.InnerText = "/upgrade";
                    ele.AppendChild(child);

                    child = doc.CreateElement("localroot");
                    child.InnerText = "TempFile";
                    ele.AppendChild(child);

                    FileInfo fi = new FileInfo(XmlPath);

                    if (!fi.Exists)
                    {
                        doc.Save(XmlPath);
                    }
                }

                bReturn = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            return bReturn;
        }

        #endregion

        #endregion
    }
}