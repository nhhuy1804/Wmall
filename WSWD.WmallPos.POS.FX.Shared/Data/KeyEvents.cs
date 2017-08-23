using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Data;

namespace WSWD.WmallPos.FX.Shared
{
    public delegate void OPOSKeyEventHandler(OPOSKeyEventArgs e);
    public class OPOSKeyEventArgs : EventArgs
    {
        [DllImport("user32.dll")]
        static extern int MapVirtualKey(uint uCode, uint uMapType);

        public object Sender { get; set; }

        public int KeyCode { get; set; }
        public string KeyCodeText
        {
            get
            {
                if (this.Key != null)                    
                {
                    if (this.Key.OPOSKey == OPOSMapKeys.KEY_NUM00)
                    {
                        return "00";
                    }
                    else if (this.Key.OPOSKey == OPOSMapKeys.KEY_NUM000)
                    {
                        return "000";
                    }
                }

                return Convert.ToString((char)this.KeyCode);
            }
        }
        public KeyMap Key { get; set; }

        public bool IsControlKey
        {
            get
            {
                return !Key.OPOSKey.ToString().StartsWith("KEY_NUM");
                    //&& (Key.OPOSKey.ToString().Equals("KEY_MANWON") || Key.OPOSKey.ToString().Equals("KEY_CASHIC"));
            }
        }
        public bool IsHandled { get; set; }

        /// <summary>
        /// From OPOS keyboard
        /// </summary>
        /// <param name="scanCode">Hexacode</param>
        /// <returns></returns>
        static public OPOSKeyEventArgs FromKeyCode(string scanCode)
        {
            return new OPOSKeyEventArgs()
            {
                KeyCode = scanCode.HexToInt(),                
                Key = ConfigData.Current.KeyMapConfig.KeyMapByHexCode(scanCode)
            };
        }

        static public OPOSKeyEventArgs FromFuncCode(string funcCode)
        {
            return new OPOSKeyEventArgs()
            {
                Key = ConfigData.Current.KeyMapConfig.KeyMapByFuncCode(funcCode)
            };
        }
    }

}
