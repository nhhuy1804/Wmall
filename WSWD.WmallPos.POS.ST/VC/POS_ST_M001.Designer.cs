namespace WSWD.WmallPos.POS.ST.VC
{
    partial class POS_ST_M001
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_ST_M001));
            this.borderPanel1 = new WSWD.WmallPos.POS.FX.Win.Controls.BorderPanel();
            this.osiKeyboard = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiScanner = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiPrinter = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiMSR = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiCDP = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiCashDrawer = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiLastTrxnNo = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.osiSystemTime = new WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem();
            this.tmCheckSysTime = new System.Windows.Forms.Timer(this.components);
            this.axKSNet_Dongle1 = new AxKSNET_DONGLELib.AxKSNet_Dongle();
            this.borderPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axKSNet_Dongle1)).BeginInit();
            this.SuspendLayout();
            // 
            // borderPanel1
            // 
            this.borderPanel1.AutoSize = true;
            this.borderPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.borderPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.borderPanel1.BorderWidth = new System.Windows.Forms.Padding(1);
            this.borderPanel1.Controls.Add(this.osiKeyboard);
            this.borderPanel1.Controls.Add(this.osiScanner);
            this.borderPanel1.Controls.Add(this.osiPrinter);
            this.borderPanel1.Controls.Add(this.osiMSR);
            this.borderPanel1.Controls.Add(this.osiCDP);
            this.borderPanel1.Controls.Add(this.osiCashDrawer);
            this.borderPanel1.Controls.Add(this.osiLastTrxnNo);
            this.borderPanel1.Controls.Add(this.osiSystemTime);
            this.borderPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.borderPanel1.Location = new System.Drawing.Point(40, 310);
            this.borderPanel1.Name = "borderPanel1";
            this.borderPanel1.Padding = new System.Windows.Forms.Padding(3);
            this.borderPanel1.Size = new System.Drawing.Size(659, 342);
            this.borderPanel1.TabIndex = 5;
            // 
            // osiKeyboard
            // 
            this.osiKeyboard.BorderColor = System.Drawing.Color.White;
            this.osiKeyboard.BorderWidth = new System.Windows.Forms.Padding(0);
            this.osiKeyboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.osiKeyboard.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiKeyboard.Location = new System.Drawing.Point(3, 297);
            this.osiKeyboard.MessageText = "";
            this.osiKeyboard.Name = "osiKeyboard";
            this.osiKeyboard.Size = new System.Drawing.Size(653, 42);
            this.osiKeyboard.TabIndex = 5;
            // 
            // osiScanner
            // 
            this.osiScanner.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(204)))));
            this.osiScanner.BorderWidth = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.osiScanner.Dock = System.Windows.Forms.DockStyle.Top;
            this.osiScanner.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiScanner.Location = new System.Drawing.Point(3, 255);
            this.osiScanner.MessageText = "";
            this.osiScanner.Name = "osiScanner";
            this.osiScanner.Size = new System.Drawing.Size(653, 42);
            this.osiScanner.TabIndex = 1;
            // 
            // osiPrinter
            // 
            this.osiPrinter.BorderColor = System.Drawing.Color.White;
            this.osiPrinter.BorderWidth = new System.Windows.Forms.Padding(0);
            this.osiPrinter.Dock = System.Windows.Forms.DockStyle.Top;
            this.osiPrinter.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiPrinter.Location = new System.Drawing.Point(3, 213);
            this.osiPrinter.MessageText = "";
            this.osiPrinter.Name = "osiPrinter";
            this.osiPrinter.Size = new System.Drawing.Size(653, 42);
            this.osiPrinter.TabIndex = 0;
            // 
            // osiMSR
            // 
            this.osiMSR.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(204)))));
            this.osiMSR.BorderWidth = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.osiMSR.Dock = System.Windows.Forms.DockStyle.Top;
            this.osiMSR.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiMSR.Location = new System.Drawing.Point(3, 171);
            this.osiMSR.MessageText = "";
            this.osiMSR.Name = "osiMSR";
            this.osiMSR.Size = new System.Drawing.Size(653, 42);
            this.osiMSR.TabIndex = 3;
            // 
            // osiCDP
            // 
            this.osiCDP.BorderColor = System.Drawing.Color.White;
            this.osiCDP.BorderWidth = new System.Windows.Forms.Padding(0);
            this.osiCDP.Dock = System.Windows.Forms.DockStyle.Top;
            this.osiCDP.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiCDP.Location = new System.Drawing.Point(3, 129);
            this.osiCDP.MessageText = "";
            this.osiCDP.Name = "osiCDP";
            this.osiCDP.Size = new System.Drawing.Size(653, 42);
            this.osiCDP.TabIndex = 2;
            // 
            // osiCashDrawer
            // 
            this.osiCashDrawer.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(204)))));
            this.osiCashDrawer.BorderWidth = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.osiCashDrawer.Dock = System.Windows.Forms.DockStyle.Top;
            this.osiCashDrawer.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiCashDrawer.Location = new System.Drawing.Point(3, 87);
            this.osiCashDrawer.MessageText = "";
            this.osiCashDrawer.Name = "osiCashDrawer";
            this.osiCashDrawer.Size = new System.Drawing.Size(653, 42);
            this.osiCashDrawer.TabIndex = 4;
            // 
            // osiLastTrxnNo
            // 
            this.osiLastTrxnNo.BorderColor = System.Drawing.Color.White;
            this.osiLastTrxnNo.BorderWidth = new System.Windows.Forms.Padding(0);
            this.osiLastTrxnNo.Dock = System.Windows.Forms.DockStyle.Top;
            this.osiLastTrxnNo.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiLastTrxnNo.Location = new System.Drawing.Point(3, 45);
            this.osiLastTrxnNo.MessageText = "";
            this.osiLastTrxnNo.Name = "osiLastTrxnNo";
            this.osiLastTrxnNo.Size = new System.Drawing.Size(653, 42);
            this.osiLastTrxnNo.TabIndex = 8;
            // 
            // osiSystemTime
            // 
            this.osiSystemTime.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(204)))));
            this.osiSystemTime.BorderWidth = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.osiSystemTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.osiSystemTime.ItemStatus = WSWD.WmallPos.POS.FX.Win.UserControls.OpenItemStatus.None;
            this.osiSystemTime.Location = new System.Drawing.Point(3, 3);
            this.osiSystemTime.MessageText = "시스템시간 동기화 하는 중...";
            this.osiSystemTime.Name = "osiSystemTime";
            this.osiSystemTime.Size = new System.Drawing.Size(653, 42);
            this.osiSystemTime.TabIndex = 6;
            // 
            // tmCheckSysTime
            // 
            this.tmCheckSysTime.Interval = 500;
            // 
            // axKSNet_Dongle1
            // 
            this.axKSNet_Dongle1.Enabled = true;
            this.axKSNet_Dongle1.Location = new System.Drawing.Point(657, 263);
            this.axKSNet_Dongle1.Name = "axKSNet_Dongle1";
            this.axKSNet_Dongle1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKSNet_Dongle1.OcxState")));
            this.axKSNet_Dongle1.Size = new System.Drawing.Size(28, 28);
            this.axKSNet_Dongle1.TabIndex = 6;
            // 
            // POS_ST_M001
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::WSWD.WmallPos.POS.ST.Properties.Resources.bg_02_f;
            this.Controls.Add(this.axKSNet_Dongle1);
            this.Controls.Add(this.borderPanel1);
            this.DoubleBuffered = true;
            this.IsModal = true;
            this.Name = "POS_ST_M001";
            this.Padding = new System.Windows.Forms.Padding(40);
            this.borderPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axKSNet_Dongle1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.Controls.BorderPanel borderPanel1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiCDP;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiScanner;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiPrinter;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiKeyboard;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiCashDrawer;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiMSR;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiLastTrxnNo;
        private WSWD.WmallPos.POS.FX.Win.UserControls.OpenStatusItem osiSystemTime;
        private System.Windows.Forms.Timer tmCheckSysTime;
        private AxKSNET_DONGLELib.AxKSNet_Dongle axKSNet_Dongle1;



    }
}
