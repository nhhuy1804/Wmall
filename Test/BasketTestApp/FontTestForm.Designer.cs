namespace BasketTestApp
{
    partial class FontTestForm
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
            this.ContainerPanel = new System.Windows.Forms.Panel();
            this.pnlFormBorder = new WSWD.WmallPos.POS.FX.Win.Controls.BorderPanel();
            this.lblPopupTitle = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.ContainerPanel.SuspendLayout();
            this.pnlFormBorder.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.BackColor = System.Drawing.Color.Transparent;
            this.ContainerPanel.Controls.Add(this.button1);
            this.ContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContainerPanel.Location = new System.Drawing.Point(3, 43);
            this.ContainerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ContainerPanel.Name = "ContainerPanel";
            this.ContainerPanel.Padding = new System.Windows.Forms.Padding(5);
            this.ContainerPanel.Size = new System.Drawing.Size(539, 289);
            this.ContainerPanel.TabIndex = 7;
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
            this.pnlFormBorder.Size = new System.Drawing.Size(545, 335);
            this.pnlFormBorder.TabIndex = 1;
            // 
            // lblPopupTitle
            // 
            this.lblPopupTitle.BackColor = System.Drawing.Color.Navy;
            this.lblPopupTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPopupTitle.Location = new System.Drawing.Point(3, 3);
            this.lblPopupTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblPopupTitle.Name = "lblPopupTitle";
            this.lblPopupTitle.Size = new System.Drawing.Size(539, 40);
            this.lblPopupTitle.TabIndex = 6;
            this.lblPopupTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(284, 66);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 33);
            this.button1.TabIndex = 0;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FontTestForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(545, 335);
            this.ControlBox = false;
            this.Controls.Add(this.pnlFormBorder);
            this.Font = new System.Drawing.Font("NanumBarunGothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FontTestForm";
            this.ContainerPanel.ResumeLayout(false);
            this.pnlFormBorder.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Panel ContainerPanel;
        private WSWD.WmallPos.POS.FX.Win.Controls.BorderPanel pnlFormBorder;
        private System.Windows.Forms.Label lblPopupTitle;
        private System.Windows.Forms.Button button1;

    }
}