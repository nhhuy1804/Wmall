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
    public partial class ctrlProgress : UserControl
    {
        /// <summary>
        /// 생성자
        /// </summary>
        public ctrlProgress()
        {
            InitializeComponent();

            //프로그레스바 설정
            ChangeProgress(clsUtilsSTRING.conProgressNormalStatus, 0, 0, string.Empty, clsUtilsEnum.eProcess.eNormal, false);
        }

        /// <summary>
        /// 프로그레스바 설정
        /// </summary>
        /// <param name="Progress"></param>
        /// <param name="Value"></param>
        /// <param name="MaxValue"></param>
        /// <param name="ProgressFileNm"></param>
        /// <param name="eProcess"></param>
        /// <param name="bComplete"></param>
        public void ChangeProgress(string Progress, Int32 Value, Int32 MaxValue, string ProgressFileNm, clsUtilsEnum.eProcess eProcess, bool bComplete)
        {
            lblProgress.Text = Progress;
            pgBar.Value = Value;
            pgBar.Maximum = MaxValue;
            lblProgressCnt.Text = string.Format("{0} / {1}", Value.ToString(), MaxValue.ToString());
            
            if (eProcess == clsUtilsEnum.eProcess.eSave)
            {
                ProgressFileNm = ProgressFileNm + (!bComplete ? clsUtilsSTRING.conProgressSave : clsUtilsSTRING.conProgressSaveComplete);
            }
            else if (eProcess == clsUtilsEnum.eProcess.eDelete)
            {
                ProgressFileNm = ProgressFileNm + (!bComplete ? clsUtilsSTRING.conProgressDelete : clsUtilsSTRING.conProgressDeleteComplete);
            }
            else if (eProcess == clsUtilsEnum.eProcess.eDownload)
            {
                ProgressFileNm = ProgressFileNm + (!bComplete ? clsUtilsSTRING.conProgressDownload : clsUtilsSTRING.conProgressDownloadComplete);
            }

            lblProgressFileNm.Text = ProgressFileNm;

            Application.DoEvents();
        }
    }
}
