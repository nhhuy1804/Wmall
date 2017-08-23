namespace WSWD.WmallPos.POS.IQ.VC
{
    partial class POS_IQ_P004
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_IQ_P004));
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnPrint = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.msgBar = new WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar();
            this.btnSearch = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSaleDate = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.txtPosNo = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.txtTrxnNo = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.txtPrefixCode = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPrint = new WSWD.WmallPos.POS.FX.Win.Controls.PrintLabelInfo();
            this.grd = new WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel();
            this.tmFallBackRead = new System.Windows.Forms.Timer(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.txtApprNo = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPBNo = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.IsHighlight = false;
            this.btnClose.Location = new System.Drawing.Point(639, 637);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPrint
            // 
            this.btnPrint.BorderSize = 1;
            this.btnPrint.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnPrint.Corner = 3;
            this.btnPrint.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnPrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnPrint.Image = null;
            this.btnPrint.IsHighlight = false;
            this.btnPrint.Location = new System.Drawing.Point(533, 637);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Selected = false;
            this.btnPrint.Size = new System.Drawing.Size(90, 42);
            this.btnPrint.TabIndex = 15;
            this.btnPrint.Text = "발행";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // msgBar
            // 
            this.msgBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.msgBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.msgBar.BorderWidth = new System.Windows.Forms.Padding(1);
            this.msgBar.ControlType = WSWD.WmallPos.POS.FX.Win.UserControls.MessageBarType.TypeStatus;
            this.msgBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.msgBar.Location = new System.Drawing.Point(10, 637);
            this.msgBar.MinimumSize = new System.Drawing.Size(0, 35);
            this.msgBar.Name = "msgBar";
            this.msgBar.Size = new System.Drawing.Size(507, 42);
            this.msgBar.TabIndex = 14;
            this.msgBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSearch
            // 
            this.btnSearch.BorderSize = 1;
            this.btnSearch.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnSearch.Corner = 3;
            this.btnSearch.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnSearch.Image = null;
            this.btnSearch.IsHighlight = false;
            this.btnSearch.Location = new System.Drawing.Point(639, 13);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Selected = false;
            this.btnSearch.Size = new System.Drawing.Size(90, 42);
            this.btnSearch.TabIndex = 17;
            this.btnSearch.Text = "검색";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(10, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 28);
            this.label1.TabIndex = 18;
            this.label1.Text = "조회일자";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(215, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 28);
            this.label2.TabIndex = 19;
            this.label2.Text = "POS번호";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(420, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 28);
            this.label3.TabIndex = 20;
            this.label3.Text = "거래번호";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSaleDate
            // 
            this.txtSaleDate.BackColor = System.Drawing.Color.White;
            this.txtSaleDate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtSaleDate.BorderWidth = 1;
            this.txtSaleDate.Corner = 1;
            this.txtSaleDate.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.DateTime;
            this.txtSaleDate.Focusable = true;
            this.txtSaleDate.FocusedIndex = 0;
            this.txtSaleDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtSaleDate.Format = null;
            this.txtSaleDate.HasBorder = true;
            this.txtSaleDate.IsFocused = false;
            this.txtSaleDate.Location = new System.Drawing.Point(95, 19);
            this.txtSaleDate.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtSaleDate.MaxLength = 8;
            this.txtSaleDate.Name = "txtSaleDate";
            this.txtSaleDate.PasswordMode = false;
            this.txtSaleDate.ReadOnly = false;
            this.txtSaleDate.Size = new System.Drawing.Size(110, 28);
            this.txtSaleDate.TabIndex = 21;
            this.txtSaleDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPosNo
            // 
            this.txtPosNo.BackColor = System.Drawing.Color.White;
            this.txtPosNo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtPosNo.BorderWidth = 1;
            this.txtPosNo.Corner = 1;
            this.txtPosNo.Focusable = true;
            this.txtPosNo.FocusedIndex = 1;
            this.txtPosNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtPosNo.Format = null;
            this.txtPosNo.HasBorder = true;
            this.txtPosNo.IsFocused = false;
            this.txtPosNo.Location = new System.Drawing.Point(300, 19);
            this.txtPosNo.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtPosNo.MaxLength = 4;
            this.txtPosNo.Name = "txtPosNo";
            this.txtPosNo.PasswordMode = false;
            this.txtPosNo.ReadOnly = false;
            this.txtPosNo.Size = new System.Drawing.Size(110, 28);
            this.txtPosNo.TabIndex = 22;
            this.txtPosNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtTrxnNo
            // 
            this.txtTrxnNo.BackColor = System.Drawing.Color.White;
            this.txtTrxnNo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtTrxnNo.BorderWidth = 1;
            this.txtTrxnNo.Corner = 1;
            this.txtTrxnNo.Focusable = true;
            this.txtTrxnNo.FocusedIndex = 2;
            this.txtTrxnNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtTrxnNo.Format = null;
            this.txtTrxnNo.HasBorder = true;
            this.txtTrxnNo.IsFocused = false;
            this.txtTrxnNo.Location = new System.Drawing.Point(505, 19);
            this.txtTrxnNo.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtTrxnNo.MaxLength = 6;
            this.txtTrxnNo.Name = "txtTrxnNo";
            this.txtTrxnNo.PasswordMode = false;
            this.txtTrxnNo.ReadOnly = false;
            this.txtTrxnNo.Size = new System.Drawing.Size(110, 28);
            this.txtTrxnNo.TabIndex = 23;
            this.txtTrxnNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPrefixCode
            // 
            this.txtPrefixCode.BackColor = System.Drawing.Color.White;
            this.txtPrefixCode.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtPrefixCode.BorderWidth = 1;
            this.txtPrefixCode.Corner = 1;
            this.txtPrefixCode.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtPrefixCode.Focusable = true;
            this.txtPrefixCode.FocusedIndex = 3;
            this.txtPrefixCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtPrefixCode.Format = null;
            this.txtPrefixCode.HasBorder = true;
            this.txtPrefixCode.IsFocused = false;
            this.txtPrefixCode.Location = new System.Drawing.Point(95, 55);
            this.txtPrefixCode.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtPrefixCode.MaxLength = 6;
            this.txtPrefixCode.Name = "txtPrefixCode";
            this.txtPrefixCode.PasswordMode = false;
            this.txtPrefixCode.ReadOnly = false;
            this.txtPrefixCode.Size = new System.Drawing.Size(110, 28);
            this.txtPrefixCode.TabIndex = 25;
            this.txtPrefixCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(10, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 28);
            this.label4.TabIndex = 24;
            this.label4.Text = "프리픽스";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPrint
            // 
            this.txtPrint.BackColor = System.Drawing.Color.White;
            this.txtPrint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPrint.Font = new System.Drawing.Font("돋움체", 9.75F);
            this.txtPrint.Location = new System.Drawing.Point(377, 106);
            this.txtPrint.Name = "txtPrint";
            this.txtPrint.Size = new System.Drawing.Size(355, 506);
            this.txtPrint.TabIndex = 28;
            // 
            // grd
            // 
            this.grd.AutoFillRows = false;
            this.grd.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.grd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grd.BorderWidth = new System.Windows.Forms.Padding(1);
            this.grd.ColumnCount = 0;
            this.grd.DisableSelection = false;
            this.grd.Location = new System.Drawing.Point(1, 106);
            this.grd.Name = "grd";
            this.grd.PageIndex = -1;
            this.grd.RowCount = 11;
            this.grd.RowHeight = 42;
            this.grd.ScrollType = WSWD.WmallPos.POS.FX.Win.UserControls.ScrollTypes.IndexChanged;
            this.grd.SelectedRowIndex = -1;
            this.grd.ShowPageNo = true;
            this.grd.Size = new System.Drawing.Size(376, 506);
            this.grd.TabIndex = 29;
            this.grd.UnSelectable = false;
            // 
            // tmFallBackRead
            // 
            this.tmFallBackRead.Interval = 1000;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(215, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 28);
            this.label5.TabIndex = 24;
            this.label5.Text = "승인번호";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtApprNo
            // 
            this.txtApprNo.BackColor = System.Drawing.Color.White;
            this.txtApprNo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtApprNo.BorderWidth = 1;
            this.txtApprNo.Corner = 1;
            this.txtApprNo.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.txtApprNo.Focusable = true;
            this.txtApprNo.FocusedIndex = 3;
            this.txtApprNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtApprNo.Format = null;
            this.txtApprNo.HasBorder = true;
            this.txtApprNo.IsFocused = false;
            this.txtApprNo.Location = new System.Drawing.Point(300, 55);
            this.txtApprNo.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtApprNo.MaxLength = 8;
            this.txtApprNo.Name = "txtApprNo";
            this.txtApprNo.PasswordMode = false;
            this.txtApprNo.ReadOnly = false;
            this.txtApprNo.Size = new System.Drawing.Size(110, 28);
            this.txtApprNo.TabIndex = 26;
            this.txtApprNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(420, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 28);
            this.label6.TabIndex = 20;
            this.label6.Text = "품번코드";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPBNo
            // 
            this.txtPBNo.BackColor = System.Drawing.Color.White;
            this.txtPBNo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtPBNo.BorderWidth = 1;
            this.txtPBNo.Corner = 1;
            this.txtPBNo.Focusable = true;
            this.txtPBNo.FocusedIndex = 2;
            this.txtPBNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtPBNo.Format = null;
            this.txtPBNo.HasBorder = true;
            this.txtPBNo.IsFocused = false;
            this.txtPBNo.Location = new System.Drawing.Point(505, 55);
            this.txtPBNo.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtPBNo.MaxLength = 6;
            this.txtPBNo.Name = "txtPBNo";
            this.txtPBNo.PasswordMode = false;
            this.txtPBNo.ReadOnly = false;
            this.txtPBNo.Size = new System.Drawing.Size(110, 28);
            this.txtPBNo.TabIndex = 27;
            this.txtPBNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // POS_IQ_P004
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Controls.Add(this.txtPrint);
            this.Controls.Add(this.grd);
            this.Controls.Add(this.txtApprNo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPrefixCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPBNo);
            this.Controls.Add(this.txtTrxnNo);
            this.Controls.Add(this.txtPosNo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSaleDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.msgBar);
            this.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.IsModal = true;
            this.Name = "POS_IQ_P004";
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnPrint;
        private WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar msgBar;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtSaleDate;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtPosNo;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtTrxnNo;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtPrefixCode;
        private System.Windows.Forms.Label label4;
        private WSWD.WmallPos.POS.FX.Win.Controls.PrintLabelInfo txtPrint;
        private WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel grd;
        private System.Windows.Forms.Timer tmFallBackRead;
        private System.Windows.Forms.Label label5;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtApprNo;
        private System.Windows.Forms.Label label6;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtPBNo;
    }
}
