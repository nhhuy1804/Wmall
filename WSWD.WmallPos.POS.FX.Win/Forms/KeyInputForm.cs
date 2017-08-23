using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Shared;
using System.Diagnostics;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Data;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.FX.Shared.Utils;
using System.Runtime.InteropServices;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.FX.Shared.Data;

namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    /// <summary>
    /// 기본폼
    /// </summary>
    public partial class KeyInputForm : Form, IKeyInputView, IObserver<OPOSKeyEventArgs>
    {
        [DllImport("psapi")]
        static extern int EmptyWorkingSet(IntPtr handle);
        [DllImport("kernel32")]
        static extern bool SetProcessWorkingSetSize(IntPtr handle, int minSize, int maxSize);

        #region 변수

        /// <summary>
        /// Refresh trick
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6)
                {
                    cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                }
                return cp;
            }
        }

        #endregion

        #region 생성자

        public KeyInputForm()
        {
            InitializeComponent();

            this.ReturnResult = new Dictionary<string, object>();
            this.inputListeners = new List<IKeyInputView>();
            this.nonInputListeners = new List<IKeyInputView>();

            //this.KeyDown += new KeyEventHandler(KeyInputForm_KeyDown);
            //this.KeyEvent += new OPOSKeyEventHandler(KeyInputForm_KeyEvent);
            //this.Activated += new EventHandler(KeyInputForm_Activated);
            //this.Deactivate += new EventHandler(KeyInputForm_Deactivate);
            //this.FormClosed += new FormClosedEventHandler(KeyInputForm_FormClosed);
            this.Load += new EventHandler(KeyInputForm_Load);
            this.Disposed += new EventHandler(KeyInputForm_Disposed);
        }

        void KeyInputForm_Disposed(object sender, EventArgs e)
        {
            this.Disposed -= new EventHandler(KeyInputForm_Disposed);
            this.Load -= new EventHandler(KeyInputForm_Load);            
        }

        void KeyInputForm_Load(object sender, EventArgs e)
        {
            this.KeyDown += new KeyEventHandler(KeyInputForm_KeyDown);
            this.KeyEvent += new OPOSKeyEventHandler(KeyInputForm_KeyEvent);
            this.Activated += new EventHandler(KeyInputForm_Activated);
            this.Deactivate += new EventHandler(KeyInputForm_Deactivate);
            this.FormClosed += new FormClosedEventHandler(KeyInputForm_FormClosed);
        }

        #endregion

        #region Form Events

        /// <summary>
        /// Clear handle not to receive key events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void KeyInputForm_Deactivate(object sender, EventArgs e)
        {
            PostMessageHandler.Current.Detach(this);
        }

        /// <summary>
        /// Set handle to receive key event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void KeyInputForm_Activated(object sender, EventArgs e)
        {
            KeyInputForm.ActiveHandle = this.Handle;
            PostMessageHandler.Current.Attach(this);
        }

        /// <summary>
        /// Clean disposed objects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void KeyInputForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PostMessageHandler.Current.Detach(this);

            this.KeyDown -= new KeyEventHandler(KeyInputForm_KeyDown);
            this.KeyEvent -= new OPOSKeyEventHandler(KeyInputForm_KeyEvent);
            this.Activated -= new EventHandler(KeyInputForm_Activated);
            this.Deactivate -= new EventHandler(KeyInputForm_Deactivate);
            this.FormClosed -= new FormClosedEventHandler(KeyInputForm_FormClosed);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            EmptyWorkingSet(Process.GetCurrentProcess().Handle);
            SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
        }

        /// <summary>
        /// Process key events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void KeyInputForm_KeyDown(object sender, KeyEventArgs e)
        {
            var ev = OPOSKeyEventArgs.FromKeyCode(e.KeyValue.IntToHex());
            ProcessOneKeyEvent(ev);
            e.Handled = ev.IsHandled;
        }

        #endregion

        #region Key Event 처리

        /// <summary>
        /// Search on list of Listener
        /// If is IFocusableControl,
        /// </summary>
        /// <param name="e"></param>
        void KeyInputForm_KeyEvent(OPOSKeyEventArgs e)
        {
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_UP || e.Key.OPOSKey == OPOSMapKeys.KEY_DOWN)
            {
                if (e.Key.OPOSKey == OPOSMapKeys.KEY_UP)
                {
                    PreviousControl();
                }
                else
                {
                    NextControl();
                }
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR || e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
            {
                // find button with key type
                var buttons = this.FindAllByType(typeof(WSWD.WmallPos.POS.FX.Win.UserControls.Button));
                foreach (var btn in buttons)
                {
                    WSWD.WmallPos.POS.FX.Win.UserControls.Button b = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)btn;
                    if ((b.KeyType == WSWD.WmallPos.POS.FX.Win.UserControls.KeyButtonTypes.Clear
                        || b.KeyType == WSWD.WmallPos.POS.FX.Win.UserControls.KeyButtonTypes.EnterOrClear)
                        && e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
                    {
                        e.IsHandled = true;
                        b.FireClick();
                    }
                    else if ((b.KeyType == WSWD.WmallPos.POS.FX.Win.UserControls.KeyButtonTypes.Enter
                        || b.KeyType == WSWD.WmallPos.POS.FX.Win.UserControls.KeyButtonTypes.EnterOrClear)
                        && e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
                    {
                        e.IsHandled = true;
                        b.FireClick();
                    }
                }
            }
        }

        #endregion

        #region IKeyInputView Members

        public event OPOSKeyEventHandler KeyEvent;

        public void PerformKeyEvent(OPOSKeyEventArgs e)
        {
            if (KeyEvent != null && e.Key != null)
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

        #region 속성 - Properties

        /// <summary>
        /// 폰트고정
        /// </summary>
        public override Font Font
        {
            get
            {
                return new Font("돋움", 11, FontStyle.Bold);
            }
        }

        /// <summary>
        /// Return result data
        /// </summary>
        public Dictionary<string, object> ReturnResult { get; private set; }

        #endregion

        #region MessageDialog

        /// <summary>
        /// Show MessageBox 메시지박스
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public DialogResult ShowMessageBox(MessageDialogType messageType, string messageCode, string strMsg)
        {
            return new MessageDialog(messageType, null, messageCode, strMsg).ShowDialog(this);
            //return new MessageBoxDialog(messageType, messageCode, strMsg).ShowDialog(this);
        }

        /// <summary>
        /// Show MessageBox 메시지박스
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="messageCode"></param>
        /// <param name="strMsg"></param>
        /// <param name="buttonLabels">표시 되는 버튼만큼 버튼글자</param>
        /// <returns></returns>
        public DialogResult ShowMessageBox(MessageDialogType messageType, string messageCode, string strMsg, string[] buttonLabels)
        {
            return new MessageDialog(messageType, null, messageCode, strMsg, buttonLabels).ShowDialog(this);
            //return new MessageBoxDialog(messageType, messageCode, strMsg, buttonLabels).ShowDialog(this);
        }

        #endregion

        #region Focused Control Methods

        private IFocusableControl m_focusedControl = null;

        /// <summary>
        /// Set current focused control in form
        /// </summary>
        public IFocusableControl FocusedControl
        {
            get
            {
                return m_focusedControl;
            }
            set
            {
                if (!value.Focusable)
                {
                    return;
                }

                if (m_focusedControl != null)
                {
                    m_focusedControl.IsFocused = false;
                }

                m_focusedControl = value;
                m_focusedControl.IsFocused = true;
            }
        }

        /// <summary>
        /// Set focused to next control
        /// </summary>
        public void NextControl()
        {
            ControlNavigate(true);
        }

        /// <summary>
        /// Set focused to previous control
        /// </summary>
        public void PreviousControl()
        {
            ControlNavigate(false);
        }

        /// <summary>
        /// navigate back and ford
        /// </summary>
        /// <param name="forward"></param>
        void ControlNavigate(bool forward)
        {
            if (inputListeners.Count == 0)
            {
                return;
            }

            IFocusableControl toFocus = null;

            List<IFocusableControl> focusableControls = new List<IFocusableControl>();
            foreach (var ctrl in inputListeners)
            {
                if (ctrl is IFocusableControl)
                {
                    var fc = (IFocusableControl)ctrl;
                    if (fc.Focusable && fc.Visible && fc.Enabled)
                    {
                        focusableControls.Add((IFocusableControl)ctrl);
                    }
                }
            }

            if (focusableControls.Count > 0)
            {
                // get current active control
                var control = m_focusedControl;

                // not found
                int focusedIndex = control == null ? -1 : control.TabIndex;
                if (focusedIndex == -1)
                {
                    // go to first control
                    toFocus = focusableControls.Count > 0 ? focusableControls[0] : null;
                }
                else
                {
                    // find next focusable control by focus index
                    IFocusableControl navControl = null;
                    if (forward)
                    {
                        navControl = focusableControls.Where(p => p.TabIndex >
                            focusedIndex).OrderBy(p => p.TabIndex).FirstOrDefault();
                    }
                    else
                    {
                        navControl = focusableControls.Where(p => p.TabIndex <
                            focusedIndex).OrderByDescending(p => p.TabIndex).FirstOrDefault();
                    }

                    if (navControl == null)
                    {
                        // get max focused index and min
                        int maxFocusedIndex = focusableControls.Max(p => p.TabIndex);
                        int minFocusedIndex = focusableControls.Min(p => p.TabIndex);

                        if (minFocusedIndex != maxFocusedIndex)
                        {
                            if (forward)
                            {
                                toFocus = focusableControls.FirstOrDefault(p => p.TabIndex == minFocusedIndex);
                            }
                            else
                            {
                                toFocus = focusableControls.FirstOrDefault(p => p.TabIndex == maxFocusedIndex);
                            }
                        }
                        else
                        {
                            // get next control in list of focuseable contrls
                            var idx = focusableControls.IndexOf(control);
                            if (idx == -1)
                            {
                                toFocus = focusableControls.Count > 0 ? focusableControls[0] : null;
                            }
                            else
                            {
                                if (forward)
                                {
                                    idx = idx == focusableControls.Count - 1 ? 0 : idx + 1;
                                }
                                else
                                {
                                    idx = idx == 0 ? focusableControls.Count - 1 : idx - 1;
                                }

                                toFocus = idx >= 0 && idx < focusableControls.Count ? focusableControls[idx] : null;
                            }
                        }
                    }
                    else
                    {
                        toFocus = navControl;
                    }
                }
            }

            if (toFocus != null)
            {
                this.FocusedControl = toFocus;
            }
        }

        #endregion

        #region KeyInputView Management

        /// <summary>
        /// Current active handl
        /// </summary>
        public static IntPtr ActiveHandle = IntPtr.Zero;
        public static IntPtr ProgressHandle = IntPtr.Zero;

        List<IKeyInputView> inputListeners;
        List<IKeyInputView> nonInputListeners;

        /// <summary>
        /// Register to receive KeyEvent event
        /// </summary>
        /// <param name="inputView"></param>
        public void Register(IKeyInputView inputView)
        {
            if (inputView.GetType().Name.Equals("InputText")
                || inputView.GetType().IsSubclassOf(typeof(InputText)))
            {
                if (!inputListeners.Contains(inputView))
                {
                    inputListeners.Add(inputView);
                }
            }
            else
            {
                if (!nonInputListeners.Contains(inputView))
                {
                    nonInputListeners.Add(inputView);
                }
            }
        }

        /// <summary>
        /// Unregister to stop listening to KeyEvent
        /// </summary>
        /// <param name="inputView"></param>
        public void Unregister(IKeyInputView inputView)
        {
            if (inputListeners.Contains(inputView))
            {
                inputListeners.Remove(inputView);
            }

            if (nonInputListeners.Contains(inputView))
            {
                nonInputListeners.Remove(inputView);
            }
        }

        /// <summary>
        /// Process one key event
        /// </summary>
        /// <param name="e"></param>
        public void ProcessOneKeyEvent(OPOSKeyEventArgs e)
        {
            List<IKeyInputView> ls = nonInputListeners.Where(p => p.Enabled && p.Visible && !p.IsDisposed).ToList();
            bool isHandled = false;
            int i = 0;
            try
            {
                while (i < ls.Count)
                {
                    if (!ls[i].IsHandleCreated)
                    {
                        i++;
                        continue;
                    }

                    ls[i].PerformKeyEvent(e);

                    if (e.IsHandled)
                    {
                        isHandled = e.IsHandled;
                        break;
                    }

                    i++;
                }

                if (!isHandled)
                {
                    // run key event on this form
                    this.PerformKeyEvent(e);
                    isHandled = e.IsHandled;
                }

                // perform key event on InputText
                if (!isHandled)
                {
                    ls = inputListeners.Where(p => p.Enabled &&
                        p.Visible && !p.IsDisposed).ToList();

                    i = 0;
                    while (i < ls.Count)
                    {
                        if (!ls[i].IsHandleCreated)
                        {
                            i++;
                            continue;
                        }

                        ls[i].PerformKeyEvent(e);

                        if (e.IsHandled)
                        {
                            isHandled = e.IsHandled;
                            break;
                        }

                        i++;
                    }
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine("ProcessOneEvent Error: " + ex.Message, "keyboard");
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
            }
        }

        #endregion

        #region IObserver<OPOSKeyEventArgs> Members

        public void Update(object sender, OPOSKeyEventArgs e)
        {
            ProcessOneKeyEvent(e);
        }

        #endregion
    }
}
