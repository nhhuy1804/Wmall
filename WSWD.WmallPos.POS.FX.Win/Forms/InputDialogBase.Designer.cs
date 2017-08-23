namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    partial class InputDialogBase
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
            this.pnlFormBorder = new WSWD.WmallPos.POS.FX.Win.Controls.BorderPanel();
            this.ContainerPanel = new System.Windows.Forms.Panel();
            this.lblPopupTitle = new System.Windows.Forms.Label();
            this.pnlFormBorder.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFormBorder
            // 
            this.pnlFormBorder.BackColor = System.Drawing.Color.White;
            this.pnlFormBorder.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(39)))), ((int)(((byte)(111)))));
            this.pnlFormBorder.BorderWidth = new System.Windows.Forms.Padding(3);
            this.pnlFormBorder.Controls.Add(this.ContainerPanel);
            this.pnlFormBorder.Controls.Add(this.lblPopupTitle);
            this.pnlFormBorder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFormBorder.Location = new System.Drawing.Point(0, 0);
            this.pnlFormBorder.Margin = new System.Windows.Forms.Padding(0);
            this.pnlFormBorder.Name = "pnlFormBorder";
            this.pnlFormBorder.Padding = new System.Windows.Forms.Padding(3);
            this.pnlFormBorder.Size = new System.Drawing.Size(451, 399);
            this.pnlFormBorder.TabIndex = 0;
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.BackColor = System.Drawing.Color.White;
            this.ContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContainerPanel.Location = new System.Drawing.Point(3, 43);
            this.ContainerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ContainerPanel.Name = "ContainerPanel";
            this.ContainerPanel.Padding = new System.Windows.Forms.Padding(5);
            this.ContainerPanel.Size = new System.Drawing.Size(445, 353);
            this.ContainerPanel.TabIndex = 7;
            // 
            // lblPopupTitle
            // 
            this.lblPopupTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(57)))), ((int)(((byte)(178)))));
            this.lblPopupTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPopupTitle.Font = new System.Drawing.Font("Dotum", 12F, System.Drawing.FontStyle.Bold);
            this.lblPopupTitle.ForeColor = System.Drawing.Color.White;
            this.lblPopupTitle.Location = new System.Drawing.Point(3, 3);
            this.lblPopupTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblPopupTitle.Name = "lblPopupTitle";
            this.lblPopupTitle.Size = new System.Drawing.Size(445, 40);
            this.lblPopupTitle.TabIndex = 6;
            this.lblPopupTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InputDialogBase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(451, 399);
            this.ControlBox = false;
            this.Controls.Add(this.pnlFormBorder);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputDialogBase";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.pnlFormBorder.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.Controls.BorderPanel pnlFormBorder;
        private System.Windows.Forms.Label lblPopupTitle;
        protected System.Windows.Forms.Panel ContainerPanel;
    }
}