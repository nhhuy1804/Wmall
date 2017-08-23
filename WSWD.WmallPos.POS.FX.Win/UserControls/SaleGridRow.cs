using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using WSWD.WmallPos.POS.FX.Win;
using WSWD.WmallPos.POS.FX.Win.Controls;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public partial class SaleGridRow : BorderPanel
    {
        /// <summary>
        /// BackgroundColor for selected row
        /// </summary>
        public static Color BGC_ROW_SELECTED = Color.FromArgb(242, 210, 211);// "#efe8f6".FromHtmlColor();

        public SaleGridRow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 그리드
        /// </summary>
        public SaleGridPanel GridControl { get; internal set; }

        /// <summary>
        /// RowIndex
        /// </summary>
        public int VisibleIndex
        {
            get
            {
                if (this.Parent != null && this.Parent.Controls.Contains(this))
                {
                    return this.Parent.Controls.GetChildIndex(this);
                }

                return -1;
            }
        }

        /// <summary>
        /// DataItemIndex
        /// </summary>
        public int RowIndex
        {
            get
            {
                return GridControl.PageIndex * GridControl.RowCount + VisibleIndex;
            }
        }

        /// <summary>
        /// Check Empty Row
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return ItemData == null;
            }
        }

        /// <summary>
        /// 행배경색
        /// </summary>
        public override Color BackColor
        {
            get
            {
                if (Selected && !this.GridControl.UnSelectable)
                {
                    return BGC_ROW_SELECTED;
                }

                if (this.Parent != null && this.Parent.Controls.Contains(this))
                {
                    int idx = this.Parent.Controls.GetChildIndex(this);
                    if (idx % 2 == 1)
                    {
                        return Color.FromArgb(246, 246, 246);
                    }
                }

                return Color.White;
            }
        }

        /// <summary>
        /// Row data
        /// </summary>
        public object ItemData { get; set; }

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

        public bool HasCell { get; set; }

        public SaleGridCell[] Cells
        {
            get
            {
                return this.Controls.Cast<SaleGridCell>().ToArray();
            }
        }

        /// <summary>
        /// 기본태투리색갈
        /// </summary>
        public override Color BorderColor
        {
            get
            {
                if (this.Parent != null && this.Parent.Controls.Contains(this))
                {
                    int idx = this.Parent.Controls.GetChildIndex(this);
                    if (idx % 2 == 1)
                    {
                        return Color.FromArgb(202, 202, 202);
                    }
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
                if (this.Parent != null && this.Parent.Controls.Contains(this))
                {
                    int idx = this.Parent.Controls.GetChildIndex(this);
                    if (idx % 2 == 1)
                    {
                        return new Padding(0, 1, 0, 1);
                    }
                }

                return new Padding(0);
            }
        }

        /// <summary>
        /// 컬럼수
        /// </summary>
        public int ColumnCount
        {
            get
            {
                return GridControl.ColumnCount;
            }
        }

        public int[] ColumnWidths { get; internal set; }

        public void PerformClick()
        {
            this.OnClick(EventArgs.Empty);
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

    public enum ScrollTypes
    {
        IndexChanged,
        PageChanged
    }
}
