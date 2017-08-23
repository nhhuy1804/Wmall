using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using WSWD.WmallPos.POS.FX.Win.Forms;
using System.Data;
using WSWD.WmallPos.FX.Shared.Data;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using System.Windows.Forms;
using WSWD.WmallPos.POS.FX.Win.Devices;

namespace WSWD.WmallPos.POS.TM.VC
{
    public class POS_TM_M005 : FormBase
    {
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel saleGridPanel1;
        private Label label1;
        private WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel gridPanel1;

        public POS_TM_M005()
        {
            InitializeComponent();
            this.Load += new EventHandler(POS_TM_M005_Load);
            saleGridPanel1.ScrollType = ScrollTypes.PageChanged;
            saleGridPanel1.SetColumn(0, "NO", 30);
            saleGridPanel1.SetColumn(1, "상품명");
            saleGridPanel1.SetColumn(2, "수량", 40);
            saleGridPanel1.SetColumn(3, "단가", 100);
            saleGridPanel1.SetColumn(4, "%", 60);
            saleGridPanel1.SetColumn(5, "할인", 60);
            saleGridPanel1.SetColumn(6, "금액", 110);

            saleGridPanel1.RowSelected += new RowSelectedEventHandler(saleGridPanel1_RowSelected);
            saleGridPanel1.InitializeCell += new CellDataBoundEventHandler(saleGridPanel1_InitializeCell);
            saleGridPanel1.CellDataBound += new CellDataBoundEventHandler(saleGridPanel1_CellDataBound);
            saleGridPanel1.PageIndexChanged += new EventHandler(saleGridPanel1_PageIndexChanged);

        }

        void saleGridPanel1_PageIndexChanged(object sender, EventArgs e)
        {
            
        }

        void saleGridPanel1_CellDataBound(CellDataBoundEventArgs e)
        {
            List<string> values = e.ItemData as List<string>;
            if (values != null)
            {
                e.Cell.Controls[0].Text = values[e.Cell.ColumnIndex];
            }
            else
            {
                e.Cell.Controls[0].Text = string.Empty;
            }
        }

        void saleGridPanel1_InitializeCell(CellDataBoundEventArgs e)
        {
            Label lbl = new Label()
            {
                Text = string.Empty,
                AutoSize = false,
                Dock = DockStyle.Fill
            };

            e.Cell.Controls.Add(lbl);   
        }

        void saleGridPanel1_RowSelected(RowChangingEventArgs e)
        {
            
        }

        void POS_TM_M005_Load(object sender, EventArgs e)
        {
            gridPanel1.AddColumn("Title", "타이틀");
            gridPanel1.AddColumn("NoticeDate", "날짜", 90);
            gridPanel1.AddColumn("ReadCount", "조회", 80);

            gridPanel1.RowDataBound += new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanelRowDataBoundEventHandler(gridPanel1_RowDataBound);
            gridPanel1.RowIndexChanged += new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanelRowIndexChangedEventHandler(gridPanel1_RowIndexChanged);

            var r = new Random();
            for (int i = 0; i < 12; i++)
            {
                List<string> values = new List<string>();
                for (int j = 0; j < saleGridPanel1.ColumnCount; j++)
                {
                    values.Add(r.Next(1000).ToString());
                }
                
                saleGridPanel1.AddRow(values);
            }            
        }

        void saleGridPanel1_RowSelected(SaleGridRow row)
        {
            label1.Text = row.RowIndex.ToString() + "/" + row.GridControl.PageIndex.ToString();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_TM_M005));
            this.gridPanel1 = new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.saleGridPanel1 = new WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // gridPanel1
            // 
            this.gridPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.gridPanel1.BorderWidth = new System.Windows.Forms.Padding(1);
            this.gridPanel1.CurrentRowIndex = -1;
            this.gridPanel1.Location = new System.Drawing.Point(13, 6);
            this.gridPanel1.Name = "gridPanel1";
            this.gridPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.gridPanel1.Size = new System.Drawing.Size(635, 266);
            this.gridPanel1.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 286);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(180, 286);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // saleGridPanel1
            // 
            this.saleGridPanel1.AutoFillRows = true;
            this.saleGridPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.saleGridPanel1.BorderWidth = new System.Windows.Forms.Padding(1);
            this.saleGridPanel1.ColumnCount = 7;
            this.saleGridPanel1.Location = new System.Drawing.Point(25, 350);
            this.saleGridPanel1.Name = "saleGridPanel1";
            this.saleGridPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.saleGridPanel1.PageIndex = -1;
            this.saleGridPanel1.RowCount = 5;
            this.saleGridPanel1.RowHeight = 57;
            this.saleGridPanel1.ScrollType = WSWD.WmallPos.POS.FX.Win.UserControls.ScrollTypes.IndexChanged;
            this.saleGridPanel1.SelectedRowIndex = -1;
            this.saleGridPanel1.Size = new System.Drawing.Size(691, 318);
            this.saleGridPanel1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 324);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 14);
            this.label1.TabIndex = 8;
            this.label1.Text = "label1";
            // 
            // POS_TM_M005
            // 
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Controls.Add(this.label1);
            this.Controls.Add(this.saleGridPanel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gridPanel1);
            this.Font = new System.Drawing.Font("Dotum", 10F, System.Drawing.FontStyle.Bold);
            this.Name = "POS_TM_M005";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrameBaseData.Current.OnDataChanged(FrameBaseDataItem.GotoMenu, string.Empty);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            POS_TM_P003 p = new POS_TM_P003();
            p.ShowDialog(this);
        }
    }
}
