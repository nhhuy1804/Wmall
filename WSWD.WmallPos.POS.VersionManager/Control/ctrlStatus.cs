using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSWD.WmallPos.POS.VersionManager.Utils;

namespace WSWD.WmallPos.POS.VersionManager.Control
{
    public partial class ctrlStatus : UserControl
    {
        /// <summary>
        /// 생성자
        /// </summary>
        public ctrlStatus()
        {
            InitializeComponent();

            //진행중 상황 설정
            ChangeStatus(string.Empty, clsUtilsEnum.eProcess.eNormal);
        }

        /// <summary>
        /// 진행중 상황 설정
        /// </summary>
        /// <param name="eProcess"></param>
        public void ChangeStatus(string strVersion, clsUtilsEnum.eProcess eProcess)
        {
            string strMsg = string.Empty;

            if (eProcess == clsUtilsEnum.eProcess.eSelect)
            {
                strMsg = strVersion + clsUtilsSTRING.conStatusSelect;
            }
            else if (eProcess == clsUtilsEnum.eProcess.eSave)
            {
                strMsg = strVersion + clsUtilsSTRING.conStatusSave;
            }
            else if (eProcess == clsUtilsEnum.eProcess.eDelete)
            {
                strMsg = strVersion + clsUtilsSTRING.conStatusDelete;
            }

            lblStatus.Text = strMsg;
            Application.DoEvents();
        }
    }
}
