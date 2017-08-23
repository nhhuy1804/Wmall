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
    public partial class GroupItemList : UserControl, ISLM001TouchItem
    {
        public GroupItemList()
        {
            InitializeComponent();
            for (int i = 0; i < BUTTON_COUNT; i++)
            {
                string btnName = string.Format("button{0}", i + 1);
                var c = this.Controls[btnName];
                //c.Click += new EventHandler(c_Click);
                c.MouseUp += new MouseEventHandler(c_MouseUp);
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

        void c_MouseUp(object sender, MouseEventArgs e)
        {
            OnItemClick(sender);
        }

        void btnNext_Click(object sender, EventArgs e)
        {
            PageIndex++;
        }

        void btnPrev_Click(object sender, EventArgs e)
        {
            PageIndex--;
        }

        /// <summary>
        /// TOUCH ITEM CLICK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void c_Click(object sender, EventArgs e)
        {
            OnItemClick(sender);
        }

        void OnItemClick(object sender)
        {
            if (OnTouch != null)
            {
                var tag = ((Control)sender).Tag;
                if (tag == null)
                {
                    return;
                }
                OnTouch(new TouchEventArgs()
                {
                    Target = TouchTarget.Item,
                    ItemData = ((Control)sender).Tag
                });
            }
        }

        private TouchItemData[] m_items = null;
        private const int BUTTON_COUNT = 18;
        private int m_pageCount = 0;
        int m_pageIndex = -1;
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

                if (pi < 0)
                {
                    m_pageIndex = pi;
                    return;
                }

                if (m_pageIndex == pi)
                {
                    return;
                }

                m_pageIndex = pi;
                lblPageNo.Text = string.Format("{0}/{1}", m_pageIndex + 1, m_pageCount);

                List<TouchItemData> viewItems = new List<TouchItemData>();
                int f = m_pageIndex * BUTTON_COUNT;
                int t = (m_pageIndex + 1) * BUTTON_COUNT;
                viewItems = this.m_items.Where(p => p.SqSort >= f && p.SqSort < t).OrderBy(p => p.SqSort).ToList();

                ResetItemList();

                foreach (var itemData in viewItems)
                {
                    int idx = itemData.SqSort % BUTTON_COUNT;
                    string btnName = string.Format("button{0}", idx + 1);
                    Control btn = this.Controls[btnName];
                    if (btn != null)
                    {
                        string text = itemData.NmItem;
                        text += Environment.NewLine;

                        // 품번인경우
                        if (SLExtensions.CDDP_PB.Equals(itemData.CdDp))
                        {
                            text += itemData.CdItem;
                            PBItemData ipd = new PBItemData()
                            {
                                Barcode = itemData.CdItem,
                                ParseType = PBItemParseType.FullCode,
                                CdDp = itemData.CdDp
                            };

                            if (ipd.Parse(itemData.CdItem, ItemInputOperation.Preset) == PBItemParseResult.Success)
                            {
                                if (TouchItemData.InRange(ipd.FgClass, TouchItemData.PB_NM_RNG_FR, TouchItemData.PB_NM_RNG_TO))
                                {
                                    text += WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00395");
                                }
                                else if (TouchItemData.InRange(ipd.FgClass, TouchItemData.PB_EV_RNG_FR, TouchItemData.PB_EV_RNG_TO))
                                {
                                    text += WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00404");
                                }

                                //2015.09.08  정광호 추가----------------------------------------
                                //***************************************************************
                                else if (TouchItemData.InRange(ipd.FgClass, TouchItemData.PB_SP_RNG_FR, TouchItemData.PB_SP_RNG_TO))
                                {
                                    text += "(특)";
                                }
                                else if (TouchItemData.InRange(ipd.FgClass, TouchItemData.PB_EQ_RNG_FR, TouchItemData.PB_EQ_RNG_TO))
                                {
                                    text += "(균)";
                                }
                                else if (TouchItemData.InRange(ipd.FgClass, TouchItemData.PB_ON_RNG_FR, TouchItemData.PB_ON_RNG_TO))
                                {
                                    text += "(온)";
                                }
                                else if (TouchItemData.InRange(ipd.FgClass, TouchItemData.PB_NO_RNG_FR, TouchItemData.PB_NO_RNG_TO))
                                {
                                    text += "(노)";
                                }
                                else if (TouchItemData.InRange(ipd.FgClass, TouchItemData.PB_TI_RNG_FR, TouchItemData.PB_TI_RNG_TO))
                                {
                                    text += "(티)";
                                }
                                //***************************************************************
                                //---------------------------------------------------------------
                                
                            }

                        }
                        else
                        {
                            text += string.Format("{0:#,##0}", itemData.UtSprc);
                        }

                        btn.Text = text;
                        btn.Tag = itemData;
                    }
                }
            }
        }

        private void ResetItemList()
        {
            // reset empty
            for (int i = 0; i < BUTTON_COUNT; i++)
            {
                string btnName = string.Format("button{0}", i + 1);
                Control btn = this.Controls[btnName];
                btn.Text = string.Empty;
                btn.Tag = null;
            }
        }

        #region IM001TouchItem Members

        /// <summary>
        /// SqSort통해 위치 Binding
        /// Paging 처리
        /// 1) Max SqSort => PageCount
        /// 2) PageIndex
        /// 3) 
        /// </summary>
        /// <param name="touchItems"></param>
        public void BindItems(WSWD.WmallPos.POS.SL.Data.TouchItemData[] touchItems)
        {
            ResetItemList();
            PageIndex = -1;

            m_items = touchItems;
            int maxIndex = touchItems.Length > 0 ? touchItems.Max(p => p.SqSort) : 0;
            maxIndex++;
            m_pageCount = (maxIndex / BUTTON_COUNT )+ (maxIndex % BUTTON_COUNT == 0 ? 0 : 1);
            PageIndex = 0;
        }

        public event WSWD.WmallPos.POS.SL.Data.TouchEventHandler OnTouch;

        #endregion
    }
}
