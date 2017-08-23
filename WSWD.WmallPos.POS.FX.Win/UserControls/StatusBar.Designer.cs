namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    partial class StatusBar
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
            this.VersionText = new System.Windows.Forms.Label();
            this.lblStatusMessage = new System.Windows.Forms.Label();
            this.lblItemStatus = new WSWD.WmallPos.POS.FX.Win.UserControls.CommStatusLabel();
            this.SuspendLayout();
            // 
            // VersionText
            // 
            this.VersionText.Dock = System.Windows.Forms.DockStyle.Right;
            this.VersionText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(191)))), ((int)(((byte)(212)))));
            this.VersionText.Location = new System.Drawing.Point(649, 0);
            this.VersionText.Name = "VersionText";
            this.VersionText.Size = new System.Drawing.Size(375, 28);
            this.VersionText.TabIndex = 2;
            this.VersionText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStatusMessage
            // 
            this.lblStatusMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatusMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(191)))), ((int)(((byte)(212)))));
            this.lblStatusMessage.Location = new System.Drawing.Point(0, 0);
            this.lblStatusMessage.Name = "lblStatusMessage";
            this.lblStatusMessage.Size = new System.Drawing.Size(366, 28);
            this.lblStatusMessage.TabIndex = 5;
            this.lblStatusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblItemStatus
            // 
            this.lblItemStatus.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblItemStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(191)))), ((int)(((byte)(212)))));
            this.lblItemStatus.Location = new System.Drawing.Point(366, 0);
            this.lblItemStatus.Name = "lblItemStatus";
            this.lblItemStatus.Size = new System.Drawing.Size(283, 28);
            this.lblItemStatus.TabIndex = 4;
            // 
            // StatusBar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(39)))), ((int)(((byte)(111)))));
            this.Controls.Add(this.lblStatusMessage);
            this.Controls.Add(this.lblItemStatus);
            this.Controls.Add(this.VersionText);
            this.Name = "StatusBar";
            this.Size = new System.Drawing.Size(1024, 28);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label VersionText;
        private WSWD.WmallPos.POS.FX.Win.UserControls.CommStatusLabel lblItemStatus;
        private System.Windows.Forms.Label lblStatusMessage;

    }
}
