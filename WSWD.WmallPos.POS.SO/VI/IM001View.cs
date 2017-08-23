using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.POS.SO.Data;

namespace WSWD.WmallPos.POS.SO.VI
{
    public interface IM001View
    {
        void Ready();
        string CasNo { get; set; }
        string CasName { get; set; }
        string CasPass { get; set; }
        

        void UpdateStatusMessage(LoginMessageTypes msgType);
        void UpdateStatusMessage(string message);

        void SetFocus(int focusCtrl);

        void LoginSuccess();
        bool ChkPrint();

        /// <summary>
        /// Invoke ?
        /// </summary>
        bool InvokeRequired { get; }

        /// <summary>
        /// Invoke method
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IAsyncResult BeginInvoke(Delegate method);
    }
}
