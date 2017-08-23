namespace WSWD.WmallPos.POS.PT.VC
{
    partial class POS_PT_P003
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
            this.roundedButton2 = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.grd = new WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnSave = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.ContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.btnClose);
            this.ContainerPanel.Controls.Add(this.btnSave);
            this.ContainerPanel.Controls.Add(this.grd);
            this.ContainerPanel.Size = new System.Drawing.Size(381, 328);
            // 
            // roundedButton2
            // 
            this.roundedButton2.BorderSize = 1;
            this.roundedButton2.Corner = 3;
            this.roundedButton2.Dock = System.Windows.Forms.DockStyle.Right;
            this.roundedButton2.Image = null;
            this.roundedButton2.IsHighlight = false;
            this.roundedButton2.Location = new System.Drawing.Point(99, 0);
            this.roundedButton2.Name = "roundedButton2";
            this.roundedButton2.Selected = false;
            this.roundedButton2.Size = new System.Drawing.Size(90, 42);
            this.roundedButton2.TabIndex = 5;
            this.roundedButton2.Text = "닫기";
            this.roundedButton2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grd
            // 
            this.grd.AutoFillRows = false;
            this.grd.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.grd.BorderWidth = new System.Windows.Forms.Padding(1);
            this.grd.ColumnCount = 0;
            this.grd.Location = new System.Drawing.Point(17, 17);
            this.grd.Name = "grd";
            this.grd.Padding = new System.Windows.Forms.Padding(1);
            this.grd.PageIndex = -1;
            this.grd.RowCount = 6;
            this.grd.RowHeight = 40;
            this.grd.ScrollType = WSWD.WmallPos.POS.FX.Win.UserControls.ScrollTypes.IndexChanged;
            this.grd.SelectedRowIndex = -1;
            this.grd.ShowPageNo = true;
            this.grd.Size = new System.Drawing.Size(349, 232);
            this.grd.TabIndex = 12;
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.IsHighlight = false;
            this.btnClose.Location = new System.Drawing.Point(198, 269);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            this.btnSave.BorderSize = 1;
            this.btnSave.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnSave.Corner = 3;
            this.btnSave.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnSave.Image = null;
            this.btnSave.IsHighlight = false;
            this.btnSave.Location = new System.Drawing.Point(92, 269);
            this.btnSave.Name = "btnSave";
            this.btnSave.Selected = false;
            this.btnSave.Size = new System.Drawing.Size(90, 42);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "확인";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // POS_PT_P003
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(387, 374);
            this.Name = "POS_PT_P003";
            this.Text = "회원정보 확인";
            this.ContainerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton roundedButton2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel grd;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnSave;
    }
}