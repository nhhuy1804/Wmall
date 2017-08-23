namespace WSWD.WmallPos.POS.PY.VC
{
    partial class POS_PY_P012
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_PY_P012));
            this.roundedButton2 = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.btnOK = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnRetry = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlBound = new System.Windows.Forms.Panel();
            this.pnlCurrencySelect = new System.Windows.Forms.Panel();
            this.btnOtherCUR = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnKRW = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.lblMarkup = new System.Windows.Forms.Label();
            this.lblExRate = new System.Windows.Forms.Label();
            this.lblOthAmt = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.axKSNet_Dongle1 = new AxKSNET_DONGLELib.AxKSNet_Dongle();
            this.ButtonsPanel.SuspendLayout();
            this.ContainerPanel.SuspendLayout();
            this.pnlBound.SuspendLayout();
            this.pnlCurrencySelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axKSNet_Dongle1)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.axKSNet_Dongle1);
            this.ButtonsPanel.Controls.Add(this.btnRetry);
            this.ButtonsPanel.Controls.Add(this.btnClose);
            this.ButtonsPanel.Controls.Add(this.btnOK);
            this.ButtonsPanel.Size = new System.Drawing.Size(451, 62);
            // 
            // MessageBar
            // 
            this.MessageBar.Location = new System.Drawing.Point(17, 213);
            this.MessageBar.Size = new System.Drawing.Size(451, 46);
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.pnlBound);
            this.ContainerPanel.Controls.Add(this.label1);
            this.ContainerPanel.Size = new System.Drawing.Size(485, 338);
            this.ContainerPanel.Controls.SetChildIndex(this.MessageBar, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.label1, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.pnlBound, 0);
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
            // btnOK
            // 
            this.btnOK.BorderSize = 1;
            this.btnOK.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnOK.Corner = 3;
            this.btnOK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnOK.Image = null;
            this.btnOK.IsHighlight = false;
            this.btnOK.KeyType = WSWD.WmallPos.POS.FX.Win.UserControls.KeyButtonTypes.Enter;
            this.btnOK.Location = new System.Drawing.Point(180, 20);
            this.btnOK.Name = "btnOK";
            this.btnOK.Selected = false;
            this.btnOK.Size = new System.Drawing.Size(90, 42);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "확인";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.IsHighlight = false;
            this.btnClose.KeyType = WSWD.WmallPos.POS.FX.Win.UserControls.KeyButtonTypes.Clear;
            this.btnClose.Location = new System.Drawing.Point(292, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRetry
            // 
            this.btnRetry.BorderSize = 1;
            this.btnRetry.Corner = 3;
            this.btnRetry.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnRetry.Image = null;
            this.btnRetry.IsHighlight = false;
            this.btnRetry.Location = new System.Drawing.Point(69, 20);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Selected = false;
            this.btnRetry.Size = new System.Drawing.Size(90, 42);
            this.btnRetry.TabIndex = 4;
            this.btnRetry.Text = "재시도";
            this.btnRetry.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(17, 200);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(451, 13);
            this.label1.TabIndex = 14;
            // 
            // pnlBound
            // 
            this.pnlBound.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(210)))), ((int)(((byte)(211)))));
            this.pnlBound.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBound.Controls.Add(this.pnlCurrencySelect);
            this.pnlBound.Controls.Add(this.lblMarkup);
            this.pnlBound.Controls.Add(this.lblExRate);
            this.pnlBound.Controls.Add(this.lblOthAmt);
            this.pnlBound.Controls.Add(this.lblAmount);
            this.pnlBound.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBound.Location = new System.Drawing.Point(17, 17);
            this.pnlBound.Name = "pnlBound";
            this.pnlBound.Size = new System.Drawing.Size(451, 183);
            this.pnlBound.TabIndex = 15;
            // 
            // pnlCurrencySelect
            // 
            this.pnlCurrencySelect.Controls.Add(this.btnOtherCUR);
            this.pnlCurrencySelect.Controls.Add(this.btnKRW);
            this.pnlCurrencySelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCurrencySelect.Location = new System.Drawing.Point(0, 120);
            this.pnlCurrencySelect.Name = "pnlCurrencySelect";
            this.pnlCurrencySelect.Size = new System.Drawing.Size(449, 61);
            this.pnlCurrencySelect.TabIndex = 13;
            // 
            // btnOtherCUR
            // 
            this.btnOtherCUR.BorderSize = 1;
            this.btnOtherCUR.Corner = 3;
            this.btnOtherCUR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnOtherCUR.Image = null;
            this.btnOtherCUR.IsHighlight = false;
            this.btnOtherCUR.Location = new System.Drawing.Point(291, 12);
            this.btnOtherCUR.Name = "btnOtherCUR";
            this.btnOtherCUR.Selected = false;
            this.btnOtherCUR.Size = new System.Drawing.Size(90, 42);
            this.btnOtherCUR.TabIndex = 1;
            this.btnOtherCUR.Text = "JPY";
            this.btnOtherCUR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnKRW
            // 
            this.btnKRW.BorderSize = 1;
            this.btnKRW.Corner = 3;
            this.btnKRW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnKRW.Image = null;
            this.btnKRW.IsHighlight = false;
            this.btnKRW.Location = new System.Drawing.Point(68, 12);
            this.btnKRW.Name = "btnKRW";
            this.btnKRW.Selected = false;
            this.btnKRW.Size = new System.Drawing.Size(90, 42);
            this.btnKRW.TabIndex = 0;
            this.btnKRW.Text = "KRW";
            this.btnKRW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMarkup
            // 
            this.lblMarkup.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMarkup.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.lblMarkup.Location = new System.Drawing.Point(0, 90);
            this.lblMarkup.Name = "lblMarkup";
            this.lblMarkup.Size = new System.Drawing.Size(449, 30);
            this.lblMarkup.TabIndex = 12;
            this.lblMarkup.Text = "Markup included in FX  : 3.0%";
            this.lblMarkup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblExRate
            // 
            this.lblExRate.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblExRate.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.lblExRate.Location = new System.Drawing.Point(0, 60);
            this.lblExRate.Name = "lblExRate";
            this.lblExRate.Size = new System.Drawing.Size(449, 30);
            this.lblExRate.TabIndex = 11;
            this.lblExRate.Text = "JPY100 = KRW 1,048.8440";
            this.lblExRate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOthAmt
            // 
            this.lblOthAmt.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOthAmt.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.lblOthAmt.Location = new System.Drawing.Point(0, 30);
            this.lblOthAmt.Name = "lblOthAmt";
            this.lblOthAmt.Size = new System.Drawing.Size(449, 30);
            this.lblOthAmt.TabIndex = 10;
            this.lblOthAmt.Text = "JPY 9,534";
            this.lblOthAmt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAmount
            // 
            this.lblAmount.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAmount.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.lblAmount.Location = new System.Drawing.Point(0, 0);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(449, 30);
            this.lblAmount.TabIndex = 9;
            this.lblAmount.Text = "KRW 100,000";
            this.lblAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // axKSNet_Dongle1
            // 
            this.axKSNet_Dongle1.Enabled = true;
            this.axKSNet_Dongle1.Location = new System.Drawing.Point(389, 20);
            this.axKSNet_Dongle1.Name = "axKSNet_Dongle1";
            this.axKSNet_Dongle1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKSNet_Dongle1.OcxState")));
            this.axKSNet_Dongle1.Size = new System.Drawing.Size(26, 18);
            this.axKSNet_Dongle1.TabIndex = 5;
            // 
            // POS_PY_P012
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(491, 384);
            this.Name = "POS_PY_P012";
            this.Text = "자국 통화 결제 선택(DCC)";
            this.ButtonsPanel.ResumeLayout(false);
            this.ContainerPanel.ResumeLayout(false);
            this.pnlBound.ResumeLayout(false);
            this.pnlCurrencySelect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axKSNet_Dongle1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton roundedButton2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnOK;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnRetry;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlBound;
        private System.Windows.Forms.Panel pnlCurrencySelect;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnOtherCUR;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnKRW;
        private System.Windows.Forms.Label lblMarkup;
        private System.Windows.Forms.Label lblExRate;
        private System.Windows.Forms.Label lblOthAmt;
        private System.Windows.Forms.Label lblAmount;
        private AxKSNET_DONGLELib.AxKSNet_Dongle axKSNet_Dongle1;
    }
}