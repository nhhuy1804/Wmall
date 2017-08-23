namespace WSWD.WmallPos.POS.PY.VC
{
    partial class POS_PY_P007
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
            this.msgBar = new WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar();
            this.btnSave = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnCancel = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTicketNo = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGetAmt = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTicketAmt = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTicketTotalAmt = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label5 = new System.Windows.Forms.Label();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.grd = new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel();
            this.btnType = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.txtType = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.lblType = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.ContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.lblType);
            this.ContainerPanel.Controls.Add(this.txtType);
            this.ContainerPanel.Controls.Add(this.btnType);
            this.ContainerPanel.Controls.Add(this.grd);
            this.ContainerPanel.Controls.Add(this.btnClose);
            this.ContainerPanel.Controls.Add(this.txtTicketTotalAmt);
            this.ContainerPanel.Controls.Add(this.label5);
            this.ContainerPanel.Controls.Add(this.txtTicketAmt);
            this.ContainerPanel.Controls.Add(this.label4);
            this.ContainerPanel.Controls.Add(this.txtGetAmt);
            this.ContainerPanel.Controls.Add(this.label2);
            this.ContainerPanel.Controls.Add(this.label1);
            this.ContainerPanel.Controls.Add(this.txtTicketNo);
            this.ContainerPanel.Controls.Add(this.label3);
            this.ContainerPanel.Controls.Add(this.btnSave);
            this.ContainerPanel.Controls.Add(this.btnCancel);
            this.ContainerPanel.Controls.Add(this.msgBar);
            this.ContainerPanel.Size = new System.Drawing.Size(599, 560);
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
            // msgBar
            // 
            this.msgBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.msgBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.msgBar.BorderWidth = new System.Windows.Forms.Padding(1);
            this.msgBar.ControlType = WSWD.WmallPos.POS.FX.Win.UserControls.MessageBarType.TypeStatus;
            this.msgBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.msgBar.Location = new System.Drawing.Point(17, 140);
            this.msgBar.MinimumSize = new System.Drawing.Size(0, 35);
            this.msgBar.Name = "msgBar";
            this.msgBar.Size = new System.Drawing.Size(565, 42);
            this.msgBar.TabIndex = 13;
            this.msgBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.btnSave.Location = new System.Drawing.Point(254, 500);
            this.btnSave.Name = "btnSave";
            this.btnSave.Selected = false;
            this.btnSave.Size = new System.Drawing.Size(90, 42);
            this.btnSave.TabIndex = 18;
            this.btnSave.Text = "결제확정";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.BorderSize = 1;
            this.btnCancel.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnCancel.Corner = 3;
            this.btnCancel.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnCancel.Image = null;
            this.btnCancel.IsHighlight = false;
            this.btnCancel.Location = new System.Drawing.Point(148, 500);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Selected = false;
            this.btnCancel.Size = new System.Drawing.Size(90, 42);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "한건취소";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 28);
            this.label3.TabIndex = 21;
            this.label3.Text = "상품권번호";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTicketNo
            // 
            this.txtTicketNo.BackColor = System.Drawing.Color.White;
            this.txtTicketNo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtTicketNo.BorderWidth = 1;
            this.txtTicketNo.Corner = 1;
            this.txtTicketNo.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtTicketNo.Focusable = true;
            this.txtTicketNo.FocusedIndex = 1;
            this.txtTicketNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtTicketNo.Format = "";
            this.txtTicketNo.HasBorder = true;
            this.txtTicketNo.IsFocused = false;
            this.txtTicketNo.Location = new System.Drawing.Point(110, 53);
            this.txtTicketNo.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtTicketNo.MaxLength = 20;
            this.txtTicketNo.Name = "txtTicketNo";
            this.txtTicketNo.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.txtTicketNo.PasswordMode = false;
            this.txtTicketNo.ReadOnly = false;
            this.txtTicketNo.Size = new System.Drawing.Size(210, 28);
            this.txtTicketNo.TabIndex = 29;
            this.txtTicketNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 28);
            this.label1.TabIndex = 30;
            this.label1.Text = "상품권종류";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.txtGetAmt.Location = new System.Drawing.Point(432, 53);
            this.txtGetAmt.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtGetAmt.MaxLength = 9;
            this.txtGetAmt.Name = "txtGetAmt";
            this.txtGetAmt.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.txtGetAmt.PasswordMode = false;
            this.txtGetAmt.ReadOnly = true;
            this.txtGetAmt.Size = new System.Drawing.Size(150, 28);
            this.txtGetAmt.TabIndex = 33;
            this.txtGetAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(330, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 28);
            this.label2.TabIndex = 32;
            this.label2.Text = "받을돈";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTicketAmt
            // 
            this.txtTicketAmt.BackColor = System.Drawing.Color.White;
            this.txtTicketAmt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtTicketAmt.BorderWidth = 1;
            this.txtTicketAmt.Corner = 1;
            this.txtTicketAmt.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtTicketAmt.Focusable = true;
            this.txtTicketAmt.FocusedIndex = 2;
            this.txtTicketAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtTicketAmt.Format = "#,##0";
            this.txtTicketAmt.HasBorder = true;
            this.txtTicketAmt.IsFocused = false;
            this.txtTicketAmt.Location = new System.Drawing.Point(110, 91);
            this.txtTicketAmt.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtTicketAmt.MaxLength = 9;
            this.txtTicketAmt.Name = "txtTicketAmt";
            this.txtTicketAmt.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.txtTicketAmt.PasswordMode = false;
            this.txtTicketAmt.ReadOnly = false;
            this.txtTicketAmt.Size = new System.Drawing.Size(210, 28);
            this.txtTicketAmt.TabIndex = 35;
            this.txtTicketAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(14, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 28);
            this.label4.TabIndex = 34;
            this.label4.Text = "권종금액";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTicketTotalAmt
            // 
            this.txtTicketTotalAmt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtTicketTotalAmt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtTicketTotalAmt.BorderWidth = 1;
            this.txtTicketTotalAmt.Corner = 1;
            this.txtTicketTotalAmt.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtTicketTotalAmt.Focusable = false;
            this.txtTicketTotalAmt.FocusedIndex = 4;
            this.txtTicketTotalAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtTicketTotalAmt.Format = "#,##0";
            this.txtTicketTotalAmt.HasBorder = true;
            this.txtTicketTotalAmt.IsFocused = false;
            this.txtTicketTotalAmt.Location = new System.Drawing.Point(432, 91);
            this.txtTicketTotalAmt.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtTicketTotalAmt.MaxLength = 9;
            this.txtTicketTotalAmt.Name = "txtTicketTotalAmt";
            this.txtTicketTotalAmt.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.txtTicketTotalAmt.PasswordMode = false;
            this.txtTicketTotalAmt.ReadOnly = true;
            this.txtTicketTotalAmt.Size = new System.Drawing.Size(150, 28);
            this.txtTicketTotalAmt.TabIndex = 37;
            this.txtTicketTotalAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(330, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 28);
            this.label5.TabIndex = 36;
            this.label5.Text = "상품권합계";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.IsHighlight = false;
            this.btnClose.Location = new System.Drawing.Point(360, 500);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 38;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grd
            // 
            this.grd.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.grd.BorderWidth = new System.Windows.Forms.Padding(1);
            this.grd.CurrentRowIndex = -1;
            this.grd.Location = new System.Drawing.Point(17, 199);
            this.grd.Name = "grd";
            this.grd.Padding = new System.Windows.Forms.Padding(1);
            this.grd.PageIndex = 0;
            this.grd.ScrollType = WSWD.WmallPos.POS.FX.Win.UserControls.GridScrollType.Row;
            this.grd.Size = new System.Drawing.Size(565, 282);
            this.grd.TabIndex = 39;
            // 
            // btnType
            // 
            this.btnType.BorderSize = 1;
            this.btnType.Corner = 3;
            this.btnType.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnType.Image = null;
            this.btnType.IsHighlight = false;
            this.btnType.Location = new System.Drawing.Point(432, 8);
            this.btnType.Name = "btnType";
            this.btnType.Selected = false;
            this.btnType.Size = new System.Drawing.Size(100, 42);
            this.btnType.TabIndex = 42;
            this.btnType.Text = "상품권확인";
            this.btnType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtType
            // 
            this.txtType.BackColor = System.Drawing.Color.White;
            this.txtType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtType.BorderWidth = 1;
            this.txtType.Corner = 1;
            this.txtType.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtType.Focusable = true;
            this.txtType.FocusedIndex = 1;
            this.txtType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtType.Format = "";
            this.txtType.HasBorder = true;
            this.txtType.IsFocused = false;
            this.txtType.Location = new System.Drawing.Point(110, 17);
            this.txtType.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtType.MaxLength = 2;
            this.txtType.Name = "txtType";
            this.txtType.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.txtType.PasswordMode = false;
            this.txtType.ReadOnly = false;
            this.txtType.Size = new System.Drawing.Size(40, 28);
            this.txtType.TabIndex = 43;
            this.txtType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblType
            // 
            this.lblType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.lblType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.lblType.BorderWidth = 1;
            this.lblType.Corner = 1;
            this.lblType.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.lblType.Focusable = false;
            this.lblType.FocusedIndex = 1;
            this.lblType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblType.Format = "";
            this.lblType.HasBorder = true;
            this.lblType.IsFocused = false;
            this.lblType.Location = new System.Drawing.Point(156, 17);
            this.lblType.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.lblType.Name = "lblType";
            this.lblType.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.lblType.PasswordMode = false;
            this.lblType.ReadOnly = true;
            this.lblType.Size = new System.Drawing.Size(264, 28);
            this.lblType.TabIndex = 44;
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // POS_PY_P007
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(605, 606);
            this.Name = "POS_PY_P007";
            this.Text = "타사상품권";
            this.ContainerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton roundedButton2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar msgBar;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnSave;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtTicketNo;
        private System.Windows.Forms.Label label1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtTicketTotalAmt;
        private System.Windows.Forms.Label label5;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtTicketAmt;
        private System.Windows.Forms.Label label4;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtGetAmt;
        private System.Windows.Forms.Label label2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel grd;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnType;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtType;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText lblType;
    }
}