using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using POS.Devices;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Exceptions;
using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.FX.Shared.Helper;

namespace WSWD.WmallPos.POS.FX.Win.Devices
{
    public delegate void POSDataEventHandler(string eventData);
    public delegate void POSMsrDataEventHandler(string eventData, string cardNo, string expMY);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="encData">암호화정보</param>
    /// <param name="posEntryMode">POS Entry Mode S, 1</param>
    /// <param name="trackII">trackIII 30</param>
    /// <param name="icCardSeqNo">IC카드 일렵번호 16</param>
    /// <param name="issuerCd">발급기관대표코드 3</param>
    /// <param name="issuePosCode">발급기관 점별코드 7</param>
    public delegate void POSICCardReaderEventHandler(string encData, string posEntryMode,
                                                        string track3Data, string icCardSeqNo, string issuerCd, string issuePosCode);


    /// <summary>
    /// IC상태확인
    /// </summary>
    /// <param name="statusOK"></param>
    public delegate void POSICStatusCheckEventHandler(bool statusOK);

    /// <summary>
    /// 신용IC Card Reader Event
    /// resCode = 00일 때 VAN 승인 요청가능
    /// </summary>
    /// <param name="resCode"></param>
    /// <param name="resErrorMsg"></param>
    /// <param name="retCardInfo"></param>
    public delegate void POSCardICOnEncCardReader(string resCode, string resErrorMsg, SignPadCardInfo retCardInfo);

    /// <summary>
    /// 2nd Generation Request after 카드사와 요청
    /// 결과 받을 때
    /// </summary>
    /// <param name="resCode"></param>
    /// <param name="resErrorMsg"></param>
    public delegate void POSCardICOnRequestCom2ndGenResult(string resCode, string resErrorMsg);

    public class POSMsr : POSDeviceBase
    {
        public event POSMsrDataEventHandler DataEvent = null;
        private OPOSMSR m_device = null;

        public POSMsr()
        {
            m_device = new OPOSMSR();
        }

        #region IPOSDevice Members

        public MSR Config
        {
            get
            {
                return ConfigData.Current.DevConfig.MSR;
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
                m_device.DataEventEnabled = value;
                if (value)
                {
                    TraceHelper.Instance.TraceWrite("m_device_DataEvent added");
                    m_device.DataEvent += new _IOPOSMSREvents_DataEventEventHandler(m_device_DataEvent);
                }
                else
                {
                    TraceHelper.Instance.TraceWrite("m_device_DataEvent deleted");
                    m_device.DataEvent -= new _IOPOSMSREvents_DataEventEventHandler(m_device_DataEvent);
                }
            }
        }


        void m_device_DataEvent(int Status)
        {
            if (this.DataEvent == null)
            {
                return;
            }

            string cardNo = string.Empty;
            string expMY = string.Empty;
            ParseTrackIIData(m_device.Track2Data, out cardNo, out expMY);
            this.DataEvent(m_device.Track2Data, cardNo, expMY);

            m_device.DataEventEnabled = this.Enabled;
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
                throw new Exception("MSR 사용 안함.");
            }

            Status = DeviceStatus.Closed;
            try
            {
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
                throw new Exception("MSR 오픈 오류.", ex);
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

        #region Keyboard event forward

        public void ForwardKeyEvent(string track1, string track2, string track3)
        {
            if (DataEvent != null)
            {
                string cardNo = string.Empty;
                string expMY = string.Empty;
                ParseTrackIIData(track2, out cardNo, out expMY);
                this.DataEvent(track2, cardNo, expMY);
            }
        }

        #endregion

        #region 사용자정의

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trackIIData"></param>
        /// <param name="cardNo">length: 15~19</param>
        /// <param name="expYM">length: 4</param>
        static public void ParseTrackIIData(string trackIIData, out string cardNo, out string expYM)
        {
            trackIIData = trackIIData.Trim();
            cardNo = expYM = string.Empty;
            int idx = trackIIData.IndexOf("=");
            if (idx >= 0)
            {
                cardNo = trackIIData.Substring(0, idx);
                expYM = trackIIData.Substring(idx + 1, Math.Min(4, trackIIData.Length - cardNo.Length - 1));

                // reverse expYM                
                expYM = expYM.Length < 4 ? string.Empty : expYM.Substring(2, 2) + expYM.Substring(0, 2);
            }
            else
            {
                cardNo = trackIIData.Substring(0, Math.Max(19, trackIIData.Length));
            }
        }

        /// <summary>
        /// APP Card parsing
        /// </summary>
        /// <param name="trackIIData"></param>
        /// <param name="cardNo"></param>
        /// <param name="corpCode"></param>
        static public bool ParseAppCard(string trackIIData, out string cardNo, out string corpCode)
        {
            cardNo = string.Empty;
            corpCode = string.Empty;
            if (string.IsNullOrEmpty(trackIIData))
            {
                return false;
            }

            cardNo = trackIIData.Substring(0, Math.Min(16, trackIIData.Length));
            corpCode = trackIIData.Length > 16 ? trackIIData.Substring(16) : string.Empty;

            return trackIIData.Length == 21;
        }

        #endregion
    }
}
