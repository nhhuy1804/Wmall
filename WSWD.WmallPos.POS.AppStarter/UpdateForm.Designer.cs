namespace WSWD.WmallPos.POS.AppStarter
{
    partial class UpdateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateForm));
            this.label1 = new System.Windows.Forms.Label();
            this.pnlInternal = new System.Windows.Forms.Panel();
            this.txtLines = new System.Windows.Forms.TextBox();
            this.colorProgressBar1 = new WSWD.WmallPos.POS.AppStarter.Controls.ColorProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlInternal.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(83)))), ((int)(((byte)(180)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.label1.Size = new System.Drawing.Size(378, 48);
            this.label1.TabIndex = 13;
            this.label1.Text = "W-Mall POS 자동 업데이트";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlInternal
            // 
            this.pnlInternal.Controls.Add(this.txtLines);
            this.pnlInternal.Controls.Add(this.colorProgressBar1);
            this.pnlInternal.Controls.Add(this.lblStatus);
            this.pnlInternal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInternal.Location = new System.Drawing.Point(0, 48);
            this.pnlInternal.Name = "pnlInternal";
            this.pnlInternal.Padding = new System.Windows.Forms.Padding(10);
            this.pnlInternal.Size = new System.Drawing.Size(378, 197);
            this.pnlInternal.TabIndex = 15;
            // 
            // txtLines
            // 
            this.txtLines.Location = new System.Drawing.Point(10, 64);
            this.txtLines.Multiline = true;
            this.txtLines.Name = "txtLines";
            this.txtLines.ReadOnly = true;
            this.txtLines.Size = new System.Drawing.Size(358, 123);
            this.txtLines.TabIndex = 16;
            // 
            // colorProgressBar1
            // 
            this.colorProgressBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.colorProgressBar1.Location = new System.Drawing.Point(10, 33);
            this.colorProgressBar1.Name = "colorProgressBar1";
            this.colorProgressBar1.Percentage = 0;
            this.colorProgressBar1.Size = new System.Drawing.Size(358, 23);
            this.colorProgressBar1.TabIndex = 15;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStatus.Location = new System.Drawing.Point(10, 10);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(358, 23);
            this.lblStatus.TabIndex = 14;
            this.lblStatus.Text = "버전 확인 중입니다.";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UpdateForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(378, 245);
            this.ControlBox = false;
            this.Controls.Add(this.pnlInternal);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UpdateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Activated += new System.EventHandler(this.UpdateForm_Activated);
            this.pnlInternal.ResumeLayout(false);
            this.pnlInternal.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlInternal;
        private WSWD.WmallPos.POS.AppStarter.Controls.ColorProgressBar colorProgressBar1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtLines;



    }
}