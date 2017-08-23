using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.VersionManager.Utils
{
    public class clsUtilsData
    {
        private static clsUtilsData m_instance = new clsUtilsData();
        public static clsUtilsData Instance
        {
            get
            {
                return m_instance;
            }
        }

        /// <summary>
        /// 업그레이드 관련 데이터테이블 집합
        /// </summary>
        private DataSet _dsVersion;
        public DataSet dsVersion
        {
            get { return this._dsVersion; }
            set { this._dsVersion = value; }
        }

        private DataTable _dtSQL;
        public DataTable dtSQL
        {
            get { return this._dtSQL; }
            set { this._dtSQL = value; }
        }

        private DataTable _dtSQLNm;
        public DataTable dtSQLNm
        {
            get { return this._dtSQLNm; }
            set { this._dtSQLNm = value; }
        }

        /// <summary>
        /// AppConfig.ini 관련 데이터테이블
        /// </summary>
        DataTable _dtApp = new DataTable();
        public DataTable dtApp
        {
            get { return this._dtApp; }
            set { this._dtApp = value; }
        }

        /// <summary>
        /// DevConfig.ini 관련 데이터테이블
        /// </summary>
        DataTable _dtDev = new DataTable();
        public DataTable dtDev
        {
            get { return this._dtDev; }
            set { this._dtDev = value; }
        }

        /// <summary>
        /// MstChange.sql 관련 데이터테이블
        /// </summary>
        DataTable _dtMst = new DataTable();
        public DataTable dtMst
        {
            get { return this._dtMst; }
            set { this._dtMst = value; }
        }

        /// <summary>
        /// TranChange.sql 관련 데이터테이블
        /// </summary>
        DataTable _dtTran = new DataTable();
        public DataTable dtTran
        {
            get { return this._dtTran; }
            set { this._dtTran = value; }
        }

        //업그레이드 일자 리스트
        DataTable _dtList = new DataTable();
        public DataTable dtList
        {
            get { return this._dtList; }
            set { this._dtList = value; }
        }

        //업그레이드 리스트
        DataTable _dtUpList = new DataTable();
        public DataTable dtUpList
        {
            get { return this._dtUpList; }
            set { this._dtUpList = value; }
        }

        #region MstChanges.sql 및 TranChanges.sql

        /// <summary>
        /// MstChanges.sql 및 TranChanges.sql 관련 데이터셋
        /// </summary>
        DataSet _dsSQL = new DataSet();
        public DataSet dsSQL
        {
            get { return this._dsSQL; }
            set { this._dsSQL = value; }
        }

        DataSet _dsConfig = new DataSet();
        public DataSet dsConfig
        {
            get { return this._dsConfig; }
            set { this._dsConfig = value; }
        }

        /// <summary>
        /// MstChanges.sql 및 TranChanges.sql 관련 데이터 테이블 생성
        /// </summary>
        public void SetDataTable()
        {
            //Version 리스트
            _dtList.Columns.Add("VersionNm");
            _dtList.Columns.Add("VersionText");
            _dtList.Columns.Add("ServerPath");
            _dtList.Columns.Add("ChangeYN");

            //파일관련 리스트
            _dtUpList.Columns.Add("DeleteYN");
            _dtUpList.Columns.Add("VersionNm");
            _dtUpList.Columns.Add("ServerDirectory");
            _dtUpList.Columns.Add("LocalDirectory");
            _dtUpList.Columns.Add("DirectoryDepth01");
            _dtUpList.Columns.Add("DirectoryDepth02");
            _dtUpList.Columns.Add("DirectoryDepth03");
            _dtUpList.Columns.Add("DirectoryDepth04");
            _dtUpList.Columns.Add("DirectoryDepth05");
            _dtUpList.Columns.Add("DirectoryDepth06");
            _dtUpList.Columns.Add("FileNm");
            _dtUpList.Columns.Add("FileYN");
            _dtUpList.Columns.Add("ChangeYN");
            _dtUpList.Columns.Add("DateCreated", typeof(DateTime));
            _dtUpList.Columns.Add("FileSize");

            //AppConfig.ini 그리드 데이터 테이블
            _dtApp.TableName = clsUtilsSTRING.conAppConfig;
            _dtApp.Columns.Add("colAppRealSection");
            _dtApp.Columns.Add("colAppSection");
            _dtApp.Columns.Add("colAppSectionNm");
            _dtApp.Columns.Add("colAppKey");
            _dtApp.Columns.Add("colAppKeyNm");
            _dtApp.Columns.Add("colAppValue");
            _dtApp.Columns.Add("colAppStore");
            _dtApp.Columns.Add("colAppStoreNm");
            _dtApp.Columns.Add("colAppPos");
            _dtApp.Columns.Add("colAppPosNm");
            _dtApp.Columns.Add("colAppChangeYN");

            //DevConfig.ini 그리드 데이터 테이블
            _dtDev.TableName = clsUtilsSTRING.conDevConfig;
            _dtDev.Columns.Add("colDevRealSection");
            _dtDev.Columns.Add("colDevSection");
            _dtDev.Columns.Add("colDevSectionNm");
            _dtDev.Columns.Add("colDevKey");
            _dtDev.Columns.Add("colDevKeyNm");
            _dtDev.Columns.Add("colDevValue");
            _dtDev.Columns.Add("colDevStore");
            _dtDev.Columns.Add("colDevStoreNm");
            _dtDev.Columns.Add("colDevPos");
            _dtDev.Columns.Add("colDevPosNm");
            _dtDev.Columns.Add("colDevChangeYN");

            DataTable dtMst = new DataTable();
            dtMst.TableName = clsUtilsSTRING.conMstChanges;
            dtMst.Columns.Add("Value01");
            dtMst.Columns.Add("Value02");
            dtMst.Columns.Add("LocalPath");
            dtMst.Columns.Add("ChangeYN");
            dtMst.Rows.Add(new object[] { "", "", "", clsUtilsEnum.eChange.Normal.ToString() });
            _dsSQL.Tables.Add(dtMst);

            DataTable dtTran = new DataTable();
            dtTran.TableName = clsUtilsSTRING.conTranChanges;
            dtTran.Columns.Add("Value01");
            dtTran.Columns.Add("Value02");
            dtTran.Columns.Add("LocalPath");
            dtTran.Columns.Add("ChangeYN");
            dtTran.Rows.Add(new object[] { "", "", "", clsUtilsEnum.eChange.Normal.ToString() });
            _dsSQL.Tables.Add(dtTran);
        }

        /// <summary>
        /// 업그레이드 일자 리스트 추가
        /// </summary>
        /// <param name="VersionNm">업그레이드 일자</param>
        /// <param name="VersionText">업그레이드 일자</param>
        /// <param name="ServerPath">서버경로</param>
        /// <param name="ChangeYN">변경여부</param>
        public void AddRowList(string VersionNm, string VersionText, string ServerPath, string ChangeYN)
        {
            try
            {
                DataRow NewDr = _dtList.NewRow();
                NewDr["VersionNm"] = VersionNm;
                NewDr["VersionText"] = VersionText;
                NewDr["ServerPath"] = ServerPath;
                NewDr["ChangeYN"] = ChangeYN;
                _dtList.Rows.Add(NewDr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 업그레이드 리스트 추가
        /// </summary>
        /// <param name="DeleteYN">삭제여부</param>
        /// <param name="VersionNm">업그레이드 일자</param>
        /// <param name="ChangeYN">변경여부</param>
        /// <param name="FileYN">파일여부</param>
        /// <param name="ServerDirectory">서버경로</param>
        /// <param name="FileNm">파일명</param>
        /// <param name="LocalDirectory">로컬경로</param>
        /// <param name="FileSize">파일사이즈</param>
        /// <param name="DateCreated">수정일자</param>
        /// <param name="bBasic">기본설정</param>
        /// <returns></returns>
        public bool AddRowUpList(
            string DeleteYN, 
            string VersionNm, 
            clsUtilsEnum.eChange ChangeYN, 
            string FileYN,
            string ServerDirectory, 
            string FileNm, 
            string LocalDirectory,
            string FileSize, 
            DateTime DateCreated,
            bool bBasic)
        {
            bool bReturn = false;

            try
            {
                if (_dtUpList != null)
                {
                    if (bBasic)
                    {
                        AddRowUpList(DeleteYN, VersionNm, ChangeYN, FileYN, ServerDirectory, clsUtilsSTRING.conBin, "", "", DateCreated, false);
                        AddRowUpList(DeleteYN, VersionNm, ChangeYN, FileYN, string.Format("{0}/{1}", ServerDirectory, clsUtilsSTRING.conBin), clsUtilsSTRING.conLib, "", "", DateCreated, false);
                        AddRowUpList(DeleteYN, VersionNm, ChangeYN, FileYN, string.Format("{0}/{1}/{2}", ServerDirectory, clsUtilsSTRING.conBin, clsUtilsSTRING.conLib), clsUtilsSTRING.conDamo, "", "", DateCreated, false);
                        AddRowUpList(DeleteYN, VersionNm, ChangeYN, FileYN, string.Format("{0}/{1}/{2}", ServerDirectory, clsUtilsSTRING.conBin, clsUtilsSTRING.conLib), clsUtilsSTRING.conOcx, "", "", DateCreated, false);
                        AddRowUpList(DeleteYN, VersionNm, ChangeYN, FileYN, ServerDirectory, clsUtilsSTRING.conConfig, "", "", DateCreated, false);
                        AddRowUpList(DeleteYN, VersionNm, ChangeYN, FileYN, ServerDirectory, clsUtilsSTRING.conData, "", "", DateCreated, false);
                        AddRowUpList(DeleteYN, VersionNm, ChangeYN, FileYN, string.Format("{0}/{1}", ServerDirectory, clsUtilsSTRING.conData), clsUtilsSTRING.conMst, "", "", DateCreated, false);
                        AddRowUpList(DeleteYN, VersionNm, ChangeYN, FileYN, string.Format("{0}/{1}", ServerDirectory, clsUtilsSTRING.conData), clsUtilsSTRING.conSchema, "", "", DateCreated, false);
                        AddRowUpList(DeleteYN, VersionNm, ChangeYN, FileYN, string.Format("{0}/{1}", ServerDirectory, clsUtilsSTRING.conData), clsUtilsSTRING.conTran, "", "", DateCreated, false);
                        AddRowUpList(DeleteYN, VersionNm, ChangeYN, FileYN, string.Format("{0}/{1}", ServerDirectory, clsUtilsSTRING.conData), clsUtilsSTRING.conPatch, "", "", DateCreated, false);
                        AddRowUpList(DeleteYN, VersionNm, ChangeYN, FileYN, string.Format("{0}/{1}/{2}", ServerDirectory, clsUtilsSTRING.conData, clsUtilsSTRING.conPatch), clsUtilsSTRING.conMst, "", "", DateCreated, false);
                        AddRowUpList(DeleteYN, VersionNm, ChangeYN, FileYN, string.Format("{0}/{1}/{2}/{3}", ServerDirectory, clsUtilsSTRING.conData, clsUtilsSTRING.conPatch, clsUtilsSTRING.conMst), clsUtilsSTRING.conMstChanges, "", "", DateCreated, false);
                        AddRowUpList(DeleteYN, VersionNm, ChangeYN, FileYN, string.Format("{0}/{1}/{2}", ServerDirectory, clsUtilsSTRING.conData, clsUtilsSTRING.conPatch), clsUtilsSTRING.conTran, "", "", DateCreated, false);
                        AddRowUpList(DeleteYN, VersionNm, ChangeYN, FileYN, string.Format("{0}/{1}/{2}/{3}", ServerDirectory, clsUtilsSTRING.conData, clsUtilsSTRING.conPatch, clsUtilsSTRING.conTran), clsUtilsSTRING.conTranChanges, "", "", DateCreated, false);
                        AddRowUpList(DeleteYN, VersionNm, ChangeYN, FileYN, ServerDirectory, clsUtilsSTRING.conResource, "", "", DateCreated, false);
                        AddRowUpList(DeleteYN, VersionNm, ChangeYN, FileYN, ServerDirectory, clsUtilsSTRING.conUpdate, "", "", DateCreated, false);
                        AddRowUpList(DeleteYN, VersionNm, ChangeYN, FileYN, ServerDirectory, clsUtilsSTRING.conAppConfig, "", "", DateCreated, false);
                        AddRowUpList(DeleteYN, VersionNm, ChangeYN, FileYN, ServerDirectory, clsUtilsSTRING.conDevConfig, "", "", DateCreated, false);
                    }
                    else
                    {
                        DataRow[] drFilter = _dtUpList.Select(
                        string.Format(@"VersionNm = '{0}' and 
                                        ChangeYN = '{1}' and 
                                        FileYN = '{2}' and 
                                        ServerDirectory = '{3}' and 
                                        LocalDirectory = '{4}' and 
                                        FileNm = '{5}'",
                                        VersionNm,
                                        ChangeYN,
                                        FileYN,
                                        ServerDirectory,
                                        LocalDirectory,
                                        FileNm));

                        if (drFilter != null && drFilter.Length > 0)
                        {
                            if (FileNm != clsUtilsSTRING.conAppConfig && FileNm != clsUtilsSTRING.conDevConfig &&
                                FileNm != clsUtilsSTRING.conMstChanges && FileNm != clsUtilsSTRING.conTranChanges)
                            {
                                MessageBox.Show("해당 파일이 존재합니다.");
                            }

                            return false;
                        }

                        string[] arrDepth = ServerDirectory.Split('/');
                        DataRow NewDr = _dtUpList.NewRow();
                        NewDr["DeleteYN"] = DeleteYN;
                        NewDr["VersionNm"] = VersionNm;
                        NewDr["ServerDirectory"] = ServerDirectory;
                        NewDr["LocalDirectory"] = LocalDirectory;
                        NewDr["DirectoryDepth01"] = arrDepth != null && arrDepth.Length >= 2 ? arrDepth[1] : "";
                        NewDr["DirectoryDepth02"] = arrDepth != null && arrDepth.Length >= 3 ? arrDepth[2] : "";
                        NewDr["DirectoryDepth03"] = arrDepth != null && arrDepth.Length >= 4 ? arrDepth[3] : "";
                        NewDr["DirectoryDepth04"] = arrDepth != null && arrDepth.Length >= 5 ? arrDepth[4] : "";
                        NewDr["DirectoryDepth05"] = arrDepth != null && arrDepth.Length >= 6 ? arrDepth[5] : "";
                        NewDr["DirectoryDepth06"] = arrDepth != null && arrDepth.Length >= 7 ? arrDepth[6] : "";
                        NewDr["FileNm"] = FileNm;
                        NewDr["FileYN"] = FileYN;
                        NewDr["DateCreated"] = DateCreated;
                        NewDr["FileSize"] = FileSize.Length > 0 ? string.Format("{0:#,##0}KB", Convert.ToInt64(FileSize)) : "";
                        NewDr["ChangeYN"] = ChangeYN;
                        _dtUpList.Rows.Add(NewDr);
                    }

                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                throw;
            }

            return bReturn;
        }

        /// <summary>
        /// AppConfig.ini 및 DevConfig.ini 항목 추가
        /// </summary>
        /// <param name="dtTemp"></param>
        /// <param name="strTempSection"></param>
        /// <param name="strTempKey"></param>
        /// <param name="strTempValue"></param>
        /// <param name="strTempStroeNo"></param>
        /// <param name="strTempPosNo"></param>
        public void ConfigAddrow(DataTable dtTemp, string strTempSection, string strTempKey, string strTempValue, string strTempStroeNo, string strTempPosNo)
        {
            try
            {
                DataRow NewDr = dtTemp.NewRow();
                NewDr[0] = strTempSection;
                if (strTempSection == clsUtilsSTRING.conINIPosComm)
                {
                    switch (strTempKey.Trim())
                    {
                        case clsUtilsSTRING.conPosCommSvrIP1:
                        case clsUtilsSTRING.conPosCommSvrIP2:
                        case clsUtilsSTRING.conPosCommComDPort1:
                        case clsUtilsSTRING.conPosCommComDPort2:
                        case clsUtilsSTRING.conPosCommComDTimeOut:
                        case clsUtilsSTRING.conPosCommComUPort1:
                        case clsUtilsSTRING.conPosCommComUPort2:
                        case clsUtilsSTRING.conPosCommComUTimeOut:
                        case clsUtilsSTRING.conPosCommComQPort1:
                        case clsUtilsSTRING.conPosCommComQPort2:
                        case clsUtilsSTRING.conPosCommComQTimeOut:
                            NewDr[1] = clsUtilsSTRING.conINIPosComm01;
                            NewDr[2] = clsUtilsSTRING.conINIPosComm01Nm;
                            break;
                        default:
                            NewDr[1] = clsUtilsSTRING.conINIPosComm02;
                            NewDr[2] = clsUtilsSTRING.conINIPosComm02Nm;
                            break;
                    }
                }
                else
                {
                    NewDr[1] = strTempSection;
                    switch (strTempSection.Trim())
                    {
                        case clsUtilsSTRING.conINIPosFTP:
                            NewDr[2] = clsUtilsSTRING.conINIPosFTPNm;
                            break;
                        case clsUtilsSTRING.conINIPosVan:
                            NewDr[2] = clsUtilsSTRING.conINIPosVanNm;
                            break;
                        case clsUtilsSTRING.conINIPosOption:
                            NewDr[2] = clsUtilsSTRING.conINIPosOptionNm;
                            break;
                        default:
                            NewDr[2] = strTempSection;
                            break;
                    }
                }

                NewDr[3] = strTempKey.Trim();

                switch (strTempKey.Trim())
                {
                    case clsUtilsSTRING.conPosCommSvrIP1:
                        NewDr[4] = clsUtilsSTRING.conPosCommSvrIP1Nm;
                        break;
                    case clsUtilsSTRING.conPosCommSvrIP2:
                        NewDr[4] = clsUtilsSTRING.conPosCommSvrIP2Nm;
                        break;
                    case clsUtilsSTRING.conPosCommComDPort1:
                        NewDr[4] = clsUtilsSTRING.conPosCommComDPort1Nm;
                        break;
                    case clsUtilsSTRING.conPosCommComDPort2:
                        NewDr[4] = clsUtilsSTRING.conPosCommComDPort2Nm;
                        break;
                    case clsUtilsSTRING.conPosCommComDTimeOut:
                        NewDr[4] = clsUtilsSTRING.conPosCommComDTimeOutNm;
                        break;
                    case clsUtilsSTRING.conPosCommComUPort1:
                        NewDr[4] = clsUtilsSTRING.conPosCommComUPort1Nm;
                        break;
                    case clsUtilsSTRING.conPosCommComUPort2:
                        NewDr[4] = clsUtilsSTRING.conPosCommComUPort2Nm;
                        break;
                    case clsUtilsSTRING.conPosCommComUTimeOut:
                        NewDr[4] = clsUtilsSTRING.conPosCommComUTimeOutNm;
                        break;
                    case clsUtilsSTRING.conPosCommComQPort1:
                        NewDr[4] = clsUtilsSTRING.conPosCommComQPort1Nm;
                        break;
                    case clsUtilsSTRING.conPosCommComQPort2:
                        NewDr[4] = clsUtilsSTRING.conPosCommComQPort2Nm;
                        break;
                    case clsUtilsSTRING.conPosCommComQTimeOut:
                        NewDr[4] = clsUtilsSTRING.conPosCommComQTimeOutNm;
                        break;

                    case clsUtilsSTRING.conPosCommSvrGftIP1:
                        NewDr[4] = clsUtilsSTRING.conPosCommSvrGftIP1Nm;
                        break;
                    case clsUtilsSTRING.conPosCommSvrGftIP2:
                        NewDr[4] = clsUtilsSTRING.conPosCommSvrGftIP2Nm;
                        break;
                    case clsUtilsSTRING.conPosCommComGPort1:
                        NewDr[4] = clsUtilsSTRING.conPosCommComGPort1Nm;
                        break;
                    case clsUtilsSTRING.conPosCommComGPort2:
                        NewDr[4] = clsUtilsSTRING.conPosCommComGPort2Nm;
                        break;
                    case clsUtilsSTRING.conPosCommComGTimeOut:
                        NewDr[4] = clsUtilsSTRING.conPosCommComGTimeOutNm;
                        break;
                    case clsUtilsSTRING.conPosCommSvrPntIP1:
                        NewDr[4] = clsUtilsSTRING.conPosCommSvrPntIP1Nm;
                        break;
                    case clsUtilsSTRING.conPosCommSvrPntIP2:
                        NewDr[4] = clsUtilsSTRING.conPosCommSvrPntIP2Nm;
                        break;
                    case clsUtilsSTRING.conPosCommComPPort1:
                        NewDr[4] = clsUtilsSTRING.conPosCommComPPort1Nm;
                        break;
                    case clsUtilsSTRING.conPosCommComPPort2:
                        NewDr[4] = clsUtilsSTRING.conPosCommComPPort2Nm;
                        break;
                    case clsUtilsSTRING.conPosCommComPTimeOut:
                        NewDr[4] = clsUtilsSTRING.conPosCommComPTimeOutNm;
                        break;
                    case clsUtilsSTRING.conPosCommHqSvrIP1:
                        NewDr[4] = clsUtilsSTRING.conPosCommHqSvrIP1Nm;
                        break;
                    case clsUtilsSTRING.conPosCommHqSvrIP2:
                        NewDr[4] = clsUtilsSTRING.conPosCommHqSvrIP2Nm;
                        break;
                    case clsUtilsSTRING.conPosCommHqComQPort1:
                        NewDr[4] = clsUtilsSTRING.conPosCommHqComQPort1Nm;
                        break;
                    case clsUtilsSTRING.conPosCommHqComQPort2:
                        NewDr[4] = clsUtilsSTRING.conPosCommHqComQPort2Nm;
                        break;
                    case clsUtilsSTRING.conPosCommHqComQTimeOut:
                        NewDr[4] = clsUtilsSTRING.conPosCommHqComQTimeOutNm;
                        break;
                    case clsUtilsSTRING.conPosCommHqComUPort1:
                        NewDr[4] = clsUtilsSTRING.conPosCommHqComUPort1Nm;
                        break;
                    case clsUtilsSTRING.conPosCommHqComUPort2:
                        NewDr[4] = clsUtilsSTRING.conPosCommHqComUPort2Nm;
                        break;
                    case clsUtilsSTRING.conPosCommHqComUTimeOut:
                        NewDr[4] = clsUtilsSTRING.conPosCommHqComUTimeOutNm;
                        break;

                    case clsUtilsSTRING.conPosFTPFtpSvrIP1:
                        NewDr[4] = clsUtilsSTRING.conPosFTPFtpSvrIP1Nm;
                        break;
                    case clsUtilsSTRING.conPosFTPFtpSvrIP2:
                        NewDr[4] = clsUtilsSTRING.conPosFTPFtpSvrIP2Nm;
                        break;
                    case clsUtilsSTRING.conPosFTPFtpComPort1:
                        NewDr[4] = clsUtilsSTRING.conPosFTPFtpComPort1Nm;
                        break;
                    case clsUtilsSTRING.conPosFTPFtpComPort2:
                        NewDr[4] = clsUtilsSTRING.conPosFTPFtpComPort2Nm;
                        break;
                    case clsUtilsSTRING.conPosFTPMode:
                        NewDr[4] = clsUtilsSTRING.conPosFTPModeNm;
                        break;
                    case clsUtilsSTRING.conPosFTPUser:
                        NewDr[4] = clsUtilsSTRING.conPosFTPUserNm;
                        break;
                    case clsUtilsSTRING.conPosFTPPass:
                        NewDr[4] = clsUtilsSTRING.conPosFTPPassNm;
                        break;
                    case clsUtilsSTRING.conPosFTPJournalPath:
                        NewDr[4] = clsUtilsSTRING.conPosFTPJournalPathNm;
                        break;
                    case clsUtilsSTRING.conPosFTPVersionInfoPath:
                        NewDr[4] = clsUtilsSTRING.conPosFTPVersionInfoPathNm;
                        break;
                    case clsUtilsSTRING.conPosFTPDataFileDownloadPath:
                        NewDr[4] = clsUtilsSTRING.conPosFTPDataFileDownloadPathNm;
                        break;
                    case clsUtilsSTRING.conPosFTPCreateUploadPathByDate:
                        NewDr[4] = clsUtilsSTRING.conPosFTPCreateUploadPathByDateNm;
                        break;

                    case clsUtilsSTRING.conPosVANVanSvrIP1:
                        NewDr[4] = clsUtilsSTRING.conPosVANVanSvrIP1Nm;
                        break;
                    case clsUtilsSTRING.conPosVANVanSvrIP2:
                        NewDr[4] = clsUtilsSTRING.conPosVANVanSvrIP2Nm;
                        break;
                    case clsUtilsSTRING.conPosVANVanComPort1:
                        NewDr[4] = clsUtilsSTRING.conPosVANVanComPort1Nm;
                        break;
                    case clsUtilsSTRING.conPosVANVanComPort2:
                        NewDr[4] = clsUtilsSTRING.conPosVANVanComPort2Nm;
                        break;
                    case clsUtilsSTRING.conPosVANComTimeOut:
                        NewDr[4] = clsUtilsSTRING.conPosVANComTimeOutNm;
                        break;

                    case clsUtilsSTRING.conPosOptionPointUse:
                        NewDr[4] = clsUtilsSTRING.conPosOptionPointUseNm;
                        break;
                    case clsUtilsSTRING.conPosOptionPointSchemePrefix:
                        NewDr[4] = clsUtilsSTRING.conPosOptionPointSchemePrefixNm;
                        break;
                    case clsUtilsSTRING.conPosOptionPointPayKeyInputEnable:
                        NewDr[4] = clsUtilsSTRING.conPosOptionPointPayKeyInputEnableNm;
                        break;
                    case clsUtilsSTRING.conPosOptionCashReceiptUse:
                        NewDr[4] = clsUtilsSTRING.conPosOptionCashReceiptUseNm;
                        break;
                    case clsUtilsSTRING.conPosOptionCashReceiptIssue:
                        NewDr[4] = clsUtilsSTRING.conPosOptionCashReceiptIssueNm;
                        break;
                    case clsUtilsSTRING.conPosOptionCashReceiptApplAmount:
                        NewDr[4] = clsUtilsSTRING.conPosOptionCashReceiptApplAmountNm;
                        break;
                    case clsUtilsSTRING.conPosOptionGoodsCodePrefix1:
                        NewDr[4] = clsUtilsSTRING.conPosOptionGoodsCodePrefix1Nm;
                        break;
                    case clsUtilsSTRING.conPosOptionGoodsCodePrefix2:
                        NewDr[4] = clsUtilsSTRING.conPosOptionGoodsCodePrefix2Nm;
                        break;
                    case clsUtilsSTRING.conPosOptionDataKeepDays:
                        NewDr[4] = clsUtilsSTRING.conPosOptionDataKeepDaysNm;
                        break;
                    case clsUtilsSTRING.conPosOptionAutoReturnCardRead:
                        NewDr[4] = clsUtilsSTRING.conPosOptionAutoReturnCardReadNm;
                        break;
                    case clsUtilsSTRING.conPosOptionSalesReturn:
                        NewDr[4] = clsUtilsSTRING.conPosOptionSalesReturnNm;
                        break;
                    case clsUtilsSTRING.conPosOptionSignUploadTask:
                        NewDr[4] = clsUtilsSTRING.conPosOptionSignUploadTaskNm;
                        break;
                    case clsUtilsSTRING.conPosOptionTransUploadTask:
                        NewDr[4] = clsUtilsSTRING.conPosOptionTransUploadTaskNm;
                        break;
                    case clsUtilsSTRING.conPosOptionTransStatusTask:
                        NewDr[4] = clsUtilsSTRING.conPosOptionTransStatusTaskNm;
                        break;
                    case clsUtilsSTRING.conPosOptionNoticeStatusTask:
                        NewDr[4] = clsUtilsSTRING.conPosOptionNoticeStatusTaskNm;
                        break;

                    case clsUtilsSTRING.conUse:
                        NewDr[4] = clsUtilsSTRING.conUseNm;
                        break;
                    case clsUtilsSTRING.conMethod:
                        NewDr[4] = clsUtilsSTRING.conMethodNm01;
                        break;
                    case clsUtilsSTRING.conLogicalName:
                        NewDr[4] = clsUtilsSTRING.conLogicalNameNm;
                        break;
                    case clsUtilsSTRING.conPort:
                        NewDr[4] = clsUtilsSTRING.conPortNm;
                        break;
                    case clsUtilsSTRING.conSpeed:
                        NewDr[4] = clsUtilsSTRING.conSpeedNm;
                        break;
                    case clsUtilsSTRING.conDataBit:
                        NewDr[4] = clsUtilsSTRING.conDataBitNm;
                        break;
                    case clsUtilsSTRING.conStopBit:
                        NewDr[4] = clsUtilsSTRING.conStopBitNm;
                        break;
                    case clsUtilsSTRING.conParity:
                        NewDr[4] = clsUtilsSTRING.conParityNm;
                        break;
                    case clsUtilsSTRING.conFlowControl:
                        NewDr[4] = clsUtilsSTRING.conFlowControlNm;
                        break;
                    case clsUtilsSTRING.conLogoBMP:
                        NewDr[4] = clsUtilsSTRING.conLogoBMPNm;
                        break;
                    case clsUtilsSTRING.conCutFeedCn:
                        NewDr[4] = clsUtilsSTRING.conCutFeedCnNm;
                        break;
                    case clsUtilsSTRING.conBarCodeWidth:
                        NewDr[4] = clsUtilsSTRING.conBarCodeWidthNm;
                        break;
                    case clsUtilsSTRING.conBarCodeHeight:
                        NewDr[4] = clsUtilsSTRING.conBarCodeHeightNm;
                        break;
                    default:
                        break;
                }
                NewDr[5] = strTempValue;
                NewDr[6] = strTempStroeNo;
                NewDr[7] = strTempStroeNo == "**" ? "전체" : strTempStroeNo;
                NewDr[8] = strTempPosNo;
                NewDr[9] = strTempPosNo == "****" ? "전체" : strTempPosNo;
                NewDr[10] = clsUtilsEnum.eChange.Normal.ToString();
                
                dtTemp.Rows.Add(NewDr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 업그레이드 관련 데이터테이블 집합 셋팅
        /// </summary>
        /// <returns></returns>
        public bool SetVersion()
        {
            try
            {
                //프로그램 리스트
                DataTable dtProgram = new DataTable();
                dtProgram.Columns.Add("Key");
                dtProgram.Columns.Add("Value");
                dtProgram.Rows.Add(new object[] { clsUtilsSTRING.conServerPOS, clsUtilsSTRING.conServerPOS });
                dtProgram.Rows.Add(new object[] { clsUtilsSTRING.conServerPDA, clsUtilsSTRING.conServerPDA });
                _dsVersion.Tables.Add(dtProgram);

                //Version 리스트
                DataTable dtVersion = new DataTable();
                dtVersion.TableName = clsUtilsSTRING.conVersion;
                dtVersion.Columns.Add("VersionNm");
                dtVersion.Columns.Add("VersionText");
                dtVersion.Columns.Add("ServerPath");
                dtVersion.Columns.Add("ChangeYN");
                _dsVersion.Tables.Add(dtVersion);

                //파일관련 리스트
                DataTable dtFile = new DataTable();
                dtFile.TableName = clsUtilsSTRING.conFile;
                dtFile.Columns.Add("DeleteYN");
                dtFile.Columns.Add("VersionNm");
                dtFile.Columns.Add("ServerDirectory");
                dtFile.Columns.Add("LocalDirectory");
                dtFile.Columns.Add("DirectoryDepth01");
                dtFile.Columns.Add("DirectoryDepth02");
                dtFile.Columns.Add("DirectoryDepth03");
                dtFile.Columns.Add("DirectoryDepth04");
                dtFile.Columns.Add("DirectoryDepth05");
                dtFile.Columns.Add("DirectoryDepth06");
                dtFile.Columns.Add("FileNm");
                dtFile.Columns.Add("FileYN");
                dtFile.Columns.Add("ChangeYN");
                dtFile.Columns.Add("DateCreated", typeof(DateTime));
                dtFile.Columns.Add("FileSize");
                _dsVersion.Tables.Add(dtFile);

                //AppConfig.ini 그리드 데이터 테이블
                DataTable dtConfigApp = new DataTable();
                dtConfigApp.TableName = clsUtilsSTRING.conAppConfig;
                dtConfigApp.Columns.Add("RealSection");
                dtConfigApp.Columns.Add("Section");
                dtConfigApp.Columns.Add("SectionNm");
                dtConfigApp.Columns.Add("Key");
                dtConfigApp.Columns.Add("KeyNm");
                dtConfigApp.Columns.Add("Value");
                dtConfigApp.Columns.Add("Store");
                dtConfigApp.Columns.Add("StoreNm");
                dtConfigApp.Columns.Add("Pos");
                dtConfigApp.Columns.Add("PosNm");
                dtConfigApp.Columns.Add("ChangeYN");
                _dsVersion.Tables.Add(dtConfigApp);

                DataTable dtAppSection = new DataTable();
                dtAppSection.TableName = clsUtilsSTRING.conAppSection;
                dtAppSection.Columns.Add("Key");
                dtAppSection.Columns.Add("Value");
                dtAppSection.Rows.Add(new object[] { "", "" });
                dtAppSection.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm01, clsUtilsSTRING.conINIPosComm01Nm });
                dtAppSection.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conINIPosComm02Nm });
                dtAppSection.Rows.Add(new object[] { clsUtilsSTRING.conINIPosFTP, clsUtilsSTRING.conINIPosFTPNm });
                dtAppSection.Rows.Add(new object[] { clsUtilsSTRING.conINIPosVan, clsUtilsSTRING.conINIPosVanNm });
                dtAppSection.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, clsUtilsSTRING.conINIPosOptionNm });
                _dsVersion.Tables.Add(dtAppSection);

                DataTable dtAppKey = new DataTable();
                dtAppKey.TableName = clsUtilsSTRING.conAppKey;
                dtAppKey.Columns.Add("Section");
                dtAppKey.Columns.Add("Key");
                dtAppKey.Columns.Add("Value");
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm01, "", "" });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm01, clsUtilsSTRING.conPosCommSvrIP1, clsUtilsSTRING.conPosCommSvrIP1Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm01, clsUtilsSTRING.conPosCommComDPort1, clsUtilsSTRING.conPosCommComDPort1Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm01, clsUtilsSTRING.conPosCommComUPort1, clsUtilsSTRING.conPosCommComUPort1Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm01, clsUtilsSTRING.conPosCommComQPort1, clsUtilsSTRING.conPosCommComQPort1Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm01, clsUtilsSTRING.conPosCommSvrIP2, clsUtilsSTRING.conPosCommSvrIP2Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm01, clsUtilsSTRING.conPosCommComDPort2, clsUtilsSTRING.conPosCommComDPort2Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm01, clsUtilsSTRING.conPosCommComUPort2, clsUtilsSTRING.conPosCommComUPort2Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm01, clsUtilsSTRING.conPosCommComQPort2, clsUtilsSTRING.conPosCommComQPort2Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm01, clsUtilsSTRING.conPosCommComDTimeOut, clsUtilsSTRING.conPosCommComDTimeOutNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm01, clsUtilsSTRING.conPosCommComUTimeOut, clsUtilsSTRING.conPosCommComUTimeOutNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm01, clsUtilsSTRING.conPosCommComQTimeOut, clsUtilsSTRING.conPosCommComQTimeOutNm });

                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, "", "" });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conPosCommSvrGftIP1, clsUtilsSTRING.conPosCommSvrGftIP1Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conPosCommComGPort1, clsUtilsSTRING.conPosCommComGPort1Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conPosCommSvrGftIP2, clsUtilsSTRING.conPosCommSvrGftIP2Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conPosCommComGPort2, clsUtilsSTRING.conPosCommComGPort2Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conPosCommComGTimeOut, clsUtilsSTRING.conPosCommComGTimeOutNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conPosCommSvrPntIP1, clsUtilsSTRING.conPosCommSvrPntIP1Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conPosCommComPPort1, clsUtilsSTRING.conPosCommComPPort1Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conPosCommSvrPntIP2, clsUtilsSTRING.conPosCommSvrPntIP2Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conPosCommComPPort2, clsUtilsSTRING.conPosCommComPPort2Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conPosCommComPTimeOut, clsUtilsSTRING.conPosCommComPTimeOutNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conPosCommHqSvrIP1, clsUtilsSTRING.conPosCommHqSvrIP1Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conPosCommHqComQPort1, clsUtilsSTRING.conPosCommHqComQPort1Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conPosCommHqComUPort1, clsUtilsSTRING.conPosCommHqComUPort1Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conPosCommHqSvrIP2, clsUtilsSTRING.conPosCommHqSvrIP2Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conPosCommHqComQPort2, clsUtilsSTRING.conPosCommHqComQPort2Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conPosCommHqComUPort2, clsUtilsSTRING.conPosCommHqComUPort2Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conPosCommHqComQTimeOut, clsUtilsSTRING.conPosCommHqComQTimeOutNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conPosCommHqComUTimeOut, clsUtilsSTRING.conPosCommHqComUTimeOutNm });

                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosFTP, "", "" });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosFTP, clsUtilsSTRING.conPosFTPFtpSvrIP1, clsUtilsSTRING.conPosFTPFtpSvrIP1Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosFTP, clsUtilsSTRING.conPosFTPFtpComPort1, clsUtilsSTRING.conPosFTPFtpComPort1Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosFTP, clsUtilsSTRING.conPosFTPFtpSvrIP2, clsUtilsSTRING.conPosFTPFtpSvrIP2Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosFTP, clsUtilsSTRING.conPosFTPFtpComPort2, clsUtilsSTRING.conPosFTPFtpComPort2Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosFTP, clsUtilsSTRING.conPosFTPMode, clsUtilsSTRING.conPosFTPModeNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosFTP, clsUtilsSTRING.conPosFTPUser, clsUtilsSTRING.conPosFTPUserNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosFTP, clsUtilsSTRING.conPosFTPPass, clsUtilsSTRING.conPosFTPPassNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosFTP, clsUtilsSTRING.conPosFTPJournalPath, clsUtilsSTRING.conPosFTPJournalPathNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosFTP, clsUtilsSTRING.conPosFTPVersionInfoPath, clsUtilsSTRING.conPosFTPVersionInfoPathNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosFTP, clsUtilsSTRING.conPosFTPDataFileDownloadPath, clsUtilsSTRING.conPosFTPDataFileDownloadPathNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosFTP, clsUtilsSTRING.conPosFTPCreateUploadPathByDate, clsUtilsSTRING.conPosFTPCreateUploadPathByDateNm });

                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosVan, "", "" });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosVan, clsUtilsSTRING.conPosVANVanSvrIP1, clsUtilsSTRING.conPosVANVanSvrIP1Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosVan, clsUtilsSTRING.conPosVANVanComPort1, clsUtilsSTRING.conPosVANVanComPort1Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosVan, clsUtilsSTRING.conPosVANVanSvrIP2, clsUtilsSTRING.conPosVANVanSvrIP2Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosVan, clsUtilsSTRING.conPosVANVanComPort2, clsUtilsSTRING.conPosVANVanComPort2Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosVan, clsUtilsSTRING.conPosVANComTimeOut, clsUtilsSTRING.conPosVANComTimeOutNm });

                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, "", "" });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, clsUtilsSTRING.conPosOptionPointUse, clsUtilsSTRING.conPosOptionPointUseNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, clsUtilsSTRING.conPosOptionPointSchemePrefix, clsUtilsSTRING.conPosOptionPointSchemePrefixNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, clsUtilsSTRING.conPosOptionPointPayKeyInputEnable, clsUtilsSTRING.conPosOptionPointPayKeyInputEnableNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, clsUtilsSTRING.conPosOptionCashReceiptUse, clsUtilsSTRING.conPosOptionCashReceiptUseNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, clsUtilsSTRING.conPosOptionCashReceiptIssue, clsUtilsSTRING.conPosOptionCashReceiptIssueNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, clsUtilsSTRING.conPosOptionCashReceiptApplAmount, clsUtilsSTRING.conPosOptionCashReceiptApplAmountNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, clsUtilsSTRING.conPosOptionGoodsCodePrefix1, clsUtilsSTRING.conPosOptionGoodsCodePrefix1Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, clsUtilsSTRING.conPosOptionGoodsCodePrefix2, clsUtilsSTRING.conPosOptionGoodsCodePrefix2Nm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, clsUtilsSTRING.conPosOptionDataKeepDays, clsUtilsSTRING.conPosOptionDataKeepDaysNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, clsUtilsSTRING.conPosOptionAutoReturnCardRead, clsUtilsSTRING.conPosOptionAutoReturnCardReadNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, clsUtilsSTRING.conPosOptionSalesReturn, clsUtilsSTRING.conPosOptionSalesReturnNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, clsUtilsSTRING.conPosOptionSignUploadTask, clsUtilsSTRING.conPosOptionSignUploadTaskNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, clsUtilsSTRING.conPosOptionTransUploadTask, clsUtilsSTRING.conPosOptionTransUploadTaskNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, clsUtilsSTRING.conPosOptionTransStatusTask, clsUtilsSTRING.conPosOptionTransStatusTaskNm });
                dtAppKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, clsUtilsSTRING.conPosOptionNoticeStatusTask, clsUtilsSTRING.conPosOptionNoticeStatusTaskNm });
                _dsVersion.Tables.Add(dtAppKey);

                //DevConfig.ini 그리드 데이터 테이블
                DataTable dtConfigDev = new DataTable();
                dtConfigDev.TableName = clsUtilsSTRING.conDevConfig;
                dtConfigDev.Columns.Add("RealSection");
                dtConfigDev.Columns.Add("Section");
                dtConfigDev.Columns.Add("SectionNm");
                dtConfigDev.Columns.Add("Key");
                dtConfigDev.Columns.Add("KeyNm");
                dtConfigDev.Columns.Add("Value");
                dtConfigDev.Columns.Add("Store");
                dtConfigDev.Columns.Add("StoreNm");
                dtConfigDev.Columns.Add("Pos");
                dtConfigDev.Columns.Add("PosNm");
                dtConfigDev.Columns.Add("ChangeYN");
                _dsVersion.Tables.Add(dtConfigDev);

                DataTable dtDevSection = new DataTable();
                dtDevSection.TableName = clsUtilsSTRING.conDevSection;
                dtDevSection.Columns.Add("Key");
                dtDevSection.Columns.Add("Value");
                dtDevSection.Rows.Add(new object[] { "", "" });
                dtDevSection.Rows.Add(new object[] { clsUtilsSTRING.conINIScannerGun, clsUtilsSTRING.conINIScannerGun });
                dtDevSection.Rows.Add(new object[] { clsUtilsSTRING.conINILineDisplay, clsUtilsSTRING.conINILineDisplay });
                dtDevSection.Rows.Add(new object[] { clsUtilsSTRING.conINIPrinter, clsUtilsSTRING.conINIPrinter });
                dtDevSection.Rows.Add(new object[] { clsUtilsSTRING.conINIMSR, clsUtilsSTRING.conINIMSR });
                dtDevSection.Rows.Add(new object[] { clsUtilsSTRING.conINICashDrawer, clsUtilsSTRING.conINICashDrawer });
                dtDevSection.Rows.Add(new object[] { clsUtilsSTRING.conINISignPad, clsUtilsSTRING.conINISignPad });
                _dsVersion.Tables.Add(dtDevSection);

                DataTable dtDevKey = new DataTable();
                dtDevKey.TableName = clsUtilsSTRING.conDevKey;
                dtDevKey.Columns.Add("Section");
                dtDevKey.Columns.Add("Key");
                dtDevKey.Columns.Add("Value");
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIScannerGun, "", "" });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIScannerGun, clsUtilsSTRING.conUse, clsUtilsSTRING.conUseNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIScannerGun, clsUtilsSTRING.conMethod, clsUtilsSTRING.conMethodNm01 });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIScannerGun, clsUtilsSTRING.conLogicalName, clsUtilsSTRING.conLogicalNameNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIScannerGun, clsUtilsSTRING.conPort, clsUtilsSTRING.conPortNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIScannerGun, clsUtilsSTRING.conSpeed, clsUtilsSTRING.conSpeedNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIScannerGun, clsUtilsSTRING.conDataBit, clsUtilsSTRING.conDataBitNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIScannerGun, clsUtilsSTRING.conStopBit, clsUtilsSTRING.conStopBitNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIScannerGun, clsUtilsSTRING.conParity, clsUtilsSTRING.conParityNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIScannerGun, clsUtilsSTRING.conFlowControl, clsUtilsSTRING.conFlowControlNm });

                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINILineDisplay, "", "" });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINILineDisplay, clsUtilsSTRING.conUse, clsUtilsSTRING.conUseNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINILineDisplay, clsUtilsSTRING.conMethod, clsUtilsSTRING.conMethodNm01 });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINILineDisplay, clsUtilsSTRING.conLogicalName, clsUtilsSTRING.conLogicalNameNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINILineDisplay, clsUtilsSTRING.conPort, clsUtilsSTRING.conPortNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINILineDisplay, clsUtilsSTRING.conSpeed, clsUtilsSTRING.conSpeedNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINILineDisplay, clsUtilsSTRING.conDataBit, clsUtilsSTRING.conDataBitNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINILineDisplay, clsUtilsSTRING.conStopBit, clsUtilsSTRING.conStopBitNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINILineDisplay, clsUtilsSTRING.conParity, clsUtilsSTRING.conParityNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINILineDisplay, clsUtilsSTRING.conFlowControl, clsUtilsSTRING.conFlowControlNm });

                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINISignPad, "", "" });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINISignPad, clsUtilsSTRING.conUse, clsUtilsSTRING.conUseNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINISignPad, clsUtilsSTRING.conMethod, clsUtilsSTRING.conMethodNm01 });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINISignPad, clsUtilsSTRING.conLogicalName, clsUtilsSTRING.conLogicalNameNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINISignPad, clsUtilsSTRING.conPort, clsUtilsSTRING.conPortNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINISignPad, clsUtilsSTRING.conSpeed, clsUtilsSTRING.conSpeedNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINISignPad, clsUtilsSTRING.conDataBit, clsUtilsSTRING.conDataBitNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINISignPad, clsUtilsSTRING.conStopBit, clsUtilsSTRING.conStopBitNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINISignPad, clsUtilsSTRING.conParity, clsUtilsSTRING.conParityNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINISignPad, clsUtilsSTRING.conFlowControl, clsUtilsSTRING.conFlowControlNm });

                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIMSR, "", "" });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIMSR, clsUtilsSTRING.conUse, clsUtilsSTRING.conUseNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIMSR, clsUtilsSTRING.conMethod, clsUtilsSTRING.conMethodNm02 });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIMSR, clsUtilsSTRING.conLogicalName, clsUtilsSTRING.conLogicalNameNm });

                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINICashDrawer, "", "" });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINICashDrawer, clsUtilsSTRING.conUse, clsUtilsSTRING.conUseNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINICashDrawer, clsUtilsSTRING.conMethod, clsUtilsSTRING.conMethodNm02 });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINICashDrawer, clsUtilsSTRING.conLogicalName, clsUtilsSTRING.conLogicalNameNm });

                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPrinter, "", "" });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPrinter, clsUtilsSTRING.conUse, clsUtilsSTRING.conUseNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPrinter, clsUtilsSTRING.conMethod, clsUtilsSTRING.conMethodNm01 });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPrinter, clsUtilsSTRING.conLogicalName, clsUtilsSTRING.conLogicalNameNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPrinter, clsUtilsSTRING.conPort, clsUtilsSTRING.conPortNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPrinter, clsUtilsSTRING.conSpeed, clsUtilsSTRING.conSpeedNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPrinter, clsUtilsSTRING.conDataBit, clsUtilsSTRING.conDataBitNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPrinter, clsUtilsSTRING.conStopBit, clsUtilsSTRING.conStopBitNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPrinter, clsUtilsSTRING.conParity, clsUtilsSTRING.conParityNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPrinter, clsUtilsSTRING.conFlowControl, clsUtilsSTRING.conFlowControlNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPrinter, clsUtilsSTRING.conLogoBMP, clsUtilsSTRING.conLogoBMPNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPrinter, clsUtilsSTRING.conCutFeedCn, clsUtilsSTRING.conCutFeedCnNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPrinter, clsUtilsSTRING.conBarCodeWidth, clsUtilsSTRING.conBarCodeWidthNm });
                dtDevKey.Rows.Add(new object[] { clsUtilsSTRING.conINIPrinter, clsUtilsSTRING.conBarCodeHeight, clsUtilsSTRING.conBarCodeHeightNm });
                _dsVersion.Tables.Add(dtDevKey);

                //MstChanges.sql 데이터 테이블
                DataTable dtMst = new DataTable();
                dtMst.TableName = clsUtilsSTRING.conMstChanges;
                dtMst.Columns.Add("Value01");
                dtMst.Columns.Add("Value02");
                dtMst.Columns.Add("LocalPath");
                dtMst.Columns.Add("ChangeYN");
                dtMst.Rows.Add(new object[] { "", "", "", clsUtilsEnum.eChange.Normal.ToString() });
                _dsVersion.Tables.Add(dtMst);

                //TranChanges.sql 데이터 테이블
                DataTable dtTran = new DataTable();
                dtTran.TableName = clsUtilsSTRING.conTranChanges;
                dtTran.Columns.Add("Value01");
                dtTran.Columns.Add("Value02");
                dtTran.Columns.Add("LocalPath");
                dtTran.Columns.Add("ChangeYN");
                dtTran.Rows.Add(new object[] { "", "", "", clsUtilsEnum.eChange.Normal.ToString() });
                _dsVersion.Tables.Add(dtTran);

                //저장항목
                DataTable dtSave = new DataTable();
                dtSave.TableName = clsUtilsSTRING.conSaveList;

                //
                DataSet dsCD_STORE = clsUtilsOra.Instance.GetOra("select distinct cd_store from BSM010T where fg_use = '1' order by cd_store asc");
                if (dsCD_STORE != null && dsCD_STORE.Tables.Count > 0 && dsCD_STORE.Tables[0] != null)
                {
                    dsCD_STORE.Tables[0].TableName = clsUtilsSTRING.conCD_STORE;
                    _dsVersion.Tables.Add(dsCD_STORE.Tables[0]);
                }

                DataSet dsNO_POS = clsUtilsOra.Instance.GetOra("select a.cd_store, b.no_pos from BSM010T a inner join BSM040T b on a.cd_store = b.cd_store where a.cd_store = '1' group by a.cd_store, b.no_pos order by a.cd_store, b.no_pos asc");
                if (dsNO_POS != null && dsNO_POS.Tables.Count > 0 && dsNO_POS.Tables[0] != null)
                {
                    dsNO_POS.Tables[0].TableName = clsUtilsSTRING.conNO_POS;
                    _dsVersion.Tables.Add(dsNO_POS.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 데이터 테이블 row 추가
        /// </summary>
        /// <param name="TableNm">테이블명</param>
        /// <param name="list">추가 Row 값</param>
        /// <returns></returns>
        public bool AddRow(string TableNm, Dictionary<string, object> list)
        {
            bool bReturn = false;

            try
            {
                if (_dsVersion != null)
                {
                    foreach (DataTable dt in _dsVersion.Tables)
                    {
                        if (dt != null && dt.TableName.Equals(TableNm))
                        {
                            DataRow NewDr = dt.NewRow();

                            foreach (DataColumn dtCol in dt.Columns)
                            {
                                if (dtCol.ColumnName.Length > 0 && list.ContainsKey(dtCol.ColumnName))
                                {
                                    NewDr[dtCol] = list[dtCol.ColumnName];    
                                }
                                else
                                {
                                    NewDr[dtCol] = "";
                                }
                            }

                            dt.Rows.Add(NewDr);
                            bReturn = true;
                            break;
                        }
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
        /// 데이터 테이블 row 삭제
        /// </summary>
        /// <param name="TableNm">테이블명</param>
        /// <param name="dr">삭제 Row 값</param>
        /// <returns></returns>
        public bool DeleteRow(string TableNm, DataRow dr)
        {
            bool bReturn = false;

            try
            {
                if (_dsVersion != null)
                {
                    foreach (DataTable dt in _dsVersion.Tables)
                    {
                        if (dt != null && dt.TableName.Equals(TableNm))
                        {
                            dt.Rows.Remove(dr);
                            bReturn = true;
                            break;
                        }
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
        /// 데이터 테이블 row 삭제
        /// </summary>
        /// <param name="TableNm">테이블명</param>
        /// <param name="iRow">삭제 Row Index</param>
        /// <returns></returns>
        public bool DeleteRow(string TableNm, int iRow)
        {
            bool bReturn = false;

            try
            {
                if (_dsVersion != null)
                {
                    foreach (DataTable dt in _dsVersion.Tables)
                    {
                        if (dt != null && dt.TableName.Equals(TableNm) && dt.Rows.Count >= iRow)
                        {
                            dt.Rows.RemoveAt(iRow);
                            bReturn = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            return bReturn;
        }

        #endregion
    }
}
