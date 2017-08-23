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

namespace WSWD.WmallPos.POS.SL.VC
{
    public partial class FuncKeyGroup : UserControl
    {
        public FuncKeyGroup()
        {
            InitializeComponent();
            this.Load += new EventHandler(FuncKeyGroup_Load);
        }

        void FuncKeyGroup_Load(object sender, EventArgs e)
        {
            foreach (var item in this.Controls)
            {
                FuncButton btn = (FuncButton)item;
                int idx = int.Parse(btn.Name.Substring("button".Length));

                btn.VisibleIndex = idx;
                btn.SubtotalOn = false;
                btn.AttachEvent();
                btn.KeyEvent += new OPOSKeyEventHandler(btn_KeyEvent);
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
            foreach (var item in this.Controls)
            {
                FuncButton btn = (FuncButton)item;
                btn.SubtotalOn = subtotalOn;
            }
        }

        /// <summary>
        /// Handle key event, click event
        /// </summary>
        /// <param name="e"></param>
        void btn_KeyEvent(OPOSKeyEventArgs e)
        {
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_SUBTOTAL)
            {
                // switch key to after mode
                ResetState(true);
            }
        }
    }
}
