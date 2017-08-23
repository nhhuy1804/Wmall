//-----------------------------------------------------------------
/*
 * 화면명   : POS_IO_M003.cs
 * 화면설명 : 마감입금 설정
 * 개발자   : 정광호
 * 개발일자 : 2015.03.24
 * 비고     : 화면 control들이 많은 관계로 form load시 시간이 많이 걸리는 관계로 control을 올린후 스크린샷을 통한 이미지를 입혀서 loading하여 속도 개선
 * Loc changed 11.09
 * - 할인쿠폰 추가
 * - 타사항품권 하나로
 * 권현주이사님 요청함.
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

using WSWD.WmallPos.POS.IO.PI;
using WSWD.WmallPos.POS.IO.PT;
using WSWD.WmallPos.POS.IO.VI;
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
using WSWD.WmallPos.POS.FX.Win.Data;
using WSWD.WmallPos.FX.Shared.Data;

namespace WSWD.WmallPos.POS.IO.VC
{
    public partial class POS_IO_M003 : FormBase, IIOM003View
    {
        #region 변수

        /// <summary>
        /// 마감입금 비즈니스 로직
        /// </summary>
        private IIOM003presenter m_Presenter;

        /// <summary>
        /// 컨트롤 value 값 임시 저장
        /// </summary>
        DataSet dsControl = null;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        #endregion

        #region 생성자

        /// <summary>
        /// POS_IO_M003 생성자
        /// </summary>
        public POS_IO_M003()
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
            
            this.btnSave.Click += new EventHandler(btnSave_Click);                                      //등록 및 등록완료 Event
            this.btnClose.Click += new EventHandler(btnClose_Click);                                    //닫기 button Event
            this.KeyEvent += new WSWD.WmallPos.FX.Shared.OPOSKeyEventHandler(form_KeyEvent);     //Key Event
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

            SetInitControl(true);

            #region 컨트롤 value 값 임시 저장 DataSet
            dsControl = new DataSet();

            DataTable dtCash = new DataTable(_cash);
            DataTable dtTicket = new DataTable(_ticket);

            dtCash.Columns.Add(txtCashCnt01.Name.ToString());
            dtCash.Columns.Add(txtCashCnt02.Name.ToString());
            dtCash.Columns.Add(txtCashCnt03.Name.ToString());
            dtCash.Columns.Add(txtCashCnt04.Name.ToString());
            dtCash.Columns.Add(txtCashCnt05.Name.ToString());
            dtCash.Columns.Add(txtCashCnt06.Name.ToString());
            dtCash.Columns.Add(txtCashCnt07.Name.ToString());
            dtCash.Columns.Add(txtCashCnt08.Name.ToString());
            dtCash.Columns.Add(txtCashCnt09.Name.ToString());

            dtCash.Columns.Add(txtCashAmt01.Name.ToString());
            dtCash.Columns.Add(txtCashAmt02.Name.ToString());
            dtCash.Columns.Add(txtCashAmt03.Name.ToString());
            dtCash.Columns.Add(txtCashAmt04.Name.ToString());
            dtCash.Columns.Add(txtCashAmt05.Name.ToString());
            dtCash.Columns.Add(txtCashAmt06.Name.ToString());
            dtCash.Columns.Add(txtCashAmt07.Name.ToString());
            dtCash.Columns.Add(txtCashAmt08.Name.ToString());
            dtCash.Columns.Add(txtCashAmt09.Name.ToString());

            dtTicket.Columns.Add(txtTicketCnt01.Name.ToString());
            dtTicket.Columns.Add(txtTicketCnt02.Name.ToString());
            dtTicket.Columns.Add(txtTicketCnt03.Name.ToString());
            /* Loc changed 11.09
            dtTicket.Columns.Add(txtTicketCnt04.Name.ToString());
            dtTicket.Columns.Add(txtTicketCnt05.Name.ToString());
            dtTicket.Columns.Add(txtTicketCnt06.Name.ToString());
            dtTicket.Columns.Add(txtTicketCnt07.Name.ToString());
            dtTicket.Columns.Add(txtTicketCnt08.Name.ToString());
            dtTicket.Columns.Add(txtTicketCnt09.Name.ToString());
            */

            dtTicket.Columns.Add(txtTicketAmt01.Name.ToString());
            dtTicket.Columns.Add(txtTicketAmt02.Name.ToString());
            dtTicket.Columns.Add(txtTicketAmt03.Name.ToString());
            /* Loc changed 11.09
            dtTicket.Columns.Add(txtTicketAmt04.Name.ToString());
            dtTicket.Columns.Add(txtTicketAmt05.Name.ToString());
            dtTicket.Columns.Add(txtTicketAmt06.Name.ToString());
            dtTicket.Columns.Add(txtTicketAmt07.Name.ToString());
            dtTicket.Columns.Add(txtTicketAmt08.Name.ToString());
            dtTicket.Columns.Add(txtTicketAmt09.Name.ToString());
            */
            dtCash.Rows.Add(new object[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" });                           //입력데이터
            dtCash.Rows.Add(new object[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" });                           //해당 입력값에서 Enter Key를 받았는지 여부
            dtCash.Rows.Add(new object[] { "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y" });         //입력여부 확인(y:기존 텍스트에 추가입력,n:기존 텍스트 지우고 새로입력)
            
            /* Loc changed 12.09
            dtTicket.Rows.Add(new object[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" });                         //입력 데이터
            dtTicket.Rows.Add(new object[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" });                         //해당 입력값에서 Enter Key를 받았는지 여부
            dtTicket.Rows.Add(new object[] { "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y", "y" });       //입력여부 확인(y:기존 텍스트에 추가입력,n:기존 텍스트 지우고 새로입력)
            dtTicket.Rows.Add(new object[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" });                         //티켓 Code값
            */

            dtTicket.Rows.Add(new object[] { "", "", "", "", "", "" });                         //입력 데이터
            dtTicket.Rows.Add(new object[] { "", "", "", "", "", "" });                         //해당 입력값에서 Enter Key를 받았는지 여부
            dtTicket.Rows.Add(new object[] { "y", "y", "y", "y", "y", "y" });       //입력여부 확인(y:기존 텍스트에 추가입력,n:기존 텍스트 지우고 새로입력)
            dtTicket.Rows.Add(new object[] { "", "", "", "", "", ""  });                         //티켓 Code값

            dsControl.Tables.Add(dtCash);
            dsControl.Tables.Add(dtTicket);
            #endregion

            //포커스
            txtCashCnt01.SetFocus();

            //마감입금 설정------------------------------
            m_Presenter = new IOM003presenter(this);
            
            // Loc changed 12.09
            // m_Presenter.GetTicketTitle();       //타사 상품권명 조회
            DisableFocus();

            //-------------------------------------------

            if (POSDeviceManager.CashDrawer != null && POSDeviceManager.CashDrawer.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened && POSDeviceManager.CashDrawer.Enabled)
            {
                //돈통 open
                POSDeviceManager.CashDrawer.OpenDrawer();
            }

            txtCashTotal.Focusable = false;
            txtTicketTotal.Focusable = false;
            txtTotalAmt.Focusable = false;
        }

        /// <summary>
        /// Key Event
        /// </summary>
        /// <param name="e"></param>
        void form_KeyEvent(WSWD.WmallPos.FX.Shared.OPOSKeyEventArgs e)
        {
            if (_bDisable)
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
            }
            else if (e.Key.OPOSKey == WSWD.WmallPos.FX.Shared.OPOSMapKeys.KEY_ENTER)
            {
                //등록
                #region KEY_ENTER

                #region 포커스 확인 및 값 셋팅

                if (txtCashAmt01.IsFocused)
                {
                    SetMathRecover(txtCashAmt01);

                    if (SetControlAmt(_cash, txtCashCnt01, txtCashAmt01, 0))
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

                    if (SetControlAmt(_cash, txtCashCnt02, txtCashAmt02, 50000))
                    {
                        txtCashCnt03.SetFocus();
                    }
                }
                else if (txtCashCnt03.IsFocused)
                {
                    SetRecover(txtCashCnt03);

                    if (SetControlAmt(_cash, txtCashCnt03, txtCashAmt03, 10000))
                    {
                        txtCashCnt04.SetFocus();
                    }
                }
                else if (txtCashCnt04.IsFocused)
                {
                    SetRecover(txtCashCnt04);

                    if (SetControlAmt(_cash, txtCashCnt04, txtCashAmt04, 5000))
                    {
                        txtCashCnt05.SetFocus();
                    }
                }
                else if (txtCashCnt05.IsFocused)
                {
                    SetRecover(txtCashCnt05);

                    if (SetControlAmt(_cash, txtCashCnt05, txtCashAmt05, 1000))
                    {
                        txtCashCnt06.SetFocus();
                    }
                }
                else if (txtCashCnt06.IsFocused)
                {
                    SetRecover(txtCashCnt06);

                    if (SetControlAmt(_cash, txtCashCnt06, txtCashAmt06, 500))
                    {
                        txtCashCnt07.SetFocus();
                    }
                }
                else if (txtCashCnt07.IsFocused)
                {
                    SetRecover(txtCashCnt07);

                    if (SetControlAmt(_cash, txtCashCnt07, txtCashAmt07, 100))
                    {
                        txtCashCnt08.SetFocus();
                    }
                }
                else if (txtCashCnt08.IsFocused)
                {
                    SetRecover(txtCashCnt08);

                    if (SetControlAmt(_cash, txtCashCnt08, txtCashAmt08, 50))
                    {
                        txtCashCnt09.SetFocus();
                    }
                }
                else if (txtCashCnt09.IsFocused)
                {
                    SetRecover(txtCashCnt09);

                    if (SetControlAmt(_cash, txtCashCnt09, txtCashAmt09, 10))
                    {
                        txtTicketCnt01.SetFocus();
                    }
                }
                else if (txtTicketCnt01.IsFocused)
                {
                    SetRecover(txtTicketCnt01);

                    if (SetControlAmt(_ticket, txtTicketCnt01, txtTicketAmt01, 0))
                    {
                        txtTicketAmt01.SetFocus();
                    }
                }
                else if (txtTicketCnt02.IsFocused)
                {
                    SetRecover(txtTicketCnt02);

                    if (SetControlAmt(_ticket, txtTicketCnt02, txtTicketAmt02, 0))
                    {
                        txtTicketAmt02.SetFocus();
                    }
                }
                else if (txtTicketCnt03.IsFocused)
                {
                    SetRecover(txtTicketCnt03);

                    if (SetControlAmt(_ticket, txtTicketCnt03, txtTicketAmt03, 0))
                    {
                        txtTicketAmt03.SetFocus();
                    }
                }
                /* Loc changed 12.09
                else if (txtTicketCnt04.IsFocused)
                {
                    SetRecover(txtTicketCnt04);

                    if (SetControlAmt(_ticket, txtTicketCnt04, txtTicketAmt04, 0))
                    {
                        txtTicketAmt04.SetFocus();
                    }
                }
                else if (txtTicketCnt05.IsFocused)
                {
                    SetRecover(txtTicketCnt05);

                    if (SetControlAmt(_ticket, txtTicketCnt05, txtTicketAmt05, 0))
                    {
                        txtTicketAmt05.SetFocus();
                    }
                }
                else if (txtTicketCnt06.IsFocused)
                {
                    SetRecover(txtTicketCnt06);

                    if (SetControlAmt(_ticket, txtTicketCnt06, txtTicketAmt06, 0))
                    {
                        txtTicketAmt06.SetFocus();
                    }
                }
                else if (txtTicketCnt07.IsFocused)
                {
                    SetRecover(txtTicketCnt07);

                    if (SetControlAmt(_ticket, txtTicketCnt07, txtTicketAmt07, 0))
                    {
                        txtTicketAmt07.SetFocus();
                    }
                }
                else if (txtTicketCnt08.IsFocused)
                {
                    SetRecover(txtTicketCnt08);

                    if (SetControlAmt(_ticket, txtTicketCnt08, txtTicketAmt08, 0))
                    {
                        txtTicketAmt08.SetFocus();
                    }
                }
                else if (txtTicketCnt09.IsFocused)
                {
                    SetRecover(txtTicketCnt09);

                    if (SetControlAmt(_ticket, txtTicketCnt09, txtTicketAmt09, 0))
                    {
                        txtTicketAmt09.SetFocus();
                    }
                }*/
                else if (txtTicketAmt01.IsFocused)
                {
                    SetMathRecover(txtTicketAmt01);

                    if (SetControlAmt(_ticket, txtTicketCnt01, txtTicketAmt01, 0))
                    {
                        SetFocus(txtTicketAmt01);
                    }
                }
                else if (txtTicketAmt02.IsFocused)
                {
                    SetMathRecover(txtTicketAmt02);

                    if (SetControlAmt(_ticket, txtTicketCnt02, txtTicketAmt02, 0))
                    {
                        SetFocus(txtTicketAmt02);
                    }
                }
                else if (txtTicketAmt03.IsFocused)
                {
                    SetMathRecover(txtTicketAmt03);

                    if (SetControlAmt(_ticket, txtTicketCnt03, txtTicketAmt03, 0))
                    {
                        SetFocus(txtTicketAmt03);
                    }
                }
                /* Loc changed 12.09
                else if (txtTicketAmt04.IsFocused)
                {
                    SetMathRecover(txtTicketAmt04);

                    if (SetControlAmt(_ticket, txtTicketCnt04, txtTicketAmt04, 0))
                    {
                        SetFocus(txtTicketAmt04);
                    }
                }
                else if (txtTicketAmt05.IsFocused)
                {
                    SetMathRecover(txtTicketAmt05);

                    if (SetControlAmt(_ticket, txtTicketCnt05, txtTicketAmt05, 0))
                    {
                        SetFocus(txtTicketAmt05);
                    }
                }
                else if (txtTicketAmt06.IsFocused)
                {
                    SetMathRecover(txtTicketAmt06);

                    if (SetControlAmt(_ticket, txtTicketCnt06, txtTicketAmt06, 0))
                    {
                        SetFocus(txtTicketAmt06);
                    }
                }
                else if (txtTicketAmt07.IsFocused)
                {
                    SetMathRecover(txtTicketAmt07);

                    if (SetControlAmt(_ticket, txtTicketCnt07, txtTicketAmt07, 0))
                    {
                        SetFocus(txtTicketAmt07);
                    }
                }
                else if (txtTicketAmt08.IsFocused)
                {
                    SetMathRecover(txtTicketAmt08);

                    if (SetControlAmt(_ticket, txtTicketCnt08, txtTicketAmt08, 0))
                    {
                        SetFocus(txtTicketAmt08);
                    }
                }
                else if (txtTicketAmt09.IsFocused)
                {
                    SetMathRecover(txtTicketAmt09);

                    if (SetControlAmt(_ticket, txtTicketCnt09, txtTicketAmt09, 0))
                    {
                        SetFocus(txtTicketAmt09);
                    }
                }*/

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
                    if (dsControl.Tables[_cash].Rows[0][txtCashCnt01.Name.ToString()] != null)
                    {
                        if (txtCashCnt01.Text.ToString() != dsControl.Tables[_cash].Rows[0][txtCashCnt01.Name.ToString()].ToString())
                        {
                            txtCashCnt01.Text = dsControl.Tables[_cash].Rows[0][txtCashCnt01.Name.ToString()].ToString();
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
                /* Loc changed 12.09
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
                 */
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
                /* Loc changed 12.09
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
                }*/

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
                    SetBackSpaveRecover(e, txtCashCnt01);
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
                /* Loc changed 12.09
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
                }*/
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
                /* Loc changed 12.09
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
                }*/

                #endregion

                #endregion
            }
            else if (!e.IsControlKey)
            {
                #region 현금 및 상품권 유효성 검사

                #region 현금

                if (txtCashCnt01.IsFocused)
                {
                    ValidationLength(e, txtCashCnt01);
                }
                else if (txtCashCnt02.IsFocused)
                {
                    ValidationLength(e, txtCashCnt02);
                }
                else if (txtCashCnt03.IsFocused)
                {
                    ValidationLength(e, txtCashCnt03);
                }
                else if (txtCashCnt04.IsFocused)
                {
                    ValidationLength(e, txtCashCnt04);
                }
                else if (txtCashCnt05.IsFocused)
                {
                    ValidationLength(e, txtCashCnt05);
                }
                else if (txtCashCnt06.IsFocused)
                {
                    ValidationLength(e, txtCashCnt06);
                }
                else if (txtCashCnt07.IsFocused)
                {
                    ValidationLength(e, txtCashCnt07);
                }
                else if (txtCashCnt08.IsFocused)
                {
                    ValidationLength(e, txtCashCnt08);
                }
                else if (txtCashCnt09.IsFocused)
                {
                    ValidationLength(e, txtCashCnt09);
                }
                else if (txtCashAmt01.IsFocused)
                {
                    ValidationLength(e, txtCashAmt01);
                }

                #endregion

                #region 상품권

                else if (txtTicketCnt01.IsFocused)
                {
                    ValidationLength(e, txtTicketCnt01);
                }
                else if (txtTicketCnt02.IsFocused)
                {
                    ValidationLength(e, txtTicketCnt02);
                }
                else if (txtTicketCnt03.IsFocused)
                {
                    ValidationLength(e, txtTicketCnt03);
                }
                /* Loc changed 12.09
                else if (txtTicketCnt04.IsFocused)
                {
                    ValidationLength(e, txtTicketCnt04);
                }
                else if (txtTicketCnt05.IsFocused)
                {
                    ValidationLength(e, txtTicketCnt05);
                }
                else if (txtTicketCnt06.IsFocused)
                {
                    ValidationLength(e, txtTicketCnt06);
                }
                else if (txtTicketCnt07.IsFocused)
                {
                    ValidationLength(e, txtTicketCnt07);
                }
                else if (txtTicketCnt08.IsFocused)
                {
                    ValidationLength(e, txtTicketCnt08);
                }
                else if (txtTicketCnt09.IsFocused)
                {
                    ValidationLength(e, txtTicketCnt09);
                }*/
                else if (txtTicketAmt01.IsFocused)
                {
                    ValidationLength(e, txtTicketAmt01);
                }
                else if (txtTicketAmt02.IsFocused)
                {
                    ValidationLength(e, txtTicketAmt02);
                }
                else if (txtTicketAmt03.IsFocused)
                {
                    ValidationLength(e, txtTicketAmt03);
                }
                /* Loc changed 12.09
                else if (txtTicketAmt04.IsFocused)
                {
                    ValidationLength(e, txtTicketAmt04);
                }
                else if (txtTicketAmt05.IsFocused)
                {
                    ValidationLength(e, txtTicketAmt05);
                }
                else if (txtTicketAmt06.IsFocused)
                {
                    ValidationLength(e, txtTicketAmt06);
                }
                else if (txtTicketAmt07.IsFocused)
                {
                    ValidationLength(e, txtTicketAmt07);
                }
                else if (txtTicketAmt08.IsFocused)
                {
                    ValidationLength(e, txtTicketAmt08);
                }
                else if (txtTicketAmt09.IsFocused)
                {
                    ValidationLength(e, txtTicketAmt09);
                }*/

                #endregion

                #endregion
            }
            else if (e.Key.OPOSKey == OPOSMapKeys.KEY_DOWN || e.Key.OPOSKey == OPOSMapKeys.KEY_UP)
            {
                e.IsHandled = true;
            }
        }

        /// <summary>
        /// 등록완료 Button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;

            SetControlDisable(true);

            try
            {
                if (TypeHelper.ToInt64(txtTotalAmt.Text) <= 0)
                {
                    txtCashCnt01.SetFocus();
                    SetControlDisable(false);
                    ShowMessageBox(MessageDialogType.Warning, null, strMsgErr_01);
                    txtCashCnt01.SetFocus();
                    return;
                }

                DataRow drC = dsControl.Tables["cash"].Rows[0];        //현금
                DataRow drT = dsControl.Tables["ticket"].Rows[0];      //상품권
                DataRow drTNm = dsControl.Tables["ticket"].Rows[3];    //상품권 명

                //수표 매수 및 금액 확인
                if (TypeHelper.ToInt64(drC["txtCashCnt01"]) > 0)
                {
                    if (TypeHelper.ToInt64(drC["txtCashAmt01"]) <= 0)
                    {
                        txtCashAmt01.SetFocus();
                        
                        SetControlDisable(false);
                        ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, strMsg05));
                        txtCashAmt01.SetFocus();
                        return;
                    }
                }

                if (TypeHelper.ToInt64(drC["txtCashAmt01"]) > 0)
                {
                    if (TypeHelper.ToInt64(drC["txtCashCnt01"]) <= 0)
                    {
                        txtCashCnt01.SetFocus();
                        SetControlDisable(false);
                        ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, strMsg05));
                        txtCashCnt01.SetFocus();
                        return;
                    }
                }

                //상품교환권 매수 및 금액 확인
                if (TypeHelper.ToInt64(drT["txtTicketCnt01"]) > 0)
                {
                    if (TypeHelper.ToInt64(drT["txtTicketAmt01"]) <= 0)
                    {
                        txtTicketAmt01.SetFocus();
                        SetControlDisable(false);
                        ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, strMsg06));
                        txtTicketAmt01.SetFocus();
                        return;
                    }
                }

                if (TypeHelper.ToInt64(drT["txtTicketAmt01"]) > 0)
                {
                    if (TypeHelper.ToInt64(drT["txtTicketCnt01"]) <= 0)
                    {
                        txtTicketCnt01.SetFocus();
                        SetControlDisable(false);
                        ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, strMsg06));
                        txtTicketCnt01.SetFocus();
                        return;
                    }
                }

                if (TypeHelper.ToString(drTNm["txtTicketAmt02"]) != "")
                {
                    if (TypeHelper.ToInt64(drT["txtTicketCnt02"]) > 0)
                    {
                        if (TypeHelper.ToInt64(drT["txtTicketAmt02"]) <= 0)
                        {
                            txtTicketAmt02.SetFocus();
                            SetControlDisable(false);
                            ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, TypeHelper.ToString(lblTicket02.Text)));
                            txtTicketAmt02.SetFocus();
                            return;
                        }
                    }

                    if (TypeHelper.ToInt64(drT["txtTicketAmt02"]) > 0)
                    {
                        if (TypeHelper.ToInt64(drT["txtTicketCnt02"]) <= 0)
                        {
                            txtTicketCnt02.SetFocus();
                            SetControlDisable(false);
                            ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, TypeHelper.ToString(lblTicket02.Text)));
                            txtTicketCnt02.SetFocus();
                            return;
                        }
                    }
                }

                if (TypeHelper.ToString(drTNm["txtTicketAmt03"]) != "")
                {
                    if (TypeHelper.ToInt64(drT["txtTicketCnt03"]) > 0)
                    {
                        if (TypeHelper.ToInt64(drT["txtTicketAmt03"]) <= 0)
                        {
                            txtTicketAmt03.SetFocus();
                            SetControlDisable(false);
                            ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, TypeHelper.ToString(lblTicket03.Text)));
                            txtTicketAmt03.SetFocus();
                            return;
                        }
                    }

                    if (TypeHelper.ToInt64(drT["txtTicketAmt03"]) > 0)
                    {
                        if (TypeHelper.ToInt64(drT["txtTicketCnt03"]) <= 0)
                        {
                            txtTicketCnt03.SetFocus();
                            SetControlDisable(false);
                            ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, TypeHelper.ToString(lblTicket03.Text)));
                            txtTicketCnt03.SetFocus();
                            return;
                        }
                    }
                }
                /* Loc changed 12.09
                if (TypeHelper.ToString(drTNm["txtTicketAmt04"]) != "")
                {
                    if (TypeHelper.ToInt64(drT["txtTicketCnt04"]) > 0)
                    {
                        if (TypeHelper.ToInt64(drT["txtTicketAmt04"]) <= 0)
                        {
                            txtTicketAmt04.SetFocus();
                            SetControlDisable(false);
                            ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, TypeHelper.ToString(lblTicket04.Text)));
                            txtTicketAmt04.SetFocus();
                            return;
                        }
                    }

                    if (TypeHelper.ToInt64(drT["txtTicketAmt04"]) > 0)
                    {
                        if (TypeHelper.ToInt64(drT["txtTicketCnt04"]) <= 0)
                        {
                            txtTicketCnt04.SetFocus();
                            SetControlDisable(false);
                            ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, TypeHelper.ToString(lblTicket04.Text)));
                            txtTicketCnt04.SetFocus();
                            return;
                        }
                    }
                }

                if (TypeHelper.ToString(drTNm["txtTicketAmt05"]) != "")
                {
                    if (TypeHelper.ToInt64(drT["txtTicketCnt05"]) > 0)
                    {
                        if (TypeHelper.ToInt64(drT["txtTicketAmt05"]) <= 0)
                        {
                            txtTicketAmt05.SetFocus();
                            SetControlDisable(false);
                            ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, TypeHelper.ToString(lblTicket05.Text)));
                            txtTicketAmt05.SetFocus();
                            return;
                        }
                    }

                    if (TypeHelper.ToInt64(drT["txtTicketAmt05"]) > 0)
                    {
                        if (TypeHelper.ToInt64(drT["txtTicketCnt05"]) <= 0)
                        {
                            txtTicketCnt05.SetFocus();
                            SetControlDisable(false);
                            ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, TypeHelper.ToString(lblTicket05.Text)));
                            txtTicketCnt05.SetFocus();
                            return;
                        }
                    }
                }

                if (TypeHelper.ToString(drTNm["txtTicketAmt06"]) != "")
                {
                    if (TypeHelper.ToInt64(drT["txtTicketCnt06"]) > 0)
                    {
                        if (TypeHelper.ToInt64(drT["txtTicketAmt06"]) <= 0)
                        {
                            txtTicketAmt06.SetFocus();
                            SetControlDisable(false);
                            ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, TypeHelper.ToString(lblTicket06.Text)));
                            txtTicketAmt06.SetFocus();
                            return;
                        }
                    }

                    if (TypeHelper.ToInt64(drT["txtTicketAmt06"]) > 0)
                    {
                        if (TypeHelper.ToInt64(drT["txtTicketCnt06"]) <= 0)
                        {
                            txtTicketCnt06.SetFocus();
                            SetControlDisable(false);
                            ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, TypeHelper.ToString(lblTicket06.Text)));
                            txtTicketCnt06.SetFocus();
                            return;
                        }
                    }
                }

                if (TypeHelper.ToString(drTNm["txtTicketAmt07"]) != "")
                {
                    if (TypeHelper.ToInt64(drT["txtTicketCnt07"]) > 0)
                    {
                        if (TypeHelper.ToInt64(drT["txtTicketAmt07"]) <= 0)
                        {
                            txtTicketAmt07.SetFocus();
                            SetControlDisable(false);
                            ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, TypeHelper.ToString(lblTicket07.Text)));
                            txtTicketAmt07.SetFocus();
                            return;
                        }
                    }

                    if (TypeHelper.ToInt64(drT["txtTicketAmt07"]) > 0)
                    {
                        if (TypeHelper.ToInt64(drT["txtTicketCnt07"]) <= 0)
                        {
                            txtTicketCnt07.SetFocus();
                            SetControlDisable(false);
                            ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, TypeHelper.ToString(lblTicket07.Text)));
                            txtTicketCnt07.SetFocus();
                            return;
                        }
                    }
                }

                if (TypeHelper.ToString(drTNm["txtTicketAmt08"]) != "")
                {
                    if (TypeHelper.ToInt64(drT["txtTicketCnt08"]) > 0)
                    {
                        if (TypeHelper.ToInt64(drT["txtTicketAmt08"]) <= 0)
                        {
                            txtTicketAmt08.SetFocus();
                            SetControlDisable(false);
                            ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, TypeHelper.ToString(lblTicket08.Text)));
                            txtTicketAmt08.SetFocus();
                            return;
                        }
                    }

                    if (TypeHelper.ToInt64(drT["txtTicketAmt08"]) > 0)
                    {
                        if (TypeHelper.ToInt64(drT["txtTicketCnt08"]) <= 0)
                        {
                            txtTicketCnt08.SetFocus();
                            SetControlDisable(false);
                            ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, TypeHelper.ToString(lblTicket08.Text)));
                            txtTicketCnt08.SetFocus();
                            return;
                        }
                    }
                }

                if (TypeHelper.ToString(drTNm["txtTicketAmt09"]) != "")
                {
                    if (TypeHelper.ToInt64(drT["txtTicketCnt09"]) > 0)
                    {
                        if (TypeHelper.ToInt64(drT["txtTicketAmt09"]) <= 0)
                        {
                            txtTicketAmt09.SetFocus();
                            SetControlDisable(false);
                            ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, TypeHelper.ToString(lblTicket09.Text)));
                            txtTicketAmt09.SetFocus();
                            return;
                        }
                    }

                    if (TypeHelper.ToInt64(drT["txtTicketAmt09"]) > 0)
                    {
                        if (TypeHelper.ToInt64(drT["txtTicketCnt09"]) <= 0)
                        {
                            txtTicketCnt09.SetFocus();
                            SetControlDisable(false);
                            ShowMessageBox(MessageDialogType.Warning, null, string.Format(strMsgErr, TypeHelper.ToString(lblTicket09.Text)));
                            txtTicketCnt09.SetFocus();
                            return;
                        }
                    }
                }*/

                //마감입금 차수 및 차수 합계 금액 저장
                m_Presenter.SetCloseAmt(dsControl);
            }
            catch (Exception ex)
            {
                SetControlDisable(false);
                LogUtils.Instance.LogException(ex);
            }
        }

        /// <summary>
        /// 닫기 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (_bDisable)
            {
                return;
            }

            this.Close();
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// InputControl 자릿수 유효성 검사 및 InputControl이 새롭게 focus를 받았다면 기존 텍스트는 지우고 새로 입력한 값으로 변경
        /// </summary>
        /// <param name="txtControl">현재 포커스 InputText</param>
        private void ValidationLength(OPOSKeyEventArgs e, InputText txt)
        {
            string strTB = txt.Name.ToString().ToLower().Contains(_cash) ? _cash : _ticket;

            if (dsControl.Tables[strTB].Rows[2][txt.Name.ToString()].ToString() == "n")
            {
                if (txt.Text.Length > 0)
                {
                    txt.Text = "";
                }

                dsControl.Tables[strTB].Rows[2][txt.Name.ToString()] = "y";
            }
        }

        /// <summary>
        /// 기존 값으로 복원
        /// </summary>
        /// <param name="txtControl">InputControl</param>
        private void SetRecover(InputText txt)
        {
            string strCOL = txt.Name.ToString();
            string strTB = strCOL.ToLower().Contains(_cash) ? _cash : _ticket;

            DataRow dr = dsControl.Tables[strTB].Rows[0];
            DataRow dr_YN = dsControl.Tables[strTB].Rows[1];

            if (dr_YN[strCOL] == null || dr_YN[strCOL].ToString() == "")
            {
                dr_YN[strCOL] = "y";
                dr[strCOL] = txt.Text.ToString();
            }
            else
            {
                if (dr[strCOL].ToString() != "" && dr[strCOL].ToString() != "0")
                {
                    if (txt.Text.ToString() == "")
                    {
                        txt.Text = dr[strCOL].ToString(); //기존에 입력했던 값이 있다면 기존 입력값으로 복원(지우려면 0을 입력)
                    }
                    else
                    {
                        dr[strCOL] = txt.Text.ToString() == "0" ? "" : txt.Text.ToString();
                    }
                }
                else
                {
                    dr[strCOL] = txt.Text.ToString() == "0" ? "" : txt.Text.ToString();
                }
            }

            dsControl.Tables[strTB].Rows[2][strCOL] = "n";
        }

        /// <summary>
        /// 기존 값으로 복원
        /// </summary>
        /// <param name="txtControl">InputControl</param>
        private void SetMathRecover(InputText txt)
        {
            int iMath = 0;
            string strCOL = txt.Name.ToString();
            string strTB = strCOL.ToLower().Contains(_cash) ? _cash : _ticket;

            DataRow dr = dsControl.Tables[strTB].Rows[0];
            DataRow dr_YN = dsControl.Tables[strTB].Rows[1];

            if (dr_YN[strCOL] == null || dr_YN[strCOL].ToString() == "")
            {
                dr_YN[strCOL] = "y";
                iMath = GetMath(txt.Text.ToString());
                dr[strCOL] = iMath.ToString();
                txt.Text = iMath.ToString();
            }
            else
            {
                if (dr[strCOL].ToString() != "" && dr[strCOL].ToString() != "0")
                {
                    if (txt.Text.ToString() == "")
                    {
                        iMath = GetMath(dr[strCOL].ToString());
                        txt.Text = iMath.ToString(); //기존에 입력했던 값이 있다면 기존 입력값으로 복원(지우려면 0을 입력)
                    }
                    else
                    {
                        iMath = GetMath(txt.Text.ToString());
                        dr[strCOL] = iMath == 0 ? "" : iMath.ToString();
                        txt.Text = iMath == 0 ? "" : iMath.ToString();
                    }
                }
                else
                {
                    iMath = GetMath(txt.Text.ToString());
                    dr[strCOL] = iMath == 0 ? "" : iMath.ToString();
                    txt.Text = iMath == 0 ? "" : iMath.ToString();
                }
            }

            dsControl.Tables[strTB].Rows[2][strCOL] = "n";
        }

        /// <summary>
        /// BackSpace Key Event
        /// </summary>
        /// <param name="e"></param>
        /// <param name="txtControl"></param>
        private void SetBackSpaveRecover(OPOSKeyEventArgs e, InputText txtControl)
        {
            string strCOL = txtControl.Name.ToString();
            string strTB = strCOL.ToLower().Contains(_cash) ? _cash : _ticket;

            string strValue = dsControl.Tables[strTB].Rows[0][strCOL].ToString();
            string strYN = dsControl.Tables[strTB].Rows[1][strCOL].ToString();

            if (txtControl.Text.Length > 0 && strYN != "" && txtControl.Text.ToString() == strValue)
            {
                txtControl.Text = "";
            }
        }

        /// <summary>
        /// Clear Key Event
        /// </summary>
        /// <param name="e"></param>
        /// <param name="txtControl"></param>
        /// <param name="txtFocusControl"></param>
        private void SetClearRecover(OPOSKeyEventArgs e, InputText txtControl, InputText txtFocusControl)
        {
            string strCOL = txtControl.Name.ToString();
            string strTB = strCOL.ToLower().Contains(_cash) ? _cash : _ticket;

            string strValue = dsControl.Tables[strTB].Rows[0][strCOL].ToString() == "0" ? "" : dsControl.Tables[strTB].Rows[0][strCOL].ToString();
            string strYN = dsControl.Tables[strTB].Rows[1][strCOL].ToString();

            if (strYN == "")
            {
                if (txtControl.Text.Length <= 0)
                {
                    txtFocusControl.SetFocus();
                    e.IsHandled = true;
                }
            }
            else
            {
                if ((txtControl.Text.ToString() == "0" ? "" : txtControl.Text.ToString()) == strValue)
                {
                    txtFocusControl.SetFocus();
                    e.IsHandled = true;
                }
                else
                {
                    txtControl.Text = strValue;
                    dsControl.Tables[strTB].Rows[2][strCOL] = "n";
                    e.IsHandled = true;
                }
            }
        }

        /// <summary>
        /// Loc added 12.09
        /// </summary>
        private void DisableFocus()
        {
            txtTicketCnt02.Focusable = true;
            txtTicketAmt02.Focusable = true;
            txtTicketAmt02.Tag = "00";

            lblTicket02.Tag = "00";
            lblTicket02.Text = "할인쿠폰";

            txtTicketCnt03.Focusable = true;
            txtTicketAmt03.Focusable = true;
            lblTicket03.Tag = "01";
            lblTicket03.Text = "타사상품권";

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

            txtTicketCnt04.Visible = false;
            txtTicketAmt04.Visible = false;
            txtTicketCnt05.Visible = false;
            txtTicketAmt05.Visible = false;
            txtTicketCnt06.Visible = false;
            txtTicketAmt06.Visible = false;
            txtTicketCnt07.Visible = false;
            txtTicketAmt07.Visible = false;
            txtTicketCnt08.Visible = false;
            txtTicketAmt08.Visible = false;
            txtTicketCnt09.Visible = false;
            txtTicketAmt09.Visible = false;
        }

        /// <summary>
        /// 타사 상품권명 조회 및 셋팅
        /// </summary>
        /// <param name="ds">타사 상품권명 조회 결과</param>
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

            lblTicket02.Tag = null;
            lblTicket03.Tag = null;
            lblTicket04.Tag = null;
            lblTicket05.Tag = null;
            lblTicket06.Tag = null;
            lblTicket07.Tag = null;
            lblTicket08.Tag = null;
            lblTicket09.Tag = null;

            lblTicket02.Text = "";
            lblTicket03.Text = "";
            lblTicket04.Text = "";
            lblTicket05.Text = "";
            lblTicket06.Text = "";
            lblTicket07.Text = "";
            lblTicket08.Text = "";
            lblTicket09.Text = "";

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
                            lblTicket02.Tag = dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "";
                            lblTicket02.Text = dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "";
                            dsControl.Tables[_ticket].Rows[3][txtTicketAmt02.Name.ToString()] = string.Format("{0}:{1}", dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "", dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "");
                            break;
                        case 1:
                            txtTicketCnt03.Focusable = true;
                            txtTicketAmt03.Focusable = true;
                            lblTicket03.Tag = dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "";
                            lblTicket03.Text = dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "";
                            dsControl.Tables[_ticket].Rows[3][txtTicketAmt03.Name.ToString()] = string.Format("{0}:{1}", dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "", dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "");
                            break;
                        case 2:
                            txtTicketCnt04.Focusable = true;
                            txtTicketAmt04.Focusable = true;
                            lblTicket04.Tag = dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "";
                            lblTicket04.Text = dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "";
                            dsControl.Tables[_ticket].Rows[3][txtTicketAmt04.Name.ToString()] = string.Format("{0}:{1}", dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "", dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "");
                            break;
                        case 3:
                            txtTicketCnt05.Focusable = true;
                            txtTicketAmt05.Focusable = true;
                            lblTicket05.Tag = dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "";
                            lblTicket05.Text = dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "";
                            dsControl.Tables[_ticket].Rows[3][txtTicketAmt05.Name.ToString()] = string.Format("{0}:{1}", dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "", dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "");
                            break;
                        case 4:
                            txtTicketCnt06.Focusable = true;
                            txtTicketAmt06.Focusable = true;
                            lblTicket06.Tag = dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "";
                            lblTicket06.Text = dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "";
                            dsControl.Tables[_ticket].Rows[3][txtTicketAmt06.Name.ToString()] = string.Format("{0}:{1}", dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "", dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "");
                            break;
                        case 5:
                            txtTicketCnt07.Focusable = true;
                            txtTicketAmt07.Focusable = true;
                            lblTicket07.Tag = dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "";
                            lblTicket07.Text = dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "";
                            dsControl.Tables[_ticket].Rows[3][txtTicketAmt07.Name.ToString()] = string.Format("{0}:{1}", dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "", dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "");
                            break;
                        case 6:
                            txtTicketCnt08.Focusable = true;
                            txtTicketAmt08.Focusable = true;
                            lblTicket08.Tag = dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "";
                            lblTicket08.Text = dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "";
                            dsControl.Tables[_ticket].Rows[3][txtTicketAmt08.Name.ToString()] = string.Format("{0}:{1}", dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "", dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "");
                            break;
                        case 7:
                            txtTicketCnt09.Focusable = true;
                            txtTicketAmt09.Focusable = true;
                            lblTicket09.Tag = dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "";
                            lblTicket09.Text = dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "";
                            dsControl.Tables[_ticket].Rows[3][txtTicketAmt09.Name.ToString()] = string.Format("{0}:{1}", dr["KD_GIFT"] != null ? dr["KD_GIFT"].ToString() : "", dr["NM_GIFT"] != null ? dr["NM_GIFT"].ToString() : "");
                            break;
                        default:
                            break;
                    }

                    iRow++;
                }
            }
        }

        /// <summary>
        /// 금액 컨트롤 초기화
        /// </summary>
        private void SetInitControl(bool bLoad)
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
            /* Loc changed 12.09
            txtTicketCnt04.Text = "";
            txtTicketCnt05.Text = "";
            txtTicketCnt06.Text = "";
            txtTicketCnt07.Text = "";
            txtTicketCnt08.Text = "";
            txtTicketCnt09.Text = "";
            */

            txtTicketAmt01.Text = "";
            txtTicketAmt02.Text = "";
            txtTicketAmt03.Text = "";
            /* Loc changed 12.09
            txtTicketAmt04.Text = "";
            txtTicketAmt05.Text = "";
            txtTicketAmt06.Text = "";
            txtTicketAmt07.Text = "";
            txtTicketAmt08.Text = "";
            txtTicketAmt09.Text = "";
            */

            txtCashTotal.Text = "0";
            txtTicketTotal.Text = "0";
            txtTotalAmt.Text = "0";

            if (bLoad)
            {
                lblTicket02.Tag = "";
                lblTicket03.Tag = "";
                /* Loc changed 12.09
                lblTicket04.Tag = "";
                lblTicket05.Tag = "";
                lblTicket06.Tag = "";
                lblTicket07.Tag = "";
                lblTicket08.Tag = "";
                lblTicket09.Tag = "";*/

                lblTicket02.Text = "";
                lblTicket03.Text = "";
                /* Loc changed 12.09
                lblTicket04.Text = "";
                lblTicket05.Text = "";
                lblTicket06.Text = "";
                lblTicket07.Text = "";
                lblTicket08.Text = "";
                lblTicket09.Text = "";*/
            }
            else
            {
                btnSave.Enabled = true;

                txtCashCnt01.SetFocus();
            }
        }

        /// <summary>
        /// 상품권 관련 포커스 설정
        /// </summary>
        /// <param name="txtNowFoucs"></param>
        private void SetFocus(InputText txt)
        {
            if (txt == txtTicketAmt01)
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
            else if (txt == txtTicketAmt02)
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
            else if (txt == txtTicketAmt03)
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
            /* Loc changed 12.09
            else if (txt == txtTicketAmt04)
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
            else if (txt == txtTicketAmt05)
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
            else if (txt == txtTicketAmt06)
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
            else if (txt == txtTicketAmt07)
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
            else if (txt == txtTicketAmt08)
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
            else if (txt == txtTicketAmt09)
            {
                txtCashCnt01.SetFocus();
            }*/
        }

        /// <summary>
        /// 컨트롤 유횽성 검사 및 합계 구하기
        /// </summary>
        /// <param name="strGubun">현금및상품권구분(c:현금, t:상품권)</param>
        /// <param name="txtCnt">현금및상품권 매수 컨트롤</param>
        /// <param name="txtAmt">현금및상품권 금액 컨트롤</param>
        /// <param name="iCash">금액권구분(예: 오만원, 만원, 오천원...)</param>
        /// <returns></returns>
        private bool SetControlAmt(string strGubun, InputText txtCnt, InputText txtAmt, int iCash)
        {
            Int64 iTempTotal = 0;

            if (strGubun == _cash)
            {
                //수표가 아닐경우 금액권 * 매수
                if (txtCnt.Name.ToString() != txtCashCnt01.Name.ToString())
                {
                    int iTemp = GetData(dsControl.Tables[_cash].Rows[0][txtCnt.Name.ToString()]) * iCash;
                    txtAmt.Text = string.Format("{0}", iTemp > 0 ? iTemp.ToString() : "");
                    dsControl.Tables[_cash].Rows[0][txtAmt.Name.ToString()] = iTemp;
                }

                if (GetData(dsControl.Tables[_cash].Rows[0][txtCnt.Name.ToString()]) <= 0)
                {
                    txtCnt.Text = "";
                }

                if (GetData(dsControl.Tables[_cash].Rows[0][txtAmt.Name.ToString()]) <= 0)
                {
                    txtAmt.Text = "";
                }

                //현금 합계 구하기--------------------------------------------------------------------------------------------------------------------------------------------
                iTempTotal += GetData(dsControl.Tables[_cash].Rows[0][txtCashCnt01.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_cash].Rows[0][txtCashAmt01.Name.ToString()]) : 0;
                iTempTotal += GetData(dsControl.Tables[_cash].Rows[0][txtCashCnt02.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_cash].Rows[0][txtCashAmt02.Name.ToString()]) : 0;
                iTempTotal += GetData(dsControl.Tables[_cash].Rows[0][txtCashCnt03.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_cash].Rows[0][txtCashAmt03.Name.ToString()]) : 0;
                iTempTotal += GetData(dsControl.Tables[_cash].Rows[0][txtCashCnt04.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_cash].Rows[0][txtCashAmt04.Name.ToString()]) : 0;
                iTempTotal += GetData(dsControl.Tables[_cash].Rows[0][txtCashCnt05.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_cash].Rows[0][txtCashAmt05.Name.ToString()]) : 0;
                iTempTotal += GetData(dsControl.Tables[_cash].Rows[0][txtCashCnt06.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_cash].Rows[0][txtCashAmt06.Name.ToString()]) : 0;
                iTempTotal += GetData(dsControl.Tables[_cash].Rows[0][txtCashCnt07.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_cash].Rows[0][txtCashAmt07.Name.ToString()]) : 0;
                iTempTotal += GetData(dsControl.Tables[_cash].Rows[0][txtCashCnt08.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_cash].Rows[0][txtCashAmt08.Name.ToString()]) : 0;
                iTempTotal += GetData(dsControl.Tables[_cash].Rows[0][txtCashCnt09.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_cash].Rows[0][txtCashAmt09.Name.ToString()]) : 0;
                txtCashTotal.Text = iTempTotal.ToString();
                //------------------------------------------------------------------------------------------------------------------------------------------------------------
            }
            else if (strGubun == _ticket)
            {
                if (GetData(dsControl.Tables[_ticket].Rows[0][txtCnt.Name.ToString()]) <= 0)
                {
                    txtCnt.Text = "";
                }

                if (GetData(dsControl.Tables[_ticket].Rows[0][txtAmt.Name.ToString()]) <= 0)
                {
                    txtAmt.Text = "";
                }

                //티켓 합계 구하기---------------------------------------------------------------------------------------------------------------------------------------------------------------
                iTempTotal += GetData(dsControl.Tables[_ticket].Rows[0][txtTicketCnt01.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_ticket].Rows[0][txtTicketAmt01.Name.ToString()]) : 0;
                iTempTotal += GetData(dsControl.Tables[_ticket].Rows[0][txtTicketCnt02.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_ticket].Rows[0][txtTicketAmt02.Name.ToString()]) : 0;
                iTempTotal += GetData(dsControl.Tables[_ticket].Rows[0][txtTicketCnt03.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_ticket].Rows[0][txtTicketAmt03.Name.ToString()]) : 0;
                /* Loc changed 12.09
                iTempTotal += GetData(dsControl.Tables[_ticket].Rows[0][txtTicketCnt04.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_ticket].Rows[0][txtTicketAmt04.Name.ToString()]) : 0;
                iTempTotal += GetData(dsControl.Tables[_ticket].Rows[0][txtTicketCnt05.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_ticket].Rows[0][txtTicketAmt05.Name.ToString()]) : 0;
                iTempTotal += GetData(dsControl.Tables[_ticket].Rows[0][txtTicketCnt06.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_ticket].Rows[0][txtTicketAmt06.Name.ToString()]) : 0;
                iTempTotal += GetData(dsControl.Tables[_ticket].Rows[0][txtTicketCnt07.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_ticket].Rows[0][txtTicketAmt07.Name.ToString()]) : 0;
                iTempTotal += GetData(dsControl.Tables[_ticket].Rows[0][txtTicketCnt08.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_ticket].Rows[0][txtTicketAmt08.Name.ToString()]) : 0;
                iTempTotal += GetData(dsControl.Tables[_ticket].Rows[0][txtTicketCnt09.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_ticket].Rows[0][txtTicketAmt09.Name.ToString()]) : 0;
                 */
                txtTicketTotal.Text = iTempTotal.ToString();
                //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            }

            //총합계 구하기--------------------------------------------------------------------------------------------
            Int64 iTotal = 0;
            iTotal += GetData(dsControl.Tables[_cash].Rows[0][txtCashCnt01.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_cash].Rows[0][txtCashAmt01.Name.ToString()]) : 0;
            iTotal += GetData(dsControl.Tables[_cash].Rows[0][txtCashCnt02.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_cash].Rows[0][txtCashAmt02.Name.ToString()]) : 0;
            iTotal += GetData(dsControl.Tables[_cash].Rows[0][txtCashCnt03.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_cash].Rows[0][txtCashAmt03.Name.ToString()]) : 0;
            iTotal += GetData(dsControl.Tables[_cash].Rows[0][txtCashCnt04.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_cash].Rows[0][txtCashAmt04.Name.ToString()]) : 0;
            iTotal += GetData(dsControl.Tables[_cash].Rows[0][txtCashCnt05.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_cash].Rows[0][txtCashAmt05.Name.ToString()]) : 0;
            iTotal += GetData(dsControl.Tables[_cash].Rows[0][txtCashCnt06.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_cash].Rows[0][txtCashAmt06.Name.ToString()]) : 0;
            iTotal += GetData(dsControl.Tables[_cash].Rows[0][txtCashCnt07.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_cash].Rows[0][txtCashAmt07.Name.ToString()]) : 0;
            iTotal += GetData(dsControl.Tables[_cash].Rows[0][txtCashCnt08.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_cash].Rows[0][txtCashAmt08.Name.ToString()]) : 0;
            iTotal += GetData(dsControl.Tables[_cash].Rows[0][txtCashCnt09.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_cash].Rows[0][txtCashAmt09.Name.ToString()]) : 0;
            iTotal += GetData(dsControl.Tables[_ticket].Rows[0][txtTicketCnt01.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_ticket].Rows[0][txtTicketAmt01.Name.ToString()]) : 0;
            iTotal += GetData(dsControl.Tables[_ticket].Rows[0][txtTicketCnt02.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_ticket].Rows[0][txtTicketAmt02.Name.ToString()]) : 0;
            iTotal += GetData(dsControl.Tables[_ticket].Rows[0][txtTicketCnt03.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_ticket].Rows[0][txtTicketAmt03.Name.ToString()]) : 0;
            /* Loc changed 12.09
            iTotal += GetData(dsControl.Tables[_ticket].Rows[0][txtTicketCnt04.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_ticket].Rows[0][txtTicketAmt04.Name.ToString()]) : 0;
            iTotal += GetData(dsControl.Tables[_ticket].Rows[0][txtTicketCnt05.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_ticket].Rows[0][txtTicketAmt05.Name.ToString()]) : 0;
            iTotal += GetData(dsControl.Tables[_ticket].Rows[0][txtTicketCnt06.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_ticket].Rows[0][txtTicketAmt06.Name.ToString()]) : 0;
            iTotal += GetData(dsControl.Tables[_ticket].Rows[0][txtTicketCnt07.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_ticket].Rows[0][txtTicketAmt07.Name.ToString()]) : 0;
            iTotal += GetData(dsControl.Tables[_ticket].Rows[0][txtTicketCnt08.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_ticket].Rows[0][txtTicketAmt08.Name.ToString()]) : 0;
            iTotal += GetData(dsControl.Tables[_ticket].Rows[0][txtTicketCnt09.Name.ToString()]) > 0 ? GetData(dsControl.Tables[_ticket].Rows[0][txtTicketAmt09.Name.ToString()]) : 0;
             */
            txtTotalAmt.Text = iTotal.ToString();
            //----------------------------------------------------------------------------------------------------------

            return true;
        }

        /// <summary>
        /// 마감입금 저장 확인후 프린팅
        /// </summary>
        /// <param name="basketHeader">마감입금 헤더정보</param>
        /// <param name="basketMiddleDeposit">마감입금 정보</param>
        public void SetTran(BasketHeader basketHeader, BasketMiddleDeposit basketMiddleDeposit)
        {
            try
            {
                if (basketHeader != null && ChkPrint())
                {
                    POSPrinterUtils.Instance.PrintIO_M002_M003(true, FXConsts.RECEIPT_NAME_POS_IO_M003, basketHeader, basketMiddleDeposit);
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                SetControlDisable(false);
                this.Close();
            }
        }

        /// <summary>
        /// int 데이터 반환
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private int GetData(object data)
        {
            int iReturn = 0;

            if (data != null && data.ToString() != "" && data.ToString() != "0")
            {
                if (int.TryParse(data.ToString(), out iReturn))
                {
                }
            }

            return iReturn;
        }

        private int GetMath(string data)
        {
            int iReturn = 0;

            if (data != null && data.ToString() != "" && data.ToString() != "0")
            {
                if (int.TryParse(data.ToString(), out iReturn))
                {
                    iReturn = TypeHelper.RoundDown32(Convert.ToDecimal(iReturn));   //원단위 절사
                }
            }

            return iReturn;
        }

        #region 프린트 확인

        /// <summary>
        /// 프린트 확인
        /// </summary>
        /// <returns></returns>
        private bool ChkPrint()
        {
            bool bReturn = false;
            string strErrMsg = string.Empty;

            try
            {
                if (POSDeviceManager.Printer.Status == WSWD.WmallPos.POS.FX.Shared.DeviceStatus.Opened)
                {
                    if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.PowerClose)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_POWER;
                    }
                    else if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.CoverOpenned)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_OPENCOVER;
                    }
                    else if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.PaperEmpty)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_PAPER;
                    }
                    else if (POSDeviceManager.Printer.PrinterStatus == POSPrinterStatus.Closed)
                    {
                        strErrMsg = FXConsts.ERR_MSG_PRINTER_ERROR;
                    }
                    else
                    {
                        bReturn = true;
                    }
                }
                else
                {
                    strErrMsg = FXConsts.ERR_MSG_PRINTER_ERROR;
                }

                if (!bReturn)
                {
                    string[] strBtnNm = new string[2];
                    strBtnNm[0] = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00045");
                    strBtnNm[1] = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00190");

                    if (this.InvokeRequired)
                    {
                        this.Invoke((MethodInvoker)delegate()
                        {
                            if (ShowMessageBox(MessageDialogType.Question, null, strErrMsg, strBtnNm) == DialogResult.Yes)
                            {
                                POSDeviceManager.Printer.Open();
                                bReturn = ChkPrint();
                            }
                        });
                    }
                    else
                    {
                        if (ShowMessageBox(MessageDialogType.Question, null, strErrMsg, strBtnNm) == DialogResult.Yes)
                        {
                            POSDeviceManager.Printer.Open();
                            bReturn = ChkPrint();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }

            return bReturn;
        }

        #endregion

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
                    foreach (var item in this.Controls)
                    {
                        if (item.GetType().Name.ToString().ToLower() == "inputtext")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.InputText txt = (WSWD.WmallPos.POS.FX.Win.UserControls.InputText)item;
                            txt.Enabled = !_bDisable;
                        }
                        else if (item.GetType().Name.ToString().ToLower() == "button")
                        {
                            WSWD.WmallPos.POS.FX.Win.UserControls.Button btn = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)item;
                            btn.Enabled = !_bDisable;
                        }
                    }
                });
            }
            else
            {
                foreach (var item in this.Controls)
                {
                    if (item.GetType().Name.ToString().ToLower() == "inputtext")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.InputText txt = (WSWD.WmallPos.POS.FX.Win.UserControls.InputText)item;
                        txt.Enabled = !_bDisable;
                    }
                    else if (item.GetType().Name.ToString().ToLower() == "button")
                    {
                        WSWD.WmallPos.POS.FX.Win.UserControls.Button btn = (WSWD.WmallPos.POS.FX.Win.UserControls.Button)item;
                        btn.Enabled = !_bDisable;
                    }
                }
            }
        }

        #endregion
    }
}
