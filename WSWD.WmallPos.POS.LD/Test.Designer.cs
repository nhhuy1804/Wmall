namespace WSWD.WmallPos.POS.LD
{
    partial class Test
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Test));
            this.pnlTableGroup = new System.Windows.Forms.Panel();
            this.topBarV21 = new WSWD.WmallPos.POS.FX.Win.UserControls.TopBarV2();
            this.txtCout = new System.Windows.Forms.TextBox();
            this.btnEnter = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.SuspendLayout();
            // 
            // pnlTableGroup
            // 
            this.pnlTableGroup.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlTableGroup.Location = new System.Drawing.Point(9, 58);
            this.pnlTableGroup.Name = "pnlTableGroup";
            this.pnlTableGroup.Size = new System.Drawing.Size(811, 701);
            this.pnlTableGroup.TabIndex = 1;
            // 
            // topBarV21
            // 
            this.topBarV21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(39)))), ((int)(((byte)(111)))));
            this.topBarV21.HasNotice = false;
            this.topBarV21.Location = new System.Drawing.Point(0, 0);
            this.topBarV21.Name = "topBarV21";
            this.topBarV21.ServerConnected = false;
            this.topBarV21.Size = new System.Drawing.Size(1024, 48);
            this.topBarV21.StateRefund = false;
            this.topBarV21.TabIndex = 0;
            this.topBarV21.TotalTransCount = 0;
            this.topBarV21.UploadedTransCount = 0;
            // 
            // txtCout
            // 
            this.txtCout.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCout.Location = new System.Drawing.Point(830, 58);
            this.txtCout.Name = "txtCout";
            this.txtCout.Size = new System.Drawing.Size(186, 44);
            this.txtCout.TabIndex = 2;
            this.txtCout.Text = "10";
            // 
            // btnEnter
            // 
            this.btnEnter.BorderSize = 1;
            this.btnEnter.Corner = 3;
            this.btnEnter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnEnter.Image = null;
            this.btnEnter.IsHighlight = false;
            this.btnEnter.Location = new System.Drawing.Point(830, 109);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Selected = false;
            this.btnEnter.Size = new System.Drawing.Size(90, 42);
            this.btnEnter.TabIndex = 3;
            this.btnEnter.Text = "button1";
            this.btnEnter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.btnEnter);
            this.Controls.Add(this.txtCout);
            this.Controls.Add(this.pnlTableGroup);
            this.Controls.Add(this.topBarV21);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Test";
            this.Text = "Test";
            this.Load += new System.EventHandler(this.Test_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FX.Win.UserControls.TopBarV2 topBarV21;
        private System.Windows.Forms.Panel pnlTableGroup;
        private System.Windows.Forms.TextBox txtCout;
        private FX.Win.UserControls.Button btnEnter;
    }
}