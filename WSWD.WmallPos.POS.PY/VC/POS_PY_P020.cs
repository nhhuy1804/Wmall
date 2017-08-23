//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P020.cs
 * 화면설명 : 타사상품권 상품권종류 선택
 * 개발자   : 정광호
 * 개발일자 : 2015.06.01
*/
//-----------------------------------------------------------------

using System;
using System.Windows.Forms;

using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.FX.Win.Data;
using System.Data;
using WSWD.WmallPos.POS.PY.PI;
using WSWD.WmallPos.POS.PY.PT;
using WSWD.WmallPos.POS.PY.VI;

namespace WSWD.WmallPos.POS.PY.VC
{
    public partial class POS_PY_P020 : PopupBase01, IPYP020View
    {
        #region 변수

        private IPYP020presenter m_Presenter;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        #endregion

        #region 생성자

        public POS_PY_P020()
        {
            InitializeComponent();

            //Form Load Event
            Load += new EventHandler(form_Load); 
        }

        #endregion

        #region 이벤트 등록

        /// <summary>
        /// Form Event 정의
        /// </summary>
        private void InitEvent()
        {
            this.btnSave.Click += new EventHandler(btnSave_Click);                                                              //적용 button Event
            this.btnClose.Click += new EventHandler(btnClose_Click);                                                            //닫기 button Event
            grd.RowDataBound += new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanelRowDataBoundEventHandler(grd_RowDataBound);  //그리드 데이터 바인딩 Event
            this.FormClosed += new FormClosedEventHandler(POS_PY_P020_FormClosed);
        }

        void POS_PY_P020_FormClosed(object sender, FormClosedEventArgs e)
        {
            Load -= new EventHandler(form_Load);
            this.btnSave.Click -= new EventHandler(btnSave_Click);                                                              //적용 button Event
            this.btnClose.Click -= new EventHandler(btnClose_Click);                                                            //닫기 button Event
            grd.RowDataBound -= new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanelRowDataBoundEventHandler(grd_RowDataBound);  //그리드 데이터 바인딩 Event
            this.FormClosed -= new FormClosedEventHandler(POS_PY_P020_FormClosed);
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

            //그리드 컬럼 설정
            grd.AddColumn("KD_GIFT", "상품권종류", 80);
            grd.AddColumn("NM_GIFT", "상품권종류명");

            SetControlDisable(true);

            //타사상품권 종류 조회
            m_Presenter = new PYP020presenter(this);
            m_Presenter.GetType();
        }

        /// <summary>
        /// 그리드 데이터 바인딩
        /// </summary>
        /// <param name="row"></param>
        void grd_RowDataBound(WSWD.WmallPos.POS.FX.Win.UserControls.GridRow row)
        {
            if (row.RowState == GridRowState.Added)
            {
                // init cells
                row.Cells[0].Controls.Add(new Label()
                {
                    Text = ((System.Data.DataRow)(row.ItemData)).ItemArray[0].ToString(),
                    AutoSize = false,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                });

                row.Cells[1].Controls.Add(new Label()
                {
                    Text = ((System.Data.DataRow)(row.ItemData)).ItemArray[1].ToString(),
                    AutoSize = false,
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill
                });
            }
        }

        /// <summary>
        /// 적용 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;

            if (grd.RowCount > 0 && grd.CurrentRowIndex >= 0)
            {
                this.ReturnResult.Add("KD_GIFT", ((System.Data.DataRow)(grd.GetRow(grd.CurrentRowIndex).ItemData)).ItemArray[0].ToString());
                this.ReturnResult.Add("NM_GIFT", ((System.Data.DataRow)(grd.GetRow(grd.CurrentRowIndex).ItemData)).ItemArray[1].ToString());
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
            if (_bDisable) return;

            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region 사용자 정의

        public void SetType(DataSet ds)
        {
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    //그리드 데이터 바인딩
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        grd.AddRow(dr);
                    }

                    //그리드 RowIndex 설정
                    if (grd.RowCount > 0)
                    {
                        grd.CurrentRowIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                SetControlDisable(false);
            }
        }

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
                        else if (item.GetType().Name.ToString().ToLower() == "gridpanel")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel grd = (WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel)item;
                            grd.Enabled = !_bDisable;
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "panel")
                        {
                            Panel pnl = (Panel)item;
                            pnl.Enabled = !_bDisable;
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
                    else if (item.GetType().Name.ToString().ToLower() == "gridpanel")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel grd = (WSWD.WmallPos.POS.FX.Win.UserControls.GridPanel)item;
                        grd.Enabled = !_bDisable;
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "panel")
                    {
                        Panel pnl = (Panel)item;
                        pnl.Enabled = !_bDisable;
                    }
                }
            }

            //Application.DoEvents();
        }

        #endregion
    }
}
