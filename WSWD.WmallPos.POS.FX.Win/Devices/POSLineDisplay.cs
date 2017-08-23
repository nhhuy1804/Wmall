using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using POS.Devices;

using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Exceptions;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.FX.Shared.Helper;
using System.Diagnostics;

namespace WSWD.WmallPos.POS.FX.Win.Devices
{
    public class POSLineDisplay : POSDeviceBase
    {
        private OPOSLineDisplay m_device = null;

        public POSLineDisplay()
        {
            m_device = new OPOSLineDisplay();
        }

        #region IPOSDevice Members

        public LineDisplay Config
        {
            get
            {
                return ConfigData.Current.DevConfig.LineDisplay;
            }
        }

        public override bool UseYN
        {
            get
            {
                return "1".Equals(Config.Use);
            }
        }

        public override bool Enabled
        {
            get
            {
                return m_device.DeviceEnabled;
            }
            set
            {
                m_device.DeviceEnabled = value;
            }
        }

        public override DeviceStatus Open()
        {
            if (Status == DeviceStatus.Opened)
            {
                return Status;
            }

            if (!UseYN)
            {
                throw new Exception("고객표시기 사용 안함.");
            }

            Status = DeviceStatus.Closed;
            try
            {
                TraceHelper.Instance.TraceWrite("Open()", "LineDisplay: {0}", Config.LogicalName);
                var rc = m_device.Open(Config.LogicalName);
                if (rc == (int)OPOS_Constants.OPOS_SUCCESS)
                {
                    rc = m_device.ClaimDevice(5000);
                    if (rc == (int)OPOS_Constants.OPOS_SUCCESS)
                    {
                        Status = DeviceStatus.Opened;
                    }
                    else
                    {
                        Status = DeviceStatus.InitError;
                    }
                }
                else
                {
                    Status = DeviceStatus.OpenError;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("고객표시기 오픈 오류.", ex);
            }

            base.Open();
            return Status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Close()
        {
            var rc = m_device.ReleaseDevice();
            if (rc == (int)OPOS_Constants.OPOS_SUCCESS)
            {
                rc = m_device.Close();
                if (rc == (int)OPOS_Constants.OPOS_SUCCESS)
                {
                    Status = DeviceStatus.Closed;
                }
            }

            return Status == DeviceStatus.Closed;
        }


        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line1Text"></param>
        /// <param name="line2Text"></param>
        public void DisplayText(string line1Text, string line2Text)
        {
            //Trace.WriteLine("CDP 1: " + line1Text, "program");
            //Trace.WriteLine("CDP 2: " + line2Text, "program");
            if (m_device == null || !Enabled || Status != DeviceStatus.Opened)
            {
                throw new Exception("고객표시기 오류.");
            }

            m_device.ClearText();
            m_device.DisplayTextAt(0, 0, line1Text, (int)OPOSLineDisplayConstants.DISP_DT_NORMAL);
            m_device.DisplayTextAt(1, 0, line2Text, (int)OPOSLineDisplayConstants.DISP_DT_NORMAL);
        }
    }
}
