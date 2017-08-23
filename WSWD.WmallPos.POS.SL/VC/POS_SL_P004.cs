//-----------------------------------------------------------------
/*
 * 화면명   : POS_SL_P004.cs
 * 화면설명 : 공지사항
 * 개발자   : 정광호
 * 개발일자 : 2015.04.27
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

using WSWD.WmallPos.POS.SL.PI;
using WSWD.WmallPos.POS.SL.PT;
using WSWD.WmallPos.POS.SL.VI;
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

namespace WSWD.WmallPos.POS.SL.VC
{
    public partial class POS_SL_P004 : PopupBase, ISLP004View
    {
        #region 변수

        /// <summary>
        /// 비즈니스 로직
        /// </summary>
        private ISLP004presenter m_Presenter;

        /// <summary>
        /// 시작일자
        /// </summary>
        private string _strDD_START = string.Empty;

        /// <summary>
        /// 순번
        /// </summary>
        private string _strNO_SEQ = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        // private int iPrintRow = 0;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        #endregion

        #region 생성자

        public POS_SL_P004()
        {
            InitializeComponent();

            //그리드 설정
            SetGrd();

            //Form Load Event
            this.Load += new EventHandler(form_Load);
            this.FormClosed += new FormClosedEventHandler(POS_SL_P004_FormClosed);
        }

        void POS_SL_P004_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Load -= new EventHandler(form_Load);
            this.FormClosed -= new FormClosedEventHandler(POS_SL_P004_FormClosed);
            this.btnSave.Click -= new EventHandler(btnSave_Click);                                                                          //적용 button Event
            this.btnClose.Click -= new EventHandler(btnClose_Click);
        }

        /// <summary>
        /// 공지사항
        /// </summary>
        /// <param name="strDD_START">시작일자</param>
        /// <param name="strNO_SEQ">순번</param>
        public POS_SL_P004(string strDD_START, string strNO_SEQ)
        {
            InitializeComponent();

            //시작일자
            _strDD_START = strDD_START;

            //순번
            _strNO_SEQ = strNO_SEQ;

            //그리드 설정
            SetGrd();

            //Form Load Event
            this.Load += new EventHandler(form_Load);
            this.FormClosed += new FormClosedEventHandler(POS_SL_P004_FormClosed);
        }

        #endregion

        #region 이벤트 등록

        /// <summary>
        /// Form Event 정의
        /// </summary>
        private void InitEvent()
        {
            this.btnSave.Click += new EventHandler(btnSave_Click);                                                                          //적용 button Event
            this.btnClose.Click += new EventHandler(btnClose_Click);                                                                        //닫기 button Event
        }

        #endregion

        #region 이벤트 정의

        #region 그리드 설정

        private void SetGrd()
        {
            grd.InitializeCell += new CellDataBoundEventHandler(grd_InitializeCell);
            grd.RowDataBound += new RowDataBoundEventHandler(grd_RowDataBound);
            grd.RowSelected += new RowSelectedEventHandler(grd_RowSelected);
            grd.RowClicked += new EventHandler(grd_RowClicked);

            grd.PageIndexChanged += new EventHandler(grd_PageIndexChanged);

            //그리드 컬럼 설정
            grd.ColumnCount = 9;
            grd.SetColumn(0, strCOL01, 120);
            grd.SetColumn(1, strCOL02, 70);
            grd.SetColumn(2, "", 0);
            grd.SetColumn(3, strCOL04);
            grd.SetColumn(4, "", 0);
            grd.SetColumn(5, "", 0);
            grd.SetColumn(6, "", 0);
            grd.SetColumn(7, strCOL08, 100);
            grd.SetColumn(8, strCOL09, 70);
            grd.AutoFillRows = true;
            grd.ShowPageNo = true;
            grd.ScrollType = ScrollTypes.PageChanged;
            grd.PageIndex = -1;


            ((System.Windows.Forms.TableLayoutPanel)(grd.Controls[2])).RowStyles[1] = new RowStyle(SizeType.Absolute, 60);
            ((System.Windows.Forms.Label)(grd.Controls[2].Controls[1])).AutoSize = false;
            ((System.Windows.Forms.Label)(grd.Controls[2].Controls[1])).Font = new Font("돋움체", 9, FontStyle.Bold);
        }

        #endregion

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
            //grd.AddColumn("DD_START", strCOL01, 120);
            //grd.AddColumn("NO_SEQ", strCOL02, 70);
            //grd.AddColumn("DD_END", "", 0);
            //grd.AddColumn("NM_TITLE", strCOL04);
            //grd.AddColumn("NM_DESC", "", 0);
            //grd.AddColumn("FG_URGT", "", 0);
            //grd.AddColumn("NO_SEND_USER", "", 0);
            //grd.AddColumn("NM_SEND_USER", strCOL08, 100);
            //grd.AddColumn("FLAG_YN", strCOL09, 70);

            SetControlDisable(true);

            //공지사항 조회------------------------------
            m_Presenter = new SLP004presenter(this);
            m_Presenter.GetNotice();
            //-------------------------------------------
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
                case 8:
                    lbl = new Label()
                    {
                        TextAlign = ContentAlignment.MiddleCenter,
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
            e.Row.Cells[2].Controls[0].Text = TypeHelper.ToString(dr[2]);
            e.Row.Cells[3].Controls[0].Text = TypeHelper.ToString(dr[3]);
            e.Row.Cells[4].Controls[0].Text = TypeHelper.ToString(dr[4]);
            e.Row.Cells[5].Controls[0].Text = TypeHelper.ToString(dr[5]);
            e.Row.Cells[6].Controls[0].Text = TypeHelper.ToString(dr[6]);
            e.Row.Cells[7].Controls[0].Text = TypeHelper.ToString(dr[7]);
            e.Row.Cells[8].Controls[0].Text = TypeHelper.ToString(dr[8]);
        }

        void grd_PageIndexChanged(object sender, EventArgs e)
        {
            if (_bDisable) return;

            SetControlDisable(true);

            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        txtPnl.Clear();
                    });
                }
                else
                {
                    txtPnl.Clear();
                }

                if (grd.RowCount > 0 && grd.SelectedRowIndex >= 0)
                {
                    DataRow dr = (DataRow)grd.GetSelectedRow().ItemData;

                    if (dr != null)
                    {
                        txtPnl.BindNoticeInfo(TypeHelper.ToString(dr["NM_DESC"]));
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

        void grd_RowClicked(object sender, EventArgs e)
        {
            if (_bDisable) return;

            SetControlDisable(true);

            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        txtPnl.Clear();
                    });
                }
                else
                {
                    txtPnl.Clear();
                }

                if (grd.RowCount > 0 && grd.SelectedRowIndex >= 0)
                {
                    DataRow dr = (DataRow)grd.GetSelectedRow().ItemData;

                    if (dr != null)
                    {
                        txtPnl.BindNoticeInfo(TypeHelper.ToString(dr["NM_DESC"]));
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

        void grd_RowSelected(RowChangingEventArgs e)
        {
            //if (_bDisable) return;

            //SetControlDisable(true);

            //try
            //{
            //    if (this.InvokeRequired)
            //    {
            //        this.BeginInvoke((MethodInvoker)delegate()
            //        {
            //            txtPnl.Clear();
            //        });
            //    }
            //    else
            //    {
            //        txtPnl.Clear();
            //    }

            //    if (grd.RowCount > 0 && grd.SelectedRowIndex >= 0 && e != null && e.Row.ItemData != null)
            //    {
            //        DataRow dr = (DataRow)e.Row.ItemData;

            //        if (dr != null)
            //        {
            //            txtPnl.BindNoticeInfo(TypeHelper.ToString(dr["NM_DESC"]));    
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogUtils.Instance.LogException(ex);
            //}
            //finally
            //{
            //    SetControlDisable(false);
            //}
        }

        /// <summary>
        /// 확인 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;

            SetControlDisable(true);

            try
            {
                if (grd.RowCount > 0 && grd.SelectedRowIndex >= 0)
                {
                    DataRow dr = (DataRow)grd.GetSelectedRow().ItemData;

                    if (TypeHelper.ToString(dr["FLAG_YN"]) != "Y")
                    {
                        //공지사항 계산원별 저장
                        if (m_Presenter.SetNoticeSave(dr["DD_START"].ToString().Replace("-", ""), TypeHelper.ToInt32(dr["NO_SEQ"]), dr["DD_END"].ToString()))
                        {
                            dr["FLAG_YN"] = "Y";
                            grd.UpdateRow(grd.SelectedRowIndex, dr);
                            btnSave.Enabled = false;
                        }
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

        /// <summary>
        /// 공지사항 셋팅
        /// </summary>
        /// <param name="ds">공시사항 내역</param>
        public void SetNotice(DataSet ds)
        {
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    //그리드 메인 데이터 바인딩
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        grd.AddRow(dr);
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

                //그리드 RowIndex 설정
                if (grd.RowCount > 0)
                {
                    if (_strDD_START.Length > 0 && _strNO_SEQ.Length > 0)
                    {
                        for (int i = 0; i < grd.RowCount; i++)
                        {
                            DataRow dr = (DataRow)grd.RowItems[i];

                            if (dr["DD_START"].ToString() == _strDD_START && _strNO_SEQ == dr["NO_SEQ"].ToString())
                            {
                                grd.SelectedRowIndex = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        grd.SelectedRowIndex = 0;
                    }
                }

                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        txtPnl.Clear();
                    });
                }
                else
                {
                    txtPnl.Clear();
                }

                if (grd.RowCount > 0 && grd.SelectedRowIndex >= 0)
                {
                    DataRow dr = (DataRow)grd.GetSelectedRow().ItemData;

                    if (dr != null)
                    {
                        txtPnl.BindNoticeInfo(TypeHelper.ToString(dr["NM_DESC"]));
                    }
                }
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

                            if (!_bDisable)
                            {
                                if (btn.Name.ToString() == btnSave.Name.ToString() &&
                                    grd.RowCount > 0 && grd.SelectedRowIndex >= 0)
                                {
                                    DataRow dr = (DataRow)grd.GetSelectedRow().ItemData;

                                    if (dr != null && TypeHelper.ToString(dr["FLAG_YN"]) == "Y")
                                    {
                                        btnSave.Enabled = false;
                                    }
                                    else
                                    {
                                        btnSave.Enabled = true;
                                    }
                                }
                            }
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

                        if (!_bDisable)
                        {
                            if (btn.Name.ToString() == btnSave.Name.ToString() &&
                                grd.RowCount > 0 && grd.SelectedRowIndex >= 0)
                            {
                                DataRow dr = (DataRow)grd.GetSelectedRow().ItemData;

                                if (dr != null && TypeHelper.ToString(dr["FLAG_YN"]) == "Y")
                                {
                                    btnSave.Enabled = false;
                                }
                                else
                                {
                                    btnSave.Enabled = true;
                                }
                            }
                        }
                    }
                }
            }

            //Application.DoEvents();
        }

        #endregion
    }
}
