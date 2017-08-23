namespace WSWD.WmallPos.POS.SL.Controls
{
    partial class AutoRtnButtons
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
            this.btnConfirm = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.BorderSize = 1;
            this.btnConfirm.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnConfirm.Corner = 3;
            this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnConfirm.Enabled = false;
            this.btnConfirm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnConfirm.Image = null;
            this.btnConfirm.Location = new System.Drawing.Point(5, 5);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Selected = false;
            this.btnConfirm.Size = new System.Drawing.Size(90, 42);
            this.btnConfirm.TabIndex = 0;
            this.btnConfirm.Text = "확정";
            this.btnConfirm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.Location = new System.Drawing.Point(105, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AutoRtnButtons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(232)))), ((int)(((byte)(231)))));
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnConfirm);
            this.Name = "AutoRtnButtons";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(200, 52);
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnConfirm;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
    }
}
