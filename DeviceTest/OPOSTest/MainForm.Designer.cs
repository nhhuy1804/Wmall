namespace OPOSTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnOpenCD = new System.Windows.Forms.Button();
            this.btnLineDisplay = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnMSR = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnMSRClose = new System.Windows.Forms.Button();
            this.axCtrlKeyboard1 = new AxKeyBoardHook.AxCtrlKeyboard();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.axCtrlKeyboard1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpenCD
            // 
            this.btnOpenCD.Location = new System.Drawing.Point(12, 12);
            this.btnOpenCD.Name = "btnOpenCD";
            this.btnOpenCD.Size = new System.Drawing.Size(98, 34);
            this.btnOpenCD.TabIndex = 0;
            this.btnOpenCD.Text = "돈통열기";
            this.btnOpenCD.UseVisualStyleBackColor = true;
            this.btnOpenCD.Click += new System.EventHandler(this.btnOpenCD_Click);
            // 
            // btnLineDisplay
            // 
            this.btnLineDisplay.Location = new System.Drawing.Point(116, 12);
            this.btnLineDisplay.Name = "btnLineDisplay";
            this.btnLineDisplay.Size = new System.Drawing.Size(98, 34);
            this.btnLineDisplay.TabIndex = 0;
            this.btnLineDisplay.Text = "LineDisplay";
            this.btnLineDisplay.UseVisualStyleBackColor = true;
            this.btnLineDisplay.Click += new System.EventHandler(this.btnLineDisplay_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(220, 12);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(98, 34);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "프린터스트";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnMSR
            // 
            this.btnMSR.Location = new System.Drawing.Point(12, 52);
            this.btnMSR.Name = "btnMSR";
            this.btnMSR.Size = new System.Drawing.Size(98, 34);
            this.btnMSR.TabIndex = 0;
            this.btnMSR.Text = "MSROpen";
            this.btnMSR.UseVisualStyleBackColor = true;
            this.btnMSR.Click += new System.EventHandler(this.btnMSR_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(128, 60);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(190, 20);
            this.textBox1.TabIndex = 1;
            // 
            // btnMSRClose
            // 
            this.btnMSRClose.Location = new System.Drawing.Point(339, 60);
            this.btnMSRClose.Name = "btnMSRClose";
            this.btnMSRClose.Size = new System.Drawing.Size(98, 34);
            this.btnMSRClose.TabIndex = 0;
            this.btnMSRClose.Text = "MSRClose";
            this.btnMSRClose.UseVisualStyleBackColor = true;
            this.btnMSRClose.Click += new System.EventHandler(this.btnMSRClose_Click);
            // 
            // axCtrlKeyboard1
            // 
            this.axCtrlKeyboard1.Enabled = true;
            this.axCtrlKeyboard1.Location = new System.Drawing.Point(352, 12);
            this.axCtrlKeyboard1.Name = "axCtrlKeyboard1";
            this.axCtrlKeyboard1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axCtrlKeyboard1.OcxState")));
            this.axCtrlKeyboard1.Size = new System.Drawing.Size(32, 32);
            this.axCtrlKeyboard1.TabIndex = 2;
            this.axCtrlKeyboard1.ScannerEvent += new AxKeyBoardHook.@__CtrlKeyboard_ScannerEventEventHandler(this.axCtrlKeyboard1_ScannerEvent);
            this.axCtrlKeyboard1.MsrEvent += new AxKeyBoardHook.@__CtrlKeyboard_MsrEventEventHandler(this.axCtrlKeyboard1_MsrEvent);
            this.axCtrlKeyboard1.KeyboardEvent += new AxKeyBoardHook.@__CtrlKeyboard_KeyboardEventEventHandler(this.axCtrlKeyboard1_KeyboardEvent);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(402, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(402, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "label1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(443, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "label1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(622, 351);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.axCtrlKeyboard1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnMSRClose);
            this.Controls.Add(this.btnMSR);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnLineDisplay);
            this.Controls.Add(this.btnOpenCD);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IBM POS - OPOS장비테스트";
            ((System.ComponentModel.ISupportInitialize)(this.axCtrlKeyboard1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenCD;
        private System.Windows.Forms.Button btnLineDisplay;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnMSR;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnMSRClose;
        private AxKeyBoardHook.AxCtrlKeyboard axCtrlKeyboard1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

