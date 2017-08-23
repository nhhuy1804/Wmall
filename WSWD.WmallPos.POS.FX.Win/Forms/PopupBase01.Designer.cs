namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    partial class PopupBase01
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
            this.pnlButtonBottom = new System.Windows.Forms.Panel();
            this.ButtonsPanel = new System.Windows.Forms.Panel();
            this.MessageBar = new WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar();
            this.ContainerPanel.SuspendLayout();
            this.pnlButtonBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.MessageBar);
            this.ContainerPanel.Controls.Add(this.pnlButtonBottom);
            this.ContainerPanel.Padding = new System.Windows.Forms.Padding(17);
            this.ContainerPanel.Size = new System.Drawing.Size(639, 397);
            // 
            // pnlButtonBottom
            // 
            this.pnlButtonBottom.Controls.Add(this.ButtonsPanel);
            this.pnlButtonBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtonBottom.Location = new System.Drawing.Point(17, 318);
            this.pnlButtonBottom.Margin = new System.Windows.Forms.Padding(4);
            this.pnlButtonBottom.Name = "pnlButtonBottom";
            this.pnlButtonBottom.Size = new System.Drawing.Size(605, 62);
            this.pnlButtonBottom.TabIndex = 7;
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonsPanel.Location = new System.Drawing.Point(0, 0);
            this.ButtonsPanel.Margin = new System.Windows.Forms.Padding(4);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Size = new System.Drawing.Size(605, 62);
            this.ButtonsPanel.TabIndex = 4;
            // 
            // MessageBar
            // 
            this.MessageBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.MessageBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.MessageBar.BorderWidth = new System.Windows.Forms.Padding(1);
            this.MessageBar.ControlType = WSWD.WmallPos.POS.FX.Win.UserControls.MessageBarType.TypeStatus;
            this.MessageBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.MessageBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.MessageBar.Location = new System.Drawing.Point(17, 272);
            this.MessageBar.Margin = new System.Windows.Forms.Padding(4);
            this.MessageBar.MinimumSize = new System.Drawing.Size(0, 35);
            this.MessageBar.Name = "MessageBar";
            this.MessageBar.Size = new System.Drawing.Size(605, 46);
            this.MessageBar.TabIndex = 8;
            this.MessageBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PopupBase01
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(645, 443);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PopupBase01";
            this.Text = "PopupBase01";
            this.ContainerPanel.ResumeLayout(false);
            this.pnlButtonBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlButtonBottom;
        public System.Windows.Forms.Panel ButtonsPanel;
        protected WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar MessageBar;

    }
}