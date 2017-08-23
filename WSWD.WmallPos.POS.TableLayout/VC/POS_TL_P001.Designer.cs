namespace WSWD.WmallPos.POS.TableLayout.VC
{
    partial class POS_TL_P001
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
            this.pnlTable = new System.Windows.Forms.Panel();
            this.btnAdd = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnCancel = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnEdit = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.btnDelete = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.cbxFloor = new System.Windows.Forms.ComboBox();
            this.btnBack = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.cbxAddTable = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // pnlTable
            // 
            this.pnlTable.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pnlTable.Location = new System.Drawing.Point(16, 10);
            this.pnlTable.Name = "pnlTable";
            this.pnlTable.Size = new System.Drawing.Size(990, 580);
            this.pnlTable.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.BorderSize = 1;
            this.btnAdd.Corner = 3;
            this.btnAdd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnAdd.Image = null;
            this.btnAdd.IsHighlight = false;
            this.btnAdd.Location = new System.Drawing.Point(158, 595);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Selected = false;
            this.btnAdd.Size = new System.Drawing.Size(84, 31);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BorderSize = 1;
            this.btnCancel.Corner = 3;
            this.btnCancel.Enabled = false;
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnCancel.Image = null;
            this.btnCancel.IsHighlight = false;
            this.btnCancel.Location = new System.Drawing.Point(273, 632);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Selected = false;
            this.btnCancel.Size = new System.Drawing.Size(84, 31);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BorderSize = 1;
            this.btnEdit.Corner = 3;
            this.btnEdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnEdit.Image = null;
            this.btnEdit.IsHighlight = false;
            this.btnEdit.Location = new System.Drawing.Point(273, 595);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Selected = false;
            this.btnEdit.Size = new System.Drawing.Size(84, 31);
            this.btnEdit.TabIndex = 7;
            this.btnEdit.Text = "Edit";
            this.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BorderSize = 1;
            this.btnDelete.Corner = 3;
            this.btnDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnDelete.Image = null;
            this.btnDelete.IsHighlight = false;
            this.btnDelete.Location = new System.Drawing.Point(387, 595);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Selected = false;
            this.btnDelete.Size = new System.Drawing.Size(84, 31);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // cbxFloor
            // 
            this.cbxFloor.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxFloor.FormattingEnabled = true;
            this.cbxFloor.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cbxFloor.Location = new System.Drawing.Point(885, 593);
            this.cbxFloor.Name = "cbxFloor";
            this.cbxFloor.Size = new System.Drawing.Size(121, 33);
            this.cbxFloor.TabIndex = 10;
            this.cbxFloor.Text = "1";
            this.cbxFloor.SelectedIndexChanged += new System.EventHandler(this.cbxFloor_SelectedIndexChanged);
            // 
            // btnBack
            // 
            this.btnBack.BorderSize = 1;
            this.btnBack.Corner = 3;
            this.btnBack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnBack.Image = null;
            this.btnBack.IsHighlight = false;
            this.btnBack.Location = new System.Drawing.Point(387, 632);
            this.btnBack.Name = "btnBack";
            this.btnBack.Selected = false;
            this.btnBack.Size = new System.Drawing.Size(84, 31);
            this.btnBack.TabIndex = 11;
            this.btnBack.Text = "Back";
            this.btnBack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(831, 601);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 18);
            this.label1.TabIndex = 12;
            this.label1.Text = "Floor";
            // 
            // btnSave
            // 
            this.btnSave.BorderSize = 1;
            this.btnSave.Corner = 3;
            this.btnSave.Enabled = false;
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnSave.Image = null;
            this.btnSave.IsHighlight = false;
            this.btnSave.Location = new System.Drawing.Point(158, 632);
            this.btnSave.Name = "btnSave";
            this.btnSave.Selected = false;
            this.btnSave.Size = new System.Drawing.Size(84, 31);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cbxAddTable
            // 
            this.cbxAddTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxAddTable.FormattingEnabled = true;
            this.cbxAddTable.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cbxAddTable.Location = new System.Drawing.Point(16, 593);
            this.cbxAddTable.Name = "cbxAddTable";
            this.cbxAddTable.Size = new System.Drawing.Size(121, 33);
            this.cbxAddTable.TabIndex = 14;
            this.cbxAddTable.Text = "1";
            // 
            // POS_TL_P001
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.cbxAddTable);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.cbxFloor);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.pnlTable);
            this.Name = "POS_TL_P001";
            this.Size = new System.Drawing.Size(1022, 692);
            this.Load += new System.EventHandler(this.POS_TL_P001_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTable;
        private FX.Win.UserControls.Button btnAdd;
        private FX.Win.UserControls.Button btnCancel;
        private FX.Win.UserControls.Button btnEdit;
        private FX.Win.UserControls.Button btnDelete;
        private System.Windows.Forms.ComboBox cbxFloor;
        private FX.Win.UserControls.Button btnBack;
        private System.Windows.Forms.Label label1;
        private FX.Win.UserControls.Button btnSave;
        private System.Windows.Forms.ComboBox cbxAddTable;
    }
}
