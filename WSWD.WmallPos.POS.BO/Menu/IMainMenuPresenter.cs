using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.POS.FX.Shared.Data;

namespace WSWD.WmallPos.POS.BO.Menu
{
    public interface IMainMenuPresenter
    {
        MenuData[] GetChildMenus(MenuData menuData);
        MenuData[] GetUpMenus(MenuData menuData, out MenuData parentMenu);
        MenuData FindMenuByKey(string menuKey);
        MenuData FindMenuByClass(string formAssemnbly, string formClass);
        MenuData FindMenuById(string menuId);
    }
}
