using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using POS.Devices;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Exceptions;
using WSWD.WmallPos.FX.Shared.Config;
using System.Windows.Forms;
using WSWD.WmallPos.POS.FX.Shared;
using System.Diagnostics;

namespace WSWD.WmallPos.POS.FX.Win.Devices
{
    public class POSScannerGun : POSDeviceBase
    {
        public event POSDataEventHandler DataEvent;
        public POSScannerGun()
        {
        }

        #region IPOSDevice Members

        public ScannerGun Config
        {
            get
            {
                return ConfigData.Current.DevConfig.ScannerGun;
            }
        }

        public override bool UseYN
        {
            get
            {
                return "1".Equals(Config.Use);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override DeviceStatus Open()
        {
            if (Status == DeviceStatus.Opened)
            {
                return Status;
            }

            if (!UseYN)
            {
                throw new Exception("바코드스캔 사용안함.");
            }

            Status = DeviceStatus.Opened;
            base.Open();
            return Status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Close()
        {
            Status = DeviceStatus.Closed;
            return Status == DeviceStatus.Closed;
        }

        #endregion

        #region Keyboard event forwarding

        public void ForwardScannerEvent(string scannerCode)
        {
            if (DataEvent != null)
            {
                this.DataEvent(scannerCode);
            }
        }

        #endregion

    }
}
