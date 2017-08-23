namespace KeyBoardTest
{
    partial class ControlTest
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.btnCardParse = new System.Windows.Forms.Button();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.directoryEntry1 = new System.DirectoryServices.DirectoryEntry();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(270, 20);
            this.textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 38);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(270, 20);
            this.textBox2.TabIndex = 1;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(12, 64);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(270, 20);
            this.textBox3.TabIndex = 2;
            // 
            // btnCardParse
            // 
            this.btnCardParse.Location = new System.Drawing.Point(298, 10);
            this.btnCardParse.Name = "btnCardParse";
            this.btnCardParse.Size = new System.Drawing.Size(75, 23);
            this.btnCardParse.TabIndex = 3;
            this.btnCardParse.Text = "카드확인";
            this.btnCardParse.UseVisualStyleBackColor = true;
            this.btnCardParse.Click += new System.EventHandler(this.btnCardParse_Click);
            // 
            // ultraLabel1
            // 
            appearance1.BorderColor = System.Drawing.Color.Purple;
            this.ultraLabel1.Appearance = appearance1;
            this.ultraLabel1.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel1.Location = new System.Drawing.Point(204, 174);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel1.TabIndex = 4;
            this.ultraLabel1.Text = "ultraLabel1";
            // 
            // ControlTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(561, 290);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.btnCardParse);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Name = "ControlTest";
            this.Text = "ControlTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button btnCardParse;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private System.DirectoryServices.DirectoryEntry directoryEntry1;
    }
}