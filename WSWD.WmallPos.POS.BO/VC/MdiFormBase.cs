using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Data;
using WSWD.WmallPos.POS.FX.Shared.Data;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.BO.Menu;

namespace WSWD.WmallPos.POS.BO.VC
{
    /// <summary>
    /// RIGHT MENU 있는 메인화면
    /// </summary>
    public partial class MdiFormBase : FrameBase, IMainFormView
    {
        #region 초기화, 생성자

        public MdiFormBase()
            : this(true, string.Empty)
        {

        }

        /// <summary>
        /// Show MdiForm with top menu specified
        /// </summary>
        /// <param name="topMenuKey"></param>
        /// <param name="selectedMenuKey">UIMenu Key</param>
        public MdiFormBase(bool menuKey, string topMenuKey)
        {
            InitializeComponent();
            Initialize();

            mainMenu1.TopMenuKey = topMenuKey;
            mainMenu1.ModeSingleMenu = !string.IsNullOrEmpty(topMenuKey);
        }

        public MdiFormBase(string formAssembly, string formClass, params object[] constructParams)
        {
            InitializeComponent();
            Initialize();

            var menu = mainMenu1.Presenter.FindMenuByClass(formAssembly, formClass);
            if (menu != null)
            {
                var pMenu = mainMenu1.Presenter.FindMenuById(menu.PMenuId);
                if (pMenu != null)
                {
                    mainMenu1.TopMenuKey = pMenu.MenuKey;
                    mainMenu1.ModeSingleMenu = !string.IsNullOrEmpty(pMenu.MenuKey);
                }

                m_preloadAssembly = formAssembly;
                m_preloadFormClass = formClass;
                m_preloadMenuParams = constructParams;
            }
        }

        void Initialize()
        {
            this.Load += new EventHandler(MainForm_Load);
            this.Activated += new EventHandler(MainForm_Activated);
            this.Deactivate += new EventHandler(MainForm_Deactivate);

            // add MainMenu
            this.mainMenu1.MenuCommand += new MenuCommandHandler(mainMenu1_MenuCommand);
            this.mainMenu1.MenuClicked += new MenuClickedHandler(mainMenu1_MenuClicked);
        }

        #endregion

        #region 속성, 변수

        /// <summary>
        /// 처음로드시 자동 로드화면ID
        /// </summary>
        private string m_preloadAssembly = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        private string m_preloadFormClass = string.Empty;

        /// <summary>
        /// 처음로드시 자동 로드화면ID 생성자 Params
        /// </summary>
        private object[] m_preloadMenuParams = null;

        #endregion

        #region MainMenu Events, MainMenu navigation

        void mainMenu1_MenuClicked(MenuData menu)
        {
            if (menu.MenuType == MenuType.Popup)
            {
                var form = ShowPopup(menu.MenuName, menu.MenuDll, menu.MenuClass);
                if (form != null)
                {
                    form.ShowDialog(this);
                }
            }
            else
            {
                ShowForm(menu.MenuName, menu.MenuDll, menu.MenuClass);
            }
        }

        /// <summary>
        /// Command-Type Menu click handler
        /// </summary>
        /// <param name="command"></param>
        void mainMenu1_MenuCommand(MenuCommands command)
        {
            switch (command)
            {
                case MenuCommands.ProgramRestart:
                    this.Close();
                    Application.Exit();
                    break;
                case MenuCommands.ProgramEnd:
                    this.Close();
                    Application.Exit();
                    break;
                case MenuCommands.POSRestart:
                    break;
                case MenuCommands.POSEnd:
                    break;
                case MenuCommands.PreviousMenu:
                    if (mainMenu1.ModeSingleMenu)
                    {
                        this.Close();
                    }
                    break;
                default:
                    break;
            }
        }

        protected override void GotoTopMenu()
        {
            mainMenu1.GotoTopMenu();
        }

        #endregion

        #region MainForm events

        void MainForm_Activated(object sender, EventArgs e)
        {
            // active child formbase
            if (ActiveChild != null)
            {
                ActiveChild.PerformActivated();
            }
        }

        void MainForm_Deactivate(object sender, EventArgs e)
        {
            // active child formbase
            if (ActiveChild != null)
            {
                ActiveChild.PerformDeactivated();
            }
        }

        /// <summary>
        /// 메인화면 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainForm_Load(object sender, EventArgs e)
        {
            if (Screen.PrimaryScreen.WorkingArea.Width > 1024)
            {
                this.Left = 100;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
                        
            if (!string.IsNullOrEmpty(m_preloadFormClass))
            {
                ShowForm(string.Empty, m_preloadAssembly, m_preloadFormClass, m_preloadMenuParams);
            }
        }

        #endregion

        #region Child Management

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuKey"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        protected override FormBase ShowChildForm(FormBase form)
        {
            this.SuspendLayout();
            this.bpnlContainer.SuspendLayout();

            form.Left = 0;
            form.Top = 0;
            form.Size = form.ChildSize;
            this.bpnlContainer.Size = form.ChildSize;

            if (this.bpnlContainer.Controls.Count > 0)
            {
                FormBase lastForm = (FormBase)this.bpnlContainer.Controls[this.bpnlContainer.Controls.Count - 1];
                lastForm.PerformDeactivated();
                lastForm.Visible = false;
            }

            this.bpnlContainer.Controls.Add(form);
            this.bpnlContainer.ResumeLayout();
            form.PerformActivated();

            // set main menu state
            EnableMainMenu(!form.IsModal, !form.HideMainMenu);
            FrameBaseData.Current.OnDataChanged(FrameBaseDataItem.ActiveTitle, form.Text);

            this.ResumeLayout();
            return form;
        }

        /// <summary>
        /// On ChildForm close, remove from list
        /// </summary>
        /// <param name="form"></param>
        public override void OnChildFormClosed(FormBase form)
        {
            this.SuspendLayout();
            this.bpnlContainer.SuspendLayout();

            try
            {
                form.PerformDeactivated();
            }
            catch
            {
            }

            int idx = this.bpnlContainer.Controls.IndexOf(form);
            if (idx >= 0)
            {
                this.bpnlContainer.Controls.Remove(form);
                form.Dispose();
            }
            else
            {
                var cs = this.bpnlContainer.Controls.Find(form.Name, true);
                if (cs.Length > 0)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke((MethodInvoker)delegate()
                        {
                            this.bpnlContainer.Controls.Remove(cs[0]);
                        });
                    }
                    else
                    {
                        this.bpnlContainer.Controls.Remove(cs[0]);
                    }
                    form.Dispose();
                }
            }

            bool enableMainMenu = true;
            bool showMainMenu = true;
            string activeTitle = string.Empty;

            if (this.bpnlContainer.Controls.Count > 0)
            {
                FormBase lastForm = (FormBase)this.bpnlContainer.Controls[this.bpnlContainer.Controls.Count - 1];
                lastForm.PerformActivated();
                lastForm.Visible = true;

                bpnlContainer.Size = lastForm.ChildSize;
                enableMainMenu = !lastForm.IsModal;
                showMainMenu = !lastForm.HideMainMenu;

                activeTitle = lastForm.Text;
            }

            EnableMainMenu(enableMainMenu, showMainMenu);
            FrameBaseData.Current.OnDataChanged(FrameBaseDataItem.ActiveTitle, activeTitle);

            this.bpnlContainer.ResumeLayout();
            this.ResumeLayout();
        }

        void EnableMainMenu(bool enable, bool show)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    mainMenu1.Enabled = enable;
                    mainMenu1.Visible = show;
                });
            }
            else
            {
                mainMenu1.Enabled = enable;
                mainMenu1.Visible = show;
            }
        }

        protected override Control GetContainer()
        {
            return bpnlContainer;
        }

        protected FormBase ActiveChild
        {
            get
            {
                if (this.bpnlContainer.Controls.Count > 0)
                {
                    var form = this.bpnlContainer.Controls[this.bpnlContainer.Controls.Count - 1];
                    return (FormBase)this.bpnlContainer.Controls[this.bpnlContainer.Controls.Count - 1];
                }

                return null;
            }
        }

        public override void OnLoggedOut()
        {
            this.bpnlContainer.SuspendLayout();
            
            // close all child forms
            while (this.bpnlContainer.Controls.Count > 0)
            {
                var c = this.bpnlContainer.Controls[this.bpnlContainer.Controls.Count - 1];
                if (c is FormBase)
                {
                    ((FormBase)c).Close();
                }
            }

            this.bpnlContainer.ResumeLayout();
        }

        #endregion
    }
}
