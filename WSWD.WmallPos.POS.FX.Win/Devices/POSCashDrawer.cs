using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Config;

using POS.Devices;
using WSWD.WmallPos.FX.Shared.Exceptions;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.POS.FX.Shared.Data;
using WSWD.WmallPos.FX.Shared.Helper;

namespace WSWD.WmallPos.POS.FX.Win.Devices
{
    public class POSCashDrawer : POSDeviceBase
    {
        #region IPOSDevice Members

        public override bool UseYN
        {
            get
            {
                return "1".Equals(Config.Use);
            }
        }

        public CashDrawer Config
        {
            get
            {
                return ConfigData.Current.DevConfig.CashDrawer;
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
                throw new Exception("돈통 사용 안함.");
            }

            Status = DeviceStatus.Closed;
            try
            {
                TraceHelper.Instance.TraceWrite("Open", "CashDrawer: {0}", Config.LogicalName);
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
                throw new Exception("돈통 초기화 오류.", ex);
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

        private OPOSCashDrawer m_device;

        public POSCashDrawer()
        {
            m_device = new OPOSCashDrawer();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OpenDrawer()
        {
            if (m_device == null || !Enabled && Status != DeviceStatus.Opened)
            {
                throw new Exception("돈통 초기화 오류.");
            }

            var rc = m_device.OpenDrawer();
            if (rc != (int)OPOS_Constants.OPOS_SUCCESS)
            {
                throw new Exception("돈통 열기 오류.");
            }
        }
    }
}
