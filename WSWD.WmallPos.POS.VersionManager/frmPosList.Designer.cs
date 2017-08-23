namespace WSWD.WmallPos.POS.VersionManager
{
    partial class frmPosList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grdPosNo = new System.Windows.Forms.DataGridView();
            this.colPosNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdPosNo)).BeginInit();
            this.SuspendLayout();
            // 
            // grdPosNo
            // 
            this.grdPosNo.AllowUserToAddRows = false;
            this.grdPosNo.AllowUserToDeleteRows = false;
            this.grdPosNo.AllowUserToResizeColumns = false;
            this.grdPosNo.AllowUserToResizeRows = false;
            this.grdPosNo.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdPosNo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.grdPosNo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdPosNo.ColumnHeadersVisible = false;
            this.grdPosNo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPosNo});
            this.grdPosNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdPosNo.Location = new System.Drawing.Point(0, 0);
            this.grdPosNo.Name = "grdPosNo";
            this.grdPosNo.ReadOnly = true;
            this.grdPosNo.RowHeadersVisible = false;
            this.grdPosNo.RowTemplate.Height = 23;
            this.grdPosNo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdPosNo.Size = new System.Drawing.Size(184, 262);
            this.grdPosNo.TabIndex = 0;
            // 
            // colPosNo
            // 
            this.colPosNo.DataPropertyName = "colPosNo";
            this.colPosNo.Frozen = true;
            this.colPosNo.HeaderText = "포스번호";
            this.colPosNo.Name = "colPosNo";
            this.colPosNo.ReadOnly = true;
            this.colPosNo.Width = 150;
            // 
            // frmPosList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 262);
            this.Controls.Add(this.grdPosNo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPosList";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "포스확인";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.grdPosNo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdPosNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPosNo;
    }
}