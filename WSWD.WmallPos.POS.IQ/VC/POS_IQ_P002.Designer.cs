namespace WSWD.WmallPos.POS.IQ.VC
{
    partial class POS_IQ_P002
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
            this.msgBar = new WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar();
            this.btnPrint = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.img = new System.Windows.Forms.PictureBox();
            this.txtPrint = new WSWD.WmallPos.POS.FX.Win.Controls.PrintLabelInfo();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.img)).BeginInit();
            this.SuspendLayout();
            // 
            // msgBar
            // 
            this.msgBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.msgBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.msgBar.BorderWidth = new System.Windows.Forms.Padding(1);
            this.msgBar.ControlType = WSWD.WmallPos.POS.FX.Win.UserControls.MessageBarType.TypeStatus;
            this.msgBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.msgBar.Location = new System.Drawing.Point(23, 558);
            this.msgBar.MinimumSize = new System.Drawing.Size(0, 35);
            this.msgBar.Name = "msgBar";
            this.msgBar.Size = new System.Drawing.Size(692, 42);
            this.msgBar.TabIndex = 0;
            this.msgBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.btnPrint.Location = new System.Drawing.Point(518, 620);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Selected = false;
            this.btnPrint.Size = new System.Drawing.Size(90, 42);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "발행";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.IsHighlight = false;
            this.btnClose.Location = new System.Drawing.Point(624, 620);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 345F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.img, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtPrint, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(23, 28);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(691, 500);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // img
            // 
            this.img.BackColor = System.Drawing.Color.White;
            this.img.Dock = System.Windows.Forms.DockStyle.Fill;
            this.img.Image = global::WSWD.WmallPos.POS.IQ.Properties.Resources.w_mall_left2;
            this.img.Location = new System.Drawing.Point(0, 0);
            this.img.Margin = new System.Windows.Forms.Padding(0);
            this.img.Name = "img";
            this.img.Size = new System.Drawing.Size(345, 500);
            this.img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.img.TabIndex = 4;
            this.img.TabStop = false;
            // 
            // txtPrint
            // 
            this.txtPrint.BackColor = System.Drawing.Color.White;
            this.txtPrint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPrint.Location = new System.Drawing.Point(348, 3);
            this.txtPrint.Name = "txtPrint";
            this.txtPrint.Size = new System.Drawing.Size(340, 494);
            this.txtPrint.TabIndex = 5;
            // 
            // POS_IQ_P002
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.msgBar);
            this.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.IsModal = true;
            this.Name = "POS_IQ_P002";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.img)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar msgBar;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnPrint;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox img;
        private WSWD.WmallPos.POS.FX.Win.Controls.PrintLabelInfo txtPrint;
    }
}
