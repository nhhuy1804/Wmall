using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;

using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.Nini.Ini;

using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Controls;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Win.Data;
using System.Runtime.InteropServices;

/* ============================================
 * 개발자: TCL
 * 날짜  : 2014.03.22
 * 설명  : Windows 용 Extensions 함수 (Helper)  
 *          
 * 
 * 
 * 
 * ============================================
 */
namespace WSWD.WmallPos.POS.FX.Win
{
    public static class WinExtensions
    {
        static WinExtensions()
        {
            roundedButtonStyles = new Dictionary<ButtonTypes, RoundedButtonStateStyle>();
            controlProperties = new Dictionary<string, Dictionary<string, string>>();

            #region Load button styles

            string configFile = Path.Combine(FXConsts.FOLDER_RESOURCE.GetFolder(), FXConsts.RESOURCE_FILE_UI_STYLES);
            if (!File.Exists(configFile))
            {
                return;
            }

            IniDocument doc = new IniDocument(configFile);

            for (int i = 1; i < 5; i++)
            {
                ButtonTypes btp = (ButtonTypes)Enum.Parse(typeof(ButtonTypes), string.Format("Type{0:d2}", i));
                var style = new RoundedButtonStateStyle();
                var props = style.GetType().GetProperties();

                foreach (var prop in props)
                {
                    if (!prop.Name.Contains("Color"))
                    {
                        continue;
                    }


                    var section = doc.Sections[btp.ToString().ToUpper()];
                    var val = section.GetValue(prop.Name);
                    Color col = val.FromHtmlColor();
                    prop.SetValue(style, col, null);
                }

                roundedButtonStyles.Add(btp, style);
            }

            #endregion

            #region Control properties

            foreach (DictionaryEntry entry in doc.Sections)
            {
                var section = doc.Sections[entry.Key.ToString()];
                Dictionary<string, string> props = new Dictionary<string, string>();
                string[] keys = section.GetKeys();
                foreach (var key in keys)
                {
                    props.Add(key, section.GetValue(key));
                }

                controlProperties.Add(entry.Key.ToString(), props);
            }

            #endregion

        }

        #region Graphics

        public static void DrawGradientBack(this Graphics g, Color color, Rectangle rec)
        {
            Color cl1 = Color.FromArgb(225, 236, 248);
            Color cl2 = Color.FromArgb(210, 223, 238);
            LinearGradientBrush linGrBrush2 = new LinearGradientBrush(rec, cl1, cl2, LinearGradientMode.Vertical);

            g.FillRectangle(linGrBrush2, 0, 10, rec.Width, rec.Height);
        }

        public static void DrawRoundedBorder(this Graphics g, Color color, Rectangle rec,
                                         int radius, int borderWidth)
        {
            using (Bitmap b = new Bitmap(rec.Width, rec.Height))
            {
                using (Graphics gb = Graphics.FromImage(b))
                {
                    var gfRec = new Rectangle(0, 0, rec.Width, rec.Height);
                    gb.Clear(Color.Green);

                    gb.DrawRoundedRectangle(color, gfRec, radius);

                    gfRec.Height -= borderWidth << 1;
                    gfRec.Width -= borderWidth << 1;
                    gfRec.X += borderWidth;
                    gfRec.Y += borderWidth;
                    gb.DrawRoundedRectangle(Color.Green, gfRec, radius - borderWidth);

                    var maskAttr = new ImageAttributes();
                    maskAttr.SetColorKey(Color.Green, Color.Green);

                    g.DrawImage(b, rec, 0, 0, b.Width, b.Height, GraphicsUnit.Pixel, maskAttr);
                }
            }
        }

        public static void DrawRoundedRectangle(this Graphics g, Color color, Rectangle rec, int radius)
        {
            using (var b = new SolidBrush(color))
            {
                int x = rec.X;
                int y = rec.Y;
                int diameter = radius * 2;
                var horiz = new Rectangle(x, y + radius, rec.Width, rec.Height - diameter);
                var vert = new Rectangle(x + radius, y, rec.Width - diameter, rec.Height);

                g.FillRectangle(b, horiz);
                g.FillRectangle(b, vert);
                g.FillRectangle(b, x + rec.Width - (diameter + 1), y + rec.Height - (diameter + 1), diameter,
                                diameter);
            }
        }

        #endregion

        #region Controls Exentions

        static Dictionary<ButtonTypes, RoundedButtonStateStyle> roundedButtonStyles;
        static Dictionary<string, Dictionary<string, string>> controlProperties;

        /// <summary>
        /// Get style by button type
        /// </summary>
        /// <param name="buttonType"></param>
        /// <returns></returns>
        static public RoundedButtonStateStyle GetButtonStateStyleByType(ButtonTypes buttonType)
        {
            if (roundedButtonStyles != null && roundedButtonStyles.ContainsKey(buttonType))
            {
                return roundedButtonStyles[buttonType];
            }
            else
            {
                return new RoundedButtonStateStyle();
            }
        }

        static public Color ColorProp(this Control control, string propName)
        {
            return control.ColorProp(propName, Color.Transparent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static public Color ColorProp(this Control control, string propName, Color defaultValue)
        {
            DateTimeUtils.PrintTime("ColorPropS");

            string typeName = control.GetType().Name;
            if (controlProperties.ContainsKey(typeName))
            {
                var props = controlProperties[typeName];
                if (props != null)
                {
                    if (props.ContainsKey(propName))
                    {
                        DateTimeUtils.PrintTime("ColorPropE");
                        return props[propName].FromHtmlColor();
                    }
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Get property Color value from ini file
        /// </summary>
        /// <param name="control"></param>
        /// <param name="typeName"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        static public Color ColorProp(this Control control, string typeName, string propName)
        {
            return ColorProp(control, typeName, propName, Color.Transparent);
        }

        /// <summary>
        /// Get property color value from ini
        /// </summary>
        /// <param name="control"></param>
        /// <param name="typeName"></param>
        /// <param name="propName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        static public Color ColorProp(this Control control, string typeName, string propName, Color defaultValue)
        {
            if (controlProperties.ContainsKey(typeName))
            {
                var props = controlProperties[typeName];
                if (props != null)
                {
                    if (props.ContainsKey(propName))
                    {
                        return props[propName].FromHtmlColor();
                    }
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Get property int value from ini
        /// </summary>
        /// <param name="control"></param>
        /// <param name="propName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        static public int IntProp(this Control control, string propName, int defaultValue)
        {
            string typeName = control.GetType().Name;
            if (controlProperties.ContainsKey(typeName))
            {
                var props = controlProperties[typeName];
                if (props != null)
                {
                    if (props.ContainsKey(propName))
                    {
                        return Convert.ToInt32(props[propName]);
                    }
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Get property int value from ini file
        /// </summary>
        /// <param name="control"></param>
        /// <param name="typeName"></param>
        /// <param name="propName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        static public int IntProp(this Control control, string typeName, string propName, int defaultValue)
        {
            if (controlProperties.ContainsKey(typeName))
            {
                var props = controlProperties[typeName];
                if (props != null)
                {
                    if (props.ContainsKey(propName))
                    {
                        return Convert.ToInt32(props[propName]);
                    }
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Get property as text from UIStyles.ini
        /// </summary>
        /// <param name="ctrol"></param>
        /// <param name="typeName"></param>
        /// <param name="propName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        static public string StringProp(this Control ctrol, string typeName, string propName, string defaultValue)
        {
            if (controlProperties.ContainsKey(typeName))
            {
                var props = controlProperties[typeName];
                if (props != null)
                {
                    if (props.ContainsKey(propName))
                    {
                        return Convert.ToString(props[propName]);
                    }
                }
            }

            return defaultValue;
        }

        static private Form GetParentForm(Control parent)
        {
            Form form = parent as Form;
            if (form != null)
            {
                return form;
            }
            if (parent != null)
            {
                // Walk up the control hierarchy
                return GetParentForm(parent.Parent);
            }
            return null; // Control is not on a Form
        }

        static public void AttachKeyInput(this Control ctrl)
        {
            var form = ctrl.FindForm();
            if (form != null && form is KeyInputForm)
            {
                ((KeyInputForm)form).Register((IKeyInputView)ctrl);
            }
        }

        static public void DetachKeyInput(this Control ctrl)
        {
            var form = ctrl.FindForm();
            if (form == null)
            {
                form = Form.ActiveForm;
            }

            if (form != null && form.GetType().IsSubclassOf(typeof(KeyInputForm)))
            {
                ((KeyInputForm)form).Unregister((IKeyInputView)ctrl);
            }
        }

        /// <summary>
        /// Find all childs by typeName
        /// </summary>
        /// <param name="control"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static public IEnumerable<Control> FindAllByType(this Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => ctrl.FindAllByType(type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        /// <summary>
        /// Find all childs by typeName
        /// </summary>
        /// <param name="control"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        static public IEnumerable<Control> FindAllByType(this Control control, string typeName)
        {
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => ctrl.FindAllByType(typeName))
                                      .Concat(controls)
                                      .Where(c => c.GetType().Name == typeName);
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;

        public static void SuspendDrawing(this Control parent)
        {
            if (parent.Disposing || parent.IsDisposed)
            {
                return;
            }
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeDrawing(this Control parent)
        {
            if (parent.Disposing || parent.IsDisposed)
            {
                return;
            } 
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }

        #endregion

        #region Color conversion

        public static string ToHtmlColor(this Color color)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        }

        static Regex htmlColorRegex = new Regex(
            @"^#((?'R'[0-9a-f]{2})(?'G'[0-9a-f]{2})(?'B'[0-9a-f]{2}))"
            + @"|((?'R'[0-9a-f])(?'G'[0-9a-f])(?'B'[0-9a-f]))$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static Color FromHtmlColor(this string colorString)
        {
            if (colorString == null)
            {
                throw new ArgumentNullException("colorString");
            }

            var match = htmlColorRegex.Match(colorString);
            if (!match.Success)
            {
                var msg = "The string \"{0}\" doesn't represent";
                msg += "a valid HTML hexadecimal color";
                msg = string.Format(msg, colorString);

                throw new ArgumentException(msg,
                    "colorString");
            }

            return Color.FromArgb(
                ColorComponentToValue(match.Groups["R"].Value),
                ColorComponentToValue(match.Groups["G"].Value),
                ColorComponentToValue(match.Groups["B"].Value));
        }

        static int ColorComponentToValue(string component)
        {
            if (component.Length == 1)
            {
                component += component;
            }

            return int.Parse(component,
                System.Globalization.NumberStyles.HexNumber);
        }

        #endregion
    }
}
