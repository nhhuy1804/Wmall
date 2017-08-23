namespace WSWD.WmallPos.POS.Config
{
    partial class AdminPassPop
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
            this.intPassword = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.btnOK = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.ButtonsPanel.SuspendLayout();
            this.ContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.btnClose);
            this.ButtonsPanel.Controls.Add(this.btnOK);
            this.ButtonsPanel.Size = new System.Drawing.Size(386, 62);
            // 
            // MessageBar
            // 
            this.MessageBar.Location = new System.Drawing.Point(17, 69);
            this.MessageBar.Size = new System.Drawing.Size(386, 46);
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.intPassword);
            this.ContainerPanel.Size = new System.Drawing.Size(420, 194);
            this.ContainerPanel.Controls.SetChildIndex(this.intPassword, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.MessageBar, 0);
            // 
            // intPassword
            // 
            this.intPassword.BackColor = System.Drawing.Color.White;
            this.intPassword.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(109)))), ((int)(((byte)(195)))));
            this.intPassword.BorderWidth = 2;
            this.intPassword.Corner = 1;
            this.intPassword.Focusable = true;
            this.intPassword.FocusedIndex = 0;
            this.intPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.intPassword.Format = null;
            this.intPassword.HasBorder = true;
            this.intPassword.IsFocused = true;
            this.intPassword.Location = new System.Drawing.Point(94, 18);
            this.intPassword.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.intPassword.Name = "intPassword";
            this.intPassword.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.intPassword.PasswordMode = true;
            this.intPassword.ReadOnly = false;
            this.intPassword.Size = new System.Drawing.Size(232, 37);
            this.intPassword.MaxLength = 6;
            this.intPassword.TabIndex = 9;
            this.intPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOK
            // 
            this.btnOK.BorderSize = 1;
            this.btnOK.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnOK.Corner = 3;
            this.btnOK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnOK.Image = null;
            this.btnOK.Location = new System.Drawing.Point(97, 19);
            this.btnOK.Name = "btnOK";
            this.btnOK.Selected = false;
            this.btnOK.Size = new System.Drawing.Size(90, 42);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "확인";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.Location = new System.Drawing.Point(200, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AdminPassPop
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(426, 240);
            this.Name = "AdminPassPop";
            this.Text = "관리자확인";
            this.ButtonsPanel.ResumeLayout(false);
            this.ContainerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText intPassword;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnOK;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;

    }
}