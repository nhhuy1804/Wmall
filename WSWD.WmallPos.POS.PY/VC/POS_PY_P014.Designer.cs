namespace WSWD.WmallPos.POS.PY.VC
{
    partial class POS_PY_P014
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_PY_P014));
            this.keyPad = new WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.messageBar1 = new WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.txtConfirmNo = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.btnEvidence = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnDeduction = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.txtBusinessNo = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.txtType = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.txtAmt = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.roundedButton2 = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.btnSelf = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.axKSNet_Dongle1 = new AxKSNET_DONGLELib.AxKSNet_Dongle();
            this.btnRdICCard = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnRdConfirmNo = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.ButtonsPanel.SuspendLayout();
            this.ContainerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axKSNet_Dongle1)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.axKSNet_Dongle1);
            this.ButtonsPanel.Controls.Add(this.btnSelf);
            this.ButtonsPanel.Controls.Add(this.btnClose);
            this.ButtonsPanel.Size = new System.Drawing.Size(570, 62);
            // 
            // MessageBar
            // 
            this.MessageBar.ControlType = WSWD.WmallPos.POS.FX.Win.UserControls.MessageBarType.TypeMessage;
            this.MessageBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this.MessageBar.Location = new System.Drawing.Point(17, 337);
            this.MessageBar.Size = new System.Drawing.Size(570, 46);
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.txtConfirmNo);
            this.ContainerPanel.Controls.Add(this.label4);
            this.ContainerPanel.Controls.Add(this.messageBar1);
            this.ContainerPanel.Controls.Add(this.btnRdConfirmNo);
            this.ContainerPanel.Controls.Add(this.btnEvidence);
            this.ContainerPanel.Controls.Add(this.btnRdICCard);
            this.ContainerPanel.Controls.Add(this.btnDeduction);
            this.ContainerPanel.Controls.Add(this.txtBusinessNo);
            this.ContainerPanel.Controls.Add(this.label2);
            this.ContainerPanel.Controls.Add(this.txtType);
            this.ContainerPanel.Controls.Add(this.label1);
            this.ContainerPanel.Controls.Add(this.txtAmt);
            this.ContainerPanel.Controls.Add(this.label3);
            this.ContainerPanel.Controls.Add(this.keyPad);
            this.ContainerPanel.Size = new System.Drawing.Size(604, 462);
            this.ContainerPanel.Controls.SetChildIndex(this.MessageBar, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.keyPad, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.label3, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.txtAmt, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.label1, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.txtType, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.label2, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.txtBusinessNo, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.btnDeduction, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.btnRdICCard, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.btnEvidence, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.btnRdConfirmNo, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.messageBar1, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.label4, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.txtConfirmNo, 0);
            // 
            // keyPad
            // 
            this.keyPad.Location = new System.Drawing.Point(342, 17);
            this.keyPad.Margin = new System.Windows.Forms.Padding(0);
            this.keyPad.MaximumSize = new System.Drawing.Size(245, 233);
            this.keyPad.Name = "keyPad";
            this.keyPad.Size = new System.Drawing.Size(245, 233);
            this.keyPad.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 28);
            this.label3.TabIndex = 21;
            this.label3.Text = "대상금액";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 28);
            this.label1.TabIndex = 30;
            this.label1.Text = "발행구분";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(14, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 28);
            this.label2.TabIndex = 32;
            this.label2.Text = "개인/사업자";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(14, 180);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 28);
            this.label4.TabIndex = 34;
            this.label4.Text = "확인번호";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // messageBar1
            // 
            this.messageBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.messageBar1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.messageBar1.BorderWidth = new System.Windows.Forms.Padding(1);
            this.messageBar1.ControlType = WSWD.WmallPos.POS.FX.Win.UserControls.MessageBarType.TypeStatus;
            this.messageBar1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.messageBar1.Location = new System.Drawing.Point(17, 277);
            this.messageBar1.MinimumSize = new System.Drawing.Size(0, 35);
            this.messageBar1.Name = "messageBar1";
            this.messageBar1.Size = new System.Drawing.Size(570, 46);
            this.messageBar1.TabIndex = 38;
            this.messageBar1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.IsHighlight = false;
            this.btnClose.Location = new System.Drawing.Point(308, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 37;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtConfirmNo
            // 
            this.txtConfirmNo.BackColor = System.Drawing.Color.White;
            this.txtConfirmNo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(109)))), ((int)(((byte)(195)))));
            this.txtConfirmNo.BorderWidth = 2;
            this.txtConfirmNo.Corner = 1;
            this.txtConfirmNo.Focusable = true;
            this.txtConfirmNo.FocusedIndex = 0;
            this.txtConfirmNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtConfirmNo.Format = "";
            this.txtConfirmNo.HasBorder = true;
            this.txtConfirmNo.IsFocused = true;
            this.txtConfirmNo.Location = new System.Drawing.Point(124, 180);
            this.txtConfirmNo.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtConfirmNo.MaxLength = 13;
            this.txtConfirmNo.Name = "txtConfirmNo";
            this.txtConfirmNo.PasswordMode = false;
            this.txtConfirmNo.ReadOnly = false;
            this.txtConfirmNo.Size = new System.Drawing.Size(200, 28);
            this.txtConfirmNo.TabIndex = 35;
            this.txtConfirmNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnEvidence
            // 
            this.btnEvidence.BorderSize = 1;
            this.btnEvidence.Corner = 3;
            this.btnEvidence.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnEvidence.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnEvidence.Image = null;
            this.btnEvidence.IsHighlight = false;
            this.btnEvidence.Location = new System.Drawing.Point(186, 17);
            this.btnEvidence.Name = "btnEvidence";
            this.btnEvidence.Selected = false;
            this.btnEvidence.Size = new System.Drawing.Size(138, 42);
            this.btnEvidence.TabIndex = 37;
            this.btnEvidence.Text = "지출증빙";
            this.btnEvidence.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDeduction
            // 
            this.btnDeduction.BorderSize = 1;
            this.btnDeduction.Corner = 3;
            this.btnDeduction.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnDeduction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnDeduction.Image = null;
            this.btnDeduction.IsHighlight = false;
            this.btnDeduction.Location = new System.Drawing.Point(17, 17);
            this.btnDeduction.Name = "btnDeduction";
            this.btnDeduction.Selected = false;
            this.btnDeduction.Size = new System.Drawing.Size(138, 42);
            this.btnDeduction.TabIndex = 36;
            this.btnDeduction.Text = "소득공제";
            this.btnDeduction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtBusinessNo
            // 
            this.txtBusinessNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtBusinessNo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtBusinessNo.BorderWidth = 1;
            this.txtBusinessNo.Corner = 1;
            this.txtBusinessNo.Focusable = false;
            this.txtBusinessNo.FocusedIndex = 0;
            this.txtBusinessNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtBusinessNo.Format = "";
            this.txtBusinessNo.HasBorder = true;
            this.txtBusinessNo.IsFocused = false;
            this.txtBusinessNo.Location = new System.Drawing.Point(124, 144);
            this.txtBusinessNo.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtBusinessNo.Name = "txtBusinessNo";
            this.txtBusinessNo.PasswordMode = false;
            this.txtBusinessNo.ReadOnly = true;
            this.txtBusinessNo.Size = new System.Drawing.Size(200, 28);
            this.txtBusinessNo.TabIndex = 33;
            this.txtBusinessNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtType
            // 
            this.txtType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtType.BorderWidth = 1;
            this.txtType.Corner = 1;
            this.txtType.Focusable = false;
            this.txtType.FocusedIndex = 0;
            this.txtType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtType.Format = "";
            this.txtType.HasBorder = true;
            this.txtType.IsFocused = false;
            this.txtType.Location = new System.Drawing.Point(124, 72);
            this.txtType.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtType.Name = "txtType";
            this.txtType.PasswordMode = false;
            this.txtType.ReadOnly = true;
            this.txtType.Size = new System.Drawing.Size(200, 28);
            this.txtType.TabIndex = 31;
            this.txtType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtAmt
            // 
            this.txtAmt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtAmt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtAmt.BorderWidth = 1;
            this.txtAmt.Corner = 1;
            this.txtAmt.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtAmt.Focusable = false;
            this.txtAmt.FocusedIndex = 0;
            this.txtAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtAmt.Format = "#,##0";
            this.txtAmt.HasBorder = true;
            this.txtAmt.IsFocused = false;
            this.txtAmt.Location = new System.Drawing.Point(124, 108);
            this.txtAmt.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtAmt.MaxLength = 9;
            this.txtAmt.Name = "txtAmt";
            this.txtAmt.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.txtAmt.PasswordMode = false;
            this.txtAmt.ReadOnly = true;
            this.txtAmt.Size = new System.Drawing.Size(200, 28);
            this.txtAmt.TabIndex = 29;
            this.txtAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // btnSelf
            // 
            this.btnSelf.BorderSize = 1;
            this.btnSelf.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnSelf.Corner = 3;
            this.btnSelf.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnSelf.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnSelf.Image = null;
            this.btnSelf.IsHighlight = false;
            this.btnSelf.Location = new System.Drawing.Point(172, 20);
            this.btnSelf.Name = "btnSelf";
            this.btnSelf.Selected = false;
            this.btnSelf.Size = new System.Drawing.Size(90, 42);
            this.btnSelf.TabIndex = 40;
            this.btnSelf.Text = "자진발급";
            this.btnSelf.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // axKSNet_Dongle1
            // 
            this.axKSNet_Dongle1.Enabled = true;
            this.axKSNet_Dongle1.Location = new System.Drawing.Point(425, 34);
            this.axKSNet_Dongle1.Name = "axKSNet_Dongle1";
            this.axKSNet_Dongle1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKSNet_Dongle1.OcxState")));
            this.axKSNet_Dongle1.Size = new System.Drawing.Size(32, 17);
            this.axKSNet_Dongle1.TabIndex = 41;
            // 
            // btnRdICCard
            // 
            this.btnRdICCard.BorderSize = 1;
            this.btnRdICCard.Corner = 3;
            this.btnRdICCard.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnRdICCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnRdICCard.Image = null;
            this.btnRdICCard.IsHighlight = false;
            this.btnRdICCard.Location = new System.Drawing.Point(17, 220);
            this.btnRdICCard.Name = "btnRdICCard";
            this.btnRdICCard.Selected = false;
            this.btnRdICCard.Size = new System.Drawing.Size(138, 42);
            this.btnRdICCard.TabIndex = 36;
            this.btnRdICCard.Text = "신용카드";
            this.btnRdICCard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRdConfirmNo
            // 
            this.btnRdConfirmNo.BorderSize = 1;
            this.btnRdConfirmNo.Corner = 3;
            this.btnRdConfirmNo.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnRdConfirmNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnRdConfirmNo.Image = null;
            this.btnRdConfirmNo.IsHighlight = false;
            this.btnRdConfirmNo.Location = new System.Drawing.Point(186, 220);
            this.btnRdConfirmNo.Name = "btnRdConfirmNo";
            this.btnRdConfirmNo.Selected = true;
            this.btnRdConfirmNo.Size = new System.Drawing.Size(138, 42);
            this.btnRdConfirmNo.TabIndex = 37;
            this.btnRdConfirmNo.Text = "일반식별번호";
            this.btnRdConfirmNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // POS_PY_P014
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(610, 508);
            this.Name = "POS_PY_P014";
            this.Text = "현금영수증 발행";
            this.ButtonsPanel.ResumeLayout(false);
            this.ContainerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axKSNet_Dongle1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton roundedButton2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad keyPad;
        private System.Windows.Forms.Label label3;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtAmt;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtType;
        private System.Windows.Forms.Label label1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnEvidence;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnDeduction;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtConfirmNo;
        private System.Windows.Forms.Label label4;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtBusinessNo;
        private System.Windows.Forms.Label label2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar messageBar1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnSelf;
        private AxKSNET_DONGLELib.AxKSNet_Dongle axKSNet_Dongle1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnRdConfirmNo;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnRdICCard;
    }
}