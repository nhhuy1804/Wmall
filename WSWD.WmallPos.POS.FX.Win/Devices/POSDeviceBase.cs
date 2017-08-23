using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.POS.FX.Shared;

namespace WSWD.WmallPos.POS.FX.Win.Devices
{
    public class POSDeviceBase 
    {
        public virtual bool UseYN { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual bool IsOpening { get; set; }

        public event EventHandler OnOpened;

        private DeviceStatus m_status = DeviceStatus.Closed;
        public virtual DeviceStatus Status
        {
            get
            {
                return m_status;
            }
            protected set
            {
                m_status = value;
                Enabled = value == DeviceStatus.Opened;
            }
        }
        public virtual DeviceStatus Open()
        {
            if (OnOpened != null)
            {
                OnOpened(this, EventArgs.Empty);
            }

            return DeviceStatus.Opened;
        }

        public virtual bool Close()
        {
            return true;
        }
    }
}
