namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    partial class PopupBase03
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
            this.ButtonsPanel = new System.Windows.Forms.Panel();
            this.ContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.ButtonsPanel);
            this.ContainerPanel.Padding = new System.Windows.Forms.Padding(17);
            this.ContainerPanel.Size = new System.Drawing.Size(670, 438);
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ButtonsPanel.Location = new System.Drawing.Point(17, 369);
            this.ButtonsPanel.Margin = new System.Windows.Forms.Padding(4);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Size = new System.Drawing.Size(636, 52);
            this.ButtonsPanel.TabIndex = 5;
            // 
            // PopupBase03
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(676, 484);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PopupBase03";
            this.Text = "PopupBase03";
            this.ContainerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel ButtonsPanel;


    }
}