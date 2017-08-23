using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.FX.Shared;

namespace WSWD.WmallPos.FX.NetComm.Tasks
{
    /// <summary>
    /// 전문 다운로드
    /// </summary>
    public class DownloadDataTask : SCDataTaskBase
    {
        public DownloadDataTask(string reqType, Type recordDataType)
            : base(reqType, recordDataType,
            ConfigData.Current.AppConfig.PosComm.SvrIP1,
            int.Parse(ConfigData.Current.AppConfig.PosComm.ComDPort1))
        {

        }
    }
}
