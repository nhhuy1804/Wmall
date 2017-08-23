namespace WSWD.WmallPos.POS.PY.VC
{
    partial class POS_PY_P002
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_PY_P002));
            this.roundedButton2 = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.btnSave = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.borderPanel1 = new WSWD.WmallPos.POS.FX.Win.Controls.BorderPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAmt = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.axKSNet_Dongle1 = new AxKSNET_DONGLELib.AxKSNet_Dongle();
            this.ButtonsPanel.SuspendLayout();
            this.ContainerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axKSNet_Dongle1)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.btnSave);
            this.ButtonsPanel.Controls.Add(this.btnClose);
            this.ButtonsPanel.Size = new System.Drawing.Size(348, 62);
            // 
            // MessageBar
            // 
            this.MessageBar.Location = new System.Drawing.Point(17, 226);
            this.MessageBar.Size = new System.Drawing.Size(348, 46);
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.axKSNet_Dongle1);
            this.ContainerPanel.Controls.Add(this.txtAmt);
            this.ContainerPanel.Controls.Add(this.label1);
            this.ContainerPanel.Controls.Add(this.borderPanel1);
            this.ContainerPanel.Size = new System.Drawing.Size(382, 351);
            this.ContainerPanel.Controls.SetChildIndex(this.MessageBar, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.borderPanel1, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.label1, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.txtAmt, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.axKSNet_Dongle1, 0);
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
            // btnSave
            // 
            this.btnSave.BorderSize = 1;
            this.btnSave.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnSave.Corner = 3;
            this.btnSave.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnSave.Image = null;
            this.btnSave.IsHighlight = false;
            this.btnSave.Location = new System.Drawing.Point(78, 20);
            this.btnSave.Name = "btnSave";
            this.btnSave.Selected = false;
            this.btnSave.Size = new System.Drawing.Size(90, 42);
            this.btnSave.TabIndex = 40;
            this.btnSave.Text = "확인";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.IsHighlight = false;
            this.btnClose.Location = new System.Drawing.Point(181, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 39;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // borderPanel1
            // 
            this.borderPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.borderPanel1.BorderWidth = new System.Windows.Forms.Padding(1);
            this.borderPanel1.Location = new System.Drawing.Point(17, 17);
            this.borderPanel1.Name = "borderPanel1";
            this.borderPanel1.Size = new System.Drawing.Size(348, 153);
            this.borderPanel1.TabIndex = 41;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 182);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 32);
            this.label1.TabIndex = 42;
            this.label1.Text = "결제금액";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtAmt
            // 
            this.txtAmt.BackColor = System.Drawing.Color.White;
            this.txtAmt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtAmt.BorderWidth = 1;
            this.txtAmt.Corner = 1;
            this.txtAmt.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtAmt.Focusable = true;
            this.txtAmt.FocusedIndex = 0;
            this.txtAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtAmt.Format = "#,##0";
            this.txtAmt.HasBorder = true;
            this.txtAmt.IsFocused = false;
            this.txtAmt.Location = new System.Drawing.Point(104, 182);
            this.txtAmt.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtAmt.Name = "txtAmt";
            this.txtAmt.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.txtAmt.PasswordMode = false;
            this.txtAmt.ReadOnly = false;
            this.txtAmt.Size = new System.Drawing.Size(174, 32);
            this.txtAmt.TabIndex = 43;
            this.txtAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // axKSNet_Dongle1
            // 
            this.axKSNet_Dongle1.Enabled = true;
            this.axKSNet_Dongle1.Location = new System.Drawing.Point(20, 20);
            this.axKSNet_Dongle1.Name = "axKSNet_Dongle1";
            this.axKSNet_Dongle1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKSNet_Dongle1.OcxState")));
            this.axKSNet_Dongle1.Size = new System.Drawing.Size(341, 146);
            this.axKSNet_Dongle1.TabIndex = 44;
            // 
            // POS_PY_P002
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(388, 397);
            this.Name = "POS_PY_P002";
            this.Text = "전자서명";
            this.ButtonsPanel.ResumeLayout(false);
            this.ContainerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axKSNet_Dongle1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton roundedButton2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnSave;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private WSWD.WmallPos.POS.FX.Win.Controls.BorderPanel borderPanel1;
        private System.Windows.Forms.Label label1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtAmt;
        private AxKSNET_DONGLELib.AxKSNet_Dongle axKSNet_Dongle1;
    }
}