namespace WSWD.WmallPos.POS.SO.VC
{
    partial class POS_SO_M001
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.messageBar1 = new WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar();
            this.keyPad1 = new WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad();
            this.txtCasNo = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCasName = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPassword = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.SuspendLayout();
            // 
            // messageBar1
            // 
            this.messageBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.messageBar1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.messageBar1.BorderWidth = new System.Windows.Forms.Padding(1);
            this.messageBar1.ControlType = WSWD.WmallPos.POS.FX.Win.UserControls.MessageBarType.TypeStatus;
            this.messageBar1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.messageBar1.Location = new System.Drawing.Point(231, 519);
            this.messageBar1.MinimumSize = new System.Drawing.Size(0, 35);
            this.messageBar1.Name = "messageBar1";
            this.messageBar1.Size = new System.Drawing.Size(565, 42);
            this.messageBar1.TabIndex = 8;
            this.messageBar1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // keyPad1
            // 
            this.keyPad1.BackColor = System.Drawing.Color.Transparent;
            this.keyPad1.Location = new System.Drawing.Point(551, 251);
            this.keyPad1.Margin = new System.Windows.Forms.Padding(0);
            this.keyPad1.MaximumSize = new System.Drawing.Size(245, 233);
            this.keyPad1.Name = "keyPad1";
            this.keyPad1.Size = new System.Drawing.Size(245, 233);
            this.keyPad1.TabIndex = 7;
            // 
            // intCasNo
            // 
            this.txtCasNo.BackColor = System.Drawing.Color.White;
            this.txtCasNo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtCasNo.BorderWidth = 1;
            this.txtCasNo.Corner = 1;
            this.txtCasNo.Focusable = true;
            this.txtCasNo.FocusedIndex = 0;
            this.txtCasNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtCasNo.Format = null;
            this.txtCasNo.HasBorder = true;
            this.txtCasNo.IsFocused = false;
            this.txtCasNo.Location = new System.Drawing.Point(326, 361);
            this.txtCasNo.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtCasNo.MaxLength = 7;
            this.txtCasNo.Name = "intCasNo";
            this.txtCasNo.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.txtCasNo.PasswordMode = false;
            this.txtCasNo.ReadOnly = false;
            this.txtCasNo.Size = new System.Drawing.Size(202, 33);
            this.txtCasNo.TabIndex = 12;
            this.txtCasNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(233, 371);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 14);
            this.label1.TabIndex = 13;
            this.label1.Text = "계산원번호";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(233, 417);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 14);
            this.label2.TabIndex = 15;
            this.label2.Text = "계산원이름";
            // 
            // intCasName
            // 
            this.txtCasName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtCasName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtCasName.BorderWidth = 1;
            this.txtCasName.Corner = 1;
            this.txtCasName.Focusable = false;
            this.txtCasName.FocusedIndex = 0;
            this.txtCasName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtCasName.Format = null;
            this.txtCasName.HasBorder = true;
            this.txtCasName.IsFocused = false;
            this.txtCasName.Location = new System.Drawing.Point(326, 407);
            this.txtCasName.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtCasName.Name = "intCasName";
            this.txtCasName.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.txtCasName.PasswordMode = false;
            this.txtCasName.ReadOnly = true;
            this.txtCasName.Size = new System.Drawing.Size(202, 33);
            this.txtCasName.TabIndex = 14;
            this.txtCasName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(233, 461);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 14);
            this.label3.TabIndex = 17;
            this.label3.Text = "비 밀 번 호";
            // 
            // intPassword
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
            this.txtPassword.Location = new System.Drawing.Point(326, 451);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtPassword.MaxLength = 4;
            this.txtPassword.Name = "intPassword";
            this.txtPassword.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.txtPassword.PasswordMode = true;
            this.txtPassword.ReadOnly = false;
            this.txtPassword.Size = new System.Drawing.Size(202, 33);
            this.txtPassword.TabIndex = 16;
            this.txtPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // POS_SO_M001
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::WSWD.WmallPos.POS.SO.Properties.Resources.bg_login1;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCasName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCasNo);
            this.Controls.Add(this.messageBar1);
            this.Controls.Add(this.keyPad1);
            this.Font = new System.Drawing.Font("Dotum", 10F, System.Drawing.FontStyle.Bold);
            this.Name = "POS_SO_M001";
            this.Size = new System.Drawing.Size(1024, 692);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar messageBar1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad keyPad1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtCasNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtCasName;
        private System.Windows.Forms.Label label3;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtPassword;

    }
}
