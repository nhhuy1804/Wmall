﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.POS.FX.Shared.Data;
using WSWD.WmallPos.FX.Shared;

namespace WSWD.WmallPos.POS.FX.Win.Interfaces
{
    public interface IMainMenuView
    {
        MenuData[] NavigateBack(MenuData menu, out MenuData parentMenu);
        MenuData[] GotoMenu(MenuData menu);
    }

    public delegate void MenuClickedHandler(MenuData menu);
    public delegate void MenuCommandHandler(MenuCommands command);
}
