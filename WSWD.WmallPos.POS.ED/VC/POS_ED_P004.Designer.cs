namespace WSWD.WmallPos.POS.ED.VC
{
    partial class POS_ED_P004
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_ED_P004));
            this.timerChk = new System.Windows.Forms.Timer(this.components);
            this.btnClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.msgBar = new WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar();
            this.btnTransTr = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnTransLoss = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.msgBarProgress = new WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar();
            this.txtTransDate = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.label1 = new System.Windows.Forms.Label();
            this.colorProgressBar1 = new WSWD.WmallPos.POS.FX.Win.Controls.ColorProgressBar();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BorderSize = 1;
            this.btnClose.Corner = 3;
            this.btnClose.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnClose.Image = null;
            this.btnClose.IsHighlight = false;
            this.btnClose.Location = new System.Drawing.Point(324, 443);
            this.btnClose.Name = "btnClose";
            this.btnClose.Selected = false;
            this.btnClose.Size = new System.Drawing.Size(90, 42);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // msgBar
            // 
            this.msgBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.msgBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.msgBar.BorderWidth = new System.Windows.Forms.Padding(1);
            this.msgBar.ControlType = WSWD.WmallPos.POS.FX.Win.UserControls.MessageBarType.TypeStatus;
            this.msgBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.msgBar.Location = new System.Drawing.Point(67, 317);
            this.msgBar.MinimumSize = new System.Drawing.Size(0, 35);
            this.msgBar.Name = "msgBar";
            this.msgBar.Size = new System.Drawing.Size(604, 42);
            this.msgBar.TabIndex = 11;
            this.msgBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTransTr
            // 
            this.btnTransTr.BorderSize = 1;
            this.btnTransTr.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnTransTr.Corner = 3;
            this.btnTransTr.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.btnTransTr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnTransTr.Image = null;
            this.btnTransTr.IsHighlight = false;
            this.btnTransTr.Location = new System.Drawing.Point(271, 255);
            this.btnTransTr.Name = "btnTransTr";
            this.btnTransTr.Selected = false;
            this.btnTransTr.Size = new System.Drawing.Size(90, 42);
            this.btnTransTr.TabIndex = 15;
            this.btnTransTr.Text = "TR 전송";
            this.btnTransTr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnTransLoss
            // 
            this.btnTransLoss.BorderSize = 1;
            this.btnTransLoss.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btnTransLoss.Corner = 3;
            this.btnTransLoss.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.btnTransLoss.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnTransLoss.Image = null;
            this.btnTransLoss.IsHighlight = false;
            this.btnTransLoss.Location = new System.Drawing.Point(377, 255);
            this.btnTransLoss.Name = "btnTransLoss";
            this.btnTransLoss.Selected = false;
            this.btnTransLoss.Size = new System.Drawing.Size(90, 42);
            this.btnTransLoss.TabIndex = 17;
            this.btnTransLoss.Text = "결락 전송";
            this.btnTransLoss.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // msgBarProgress
            // 
            this.msgBarProgress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(212)))));
            this.msgBarProgress.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.msgBarProgress.BorderWidth = new System.Windows.Forms.Padding(1);
            this.msgBarProgress.ControlType = WSWD.WmallPos.POS.FX.Win.UserControls.MessageBarType.TypeStatus;
            this.msgBarProgress.ForeColor = System.Drawing.SystemColors.ControlText;
            this.msgBarProgress.Location = new System.Drawing.Point(571, 381);
            this.msgBarProgress.MinimumSize = new System.Drawing.Size(0, 35);
            this.msgBarProgress.Name = "msgBarProgress";
            this.msgBarProgress.Size = new System.Drawing.Size(100, 42);
            this.msgBarProgress.TabIndex = 19;
            this.msgBarProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtTransDate
            // 
            this.txtTransDate.BackColor = System.Drawing.Color.White;
            this.txtTransDate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.txtTransDate.BorderWidth = 1;
            this.txtTransDate.Corner = 1;
            this.txtTransDate.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.DateTime;
            this.txtTransDate.Focusable = true;
            this.txtTransDate.FocusedIndex = 0;
            this.txtTransDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtTransDate.Format = null;
            this.txtTransDate.HasBorder = true;
            this.txtTransDate.IsFocused = false;
            this.txtTransDate.Location = new System.Drawing.Point(144, 207);
            this.txtTransDate.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtTransDate.MaxLength = 10;
            this.txtTransDate.Name = "txtTransDate";
            this.txtTransDate.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.txtTransDate.PasswordMode = false;
            this.txtTransDate.ReadOnly = false;
            this.txtTransDate.Size = new System.Drawing.Size(150, 28);
            this.txtTransDate.TabIndex = 21;
            this.txtTransDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(67, 207);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 28);
            this.label1.TabIndex = 20;
            this.label1.Text = "영업일자";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // colorProgressBar1
            // 
            this.colorProgressBar1.Location = new System.Drawing.Point(67, 381);
            this.colorProgressBar1.Name = "colorProgressBar1";
            this.colorProgressBar1.Percentage = 0;
            this.colorProgressBar1.Size = new System.Drawing.Size(498, 42);
            this.colorProgressBar1.TabIndex = 22;
            // 
            // POS_ED_P004
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.colorProgressBar1);
            this.Controls.Add(this.txtTransDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.msgBarProgress);
            this.Controls.Add(this.btnTransLoss);
            this.Controls.Add(this.btnTransTr);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.msgBar);
            this.Font = new System.Drawing.Font("Dotum", 11F, System.Drawing.FontStyle.Bold);
            this.Name = "POS_ED_P004";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerChk;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnClose;
        private WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar msgBar;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnTransTr;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnTransLoss;
        private WSWD.WmallPos.POS.FX.Win.UserControls.MessageBar msgBarProgress;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText txtTransDate;
        private System.Windows.Forms.Label label1;
        private WSWD.WmallPos.POS.FX.Win.Controls.ColorProgressBar colorProgressBar1;
    }
}
