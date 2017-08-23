namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    partial class OpenStatusItem
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
            this.StatusLabel = new WSWD.WmallPos.POS.FX.Win.UserControls.IconLabel();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // StatusLabel
            // 
            this.StatusLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.StatusLabel.Icon = null;
            this.StatusLabel.IconPosition = WSWD.WmallPos.POS.FX.Win.UserControls.IconLabelPosition.Right;
            this.StatusLabel.Location = new System.Drawing.Point(297, 0);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(86, 39);
            this.StatusLabel.TabIndex = 1;
            this.StatusLabel.Text = "ERROR";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MessageLabel
            // 
            this.MessageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessageLabel.Location = new System.Drawing.Point(0, 0);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(297, 39);
            this.MessageLabel.TabIndex = 2;
            this.MessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OpenStatusItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.MessageLabel);
            this.Controls.Add(this.StatusLabel);
            this.Name = "OpenStatusItem";
            this.Size = new System.Drawing.Size(383, 39);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label MessageLabel;
        private WSWD.WmallPos.POS.FX.Win.UserControls.IconLabel StatusLabel;

    }
}
