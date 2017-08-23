namespace WSWD.WmallPos.POS.Utils
{
    partial class MainForm
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
            this.btnProcMsg = new System.Windows.Forms.Button();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.btnEnc = new System.Windows.Forms.Button();
            this.txtText = new System.Windows.Forms.TextBox();
            this.txtEncText = new System.Windows.Forms.TextBox();
            this.txtDecText = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lblProgress = new System.Windows.Forms.Label();
            this.btnStartChkDupMsg = new System.Windows.Forms.Button();
            this.txtDupSrcPath = new System.Windows.Forms.TextBox();
            this.btnBrowse1 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnProcMsg
            // 
            this.btnProcMsg.Location = new System.Drawing.Point(6, 4);
            this.btnProcMsg.Name = "btnProcMsg";
            this.btnProcMsg.Size = new System.Drawing.Size(100, 35);
            this.btnProcMsg.TabIndex = 2;
            this.btnProcMsg.Text = "일괄적용";
            this.btnProcMsg.UseVisualStyleBackColor = true;
            this.btnProcMsg.Click += new System.EventHandler(this.btnProcMsg_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(112, 15);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(374, 20);
            this.txtFolder.TabIndex = 3;
            // 
            // btnEnc
            // 
            this.btnEnc.Location = new System.Drawing.Point(23, 29);
            this.btnEnc.Name = "btnEnc";
            this.btnEnc.Size = new System.Drawing.Size(83, 34);
            this.btnEnc.TabIndex = 4;
            this.btnEnc.Text = "Encrypt";
            this.btnEnc.UseVisualStyleBackColor = true;
            this.btnEnc.Click += new System.EventHandler(this.btnEnc_Click);
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(112, 29);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(370, 20);
            this.txtText.TabIndex = 5;
            // 
            // txtEncText
            // 
            this.txtEncText.Location = new System.Drawing.Point(112, 55);
            this.txtEncText.Name = "txtEncText";
            this.txtEncText.Size = new System.Drawing.Size(370, 20);
            this.txtEncText.TabIndex = 6;
            // 
            // txtDecText
            // 
            this.txtDecText.Location = new System.Drawing.Point(112, 81);
            this.txtDecText.Name = "txtDecText";
            this.txtDecText.Size = new System.Drawing.Size(370, 20);
            this.txtDecText.TabIndex = 8;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(23, 67);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(83, 34);
            this.button3.TabIndex = 7;
            this.button3.Text = "Decrypt";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.btnDec_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(10, 10);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(548, 331);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lstFiles);
            this.tabPage1.Controls.Add(this.btnBrowse);
            this.tabPage1.Controls.Add(this.btnProcMsg);
            this.tabPage1.Controls.Add(this.txtFolder);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(540, 305);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "POS/PDA 알림메시지 일괄등록";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lstFiles
            // 
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.Location = new System.Drawing.Point(6, 44);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(525, 251);
            this.lstFiles.TabIndex = 4;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(492, 10);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(39, 28);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtText);
            this.tabPage2.Controls.Add(this.txtDecText);
            this.tabPage2.Controls.Add(this.btnEnc);
            this.tabPage2.Controls.Add(this.button3);
            this.tabPage2.Controls.Add(this.txtEncText);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(540, 305);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "설정암호화";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtDupSrcPath);
            this.tabPage3.Controls.Add(this.lblProgress);
            this.tabPage3.Controls.Add(this.btnBrowse1);
            this.tabPage3.Controls.Add(this.btnStartChkDupMsg);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(540, 305);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "중복 메시지 처리";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(17, 65);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 13);
            this.lblProgress.TabIndex = 6;
            // 
            // btnStartChkDupMsg
            // 
            this.btnStartChkDupMsg.Location = new System.Drawing.Point(6, 15);
            this.btnStartChkDupMsg.Name = "btnStartChkDupMsg";
            this.btnStartChkDupMsg.Size = new System.Drawing.Size(83, 34);
            this.btnStartChkDupMsg.TabIndex = 5;
            this.btnStartChkDupMsg.Text = "Start";
            this.btnStartChkDupMsg.UseVisualStyleBackColor = true;
            this.btnStartChkDupMsg.Click += new System.EventHandler(this.btnStartChkDupMsg_Click);
            // 
            // txtDupSrcPath
            // 
            this.txtDupSrcPath.Location = new System.Drawing.Point(95, 23);
            this.txtDupSrcPath.Name = "txtDupSrcPath";
            this.txtDupSrcPath.Size = new System.Drawing.Size(439, 20);
            this.txtDupSrcPath.TabIndex = 7;
            this.txtDupSrcPath.Text = "D:\\Working\\01.Projects\\Wmall\\02.Source\\wspos_wmall\\01.Source\\POS";
            // 
            // btnBrowse1
            // 
            this.btnBrowse1.Location = new System.Drawing.Point(501, 49);
            this.btnBrowse1.Name = "btnBrowse1";
            this.btnBrowse1.Size = new System.Drawing.Size(33, 24);
            this.btnBrowse1.TabIndex = 5;
            this.btnBrowse1.Text = "...";
            this.btnBrowse1.UseVisualStyleBackColor = true;
            this.btnBrowse1.Click += new System.EventHandler(this.btnBrowse1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(568, 351);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "POS 도구";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnProcMsg;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Button btnEnc;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.TextBox txtEncText;
        private System.Windows.Forms.TextBox txtDecText;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Button btnStartChkDupMsg;
        private System.Windows.Forms.TextBox txtDupSrcPath;
        private System.Windows.Forms.Button btnBrowse1;
    }
}

