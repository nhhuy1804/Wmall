namespace WSWD.WmallPos.POS.SL.Controls
{
    partial class AutoRtnTrxnInfo
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
            this.pnlUpDn = new System.Windows.Forms.Panel();
            this.btnDown = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.btnUp = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.pnlBound = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.lblText = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlUpDn.SuspendLayout();
            this.pnlBound.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlUpDn
            // 
            this.pnlUpDn.Controls.Add(this.tableLayoutPanel1);
            this.pnlUpDn.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlUpDn.Location = new System.Drawing.Point(380, 0);
            this.pnlUpDn.Name = "pnlUpDn";
            this.pnlUpDn.Size = new System.Drawing.Size(33, 379);
            this.pnlUpDn.TabIndex = 2;
            // 
            // btnDown
            // 
            this.btnDown.BorderSize = 1;
            this.btnDown.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type04;
            this.btnDown.Corner = 0;
            this.btnDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnDown.Image = global::WSWD.WmallPos.POS.SL.Properties.Resources.ico_list_dn;
            this.btnDown.IsHighlight = false;
            this.btnDown.Location = new System.Drawing.Point(0, 189);
            this.btnDown.Margin = new System.Windows.Forms.Padding(0);
            this.btnDown.Name = "btnDown";
            this.btnDown.Padding = new System.Windows.Forms.Padding(1);
            this.btnDown.Selected = false;
            this.btnDown.Size = new System.Drawing.Size(33, 190);
            this.btnDown.TabIndex = 0;
            this.btnDown.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnUp
            // 
            this.btnUp.BorderSize = 1;
            this.btnUp.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type04;
            this.btnUp.Corner = 0;
            this.btnUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnUp.Image = global::WSWD.WmallPos.POS.SL.Properties.Resources.ico_list_up;
            this.btnUp.IsHighlight = false;
            this.btnUp.Location = new System.Drawing.Point(0, 0);
            this.btnUp.Margin = new System.Windows.Forms.Padding(0);
            this.btnUp.Name = "btnUp";
            this.btnUp.Selected = false;
            this.btnUp.Size = new System.Drawing.Size(33, 189);
            this.btnUp.TabIndex = 0;
            this.btnUp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBound
            // 
            this.pnlBound.Controls.Add(this.pnlContent);
            this.pnlBound.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBound.Location = new System.Drawing.Point(0, 0);
            this.pnlBound.Name = "pnlBound";
            this.pnlBound.Padding = new System.Windows.Forms.Padding(8);
            this.pnlBound.Size = new System.Drawing.Size(380, 379);
            this.pnlBound.TabIndex = 4;
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.lblText);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(8, 8);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(364, 363);
            this.pnlContent.TabIndex = 5;
            // 
            // lblText
            // 
            this.lblText.Location = new System.Drawing.Point(0, 0);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(364, 360);
            this.lblText.TabIndex = 5;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnDown, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnUp, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(33, 379);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // AutoRtnTrxnInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlBound);
            this.Controls.Add(this.pnlUpDn);
            this.Name = "AutoRtnTrxnInfo";
            this.Size = new System.Drawing.Size(413, 379);
            this.pnlUpDn.ResumeLayout(false);
            this.pnlBound.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlUpDn;
        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton btnDown;
        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton btnUp;
        private System.Windows.Forms.Panel pnlBound;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

    }
}
