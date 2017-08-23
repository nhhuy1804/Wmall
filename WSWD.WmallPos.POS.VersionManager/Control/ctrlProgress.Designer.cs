namespace WSWD.WmallPos.POS.VersionManager.Control
{
    partial class ctrlProgress
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
            this.lblProgressFileNm = new System.Windows.Forms.Label();
            this.lblProgressCnt = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.pgBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lblProgressFileNm
            // 
            this.lblProgressFileNm.AutoSize = true;
            this.lblProgressFileNm.Location = new System.Drawing.Point(581, 16);
            this.lblProgressFileNm.Name = "lblProgressFileNm";
            this.lblProgressFileNm.Size = new System.Drawing.Size(121, 12);
            this.lblProgressFileNm.TabIndex = 11;
            this.lblProgressFileNm.Text = "파일을 저장중입니다.";
            // 
            // lblProgressCnt
            // 
            this.lblProgressCnt.Location = new System.Drawing.Point(501, 13);
            this.lblProgressCnt.Name = "lblProgressCnt";
            this.lblProgressCnt.Size = new System.Drawing.Size(74, 18);
            this.lblProgressCnt.TabIndex = 10;
            this.lblProgressCnt.Text = "100 / 100";
            this.lblProgressCnt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(27, 16);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(53, 12);
            this.lblProgress.TabIndex = 9;
            this.lblProgress.Text = "저장현황";
            // 
            // pgBar
            // 
            this.pgBar.Location = new System.Drawing.Point(110, 10);
            this.pgBar.Name = "pgBar";
            this.pgBar.Size = new System.Drawing.Size(385, 23);
            this.pgBar.TabIndex = 8;
            // 
            // ctrlProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblProgressFileNm);
            this.Controls.Add(this.lblProgressCnt);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.pgBar);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ctrlProgress";
            this.Size = new System.Drawing.Size(1002, 44);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProgressFileNm;
        private System.Windows.Forms.Label lblProgressCnt;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar pgBar;
    }
}
