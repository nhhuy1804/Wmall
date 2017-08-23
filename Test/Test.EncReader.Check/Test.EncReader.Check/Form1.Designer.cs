namespace Test.EncReader.Check
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
            this.txtBox_Port = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBox_Baudrate = new System.Windows.Forms.TextBox();
            this.listBox_Log = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtBox_EncCardNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBox_NoEncCardNo = new System.Windows.Forms.TextBox();
            this.btn_SIgnPadInit = new System.Windows.Forms.Button();
            this.btn_SignComReqA1 = new System.Windows.Forms.Button();
            this.axKSNet_Dongle1 = new AxKSNET_DONGLELib.AxKSNet_Dongle();
            this.btn_SignComReqA2 = new System.Windows.Forms.Button();
            this.btn_SignComReqAC = new System.Windows.Forms.Button();
            this.btn_SignComReqA3 = new System.Windows.Forms.Button();
            this.btn_SignComReqF2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBox_Pindata = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtBox_Signdata = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBox_EnvData = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.axKSNet_Dongle1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBox_Port
            // 
            this.txtBox_Port.Location = new System.Drawing.Point(48, 13);
            this.txtBox_Port.Name = "txtBox_Port";
            this.txtBox_Port.Size = new System.Drawing.Size(31, 20);
            this.txtBox_Port.TabIndex = 0;
            this.txtBox_Port.Text = "5";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "PORT";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(93, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "BaudRate";
            // 
            // txtBox_Baudrate
            // 
            this.txtBox_Baudrate.Location = new System.Drawing.Point(148, 13);
            this.txtBox_Baudrate.Name = "txtBox_Baudrate";
            this.txtBox_Baudrate.Size = new System.Drawing.Size(48, 20);
            this.txtBox_Baudrate.TabIndex = 3;
            this.txtBox_Baudrate.Text = "38400";
            // 
            // listBox_Log
            // 
            this.listBox_Log.FormattingEnabled = true;
            this.listBox_Log.HorizontalScrollbar = true;
            this.listBox_Log.Location = new System.Drawing.Point(10, 404);
            this.listBox_Log.Name = "listBox_Log";
            this.listBox_Log.Size = new System.Drawing.Size(1002, 303);
            this.listBox_Log.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(208, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 25);
            this.button1.TabIndex = 6;
            this.button1.Text = "Open";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(208, 44);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 25);
            this.button2.TabIndex = 7;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtBox_EncCardNo
            // 
            this.txtBox_EncCardNo.Location = new System.Drawing.Point(297, 373);
            this.txtBox_EncCardNo.Name = "txtBox_EncCardNo";
            this.txtBox_EncCardNo.Size = new System.Drawing.Size(715, 20);
            this.txtBox_EncCardNo.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(207, 376);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Enc Card No.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(207, 342);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "No Enc Card No.";
            // 
            // txtBox_NoEncCardNo
            // 
            this.txtBox_NoEncCardNo.Location = new System.Drawing.Point(297, 339);
            this.txtBox_NoEncCardNo.Name = "txtBox_NoEncCardNo";
            this.txtBox_NoEncCardNo.Size = new System.Drawing.Size(715, 20);
            this.txtBox_NoEncCardNo.TabIndex = 11;
            // 
            // btn_SIgnPadInit
            // 
            this.btn_SIgnPadInit.Location = new System.Drawing.Point(297, 42);
            this.btn_SIgnPadInit.Name = "btn_SIgnPadInit";
            this.btn_SIgnPadInit.Size = new System.Drawing.Size(134, 25);
            this.btn_SIgnPadInit.TabIndex = 12;
            this.btn_SIgnPadInit.Text = "SignPad Init";
            this.btn_SIgnPadInit.UseVisualStyleBackColor = true;
            this.btn_SIgnPadInit.Click += new System.EventHandler(this.btn_SIgnPadInit_Click);
            // 
            // btn_SignComReqA1
            // 
            this.btn_SignComReqA1.Location = new System.Drawing.Point(297, 74);
            this.btn_SignComReqA1.Name = "btn_SignComReqA1";
            this.btn_SignComReqA1.Size = new System.Drawing.Size(134, 25);
            this.btn_SignComReqA1.TabIndex = 13;
            this.btn_SignComReqA1.Text = "SignPad SignComReqA1";
            this.btn_SignComReqA1.UseVisualStyleBackColor = true;
            this.btn_SignComReqA1.Click += new System.EventHandler(this.btn_SignComReqA1_Click);
            // 
            // axKSNet_Dongle1
            // 
            this.axKSNet_Dongle1.Enabled = true;
            this.axKSNet_Dongle1.Location = new System.Drawing.Point(14, 39);
            this.axKSNet_Dongle1.Name = "axKSNet_Dongle1";
            this.axKSNet_Dongle1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKSNet_Dongle1.OcxState")));
            this.axKSNet_Dongle1.Size = new System.Drawing.Size(214, 328);
            this.axKSNet_Dongle1.TabIndex = 5;
            this.axKSNet_Dongle1.OnRecvSignData += new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvSignDataEventHandler(this.axKSNet_Dongle1_OnRecvSignData);
            this.axKSNet_Dongle1.OnRecvEncReaderRemove += new System.EventHandler(this.axKSNet_Dongle1_OnRecvEncReaderRemove);
            this.axKSNet_Dongle1.OnRecvPinData += new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvPinDataEventHandler(this.axKSNet_Dongle1_OnRecvPinData);
            this.axKSNet_Dongle1.OnRecvEncReaderCard += new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvEncReaderCardEventHandler(this.axKSNet_Dongle1_OnRecvEncReaderCard);
            this.axKSNet_Dongle1.OnRecvEncReaderInsert += new System.EventHandler(this.axaxKSNet_Dongle1_OnRecvEncReaderInsert);
            // 
            // btn_SignComReqA2
            // 
            this.btn_SignComReqA2.Location = new System.Drawing.Point(297, 105);
            this.btn_SignComReqA2.Name = "btn_SignComReqA2";
            this.btn_SignComReqA2.Size = new System.Drawing.Size(134, 25);
            this.btn_SignComReqA2.TabIndex = 14;
            this.btn_SignComReqA2.Text = "SignPad SignComReqA2";
            this.btn_SignComReqA2.UseVisualStyleBackColor = true;
            this.btn_SignComReqA2.Click += new System.EventHandler(this.btn_SignComReqA2_Click);
            // 
            // btn_SignComReqAC
            // 
            this.btn_SignComReqAC.Location = new System.Drawing.Point(297, 137);
            this.btn_SignComReqAC.Name = "btn_SignComReqAC";
            this.btn_SignComReqAC.Size = new System.Drawing.Size(134, 25);
            this.btn_SignComReqAC.TabIndex = 15;
            this.btn_SignComReqAC.Text = "SignPad SignComReqAC";
            this.btn_SignComReqAC.UseVisualStyleBackColor = true;
            this.btn_SignComReqAC.Click += new System.EventHandler(this.btn_SignComReqAC_Click);
            // 
            // btn_SignComReqA3
            // 
            this.btn_SignComReqA3.Location = new System.Drawing.Point(297, 168);
            this.btn_SignComReqA3.Name = "btn_SignComReqA3";
            this.btn_SignComReqA3.Size = new System.Drawing.Size(134, 25);
            this.btn_SignComReqA3.TabIndex = 16;
            this.btn_SignComReqA3.Text = "SignPad SignComReqA3";
            this.btn_SignComReqA3.UseVisualStyleBackColor = true;
            this.btn_SignComReqA3.Click += new System.EventHandler(this.btn_SignComReqA3_Click);
            // 
            // btn_SignComReqF2
            // 
            this.btn_SignComReqF2.Location = new System.Drawing.Point(297, 199);
            this.btn_SignComReqF2.Name = "btn_SignComReqF2";
            this.btn_SignComReqF2.Size = new System.Drawing.Size(134, 25);
            this.btn_SignComReqF2.TabIndex = 17;
            this.btn_SignComReqF2.Text = "SignPad SignComReqF2";
            this.btn_SignComReqF2.UseVisualStyleBackColor = true;
            this.btn_SignComReqF2.Click += new System.EventHandler(this.btn_SignComReqF2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(436, 42);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(64, 25);
            this.button3.TabIndex = 18;
            this.button3.Text = "Clear";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(207, 310);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Pindata";
            // 
            // txtBox_Pindata
            // 
            this.txtBox_Pindata.Location = new System.Drawing.Point(297, 307);
            this.txtBox_Pindata.Name = "txtBox_Pindata";
            this.txtBox_Pindata.Size = new System.Drawing.Size(396, 20);
            this.txtBox_Pindata.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(207, 277);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "SignData";
            // 
            // txtBox_Signdata
            // 
            this.txtBox_Signdata.Location = new System.Drawing.Point(297, 274);
            this.txtBox_Signdata.Name = "txtBox_Signdata";
            this.txtBox_Signdata.Size = new System.Drawing.Size(396, 20);
            this.txtBox_Signdata.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(207, 242);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "EncData";
            // 
            // txtBox_EnvData
            // 
            this.txtBox_EnvData.Location = new System.Drawing.Point(297, 238);
            this.txtBox_EnvData.Name = "txtBox_EnvData";
            this.txtBox_EnvData.Size = new System.Drawing.Size(715, 20);
            this.txtBox_EnvData.TabIndex = 24;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(437, 74);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(171, 25);
            this.button4.TabIndex = 25;
            this.button4.Text = "SignPad SignPinComReqA4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(437, 105);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(134, 25);
            this.button5.TabIndex = 26;
            this.button5.Text = "SignPad SignComReq54";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 714);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.txtBox_EnvData);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtBox_Signdata);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtBox_Pindata);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btn_SignComReqF2);
            this.Controls.Add(this.btn_SignComReqA3);
            this.Controls.Add(this.btn_SignComReqAC);
            this.Controls.Add(this.btn_SignComReqA2);
            this.Controls.Add(this.btn_SignComReqA1);
            this.Controls.Add(this.btn_SIgnPadInit);
            this.Controls.Add(this.txtBox_NoEncCardNo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBox_EncCardNo);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.axKSNet_Dongle1);
            this.Controls.Add(this.listBox_Log);
            this.Controls.Add(this.txtBox_Baudrate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBox_Port);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.axKSNet_Dongle1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBox_Port;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBox_Baudrate;
        private System.Windows.Forms.ListBox listBox_Log;

        private AxKSNET_DONGLELib.AxKSNet_Dongle axKSNet_Dongle1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtBox_EncCardNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBox_NoEncCardNo;
        private System.Windows.Forms.Button btn_SIgnPadInit;
        private System.Windows.Forms.Button btn_SignComReqA1;
        private System.Windows.Forms.Button btn_SignComReqA2;
        private System.Windows.Forms.Button btn_SignComReqAC;
        private System.Windows.Forms.Button btn_SignComReqA3;
        private System.Windows.Forms.Button btn_SignComReqF2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBox_Pindata;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtBox_Signdata;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBox_EnvData;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}

