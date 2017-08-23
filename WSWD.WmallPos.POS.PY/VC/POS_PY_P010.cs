//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P010.cs
 * 화면설명 : 할인쿠폰
 * 개발자   : 정광호
 * 개발일자 : 2015.04.21
 * 수정     : TCL
 * 수정일자 : 2015.06.01
 * 수정내역 : 반품처리
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

using WSWD.WmallPos.POS.PY.PI;
using WSWD.WmallPos.POS.PY.PT;
using WSWD.WmallPos.POS.PY.VI;
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
using WSWD.WmallPos.FX.Shared.NetComm.Types;

namespace WSWD.WmallPos.POS.PY.VC
{
    public partial class POS_PY_P010 : PopupBase01, IPYP010View
    {
        #region 변수

        //할인쿠폰 비즈니스 로직
        private IPYP010presenter m_Presenter;

        /// <summary>
        /// 받을돈
        /// </summary>
        private int _iGetAmt = 0;

        /// <summary>
        /// 쿠폰적용 제한(true:복합브랜드, false:단일브랜드)
        /// </summary>
        private bool _bType = true;

        /// <summary>
        /// 반품여부
        /// </summary>
        private bool _modeReturn = false;

        /// <summary>
        /// 쿠폰 내역
        /// </summary>
        private DataRow _drRow = null;

        List<BasketItem> _BasketItems;

        List<BasketPay> _BasketPays;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        #endregion

        #region 생성자

        /// <summary>
        /// 할인쿠폰
        /// </summary>
        /// <param name="iGetAmt">받을돈</param>
        /// <param name="iGetAmt">쿠폰적용 제한(true:제한없음, false:단일브랜드)</param>
        /// <param name="modeReturn">반품여부</param>
        public POS_PY_P010(int iGetAmt, List<BasketItem> BasketItems, List<BasketPay> BasketPays, bool bType, bool modeReturn)
        {
            InitializeComponent();

            //받을돈
            _iGetAmt = iGetAmt;

            //쿠폰적용 제한(0:제한없음, 1:단일브랜드)
            _bType = bType;

            // 반품처리?
            _modeReturn = modeReturn;
            this.Text = this.Text + (modeReturn ? TITLE_CANCEL : string.Empty);

            _BasketItems = BasketItems;

            _BasketPays = BasketPays;

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
            this.KeyEvent += new OPOSKeyEventHandler(form_KeyEvent);                                                             //KeyEvent
            this.btnSave.Click += new EventHandler(btnSave_Click);                                                                      //적용 button Event
            this.btnClose.Click += new EventHandler(btnClose_Click);                                                                    //닫기 button Event
            grd.RowDataBound += new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanelRowDataBoundEventHandler(grd_RowDataBound);          //그리드 데이터 바인딩 Event
            grd.RowIndexChanged += new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanelRowIndexChangedEventHandler(grd_RowIndexChanged); //그리드 Row Index Change Event
            FormClosed += new FormClosedEventHandler(POS_PY_P010_FormClosed);
        }

        void POS_PY_P010_FormClosed(object sender, FormClosedEventArgs e)
        {
            Load -= new EventHandler(form_Load);
            this.KeyEvent -= new OPOSKeyEventHandler(form_KeyEvent);                                                             //KeyEvent
            this.btnSave.Click -= new EventHandler(btnSave_Click);                                                                      //적용 button Event
            this.btnClose.Click -= new EventHandler(btnClose_Click);                                                                    //닫기 button Event
            grd.RowDataBound -= new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanelRowDataBoundEventHandler(grd_RowDataBound);          //그리드 데이터 바인딩 Event
            grd.RowIndexChanged -= new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanelRowIndexChangedEventHandler(grd_RowIndexChanged); //그리드 Row Index Change Event
            FormClosed -= new FormClosedEventHandler(POS_PY_P010_FormClosed);
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

            //받을돈
            txtGetAmt.Text = _iGetAmt.ToString();

            //그리드 컬럼 설정
            grd.AddColumn("NO_COUPON", strMsg05, 140);
            grd.AddColumn("NM_COUPON", strMsg06);
            grd.AddColumn("FG_LIMIT", "", 0);               //쿠폰적용제한
            grd.AddColumn("DD_START_DELAY", "", 0);         //쿠폰적용시작일
            grd.AddColumn("DD_END_DELAY", "", 0);           //쿠폰적용종료일
            grd.AddColumn("AM_MAX", "", 0);                 //쿠폰적용판매금액
            grd.AddColumn("FG_DC", "", 0);                  //할인구분
            grd.AddColumn("AM_DC", strMsg07, 120);

            SetControlDisable(true);

            //쿠폰 정보 조회
            m_Presenter = new PYP010presenter(this);
            m_Presenter.GetSaleCoupon();
        }

        /// <summary>
        /// Key Event
        /// </summary>
        /// <param name="e"></param>
        void form_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (_bDisable || _drRow == null)
            {
                e.IsHandled = true;
                return;
            }

            if (e.Key.OPOSKey == OPOSMapKeys.KEY_NOSALE)
            {
                if (POSDeviceManager.CashDrawer != null && POSDeviceManager.CashDrawer.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened && POSDeviceManager.CashDrawer.Enabled)
                {
                    //돈통 open
                    POSDeviceManager.CashDrawer.OpenDrawer();
                }
                return;
            }

            bool bFG_LIMIT = _drRow["FG_LIMIT"].ToString() == "0" ? true : false;                                                           //쿠폰적용 제한(0:제한없음, 1:단일브랜드)
            int iAM_MAX = TypeHelper.ToInt32(_drRow["AM_MAX"]);                                                                             //쿠폰 적용 판매 금엑
            bool bFG_DC = _drRow["FG_DC"].ToString() == "0" ? true : false;                                                                 //할인구분 (0:%, 1:금액)
            int iAM_DC = TypeHelper.ToInt32(_drRow["AM_DC"] != null ? _drRow["AM_DC"].ToString().Replace(WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00173"), "").Replace("%", "") : "");  //쿠폰 할인금액

            if (_drRow != null)
            {
                if (bFG_DC)
                {
                    e.IsHandled = true;
                }
                else
                {
                    StatusMessage = strMsg01;

                    if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
                    {
                        if (txtPaymentCnt.Text.Length <= 0 && txtPaymentAmt.Text.Length <= 0)
                        {
                            e.IsHandled = true;
                            this.DialogResult = DialogResult.Cancel;
                            return;
                        }
                        else
                        {
                            txtPaymentAmt.Text = "";
                        }
                    }
                    else if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
                    {
                        if (TypeHelper.ToInt32(txtPaymentCnt.Text) > 0)
                        {
                            if (!bFG_DC)
                            {
                                if (TypeHelper.ToInt32(txtPaymentCnt.Text) * iAM_DC <= _iGetAmt)
                                {
                                    txtPaymentAmt.Text = string.Format("{0}", TypeHelper.ToInt32(txtPaymentCnt.Text) * iAM_DC);
                                    StatusMessage = strMsg08;
                                }
                                else
                                {
                                    txtPaymentCnt.Text = "";
                                    StatusMessage = strMsg02;
                                }
                            }
                        }
                        else
                        {
                            txtPaymentCnt.Text = "";
                        }
                    }
                    else if (e.Key.OPOSKey == OPOSMapKeys.KEY_BKS)
                    {
                        txtPaymentAmt.Text = "";
                    }
                    else if (!e.IsControlKey)
                    {
                        txtPaymentAmt.Text = "";
                    }
                }
            }
            else
            {
                e.IsHandled = true;

                if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
                {
                    this.DialogResult = DialogResult.Cancel;
                }
            }
        }

        /// <summary>
        /// 그리드 데이터 바인딩
        /// </summary>
        /// <param name="row"></param>
        void grd_RowDataBound(WSWD.WmallPos.POS.FX.Win.UserControls.GridRow row)
        {
            int iRow = 0;

            if (row.RowState == GridRowState.Added)
            {
                //쿠폰코드
                row.Cells[iRow].Controls.Add(new Label()
                {
                    Text = ((System.Data.DataRow)(row.ItemData)).ItemArray[iRow].ToString(),
                    AutoSize = false,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                });
                iRow++;
                //쿠폰종류
                row.Cells[iRow].Controls.Add(new Label()
                {
                    Text = ((System.Data.DataRow)(row.ItemData)).ItemArray[iRow].ToString(),
                    AutoSize = false,
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill
                });
                iRow++;
                //쿠폰적용제한
                row.Cells[iRow].Controls.Add(new Label()
                {
                    Text = ((System.Data.DataRow)(row.ItemData)).ItemArray[iRow].ToString(),
                    AutoSize = false,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                });
                iRow++;
                //쿠폰적용시작일
                row.Cells[iRow].Controls.Add(new Label()
                {
                    Text = ((System.Data.DataRow)(row.ItemData)).ItemArray[iRow].ToString(),
                    AutoSize = false,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                });
                iRow++;
                //쿠폰적용종료일
                row.Cells[iRow].Controls.Add(new Label()
                {
                    Text = ((System.Data.DataRow)(row.ItemData)).ItemArray[iRow].ToString(),
                    AutoSize = false,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                });
                iRow++;
                //쿠폰적용판매금액
                row.Cells[iRow].Controls.Add(new Label()
                {
                    Text = ((System.Data.DataRow)(row.ItemData)).ItemArray[iRow].ToString(),
                    AutoSize = false,
                    TextAlign = System.Drawing.ContentAlignment.MiddleRight,
                    Dock = DockStyle.Fill
                });
                iRow++;
                //할인구분
                row.Cells[iRow].Controls.Add(new Label()
                {
                    Text = ((System.Data.DataRow)(row.ItemData)).ItemArray[iRow].ToString(),
                    AutoSize = false,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                });
                iRow++;

                string val = ((System.Data.DataRow)(row.ItemData)).ItemArray[iRow].ToString();
                int amt = int.Parse(val.Substring(0, val.Length - 1));
                string gb = ((System.Data.DataRow)(row.ItemData)).ItemArray[6].ToString();

                //할인금액
                row.Cells[iRow].Controls.Add(new Label()
                {
                    Text = string.Format("{0:#,##0}{1}",
                     amt, gb.Equals("1") ? "원" : "%"),
                    AutoSize = false,
                    TextAlign = System.Drawing.ContentAlignment.MiddleRight,
                    Dock = DockStyle.Fill
                });
            }
        }

        /// <summary>
        /// 그리드 Row Index Change Event
        /// </summary>
        /// <param name="beforeRow"></param>
        /// <param name="afterRow"></param>
        void grd_RowIndexChanged(WSWD.WmallPos.POS.FX.Win.UserControls.GridRow beforeRow, WSWD.WmallPos.POS.FX.Win.UserControls.GridRow afterRow)
        {
            _drRow = null;

            if (grd.CurrentRowIndex >= 0 && grd.RowCount > 0)
            {
                _drRow = (System.Data.DataRow)(grd.GetRow(grd.CurrentRowIndex).ItemData);
            }

            //컨트롤 초기화
            InitControl();
        }

        /// <summary>
        /// 적용 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;

            if (grd.RowCount <= 0 || grd.CurrentRowIndex < 0 || _drRow == null) return;

            try
            {
                bool bFG_LIMIT = _drRow["FG_LIMIT"].ToString() == "0" ? true : false;                                                           //쿠폰적용 제한(0:제한없음, 1:단일브랜드)
                int iAM_MAX = TypeHelper.ToInt32(_drRow["AM_MAX"]);                                                                             //쿠폰 적용 판매 금엑
                /// = true: 금액으로 계산; false = Percentage
                /// FG_DC = 1, 금액
                bool bFG_DC = _drRow["FG_DC"].ToString() == "1" ? true : false;                                                                 //할인구분 (0:%, 1:금액)
                int iAM_DC = TypeHelper.ToInt32(_drRow["AM_DC"] != null ? _drRow["AM_DC"].ToString().Replace(WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00173"), "").Replace("%", "") : "");  //쿠폰 할인금액

                if (bFG_DC)
                {
                    if (TypeHelper.ToInt32(txtPaymentCnt.Text) <= 0)
                    {
                        StatusMessage = strMsg01;
                        return;
                    }
                }

                if (TypeHelper.ToInt32(txtPaymentAmt.Text) <= 0)
                {
                    StatusMessage = strMsg11;
                    return;
                }

                bool bCdClass = false;  //true:복합 false:단일
                string strTemp = string.Empty;
                foreach (BasketItem bp in _BasketItems)
                {
                    if (bp.FgCanc == "0")
                    {
                        if (strTemp.Length <= 0)
                        {
                            strTemp = bp.CdClass;
                        }
                        else
                        {
                            if (strTemp != bp.CdClass)
                            {
                                bCdClass = true;
                            }
                        }
                    }
                }

                if (bCdClass)
                {
                    if (!bFG_LIMIT)
                    {
                        StatusMessage = strMsg10;
                        return;
                    }
                }

                if (iAM_MAX > _iGetAmt)
                {
                    StatusMessage = string.Format(strMsg09, iAM_MAX);
                    return;
                }

                if (_iGetAmt < TypeHelper.ToInt32(txtPaymentAmt.Text))
                {
                    //컨트롤 초기화
                    StatusMessage = strMsg02;
                    return;
                }

                #region Loc added 11.13 - 주석처리

                
                // 금액할인쿠폰
                if (bFG_DC)
                {
                    // Loc added 11.13
                    // 갯수 확인
                    int maxCount = _iGetAmt / iAM_MAX;

                    if (TypeHelper.ToInt32(txtPaymentCnt.Text) > maxCount)
                    {
                        StatusMessage = strMsg12;
                        return;
                    }
                }

                #endregion

                // 할인쿠폰 TRAN                    
                BasketCoupon basketCoupon = new BasketCoupon();
                basketCoupon.BasketType = BasketTypes.BasketPay;
                basketCoupon.PayGrpCd = NetCommConstants.PAYMENT_GROUP_COUPON;
                basketCoupon.PayDtlCd = NetCommConstants.PAYMENT_DETAIL_COUPON;
                basketCoupon.PayAmt = txtPaymentAmt.Text;
                basketCoupon.BalGrpCd = NetCommConstants.PAYMENT_GROUP_NONE;
                basketCoupon.BalDtlCd = NetCommConstants.PAYMENT_DETAIL_NONE;
                basketCoupon.CancFg = NetCommConstants.CANCEL_TYPE_NORMAL;
                basketCoupon.CouponCd = ((System.Data.DataRow)(grd.GetRow(grd.CurrentRowIndex).ItemData)).ItemArray[0].ToString();
                basketCoupon.CouponNm = ((System.Data.DataRow)(grd.GetRow(grd.CurrentRowIndex).ItemData)).ItemArray[1].ToString();
                basketCoupon.CouponCnt = !bFG_DC ? "1" : txtPaymentCnt.Text.ToString();

                this.ReturnResult.Add("PAY_DATA", basketCoupon);
                this.DialogResult = DialogResult.OK;
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
        /// 쿠폰정보 그리드 조회값
        /// </summary>
        /// <param name="ds"></param>
        public void SetSaleCoupon(DataSet ds)
        {
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    //쿠폰 그리드 바인딩
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        grd.AddRow(dr);
                    }

                    //메세지바 설정
                    StatusMessage = strMsg04;

                    //그리드 RowIndex 설정
                    if (grd.RowCount > 0)
                    {
                        grd.CurrentRowIndex = 0;
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                SetControlDisable(false);
            }

        }

        /// <summary>
        /// 컨트롤 초기화
        /// </summary>
        private void InitControl()
        {
            if (_drRow != null)
            {
                string strFG_LIMIT = "";    //쿠폰적용 제한(0:제한없음, 1:단일브랜드)
                int iAM_MAX = 0;            //쿠폰 적용 판매 금엑
                string strFG_DC = "";       //할인구분 (0:%, 1:금액)
                int iAM_DC = 0;             //쿠폰 할인금액

                strFG_LIMIT = _drRow["FG_LIMIT"].ToString();
                iAM_MAX = TypeHelper.ToInt32(_drRow["AM_MAX"]);
                strFG_DC = _drRow["FG_DC"].ToString();
                iAM_DC = TypeHelper.ToInt32(_drRow["AM_DC"] != null ? _drRow["AM_DC"].ToString().Replace(WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00173"), "").Replace("%", "") : "");

                if (TypeHelper.ToInt32(txtGetAmt.Text) >= 0)
                {
                    if (strFG_DC == "0")
                    {
                        txtPaymentCnt.ReadOnly = true;
                        txtPaymentCnt.Text = "";
                        txtPaymentAmt.Text = iAM_DC > 0 ? (_iGetAmt * iAM_DC / 100).ToString() : "";
                        StatusMessage = strMsg08;
                    }
                    else
                    {
                        // 금액으로
                        txtPaymentCnt.ReadOnly = false;
                        txtPaymentCnt.Text = "1";
                        txtPaymentAmt.Text = iAM_DC.ToString();
                        StatusMessage = strMsg01;
                    }
                }
                else
                {
                    txtPaymentCnt.ReadOnly = true;
                    txtPaymentCnt.Text = "";
                    txtPaymentAmt.Text = "";
                    StatusMessage = strMsg08;
                }
            }
            else
            {
                txtPaymentCnt.ReadOnly = true;
                txtPaymentCnt.Text = "";
                txtPaymentAmt.Text = "";
                StatusMessage = strMsg04;
            }
        }

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable/Disable
        /// </summary>
        void SetControlDisable(bool bDisable)
        {
            ChildManager.ShowProgress(bDisable);
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