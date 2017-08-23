namespace WSWD.WmallPos.POS.TM.VC
{
    partial class POS_TM_P001
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
            this.roundedButton1 = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.roundedButton2 = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.ButtonsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.roundedButton2);
            this.ButtonsPanel.Controls.Add(this.roundedButton1);
            this.ButtonsPanel.Size = new System.Drawing.Size(752, 42);
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Size = new System.Drawing.Size(772, 497);
            // 
            // roundedButton1
            // 
            this.roundedButton1.BorderSize = 1;
            this.roundedButton1.Corner = 3;
            this.roundedButton1.Location = new System.Drawing.Point(659, 0);
            this.roundedButton1.Name = "roundedButton1";
            this.roundedButton1.Selected = false;
            this.roundedButton1.Size = new System.Drawing.Size(90, 42);
            this.roundedButton1.TabIndex = 0;
            this.roundedButton1.Text = "닫기";
            this.roundedButton1.Click += new System.EventHandler(this.roundedButton1_Click);
            // 
            // roundedButton2
            // 
            this.roundedButton2.BorderSize = 1;
            this.roundedButton2.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type02;
            this.roundedButton2.Corner = 3;
            this.roundedButton2.Location = new System.Drawing.Point(514, -3);
            this.roundedButton2.Name = "roundedButton2";
            this.roundedButton2.Selected = false;
            this.roundedButton2.Size = new System.Drawing.Size(90, 42);
            this.roundedButton2.TabIndex = 1;
            this.roundedButton2.Text = "확인";
            // 
            // POS_TM_P001
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(782, 547);
            this.Name = "POS_TM_P001";
            this.Text = "POS_TM_P001";
            this.ButtonsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton roundedButton1;
        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton roundedButton2;
    }
}