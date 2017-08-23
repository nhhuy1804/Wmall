using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Controls;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.POS.FX.Win.Devices;
using System.Diagnostics;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using System.Runtime.InteropServices;

namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    public partial class FormBase : BaseUserControl, IChildFormView, IKeyInputView
    {
        [DllImport("psapi")]
        static extern int EmptyWorkingSet(IntPtr handle);
        [DllImport("kernel32")]
        static extern bool SetProcessWorkingSetSize(IntPtr handle, int minSize, int maxSize);

        #region Properties

        public event EventHandler Activated;
        public event EventHandler Deactivated;
                
        public IChildFormManager ChildManager { get; set; }

        [Category("Operation"), DefaultValue(true),
        Description("기본적으로 닫기 버튼있음으로 모달폼임.")]
        public bool IsModal { get; set; }

        /// <summary>
        /// Text
        /// </summary>
        [Category("Appearance"),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text { get; set; }

        /// <summary>
        /// Unload event
        /// </summary>
        public event EventHandler Unload;

        private DialogResult m_dialogResult = DialogResult.None;
        public DialogResult DialogResult
        {
            get
            {
                return m_dialogResult;
            }
            set
            {
                m_dialogResult = value;
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        this.Close();
                    });
                }
                else
                {
                    this.Close();
                }
            }
        }

        /// <summary>
        /// Return result data
        /// </summary>
        public Dictionary<string, object> ReturnResult { get; private set; }

        /// <summary>
        /// Force to be white
        /// </summary>
        public override Color BackColor
        {
            get
            {
                return SystemColors.Control;
            }
        }

        /// <summary>
        /// 기본폰트
        /// </summary>
        public override Font Font
        {
            get
            {
                return new Font("돋움", 11, FontStyle.Bold);
            }
        }

        /// <summary>
        /// 키보드MSR활성화
        /// </summary>
        public bool UseMsr { get; set; }

        /// <summary>
        /// 스캔활성화
        /// </summary>
        public bool UseScanner { get; set; }

        public bool HideMainMenu { get; set; }

        /// <summary>
        /// 화면사이즈
        /// </summary>
        public Size ChildSize
        {
            get
            {
                return HideMainMenu ? new Size(1024, 692)
                    : new Size(736, 692);                    
            }
        }

        #endregion

        #region MessageDialog

        /// <summary>
        /// Show MessageBox 메시지박스
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public DialogResult     ShowMessageBox(MessageDialogType messageType, string messageCode, string strMsg)
        {
            Form f = (Form)ChildManager;
            if (f == null || f.IsDisposed || f.Disposing)
            {
                return new MessageDialog(messageType, null, messageCode, strMsg).ShowDialog();
            }

            return new MessageDialog(messageType, null, messageCode, strMsg).ShowDialog((Form)ChildManager);
            //return new MessageBoxDialog(messageType, messageCode, strMsg).ShowDialog((Form)ChildManager);
        }

        /// <summary>
        /// Show MessageBox 메시지박스
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="messageCode"></param>
        /// <param name="strMsg"></param>
        /// <param name="buttonLabels">표시 되는 버튼만큼 버튼글자</param>
        /// <returns></returns>
        public DialogResult ShowMessageBox(MessageDialogType messageType, string messageCode, 
            string strMsg, string[] buttonLabels)
        {
            Form f = (Form)ChildManager;
            if (f == null || f.IsDisposed || f.Disposing)
            {
                return new MessageDialog(messageType, null, messageCode, strMsg, buttonLabels).ShowDialog();
            }
            return new MessageDialog(messageType, null, messageCode, strMsg, buttonLabels).ShowDialog((Form)ChildManager);
            //return new MessageBoxDialog(messageType, messageCode, strMsg, buttonLabels).ShowDialog((Form)ChildManager);
        }

        #endregion

        #region Methods

        public FormBase()
        {
            InitializeComponent();

            this.Load += new EventHandler(FormBase_Load);
            this.Unload += new EventHandler(FormBase_Unload);
            this.ReturnResult = new Dictionary<string, object>();
            this.Top = this.Left = 0;
            this.BackgroundImage = Properties.Resources.bg_031;
        }

        /// <summary>
        /// Close form
        /// </summary>
        public void Close()
        {
            if (Unload != null)
            {
                this.Unload(this, EventArgs.Empty);
            }

            ChildManager.OnChildFormClosed(this);
        }

        public void PerformActivated()
        {
            this.AttachKeyInput();

            if (this.Activated != null)
            {
                this.Activated(this, EventArgs.Empty);
            }
        }

        public void PerformDeactivated()
        {
            this.DetachKeyInput();
            if (this.Deactivated != null)
            {
                this.Deactivated(this, EventArgs.Empty);
            }
        }

        new public IAsyncResult BeginInvoke(Delegate method)
        {
            if (!this.IsHandleCreated)
            {
                return null;
            }

            return base.BeginInvoke(method);
        }

        #endregion

        #region IKeyInputView Members

        public event OPOSKeyEventHandler KeyEvent;

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

        #region 이벤트정의
        
        void FormBase_Unload(object sender, EventArgs e)
        {
            GC.Collect();
            //GC.WaitForPendingFinalizers();

            EmptyWorkingSet(Process.GetCurrentProcess().Handle);
            SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);

            this.DetachKeyInput();
        }

        void FormBase_Load(object sender, EventArgs e)
        {
            if (this.Activated != null)
            {
                this.Activated(this, EventArgs.Empty);
            }
        }


        #endregion
    }

    public enum FormBaseType
    {
        Custom,
        Normal,
        Main,
        Login
    }
}
