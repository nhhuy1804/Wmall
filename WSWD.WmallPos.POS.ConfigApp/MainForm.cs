using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Data;
using WSWD.WmallPos.FX.Shared.Config;

namespace WSWD.WmallPos.POS.Config
{
    public partial class MainForm : MainFormBase
    {
        bool m_activated = false;

        #region 생성자, 초기화

        public MainForm()
        {
            InitializeComponent();
            Initialize();
        }

        void Initialize()
        {
            // Create directories
            ConfigData config = new ConfigData()
            {
                AppConfig = AppConfig.Load(),
                DevConfig = DevConfig.Load(),
                KeyMapConfig = KeyMapConfig.Load(),
                SysMessage = SysMessage.Load()
            };

            ConfigData.Initialize(config);

            // refresh global config
            FrameBaseData.Current.OnDataChanged(FrameBaseDataItem.ConfigChanged, null);
            SetTopMenu("MNU_CONFIG");

            this.Shown += new EventHandler(MainForm_Shown);
        }

        void MainForm_Shown(object sender, EventArgs e)
        {
            if (m_activated)
            {
                return;
            }

            m_activated = true;
            var dlg = new AdminPassPop();
            var res = dlg.ShowDialog(this);

            if (res != DialogResult.OK)
            {
                this.Close();
            }
        }

        #endregion
    }
}
