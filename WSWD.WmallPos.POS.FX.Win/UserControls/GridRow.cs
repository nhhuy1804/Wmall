using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Reflection;

using WSWD.WmallPos.POS.FX.Win.Controls;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public class GridRow : BorderPanel
    {
        /// <summary>
        /// BackgroundColor for selected row
        /// </summary>
        public static Color BGC_ROW_SELECTED = Color.FromArgb(242, 210, 211);// "#efe8f6".FromHtmlColor();

        public GridRow()
        {
            this.RowState = GridRowState.Added;
        }

        internal void LoadCells()
        {
            // adding cells
            int x = 0;
            foreach (int colWidth in this.ColumnWidths)
            {
                GridCell cell = new GridCell();
                cell.Row = this;
                cell.BackColor = Color.Transparent;
                cell.Top = 0;
                cell.Left = x;
                cell.Height = this.Height;
                cell.Width = colWidth;

                this.SuspendLayout();
                this.Controls.Add(cell);
                this.ResumeLayout();

                x += colWidth;
            }
        }

        /// <summary>
        /// GridControl
        /// </summary>
        public GridPanel GridControl { get; internal set; }

        /// <summary>
        /// RowIndex
        /// </summary>
        public int RowIndex
        {
            get
            {
                if (this.Parent != null)
                {
                    return this.Parent.Controls.GetChildIndex(this);                    
                }

                return -1;
            }
        }

        /// <summary>
        /// 행배경색
        /// </summary>
        public override Color BackColor
        {
            get
            {
                if (Selected)
                {
                    return BGC_ROW_SELECTED;
                }

                if (this.Parent != null)
                {
                    int idx = this.Parent.Controls.GetChildIndex(this, false);
                    if (idx % 2 == 1)
                    {
                        //return "#f6f6f6".FromHtmlColor();
                        return Color.FromArgb(246, 246, 246);
                    }
                }

                return Color.White;
            }            
        }

        /// <summary>
        /// Row data
        /// </summary>
        public Object ItemData { get; set; }

        /// <summary>
        /// Grid Row State
        /// </summary>
        public GridRowState RowState { get; set; }

        private bool m_selected = false;
        /// <summary>
        /// 선택여부
        /// </summary>
        public bool Selected
        {
            get
            {
                return m_selected;
            }
            set
            {
                m_selected = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 기본태투리색갈
        /// </summary>
        public override Color BorderColor
        {
            get
            {
                if (this.Parent != null)
                {
                    return Color.FromArgb(202, 202, 202);
                    /*
                    int idx = this.Parent.Controls.GetChildIndex(this);
                    if (idx % 2 == 0)
                    {
                        return Color.FromArgb(202, 202, 202);
                    }*/
                }

                return Color.Empty;
            }
        }
        
        /// <summary>
        /// 태투리사이즈
        /// </summary>
        public override Padding BorderWidth
        {
            get
            {                
                if (this.Parent != null)
                {
                    int idx = this.Parent.Controls.GetChildIndex(this);
                    if (idx % 2 == 1)
                    {
                        return new Padding(0, 1, 0, 1);
                    }
                }

                return new Padding(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// 컬럼수
        /// </summary>
        public int ColumnCount { get; internal set; }

        /// <summary>
        /// 컬럼넓이
        /// </summary>
        public int[] ColumnWidths { get; internal set; }

        public GridCell[] Cells
        {
            get
            {
                return this.Controls.Cast<GridCell>().ToArray();
            }
        }

        /// <summary>
        /// IsEmpty DataRow
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return ItemData == null;
            }
        }

        public void ClickHandler(object sender, EventArgs e)
        {
            this.OnClick(EventArgs.Empty);
        }

        internal void ForwareClickEvents()
        {
            foreach (Control c in this.Controls)
            {
                ForwardEvents(c);
            }
        }

        private void ForwardEvents(Control c)
        {
            SetClickEvents(c);
            foreach (Control ct in c.Controls)
            {
                ForwardEvents(ct);
            }
        }

        void SetClickEvents(Control c)
        {
            EventInfo ei = c.GetType().GetEvent("Click");
            Delegate d = Delegate.CreateDelegate(ei.EventHandlerType, this, "ClickHandler");
            ei.AddEventHandler(c, d);
        }
    }

    public enum GridRowState
    {
        Added,
        Existed
    }
}
