namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    partial class MessageBoxDialog
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
            this.picMsgIcon = new System.Windows.Forms.PictureBox();
            this.lblPopupTitle = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnCancelClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnNoOK = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnYes = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picMsgIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // picMsgIcon
            // 
            this.picMsgIcon.BackColor = System.Drawing.Color.Transparent;
            this.picMsgIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picMsgIcon.Image = global::WSWD.WmallPos.POS.FX.Win.Properties.Resources.messagedialog_error;
            this.picMsgIcon.Location = new System.Drawing.Point(29, 62);
            this.picMsgIcon.Name = "picMsgIcon";
            this.picMsgIcon.Size = new System.Drawing.Size(60, 60);
            this.picMsgIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picMsgIcon.TabIndex = 3;
            this.picMsgIcon.TabStop = false;
            // 
            // lblPopupTitle
            // 
            this.lblPopupTitle.AutoSize = true;
            this.lblPopupTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(57)))), ((int)(((byte)(178)))));
            this.lblPopupTitle.Font = new System.Drawing.Font("Dotum", 12F, System.Drawing.FontStyle.Bold);
            this.lblPopupTitle.ForeColor = System.Drawing.Color.White;
            this.lblPopupTitle.Location = new System.Drawing.Point(16, 15);
            this.lblPopupTitle.Name = "lblPopupTitle";
            this.lblPopupTitle.Size = new System.Drawing.Size(0, 16);
            this.lblPopupTitle.TabIndex = 4;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoEllipsis = true;
            this.lblMessage.BackColor = System.Drawing.Color.Transparent;
            this.lblMessage.Font = new System.Drawing.Font("NanumBarunGothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(112, 62);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Padding = new System.Windows.Forms.Padding(5);
            this.lblMessage.Size = new System.Drawing.Size(354, 153);
            this.lblMessage.TabIndex = 5;
            // 
            // btnCancelClose
            // 
            this.btnCancelClose.BorderSize = 1;
            this.btnCancelClose.Corner = 3;
            this.btnCancelClose.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.btnCancelClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnCancelClose.Image = null;
            this.btnCancelClose.Location = new System.Drawing.Point(376, 231);
            this.btnCancelClose.Name = "btnCancelClose";
            this.btnCancelClose.Selected = false;
            this.btnCancelClose.Size = new System.Drawing.Size(90, 42);
            this.btnCancelClose.TabIndex = 7;
            this.btnCancelClose.Text = "Cancel";
            this.btnCancelClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancelClose.Visible = false;
            // 
            // btnNoOK
            // 
            this.btnNoOK.BorderSize = 1;
            this.btnNoOK.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnNoOK.Corner = 3;
            this.btnNoOK.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.btnNoOK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnNoOK.Image = null;
            this.btnNoOK.Location = new System.Drawing.Point(280, 231);
            this.btnNoOK.Name = "btnNoOK";
            this.btnNoOK.Selected = false;
            this.btnNoOK.Size = new System.Drawing.Size(90, 42);
            this.btnNoOK.TabIndex = 8;
            this.btnNoOK.Text = "No";
            this.btnNoOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNoOK.Visible = false;
            // 
            // btnYes
            // 
            this.btnYes.BorderSize = 1;
            this.btnYes.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnYes.Corner = 3;
            this.btnYes.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.btnYes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnYes.Image = null;
            this.btnYes.Location = new System.Drawing.Point(184, 231);
            this.btnYes.Name = "btnYes";
            this.btnYes.Selected = false;
            this.btnYes.Size = new System.Drawing.Size(90, 42);
            this.btnYes.TabIndex = 6;
            this.btnYes.Text = "예";
            this.btnYes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnYes.Visible = false;
            // 
            // MessageBoxDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WSWD.WmallPos.POS.FX.Win.Properties.Resources.msgdialog_bg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(490, 291);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancelClose);
            this.Controls.Add(this.btnNoOK);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblPopupTitle);
            this.Controls.Add(this.picMsgIcon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MessageBoxDialog";
            this.Text = "MessageBoxDialog";
            ((System.ComponentModel.ISupportInitialize)(this.picMsgIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picMsgIcon;
        private System.Windows.Forms.Label lblPopupTitle;
        private System.Windows.Forms.Label lblMessage;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnCancelClose;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnNoOK;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnYes;
    }
}