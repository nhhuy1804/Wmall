using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KeyBoardTest
{
    public partial class KeyBoardForm : Form1
    {
        [DllImport("user32.dll")]        
        static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646270(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct INPUT
        {
            public uint Type;
            public MOUSEKEYBDHARDWAREINPUT Data;
        }

        /// <summary>
        /// http://social.msdn.microsoft.com/Forums/en/csharplanguage/thread/f0e82d6e-4999-4d22-b3d3-32b25f61fb2a
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        internal struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public HARDWAREINPUT Hardware;
            [FieldOffset(0)]
            public KEYBDINPUT Keyboard;
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct HARDWAREINPUT
        {
            public uint Msg;
            public ushort ParamL;
            public ushort ParamH;
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            public ushort Vk;
            public ushort Scan;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        /// <summary>
        /// http://social.msdn.microsoft.com/forums/en-US/netfxbcl/thread/2abc6be8-c593-4686-93d2-89785232dacd
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            public int X;
            public int Y;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        const uint WM_KEYDOWN = 0x0100;
        const uint WM_KEYUP = 0x0101;
        
        public KeyBoardForm()
        {
            InitializeComponent();

            this.axCtrlKeyboard1.ThrowKeyEvent = false;

            this.axCtrlKeyboard1.KeyboardEvent +=
                new AxKeyBoardHook.__CtrlKeyboard_KeyboardEventEventHandler(axCtrlKeyboard1_KeyboardEvent);
            this.axCtrlKeyboard1.ScannerEvent += new AxKeyBoardHook.__CtrlKeyboard_ScannerEventEventHandler(axCtrlKeyboard1_ScannerEvent);

            this.KeyDown += new KeyEventHandler(KeyBoardForm_KeyDown);
            this.Activated += new EventHandler(KeyBoardForm_Activated);
            this.KeyPress += new KeyPressEventHandler(KeyBoardForm_KeyPress);
            //this.textBox1.KeyDown += new KeyEventHandler(textBox1_KeyDown);
            this.textBox1.ReadOnly = true;
        }

        void KeyBoardForm_Activated(object sender, EventArgs e)
        {
            Form1.ActiveHandle = this.Handle;
        }

        void axCtrlKeyboard1_ScannerEvent(object sender, AxKeyBoardHook.__CtrlKeyboard_ScannerEventEvent e)
        {
            Debug.WriteLine("axCtrlKeyboard1_ScannerEvent");
            label1.Text = e.strData;
        }

        void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("textBox1_KeyDown");
        }

        bool isScannerEvent = false;
        long prevTime = 0;
        Int32 lastChar = Int32.MaxValue;

        void KeyBoardForm_KeyDown(object sender, KeyEventArgs e)
        {            
            Debug.WriteLine("KeyBoardForm_KeyDown " + e.KeyData.ToString());
            label1.Text += (char)e.KeyCode;
        }

        void KeyBoardForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        void axCtrlKeyboard1_KeyboardEvent(object sender, AxKeyBoardHook.__CtrlKeyboard_KeyboardEventEvent e)
        {
            //MessageBox.Show(e.strData);
            Debug.WriteLine("axCtrlKeyboard1_KeyboardEvent");

            //e.strData = "025";
            //Int64 k = Int64.Parse(e.strData, System.Globalization.NumberStyles.HexNumber);
            //PostMessage(Form1.ActiveHandle, WM_KEYDOWN, (IntPtr)k, IntPtr.Zero);

            // e.strData = "025";
            int k = int.Parse(e.strData, System.Globalization.NumberStyles.HexNumber);

            //SendKeys.Send(Convert.ToString((char)k));
            PostMessage(Form1.ActiveHandle, WM_KEYDOWN, new IntPtr(k), IntPtr.Zero);
            //SendKeyDown(k);
            //PostMessage(Form1.ActiveHandle, WM_KEYUP, (IntPtr)k, IntPtr.Zero);

            //PostMessage(Form1.ActiveHandle, WM_KEYUP, (IntPtr)k, IntPtr.Zero);

            //PostMessage(this.Handle, WM_KEYUP, (IntPtr)k, IntPtr.Zero);
            //SendMessage(this.Handle, WM_KEYDOWN, (IntPtr)k, IntPtr.Zero);
            //SendMessage(this.Handle, WM_KEYUP, (IntPtr)k, IntPtr.Zero);
            //textBox1.Text += e.strData;

            textBox1.Text += k.ToString();
            textBox1.Text += " ";

            //var ev = OPOSKeyEventArgs.FromKeyCode(e.strData);
        }

        PictureBox GetPicture(int idx)
        {
            return (PictureBox)this.Controls.Find("pictureBox" + idx.ToString(), true)[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new ControlTest().ShowDialog(this);
        }

        private void KeyBoardForm_Load(object sender, EventArgs e)
        {           
            //var val = (int)Math.Ceiling((double)(6 / 10) * 10);
            //MessageBox.Show(val.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Form1().ShowDialog(this);
        }

        public static void SendKeyDown(int keyCode)
        {
            INPUT input = new INPUT
            {
                Type = 1
            };
            input.Data.Keyboard = new KEYBDINPUT();
            input.Data.Keyboard.Vk = (ushort)keyCode;
            input.Data.Keyboard.Scan = 0;
            input.Data.Keyboard.Flags = 0;
            input.Data.Keyboard.Time = 0;
            input.Data.Keyboard.ExtraInfo = IntPtr.Zero;
            INPUT[] inputs = new INPUT[] { input };
            if (SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT))) == 0)
            {
                throw new Exception();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        ProgressForm progressForm = new ProgressForm();
        private void button3_Click(object sender, EventArgs e)
        {
            progressForm.Show(this);
        }

        const uint WM_LBUTTONDOWN = 0x0201;
        protected override void WndProc(ref Message m)
        {
            Debug.WriteLine(m.Msg.ToString());
            if (m.Msg == WM_LBUTTONDOWN && progressForm.Visible)
            {
                m.Msg = 0;
            }

            base.WndProc(ref m);
        }
    }
}

