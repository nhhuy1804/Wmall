namespace WSWD.WmallPos.POS.SL.VC
{
    partial class POS_SL_P007
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtTotalGiftAmt = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRetAmt = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.lblBarcode01 = new System.Windows.Forms.Label();
            this.gpPQ11 = new WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel();
            this.btnSave = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.ButtonsPanel.SuspendLayout();
            this.ContainerPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.btnSave);
            this.ButtonsPanel.Controls.Add(this.btnClose);
            this.ButtonsPanel.Size = new System.Drawing.Size(674, 62);
            // 
            // MessageBar
            // 
            this.MessageBar.Location = new System.Drawing.Point(17, 419);
            this.MessageBar.Size = new System.Drawing.Size(674, 46);
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.gpPQ11);
            this.ContainerPanel.Controls.Add(this.panel1);
            this.ContainerPanel.Size = new System.Drawing.Size(708, 544);
            this.ContainerPanel.Controls.SetChildIndex(this.MessageBar, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.panel1, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.gpPQ11, 0);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtTotalGiftAmt);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtRetAmt);
            this.panel1.Controls.Add(this.lblBarcode01);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(17, 17);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.panel1.Size = new System.Drawing.Size(674, 39);
            this.panel1.TabIndex = 10;
            // 
            // txtTotalGiftAmt
            // 
            this.txtTotalGiftAmt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtTotalGiftAmt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtTotalGiftAmt.BorderWidth = 1;
            this.txtTotalGiftAmt.Corner = 1;
            this.txtTotalGiftAmt.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtTotalGiftAmt.Focusable = false;
            this.txtTotalGiftAmt.FocusedIndex = 1;
            this.txtTotalGiftAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtTotalGiftAmt.Format = "#,##0";
            this.txtTotalGiftAmt.HasBorder = true;
            this.txtTotalGiftAmt.IsFocused = false;
            this.txtTotalGiftAmt.Location = new System.Drawing.Point(531, 5);
            this.txtTotalGiftAmt.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtTotalGiftAmt.Name = "txtTotalGiftAmt";
            this.txtTotalGiftAmt.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.txtTotalGiftAmt.PasswordMode = false;
            this.txtTotalGiftAmt.ReadOnly = true;
            this.txtTotalGiftAmt.Size = new System.Drawing.Size(143, 28);
            this.txtTotalGiftAmt.TabIndex = 115;
            this.txtTotalGiftAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.Location = new System.Drawing.Point(406, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 33);
            this.label1.TabIndex = 114;
            this.label1.Tag = "31";
            this.label1.Text = "상품권합계";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRetAmt
            // 
            this.txtRetAmt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtRetAmt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtRetAmt.BorderWidth = 1;
            this.txtRetAmt.Corner = 1;
            this.txtRetAmt.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtRetAmt.Focusable = false;
            this.txtRetAmt.FocusedIndex = 1;
            this.txtRetAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtRetAmt.Format = "#,##0";
            this.txtRetAmt.HasBorder = true;
            this.txtRetAmt.IsFocused = false;
            this.txtRetAmt.Location = new System.Drawing.Point(96, 5);
            this.txtRetAmt.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtRetAmt.Name = "txtRetAmt";
            this.txtRetAmt.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.txtRetAmt.PasswordMode = false;
            this.txtRetAmt.ReadOnly = true;
            this.txtRetAmt.Size = new System.Drawing.Size(143, 28);
            this.txtRetAmt.TabIndex = 113;
            this.txtRetAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBarcode01
            // 
            this.lblBarcode01.AutoEllipsis = true;
            this.lblBarcode01.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblBarcode01.Location = new System.Drawing.Point(0, 3);
            this.lblBarcode01.Name = "lblBarcode01";
            this.lblBarcode01.Size = new System.Drawing.Size(92, 33);
            this.lblBarcode01.TabIndex = 112;
            this.lblBarcode01.Tag = "31";
            this.lblBarcode01.Text = "회수금액";
            this.lblBarcode01.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gpPQ11
            // 
            this.gpPQ11.AutoFillRows = true;
            this.gpPQ11.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.gpPQ11.BorderWidth = new System.Windows.Forms.Padding(1);
            this.gpPQ11.ColumnCount = 0;
            this.gpPQ11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpPQ11.Location = new System.Drawing.Point(17, 56);
            this.gpPQ11.Name = "gpPQ11";
            this.gpPQ11.Padding = new System.Windows.Forms.Padding(1);
            this.gpPQ11.PageIndex = -1;
            this.gpPQ11.RowCount = 10;
            this.gpPQ11.RowHeight = 33;
            this.gpPQ11.ScrollType = WSWD.WmallPos.POS.FX.Win.UserControls.ScrollTypes.IndexChanged;
            this.gpPQ11.SelectedRowIndex = -1;
            this.gpPQ11.ShowPageNo = true;
            this.gpPQ11.Size = new System.Drawing.Size(674, 363);
            this.gpPQ11.TabIndex = 11;
            this.gpPQ11.UnSelectable = false;
            // 
            // btnSave
            // 
            this.btnSave.BorderSize = 1;
            this.btnSave.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnSave.Corner = 3;
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnSave.Image = null;
            this.btnSave.IsHighlight = false;
            this.btnSave.Location = new System.Drawing.Point(0, 20);
            this.btnSave.Name = "btnSave";
            this.btnSave.Selected = false;
            this.btnSave.Size = new System.Drawing.Size(90, 42);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "적용";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.IsHighlight = false;
            this.btnClose.Location = new System.Drawing.Point(584, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // POS_SL_P007
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 590);
            this.Name = "POS_SL_P007";
            this.Text = "반납 상품교환권 등록";
            this.ButtonsPanel.ResumeLayout(false);
            this.ContainerPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblBarcode01;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtRetAmt;
        private System.Windows.Forms.Label label1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtTotalGiftAmt;
        private WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel gpPQ11;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnSave;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
    }
}