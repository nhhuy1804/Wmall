namespace WSWD.WmallPos.POS.PY.VC
{
    partial class POS_PY_P004
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_PY_P004));
            this.roundedButton2 = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.lbl = new System.Windows.Forms.Label();
            this.btnRetry = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.axKSNet_Dongle1 = new AxKSNET_DONGLELib.AxKSNet_Dongle();
            this.ButtonsPanel.SuspendLayout();
            this.ContainerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axKSNet_Dongle1)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.axKSNet_Dongle1);
            this.ButtonsPanel.Controls.Add(this.btnRetry);
            this.ButtonsPanel.Controls.Add(this.btnClose);
            this.ButtonsPanel.Size = new System.Drawing.Size(356, 62);
            // 
            // MessageBar
            // 
            this.MessageBar.Location = new System.Drawing.Point(17, 54);
            this.MessageBar.Size = new System.Drawing.Size(356, 46);
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.lbl);
            this.ContainerPanel.Size = new System.Drawing.Size(390, 179);
            this.ContainerPanel.Controls.SetChildIndex(this.MessageBar, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.lbl, 0);
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
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.IsHighlight = false;
            this.btnClose.Location = new System.Drawing.Point(186, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl
            // 
            this.lbl.Location = new System.Drawing.Point(14, 17);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(359, 28);
            this.lbl.TabIndex = 21;
            this.lbl.Text = "IC카드를 동글이에 삽이하여 주십시오.";
            this.lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRetry
            // 
            this.btnRetry.BorderSize = 1;
            this.btnRetry.Corner = 3;
            this.btnRetry.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.btnRetry.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnRetry.Image = null;
            this.btnRetry.IsHighlight = false;
            this.btnRetry.Location = new System.Drawing.Point(81, 20);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Selected = false;
            this.btnRetry.Size = new System.Drawing.Size(90, 42);
            this.btnRetry.TabIndex = 19;
            this.btnRetry.Text = "재시도";
            this.btnRetry.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // axKSNet_Dongle1
            // 
            this.axKSNet_Dongle1.Enabled = true;
            this.axKSNet_Dongle1.Location = new System.Drawing.Point(331, 20);
            this.axKSNet_Dongle1.Name = "axKSNet_Dongle1";
            this.axKSNet_Dongle1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKSNet_Dongle1.OcxState")));
            this.axKSNet_Dongle1.Size = new System.Drawing.Size(25, 17);
            this.axKSNet_Dongle1.TabIndex = 22;
            // 
            // POS_PY_P004
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(396, 225);
            this.Name = "POS_PY_P004";
            this.Text = "IC카드 정보 입력대기";
            this.ButtonsPanel.ResumeLayout(false);
            this.ContainerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axKSNet_Dongle1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton roundedButton2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private System.Windows.Forms.Label lbl;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnRetry;
        private AxKSNET_DONGLELib.AxKSNet_Dongle axKSNet_Dongle1;
    }
}