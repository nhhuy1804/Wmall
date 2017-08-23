namespace WSWD.WmallPos.POS.PY.VC
{
    partial class POS_PY_P017
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
            this.txtPaymentAmt = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.txtGetAmt = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label1 = new System.Windows.Forms.Label();
            this.ButtonsPanel.SuspendLayout();
            this.ContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.btnSave);
            this.ButtonsPanel.Controls.Add(this.btnClose);
            this.ButtonsPanel.Size = new System.Drawing.Size(525, 62);
            // 
            // MessageBar
            // 
            this.MessageBar.Location = new System.Drawing.Point(17, 261);
            this.MessageBar.Size = new System.Drawing.Size(525, 46);
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.txtGetAmt);
            this.ContainerPanel.Controls.Add(this.label1);
            this.ContainerPanel.Controls.Add(this.txtPaymentAmt);
            this.ContainerPanel.Controls.Add(this.label3);
            this.ContainerPanel.Controls.Add(this.keyPad);
            this.ContainerPanel.Size = new System.Drawing.Size(559, 386);
            this.ContainerPanel.Controls.SetChildIndex(this.MessageBar, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.keyPad, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.label3, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.txtPaymentAmt, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.label1, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.txtGetAmt, 0);
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
            this.keyPad.Location = new System.Drawing.Point(297, 17);
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
            this.btnClose.Location = new System.Drawing.Point(265, 20);
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
            this.btnSave.Location = new System.Drawing.Point(159, 20);
            this.btnSave.Name = "btnSave";
            this.btnSave.Selected = false;
            this.btnSave.Size = new System.Drawing.Size(90, 42);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "확인";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 28);
            this.label3.TabIndex = 21;
            this.label3.Text = "결제금액";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPaymentAmt
            // 
            this.txtPaymentAmt.BackColor = System.Drawing.Color.White;
            this.txtPaymentAmt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(109)))), ((int)(((byte)(195)))));
            this.txtPaymentAmt.BorderWidth = 2;
            this.txtPaymentAmt.Corner = 1;
            this.txtPaymentAmt.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtPaymentAmt.Focusable = true;
            this.txtPaymentAmt.FocusedIndex = 0;
            this.txtPaymentAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtPaymentAmt.Format = "#,##0";
            this.txtPaymentAmt.HasBorder = true;
            this.txtPaymentAmt.IsFocused = true;
            this.txtPaymentAmt.Location = new System.Drawing.Point(105, 53);
            this.txtPaymentAmt.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtPaymentAmt.MaxLength = 9;
            this.txtPaymentAmt.Name = "txtPaymentAmt";
            this.txtPaymentAmt.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.txtPaymentAmt.PasswordMode = false;
            this.txtPaymentAmt.ReadOnly = false;
            this.txtPaymentAmt.Size = new System.Drawing.Size(178, 28);
            this.txtPaymentAmt.TabIndex = 29;
            this.txtPaymentAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.txtGetAmt.Size = new System.Drawing.Size(178, 28);
            this.txtGetAmt.TabIndex = 31;
            this.txtGetAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 28);
            this.label1.TabIndex = 30;
            this.label1.Text = "받을돈";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // POS_PY_P017
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(565, 432);
            this.Name = "POS_PY_P017";
            this.Text = "타건카드";
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
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtPaymentAmt;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtGetAmt;
        private System.Windows.Forms.Label label1;
    }
}