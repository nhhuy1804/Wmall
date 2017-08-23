using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    /// <summary>
    /// 통신상태라벨
    /// </summary>
    public class CommStatusLabel : UserControl
    {
        #region 변수

        private Dictionary<string, string> m_items; 

        #endregion

        #region 생성자, 초기화

        public CommStatusLabel()
        {
            InitializeComponent();
            m_items = new Dictionary<string, string>();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CommStatusLabel
            // 
            this.Name = "CommStatusLabel";
            this.Size = new System.Drawing.Size(252, 59);
            this.ResumeLayout(false);

        }

        #endregion

        #region 사용자정의

        private void RefreshItems()
        {
            this.SuspendDrawing();
            int totalWidth = 0;
            foreach (var item in m_items.Keys)
            {
                Label lbl = null;
                if (this.Controls.ContainsKey(item))
                {
                    lbl = (Label)this.Controls.Find(item, false)[0];
                }
                else
                {
                    lbl = new Label()
                    {
                        Name = item,
                        AutoSize = false,
                        TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Left,
                        Padding = new Padding(5, 0, 5, 0)
                    };

                    this.Controls.Add(lbl);
                }

                lbl.Visible = false;
                lbl.AutoSize = true;
                lbl.Text = string.Format("{0} : {1}", item, m_items[item]);
                int w = lbl.Width;
                totalWidth += lbl.Width;
                lbl.AutoSize = false;
                lbl.Width = w;
                lbl.ForeColor = m_items[item].Contains("ER") ? Color.Red : this.ForeColor;
                lbl.Visible = true;
            }

            this.Width = totalWidth + 10;
            this.ResumeDrawing();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        public void UpdateItem(string label, string value)
        {
            if (m_items.ContainsKey(label))
            {
                m_items[label] = value;
            }
            else
            {
                m_items.Add(label, value);
            }

            RefreshItems();
        }

        #endregion
    }
}
