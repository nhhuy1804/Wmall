using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

using WSWD.WmallPos.POS.SL.Data;
using System.Drawing;
using WSWD.WmallPos.FX.Shared.Data;

namespace WSWD.WmallPos.POS.SL.VC
{
    partial class POS_SL_M001
    {
        /// <summary>
        /// GRID DATAROWS
        /// </summary>
        public PBItemData[] DataRows
        {
            get
            {
                return gpItems.RowItems.Cast<PBItemData>().ToArray();
            }
        }

        /// <summary>
        /// Text
        /// </summary>
        public string InputText
        {
            get
            {
                return KeyInputText.Text;
            }
            set
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        KeyInputText.Text = value;
                    });
                }
                else
                {

                    KeyInputText.Text = value;
                }
            }
        }

        /// <summary>
        /// Set MAXIMUM input length 
        /// </summary>
        public int InputLength
        {
            get
            {
                return KeyInputText.MaxLength;
            }
            set
            {
                KeyInputText.MaxLength = value;
            }
        }

        /// <summary>
        /// 품번입력상태
        /// </summary>
        private SaleProcessState m_processState = SaleProcessState.InputStarted;
        public SaleProcessState ProcessState
        {
            get
            {
                return m_processState;
            }
            set
            {
                m_processState = value;
                gpItems.DisableScroll = value == SaleProcessState.ItemInputing;
                gpItems.DisableSelection = value == SaleProcessState.ItemInputing;

                if (value == SaleProcessState.InputStarted)
                {
                    funcKeyGroup1.ResetState();
                    GuideMessage = gpItems.RowItems.Length == 0 || m_retainItems ?
                        GUIDE_MSG_PSTATE_INITIAL : GUIDE_MSG_INTPUT_STARTED;

                    this.InputOperation = ItemInputOperation.None;
                    this.InputState = ItemInputState.Ready;

                    //2015.09.11 정광호 수정
                    //자동반품시 바코드길이가 14자리에서 StoreNo를 포함한 16자리로 변경
                    //this.InputLength = SaleMode == SaleModes.AutoReturn ? 14 : 13;
                    this.InputLength = SaleMode == SaleModes.AutoReturn ? 16 : 13;

                    if (SaleMode == SaleModes.AutoReturn)
                    {
                        GuideMessage = GUIDE_MSG_PSTATE_INITIAL_AUTORTN;
                        autoRtnPanel1.ResetState();
                    }

                    m_scannerOn = true;
                }
                else if (value == SaleProcessState.SubTotal)
                {
                    m_scannerOn = false;
                    InputText = string.Empty;
                    this.InputLength = 9;
                    GuideMessage = GUIDE_MSG_PAYMENT_STARTED;
                }
                else if (value == SaleProcessState.Payment)
                {
                    InputText = string.Empty;
                    InputLength = 9; // 99 999 999 999
                }
                else if (value == SaleProcessState.Completed)
                {
                    funcKeyGroup1.ResetState();

                    // PAYMENT COMPLETED
                    this.ProcessState = SaleProcessState.InputStarted;
                }
                else if (value == SaleProcessState.AutoRtnProcessing)
                {
                    m_scannerOn = false;
                    GuideMessage = GUIDE_MSG_AUTORTN_PROCESSING;
                    autoRtnPanel1.SetEnableButton(0, false);
                    autoRtnPanel1.SetEnableButton(1, false);
                }
                else if (value == SaleProcessState.AutoRtnReady)
                {
                    m_scannerOn = true;
                    ReportInvalidState(InvalidDataInputState.ConfirmAutoRtn);
                }
            }
        }

        /// <summary>
        /// 상품입력중인 상태
        /// </summary>
        private ItemInputState m_itemInputState = ItemInputState.Ready;
        public ItemInputState InputState
        {
            get
            {
                return m_itemInputState;
            }
            set
            {
                if (value == ItemInputState.ItemPumNoEntered)
                {
                    // waiting for next input
                    // 다음 입력 기다린다
                    InputText = string.Empty;

                    // "품목 코드를 입력하십시오"
                    GuideMessage = GUIDE_MSG_INPUTING_CDITEM;
                    InputLength = 4;
                }
                else if (value == ItemInputState.ItemCodeEntered)
                {
                    InputText = string.Empty;
                    GuideMessage = this.InputOperation == ItemInputOperation.ManualEnter ? GUIDE_MSG_INPUTING_FGCLASS :
                        GUIDE_MSG_INPUTING_2NDBARCODE;
                    InputLength = this.InputOperation == ItemInputOperation.ManualEnter ? 2 : 13;
                }
                else if (value == ItemInputState.ItemGubunEntered)
                {
                    InputText = string.Empty;
                    GuideMessage = GUIDE_MSG_INPUTING_UTSPRC;
                    InputLength = 8;
                }
                else if (value == ItemInputState.Ready)
                {
                    InputText = string.Empty;
                    GuideMessage = gpItems.RowItems.Length == 0 || m_retainItems ? GUIDE_MSG_PSTATE_INITIAL :
                        GUIDE_MSG_INTPUT_STARTED;
                    InputLength = 0;
                }

                m_itemInputState = value;
            }
        }

        /// <summary>
        /// 입력타이브
        /// </summary>
        public ItemInputOperation InputOperation { get; set; }

        /// <summary>
        /// 판매모드
        /// </summary>
        private SaleModes m_saleMode = SaleModes.Sale;
        public SaleModes SaleMode
        {
            get
            {
                return m_saleMode;
            }
            set
            {
                //bool sf = StateRefund;
                var sm = m_saleMode;
                m_saleMode = value;

                switch (value)
                {
                    case SaleModes.Sale:
                        this.Text = TITLE_SL_SALE;
                        break;
                    case SaleModes.OtherSale:
                        this.Text = TITLE_SL_OTH_SALE;
                        break;
                    case SaleModes.ManuReturn:
                        this.Text = TITLE_SL_RETURN_MANU;
                        break;
                    case SaleModes.AutoReturn:
                        this.Text = TITLE_SL_RETURN_AUTO;
                        break;
                    default:
                        this.Text = TITLE_SL_OTH_SALE_RETURN;
                        break;
                }

                // change active text
                FrameBaseData.Current.OnDataChanged(FrameBaseDataItem.ActiveTitle, this.Text);

                // form back color
                if (sm != value)
                {
                    //this.BackgroundImage = StateRefund ? Properties.Resources.bg_pos_02 : Properties.Resources.bg_pos_01;
                    ChangeLayoutByMode();
                }

                // change mode;
                FrameBaseData.Current.OnDataChanged(FrameBaseDataItem.StateRefund, StateRefund);

            }
        }

        public bool StateRefund
        {
            get
            {
                return m_saleMode.ToString().Contains("Return");
            }
        }
    }
}
