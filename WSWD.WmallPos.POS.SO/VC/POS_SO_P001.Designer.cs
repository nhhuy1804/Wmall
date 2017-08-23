namespace WSWD.WmallPos.POS.SO.VC
{
    partial class POS_SO_P001
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
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnSave = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.txtPassword = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.txtCasNo = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.keyPad = new WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad();
            this.ButtonsPanel.SuspendLayout();
            this.ContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.btnClose);
            this.ButtonsPanel.Controls.Add(this.btnSave);
            this.ButtonsPanel.Size = new System.Drawing.Size(530, 62);
            // 
            // MessageBar
            // 
            this.MessageBar.Location = new System.Drawing.Point(17, 266);
            this.MessageBar.Size = new System.Drawing.Size(530, 46);
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.txtPassword);
            this.ContainerPanel.Controls.Add(this.txtCasNo);
            this.ContainerPanel.Controls.Add(this.label3);
            this.ContainerPanel.Controls.Add(this.label1);
            this.ContainerPanel.Controls.Add(this.keyPad);
            this.ContainerPanel.Size = new System.Drawing.Size(564, 391);
            this.ContainerPanel.Controls.SetChildIndex(this.MessageBar, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.keyPad, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.label1, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.label3, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.txtCasNo, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.txtPassword, 0);
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.IsHighlight = false;
            this.btnClose.Location = new System.Drawing.Point(276, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 20;
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
            this.btnSave.Location = new System.Drawing.Point(170, 20);
            this.btnSave.Name = "btnSave";
            this.btnSave.Selected = false;
            this.btnSave.Size = new System.Drawing.Size(90, 42);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "확인";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.White;
            this.txtPassword.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtPassword.BorderWidth = 1;
            this.txtPassword.Corner = 1;
            this.txtPassword.Focusable = true;
            this.txtPassword.FocusedIndex = 0;
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtPassword.Format = null;
            this.txtPassword.HasBorder = true;
            this.txtPassword.IsFocused = false;
            this.txtPassword.Location = new System.Drawing.Point(118, 58);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtPassword.MaxLength = 8;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.txtPassword.PasswordMode = true;
            this.txtPassword.ReadOnly = false;
            this.txtPassword.Size = new System.Drawing.Size(171, 28);
            this.txtPassword.TabIndex = 34;
            this.txtPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCasNo
            // 
            this.txtCasNo.BackColor = System.Drawing.Color.White;
            this.txtCasNo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(109)))), ((int)(((byte)(195)))));
            this.txtCasNo.BorderWidth = 2;
            this.txtCasNo.Corner = 1;
            this.txtCasNo.Focusable = true;
            this.txtCasNo.FocusedIndex = 0;
            this.txtCasNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtCasNo.Format = "#,##0";
            this.txtCasNo.HasBorder = true;
            this.txtCasNo.IsFocused = true;
            this.txtCasNo.Location = new System.Drawing.Point(118, 18);
            this.txtCasNo.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtCasNo.MaxLength = 7;
            this.txtCasNo.Name = "txtCasNo";
            this.txtCasNo.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.txtCasNo.PasswordMode = false;
            this.txtCasNo.ReadOnly = false;
            this.txtCasNo.Size = new System.Drawing.Size(171, 28);
            this.txtCasNo.TabIndex = 33;
            this.txtCasNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 28);
            this.label3.TabIndex = 32;
            this.label3.Text = "비밀번호";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 28);
            this.label1.TabIndex = 31;
            this.label1.Text = "관리자번호";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // keyPad
            // 
            this.keyPad.Location = new System.Drawing.Point(302, 17);
            this.keyPad.Margin = new System.Windows.Forms.Padding(0);
            this.keyPad.MaximumSize = new System.Drawing.Size(245, 233);
            this.keyPad.Name = "keyPad";
            this.keyPad.Size = new System.Drawing.Size(245, 233);
            this.keyPad.TabIndex = 30;
            // 
            // POS_SO_P001
            // 
            this.ClientSize = new System.Drawing.Size(570, 437);
            this.Name = "POS_SO_P001";
            this.Text = "관리자확인";
            this.ButtonsPanel.ResumeLayout(false);
            this.ContainerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtPassword;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtCasNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad keyPad;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnSave;
    }
}