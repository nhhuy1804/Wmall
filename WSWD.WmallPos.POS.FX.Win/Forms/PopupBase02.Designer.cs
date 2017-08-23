namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    partial class PopupBase02
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
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.ButtonsPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.MessageBar = new WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar();
            this.ContainerPanel.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.BottomPanel);
            this.ContainerPanel.Padding = new System.Windows.Forms.Padding(17);
            this.ContainerPanel.Size = new System.Drawing.Size(656, 389);
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.ButtonsPanel);
            this.BottomPanel.Controls.Add(this.label1);
            this.BottomPanel.Controls.Add(this.MessageBar);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(17, 316);
            this.BottomPanel.Margin = new System.Windows.Forms.Padding(4);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.BottomPanel.Size = new System.Drawing.Size(622, 56);
            this.BottomPanel.TabIndex = 4;
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonsPanel.Location = new System.Drawing.Point(272, 10);
            this.ButtonsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Size = new System.Drawing.Size(350, 46);
            this.ButtonsPanel.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(267, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(5, 46);
            this.label1.TabIndex = 12;
            // 
            // MessageBar
            // 
            this.MessageBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.MessageBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.MessageBar.BorderWidth = new System.Windows.Forms.Padding(1);
            this.MessageBar.ControlType = WSWD.WmallPos.POS.FX.Win.UserControls.MessageBarType.TypeStatus;
            this.MessageBar.Dock = System.Windows.Forms.DockStyle.Left;
            this.MessageBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MessageBar.Location = new System.Drawing.Point(0, 10);
            this.MessageBar.Margin = new System.Windows.Forms.Padding(4);
            this.MessageBar.MinimumSize = new System.Drawing.Size(0, 46);
            this.MessageBar.Name = "MessageBar";
            this.MessageBar.Size = new System.Drawing.Size(267, 46);
            this.MessageBar.TabIndex = 11;
            this.MessageBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PopupBase02
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(662, 435);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PopupBase02";
            this.Text = "PopupBase02";
            this.ContainerPanel.ResumeLayout(false);
            this.BottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BottomPanel;
        public System.Windows.Forms.Panel ButtonsPanel;
        private System.Windows.Forms.Label label1;
        protected WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar MessageBar;

    }
}