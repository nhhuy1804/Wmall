namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    partial class MainFormBase
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
            this.tmTaskTimer = new System.Windows.Forms.Timer(this.components);
            this.ChildContainer = new System.Windows.Forms.Panel();
            this.mainMenu1 = new WSWD.WmallPos.POS.FX.Win.UserControls.MainMenuV2();
            this.SuspendLayout();
            // 
            // tmTaskTimer
            // 
            this.tmTaskTimer.Interval = 1000;
            // 
            // ChildContainer
            // 
            this.ChildContainer.Location = new System.Drawing.Point(0, 48);
            this.ChildContainer.Name = "ChildContainer";
            this.ChildContainer.Size = new System.Drawing.Size(736, 692);
            this.ChildContainer.TabIndex = 4;
            // 
            // mainMenu1
            // 
            this.mainMenu1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mainMenu1.Location = new System.Drawing.Point(736, 48);
            this.mainMenu1.ModeSingleMenu = false;
            this.mainMenu1.Name = "mainMenu1";
            this.mainMenu1.Padding = new System.Windows.Forms.Padding(44, 50, 44, 0);
            this.mainMenu1.Size = new System.Drawing.Size(288, 692);
            this.mainMenu1.TabIndex = 3;
            this.mainMenu1.TopMenuKey = null;
            // 
            // MainFormBase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.ChildContainer);
            this.Controls.Add(this.mainMenu1);
            this.Location = new System.Drawing.Point(1920, 1080);
            this.Name = "MainFormBase";
            this.Controls.SetChildIndex(this.mainMenu1, 0);
            this.Controls.SetChildIndex(this.ChildContainer, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmTaskTimer;
        private System.Windows.Forms.Panel ChildContainer;
        private WSWD.WmallPos.POS.FX.Win.UserControls.MainMenuV2 mainMenu1;
    }
}

