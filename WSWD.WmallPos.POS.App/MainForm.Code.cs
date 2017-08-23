using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using WSWD.WmallPos.POS.App.VI;
using WSWD.WmallPos.POS.App.PI;
using WSWD.WmallPos.POS.App.PT;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win;
using WSWD.WmallPos.POS.FX.Win.Data;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.POS.FX.Shared.Data;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.POS.ST.VC;
using WSWD.WmallPos.FX.Shared.Data;
using WSWD.WmallPos.FX.Shared.Helper;
using System.Runtime.InteropServices;
using System.Windows.Automation;


namespace WSWD.WmallPos.POS.App
{
    /// <summary>
    /// 개발자 : TCL
    /// 
    /// </summary>
    public partial class MainForm : IMainView
    {
        #region 변수

        const string AppStarterExe = @"..\update\AppStarter.exe";
        private IMainFormPresenter m_presenter;
        private bool m_loaded = false;

        #endregion

        #region 이벤트정의

        /// <summary>
        /// 개시화면 리턴한다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void POS_ST_M001_Unload(object sender, EventArgs e)
        {
            FormBase fb = (FormBase)sender;
            if (fb.DialogResult == DialogResult.Ignore)
            {
                this.Close();
                return;
            }
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            MainFormLoad();
        }

        void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmTaskTimer.Enabled = false;
            m_timerTasks.Clear();
            POSDeviceManager.Terminate();

            this.FormClosed -= new FormClosedEventHandler(MainForm_FormClosed);
            //this.Activated -= new EventHandler(MainForm_Activated);
            this.Load -= new EventHandler(MainForm_Load);
        }

        void MainForm_Activated(object sender, EventArgs e)
        {
            LoadInitUI();
        }

        void tmSysTime_Tick(object sender, EventArgs e)
        {
            ProcessTimerTask();
        }

        void tmShowFirstForm_Tick(object sender, EventArgs e)
        {
            LoadInitUI();
        }

        void axCtrlKeyboard1_ErrorEvent(object sender, AxKeyBoardHook.__CtrlKeyboard_ErrorEventEvent e)
        {
            Trace.WriteLine("axCtrlKeyboard1_ErrorEvent " + e.strData, "keyboard");
        }

        #endregion

        #region 사용자정의

        /// <summary>
        /// Load first UI, 개시
        /// </summary>
        void LoadInitUI()
        {
            if (m_loaded)
            {
                return;
            }

            m_loaded = true;
            #region 개시화면

            // WSWD.WmallPos.POS.ST.dll;WSWD.WmallPos.POS.ST.VC.POS_ST_M001
            var stM001Form = ShowForm(string.Empty, "WSWD.WmallPos.POS.ST.dll", "WSWD.WmallPos.POS.ST.VC.POS_ST_M001");
            stM001Form.Unload += new EventHandler(POS_ST_M001_Unload);

            #endregion

            var auto = AutomationElement.FromHandle(this.Handle);
            if (auto != null)
            {
                try
                {
                    auto.SetFocus();
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// initialize
        /// </summary>
        void Initialize()
        {
            //this.TopMost = true;
            this.FormClosed += new FormClosedEventHandler(MainForm_FormClosed);
            this.Load += new EventHandler(MainForm_Load);
            //this.Activated += new EventHandler(MainForm_Activated);
            this.tmTaskTimer.Tick += new EventHandler(tmSysTime_Tick);
            this.m_presenter = new MainFormPresenter(this);
        }

        void MainFormLoad()
        {
            LoadTimerTaskConfig();
            this.tmTaskTimer.Enabled = true;
#if !DEBUG
            Cursor.Hide();            
#endif
            if (Screen.PrimaryScreen.WorkingArea.Width > 1024)
            {
                this.Left = 0;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }


            Timer tm = new Timer()
            {
                Interval = 500,
            };
            tm.Tick += new EventHandler(tmShowFirstForm_Tick);
            tm.Enabled = true;
        }

        /// <summary>
        /// 판매등록화면에 상품등록중인지?
        /// </summary>
        public override bool SaleItemInputing
        {
            get
            {
                var child = FindChildByType("POS_SL_M001");
                if (child != null)
                {
                    var pi = child.GetType().GetProperty("HasItems");
                    if (pi != null)
                    {
                        var h = pi.GetValue(child, null);
                        return h != null && (bool)h;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 판매등록화면 있는상태?
        /// </summary>
        public override bool InSaleMode
        {
            get
            {
                return FindChildByType("POS_SL_M001") != null;
            }
        }

        #endregion

        #region 폰스관련 명령

        protected override void OnProgramRestart()
        {
            string exePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), AppStarterExe);

            if (File.Exists(exePath))
            {
                Process.Start(exePath);
            }
        }

        #endregion
    }
}
