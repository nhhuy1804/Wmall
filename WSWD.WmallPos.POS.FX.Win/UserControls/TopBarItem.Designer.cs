namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    partial class TopBarItem
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
            this.DataLabel = new System.Windows.Forms.Label();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // DataLabel
            // 
            this.DataLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataLabel.Location = new System.Drawing.Point(91, 0);
            this.DataLabel.Margin = new System.Windows.Forms.Padding(3);
            this.DataLabel.Name = "DataLabel";
            this.DataLabel.Size = new System.Drawing.Size(164, 44);
            this.DataLabel.TabIndex = 1;
            this.DataLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TitleLabel
            // 
            this.TitleLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.TitleLabel.Location = new System.Drawing.Point(0, 0);
            this.TitleLabel.Margin = new System.Windows.Forms.Padding(3);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(91, 44);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TopBarItem
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.DataLabel);
            this.Controls.Add(this.TitleLabel);
            this.Name = "TopBarItem";
            this.Size = new System.Drawing.Size(255, 44);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label TitleLabel;
        public System.Windows.Forms.Label DataLabel;
    }
}
