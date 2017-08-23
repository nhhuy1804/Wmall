namespace WSWD.WmallPos.POS.ST.VC
{
    partial class POS_ST_M002
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_ST_M002));
            this.label1 = new WSWD.WmallPos.POS.FX.Win.UserControls.SectionLabel();
            this.grd = new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Icon = ((System.Drawing.Image)(resources.GetObject("label1.Icon")));
            this.label1.IconPosition = WSWD.WmallPos.POS.FX.Win.UserControls.IconLabelPosition.Left;
            this.label1.Location = new System.Drawing.Point(62, 360);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(519, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "공지사항";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grd
            // 
            this.grd.BackColor = System.Drawing.Color.White;
            this.grd.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.grd.BorderWidth = new System.Windows.Forms.Padding(1);
            this.grd.CurrentRowIndex = -1;
            this.grd.Location = new System.Drawing.Point(62, 387);
            this.grd.Name = "grd";
            this.grd.Padding = new System.Windows.Forms.Padding(1);
            this.grd.PageIndex = 0;
            this.grd.ScrollType = WSWD.WmallPos.POS.FX.Win.UserControls.GridScrollType.Row;
            this.grd.Size = new System.Drawing.Size(623, 256);
            this.grd.TabIndex = 13;
            // 
            // POS_ST_M002
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::WSWD.WmallPos.POS.ST.Properties.Resources.bg_02_f;
            this.Controls.Add(this.grd);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.IsModal = true;
            this.Name = "POS_ST_M002";
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.UserControls.SectionLabel label1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel grd;


    }
}
