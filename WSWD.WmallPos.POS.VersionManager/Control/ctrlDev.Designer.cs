namespace WSWD.WmallPos.POS.VersionManager.Control
{
    partial class ctrlDev
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
            this.cboDevPos = new WSWD.WmallPos.POS.VersionManager.Control.ctrlCheckedComboBox();
            this.txtDevInput = new System.Windows.Forms.TextBox();
            this.cboDevStore = new System.Windows.Forms.ComboBox();
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
            this.colDevChangeYN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDevRealSection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpDev.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDev)).BeginInit();
            this.SuspendLayout();
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
            this.tlpDev.Location = new System.Drawing.Point(0, 0);
            this.tlpDev.Name = "tlpDev";
            this.tlpDev.RowCount = 4;
            this.tlpDev.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpDev.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpDev.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDev.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpDev.Size = new System.Drawing.Size(740, 598);
            this.tlpDev.TabIndex = 8;
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.ColumnCount = 3;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel12.Controls.Add(this.btnAddDevConfig, 1, 0);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(3, 63);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(734, 44);
            this.tableLayoutPanel12.TabIndex = 0;
            // 
            // btnAddDevConfig
            // 
            this.btnAddDevConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddDevConfig.Location = new System.Drawing.Point(295, 3);
            this.btnAddDevConfig.Name = "btnAddDevConfig";
            this.btnAddDevConfig.Size = new System.Drawing.Size(144, 38);
            this.btnAddDevConfig.TabIndex = 0;
            this.btnAddDevConfig.TabStop = false;
            this.btnAddDevConfig.Text = "등록";
            this.btnAddDevConfig.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.ColumnCount = 3;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.Controls.Add(this.btnDeleteDevConfig, 1, 0);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(3, 551);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(734, 44);
            this.tableLayoutPanel13.TabIndex = 1;
            // 
            // btnDeleteDevConfig
            // 
            this.btnDeleteDevConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeleteDevConfig.Location = new System.Drawing.Point(295, 3);
            this.btnDeleteDevConfig.Name = "btnDeleteDevConfig";
            this.btnDeleteDevConfig.Size = new System.Drawing.Size(144, 38);
            this.btnDeleteDevConfig.TabIndex = 0;
            this.btnDeleteDevConfig.TabStop = false;
            this.btnDeleteDevConfig.Text = "삭제";
            this.btnDeleteDevConfig.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.ColumnCount = 5;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel14.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel14.Controls.Add(this.label10, 1, 0);
            this.tableLayoutPanel14.Controls.Add(this.label11, 2, 0);
            this.tableLayoutPanel14.Controls.Add(this.label12, 3, 0);
            this.tableLayoutPanel14.Controls.Add(this.label13, 4, 0);
            this.tableLayoutPanel14.Controls.Add(this.cboDevSection, 0, 1);
            this.tableLayoutPanel14.Controls.Add(this.cboDevKey, 1, 1);
            this.tableLayoutPanel14.Controls.Add(this.cboDevPos, 4, 1);
            this.tableLayoutPanel14.Controls.Add(this.txtDevInput, 2, 1);
            this.tableLayoutPanel14.Controls.Add(this.cboDevStore, 3, 1);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 2;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(734, 54);
            this.tableLayoutPanel14.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 27);
            this.label9.TabIndex = 0;
            this.label9.Text = "분류선택";
            this.label9.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(113, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(194, 27);
            this.label10.TabIndex = 1;
            this.label10.Text = "항목선택";
            this.label10.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label11
            // 
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(313, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(168, 27);
            this.label11.TabIndex = 2;
            this.label11.Text = "설정값 입력";
            this.label11.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label12
            // 
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Location = new System.Drawing.Point(487, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(94, 27);
            this.label12.TabIndex = 3;
            this.label12.Text = "점포선택";
            this.label12.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label13
            // 
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(587, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(144, 27);
            this.label13.TabIndex = 4;
            this.label13.Text = "포스선택";
            this.label13.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // cboDevSection
            // 
            this.cboDevSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboDevSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevSection.FormattingEnabled = true;
            this.cboDevSection.Location = new System.Drawing.Point(3, 30);
            this.cboDevSection.Name = "cboDevSection";
            this.cboDevSection.Size = new System.Drawing.Size(104, 20);
            this.cboDevSection.TabIndex = 0;
            // 
            // cboDevKey
            // 
            this.cboDevKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboDevKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevKey.FormattingEnabled = true;
            this.cboDevKey.Location = new System.Drawing.Point(113, 30);
            this.cboDevKey.Name = "cboDevKey";
            this.cboDevKey.Size = new System.Drawing.Size(194, 20);
            this.cboDevKey.TabIndex = 1;
            // 
            // cboDevPos
            // 
            this.cboDevPos.CheckOnClick = true;
            this.cboDevPos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboDevPos.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboDevPos.DropDownHeight = 1;
            this.cboDevPos.FormattingEnabled = true;
            this.cboDevPos.IntegralHeight = false;
            this.cboDevPos.Location = new System.Drawing.Point(587, 30);
            this.cboDevPos.Name = "cboDevPos";
            this.cboDevPos.Size = new System.Drawing.Size(144, 22);
            this.cboDevPos.TabIndex = 4;
            this.cboDevPos.ValueSeparator = ", ";
            // 
            // txtDevInput
            // 
            this.txtDevInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDevInput.Location = new System.Drawing.Point(313, 30);
            this.txtDevInput.Name = "txtDevInput";
            this.txtDevInput.Size = new System.Drawing.Size(168, 21);
            this.txtDevInput.TabIndex = 2;
            // 
            // cboDevStore
            // 
            this.cboDevStore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboDevStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevStore.FormattingEnabled = true;
            this.cboDevStore.Location = new System.Drawing.Point(487, 30);
            this.cboDevStore.Name = "cboDevStore";
            this.cboDevStore.Size = new System.Drawing.Size(94, 20);
            this.cboDevStore.TabIndex = 3;
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
            this.colDevChangeYN,
            this.colDevRealSection});
            this.grdDev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDev.Location = new System.Drawing.Point(3, 113);
            this.grdDev.Name = "grdDev";
            this.grdDev.RowHeadersVisible = false;
            this.grdDev.RowTemplate.Height = 23;
            this.grdDev.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdDev.Size = new System.Drawing.Size(734, 432);
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
            this.colDevKeyNm.Width = 250;
            // 
            // colDevValue
            // 
            this.colDevValue.DataPropertyName = "colDevValue";
            this.colDevValue.Frozen = true;
            this.colDevValue.HeaderText = "값";
            this.colDevValue.Name = "colDevValue";
            this.colDevValue.ReadOnly = true;
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
            // ctrlDev
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpDev);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ctrlDev";
            this.Size = new System.Drawing.Size(740, 598);
            this.tlpDev.ResumeLayout(false);
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDev)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

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
        public ctrlCheckedComboBox cboDevPos;
        public System.Windows.Forms.TextBox txtDevInput;
        private System.Windows.Forms.ComboBox cboDevStore;
        private System.Windows.Forms.DataGridView grdDev;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevSection;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevSectionNm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevKeyNm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevStore;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevStoreNm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevPosNm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevChangeYN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevRealSection;

    }
}
