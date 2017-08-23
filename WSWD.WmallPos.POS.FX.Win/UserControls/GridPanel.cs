using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;


using WSWD.WmallPos.POS.FX.Win.Controls;
using System.Drawing;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.FX.Shared;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public delegate void GridPanelRowDataBoundEventHandler(GridRow row);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="beforeRow"></param>
    /// <param name="afterRow">this can be null, if null, no row is selected or GridPanel has no rows</param>
    public delegate void GridPanelRowIndexChangedEventHandler(GridRow beforeRow, GridRow afterRow);

    /// <summary>
    /// 행바꾸기전에..
    /// </summary>
    /// <param name="row"></param>
    public delegate void GridPanelBeforeRowIndexChangedEventHandler(BeforeRowIndexChangedEventArgs e);

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
    // [Designer(typeof(GridPanelDesigner))]
    public class GridPanel : BorderPanel, IKeyInputView, IGridControl
    {
        #region GridPanel event handler

        void GridPanel_Disposed(object sender, EventArgs e)
        {
            this.DetachKeyInput();
        }

        void GridPanel_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_UP || e.Key.OPOSKey == OPOSMapKeys.KEY_DOWN)
            {
                DoScroll(e.Key.OPOSKey == OPOSMapKeys.KEY_UP);
            }
        }

        /// <summary>
        /// Resize
        /// - reallocate position and height of rows
        /// - maximum row is RowCount
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GridPanel_SizeChanged(object sender, EventArgs e)
        {
            ResizeChilds();
        }

        void pnlUpDn_Resize(object sender, EventArgs e)
        {
            btnUp.Height = this.pnlUpDn.Height / 2;
            this.btnDown.Height = this.pnlUpDn.Height - this.btnUp.Height;
        }

        void GridPanel_Resize(object sender, EventArgs e)
        {
            ResizeChilds();
        }

        void btnUp_MouseUp(object sender, MouseEventArgs e)
        {
            RoundedButton bt = (RoundedButton)sender;
            if (bt.Name.Contains("Up"))
            {
                bt.Image = Properties.Resources.ico_list_up;
                DoScroll(true);
            }
            else
            {
                bt.Image = Properties.Resources.ico_list_dn;
                DoScroll(false);
            }
        }

        void btnUp_MouseDown(object sender, MouseEventArgs e)
        {
            RoundedButton bt = (RoundedButton)sender;
            if (bt.Name.Contains("Up"))
            {
                bt.Image = Properties.Resources.ico_list_up_P;
            }
            else
            {
                bt.Image = Properties.Resources.ico_list_dn_P;
            }
        }

        void row_Click(object sender, EventArgs e)
        {
            GridRow row = (GridRow)sender;
            if (row.IsEmpty)
            {
                return;
            }

            CurrentRowIndex = row.RowIndex;
        }

        #endregion

        #region Constructor

        public GridPanel()
        {
            InitializeComponent();

            m_columnKeyWidths = new Dictionary<string, int>();
            m_dataRows = new List<Object>();

            #region event registration

            this.pnlUpDn.Resize += new EventHandler(pnlUpDn_Resize);
            this.Resize += new EventHandler(GridPanel_Resize);
            this.KeyEvent += new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(GridPanel_KeyEvent);
            this.Disposed += new EventHandler(GridPanel_Disposed);
            this.AttachKeyInput();
            #endregion
        }

        #endregion

        #region Privates

        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton btnUp;
        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton btnDown;
        private System.Windows.Forms.Panel pnlUpDn;
        private Panel pnlContainer;
        private GridHeader gridHeader1;
        private Dictionary<string, int> m_columnKeyWidths = null;
        private List<Object> m_dataRows = null;
        private int m_currentRowIndex = -1;
        private int m_pageIndex = 0;

        #endregion

        #region Events

        /// <summary>
        /// Fire when row is added or updated with new data
        /// </summary>
        public event GridPanelRowDataBoundEventHandler RowDataBound;

        /// <summary>
        /// fire when row is changed
        /// </summary>
        public event GridPanelRowIndexChangedEventHandler RowIndexChanged;

        /// <summary>
        /// Before Row Index Changed
        /// </summary>
        public event GridPanelBeforeRowIndexChangedEventHandler BeforeRowIndexChanged;

        /// <summary>
        /// Column clicked
        /// </summary>
        public event GridHeaderColumnClickedHandler ColumnClicked;

        #endregion

        #region Properties

        /// <summary>
        /// 현재선택 된행의 index
        /// </summary>
        public int CurrentRowIndex
        {
            get
            {
                return m_currentRowIndex;
            }
            set
            {
                #region unselected row

                var beforeRow = GetRow(m_currentRowIndex);
                if (beforeRow != null)
                {
                    beforeRow.Selected = false;
                }

                #endregion

                GridRow newRow = null;
                if (value >= 0 && value < RowCount)
                {
                    newRow = GetRow(value);

                    #region scroll row into view

                    if (newRow.Top < 0)
                    {
                        // move down
                        RowMove(-1 * newRow.Top);
                    }
                    else
                    {
                        int topOffset = 0;
                        topOffset = this.pnlContainer.Height - (newRow.Height + newRow.Top);
                        if (topOffset < 0)
                        {
                            // move up
                            RowMove(topOffset);
                        }
                    }

                    #endregion

                    #region selected new row

                    newRow.Selected = true;

                    #endregion
                }

                #region fire event and set value

                m_currentRowIndex = value;
                if (RowIndexChanged != null)
                {
                    RowIndexChanged(beforeRow, newRow);
                }

                #endregion
            }
        }

        /// <summary>
        /// Set fixed row count
        /// </summary>        
        public int RowCount
        {
            get
            {
                return m_dataRows.Count;
            }
        }

        /// <summary>
        /// Set row height
        /// </summary>
        [Category("Appearance"), DefaultValue(42),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Browsable(true)]
        public int RowHeight
        {
            get
            {
                return 42;
            }
        }

        /// <summary>
        /// 컬럼수
        /// </summary>
        public int ColumnCount
        {
            get
            {
                return m_columnKeyWidths.Count;
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
        /// ScrollType
        /// Paging or Row changing
        /// </summary>
        public GridScrollType ScrollType { get; set; }

        public int PageIndex
        {
            get
            {
                return m_pageIndex;
            }
            set
            {
                if (this.ScrollType == GridScrollType.Row)
                {
                    return;
                }

                // Check having scroll?
                var childHeight = pnlContainer.Controls.Cast<Control>().Sum(p => p.Height);
                int containerHeight = this.pnlContainer.Height;
                if (containerHeight > childHeight)
                {
                    return;
                }

                if (value < 0)
                {
                    value = 0;
                }

                if (m_pageIndex == value)
                {
                    return;
                }

                int fromHeight = value * containerHeight;
                int toHeight = (value + 1) * containerHeight;

                int showStart = -1;
                int showEnd = -1;
                int height = 0;
                for (int i = 0; i < pnlContainer.Controls.Count; i++)
                {
                    height += RowHeight;

                    if (height >= fromHeight && showStart == -1)
                    {
                        showStart = i;
                    }

                    if (height >= toHeight)
                    {
                        showEnd = i;
                        if (showStart != -1)
                        {
                            break;
                        }
                    }
                }

                if (showStart == -1)
                {
                    return;
                }

                if (showEnd == -1)
                {
                    showEnd = pnlContainer.Controls.Count;
                }

                // hide all
                for (int i = 0; i < pnlContainer.Controls.Count; i++)
                {
                    pnlContainer.Controls[i].Visible = false;
                }

                // show page
                for (int i = showStart; i < showEnd; i++)
                {
                    pnlContainer.Controls[i].Top = (i - showStart) * RowHeight;
                }

                this.pnlContainer.SuspendDrawing();

                // show page
                for (int i = showStart; i < showEnd; i++)
                {
                    pnlContainer.Controls[i].Visible = true;
                }

                this.pnlContainer.ResumeDrawing();
                m_pageIndex = value;
            }
        }

        #endregion

        #region Methods, column settings

        /// <summary>
        /// Add column to grid
        /// </summary>
        /// <param name="key">field Name in datasource</param>
        /// <param name="caption"></param>
        /// <param name="width"></param>
        public void AddColumn(string key, string caption, int width)
        {
            m_columnKeyWidths.Add(key, width);
            gridHeader1.AddColumn(caption, width);
        }

        /// <summary>
        /// Add fill grid column
        /// </summary>
        /// <param name="key"></param>
        /// <param name="caption"></param>
        public void AddColumn(string key, string caption)
        {
            m_columnKeyWidths.Add(key, -1);
            gridHeader1.AddColumn(caption, -1);
        }

        public void AddRow(object rowData)
        {
            AddRow(rowData, true);
        }

        /// <summary>
        /// 행추가
        /// </summary>
        /// <param name="rowData"></param>
        public void AddRow(object rowData, bool setSelected)
        {
            m_dataRows.Add(rowData);

            // add Grid row
            GridRow row = new GridRow();
            row.Height = RowHeight;
            row.Width = this.pnlContainer.Width;
            row.Left = 0;
            row.Top = this.pnlContainer.Controls.Count == 0 ? 0 :
                this.pnlContainer.Controls[this.pnlContainer.Controls.Count - 1].Top + RowHeight;

            row.ItemData = m_dataRows[m_dataRows.Count - 1];
            row.GridControl = this;
            row.ColumnCount = gridHeader1.ColumnWidths.Length;
            row.Parent = pnlContainer;
            row.ColumnWidths = gridHeader1.ColumnWidths;
            row.Click += new EventHandler(row_Click);

            this.SuspendDrawing();
            row.LoadCells();

            // fire row event
            if (RowDataBound != null)
            {
                RowDataBound(row);
            }

            row.RowState = GridRowState.Existed;
            this.pnlContainer.Controls.Add(row);

            row.ForwareClickEvents();
            this.ResumeDrawing();

            if (setSelected)
            {
                // selected selected index
                CurrentRowIndex = this.pnlContainer.Controls.Count - 1;
            }
        }

        /// <summary>
        /// 행에 데이타업데이트
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="rowData"></param>
        public void UpdateRow(int rowIndex, DataRow rowData)
        {
            if (rowIndex < 0 || rowIndex > RowCount - 1)
            {
                return;
            }

            m_dataRows[rowIndex] = rowData;
            var gridRow = GetRow(rowIndex);
            gridRow.ItemData = rowData;

            // fire row event
            if (RowDataBound != null)
            {
                RowDataBound(GetRow(rowIndex));
            }
        }

        /// <summary>
        /// 행삭제
        /// </summary>
        /// <param name="rowIndex"></param>
        public void DeleteRow(int rowIndex)
        {
            DeleteRow(rowIndex, true);
        }

        void DeleteRow(int rowIndex, bool setRowIndex)
        {
            if (rowIndex < 0 || rowIndex > RowCount - 1)
            {
                return;
            }

            // delete GridRow control
            this.pnlContainer.Controls[rowIndex].Dispose();

            // clear data in list
            m_dataRows.RemoveAt(rowIndex);

            if (setRowIndex)
            {
                // set selected row again
                if (rowIndex >= 0 && rowIndex < RowCount)
                {
                    CurrentRowIndex = rowIndex;
                }
                else
                {
                    CurrentRowIndex = RowCount - 1;
                }
            }

            ResizeChilds();
        }

        /// <summary>
        /// 현재선택행을 삭제한다
        /// </summary>
        public void DeleteActiveRow()
        {
            DeleteRow(CurrentRowIndex);
        }

        public GridRow GetRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex > RowCount - 1)
            {
                return null;
            }
            return this.pnlContainer.Controls[rowIndex] as GridRow;
        }

        public void ClearAll()
        {
            // delete GridRow control
            this.pnlContainer.Controls.Clear();

            // clear data in list
            m_dataRows.Clear();

        }

        void DoScroll(bool isUp)
        {
            if (isUp)
            {
                #region Move Row Up

                if (ScrollType == GridScrollType.Row && CurrentRowIndex > 0)
                {
                    bool cancelEvent = false;
                    if (BeforeRowIndexChanged != null)
                    {
                        var ev = new BeforeRowIndexChangedEventArgs()
                        {
                            Cancel = false,
                            Row = GetRow(CurrentRowIndex),
                            MoveUp = true
                        };

                        BeforeRowIndexChanged(ev);
                        cancelEvent = ev.Cancel;
                    }

                    if (!cancelEvent)
                    {
                        CurrentRowIndex--;
                    }

                    return;
                }

                #endregion

                #region PageUp

                PageIndex--;

                #endregion
            }
            else
            {
                #region Row Move Down

                if (ScrollType == GridScrollType.Row && CurrentRowIndex < RowCount - 1)
                {
                    bool cancelEvent = false;
                    if (BeforeRowIndexChanged != null)
                    {
                        var ev = new BeforeRowIndexChangedEventArgs()
                        {
                            Cancel = false,
                            Row = GetRow(CurrentRowIndex),
                            MoveUp = false
                        };

                        BeforeRowIndexChanged(ev);
                        cancelEvent = ev.Cancel;
                    }

                    if (!cancelEvent)
                    {
                        CurrentRowIndex++;
                    }

                    return;
                }

                #endregion

                PageIndex++;
            }
        }

        #endregion

        #region Rendering methods

        private void InitializeComponent()
        {
            this.btnUp = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.btnDown = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.pnlUpDn = new System.Windows.Forms.Panel();
            this.gridHeader1 = new WSWD.WmallPos.POS.FX.Win.UserControls.GridHeader();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.pnlUpDn.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUp
            // 
            this.btnUp.BorderSize = 1;
            this.btnUp.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type04;
            this.btnUp.Corner = 0;
            this.btnUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnUp.Image = global::WSWD.WmallPos.POS.FX.Win.Properties.Resources.ico_list_up;
            this.btnUp.Location = new System.Drawing.Point(0, 0);
            this.btnUp.Name = "btnUp";
            this.btnUp.Selected = false;
            this.btnUp.Size = new System.Drawing.Size(33, 135);
            this.btnUp.TabIndex = 0;
            this.btnUp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnUp_MouseDown);
            this.btnUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnUp_MouseUp);
            // 
            // btnDown
            // 
            this.btnDown.BorderSize = 1;
            this.btnDown.ButtonType = WSWD.WmallPos.POS.FX.Shared.ButtonTypes.Type04;
            this.btnDown.Corner = 0;
            this.btnDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(59)))), ((int)(((byte)(152)))));
            this.btnDown.Image = global::WSWD.WmallPos.POS.FX.Win.Properties.Resources.ico_list_dn;
            this.btnDown.Location = new System.Drawing.Point(0, 135);
            this.btnDown.Name = "btnDown";
            this.btnDown.Selected = false;
            this.btnDown.Size = new System.Drawing.Size(33, 133);
            this.btnDown.TabIndex = 0;
            this.btnDown.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnUp_MouseDown);
            this.btnDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnUp_MouseUp);
            // 
            // pnlUpDn
            // 
            this.pnlUpDn.Controls.Add(this.btnDown);
            this.pnlUpDn.Controls.Add(this.btnUp);
            this.pnlUpDn.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlUpDn.Location = new System.Drawing.Point(423, 1);
            this.pnlUpDn.Name = "pnlUpDn";
            this.pnlUpDn.Size = new System.Drawing.Size(33, 268);
            this.pnlUpDn.TabIndex = 1;
            // 
            // gridHeader1
            // 
            this.gridHeader1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.gridHeader1.BorderWidth = new System.Windows.Forms.Padding(0, 0, 1, 1);
            this.gridHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gridHeader1.Location = new System.Drawing.Point(1, 1);
            this.gridHeader1.Name = "gridHeader1";
            this.gridHeader1.Size = new System.Drawing.Size(422, 30);
            this.gridHeader1.TabIndex = 3;
            // 
            // pnlContainer
            // 
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(1, 31);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(422, 238);
            this.pnlContainer.TabIndex = 4;
            // 
            // GridPanel
            // 
            this.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.BorderWidth = new System.Windows.Forms.Padding(1);
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.gridHeader1);
            this.Controls.Add(this.pnlUpDn);
            this.Name = "GridPanel";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(457, 270);
            this.pnlUpDn.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Resize width of all rows
        /// </summary>
        private void ResizeChilds()
        {
            if (RowCount == 0)
            {
                return;
            }

            for (int i = 0; i < RowCount; i++)
            {
                // add Grid row
                GridRow row = (GridRow)this.pnlContainer.Controls[i];
                row.Width = this.pnlContainer.Width;
                row.Top = i * RowHeight;
            }
        }

        /// <summary>
        /// Moving rows
        /// </summary>
        /// <param name="topOffset"></param>
        private void RowMove(int topOffset)
        {
            this.SuspendDrawing();

            for (int i = 0; i < RowCount; i++)
            {
                // add Grid row
                var row = this.pnlContainer.Controls[i];
                row.Visible = false;
                row.Top += topOffset;
                row.Visible = true;
            }

            this.ResumeDrawing();
        }

        #endregion

        #region IKeyInputView Members

        public event WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler KeyEvent;

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

    public class BeforeRowIndexChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Set to cancel RowIndexChanged event
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Current row
        /// </summary>
        public GridRow Row { get; set; }

        /// <summary>
        /// MoveUp or Down
        /// </summary>
        public bool MoveUp { get; set; }
    }

    public enum GridScrollType
    {
        Row,
        Page
    }

    public interface IGridControl
    {
        void PerformColumnClicked(int columnIndex);
    }
}
