namespace WSWD.WmallPos.POS.ST.VC
{
    partial class POS_ST_P001
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
            this.incSaleDate = new WSWD.WmallPos.POS.FX.Win.UserControls.InputControl();
            this.incCurDate = new WSWD.WmallPos.POS.FX.Win.UserControls.InputControl();
            this.messageBar1 = new WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar();
            this.btnOpen = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.borderPanel1 = new WSWD.WmallPos.POS.FX.Win.Controls.BorderPanel();
            this.openStatusItem3 = new WSWD.WmallPos.POS.ST.VC.OpenStatusItem();
            this.openStatusItem2 = new WSWD.WmallPos.POS.ST.VC.OpenStatusItem();
            this.openStatusItem1 = new WSWD.WmallPos.POS.ST.VC.OpenStatusItem();
            this.borderPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // incSaleDate
            // 
            this.incSaleDate.BackColor = System.Drawing.Color.Transparent;
            this.incSaleDate.BorderColor = System.Drawing.Color.LightGray;
            this.incSaleDate.BorderWidth = new System.Windows.Forms.Padding(0);
            this.incSaleDate.Format = null;
            this.incSaleDate.InputType = WSWD.WmallPos.POS.FX.Win.UserControls.InputControlType.ReadOnly;
            this.incSaleDate.IsFocused = false;
            this.incSaleDate.Location = new System.Drawing.Point(72, 50);
            this.incSaleDate.MinimumSize = new System.Drawing.Size(288, 34);
            this.incSaleDate.Name = "incSaleDate";
            this.incSaleDate.Padding = new System.Windows.Forms.Padding(3);
            this.incSaleDate.Size = new System.Drawing.Size(288, 36);
            this.incSaleDate.TabIndex = 0;
            this.incSaleDate.Title = "영업일자";
            this.incSaleDate.TitleWidth = 80;
            // 
            // incCurDate
            // 
            this.incCurDate.BackColor = System.Drawing.Color.Transparent;
            this.incCurDate.BorderColor = System.Drawing.Color.LightGray;
            this.incCurDate.BorderWidth = new System.Windows.Forms.Padding(0);
            this.incCurDate.Format = null;
            this.incCurDate.InputType = WSWD.WmallPos.POS.FX.Win.UserControls.InputControlType.ReadOnly;
            this.incCurDate.IsFocused = false;
            this.incCurDate.Location = new System.Drawing.Point(72, 90);
            this.incCurDate.MinimumSize = new System.Drawing.Size(288, 34);
            this.incCurDate.Name = "incCurDate";
            this.incCurDate.Padding = new System.Windows.Forms.Padding(3);
            this.incCurDate.Size = new System.Drawing.Size(288, 36);
            this.incCurDate.TabIndex = 1;
            this.incCurDate.Title = "현재일자";
            this.incCurDate.TitleWidth = 80;
            // 
            // messageBar1
            // 
            this.messageBar1.BackColor = System.Drawing.SystemColors.Control;
            this.messageBar1.BorderColor = System.Drawing.Color.LightGray;
            this.messageBar1.BorderWidth = new System.Windows.Forms.Padding(1);
            this.messageBar1.ControlType = WSWD.WmallPos.POS.FX.Win.UserControls.MessageBarType.TypeStatus;
            this.messageBar1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.messageBar1.Location = new System.Drawing.Point(72, 153);
            this.messageBar1.MinimumSize = new System.Drawing.Size(0, 35);
            this.messageBar1.Name = "messageBar1";
            this.messageBar1.Size = new System.Drawing.Size(587, 35);
            this.messageBar1.TabIndex = 2;
            this.messageBar1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOpen
            // 
            this.btnOpen.BorderSize = 1;
            this.btnOpen.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnOpen.Corner = 3;
            this.btnOpen.Location = new System.Drawing.Point(276, 209);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Selected = false;
            this.btnOpen.Size = new System.Drawing.Size(90, 42);
            this.btnOpen.TabIndex = 3;
            this.btnOpen.Text = "개설";
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.Location = new System.Drawing.Point(376, 209);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "닫기";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // borderPanel1
            // 
            this.borderPanel1.BackColor = System.Drawing.Color.Transparent;
            this.borderPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.borderPanel1.BorderWidth = new System.Windows.Forms.Padding(1);
            this.borderPanel1.Controls.Add(this.openStatusItem3);
            this.borderPanel1.Controls.Add(this.openStatusItem2);
            this.borderPanel1.Controls.Add(this.openStatusItem1);
            this.borderPanel1.Location = new System.Drawing.Point(72, 353);
            this.borderPanel1.Name = "borderPanel1";
            this.borderPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.borderPanel1.Size = new System.Drawing.Size(587, 288);
            this.borderPanel1.TabIndex = 4;
            // 
            // openStatusItem3
            // 
            this.openStatusItem3.Dock = System.Windows.Forms.DockStyle.Top;
            this.openStatusItem3.ItemStatus = WSWD.WmallPos.POS.ST.OpenItemStatus.Error;
            this.openStatusItem3.Location = new System.Drawing.Point(5, 67);
            this.openStatusItem3.MessageText = "스케너 초기화 오류.";
            this.openStatusItem3.Name = "openStatusItem3";
            this.openStatusItem3.Size = new System.Drawing.Size(577, 31);
            this.openStatusItem3.TabIndex = 2;
            // 
            // openStatusItem2
            // 
            this.openStatusItem2.Dock = System.Windows.Forms.DockStyle.Top;
            this.openStatusItem2.ItemStatus = WSWD.WmallPos.POS.ST.OpenItemStatus.OK;
            this.openStatusItem2.Location = new System.Drawing.Point(5, 36);
            this.openStatusItem2.MessageText = "프린터 초기화 완료";
            this.openStatusItem2.Name = "openStatusItem2";
            this.openStatusItem2.Size = new System.Drawing.Size(577, 31);
            this.openStatusItem2.TabIndex = 1;
            // 
            // openStatusItem1
            // 
            this.openStatusItem1.Dock = System.Windows.Forms.DockStyle.Top;
            this.openStatusItem1.ItemStatus = WSWD.WmallPos.POS.ST.OpenItemStatus.OK;
            this.openStatusItem1.Location = new System.Drawing.Point(5, 5);
            this.openStatusItem1.MessageText = "개설 승인 확인 완료";
            this.openStatusItem1.Name = "openStatusItem1";
            this.openStatusItem1.Size = new System.Drawing.Size(577, 31);
            this.openStatusItem1.TabIndex = 0;
            // 
            // POS_ST_P001
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.borderPanel1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.messageBar1);
            this.Controls.Add(this.incCurDate);
            this.Controls.Add(this.incSaleDate);
            this.Name = "POS_ST_P001";
            this.borderPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.UserControls.InputControl incSaleDate;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputControl incCurDate;
        private WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar messageBar1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnOpen;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private WSWD.WmallPos.POS.FX.Win.Controls.BorderPanel borderPanel1;
        private OpenStatusItem openStatusItem1;
        private OpenStatusItem openStatusItem3;
        private OpenStatusItem openStatusItem2;
    }
}
