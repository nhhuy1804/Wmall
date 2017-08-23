using System.ComponentModel;
namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    partial class InputControl
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
            this.TextControl = new WSWD.WmallPos.POS.FX.Win.UserControls.InputText();
            this.TitleControl = new WSWD.WmallPos.POS.FX.Win.UserControls.InputLabel();
            this.SuspendLayout();
            // 
            // TextControl
            // 
            this.TextControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.TextControl.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.TextControl.BorderWidth = 1;
            this.TextControl.Corner = 1;
            this.TextControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextControl.Focusable = true;
            this.TextControl.FocusedIndex = 0;
            this.TextControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TextControl.Format = null;
            this.TextControl.HasBorder = true;
            this.TextControl.IsFocused = false;
            this.TextControl.Location = new System.Drawing.Point(73, 3);
            this.TextControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TextControl.Name = "TextControl";
            this.TextControl.PasswordMode = false;
            this.TextControl.ReadOnly = false;
            this.TextControl.Size = new System.Drawing.Size(144, 28);
            this.TextControl.TabIndex = 2;
            this.TextControl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TitleControl
            // 
            this.TitleControl.BackColor = System.Drawing.Color.Transparent;
            this.TitleControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.TitleControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TitleControl.Location = new System.Drawing.Point(3, 3);
            this.TitleControl.Name = "TitleControl";
            this.TitleControl.Size = new System.Drawing.Size(70, 28);
            this.TitleControl.TabIndex = 1;
            this.TitleControl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // InputControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.TextControl);
            this.Controls.Add(this.TitleControl);
            this.Name = "InputControl";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(220, 34);
            this.ResumeLayout(false);

        }

        #endregion

        private InputLabel TitleControl;

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), 
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public InputText TextControl;
    }
}
