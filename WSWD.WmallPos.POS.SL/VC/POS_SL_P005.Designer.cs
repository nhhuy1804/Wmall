namespace WSWD.WmallPos.POS.SL.VC
{
    partial class POS_SL_P005
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
            this.panel2 = new System.Windows.Forms.Label();
            this.gpPQ11 = new WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel();
            this.panel1 = new System.Windows.Forms.Label();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnConfirm = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.gpPQ12 = new WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblRtnReason = new System.Windows.Forms.Label();
            this.btnRefundCanc = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.ButtonsPanel.SuspendLayout();
            this.ContainerPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.btnRefundCanc);
            this.ButtonsPanel.Controls.Add(this.btnConfirm);
            this.ButtonsPanel.Controls.Add(this.btnClose);
            this.ButtonsPanel.Location = new System.Drawing.Point(659, 10);
            this.ButtonsPanel.Size = new System.Drawing.Size(325, 46);
            // 
            // MessageBar
            // 
            this.MessageBar.Size = new System.Drawing.Size(654, 46);
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.gpPQ12);
            this.ContainerPanel.Controls.Add(this.panel2);
            this.ContainerPanel.Controls.Add(this.gpPQ11);
            this.ContainerPanel.Controls.Add(this.panel3);
            this.ContainerPanel.Size = new System.Drawing.Size(1018, 497);
            this.ContainerPanel.Controls.SetChildIndex(this.panel3, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.gpPQ11, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.panel2, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.gpPQ12, 0);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Image = global::WSWD.WmallPos.POS.SL.Properties.Resources.bullet;
            this.panel2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.panel2.Location = new System.Drawing.Point(17, 219);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.panel2.Size = new System.Drawing.Size(984, 33);
            this.panel2.TabIndex = 9;
            this.panel2.Text = "    사은품 증정별 행사, 합산 영수증 정보";
            this.panel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gpPQ11
            // 
            this.gpPQ11.AutoFillRows = true;
            this.gpPQ11.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.gpPQ11.BorderWidth = new System.Windows.Forms.Padding(1);
            this.gpPQ11.ColumnCount = 0;
            this.gpPQ11.DisableSelection = false;
            this.gpPQ11.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpPQ11.Location = new System.Drawing.Point(17, 47);
            this.gpPQ11.Name = "gpPQ11";
            this.gpPQ11.Padding = new System.Windows.Forms.Padding(1);
            this.gpPQ11.PageIndex = -1;
            this.gpPQ11.RowCount = 4;
            this.gpPQ11.RowHeight = 35;
            this.gpPQ11.ScrollType = WSWD.WmallPos.POS.FX.Win.UserControls.ScrollTypes.IndexChanged;
            this.gpPQ11.SelectedRowIndex = -1;
            this.gpPQ11.ShowPageNo = true;
            this.gpPQ11.Size = new System.Drawing.Size(984, 172);
            this.gpPQ11.TabIndex = 8;
            this.gpPQ11.UnSelectable = false;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Image = global::WSWD.WmallPos.POS.SL.Properties.Resources.bullet;
            this.panel1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 7);
            this.panel1.Size = new System.Drawing.Size(534, 30);
            this.panel1.TabIndex = 7;
            this.panel1.Text = "    사은품 증정 내역";
            this.panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.IsHighlight = false;
            this.btnClose.Location = new System.Drawing.Point(6, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            this.btnConfirm.BorderSize = 1;
            this.btnConfirm.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnConfirm.Corner = 3;
            this.btnConfirm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnConfirm.Image = null;
            this.btnConfirm.IsHighlight = false;
            this.btnConfirm.Location = new System.Drawing.Point(232, 3);
            this.btnConfirm.Name = "btnSave";
            this.btnConfirm.Selected = false;
            this.btnConfirm.Size = new System.Drawing.Size(90, 42);
            this.btnConfirm.TabIndex = 1;
            this.btnConfirm.Text = "확정";
            this.btnConfirm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gpPQ12
            // 
            this.gpPQ12.AutoFillRows = true;
            this.gpPQ12.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.gpPQ12.BorderWidth = new System.Windows.Forms.Padding(1);
            this.gpPQ12.ColumnCount = 0;
            this.gpPQ12.DisableSelection = false;
            this.gpPQ12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpPQ12.Location = new System.Drawing.Point(17, 252);
            this.gpPQ12.Name = "gpPQ12";
            this.gpPQ12.Padding = new System.Windows.Forms.Padding(1);
            this.gpPQ12.PageIndex = -1;
            this.gpPQ12.RowCount = 4;
            this.gpPQ12.RowHeight = 35;
            this.gpPQ12.ScrollType = WSWD.WmallPos.POS.FX.Win.UserControls.ScrollTypes.IndexChanged;
            this.gpPQ12.SelectedRowIndex = -1;
            this.gpPQ12.ShowPageNo = true;
            this.gpPQ12.Size = new System.Drawing.Size(984, 172);
            this.gpPQ12.TabIndex = 12;
            this.gpPQ12.UnSelectable = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblRtnReason);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(17, 17);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(984, 30);
            this.panel3.TabIndex = 13;
            // 
            // lblRtnReason
            // 
            this.lblRtnReason.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblRtnReason.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.lblRtnReason.Location = new System.Drawing.Point(565, 0);
            this.lblRtnReason.Name = "lblRtnReason";
            this.lblRtnReason.Size = new System.Drawing.Size(419, 30);
            this.lblRtnReason.TabIndex = 8;
            this.lblRtnReason.Text = "미회수 사유: 1-불응 / 2-기타";
            this.lblRtnReason.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnRefundCanc
            // 
            this.btnRefundCanc.BorderSize = 1;
            this.btnRefundCanc.Corner = 3;
            this.btnRefundCanc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnRefundCanc.Image = null;
            this.btnRefundCanc.IsHighlight = false;
            this.btnRefundCanc.Location = new System.Drawing.Point(102, 2);
            this.btnRefundCanc.Name = "btnRefundCanc";
            this.btnRefundCanc.Selected = false;
            this.btnRefundCanc.Size = new System.Drawing.Size(90, 42);
            this.btnRefundCanc.TabIndex = 2;
            this.btnRefundCanc.Text = "반납취소";
            this.btnRefundCanc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // POS_SL_P005
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1024, 543);
            this.Name = "POS_SL_P005";
            this.Text = "사은품 회수";
            this.ButtonsPanel.ResumeLayout(false);
            this.ContainerPanel.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label panel2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel gpPQ11;
        private System.Windows.Forms.Label panel1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnConfirm;
        private WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel gpPQ12;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblRtnReason;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnRefundCanc;
    }
}