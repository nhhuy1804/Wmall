using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.POS.FX.Shared.Data;

namespace WSWD.WmallPos.POS.BO.VC
{
    public interface IMainMenuView
    {
        MenuData[] NavigateBack(MenuData menu, out MenuData parentMenu);
        MenuData[] GotoMenu(MenuData menu);
    }
}
