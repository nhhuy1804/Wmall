namespace WSWD.WmallPos.POS.CD.VC
{
    partial class POS_CD_P001
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAdd = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnEdit = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Location = new System.Drawing.Point(11, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1002, 562);
            this.panel1.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.BorderSize = 1;
            this.btnAdd.Corner = 3;
            this.btnAdd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnAdd.Image = null;
            this.btnAdd.IsHighlight = false;
            this.btnAdd.Location = new System.Drawing.Point(11, 590);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Selected = false;
            this.btnAdd.Size = new System.Drawing.Size(90, 42);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnEdit
            // 
            this.btnEdit.BorderSize = 1;
            this.btnEdit.Corner = 3;
            this.btnEdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnEdit.Image = null;
            this.btnEdit.IsHighlight = false;
            this.btnEdit.Location = new System.Drawing.Point(107, 590);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Selected = false;
            this.btnEdit.Size = new System.Drawing.Size(90, 42);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit";
            this.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // POS_CD_P001
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.panel1);
            this.Name = "POS_CD_P001";
            this.Size = new System.Drawing.Size(1022, 692);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private FX.Win.UserControls.Button btnAdd;
        private FX.Win.UserControls.Button btnEdit;
    }
}
