using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Data;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Win.Data;
using WSWD.WmallPos.POS.FX.Win.Devices;
using System.Runtime.InteropServices;
using System.Diagnostics;
using WSWD.WmallPos.FX.Shared.Helper;

namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    partial class FrameBase : KeyInputForm, IChildFormManager, IObserver<FrameBaseDataChangedEventArgs>
    {
        #region DLL Imports

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        const uint WM_KEYDOWN = 0x0100;
        //const uint WM_LBUTTONDOWN = 0x0201;

        #endregion

        #region 속성, 변수

        /// <summary>
        /// 반품인지?
        /// </summary>
        public bool StateRefund
        {
            get
            {
                return topBar1.StateRefund;
            }
            set
            {
                topBar1.StateRefund = value;
            }
        }

        /// <summary>
        /// 상내메시지
        /// </summary>
        public string StatusMessage
        {
            get
            {
                return statusBar1.StatusMessage;
            }
            set
            {
                statusBar1.StatusMessage = value;
            }
        }

        /// <summary>
        /// 판매중인상태?
        /// </summary>
        public virtual bool SaleItemInputing
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 판매등록하는중인지?
        /// </summary>
        public virtual bool InSaleMode
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 로그인되어있는지
        /// </summary>
        public bool IsLoggedIn { get; set; }

        #endregion

        #region 초기화

        private void FormInit()
        {
            this.FormClosed += new FormClosedEventHandler(FrameBase_FormClosed);
            this.Load += new EventHandler(FrameBase_Load);
        }

        #endregion

        #region 이벤트정의

        void FrameBase_Load(object sender, EventArgs e)
        {
            this.axCtrlKeyboard1.ScannerEvent += new AxKeyBoardHook.__CtrlKeyboard_ScannerEventEventHandler(axCtrlKeyboard1_ScannerEvent);

            // 여전법 05.24 비활성화
            // this.axCtrlKeyboard1.MsrEvent += new AxKeyBoardHook.__CtrlKeyboard_MsrEventEventHandler(axCtrlKeyboard1_MsrEvent);
            this.axCtrlKeyboard1.KeyboardEvent += new AxKeyBoardHook.__CtrlKeyboard_KeyboardEventEventHandler(axCtrlKeyboard1_KeyboardEvent);

            // Top, status update notifier
            FrameBaseData.Current.Attach(this);

            // update config to top and status
            FrameBaseData.Current.OnDataChanged(FrameBaseDataItem.ConfigChanged, null);
            this.topBar1.NoticeClick += new EventHandler(topBar1_NoticeClick);
        }

        void FrameBase_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.axCtrlKeyboard1.ScannerEvent -= new AxKeyBoardHook.__CtrlKeyboard_ScannerEventEventHandler(axCtrlKeyboard1_ScannerEvent);
            
            // 여전법 05.24 비활성화
            // this.axCtrlKeyboard1.MsrEvent -= new AxKeyBoardHook.__CtrlKeyboard_MsrEventEventHandler(axCtrlKeyboard1_MsrEvent);
            
            this.axCtrlKeyboard1.KeyboardEvent -= new AxKeyBoardHook.__CtrlKeyboard_KeyboardEventEventHandler(axCtrlKeyboard1_KeyboardEvent);
            this.topBar1.NoticeClick -= new EventHandler(topBar1_NoticeClick);

            this.FormClosed -= new FormClosedEventHandler(FrameBase_FormClosed);
            this.Load -= new EventHandler(FrameBase_Load);
            FrameBaseData.Current.Detach(this);
        }

        /// <summary>
        /// On notice icon clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void topBar1_NoticeClick(object sender, EventArgs e)
        {
            if (SaleItemInputing)
            {
                return;
            }

            using (var form = ShowPopup(string.Empty, "WSWD.WmallPos.POS.SL.dll", "WSWD.WmallPos.POS.SL.VC.POS_SL_P004"))
            {
                form.ShowDialog(this);
            }
        }

        #endregion

        #region OCX Events

        protected void axCtrlKeyboard1_KeyboardEvent(object sender, AxKeyBoardHook.__CtrlKeyboard_KeyboardEventEvent e)
        {
            if (KeyInputForm.ProgressHandle == KeyInputForm.ActiveHandle)
            {
                return;
            }

            if (Form.ActiveForm != null && 
                Form.ActiveForm.Handle != KeyInputForm.ActiveHandle)
            {
                return;
            }

            if (e.strData.Equals("025") ||
                e.strData.Equals("026") ||
                e.strData.Equals("027") ||
                e.strData.Equals("009") ||
                e.strData.Equals("06A"))
            {
                var ev = OPOSKeyEventArgs.FromKeyCode(e.strData);
                PostMessageHandler.Current.PostEvent(ev);
            }
            else
            {
                int k = int.Parse(e.strData, System.Globalization.NumberStyles.HexNumber);
                PostMessage(KeyInputForm.ActiveHandle, WM_KEYDOWN, (IntPtr)k, IntPtr.Zero);
            }
        }

        /// <summary>
        /// 여전법 05.24 비활성화
        /// 사용안함
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void axCtrlKeyboard1_MsrEvent(object sender, AxKeyBoardHook.__CtrlKeyboard_MsrEventEvent e)
        {
            TraceHelper.Instance.TraceWrite("axCtrlKeyboard1_MsrEvent:" + e.strTrack2);
            // post event to active form
            POSDeviceManager.Msr.ForwardKeyEvent(e.strTrack1, e.strTrack2, e.strTrack3);
        }

        protected void axCtrlKeyboard1_ScannerEvent(object sender, AxKeyBoardHook.__CtrlKeyboard_ScannerEventEvent e)
        {
            Trace.WriteLine("Read from scanner: " + e.strData, "program");
            POSDeviceManager.Scanner.ForwardScannerEvent(e.strData);
        }

        #endregion

        #region IChildFormManager Members

        public FormBase ShowForm(string formText, string formAssembly, string formClass, params object[] constructorParams)
        {
            return ShowForm(formText, formAssembly, formClass, true, constructorParams);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formAssembly"></param>
        /// <param name="formClass"></param>
        /// <param name="constructorParams"></param>
        /// <returns></returns>
        public virtual FormBase ShowForm(string formText, string formAssembly, string formClass, bool showProgress,
            params object[] constructorParams)
        {
            return _ShowForm(formText, formAssembly, formClass, showProgress, constructorParams);
        }

        FormBase _ShowForm(string formText, string formAssembly, string formClass, bool showProgress,
            params object[] constructorParams)
        {
            // Loading
            //if (showProgress) ShowProgress(true);
            FormBase newForm = null;

            try
            {
                string menuKey = formClass.Trim();
                if (!string.IsNullOrEmpty(menuKey))
                {
                    Object genericInstance = ClassLoader(formAssembly, menuKey, constructorParams);
                    if (genericInstance == null) return null;

                    newForm = genericInstance is FormBase ? (FormBase)genericInstance : null;
                    newForm.Name = menuKey;
                    if (!string.IsNullOrEmpty(formText))
                    {
                        newForm.Text = formText;
                    }
                    ShowChildForm(newForm);
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //if (showProgress) ShowProgress(false);
                this.Cursor = Cursors.Default;
            }

            return newForm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formAssembly"></param>
        /// <param name="formClass"></param>
        /// <param name="constructorParams"></param>
        /// <returns></returns>
        public KeyInputForm ShowPopup(string formText, string formAssembly, string formClass, params object[] constructorParams)
        {
            // Loading
            //ShowProgress(true);
            KeyInputForm newForm = null;

            try
            {
                string menuKey = formClass.Trim();
                if (newForm == null && !string.IsNullOrEmpty(menuKey))
                {
                    Object genericInstance = ClassLoader(formAssembly, menuKey, constructorParams);
                    if (genericInstance == null) return null;
                    newForm = genericInstance is KeyInputForm ? (KeyInputForm)genericInstance : null;
                }

                if (newForm != null)
                {
                    this.Cursor = Cursors.Default;
                    newForm.Name = menuKey;
                    if (!string.IsNullOrEmpty(formText)) newForm.Text = formText;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //ShowProgress(false);
                this.Cursor = Cursors.Default;
            }

            return newForm;
        }

        public virtual void OnChildFormClosed(FormBase form)
        {

        }

        /// <summary>
        /// Show Right Menu
        /// </summary>
        /// <param name="menuKey"></param>
        public virtual void ShowMenu(string menuKey, bool modeSale)
        {
        }

        /// <summary>
        /// 로그인
        /// </summary>
        public virtual void OnLoggedIn()
        {
            IsLoggedIn = true;
        }

        /// <summary>
        /// 로그인아웃
        /// </summary>
        public virtual void OnLoggedOut()
        {
            IsLoggedIn = false;
        }

        #endregion

        #region Overridables

        protected virtual FormBase ShowChildForm(FormBase form)
        {
            return null;
        }

        /// <summary>
        /// Navigate to top menu
        /// </summary>
        protected virtual void GotoTopMenu()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected virtual FormBase FindChildByType(string typeName)
        {
            return null;
        }

        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == WM_LBUTTONDOWN && m_progressForm.Visible)
        //    {
        //        m.Msg = 0;
        //    }

        //    base.WndProc(ref m);
        //}

        #endregion

        #region privates

        /// <summary>
        /// load form, popup class with constructor is FrameBase
        /// </summary>
        /// <param name="formAssembly"></param>
        /// <param name="formClass"></param>
        /// <param name="constructorParams"></param>
        /// <returns></returns>
        private Object ClassLoader(string formAssembly, string formClass, params object[] constructorParams)
        {
            var instance = ClassHelper.SafeClassLoad(formAssembly, formClass, constructorParams);
            if (instance is IChildFormView)
            {
                ((IChildFormView)instance).ChildManager = this;
            }
            return instance;
        }

        #endregion

        #region IObserver<FrameBaseDataChangedEventArgs> Members - Update top, status data

        /// <summary>
        /// Update top and status data;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Update(object sender, FrameBaseDataChangedEventArgs e)
        {
            switch (e.ChangedItem)
            {
                case FrameBaseDataItem.AllItem:
                    break;
                case FrameBaseDataItem.HasNotice:
                    topBar1.HasNotice = IsLoggedIn ? e.HasNotice : false;
                    break;
                case FrameBaseDataItem.ServerConnected:
                    topBar1.ServerConnected = e.ServerConnected;
                    break;
                case FrameBaseDataItem.SaleHoldCount:
                    topBar1.SaleHoldCount = e.SaleHoldCount;
                    break;
                case FrameBaseDataItem.UploadedTransCount:
                    topBar1.UploadedTransCount = e.UploadedTransCount;
                    break;
                case FrameBaseDataItem.TotalTransCount:
                    topBar1.TotalTransCount = e.TotalTransCount;
                    break;
                case FrameBaseDataItem.StatusBarMessage:
                    statusBar1.StatusMessage = e.StatusBarMessage;
                    break;
                case FrameBaseDataItem.ConfigChanged:
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate()
                        {
                            topBar1.RefreshGlobalConfig();
                            statusBar1.RefreshGlobalConfig();
                        });
                    }
                    else
                    {
                        topBar1.RefreshGlobalConfig();
                        statusBar1.RefreshGlobalConfig();
                    }
                    break;
                case FrameBaseDataItem.StateRefund:
                    topBar1.StateRefund = e.StateRefund;
                    break;
                case FrameBaseDataItem.ActiveTitle:
                    topBar1.ActiveTitle = e.ActiveTitle;
                    break;
                case FrameBaseDataItem.GotoMenu:
                    GotoTopMenu();
                    break;
                case FrameBaseDataItem.StatusBarItem:
                    if (e.StatusBarItem != null)
                    {
                        string[] data = (string[])e.StatusBarItem;
                        statusBar1.UpdateCommStatus(data[0], data[1]);
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region ProgressBar

        public virtual void ShowProgress(bool show)
        {
            ShowProgress(show, string.Empty);
        }

        public virtual void ShowProgress(bool show, string message)
        {
            if (show)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        ShowProgressForm(message);
                    });
                }
                else
                {
                    ShowProgressForm(message);
                }
            }
            else
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        HideProgressForm();
                    });
                }
                else
                {
                    HideProgressForm();
                }
            }
        }

        private ProgressForm m_progressForm = new ProgressForm();
        void ShowProgressForm(string message)
        {
            if (m_progressForm.Visible)
            {
                m_progressForm.SetMessage(message);
                Application.DoEvents();
                return;
            }

            m_progressForm.Show(this);
            m_progressForm.SetMessage(message);
            Application.DoEvents();
        }

        void HideProgressForm()
        {
            if (m_progressForm.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    m_progressForm.HideProgress();
                    Application.DoEvents();
                });
            }
            else
            {
                m_progressForm.HideProgress();
                Application.DoEvents();
            }
        }

        #endregion
    }
}
