//-----------------------------------------------------------------
/*
 * 화면명   : POS_ST_M002.cs
 * 화면설명 : 메인 공지사항
 * 개발자   : 정광호
 * 개발일자 : 2015.04.30
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

using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Win;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.ST.VI;
using WSWD.WmallPos.POS.ST.PT;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.ST.PI;
using WSWD.WmallPos.POS.FX.Win.Devices;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.FX.Shared.Data;

namespace WSWD.WmallPos.POS.ST.VC
{
    /// <summary>
    /// 메인
    /// </summary>
    public partial class POS_ST_M002 : FormBase, ISTM002View, IObserver<FrameBaseDataChangedEventArgs>
    {
        #region 변수

        //메인 공지사항 비즈니스 로직
        private ISTM002Presenter m_Presenter;

        /// <summary>
        /// 그리드 Label
        /// </summary>
        Label lbl = null;

        #endregion

        #region 생성자

        public POS_ST_M002()
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
            this.grd.RowDataBound += new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanelRowDataBoundEventHandler(grd_RowDataBound);  //그리드 데이터 바인딩 Event
            this.Activated += new EventHandler(POS_ST_M002_Activated);
            this.Unload += new EventHandler(POS_ST_M002_Unload);

            FrameBaseData.Current.Attach(this);
        }

        void POS_ST_M002_Unload(object sender, EventArgs e)
        {
            this.grd.RowDataBound -= new WSWD.WmallPos.POS.FX.Win.UserControls.GridPanelRowDataBoundEventHandler(grd_RowDataBound);  //그리드 데이터 바인딩 Event
            this.Activated -= new EventHandler(POS_ST_M002_Activated);
            this.Unload -= new EventHandler(POS_ST_M002_Unload);
            this.Load -= new EventHandler(form_Load); 

            FrameBaseData.Current.Detach(this);
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

            this.Text = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00241");
            this.IsModal = false;

            grd.ClearAll();

            if (grd.ColumnCount <= 0)
            {
                //그리드 컬럼 설정
                grd.AddColumn("DD_START", "일자", 120);
                grd.AddColumn("NO_SEQ", "순번", 70);
                grd.AddColumn("NM_TITLE", "제목");
                grd.AddColumn("NM_SEND_USER", "발신자", 100);
            }

            //메인 공지사항 조회-------------------------
            m_Presenter = new STM002Presenter(this);
            m_Presenter.GetNotice();
            m_Presenter.CheckNewNotice();
            //-------------------------------------------
        }

        void POS_ST_M002_Activated(object sender, EventArgs e)
        {
            // show CDP 고객용표시기
            if (POSDeviceManager.LineDisplay.Status == DeviceStatus.Opened)
            {
                POSDeviceManager.LineDisplay.DisplayText(MSG_CDP_MAIN_1, MSG_CDP_MAIN_2);
            }
        }

        /// <summary>
        /// 그리드 메인 데이터 바인딩
        /// </summary>
        /// <param name="row"></param>
        void grd_RowDataBound(WSWD.WmallPos.POS.FX.Win.UserControls.GridRow row)
        {
            if (row.RowState == GridRowState.Added)
            {
                for (int i = 0; i < 4; i++)
                {
                    lbl = new Label();
                    lbl.Text = ((System.Data.DataRow)(row.ItemData)).ItemArray[i].ToString();
                    lbl.AutoSize = false;
                    lbl.TextAlign = i == 0 || i == 1 ? System.Drawing.ContentAlignment.MiddleCenter : System.Drawing.ContentAlignment.MiddleLeft;
                    lbl.Dock = DockStyle.Fill;
                    lbl.DoubleClick += new EventHandler(lbl_DoubleClick);

                    row.Cells[i].Controls.Add(lbl);
                }
            }
        }

        /// <summary>
        /// 그리드 Label DoubleClick Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lbl_DoubleClick(object sender, EventArgs e)
        {
            if (grd.RowCount > 0 && grd.CurrentRowIndex >= 0)
            {
                DataRow dr = (System.Data.DataRow)(grd.GetRow(grd.CurrentRowIndex).ItemData);

                if (dr != null &&
                    dr["DD_START"] != null && dr["DD_START"].ToString().Length > 0 &&
                    dr["NO_SEQ"] != null && dr["NO_SEQ"].ToString().Length > 0)
                {
                    //공지사항 팝업
                    using (var pop = ChildManager.ShowPopup(string.Empty, "WSWD.WmallPos.POS.SL.dll",
                        "WSWD.WmallPos.POS.SL.VC.POS_SL_P004",
                        new object[] { dr["DD_START"].ToString(), dr["NO_SEQ"].ToString() }))
                    {
                        pop.ShowDialog(this);
                    }
                }
            }
        }

        #endregion

        #region 사용자 정의

        /// <summary>
        /// 공지사항 셋팅
        /// </summary>
        /// <param name="ds">공시사항 내역</param>
        public void SetNotice(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                grd.ClearAll();

                //그리드 메인 데이터 바인딩
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

        #endregion

        #region IObserver<FrameBaseDataChangedEventArgs> Members

        /// <summary>
        /// 공지사항 업데이트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Update(object sender, FrameBaseDataChangedEventArgs e)
        {
            if (e.ChangedItem == FrameBaseDataItem.RefreshNotice)
            {
                m_Presenter.GetNotice();            
            }
        }

        #endregion
    }
}
