using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.FX.Shared;

namespace WSWD.WmallPos.POS.FX.Win.Interfaces
{
    public interface IKeyInputView
    {
        /// <summary>
        /// Key event;
        /// </summary>
        event OPOSKeyEventHandler KeyEvent;

        /// <summary>
        /// Enable or disable?
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Visible
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// 객체 존재여부
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void PerformKeyEvent(OPOSKeyEventArgs e);

        /// <summary>
        /// invoke
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        object Invoke(Delegate method);

        /// <summary>
        /// Handle
        /// </summary>
        bool IsHandleCreated { get; }
    }
}
