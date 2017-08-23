namespace WSWD.WmallPos.POS.SL.Controls
{
    partial class SalePayItem
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
            this.lblPayAmt = new System.Windows.Forms.Label();
            this.lblPayItemName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.lblPayAmt.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblPayAmt.Location = new System.Drawing.Point(72, 0);
            this.lblPayAmt.Name = "label2";
            this.lblPayAmt.Size = new System.Drawing.Size(144, 42);
            this.lblPayAmt.TabIndex = 1;
            this.lblPayAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.lblPayItemName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPayItemName.Location = new System.Drawing.Point(0, 0);
            this.lblPayItemName.Name = "label1";
            this.lblPayItemName.Size = new System.Drawing.Size(72, 42);
            this.lblPayItemName.TabIndex = 2;
            this.lblPayItemName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SalePayItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.lblPayItemName);
            this.Controls.Add(this.lblPayAmt);
            this.Name = "SalePayItem";
            this.Size = new System.Drawing.Size(216, 42);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblPayAmt;
        private System.Windows.Forms.Label lblPayItemName;
    }
}
