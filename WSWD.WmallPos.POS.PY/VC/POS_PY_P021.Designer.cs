namespace WSWD.WmallPos.POS.PY.VC
{
    partial class POS_PY_P021
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
            this.roundedButton2 = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.lbl = new System.Windows.Forms.Label();
            this.cpbWaiting = new WSWD.WmallPos.POS.FX.Win.Controls.ColorProgressBar();
            this.lblRemTime = new System.Windows.Forms.Label();
            this.ButtonsPanel.SuspendLayout();
            this.ContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.lblRemTime);
            this.ButtonsPanel.Controls.Add(this.btnClose);
            this.ButtonsPanel.Location = new System.Drawing.Point(17, 130);
            this.ButtonsPanel.Size = new System.Drawing.Size(415, 62);
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.cpbWaiting);
            this.ContainerPanel.Controls.Add(this.lbl);
            this.ContainerPanel.Size = new System.Drawing.Size(449, 209);
            this.ContainerPanel.Controls.SetChildIndex(this.ButtonsPanel, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.lbl, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.cpbWaiting, 0);
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
            this.btnClose.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.IsHighlight = false;
            this.btnClose.Location = new System.Drawing.Point(162, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl
            // 
            this.lbl.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.lbl.Location = new System.Drawing.Point(17, 17);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(415, 61);
            this.lbl.TabIndex = 21;
            this.lbl.Text = "IC칩 카드 인식에 실패하였습니다.\r\n카드제거 후 사인패드에 신용카드를\r\n리딩 하여 주십시오.";
            this.lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cpbWaiting
            // 
            this.cpbWaiting.Location = new System.Drawing.Point(17, 81);
            this.cpbWaiting.Name = "cpbWaiting";
            this.cpbWaiting.Percentage = 0;
            this.cpbWaiting.Size = new System.Drawing.Size(415, 33);
            this.cpbWaiting.TabIndex = 22;
            // 
            // lblRemTime
            // 
            this.lblRemTime.Location = new System.Drawing.Point(347, 0);
            this.lblRemTime.Name = "lblRemTime";
            this.lblRemTime.Size = new System.Drawing.Size(68, 24);
            this.lblRemTime.TabIndex = 19;
            this.lblRemTime.Text = "3초";
            this.lblRemTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // POS_PY_P021
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(455, 255);
            this.Name = "POS_PY_P021";
            this.Text = "IC카드 승인 입력대기";
            this.ButtonsPanel.ResumeLayout(false);
            this.ContainerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton roundedButton2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private System.Windows.Forms.Label lbl;
        private WSWD.WmallPos.POS.FX.Win.Controls.ColorProgressBar cpbWaiting;
        private System.Windows.Forms.Label lblRemTime;
    }
}