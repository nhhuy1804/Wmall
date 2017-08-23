using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.FX.Shared.Data;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Shared.Data;
using System.Runtime.InteropServices;
using WSWD.WmallPos.POS.FX.Win.Interfaces;

namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    partial class MainFormBase
    {
        #region 변수
        private int m_saleFormIndex = -1;
        private bool m_inSaleMode = false;

        /// <summary>
        /// 시스템 강제종료/재시작
        /// </summary>
        /// <param name="lpMachineName">컴퓨터 이름</param>
        /// <param name="lpMessage">종료 전 사용자에게 알릴 메시지</param>
        /// <param name="dwTimeout">종료까지 대기 시간</param>
        /// <param name="bForceAppsClosed">프로그램 강제 종료 여부(false > 강제 종료)</param>
        /// <param name="bRebootAfterShutdown">시스템 종료 후 다시 시작 여부(true > 다시 시작)</param>
        [DllImport("advapi32.dll")]
        public static extern void InitiateSystemShutdown(string lpMachineName, string lpMessage, int dwTimeout, bool bForceAppsClosed, bool bRebootAfterShutdown);

        #endregion

        #region 초기화
        /// <summary>
        /// initialize
        /// </summary>
        void Initialize()
        {
            this.Activated += new EventHandler(MainFormBase_Activated);
            this.Deactivate += new EventHandler(MainFormBase_Deactivate);

            this.mainMenu1.MenuClicked += new WSWD.WmallPos.POS.FX.Win.Interfaces.MenuClickedHandler(mainMenu1_MenuClicked);
            this.mainMenu1.MenuCommand += new WSWD.WmallPos.POS.FX.Win.Interfaces.MenuCommandHandler(mainMenu1_MenuCommand);
            this.mainMenu1.ValidateAdmin += new WSWD.WmallPos.POS.FX.Win.UserControls.MainMenuV2.ValidateAdminEventHandler(mainMenu1_ValidateAdmin);
            this.mainMenu1.ValidateMenuOnClick += new WSWD.WmallPos.POS.FX.Win.UserControls.MainMenuV2.ValidateMenuOnClickEventHandler(mainMenu1_ValidateMenuOnClick);
        }

        #endregion

        #region MainForm 이벤트

        void MainFormBase_Activated(object sender, EventArgs e)
        {
            // active child formbase
            if (ActiveChild != null)
            {
                ActiveChild.PerformActivated();
            }
        }

        void MainFormBase_Deactivate(object sender, EventArgs e)
        {
            // active child formbase
            if (ActiveChild != null)
            {
                ActiveChild.PerformDeactivated();
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
            this.SuspendDrawing();

            form.Left = 0;
            form.Top = 0;
            form.Size = form.ChildSize;
            this.ChildContainer.Size = form.ChildSize;

            int hideIndex = m_inSaleMode ? 0 : this.ChildContainer.Controls.Count - 1;

            if (this.ChildContainer.Controls.Count > 0)
            {
                FormBase lastForm = (FormBase)this.ChildContainer.Controls[hideIndex];
                lastForm.PerformDeactivated();
                lastForm.Visible = false;
            }

            this.ChildContainer.Controls.Add(form);

            // set main menu state
            EnableMainMenu(!form.IsModal, !form.HideMainMenu);
            FrameBaseData.Current.OnDataChanged(FrameBaseDataItem.ActiveTitle, form.Text);

            this.ResumeDrawing();

            // activated form
            form.PerformActivated();
            return form;
        }

        /// <summary>
        /// On ChildForm close, remove from list
        /// </summary>
        /// <param name="form"></param>
        public override void OnChildFormClosed(FormBase form)
        {
            this.SuspendDrawing();

            #region 현재폼 닫기

            try
            {
                form.PerformDeactivated();
            }
            catch
            {
            }

            int idx = this.ChildContainer.Controls.IndexOf(form);
            if (idx >= 0)
            {
                this.ChildContainer.Controls.Remove(form);
                form.Dispose();
            }
            else
            {
                var cs = this.ChildContainer.Controls.Find(form.Name, true);
                if (cs.Length > 0)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke((MethodInvoker)delegate()
                        {
                            this.ChildContainer.Controls.Remove(cs[0]);
                        });
                    }
                    else
                    {
                        this.ChildContainer.Controls.Remove(cs[0]);
                    }
                    form.Dispose();
                }
            }

            #endregion

            bool enableMainMenu = true;
            bool showMainMenu = true;
            string activeTitle = string.Empty;
            FormBase lastForm = null;

            if (this.ChildContainer.Controls.Count > 0)
            {
                if (m_inSaleMode)
                {
                    lastForm = (FormBase)this.ChildContainer.Controls[0];
                }
                else
                {
                    lastForm = (FormBase)this.ChildContainer.Controls[this.ChildContainer.Controls.Count - 1];
                }

                lastForm.Visible = true;
                ChildContainer.Size = lastForm.ChildSize;
                enableMainMenu = !lastForm.IsModal;
                showMainMenu = !lastForm.HideMainMenu;

                activeTitle = lastForm.Text;
            }

            EnableMainMenu(enableMainMenu, showMainMenu);
            FrameBaseData.Current.OnDataChanged(FrameBaseDataItem.ActiveTitle, activeTitle);

            this.ResumeDrawing();

            // activate form
            if (lastForm != null)
            {
                lastForm.PerformActivated();
            }
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

        protected FormBase ActiveChild
        {
            get
            {
                if (this.ChildContainer.Controls.Count > 0)
                {
                    var form = this.ChildContainer.Controls[this.ChildContainer.Controls.Count - 1];
                    return (FormBase)this.ChildContainer.Controls[this.ChildContainer.Controls.Count - 1];
                }

                return null;
            }
        }

        /// <summary>
        /// After login
        /// </summary>
        public override void OnLoggedIn()
        {
            base.OnLoggedIn();

            // 메인화면 호출한다
            ShowForm(string.Empty, "WSWD.WmallPos.POS.ST.dll", "WSWD.WmallPos.POS.ST.VC.POS_ST_M002");

            // reset MainMenu
            mainMenu1.GotoTopMenu();
        }

        /// <summary>
        /// 로그인아웃이벤트
        /// </summary>
        public override void OnLoggedOut()
        {
            base.OnLoggedOut();

            this.SuspendDrawing();

            // close all child forms
            while (this.ChildContainer.Controls.Count > 0)
            {
                var c = this.ChildContainer.Controls[this.ChildContainer.Controls.Count - 1];
                if (c is FormBase)
                {
                    try
                    {
                        ((FormBase)c).Close();
                    }
                    catch
                    {
                        
                    }
                }
            }

            // make signOff
            ClassHelper.InvokeMethod("WSWD.WmallPos.POS.SO.dll", "WSWD.WmallPos.POS.SO.PT.SOPresenter", "DoSignOff", null, null);

            // Show SIGNON UI
            var pop = ShowForm(string.Empty, "WSWD.WmallPos.POS.SO.dll", "WSWD.WmallPos.POS.SO.VC.POS_SO_M001");

            // repaint
            this.ResumeDrawing();
        }


        /// <summary>
        /// Show menu with empty left in container
        /// Hide all
        /// 1) hiden current menu (판매들록)
        /// 2) show right menu
        ///     - set mode of menu to SinggleMode
        ///     - Prev click; reshow menu
        /// 
        /// </summary>
        /// <param name="menuKey"></param>
        public override void ShowMenu(string menuKey, bool modeSale)
        {
            /*
            - hide current sale - index of control
            - show main previous
            - set mode to menu
            - show menu
            - prev menu; show back
             */
            m_inSaleMode = modeSale;
            if (modeSale)
            {
                m_saleFormIndex = this.ChildContainer.Controls.Count - 1;

                this.SuspendDrawing();

                // hide Sale and show first form
                FormBase form = (FormBase)this.ChildContainer.Controls[m_saleFormIndex];
                form.PerformDeactivated();
                form.Visible = false;
                //Application.DoEvents();

                // show first form
                form = (FormBase)this.ChildContainer.Controls[0];
                form.PerformActivated();
                form.Visible = true;
                ChildContainer.Size = form.ChildSize;
                FrameBaseData.Current.OnDataChanged(FrameBaseDataItem.ActiveTitle, form.Text);

                EnableMainMenu(!form.IsModal, !form.HideMainMenu);

                // show only menu
                mainMenu1.ModeSingleMenu = true;

                this.ResumeDrawing();
            }

            // show menu
            mainMenu1.ShowMenu(menuKey);
        }

        /// <summary>
        /// Hide 1st, and show sale form
        /// </summary>
        private void ShowLastHiddenForm()
        {
            this.SuspendDrawing();

            FormBase form = (FormBase)this.ChildContainer.Controls[0];
            form.PerformDeactivated();
            form.Visible = false;
            //Application.DoEvents();

            if (m_saleFormIndex.InRange(0, this.ChildContainer.Controls.Count))
            {
                form = (FormBase)this.ChildContainer.Controls[m_saleFormIndex];
                form.PerformActivated();
                form.Visible = true;    
            }
            
            FrameBaseData.Current.OnDataChanged(FrameBaseDataItem.ActiveTitle, form.Text);

            ChildContainer.Size = form.ChildSize;
            EnableMainMenu(!form.IsModal, !form.HideMainMenu);

            mainMenu1.ModeSingleMenu = false;
            m_inSaleMode = false;

            this.ResumeDrawing();
        }

        #endregion        

        #region MainMenu Events, MainMenu navigation

        protected void SetTopMenu(string menuKey)
        {
            mainMenu1.TopMenuKey = menuKey;
        }

        void mainMenu1_MenuClicked(MenuData menu)
        {
            string mName = menu.MenuName.IndexOf(".") < 4 ? menu.MenuName.Substring(menu.MenuName.IndexOf(".") + 1) :
                menu.MenuName;
            mName = mName.Trim();

            if (menu.MenuType == MenuType.Popup)
            {
                using (var form = ShowPopup(mName, menu.MenuDll, menu.MenuClass))
                {
                    if (form != null)
                    {
                        form.ShowDialog(this);
                    }
                }
            }
            else
            {
                ShowForm(mName, menu.MenuDll, menu.MenuClass);
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
                    OnProgramRestart();
                    this.Close();
                    break;
                case MenuCommands.ProgramEnd:
                    this.Close();
                    break;
                case MenuCommands.POSRestart:
                    //시스템 재시작
                    InitiateSystemShutdown("\\\\127.0.0.1", null, 0, false, true);
                    break;
                case MenuCommands.POSEnd:
                    //시스템 종료
                    InitiateSystemShutdown("\\\\127.0.0.1", null, 0, false, false);
                    break;
                case MenuCommands.PreviousMenu:
                    if (mainMenu1.ModeSingleMenu)
                    {
                        ShowLastHiddenForm();
                        GotoTopMenu();
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

        bool mainMenu1_ValidateAdmin()
        {
            using (var form = ShowPopup(string.Empty, "WSWD.WmallPos.POS.SO.dll",
                "WSWD.WmallPos.POS.SO.VC.POS_SO_P001", "04"))
            {
                var ret = form.ShowDialog(this);
                return ret == DialogResult.OK;
            }
        }

        /// <summary>
        /// Validator on click menu
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="validatorClass">IMenuClickValidator 상속받은 Class</param>
        /// <returns></returns>
        bool mainMenu1_ValidateMenuOnClick(string assemblyPath, string validatorClass)
        {
            var instance = ClassHelper.SafeClassLoad(assemblyPath, validatorClass);
            if (instance != null)
            {
                IMenuClickValidator validator = (IMenuClickValidator)instance;
                string errorMessage = string.Empty;

                if (!validator.ValidateMenuOnClick(out errorMessage))
                {
                    ShowMessageBox(MessageDialogType.Warning, string.Empty, errorMessage);
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Overridables

        protected virtual void OnProgramRestart()
        {

        }

        /// <summary>
        /// Find child form by type
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected override FormBase FindChildByType(string typeName)
        {
            var childs = ChildContainer.FindAllByType(typeName).ToArray();
            return childs.Length > 0 ? (FormBase)childs[0] : null;
        }

        #endregion

    }
}
