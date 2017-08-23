using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Controls;
using WSWD.WmallPos.FX.Shared.Data;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Data;
using System.Runtime.InteropServices;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public partial class KeyPadBase : BaseUserControl
    {
        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        const uint WM_KEYDOWN = 0x0100;

        public KeyPadBase()
        {
            InitializeComponent();
            this.Load += new EventHandler(KeyPadBase_Load);
            this.Disposed += new EventHandler(KeyPadBase_Disposed);
        }

        void KeyPadBase_Disposed(object sender, EventArgs e)
        {
            this.Load -= new EventHandler(KeyPadBase_Load);
            this.Disposed -= new EventHandler(KeyPadBase_Disposed);
        }

        void KeyPadBase_Load(object sender, EventArgs e)
        {
            foreach (var item in this.Controls)
            {
                ImageButton btn = (ImageButton)item;
                btn.MouseUp += new MouseEventHandler(btn_MouseUp);
            }
        }

        void btn_MouseUp(object sender, MouseEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            string keyName = btn.Name.Substring("ibtnKey".Length);            
            ProcessKeyEvent(keyName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyName"></param>
        void ProcessKeyEvent(string keyName)
        {
            var mapKey = (OPOSMapKeys)Enum.Parse(typeof(OPOSMapKeys), keyName);
            KeyMap keyMap = ConfigData.Current.KeyMapConfig.KeyMapByKeyName(mapKey.ToString());

            var ev = new OPOSKeyEventArgs()
            {
                Key = keyMap,
                KeyCode = keyMap.KeyCode
            };

            this.KeyListener.ProcessOneKeyEvent(ev);            
        }
    }
}
