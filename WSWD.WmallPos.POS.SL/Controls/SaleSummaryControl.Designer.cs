namespace WSWD.WmallPos.POS.SL.Controls
{
    partial class SaleSummaryControl
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
            this.lPnlPayList = new System.Windows.Forms.Panel();
            this.btnUp = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.btnDn = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.sumChange = new WSWD.WmallPos.POS.SL.Controls.SummaryItemText();
            this.sumPaidMoney = new WSWD.WmallPos.POS.SL.Controls.SummaryItemText();
            this.sumRecvMoney = new WSWD.WmallPos.POS.SL.Controls.SummaryItemText();
            this.sumDisc = new WSWD.WmallPos.POS.SL.Controls.SummaryItemText();
            this.sumTotalAmt = new WSWD.WmallPos.POS.SL.Controls.SummaryItemText();
            this.salePayItem4 = new WSWD.WmallPos.POS.SL.Controls.SalePayItem();
            this.salePayItem3 = new WSWD.WmallPos.POS.SL.Controls.SalePayItem();
            this.salePayItem2 = new WSWD.WmallPos.POS.SL.Controls.SalePayItem();
            this.salePayItem1 = new WSWD.WmallPos.POS.SL.Controls.SalePayItem();
            this.lPnlPayList.SuspendLayout();
            this.SuspendLayout();
            // 
            // lPnlPayList
            // 
            this.lPnlPayList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.lPnlPayList.Controls.Add(this.salePayItem4);
            this.lPnlPayList.Controls.Add(this.salePayItem3);
            this.lPnlPayList.Controls.Add(this.salePayItem2);
            this.lPnlPayList.Controls.Add(this.salePayItem1);
            this.lPnlPayList.Location = new System.Drawing.Point(26, 228);
            this.lPnlPayList.Name = "lPnlPayList";
            this.lPnlPayList.Size = new System.Drawing.Size(247, 92);
            this.lPnlPayList.TabIndex = 22;
            // 
            // btnUp
            // 
            this.btnUp.BorderSize = 1;
            this.btnUp.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type04;
            this.btnUp.Corner = 0;
            this.btnUp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnUp.Image = global::WSWD.WmallPos.POS.SL.Properties.Resources.ico_list_up;
            this.btnUp.IsHighlight = false;
            this.btnUp.Location = new System.Drawing.Point(276, 224);
            this.btnUp.Name = "btnUp";
            this.btnUp.Selected = false;
            this.btnUp.Size = new System.Drawing.Size(33, 50);
            this.btnUp.TabIndex = 1;
            this.btnUp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDn
            // 
            this.btnDn.BorderSize = 1;
            this.btnDn.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type04;
            this.btnDn.Corner = 0;
            this.btnDn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnDn.Image = global::WSWD.WmallPos.POS.SL.Properties.Resources.ico_list_dn;
            this.btnDn.IsHighlight = false;
            this.btnDn.Location = new System.Drawing.Point(276, 274);
            this.btnDn.Name = "btnDn";
            this.btnDn.Selected = false;
            this.btnDn.Size = new System.Drawing.Size(33, 50);
            this.btnDn.TabIndex = 23;
            this.btnDn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sumChange
            // 
            this.sumChange.EvenPosition = false;
            this.sumChange.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(59)))), ((int)(((byte)(57)))));
            this.sumChange.Location = new System.Drawing.Point(106, 157);
            this.sumChange.Name = "sumChange";
            this.sumChange.Size = new System.Drawing.Size(186, 29);
            this.sumChange.TabIndex = 21;
            this.sumChange.Text = "0";
            this.sumChange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sumPaidMoney
            // 
            this.sumPaidMoney.EvenPosition = true;
            this.sumPaidMoney.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.sumPaidMoney.Location = new System.Drawing.Point(106, 121);
            this.sumPaidMoney.Name = "sumPaidMoney";
            this.sumPaidMoney.Size = new System.Drawing.Size(186, 29);
            this.sumPaidMoney.TabIndex = 20;
            this.sumPaidMoney.Text = "0";
            this.sumPaidMoney.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sumRecvMoney
            // 
            this.sumRecvMoney.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.sumRecvMoney.EvenPosition = false;
            this.sumRecvMoney.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(59)))), ((int)(((byte)(57)))));
            this.sumRecvMoney.Location = new System.Drawing.Point(106, 85);
            this.sumRecvMoney.Name = "sumRecvMoney";
            this.sumRecvMoney.Size = new System.Drawing.Size(186, 29);
            this.sumRecvMoney.TabIndex = 19;
            this.sumRecvMoney.Text = "0";
            this.sumRecvMoney.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sumDisc
            // 
            this.sumDisc.EvenPosition = true;
            this.sumDisc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.sumDisc.Location = new System.Drawing.Point(106, 49);
            this.sumDisc.Name = "sumDisc";
            this.sumDisc.Size = new System.Drawing.Size(186, 29);
            this.sumDisc.TabIndex = 18;
            this.sumDisc.Text = "0";
            this.sumDisc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sumTotalAmt
            // 
            this.sumTotalAmt.EvenPosition = true;
            this.sumTotalAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.sumTotalAmt.Location = new System.Drawing.Point(106, 13);
            this.sumTotalAmt.Name = "sumTotalAmt";
            this.sumTotalAmt.Size = new System.Drawing.Size(186, 29);
            this.sumTotalAmt.TabIndex = 17;
            this.sumTotalAmt.Text = "0";
            this.sumTotalAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // salePayItem4
            // 
            this.salePayItem4.Dock = System.Windows.Forms.DockStyle.Top;
            this.salePayItem4.Location = new System.Drawing.Point(0, 69);
            this.salePayItem4.Name = "salePayItem4";
            this.salePayItem4.PayAmt = ((long)(0));
            this.salePayItem4.PayItemKey = null;
            this.salePayItem4.PayItemName = "";
            this.salePayItem4.Size = new System.Drawing.Size(247, 23);
            this.salePayItem4.TabIndex = 3;
            // 
            // salePayItem3
            // 
            this.salePayItem3.Dock = System.Windows.Forms.DockStyle.Top;
            this.salePayItem3.Location = new System.Drawing.Point(0, 46);
            this.salePayItem3.Name = "salePayItem3";
            this.salePayItem3.PayAmt = ((long)(0));
            this.salePayItem3.PayItemKey = null;
            this.salePayItem3.PayItemName = "";
            this.salePayItem3.Size = new System.Drawing.Size(247, 23);
            this.salePayItem3.TabIndex = 2;
            // 
            // salePayItem2
            // 
            this.salePayItem2.Dock = System.Windows.Forms.DockStyle.Top;
            this.salePayItem2.Location = new System.Drawing.Point(0, 23);
            this.salePayItem2.Name = "salePayItem2";
            this.salePayItem2.PayAmt = ((long)(0));
            this.salePayItem2.PayItemKey = null;
            this.salePayItem2.PayItemName = "";
            this.salePayItem2.Size = new System.Drawing.Size(247, 23);
            this.salePayItem2.TabIndex = 1;
            // 
            // salePayItem1
            // 
            this.salePayItem1.Dock = System.Windows.Forms.DockStyle.Top;
            this.salePayItem1.Location = new System.Drawing.Point(0, 0);
            this.salePayItem1.Name = "salePayItem1";
            this.salePayItem1.PayAmt = ((long)(0));
            this.salePayItem1.PayItemKey = null;
            this.salePayItem1.PayItemName = "";
            this.salePayItem1.Size = new System.Drawing.Size(247, 23);
            this.salePayItem1.TabIndex = 0;
            // 
            // SaleSummaryControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::WSWD.WmallPos.POS.SL.Properties.Resources.bg_pay_sum;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(123)))), ((int)(((byte)(190)))));
            this.BorderWidth = new System.Windows.Forms.Padding(1);
            this.Controls.Add(this.btnDn);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.sumChange);
            this.Controls.Add(this.sumPaidMoney);
            this.Controls.Add(this.sumRecvMoney);
            this.Controls.Add(this.sumDisc);
            this.Controls.Add(this.sumTotalAmt);
            this.Controls.Add(this.lPnlPayList);
            this.Name = "SaleSummaryControl";
            this.Size = new System.Drawing.Size(319, 336);
            this.lPnlPayList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SummaryItemText sumChange;
        private SummaryItemText sumPaidMoney;
        private SummaryItemText sumRecvMoney;
        private SummaryItemText sumDisc;
        private SummaryItemText sumTotalAmt;
        private System.Windows.Forms.Panel lPnlPayList;
        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton btnUp;
        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton btnDn;
        private SalePayItem salePayItem3;
        private SalePayItem salePayItem2;
        private SalePayItem salePayItem1;
        private SalePayItem salePayItem4;
    }
}
