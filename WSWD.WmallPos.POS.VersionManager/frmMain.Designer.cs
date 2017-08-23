namespace WSWD.WmallPos.POS.VersionManager
{
    partial class frmMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tlpSelect = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSelect = new System.Windows.Forms.Panel();
            this.cboProgram = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupTree = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tv = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnTreeNew = new System.Windows.Forms.Button();
            this.groupList = new System.Windows.Forms.GroupBox();
            this.cTran = new WSWD.WmallPos.POS.VersionManager.Control.ctrlTran();
            this.cMst = new WSWD.WmallPos.POS.VersionManager.Control.ctrlMst();
            this.tlpFile = new System.Windows.Forms.TableLayoutPanel();
            this.grdFile = new System.Windows.Forms.DataGridView();
            this.DeleteYN = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.VersionNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServerDirectory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocalDirectory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DirectoryDepth01 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DirectoryDepth02 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DirectoryDepth03 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DirectoryDepth04 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DirectoryDepth05 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DirectoryDepth06 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileYN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChangeYN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateCreated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFile = new System.Windows.Forms.TableLayoutPanel();
            this.btnFileDelete = new System.Windows.Forms.Button();
            this.btnFileAdd = new System.Windows.Forms.Button();
            this.tlpDev = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddDevConfig = new System.Windows.Forms.Button();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.btnDeleteDevConfig = new System.Windows.Forms.Button();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cboDevSection = new System.Windows.Forms.ComboBox();
            this.cboDevKey = new System.Windows.Forms.ComboBox();
            this.txtDevInput = new System.Windows.Forms.TextBox();
            this.cboDevStore = new System.Windows.Forms.ComboBox();
            this.cboDevPos = new WSWD.WmallPos.POS.VersionManager.Control.ctrlCheckedComboBox();
            this.grdDev = new System.Windows.Forms.DataGridView();
            this.colDevSection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDevSectionNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDevKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDevKeyNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDevValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDevStore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDevStoreNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDevPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDevPosNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDevPosBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colDevChangeYN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDevRealSection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpApp = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddAppConfig = new System.Windows.Forms.Button();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.btnDeleteAppConfig = new System.Windows.Forms.Button();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cboAppSection = new System.Windows.Forms.ComboBox();
            this.cboAppKey = new System.Windows.Forms.ComboBox();
            this.cboAppPos = new WSWD.WmallPos.POS.VersionManager.Control.ctrlCheckedComboBox();
            this.txtAppInput = new System.Windows.Forms.TextBox();
            this.cboAppStore = new System.Windows.Forms.ComboBox();
            this.grdApp = new System.Windows.Forms.DataGridView();
            this.colAppSection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAppSectionNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAppKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAppKeyNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAppValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAppStore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAppStoreNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAppPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAppPosNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAppPosBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colAppChangeYN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAppRealSection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlShowStatus = new System.Windows.Forms.GroupBox();
            this.cStatus = new WSWD.WmallPos.POS.VersionManager.Control.ctrlStatus();
            this.cProgress = new WSWD.WmallPos.POS.VersionManager.Control.ctrlProgress();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.imgList = new System.Windows.Forms.ImageList();
            this.tableLayoutPanel1.SuspendLayout();
            this.tlpSelect.SuspendLayout();
            this.pnlSelect.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupTree.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupList.SuspendLayout();
            this.tlpFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFile)).BeginInit();
            this.pnlFile.SuspendLayout();
            this.tlpDev.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDev)).BeginInit();
            this.tlpApp.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdApp)).BeginInit();
            this.pnlShowStatus.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tlpSelect, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.pnlShowStatus, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(864, 733);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tlpSelect
            // 
            this.tlpSelect.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpSelect.ColumnCount = 5;
            this.tlpSelect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSelect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 103F));
            this.tlpSelect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 103F));
            this.tlpSelect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 129F));
            this.tlpSelect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 124F));
            this.tlpSelect.Controls.Add(this.pnlSelect, 0, 0);
            this.tlpSelect.Controls.Add(this.btnSelect, 1, 0);
            this.tlpSelect.Controls.Add(this.btnSave, 2, 0);
            this.tlpSelect.Controls.Add(this.btnDelete, 3, 0);
            this.tlpSelect.Controls.Add(this.btnDownload, 4, 0);
            this.tlpSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSelect.Location = new System.Drawing.Point(3, 3);
            this.tlpSelect.Name = "tlpSelect";
            this.tlpSelect.RowCount = 1;
            this.tlpSelect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSelect.Size = new System.Drawing.Size(858, 48);
            this.tlpSelect.TabIndex = 0;
            // 
            // pnlSelect
            // 
            this.pnlSelect.Controls.Add(this.cboProgram);
            this.pnlSelect.Controls.Add(this.label2);
            this.pnlSelect.Controls.Add(this.cboList);
            this.pnlSelect.Controls.Add(this.label1);
            this.pnlSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSelect.Location = new System.Drawing.Point(4, 4);
            this.pnlSelect.Name = "pnlSelect";
            this.pnlSelect.Size = new System.Drawing.Size(387, 40);
            this.pnlSelect.TabIndex = 2;
            // 
            // cboProgram
            // 
            this.cboProgram.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProgram.FormattingEnabled = true;
            this.cboProgram.Location = new System.Drawing.Point(67, 9);
            this.cboProgram.Name = "cboProgram";
            this.cboProgram.Size = new System.Drawing.Size(86, 21);
            this.cboProgram.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "프로그램";
            // 
            // cboList
            // 
            this.cboList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboList.FormattingEnabled = true;
            this.cboList.Location = new System.Drawing.Point(188, 9);
            this.cboList.Name = "cboList";
            this.cboList.Size = new System.Drawing.Size(138, 21);
            this.cboList.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(158, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "버전";
            // 
            // btnSelect
            // 
            this.btnSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelect.Location = new System.Drawing.Point(398, 4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(97, 40);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.TabStop = false;
            this.btnSelect.Text = "조회";
            this.btnSelect.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Location = new System.Drawing.Point(502, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(97, 40);
            this.btnSave.TabIndex = 1;
            this.btnSave.TabStop = false;
            this.btnSave.Text = "서버적용";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.Location = new System.Drawing.Point(606, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(123, 40);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.TabStop = false;
            this.btnDelete.Text = "업그레이드 정보삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnDownload
            // 
            this.btnDownload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDownload.Location = new System.Drawing.Point(736, 4);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(118, 40);
            this.btnDownload.TabIndex = 1;
            this.btnDownload.TabStop = false;
            this.btnDownload.Text = "다운로드";
            this.btnDownload.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 214F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.groupTree, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.groupList, 1, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 127);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(858, 603);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // groupTree
            // 
            this.groupTree.Controls.Add(this.tableLayoutPanel5);
            this.groupTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupTree.Location = new System.Drawing.Point(3, 3);
            this.groupTree.Name = "groupTree";
            this.groupTree.Size = new System.Drawing.Size(208, 597);
            this.groupTree.TabIndex = 0;
            this.groupTree.TabStop = false;
            this.groupTree.Text = "업그레이드 목록";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.tv, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(202, 578);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // tv
            // 
            this.tv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tv.Location = new System.Drawing.Point(3, 3);
            this.tv.Name = "tv";
            this.tv.Size = new System.Drawing.Size(196, 518);
            this.tv.TabIndex = 3;
            this.tv.TabStop = false;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 103F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel4.Controls.Add(this.btnTreeNew, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 527);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(196, 48);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // btnTreeNew
            // 
            this.btnTreeNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTreeNew.Location = new System.Drawing.Point(49, 3);
            this.btnTreeNew.Name = "btnTreeNew";
            this.btnTreeNew.Size = new System.Drawing.Size(97, 42);
            this.btnTreeNew.TabIndex = 0;
            this.btnTreeNew.TabStop = false;
            this.btnTreeNew.Text = "신규";
            this.btnTreeNew.UseVisualStyleBackColor = true;
            // 
            // groupList
            // 
            this.groupList.BackColor = System.Drawing.SystemColors.Control;
            this.groupList.Controls.Add(this.cTran);
            this.groupList.Controls.Add(this.cMst);
            this.groupList.Controls.Add(this.tlpFile);
            this.groupList.Controls.Add(this.tlpDev);
            this.groupList.Controls.Add(this.tlpApp);
            this.groupList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupList.Location = new System.Drawing.Point(217, 3);
            this.groupList.Name = "groupList";
            this.groupList.Size = new System.Drawing.Size(638, 597);
            this.groupList.TabIndex = 1;
            this.groupList.TabStop = false;
            this.groupList.Text = "업그레이드 상세 목록";
            // 
            // cTran
            // 
            this.cTran.Location = new System.Drawing.Point(441, 25);
            this.cTran.Margin = new System.Windows.Forms.Padding(0);
            this.cTran.Name = "cTran";
            this.cTran.Size = new System.Drawing.Size(166, 197);
            this.cTran.TabIndex = 9;
            // 
            // cMst
            // 
            this.cMst.Location = new System.Drawing.Point(261, 25);
            this.cMst.Margin = new System.Windows.Forms.Padding(0);
            this.cMst.Name = "cMst";
            this.cMst.Size = new System.Drawing.Size(162, 194);
            this.cMst.TabIndex = 8;
            // 
            // tlpFile
            // 
            this.tlpFile.ColumnCount = 1;
            this.tlpFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFile.Controls.Add(this.grdFile, 0, 0);
            this.tlpFile.Controls.Add(this.pnlFile, 0, 1);
            this.tlpFile.Location = new System.Drawing.Point(9, 22);
            this.tlpFile.Name = "tlpFile";
            this.tlpFile.RowCount = 2;
            this.tlpFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tlpFile.Size = new System.Drawing.Size(243, 204);
            this.tlpFile.TabIndex = 0;
            // 
            // grdFile
            // 
            this.grdFile.AllowDrop = true;
            this.grdFile.AllowUserToAddRows = false;
            this.grdFile.AllowUserToDeleteRows = false;
            this.grdFile.AllowUserToResizeColumns = false;
            this.grdFile.AllowUserToResizeRows = false;
            this.grdFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdFile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DeleteYN,
            this.VersionNm,
            this.ServerDirectory,
            this.LocalDirectory,
            this.DirectoryDepth01,
            this.DirectoryDepth02,
            this.DirectoryDepth03,
            this.DirectoryDepth04,
            this.DirectoryDepth05,
            this.DirectoryDepth06,
            this.FileNm,
            this.FileYN,
            this.ChangeYN,
            this.DateCreated,
            this.FileSize});
            this.grdFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdFile.Location = new System.Drawing.Point(3, 3);
            this.grdFile.Name = "grdFile";
            this.grdFile.RowHeadersVisible = false;
            this.grdFile.RowTemplate.Height = 23;
            this.grdFile.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdFile.Size = new System.Drawing.Size(237, 144);
            this.grdFile.TabIndex = 1;
            // 
            // DeleteYN
            // 
            this.DeleteYN.DataPropertyName = "DeleteYN";
            this.DeleteYN.FalseValue = "N";
            this.DeleteYN.Frozen = true;
            this.DeleteYN.HeaderText = "삭제";
            this.DeleteYN.Name = "DeleteYN";
            this.DeleteYN.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DeleteYN.TrueValue = "Y";
            this.DeleteYN.Width = 50;
            // 
            // VersionNm
            // 
            this.VersionNm.DataPropertyName = "VersionNm";
            this.VersionNm.Frozen = true;
            this.VersionNm.HeaderText = "업그레이드일자";
            this.VersionNm.Name = "VersionNm";
            this.VersionNm.ReadOnly = true;
            this.VersionNm.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.VersionNm.Visible = false;
            this.VersionNm.Width = 300;
            // 
            // ServerDirectory
            // 
            this.ServerDirectory.DataPropertyName = "ServerDirectory";
            this.ServerDirectory.Frozen = true;
            this.ServerDirectory.HeaderText = "서버경로";
            this.ServerDirectory.Name = "ServerDirectory";
            this.ServerDirectory.ReadOnly = true;
            this.ServerDirectory.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ServerDirectory.Visible = false;
            // 
            // LocalDirectory
            // 
            this.LocalDirectory.DataPropertyName = "LocalDirectory";
            this.LocalDirectory.Frozen = true;
            this.LocalDirectory.HeaderText = "로컬경로";
            this.LocalDirectory.Name = "LocalDirectory";
            this.LocalDirectory.ReadOnly = true;
            this.LocalDirectory.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LocalDirectory.Visible = false;
            // 
            // DirectoryDepth01
            // 
            this.DirectoryDepth01.DataPropertyName = "DirectoryDepth01";
            this.DirectoryDepth01.Frozen = true;
            this.DirectoryDepth01.HeaderText = "경로1";
            this.DirectoryDepth01.Name = "DirectoryDepth01";
            this.DirectoryDepth01.ReadOnly = true;
            this.DirectoryDepth01.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DirectoryDepth01.Visible = false;
            // 
            // DirectoryDepth02
            // 
            this.DirectoryDepth02.DataPropertyName = "DirectoryDepth02";
            this.DirectoryDepth02.Frozen = true;
            this.DirectoryDepth02.HeaderText = "경로2";
            this.DirectoryDepth02.Name = "DirectoryDepth02";
            this.DirectoryDepth02.ReadOnly = true;
            this.DirectoryDepth02.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DirectoryDepth02.Visible = false;
            // 
            // DirectoryDepth03
            // 
            this.DirectoryDepth03.DataPropertyName = "DirectoryDepth03";
            this.DirectoryDepth03.Frozen = true;
            this.DirectoryDepth03.HeaderText = "경로3";
            this.DirectoryDepth03.Name = "DirectoryDepth03";
            this.DirectoryDepth03.ReadOnly = true;
            this.DirectoryDepth03.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DirectoryDepth03.Visible = false;
            // 
            // DirectoryDepth04
            // 
            this.DirectoryDepth04.DataPropertyName = "DirectoryDepth04";
            this.DirectoryDepth04.Frozen = true;
            this.DirectoryDepth04.HeaderText = "경로4";
            this.DirectoryDepth04.Name = "DirectoryDepth04";
            this.DirectoryDepth04.ReadOnly = true;
            this.DirectoryDepth04.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DirectoryDepth04.Visible = false;
            // 
            // DirectoryDepth05
            // 
            this.DirectoryDepth05.DataPropertyName = "DirectoryDepth05";
            this.DirectoryDepth05.Frozen = true;
            this.DirectoryDepth05.HeaderText = "경로5";
            this.DirectoryDepth05.Name = "DirectoryDepth05";
            this.DirectoryDepth05.ReadOnly = true;
            this.DirectoryDepth05.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DirectoryDepth05.Visible = false;
            // 
            // DirectoryDepth06
            // 
            this.DirectoryDepth06.DataPropertyName = "DirectoryDepth06";
            this.DirectoryDepth06.Frozen = true;
            this.DirectoryDepth06.HeaderText = "경로6";
            this.DirectoryDepth06.Name = "DirectoryDepth06";
            this.DirectoryDepth06.ReadOnly = true;
            this.DirectoryDepth06.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DirectoryDepth06.Visible = false;
            // 
            // FileNm
            // 
            this.FileNm.DataPropertyName = "FileNm";
            this.FileNm.Frozen = true;
            this.FileNm.HeaderText = "이름";
            this.FileNm.Name = "FileNm";
            this.FileNm.ReadOnly = true;
            this.FileNm.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.FileNm.Width = 400;
            // 
            // FileYN
            // 
            this.FileYN.DataPropertyName = "FileYN";
            this.FileYN.Frozen = true;
            this.FileYN.HeaderText = "파일여부";
            this.FileYN.Name = "FileYN";
            this.FileYN.ReadOnly = true;
            this.FileYN.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.FileYN.Visible = false;
            // 
            // ChangeYN
            // 
            this.ChangeYN.DataPropertyName = "ChangeYN";
            this.ChangeYN.Frozen = true;
            this.ChangeYN.HeaderText = "변경여부";
            this.ChangeYN.Name = "ChangeYN";
            this.ChangeYN.ReadOnly = true;
            this.ChangeYN.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ChangeYN.Visible = false;
            // 
            // DateCreated
            // 
            this.DateCreated.DataPropertyName = "DateCreated";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DateCreated.DefaultCellStyle = dataGridViewCellStyle1;
            this.DateCreated.Frozen = true;
            this.DateCreated.HeaderText = "수정한 날짜";
            this.DateCreated.Name = "DateCreated";
            this.DateCreated.ReadOnly = true;
            this.DateCreated.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DateCreated.Width = 160;
            // 
            // FileSize
            // 
            this.FileSize.DataPropertyName = "FileSize";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.FileSize.DefaultCellStyle = dataGridViewCellStyle2;
            this.FileSize.Frozen = true;
            this.FileSize.HeaderText = "크기";
            this.FileSize.Name = "FileSize";
            this.FileSize.ReadOnly = true;
            this.FileSize.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // pnlFile
            // 
            this.pnlFile.ColumnCount = 4;
            this.pnlFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 103F));
            this.pnlFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 103F));
            this.pnlFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlFile.Controls.Add(this.btnFileDelete, 2, 0);
            this.pnlFile.Controls.Add(this.btnFileAdd, 1, 0);
            this.pnlFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFile.Location = new System.Drawing.Point(3, 153);
            this.pnlFile.Name = "pnlFile";
            this.pnlFile.RowCount = 1;
            this.pnlFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlFile.Size = new System.Drawing.Size(237, 48);
            this.pnlFile.TabIndex = 3;
            // 
            // btnFileDelete
            // 
            this.btnFileDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFileDelete.Location = new System.Drawing.Point(121, 3);
            this.btnFileDelete.Name = "btnFileDelete";
            this.btnFileDelete.Size = new System.Drawing.Size(97, 42);
            this.btnFileDelete.TabIndex = 2;
            this.btnFileDelete.TabStop = false;
            this.btnFileDelete.Text = "파일삭제";
            this.btnFileDelete.UseVisualStyleBackColor = true;
            // 
            // btnFileAdd
            // 
            this.btnFileAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFileAdd.Location = new System.Drawing.Point(18, 3);
            this.btnFileAdd.Name = "btnFileAdd";
            this.btnFileAdd.Size = new System.Drawing.Size(97, 42);
            this.btnFileAdd.TabIndex = 0;
            this.btnFileAdd.TabStop = false;
            this.btnFileAdd.Text = "파일신규";
            this.btnFileAdd.UseVisualStyleBackColor = true;
            // 
            // tlpDev
            // 
            this.tlpDev.ColumnCount = 1;
            this.tlpDev.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDev.Controls.Add(this.tableLayoutPanel12, 0, 1);
            this.tlpDev.Controls.Add(this.tableLayoutPanel13, 0, 3);
            this.tlpDev.Controls.Add(this.tableLayoutPanel14, 0, 0);
            this.tlpDev.Controls.Add(this.grdDev, 0, 2);
            this.tlpDev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDev.Location = new System.Drawing.Point(3, 16);
            this.tlpDev.Name = "tlpDev";
            this.tlpDev.RowCount = 4;
            this.tlpDev.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tlpDev.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tlpDev.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDev.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tlpDev.Size = new System.Drawing.Size(632, 578);
            this.tlpDev.TabIndex = 7;
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.ColumnCount = 3;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 129F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel12.Controls.Add(this.btnAddDevConfig, 1, 0);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(3, 68);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(626, 48);
            this.tableLayoutPanel12.TabIndex = 0;
            // 
            // btnAddDevConfig
            // 
            this.btnAddDevConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddDevConfig.Location = new System.Drawing.Point(251, 3);
            this.btnAddDevConfig.Name = "btnAddDevConfig";
            this.btnAddDevConfig.Size = new System.Drawing.Size(123, 42);
            this.btnAddDevConfig.TabIndex = 0;
            this.btnAddDevConfig.TabStop = false;
            this.btnAddDevConfig.Text = "등록";
            this.btnAddDevConfig.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.ColumnCount = 3;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 129F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.Controls.Add(this.btnDeleteDevConfig, 1, 0);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(3, 527);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(626, 48);
            this.tableLayoutPanel13.TabIndex = 1;
            // 
            // btnDeleteDevConfig
            // 
            this.btnDeleteDevConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeleteDevConfig.Location = new System.Drawing.Point(251, 3);
            this.btnDeleteDevConfig.Name = "btnDeleteDevConfig";
            this.btnDeleteDevConfig.Size = new System.Drawing.Size(123, 42);
            this.btnDeleteDevConfig.TabIndex = 0;
            this.btnDeleteDevConfig.TabStop = false;
            this.btnDeleteDevConfig.Text = "삭제";
            this.btnDeleteDevConfig.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.ColumnCount = 5;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 129F));
            this.tableLayoutPanel14.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel14.Controls.Add(this.label10, 1, 0);
            this.tableLayoutPanel14.Controls.Add(this.label11, 2, 0);
            this.tableLayoutPanel14.Controls.Add(this.label12, 3, 0);
            this.tableLayoutPanel14.Controls.Add(this.label13, 4, 0);
            this.tableLayoutPanel14.Controls.Add(this.cboDevSection, 0, 1);
            this.tableLayoutPanel14.Controls.Add(this.cboDevKey, 1, 1);
            this.tableLayoutPanel14.Controls.Add(this.txtDevInput, 2, 1);
            this.tableLayoutPanel14.Controls.Add(this.cboDevStore, 3, 1);
            this.tableLayoutPanel14.Controls.Add(this.cboDevPos, 4, 1);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 2;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(626, 59);
            this.tableLayoutPanel14.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 29);
            this.label9.TabIndex = 0;
            this.label9.Text = "분류선택";
            this.label9.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(97, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(165, 29);
            this.label10.TabIndex = 1;
            this.label10.Text = "항목선택";
            this.label10.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label11
            // 
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(268, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(140, 29);
            this.label11.TabIndex = 2;
            this.label11.Text = "설정값 입력";
            this.label11.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label12
            // 
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Location = new System.Drawing.Point(414, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 29);
            this.label12.TabIndex = 3;
            this.label12.Text = "점포선택";
            this.label12.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label13
            // 
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(500, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(123, 29);
            this.label13.TabIndex = 4;
            this.label13.Text = "포스선택";
            this.label13.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // cboDevSection
            // 
            this.cboDevSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboDevSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevSection.FormattingEnabled = true;
            this.cboDevSection.Location = new System.Drawing.Point(3, 32);
            this.cboDevSection.Name = "cboDevSection";
            this.cboDevSection.Size = new System.Drawing.Size(88, 21);
            this.cboDevSection.TabIndex = 0;
            // 
            // cboDevKey
            // 
            this.cboDevKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboDevKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevKey.FormattingEnabled = true;
            this.cboDevKey.Location = new System.Drawing.Point(97, 32);
            this.cboDevKey.Name = "cboDevKey";
            this.cboDevKey.Size = new System.Drawing.Size(165, 21);
            this.cboDevKey.TabIndex = 1;
            // 
            // txtDevInput
            // 
            this.txtDevInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDevInput.Location = new System.Drawing.Point(268, 32);
            this.txtDevInput.Name = "txtDevInput";
            this.txtDevInput.Size = new System.Drawing.Size(140, 20);
            this.txtDevInput.TabIndex = 2;
            // 
            // cboDevStore
            // 
            this.cboDevStore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboDevStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevStore.FormattingEnabled = true;
            this.cboDevStore.Location = new System.Drawing.Point(414, 32);
            this.cboDevStore.Name = "cboDevStore";
            this.cboDevStore.Size = new System.Drawing.Size(80, 21);
            this.cboDevStore.TabIndex = 3;
            // 
            // cboDevPos
            // 
            this.cboDevPos.CheckOnClick = true;
            this.cboDevPos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboDevPos.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboDevPos.DropDownHeight = 1;
            this.cboDevPos.FormattingEnabled = true;
            this.cboDevPos.IntegralHeight = false;
            this.cboDevPos.Location = new System.Drawing.Point(500, 32);
            this.cboDevPos.Name = "cboDevPos";
            this.cboDevPos.Size = new System.Drawing.Size(123, 21);
            this.cboDevPos.TabIndex = 4;
            this.cboDevPos.ValueSeparator = ", ";
            // 
            // grdDev
            // 
            this.grdDev.AllowUserToAddRows = false;
            this.grdDev.AllowUserToDeleteRows = false;
            this.grdDev.AllowUserToResizeColumns = false;
            this.grdDev.AllowUserToResizeRows = false;
            this.grdDev.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdDev.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDevSection,
            this.colDevSectionNm,
            this.colDevKey,
            this.colDevKeyNm,
            this.colDevValue,
            this.colDevStore,
            this.colDevStoreNm,
            this.colDevPos,
            this.colDevPosNm,
            this.colDevPosBtn,
            this.colDevChangeYN,
            this.colDevRealSection});
            this.grdDev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDev.Location = new System.Drawing.Point(3, 122);
            this.grdDev.Name = "grdDev";
            this.grdDev.RowHeadersVisible = false;
            this.grdDev.RowTemplate.Height = 23;
            this.grdDev.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdDev.Size = new System.Drawing.Size(626, 399);
            this.grdDev.TabIndex = 3;
            this.grdDev.TabStop = false;
            // 
            // colDevSection
            // 
            this.colDevSection.DataPropertyName = "colDevSection";
            this.colDevSection.Frozen = true;
            this.colDevSection.HeaderText = "분류";
            this.colDevSection.Name = "colDevSection";
            this.colDevSection.ReadOnly = true;
            this.colDevSection.Visible = false;
            this.colDevSection.Width = 5;
            // 
            // colDevSectionNm
            // 
            this.colDevSectionNm.DataPropertyName = "colDevSectionNm";
            this.colDevSectionNm.Frozen = true;
            this.colDevSectionNm.HeaderText = "분류";
            this.colDevSectionNm.Name = "colDevSectionNm";
            this.colDevSectionNm.ReadOnly = true;
            // 
            // colDevKey
            // 
            this.colDevKey.DataPropertyName = "colDevKey";
            this.colDevKey.Frozen = true;
            this.colDevKey.HeaderText = "항목";
            this.colDevKey.Name = "colDevKey";
            this.colDevKey.ReadOnly = true;
            this.colDevKey.Visible = false;
            // 
            // colDevKeyNm
            // 
            this.colDevKeyNm.DataPropertyName = "colDevKeyNm";
            this.colDevKeyNm.Frozen = true;
            this.colDevKeyNm.HeaderText = "항목";
            this.colDevKeyNm.Name = "colDevKeyNm";
            this.colDevKeyNm.ReadOnly = true;
            this.colDevKeyNm.Width = 194;
            // 
            // colDevValue
            // 
            this.colDevValue.DataPropertyName = "colDevValue";
            this.colDevValue.Frozen = true;
            this.colDevValue.HeaderText = "값";
            this.colDevValue.Name = "colDevValue";
            this.colDevValue.ReadOnly = true;
            this.colDevValue.Width = 168;
            // 
            // colDevStore
            // 
            this.colDevStore.DataPropertyName = "colDevStore";
            this.colDevStore.Frozen = true;
            this.colDevStore.HeaderText = "점포";
            this.colDevStore.Name = "colDevStore";
            this.colDevStore.ReadOnly = true;
            this.colDevStore.Visible = false;
            // 
            // colDevStoreNm
            // 
            this.colDevStoreNm.DataPropertyName = "colDevStoreNm";
            this.colDevStoreNm.Frozen = true;
            this.colDevStoreNm.HeaderText = "점포";
            this.colDevStoreNm.Name = "colDevStoreNm";
            this.colDevStoreNm.ReadOnly = true;
            // 
            // colDevPos
            // 
            this.colDevPos.DataPropertyName = "colDevPos";
            this.colDevPos.Frozen = true;
            this.colDevPos.HeaderText = "포스";
            this.colDevPos.Name = "colDevPos";
            this.colDevPos.ReadOnly = true;
            this.colDevPos.Visible = false;
            // 
            // colDevPosNm
            // 
            this.colDevPosNm.DataPropertyName = "colDevPosNm";
            this.colDevPosNm.Frozen = true;
            this.colDevPosNm.HeaderText = "포스";
            this.colDevPosNm.Name = "colDevPosNm";
            this.colDevPosNm.ReadOnly = true;
            // 
            // colDevPosBtn
            // 
            this.colDevPosBtn.DataPropertyName = "colDevPosBtn";
            this.colDevPosBtn.Frozen = true;
            this.colDevPosBtn.HeaderText = "";
            this.colDevPosBtn.Name = "colDevPosBtn";
            this.colDevPosBtn.Text = "...";
            this.colDevPosBtn.UseColumnTextForButtonValue = true;
            this.colDevPosBtn.Width = 30;
            // 
            // colDevChangeYN
            // 
            this.colDevChangeYN.DataPropertyName = "colDevChangeYN";
            this.colDevChangeYN.Frozen = true;
            this.colDevChangeYN.HeaderText = "변경여부";
            this.colDevChangeYN.Name = "colDevChangeYN";
            this.colDevChangeYN.ReadOnly = true;
            this.colDevChangeYN.Visible = false;
            // 
            // colDevRealSection
            // 
            this.colDevRealSection.DataPropertyName = "colDevRealSection";
            this.colDevRealSection.Frozen = true;
            this.colDevRealSection.HeaderText = "Column1";
            this.colDevRealSection.Name = "colDevRealSection";
            this.colDevRealSection.ReadOnly = true;
            this.colDevRealSection.Visible = false;
            // 
            // tlpApp
            // 
            this.tlpApp.ColumnCount = 1;
            this.tlpApp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpApp.Controls.Add(this.tableLayoutPanel9, 0, 1);
            this.tlpApp.Controls.Add(this.tableLayoutPanel10, 0, 3);
            this.tlpApp.Controls.Add(this.tableLayoutPanel11, 0, 0);
            this.tlpApp.Controls.Add(this.grdApp, 0, 2);
            this.tlpApp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpApp.Location = new System.Drawing.Point(3, 16);
            this.tlpApp.Name = "tlpApp";
            this.tlpApp.RowCount = 4;
            this.tlpApp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tlpApp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tlpApp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpApp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tlpApp.Size = new System.Drawing.Size(632, 578);
            this.tlpApp.TabIndex = 6;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 3;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 129F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.Controls.Add(this.btnAddAppConfig, 1, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 68);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(626, 48);
            this.tableLayoutPanel9.TabIndex = 0;
            // 
            // btnAddAppConfig
            // 
            this.btnAddAppConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddAppConfig.Location = new System.Drawing.Point(251, 3);
            this.btnAddAppConfig.Name = "btnAddAppConfig";
            this.btnAddAppConfig.Size = new System.Drawing.Size(123, 42);
            this.btnAddAppConfig.TabIndex = 0;
            this.btnAddAppConfig.TabStop = false;
            this.btnAddAppConfig.Text = "등록";
            this.btnAddAppConfig.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 3;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 129F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.Controls.Add(this.btnDeleteAppConfig, 1, 0);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 527);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(626, 48);
            this.tableLayoutPanel10.TabIndex = 1;
            // 
            // btnDeleteAppConfig
            // 
            this.btnDeleteAppConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeleteAppConfig.Location = new System.Drawing.Point(251, 3);
            this.btnDeleteAppConfig.Name = "btnDeleteAppConfig";
            this.btnDeleteAppConfig.Size = new System.Drawing.Size(123, 42);
            this.btnDeleteAppConfig.TabIndex = 0;
            this.btnDeleteAppConfig.TabStop = false;
            this.btnDeleteAppConfig.Text = "삭제";
            this.btnDeleteAppConfig.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.ColumnCount = 5;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 129F));
            this.tableLayoutPanel11.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.label5, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.label6, 2, 0);
            this.tableLayoutPanel11.Controls.Add(this.label7, 3, 0);
            this.tableLayoutPanel11.Controls.Add(this.label8, 4, 0);
            this.tableLayoutPanel11.Controls.Add(this.cboAppSection, 0, 1);
            this.tableLayoutPanel11.Controls.Add(this.cboAppKey, 1, 1);
            this.tableLayoutPanel11.Controls.Add(this.cboAppPos, 4, 1);
            this.tableLayoutPanel11.Controls.Add(this.txtAppInput, 2, 1);
            this.tableLayoutPanel11.Controls.Add(this.cboAppStore, 3, 1);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 2;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(626, 59);
            this.tableLayoutPanel11.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 29);
            this.label4.TabIndex = 0;
            this.label4.Text = "분류선택";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(97, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(165, 29);
            this.label5.TabIndex = 1;
            this.label5.Text = "항목선택";
            this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(268, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(140, 29);
            this.label6.TabIndex = 2;
            this.label6.Text = "설정값 입력";
            this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(414, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 29);
            this.label7.TabIndex = 3;
            this.label7.Text = "점포선택";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(500, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(123, 29);
            this.label8.TabIndex = 4;
            this.label8.Text = "포스선택";
            this.label8.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // cboAppSection
            // 
            this.cboAppSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboAppSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAppSection.FormattingEnabled = true;
            this.cboAppSection.Location = new System.Drawing.Point(3, 32);
            this.cboAppSection.Name = "cboAppSection";
            this.cboAppSection.Size = new System.Drawing.Size(88, 21);
            this.cboAppSection.TabIndex = 0;
            // 
            // cboAppKey
            // 
            this.cboAppKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboAppKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAppKey.FormattingEnabled = true;
            this.cboAppKey.Location = new System.Drawing.Point(97, 32);
            this.cboAppKey.Name = "cboAppKey";
            this.cboAppKey.Size = new System.Drawing.Size(165, 21);
            this.cboAppKey.TabIndex = 1;
            // 
            // cboAppPos
            // 
            this.cboAppPos.CheckOnClick = true;
            this.cboAppPos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboAppPos.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboAppPos.DropDownHeight = 1;
            this.cboAppPos.FormattingEnabled = true;
            this.cboAppPos.IntegralHeight = false;
            this.cboAppPos.Location = new System.Drawing.Point(500, 32);
            this.cboAppPos.Name = "cboAppPos";
            this.cboAppPos.Size = new System.Drawing.Size(123, 21);
            this.cboAppPos.TabIndex = 4;
            this.cboAppPos.ValueSeparator = ", ";
            // 
            // txtAppInput
            // 
            this.txtAppInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAppInput.Location = new System.Drawing.Point(268, 32);
            this.txtAppInput.Name = "txtAppInput";
            this.txtAppInput.Size = new System.Drawing.Size(140, 20);
            this.txtAppInput.TabIndex = 2;
            // 
            // cboAppStore
            // 
            this.cboAppStore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboAppStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAppStore.FormattingEnabled = true;
            this.cboAppStore.Location = new System.Drawing.Point(414, 32);
            this.cboAppStore.Name = "cboAppStore";
            this.cboAppStore.Size = new System.Drawing.Size(80, 21);
            this.cboAppStore.TabIndex = 3;
            // 
            // grdApp
            // 
            this.grdApp.AllowUserToAddRows = false;
            this.grdApp.AllowUserToDeleteRows = false;
            this.grdApp.AllowUserToResizeColumns = false;
            this.grdApp.AllowUserToResizeRows = false;
            this.grdApp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdApp.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAppSection,
            this.colAppSectionNm,
            this.colAppKey,
            this.colAppKeyNm,
            this.colAppValue,
            this.colAppStore,
            this.colAppStoreNm,
            this.colAppPos,
            this.colAppPosNm,
            this.colAppPosBtn,
            this.colAppChangeYN,
            this.colAppRealSection});
            this.grdApp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdApp.Location = new System.Drawing.Point(3, 122);
            this.grdApp.Name = "grdApp";
            this.grdApp.RowHeadersVisible = false;
            this.grdApp.RowTemplate.Height = 23;
            this.grdApp.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdApp.Size = new System.Drawing.Size(626, 399);
            this.grdApp.TabIndex = 3;
            // 
            // colAppSection
            // 
            this.colAppSection.DataPropertyName = "colAppSection";
            this.colAppSection.Frozen = true;
            this.colAppSection.HeaderText = "Column1";
            this.colAppSection.Name = "colAppSection";
            this.colAppSection.ReadOnly = true;
            this.colAppSection.Visible = false;
            // 
            // colAppSectionNm
            // 
            this.colAppSectionNm.DataPropertyName = "colAppSectionNm";
            this.colAppSectionNm.Frozen = true;
            this.colAppSectionNm.HeaderText = "분류";
            this.colAppSectionNm.Name = "colAppSectionNm";
            this.colAppSectionNm.ReadOnly = true;
            // 
            // colAppKey
            // 
            this.colAppKey.DataPropertyName = "colAppKey";
            this.colAppKey.Frozen = true;
            this.colAppKey.HeaderText = "Column1";
            this.colAppKey.Name = "colAppKey";
            this.colAppKey.ReadOnly = true;
            this.colAppKey.Visible = false;
            // 
            // colAppKeyNm
            // 
            this.colAppKeyNm.DataPropertyName = "colAppKeyNm";
            this.colAppKeyNm.Frozen = true;
            this.colAppKeyNm.HeaderText = "항목";
            this.colAppKeyNm.Name = "colAppKeyNm";
            this.colAppKeyNm.ReadOnly = true;
            this.colAppKeyNm.Width = 194;
            // 
            // colAppValue
            // 
            this.colAppValue.DataPropertyName = "colAppValue";
            this.colAppValue.Frozen = true;
            this.colAppValue.HeaderText = "설정값";
            this.colAppValue.Name = "colAppValue";
            this.colAppValue.ReadOnly = true;
            this.colAppValue.Width = 168;
            // 
            // colAppStore
            // 
            this.colAppStore.DataPropertyName = "colAppStore";
            this.colAppStore.Frozen = true;
            this.colAppStore.HeaderText = "Column1";
            this.colAppStore.Name = "colAppStore";
            this.colAppStore.ReadOnly = true;
            this.colAppStore.Visible = false;
            // 
            // colAppStoreNm
            // 
            this.colAppStoreNm.DataPropertyName = "colAppStoreNm";
            this.colAppStoreNm.Frozen = true;
            this.colAppStoreNm.HeaderText = "점포";
            this.colAppStoreNm.Name = "colAppStoreNm";
            this.colAppStoreNm.ReadOnly = true;
            // 
            // colAppPos
            // 
            this.colAppPos.DataPropertyName = "colAppPos";
            this.colAppPos.Frozen = true;
            this.colAppPos.HeaderText = "Column1";
            this.colAppPos.Name = "colAppPos";
            this.colAppPos.ReadOnly = true;
            this.colAppPos.Visible = false;
            // 
            // colAppPosNm
            // 
            this.colAppPosNm.DataPropertyName = "colAppPosNm";
            this.colAppPosNm.Frozen = true;
            this.colAppPosNm.HeaderText = "포스";
            this.colAppPosNm.Name = "colAppPosNm";
            this.colAppPosNm.ReadOnly = true;
            // 
            // colAppPosBtn
            // 
            this.colAppPosBtn.DataPropertyName = "colAppPosBtn";
            this.colAppPosBtn.Frozen = true;
            this.colAppPosBtn.HeaderText = "";
            this.colAppPosBtn.Name = "colAppPosBtn";
            this.colAppPosBtn.ReadOnly = true;
            this.colAppPosBtn.Text = "...";
            this.colAppPosBtn.UseColumnTextForButtonValue = true;
            this.colAppPosBtn.Width = 30;
            // 
            // colAppChangeYN
            // 
            this.colAppChangeYN.DataPropertyName = "colAppChangeYN";
            this.colAppChangeYN.Frozen = true;
            this.colAppChangeYN.HeaderText = "Column1";
            this.colAppChangeYN.Name = "colAppChangeYN";
            this.colAppChangeYN.ReadOnly = true;
            this.colAppChangeYN.Visible = false;
            // 
            // colAppRealSection
            // 
            this.colAppRealSection.DataPropertyName = "colAppRealSection";
            this.colAppRealSection.Frozen = true;
            this.colAppRealSection.HeaderText = "Column1";
            this.colAppRealSection.Name = "colAppRealSection";
            this.colAppRealSection.ReadOnly = true;
            this.colAppRealSection.Visible = false;
            // 
            // pnlShowStatus
            // 
            this.pnlShowStatus.Controls.Add(this.cStatus);
            this.pnlShowStatus.Controls.Add(this.cProgress);
            this.pnlShowStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlShowStatus.Location = new System.Drawing.Point(3, 57);
            this.pnlShowStatus.Name = "pnlShowStatus";
            this.pnlShowStatus.Size = new System.Drawing.Size(858, 64);
            this.pnlShowStatus.TabIndex = 4;
            this.pnlShowStatus.TabStop = false;
            this.pnlShowStatus.Text = "현황";
            // 
            // cStatus
            // 
            this.cStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cStatus.Location = new System.Drawing.Point(3, 16);
            this.cStatus.Margin = new System.Windows.Forms.Padding(0);
            this.cStatus.Name = "cStatus";
            this.cStatus.Size = new System.Drawing.Size(852, 45);
            this.cStatus.TabIndex = 1;
            // 
            // cProgress
            // 
            this.cProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cProgress.Location = new System.Drawing.Point(3, 16);
            this.cProgress.Margin = new System.Windows.Forms.Padding(0);
            this.cProgress.Name = "cProgress";
            this.cProgress.Size = new System.Drawing.Size(852, 45);
            this.cProgress.TabIndex = 0;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 4;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.Controls.Add(this.button7, 0, 0);
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel7.TabIndex = 0;
            // 
            // button7
            // 
            this.button7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button7.Location = new System.Drawing.Point(3, 3);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(44, 94);
            this.button7.TabIndex = 0;
            this.button7.Text = "신규";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button8.Location = new System.Drawing.Point(123, 3);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(114, 536);
            this.button8.TabIndex = 1;
            this.button8.Text = "수정";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 4;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel8.Controls.Add(this.button9, 0, 0);
            this.tableLayoutPanel8.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel8.TabIndex = 0;
            // 
            // button9
            // 
            this.button9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button9.Location = new System.Drawing.Point(3, 3);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(44, 94);
            this.button9.TabIndex = 0;
            this.button9.Text = "신규";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            this.button10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button10.Location = new System.Drawing.Point(53, 3);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(44, 38);
            this.button10.TabIndex = 1;
            this.button10.Text = "수정";
            this.button10.UseVisualStyleBackColor = true;
            // 
            // imgList
            // 
            this.imgList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imgList.ImageSize = new System.Drawing.Size(16, 16);
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // frmMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 733);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "버전 관리 프로그램";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tlpSelect.ResumeLayout(false);
            this.pnlSelect.ResumeLayout(false);
            this.pnlSelect.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupTree.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.groupList.ResumeLayout(false);
            this.tlpFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdFile)).EndInit();
            this.pnlFile.ResumeLayout(false);
            this.tlpDev.ResumeLayout(false);
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDev)).EndInit();
            this.tlpApp.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdApp)).EndInit();
            this.pnlShowStatus.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tlpSelect;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Panel pnlSelect;
        private System.Windows.Forms.ComboBox cboList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnTreeNew;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupTree;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TreeView tv;
        private System.Windows.Forms.GroupBox groupList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.ComboBox cboProgram;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView grdFile;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridViewCheckBoxColumn DeleteYN;
        private System.Windows.Forms.DataGridViewTextBoxColumn VersionNm;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServerDirectory;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocalDirectory;
        private System.Windows.Forms.DataGridViewTextBoxColumn DirectoryDepth01;
        private System.Windows.Forms.DataGridViewTextBoxColumn DirectoryDepth02;
        private System.Windows.Forms.DataGridViewTextBoxColumn DirectoryDepth03;
        private System.Windows.Forms.DataGridViewTextBoxColumn DirectoryDepth04;
        private System.Windows.Forms.DataGridViewTextBoxColumn DirectoryDepth05;
        private System.Windows.Forms.DataGridViewTextBoxColumn DirectoryDepth06;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileNm;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileYN;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChangeYN;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateCreated;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileSize;
        private System.Windows.Forms.GroupBox pnlShowStatus;
        private System.Windows.Forms.TableLayoutPanel pnlFile;
        private System.Windows.Forms.Button btnFileDelete;
        private System.Windows.Forms.Button btnFileAdd;
        private System.Windows.Forms.TableLayoutPanel tlpFile;
        private System.Windows.Forms.TableLayoutPanel tlpApp;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        public System.Windows.Forms.Button btnAddAppConfig;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        public System.Windows.Forms.Button btnDeleteAppConfig;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.ComboBox cboAppSection;
        public System.Windows.Forms.ComboBox cboAppKey;
        public WSWD.WmallPos.POS.VersionManager.Control.ctrlCheckedComboBox cboAppPos;
        public System.Windows.Forms.TextBox txtAppInput;
        private System.Windows.Forms.TableLayoutPanel tlpDev;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private System.Windows.Forms.Button btnAddDevConfig;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.Button btnDeleteDevConfig;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        public System.Windows.Forms.ComboBox cboDevSection;
        public System.Windows.Forms.ComboBox cboDevKey;
        public WSWD.WmallPos.POS.VersionManager.Control.ctrlCheckedComboBox cboDevPos;
        public System.Windows.Forms.TextBox txtDevInput;
        private System.Windows.Forms.DataGridView grdDev;
        private System.Windows.Forms.ComboBox cboAppStore;
        private System.Windows.Forms.ComboBox cboDevStore;
        private WSWD.WmallPos.POS.VersionManager.Control.ctrlProgress cProgress;
        private WSWD.WmallPos.POS.VersionManager.Control.ctrlStatus cStatus;
        private WSWD.WmallPos.POS.VersionManager.Control.ctrlMst cMst;
        private WSWD.WmallPos.POS.VersionManager.Control.ctrlTran cTran;
        private System.Windows.Forms.DataGridView grdApp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAppSection;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAppSectionNm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAppKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAppKeyNm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAppValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAppStore;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAppStoreNm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAppPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAppPosNm;
        private System.Windows.Forms.DataGridViewButtonColumn colAppPosBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAppChangeYN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAppRealSection;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevSection;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevSectionNm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevKeyNm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevStore;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevStoreNm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevPosNm;
        private System.Windows.Forms.DataGridViewButtonColumn colDevPosBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevChangeYN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevRealSection;
    }
}