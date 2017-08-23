using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.TM.VI;
using WSWD.WmallPos.POS.TM.PI;
using WSWD.WmallPos.POS.TM.PT;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PP;
using WSWD.WmallPos.FX.Shared;
using System.IO;
using WSWD.WmallPos.POS.FX.Shared.Utils;
using WSWD.WmallPos.POS.FX.Win.Utils;

namespace WSWD.WmallPos.POS.TM.VC
{
    public partial class POS_TM_M001 : FormBase, IM001TestView, IM001SummaryView
    {
        ITestPresenter testPI;
        public POS_TM_M001()
        {
            InitializeComponent();
            inputControl3.Text = "1000";

            FormInitialize();

            this.KeyEvent += new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(POS_TM_M001_KeyEvent);
            this.Load += new EventHandler(POS_TM_M001_Load);
        }

        void POS_TM_M001_Load(object sender, EventArgs e)
        {
            inputControl1.SetFocus();
        }

        private void FormInitialize()
        {
            testPI = new TestPresenter(this, this);
        }

        private void wButton1_Click(object sender, EventArgs e)
        {
            ShowMessageBox(WSWD.WmallPos.FX.Shared.MessageDialogType.YesNoCancel, "ER00001", null);
        }

        private void wButton2_Click(object sender, EventArgs e)
        {
            var pop = ChildManager.ShowPopup("", "WSWD.WmallPos.POS.TM.dll", "WSWD.WmallPos.POS.TM.VC.POS_TM_P001");
            pop.ShowDialog(this);
        }

        private void wButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void wButton4_Click(object sender, EventArgs e)
        {
            var pop = ChildManager.ShowPopup("", "WSWD.WmallPos.POS.TM.dll", "WSWD.WmallPos.POS.TM.VC.POS_TM_P002");
            pop.ShowDialog(this);
        }

        private void wButton9_Click(object sender, EventArgs e)
        {
            wButton9.Selected = true;
            wButton8.Selected = false;
        }

        private void wButton8_Click(object sender, EventArgs e)
        {
            wButton9.Selected = false;
            wButton8.Selected = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            testPI.ProcessBSM043T();
        }

        #region IM001TestView Members

        public void UpdateBSM043TData(DataSet ds)
        {
            dataGridView1.DataSource = ds.Tables[0];
        }

        #endregion

        #region IM001SummaryView Members

        public void UpdateTotalCount(int total)
        {
            label1.Text = string.Format("Total {0}", total);
        }

        #endregion

        #region KeyEvent 처리방법

        /// <summary>
        /// 폼전체 KeyEvent 처리
        /// </summary>
        /// <param name="e"></param>
        void POS_TM_M001_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            // incAmt 금액이 입력가능하며 10,000,000넘으면 오류표시하한다
            if (!e.IsControlKey && incAmt.IsFocused)
            {
                int amt = TypeHelper.ToInt32(incAmt.Text);
                if (amt > 10000000)
                {
                    // ER00012은 PosMesg.dat에 등록해야함
                    ShowMessageBox(WSWD.WmallPos.FX.Shared.MessageDialogType.Warning, "ER00012", null);
                    e.IsHandled = true;
                }
            }
        }
        

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            // 포인트 조회
            var pop = ChildManager.ShowPopup("포인트 조회", "WSWD.WmallPos.POS.PT.dll", "WSWD.WmallPos.POS.PT.VC.POS_PT_P001");
            if (pop.ShowDialog(this) == DialogResult.OK)
            {
                if (pop.ReturnResult != null && pop.ReturnResult.Count > 0)
                {
                    foreach (var item in pop.ReturnResult)
                    {
                        MessageBox.Show(item.Value.ToString());
                        break;
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var pop = ChildManager.ShowPopup("복지카드", "WSWD.WmallPos.POS.PY.dll", "WSWD.WmallPos.POS.PY.VC.POS_PY_P008", 100000);
            if (pop.ShowDialog(this) == DialogResult.OK)
            {
                if (pop.ReturnResult != null && pop.ReturnResult.Count > 0)
                {
                    foreach (var item in pop.ReturnResult)
                    {
                        MessageBox.Show(item.Value.ToString());
                        break;
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //            카드번호 2701005166385
            //전화번호 01046627888

            //PP01RespData pp = new PP01RespData();
            //pp.CardNo = "2701005166385";
            //pp.CustName;
            //pp.GradeName;
            //pp.AbtyPoint = "100000";
            var pop = ChildManager.ShowPopup("포인트사용", "WSWD.WmallPos.POS.PY.dll", "WSWD.WmallPos.POS.PY.VC.POS_PY_P009", new object[] { 100000, null });
            if (pop.ShowDialog(this) == DialogResult.OK)
            {
                if (pop.ReturnResult != null && pop.ReturnResult.Count > 0)
                {
                    foreach (var item in pop.ReturnResult)
                    {
                        MessageBox.Show(item.Value.ToString());
                        break;
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var pop = ChildManager.ShowPopup("결제할인", "WSWD.WmallPos.POS.PY.dll", "WSWD.WmallPos.POS.PY.VC.POS_PY_P011", 100000);
            if (pop.ShowDialog(this) == DialogResult.OK)
            {
                if (pop.ReturnResult != null && pop.ReturnResult.Count > 0)
                {
                    foreach (var item in pop.ReturnResult)
                    {
                        MessageBox.Show(item.Value.ToString());
                        break;
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var pop = ChildManager.ShowPopup("할인쿠폰", "WSWD.WmallPos.POS.PY.dll", "WSWD.WmallPos.POS.PY.VC.POS_PY_P012", new object[] { 100000, "0" });
            if (pop.ShowDialog(this) == DialogResult.OK)
            {
                if (pop.ReturnResult != null && pop.ReturnResult.Count > 0)
                {
                    foreach (var item in pop.ReturnResult)
                    {
                        MessageBox.Show(item.Value.ToString());
                        break;
                    }
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var pop = ChildManager.ShowPopup("할인쿠폰", "WSWD.WmallPos.POS.PY.dll", "WSWD.WmallPos.POS.PY.VC.POS_PY_P012", new object[] { 100000, "1" });
            if (pop.ShowDialog(this) == DialogResult.OK)
            {
                if (pop.ReturnResult != null && pop.ReturnResult.Count > 0)
                {
                    foreach (var item in pop.ReturnResult)
                    {
                        MessageBox.Show(item.Value.ToString());
                        break;
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var pop = ChildManager.ShowPopup("공지사항", "WSWD.WmallPos.POS.SL.dll", "WSWD.WmallPos.POS.SL.VC.POS_SL_P004");
            pop.ShowDialog(this);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var pop = ChildManager.ShowPopup("가격조회", "WSWD.WmallPos.POS.SL.dll", "WSWD.WmallPos.POS.SL.VC.POS_SL_P002");
            pop.ShowDialog(this);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var edForm = ChildManager.ShowForm("POS정산", "WSWD.WmallPos.POS.ED.dll", "WSWD.WmallPos.POS.ED.VC.POS_ED_P003", new object[] { true });
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var pop = ChildManager.ShowPopup("타사상품권", "WSWD.WmallPos.POS.PY.dll", "WSWD.WmallPos.POS.PY.VC.POS_PY_P007", new object[] { 100000, null });
            if (pop.ShowDialog(this) == DialogResult.OK)
            {
                if (pop.ReturnResult != null && pop.ReturnResult.Count > 0)
                {
                    foreach (var item in pop.ReturnResult)
                    {
                        MessageBox.Show(item.Value.ToString());
                        break;
                    }
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            SetFTP();
        }

        /// <summary>
        /// 저널파일 FTP 전송
        /// </summary>
        /// <returns></returns>
        private bool SetFTP()
        {
            bool bReturn = false;
            string strFtpMsg = string.Empty;

            try
            {
                string strServer = ConfigData.Current.AppConfig.PosFTP.FtpSvrIP1;
                string strUser = ConfigData.Current.AppConfig.PosFTP.User;
                string strPass = ConfigData.Current.AppConfig.PosFTP.Pass;
                string strPort = ConfigData.Current.AppConfig.PosFTP.FtpComPort1;
                string strPath = Path.Combine(
                    FXConsts.JOURNAL.GetFolder(),
                    string.Format("{0}-{1}-{2}.jrn", ConfigData.Current.AppConfig.PosInfo.SaleDate, ConfigData.Current.AppConfig.PosInfo.StoreNo, ConfigData.Current.AppConfig.PosInfo.PosNo)
                    );
                FileInfo fi = new FileInfo(strPath);

                if (strServer.Length > 0 && strUser.Length > 0 && strPass.Length > 0 && strPort.Length > 0 && strPath.Length > 0 && fi.Exists)
                {
                    FtpUtils ftp = new FtpUtils(strServer, strUser, strPass, 10, TypeHelper.ToInt32(strPort));

                    if (ftp.Login(out strFtpMsg))
                    {
                        //폴더 검사
                        string[] arrDir = ftp.GetFileList("/", out bReturn, out strFtpMsg);

                        if (bReturn)
                        {
                            if (arrDir.Length > 0)
                            {
                                string strDir = string.Format("{0}-{1}-{2}", ConfigData.Current.AppConfig.PosInfo.SaleDate, ConfigData.Current.AppConfig.PosInfo.StoreNo, ConfigData.Current.AppConfig.PosInfo.PosNo);
                                bool bMake = false;
                                foreach (string strTemp in arrDir)
                                {
                                    if (strTemp == @"/" + strDir)
                                    {
                                        bMake = true;
                                        break;
                                    }
                                }

                                if (bMake)
                                {
                                    //폴더존재시 이동
                                    bReturn = ftp.ChangeDir(strDir, out strFtpMsg);
                                }
                                else
                                {
                                    bReturn = ftp.MakeDir(strDir, out strFtpMsg);
                                }
                            }
                        }
                    }

                    if (bReturn)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (ftp.Upload(strPath, out strFtpMsg))
                            {
                                ftp.Close();
                                bReturn = true;
                                break;
                            }
                        }
                    }
                }

                if (!bReturn)
                {
                    string[] strBtnNm = new string[2];
                    strBtnNm[0] = "재시도";
                    strBtnNm[1] = "닫기";

                    LogUtils.Instance.LogException(strFtpMsg);

                    if (ShowMessageBox(MessageDialogType.Question, null, "재시도", strBtnNm) == DialogResult.Yes)
                    {
                        bReturn = SetFTP();
                    }
                    else
                    {
                        //화면 종료
                        this.DialogResult = DialogResult.Cancel;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                
            }

            return bReturn;
        }
    }
}
