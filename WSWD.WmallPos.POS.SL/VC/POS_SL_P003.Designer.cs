namespace WSWD.WmallPos.POS.SL.VC
{
    partial class POS_SL_P003
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
            this.gpHoldList = new WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel();
            this.btOK = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btClose = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.iptNoBoru = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.gpItemsList = new WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel();
            this.ButtonsPanel.SuspendLayout();
            this.ContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.iptNoBoru);
            this.ButtonsPanel.Controls.Add(this.btClose);
            this.ButtonsPanel.Controls.Add(this.btOK);
            this.ButtonsPanel.Size = new System.Drawing.Size(431, 62);
            // 
            // MessageBar
            // 
            this.MessageBar.Location = new System.Drawing.Point(17, 337);
            this.MessageBar.Size = new System.Drawing.Size(431, 46);
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.gpHoldList);
            this.ContainerPanel.Controls.Add(this.gpItemsList);
            this.ContainerPanel.Size = new System.Drawing.Size(465, 462);
            this.ContainerPanel.Controls.SetChildIndex(this.MessageBar, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.gpItemsList, 0);
            this.ContainerPanel.Controls.SetChildIndex(this.gpHoldList, 0);
            // 
            // gpHoldList
            // 
            this.gpHoldList.AutoFillRows = true;
            this.gpHoldList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.gpHoldList.BorderWidth = new System.Windows.Forms.Padding(1);
            this.gpHoldList.ColumnCount = 3;
            this.gpHoldList.Location = new System.Drawing.Point(17, 17);
            this.gpHoldList.Name = "gpHoldList";
            this.gpHoldList.Padding = new System.Windows.Forms.Padding(1);
            this.gpHoldList.PageIndex = -1;
            this.gpHoldList.RowCount = 6;
            this.gpHoldList.RowHeight = 45;
            this.gpHoldList.ScrollType = WSWD.WmallPos.POS.FX.Win.UserControls.ScrollTypes.PageChanged;
            this.gpHoldList.SelectedRowIndex = -1;
            this.gpHoldList.ShowPageNo = true;
            this.gpHoldList.Size = new System.Drawing.Size(431, 302);
            this.gpHoldList.TabIndex = 9;
            this.gpHoldList.UnSelectable = false;
            // 
            // btOK
            // 
            this.btOK.BorderSize = 1;
            this.btOK.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type03;
            this.btOK.Corner = 3;
            this.btOK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btOK.Image = null;
            this.btOK.IsHighlight = false;
            this.btOK.Location = new System.Drawing.Point(242, 20);
            this.btOK.Name = "btOK";
            this.btOK.Selected = false;
            this.btOK.Size = new System.Drawing.Size(90, 42);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "확인";
            this.btOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btClose
            // 
            this.btClose.BorderSize = 1;
            this.btClose.Corner = 3;
            this.btClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btClose.Image = null;
            this.btClose.IsHighlight = false;
            this.btClose.Location = new System.Drawing.Point(341, 20);
            this.btClose.Name = "btClose";
            this.btClose.Selected = false;
            this.btClose.Size = new System.Drawing.Size(90, 42);
            this.btClose.TabIndex = 0;
            this.btClose.Text = "닫기";
            this.btClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // iptNoBoru
            // 
            this.iptNoBoru.BackColor = System.Drawing.Color.White;
            this.iptNoBoru.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(109)))), ((int)(((byte)(195)))));
            this.iptNoBoru.BorderWidth = 2;
            this.iptNoBoru.Corner = 1;
            this.iptNoBoru.Focusable = true;
            this.iptNoBoru.FocusedIndex = 0;
            this.iptNoBoru.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.iptNoBoru.Format = null;
            this.iptNoBoru.HasBorder = true;
            this.iptNoBoru.IsFocused = true;
            this.iptNoBoru.Location = new System.Drawing.Point(7, 24);
            this.iptNoBoru.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.iptNoBoru.MaxLength = 16;
            this.iptNoBoru.Name = "iptNoBoru";
            this.iptNoBoru.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.iptNoBoru.PasswordMode = false;
            this.iptNoBoru.ReadOnly = false;
            this.iptNoBoru.Size = new System.Drawing.Size(227, 35);
            this.iptNoBoru.TabIndex = 1;
            this.iptNoBoru.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gpItemsList
            // 
            this.gpItemsList.AutoFillRows = true;
            this.gpItemsList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.gpItemsList.BorderWidth = new System.Windows.Forms.Padding(1);
            this.gpItemsList.ColumnCount = 4;
            this.gpItemsList.Location = new System.Drawing.Point(17, 17);
            this.gpItemsList.Name = "gpItemsList";
            this.gpItemsList.Padding = new System.Windows.Forms.Padding(1);
            this.gpItemsList.PageIndex = -1;
            this.gpItemsList.RowCount = 6;
            this.gpItemsList.RowHeight = 45;
            this.gpItemsList.ScrollType = WSWD.WmallPos.POS.FX.Win.UserControls.ScrollTypes.PageChanged;
            this.gpItemsList.SelectedRowIndex = -1;
            this.gpItemsList.ShowPageNo = true;
            this.gpItemsList.Size = new System.Drawing.Size(431, 302);
            this.gpItemsList.TabIndex = 10;
            this.gpItemsList.UnSelectable = false;
            // 
            // POS_SL_P003
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(471, 508);
            this.Name = "POS_SL_P003";
            this.Text = "보류해제";
            this.ButtonsPanel.ResumeLayout(false);
            this.ContainerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel gpHoldList;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btClose;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button btOK;
        private WSWD.WmallPos.POS.FX.Win.UserControls.InputText iptNoBoru;
        private WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel gpItemsList;
    }
}