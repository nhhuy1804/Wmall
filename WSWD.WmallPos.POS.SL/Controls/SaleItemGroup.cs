using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSWD.WmallPos.POS.SL.VI;
using WSWD.WmallPos.POS.SL.Data;

namespace WSWD.WmallPos.POS.SL.Controls
{
    public partial class SaleItemGroup : UserControl, ISLM001TouchGroup
    {
        int m_pageIndex = -1;
        WSWD.WmallPos.POS.FX.Win.UserControls.Button m_lastSelGroup = null;
        int PageIndex
        {
            get
            {
                return m_pageIndex;
            }
            set
            {
                int pi = Math.Min(value, m_pageCount > 0 ? m_pageCount - 1 : -1);
                if (pi > m_pageCount - 1)
                {
                    pi = m_pageCount - 1;
                }

                //if (pi < 0)
                //{
                //    m_pageIndex = pi;
                //    return;
                //}

                if (m_pageIndex == pi)
                {
                    return;
                }

                m_pageIndex = pi;

                // reshowing
                for (int i = 0; i < 7; i++)
                {
                    string btnName = string.Format("button{0}", i + 1);
                    Control btn = this.Controls[btnName];
                    btn.Text = string.Empty;
                    btn.Tag = null;
                    ((WSWD.WmallPos.POS.FX.Win.UserControls.Button)btn).Selected = false;

                    int gid = i + m_pageIndex * 7;
                    if (gid >= 0 && gid <= m_touchGroups.Length - 1)
                    {
                        btn.Text = m_touchGroups[gid].NmGrop;
                        btn.Tag = m_touchGroups[gid].CdGrop;
                    }
                }

            }
        }
        private int m_pageCount = 0;
        private TouchGroupData[] m_touchGroups = null;

        public SaleItemGroup()
        {
            InitializeComponent();
            for (int i = 0; i < 7; i++)
            {
                string btnName = string.Format("button{0}", i + 1);
                Control c = this.Controls[btnName];
                c.Click += new EventHandler(GroupButton_Click);
            }

            btnPrev.Click += new EventHandler(btnPrev_Click);
            btnNext.Click += new EventHandler(btnNext_Click);
        }

        public override Color BackColor
        {
            get
            {
                return this.Parent == null ? base.BackColor : this.Parent.BackColor;
            }
        }

        void btnNext_Click(object sender, EventArgs e)
        {
            PageIndex++;
        }

        void btnPrev_Click(object sender, EventArgs e)
        {
            PageIndex--;
        }

        void GroupButton_Click(object sender, EventArgs e)
        {
            if (OnTouch != null)
            {
                object tagData = null;

                if (sender != null)
                {
                    Control button = (Control)sender;
                    tagData = button.Tag;

                    if (tagData == null)
                    {
                        return;
                    }

                    // reset last selected group
                    if (m_lastSelGroup != null)
                    {
                        m_lastSelGroup.Selected = false;
                    }

                    m_lastSelGroup = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)button;
                    m_lastSelGroup.Selected = true;
                }
                else
                {
                    if (m_lastSelGroup != null)
                    {
                        m_lastSelGroup.Selected = false;
                    }

                    m_lastSelGroup = null;
                }

                OnTouch(new TouchEventArgs()
                    {
                        Target = TouchTarget.Group,
                        ItemData = tagData
                    });
            }
        }

        #region IM001TouchGroup Members

        /// <summary>
        /// TOUCH GROUP binding
        /// Refresh selected group
        /// </summary>
        /// <param name="touchGroups"></param>
        public void BindGroups(TouchGroupData[] touchGroups)
        {
            m_touchGroups = touchGroups;
            m_pageCount = touchGroups.Length / 7 + (touchGroups.Length % 7 == 0 ? 0 : 1);
            PageIndex = -1;
            PageIndex = touchGroups.Length > 0 ? 0 : -1;

            Control c = this.Controls["button1"];
            m_lastSelGroup = touchGroups.Length > 0 ? (WSWD.WmallPos.POS.FX.Win.UserControls.Button)c : null;

            // 선택 또는 첫번째 그룹웨어의 TOUCH 상품 표시
            GroupButton_Click(m_lastSelGroup, EventArgs.Empty);
        }

        public event WSWD.WmallPos.POS.SL.Data.TouchEventHandler OnTouch;

        #endregion
    }
}
