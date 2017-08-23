//-----------------------------------------------------------------
/*
 * 화면명   : POS_TM_M003.cs
 * 화면설명 : 중간입금 설정
 * 개발자   : 정광호
 * 개발일자 : 2015.03.24
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

using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.FX.Shared.NetComm;

namespace WSWD.WmallPos.POS.TM.VC
{
    public partial class POS_TM_M003 : FormBase
    {
        #region 변수

        /// <summary>
        /// 신규 또는 수정여부(true : 신규, false : 수정)
        /// </summary>
        private bool _bNew = false;

        /// <summary>
        /// 현재 수정회차
        /// </summary>
        private decimal _dTotalCnt = 0;

        /// <summary>
        /// 프린터 발생시간
        /// </summary>
        private string _strDate = string.Empty;
        public string strDate
        {
            set
            {
                _strDate = value;
            }
        }

        /// <summary>
        /// 프린터 입금회차
        /// </summary>
        private string _strCT_ITEM = string.Empty;
        public string strCT_ITEM
        {
            set
            {
                _strCT_ITEM = value;
            }
        }

        /// <summary>
        /// 거래번호
        /// </summary>
        private string _strTrxnNo = string.Empty;
        public string strTrxnNo
        {
            set
            {
                _strTrxnNo = value;
            }
        }

        /// <summary>
        /// 컨트롤 value 값 임시 저장
        /// </summary>
        DataSet dsControl = null;
        #endregion

        #region 생성자

        /// <summary>
        /// POS_TM_M003 생성자
        /// </summary>
        public POS_TM_M003()
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
            this.wbSave.Click += new EventHandler(wbSave_Click);                                        //등록 및 등록완료 Event
            this.wbClose.Click += new EventHandler(wbClose_Click);                                      //닫기 button Event
            this.KeyEvent += new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(POS_TM_M003_KeyEvent);     //Key Event
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

            #region 컨트롤 value 값 임시 저장 DataSet
            dsControl = new DataSet();

            DataTable dtCash = new DataTable("cash");
            DataTable dtTicket = new DataTable("ticket");

            dtCash.Columns.Add("cash_cnt01");
            dtCash.Columns.Add("cash_cnt02");
            dtCash.Columns.Add("cash_cnt03");
            dtCash.Columns.Add("cash_cnt04");
            dtCash.Columns.Add("cash_cnt05");
            dtCash.Columns.Add("cash_cnt06");
            dtCash.Columns.Add("cash_cnt07");
            dtCash.Columns.Add("cash_cnt08");
            dtCash.Columns.Add("cash_cnt09");

            dtCash.Columns.Add("cash_amt01");
            dtCash.Columns.Add("cash_amt02");
            dtCash.Columns.Add("cash_amt03");
            dtCash.Columns.Add("cash_amt04");
            dtCash.Columns.Add("cash_amt05");
            dtCash.Columns.Add("cash_amt06");
            dtCash.Columns.Add("cash_amt07");
            dtCash.Columns.Add("cash_amt08");
            dtCash.Columns.Add("cash_amt09");

            dtTicket.Columns.Add("ticket_cnt01");
            dtTicket.Columns.Add("ticket_cnt02");
            dtTicket.Columns.Add("ticket_cnt03");
            dtTicket.Columns.Add("ticket_cnt04");
            dtTicket.Columns.Add("ticket_cnt05");
            dtTicket.Columns.Add("ticket_cnt06");
            dtTicket.Columns.Add("ticket_cnt07");
            dtTicket.Columns.Add("ticket_cnt08");
            dtTicket.Columns.Add("ticket_cnt09");

            dtTicket.Columns.Add("ticket_amt01");
            dtTicket.Columns.Add("ticket_amt02");
            dtTicket.Columns.Add("ticket_amt03");
            dtTicket.Columns.Add("ticket_amt04");
            dtTicket.Columns.Add("ticket_amt05");
            dtTicket.Columns.Add("ticket_amt06");
            dtTicket.Columns.Add("ticket_amt07");
            dtTicket.Columns.Add("ticket_amt08");
            dtTicket.Columns.Add("ticket_amt09");

            dtCash.Rows.Add(new object[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" });
            dtCash.Rows.Add(new object[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" });
            dtCash.Rows.Add(new object[] { "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y" });
            dtCash.Rows.Add(new object[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" });
            dtTicket.Rows.Add(new object[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" });
            dtTicket.Rows.Add(new object[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" });
            dtTicket.Rows.Add(new object[] { "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y" });
            dtTicket.Rows.Add(new object[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" });

            dsControl.Tables.Add(dtCash);
            dsControl.Tables.Add(dtTicket);
            #endregion

            //수정회차 포커스
            txtCnt.SetFocus();

            //돈통 open
            //POSDeviceManager.CashDrawer.OpenDrawer();
        }

        /// <summary>
        /// Key Event
        /// </summary>
        /// <param name="e"></param>
        void POS_TM_M003_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            decimal dTemp = 0;
            decimal dNewCnt = 0;
            decimal dTempTotalCnt = 0;
            DataRow dr = null;

            if (e.Key.OPOSKey == OPOSMapKeys.KEY_NOSALE)
            {
                POSDeviceManager.CashDrawer.OpenDrawer();
            }
            else if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_ENTER)
            {
                //등록
                #region KEY_ENTER

                #region 포커스 확인 및 값 셋팅

                if (txtCashAmt01.IsFocused)
                {
                    SetRecover(txtCashAmt01);

                    if (SetControlAmt("c", txtCashCnt01, txtCashAmt01, 0))
                    {
                        txtCashCnt02.SetFocus();
                    }
                }
                else if (txtCashCnt01.IsFocused)
                {
                    SetRecover(txtCashCnt01);

                    txtCashAmt01.SetFocus();
                }
                else if (txtCashCnt02.IsFocused)
                {
                    SetRecover(txtCashCnt02);

                    if (SetControlAmt("c", txtCashCnt02, txtCashAmt02, 50000))
                    {
                        txtCashCnt03.SetFocus();
                    }
                }
                else if (txtCashCnt03.IsFocused)
                {
                    SetRecover(txtCashCnt03);

                    if (SetControlAmt("c", txtCashCnt03, txtCashAmt03, 10000))
                    {
                        txtCashCnt04.SetFocus();
                    }
                }
                else if (txtCashCnt04.IsFocused)
                {
                    SetRecover(txtCashCnt04);

                    if (SetControlAmt("c", txtCashCnt04, txtCashAmt04, 5000))
                    {
                        txtCashCnt05.SetFocus();
                    }
                }
                else if (txtCashCnt05.IsFocused)
                {
                    SetRecover(txtCashCnt05);

                    if (SetControlAmt("c", txtCashCnt05, txtCashAmt05, 1000))
                    {
                        txtCashCnt06.SetFocus();
                    }
                }
                else if (txtCashCnt06.IsFocused)
                {
                    SetRecover(txtCashCnt06);

                    if (SetControlAmt("c", txtCashCnt06, txtCashAmt06, 500))
                    {
                        txtCashCnt07.SetFocus();
                    }
                }
                else if (txtCashCnt07.IsFocused)
                {
                    SetRecover(txtCashCnt07);

                    if (SetControlAmt("c", txtCashCnt07, txtCashAmt07, 100))
                    {
                        txtCashCnt08.SetFocus();
                    }
                }
                else if (txtCashCnt08.IsFocused)
                {
                    SetRecover(txtCashCnt08);

                    if (SetControlAmt("c", txtCashCnt08, txtCashAmt08, 50))
                    {
                        txtCashCnt09.SetFocus();
                    }
                }
                else if (txtCashCnt09.IsFocused)
                {
                    SetRecover(txtCashCnt09);

                    if (SetControlAmt("c", txtCashCnt09, txtCashAmt09, 10))
                    {
                        txtTicketCnt01.SetFocus();
                    }
                }
                else if (txtTicketCnt01.IsFocused)
                {
                    SetRecover(txtTicketCnt01);
                    txtTicketAmt01.SetFocus();
                }
                else if (txtTicketCnt02.IsFocused)
                {
                    SetRecover(txtTicketCnt02);
                    txtTicketAmt02.SetFocus();
                }
                else if (txtTicketCnt03.IsFocused)
                {
                    SetRecover(txtTicketCnt03);
                    txtTicketAmt03.SetFocus();
                }
                else if (txtTicketCnt04.IsFocused)
                {
                    SetRecover(txtTicketCnt04);
                    txtTicketAmt04.SetFocus();
                }
                else if (txtTicketCnt05.IsFocused)
                {
                    SetRecover(txtTicketCnt05);
                    txtTicketAmt05.SetFocus();
                }
                else if (txtTicketCnt06.IsFocused)
                {
                    SetRecover(txtTicketCnt06);
                    txtTicketAmt06.SetFocus();
                }
                else if (txtTicketCnt07.IsFocused)
                {
                    SetRecover(txtTicketCnt07);
                    txtTicketAmt07.SetFocus();
                }
                else if (txtTicketCnt08.IsFocused)
                {
                    SetRecover(txtTicketCnt08);
                    txtTicketAmt08.SetFocus();
                }
                else if (txtTicketCnt09.IsFocused)
                {
                    SetRecover(txtTicketCnt09);
                    txtTicketAmt09.SetFocus();
                }
                else if (txtTicketAmt01.IsFocused)
                {
                    SetRecover(txtTicketAmt01);

                    if (SetControlAmt("t", txtTicketCnt01, txtTicketAmt01, 0))
                    {
                        SetFocus(txtTicketAmt01);
                    }
                }
                else if (txtTicketAmt02.IsFocused)
                {
                    SetRecover(txtTicketAmt02);

                    if (SetControlAmt("t", txtTicketCnt02, txtTicketAmt02, 0))
                    {
                        SetFocus(txtTicketAmt02);
                    }
                }
                else if (txtTicketAmt03.IsFocused)
                {
                    SetRecover(txtTicketAmt03);

                    if (SetControlAmt("t", txtTicketCnt03, txtTicketAmt03, 0))
                    {
                        SetFocus(txtTicketAmt03);
                    }
                }
                else if (txtTicketAmt04.IsFocused)
                {
                    SetRecover(txtTicketAmt04);

                    if (SetControlAmt("t", txtTicketCnt04, txtTicketAmt04, 0))
                    {
                        SetFocus(txtTicketAmt04);
                    }
                }
                else if (txtTicketAmt05.IsFocused)
                {
                    SetRecover(txtTicketAmt05);

                    if (SetControlAmt("t", txtTicketCnt05, txtTicketAmt05, 0))
                    {
                        SetFocus(txtTicketAmt05);
                    }
                }
                else if (txtTicketAmt06.IsFocused)
                {
                    SetRecover(txtTicketAmt06);

                    if (SetControlAmt("t", txtTicketCnt06, txtTicketAmt06, 0))
                    {
                        SetFocus(txtTicketAmt06);
                    }
                }
                else if (txtTicketAmt07.IsFocused)
                {
                    SetRecover(txtTicketAmt07);

                    if (SetControlAmt("t", txtTicketCnt07, txtTicketAmt07, 0))
                    {
                        SetFocus(txtTicketAmt07);
                    }
                }
                else if (txtTicketAmt08.IsFocused)
                {
                    SetRecover(txtTicketAmt08);

                    if (SetControlAmt("t", txtTicketCnt08, txtTicketAmt08, 0))
                    {
                        SetFocus(txtTicketAmt08);
                    }
                }
                else if (txtTicketAmt09.IsFocused)
                {
                    SetRecover(txtTicketAmt09);

                    if (SetControlAmt("t", txtTicketCnt09, txtTicketAmt09, 0))
                    {
                        SetFocus(txtTicketAmt09);
                    }
                }

                #endregion

                #region
                else
                {
                    if (txtCnt.Text.Trim().Length <= 0)
                    {
                        //신규 중간입금 등록

                        dNewCnt = POSCommon.Instance.GetCLabel(lblMidCnt) > 0 ? POSCommon.Instance.GetCLabel(lblMidCnt) + 1 : 1;
                        if (dNewCnt <= 5)
                        {
                            if (ShowMessageBox(MessageDialogType.YesNoCancel, "", SetMsg(dNewCnt, "추가")) == DialogResult.Yes)
                            {
                                _bNew = true;
                                _dTotalCnt = dNewCnt;

                                //컨트롤 초기화
                                SetInitControl();
                            }
                            else
                            {
                                txtCnt.Text = "";
                                txtCnt.SetFocus();
                            }
                        }
                        else
                        {
                            //오류메시지박스
                            ShowMessageBox(WSWD.WmallPos.FX.Shared.MessageDialogType.Information, "IN00005", null);
                        }
                    }
                    else
                    {
                        dTemp = POSCommon.Instance.GetInputControl(txtCnt);
                        dTempTotalCnt = POSCommon.Instance.GetCLabel(lblMidCnt);

                        if (dTemp > 0 && dTemp <= 5)
                        {
                            if (dTempTotalCnt <= 0)
                            {
                                txtCnt.Text = "";
                                ShowMessageBox(WSWD.WmallPos.FX.Shared.MessageDialogType.Information, "IN00004", null);
                                e.IsHandled = true;
                            }
                            else
                            {
                                if (dTemp > dTempTotalCnt)
                                {
                                    txtCnt.Text = "";
                                    ShowMessageBox(WSWD.WmallPos.FX.Shared.MessageDialogType.Information, "IN00004", null);
                                    e.IsHandled = true;
                                }
                                else
                                {
                                    if (ShowMessageBox(MessageDialogType.YesNoCancel, "", SetMsg(dTemp, "수정")) == DialogResult.Yes)
                                    {
                                        _bNew = false;
                                        _dTotalCnt = dTemp;

                                        //금액 컨트롤 초기화
                                        SetInitControl();

                                        //
                                        SetViewTotal();
                                    }
                                    else
                                    {
                                        txtCnt.Text = "";
                                        txtCnt.SetFocus();
                                    }
                                }
                            }
                        }
                        else
                        {
                            txtCnt.Text = "";
                            ShowMessageBox(WSWD.WmallPos.FX.Shared.MessageDialogType.Information, "IN00004", null);
                            e.IsHandled = true;
                        }
                    }
                }
                #endregion

                #endregion
            }
            else if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_CLEAR)
            {
                #region 현금 및 상품권 유효성 검사

                #region 현금

                if (txtCashAmt01.IsFocused)
                {
                    SetClearRecover(e, txtCashAmt01, txtCashCnt01);
                }
                else if (txtCashCnt01.IsFocused)
                {
                    if (dsControl.Tables["cash"].Rows[0]["cash_cnt01"] != null)
                    {
                        if (txtCashCnt01.Text.ToString() != dsControl.Tables["cash"].Rows[0]["cash_cnt01"].ToString())
                        {
                            txtCashCnt01.Text = dsControl.Tables["cash"].Rows[0]["cash_cnt01"].ToString();
                            e.IsHandled = true;
                        }
                    }
                }
                else if (txtCashCnt02.IsFocused)
                {
                    SetClearRecover(e, txtCashCnt02, txtCashAmt01);
                }
                else if (txtCashCnt03.IsFocused)
                {
                    SetClearRecover(e, txtCashCnt03, txtCashCnt02);
                }
                else if (txtCashCnt04.IsFocused)
                {
                    SetClearRecover(e, txtCashCnt04, txtCashCnt03);
                }
                else if (txtCashCnt05.IsFocused)
                {
                    SetClearRecover(e, txtCashCnt05, txtCashCnt04);
                }
                else if (txtCashCnt06.IsFocused)
                {
                    SetClearRecover(e, txtCashCnt06, txtCashCnt05);
                }
                else if (txtCashCnt07.IsFocused)
                {
                    SetClearRecover(e, txtCashCnt07, txtCashCnt06);
                }
                else if (txtCashCnt08.IsFocused)
                {
                    SetClearRecover(e, txtCashCnt08, txtCashCnt07);
                }
                else if (txtCashCnt09.IsFocused)
                {
                    SetClearRecover(e, txtCashCnt09, txtCashCnt08);
                }                

                #endregion

                #region 상품권

                else if (txtTicketCnt01.IsFocused)
                {
                    SetClearRecover(e, txtTicketCnt01, txtCashCnt09);
                }
                else if (txtTicketCnt02.IsFocused)
                {
                    SetClearRecover(e, txtTicketCnt02, txtTicketAmt01);
                }
                else if (txtTicketCnt03.IsFocused)
                {
                    SetClearRecover(e, txtTicketCnt03, txtTicketAmt02);
                }
                else if (txtTicketCnt04.IsFocused)
                {
                    SetClearRecover(e, txtTicketCnt04, txtTicketAmt03);
                }
                else if (txtTicketCnt05.IsFocused)
                {
                    SetClearRecover(e, txtTicketCnt05, txtTicketAmt04);
                }
                else if (txtTicketCnt06.IsFocused)
                {
                    SetClearRecover(e, txtTicketCnt06, txtTicketAmt05);
                }
                else if (txtTicketCnt07.IsFocused)
                {
                    SetClearRecover(e, txtTicketCnt07, txtTicketAmt06);
                }
                else if (txtTicketCnt08.IsFocused)
                {
                    SetClearRecover(e, txtTicketCnt08, txtTicketAmt07);
                }
                else if (txtTicketCnt09.IsFocused)
                {
                    SetClearRecover(e, txtTicketCnt09, txtTicketAmt08);
                }
                else if (txtTicketAmt01.IsFocused)
                {
                    SetClearRecover(e, txtTicketAmt01, txtTicketCnt01);
                }
                else if (txtTicketAmt02.IsFocused)
                {
                    SetClearRecover(e, txtTicketAmt02, txtTicketCnt02);
                }
                else if (txtTicketAmt03.IsFocused)
                {
                    SetClearRecover(e, txtTicketAmt03, txtTicketCnt03);
                }
                else if (txtTicketAmt04.IsFocused)
                {
                    SetClearRecover(e, txtTicketAmt04, txtTicketCnt04);
                }
                else if (txtTicketAmt05.IsFocused)
                {
                    SetClearRecover(e, txtTicketAmt05, txtTicketCnt05);
                }
                else if (txtTicketAmt06.IsFocused)
                {
                    SetClearRecover(e, txtTicketAmt06, txtTicketCnt06);
                }
                else if (txtTicketAmt07.IsFocused)
                {
                    SetClearRecover(e, txtTicketAmt07, txtTicketCnt07);
                }
                else if (txtTicketAmt08.IsFocused)
                {
                    SetClearRecover(e, txtTicketAmt08, txtTicketCnt08);
                }
                else if (txtTicketAmt09.IsFocused)
                {
                    SetClearRecover(e, txtTicketAmt09, txtTicketCnt09);
                }

                #endregion

                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_BKS)
            {
                #region 현금 및 상품권 유효성 검사

                #region 현금

                if (txtCashAmt01.IsFocused)
                {
                    SetBackSpaveRecover(e, txtCashAmt01);
                }
                else if (txtCashCnt01.IsFocused)
                {
                    if (dsControl.Tables["cash"].Rows[0]["cash_cnt01"] != null)
                    {
                        if (txtCashCnt01.Text.ToString() != dsControl.Tables["cash"].Rows[0]["cash_cnt01"].ToString())
                        {
                            txtCashCnt01.Text = dsControl.Tables["cash"].Rows[0]["cash_cnt01"].ToString();
                            e.IsHandled = true;
                        }
                    }
                }
                else if (txtCashCnt02.IsFocused)
                {
                    SetBackSpaveRecover(e, txtCashCnt02);
                }
                else if (txtCashCnt03.IsFocused)
                {
                    SetBackSpaveRecover(e, txtCashCnt03);
                }
                else if (txtCashCnt04.IsFocused)
                {
                    SetBackSpaveRecover(e, txtCashCnt04);
                }
                else if (txtCashCnt05.IsFocused)
                {
                    SetBackSpaveRecover(e, txtCashCnt05);
                }
                else if (txtCashCnt06.IsFocused)
                {
                    SetBackSpaveRecover(e, txtCashCnt06);
                }
                else if (txtCashCnt07.IsFocused)
                {
                    SetBackSpaveRecover(e, txtCashCnt07);
                }
                else if (txtCashCnt08.IsFocused)
                {
                    SetBackSpaveRecover(e, txtCashCnt08);
                }
                else if (txtCashCnt09.IsFocused)
                {
                    SetBackSpaveRecover(e, txtCashCnt09);
                }

                #endregion

                #region 상품권

                else if (txtTicketCnt01.IsFocused)
                {
                    SetBackSpaveRecover(e, txtTicketCnt01);
                }
                else if (txtTicketCnt02.IsFocused)
                {
                    SetBackSpaveRecover(e, txtTicketCnt02);
                }
                else if (txtTicketCnt03.IsFocused)
                {
                    SetBackSpaveRecover(e, txtTicketCnt03);
                }
                else if (txtTicketCnt04.IsFocused)
                {
                    SetBackSpaveRecover(e, txtTicketCnt04);
                }
                else if (txtTicketCnt05.IsFocused)
                {
                    SetBackSpaveRecover(e, txtTicketCnt05);
                }
                else if (txtTicketCnt06.IsFocused)
                {
                    SetBackSpaveRecover(e, txtTicketCnt06);
                }
                else if (txtTicketCnt07.IsFocused)
                {
                    SetBackSpaveRecover(e, txtTicketCnt07);
                }
                else if (txtTicketCnt08.IsFocused)
                {
                    SetBackSpaveRecover(e, txtTicketCnt08);
                }
                else if (txtTicketCnt09.IsFocused)
                {
                    SetBackSpaveRecover(e, txtTicketCnt09);
                }
                else if (txtTicketAmt01.IsFocused)
                {
                    SetBackSpaveRecover(e, txtTicketAmt01);
                }
                else if (txtTicketAmt02.IsFocused)
                {
                    SetBackSpaveRecover(e, txtTicketAmt02);
                }
                else if (txtTicketAmt03.IsFocused)
                {
                    SetBackSpaveRecover(e, txtTicketAmt03);
                }
                else if (txtTicketAmt04.IsFocused)
                {
                    SetBackSpaveRecover(e, txtTicketAmt04);
                }
                else if (txtTicketAmt05.IsFocused)
                {
                    SetBackSpaveRecover(e, txtTicketAmt05);
                }
                else if (txtTicketAmt06.IsFocused)
                {
                    SetBackSpaveRecover(e, txtTicketAmt06);
                }
                else if (txtTicketAmt07.IsFocused)
                {
                    SetBackSpaveRecover(e, txtTicketAmt07);
                }
                else if (txtTicketAmt08.IsFocused)
                {
                    SetBackSpaveRecover(e, txtTicketAmt08);
                }
                else if (txtTicketAmt09.IsFocused)
                {
                    SetBackSpaveRecover(e, txtTicketAmt09);
                }

                #endregion

                #endregion
            }
            else if (!e.IsControlKey)
            {
                if (txtCnt.IsFocused)
                {
                    if (txtCnt.Text.Trim().Length > 1)
                    {
                        txtCnt.Text = txtCnt.Text.Substring(0, 1);
                        e.IsHandled = true;
                    }
                }
                else
                {
                    #region 현금 및 상품권 유효성 검사

                    #region 현금

                    if (txtCashCnt01.IsFocused)
                    {
                        ValidationLength(e, 3, txtCashCnt01, "cash", "cash_cnt01");
                    }
                    else if (txtCashCnt02.IsFocused)
                    {
                        ValidationLength(e, 3, txtCashCnt02, "cash", "cash_cnt02");
                    }
                    else if (txtCashCnt03.IsFocused)
                    {
                        ValidationLength(e, 3, txtCashCnt03, "cash", "cash_cnt03");
                    }
                    else if (txtCashCnt04.IsFocused)
                    {
                        ValidationLength(e, 3, txtCashCnt04, "cash", "cash_cnt04");
                    }
                    else if (txtCashCnt05.IsFocused)
                    {
                        ValidationLength(e, 3, txtCashCnt05, "cash", "cash_cnt05");
                    }
                    else if (txtCashCnt06.IsFocused)
                    {
                        ValidationLength(e, 3, txtCashCnt06, "cash", "cash_cnt06");
                    }
                    else if (txtCashCnt07.IsFocused)
                    {
                        ValidationLength(e, 3, txtCashCnt07, "cash", "cash_cnt07");
                    }
                    else if (txtCashCnt08.IsFocused)
                    {
                        ValidationLength(e, 3, txtCashCnt08, "cash", "cash_cnt08");
                    }
                    else if (txtCashCnt09.IsFocused)
                    {
                        ValidationLength(e, 3, txtCashCnt09, "cash", "cash_cnt09");
                    }
                    else if (txtCashAmt01.IsFocused)
                    {
                        ValidationLength(e, 9, txtCashAmt01, "cash", "cash_amt01");
                    }

                    #endregion

                    #region 상품권

                    else if (txtTicketCnt01.IsFocused)
                    {
                        ValidationLength(e, 3, txtTicketCnt01, "ticket", "ticket_cnt01");
                    }
                    else if (txtTicketCnt02.IsFocused)
                    {
                        ValidationLength(e, 3, txtTicketCnt02, "ticket", "ticket_cnt02");
                    }
                    else if (txtTicketCnt03.IsFocused)
                    {
                        ValidationLength(e, 3, txtTicketCnt03, "ticket", "ticket_cnt03");
                    }
                    else if (txtTicketCnt04.IsFocused)
                    {
                        ValidationLength(e, 3, txtTicketCnt04, "ticket", "ticket_cnt04");
                    }
                    else if (txtTicketCnt05.IsFocused)
                    {
                        ValidationLength(e, 3, txtTicketCnt05, "ticket", "ticket_cnt05");
                    }
                    else if (txtTicketCnt06.IsFocused)
                    {
                        ValidationLength(e, 3, txtTicketCnt06, "ticket", "ticket_cnt06");
                    }
                    else if (txtTicketCnt07.IsFocused)
                    {
                        ValidationLength(e, 3, txtTicketCnt07, "ticket", "ticket_cnt07");
                    }
                    else if (txtTicketCnt08.IsFocused)
                    {
                        ValidationLength(e, 3, txtTicketCnt08, "ticket", "ticket_cnt08");
                    }
                    else if (txtTicketCnt09.IsFocused)
                    {
                        ValidationLength(e, 3, txtTicketCnt09, "ticket", "ticket_cnt09");
                    }
                    else if (txtTicketAmt01.IsFocused)
                    {
                        ValidationLength(e, 9, txtTicketAmt01, "ticket", "ticket_amt01");
                    }
                    else if (txtTicketAmt02.IsFocused)
                    {
                        ValidationLength(e, 9, txtTicketAmt02, "ticket", "ticket_amt02");
                    }
                    else if (txtTicketAmt03.IsFocused)
                    {
                        ValidationLength(e, 9, txtTicketAmt03, "ticket", "ticket_amt03");
                    }
                    else if (txtTicketAmt04.IsFocused)
                    {
                        ValidationLength(e, 9, txtTicketAmt04, "ticket", "ticket_amt04");
                    }
                    else if (txtTicketAmt05.IsFocused)
                    {
                        ValidationLength(e, 9, txtTicketAmt05, "ticket", "ticket_amt05");
                    }
                    else if (txtTicketAmt06.IsFocused)
                    {
                        ValidationLength(e, 9, txtTicketAmt06, "ticket", "ticket_amt06");
                    }
                    else if (txtTicketAmt07.IsFocused)
                    {
                        ValidationLength(e, 9, txtTicketAmt07, "ticket", "ticket_amt07");
                    }
                    else if (txtTicketAmt08.IsFocused)
                    {
                        ValidationLength(e, 9, txtTicketAmt08, "ticket", "ticket_amt08");
                    }
                    else if (txtTicketAmt09.IsFocused)
                    {
                        ValidationLength(e, 9, txtTicketAmt09, "ticket", "ticket_amt09");
                    }

                    #endregion

                    #endregion
                }
            }
        }

        /// <summary>
        /// 등록완료 Button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void wbSave_Click(object sender, EventArgs e)
        {
            if (_bNew || _dTotalCnt > 0)
            {
                //중간입금 차수 및 차수 합계 금액 저장
                SetAmt();
            }
        }

        /// <summary>
        /// 닫기 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// 타사 상품권명 조회 및 셋팅
        /// </summary>
        /// <param name="ds">타사 상품권명 조회 결과</param>
        public void SetTicketTitle(DataSet ds)
        {
            txtTicketCnt02.Focusable = false;
            txtTicketAmt02.Focusable = false;
            txtTicketCnt03.Focusable = false;
            txtTicketAmt03.Focusable = false;
            txtTicketCnt04.Focusable = false;
            txtTicketAmt04.Focusable = false;
            txtTicketCnt05.Focusable = false;
            txtTicketAmt05.Focusable = false;
            txtTicketCnt06.Focusable = false;
            txtTicketAmt06.Focusable = false;
            txtTicketCnt07.Focusable = false;
            txtTicketAmt07.Focusable = false;
            txtTicketCnt08.Focusable = false;
            txtTicketAmt08.Focusable = false;
            txtTicketCnt09.Focusable = false;
            txtTicketAmt09.Focusable = false;

            txtTicketCnt02.Tag = null;
            txtTicketCnt03.Tag = null;
            txtTicketCnt04.Tag = null;
            txtTicketCnt05.Tag = null;
            txtTicketCnt06.Tag = null;
            txtTicketCnt07.Tag = null;
            txtTicketCnt08.Tag = null;
            txtTicketCnt09.Tag = null;

            txtTicketCnt02.Title = "";
            txtTicketCnt03.Title = "";
            txtTicketCnt04.Title = "";
            txtTicketCnt05.Title = "";
            txtTicketCnt06.Title = "";
            txtTicketCnt07.Title = "";
            txtTicketCnt08.Title = "";
            txtTicketCnt09.Title = "";

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                int iRow = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (iRow >= 8)
                    {
                        break;
                    }

                    switch (iRow)
                    {
                        case 0:
                            txtTicketCnt02.Focusable = true;
                            txtTicketAmt02.Focusable = true;
                            txtTicketCnt02.Tag = dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "";
                            txtTicketCnt02.Title = dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "";
                            break;
                        case 1:
                            txtTicketCnt03.Focusable = true;
                            txtTicketAmt03.Focusable = true;
                            txtTicketCnt03.Tag = dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "";
                            txtTicketCnt03.Title = dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "";
                            break;
                        case 2:
                            txtTicketCnt04.Focusable = true;
                            txtTicketAmt04.Focusable = true;
                            txtTicketCnt04.Tag = dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "";
                            txtTicketCnt04.Title = dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "";
                            break;
                        case 3:
                            txtTicketCnt05.Focusable = true;
                            txtTicketAmt05.Focusable = true;
                            txtTicketCnt05.Tag = dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "";
                            txtTicketCnt05.Title = dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "";
                            break;
                        case 4:
                            txtTicketCnt06.Focusable = true;
                            txtTicketAmt06.Focusable = true;
                            txtTicketCnt06.Tag = dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "";
                            txtTicketCnt06.Title = dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "";
                            break;
                        case 5:
                            txtTicketCnt07.Focusable = true;
                            txtTicketAmt07.Focusable = true;
                            txtTicketCnt07.Tag = dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "";
                            txtTicketCnt07.Title = dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "";
                            break;
                        case 6:
                            txtTicketCnt08.Focusable = true;
                            txtTicketAmt08.Focusable = true;
                            txtTicketCnt08.Tag = dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "";
                            txtTicketCnt08.Title = dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "";
                            break;
                        case 7:
                            txtTicketCnt09.Focusable = true;
                            txtTicketAmt09.Focusable = true;
                            txtTicketCnt09.Tag = dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "";
                            txtTicketCnt09.Title = dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "";
                            break;
                        default:
                            break;
                    }

                    iRow++;
                }
            }
        }

        /// <summary>
        /// 중간입금 회차 및 금액 셋팅
        /// </summary>
        /// <param name="ds">중간입금 회차 및 금액 조회결과</param>
        public void SetMiddleDeposit(DataSet ds)
        {
            _dTotalCnt = 0;             //중간입금 현재회차
            lblMidAmt01.Text = "0";     //중간입금 01회차 금액
            lblMidAmt02.Text = "0";     //중간입금 02회차 금액
            lblMidAmt03.Text = "0";     //중간입금 03회차 금액
            lblMidAmt04.Text = "0";     //중간입금 04회차 금액
            lblMidAmt05.Text = "0";     //중간입금 05회차 금액
            lblMidCnt.Text = "0";       //중간입금 총회차
            lblMidAmtTotal.Text = "0";  //중간 입금 총금액

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
            {
                decimal dTotalAmt = 0;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    decimal dTempCash = Convert.ToDecimal(string.Format("{0:F0}", dr["AM_ITEM"] != null && dr["AM_ITEM"].ToString() != "" ? dr["AM_ITEM"] : "0"));
                    
                    switch (dr["ID_ITEM"].ToString())
                    {
                        case "E00": //중간입금 총회차
                            lblMidCnt.Text = dr["CT_ITEM"] != null ? dr["CT_ITEM"].ToString() : "";
                            _dTotalCnt = Convert.ToInt16(dr["CT_ITEM"] != null && dr["CT_ITEM"].ToString() != "" ? dr["CT_ITEM"].ToString() : "0");
                            break;
                        case "E01": //중간입금 현금 및 상품권 01회차 금액
                        case "E21":
                            lblMidAmt01.Text = string.Format("{0:n0}", dTempCash + POSCommon.Instance.GetCLabel(lblMidAmt01));
                            dTotalAmt += dTempCash;
                            break;
                        case "E02": //중간입금 현금 및 상품권 02회차 금액
                        case "E22":
                            lblMidAmt02.Text = string.Format("{0:n0}", dTempCash + POSCommon.Instance.GetCLabel(lblMidAmt02));
                            dTotalAmt += dTempCash;
                            break;
                        case "E03": //중간입금 현금 및 상품권 03회차 금액
                        case "E23":
                            lblMidAmt03.Text = string.Format("{0:n0}", dTempCash + POSCommon.Instance.GetCLabel(lblMidAmt03));
                            dTotalAmt += dTempCash;
                            break;
                        case "E04": //중간입금 현금 및 상품권 04회차 금액
                        case "E24":
                            lblMidAmt04.Text = string.Format("{0:n0}", dTempCash + POSCommon.Instance.GetCLabel(lblMidAmt04));
                            dTotalAmt += dTempCash;
                            break;
                        case "E05": //중간입금 현금 및 상품권 05회차 금액
                        case "E25":
                            lblMidAmt05.Text = string.Format("{0:n0}", dTempCash + POSCommon.Instance.GetCLabel(lblMidAmt05));
                            dTotalAmt += dTempCash;
                            break;
                        default:
                            break;
                    }
                }

                //중간 입금 총금액
                lblMidAmtTotal.Text = string.Format("{0:n0}", dTotalAmt);
            }
        }

        /// <summary>
        /// InputControl 자릿수 유효성 검사 및 InputControl이 새롭게 focus를 받았다면 기존 텍스트는 지우고 새로 입력한 값으로 변경
        /// </summary>
        /// <param name="e"></param>
        /// <param name="iLength"></param>
        /// <param name="txtControl"></param>
        /// <param name="strTableNm"></param>
        /// <param name="strColNm"></param>
        private void ValidationLength(OPOSKeyEventArgs e, int iLength, InputControl txtControl, string strTableNm, string strColNm)
        {
            if (dsControl.Tables[strTableNm].Rows[2][strColNm].ToString() == "n")
            {
                if (txtControl.Text.Length > 0)
                {
                    txtControl.Text = e.KeyCodeText;
                }

                dsControl.Tables[strTableNm].Rows[2][strColNm] = "y";
            }

            if (txtControl.Text.Trim().Length >= iLength)
            {
                txtControl.Text = txtControl.Text.Substring(0, iLength);

                e.IsHandled = true;
            }

            dsControl.Tables[strTableNm].Rows[3][strColNm] = txtControl.Text;
        }

        /// <summary>
        /// 금액 컨트롤 초기화
        /// </summary>
        private void SetInitControl()
        {
            txtCashCnt01.Text = "";
            txtCashCnt02.Text = "";
            txtCashCnt03.Text = "";
            txtCashCnt04.Text = "";
            txtCashCnt05.Text = "";
            txtCashCnt06.Text = "";
            txtCashCnt07.Text = "";
            txtCashCnt08.Text = "";
            txtCashCnt09.Text = "";

            txtCashAmt01.Text = "";
            txtCashAmt02.Text = "";
            txtCashAmt03.Text = "";
            txtCashAmt04.Text = "";
            txtCashAmt05.Text = "";
            txtCashAmt06.Text = "";
            txtCashAmt07.Text = "";
            txtCashAmt08.Text = "";
            txtCashAmt09.Text = "";

            txtTicketCnt01.Text = "";
            txtTicketCnt02.Text = "";
            txtTicketCnt03.Text = "";
            txtTicketCnt04.Text = "";
            txtTicketCnt05.Text = "";
            txtTicketCnt06.Text = "";
            txtTicketCnt07.Text = "";
            txtTicketCnt08.Text = "";
            txtTicketCnt09.Text = "";

            txtTicketAmt01.Text = "";
            txtTicketAmt02.Text = "";
            txtTicketAmt03.Text = "";
            txtTicketAmt04.Text = "";
            txtTicketAmt05.Text = "";
            txtTicketAmt06.Text = "";
            txtTicketAmt07.Text = "";
            txtTicketAmt08.Text = "";
            txtTicketAmt09.Text = "";

            txtCashTotal.Text = "0";
            txtTicketTotal.Text = "0";
            txtTotalAmt.Text = "0";

            wbSave.Enabled = true;

            txtCashCnt01.SetFocus();
        }

        /// <summary>
        /// 상품권 관련 포커스 설정
        /// </summary>
        /// <param name="txtNowFoucs"></param>
        private void SetFocus(InputControl txtNowFoucs)
        {
            if (txtNowFoucs == txtTicketAmt01)
            {
                if (txtTicketCnt02.Focusable)
                {
                    txtTicketCnt02.SetFocus();
                }
                else
                {
                    txtCashCnt01.SetFocus();
                }
            }
            else if (txtNowFoucs == txtTicketAmt02)
            {
                if (txtTicketCnt03.Focusable)
                {
                    txtTicketCnt03.SetFocus();
                }
                else
                {
                    txtCashCnt01.SetFocus();
                }
            }
            else if (txtNowFoucs == txtTicketAmt03)
            {
                if (txtTicketCnt04.Focusable)
                {
                    txtTicketCnt04.SetFocus();
                }
                else
                {
                    txtCashCnt01.SetFocus();
                }
            }
            else if (txtNowFoucs == txtTicketAmt04)
            {
                if (txtTicketCnt05.Focusable)
                {
                    txtTicketCnt05.SetFocus();
                }
                else
                {
                    txtCashCnt01.SetFocus();
                }
            }
            else if (txtNowFoucs == txtTicketAmt05)
            {
                if (txtTicketCnt06.Focusable)
                {
                    txtTicketCnt06.SetFocus();
                }
                else
                {
                    txtCashCnt01.SetFocus();
                }
            }
            else if (txtNowFoucs == txtTicketAmt06)
            {
                if (txtTicketCnt07.Focusable)
                {
                    txtTicketCnt07.SetFocus();
                }
                else
                {
                    txtCashCnt01.SetFocus();
                }
            }
            else if (txtNowFoucs == txtTicketAmt07)
            {
                if (txtTicketCnt08.Focusable)
                {
                    txtTicketCnt08.SetFocus();
                }
                else
                {
                    txtCashCnt01.SetFocus();
                }
            }
            else if (txtNowFoucs == txtTicketAmt08)
            {
                if (txtTicketCnt09.Focusable)
                {
                    txtTicketCnt09.SetFocus();
                }
                else
                {
                    txtCashCnt01.SetFocus();
                }
            }
            else if (txtNowFoucs == txtTicketAmt09)
            {
                txtCashCnt01.SetFocus();
            }
        }

        /// <summary>
        /// 기존 값으로 복원
        /// </summary>
        /// <param name="txtControl">InputControl</param>
        private void SetRecover(InputControl txtControl)
        {
            string strTableNm = string.Empty;
            string strColNm = string.Empty;

            #region 테이블 및 컬럼명 설정

            if (txtControl == txtCashCnt01)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt01";
            }
            else if (txtControl == txtCashCnt02)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt02";
            }
            else if (txtControl == txtCashCnt03)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt03";
            }
            else if (txtControl == txtCashCnt04)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt04";
            }
            else if (txtControl == txtCashCnt05)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt05";
            }
            else if (txtControl == txtCashCnt06)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt06";
            }
            else if (txtControl == txtCashCnt07)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt07";
            }
            else if (txtControl == txtCashCnt08)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt08";
            }
            else if (txtControl == txtCashCnt09)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt09";
            }
            else if (txtControl == txtCashAmt01)
            {
                strTableNm = "cash";
                strColNm = "cash_amt01";
            }
            else if (txtControl == txtCashAmt02)
            {
                strTableNm = "cash";
                strColNm = "cash_amt02";
            }
            else if (txtControl == txtCashAmt03)
            {
                strTableNm = "cash";
                strColNm = "cash_amt03";
            }
            else if (txtControl == txtCashAmt04)
            {
                strTableNm = "cash";
                strColNm = "cash_amt04";
            }
            else if (txtControl == txtCashAmt05)
            {
                strTableNm = "cash";
                strColNm = "cash_amt05";
            }
            else if (txtControl == txtCashAmt06)
            {
                strTableNm = "cash";
                strColNm = "cash_amt06";
            }
            else if (txtControl == txtCashAmt07)
            {
                strTableNm = "cash";
                strColNm = "cash_amt07";
            }
            else if (txtControl == txtCashAmt08)
            {
                strTableNm = "cash";
                strColNm = "cash_amt08";
            }
            else if (txtControl == txtCashAmt09)
            {
                strTableNm = "cash";
                strColNm = "cash_amt09";
            }
            else if (txtControl == txtTicketCnt01)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt01";
            }
            else if (txtControl == txtTicketCnt02)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt02";
            }
            else if (txtControl == txtTicketCnt03)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt03";
            }
            else if (txtControl == txtTicketCnt04)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt04";
            }
            else if (txtControl == txtTicketCnt05)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt05";
            }
            else if (txtControl == txtTicketCnt06)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt06";
            }
            else if (txtControl == txtTicketCnt07)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt07";
            }
            else if (txtControl == txtTicketCnt08)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt08";
            }
            else if (txtControl == txtTicketCnt09)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt09";
            }
            else if (txtControl == txtTicketAmt01)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt01";
            }
            else if (txtControl == txtTicketAmt02)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt02";
            }
            else if (txtControl == txtTicketAmt03)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt03";
            }
            else if (txtControl == txtTicketAmt04)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt04";
            }
            else if (txtControl == txtTicketAmt05)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt05";
            }
            else if (txtControl == txtTicketAmt06)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt06";
            }
            else if (txtControl == txtTicketAmt07)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt07";
            }
            else if (txtControl == txtTicketAmt08)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt08";
            }
            else if (txtControl == txtTicketAmt09)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt09";
            }


            #endregion

            DataRow dr = dsControl.Tables[strTableNm].Rows[0];
            DataRow dr_YN = dsControl.Tables[strTableNm].Rows[1];

            if (dr_YN[strColNm] == "")
            {
                dr_YN[strColNm] = "y";
                dr[strColNm] = txtControl.Text.ToString();
            }
            else
            {
                if (dr[strColNm].ToString() != "" && dr[strColNm].ToString() != "0")
                {
                    if (txtControl.Text.ToString() == "")
                    {
                        txtControl.Text = dr[strColNm].ToString();
                    }
                    else
                    {
                        dr[strColNm] = txtControl.Text.ToString() == "0" ? "" : txtControl.Text.ToString();
                    }
                }
                else
                {
                    dr[strColNm] = txtControl.Text.ToString() == "0" ? "" : txtControl.Text.ToString();
                }
            }

            dsControl.Tables[strTableNm].Rows[2][strColNm] = "n";
        }

        private void SetClearRecover(OPOSKeyEventArgs e, InputControl txtControl, InputControl txtFocusControl)
        {
            string strTableNm = string.Empty;
            string strColNm = string.Empty;

            #region 테이블 및 컬럼명 설정

            if (txtControl == txtCashCnt01)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt01";
            }
            else if (txtControl == txtCashCnt02)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt02";
            }
            else if (txtControl == txtCashCnt03)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt03";
            }
            else if (txtControl == txtCashCnt04)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt04";
            }
            else if (txtControl == txtCashCnt05)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt05";
            }
            else if (txtControl == txtCashCnt06)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt06";
            }
            else if (txtControl == txtCashCnt07)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt07";
            }
            else if (txtControl == txtCashCnt08)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt08";
            }
            else if (txtControl == txtCashCnt09)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt09";
            }
            else if (txtControl == txtCashAmt01)
            {
                strTableNm = "cash";
                strColNm = "cash_amt01";
            }
            else if (txtControl == txtCashAmt02)
            {
                strTableNm = "cash";
                strColNm = "cash_amt02";
            }
            else if (txtControl == txtCashAmt03)
            {
                strTableNm = "cash";
                strColNm = "cash_amt03";
            }
            else if (txtControl == txtCashAmt04)
            {
                strTableNm = "cash";
                strColNm = "cash_amt04";
            }
            else if (txtControl == txtCashAmt05)
            {
                strTableNm = "cash";
                strColNm = "cash_amt05";
            }
            else if (txtControl == txtCashAmt06)
            {
                strTableNm = "cash";
                strColNm = "cash_amt06";
            }
            else if (txtControl == txtCashAmt07)
            {
                strTableNm = "cash";
                strColNm = "cash_amt07";
            }
            else if (txtControl == txtCashAmt08)
            {
                strTableNm = "cash";
                strColNm = "cash_amt08";
            }
            else if (txtControl == txtCashAmt09)
            {
                strTableNm = "cash";
                strColNm = "cash_amt09";
            }
            else if (txtControl == txtTicketCnt01)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt01";
            }
            else if (txtControl == txtTicketCnt02)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt02";
            }
            else if (txtControl == txtTicketCnt03)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt03";
            }
            else if (txtControl == txtTicketCnt04)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt04";
            }
            else if (txtControl == txtTicketCnt05)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt05";
            }
            else if (txtControl == txtTicketCnt06)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt06";
            }
            else if (txtControl == txtTicketCnt07)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt07";
            }
            else if (txtControl == txtTicketCnt08)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt08";
            }
            else if (txtControl == txtTicketCnt09)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt09";
            }
            else if (txtControl == txtTicketAmt01)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt01";
            }
            else if (txtControl == txtTicketAmt02)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt02";
            }
            else if (txtControl == txtTicketAmt03)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt03";
            }
            else if (txtControl == txtTicketAmt04)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt04";
            }
            else if (txtControl == txtTicketAmt05)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt05";
            }
            else if (txtControl == txtTicketAmt06)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt06";
            }
            else if (txtControl == txtTicketAmt07)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt07";
            }
            else if (txtControl == txtTicketAmt08)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt08";
            }
            else if (txtControl == txtTicketAmt09)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt09";
            }

            #endregion

            DataRow dr = dsControl.Tables[strTableNm].Rows[0];
            DataRow dr_YN = dsControl.Tables[strTableNm].Rows[1];
            DataRow dr_copy = dsControl.Tables[strTableNm].Rows[3];

            txtControl.Text = dr_copy[strColNm].ToString() == "0" ? "" : dr_copy[strColNm].ToString();

            if (dr_YN[strColNm] == "")
            {
                if (txtControl.Text.Length <= 0)
                {
                    txtFocusControl.SetFocus();
                    e.IsHandled = true;
                }
                else
                {
                    txtControl.Text = "";
                    dr_copy[strColNm] = "";
                    e.IsHandled = true;
                }
            }
            else
	        {
                if ((txtControl.Text.ToString() == "" ? "0" : txtControl.Text.ToString()) == (dr[strColNm].ToString() == "" ? "0" : dr[strColNm].ToString()))
                {
                    txtFocusControl.SetFocus();
                    e.IsHandled = true;
                }
                else
                {
                    txtControl.Text = dr[strColNm].ToString() == "0" ? "" : dr[strColNm].ToString();
                    dsControl.Tables[strTableNm].Rows[2][strColNm] = "n";
                    e.IsHandled = true;
                }
	        }
        }

        private void SetBackSpaveRecover(OPOSKeyEventArgs e, InputControl txtControl)
        {
            string strTableNm = string.Empty;
            string strColNm = string.Empty;

            #region 테이블 및 컬럼명 설정

            if (txtControl == txtCashCnt01)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt01";
            }
            else if (txtControl == txtCashCnt02)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt02";
            }
            else if (txtControl == txtCashCnt03)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt03";
            }
            else if (txtControl == txtCashCnt04)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt04";
            }
            else if (txtControl == txtCashCnt05)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt05";
            }
            else if (txtControl == txtCashCnt06)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt06";
            }
            else if (txtControl == txtCashCnt07)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt07";
            }
            else if (txtControl == txtCashCnt08)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt08";
            }
            else if (txtControl == txtCashCnt09)
            {
                strTableNm = "cash";
                strColNm = "cash_cnt09";
            }
            else if (txtControl == txtCashAmt01)
            {
                strTableNm = "cash";
                strColNm = "cash_amt01";
            }
            else if (txtControl == txtCashAmt02)
            {
                strTableNm = "cash";
                strColNm = "cash_amt02";
            }
            else if (txtControl == txtCashAmt03)
            {
                strTableNm = "cash";
                strColNm = "cash_amt03";
            }
            else if (txtControl == txtCashAmt04)
            {
                strTableNm = "cash";
                strColNm = "cash_amt04";
            }
            else if (txtControl == txtCashAmt05)
            {
                strTableNm = "cash";
                strColNm = "cash_amt05";
            }
            else if (txtControl == txtCashAmt06)
            {
                strTableNm = "cash";
                strColNm = "cash_amt06";
            }
            else if (txtControl == txtCashAmt07)
            {
                strTableNm = "cash";
                strColNm = "cash_amt07";
            }
            else if (txtControl == txtCashAmt08)
            {
                strTableNm = "cash";
                strColNm = "cash_amt08";
            }
            else if (txtControl == txtCashAmt09)
            {
                strTableNm = "cash";
                strColNm = "cash_amt09";
            }
            else if (txtControl == txtTicketCnt01)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt01";
            }
            else if (txtControl == txtTicketCnt02)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt02";
            }
            else if (txtControl == txtTicketCnt03)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt03";
            }
            else if (txtControl == txtTicketCnt04)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt04";
            }
            else if (txtControl == txtTicketCnt05)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt05";
            }
            else if (txtControl == txtTicketCnt06)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt06";
            }
            else if (txtControl == txtTicketCnt07)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt07";
            }
            else if (txtControl == txtTicketCnt08)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt08";
            }
            else if (txtControl == txtTicketCnt09)
            {
                strTableNm = "ticket";
                strColNm = "ticket_cnt09";
            }
            else if (txtControl == txtTicketAmt01)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt01";
            }
            else if (txtControl == txtTicketAmt02)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt02";
            }
            else if (txtControl == txtTicketAmt03)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt03";
            }
            else if (txtControl == txtTicketAmt04)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt04";
            }
            else if (txtControl == txtTicketAmt05)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt05";
            }
            else if (txtControl == txtTicketAmt06)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt06";
            }
            else if (txtControl == txtTicketAmt07)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt07";
            }
            else if (txtControl == txtTicketAmt08)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt08";
            }
            else if (txtControl == txtTicketAmt09)
            {
                strTableNm = "ticket";
                strColNm = "ticket_amt09";
            }

            #endregion

            DataRow dr = dsControl.Tables[strTableNm].Rows[0];
            DataRow dr_YN = dsControl.Tables[strTableNm].Rows[1];
            DataRow dr_copy = dsControl.Tables[strTableNm].Rows[3];

            txtControl.Text = dr_copy[strColNm].ToString();

            if (txtControl.Text.Length > 0)
            {
                if (dr_YN[strColNm] != "")
                {
                    if (txtControl.Text.ToString() == dr[strColNm].ToString())
                    {
                        txtControl.Text = "";
                        e.IsHandled = true;
                    }
                    else
                    {
                        txtControl.Text = txtControl.Text.Substring(0, txtControl.Text.Length - 1);
                        dr_copy[strColNm] = txtControl.Text.ToString() == "0" ? "" : txtControl.Text.ToString();
                    }
                }
                else
                {
                    txtControl.Text = txtControl.Text.Substring(0, txtControl.Text.Length - 1);
                    dr_copy[strColNm] = txtControl.Text.ToString() == "0" ? "" : txtControl.Text.ToString();
                }
            }
        }

        /// <summary>
        /// 컨트롤 유횽성 검사 및 합계 구하기
        /// </summary>
        /// <param name="strGubun">현금및상품권구분(c:현금, t:상품권)</param>
        /// <param name="txtCnt">현금및상품권 매수 컨트롤</param>
        /// <param name="txtAmt">현금및상품권 금액 컨트롤</param>
        /// <param name="iCash">금액권구분(예: 오만원, 만원, 오천원...)</param>
        /// <returns></returns>
        private bool SetControlAmt(string strGubun, InputControl txtCnt, InputControl txtAmt, int iCash)
        {
            decimal dTempTotal = 0;

            if (strGubun == "c")
            {
                //수표가 아닐경우 금액권 * 매수
                if (txtCnt.Name.ToString() != "txtCashCnt01")
                {
                    decimal dTemp = POSCommon.Instance.GetInputControl(txtCnt) * iCash;
                    txtAmt.Text = string.Format("{0}", dTemp > 0 ? dTemp.ToString() : "");
                }

                if (POSCommon.Instance.GetInputControl(txtCnt) <= 0)
                {
                    txtCnt.Text = "";
                }

                if (POSCommon.Instance.GetInputControl(txtAmt) <= 0)
                {
                    txtAmt.Text = "";
                }

                //현금 합계 구하기--------------------------------------------------------------------------------------------------------------------------------------------
                dTempTotal += POSCommon.Instance.GetInputControl(txtCashAmt01);
                dTempTotal += POSCommon.Instance.GetInputControl(txtCashAmt02);
                dTempTotal += POSCommon.Instance.GetInputControl(txtCashAmt03);
                dTempTotal += POSCommon.Instance.GetInputControl(txtCashAmt04);
                dTempTotal += POSCommon.Instance.GetInputControl(txtCashAmt05);
                dTempTotal += POSCommon.Instance.GetInputControl(txtCashAmt06);
                dTempTotal += POSCommon.Instance.GetInputControl(txtCashAmt07);
                dTempTotal += POSCommon.Instance.GetInputControl(txtCashAmt08);
                dTempTotal += POSCommon.Instance.GetInputControl(txtCashAmt09);
                txtCashTotal.Text = dTempTotal.ToString();
                //------------------------------------------------------------------------------------------------------------------------------------------------------------
            }
            else if (strGubun == "t")
            {
                if (POSCommon.Instance.GetInputControl(txtCnt) <= 0)
                {
                    txtCnt.Text = "";
                }

                if (POSCommon.Instance.GetInputControl(txtAmt) <= 0)
                {
                    txtAmt.Text = "";
                }

                //티켓 합계 구하기---------------------------------------------------------------------------------------------------------------------------------------------------------------
                dTempTotal += POSCommon.Instance.GetInputControl(txtTicketAmt01);
                dTempTotal += POSCommon.Instance.GetInputControl(txtTicketAmt02);
                dTempTotal += POSCommon.Instance.GetInputControl(txtTicketAmt03);
                dTempTotal += POSCommon.Instance.GetInputControl(txtTicketAmt04);
                dTempTotal += POSCommon.Instance.GetInputControl(txtTicketAmt05);
                dTempTotal += POSCommon.Instance.GetInputControl(txtTicketAmt06);
                dTempTotal += POSCommon.Instance.GetInputControl(txtTicketAmt07);
                dTempTotal += POSCommon.Instance.GetInputControl(txtTicketAmt08);
                dTempTotal += POSCommon.Instance.GetInputControl(txtTicketAmt09);
                txtTicketTotal.Text = dTempTotal.ToString();
                //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            }

            //총합계 구하기--------------------------------------------------------------------------------------------
            dTempTotal = 0;
            dTempTotal += POSCommon.Instance.GetInputControl(txtCashTotal);
            dTempTotal += POSCommon.Instance.GetInputControl(txtTicketTotal);
            txtTotalAmt.Text = dTempTotal.ToString();
            //----------------------------------------------------------------------------------------------------------

            //중간입금 회차 및 중간입금 금액
            SetViewTotal();

            return true;
        }

        /// <summary>
        /// 중간입금 회차 및 중간입금 금액
        /// </summary>
        private void SetViewTotal()
        {
            string strTempTotalAmt = string.Format("{0:n0}", POSCommon.Instance.GetInputControl(txtTotalAmt));
            switch (Convert.ToInt32(_dTotalCnt))
            {
                case 1:
                    lblMidAmt01.Text = strTempTotalAmt;
                    break;
                case 2:
                    lblMidAmt02.Text = strTempTotalAmt;
                    break;
                case 3:
                    lblMidAmt03.Text = strTempTotalAmt;
                    break;
                case 4:
                    lblMidAmt04.Text = strTempTotalAmt;
                    break;
                case 5:
                    lblMidAmt05.Text = strTempTotalAmt;
                    break;
                default:
                    break;
            }

            decimal dTempTotal = 0;
            dTempTotal += POSCommon.Instance.GetCLabel(lblMidAmt01);
            dTempTotal += POSCommon.Instance.GetCLabel(lblMidAmt02);
            dTempTotal += POSCommon.Instance.GetCLabel(lblMidAmt03);
            dTempTotal += POSCommon.Instance.GetCLabel(lblMidAmt04);
            dTempTotal += POSCommon.Instance.GetCLabel(lblMidAmt05);
            lblMidAmtTotal.Text = string.Format("{0:n0}", dTempTotal);
        }

        /// <summary>
        /// 중간입금 차수 및 차수 합계 금액 저장
        /// </summary>
        private void SetAmt()
        {
            #region 중간입금 상세자료 확인

            DataTable dt = new DataTable();
            for (int i = 0; i <= 60; i++)
			{
			    dt.Columns.Add(i.ToString()); 
			}

            decimal dTempCnt = 0;       //총건수
            decimal dTempAmt = 0;   //총금액

            DataRow dr = dt.NewRow();
            dr[1] = _bNew ? NetCommConstants.INPUT_INSERT : NetCommConstants.INPUT_UPDATE;
            dr[2] = _dTotalCnt.ToString();

            dTempCnt += POSCommon.Instance.GetInputControl(txtCashAmt01) > 0 ? POSCommon.Instance.GetInputControl(txtCashCnt01) : 0;
            dTempCnt += POSCommon.Instance.GetInputControl(txtCashAmt02) > 0 ? POSCommon.Instance.GetInputControl(txtCashCnt02) : 0;
            dTempCnt += POSCommon.Instance.GetInputControl(txtCashAmt03) > 0 ? POSCommon.Instance.GetInputControl(txtCashCnt03) : 0;
            dTempCnt += POSCommon.Instance.GetInputControl(txtCashAmt04) > 0 ? POSCommon.Instance.GetInputControl(txtCashCnt04) : 0;
            dTempCnt += POSCommon.Instance.GetInputControl(txtCashAmt05) > 0 ? POSCommon.Instance.GetInputControl(txtCashCnt05) : 0;
            dTempCnt += POSCommon.Instance.GetInputControl(txtCashAmt06) > 0 ? POSCommon.Instance.GetInputControl(txtCashCnt06) : 0;
            dTempCnt += POSCommon.Instance.GetInputControl(txtCashAmt07) > 0 ? POSCommon.Instance.GetInputControl(txtCashCnt07) : 0;
            dTempCnt += POSCommon.Instance.GetInputControl(txtCashAmt08) > 0 ? POSCommon.Instance.GetInputControl(txtCashCnt08) : 0;
            dTempCnt += POSCommon.Instance.GetInputControl(txtCashAmt09) > 0 ? POSCommon.Instance.GetInputControl(txtCashCnt09) : 0;
            dr[3] = dTempCnt > 0 ? dTempCnt.ToString() : "";
            dr[4] = POSCommon.Instance.GetStringInputControl(txtCashTotal);

            dr[5] = POSCommon.Instance.GetInputControl(txtTicketAmt01) > 0 ? POSCommon.Instance.GetStringInputControl(txtTicketCnt01) : "";
            dr[6] = POSCommon.Instance.GetStringInputControl(txtTicketAmt01);
            dr[7] = "";
            dr[8] = "";
            dTempCnt = 0;
            dTempCnt += POSCommon.Instance.GetInputControl(txtTicketAmt02) > 0 ? POSCommon.Instance.GetInputControl(txtTicketCnt02) : 0;
            dTempCnt += POSCommon.Instance.GetInputControl(txtTicketAmt03) > 0 ? POSCommon.Instance.GetInputControl(txtTicketCnt03) : 0;
            dTempCnt += POSCommon.Instance.GetInputControl(txtTicketAmt04) > 0 ? POSCommon.Instance.GetInputControl(txtTicketCnt04) : 0;
            dTempCnt += POSCommon.Instance.GetInputControl(txtTicketAmt05) > 0 ? POSCommon.Instance.GetInputControl(txtTicketCnt05) : 0;
            dTempCnt += POSCommon.Instance.GetInputControl(txtTicketAmt06) > 0 ? POSCommon.Instance.GetInputControl(txtTicketCnt06) : 0;
            dTempCnt += POSCommon.Instance.GetInputControl(txtTicketAmt07) > 0 ? POSCommon.Instance.GetInputControl(txtTicketCnt07) : 0;
            dTempCnt += POSCommon.Instance.GetInputControl(txtTicketAmt08) > 0 ? POSCommon.Instance.GetInputControl(txtTicketCnt08) : 0;
            dTempCnt += POSCommon.Instance.GetInputControl(txtTicketAmt09) > 0 ? POSCommon.Instance.GetInputControl(txtTicketCnt09) : 0;
            dr[9] = dTempCnt > 0 ? dTempCnt.ToString() : "";
            dTempAmt = 0;
            dTempAmt += POSCommon.Instance.GetInputControl(txtTicketAmt02);
            dTempAmt += POSCommon.Instance.GetInputControl(txtTicketAmt03);
            dTempAmt += POSCommon.Instance.GetInputControl(txtTicketAmt04);
            dTempAmt += POSCommon.Instance.GetInputControl(txtTicketAmt05);
            dTempAmt += POSCommon.Instance.GetInputControl(txtTicketAmt06);
            dTempAmt += POSCommon.Instance.GetInputControl(txtTicketAmt07);
            dTempAmt += POSCommon.Instance.GetInputControl(txtTicketAmt08);
            dTempAmt += POSCommon.Instance.GetInputControl(txtTicketAmt09);
            dr[10] = dTempAmt > 0 ? dTempAmt.ToString() : "";

            dr[11] = POSCommon.Instance.GetInputControl(txtCashAmt01) > 0 ? POSCommon.Instance.GetStringInputControl(txtCashCnt01) : "";
            dr[12] = POSCommon.Instance.GetStringInputControl(txtCashAmt01);
            dr[13] = POSCommon.Instance.GetInputControl(txtCashAmt02) > 0 ? POSCommon.Instance.GetStringInputControl(txtCashCnt02) : "";
            dr[14] = POSCommon.Instance.GetStringInputControl(txtCashAmt02);
            dr[15] = POSCommon.Instance.GetInputControl(txtCashAmt03) > 0 ? POSCommon.Instance.GetStringInputControl(txtCashCnt03) : "";
            dr[16] = POSCommon.Instance.GetStringInputControl(txtCashAmt03);
            dr[17] = POSCommon.Instance.GetInputControl(txtCashAmt04) > 0 ? POSCommon.Instance.GetStringInputControl(txtCashCnt04) : "";
            dr[18] = POSCommon.Instance.GetStringInputControl(txtCashAmt04);
            dr[19] = POSCommon.Instance.GetInputControl(txtCashAmt05) > 0 ? POSCommon.Instance.GetStringInputControl(txtCashCnt05) : "";
            dr[20] = POSCommon.Instance.GetStringInputControl(txtCashAmt05);
            dr[21] = POSCommon.Instance.GetInputControl(txtCashAmt06) > 0 ? POSCommon.Instance.GetStringInputControl(txtCashCnt06) : "";
            dr[22] = POSCommon.Instance.GetStringInputControl(txtCashAmt06);
            dr[23] = POSCommon.Instance.GetInputControl(txtCashAmt07) > 0 ? POSCommon.Instance.GetStringInputControl(txtCashCnt07) : "";
            dr[24] = POSCommon.Instance.GetStringInputControl(txtCashAmt07);
            dr[25] = POSCommon.Instance.GetInputControl(txtCashAmt08) > 0 ? POSCommon.Instance.GetStringInputControl(txtCashCnt08) : "";
            dr[26] = POSCommon.Instance.GetStringInputControl(txtCashAmt08);
            dr[27] = POSCommon.Instance.GetInputControl(txtCashAmt09) > 0 ? POSCommon.Instance.GetStringInputControl(txtCashCnt09) : "";
            dr[28] = POSCommon.Instance.GetStringInputControl(txtCashAmt09);

            dr[29] = POSCommon.Instance.GetInputControl(txtTicketAmt01) > 0 ? POSCommon.Instance.GetStringInputControl(txtTicketCnt01) : "";
            dr[30] = POSCommon.Instance.GetStringInputControl(txtTicketAmt01);

            dr[31] = txtTicketCnt02.Tag != null && txtTicketCnt02.Tag.ToString() != "" ? txtTicketCnt02.Title.Trim() : "";
            dr[32] = txtTicketCnt02.Tag != null && txtTicketCnt02.Tag.ToString() != "" ? (POSCommon.Instance.GetInputControl(txtTicketAmt02) > 0 ? POSCommon.Instance.GetStringInputControl(txtTicketCnt02) : "") : "";
            dr[33] = txtTicketCnt02.Tag != null && txtTicketCnt02.Tag.ToString() != "" ? POSCommon.Instance.GetStringInputControl(txtTicketAmt02) : "";
            dr[34] = txtTicketCnt03.Tag != null && txtTicketCnt03.Tag.ToString() != "" ? txtTicketCnt03.Title.Trim() : "";
            dr[35] = txtTicketCnt03.Tag != null && txtTicketCnt03.Tag.ToString() != "" ? (POSCommon.Instance.GetInputControl(txtTicketAmt03) > 0 ? POSCommon.Instance.GetStringInputControl(txtTicketCnt03) : "") : "";
            dr[36] = txtTicketCnt03.Tag != null && txtTicketCnt03.Tag.ToString() != "" ? POSCommon.Instance.GetStringInputControl(txtTicketAmt03) : "";
            dr[37] = txtTicketCnt04.Tag != null && txtTicketCnt04.Tag.ToString() != "" ? txtTicketCnt04.Title.Trim() : "";
            dr[38] = txtTicketCnt04.Tag != null && txtTicketCnt04.Tag.ToString() != "" ? (POSCommon.Instance.GetInputControl(txtTicketAmt04) > 0 ? POSCommon.Instance.GetStringInputControl(txtTicketCnt04) : "") : "";
            dr[39] = txtTicketCnt04.Tag != null && txtTicketCnt04.Tag.ToString() != "" ? POSCommon.Instance.GetStringInputControl(txtTicketAmt04) : "";
            dr[40] = txtTicketCnt05.Tag != null && txtTicketCnt05.Tag.ToString() != "" ? txtTicketCnt05.Title.Trim() : "";
            dr[41] = txtTicketCnt05.Tag != null && txtTicketCnt05.Tag.ToString() != "" ? (POSCommon.Instance.GetInputControl(txtTicketAmt05) > 0 ? POSCommon.Instance.GetStringInputControl(txtTicketCnt05) : "") : "";
            dr[42] = txtTicketCnt05.Tag != null && txtTicketCnt05.Tag.ToString() != "" ? POSCommon.Instance.GetStringInputControl(txtTicketAmt05) : "";
            dr[43] = txtTicketCnt06.Tag != null && txtTicketCnt06.Tag.ToString() != "" ? txtTicketCnt06.Title.Trim() : "";
            dr[44] = txtTicketCnt06.Tag != null && txtTicketCnt06.Tag.ToString() != "" ? (POSCommon.Instance.GetInputControl(txtTicketAmt06) > 0 ? POSCommon.Instance.GetStringInputControl(txtTicketCnt06) : "") : "";
            dr[45] = txtTicketCnt06.Tag != null && txtTicketCnt06.Tag.ToString() != "" ? POSCommon.Instance.GetStringInputControl(txtTicketAmt06) : "";
            dr[46] = txtTicketCnt07.Tag != null && txtTicketCnt07.Tag.ToString() != "" ? txtTicketCnt07.Title.Trim() : "";
            dr[47] = txtTicketCnt07.Tag != null && txtTicketCnt07.Tag.ToString() != "" ? (POSCommon.Instance.GetInputControl(txtTicketAmt07) > 0 ? POSCommon.Instance.GetStringInputControl(txtTicketCnt07) : "") : "";
            dr[48] = txtTicketCnt07.Tag != null && txtTicketCnt07.Tag.ToString() != "" ? POSCommon.Instance.GetStringInputControl(txtTicketAmt07) : "";
            dr[49] = txtTicketCnt08.Tag != null && txtTicketCnt08.Tag.ToString() != "" ? txtTicketCnt08.Title.Trim() : "";
            dr[50] = txtTicketCnt08.Tag != null && txtTicketCnt08.Tag.ToString() != "" ? (POSCommon.Instance.GetInputControl(txtTicketAmt08) > 0 ? POSCommon.Instance.GetStringInputControl(txtTicketCnt08) : "") : "";
            dr[51] = txtTicketCnt08.Tag != null && txtTicketCnt08.Tag.ToString() != "" ? POSCommon.Instance.GetStringInputControl(txtTicketAmt08) : "";
            dr[52] = txtTicketCnt09.Tag != null && txtTicketCnt09.Tag.ToString() != "" ? txtTicketCnt09.Title.Trim() : "";
            dr[53] = txtTicketCnt09.Tag != null && txtTicketCnt09.Tag.ToString() != "" ? (POSCommon.Instance.GetInputControl(txtTicketAmt09) > 0 ? POSCommon.Instance.GetStringInputControl(txtTicketCnt09) : "") : "";
            dr[54] = txtTicketCnt09.Tag != null && txtTicketCnt09.Tag.ToString() != "" ? POSCommon.Instance.GetStringInputControl(txtTicketAmt09) : "";
            dr[55] = "";
            dr[56] = "";
            dr[57] = "";
            dr[58] = "";
            dr[59] = "";
            dr[60] = "";

            #endregion

            //중간입금 금액 및 회차정보 TRAN정보 저장---------------------------------------------------------------------------
            decimal dMiddleTotalAmt = POSCommon.Instance.GetInputControl(txtCashTotal);
            decimal dMiddleTotalCnt =   (POSCommon.Instance.GetInputControl(txtCashAmt01) > 0 ? POSCommon.Instance.GetInputControl(txtCashCnt01) : 0) +
                                        (POSCommon.Instance.GetInputControl(txtCashAmt02) > 0 ? POSCommon.Instance.GetInputControl(txtCashCnt02) : 0) +
                                        (POSCommon.Instance.GetInputControl(txtCashAmt03) > 0 ? POSCommon.Instance.GetInputControl(txtCashCnt03) : 0) +
                                        (POSCommon.Instance.GetInputControl(txtCashAmt04) > 0 ? POSCommon.Instance.GetInputControl(txtCashCnt04) : 0) +
                                        (POSCommon.Instance.GetInputControl(txtCashAmt05) > 0 ? POSCommon.Instance.GetInputControl(txtCashCnt05) : 0) +
                                        (POSCommon.Instance.GetInputControl(txtCashAmt06) > 0 ? POSCommon.Instance.GetInputControl(txtCashCnt06) : 0) +
                                        (POSCommon.Instance.GetInputControl(txtCashAmt07) > 0 ? POSCommon.Instance.GetInputControl(txtCashCnt07) : 0) +
                                        (POSCommon.Instance.GetInputControl(txtCashAmt08) > 0 ? POSCommon.Instance.GetInputControl(txtCashCnt08) : 0) +
                                        (POSCommon.Instance.GetInputControl(txtCashAmt09) > 0 ? POSCommon.Instance.GetInputControl(txtCashCnt09) : 0);
            decimal dMiddleTicketTotalAmt = POSCommon.Instance.GetInputControl(txtTicketTotal);
            decimal dMiddleTicketTotalCnt = (POSCommon.Instance.GetInputControl(txtTicketAmt01) > 0 ? POSCommon.Instance.GetInputControl(txtTicketCnt01) : 0) +
                                        (POSCommon.Instance.GetInputControl(txtTicketAmt02) > 0 ? POSCommon.Instance.GetInputControl(txtTicketCnt02) : 0) +
                                        (POSCommon.Instance.GetInputControl(txtTicketAmt03) > 0 ? POSCommon.Instance.GetInputControl(txtTicketCnt03) : 0) +
                                        (POSCommon.Instance.GetInputControl(txtTicketAmt04) > 0 ? POSCommon.Instance.GetInputControl(txtTicketCnt04) : 0) +
                                        (POSCommon.Instance.GetInputControl(txtTicketAmt05) > 0 ? POSCommon.Instance.GetInputControl(txtTicketCnt05) : 0) +
                                        (POSCommon.Instance.GetInputControl(txtTicketAmt06) > 0 ? POSCommon.Instance.GetInputControl(txtTicketCnt06) : 0) +
                                        (POSCommon.Instance.GetInputControl(txtTicketAmt07) > 0 ? POSCommon.Instance.GetInputControl(txtTicketCnt07) : 0) +
                                        (POSCommon.Instance.GetInputControl(txtTicketAmt08) > 0 ? POSCommon.Instance.GetInputControl(txtTicketCnt08) : 0) +
                                        (POSCommon.Instance.GetInputControl(txtTicketAmt09) > 0 ? POSCommon.Instance.GetInputControl(txtTicketCnt09) : 0);

            //m_Presenter.SetMiddelAmt(dr, dMiddleTotalAmt, dMiddleTotalCnt, dMiddleTicketTotalAmt, dMiddleTicketTotalCnt);
            //------------------------------------------------------------------------------------------------------------------

            string strTitle = PrinterUtils.ReceiptSaleTitle(
                "0",
                ConfigData.Current.AppConfig.PosInfo.CasName,
                "",
                string.Format("{0}-{1}", ConfigData.Current.AppConfig.PosInfo.PosNo, _strTrxnNo),
                _strDate);

            List<string> strMidCash = new List<string>();
            SetPrintDesc(strMidCash, txtCashCnt01, txtCashAmt01);
            SetPrintDesc(strMidCash, txtCashCnt02, txtCashAmt02);
            SetPrintDesc(strMidCash, txtCashCnt03, txtCashAmt03);
            SetPrintDesc(strMidCash, txtCashCnt04, txtCashAmt04);
            SetPrintDesc(strMidCash, txtCashCnt05, txtCashAmt05);
            SetPrintDesc(strMidCash, txtCashCnt06, txtCashAmt06);
            SetPrintDesc(strMidCash, txtCashCnt07, txtCashAmt07);
            SetPrintDesc(strMidCash, txtCashCnt08, txtCashAmt08);
            SetPrintDesc(strMidCash, txtCashCnt09, txtCashAmt09);

            List<string> strMidTicket = new List<string>();
            SetPrintDesc(strMidTicket, txtTicketCnt01, txtTicketAmt01);
            SetPrintDesc(strMidTicket, txtTicketCnt02, txtTicketAmt02);
            SetPrintDesc(strMidTicket, txtTicketCnt03, txtTicketAmt03);
            SetPrintDesc(strMidTicket, txtTicketCnt04, txtTicketAmt04);
            SetPrintDesc(strMidTicket, txtTicketCnt05, txtTicketAmt05);
            SetPrintDesc(strMidTicket, txtTicketCnt06, txtTicketAmt06);
            SetPrintDesc(strMidTicket, txtTicketCnt07, txtTicketAmt07);
            SetPrintDesc(strMidTicket, txtTicketCnt08, txtTicketAmt08);
            SetPrintDesc(strMidTicket, txtTicketCnt09, txtTicketAmt09);

            string strMidPrintEnd = "";

            #region Print
            //POSDeviceManager.Printer.StartPrint();
            //POSDeviceManager.Printer.PrintNormal(PrinterUtils.ReceiptLogoImage());
            //POSDeviceManager.Printer.PrintNormal(strTitle);
            //POSDeviceManager.Printer.PrintNormal(PrinterUtils.DepositName("0", Convert.ToInt32(_dTotalCnt)));
            //foreach (string strTemp in strMidCash)
            //{
            //    POSDeviceManager.Printer.PrintNormal(strTemp);
            //}

            //POSDeviceManager.Printer.PrintNormal(FXConsts.RECEIPT_DEPOSIT_TICKET + Environment.NewLine);

            //foreach (string strTemp in strMidTicket)
            //{
            //    POSDeviceManager.Printer.PrintNormal(strTemp);
            //}

            //POSDeviceManager.Printer.PrintNormal(strMidPrintEnd);

            //POSDeviceManager.Printer.EndPrint(); 
            #endregion

            #region 전자저널에 프린트 정보 저장
            TraceHelper.Instance.JournalWrite("중간입금", PrinterUtils.ReceiptLogoImage());
            TraceHelper.Instance.JournalWrite("중간입금", strTitle);
            //TraceHelper.Instance.JournalWrite("중간입금", PrinterUtils.DepositName("0", Convert.ToInt32(_dTotalCnt)));
            foreach (string strTemp in strMidCash)
            {
                TraceHelper.Instance.JournalWrite("중간입금", strTemp);
            }

            //TraceHelper.Instance.JournalWrite("중간입금", FXConsts.RECEIPT_DEPOSIT_TICKET + Environment.NewLine);

            foreach (string strTemp in strMidTicket)
            {
                TraceHelper.Instance.JournalWrite("중간입금", strTemp);
            }
            TraceHelper.Instance.JournalWrite("중간입금", strMidPrintEnd); 
            #endregion


            this.Close();
        }

        /// <summary>
        /// 프린터 출력물 
        /// </summary>
        /// <param name="strMid"></param>
        /// <param name="txtCnt"></param>
        /// <param name="txtAmt"></param>
        private void SetPrintDesc(List<string> strMid, InputControl txtCnt, InputControl txtAmt)
        {
            if (txtCnt.Tag != null && txtCnt.Tag.ToString() != "")
            {
                //strMid.Add(PrinterUtils.DepositList(txtCnt.Tag.ToString(), txtCnt.Title.ToString(), Convert.ToInt32(POSCommon.Instance.GetInputControl(txtCnt)), Convert.ToInt32(POSCommon.Instance.GetInputControl(txtAmt))));
            }
        }

        /// <summary>
        /// 메세지
        /// </summary>
        /// <param name="iCnt">중간입금회차</param>
        /// <param name="strGubun">중간입금 신규,수정구분</param>
        /// <returns>메세지</returns>
        private string SetMsg(decimal dCnt, string strGubun)
        {
            string strReturn = string.Empty;
            strReturn = string.Format("중간입금 {0}회를 {1}하시겠습니까?\n예(등록), 아니오(CLEAR)", dCnt, strGubun);
            return strReturn;
        }

        #endregion
    }
}
