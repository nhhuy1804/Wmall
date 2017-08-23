using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win;
using WSWD.WmallPos.POS.FX.Win.Controls;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.FX.Win.Interfaces;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public delegate void RowDataBoundEventHandler(RowDataBoundEventArgs e);

    public delegate void CellDataBoundEventHandler(CellDataBoundEventArgs e);

    public delegate void RowSelectedEventHandler(RowChangingEventArgs e);

    /// <summary>
    /// ScrollablePanel with up/down control
    /// Features
    ///     - Add row with data binding
    ///         - raise event for binding
    ///     - Delete row by index
    ///     - Row update how to?
    ///     - Row binding with DataItem (DataRow)
    ///     - Datasource, row index?
    ///     - Lam sao update row? update sao cho coder tu thay doi data trong row by index?
    /// </summary>
    public class SaleGridPanel : BorderPanel, IKeyInputView, IGridControl
    {
        #region 변수

        private Panel pnlContainer;
        private GridHeader gridHeader1;
        private RoundedButton btnUp;
        private RoundedButton btnDown;

        private List<int> m_columnKeyWidths = null;

        private int m_pageCount = 0;
        private int m_pageIndex = -1;
        private int m_rowCount = 0;
        private int m_selectedRowIndex = -1;
        private int m_selectedVisibleIndex = -1;
        private List<SaleGridRow> m_rows;
        private List<object> m_dataRows;
        private bool m_autoFillRows = false;
        private Label lblPage;
        public TableLayoutPanel pnlUpDn;
        private int m_columnCount = 0;

        /// <summary>
        /// Grid Scroll Disabling
        /// </summary>
        public bool DisableScroll = false;

        #endregion

        #region 초기화

        public SaleGridPanel()
        {
            InitializeComponent();

            RegistEvents();
            InitPrivates();
        }

        private void InitializeComponent()
        {
            this.btnUp = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.pnlUpDn = new System.Windows.Forms.TableLayoutPanel();
            this.lblPage = new System.Windows.Forms.Label();
            this.btnDown = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.gridHeader1 = new WSWD.WmallPos.POS.FX.Win.UserControls.GridHeader();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.pnlUpDn.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUp
            // 
            this.btnUp.BackgroundImage = global::WSWD.WmallPos.POS.FX.Win.Properties.Resources.ico_list_dn;
            this.btnUp.BorderSize = 1;
            this.btnUp.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type04;
            this.btnUp.Corner = 0;
            this.btnUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnUp.Image = global::WSWD.WmallPos.POS.FX.Win.Properties.Resources.ico_list_up;
            this.btnUp.IsHighlight = false;
            this.btnUp.Location = new System.Drawing.Point(0, 0);
            this.btnUp.Margin = new System.Windows.Forms.Padding(0);
            this.btnUp.Name = "btnUp";
            this.btnUp.Selected = false;
            this.btnUp.Size = new System.Drawing.Size(32, 105);
            this.btnUp.TabIndex = 0;
            this.btnUp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnUp_MouseUp);
            // 
            // pnlUpDn
            // 
            this.pnlUpDn.ColumnCount = 1;
            this.pnlUpDn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlUpDn.Controls.Add(this.btnUp, 0, 0);
            this.pnlUpDn.Controls.Add(this.lblPage, 0, 1);
            this.pnlUpDn.Controls.Add(this.btnDown, 0, 2);
            this.pnlUpDn.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlUpDn.Location = new System.Drawing.Point(424, 0);
            this.pnlUpDn.Name = "pnlUpDn";
            this.pnlUpDn.Padding = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.pnlUpDn.RowCount = 3;
            this.pnlUpDn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlUpDn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.pnlUpDn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlUpDn.Size = new System.Drawing.Size(33, 270);
            this.pnlUpDn.TabIndex = 1;
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPage.Location = new System.Drawing.Point(3, 105);
            this.lblPage.Name = "lblPage";
            this.lblPage.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.lblPage.Size = new System.Drawing.Size(26, 60);
            this.lblPage.TabIndex = 2;
            this.lblPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDown
            // 
            this.btnDown.BorderSize = 1;
            this.btnDown.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type04;
            this.btnDown.Corner = 0;
            this.btnDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnDown.Image = global::WSWD.WmallPos.POS.FX.Win.Properties.Resources.ico_list_dn;
            this.btnDown.IsHighlight = false;
            this.btnDown.Location = new System.Drawing.Point(0, 165);
            this.btnDown.Margin = new System.Windows.Forms.Padding(0);
            this.btnDown.Name = "btnDown";
            this.btnDown.Selected = false;
            this.btnDown.Size = new System.Drawing.Size(32, 105);
            this.btnDown.TabIndex = 3;
            this.btnDown.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnUp_MouseUp);
            // 
            // gridHeader1
            // 
            this.gridHeader1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.gridHeader1.BorderWidth = new System.Windows.Forms.Padding(0, 0, 1, 1);
            this.gridHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gridHeader1.Location = new System.Drawing.Point(0, 0);
            this.gridHeader1.Name = "gridHeader1";
            this.gridHeader1.Size = new System.Drawing.Size(424, 30);
            this.gridHeader1.TabIndex = 3;
            // 
            // pnlContainer
            // 
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(0, 30);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(424, 240);
            this.pnlContainer.TabIndex = 4;
            // 
            // SaleGridPanel
            // 
            this.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.BorderWidth = new System.Windows.Forms.Padding(1);
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.gridHeader1);
            this.Controls.Add(this.pnlUpDn);
            this.Name = "SaleGridPanel";
            this.Size = new System.Drawing.Size(457, 270);
            this.pnlUpDn.ResumeLayout(false);
            this.pnlUpDn.PerformLayout();
            this.ResumeLayout(false);

        }

        private void RegistEvents()
        {
            this.SetStyle(
                System.Windows.Forms.ControlStyles.UserPaint |
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
                true);

            #region event registration

            this.Load += new EventHandler(SaleGridPanel_Load);
            this.pnlContainer.Resize += new EventHandler(GridPanel_Resize);
            this.KeyEvent += new OPOSKeyEventHandler(SaleGridPanel_KeyEvent);
            this.AttachKeyInput();
            this.Disposed += new EventHandler(SaleGridPanel_Disposed);
            #endregion
        }

        private void InitPrivates()
        {
            m_columnKeyWidths = new List<int>();
            m_dataRows = new List<object>();
            m_rows = new List<SaleGridRow>();
        }

        private void InitializeRows()
        {
            #region BuildGrid

            // add rows, add SaleGridRow
            this.SuspendLayout();
            this.pnlContainer.SuspendLayout();
            this.pnlContainer.Controls.Clear();

            for (int i = 0; i < RowCount; i++)
            {
                var row = new SaleGridRow()
                {
                    Top = i * RowHeight,
                    GridControl = this,
                    Width = this.pnlContainer.Width,
                    Height = RowHeight,
                    Parent = this.pnlContainer
                };
                row.Click += new EventHandler(row_Clicked);
                this.pnlContainer.Controls.Add(row);
                m_rows.Add(row);
            }

            this.pnlContainer.ResumeLayout();
            this.ResumeLayout();

            #endregion
        }

        private void BuildRows()
        {
            foreach (Control c in this.pnlContainer.Controls)
            {
                SaleGridRow row = c as SaleGridRow;
                row.ColumnWidths = gridHeader1.ColumnWidths;

                // InitializeRow event
                if (InitializeRow != null)
                {
                    InitializeRow(new RowDataBoundEventArgs()
                    {
                        Row = row
                    });
                }

                // if there is nothing doing with row,
                // make cells and fire InitializeCell event
                if (row.Controls.Count == 0 && ColumnCount > 0)
                {
                    #region Render cells

                    int left = 0;
                    for (int j = 0; j < gridHeader1.ColumnWidths.Length; j++)
                    {
                        SaleGridCell cell = new SaleGridCell()
                        {
                            Width = gridHeader1.ColumnWidths[j],
                            Height = RowHeight - 2,
                            Left = left,
                            Top = 1,
                            Parent = row
                        };

                        row.Controls.Add(cell);
                        if (InitializeCell != null)
                        {
                            InitializeCell(new CellDataBoundEventArgs()
                            {
                                Cell = cell
                            });
                        }

                        left += gridHeader1.ColumnWidths[j];
                    }

                    row.HasCell = true;

                    #endregion
                }

                if (!UnSelectable)
                {
                    row.ForwareClickEvents();
                }
            }
        }

        #endregion

        #region 화면 속성

        /// <summary>
        /// Selected visible row (maximum RowCount-1)
        /// </summary>
        public int SelectedVisibleIndex
        {
            get
            {
                return m_selectedVisibleIndex;
            }
            private set
            {

                // Get last selectedRow
                var oldRow = GetRowAtVisibleIndex(m_selectedVisibleIndex);

                if (oldRow != null)
                {
                    oldRow.Selected = false;
                }

                var row = GetRowAtVisibleIndex(value);
                bool cancel = false;

                if (RowSelected != null && row != null)
                {
                    RowChangingEventArgs e = new RowChangingEventArgs()
                    {
                        Row = row
                    };

                    RowSelected(e);
                    cancel = e.Cancel;
                }

                if (cancel)
                {
                    return;
                }

                m_selectedVisibleIndex = value;
                if (row != null)
                {
                    row.Selected = true;
                }
            }
        }

        /// <summary>
        /// Selected row data index
        /// </summary>
        public int SelectedRowIndex
        {
            get
            {
                return m_selectedRowIndex;
            }
            set
            {
                // KIEM TRA IN RANGE
                if (!value.InRange(0, m_dataRows.Count))
                {
                    return;
                }

                // SET VALUE
                m_selectedRowIndex = value;

                // SET VISIBLE SELECTED
                SelectedVisibleIndex = value % RowCount;// GetVisibleIndex(value);

                // SCROLL OR NOT?
                int pageIndex = GetPageIndexByRowIndex(value);
                if (pageIndex != PageIndex)
                {
                    // REFRESH DATA?
                    PageIndex = pageIndex;
                }
                else
                {
                    // Fire event for one row only
                    RefreshSelectedRow();
                }
            }
        }

        /// <summary>
        /// page count
        /// </summary>
        int PageCount
        {
            get
            {
                return m_pageCount;
            }
            set
            {
                m_pageCount = value;
                UpdatePageNo();
            }
        }

        /// <summary>
        /// Page index
        /// </summary>
        public int PageIndex
        {
            get
            {
                return m_pageIndex;
            }
            set
            {
                if (!value.InRange(0, m_pageCount))
                {
                    return;
                }

                bool changed = m_pageIndex != value;
                m_pageIndex = value;

                if (changed)
                {
                    RefreshPageRows();
                }

                // Change visible index
                // if visible index > max row - 1

                if (PageIndexChanged != null)
                {
                    PageIndexChanged(this, EventArgs.Empty);
                }

                UpdatePageNo();
            }
        }

        private void UpdatePageNo()
        {

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    lblPage.Text = m_pageCount <= 0 || m_pageIndex < 0 ? string.Empty :
                        string.Format("{0}\n/\n{1}", m_pageIndex + 1, m_pageCount);                    
                });
            }
            else
            {
                lblPage.Text = m_pageCount <= 0 || m_pageIndex < 0 ? string.Empty :
                    string.Format("{0}\n/\n{1}", m_pageIndex + 1, m_pageCount);
            }
        }

        /// <summary>
        /// Set fixed row count
        /// </summary>        
        public int RowCount
        {
            get
            {
                return m_rowCount;
            }
            set
            {
                m_rowCount = value;
                RowHeight = m_autoFillRows && RowCount > 0 ? this.pnlContainer.Height / RowCount : 42;
                InitializeRows();
            }
        }

        /// <summary>
        /// Set row height
        /// </summary>
        public int RowHeight
        {
            get;
            set;
        }

        public bool AutoFillRows
        {
            get
            {
                return m_autoFillRows;
            }
            set
            {
                m_autoFillRows = value;
                RowHeight = m_autoFillRows && RowCount > 0 ? this.pnlContainer.Height / RowCount : 42;
                ResizeRows();
            }
        }

        /// <summary>
        /// 컬럼수
        /// </summary>
        public int ColumnCount
        {
            get
            {
                return m_columnCount;
            }
            set
            {
                m_columnCount = value;
                gridHeader1.SetColumnCount(value);
                m_columnKeyWidths = new List<int>();
                for (int i = 0; i < value; i++)
                {
                    m_columnKeyWidths.Add(0);
                }
            }
        }

        /// <summary>
        /// always 1
        /// </summary>
        public override Padding BorderWidth
        {
            get
            {
                return new Padding(1);
            }
        }

        /// <summary>
        /// 고정태투리색갈
        /// </summary>
        public override Color BorderColor
        {
            get
            {
                return "#cacaca".FromHtmlColor();
            }
        }

        /// <summary>
        /// Scroll to page up/down or change active row index
        /// </summary>
        public ScrollTypes ScrollType { get; set; }

        /// <summary>
        /// 선택비활성화
        /// </summary>
        public bool UnSelectable { get; set; }

        /// <summary>
        /// 선택 못 하게
        /// </summary>
        public bool DisableSelection { get; set; }

        #endregion

        #region 데이타 속성

        /// <summary>
        /// 그리드의 행
        /// </summary>
        [Category("Data"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Browsable(false)]
        public SaleGridRow[] Rows
        {
            get
            {
                return m_rows.ToArray();
            }
        }

        /// <summary>
        /// 그리드의 데이타리스
        /// </summary>
        [Category("Data"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Browsable(false)]
        public object[] RowItems
        {
            get
            {
                return m_dataRows.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ShowPageNo
        {
            get
            {
                return lblPage.Visible;
            }
            set
            {
                lblPage.Visible = value;
            }
        }

        #endregion

        #region Events

        public event RowDataBoundEventHandler InitializeRow;

        public event CellDataBoundEventHandler InitializeCell;

        public event RowDataBoundEventHandler RowDataBound;

        public event CellDataBoundEventHandler CellDataBound;

        public event EventHandler DataBoundCompleted;

        public event RowSelectedEventHandler RowSelected;

        public event EventHandler PageIndexChanged;

        public event EventHandler RowClicked;

        /// <summary>
        /// Column clicked
        /// </summary>
        public event GridHeaderColumnClickedHandler ColumnClicked;

        #endregion

        #region 이벤트정의

        void SaleGridPanel_KeyEvent(OPOSKeyEventArgs e)
        {
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_UP || e.Key.OPOSKey == OPOSMapKeys.KEY_DOWN)
            {
                e.IsHandled = true;
                DoUpDown(e.Key.OPOSKey == OPOSMapKeys.KEY_UP);
            }
        }

        void SaleGridPanel_Disposed(object sender, EventArgs e)
        {
            this.DetachKeyInput();
        }

        void SaleGridPanel_Load(object sender, EventArgs e)
        {
            BuildRows();
        }

        void row_Clicked(object sender, EventArgs e)
        {
            SaleGridRow row = (SaleGridRow)sender;
            if (row.IsEmpty)
            {
                return;
            }

            if (DisableSelection)
            {
                return;
            }

            //SelectedVisibleIndex = row.VisibleIndex;
            SelectedRowIndex = row.RowIndex;
            if (RowClicked != null)
            {
                RowClicked(this, e);
            }
        }

        #endregion

        #region Methods, Column, Row Operations

        /// <summary>
        /// Add column to grid
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="width"></param>
        public void SetColumn(int columnIndex, string caption, int width)
        {
            gridHeader1.SetColumn(columnIndex, width, caption);
            m_columnKeyWidths[columnIndex] = width;
        }

        /// <summary>
        /// Set column caption
        /// and set Fill column
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="caption"></param>
        public void SetColumn(int columnIndex, string caption)
        {
            SetColumn(columnIndex, caption, -1);
        }

        /// <summary>
        /// Add empty row
        /// </summary>
        public int AddEmptyRow()
        {
            return AddRow(null);
        }

        /// <summary>
        /// Add row and now activate row
        /// </summary>
        /// <param name="itemData"></param>
        /// <returns></returns>
        public int AddRow(object itemData, params object[] userStates)
        {
            return AddRow(itemData, true, userStates);
        }

        /// <summary>
        /// 행추가
        /// </summary>
        /// <param name="itemData"></param>
        /// <param name="setSelectedIndex"></param>
        /// <param name="userStates"></param>
        /// <returns></returns>
        public int AddRow(object itemData, bool setSelectedIndex, params object[] userStates)
        {
            // add data
            m_dataRows.Add(itemData);

            // Set SelectedRowIndex
            // Update SelectedVisibleIndex
            PageCount = m_dataRows.Count / RowCount + (m_dataRows.Count % RowCount == 0 ? 0 : 1);
            if (setSelectedIndex)
            {
                SelectedRowIndex = m_dataRows.Count - 1;
            }

            return m_dataRows.Count - 1;
        }

        /// <summary>
        /// Update row data
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="itemData"></param>
        /// <param name="userStates"></param>
        public void UpdateRow(int rowIndex, object itemData, params object[] userStates)
        {
            if (rowIndex.InRange(0, m_dataRows.Count))
            {
                m_dataRows[rowIndex] = itemData;
                int visibleIndex = rowIndex % RowCount; //GetVisibleIndex(rowIndex);

                // Find assc SaleGridRow
                // if found, refresh DataBound
                // if not, pass
                if (visibleIndex.InRange(0, RowCount))
                {
                    var row = GetRowAtVisibleIndex(visibleIndex);
                    FireRowEvent(row, itemData);
                }

                if (DataBoundCompleted != null)
                {
                    DataBoundCompleted(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Update data for row only
        /// </summary>
        /// <param name="row"></param>
        /// <param name="itemData"></param>
        public void SetRowData(SaleGridRow row, object itemData)
        {
            if (row.RowIndex.InRange(0, m_dataRows.Count))
                m_dataRows[row.RowIndex] = itemData;
            row.ItemData = itemData;
        }

        /// <summary>
        /// Update current row with data
        /// </summary>
        /// <param name="itemData"></param>
        public void UpdateCurrentRow(object itemData)
        {
            int selectedRowIndex = GetRowIndex(SelectedVisibleIndex);
            UpdateRow(selectedRowIndex, itemData);
        }

        /// <summary>
        /// Get selected SaleGridRow
        /// </summary>
        /// <returns></returns>
        public SaleGridRow GetSelectedRow()
        {
            return GetRowAtVisibleIndex(SelectedVisibleIndex);
        }

        /// <summary>
        /// Delete data for SelectedRowIndex
        /// Dont select any row
        /// </summary>
        public void DeleteSelectedRow()
        {
            if (SelectedRowIndex.InRange(0, m_dataRows.Count))
            {
                UpdateCurrentRow(null);
                m_dataRows.RemoveAt(SelectedRowIndex);
            }

            SelectedRowIndex = -1;
            SelectedVisibleIndex = -1;
            // RefreshVisibleRows();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        public void ClearAll()
        {
            m_dataRows.Clear();
            foreach (var row in m_rows)
            {
                row.ItemData = null;
            }
            PageCount = 0;
            SelectedRowIndex = -1;
            SelectedVisibleIndex = -1;
            RefreshPageRows();
        }

        public void DoUpDown(bool isUp)
        {
            if (isUp)
            {
                if (this.ScrollType == ScrollTypes.IndexChanged)
                {
                    this.SelectedRowIndex--;
                }
                else
                {
                    this.PageIndex--;
                }
            }
            else
            {
                if (this.ScrollType == ScrollTypes.IndexChanged)
                {
                    this.SelectedRowIndex++;
                }
                else
                {
                    this.PageIndex++;
                }
            }
        }

        #endregion

        #region 기타이벤트

        void btnUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (!DisableScroll)
            {
                RoundedButton bt = (RoundedButton)sender;
                if (bt.Name.Contains("Up"))
                {
                    DoUpDown(true);
                }
                else
                {
                    DoUpDown(false);
                }    
            }
        }

        void GridPanel_Resize(object sender, EventArgs e)
        {
            this.SuspendLayout();
            this.pnlContainer.SuspendLayout();
            for (int i = 0; i < this.pnlContainer.Controls.Count; i++)
            {
                // add Grid row
                SaleGridRow row = (SaleGridRow)this.pnlContainer.Controls[i];
                row.Top = i * RowHeight;
                row.Height = RowHeight;
                row.Width = this.pnlContainer.Width;
            }

            this.pnlContainer.ResumeLayout();
            this.ResumeLayout();
        }

        #endregion

        #region RowIndex, Visible Index

        /// <summary>
        /// Get visible index by row index
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns>Found: >=0, -1: row is not visible</returns>
        public int GetVisibleIndex(int rowIndex)
        {
            if (m_pageCount == 1)
            {
                return rowIndex;
            }

            int pageFromRowIndex = m_pageIndex * RowCount;
            int pageToRowIndex = (m_pageIndex + 1) * RowCount;
            if (rowIndex >= pageFromRowIndex && rowIndex < pageToRowIndex)
            {
                return (rowIndex - pageFromRowIndex) % RowCount;
            }

            // not in range
            return -1;
        }

        /// <summary>
        /// Get RowIndex by VisibleIndex
        /// </summary>
        /// <param name="visibleIndex"></param>
        /// <returns></returns>
        public int GetRowIndex(int visibleIndex)
        {
            return m_pageIndex * RowCount + visibleIndex;
        }

        private int GetPageIndexByRowIndex(int rowIndex)
        {
            int ridx = rowIndex + 1;
            return (ridx / RowCount + (ridx % RowCount == 0 ? 0 : 1)) - 1;
        }

        #endregion

        #region Binding Data Row Visible Rows

        /// <summary>
        /// Refresh DataBound
        /// </summary>
        public void RefreshPageRows()
        {
            ///
            /// TODO
            /// 1) REFRESH 0  ~ ROWCOUNT - 1
            /// 2) DATA INDEX
            ///     - PAGEINDEX
            ///     - IN THAT PAGE: FROM ~ TO
            /// 
            for (int i = 0; i < RowCount; i++)
            {
                var row = GetRowAtVisibleIndex(i);
                row.ItemData = null;

                int dataIndex = GetRowIndex(i);
                object data = null;
                if (dataIndex.InRange(0, m_dataRows.Count))
                {
                    data = m_dataRows[dataIndex];
                }

                FireRowEvent(row, data);
            }

            // SelectedVisibleIndex = SelectedVisibleIndex;

            if (DataBoundCompleted != null)
            {
                DataBoundCompleted(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Refresh new selected row
        /// </summary>
        private void RefreshSelectedRow()
        {
            var row = GetRowAtVisibleIndex(SelectedVisibleIndex);
            object data = m_dataRows[SelectedRowIndex];
            FireRowEvent(row, data);

            if (DataBoundCompleted != null)
            {
                DataBoundCompleted(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Find events
        /// </summary>
        /// <param name="row"></param>
        /// <param name="userStates"></param>
        private void FireRowEvent(SaleGridRow row, object itemData)
        {
            if (RowDataBound != null)
            {
                var e = new RowDataBoundEventArgs()
                {
                    Row = row,
                    ItemData = itemData
                };
                RowDataBound(e);
                if (!e.CancelSet)
                {
                    row.ItemData = itemData;
                }
            }

            if (row.HasCell)
            {
                if (CellDataBound != null)
                {
                    foreach (SaleGridCell cell in row.Cells)
                    {
                        var e = new CellDataBoundEventArgs()
                        {
                            Cell = cell,
                            ItemData = itemData
                        };

                        CellDataBound(e);

                        if (!e.CancelSet)
                        {
                            row.ItemData = itemData;
                        }
                    }
                }
            }
        }

        private void ResizeRows()
        {
            this.SuspendLayout();
            this.pnlContainer.SuspendLayout();
            for (int i = 0; i < this.pnlContainer.Controls.Count; i++)
            {
                var c = this.pnlContainer.Controls[i];
                c.Top = i * RowHeight;
                c.Height = RowHeight;
                c.Width = this.pnlContainer.Width;
            }

            this.pnlContainer.ResumeLayout();
            this.ResumeLayout();
        }

        /// <summary>
        /// Get row at visible index or return null
        /// </summary>
        /// <param name="visibleIndex"></param>
        /// <returns></returns>
        private SaleGridRow GetRowAtVisibleIndex(int visibleIndex)
        {
            return visibleIndex.InRange(0, m_rows.Count) ? m_rows[visibleIndex] : null;
        }

        #endregion

        #region IKeyInputView Members

        public event OPOSKeyEventHandler KeyEvent;

        public void PerformKeyEvent(OPOSKeyEventArgs e)
        {
            if (KeyEvent != null)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        KeyEvent(e);
                    });
                }
                else
                {
                    KeyEvent(e);
                }
            }
        }

        #endregion

        #region ColumnClickGeneric Members

        public void PerformColumnClicked(int columnIndex)
        {
            if (this.ColumnClicked != null)
            {
                this.ColumnClicked(columnIndex);
            }
        }

        #endregion
    }

    public class RowChangingEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public SaleGridRow Row { get; set; }
    }

    public class DataBoundEventArgs : EventArgs
    {
        public object ItemData { get; set; }

        /// <summary>
        /// Cancel set itemData to Row.ItemData?
        /// </summary>
        public bool CancelSet { get; set; }
    }

    public class RowDataBoundEventArgs : DataBoundEventArgs
    {
        public SaleGridRow Row { get; set; }
    }

    public class CellDataBoundEventArgs : DataBoundEventArgs
    {
        public SaleGridCell Cell { get; set; }
    }
}