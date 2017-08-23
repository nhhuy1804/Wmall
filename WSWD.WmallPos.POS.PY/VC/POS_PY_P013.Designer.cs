namespace WSWD.WmallPos.POS.PY.VC
{
    partial class POS_PY_P013
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
            this.msgBar01 = new WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCardNo = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.txtPayAmt = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOrgApprDate = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.lblOrgApprDate = new System.Windows.Forms.Label();
            this.txtOrgApprNo = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.lblOrgApprNo = new System.Windows.Forms.Label();
            this.btnICCard = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnSimpleCancel = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.lblYmd = new System.Windows.Forms.Label();
            this.btnCancNoCard = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.ButtonsPanel.SuspendLayout();
            this.ContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.btnSimpleCancel);
            this.ButtonsPanel.Controls.Add(this.btnClose);
            this.ButtonsPanel.Size = new System.Drawing.Size(559, 62);
            // 
            // MessageBar
            // 
            this.MessageBar.ControlType = WSWD.WmallPos.POS.FX.Win.UserControls.MessageBarType.TypeMessage;
            this.MessageBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this.MessageBar.Location = new System.Drawing.Point(17, 320);
            this.MessageBar.Size = new System.Drawing.Size(559, 46);
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.btnCancNoCard);
            this.ContainerPanel.Controls.Add(this.btnICCard);
            this.ContainerPanel.Controls.Add(this.txtOrgApprNo);
            this.ContainerPanel.Controls.Add(this.lblOrgApprNo);
            this.ContainerPanel.Controls.Add(this.txtOrgApprDate);
            this.ContainerPanel.Controls.Add(this.lblYmd);
            this.ContainerPanel.Controls.Add(this.lblOrgApprDate);
            this.ContainerPanel.Controls.Add(this.txtPayAmt);
            this.ContainerPanel.Controls.Add(this.label1);
            this.ContainerPanel.Controls.Add(this.txtCardNo);
            this.ContainerPanel.Controls.Add(this.label3);
            this.ContainerPanel.Controls.Add(this.msgBar01);
            this.ContainerPanel.Controls.Add(this.keyPad);
            this.ContainerPanel.Size = new System.Drawing.Size(593, 445);
            this.ContainerPanel.TabIndex = 0;
            this.ContainerPanel.Controls.SetChildIndex(this.MessageBar, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.keyPad, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.msgBar01, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.label3, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.txtCardNo, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.label1, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.txtPayAmt, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.lblOrgApprDate, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.lblYmd, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.txtOrgApprDate, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.lblOrgApprNo, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.txtOrgApprNo, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.btnICCard, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.btnCancNoCard, 0);
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
            this.keyPad.Location = new System.Drawing.Point(331, 17);
            this.keyPad.Margin = new System.Windows.Forms.Padding(0);
            this.keyPad.MaximumSize = new System.Drawing.Size(245, 233);
            this.keyPad.Name = "keyPad";
            this.keyPad.Size = new System.Drawing.Size(245, 233);
            this.keyPad.TabIndex = 5;
            // 
            // msgBar01
            // 
            this.msgBar01.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.msgBar01.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.msgBar01.BorderWidth = new System.Windows.Forms.Padding(1);
            this.msgBar01.ControlType = WSWD.WmallPos.POS.FX.Win.UserControls.MessageBarType.TypeStatus;
            this.msgBar01.ForeColor = System.Drawing.SystemColors.ControlText;
            this.msgBar01.Location = new System.Drawing.Point(17, 266);
            this.msgBar01.MinimumSize = new System.Drawing.Size(0, 35);
            this.msgBar01.Name = "msgBar01";
            this.msgBar01.Size = new System.Drawing.Size(559, 42);
            this.msgBar01.TabIndex = 13;
            this.msgBar01.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.IsHighlight = false;
            this.btnClose.Location = new System.Drawing.Point(234, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 28);
            this.label3.TabIndex = 21;
            this.label3.Text = "IC번호";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCardNo
            // 
            this.txtCardNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtCardNo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtCardNo.BorderWidth = 1;
            this.txtCardNo.Corner = 1;
            this.txtCardNo.Focusable = false;
            this.txtCardNo.FocusedIndex = 0;
            this.txtCardNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtCardNo.Format = "#,##0";
            this.txtCardNo.HasBorder = true;
            this.txtCardNo.IsFocused = false;
            this.txtCardNo.Location = new System.Drawing.Point(114, 53);
            this.txtCardNo.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtCardNo.MaxLength = 9;
            this.txtCardNo.Name = "txtCardNo";
            this.txtCardNo.Padding = new System.Windows.Forms.Padding(4, 0, 5, 0);
            this.txtCardNo.PasswordMode = false;
            this.txtCardNo.ReadOnly = true;
            this.txtCardNo.Size = new System.Drawing.Size(200, 28);
            this.txtCardNo.TabIndex = 1;
            this.txtCardNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPayAmt
            // 
            this.txtPayAmt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtPayAmt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtPayAmt.BorderWidth = 1;
            this.txtPayAmt.Corner = 1;
            this.txtPayAmt.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtPayAmt.Focusable = false;
            this.txtPayAmt.FocusedIndex = 0;
            this.txtPayAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtPayAmt.Format = "#,##0";
            this.txtPayAmt.HasBorder = true;
            this.txtPayAmt.IsFocused = false;
            this.txtPayAmt.Location = new System.Drawing.Point(114, 17);
            this.txtPayAmt.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtPayAmt.Name = "txtPayAmt";
            this.txtPayAmt.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.txtPayAmt.PasswordMode = false;
            this.txtPayAmt.ReadOnly = true;
            this.txtPayAmt.Size = new System.Drawing.Size(200, 28);
            this.txtPayAmt.TabIndex = 0;
            this.txtPayAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 28);
            this.label1.TabIndex = 30;
            this.label1.Text = "결제금액";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtOrgApprDate
            // 
            this.txtOrgApprDate.BackColor = System.Drawing.Color.White;
            this.txtOrgApprDate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtOrgApprDate.BorderWidth = 1;
            this.txtOrgApprDate.Corner = 1;
            this.txtOrgApprDate.Focusable = true;
            this.txtOrgApprDate.FocusedIndex = 0;
            this.txtOrgApprDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtOrgApprDate.Format = "";
            this.txtOrgApprDate.HasBorder = true;
            this.txtOrgApprDate.IsFocused = false;
            this.txtOrgApprDate.Location = new System.Drawing.Point(114, 89);
            this.txtOrgApprDate.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtOrgApprDate.MaxLength = 8;
            this.txtOrgApprDate.Name = "txtOrgApprDate";
            this.txtOrgApprDate.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.txtOrgApprDate.PasswordMode = false;
            this.txtOrgApprDate.ReadOnly = false;
            this.txtOrgApprDate.Size = new System.Drawing.Size(88, 28);
            this.txtOrgApprDate.TabIndex = 2;
            this.txtOrgApprDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOrgApprDate
            // 
            this.lblOrgApprDate.Location = new System.Drawing.Point(14, 89);
            this.lblOrgApprDate.Name = "lblOrgApprDate";
            this.lblOrgApprDate.Size = new System.Drawing.Size(90, 28);
            this.lblOrgApprDate.TabIndex = 32;
            this.lblOrgApprDate.Text = "원거래일자";
            this.lblOrgApprDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtOrgApprNo
            // 
            this.txtOrgApprNo.BackColor = System.Drawing.Color.White;
            this.txtOrgApprNo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtOrgApprNo.BorderWidth = 1;
            this.txtOrgApprNo.Corner = 1;
            this.txtOrgApprNo.Focusable = true;
            this.txtOrgApprNo.FocusedIndex = 1;
            this.txtOrgApprNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtOrgApprNo.Format = "";
            this.txtOrgApprNo.HasBorder = true;
            this.txtOrgApprNo.IsFocused = false;
            this.txtOrgApprNo.Location = new System.Drawing.Point(114, 125);
            this.txtOrgApprNo.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtOrgApprNo.MaxLength = 16;
            this.txtOrgApprNo.Name = "txtOrgApprNo";
            this.txtOrgApprNo.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.txtOrgApprNo.PasswordMode = false;
            this.txtOrgApprNo.ReadOnly = false;
            this.txtOrgApprNo.Size = new System.Drawing.Size(200, 28);
            this.txtOrgApprNo.TabIndex = 3;
            this.txtOrgApprNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOrgApprNo
            // 
            this.lblOrgApprNo.Location = new System.Drawing.Point(14, 125);
            this.lblOrgApprNo.Name = "lblOrgApprNo";
            this.lblOrgApprNo.Size = new System.Drawing.Size(90, 28);
            this.lblOrgApprNo.TabIndex = 34;
            this.lblOrgApprNo.Text = "원승인번호";
            this.lblOrgApprNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnICCard
            // 
            this.btnICCard.BorderSize = 1;
            this.btnICCard.Corner = 3;
            this.btnICCard.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnICCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnICCard.Image = null;
            this.btnICCard.IsHighlight = false;
            this.btnICCard.Location = new System.Drawing.Point(17, 172);
            this.btnICCard.Name = "btnICCard";
            this.btnICCard.Selected = false;
            this.btnICCard.Size = new System.Drawing.Size(124, 42);
            this.btnICCard.TabIndex = 4;
            this.btnICCard.Text = "IC카드 정보";
            this.btnICCard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSimpleCancel
            // 
            this.btnSimpleCancel.BorderSize = 1;
            this.btnSimpleCancel.Corner = 3;
            this.btnSimpleCancel.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnSimpleCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnSimpleCancel.Image = null;
            this.btnSimpleCancel.IsHighlight = false;
            this.btnSimpleCancel.Location = new System.Drawing.Point(138, 20);
            this.btnSimpleCancel.Name = "btnSimpleCancel";
            this.btnSimpleCancel.Selected = false;
            this.btnSimpleCancel.Size = new System.Drawing.Size(90, 42);
            this.btnSimpleCancel.TabIndex = 0;
            this.btnSimpleCancel.Text = "간소화취소";
            this.btnSimpleCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSimpleCancel.Visible = false;
            // 
            // lblYmd
            // 
            this.lblYmd.Location = new System.Drawing.Point(206, 89);
            this.lblYmd.Name = "lblYmd";
            this.lblYmd.Size = new System.Drawing.Size(118, 28);
            this.lblYmd.TabIndex = 32;
            this.lblYmd.Text = "(YYYYMMDD)";
            this.lblYmd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancNoCard
            // 
            this.btnCancNoCard.BorderSize = 1;
            this.btnCancNoCard.Corner = 3;
            this.btnCancNoCard.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnCancNoCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnCancNoCard.Image = null;
            this.btnCancNoCard.IsHighlight = false;
            this.btnCancNoCard.Location = new System.Drawing.Point(190, 172);
            this.btnCancNoCard.Name = "btnCancNoCard";
            this.btnCancNoCard.Selected = false;
            this.btnCancNoCard.Size = new System.Drawing.Size(124, 42);
            this.btnCancNoCard.TabIndex = 35;
            this.btnCancNoCard.Text = "무카드 취소";
            this.btnCancNoCard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // POS_PY_P013
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(599, 491);
            this.Name = "POS_PY_P013";
            this.Text = "현금 IC";
            this.ButtonsPanel.ResumeLayout(false);
            this.ContainerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton roundedButton2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar msgBar01;
        private WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad keyPad;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private System.Windows.Forms.Label label3;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtCardNo;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtPayAmt;
        private System.Windows.Forms.Label label1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnICCard;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtOrgApprNo;
        private System.Windows.Forms.Label lblOrgApprNo;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtOrgApprDate;
        private System.Windows.Forms.Label lblOrgApprDate;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnSimpleCancel;
        private System.Windows.Forms.Label lblYmd;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnCancNoCard;
    }
}