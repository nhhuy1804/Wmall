namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    partial class FrameBase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrameBase));
            this.topBar1 = new WSWD.WmallPos.POS.FX.Win.UserControls.TopBarV2();
            this.statusBar1 = new WSWD.WmallPos.POS.FX.Win.UserControls.StatusBar();
            this.axCtrlKeyboard1 = new AxKeyBoardHook.AxCtrlKeyboard();
            ((System.ComponentModel.ISupportInitialize)(this.axCtrlKeyboard1)).BeginInit();
            this.SuspendLayout();
            // 
            // topBar1
            // 
            this.topBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(39)))), ((int)(((byte)(111)))));
            this.topBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar1.HasNotice = false;
            this.topBar1.Location = new System.Drawing.Point(0, 0);
            this.topBar1.Name = "topBar1";
            this.topBar1.ServerConnected = false;
            this.topBar1.Size = new System.Drawing.Size(1024, 48);
            this.topBar1.StateRefund = false;
            this.topBar1.TabIndex = 0;
            this.topBar1.TotalTransCount = 0;
            this.topBar1.UploadedTransCount = 0;
            // 
            // statusBar1
            // 
            this.statusBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(39)))), ((int)(((byte)(111)))));
            this.statusBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusBar1.Location = new System.Drawing.Point(0, 740);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(1024, 28);
            this.statusBar1.StatusMessage = "";
            this.statusBar1.TabIndex = 1;
            // 
            // axCtrlKeyboard1
            // 
            this.axCtrlKeyboard1.Enabled = true;
            this.axCtrlKeyboard1.Location = new System.Drawing.Point(428, 663);
            this.axCtrlKeyboard1.Name = "axCtrlKeyboard1";
            this.axCtrlKeyboard1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axCtrlKeyboard1.OcxState")));
            this.axCtrlKeyboard1.Size = new System.Drawing.Size(32, 32);
            this.axCtrlKeyboard1.TabIndex = 2;
            // 
            // FrameBase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.ControlBox = false;
            this.Controls.Add(this.axCtrlKeyboard1);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.topBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "FrameBase";
            this.ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)(this.axCtrlKeyboard1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.UserControls.TopBarV2 topBar1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.StatusBar statusBar1;
        private AxKeyBoardHook.AxCtrlKeyboard axCtrlKeyboard1;
    }
}

