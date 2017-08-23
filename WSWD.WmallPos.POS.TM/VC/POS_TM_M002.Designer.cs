namespace WSWD.WmallPos.POS.TM.VC
{
    partial class POS_TM_M002
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_TM_M002));
            this.button1 = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.inputControl1 = new WSWD.WmallPos.POS.FX.Win.UserControls.InputControl();
            this.button2 = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.inputControl2 = new WSWD.WmallPos.POS.FX.Win.UserControls.InputControl();
            this.button3 = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.inputControl3 = new WSWD.WmallPos.POS.FX.Win.UserControls.InputControl();
            this.inputControl4 = new WSWD.WmallPos.POS.FX.Win.UserControls.InputControl();
            this.button4 = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.inputPrinter = new WSWD.WmallPos.POS.FX.Win.UserControls.InputControl();
            this.button5 = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.button7 = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.inputControl5 = new WSWD.WmallPos.POS.FX.Win.UserControls.InputControl();
            this.btnCD = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.axKSNet_Dongle1 = new AxKSNET_DONGLELib.AxKSNet_Dongle();
            ((System.ComponentModel.ISupportInitialize)(this.axKSNet_Dongle1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BorderSize = 1;
            this.button1.Corner = 3;
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.button1.Image = null;
            this.button1.Location = new System.Drawing.Point(3, 182);
            this.button1.Name = "button1";
            this.button1.Selected = false;
            this.button1.Size = new System.Drawing.Size(90, 42);
            this.button1.TabIndex = 1;
            this.button1.Text = "싸인요청";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // inputControl1
            // 
            this.inputControl1.BorderColor = System.Drawing.Color.LightGray;
            this.inputControl1.BorderWidth = new System.Windows.Forms.Padding(0);
            this.inputControl1.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.inputControl1.Focusable = true;
            this.inputControl1.FocusedIndex = 0;
            this.inputControl1.Format = null;
            this.inputControl1.InputType = WSWD.WmallPos.POS.FX.Win.UserControls.InputControlType.Editable;
            this.inputControl1.IsFocused = false;
            this.inputControl1.Location = new System.Drawing.Point(17, 230);
            this.inputControl1.MaxLength = 0;
            this.inputControl1.MinimumSize = new System.Drawing.Size(250, 34);
            this.inputControl1.Name = "inputControl1";
            this.inputControl1.Padding = new System.Windows.Forms.Padding(3);
            this.inputControl1.Size = new System.Drawing.Size(250, 34);
            this.inputControl1.TabIndex = 2;
            this.inputControl1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.inputControl1.Title = "Code";
            this.inputControl1.TitleAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.inputControl1.TitleWidth = 70;
            // 
            // button2
            // 
            this.button2.BorderSize = 1;
            this.button2.Corner = 3;
            this.button2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.button2.Image = null;
            this.button2.Location = new System.Drawing.Point(538, 3);
            this.button2.Name = "button2";
            this.button2.Selected = false;
            this.button2.Size = new System.Drawing.Size(90, 42);
            this.button2.TabIndex = 1;
            this.button2.Text = "CDP";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 314);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 14);
            this.label1.TabIndex = 3;
            // 
            // inputControl2
            // 
            this.inputControl2.BorderColor = System.Drawing.Color.LightGray;
            this.inputControl2.BorderWidth = new System.Windows.Forms.Padding(0);
            this.inputControl2.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.inputControl2.Focusable = true;
            this.inputControl2.FocusedIndex = 0;
            this.inputControl2.Format = "#,##0";
            this.inputControl2.InputType = WSWD.WmallPos.POS.FX.Win.UserControls.InputControlType.Editable;
            this.inputControl2.IsFocused = false;
            this.inputControl2.Location = new System.Drawing.Point(282, 7);
            this.inputControl2.MaxLength = 0;
            this.inputControl2.MinimumSize = new System.Drawing.Size(250, 34);
            this.inputControl2.Name = "inputControl2";
            this.inputControl2.Padding = new System.Windows.Forms.Padding(3);
            this.inputControl2.Size = new System.Drawing.Size(250, 34);
            this.inputControl2.TabIndex = 2;
            this.inputControl2.Text = "12121";
            this.inputControl2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.inputControl2.Title = "고객화면";
            this.inputControl2.TitleAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.inputControl2.TitleWidth = 70;
            // 
            // button3
            // 
            this.button3.BorderSize = 1;
            this.button3.Corner = 3;
            this.button3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.button3.Image = null;
            this.button3.Location = new System.Drawing.Point(634, 3);
            this.button3.Name = "button3";
            this.button3.Selected = false;
            this.button3.Size = new System.Drawing.Size(90, 42);
            this.button3.TabIndex = 1;
            this.button3.Text = "닫기";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // inputControl3
            // 
            this.inputControl3.BorderColor = System.Drawing.Color.LightGray;
            this.inputControl3.BorderWidth = new System.Windows.Forms.Padding(0);
            this.inputControl3.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.inputControl3.Focusable = true;
            this.inputControl3.FocusedIndex = 0;
            this.inputControl3.Format = "#,##0";
            this.inputControl3.InputType = WSWD.WmallPos.POS.FX.Win.UserControls.InputControlType.Editable;
            this.inputControl3.IsFocused = false;
            this.inputControl3.Location = new System.Drawing.Point(282, 47);
            this.inputControl3.MaxLength = 0;
            this.inputControl3.MinimumSize = new System.Drawing.Size(250, 34);
            this.inputControl3.Name = "inputControl3";
            this.inputControl3.Padding = new System.Windows.Forms.Padding(3);
            this.inputControl3.Size = new System.Drawing.Size(250, 34);
            this.inputControl3.TabIndex = 4;
            this.inputControl3.Text = "12121";
            this.inputControl3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.inputControl3.Title = "고객화면";
            this.inputControl3.TitleAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.inputControl3.TitleWidth = 70;
            // 
            // inputControl4
            // 
            this.inputControl4.BorderColor = System.Drawing.Color.LightGray;
            this.inputControl4.BorderWidth = new System.Windows.Forms.Padding(0);
            this.inputControl4.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.inputControl4.Focusable = true;
            this.inputControl4.FocusedIndex = 0;
            this.inputControl4.Format = null;
            this.inputControl4.InputType = WSWD.WmallPos.POS.FX.Win.UserControls.InputControlType.Editable;
            this.inputControl4.IsFocused = false;
            this.inputControl4.Location = new System.Drawing.Point(17, 270);
            this.inputControl4.MaxLength = 0;
            this.inputControl4.MinimumSize = new System.Drawing.Size(250, 34);
            this.inputControl4.Name = "inputControl4";
            this.inputControl4.Padding = new System.Windows.Forms.Padding(3);
            this.inputControl4.Size = new System.Drawing.Size(250, 34);
            this.inputControl4.TabIndex = 5;
            this.inputControl4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.inputControl4.Title = "MSR";
            this.inputControl4.TitleAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.inputControl4.TitleWidth = 70;
            // 
            // button4
            // 
            this.button4.BorderSize = 1;
            this.button4.Corner = 3;
            this.button4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.button4.Image = null;
            this.button4.Location = new System.Drawing.Point(99, 182);
            this.button4.Name = "button4";
            this.button4.Selected = false;
            this.button4.Size = new System.Drawing.Size(90, 42);
            this.button4.TabIndex = 1;
            this.button4.Text = "확인";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // inputPrinter
            // 
            this.inputPrinter.BackColor = System.Drawing.SystemColors.Control;
            this.inputPrinter.BorderColor = System.Drawing.Color.LightGray;
            this.inputPrinter.BorderWidth = new System.Windows.Forms.Padding(0);
            this.inputPrinter.DataType = WSWD.WmallPos.POS.FX.Shared.InputTextDataType.Numeric;
            this.inputPrinter.Focusable = true;
            this.inputPrinter.FocusedIndex = 0;
            this.inputPrinter.Format = null;
            this.inputPrinter.InputType = WSWD.WmallPos.POS.FX.Win.UserControls.InputControlType.Editable;
            this.inputPrinter.IsFocused = false;
            this.inputPrinter.Location = new System.Drawing.Point(292, 134);
            this.inputPrinter.MaxLength = 0;
            this.inputPrinter.MinimumSize = new System.Drawing.Size(250, 34);
            this.inputPrinter.Name = "inputPrinter";
            this.inputPrinter.Padding = new System.Windows.Forms.Padding(3);
            this.inputPrinter.Size = new System.Drawing.Size(250, 36);
            this.inputPrinter.TabIndex = 5;
            this.inputPrinter.Text = "테스트프린";
            this.inputPrinter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.inputPrinter.Title = "프린터";
            this.inputPrinter.TitleAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.inputPrinter.TitleWidth = 70;
            // 
            // button5
            // 
            this.button5.BorderSize = 1;
            this.button5.Corner = 3;
            this.button5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.button5.Image = null;
            this.button5.Location = new System.Drawing.Point(557, 134);
            this.button5.Name = "button5";
            this.button5.Selected = false;
            this.button5.Size = new System.Drawing.Size(90, 42);
            this.button5.TabIndex = 1;
            this.button5.Text = "Print";
            this.button5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button7
            // 
            this.button7.BorderSize = 1;
            this.button7.Corner = 3;
            this.button7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.button7.Image = null;
            this.button7.Location = new System.Drawing.Point(557, 182);
            this.button7.Name = "button7";
            this.button7.Selected = false;
            this.button7.Size = new System.Drawing.Size(90, 42);
            this.button7.TabIndex = 1;
            this.button7.Text = "RFCard";
            this.button7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // inputControl5
            // 
            this.inputControl5.BorderColor = System.Drawing.Color.LightGray;
            this.inputControl5.BorderWidth = new System.Windows.Forms.Padding(0);
            this.inputControl5.Focusable = true;
            this.inputControl5.FocusedIndex = 0;
            this.inputControl5.Format = null;
            this.inputControl5.InputType = WSWD.WmallPos.POS.FX.Win.UserControls.InputControlType.Editable;
            this.inputControl5.IsFocused = false;
            this.inputControl5.Location = new System.Drawing.Point(313, 230);
            this.inputControl5.MaxLength = 0;
            this.inputControl5.MinimumSize = new System.Drawing.Size(250, 34);
            this.inputControl5.Name = "inputControl5";
            this.inputControl5.Padding = new System.Windows.Forms.Padding(3);
            this.inputControl5.Size = new System.Drawing.Size(250, 34);
            this.inputControl5.TabIndex = 5;
            this.inputControl5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.inputControl5.Title = "RFCardNo";
            this.inputControl5.TitleAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.inputControl5.TitleWidth = 70;
            // 
            // btnCD
            // 
            this.btnCD.BorderSize = 1;
            this.btnCD.Corner = 3;
            this.btnCD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnCD.Image = null;
            this.btnCD.Location = new System.Drawing.Point(538, 51);
            this.btnCD.Name = "btnCD";
            this.btnCD.Selected = false;
            this.btnCD.Size = new System.Drawing.Size(90, 42);
            this.btnCD.TabIndex = 1;
            this.btnCD.Text = "돈통열기";
            this.btnCD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCD.Click += new System.EventHandler(this.btnCD_Click);
            // 
            // axKSNet_Dongle1
            // 
            this.axKSNet_Dongle1.Enabled = true;
            this.axKSNet_Dongle1.Location = new System.Drawing.Point(3, 0);
            this.axKSNet_Dongle1.Name = "axKSNet_Dongle1";
            this.axKSNet_Dongle1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKSNet_Dongle1.OcxState")));
            this.axKSNet_Dongle1.Size = new System.Drawing.Size(273, 176);
            this.axKSNet_Dongle1.TabIndex = 6;
            // 
            // POS_TM_M002
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Controls.Add(this.axKSNet_Dongle1);
            this.Controls.Add(this.inputPrinter);
            this.Controls.Add(this.inputControl5);
            this.Controls.Add(this.inputControl4);
            this.Controls.Add(this.inputControl3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.inputControl2);
            this.Controls.Add(this.inputControl1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnCD);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button1);
            this.IsModal = true;
            this.Name = "POS_TM_M002";
            this.Load += new System.EventHandler(this.POS_TM_M002_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axKSNet_Dongle1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.UserControls.Button button1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputControl inputControl1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button button2;
        private System.Windows.Forms.Label label1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputControl inputControl2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button button3;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputControl inputControl3;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputControl inputControl4;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button button4;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputControl inputPrinter;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button button5;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button button7;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputControl inputControl5;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btnCD;
        private AxKSNET_DONGLELib.AxKSNet_Dongle axKSNet_Dongle1;


    }
}
