namespace WSWD.WmallPos.POS.SL.VC
{
    partial class POS_SL_M001
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_SL_M001));
            this.gpItems = new WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel();
            this.saleKeyPad1 = new WSWD.WmallPos.POS.FX.Win.UserControls.SaleKeyPad();
            this.pnlKeyPad = new System.Windows.Forms.Panel();
            this.pnlStatusGuide = new System.Windows.Forms.Panel();
            this.guideMessage1 = new WSWD.WmallPos.POS.SL.Controls.GuideMessage();
            this.label1 = new System.Windows.Forms.Label();
            this.saleSummaryControl1 = new WSWD.WmallPos.POS.SL.Controls.SaleSummaryControl();
            this.KeyInputText = new WSWD.WmallPos.POS.SL.Controls.KeyInputText();
            this.pnlRightTop = new System.Windows.Forms.Panel();
            this.groupItemList1 = new WSWD.WmallPos.POS.SL.Controls.GroupItemList();
            this.saleItemGroup1 = new WSWD.WmallPos.POS.SL.Controls.SaleItemGroup();
            this.pnFuncKeyGroup = new System.Windows.Forms.Panel();
            this.funcKeyGroup1 = new WSWD.WmallPos.POS.SL.Controls.FuncKeyGroup();
            this.currentItemInfo1 = new WSWD.WmallPos.POS.SL.Controls.CurrentItemInfo();
            this.autoRtnPanel1 = new WSWD.WmallPos.POS.SL.Controls.AutoRtnPanel();
            this.pnlKeyPad.SuspendLayout();
            this.pnlStatusGuide.SuspendLayout();
            this.pnlRightTop.SuspendLayout();
            this.pnFuncKeyGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpItems
            // 
            this.gpItems.AutoFillRows = true;
            this.gpItems.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.gpItems.BorderWidth = new System.Windows.Forms.Padding(1);
            this.gpItems.ColumnCount = 7;
            this.gpItems.DisableSelection = false;
            this.gpItems.Location = new System.Drawing.Point(0, 58);
            this.gpItems.Name = "gpItems";
            this.gpItems.Padding = new System.Windows.Forms.Padding(1);
            this.gpItems.PageIndex = -1;
            this.gpItems.RowCount = 5;
            this.gpItems.RowHeight = 43;
            this.gpItems.ScrollType = WSWD.WmallPos.POS.FX.Win.UserControls.ScrollTypes.IndexChanged;
            this.gpItems.SelectedRowIndex = -1;
            this.gpItems.ShowPageNo = true;
            this.gpItems.Size = new System.Drawing.Size(594, 244);
            this.gpItems.TabIndex = 15;
            this.gpItems.UnSelectable = false;
            // 
            // saleKeyPad1
            // 
            this.saleKeyPad1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(171)))), ((int)(((byte)(231)))));
            this.saleKeyPad1.Location = new System.Drawing.Point(339, 90);
            this.saleKeyPad1.Margin = new System.Windows.Forms.Padding(0);
            this.saleKeyPad1.MaximumSize = new System.Drawing.Size(245, 292);
            this.saleKeyPad1.Name = "saleKeyPad1";
            this.saleKeyPad1.Size = new System.Drawing.Size(245, 292);
            this.saleKeyPad1.TabIndex = 14;
            // 
            // pnlKeyPad
            // 
            this.pnlKeyPad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(171)))), ((int)(((byte)(231)))));
            this.pnlKeyPad.Controls.Add(this.pnlStatusGuide);
            this.pnlKeyPad.Controls.Add(this.saleSummaryControl1);
            this.pnlKeyPad.Controls.Add(this.KeyInputText);
            this.pnlKeyPad.Controls.Add(this.saleKeyPad1);
            this.pnlKeyPad.Location = new System.Drawing.Point(0, 302);
            this.pnlKeyPad.Name = "pnlKeyPad";
            this.pnlKeyPad.Size = new System.Drawing.Size(594, 390);
            this.pnlKeyPad.TabIndex = 16;
            // 
            // pnlStatusGuide
            // 
            this.pnlStatusGuide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.pnlStatusGuide.Controls.Add(this.guideMessage1);
            this.pnlStatusGuide.Controls.Add(this.label1);
            this.pnlStatusGuide.Location = new System.Drawing.Point(0, 0);
            this.pnlStatusGuide.Name = "pnlStatusGuide";
            this.pnlStatusGuide.Size = new System.Drawing.Size(594, 40);
            this.pnlStatusGuide.TabIndex = 18;
            // 
            // guideMessage1
            // 
            this.guideMessage1.Location = new System.Drawing.Point(41, 6);
            this.guideMessage1.Name = "guideMessage1";
            this.guideMessage1.Size = new System.Drawing.Size(550, 32);
            this.guideMessage1.TabIndex = 18;
            this.guideMessage1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Image = global::WSWD.WmallPos.POS.SL.Properties.Resources.bullet;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 34);
            this.label1.TabIndex = 17;
            // 
            // saleSummaryControl1
            // 
            this.saleSummaryControl1.BackColor = System.Drawing.Color.White;
            this.saleSummaryControl1.BackgroundImage = global::WSWD.WmallPos.POS.SL.Properties.Resources.bg_pay_sum;
            this.saleSummaryControl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.saleSummaryControl1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(123)))), ((int)(((byte)(190)))));
            this.saleSummaryControl1.BorderWidth = new System.Windows.Forms.Padding(1);
            this.saleSummaryControl1.Location = new System.Drawing.Point(12, 48);
            this.saleSummaryControl1.Name = "saleSummaryControl1";
            this.saleSummaryControl1.RecvMoney = "0";
            this.saleSummaryControl1.Size = new System.Drawing.Size(317, 334);
            this.saleSummaryControl1.TabIndex = 16;
            this.saleSummaryControl1.TotalAmt = "0";
            // 
            // KeyInputText
            // 
            this.KeyInputText.BackColor = System.Drawing.Color.White;
            this.KeyInputText.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(109)))), ((int)(((byte)(195)))));
            this.KeyInputText.BorderWidth = 1;
            this.KeyInputText.Corner = 1;
            this.KeyInputText.Focusable = true;
            this.KeyInputText.FocusedBorderWidth = 1;
            this.KeyInputText.FocusedIndex = 0;
            this.KeyInputText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.KeyInputText.Format = null;
            this.KeyInputText.HasBorder = true;
            this.KeyInputText.IsFocused = true;
            this.KeyInputText.Location = new System.Drawing.Point(339, 48);
            this.KeyInputText.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.KeyInputText.Name = "KeyInputText";
            this.KeyInputText.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.KeyInputText.PasswordMode = false;
            this.KeyInputText.ReadOnly = false;
            this.KeyInputText.Size = new System.Drawing.Size(245, 34);
            this.KeyInputText.TabIndex = 15;
            this.KeyInputText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlRightTop
            // 
            this.pnlRightTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(201)))), ((int)(((byte)(239)))));
            this.pnlRightTop.Controls.Add(this.groupItemList1);
            this.pnlRightTop.Controls.Add(this.saleItemGroup1);
            this.pnlRightTop.Location = new System.Drawing.Point(593, 0);
            this.pnlRightTop.Name = "pnlRightTop";
            this.pnlRightTop.Padding = new System.Windows.Forms.Padding(8);
            this.pnlRightTop.Size = new System.Drawing.Size(430, 436);
            this.pnlRightTop.TabIndex = 17;
            // 
            // groupItemList1
            // 
            this.groupItemList1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(201)))), ((int)(((byte)(239)))));
            this.groupItemList1.Location = new System.Drawing.Point(8, 132);
            this.groupItemList1.MaximumSize = new System.Drawing.Size(413, 297);
            this.groupItemList1.MinimumSize = new System.Drawing.Size(413, 297);
            this.groupItemList1.Name = "groupItemList1";
            this.groupItemList1.Size = new System.Drawing.Size(413, 297);
            this.groupItemList1.TabIndex = 6;
            // 
            // saleItemGroup1
            // 
            this.saleItemGroup1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.saleItemGroup1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(201)))), ((int)(((byte)(239)))));
            this.saleItemGroup1.Location = new System.Drawing.Point(8, 8);
            this.saleItemGroup1.Margin = new System.Windows.Forms.Padding(0);
            this.saleItemGroup1.MaximumSize = new System.Drawing.Size(413, 117);
            this.saleItemGroup1.MinimumSize = new System.Drawing.Size(413, 117);
            this.saleItemGroup1.Name = "saleItemGroup1";
            this.saleItemGroup1.Size = new System.Drawing.Size(413, 117);
            this.saleItemGroup1.TabIndex = 5;
            // 
            // pnFuncKeyGroup
            // 
            this.pnFuncKeyGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(232)))), ((int)(((byte)(246)))));
            this.pnFuncKeyGroup.Controls.Add(this.funcKeyGroup1);
            this.pnFuncKeyGroup.Location = new System.Drawing.Point(593, 435);
            this.pnFuncKeyGroup.Name = "pnFuncKeyGroup";
            this.pnFuncKeyGroup.Padding = new System.Windows.Forms.Padding(8);
            this.pnFuncKeyGroup.Size = new System.Drawing.Size(430, 257);
            this.pnFuncKeyGroup.TabIndex = 18;
            // 
            // funcKeyGroup1
            // 
            this.funcKeyGroup1.Location = new System.Drawing.Point(8, 10);
            this.funcKeyGroup1.MaximumSize = new System.Drawing.Size(413, 237);
            this.funcKeyGroup1.MinimumSize = new System.Drawing.Size(413, 237);
            this.funcKeyGroup1.Name = "funcKeyGroup1";
            this.funcKeyGroup1.Size = new System.Drawing.Size(413, 237);
            this.funcKeyGroup1.TabIndex = 7;
            // 
            // currentItemInfo1
            // 
            this.currentItemInfo1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("currentItemInfo1.BackgroundImage")));
            this.currentItemInfo1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.currentItemInfo1.CdDp = null;
            this.currentItemInfo1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentItemInfo1.Location = new System.Drawing.Point(0, -1);
            this.currentItemInfo1.Margin = new System.Windows.Forms.Padding(0);
            this.currentItemInfo1.Name = "currentItemInfo1";
            this.currentItemInfo1.NmClass = "";
            this.currentItemInfo1.NmItem = "";
            this.currentItemInfo1.Size = new System.Drawing.Size(593, 60);
            this.currentItemInfo1.TabIndex = 20;
            // 
            // autoRtnPanel1
            // 
            this.autoRtnPanel1.Location = new System.Drawing.Point(593, 0);
            this.autoRtnPanel1.Name = "autoRtnPanel1";
            this.autoRtnPanel1.Size = new System.Drawing.Size(429, 693);
            this.autoRtnPanel1.TabIndex = 21;
            // 
            // POS_SL_M001
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.currentItemInfo1);
            this.Controls.Add(this.pnFuncKeyGroup);
            this.Controls.Add(this.pnlRightTop);
            this.Controls.Add(this.pnlKeyPad);
            this.Controls.Add(this.gpItems);
            this.Controls.Add(this.autoRtnPanel1);
            this.HideMainMenu = true;
            this.MinimumSize = new System.Drawing.Size(1022, 692);
            this.Name = "POS_SL_M001";
            this.Size = new System.Drawing.Size(1022, 692);
            this.Text = "판매등록";
            this.pnlKeyPad.ResumeLayout(false);
            this.pnlStatusGuide.ResumeLayout(false);
            this.pnlRightTop.ResumeLayout(false);
            this.pnFuncKeyGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel gpItems;
        private WSWD.WmallPos.POS.FX.Win.UserControls.SaleKeyPad saleKeyPad1;
        private WSWD.WmallPos.POS.SL.Controls.KeyInputText KeyInputText;
        private System.Windows.Forms.Panel pnlKeyPad;
        private WSWD.WmallPos.POS.SL.Controls.SaleSummaryControl saleSummaryControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlStatusGuide;
        private WSWD.WmallPos.POS.SL.Controls.GuideMessage guideMessage1;
        private System.Windows.Forms.Panel pnlRightTop;
        private WSWD.WmallPos.POS.SL.Controls.SaleItemGroup saleItemGroup1;
        private WSWD.WmallPos.POS.SL.Controls.GroupItemList groupItemList1;
        private System.Windows.Forms.Panel pnFuncKeyGroup;
        private WSWD.WmallPos.POS.SL.Controls.FuncKeyGroup funcKeyGroup1;
        private WSWD.WmallPos.POS.SL.Controls.CurrentItemInfo currentItemInfo1;
        private WSWD.WmallPos.POS.SL.Controls.AutoRtnPanel autoRtnPanel1;
    }
}