namespace WSWD.WmallPos.POS.BO.VC
{
    partial class MdiFormBase
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
            this.mainMenu1 = new WSWD.WmallPos.POS.BO.VC.MainMenuV2();
            this.bpnlContainer = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.BackColor = System.Drawing.Color.Transparent;
            this.mainMenu1.BackgroundImage = global::WSWD.WmallPos.POS.BO.Properties.Resources.bg_04;
            this.mainMenu1.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mainMenu1.Location = new System.Drawing.Point(736, 48);
            this.mainMenu1.Name = "mainMenu1";
            this.mainMenu1.Padding = new System.Windows.Forms.Padding(44, 50, 44, 0);
            this.mainMenu1.Size = new System.Drawing.Size(288, 692);
            this.mainMenu1.TabIndex = 0;
            // 
            // bpnlContainer
            // 
            this.bpnlContainer.Location = new System.Drawing.Point(0, 48);
            this.bpnlContainer.Name = "bpnlContainer";
            this.bpnlContainer.Size = new System.Drawing.Size(736, 692);
            this.bpnlContainer.TabIndex = 2;
            // 
            // MdiFormBase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.bpnlContainer);
            this.Controls.Add(this.mainMenu1);
            this.Name = "MdiFormBase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.Controls.SetChildIndex(this.mainMenu1, 0);
            this.Controls.SetChildIndex(this.bpnlContainer, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WSWD.WmallPos.POS.BO.VC.MainMenuV2 mainMenu1;
        private System.Windows.Forms.Panel bpnlContainer;
    }
}

