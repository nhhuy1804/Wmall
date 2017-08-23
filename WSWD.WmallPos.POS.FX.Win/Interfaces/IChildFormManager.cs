using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.POS.FX.Win.Forms;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.FX.Win.Interfaces
{
    public interface IChildFormManager
    {
        FormBase ShowForm(string formText, string formAssembly, string formClass,
            bool showProgress, params object[] constructorParams);
        FormBase ShowForm(string formText, string formAssembly, string formClass, params object[] constructorParams);
        KeyInputForm ShowPopup(string formText, string formAssembly, string formClass, params object[] constructorParams);

        /// <summary>
        /// Show MainForm Menu
        /// </summary>
        /// <param name="menuKey"></param>
        void ShowMenu(string menuKey, bool modeSale);

        void OnChildFormClosed(FormBase form);
        void ShowProgress(bool show);
        void ShowProgress(bool show, string message);

        void OnLoggedIn();
        void OnLoggedOut();
    }
}
