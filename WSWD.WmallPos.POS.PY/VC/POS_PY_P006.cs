//-----------------------------------------------------------------
/*
 * 화면명   : POS_PY_P006.cs
 * 화면설명 : 상품교환권
 * 개발자   : 정광호
 * 개발일자 : 2015.05.
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
using WSWD.WmallPos.FX.NetComm.Tasks.PG;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PG;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PG;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using System.Diagnostics;


namespace WSWD.WmallPos.POS.PY.VC
{
    public partial class POS_PY_P006 : PopupBase, IPYP006View
    {
        #region 변수

        /// <summary>
        /// 
        /// </summary>
        //private IPYP006presenter m_Presenter;

        /// <summary>
        /// 추가된 상품교환권 내역
        /// </summary>
        private DataTable dtAddExchange = null;

        /// <summary>
        /// 받을돈
        /// </summary>
        private int _iGetAmt = 0;

        /// <summary>
        /// 상품교환권 내역
        /// </summary>
        private List<BasketExchange> _basketExchanges = null;

        /// <summary>
        /// 최대 등록건수 확인
        /// </summary>
        private int iTranOverCnt = 0;

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable 여부
        /// </summary>
        private bool _bDisable = false;

        #endregion

        #region 생성자

        /// <summary>
        /// 상품교환권
        /// </summary>
        /// <param name="iGetAmt">받을돈</param>
        /// <param name="basket">상품교환권 내역</param>
        /// <param name="_iOverCnt">Tran 개수</param>
        public POS_PY_P006(int iGetAmt, List<BasketPay> basket, int _iTranOverCnt)
        {
            InitializeComponent();

            //현재 Tran 개수
            iTranOverCnt = _iTranOverCnt;

            //받을돈
            _iGetAmt = iGetAmt;

            //상품교환권 내역
            _basketExchanges = new List<BasketExchange>();

            foreach (var item in basket)
            {
                if (item.GetType().Name.ToString() == "BasketExchange")
                {
                    BasketExchange bp = (BasketExchange)item;

                    if (bp != null)
                    {
                        _basketExchanges.Add(bp);
                    }
                }
            }

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
            this.FormClosed += new FormClosedEventHandler(form_FormClosed);
            this.KeyEvent += new OPOSKeyEventHandler(form_KeyEvent);                                                                    //KeyEvent
            this.btnCancel.Click += new EventHandler(btnCancel_Click);                                                                  //한건취소 button Event
            this.btnSave.Click += new EventHandler(btnSave_Click);                                                                      //결제확정 button Event
            this.btnClose.Click += new EventHandler(btnClose_Click);                                                                    //닫기 button Event
            grd.RowDataBound += new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanelRowDataBoundEventHandler(grd_RowDataBound);          //그리드 데이터 바인딩 Event
            if (POSDeviceManager.Scanner != null)
            {
                POSDeviceManager.Scanner.DataEvent += new POSDataEventHandler(Scanner_DataEvent);                                       //Scanner Event    
            }
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

            dtAddExchange = new DataTable();
            dtAddExchange.Columns.Add("ColNo");
            dtAddExchange.Columns.Add("ColNum");
            dtAddExchange.Columns.Add("ColAmt");
            dtAddExchange.Columns.Add("ColTypeNm");
            dtAddExchange.Columns.Add("ColRealAmt");

            //그리드 컬럼 설정
            grd.AddColumn("ColNo", "NO", 50);               //
            grd.AddColumn("ColNum", "교환권번호", 150);     //
            grd.AddColumn("ColAmt", "권종금액", 100);       //
            grd.AddColumn("ColTypeNm", "용도");             //
            grd.AddColumn("ColRealAmt", "", 0);             //권종 실제금액

            //상품교환권 조회------------------------------
            // m_Presenter = new PYP006presenter(this);
            //---------------------------------------------

            //받을돈
            txtGetAmt.Text = _iGetAmt.ToString();

            //상품권종류
            txtTicketNo.SetFocus();

            //메세지 설정
            msgBar.Text = strMsg01;

            //최대건수 확인
            iTranOverCnt += _basketExchanges != null ? _basketExchanges.Count : 0;

            if (iTranOverCnt >= 90)
            {
                txtTicketNo.ReadOnly = true;
                msgBar.Text = strMsg02;
            }
            else
            {
                txtTicketNo.ReadOnly = false;
                txtTicketNo.Text = "";
                msgBar.Text = strMsg01;     //메세지 설정
                txtTicketNo.SetFocus();     //상품권번호 포커스
            }
        }

        void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Load -= new EventHandler(form_Load); 

            if (POSDeviceManager.Scanner != null)
            {
                POSDeviceManager.Scanner.DataEvent -= new POSDataEventHandler(Scanner_DataEvent);                                       //Scanner Event    
            }
        }

        /// <summary>
        /// KeyEvent
        /// </summary>
        /// <param name="e"></param>
        void form_KeyEvent(OPOSKeyEventArgs e)
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
                return;
            }

            if (txtTicketNo.IsFocused)
            {
                if (TypeHelper.ToInt32(txtGetAmt.Text.Length > 0 ? txtGetAmt.Text : "0") <= 0)
                {
                    e.IsHandled = true;
                    txtTicketNo.Text = "";
                    txtTicketNo.ReadOnly = true;
                    msgBar.Text = strMsg05;
                    return;
                }

                if (e.Key.OPOSKey == OPOSMapKeys.KEY_CLEAR)
                {
                    if (txtTicketNo.Text.Length <= 0)
                    {
                        e.IsHandled = true;
                        this.DialogResult = DialogResult.Cancel;
                    }
                    else
                    {
                        msgBar.Text = strMsg01;
                    }
                }
                else if (e.Key.OPOSKey == OPOSMapKeys.KEY_BKS)
	            {
                    msgBar.Text = strMsg01;
	            }
                else if (e.Key.OPOSKey == OPOSMapKeys.KEY_ENTER)
                {
                    if (txtTicketNo.Text.Length > 0)
                    {
                        //상품권번호 확인
                        CheckExchange();
                    }
                    else
                    {
                        msgBar.Text = strMsg01;
                    }
                }
                else if (!e.IsControlKey)
                {
                    msgBar.Text = strMsg01;
                }
            }
        }

        /// <summary>
        /// 한건취소 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCancel_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;
            if (grd == null || grd.RowCount <= 0 || grd.CurrentRowIndex < 0) return;

            SetControlDisable(true);

            try
            {
                DataRow dr = (System.Data.DataRow)(grd.GetRow(grd.CurrentRowIndex).ItemData);

                //그리드 선택행 삭제
                dtAddExchange.Rows.Remove(dr);
                grd.DeleteActiveRow();

                int iRow = 1;
                Int32 iAmt = 0;

                foreach (DataRow drTemp in dtAddExchange.Rows)
                {
                    iAmt += TypeHelper.ToInt32(TypeHelper.ToString(drTemp["ColAmt"]).Replace(",", ""));
                    drTemp[0] = iRow.ToString();
                    iRow++;
                }

                for (int i = 0; i < grd.RowCount; i++)
                {
                    DataRow drTemp = (DataRow)grd.GetRow(i).ItemData;
                    grd.UpdateRow(i, drTemp);
                }

                txtGetAmt.Text = _iGetAmt - iAmt > 0 ? string.Format("{0}", _iGetAmt - iAmt) : "0";   //받을돈 원복
                txtTicketTotalAmt.Text = iAmt > 0 ? string.Format("{0}", iAmt) : "0";                 //상품권합계 원복

                if (ChkCnt(-1))
                {
                    txtTicketNo.Text = "";
                    txtTicketNo.ReadOnly = false;
                    txtTicketNo.SetFocus();
                    msgBar.Text = strMsg01;
                    iTranOverCnt--;
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
        /// 적용 button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (_bDisable) return;

            if (grd == null || grd.RowCount <= 0) return;

            ChildManager.ShowProgress(true);
            SetControlDisable(true);
            

            try
            {

                bool bAdd = false;
                string[] str = new string[grd.RowCount];

                for (int i = 0; i < grd.RowCount; i++)
                {
                    DataRow dr = (System.Data.DataRow)(grd.GetRow(i).ItemData);

                    str[i] = dr["ColNum"].ToString();

                    bAdd = true;
                }

                if (bAdd)
                {
                    PG02ReqData reqData = new PG02ReqData(str);
                    PG02DataTask pg02 = new PG02DataTask(reqData);
                    pg02.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pg02_TaskCompleted);
                    pg02.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pg02_Errored);
                    pg02.ExecuteTask();
                }
                else
                {
                    ChildManager.ShowProgress(false);
                    SetControlDisable(false);
                }
            }
            catch (Exception)
            {
                ChildManager.ShowProgress(false);
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

        /// <summary>
        /// 그리드 메인 데이터 바인딩
        /// </summary>
        /// <param name="row"></param>
        void grd_RowDataBound(WSWD.WmallPos.POS.FX.Win.UserControls.GridRow row)
        {
            if (row.RowState == GridRowState.Added)
            {
                for (int i = 0; i < 5; i++)
                {
                    row.Cells[i].Controls.Add(new Label()
                    {
                        Text = ((System.Data.DataRow)(row.ItemData)).ItemArray[i].ToString(),
                        AutoSize = false,
                        TextAlign = i == 0 || i == 2 ? ContentAlignment.MiddleRight : (i == 1 ? ContentAlignment.MiddleCenter : ContentAlignment.MiddleLeft),
                        Dock = DockStyle.Fill
                    });
                }
            }
            else if (row.RowState == GridRowState.Existed)
            {
                DataRow[] drFilter = dtAddExchange.Select(string.Format("ColNum = '{0}'", row.Cells[1].Controls[0].Text.ToString()));

                if (drFilter != null && drFilter.Length > 0)
                {
                    row.Cells[0].Controls[0].Text = drFilter[0]["ColNo"].ToString();
                    row.Update();
                }
            }
        }

        /// <summary>
        /// Scanner Event
        /// </summary>
        /// <param name="msrData"></param>
        void Scanner_DataEvent(string msrData)
        {
            Trace.WriteLine("PY_P006_Scanner_DataEvent " + msrData, "program");

            if (_bDisable || txtTicketNo.ReadOnly || msrData.Length <= 0 || TypeHelper.ToInt32(txtGetAmt.Text) <= 0) return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    txtTicketNo.Text = msrData;

                    //상품권번호 확인
                    CheckExchange();  
                });
            }
            else
            {
                txtTicketNo.Text = msrData;

                //상품권번호 확인
                CheckExchange();  
            } 
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// 상품교환권 확인
        /// </summary>
        void CheckExchange()
        {
            ChildManager.ShowProgress(true);
            SetControlDisable(true);
            

            //중복 및 건수확인
            if (!ChkDup())
            {
                ChildManager.ShowProgress(false);
                SetControlDisable(false);
                return;
            }

            try
            {
                PG01ReqData reqData = new PG01ReqData();
                reqData.GiftChangeNo = txtTicketNo.Text.ToString();

                PG01DataTask pg01 = new PG01DataTask(reqData);
                pg01.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pg01_TaskCompleted);
                pg01.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pg01_Errored);
                pg01.ExecuteTask();
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                ChildManager.ShowProgress(false);
                SetControlDisable(false);
            }
        }

        /// <summary>
        /// 상품교환권 중복확인
        /// </summary>
        /// <returns></returns>
        private bool ChkDup()
        {
            //상품교환권 최대건수 확인
            if (!ChkCnt(1)) return false;

            string strTicket = txtTicketNo.Text.ToString();

            if (_basketExchanges != null)
            {
                foreach (BasketExchange item in _basketExchanges)
                {
                    if (item != null)
                    {
                        if (item.ExchangeNo == strTicket)
                        {
                            txtTicketNo.Text = "";
                            txtTicketNo.SetFocus();
                            msgBar.Text = strMsg03;
                            return false;
                        }
                    }
                }
            }

            if (grd != null)
            {
                for (int i = 0; i < grd.RowCount; i++)
                {
                    DataRow dr = (System.Data.DataRow)(grd.GetRow(i).ItemData);

                    if (dr["ColNum"].ToString() == strTicket)
                    {
                        txtTicketNo.Text = "";
                        txtTicketNo.SetFocus();
                        msgBar.Text = strMsg03;
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 상품교환권 최대건수 확인
        /// </summary>
        private bool ChkCnt(int iCnt)
        {
            if (iTranOverCnt + iCnt >= 90)
            {
                txtTicketNo.Text = "";
                txtTicketNo.ReadOnly = true;
                msgBar.Text = strMsg02;
                return false;
            }

            return true;
        }

        #region 전문통신

        /// <summary>
        /// 상품교환권 회수가능여부 전문
        /// </summary>
        /// <param name="responseData"></param>
        void pg01_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            ChildManager.ShowProgress(false);

            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                var data = responseData.DataRecords.ToDataRecords<PG01RespData>();

                if (data.Length > 0)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke((MethodInvoker)delegate()
                        {
                            if (responseData.Response.ReqHeader.RespCode.ToString() == "00")
                            {
                                int iPayAmt = TypeHelper.ToInt32(data[0].PayAmt);
                                int iTotalAmt = TypeHelper.ToInt32(txtTicketTotalAmt.Text);
                                int iGetAmt = TypeHelper.ToInt32(txtGetAmt.Text);

                                DataRow drAdd = dtAddExchange.NewRow();
                                drAdd[0] = (grd.RowCount + 1).ToString();
                                drAdd[1] = txtTicketNo.Text.ToString();
                                drAdd[2] = string.Format("{0:#,##0}", iPayAmt);
                                drAdd[3] = data[0].EtcData != null ? data[0].EtcData.ToString() : "";
                                drAdd[4] = iGetAmt - iPayAmt >= 0 ? iPayAmt.ToString() : iGetAmt.ToString();
                                dtAddExchange.Rows.Add(drAdd);

                                grd.AddRow(drAdd);
                                iTranOverCnt++;

                                txtTicketTotalAmt.Text = iTotalAmt + iPayAmt > 0 ? string.Format("{0}", iTotalAmt + iPayAmt) : "0";
                                txtGetAmt.Text = iGetAmt - iPayAmt > 0 ? string.Format("{0}", iGetAmt - iPayAmt) : "0";
                                txtTicketNo.Text = "";

                                if (iGetAmt - iPayAmt <= 0)
                                {
                                    txtTicketNo.Text = "";
                                    txtTicketNo.ReadOnly = true;
                                    msgBar.Text = strMsg05;
                                }
                                else
                                {
                                    txtTicketNo.Text = "";
                                    txtTicketNo.ReadOnly = false;
                                    txtTicketNo.SetFocus();
                                    msgBar.Text = strMsg01;
                                }
                            }
                            else
                            {
                                txtTicketNo.Text = "";
                                msgBar.Text = data[0].EtcData != null ? data[0].EtcData.ToString() : "";
                            }

                            SetControlDisable(false);
                        });
                    }
                    else
                    {
                        if (responseData.Response.ReqHeader.RespCode.ToString() == "00")
                        {
                            int iPayAmt = TypeHelper.ToInt32(data[0].PayAmt);
                            int iTotalAmt = TypeHelper.ToInt32(txtTicketTotalAmt.Text);
                            int iGetAmt = TypeHelper.ToInt32(txtGetAmt.Text);

                            DataRow drAdd = dtAddExchange.NewRow();
                            drAdd[0] = (grd.RowCount + 1).ToString();
                            drAdd[1] = txtTicketNo.Text.ToString();
                            drAdd[2] = string.Format("{0:#,##0}", iPayAmt);
                            drAdd[3] = data[0].EtcData != null ? data[0].EtcData.ToString() : "";
                            drAdd[4] = iGetAmt - iPayAmt >= 0 ? iPayAmt.ToString() : iGetAmt.ToString();
                            dtAddExchange.Rows.Add(drAdd);

                            grd.AddRow(drAdd);
                            iTranOverCnt++;

                            txtTicketTotalAmt.Text = iTotalAmt + iPayAmt > 0 ? string.Format("{0}", iTotalAmt + iPayAmt) : "0";
                            txtGetAmt.Text = iGetAmt - iPayAmt > 0 ? string.Format("{0}", iGetAmt - iPayAmt) : "0";
                            txtTicketNo.Text = "";

                            if (iGetAmt - iPayAmt <= 0)
                            {
                                txtTicketNo.Text = "";
                                txtTicketNo.ReadOnly = true;
                                msgBar.Text = strMsg05;
                            }
                            else
                            {
                                txtTicketNo.Text = "";
                                txtTicketNo.ReadOnly = false;
                                txtTicketNo.SetFocus();
                                msgBar.Text = strMsg01;
                            }
                        }
                        else
                        {
                            txtTicketNo.Text = "";
                            msgBar.Text = data[0].EtcData != null ? data[0].EtcData.ToString() : "";
                        }

                        SetControlDisable(false);
                    }
                }
            }
            else if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.NoInfo)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        txtTicketNo.Text = "";
                        txtTicketNo.ReadOnly = false;
                        txtTicketNo.SetFocus();
                        msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false);
                    });
                }
                else
                {
                    txtTicketNo.Text = "";
                    txtTicketNo.ReadOnly = false;
                    txtTicketNo.SetFocus();
                    msgBar.Text = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false);
                }
            }
            else
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        txtTicketNo.Text = "";
                        txtTicketNo.ReadOnly = false;
                        txtTicketNo.SetFocus();
                        msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false);
                    });
                }
                else
                {
                    txtTicketNo.Text = "";
                    txtTicketNo.ReadOnly = false;
                    txtTicketNo.SetFocus();
                    msgBar.Text = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false);
                }
            }
        }

        /// <summary>
        /// 상품교환권 회수가능여부 전문 오류
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="lastException"></param>
        void pg01_Errored(string errorMessage, Exception lastException)
        {
            ChildManager.ShowProgress(false);

            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    msgBar.Text = strMsg06;
                    SetControlDisable(false);
                });
            }
            else
            {
                msgBar.Text = strMsg06;
                SetControlDisable(false);
            }
        }

        /// <summary>
        /// 상품교환권 가회수처리 전문
        /// </summary>
        /// <param name="responseData"></param>
        void pg02_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            ChildManager.ShowProgress(false);

            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                var data = responseData.DataRecords.ToDataRecords<PG02RespData>();

                if (data.Length > 0)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke((MethodInvoker)delegate()
                        {
                            bool bAdd = true;
                            for (int i = 0; i < TypeHelper.ToInt32(data[0].GiftCount); i++)
                            {
                                if (data[0].GiftList[i].GiftApprNo.Length <= 0)
                                {
                                    bAdd = false;
                                    break;
                                }
                            }

                            if (bAdd)
                            {
                                List<BasketExchange> basketReturns = new List<BasketExchange>();
                                BasketExchange bp = null;

                                for (int i = 0; i < TypeHelper.ToInt32(data[0].GiftCount); i++)
                                {
                                    DataRow[] drFilter = dtAddExchange.Select(string.Format("ColNum = '{0}'", data[0].GiftList[i].GiftNo.ToString()));

                                    if (drFilter != null && drFilter.Length > 0)
                                    {
                                        bp = new BasketExchange();
                                        bp.PayGrpCd = NetCommConstants.PAYMENT_GROUP_COUPON;                                //지불 수단 그룹 코드
                                        bp.PayDtlCd = NetCommConstants.PAYMENT_DETAIL_EXCHANGE;                             //지불 수단 상세 코드
                                        bp.BalGrpCd = NetCommConstants.PAYMENT_GROUP_NONE;                                  //잔액 수단 그룹 코드
                                        bp.BalDtlCd = NetCommConstants.PAYMENT_DETAIL_NONE;                                 //잔액 수단 상세 코드
                                        bp.PayAmt = TypeHelper.ToInt32(TypeHelper.ToString(drFilter[0]["ColRealAmt"]).Replace(",", "")).ToString();    //지불 금액
                                        bp.CancFg = "0";                                                                    //취소 flag
                                        bp.ExchangeNo = data[0].GiftList[i].GiftNo.ToString();                              //상품교환권 번호
                                        bp.ExchangeAmt = TypeHelper.ToInt32(TypeHelper.ToString(drFilter[0]["ColAmt"]).Replace(",","")).ToString();   //권종 금액
                                        bp.ExchangeApprNo = data[0].GiftList[i].GiftApprNo.ToString();
                                        bp.ExchangeDivision = "01";
                                        bp.ExchangeType = "";

                                        basketReturns.Add(bp);
                                    }
                                }

                                this.ReturnResult.Add("PAY_DATA", basketReturns);
                                this.DialogResult = DialogResult.OK;
                            }

                            SetControlDisable(false);
                        });
                    }
                    else
                    {
                        bool bAdd = true;
                        for (int i = 0; i < TypeHelper.ToInt32(data[0].GiftCount); i++)
                        {
                            if (data[0].GiftList[i].GiftApprNo.Length <= 0)
                            {
                                bAdd = false;
                                break;
                            }
                        }

                        if (bAdd)
                        {
                            List<BasketExchange> basketReturns = new List<BasketExchange>();
                            BasketExchange bp = null;

                            for (int i = 0; i < TypeHelper.ToInt32(data[0].GiftCount); i++)
                            {
                                DataRow[] drFilter = dtAddExchange.Select(string.Format("ColNum = '{0}'", data[0].GiftList[i].GiftNo.ToString()));

                                if (drFilter != null && drFilter.Length > 0)
                                {
                                    bp = new BasketExchange();
                                    bp.PayGrpCd = NetCommConstants.PAYMENT_GROUP_COUPON;                                //지불 수단 그룹 코드
                                    bp.PayDtlCd = NetCommConstants.PAYMENT_DETAIL_EXCHANGE;                             //지불 수단 상세 코드
                                    bp.BalGrpCd = NetCommConstants.PAYMENT_GROUP_NONE;                                  //잔액 수단 그룹 코드
                                    bp.BalDtlCd = NetCommConstants.PAYMENT_DETAIL_NONE;                                 //잔액 수단 상세 코드
                                    bp.PayAmt = TypeHelper.ToInt32(TypeHelper.ToString(drFilter[0]["ColRealAmt"]).Replace(",", "")).ToString();    //지불 금액
                                    bp.CancFg = "0";                                                                    //취소 flag
                                    bp.ExchangeNo = data[0].GiftList[i].GiftNo.ToString();                              //상품교환권 번호
                                    bp.ExchangeAmt = TypeHelper.ToInt32(TypeHelper.ToString(drFilter[0]["ColAmt"]).Replace(",", "")).ToString();   //권종 금액
                                    bp.ExchangeApprNo = data[0].GiftList[i].GiftApprNo.ToString();
                                    bp.ExchangeDivision = "01";
                                    bp.ExchangeType = "";

                                    basketReturns.Add(bp);
                                }
                            }

                            this.ReturnResult.Add("PAY_DATA", basketReturns);
                            this.DialogResult = DialogResult.OK;
                        }

                        SetControlDisable(false);
                    }
                }
            }
            else if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.NoInfo)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false);
                    });
                }
                else
                {
                    msgBar.Text = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false);
                }
            }
            else
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        msgBar.Text = responseData.Response.ErrorMessage.ToString();
                        SetControlDisable(false);
                    });
                }
                else
                {
                    msgBar.Text = responseData.Response.ErrorMessage.ToString();
                    SetControlDisable(false);
                }
            }
        }

        /// <summary>
        /// 상품교환권 가회수처리 전문 오류
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="lastException"></param>
        void pg02_Errored(string errorMessage, Exception lastException)
        {
            ChildManager.ShowProgress(false);

            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    msgBar.Text = strMsg06;
                    SetControlDisable(false);
                });
            }
            else
            {
                msgBar.Text = strMsg06;
                SetControlDisable(false);
            }
        }

        #endregion

        /// <summary>
        /// 조회, 저장등의 DB, TRAN 시 Control Enable/Disable
        /// </summary>
        void SetControlDisable(bool bDisable)
        {
            _bDisable = bDisable;

            if (!bDisable)
            {
                if (POSDeviceManager.Scanner != null)
                {
                    POSDeviceManager.Scanner.DataEvent += new POSDataEventHandler(Scanner_DataEvent);                                       //Scanner Event    
                }
            }
            else
            {
                if (POSDeviceManager.Scanner != null)
                {
                    POSDeviceManager.Scanner.DataEvent -= new POSDataEventHandler(Scanner_DataEvent);                                       //Scanner Event    
                }
            }

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
