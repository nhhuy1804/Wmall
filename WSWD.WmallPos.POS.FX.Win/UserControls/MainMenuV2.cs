using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Controls;
using WSWD.WmallPos.POS.FX.Win;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Shared.Data;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.Data;
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public partial class MainMenuV2 : UserControl, IMainMenuView, IKeyInputView
    {
        public delegate bool ValidateAdminEventHandler();
        public delegate bool ValidateMenuOnClickEventHandler(string assemblyPath, string validatorClass);
        
        #region 변수 & 속성

        public event MenuClickedHandler MenuClicked;
        public event MenuCommandHandler MenuCommand;
        public event ValidateAdminEventHandler ValidateAdmin;
        public event ValidateMenuOnClickEventHandler ValidateMenuOnClick;

        
        private const int MAX_MENU_COUNT = 6;
        private MenuData m_menuData;
        private IMainMenuPresenter m_menuPresenter;

        /// <summary>
        /// 한 메뉴만 선택한경우
        /// 판매화면에서 기능키 누를때 특정메뉴만 표시
        /// 그 메뉴안에 밖으로 못 나간다
        /// </summary>
        public bool ModeSingleMenu { get; set; }

        public IMainMenuPresenter Presenter
        {
            get
            {
                return m_menuPresenter;
            }
        }

        /// <summary>
        /// Set Top Menu Key mode
        /// </summary>
        public string TopMenuKey { get; set; }

        #endregion

        #region 생성자

        public MainMenuV2()
        {
            InitializeComponent();
            this.Load += new EventHandler(MainMenu_Load);
        }

        #endregion

        #region 이벤트정의

        void MainMenu_KeyEvent(OPOSKeyEventArgs e)
        {
            if (!this.Visible || !this.Enabled)
            {
                return;
            }

            if (!e.IsControlKey)
            {
                e.IsHandled = true;
                if (this.lblSelectMenu.Text.Length == 2)
                {
                    this.lblSelectMenu.Text = string.Empty;
                }

                this.lblSelectMenu.Text += (char)e.KeyCode;
            }
            else
            {
                if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
                {
                    e.IsHandled = true;
                    FindMenuByText();
                }
                else if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
                {
                    if (this.lblSelectMenu.Text.Length == 0)
                    {
                        if (ModeSingleMenu)
                        {
                            // back key
                            var backMenu = FindChildMenuByClass(m_menuData, "PreviousMenu");
                            if (backMenu != null)
                            {
                                SelectMenu(backMenu);
                                return;
                            }
                        }
                    }
                    else
                    {
                        this.lblSelectMenu.Text = string.Empty;
                    }
                }
                else
                {                    
                    FrameBase fb = (FrameBase)this.ParentForm;
                    ((IChildFormManager)this.ParentForm).ProcessFuncKey(e, false, fb.InSaleMode || fb.SaleItemInputing);
                }
            }
        }

        void MainMenu_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                InitializeMainMenu();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MenuButton_Click(object sender, EventArgs e)
        {
            MenuButton btn = (MenuButton)sender;
            if (string.IsNullOrEmpty(btn.Text))
            {
                return;
            }

            MenuData data = (MenuData)btn.Tag;
            SelectMenu(data);
        }

        #endregion

        #region 사용자정의

        /// <summary>
        /// 
        /// </summary>
        private void InitializeMainMenu()
        {
            this.AttachKeyInput();
            this.KeyEvent += new OPOSKeyEventHandler(MainMenu_KeyEvent);

            for (int i = 0; i < MAX_MENU_COUNT; i++)
            {
                MenuButton btn = (MenuButton)this.Controls.Find(string.Format("button{0}", i + 1), true)[0];
                btn.Text = string.Empty;
                btn.Tag = null;
                btn.Click += new EventHandler(MenuButton_Click);
            }

            m_menuPresenter = new MainMenuPresenter();

            // init root
            m_menuData = null;
            if (!string.IsNullOrEmpty(TopMenuKey))
            {
                m_menuData = m_menuPresenter.FindMenuByKey(TopMenuKey); ;
            }

            var menus = GotoMenu(m_menuData);
            RenderSubMenus(m_menuData, menus);
        }

        private void SelectMenu(MenuData data)
        {
            if (data.IsAdminMenu)
            {
                if (!this.ValidateAdmin())
                {
                    return;
                }
            }

            if (!string.IsNullOrEmpty(data.ValidatorClass))
            {
                if (!ValidateMenuOnClick(data.MenuDll, data.ValidatorClass))
                {
                    return;
                }
            }

            if (data.MenuType == MenuType.Menu)
            {
                var menus = GotoMenu(data);
                if (menus == null || menus.Length == 0)
                {
                    return;
                }

                RenderSubMenus(data, menus);
                return;
            }
            else if (data.MenuType == MenuType.MenuCommand)
            {
                MenuCommands command = (MenuCommands)Enum.Parse(typeof(MenuCommands), data.MenuClass);

                if (command == MenuCommands.PreviousMenu)
                {
                    if (ModeSingleMenu)
                    {
                        if (MenuCommand != null)
                        {
                            this.MenuCommand(command);
                        }
                    }
                    else
                    {
                        MenuData parentMenu = null;
                        var menus = NavigateBack(data, out parentMenu);
                        RenderSubMenus(parentMenu, menus);
                    }
                }
                else
                {
                    if (MenuCommand != null)
                    {
                        this.MenuCommand(command);
                    }
                }
            }
            else
            {
                if (MenuClicked != null)
                {
                    this.MenuClicked(data);
                }
            }
        }

        public void ShowMenu(string menuKey)
        {
            m_menuData = m_menuPresenter.FindMenuByKey(menuKey);

            var menus = GotoMenu(m_menuData);
            RenderSubMenus(m_menuData, menus);
        }

        private void RenderSubMenus(MenuData currentMenu, MenuData[] menus)
        {
            m_menuData = currentMenu;
            if (menus != null && menus.Length > 0)
            {
                MenuButton btn = null;
                for (int i = 0; i < MAX_MENU_COUNT; i++)
                {
                    btn = (MenuButton)this.Controls.Find(string.Format("button{0}", i + 1), true)[0];
                    if (i > menus.Length - 2)
                    {
                        btn.Tag = null;
                        btn.Text = string.Empty;
                    }
                    else
                    {
                        btn.Tag = menus[i];
                        btn.Text = menus[i].MenuName;
                    }
                }

                btn = (MenuButton)this.Controls.Find(string.Format("button{0}", MAX_MENU_COUNT), true)[0];
                btn.Tag = menus[menus.Length - 1];
                btn.Text = menus[menus.Length - 1].MenuName;
                btn.ButtonType = currentMenu == null ? MenuButtonType.TypeNorm : MenuButtonType.TypeBack;
            }
        }

        #endregion

        #region IMainMenuView Members

        public void GotoTopMenu()
        {
            var menus = GotoMenu(null);
            RenderSubMenus(null, menus);
        }

        public MenuData[] NavigateBack(MenuData menu, out MenuData parentMenu)
        {
            return m_menuPresenter.GetUpMenus(menu, out parentMenu);
        }

        public MenuData[] GotoMenu(MenuData menu)
        {
            return m_menuPresenter.GetChildMenus(menu);
        }

        /// <summary>
        /// Find child menu by class name
        /// </summary>
        /// <param name="pMenu"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        private MenuData FindChildMenuByClass(MenuData pMenu, string className)
        {
            var childs = m_menuPresenter.GetChildMenus(pMenu);
            return childs.FirstOrDefault(p => p.MenuClass.Equals(className));
        }
        
        void FindMenuByText()
        {
            if (string.IsNullOrEmpty(lblSelectMenu.Text))
            {
                return;
            }

            // search in Menu buttons
            string searchText = lblSelectMenu.Text + ".";
            MenuData m = null;
            foreach (Control c in this.Controls)
            {
                if (c.Text.StartsWith(searchText))
                {
                    m = (MenuData)c.Tag;
                    if (m != null)
                    {
                        break;
                    }
                }
            }

            lblSelectMenu.Text = string.Empty;

            if (m != null)
            {
                SelectMenu(m);
            }
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
    }
}
