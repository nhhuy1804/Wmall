namespace WSWD.WmallPos.POS.PY.VC
{
    partial class POS_PY_P010
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
            this.grd = new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnSave = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.txtPaymentAmt = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGetAmt = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPaymentCnt = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label2 = new System.Windows.Forms.Label();
            this.ButtonsPanel.SuspendLayout();
            this.ContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.btnSave);
            this.ButtonsPanel.Controls.Add(this.btnClose);
            this.ButtonsPanel.Size = new System.Drawing.Size(525, 62);
            // 
            // MessageBar
            // 
            this.MessageBar.Location = new System.Drawing.Point(17, 424);
            this.MessageBar.Size = new System.Drawing.Size(525, 46);
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.txtGetAmt);
            this.ContainerPanel.Controls.Add(this.txtPaymentCnt);
            this.ContainerPanel.Controls.Add(this.label3);
            this.ContainerPanel.Controls.Add(this.txtPaymentAmt);
            this.ContainerPanel.Controls.Add(this.label2);
            this.ContainerPanel.Controls.Add(this.grd);
            this.ContainerPanel.Controls.Add(this.label1);
            this.ContainerPanel.Size = new System.Drawing.Size(559, 549);
            this.ContainerPanel.Controls.SetChildIndex(this.label1, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.grd, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.label2, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.txtPaymentAmt, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.label3, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.txtPaymentCnt, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.txtGetAmt, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.MessageBar, 0);
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
            this.grd.BackColor = System.Drawing.Color.White;
            this.grd.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.grd.BorderWidth = new System.Windows.Forms.Padding(1);
            this.grd.CurrentRowIndex = -1;
            this.grd.Location = new System.Drawing.Point(17, 58);
            this.grd.Name = "grd";
            this.grd.Padding = new System.Windows.Forms.Padding(1);
            this.grd.PageIndex = 0;
            this.grd.ScrollType = WSWD.WmallPos.POS.FX.Win.UserControls.GridScrollType.Row;
            this.grd.Size = new System.Drawing.Size(525, 271);
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
            this.btnClose.Location = new System.Drawing.Point(272, 20);
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
            this.btnSave.Location = new System.Drawing.Point(163, 20);
            this.btnSave.Name = "btnSave";
            this.btnSave.Selected = false;
            this.btnSave.Size = new System.Drawing.Size(90, 42);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "쿠폰확정";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPaymentAmt
            // 
            this.txtPaymentAmt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtPaymentAmt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtPaymentAmt.BorderWidth = 1;
            this.txtPaymentAmt.Corner = 1;
            this.txtPaymentAmt.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtPaymentAmt.Focusable = false;
            this.txtPaymentAmt.FocusedIndex = 0;
            this.txtPaymentAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtPaymentAmt.Format = "#,##0";
            this.txtPaymentAmt.HasBorder = true;
            this.txtPaymentAmt.IsFocused = false;
            this.txtPaymentAmt.Location = new System.Drawing.Point(100, 380);
            this.txtPaymentAmt.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtPaymentAmt.MaxLength = 9;
            this.txtPaymentAmt.Name = "txtPaymentAmt";
            this.txtPaymentAmt.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.txtPaymentAmt.PasswordMode = false;
            this.txtPaymentAmt.ReadOnly = true;
            this.txtPaymentAmt.Size = new System.Drawing.Size(200, 28);
            this.txtPaymentAmt.TabIndex = 35;
            this.txtPaymentAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 380);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 28);
            this.label1.TabIndex = 34;
            this.label1.Text = "할인금액";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtGetAmt
            // 
            this.txtGetAmt.BackColor = System.Drawing.Color.White;
            this.txtGetAmt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtGetAmt.BorderWidth = 1;
            this.txtGetAmt.Corner = 1;
            this.txtGetAmt.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtGetAmt.Focusable = true;
            this.txtGetAmt.FocusedIndex = 0;
            this.txtGetAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtGetAmt.Format = "#,##0";
            this.txtGetAmt.HasBorder = true;
            this.txtGetAmt.IsFocused = false;
            this.txtGetAmt.Location = new System.Drawing.Point(100, 17);
            this.txtGetAmt.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtGetAmt.MaxLength = 9;
            this.txtGetAmt.Name = "txtGetAmt";
            this.txtGetAmt.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.txtGetAmt.PasswordMode = false;
            this.txtGetAmt.ReadOnly = false;
            this.txtGetAmt.Size = new System.Drawing.Size(200, 28);
            this.txtGetAmt.TabIndex = 33;
            this.txtGetAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 28);
            this.label3.TabIndex = 32;
            this.label3.Text = "받을돈";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPaymentCnt
            // 
            this.txtPaymentCnt.BackColor = System.Drawing.Color.White;
            this.txtPaymentCnt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(109)))), ((int)(((byte)(195)))));
            this.txtPaymentCnt.BorderWidth = 2;
            this.txtPaymentCnt.Corner = 1;
            this.txtPaymentCnt.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtPaymentCnt.Focusable = true;
            this.txtPaymentCnt.FocusedIndex = 0;
            this.txtPaymentCnt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtPaymentCnt.Format = "#,##0";
            this.txtPaymentCnt.HasBorder = true;
            this.txtPaymentCnt.IsFocused = true;
            this.txtPaymentCnt.Location = new System.Drawing.Point(100, 344);
            this.txtPaymentCnt.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtPaymentCnt.MaxLength = 3;
            this.txtPaymentCnt.Name = "txtPaymentCnt";
            this.txtPaymentCnt.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.txtPaymentCnt.PasswordMode = false;
            this.txtPaymentCnt.ReadOnly = false;
            this.txtPaymentCnt.Size = new System.Drawing.Size(200, 28);
            this.txtPaymentCnt.TabIndex = 38;
            this.txtPaymentCnt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(14, 344);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 28);
            this.label2.TabIndex = 37;
            this.label2.Text = "수량";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // POS_PY_P010
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(565, 595);
            this.Name = "POS_PY_P010";
            this.Text = "쿠폰할인";
            this.ButtonsPanel.ResumeLayout(false);
            this.ContainerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton roundedButton2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel grd;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnSave;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtPaymentAmt;
        private System.Windows.Forms.Label label1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtGetAmt;
        private System.Windows.Forms.Label label3;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtPaymentCnt;
        private System.Windows.Forms.Label label2;
    }
}