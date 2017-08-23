namespace WSWD.WmallPos.POS.SL.VC
{
    partial class POS_SL_P004
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
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnSave = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.txtPnl = new WSWD.WmallPos.POS.SL.Controls.AutoRtnTrxnInfo();
            this.grd = new WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel();
            this.ContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.grd);
            this.ContainerPanel.Controls.Add(this.txtPnl);
            this.ContainerPanel.Controls.Add(this.btnClose);
            this.ContainerPanel.Controls.Add(this.btnSave);
            this.ContainerPanel.Size = new System.Drawing.Size(618, 607);
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
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.IsHighlight = false;
            this.btnClose.Location = new System.Drawing.Point(320, 550);
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
            this.btnSave.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnSave.Image = null;
            this.btnSave.IsHighlight = false;
            this.btnSave.Location = new System.Drawing.Point(214, 550);
            this.btnSave.Name = "btnSave";
            this.btnSave.Selected = false;
            this.btnSave.Size = new System.Drawing.Size(90, 42);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "확인";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPnl
            // 
            this.txtPnl.BackColor = System.Drawing.Color.White;
            this.txtPnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPnl.Location = new System.Drawing.Point(17, 280);
            this.txtPnl.Name = "txtPnl";
            this.txtPnl.Size = new System.Drawing.Size(582, 250);
            this.txtPnl.TabIndex = 17;
            // 
            // grd
            // 
            this.grd.AutoFillRows = false;
            this.grd.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.grd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grd.BorderWidth = new System.Windows.Forms.Padding(1);
            this.grd.ColumnCount = 0;
            this.grd.Location = new System.Drawing.Point(17, 17);
            this.grd.Name = "grd";
            this.grd.PageIndex = -1;
            this.grd.RowCount = 5;
            this.grd.RowHeight = 43;
            this.grd.ScrollType = WSWD.WmallPos.POS.FX.Win.UserControls.ScrollTypes.IndexChanged;
            this.grd.SelectedRowIndex = -1;
            this.grd.ShowPageNo = true;
            this.grd.Size = new System.Drawing.Size(582, 244);
            this.grd.TabIndex = 18;
            // 
            // POS_SL_P004
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(624, 653);
            this.Name = "POS_SL_P004";
            this.Text = "공지사항";
            this.ContainerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton roundedButton2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnSave;
        private WSWD.WmallPos.POS.SL.Controls.AutoRtnTrxnInfo txtPnl;
        private WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel grd;
    }
}