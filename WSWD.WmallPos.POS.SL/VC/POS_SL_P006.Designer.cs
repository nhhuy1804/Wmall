namespace WSWD.WmallPos.POS.SL.VC
{
    partial class POS_SL_P006
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
            this.btnOK = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.ButtonsPanel.SuspendLayout();
            this.ContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.btnOK);
            this.ButtonsPanel.Location = new System.Drawing.Point(17, 215);
            this.ButtonsPanel.Size = new System.Drawing.Size(445, 55);
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.lblMessage);
            this.ContainerPanel.Size = new System.Drawing.Size(479, 287);
            this.ContainerPanel.Controls.SetChildIndex(this.ButtonsPanel, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.lblMessage, 0);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoEllipsis = true;
            this.lblMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.lblMessage.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblMessage.BorderWidth = new System.Windows.Forms.Padding(1);
            this.lblMessage.Corner = 1;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Font = new System.Drawing.Font("Dotum", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(17, 17);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Padding = new System.Windows.Forms.Padding(8);
            this.lblMessage.Size = new System.Drawing.Size(445, 198);
            this.lblMessage.TabIndex = 8;
            // 
            // btnOK
            // 
            this.btnOK.BorderSize = 1;
            this.btnOK.Corner = 3;
            this.btnOK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnOK.Image = null;
            this.btnOK.IsHighlight = false;
            this.btnOK.KeyType = WSWD.WmallPos.POS.FX.Win.UserControls.KeyButtonTypes.EnterOrClear;
            this.btnOK.Location = new System.Drawing.Point(177, 13);
            this.btnOK.Name = "btnOK";
            this.btnOK.Selected = false;
            this.btnOK.Size = new System.Drawing.Size(90, 42);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "확인";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // POS_SL_P006
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 333);
            this.Name = "POS_SL_P006";
            this.Text = "강제취소 확인";
            this.ButtonsPanel.ResumeLayout(false);
            this.ContainerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedLabel lblMessage;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnOK;
    }
}