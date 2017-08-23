namespace WSWD.WmallPos.POS.VersionManager.Control
{
    partial class ctrlApp
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
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
            this.colAppChangeYN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAppRealSection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpApp.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdApp)).BeginInit();
            this.SuspendLayout();
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
            this.tlpApp.Location = new System.Drawing.Point(0, 0);
            this.tlpApp.Name = "tlpApp";
            this.tlpApp.RowCount = 4;
            this.tlpApp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tlpApp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tlpApp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpApp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tlpApp.Size = new System.Drawing.Size(634, 648);
            this.tlpApp.TabIndex = 7;
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
            this.tableLayoutPanel9.Size = new System.Drawing.Size(628, 48);
            this.tableLayoutPanel9.TabIndex = 0;
            // 
            // btnAddAppConfig
            // 
            this.btnAddAppConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddAppConfig.Location = new System.Drawing.Point(252, 3);
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
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 597);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(628, 48);
            this.tableLayoutPanel10.TabIndex = 1;
            // 
            // btnDeleteAppConfig
            // 
            this.btnDeleteAppConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeleteAppConfig.Location = new System.Drawing.Point(252, 3);
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
            this.tableLayoutPanel11.Size = new System.Drawing.Size(628, 59);
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
            this.label6.Size = new System.Drawing.Size(142, 29);
            this.label6.TabIndex = 2;
            this.label6.Text = "설정값 입력";
            this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(416, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 29);
            this.label7.TabIndex = 3;
            this.label7.Text = "점포선택";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(502, 0);
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
            this.cboAppPos.Location = new System.Drawing.Point(502, 32);
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
            this.txtAppInput.Size = new System.Drawing.Size(142, 20);
            this.txtAppInput.TabIndex = 2;
            // 
            // cboAppStore
            // 
            this.cboAppStore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboAppStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAppStore.FormattingEnabled = true;
            this.cboAppStore.Location = new System.Drawing.Point(416, 32);
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
            this.colAppChangeYN,
            this.colAppRealSection});
            this.grdApp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdApp.Location = new System.Drawing.Point(3, 122);
            this.grdApp.Name = "grdApp";
            this.grdApp.RowHeadersVisible = false;
            this.grdApp.RowTemplate.Height = 23;
            this.grdApp.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdApp.Size = new System.Drawing.Size(628, 469);
            this.grdApp.TabIndex = 3;
            this.grdApp.TabStop = false;
            // 
            // colAppSection
            // 
            this.colAppSection.DataPropertyName = "colAppSection";
            this.colAppSection.Frozen = true;
            this.colAppSection.HeaderText = "Column1";
            this.colAppSection.Name = "colAppSection";
            this.colAppSection.ReadOnly = true;
            this.colAppSection.Visible = false;
            this.colAppSection.Width = 5;
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
            this.colAppKey.HeaderText = "항목";
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
            this.colAppKeyNm.Width = 250;
            // 
            // colAppValue
            // 
            this.colAppValue.DataPropertyName = "colAppValue";
            this.colAppValue.Frozen = true;
            this.colAppValue.HeaderText = "값";
            this.colAppValue.Name = "colAppValue";
            this.colAppValue.ReadOnly = true;
            // 
            // colAppStore
            // 
            this.colAppStore.DataPropertyName = "colAppStore";
            this.colAppStore.Frozen = true;
            this.colAppStore.HeaderText = "점포";
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
            this.colAppPos.HeaderText = "포스";
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
            // colAppChangeYN
            // 
            this.colAppChangeYN.DataPropertyName = "colAppChangeYN";
            this.colAppChangeYN.Frozen = true;
            this.colAppChangeYN.HeaderText = "변경여부";
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
            // ctrlApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpApp);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ctrlApp";
            this.Size = new System.Drawing.Size(634, 648);
            this.tlpApp.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdApp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

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
        public ctrlCheckedComboBox cboAppPos;
        public System.Windows.Forms.TextBox txtAppInput;
        private System.Windows.Forms.ComboBox cboAppStore;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colAppChangeYN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAppRealSection;

    }
}
