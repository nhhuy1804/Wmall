namespace BasketTestApp
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.axCtrlKeyboard1 = new AxKeyBoardHook.AxCtrlKeyboard();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblActiveTitle = new System.Windows.Forms.Label();
            this.tbiCasName = new System.Windows.Forms.Label();
            this.tbiStoreNm = new System.Windows.Forms.Label();
            this.tbiDataStatus = new System.Windows.Forms.Label();
            this.tbiSaleHold = new System.Windows.Forms.Label();
            this.tbiTranNo = new System.Windows.Forms.Label();
            this.tbiPOSNo = new System.Windows.Forms.Label();
            this.tbiSaleDate = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblNoticeState = new System.Windows.Forms.Label();
            this.lblServerState = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tmSystemTime = new System.Windows.Forms.Timer(this.components);
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.statusBar1 = new WSWD.WmallPos.POS.FX.Win.UserControls.StatusBar();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.axCtrlKeyboard1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(160, 162);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Form2";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(414, 300);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(115, 30);
            this.button2.TabIndex = 3;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(146, 289);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(115, 30);
            this.button3.TabIndex = 7;
            this.button3.Text = "Form1";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // axCtrlKeyboard1
            // 
            this.axCtrlKeyboard1.Enabled = true;
            this.axCtrlKeyboard1.Location = new System.Drawing.Point(439, 110);
            this.axCtrlKeyboard1.Name = "axCtrlKeyboard1";
            this.axCtrlKeyboard1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axCtrlKeyboard1.OcxState")));
            this.axCtrlKeyboard1.Size = new System.Drawing.Size(32, 32);
            this.axCtrlKeyboard1.TabIndex = 8;
            this.axCtrlKeyboard1.Visible = false;
            this.axCtrlKeyboard1.TraceLogEvent += new AxKeyBoardHook.@__CtrlKeyboard_TraceLogEventEventHandler(this.axCtrlKeyboard1_TraceLogEvent);
            this.axCtrlKeyboard1.ErrorEvent += new AxKeyBoardHook.@__CtrlKeyboard_ErrorEventEventHandler(this.axCtrlKeyboard1_ErrorEvent);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(156, 359);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(437, 20);
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(142, 214);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "label1";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(160, 429);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(437, 20);
            this.textBox2.TabIndex = 11;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(39)))), ((int)(((byte)(111)))));
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.lblActiveTitle);
            this.panel1.Controls.Add(this.tbiCasName);
            this.panel1.Controls.Add(this.tbiStoreNm);
            this.panel1.Controls.Add(this.tbiDataStatus);
            this.panel1.Controls.Add(this.tbiSaleHold);
            this.panel1.Controls.Add(this.tbiTranNo);
            this.panel1.Controls.Add(this.tbiPOSNo);
            this.panel1.Controls.Add(this.tbiSaleDate);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.lblNoticeState);
            this.panel1.Controls.Add(this.lblServerState);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("Dotum", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1024, 48);
            this.panel1.TabIndex = 12;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(0)))), ((int)(((byte)(45)))));
            this.label10.Location = new System.Drawing.Point(820, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(2, 48);
            this.label10.TabIndex = 61;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(0)))), ((int)(((byte)(45)))));
            this.label9.Location = new System.Drawing.Point(924, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(2, 48);
            this.label9.TabIndex = 60;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(0)))), ((int)(((byte)(45)))));
            this.label8.Location = new System.Drawing.Point(974, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(2, 48);
            this.label8.TabIndex = 59;
            // 
            // lblActiveTitle
            // 
            this.lblActiveTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblActiveTitle.ForeColor = System.Drawing.Color.White;
            this.lblActiveTitle.Location = new System.Drawing.Point(152, 0);
            this.lblActiveTitle.Name = "lblActiveTitle";
            this.lblActiveTitle.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblActiveTitle.Size = new System.Drawing.Size(240, 48);
            this.lblActiveTitle.TabIndex = 58;
            this.lblActiveTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbiCasName
            // 
            this.tbiCasName.AutoSize = true;
            this.tbiCasName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(239)))), ((int)(((byte)(170)))));
            this.tbiCasName.Location = new System.Drawing.Point(603, 28);
            this.tbiCasName.Name = "tbiCasName";
            this.tbiCasName.Size = new System.Drawing.Size(0, 12);
            this.tbiCasName.TabIndex = 53;
            this.tbiCasName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbiStoreNm
            // 
            this.tbiStoreNm.AutoSize = true;
            this.tbiStoreNm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(239)))), ((int)(((byte)(170)))));
            this.tbiStoreNm.Location = new System.Drawing.Point(725, 28);
            this.tbiStoreNm.Name = "tbiStoreNm";
            this.tbiStoreNm.Size = new System.Drawing.Size(0, 12);
            this.tbiStoreNm.TabIndex = 52;
            this.tbiStoreNm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbiDataStatus
            // 
            this.tbiDataStatus.AutoSize = true;
            this.tbiDataStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(239)))), ((int)(((byte)(170)))));
            this.tbiDataStatus.Location = new System.Drawing.Point(863, 28);
            this.tbiDataStatus.Name = "tbiDataStatus";
            this.tbiDataStatus.Size = new System.Drawing.Size(0, 12);
            this.tbiDataStatus.TabIndex = 51;
            this.tbiDataStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbiSaleHold
            // 
            this.tbiSaleHold.AutoSize = true;
            this.tbiSaleHold.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tbiSaleHold.Location = new System.Drawing.Point(863, 8);
            this.tbiSaleHold.Name = "tbiSaleHold";
            this.tbiSaleHold.Size = new System.Drawing.Size(0, 12);
            this.tbiSaleHold.TabIndex = 54;
            this.tbiSaleHold.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbiTranNo
            // 
            this.tbiTranNo.AutoSize = true;
            this.tbiTranNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(239)))), ((int)(((byte)(170)))));
            this.tbiTranNo.Location = new System.Drawing.Point(745, 8);
            this.tbiTranNo.Name = "tbiTranNo";
            this.tbiTranNo.Size = new System.Drawing.Size(0, 12);
            this.tbiTranNo.TabIndex = 57;
            this.tbiTranNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbiPOSNo
            // 
            this.tbiPOSNo.AutoSize = true;
            this.tbiPOSNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(239)))), ((int)(((byte)(170)))));
            this.tbiPOSNo.Location = new System.Drawing.Point(612, 8);
            this.tbiPOSNo.Name = "tbiPOSNo";
            this.tbiPOSNo.Size = new System.Drawing.Size(0, 12);
            this.tbiPOSNo.TabIndex = 56;
            this.tbiPOSNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbiSaleDate
            // 
            this.tbiSaleDate.AutoSize = true;
            this.tbiSaleDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(239)))), ((int)(((byte)(170)))));
            this.tbiSaleDate.Location = new System.Drawing.Point(454, 8);
            this.tbiSaleDate.Name = "tbiSaleDate";
            this.tbiSaleDate.Size = new System.Drawing.Size(0, 12);
            this.tbiSaleDate.TabIndex = 55;
            this.tbiSaleDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(191)))), ((int)(((byte)(212)))));
            this.label7.Location = new System.Drawing.Point(397, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 12);
            this.label7.TabIndex = 49;
            this.label7.Text = "영업일자";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(191)))), ((int)(((byte)(212)))));
            this.label5.Location = new System.Drawing.Point(557, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 12);
            this.label5.TabIndex = 48;
            this.label5.Text = "계산원";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(191)))), ((int)(((byte)(212)))));
            this.label6.Location = new System.Drawing.Point(557, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 12);
            this.label6.TabIndex = 47;
            this.label6.Text = "POS No.";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(191)))), ((int)(((byte)(212)))));
            this.label3.Location = new System.Drawing.Point(690, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 12);
            this.label3.TabIndex = 46;
            this.label3.Text = "매장";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(191)))), ((int)(((byte)(212)))));
            this.label4.Location = new System.Drawing.Point(690, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 12);
            this.label4.TabIndex = 45;
            this.label4.Text = "거래 No.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(191)))), ((int)(((byte)(212)))));
            this.label2.Location = new System.Drawing.Point(828, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 12);
            this.label2.TabIndex = 44;
            this.label2.Text = "전송";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(191)))), ((int)(((byte)(212)))));
            this.label11.Location = new System.Drawing.Point(828, 8);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 12);
            this.label11.TabIndex = 43;
            this.label11.Text = "보류";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNoticeState
            // 
            this.lblNoticeState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(22)))), ((int)(((byte)(79)))));
            this.lblNoticeState.Location = new System.Drawing.Point(926, 0);
            this.lblNoticeState.Name = "lblNoticeState";
            this.lblNoticeState.Padding = new System.Windows.Forms.Padding(1);
            this.lblNoticeState.Size = new System.Drawing.Size(48, 48);
            this.lblNoticeState.TabIndex = 42;
            // 
            // lblServerState
            // 
            this.lblServerState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(22)))), ((int)(((byte)(79)))));
            this.lblServerState.Location = new System.Drawing.Point(975, 0);
            this.lblServerState.Name = "lblServerState";
            this.lblServerState.Padding = new System.Windows.Forms.Padding(1);
            this.lblServerState.Size = new System.Drawing.Size(48, 48);
            this.lblServerState.TabIndex = 41;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::BasketTestApp.Properties.Resources.wmall_logo;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(148, 48);
            this.pictureBox1.TabIndex = 40;
            this.pictureBox1.TabStop = false;
            // 
            // tmSystemTime
            // 
            this.tmSystemTime.Interval = 1000;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(478, 197);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(115, 30);
            this.button4.TabIndex = 13;
            this.button4.Text = "Form2";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(559, 252);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(115, 30);
            this.button5.TabIndex = 14;
            this.button5.Text = "Parse";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // statusBar1
            // 
            this.statusBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(39)))), ((int)(((byte)(111)))));
            this.statusBar1.Font = new System.Drawing.Font("Dotum", 12F, System.Drawing.FontStyle.Bold);
            this.statusBar1.Location = new System.Drawing.Point(0, 740);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(1024, 28);
            this.statusBar1.StatusMessage = "";
            this.statusBar1.TabIndex = 5;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(12, 429);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(115, 30);
            this.button6.TabIndex = 15;
            this.button6.Text = "MakePV01";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(518, 147);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(115, 30);
            this.button7.TabIndex = 16;
            this.button7.Text = "FormTest";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.axCtrlKeyboard1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axCtrlKeyboard1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.StatusBar statusBar1;
        private System.Windows.Forms.Button button3;
        private AxKeyBoardHook.AxCtrlKeyboard axCtrlKeyboard1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblActiveTitle;
        private System.Windows.Forms.Label tbiCasName;
        private System.Windows.Forms.Label tbiStoreNm;
        private System.Windows.Forms.Label tbiDataStatus;
        private System.Windows.Forms.Label tbiSaleHold;
        private System.Windows.Forms.Label tbiTranNo;
        private System.Windows.Forms.Label tbiPOSNo;
        private System.Windows.Forms.Label tbiSaleDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblNoticeState;
        private System.Windows.Forms.Label lblServerState;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer tmSystemTime;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;

    }
}