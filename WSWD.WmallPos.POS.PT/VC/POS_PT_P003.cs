//-----------------------------------------------------------------
/*
 * 화면명   : POS_PT_P003.cs
 * 화면설명 : 포인트 조회 (회원정보 확인)
 * 개발자   : 정광호
 * 개발일자 : 2015.04.06
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.POS.PT.PI;
using WSWD.WmallPos.POS.PT.PT;
using WSWD.WmallPos.POS.PT.VI;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm;

using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PP;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PP;
using WSWD.WmallPos.FX.NetComm.Tasks.PP;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.POS.FX.Win.Devices;

namespace WSWD.WmallPos.POS.PT.VC
{
    public partial class POS_PT_P003 : PopupBase
    {
        #region 변수

        /// <summary>
        /// 고객정보
        /// </summary>
        public DataTable dtCust = null;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        #endregion

        #region 생성자

        public POS_PT_P003(DataTable dt)
        {
            InitializeComponent();

            //회원정보 Copy
            dtCust = dt.Copy();

            //그리드 설정
            SetGrd();

            //Form Load Event
            this.Load += new EventHandler(form_Load);
            this.FormClosed += new FormClosedEventHandler(POS_PT_P003_FormClosed);
        }

        void POS_PT_P003_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Load -= new EventHandler(form_Load);
            this.FormClosed -= new FormClosedEventHandler(POS_PT_P003_FormClosed);

            grd.InitializeCell -= new CellDataBoundEventHandler(grd_InitializeCell);
            grd.RowDataBound -= new RowDataBoundEventHandler(grd_RowDataBound);

            this.KeyEvent -= new OPOSKeyEventHandler(form_KeyEvent);                    //Key Event
            this.btnSave.Click -= new EventHandler(btnSave_Click);                      //적용 button Event
            this.btnClose.Click -= new EventHandler(btnClose_Click);                    //닫기 button Event
        }

        #endregion

        #region 그리드 설정

        private void SetGrd()
        {
            grd.InitializeCell += new CellDataBoundEventHandler(grd_InitializeCell);
            grd.RowDataBound += new RowDataBoundEventHandler(grd_RowDataBound);

            //그리드 컬럼 설정
            grd.ColumnCount = 2;
            grd.SetColumn(0, strMsg01);
            grd.SetColumn(1, strMsg02, 120);
            grd.AutoFillRows = true;
            grd.ShowPageNo = true;
            grd.ScrollType = ScrollTypes.PageChanged;
            grd.PageIndex = -1;

            ((System.Windows.Forms.TableLayoutPanel)(grd.Controls[2])).RowStyles[1] = new RowStyle(SizeType.Absolute, 60);
            ((System.Windows.Forms.Label)(grd.Controls[2].Controls[1])).AutoSize = false;
            ((System.Windows.Forms.Label)(grd.Controls[2].Controls[1])).Font = new Font("돋움체", 9, FontStyle.Bold);
        }

        #endregion

        #region 이벤트 등록

        /// <summary>
        /// Form Event 정의
        /// </summary>
        private void InitEvent()
        {
            this.KeyEvent += new OPOSKeyEventHandler(form_KeyEvent);                    //Key Event
            this.btnSave.Click += new EventHandler(btnSave_Click);                      //적용 button Event
            this.btnClose.Click += new EventHandler(btnClose_Click);                    //닫기 button Event
        }

        #endregion

        #region 이벤트 정의

        /// <summary>
        /// Form Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void form_Load(object sender, EventArgs e)
        {
            //이벤트 등록
            InitEvent();

            //그리드 데이터 바인딩
            foreach (DataRow dr in dtCust.Rows)
            {
                grd.AddRow(dr);
            }

            //그리드 RowIndex 설정
            if (grd.RowCount > 0)
            {
                grd.PageIndex = 0;
                grd.SelectedRowIndex = 0;
            }
        }

        /// <summary>
        /// Key Event
        /// </summary>
        /// <param name="e"></param>
        void form_KeyEvent(OPOSKeyEventArgs e)
        {
            if (e.Key.OPOSKey == OPOSMapKeys.KEY_NOSALE)
            {
                if (POSDeviceManager.CashDrawer != null && POSDeviceManager.CashDrawer.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened && POSDeviceManager.CashDrawer.Enabled)
                {
                    //돈통 open
                    POSDeviceManager.CashDrawer.OpenDrawer();
                }
                return;
            }

            if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
            {
                e.IsHandled = true;
                btnClose_Click(btnClose, null);
            }
        }

        /// <summary>
        /// 그리드 초기화
        /// </summary>
        /// <param name="e"></param>
        void grd_InitializeCell(CellDataBoundEventArgs e)
        {
            Label lbl = null;
            switch (e.Cell.ColumnIndex)
            {
                case 0:
                case 1:
                    lbl = new Label()
                    {
                        TextAlign = ContentAlignment.MiddleLeft,
                        AutoSize = false,
                        Left = 0,
                        Top = 0,
                        Width = e.Cell.Width,
                        Height = e.Cell.Height,
                        BackColor = Color.Transparent
                    };
                    e.Cell.Controls.Add(lbl);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 그리드 데이터 바인딩
        /// </summary>
        /// <param name="row"></param>
        void grd_RowDataBound(RowDataBoundEventArgs e)
        {
            DataRow dr = (DataRow)e.ItemData;

            if (dr == null)
            {
                for (int i = 0; i < e.Row.Cells.Length; i++)
                {
                    e.Row.Cells[i].Controls[0].Text = string.Empty;
                }
                return;
            }

            e.Row.Cells[0].Controls[0].Text = TypeHelper.ToString(dr[0]);
            e.Row.Cells[1].Controls[0].Text = TypeHelper.ToString(dr[1]);
        }

        /// <summary>
        /// 적용 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (grd.RowCount > 0 && grd.SelectedRowIndex >= 0)
            {
                this.ReturnResult.Add("CardNo", ((DataRow)(grd.GetSelectedRow().ItemData)).ItemArray[0].ToString());
                this.DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// 닫기 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable/Disable
        /// </summary>
        void SetControlDisable(bool bDisable)
        {
            _bDisable = bDisable;

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    foreach (var item in this.ContainerPanel.Controls)
                    {
                        if (item.GetType().Name.ToString().ToLower() == "keypad")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad key = (WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad)item;
                            key.Enabled = !_bDisable;
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "inputtext")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.InputText txt = (WSWD.WmallPos.POS.FX.Win.UserControls.InputText)item;
                            txt.Enabled = !_bDisable;
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "button")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.Button btn = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)item;
                            btn.Enabled = !_bDisable;
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "salegridpanel")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel grdp = (WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel)item;
                            grdp.Enabled = !_bDisable;
                        }
                    }
                });
            }
            else
            {
                foreach (var item in this.ContainerPanel.Controls)
                {
                    if (item.GetType().Name.ToString().ToLower() == "keypad")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad key = (WSWD.WmallPos.POS.FX.Win.UserControls.KeyPad)item;
                        key.Enabled = !_bDisable;
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "inputtext")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.InputText txt = (WSWD.WmallPos.POS.FX.Win.UserControls.InputText)item;
                        txt.Enabled = !_bDisable;
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "button")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.Button btn = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)item;
                        btn.Enabled = !_bDisable;
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "salegridpanel")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel grdp = (WSWD.WmallPos.POS.FX.Win.UserControls.SaleGridPanel)item;
                        grdp.Enabled = !_bDisable;
                    }
                }
            }

            //Application.DoEvents();
        }

        #endregion
    }
}
