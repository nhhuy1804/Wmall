namespace WSWD.WmallPos.POS.SL.Controls
{
    partial class CurrentItemInfo
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
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblFormular = new System.Windows.Forms.Label();
            this.lblItem = new System.Windows.Forms.Label();
            this.pnlBg = new System.Windows.Forms.Panel();
            this.pnlBg.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTotal
            // 
            this.lblTotal.BackColor = System.Drawing.Color.Transparent;
            this.lblTotal.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTotal.Font = new System.Drawing.Font("Dotum", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(115)))), ((int)(((byte)(114)))));
            this.lblTotal.Location = new System.Drawing.Point(530, 0);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(3);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(56, 55);
            this.lblTotal.TabIndex = 3;
            this.lblTotal.Text = "100";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFormular
            // 
            this.lblFormular.BackColor = System.Drawing.Color.Transparent;
            this.lblFormular.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblFormular.Font = new System.Drawing.Font("Dotum", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormular.ForeColor = System.Drawing.Color.Black;
            this.lblFormular.Location = new System.Drawing.Point(360, 0);
            this.lblFormular.Margin = new System.Windows.Forms.Padding(3);
            this.lblFormular.Name = "lblFormular";
            this.lblFormular.Size = new System.Drawing.Size(170, 55);
            this.lblFormular.TabIndex = 4;
            this.lblFormular.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblItem
            // 
            this.lblItem.AutoEllipsis = true;
            this.lblItem.BackColor = System.Drawing.Color.Transparent;
            this.lblItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblItem.Font = new System.Drawing.Font("Dotum", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(239)))), ((int)(((byte)(170)))));
            this.lblItem.Location = new System.Drawing.Point(0, 0);
            this.lblItem.Margin = new System.Windows.Forms.Padding(3);
            this.lblItem.Name = "lblItem";
            this.lblItem.Size = new System.Drawing.Size(360, 55);
            this.lblItem.TabIndex = 5;
            this.lblItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBg
            // 
            this.pnlBg.BackgroundImage = global::WSWD.WmallPos.POS.SL.Properties.Resources.bg_pos_top_item_status;
            this.pnlBg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlBg.Controls.Add(this.lblItem);
            this.pnlBg.Controls.Add(this.lblFormular);
            this.pnlBg.Controls.Add(this.lblTotal);
            this.pnlBg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBg.Location = new System.Drawing.Point(0, 0);
            this.pnlBg.Name = "pnlBg";
            this.pnlBg.Size = new System.Drawing.Size(586, 55);
            this.pnlBg.TabIndex = 6;
            // 
            // CurrentItemInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.pnlBg);
            this.Font = new System.Drawing.Font("Dotum", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CurrentItemInfo";
            this.Size = new System.Drawing.Size(586, 55);
            this.pnlBg.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblFormular;
        private System.Windows.Forms.Label lblItem;
        private System.Windows.Forms.Panel pnlBg;
    }
}
