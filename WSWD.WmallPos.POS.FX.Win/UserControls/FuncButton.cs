using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

using WSWD.WmallPos.FX.Shared.Data;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Shared;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public class FuncButton : Button, IKeyInputView
    {
        private int m_visibleIndex = 0;
        /// <summary>
        /// visible index in sale UI
        /// </summary>
        public int VisibleIndex
        {
            set
            {
                m_visibleIndex = value;
            }
        }

        /// <summary>
        /// 소계키 상태아닌지?
        /// </summary>
        public bool SubtotalOn
        {
            set
            {
                SetVisibleIndex(value, m_visibleIndex);
            }
        }

        /// <summary>
        /// Mapped key data
        /// </summary>
        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public KeyMap MappedKey { get; set; }

        public FuncButton()
        {
            //this.Click += new EventHandler(FuncButton_Click);
            this.Disposed += new EventHandler(FuncButton_Disposed);
        }

        public void AttachKeyEvent()
        {
            this.AttachKeyInput();
        }

        void FuncButton_Disposed(object sender, EventArgs e)
        {
            this.DetachKeyInput();
        }

        /// <summary>
        /// Perform key event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FuncButton_Click(object sender, EventArgs e)
        {
            if (this.MappedKey == null)
            {
                return;
            }
            PerformKeyEvent(OPOSKeyEventArgs.FromFuncCode(this.MappedKey.CdFunc));
        }

        void SetVisibleIndex(bool subTotalOn, int index)
        {
            if (ConfigData.Current == null || ConfigData.Current.KeyMapConfig == null)
            {
                return;
            }

            // set visible index
            KeyMap key;

            if (subTotalOn)
            {
                key = ConfigData.Current.KeyMapConfig.KeyMaps.FirstOrDefault(p => p.FgDisp == "A"
                 && p.PoDisp == index.ToString());
            }
            else
            {
                key = ConfigData.Current.KeyMapConfig.KeyMaps.FirstOrDefault(p => p.FgDisp == "B"
                 && p.PoDisp == index.ToString());    
            }
            
            this.MappedKey = key;
            this.Text = key != null ? key.NmDisp : string.Empty;
        }

        #region IKeyInputView Members

        public event OPOSKeyEventHandler KeyEvent;

        public void PerformKeyEvent(OPOSKeyEventArgs e)
        {
            if (this.MappedKey == null)
            {
                return;
            }

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
}
