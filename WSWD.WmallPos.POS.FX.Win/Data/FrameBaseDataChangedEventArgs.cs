using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.FX.Win.Data
{
    public class FrameBaseDataChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 공지사항 도착?
        /// </summary>
        public bool HasNotice { get;  set; }

        /// <summary>
        /// 서버와 통신상태
        /// </summary>
        public bool ServerConnected { get; set; }

        /// <summary>
        /// 보류건수
        /// </summary>
        public int SaleHoldCount { get; set; }

        /// <summary>
        /// 업로드 된 매출건수
        /// </summary>
        public int UploadedTransCount { get; set; }

        /// <summary>
        /// 전체매출건수
        /// </summary>
        public int TotalTransCount { get; set; }

        /// <summary>
        /// Config data 변경
        /// </summary>
        public bool ConfigChanged { get; set; }

        /// <summary>
        /// Statusbar message
        /// </summary>
        public string StatusBarMessage { get; set; }

        /// <summary>
        /// 반품상태
        /// </summary>
        public bool StateRefund { get; set; }

        /// <summary>
        /// 현재 보여 주눈 폼의 타이틀
        /// </summary>
        public string ActiveTitle { get; set; }

        /// <summary>
        /// Item to be updated
        /// </summary>
        public FrameBaseDataItem ChangedItem { get; set; }
    }

    public enum FrameBaseDataItem
    {
        AllItem,
        HasNotice,
        ServerConnected,
        SaleHoldCount,
        UploadedTransCount,
        TotalTransCount,
        StatusBarMessage,
        StateRefund,
        ActiveTitle,
        ConfigChanged
    }
}
