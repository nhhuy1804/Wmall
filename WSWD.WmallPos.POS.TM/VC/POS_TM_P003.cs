using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;


using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.UserControls;

namespace WSWD.WmallPos.POS.TM.VC
{
    public class POS_TM_P003 : PopupBase
    {
        private WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel gridPanel1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.Button button1;

        public POS_TM_P003()
        {
            InitializeComponent();
            this.Load += new EventHandler(POS_TM_P003_Load);
        }

        void POS_TM_P003_Load(object sender, EventArgs e)
        {
            gridPanel1.AddColumn("Title", "타이틀");
            gridPanel1.AddColumn("NoticeDate", "날짜", 90);
            gridPanel1.AddColumn("ReadCount", "조회", 80);

            gridPanel1.RowDataBound += new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanelRowDataBoundEventHandler(gridPanel1_RowDataBound);
            gridPanel1.RowIndexChanged += new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanelRowIndexChangedEventHandler(gridPanel1_RowIndexChanged);

            DataTable dt = new DataTable();
            for (int i = 0; i < 200; i++)
            {
                var row = dt.NewRow();
                dt.Rows.Add(row);
                gridPanel1.AddRow(row);
            }
        }

        void gridPanel1_RowIndexChanged(WSWD.WmallPos.POS.FX.Win.UserControls.GridRow beforeRow, WSWD.WmallPos.POS.FX.Win.UserControls.GridRow afterRow)
        {
            
        }

        void gridPanel1_RowDataBound(WSWD.WmallPos.POS.FX.Win.UserControls.GridRow row)
        {
            if (row.RowState == GridRowState.Added)
            {
                // init cells
                row.Cells[0].Controls.Add(new Label()
                {
                    Text = "Title",
                    AutoSize = false,
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill
                });

                row.Cells[1].Controls.Add(new Label()
                {
                    Text = "Date",
                    AutoSize = false,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                });

                row.Cells[2].Controls.Add(new Label()
                {
                    Text = new Random().Next(100).ToString(),
                    AutoSize = false,
                    TextAlign = System.Drawing.ContentAlignment.BottomRight,
                    Dock = DockStyle.Fill
                });
            }
        }


        private void InitializeComponent()
        {
            this.button1 = new WSWD.WmallPos.POS.FX.Win.UserControls.Button();
            this.gridPanel1 = new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel();
            this.ContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContainerPanel
            // 
            this.ContainerPanel.Controls.Add(this.gridPanel1);
            this.ContainerPanel.Controls.Add(this.button1);
            this.ContainerPanel.Size = new System.Drawing.Size(751, 469);
            // 
            // button1
            // 
            this.button1.BorderSize = 1;
            this.button1.Corner = 3;
            this.button1.Location = new System.Drawing.Point(165, 372);
            this.button1.Name = "button1";
            this.button1.Selected = false;
            this.button1.Size = new System.Drawing.Size(90, 42);
            this.button1.TabIndex = 0;
            this.button1.Text = "닫기";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // gridPanel1
            // 
            this.gridPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.gridPanel1.BorderWidth = new System.Windows.Forms.Padding(1);
            this.gridPanel1.CurrentRowIndex = -1;
            this.gridPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gridPanel1.Location = new System.Drawing.Point(10, 10);
            this.gridPanel1.Name = "gridPanel1";
            this.gridPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.gridPanel1.Size = new System.Drawing.Size(731, 264);
            this.gridPanel1.TabIndex = 1;
            // 
            // POS_TM_P003
            // 
            this.ClientSize = new System.Drawing.Size(751, 509);
            this.Name = "POS_TM_P003";
            this.ContainerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
