namespace WSWD.WmallPos.POS.SL.Controls
{
    partial class AutoRtnPanel
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
            this.autoRtnProgress1 = new WSWD.WmallPos.POS.SL.Controls.AutoRtnProgress();
            this.autoRtnTrxnInfo1 = new WSWD.WmallPos.POS.SL.Controls.AutoRtnTrxnInfo();
            this.autoRtnButtons1 = new WSWD.WmallPos.POS.SL.Controls.AutoRtnButtons();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // autoRtnProgress1
            // 
            this.autoRtnProgress1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(180)))), ((int)(((byte)(176)))));
            this.autoRtnProgress1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.autoRtnProgress1.Location = new System.Drawing.Point(8, 387);
            this.autoRtnProgress1.Name = "autoRtnProgress1";
            this.autoRtnProgress1.Padding = new System.Windows.Forms.Padding(12);
            this.autoRtnProgress1.Size = new System.Drawing.Size(413, 240);
            this.autoRtnProgress1.TabIndex = 6;
            // 
            // autoRtnTrxnInfo1
            // 
            this.autoRtnTrxnInfo1.BackColor = System.Drawing.Color.White;
            this.autoRtnTrxnInfo1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.autoRtnTrxnInfo1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoRtnTrxnInfo1.Font = new System.Drawing.Font("Dotum", 11F);
            this.autoRtnTrxnInfo1.Location = new System.Drawing.Point(8, 8);
            this.autoRtnTrxnInfo1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.autoRtnTrxnInfo1.Name = "autoRtnTrxnInfo1";
            this.autoRtnTrxnInfo1.Size = new System.Drawing.Size(413, 379);
            this.autoRtnTrxnInfo1.TabIndex = 4;
            // 
            // autoRtnButtons1
            // 
            this.autoRtnButtons1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(232)))), ((int)(((byte)(231)))));
            this.autoRtnButtons1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.autoRtnButtons1.Location = new System.Drawing.Point(0, 635);
            this.autoRtnButtons1.Name = "autoRtnButtons1";
            this.autoRtnButtons1.Padding = new System.Windows.Forms.Padding(8);
            this.autoRtnButtons1.Size = new System.Drawing.Size(429, 58);
            this.autoRtnButtons1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(210)))), ((int)(((byte)(211)))));
            this.panel1.Controls.Add(this.autoRtnTrxnInfo1);
            this.panel1.Controls.Add(this.autoRtnProgress1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(8);
            this.panel1.Size = new System.Drawing.Size(429, 635);
            this.panel1.TabIndex = 7;
            // 
            // AutoRtnPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.autoRtnButtons1);
            this.Name = "AutoRtnPanel";
            this.Size = new System.Drawing.Size(429, 693);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AutoRtnButtons autoRtnButtons1;
        private AutoRtnTrxnInfo autoRtnTrxnInfo1;
        private AutoRtnProgress autoRtnProgress1;
        private System.Windows.Forms.Panel panel1;
    }
}
