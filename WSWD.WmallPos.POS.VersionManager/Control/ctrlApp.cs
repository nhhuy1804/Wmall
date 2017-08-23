using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSWD.WmallPos.POS.VersionManager.Utils;

namespace WSWD.WmallPos.POS.VersionManager.Control
{
    public partial class ctrlApp : UserControl
    {
        private DataTable _dtApp;
        public DataTable dtApp
        {
            get { return this._dtApp; }
            set { this._dtApp = value; }
        }

        public ctrlApp()
        {
            InitializeComponent();

            //InitControl();

            //InitEvent();
        }

        //private void InitControl()
        //{
        //    try
        //    {
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //    }
        //}

        //private void InitEvent()
        //{
        //    try
        //    {
        //        btnAddAppConfig.Click += new EventHandler(btnConfigAdd_Click);
        //        btnDeleteAppConfig.Click += new EventHandler(btnConfigDelete_Click);
        //        cboAppSection.SelectedIndexChanged += new EventHandler(cboConfigSection_SelectedIndexChanged);
        //        cboAppKey.SelectedIndexChanged += new EventHandler(cboConfigKey_SelectedIndexChanged);
        //        cboAppStore.SelectedIndexChanged += new EventHandler(cboConfigStore_SelectedIndexChanged);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //    }
        //}

        //public void SetControl()
        //{
        //    try
        //    {
        //        if (cboAppSection == null || cboAppSection.Items.Count <= 0)
        //        {
        //            DataTable dtSection = new DataTable();
        //            dtSection.Columns.Add("Id");
        //            dtSection.Columns.Add("Name");

        //            dtSection.Rows.Add(new object[] { "", "" });
        //            dtSection.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm01, clsUtilsSTRING.conINIPosComm01Nm });
        //            dtSection.Rows.Add(new object[] { clsUtilsSTRING.conINIPosComm02, clsUtilsSTRING.conINIPosComm02Nm });
        //            dtSection.Rows.Add(new object[] { clsUtilsSTRING.conINIPosFTP, clsUtilsSTRING.conINIPosFTPNm });
        //            dtSection.Rows.Add(new object[] { clsUtilsSTRING.conINIPosVan, clsUtilsSTRING.conINIPosVanNm });
        //            dtSection.Rows.Add(new object[] { clsUtilsSTRING.conINIPosOption, clsUtilsSTRING.conINIPosOptionNm });

        //            cboAppSection.DataSource = dtSection;
        //            cboAppSection.DisplayMember = "Name";
        //            cboAppSection.ValueMember = "Id";
        //        }
        //        cboAppSection.SelectedIndex = 0;
        //        cboConfigSection_SelectedIndexChanged(cboAppSection, null);

        //        grdApp.DataSource = clsUtilsData.Instance.dtApp;
        //        clsUtilsData.Instance.dtApp.DefaultView.RowFilter = string.Format("colAppChangeYN <> '{1}'", clsUtilsEnum.eChange.Delete.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //    }
        //}

        ///// <summary>
        ///// AppConfig.ini 및 DevConfig.ini 등록 버튼 클릭 이벤트
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void btnConfigAdd_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Button btn = (Button)sender;
        //        ComboBox cboTempSection = null;
        //        ComboBox cboTempKey = null;
        //        TextBox txtTemp = null;
        //        ComboBox cboTempStore = null;
        //        ctrlCheckedComboBox cboTempPos = null;
        //        DataTable dtTemp = null;

        //        string strSection = string.Empty;
        //        string strKey = string.Empty;
        //        string strValue = string.Empty;
        //        string strStore = string.Empty;
        //        string strPos = string.Empty;
        //        string strSelect = string.Empty;
        //        string strConfig = string.Empty;
        //        string strServerDirectory = string.Empty;
        //        string strLocalDirectory = string.Empty;

        //        TreeNode node = tv.SelectedNode;
        //        TreeNode ParentNode = ParentNodes(node);
        //        string strTempNm = node.Name;

        //        if (btn.Name.ToString() == btnAddAppConfig.Name.ToString())
        //        {
        //            cboTempSection = cboAppSection;
        //            cboTempKey = cboAppKey;
        //            txtTemp = txtAppInput;
        //            cboTempStore = cboAppStore;
        //            cboTempPos = cboAppPos;
        //            dtTemp = clsUtilsData.Instance.dtApp;
        //            strConfig = clsUtilsSTRING.conAppConfig;
        //        }
        //        else
        //        {
        //            cboTempSection = cboDevSection;
        //            cboTempKey = cboDevKey;
        //            txtTemp = txtDevInput;
        //            cboTempStore = cboDevStore;
        //            cboTempPos = cboDevPos;
        //            dtTemp = clsUtilsData.Instance.dtDev;
        //            strConfig = clsUtilsSTRING.conDevConfig;
        //        }

        //        if (cboTempSection == null || cboTempSection.Items.Count <= 0 || cboTempSection.SelectedValue == null || cboTempSection.SelectedValue.ToString().Length <= 0 ||
        //            cboTempKey == null || cboTempKey.Items.Count <= 0 || cboTempKey.SelectedValue == null || cboTempKey.SelectedValue.ToString().Length <= 0 ||
        //            txtTemp.Text.Length <= 0 ||
        //            cboTempStore == null || cboTempStore.Text.Length <= 0 ||
        //            cboTempPos == null || cboTempPos.Text.Length <= 0) return;

        //        strSection = cboTempSection.SelectedValue.ToString();
        //        strKey = cboTempKey.SelectedValue.ToString();
        //        strValue = txtTemp.Text;
        //        strStore = cboTempStore.Text;
        //        strPos = cboTempPos.Text;


        //        if (btn.Name.ToString() == btnAddAppConfig.Name.ToString())
        //        {
        //            strSection = strSection == clsUtilsSTRING.conINIPosComm01 || strSection == clsUtilsSTRING.conINIPosComm02 ? clsUtilsSTRING.conINIPosComm : strSection;
        //            strSelect = string.Format("colAppRealSection = '{0}' and colAppKey = '{1}' and colAppValue = '{2}' and colAppStore = '{3}' and colAppPos = '{4}' and colAppChangeYN <> '{5}'",
        //            strSection, strKey, strValue, strStore, strPos, clsUtilsEnum.eChange.Delete.ToString());
        //        }
        //        else
        //        {
        //            strSelect = string.Format("colDevRealSection = '{0}' and colDevKey = '{1}' and colDevValue = '{2}' and colDevStore = '{3}' and colDevPos = '{4}' and colDevChangeYN <> '{5}'",
        //            strSection, strKey, strValue, strStore, strPos, clsUtilsEnum.eChange.Delete.ToString());
        //        }

        //        DataRow[] drFilter = dtTemp.Select(strSelect);

        //        if (drFilter != null && drFilter.Length > 0)
        //        {
        //            MessageBox.Show("존재");
        //            return;
        //        }
        //        ConfigAddrow(dtTemp, strSection, strKey, strValue, strStore, strPos);

        //        string strTempLocalPath = string.Format("{0}/{1}{2}/{3}/{4}", Application.StartupPath, _strLocalRoot, _strServerRoot, _strProgram, ParentNode.Name.ToString());

        //        drFilter = clsUtilsData.Instance.dtUpList.Select(
        //            string.Format("FileNm = '{0}'", strConfig));

        //        if (drFilter != null && drFilter.Length > 0)
        //        {
        //            drFilter[0]["ChangeYN"] = clsUtilsEnum.eChange.Modify.ToString();
        //        }
        //        else
        //        {
        //            UpgradeListAddRow(clsUtilsSTRING.conN, ParentNode.Name.ToString(), clsUtilsEnum.eChange.New, clsUtilsSTRING.conY, node.Tag.ToString(), strConfig, strTempLocalPath, "0", DateTime.Now);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //    }
        //}

        ///// <summary>
        ///// AppConfig.ini 및 DevConfig.ini 삭제 버튼 클릭 이벤트
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void btnConfigDelete_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Button btn = (Button)sender;
        //        DataTable dtTemp = null;
        //        DataGridView grdTemp = null;
        //        string strFileNm = string.Empty;
        //        string strChangeYN = string.Empty;

        //        if (btn.Name.ToString() == btnDeleteAppConfig.Name.ToString())
        //        {
        //            dtTemp = clsUtilsData.Instance.dtApp;
        //            grdTemp = grdApp;
        //            strChangeYN = "colAppChangeYN";
        //        }
        //        else
        //        {
        //            dtTemp = clsUtilsData.Instance.dtDev;
        //            grdTemp = grdDev;
        //            strChangeYN = "colDevChangeYN";
        //        }

        //        if (dtTemp == null || dtTemp.Rows.Count <= 0 || grdTemp.SelectedRows == null || grdTemp.SelectedRows.Count <= 0) return;

        //        if (grdTemp.SelectedRows[0].Cells[strChangeYN].Value.ToString() == clsUtilsEnum.eChange.New.ToString())
        //        {
        //            grdTemp.Rows.Remove(grdTemp.SelectedRows[0]);

        //            DataRow[] drFilter = dtTemp.Select(string.Format("{0} <> '{1}'", strChangeYN, clsUtilsEnum.eChange.Normal.ToString()));

        //            if (drFilter.Length <= 0)
        //            {
        //                if (dtTemp.Rows.Count <= 0)
        //                {
        //                    drFilter = clsUtilsData.Instance.dtUpList.Select(
        //                        string.Format("FileNm = '{0}'", btn.Name.ToString() == btnDeleteAppConfig.Name.ToString() ? clsUtilsSTRING.conAppConfig : clsUtilsSTRING.conDevConfig));

        //                    if (drFilter != null && drFilter.Length > 0)
        //                    {
        //                        clsUtilsData.Instance.dtUpList.Rows.Remove(drFilter[0]);
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            grdTemp.SelectedRows[0].Cells[strChangeYN].Value = clsUtilsEnum.eChange.Delete.ToString();

        //            DataRow[] drFilter = dtTemp.Select(string.Format("{0} <> '{1}'", strChangeYN, clsUtilsEnum.eChange.Delete.ToString()));

        //            if (drFilter.Length <= 0)
        //            {
        //                drFilter = clsUtilsData.Instance.dtUpList.Select(
        //                        string.Format("FileNm = '{0}'", btn.Name.ToString() == btnDeleteAppConfig.Name.ToString() ? clsUtilsSTRING.conAppConfig : clsUtilsSTRING.conDevConfig));

        //                if (drFilter != null && drFilter.Length > 0)
        //                {
        //                    drFilter[0]["ChangeYN"] = clsUtilsEnum.eChange.Delete.ToString();
        //                }
        //            }
        //            else
        //            {
        //                drFilter = clsUtilsData.Instance.dtUpList.Select(
        //                        string.Format("FileNm = '{0}'", btn.Name.ToString() == btnDeleteAppConfig.Name.ToString() ? clsUtilsSTRING.conAppConfig : clsUtilsSTRING.conDevConfig));

        //                if (drFilter != null && drFilter.Length > 0)
        //                {
        //                    drFilter[0]["ChangeYN"] = clsUtilsEnum.eChange.Modify.ToString();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //    }
        //}

        ///// <summary>
        ///// AppConfig.ini 및 DevConfig.ini 분류 콤보박스 변경 이벤트
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void cboConfigSection_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ComboBox cbo = (ComboBox)sender;

        //        DataTable dtKey = new DataTable();
        //        dtKey.Columns.Add("Id");
        //        dtKey.Columns.Add("Name");
        //        dtKey.Rows.Add(new object[] { "", "" });

        //        if (cbo.SelectedValue != null && cbo.SelectedValue.ToString().Length > 0)
        //        {
        //            switch (cbo.SelectedValue.ToString())
        //            {
        //                case clsUtilsSTRING.conINIPosComm01:
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommSvrIP1, clsUtilsSTRING.conPosCommSvrIP1Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComDPort1, clsUtilsSTRING.conPosCommComDPort1Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComUPort1, clsUtilsSTRING.conPosCommComUPort1Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComQPort1, clsUtilsSTRING.conPosCommComQPort1Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommSvrIP2, clsUtilsSTRING.conPosCommSvrIP2Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComDPort2, clsUtilsSTRING.conPosCommComDPort2Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComUPort2, clsUtilsSTRING.conPosCommComUPort2Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComQPort2, clsUtilsSTRING.conPosCommComQPort2Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComDTimeOut, clsUtilsSTRING.conPosCommComDTimeOutNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComUTimeOut, clsUtilsSTRING.conPosCommComUTimeOutNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComQTimeOut, clsUtilsSTRING.conPosCommComQTimeOutNm });
        //                    break;
        //                case clsUtilsSTRING.conINIPosComm02:
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommSvrGftIP1, clsUtilsSTRING.conPosCommSvrGftIP1Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComGPort1, clsUtilsSTRING.conPosCommComGPort1Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommSvrGftIP2, clsUtilsSTRING.conPosCommSvrGftIP2Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComGPort2, clsUtilsSTRING.conPosCommComGPort2Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComGTimeOut, clsUtilsSTRING.conPosCommComGTimeOutNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommSvrPntIP1, clsUtilsSTRING.conPosCommSvrPntIP1Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComPPort1, clsUtilsSTRING.conPosCommComPPort1Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommSvrPntIP2, clsUtilsSTRING.conPosCommSvrPntIP2Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComPPort2, clsUtilsSTRING.conPosCommComPPort2Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommComPTimeOut, clsUtilsSTRING.conPosCommComPTimeOutNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommHqSvrIP1, clsUtilsSTRING.conPosCommHqSvrIP1Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommHqComQPort1, clsUtilsSTRING.conPosCommHqComQPort1Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommHqComUPort1, clsUtilsSTRING.conPosCommHqComUPort1Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommHqSvrIP2, clsUtilsSTRING.conPosCommHqSvrIP2Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommHqComQPort2, clsUtilsSTRING.conPosCommHqComQPort2Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommHqComUPort2, clsUtilsSTRING.conPosCommHqComUPort2Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommHqComQTimeOut, clsUtilsSTRING.conPosCommHqComQTimeOutNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosCommHqComUTimeOut, clsUtilsSTRING.conPosCommHqComUTimeOutNm });

        //                    break;
        //                case clsUtilsSTRING.conINIPosFTP:
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPFtpSvrIP1, clsUtilsSTRING.conPosFTPFtpSvrIP1Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPFtpComPort1, clsUtilsSTRING.conPosFTPFtpComPort1Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPFtpSvrIP2, clsUtilsSTRING.conPosFTPFtpSvrIP2Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPFtpComPort2, clsUtilsSTRING.conPosFTPFtpComPort2Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPMode, clsUtilsSTRING.conPosFTPModeNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPUser, clsUtilsSTRING.conPosFTPUserNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPPass, clsUtilsSTRING.conPosFTPPassNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPJournalPath, clsUtilsSTRING.conPosFTPJournalPathNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPVersionInfoPath, clsUtilsSTRING.conPosFTPVersionInfoPathNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPDataFileDownloadPath, clsUtilsSTRING.conPosFTPDataFileDownloadPathNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosFTPCreateUploadPathByDate, clsUtilsSTRING.conPosFTPCreateUploadPathByDateNm });
        //                    break;
        //                case clsUtilsSTRING.conINIPosVan:
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosVANVanSvrIP1, clsUtilsSTRING.conPosVANVanSvrIP1Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosVANVanComPort1, clsUtilsSTRING.conPosVANVanComPort1Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosVANVanSvrIP2, clsUtilsSTRING.conPosVANVanSvrIP2Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosVANVanComPort2, clsUtilsSTRING.conPosVANVanComPort2Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosVANComTimeOut, clsUtilsSTRING.conPosVANComTimeOutNm });
        //                    break;
        //                case clsUtilsSTRING.conINIPosOption:
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionPointUse, clsUtilsSTRING.conPosOptionPointUseNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionPointSchemePrefix, clsUtilsSTRING.conPosOptionPointSchemePrefixNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionPointPayKeyInputEnable, clsUtilsSTRING.conPosOptionPointPayKeyInputEnableNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionCashReceiptUse, clsUtilsSTRING.conPosOptionCashReceiptUseNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionCashReceiptIssue, clsUtilsSTRING.conPosOptionCashReceiptIssueNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionCashReceiptApplAmount, clsUtilsSTRING.conPosOptionCashReceiptApplAmountNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionGoodsCodePrefix1, clsUtilsSTRING.conPosOptionGoodsCodePrefix1Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionGoodsCodePrefix2, clsUtilsSTRING.conPosOptionGoodsCodePrefix2Nm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionDataKeepDays, clsUtilsSTRING.conPosOptionDataKeepDaysNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionAutoReturnCardRead, clsUtilsSTRING.conPosOptionAutoReturnCardReadNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionSalesReturn, clsUtilsSTRING.conPosOptionSalesReturnNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionSignUploadTask, clsUtilsSTRING.conPosOptionSignUploadTaskNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionTransUploadTask, clsUtilsSTRING.conPosOptionTransUploadTaskNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionTransStatusTask, clsUtilsSTRING.conPosOptionTransStatusTaskNm });
        //                    dtKey.Rows.Add(new object[] { clsUtilsSTRING.conPosOptionNoticeStatusTask, clsUtilsSTRING.conPosOptionNoticeStatusTaskNm });
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }

        //        if (cboAppKey != null && cboAppKey.DataSource != null)
        //        {
        //            cboAppKey.DataSource = null;
        //        }

        //        cboAppKey.DataSource = dtKey;
        //        cboAppKey.DisplayMember = "Name";
        //        cboAppKey.ValueMember = "Id";
        //        cboAppKey.SelectedIndex = 0;
        //        cboConfigKey_SelectedIndexChanged(cboAppKey, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //    }
        //}

        ///// <summary>
        ///// AppConfig.ini 및 DevConfig.ini 항목 콤보박스 변경 이벤트
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void cboConfigKey_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ComboBox cbo = (ComboBox)sender;
        //        txtAppInput.Text = "";

        //        if (cboAppStore == null || cboAppStore.Items.Count <= 0)
        //        {
        //            InitControlConfig(cboAppStore);
        //        }

        //        cboAppStore.SelectedIndex = 0;
        //        cboConfigStore_SelectedIndexChanged(cboAppStore, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void cboConfigStore_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ComboBox cbo = (ComboBox)sender;
        //        InitControlConfig(cboAppPos);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //    }
        //}

        ///// <summary>
        ///// 점포 및 포스 체크콤보박스 셋팅
        ///// </summary>
        ///// <param name="cbo">점포 및 포스 체크콤보박스</param>
        //void InitControlConfig(ComboBox cbo)
        //{
        //    try
        //    {
        //        if (cbo == null || cbo.Items.Count <= 0)
        //        {
        //            DataSet ds = clsUtilsOra.Instance.GetOra("select distinct CD_STORE from BSM010T where fg_use = '1' order by CD_STORE ASC");

        //            DataTable dt = new DataTable();
        //            dt.Columns.Add("Id");
        //            dt.Columns.Add("Name");

        //            dt.Rows.Add(new object[] { "", "" });

        //            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
        //            {
        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    dt.Rows.Add(new object[] { "*", "전체" });
        //                }

        //                foreach (DataRow dr in ds.Tables[0].Rows)
        //                {
        //                    dt.Rows.Add(new object[] { dr[0].ToString(), dr[0].ToString() });
        //                }
        //            }

        //            cbo.DataSource = dt;
        //            cbo.ValueMember = "Id";
        //            cbo.DisplayMember = "Name";
        //        }
        //        else
        //        {
        //            cbo.SelectedIndex = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //    }
        //}

        //void InitControlConfig(ctrlCheckedComboBox cbo)
        //{
        //    try
        //    {
        //        cbo.MaxDropDownItems = 5;
        //        cbo.DisplayMember = "Name";
        //        cbo.ValueSeparator = ",";
        //        cbo.Items.Clear();
        //        cbo.Text = "";

        //        ComboBox cboTempStore = cboAppStore;

        //        if (cboTempStore.SelectedValue != null && cboTempStore.SelectedValue.ToString().Length > 0)
        //        {
        //            if (cboTempStore.SelectedValue.ToString() == "*")
        //            {
        //                ctrlCCBoxItem cItem = new ctrlCCBoxItem("전체", 0);
        //                cbo.Items.Add(cItem);
        //            }
        //            else
        //            {
        //                DataSet ds = clsUtilsOra.Instance.GetOra(string.Format("select distinct NO_POS from BSM040T where cd_store in ({0}) order by NO_POS ASC", cboTempStore.SelectedValue.ToString()));

        //                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
        //                {
        //                    int iRow = 0;
        //                    foreach (DataRow dr in ds.Tables[0].Rows)
        //                    {
        //                        ctrlCCBoxItem cItem = new ctrlCCBoxItem(dr[0].ToString(), iRow);
        //                        cbo.Items.Add(cItem);
        //                        iRow++;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //    }
        //}
    }
}
