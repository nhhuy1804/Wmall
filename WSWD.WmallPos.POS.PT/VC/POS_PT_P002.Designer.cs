namespace WSWD.WmallPos.POS.PT.VC
{
    partial class POS_PT_P002
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
            this.txtCustName = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.txtCardNo = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnRetry = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.msgBar = new WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar();
            this.ContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.txtCustName);
            this.ContainerPanel.Controls.Add(this.txtCardNo);
            this.ContainerPanel.Controls.Add(this.label4);
            this.ContainerPanel.Controls.Add(this.label3);
            this.ContainerPanel.Controls.Add(this.btnClose);
            this.ContainerPanel.Controls.Add(this.btnRetry);
            this.ContainerPanel.Controls.Add(this.msgBar);
            this.ContainerPanel.Size = new System.Drawing.Size(566, 159);
            // 
            // txtCustName
            // 
            this.txtCustName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtCustName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtCustName.BorderWidth = 1;
            this.txtCustName.Corner = 1;
            this.txtCustName.Focusable = false;
            this.txtCustName.FocusedIndex = 0;
            this.txtCustName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtCustName.Format = null;
            this.txtCustName.HasBorder = true;
            this.txtCustName.IsFocused = false;
            this.txtCustName.Location = new System.Drawing.Point(107, 53);
            this.txtCustName.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtCustName.Name = "txtCustName";
            this.txtCustName.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.txtCustName.PasswordMode = false;
            this.txtCustName.ReadOnly = true;
            this.txtCustName.Size = new System.Drawing.Size(238, 28);
            this.txtCustName.TabIndex = 37;
            this.txtCustName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCardNo
            // 
            this.txtCardNo.BackColor = System.Drawing.Color.White;
            this.txtCardNo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtCardNo.BorderWidth = 1;
            this.txtCardNo.Corner = 1;
            this.txtCardNo.Focusable = true;
            this.txtCardNo.FocusedIndex = 1;
            this.txtCardNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtCardNo.Format = null;
            this.txtCardNo.HasBorder = true;
            this.txtCardNo.IsFocused = false;
            this.txtCardNo.Location = new System.Drawing.Point(107, 17);
            this.txtCardNo.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtCardNo.MaxLength = 13;
            this.txtCardNo.Name = "txtCardNo";
            this.txtCardNo.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.txtCardNo.PasswordMode = false;
            this.txtCardNo.ReadOnly = false;
            this.txtCardNo.Size = new System.Drawing.Size(238, 28);
            this.txtCardNo.TabIndex = 36;
            this.txtCardNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(14, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 28);
            this.label4.TabIndex = 35;
            this.label4.Text = "회원이름";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 28);
            this.label3.TabIndex = 34;
            this.label3.Text = "카드번호";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.Location = new System.Drawing.Point(459, 29);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 33;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRetry
            // 
            this.btnRetry.BorderSize = 1;
            this.btnRetry.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnRetry.Corner = 3;
            this.btnRetry.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.btnRetry.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnRetry.Image = null;
            this.btnRetry.Location = new System.Drawing.Point(357, 29);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Selected = false;
            this.btnRetry.Size = new System.Drawing.Size(90, 42);
            this.btnRetry.TabIndex = 32;
            this.btnRetry.Text = "재시도";
            this.btnRetry.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // msgBar
            // 
            this.msgBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.msgBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.msgBar.BorderWidth = new System.Windows.Forms.Padding(1);
            this.msgBar.ControlType = WSWD.WmallPos.POS.FX.Win.UserControls.MessageBarType.TypeStatus;
            this.msgBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.msgBar.Location = new System.Drawing.Point(17, 99);
            this.msgBar.MinimumSize = new System.Drawing.Size(0, 35);
            this.msgBar.Name = "msgBar";
            this.msgBar.Size = new System.Drawing.Size(532, 42);
            this.msgBar.TabIndex = 31;
            this.msgBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // POS_PT_P002
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(572, 205);
            this.Name = "POS_PT_P002";
            this.Text = "포인트 적립";
            this.ContainerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtCustName;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtCardNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnRetry;
        private WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar msgBar;
    }
}