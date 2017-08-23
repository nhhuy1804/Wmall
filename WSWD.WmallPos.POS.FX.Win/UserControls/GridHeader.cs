using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using WSWD.WmallPos.POS.FX.Win.Controls;
using System.ComponentModel;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public delegate void GridHeaderColumnClickedHandler(int columnIndex);

    /// <summary>
    /// Grid header
    /// </summary>
    public class GridHeader : BorderPanel
    {
        #region Variables

        private List<string> m_columnNames = null;
        private List<int> m_columnWidths = null;
        //private List<int> m_realColumnWidths = null;

        #endregion

        public GridHeader()
        {
            m_columnNames = new List<string>();
            m_columnWidths = new List<int>();
            this.MouseUp += new MouseEventHandler(GridHeader_MouseUp);            
        }

        public void AddColumn(string caption, int width)
        {
            m_columnNames.Add(caption);
            m_columnWidths.Add(width);
        }

        public void SetColumnCount(int columnCount)
        {
            m_columnWidths = new List<int>();
            m_columnNames = new List<string>();
            for (int i = 0; i < columnCount; i++)
            {
                m_columnWidths.Add(0);
                m_columnNames.Add(string.Empty);
            }
        }

        public void SetColumn(int columnIndex, int columnWidth, string caption)
        {
            m_columnNames[columnIndex] = caption;
            m_columnWidths[columnIndex] = columnWidth;
        }

        #region 속성 - Properties

        /// <summary>
        /// Header font
        /// </summary>
        private Font PaintFont
        {
            get
            {
                return new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Bold);
            }
        }

        /// <summary>
        /// Fixed border color
        /// </summary>
        public override Color BorderColor
        {
            get
            {
                return "#cacaca".FromHtmlColor();
            }
        }

        public override Font Font
        {
            get
            {
                return new Font("돋움", 10, FontStyle.Bold);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int[] ColumnWidths
        {
            get
            {
                List<int> columnWidths = new List<int>();
                List<int> fillColIndex = new List<int>();
                int fillColCount = 0;
                int otherTotalWidth = 0;

                for (int i = 0; i < m_columnWidths.Count; i++)
                {
                    int colWidth = m_columnWidths[i];
                    columnWidths.Add(colWidth);

                    if (colWidth == -1)
                    {
                        fillColCount++;
                        fillColIndex.Add(i);
                    }
                    else
                    {
                        otherTotalWidth += colWidth;
                    }
                }

                if (fillColCount > 0)
                {
                    int fillColWidth = (this.Width - otherTotalWidth) / fillColCount;
                    foreach (int colIdx in fillColIndex)
                    {
                        columnWidths[colIdx] = fillColWidth;
                    }
                }

                return columnWidths.ToArray();
            }
        }

        /// <summary>
        /// Grid Control
        /// </summary>
        public IGridControl GridControl
        {
            get
            {
                if (this.Parent != null)
                {
                    return (IGridControl)this.Parent;
                }

                return null;
            }
        }


        #endregion

        #region Event Handlers

        protected override void OnPaint(PaintEventArgs e)
        {
            Color topColor = "#ffffff".FromHtmlColor();
            Color bottomColor = "#eaeaea".FromHtmlColor();
            //Color borderColor = "#cacaca".FromHtmlColor();
            Color foreColor = "#333333".FromHtmlColor();

            Graphics g = e.Graphics;

            // Paint the outer rounded rectangle
            Rectangle outerRect = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            using (GraphicsPath outerPath = RoundedRectangle(outerRect, 1, 0))
            {
                using (LinearGradientBrush outerBrush = new LinearGradientBrush(outerRect, topColor, bottomColor,
                    LinearGradientMode.Vertical))
                {
                    g.FillPath(outerBrush, outerPath);
                }
            }

            base.OnPaint(e);

            if (m_columnNames == null || m_columnNames.Count == 0)
            {
                return;
            }

            // render column separator
            // #bdbdbd
            using (Pen pen = new Pen("#bdbdbd".FromHtmlColor()))
            {
                List<Point> points = new List<Point>();
                int x = 0;
                for (int i = 0; i < ColumnWidths.Length - 1; i++)
                {
                    x += ColumnWidths[i];
                    var p1 = new Point()
                    {
                        X = x,
                        Y = this.Height / 4
                    };

                    var p2 = new Point()
                    {
                        X = x,
                        Y = 3 * this.Height / 4
                    };

                    g.DrawLine(pen, p1, p2);
                }
            }

            // draw labels
            #region draw texts
            using (SolidBrush backgroundBrush = new SolidBrush(foreColor))
            {
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Center;
                drawFormat.LineAlignment = StringAlignment.Center;

                float x = 0;
                for (int i = 0; i < ColumnWidths.Length; i++)
                {
                    var w = ColumnWidths[i];
                    var t = m_columnNames[i];
                    var rect = new RectangleF(x, 0, w, this.Height);
                    g.DrawString(t, this.PaintFont, backgroundBrush, rect, drawFormat);

                    x += w;
                }
            }

            #endregion
        }

        private GraphicsPath RoundedRectangle(Rectangle boundingRect, int cornerRadius, int margin)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddRectangle(boundingRect);

            //roundedRect.AddArc(boundingRect.X + margin, boundingRect.Y + margin, cornerRadius * 2, cornerRadius * 2, 180, 90);
            //roundedRect.AddArc(boundingRect.X + boundingRect.Width - margin - cornerRadius * 2, boundingRect.Y + margin, cornerRadius * 2, cornerRadius * 2, 270, 90);
            //roundedRect.AddArc(boundingRect.X + boundingRect.Width - margin - cornerRadius * 2, boundingRect.Y + boundingRect.Height - margin - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            //roundedRect.AddArc(boundingRect.X + margin, boundingRect.Y + boundingRect.Height - margin - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            //roundedRect.CloseFigure();
            return roundedRect;
        }


        void GridHeader_MouseUp(object sender, MouseEventArgs e)
        {
            int left = e.X;
            int colIndex = -1;
            int from = 0;
            int to = 0;
            for (int i = 0; i < this.ColumnWidths.Length - 1; i++)
            {
                to = 0;
                for (int j = i; j >= 0; j--)
                {
                    to += this.ColumnWidths[j];    
                }
                
                from = to - (i == 0 ? to : this.ColumnWidths[i]);

                if (left > from & (from >= to || left < to))
                {
                    colIndex = i;
                    break;
                }
            }

            if (colIndex >= 0 && GridControl != null)
            {
                GridControl.PerformColumnClicked(colIndex);
            }
        }

        #endregion
    }
}
