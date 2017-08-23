namespace WSWD.WmallPos.POS.IO.VC
{
    partial class POS_IO_M001
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_IO_M001));
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.msgBar = new WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar();
            this.keyPad1 = new WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad();
            this.lblTicket01 = new System.Windows.Forms.Label();
            this.txtPreReserveAmt = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label1 = new System.Windows.Forms.Label();
            this.txtReserveAmt = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.Location = new System.Drawing.Point(324, 488);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // msgBar
            // 
            this.msgBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.msgBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.msgBar.BorderWidth = new System.Windows.Forms.Padding(1);
            this.msgBar.ControlType = WSWD.WmallPos.POS.FX.Win.UserControls.MessageBarType.TypeStatus;
            this.msgBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.msgBar.Location = new System.Drawing.Point(67, 426);
            this.msgBar.MinimumSize = new System.Drawing.Size(0, 35);
            this.msgBar.Name = "msgBar";
            this.msgBar.Size = new System.Drawing.Size(605, 42);
            this.msgBar.TabIndex = 3;
            this.msgBar.TabStop = false;
            this.msgBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // keyPad1
            // 
            this.keyPad1.Location = new System.Drawing.Point(427, 162);
            this.keyPad1.Margin = new System.Windows.Forms.Padding(0);
            this.keyPad1.MaximumSize = new System.Drawing.Size(245, 233);
            this.keyPad1.Name = "keyPad1";
            this.keyPad1.Size = new System.Drawing.Size(245, 233);
            this.keyPad1.TabIndex = 4;
            // 
            // lblTicket01
            // 
            this.lblTicket01.AutoEllipsis = true;
            this.lblTicket01.BackColor = System.Drawing.Color.Transparent;
            this.lblTicket01.Location = new System.Drawing.Point(67, 248);
            this.lblTicket01.Name = "lblTicket01";
            this.lblTicket01.Size = new System.Drawing.Size(100, 28);
            this.lblTicket01.TabIndex = 108;
            this.lblTicket01.Tag = "31";
            this.lblTicket01.Text = "이전준비금";
            this.lblTicket01.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPreReserveAmt
            // 
            this.txtPreReserveAmt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtPreReserveAmt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtPreReserveAmt.BorderWidth = 1;
            this.txtPreReserveAmt.Corner = 1;
            this.txtPreReserveAmt.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtPreReserveAmt.Focusable = false;
            this.txtPreReserveAmt.FocusedIndex = 0;
            this.txtPreReserveAmt.Font = new System.Drawing.Font("Dotum", 12F, System.Drawing.FontStyle.Bold);
            this.txtPreReserveAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtPreReserveAmt.Format = "#,##0";
            this.txtPreReserveAmt.HasBorder = true;
            this.txtPreReserveAmt.IsFocused = false;
            this.txtPreReserveAmt.Location = new System.Drawing.Point(173, 248);
            this.txtPreReserveAmt.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtPreReserveAmt.MaxLength = 8;
            this.txtPreReserveAmt.Name = "txtPreReserveAmt";
            this.txtPreReserveAmt.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.txtPreReserveAmt.PasswordMode = false;
            this.txtPreReserveAmt.ReadOnly = true;
            this.txtPreReserveAmt.Size = new System.Drawing.Size(160, 28);
            this.txtPreReserveAmt.TabIndex = 109;
            this.txtPreReserveAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(67, 284);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 28);
            this.label1.TabIndex = 110;
            this.label1.Tag = "31";
            this.label1.Text = "준비금";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtReserveAmt
            // 
            this.txtReserveAmt.BackColor = System.Drawing.Color.White;
            this.txtReserveAmt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtReserveAmt.BorderWidth = 1;
            this.txtReserveAmt.Corner = 1;
            this.txtReserveAmt.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtReserveAmt.Focusable = true;
            this.txtReserveAmt.FocusedIndex = 0;
            this.txtReserveAmt.Font = new System.Drawing.Font("Dotum", 12F, System.Drawing.FontStyle.Bold);
            this.txtReserveAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtReserveAmt.Format = "#,##0";
            this.txtReserveAmt.HasBorder = true;
            this.txtReserveAmt.IsFocused = false;
            this.txtReserveAmt.Location = new System.Drawing.Point(173, 284);
            this.txtReserveAmt.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtReserveAmt.MaxLength = 8;
            this.txtReserveAmt.Name = "txtReserveAmt";
            this.txtReserveAmt.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.txtReserveAmt.PasswordMode = false;
            this.txtReserveAmt.ReadOnly = false;
            this.txtReserveAmt.Size = new System.Drawing.Size(160, 28);
            this.txtReserveAmt.TabIndex = 111;
            this.txtReserveAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // POS_IO_M001
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtReserveAmt);
            this.Controls.Add(this.lblTicket01);
            this.Controls.Add(this.txtPreReserveAmt);
            this.Controls.Add(this.keyPad1);
            this.Controls.Add(this.msgBar);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Dotum", 10F, System.Drawing.FontStyle.Bold);
            this.IsModal = true;
            this.Name = "POS_IO_M001";
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar msgBar;
        private WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad keyPad1;
        private System.Windows.Forms.Label lblTicket01;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtPreReserveAmt;
        private System.Windows.Forms.Label label1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtReserveAmt;
    }
}
