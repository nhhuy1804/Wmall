using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.FX.Shared;

namespace WSWD.WmallPos.POS.SL.Controls
{
    public partial class FuncKeyGroup : UserControl
    {
        public event OPOSKeyEventHandler KeyEvent;

        public FuncKeyGroup()
        {
            InitializeComponent();
            this.Load += new EventHandler(FuncKeyGroup_Load);                        
        }

        public override Color BackColor
        {
            get
            {
                return this.Parent == null ? base.BackColor : this.Parent.BackColor;
            }
        }

        void FuncKeyGroup_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();
            foreach (var item in this.Controls)
            {
                FuncButton btn = (FuncButton)item;
                int idx = int.Parse(btn.Name.Substring("button".Length));

                btn.VisibleIndex = idx;
                btn.SubtotalOn = false;
                btn.AttachKeyEvent();
                btn.Click += new EventHandler(btn_Click);
                btn.KeyEvent += new OPOSKeyEventHandler(btn_KeyEvent);
            }
            this.ResumeLayout();
        }

        void btn_Click(object sender, EventArgs e)
        {
            FuncButton fb = (FuncButton)sender;
            if (fb.MappedKey == null)
            {
                return;
            }

            var ev = OPOSKeyEventArgs.FromFuncCode(fb.MappedKey.CdFunc);
            if (ev != null && ev.Key.OPOSKey != OPOSMapKeys.INVALID)
            {
                btn_KeyEvent(ev);
            }
        }

        /// <summary>
        /// Reset to normal state, subtotal before,
        /// starting sales
        /// </summary>
        public void ResetState()
        {
            ResetState(false);
        }

        private void ResetState(bool subtotalOn)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    foreach (var item in this.Controls)
                    {
                        FuncButton btn = (FuncButton)item;
                        btn.SubtotalOn = subtotalOn;
                    }
                });
            }
            else
            {
                foreach (var item in this.Controls)
                {
                    FuncButton btn = (FuncButton)item;
                    btn.SubtotalOn = subtotalOn;
                }
            }
        }

        /// <summary>
        /// Handle key event, click event
        /// </summary>
        /// <param name="e"></param>
        void btn_KeyEvent(OPOSKeyEventArgs e)
        {
            if (KeyEvent != null)
            {
                KeyEvent(e);

                if (!e.IsHandled)
                {
                    if (e.Key.OPOSKey == OPOSMapKeys.KEY_SUBTOTAL)
                    {
                        // switch key to after mode
                        ResetState(true);
                        e.IsHandled = true;
                    }
                }
            }
        }
    }
}
