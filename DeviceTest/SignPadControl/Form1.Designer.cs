namespace SignPadControl
{
    partial class Form1
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.portnum = new System.Windows.Forms.TextBox();
            this.baudrate = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.signdata = new System.Windows.Forms.TextBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.pindata = new System.Windows.Forms.TextBox();
            this.button9 = new System.Windows.Forms.Button();
            this.txtRFCardNo = new System.Windows.Forms.TextBox();
            this.axKSNet_Dongle1 = new AxKSNET_DONGLELib.AxKSNet_Dongle();
            this.button10 = new System.Windows.Forms.Button();
            this.iccardata = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnScanPort = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axKSNet_Dongle1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "포트번호 :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(373, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "USB 사용의 경우 포트번호는 0 , 통신속도는 아무렇게나 입력해도 상관없음";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(119, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "통신속도 :";
            // 
            // portnum
            // 
            this.portnum.Location = new System.Drawing.Point(71, 10);
            this.portnum.Name = "portnum";
            this.portnum.Size = new System.Drawing.Size(26, 20);
            this.portnum.TabIndex = 4;
            this.portnum.Text = "1";
            // 
            // baudrate
            // 
            this.baudrate.Location = new System.Drawing.Point(177, 10);
            this.baudrate.Name = "baudrate";
            this.baudrate.Size = new System.Drawing.Size(54, 20);
            this.baudrate.TabIndex = 5;
            this.baudrate.Text = "38400";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(313, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 34);
            this.button1.TabIndex = 6;
            this.button1.Text = "포트연결";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(414, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(92, 34);
            this.button2.TabIndex = 7;
            this.button2.Text = "포트닫기";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(303, 69);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(268, 27);
            this.button3.TabIndex = 8;
            this.button3.Text = "싸인패드초기화";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(303, 102);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(126, 29);
            this.button4.TabIndex = 9;
            this.button4.Text = "전자서명 입력 요청";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(303, 138);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(125, 27);
            this.button5.TabIndex = 10;
            this.button5.Text = "데이터 전송 요청";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(303, 173);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(125, 27);
            this.button6.TabIndex = 11;
            this.button6.Text = "서명 취소 요청";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // signdata
            // 
            this.signdata.Location = new System.Drawing.Point(303, 211);
            this.signdata.Name = "signdata";
            this.signdata.Size = new System.Drawing.Size(268, 20);
            this.signdata.TabIndex = 12;
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(438, 106);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(131, 95);
            this.pictureBox.TabIndex = 13;
            this.pictureBox.TabStop = false;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(20, 240);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(107, 30);
            this.button7.TabIndex = 14;
            this.button7.Text = "핀패드 입력 요청";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(133, 240);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(144, 30);
            this.button8.TabIndex = 15;
            this.button8.Text = "핀패드 입력 요청(암호화)";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // pindata
            // 
            this.pindata.Location = new System.Drawing.Point(303, 240);
            this.pindata.Name = "pindata";
            this.pindata.Size = new System.Drawing.Size(267, 20);
            this.pindata.TabIndex = 16;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(21, 286);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(107, 30);
            this.button9.TabIndex = 14;
            this.button9.Text = "RF카드리딩요청";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // txtRFCardNo
            // 
            this.txtRFCardNo.Location = new System.Drawing.Point(133, 292);
            this.txtRFCardNo.Name = "txtRFCardNo";
            this.txtRFCardNo.Size = new System.Drawing.Size(267, 20);
            this.txtRFCardNo.TabIndex = 16;
            // 
            // axKSNet_Dongle1
            // 
            this.axKSNet_Dongle1.Enabled = true;
            this.axKSNet_Dongle1.Location = new System.Drawing.Point(18, 76);
            this.axKSNet_Dongle1.Name = "axKSNet_Dongle1";
            this.axKSNet_Dongle1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKSNet_Dongle1.OcxState")));
            this.axKSNet_Dongle1.Size = new System.Drawing.Size(279, 150);
            this.axKSNet_Dongle1.TabIndex = 17;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(21, 333);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(107, 30);
            this.button10.TabIndex = 18;
            this.button10.Text = "IC카드리딩요청";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // iccardata
            // 
            this.iccardata.Location = new System.Drawing.Point(134, 339);
            this.iccardata.Name = "iccardata";
            this.iccardata.Size = new System.Drawing.Size(267, 20);
            this.iccardata.TabIndex = 19;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(21, 379);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(372, 20);
            this.textBox1.TabIndex = 21;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(21, 405);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(372, 20);
            this.textBox2.TabIndex = 22;
            // 
            // btnScanPort
            // 
            this.btnScanPort.Location = new System.Drawing.Point(406, 286);
            this.btnScanPort.Name = "btnScanPort";
            this.btnScanPort.Size = new System.Drawing.Size(93, 34);
            this.btnScanPort.TabIndex = 23;
            this.btnScanPort.Text = "포트연결";
            this.btnScanPort.UseVisualStyleBackColor = true;
            this.btnScanPort.Click += new System.EventHandler(this.btnScanPort_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(665, 451);
            this.Controls.Add(this.btnScanPort);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.iccardata);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.axKSNet_Dongle1);
            this.Controls.Add(this.txtRFCardNo);
            this.Controls.Add(this.pindata);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.signdata);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.baudrate);
            this.Controls.Add(this.portnum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "KSNET SignPad KSP-6000 테스트";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axKSNet_Dongle1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox portnum;
        private System.Windows.Forms.TextBox baudrate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox signdata;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox pindata;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.TextBox txtRFCardNo;
        private AxKSNET_DONGLELib.AxKSNet_Dongle axKSNet_Dongle1;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.TextBox iccardata;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnScanPort;
    }
}

