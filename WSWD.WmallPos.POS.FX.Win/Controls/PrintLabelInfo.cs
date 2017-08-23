using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace WSWD.WmallPos.POS.FX.Win.Controls
{
    /// <summary>
    /// 자동반품용
    /// 거래정보표시
    /// 
    /// 개발자     : TCL
    /// 개발일자   : 06.02
    /// 변경일자   : 10.29
    /// 
    /// </summary>
    public partial class PrintLabelInfo : UserControl
    {
        private bool m_canUp = false;
        private bool CanUp
        {
            get
            {
                return m_canUp;
            }
            set
            {
                m_canUp = value;
                btnUp.IsHighlight = value;
            }
        }

        private bool m_canDown = false;
        private bool CanDown
        {
            get
            {
                return m_canDown;
            }
            set
            {
                m_canDown = value;
                btnDown.IsHighlight = value;
            }
        }

        public PrintLabelInfo()
        {
            InitializeComponent();

            lblText.Top = 0;
            lblText.Width = this.pnlContent.Width;

            //this.lblText.Font = new Font("돋움", 11, FontStyle.Regular);
            this.btnUp.MouseUp += new MouseEventHandler(btnUp_MouseUp);
            this.btnUp.MouseDown += new MouseEventHandler(btnUp_MouseDown);

            this.btnDown.MouseUp += new MouseEventHandler(btnUp_MouseUp);
            this.btnDown.MouseDown += new MouseEventHandler(btnUp_MouseDown);
        }

        private int m_textLinesCount = 0;
        private int m_lineOffset = 0;
        private int m_visibleLinesCount = 0;
        private string[] m_textLines = null;

        /// <summary>
        /// 공지사항
        /// 1. Calculate total lines, how many lines can show in screen
        /// 2. Scroll: 
        /// </summary>
        /// <param name="textData"></param>
        public void BindNoticeInfo(string textData)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    CalcLines(textData);
                });
            }
            else
            {
                CalcLines(textData);
            }
        }

        /// <summary>
        /// Calculate
        /// </summary>
        /// <param name="textData"></param>
        private void CalcLines(string textData)
        {
            int clientHeight = this.pnlContent.Height;

            lblText.Visible = false;
            lblText.AutoSize = true;
            m_visibleLinesCount = 0;
            m_lineOffset = 0;
            
            m_textLines = textData.Split(new char[] { '\n' }, StringSplitOptions.None);
            m_textLinesCount = m_textLines.Length;
            string lastText = string.Empty;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < m_textLines.Length; i++)
            {
                lastText = sb.ToString();
                sb.Append(m_textLines[i] + '\n');
                lblText.Text = sb.ToString();

                if (lblText.Height > clientHeight)
                {
                    // get the previous
                    m_visibleLinesCount = i;
                    m_lineOffset = 0;
                    break;
                }
            }

            if (m_visibleLinesCount == 0)
            {
                m_visibleLinesCount = m_textLinesCount;
                m_lineOffset = 0;
            }

            lblText.AutoSize = false;
            lblText.Width = this.pnlContent.Width;
            lblText.Height = this.pnlContent.Height;
            lblText.Visible = true;
            
            SetVisibleText();
            CanUp = false;
            CanDown = m_lineOffset + m_visibleLinesCount < m_textLinesCount;
        }

        /// <summary>
        /// Set current page text to view
        /// </summary>
        void SetVisibleText()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = m_lineOffset; i < m_lineOffset + m_visibleLinesCount; i++)
            {
                if (i > m_textLinesCount - 1)
                {
                    break;
                }

                sb.Append(m_textLines[i]);
                sb.Append('\n');
            }

            lblText.Text = sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isUp"></param>
        void DoScroll(bool isUp)
        {
            if (m_textLinesCount <= m_visibleLinesCount)
            {
                CanUp = false;
                CanDown = false;
                return;
            }

            if (isUp)
            {
                if (m_lineOffset <= 0)
                {
                    return;
                }

                m_lineOffset -= m_visibleLinesCount;
                m_lineOffset = Math.Max(m_lineOffset, 0);
                SetVisibleText();
            }
            else
            {
                if (m_visibleLinesCount + m_lineOffset >= m_textLinesCount)
                {
                    return;
                }

                m_lineOffset += m_visibleLinesCount;
                m_lineOffset = Math.Min(m_lineOffset, m_textLinesCount - 1);
                SetVisibleText();
            }

            CanUp = m_lineOffset > 0;
            CanDown = m_lineOffset + m_visibleLinesCount < m_textLinesCount;
        }

        public void Clear()
        {
            lblText.Text = string.Empty;
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
    }
}
