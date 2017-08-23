namespace WSWD.WmallPos.POS.ED.VC
{
    partial class POS_ED_P003
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_ED_P003));
            this.timerChk = new System.Windows.Forms.Timer(this.components);
            this.txtSaleDate = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnRun = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.borderPanel1 = new WSWD.WmallPos.POS.FX.Win.Controls.BorderPanel();
            this.osiMsgBar07 = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiMsgBar06 = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiMsgBar05 = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiMsgBar04 = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiMsgBar03 = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiMsgBar02 = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiMsgBar01 = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.msgBar = new WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar();
            this.borderPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSaleDate
            // 
            this.txtSaleDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.txtSaleDate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtSaleDate.BorderWidth = 1;
            this.txtSaleDate.Corner = 1;
            this.txtSaleDate.Focusable = false;
            this.txtSaleDate.FocusedIndex = 0;
            this.txtSaleDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtSaleDate.Format = null;
            this.txtSaleDate.HasBorder = true;
            this.txtSaleDate.IsFocused = false;
            this.txtSaleDate.Location = new System.Drawing.Point(144, 110);
            this.txtSaleDate.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtSaleDate.Name = "txtSaleDate";
            this.txtSaleDate.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.txtSaleDate.PasswordMode = false;
            this.txtSaleDate.ReadOnly = true;
            this.txtSaleDate.Size = new System.Drawing.Size(150, 28);
            this.txtSaleDate.TabIndex = 19;
            this.txtSaleDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(67, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 28);
            this.label1.TabIndex = 17;
            this.label1.Text = "영업일자";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.IsHighlight = false;
            this.btnClose.Location = new System.Drawing.Point(377, 220);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRun
            // 
            this.btnRun.BorderSize = 1;
            this.btnRun.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnRun.Corner = 3;
            this.btnRun.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.btnRun.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnRun.Image = null;
            this.btnRun.IsHighlight = false;
            this.btnRun.Location = new System.Drawing.Point(271, 220);
            this.btnRun.Name = "btnRun";
            this.btnRun.Selected = false;
            this.btnRun.Size = new System.Drawing.Size(90, 42);
            this.btnRun.TabIndex = 15;
            this.btnRun.Text = "실행";
            this.btnRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // borderPanel1
            // 
            this.borderPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.borderPanel1.BackColor = System.Drawing.Color.Transparent;
            this.borderPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.borderPanel1.BorderWidth = new System.Windows.Forms.Padding(1);
            this.borderPanel1.Controls.Add(this.osiMsgBar07);
            this.borderPanel1.Controls.Add(this.osiMsgBar06);
            this.borderPanel1.Controls.Add(this.osiMsgBar05);
            this.borderPanel1.Controls.Add(this.osiMsgBar04);
            this.borderPanel1.Controls.Add(this.osiMsgBar03);
            this.borderPanel1.Controls.Add(this.osiMsgBar02);
            this.borderPanel1.Controls.Add(this.osiMsgBar01);
            this.borderPanel1.Location = new System.Drawing.Point(67, 282);
            this.borderPanel1.Name = "borderPanel1";
            this.borderPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.borderPanel1.Size = new System.Drawing.Size(604, 300);
            this.borderPanel1.TabIndex = 14;
            // 
            // osiMsgBar07
            // 
            this.osiMsgBar07.BorderColor = System.Drawing.Color.White;
            this.osiMsgBar07.BorderWidth = new System.Windows.Forms.Padding(0);
            this.osiMsgBar07.Dock = System.Windows.Forms.DockStyle.Top;
            this.osiMsgBar07.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiMsgBar07.Location = new System.Drawing.Point(1, 253);
            this.osiMsgBar07.MessageText = "";
            this.osiMsgBar07.Name = "osiMsgBar07";
            this.osiMsgBar07.Size = new System.Drawing.Size(602, 42);
            this.osiMsgBar07.TabIndex = 5;
            // 
            // osiMsgBar06
            // 
            this.osiMsgBar06.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(204)))));
            this.osiMsgBar06.BorderWidth = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.osiMsgBar06.Dock = System.Windows.Forms.DockStyle.Top;
            this.osiMsgBar06.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiMsgBar06.Location = new System.Drawing.Point(1, 211);
            this.osiMsgBar06.MessageText = "";
            this.osiMsgBar06.Name = "osiMsgBar06";
            this.osiMsgBar06.Size = new System.Drawing.Size(602, 42);
            this.osiMsgBar06.TabIndex = 4;
            // 
            // osiMsgBar05
            // 
            this.osiMsgBar05.BorderColor = System.Drawing.Color.White;
            this.osiMsgBar05.BorderWidth = new System.Windows.Forms.Padding(0);
            this.osiMsgBar05.Dock = System.Windows.Forms.DockStyle.Top;
            this.osiMsgBar05.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiMsgBar05.Location = new System.Drawing.Point(1, 169);
            this.osiMsgBar05.MessageText = "";
            this.osiMsgBar05.Name = "osiMsgBar05";
            this.osiMsgBar05.Size = new System.Drawing.Size(602, 42);
            this.osiMsgBar05.TabIndex = 3;
            // 
            // osiMsgBar04
            // 
            this.osiMsgBar04.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(204)))));
            this.osiMsgBar04.BorderWidth = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.osiMsgBar04.Dock = System.Windows.Forms.DockStyle.Top;
            this.osiMsgBar04.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiMsgBar04.Location = new System.Drawing.Point(1, 127);
            this.osiMsgBar04.MessageText = "";
            this.osiMsgBar04.Name = "osiMsgBar04";
            this.osiMsgBar04.Size = new System.Drawing.Size(602, 42);
            this.osiMsgBar04.TabIndex = 2;
            // 
            // osiMsgBar03
            // 
            this.osiMsgBar03.BorderColor = System.Drawing.Color.White;
            this.osiMsgBar03.BorderWidth = new System.Windows.Forms.Padding(0);
            this.osiMsgBar03.Dock = System.Windows.Forms.DockStyle.Top;
            this.osiMsgBar03.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiMsgBar03.Location = new System.Drawing.Point(1, 85);
            this.osiMsgBar03.MessageText = "";
            this.osiMsgBar03.Name = "osiMsgBar03";
            this.osiMsgBar03.Size = new System.Drawing.Size(602, 42);
            this.osiMsgBar03.TabIndex = 1;
            // 
            // osiMsgBar02
            // 
            this.osiMsgBar02.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(204)))));
            this.osiMsgBar02.BorderWidth = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.osiMsgBar02.Dock = System.Windows.Forms.DockStyle.Top;
            this.osiMsgBar02.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiMsgBar02.Location = new System.Drawing.Point(1, 43);
            this.osiMsgBar02.MessageText = "";
            this.osiMsgBar02.Name = "osiMsgBar02";
            this.osiMsgBar02.Size = new System.Drawing.Size(602, 42);
            this.osiMsgBar02.TabIndex = 0;
            // 
            // osiMsgBar01
            // 
            this.osiMsgBar01.BorderColor = System.Drawing.Color.White;
            this.osiMsgBar01.BorderWidth = new System.Windows.Forms.Padding(0);
            this.osiMsgBar01.Dock = System.Windows.Forms.DockStyle.Top;
            this.osiMsgBar01.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiMsgBar01.Location = new System.Drawing.Point(1, 1);
            this.osiMsgBar01.MessageText = "";
            this.osiMsgBar01.Name = "osiMsgBar01";
            this.osiMsgBar01.Size = new System.Drawing.Size(602, 42);
            this.osiMsgBar01.TabIndex = 6;
            // 
            // msgBar
            // 
            this.msgBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.msgBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.msgBar.BorderWidth = new System.Windows.Forms.Padding(1);
            this.msgBar.ControlType = WSWD.WmallPos.POS.FX.Win.UserControls.MessageBarType.TypeStatus;
            this.msgBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.msgBar.Location = new System.Drawing.Point(67, 158);
            this.msgBar.MinimumSize = new System.Drawing.Size(0, 35);
            this.msgBar.Name = "msgBar";
            this.msgBar.Size = new System.Drawing.Size(604, 42);
            this.msgBar.TabIndex = 13;
            this.msgBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // POS_ED_P003
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.txtSaleDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.borderPanel1);
            this.Controls.Add(this.msgBar);
            this.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.Name = "POS_ED_P003";
            this.borderPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerChk;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtSaleDate;
        private System.Windows.Forms.Label label1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnRun;
        private WSWD.WmallPos.POS.FX.Win.Controls.BorderPanel borderPanel1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiMsgBar07;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiMsgBar06;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiMsgBar05;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiMsgBar04;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiMsgBar03;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiMsgBar02;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiMsgBar01;
        private WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar msgBar;
    }
}
