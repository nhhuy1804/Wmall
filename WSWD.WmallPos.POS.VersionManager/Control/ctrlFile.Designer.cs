namespace WSWD.WmallPos.POS.VersionManager.Control
{
    partial class ctrlFile
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpFile = new System.Windows.Forms.TableLayoutPanel();
            this.grd = new System.Windows.Forms.DataGridView();
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
            this.tlpFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd)).BeginInit();
            this.pnlFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpFile
            // 
            this.tlpFile.ColumnCount = 1;
            this.tlpFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFile.Controls.Add(this.grd, 0, 0);
            this.tlpFile.Controls.Add(this.pnlFile, 0, 1);
            this.tlpFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFile.Location = new System.Drawing.Point(0, 0);
            this.tlpFile.Name = "tlpFile";
            this.tlpFile.RowCount = 2;
            this.tlpFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpFile.Size = new System.Drawing.Size(740, 598);
            this.tlpFile.TabIndex = 1;
            // 
            // grd
            // 
            this.grd.AllowDrop = true;
            this.grd.AllowUserToAddRows = false;
            this.grd.AllowUserToDeleteRows = false;
            this.grd.AllowUserToResizeColumns = false;
            this.grd.AllowUserToResizeRows = false;
            this.grd.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grd.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
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
            this.grd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd.Location = new System.Drawing.Point(3, 3);
            this.grd.Name = "grd";
            this.grd.RowHeadersVisible = false;
            this.grd.RowTemplate.Height = 23;
            this.grd.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grd.Size = new System.Drawing.Size(734, 542);
            this.grd.TabIndex = 1;
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
            this.pnlFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.pnlFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.pnlFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlFile.Controls.Add(this.btnFileDelete, 2, 0);
            this.pnlFile.Controls.Add(this.btnFileAdd, 1, 0);
            this.pnlFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFile.Location = new System.Drawing.Point(3, 551);
            this.pnlFile.Name = "pnlFile";
            this.pnlFile.RowCount = 1;
            this.pnlFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlFile.Size = new System.Drawing.Size(734, 44);
            this.pnlFile.TabIndex = 3;
            // 
            // btnFileDelete
            // 
            this.btnFileDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFileDelete.Location = new System.Drawing.Point(370, 3);
            this.btnFileDelete.Name = "btnFileDelete";
            this.btnFileDelete.Size = new System.Drawing.Size(114, 38);
            this.btnFileDelete.TabIndex = 2;
            this.btnFileDelete.Text = "파일삭제";
            this.btnFileDelete.UseVisualStyleBackColor = true;
            // 
            // btnFileAdd
            // 
            this.btnFileAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFileAdd.Location = new System.Drawing.Point(250, 3);
            this.btnFileAdd.Name = "btnFileAdd";
            this.btnFileAdd.Size = new System.Drawing.Size(114, 38);
            this.btnFileAdd.TabIndex = 0;
            this.btnFileAdd.Text = "파일신규";
            this.btnFileAdd.UseVisualStyleBackColor = true;
            // 
            // ctrlFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpFile);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ctrlFile";
            this.Size = new System.Drawing.Size(740, 598);
            this.tlpFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd)).EndInit();
            this.pnlFile.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpFile;
        private System.Windows.Forms.DataGridView grd;
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
        private System.Windows.Forms.TableLayoutPanel pnlFile;
        private System.Windows.Forms.Button btnFileDelete;
        private System.Windows.Forms.Button btnFileAdd;
    }
}
