namespace BasketTestApp
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
            this.btnPQ06 = new System.Windows.Forms.Button();
            this.txtTraceLog = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPQ01 = new System.Windows.Forms.Button();
            this.btnPU01 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnPP02RespData = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPQ06
            // 
            this.btnPQ06.Location = new System.Drawing.Point(3, 3);
            this.btnPQ06.Name = "btnPQ06";
            this.btnPQ06.Size = new System.Drawing.Size(101, 39);
            this.btnPQ06.TabIndex = 0;
            this.btnPQ06.Text = "PQ06";
            this.btnPQ06.UseVisualStyleBackColor = true;
            this.btnPQ06.Click += new System.EventHandler(this.btnPQ06_Click);
            // 
            // txtTraceLog
            // 
            this.txtTraceLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtTraceLog.Location = new System.Drawing.Point(0, 415);
            this.txtTraceLog.Multiline = true;
            this.txtTraceLog.Name = "txtTraceLog";
            this.txtTraceLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTraceLog.Size = new System.Drawing.Size(803, 82);
            this.txtTraceLog.TabIndex = 4;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 392);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(803, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 339);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.btnPP02RespData);
            this.panel1.Controls.Add(this.btnPQ01);
            this.panel1.Controls.Add(this.btnPU01);
            this.panel1.Controls.Add(this.btnPQ06);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(803, 45);
            this.panel1.TabIndex = 8;
            // 
            // btnPQ01
            // 
            this.btnPQ01.Location = new System.Drawing.Point(217, 3);
            this.btnPQ01.Name = "btnPQ01";
            this.btnPQ01.Size = new System.Drawing.Size(101, 39);
            this.btnPQ01.TabIndex = 1;
            this.btnPQ01.Text = "PQ01";
            this.btnPQ01.UseVisualStyleBackColor = true;
            this.btnPQ01.Click += new System.EventHandler(this.btnPQ01_Click);
            // 
            // btnPU01
            // 
            this.btnPU01.Location = new System.Drawing.Point(110, 3);
            this.btnPU01.Name = "btnPU01";
            this.btnPU01.Size = new System.Drawing.Size(101, 39);
            this.btnPU01.TabIndex = 0;
            this.btnPU01.Text = "PU01";
            this.btnPU01.UseVisualStyleBackColor = true;
            this.btnPU01.Click += new System.EventHandler(this.btnPU01_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 45);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(803, 347);
            this.dataGridView1.TabIndex = 9;
            // 
            // btnPP02RespData
            // 
            this.btnPP02RespData.Location = new System.Drawing.Point(324, 3);
            this.btnPP02RespData.Name = "btnPP02RespData";
            this.btnPP02RespData.Size = new System.Drawing.Size(101, 39);
            this.btnPP02RespData.TabIndex = 1;
            this.btnPP02RespData.Text = "PP02RespData";
            this.btnPP02RespData.UseVisualStyleBackColor = true;
            this.btnPP02RespData.Click += new System.EventHandler(this.btnPP02RespData_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(803, 497);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.txtTraceLog);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "바스겟 & 소겟통신";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPQ06;
        private System.Windows.Forms.TextBox txtTraceLog;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPU01;
        private System.Windows.Forms.Button btnPQ01;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnPP02RespData;
    }
}

