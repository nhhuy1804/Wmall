using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Shared.Data;

namespace WSWD.WmallPos.POS.BO.Menu
{
    public class MainMenuPresenter : IMainMenuPresenter
    {
        private List<MenuData> m_allMenus;
        public MainMenuPresenter()
        {
            this.m_allMenus = new List<MenuData>();
            LoadMenus(null, null);
        }

        #region IMainMenuPresenter Members

        /// <summary>
        /// Get child menus
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public MenuData[] GetChildMenus(MenuData menu)
        {
            if (menu == null)
            {
                return m_allMenus.Where(p => p.PMenuId == string.Empty).ToArray();
            }

            return m_allMenus.Where(p => p.PMenuId == menu.MenuId).ToArray();
        }

        /// <summary>
        /// Get uper menus
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="parentMenu"></param>
        /// <returns></returns>
        public MenuData[] GetUpMenus(MenuData menu, out MenuData parentMenu)
        {
            MenuData pMenu = m_allMenus.FirstOrDefault(p => p.MenuId == menu.PMenuId);

            // get parent of pMenu
            pMenu = m_allMenus.FirstOrDefault(p => p.MenuId == pMenu.PMenuId);
            parentMenu = pMenu;
            return pMenu == null ? m_allMenus.Where(p => p.PMenuId == string.Empty).ToArray() :
                m_allMenus.Where(p => p.PMenuId == pMenu.MenuId).ToArray();
        }

        #endregion

        #region Load menus from menus.dat

        private void LoadMenus(XmlNode node, MenuData parentMenu)
        {
            if (node == null)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Path.Combine(FXConsts.FOLDER_RESOURCE.GetFolder(), FXConsts.RESOURCE_FILE_MENUS));
                node = doc.SelectSingleNode("/menu");
            }

            XmlNodeList nodes = node.SelectNodes("menu");
            int seq = 1;
            foreach (XmlNode n in nodes)
            {
                MenuData mr = new MenuData();

                // debug or test menu이면 추가안함
                if (n.Attributes["mode"] != null)
                {
                    string c = n.Attributes["mode"].Value;
                    mr.TestMenu = !string.IsNullOrEmpty(c);
#if !DEBUG
                    if (!string.IsNullOrEmpty(c))
                    {
                        continue;
                    }
#endif
                }

                mr.MenuName = n.Attributes["name"].Value;
                if (n.Attributes["type"] != null)
                {
                    string t = n.Attributes["type"].Value;
                    mr.MenuType = (MenuType)Enum.Parse(typeof(MenuType), t);
                }

                if (n.Attributes["url"] != null)
                {
                    string u = n.Attributes["url"].Value;
                    mr.MenuDll = u;
                }

                if (n.Attributes["class"] != null)
                {
                    string c = n.Attributes["class"].Value;
                    mr.MenuClass = c;
                }

                if (n.Attributes["key"] != null)
                {
                    string c = n.Attributes["key"].Value;
                    mr.MenuKey = c;
                }

                mr.MenuId = Guid.NewGuid().ToString("n");
                mr.PMenuId = parentMenu != null ? parentMenu.MenuId : string.Empty;
                mr.MenuSeq = seq;

                m_allMenus.Add(mr);
                seq++;

                // recursive adding
                LoadMenus(n, mr);
            }
        }

        /// <summary>
        /// FInd Menu by MenuKey
        /// </summary>
        /// <param name="menuKey"></param>
        /// <returns></returns>
        public MenuData FindMenuByKey(string menuKey)
        {
            return this.m_allMenus.FirstOrDefault(p => menuKey.Equals(p.MenuKey));
        }

        /// <summary>
        /// FormAssemly이나 FormClass으로 검색한다
        /// </summary>
        /// <param name="formAssembly"></param>
        /// <param name="formClass"></param>
        /// <returns></returns>
        public MenuData FindMenuByClass(string formAssembly, string formClass)
        {
            if (string.IsNullOrEmpty(formAssembly))
            {
                return this.m_allMenus.FirstOrDefault(p => formAssembly.Equals(p.MenuDll) && formClass.Equals(p.MenuClass));
            }
            else
            {
                return this.m_allMenus.FirstOrDefault(p => formClass.Equals(p.MenuClass));
            }
        }

        /// <summary>
        /// MenuID로 검색한다
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public MenuData FindMenuById(string menuId)
        {
            return this.m_allMenus.FirstOrDefault(p => p.MenuId.Equals(menuId));
        }

        #endregion

    }
}
