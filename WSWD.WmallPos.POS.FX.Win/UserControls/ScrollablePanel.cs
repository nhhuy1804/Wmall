using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;


using WSWD.WmallPos.POS.FX.Win.Controls;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.FX.Shared;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    /// <summary>
    /// ScrollablePanel with up/down control
    /// </summary>
    [Designer(typeof(ScrollablePanelDesigner))]
    public class ScrollablePanel : BorderPanel, IKeyInputView
    {
        #region 생성자

        public ScrollablePanel()
        {
            InitializeComponent();
            this.KeyEvent += new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(ScrollablePanel_KeyEvent);
            this.Disposed += new EventHandler(ScrollablePanel_Disposed);
            this.AttachKeyInput();
        }
                
        #endregion

        #region 변수

        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton btnUp;
        private WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton btnDown;
        private System.Windows.Forms.Panel pnlUpDn;
        private System.Windows.Forms.Panel pnlContainer;

        private void InitializeComponent()
        {
            this.btnUp = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.btnDown = new WSWD.WmallPos.POS.FX.Win.Controls.RoundedButton();
            this.pnlUpDn = new System.Windows.Forms.Panel();
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
            this.btnDown.Padding = new System.Windows.Forms.Padding(1);
            this.btnDown.Selected = false;
            this.btnDown.Size = new System.Drawing.Size(33, 133);
            this.btnDown.TabIndex = 0;
            this.btnDown.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDown.Click += new System.EventHandler(this.UpDown_Click);
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
            this.pnlUpDn.SizeChanged += new System.EventHandler(this.pnlUpDn_SizeChanged);
            // 
            // pnlContainer
            // 
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(1, 1);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(422, 268);
            this.pnlContainer.TabIndex = 2;
            this.pnlContainer.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.pnlContainer_ControlAdded);
            // 
            // ScrollablePanel
            // 
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.pnlUpDn);
            this.Name = "ScrollablePanel";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(457, 270);
            this.Load += new System.EventHandler(this.ScrollablePanel_Load);
            this.pnlUpDn.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        #region 이벤트정의

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

        void ScrollablePanel_Load(object sender, EventArgs e)
        {
            DoResize();
        }
        
        void ScrollablePanel_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_UP || e.Key.OPOSKey == OPOSMapKeys.KEY_DOWN)
            {
                DoScroll(e.Key.OPOSKey == OPOSMapKeys.KEY_UP);
            }
        }

        void ScrollablePanel_Disposed(object sender, EventArgs e)
        {
            this.DetachKeyInput();
        }
        
        /// <summary>
        /// Set size and position
        /// Assume like docking top
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pnlContainer_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.Left = 0;
            e.Control.Width = this.pnlContainer.Width;
            if (pnlContainer.Controls.Count == 1)
            {
                e.Control.Top = ScrollPosition;
            }
            else
            {
                int top = pnlContainer.Controls[pnlContainer.Controls.Count - 2].Top;
                e.Control.Top = top + e.Control.Height;
            }
        }

        void UpDown_Click(object sender, EventArgs e)
        {
            RoundedButton bt = (RoundedButton)sender;
            if (bt.Tag == null || !(bool)bt.Tag)
            {
                bt.Tag = true;
            }
            else
            {
                bt.Tag = false;
            }

            DoScroll(bt.Name.Contains("Up"));            
        }

        void pnlUpDn_SizeChanged(object sender, EventArgs e)
        {
            btnUp.Height = this.pnlUpDn.Height / 2;
        }

        #endregion

        #region 사용자정의

        void DoScroll(bool isUp)
        {
            if (ScrollUpAvail && isUp)
            {
                ScrollPosition += ScrollSize;
                //pnlContainer.ScrollUp(ScrollSize);
            }
            else if (ScrollDownAvail && !isUp)
            {
                ScrollPosition += -1 * ScrollSize;
                //pnlContainer.ScrollDown(ScrollSize);
            }
        }

        void DoResize()
        {
            if (this.pnlContainer.Controls.Count == 0)
            {
                return;
            }
            this.pnlContainer.SuspendDrawing();
            this.pnlContainer.Controls[0].Top = 0;
            this.pnlContainer.Controls[0].Left = 0;
            this.pnlContainer.Controls[0].Width = this.pnlContainer.Width;
            for (int i = 1; i < this.pnlContainer.Controls.Count; i++)
            {
                this.pnlContainer.Controls[i].Left = 0;
                this.pnlContainer.Controls[i].Width = this.pnlContainer.Width;
                this.pnlContainer.Controls[i].Top = this.pnlContainer.Controls[i - 1].Top + this.pnlContainer.Controls[i].Height;
            }

            this.pnlContainer.ResumeDrawing();
        }

        #endregion

        #region 속성
                
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Panel ContainerZone
        {
            get
            {
                return this.pnlContainer;
            }
        }

        /// <summary>
        /// Up/Down scroll amount
        /// </summary>
        public int ScrollSize { get; set; }

        /// <summary>
        /// Current scroll offset
        /// </summary>
        private bool ScrollDownAvail
        {
            get
            {
                if (this.pnlContainer.Controls.Count == 0)
                {
                    return false;
                }

                return this.pnlContainer.Controls[this.pnlContainer.Controls.Count - 1].Top +
                                this.pnlContainer.Controls[this.pnlContainer.Controls.Count - 1].Height > 
                                this.pnlContainer.Height;
            }
        }

        private bool ScrollUpAvail
        {
            get
            {
                if (this.pnlContainer.Controls.Count == 0)
                {
                    return false;
                }

                return this.pnlContainer.Controls[0].Top < 0;
            }
        }
          
        private int m_scrollPosition = 0;
        public int ScrollPosition
        {
            get
            {
                return m_scrollPosition;
            }
            set
            {
                m_scrollPosition = value;
                if (this.pnlContainer.Controls.Count == 0)
                {
                    return;
                }

                this.SuspendDrawing();
                
                this.pnlContainer.Controls[0].Visible = false;
                this.pnlContainer.Controls[0].Top = value;
                this.pnlContainer.Controls[0].Left = 0;
                this.pnlContainer.Controls[0].Width = this.pnlContainer.Width;
                this.pnlContainer.Controls[0].Visible = true;

                for (int i = 1; i < this.pnlContainer.Controls.Count; i++)
                {
                    this.pnlContainer.Controls[i].Visible = false;
                    this.pnlContainer.Controls[i].Left = 0;
                    this.pnlContainer.Controls[i].Width = this.pnlContainer.Width;
                    this.pnlContainer.Controls[i].Top = this.pnlContainer.Controls[i - 1].Top + 
                        this.pnlContainer.Controls[i].Height;

                    this.pnlContainer.Controls[i].Visible = true;
                }

                this.ResumeDrawing();
            }
        }

        public void ScrollUp()
        {
            DoScroll(true);
        }

        public void ScrollDown()
        {
            DoScroll(false);
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
    }

    public static class PanelExtension
    {
        public static void ScrollDown(this Panel p, int pos)
        {
            //pos passed in should be positive
            using (Control c = new Control() { Parent = p, Height = 1, Top = p.ClientSize.Height + pos })
            {
                p.ScrollControlIntoView(c);
            }
        }
        public static void ScrollUp(this Panel p, int pos)
        {
            //pos passed in should be negative
            using (Control c = new Control() { Parent = p, Height = 1, Top = pos })
            {
                p.ScrollControlIntoView(c);
            }
        }
    }
}
