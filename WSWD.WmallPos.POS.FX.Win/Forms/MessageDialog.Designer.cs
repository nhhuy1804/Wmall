namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    partial class MessageDialog
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
            this.lblMessage = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedLabel();
            this.btnYes = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.picMsgIcon = new System.Windows.Forms.PictureBox();
            this.btnNoOK = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnCancelClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.ContainerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMsgIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.btnCancelClose);
            this.ContainerPanel.Controls.Add(this.btnNoOK);
            this.ContainerPanel.Controls.Add(this.picMsgIcon);
            this.ContainerPanel.Controls.Add(this.btnYes);
            this.ContainerPanel.Controls.Add(this.lblMessage);
            this.ContainerPanel.Size = new System.Drawing.Size(444, 235);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoEllipsis = true;
            this.lblMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.lblMessage.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblMessage.BorderWidth = new System.Windows.Forms.Padding(1);
            this.lblMessage.Corner = 1;
            this.lblMessage.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(108, 17);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Padding = new System.Windows.Forms.Padding(8);
            this.lblMessage.Size = new System.Drawing.Size(322, 153);
            this.lblMessage.TabIndex = 0;
            // 
            // btnYes
            // 
            this.btnYes.BorderSize = 1;
            this.btnYes.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnYes.Corner = 3;
            this.btnYes.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnYes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnYes.Image = null;
            this.btnYes.IsHighlight = false;
            this.btnYes.Location = new System.Drawing.Point(148, 184);
            this.btnYes.Name = "btnYes";
            this.btnYes.Selected = false;
            this.btnYes.Size = new System.Drawing.Size(90, 42);
            this.btnYes.TabIndex = 1;
            this.btnYes.Text = "예";
            this.btnYes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnYes.Visible = false;
            // 
            // picMsgIcon
            // 
            this.picMsgIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picMsgIcon.Image = global::WSWD.WmallPos.POS.FX.Win.Properties.Resources.messagedialog_error;
            this.picMsgIcon.Location = new System.Drawing.Point(24, 17);
            this.picMsgIcon.Name = "picMsgIcon";
            this.picMsgIcon.Size = new System.Drawing.Size(60, 60);
            this.picMsgIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picMsgIcon.TabIndex = 2;
            this.picMsgIcon.TabStop = false;
            // 
            // btnNoOK
            // 
            this.btnNoOK.BorderSize = 1;
            this.btnNoOK.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnNoOK.Corner = 3;
            this.btnNoOK.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnNoOK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnNoOK.Image = null;
            this.btnNoOK.IsHighlight = false;
            this.btnNoOK.Location = new System.Drawing.Point(244, 184);
            this.btnNoOK.Name = "btnNoOK";
            this.btnNoOK.Selected = false;
            this.btnNoOK.Size = new System.Drawing.Size(90, 42);
            this.btnNoOK.TabIndex = 3;
            this.btnNoOK.Text = "No";
            this.btnNoOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNoOK.Visible = false;
            // 
            // btnCancelClose
            // 
            this.btnCancelClose.BorderSize = 1;
            this.btnCancelClose.Corner = 3;
            this.btnCancelClose.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnCancelClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnCancelClose.Image = null;
            this.btnCancelClose.IsHighlight = false;
            this.btnCancelClose.Location = new System.Drawing.Point(340, 184);
            this.btnCancelClose.Name = "btnCancelClose";
            this.btnCancelClose.Selected = false;
            this.btnCancelClose.Size = new System.Drawing.Size(90, 42);
            this.btnCancelClose.TabIndex = 3;
            this.btnCancelClose.Text = "Cancel";
            this.btnCancelClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancelClose.Visible = false;
            // 
            // MessageDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(450, 281);
            this.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.Name = "MessageDialog";
            this.ContainerPanel.ResumeLayout(false);
            this.ContainerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMsgIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedLabel lblMessage;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnYes;
        private System.Windows.Forms.PictureBox picMsgIcon;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnNoOK;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnCancelClose;
    }
}