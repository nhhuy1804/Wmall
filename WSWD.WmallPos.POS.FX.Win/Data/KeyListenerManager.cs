using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.POS.FX.Win.Utils;
using System.Security.Permissions;
using System.ComponentModel;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.FX.Win.Data
{
    public class KeyListenerManager
    {
        #region 생성자 & 변수

        Dictionary<string, List<IKeyInputView>> inputListeners;
        Dictionary<string, List<IKeyInputView>> nonInputListeners;

        Dictionary<string, IFocusableControl> activeControls;

        Dictionary<string, BackgroundWorker> containerWorkers;
        Dictionary<string, SizeQueue<OPOSKeyEventArgs>> containerKeys = null;

        public KeyListenerManager()
        {
            inputListeners = new Dictionary<string, List<IKeyInputView>>();
            nonInputListeners = new Dictionary<string, List<IKeyInputView>>();

            activeControls = new Dictionary<string, IFocusableControl>();
            containerKeys = new Dictionary<string, SizeQueue<OPOSKeyEventArgs>>();
            containerWorkers = new Dictionary<string, BackgroundWorker>();
        }

        #endregion

        #region KeyListeners

        private static KeyListenerManager m_instance = null;
        public static KeyListenerManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new KeyListenerManager();
                }

                return m_instance;
            }
        }

        private string m_activeContainer = string.Empty;
        public string ActiveContainer
        {
            get
            {
                return m_activeContainer;
            }
            set
            {
                m_activeContainer = value;
            }
        }

        public void DisposeActiveContainer()
        {
            if (inputListeners.ContainsKey(ActiveContainer))
            {
                inputListeners.Remove(ActiveContainer);
            }
            if (nonInputListeners.ContainsKey(ActiveContainer))
            {
                nonInputListeners.Remove(ActiveContainer);
            }
            if (activeControls.ContainsKey(ActiveContainer))
            {
                activeControls.Remove(ActiveContainer);
            }
            if (containerWorkers.ContainsKey(ActiveContainer))
            {
                containerWorkers.Remove(ActiveContainer);
            }
            ActiveContainer = string.Empty;
        }

        public void Register(IKeyInputView inputView)
        {
            Register(ActiveContainer, inputView);
        }

        public void Register(string container, IKeyInputView inputView)
        {
            if (!containerWorkers.ContainsKey(container))
            {
                BackgroundWorker wk = new BackgroundWorker();
                wk.DoWork += worker_DoWork;
                wk.RunWorkerCompleted += worker_RunWorkerCompleted;
                containerWorkers[container] = wk;
            }

            List<IKeyInputView> childs = null;

            if (inputView.GetType().Name.Equals("InputText")
                || inputView.GetType().IsSubclassOf(typeof(InputText)))
            {
                if (!inputListeners.ContainsKey(container))
                {
                    childs = new List<IKeyInputView>();
                    inputListeners.Add(container, childs);
                    activeControls[container] = null;
                }
                else
                {
                    childs = inputListeners[container];
                }

                if (childs.Contains(inputView))
                {
                    return;
                }

                childs.Add(inputView);
            }
            else
            {
                if (!nonInputListeners.ContainsKey(container))
                {
                    childs = new List<IKeyInputView>();
                    nonInputListeners.Add(container, childs);
                }
                else
                {
                    childs = nonInputListeners[container];
                }

                if (childs.Contains(inputView))
                {
                    return;
                }

                childs.Add(inputView);
            }

        }

        public void Deregister(IKeyInputView inputView)
        {
            Deregister(ActiveContainer, inputView);
        }

        public void Deregister(string container, IKeyInputView inputView)
        {
            List<IKeyInputView> childs = null;
            if (!inputListeners.ContainsKey(container) &&
                !nonInputListeners.ContainsKey(container))
            {
                return;
            }

            if (inputListeners.ContainsKey(container))
            {
                childs = inputListeners[container];
                childs.Remove(inputView);
            }

            if (nonInputListeners.ContainsKey(container))
            {
                childs = nonInputListeners[container];
                childs.Remove(inputView);
            }

            if (activeControls.ContainsKey(ActiveContainer) && activeControls[ActiveContainer] == inputView)
            {
                activeControls[ActiveContainer] = null;
            }
        }

        #endregion

        #region key queueing

        void Enqueue(OPOSKeyEventArgs e)
        {
            if (string.IsNullOrEmpty(ActiveContainer))
            {
                return;
            }

            SizeQueue<OPOSKeyEventArgs> keys = null;
            if (containerKeys.ContainsKey(ActiveContainer))
            {
                keys = containerKeys[ActiveContainer];
            }
            else
            {
                keys = new SizeQueue<OPOSKeyEventArgs>(20);
                containerKeys[ActiveContainer] = keys;
            }

            keys.Enqueue(e);
        }

        OPOSKeyEventArgs Dequeue()
        {
            SizeQueue<OPOSKeyEventArgs> keys = null;
            if (containerKeys.ContainsKey(ActiveContainer))
            {
                keys = containerKeys[ActiveContainer];
                return keys.Dequeue();
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region KeyProcessing Stack

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ProcessOneEvent((OPOSKeyEventArgs)e.Argument);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Trace.WriteLine(string.Format("{0} {1} completed", sender.GetHashCode(), ActiveContainer), "keyboard");
            StartProcessKeyEvent();
        }

        /// <summary>
        /// Key event 발생
        /// </summary>
        /// <param name="e"></param>
        public void PostKeyEvent(OPOSKeyEventArgs e)
        {
            if (e.Key == null)
            {
                return;
            }

            // Trace.WriteLine(e.KeyCodeText, "program");
            Enqueue(e);
            StartProcessKeyEvent();
        }

        public void ProcessOneEvent(OPOSKeyEventArgs e)
        {
            try
            {
#if DEBUG
                Trace.WriteLine(string.Format("Key: {0} {1} {2}", e.KeyCode, e.KeyCodeText, e.Key != null ?
                    e.Key.OPOSKey.ToString() : string.Empty), "keyboard");
#endif

                List<IKeyInputView> ls = nonInputListeners.ContainsKey(ActiveContainer) ?
                    nonInputListeners[ActiveContainer].Where(p => p.Enabled &&
                        p.Visible && !p.IsDisposed).ToList() : new List<IKeyInputView>();
                bool isHandled = false;
                int i = 0;

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
                    ls = inputListeners.ContainsKey(ActiveContainer) ?
                        inputListeners[ActiveContainer].Where(p => p.Enabled &&
                        p.Visible && !p.IsDisposed).ToList() : new List<IKeyInputView>();

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
                Trace.WriteLine("ActiveContainer: " + this.ActiveContainer, "keyboard");
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
            }
        }

        private static Object keyObject = new object();
        void StartProcessKeyEvent()
        {
            if (string.IsNullOrEmpty(ActiveContainer))
            {
                Trace.WriteLine("ActiveContainer Not found.", "keyboard");
                return;
            }

            BackgroundWorker wk = null;
            if (!containerWorkers.ContainsKey(ActiveContainer))
            {
                wk = new BackgroundWorker();
                wk.DoWork += worker_DoWork;
                wk.RunWorkerCompleted += worker_RunWorkerCompleted;
                containerWorkers[ActiveContainer] = wk;
            }
            else
            {
                wk = containerWorkers[ActiveContainer];
            }

            if (wk.IsBusy)
            {
                return;
            }

            var e = Dequeue();
            if (e != null)
            {
                Trace.WriteLine(string.Format("{0} {1} started", wk.GetHashCode(), ActiveContainer), "keyboard");
                wk.RunWorkerAsync(e);
            }
        }

        #endregion

        #region Control Navigation Up/Down

        public IFocusableControl FocusedControl
        {
            get
            {
                if (!inputListeners.ContainsKey(ActiveContainer))
                {
                    return null;
                }
                if (!activeControls.ContainsKey(ActiveContainer))
                {
                    return null;
                }

                return activeControls[ActiveContainer];
            }
        }

        public void ActivateControl(IFocusableControl control)
        {
            if (!control.Focusable)
            {
                return;
            }

            if (!activeControls.ContainsKey(ActiveContainer))
            {
                return;
            }

            var ctrl = activeControls[ActiveContainer];
            if (ctrl != null)
            {
                ctrl.IsFocused = false;
            }

            control.IsFocused = true;
            activeControls[ActiveContainer] = control;
        }

        public void NextControl()
        {
            ControlNavigate(true);
        }

        public void PreviousControl()
        {
            ControlNavigate(false);
        }

        void ControlNavigate(bool forward)
        {
            if (!inputListeners.ContainsKey(ActiveContainer))
            {
                return;
            }

            IFocusableControl toFocus = null;

            List<IFocusableControl> focusableControls = new List<IFocusableControl>();
            for (int i = 0; i < inputListeners[ActiveContainer].Count; i++)
            {
                var ctrl = inputListeners[ActiveContainer][i];

                if (ctrl is IFocusableControl)
                {
                    var fc = (IFocusableControl)ctrl;
                    if (fc.Focusable && fc.Visible && fc.Enabled)
                    {
                        focusableControls.Add((IFocusableControl)ctrl);
                    }
                }
            }

            // get current active control
            var control = activeControls[ActiveContainer];

            // not found
            int focusedIndex = control == null ? -1 : control.FocusedIndex;
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
                    navControl = focusableControls.Where(p => p.FocusedIndex >
                        focusedIndex).OrderBy(p => p.FocusedIndex).FirstOrDefault();
                }
                else
                {
                    navControl = focusableControls.Where(p => p.FocusedIndex <
                        focusedIndex).OrderByDescending(p => p.FocusedIndex).FirstOrDefault();
                }

                if (navControl == null)
                {
                    // get max focused index and min
                    int maxFocusedIndex = focusableControls.Max(p => p.FocusedIndex);
                    int minFocusedIndex = focusableControls.Min(p => p.FocusedIndex);

                    if (minFocusedIndex != maxFocusedIndex)
                    {
                        if (forward)
                        {
                            toFocus = focusableControls.FirstOrDefault(p => p.FocusedIndex == minFocusedIndex);
                        }
                        else
                        {
                            toFocus = focusableControls.FirstOrDefault(p => p.FocusedIndex == maxFocusedIndex);
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

            if (toFocus != null)
            {
                ActivateControl(toFocus);
            }
        }

        #endregion
    }

    class SizeQueue<T>
    {
        private readonly Queue<T> queue = new Queue<T>();
        private readonly int maxSize;
        public SizeQueue(int maxSize) { this.maxSize = maxSize; }

        public void Clear()
        {
            lock (queue)
            {
                queue.Clear();
            }
        }

        public void Enqueue(T item)
        {
            lock (queue)
            {
                while (queue.Count >= maxSize)
                {
                    Monitor.Wait(queue);
                }
                queue.Enqueue(item);
                if (queue.Count == 1)
                {
                    // wake up any blocked dequeue
                    Monitor.PulseAll(queue);
                }
            }
        }
        public T Dequeue()
        {
            lock (queue)
            {
                if (queue.Count == 0)
                {
                    //Monitor.Wait(queue);
                    return default(T);
                }

                T item = queue.Dequeue();
                if (queue.Count == maxSize - 1)
                {
                    // wake up any blocked enqueue
                    Monitor.PulseAll(queue);
                }
                return item;
            }
        }
    }
}
