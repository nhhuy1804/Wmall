namespace WSWD.WmallPos.POS.PT.VC
{
    partial class POS_PT_P001
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_PT_P001));
            this.roundedButton2 = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.keyPad = new WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad();
            this.btnCard = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.msgBar = new WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnSave = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtType = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.txtCardNo = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.txtCustName = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.txtGradeName = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.txtAbtyPoint = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.txtCltePoint = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.txtDelayPoint = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.txtRemark = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.axKSNet_Dongle1 = new AxKSNET_DONGLELib.AxKSNet_Dongle();
            this.ContainerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axKSNet_Dongle1)).BeginInit();
            this.SuspendLayout();
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.axKSNet_Dongle1);
            this.ContainerPanel.Controls.Add(this.txtRemark);
            this.ContainerPanel.Controls.Add(this.txtDelayPoint);
            this.ContainerPanel.Controls.Add(this.txtCltePoint);
            this.ContainerPanel.Controls.Add(this.txtAbtyPoint);
            this.ContainerPanel.Controls.Add(this.txtGradeName);
            this.ContainerPanel.Controls.Add(this.txtCustName);
            this.ContainerPanel.Controls.Add(this.txtCardNo);
            this.ContainerPanel.Controls.Add(this.txtType);
            this.ContainerPanel.Controls.Add(this.label9);
            this.ContainerPanel.Controls.Add(this.label8);
            this.ContainerPanel.Controls.Add(this.label7);
            this.ContainerPanel.Controls.Add(this.label6);
            this.ContainerPanel.Controls.Add(this.label5);
            this.ContainerPanel.Controls.Add(this.label4);
            this.ContainerPanel.Controls.Add(this.label3);
            this.ContainerPanel.Controls.Add(this.label2);
            this.ContainerPanel.Controls.Add(this.label1);
            this.ContainerPanel.Controls.Add(this.btnClose);
            this.ContainerPanel.Controls.Add(this.btnSave);
            this.ContainerPanel.Controls.Add(this.msgBar);
            this.ContainerPanel.Controls.Add(this.btnCard);
            this.ContainerPanel.Controls.Add(this.keyPad);
            this.ContainerPanel.Size = new System.Drawing.Size(634, 513);
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
            this.keyPad.Location = new System.Drawing.Point(368, 20);
            this.keyPad.Margin = new System.Windows.Forms.Padding(0);
            this.keyPad.MaximumSize = new System.Drawing.Size(245, 233);
            this.keyPad.Name = "keyPad";
            this.keyPad.Size = new System.Drawing.Size(245, 233);
            this.keyPad.TabIndex = 9;
            // 
            // btnCard
            // 
            this.btnCard.BorderSize = 1;
            this.btnCard.Corner = 3;
            this.btnCard.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnCard.Image = null;
            this.btnCard.IsHighlight = false;
            this.btnCard.Location = new System.Drawing.Point(523, 272);
            this.btnCard.Name = "btnCard";
            this.btnCard.Selected = false;
            this.btnCard.Size = new System.Drawing.Size(90, 42);
            this.btnCard.TabIndex = 12;
            this.btnCard.Text = "지류카드";
            this.btnCard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCard.Visible = false;
            // 
            // msgBar
            // 
            this.msgBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.msgBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.msgBar.BorderWidth = new System.Windows.Forms.Padding(1);
            this.msgBar.ControlType = WSWD.WmallPos.POS.FX.Win.UserControls.MessageBarType.TypeStatus;
            this.msgBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.msgBar.Location = new System.Drawing.Point(21, 392);
            this.msgBar.MinimumSize = new System.Drawing.Size(0, 35);
            this.msgBar.Name = "msgBar";
            this.msgBar.Size = new System.Drawing.Size(592, 42);
            this.msgBar.TabIndex = 13;
            this.msgBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.IsHighlight = false;
            this.btnClose.Location = new System.Drawing.Point(326, 454);
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
            this.btnSave.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnSave.Image = null;
            this.btnSave.IsHighlight = false;
            this.btnSave.Location = new System.Drawing.Point(220, 454);
            this.btnSave.Name = "btnSave";
            this.btnSave.Selected = false;
            this.btnSave.Size = new System.Drawing.Size(90, 42);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "확인";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(18, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 28);
            this.label1.TabIndex = 19;
            this.label1.Text = "구분";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(158, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 28);
            this.label2.TabIndex = 20;
            this.label2.Text = "(1:카드번호 2:전화번호)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(18, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 28);
            this.label3.TabIndex = 21;
            this.label3.Text = "카드번호";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(18, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 28);
            this.label4.TabIndex = 22;
            this.label4.Text = "회원이름";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(18, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 28);
            this.label5.TabIndex = 23;
            this.label5.Text = "등급";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(18, 200);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 28);
            this.label6.TabIndex = 24;
            this.label6.Text = "가용점수";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(18, 236);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 28);
            this.label7.TabIndex = 25;
            this.label7.Text = "누적점수";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label7.Visible = false;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(18, 164);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 28);
            this.label8.TabIndex = 26;
            this.label8.Text = "예정점수";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(18, 272);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 28);
            this.label9.TabIndex = 27;
            this.label9.Text = "비고";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtType
            // 
            this.txtType.BackColor = System.Drawing.Color.White;
            this.txtType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtType.BorderWidth = 1;
            this.txtType.Corner = 1;
            this.txtType.Focusable = true;
            this.txtType.FocusedIndex = 0;
            this.txtType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtType.Format = null;
            this.txtType.HasBorder = true;
            this.txtType.IsFocused = false;
            this.txtType.Location = new System.Drawing.Point(108, 20);
            this.txtType.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtType.MaxLength = 1;
            this.txtType.Name = "txtType";
            this.txtType.PasswordMode = false;
            this.txtType.ReadOnly = false;
            this.txtType.Size = new System.Drawing.Size(40, 28);
            this.txtType.TabIndex = 28;
            this.txtType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCardNo
            // 
            this.txtCardNo.BackColor = System.Drawing.Color.White;
            this.txtCardNo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtCardNo.BorderWidth = 1;
            this.txtCardNo.Corner = 1;
            this.txtCardNo.Focusable = true;
            this.txtCardNo.FocusedIndex = 1;
            this.txtCardNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtCardNo.Format = null;
            this.txtCardNo.HasBorder = true;
            this.txtCardNo.IsFocused = false;
            this.txtCardNo.Location = new System.Drawing.Point(108, 56);
            this.txtCardNo.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtCardNo.MaxLength = 13;
            this.txtCardNo.Name = "txtCardNo";
            this.txtCardNo.PasswordMode = false;
            this.txtCardNo.ReadOnly = false;
            this.txtCardNo.Size = new System.Drawing.Size(238, 28);
            this.txtCardNo.TabIndex = 29;
            this.txtCardNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCustName
            // 
            this.txtCustName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtCustName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtCustName.BorderWidth = 1;
            this.txtCustName.Corner = 1;
            this.txtCustName.Focusable = false;
            this.txtCustName.FocusedIndex = 0;
            this.txtCustName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtCustName.Format = null;
            this.txtCustName.HasBorder = true;
            this.txtCustName.IsFocused = false;
            this.txtCustName.Location = new System.Drawing.Point(108, 92);
            this.txtCustName.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtCustName.Name = "txtCustName";
            this.txtCustName.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.txtCustName.PasswordMode = false;
            this.txtCustName.ReadOnly = true;
            this.txtCustName.Size = new System.Drawing.Size(238, 28);
            this.txtCustName.TabIndex = 30;
            this.txtCustName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtGradeName
            // 
            this.txtGradeName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtGradeName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtGradeName.BorderWidth = 1;
            this.txtGradeName.Corner = 1;
            this.txtGradeName.Focusable = false;
            this.txtGradeName.FocusedIndex = 0;
            this.txtGradeName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtGradeName.Format = null;
            this.txtGradeName.HasBorder = true;
            this.txtGradeName.IsFocused = false;
            this.txtGradeName.Location = new System.Drawing.Point(108, 128);
            this.txtGradeName.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtGradeName.Name = "txtGradeName";
            this.txtGradeName.PasswordMode = false;
            this.txtGradeName.ReadOnly = true;
            this.txtGradeName.Size = new System.Drawing.Size(238, 28);
            this.txtGradeName.TabIndex = 31;
            this.txtGradeName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtAbtyPoint
            // 
            this.txtAbtyPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtAbtyPoint.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtAbtyPoint.BorderWidth = 1;
            this.txtAbtyPoint.Corner = 1;
            this.txtAbtyPoint.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtAbtyPoint.Focusable = false;
            this.txtAbtyPoint.FocusedIndex = 0;
            this.txtAbtyPoint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtAbtyPoint.Format = "#,##0";
            this.txtAbtyPoint.HasBorder = true;
            this.txtAbtyPoint.IsFocused = false;
            this.txtAbtyPoint.Location = new System.Drawing.Point(108, 200);
            this.txtAbtyPoint.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtAbtyPoint.Name = "txtAbtyPoint";
            this.txtAbtyPoint.Padding = new System.Windows.Forms.Padding(4, 0, 5, 0);
            this.txtAbtyPoint.PasswordMode = false;
            this.txtAbtyPoint.ReadOnly = true;
            this.txtAbtyPoint.Size = new System.Drawing.Size(238, 28);
            this.txtAbtyPoint.TabIndex = 32;
            this.txtAbtyPoint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCltePoint
            // 
            this.txtCltePoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtCltePoint.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtCltePoint.BorderWidth = 1;
            this.txtCltePoint.Corner = 1;
            this.txtCltePoint.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtCltePoint.Focusable = false;
            this.txtCltePoint.FocusedIndex = 0;
            this.txtCltePoint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtCltePoint.Format = "#,##0";
            this.txtCltePoint.HasBorder = true;
            this.txtCltePoint.IsFocused = false;
            this.txtCltePoint.Location = new System.Drawing.Point(108, 236);
            this.txtCltePoint.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtCltePoint.Name = "txtCltePoint";
            this.txtCltePoint.Padding = new System.Windows.Forms.Padding(4, 0, 5, 0);
            this.txtCltePoint.PasswordMode = false;
            this.txtCltePoint.ReadOnly = true;
            this.txtCltePoint.Size = new System.Drawing.Size(238, 28);
            this.txtCltePoint.TabIndex = 33;
            this.txtCltePoint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtCltePoint.Visible = false;
            // 
            // txtDelayPoint
            // 
            this.txtDelayPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtDelayPoint.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtDelayPoint.BorderWidth = 1;
            this.txtDelayPoint.Corner = 1;
            this.txtDelayPoint.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtDelayPoint.Focusable = false;
            this.txtDelayPoint.FocusedIndex = 0;
            this.txtDelayPoint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtDelayPoint.Format = "#,##0";
            this.txtDelayPoint.HasBorder = true;
            this.txtDelayPoint.IsFocused = false;
            this.txtDelayPoint.Location = new System.Drawing.Point(108, 164);
            this.txtDelayPoint.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtDelayPoint.Name = "txtDelayPoint";
            this.txtDelayPoint.Padding = new System.Windows.Forms.Padding(4, 0, 5, 0);
            this.txtDelayPoint.PasswordMode = false;
            this.txtDelayPoint.ReadOnly = true;
            this.txtDelayPoint.Size = new System.Drawing.Size(238, 28);
            this.txtDelayPoint.TabIndex = 34;
            this.txtDelayPoint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtRemark.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtRemark.BorderWidth = 1;
            this.txtRemark.Corner = 1;
            this.txtRemark.Focusable = false;
            this.txtRemark.FocusedIndex = 0;
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtRemark.Format = null;
            this.txtRemark.HasBorder = true;
            this.txtRemark.IsFocused = false;
            this.txtRemark.Location = new System.Drawing.Point(108, 272);
            this.txtRemark.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.txtRemark.PasswordMode = false;
            this.txtRemark.ReadOnly = true;
            this.txtRemark.Size = new System.Drawing.Size(379, 100);
            this.txtRemark.TabIndex = 35;
            this.txtRemark.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // axKSNet_Dongle1
            // 
            this.axKSNet_Dongle1.Enabled = true;
            this.axKSNet_Dongle1.Location = new System.Drawing.Point(500, 326);
            this.axKSNet_Dongle1.Name = "axKSNet_Dongle1";
            this.axKSNet_Dongle1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKSNet_Dongle1.OcxState")));
            this.axKSNet_Dongle1.Size = new System.Drawing.Size(95, 45);
            this.axKSNet_Dongle1.TabIndex = 36;
            // 
            // POS_PT_P001
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(640, 559);
            this.Name = "POS_PT_P001";
            this.Text = "포인트 조회";
            this.ContainerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axKSNet_Dongle1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton roundedButton2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar msgBar;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnCard;
        private WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad keyPad;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnSave;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtType;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtDelayPoint;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtCltePoint;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtAbtyPoint;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtGradeName;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtCustName;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtCardNo;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtRemark;
        private AxKSNET_DONGLELib.AxKSNet_Dongle axKSNet_Dongle1;
    }
}