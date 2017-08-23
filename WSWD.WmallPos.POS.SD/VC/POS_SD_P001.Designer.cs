namespace WSWD.WmallPos.POS.SD.VC
{
    partial class POS_SD_P001
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_SD_P001));
            this.messageBar1 = new WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar();
            this.borderPanel1 = new WSWD.WmallPos.POS.FX.Win.Controls.BorderPanel();
            this.osiSystemTime = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiCashDrawer = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiCDP = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiMSR = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiPrinter = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiScanner = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiKeyboard = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.openStatusItem1 = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.incSaleDate = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.incCurDate = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.borderPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // messageBar1
            // 
            this.messageBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.messageBar1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.messageBar1.BorderWidth = new System.Windows.Forms.Padding(1);
            this.messageBar1.ControlType = WSWD.WmallPos.POS.FX.Win.UserControls.MessageBarType.TypeStatus;
            this.messageBar1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.messageBar1.Location = new System.Drawing.Point(76, 203);
            this.messageBar1.MinimumSize = new System.Drawing.Size(0, 35);
            this.messageBar1.Name = "messageBar1";
            this.messageBar1.Size = new System.Drawing.Size(587, 42);
            this.messageBar1.TabIndex = 2;
            this.messageBar1.Text = "개설 작업 중입니다.";
            this.messageBar1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // borderPanel1
            // 
            this.borderPanel1.AutoSize = true;
            this.borderPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.borderPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.borderPanel1.BorderWidth = new System.Windows.Forms.Padding(1);
            this.borderPanel1.Controls.Add(this.osiSystemTime);
            this.borderPanel1.Controls.Add(this.osiCashDrawer);
            this.borderPanel1.Controls.Add(this.osiCDP);
            this.borderPanel1.Controls.Add(this.osiMSR);
            this.borderPanel1.Controls.Add(this.osiPrinter);
            this.borderPanel1.Controls.Add(this.osiScanner);
            this.borderPanel1.Controls.Add(this.osiKeyboard);
            this.borderPanel1.Controls.Add(this.openStatusItem1);
            this.borderPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.borderPanel1.Location = new System.Drawing.Point(40, 306);
            this.borderPanel1.Name = "borderPanel1";
            this.borderPanel1.Padding = new System.Windows.Forms.Padding(3);
            this.borderPanel1.Size = new System.Drawing.Size(659, 346);
            this.borderPanel1.TabIndex = 6;
            // 
            // osiSystemTime
            // 
            this.osiSystemTime.BorderColor = System.Drawing.Color.White;
            this.osiSystemTime.BorderWidth = new System.Windows.Forms.Padding(0);
            this.osiSystemTime.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiSystemTime.Location = new System.Drawing.Point(3, 3);
            this.osiSystemTime.MessageText = "";
            this.osiSystemTime.Name = "osiSystemTime";
            this.osiSystemTime.Size = new System.Drawing.Size(653, 42);
            this.osiSystemTime.TabIndex = 6;
            // 
            // osiCashDrawer
            // 
            this.osiCashDrawer.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(204)))));
            this.osiCashDrawer.BorderWidth = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.osiCashDrawer.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiCashDrawer.Location = new System.Drawing.Point(3, 45);
            this.osiCashDrawer.MessageText = "";
            this.osiCashDrawer.Name = "osiCashDrawer";
            this.osiCashDrawer.Size = new System.Drawing.Size(653, 42);
            this.osiCashDrawer.TabIndex = 4;
            // 
            // osiCDP
            // 
            this.osiCDP.BorderColor = System.Drawing.Color.White;
            this.osiCDP.BorderWidth = new System.Windows.Forms.Padding(0);
            this.osiCDP.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiCDP.Location = new System.Drawing.Point(3, 87);
            this.osiCDP.MessageText = "";
            this.osiCDP.Name = "osiCDP";
            this.osiCDP.Size = new System.Drawing.Size(653, 42);
            this.osiCDP.TabIndex = 2;
            // 
            // osiMSR
            // 
            this.osiMSR.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(204)))));
            this.osiMSR.BorderWidth = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.osiMSR.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiMSR.Location = new System.Drawing.Point(3, 129);
            this.osiMSR.MessageText = "";
            this.osiMSR.Name = "osiMSR";
            this.osiMSR.Size = new System.Drawing.Size(653, 42);
            this.osiMSR.TabIndex = 3;
            // 
            // osiPrinter
            // 
            this.osiPrinter.BorderColor = System.Drawing.Color.White;
            this.osiPrinter.BorderWidth = new System.Windows.Forms.Padding(0);
            this.osiPrinter.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiPrinter.Location = new System.Drawing.Point(3, 171);
            this.osiPrinter.MessageText = "";
            this.osiPrinter.Name = "osiPrinter";
            this.osiPrinter.Size = new System.Drawing.Size(653, 42);
            this.osiPrinter.TabIndex = 0;
            // 
            // osiScanner
            // 
            this.osiScanner.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(204)))));
            this.osiScanner.BorderWidth = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.osiScanner.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiScanner.Location = new System.Drawing.Point(3, 213);
            this.osiScanner.MessageText = "";
            this.osiScanner.Name = "osiScanner";
            this.osiScanner.Size = new System.Drawing.Size(653, 42);
            this.osiScanner.TabIndex = 1;
            // 
            // osiKeyboard
            // 
            this.osiKeyboard.BorderColor = System.Drawing.Color.White;
            this.osiKeyboard.BorderWidth = new System.Windows.Forms.Padding(0);
            this.osiKeyboard.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiKeyboard.Location = new System.Drawing.Point(3, 255);
            this.osiKeyboard.MessageText = "";
            this.osiKeyboard.Name = "osiKeyboard";
            this.osiKeyboard.Size = new System.Drawing.Size(653, 42);
            this.osiKeyboard.TabIndex = 5;
            // 
            // openStatusItem1
            // 
            this.openStatusItem1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(204)))));
            this.openStatusItem1.BorderWidth = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.openStatusItem1.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.openStatusItem1.Location = new System.Drawing.Point(3, 298);
            this.openStatusItem1.MessageText = "";
            this.openStatusItem1.Name = "openStatusItem1";
            this.openStatusItem1.Size = new System.Drawing.Size(653, 42);
            this.openStatusItem1.TabIndex = 7;
            // 
            // incSaleDate
            // 
            this.incSaleDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.incSaleDate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.incSaleDate.BorderWidth = 1;
            this.incSaleDate.Corner = 1;
            this.incSaleDate.Focusable = false;
            this.incSaleDate.FocusedIndex = 0;
            this.incSaleDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.incSaleDate.Format = null;
            this.incSaleDate.HasBorder = true;
            this.incSaleDate.IsFocused = false;
            this.incSaleDate.Location = new System.Drawing.Point(179, 97);
            this.incSaleDate.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.incSaleDate.Name = "incSaleDate";
            this.incSaleDate.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.incSaleDate.PasswordMode = false;
            this.incSaleDate.ReadOnly = true;
            this.incSaleDate.Size = new System.Drawing.Size(271, 30);
            this.incSaleDate.TabIndex = 7;
            this.incSaleDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // incCurDate
            // 
            this.incCurDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.incCurDate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.incCurDate.BorderWidth = 1;
            this.incCurDate.Corner = 1;
            this.incCurDate.Focusable = false;
            this.incCurDate.FocusedIndex = 0;
            this.incCurDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.incCurDate.Format = null;
            this.incCurDate.HasBorder = true;
            this.incCurDate.IsFocused = false;
            this.incCurDate.Location = new System.Drawing.Point(179, 142);
            this.incCurDate.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.incCurDate.Name = "incCurDate";
            this.incCurDate.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.incCurDate.PasswordMode = false;
            this.incCurDate.ReadOnly = true;
            this.incCurDate.Size = new System.Drawing.Size(271, 30);
            this.incCurDate.TabIndex = 8;
            this.incCurDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(93, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "영업일자";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(93, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "현재일자";
            // 
            // POS_SD_P001
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.incCurDate);
            this.Controls.Add(this.incSaleDate);
            this.Controls.Add(this.borderPanel1);
            this.Controls.Add(this.messageBar1);
            this.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.Name = "POS_SD_P001";
            this.Padding = new System.Windows.Forms.Padding(40);
            this.borderPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar messageBar1;
        private WSWD.WmallPos.POS.FX.Win.Controls.BorderPanel borderPanel1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiKeyboard;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiScanner;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiPrinter;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiMSR;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiCDP;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiCashDrawer;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiSystemTime;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem openStatusItem1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText incSaleDate;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText incCurDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
