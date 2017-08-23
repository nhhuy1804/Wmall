namespace WSWD.WmallPos.POS.PY.VC
{
    partial class POS_PY_P011
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
            this.roundedButton2 = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.keyPad = new WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnSave = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGetAmt = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.txtDiscAmt = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label1 = new System.Windows.Forms.Label();
            this.ButtonsPanel.SuspendLayout();
            this.ContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.btnSave);
            this.ButtonsPanel.Controls.Add(this.btnClose);
            this.ButtonsPanel.Size = new System.Drawing.Size(533, 62);
            // 
            // MessageBar
            // 
            this.MessageBar.Location = new System.Drawing.Point(17, 263);
            this.MessageBar.Size = new System.Drawing.Size(533, 46);
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.txtDiscAmt);
            this.ContainerPanel.Controls.Add(this.txtGetAmt);
            this.ContainerPanel.Controls.Add(this.label1);
            this.ContainerPanel.Controls.Add(this.label3);
            this.ContainerPanel.Controls.Add(this.keyPad);
            this.ContainerPanel.Size = new System.Drawing.Size(567, 388);
            this.ContainerPanel.Controls.SetChildIndex(this.MessageBar, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.keyPad, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.label3, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.label1, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.txtGetAmt, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.txtDiscAmt, 0);
            // 
            // roundedButton2
            // 
            this.roundedButton2.BorderSize = 1;
            this.roundedButton2.Corner = 3;
            this.roundedButton2.Dock = System.Windows.Forms.DockStyle.Right;
            this.roundedButton2.Image = null;
            this.roundedButton2.IsHighlight = false;
            this.roundedButton2.Location = new System.Drawing.Point(99, 0);
            this.roundedButton2.Name = "roundedButton2";
            this.roundedButton2.Selected = false;
            this.roundedButton2.Size = new System.Drawing.Size(90, 42);
            this.roundedButton2.TabIndex = 5;
            this.roundedButton2.Text = "닫기";
            this.roundedButton2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // keyPad
            // 
            this.keyPad.Location = new System.Drawing.Point(305, 17);
            this.keyPad.Margin = new System.Windows.Forms.Padding(0);
            this.keyPad.MaximumSize = new System.Drawing.Size(245, 233);
            this.keyPad.Name = "keyPad";
            this.keyPad.Size = new System.Drawing.Size(245, 233);
            this.keyPad.TabIndex = 9;
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.IsHighlight = false;
            this.btnClose.Location = new System.Drawing.Point(274, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            this.btnSave.BorderSize = 1;
            this.btnSave.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnSave.Corner = 3;
            this.btnSave.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnSave.Image = null;
            this.btnSave.IsHighlight = false;
            this.btnSave.Location = new System.Drawing.Point(169, 20);
            this.btnSave.Name = "btnSave";
            this.btnSave.Selected = false;
            this.btnSave.Size = new System.Drawing.Size(90, 42);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "확인";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 28);
            this.label3.TabIndex = 21;
            this.label3.Text = "받을돈";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtGetAmt
            // 
            this.txtGetAmt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtGetAmt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtGetAmt.BorderWidth = 1;
            this.txtGetAmt.Corner = 1;
            this.txtGetAmt.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtGetAmt.Focusable = false;
            this.txtGetAmt.FocusedIndex = 0;
            this.txtGetAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtGetAmt.Format = "#,##0";
            this.txtGetAmt.HasBorder = true;
            this.txtGetAmt.IsFocused = false;
            this.txtGetAmt.Location = new System.Drawing.Point(105, 17);
            this.txtGetAmt.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtGetAmt.MaxLength = 9;
            this.txtGetAmt.Name = "txtGetAmt";
            this.txtGetAmt.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.txtGetAmt.PasswordMode = false;
            this.txtGetAmt.ReadOnly = true;
            this.txtGetAmt.Size = new System.Drawing.Size(186, 28);
            this.txtGetAmt.TabIndex = 29;
            this.txtGetAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDiscAmt
            // 
            this.txtDiscAmt.BackColor = System.Drawing.Color.White;
            this.txtDiscAmt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(109)))), ((int)(((byte)(195)))));
            this.txtDiscAmt.BorderWidth = 2;
            this.txtDiscAmt.Corner = 1;
            this.txtDiscAmt.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtDiscAmt.Focusable = true;
            this.txtDiscAmt.FocusedIndex = 0;
            this.txtDiscAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtDiscAmt.Format = "#,##0";
            this.txtDiscAmt.HasBorder = true;
            this.txtDiscAmt.IsFocused = true;
            this.txtDiscAmt.Location = new System.Drawing.Point(105, 53);
            this.txtDiscAmt.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtDiscAmt.MaxLength = 9;
            this.txtDiscAmt.Name = "txtDiscAmt";
            this.txtDiscAmt.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.txtDiscAmt.PasswordMode = false;
            this.txtDiscAmt.ReadOnly = false;
            this.txtDiscAmt.Size = new System.Drawing.Size(186, 28);
            this.txtDiscAmt.TabIndex = 31;
            this.txtDiscAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 28);
            this.label1.TabIndex = 30;
            this.label1.Text = "할인금액";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // POS_PY_P011
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(573, 434);
            this.Name = "POS_PY_P011";
            this.Text = "결제할인";
            this.ButtonsPanel.ResumeLayout(false);
            this.ContainerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton roundedButton2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad keyPad;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnSave;
        private System.Windows.Forms.Label label3;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtGetAmt;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtDiscAmt;
        private System.Windows.Forms.Label label1;
    }
}