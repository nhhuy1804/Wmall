using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.SL.Controls
{
    /// <summary>
    /// 자동반품 PROGRESS
    /// 개발자     : TCL
    /// 개발일자   : 06.01
    /// </summary>
    public partial class AutoRtnProgress : UserControl
    {
        #region 변수, 속성
        
        private int maxMessageCount = 1;
        private int messageCount = 0;

        /// <summary>
        /// 메시지높이
        /// </summary>
        [DefaultValue(25)]
        public int MessageHeight { get; set; }

        #endregion

        #region 생성자
                
        public AutoRtnProgress()
        {
            InitializeComponent();

            this.MessageHeight = 25;

            #region 이벤트등록

            this.Load += new EventHandler(AutoRtnProgress_Load);

            #endregion
        }

        #endregion

        #region 이벤트정의

        void AutoRtnProgress_Load(object sender, EventArgs e)
        {
            #region Create labels

            maxMessageCount = (this.Height - this.Padding.Top - this.Padding.Bottom) / MessageHeight;
            for (int i = 0; i < maxMessageCount; i++)
            {
                Label lbl = new Label()
                {
                    AutoSize = false,
                    Width = this.Width - this.Padding.Left - this.Padding.Right,
                    Height = MessageHeight,
                    Left = this.Padding.Left,
                    Top = this.Padding.Top + i * MessageHeight,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font(this.Font.FontFamily, 11, FontStyle.Regular)
                };

                this.Controls.Add(lbl);
            }

            #endregion

        }
        
        #endregion

        #region 사용자정의 - 함수
                
        /// <summary>
        /// Clear all messages in control
        /// </summary>
        public void Clear()
        {
            foreach (Control c in this.Controls)
            {
                c.Text = string.Empty;
            }

            messageCount = 0;
        }

        /// <summary>
        /// Add new progress message
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(string message)
        {
            if (messageCount == maxMessageCount)
            {
                // scroll messages
                for (int i = 0; i < messageCount - 1; i++)
                {
                    this.Controls[i].Text = this.Controls[i + 1].Text;
                }
            }

            messageCount++;
            messageCount = Math.Min(maxMessageCount, messageCount);
            this.Controls[messageCount - 1].Text = message;
            
        }

        #endregion

    }
}
