using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using POS.Devices;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Exceptions;
using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.FX.Shared.Helper;
using WSWD.WmallPos.FX.Shared.Utils;
using System.Diagnostics;
using WSWD.WmallPos.FX.Shared.NetComm;


namespace WSWD.WmallPos.POS.FX.Win.Devices
{
    public class POSSignPad : POSDeviceBase
    {
        #region 생성자 & 변수

        private const string TEMP_SIGN_FILE = "WmallSignTemp.bmp";

        private AxKSNET_DONGLELib.AxKSNet_Dongle m_dksNetdongle;

        String m_Reader;
        short m_Rtn = 0;
        string m_sRtn = "";

        string m_sYear = DateTime.Today.Year.ToString().Substring(2, 2);
        string m_sCardData = "3";

        private bool m_useCashICMode = false;
        private int m_icCardReqType = 1;
        private Encoding transferEnc = Encoding.GetEncoding(NetCommConstants.TRANFER_ENCODING);


        public POSSignPad()
        {
            // 저장할 파일의 경로
            this.TempSignFile = Path.Combine(Path.GetTempPath(), TEMP_SIGN_FILE);
            m_dksNetdongle = null;
        }

        #endregion

        #region 데이타이벤트

        /// <summary>
        /// RFCard Reading Event
        /// </summary>
        public event POSDataEventHandler RFCardEvent;

        /// <summary>
        /// 핀패드이벤트
        /// </summary>
        public event POSDataEventHandler PinEvent;

        /// <summary>
        /// ICCard reading event
        /// </summary>
        public event POSICCardReaderEventHandler ICCardEvent;

        /// <summary>
        /// IC상태 확인
        /// </summary>
        public event POSICStatusCheckEventHandler ICStatusEvent;

        /// <summary>
        /// 싸인패드에서 취소이벤트
        /// </summary>
        public event EventHandler SignPadCancelledEvent;

        /// <summary>
        /// DCC Response Event
        /// eventData = empty, error
        /// 아닐때 성공
        /// </summary>
        public event POSDataEventHandler DCCRespEvent;

        /// <summary>
        /// Added on 05.23,
        /// 여전법 개발
        /// </summary>
        public event POSCardICOnEncCardReader CardICReaderEvent;

        /// <summary>
        /// Com2ndGen Result
        /// 코드 . “00 ” . 정상 , 나머 지는 코드 표 참고 .
        /// 승인 정상 응답 후의 2nd Generate 응답이 정상이 아닐 경우 
        /// 반드시 승인 취소를 취소를 해야함.
        /// </summary>
        public event POSCardICOnRequestCom2ndGenResult CardICApproveResult;

        #endregion

        #region IPOSDevice Members

        public SignPad Config
        {
            get
            {
                return ConfigData.Current.DevConfig.SignPad;
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
                throw new Exception("싸인패드 오픈 오류.");
            }

            ResetSignData();
            IsOpening = true;
            Status = DeviceStatus.OpenError;

            try
            {
                short port = 0;   //포트
                int baud = 0;

                //포트와 통신속도를 받아 온다.
                port = Convert.ToInt16(Config.Port);
                baud = Convert.ToInt32(Config.Speed);

                //포트 연결
                m_dksNetdongle.SetComPort(port, baud);

                if (!m_useCashICMode)
                {
                    m_Rtn = m_dksNetdongle.SetComPortEncReaderSync(port, baud);
                    m_sRtn = m_dksNetdongle.EncReaderComInit(m_sYear, m_sCardData);
                    m_Reader = m_sRtn.Length > 2 ? m_sRtn.Substring(2, 16) : m_sRtn;
                    m_sRtn = m_dksNetdongle.EncReaderComCheck(m_Reader, "K");
                    m_sRtn = m_dksNetdongle.EncReaderComCheck(m_Reader, "S");
                }

                if (m_dksNetdongle.CheckPort() > 0)
                {
                    int rtn = m_dksNetdongle.SignComReqA0();
                    if (rtn == 0)
                    {
                        Status = DeviceStatus.Opened;

                        m_dksNetdongle.OnRecvSignData +=
                            new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvSignDataEventHandler(m_dksNetdongle_OnRecvSignData);
                        m_dksNetdongle.OnReadMSR +=
                            new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnReadMSREventHandler(m_dksNetdongle_OnReadMSR);
                        m_dksNetdongle.OnRecvPinData +=
                            new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvPinDataEventHandler(m_dksNetdongle_OnRecvPinData);
                        m_dksNetdongle.OnRecvF2 +=
                            new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvF2EventHandler(m_dksNetdongle_OnRecvF2);
                        m_dksNetdongle.OnRecvBC += new EventHandler(m_dksNetdongle_OnRecvBC);

                        m_dksNetdongle.OnRecv51 +=
                            new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecv51EventHandler(m_dksNetdongle_OnRecv51);
                        m_dksNetdongle.OnRecvF0 +=
                            new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvF0EventHandler(m_dksNetdongle_OnRecvF0);

                        m_dksNetdongle.OnRecv54 += new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecv54EventHandler(m_dksNetdongle_OnRecv54);

                        // new event 05/18
                        m_dksNetdongle.OnRecvEncReaderRemove += new System.EventHandler(this.m_dksNetdongle_OnRecvEncReaderRemove);
                        m_dksNetdongle.OnRecvEncReaderInsert += new System.EventHandler(this.m_dksNetdongle_OnRecvEncReaderInsert);

                        m_dksNetdongle.OnRecvEncReaderCard += new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvEncReaderCardEventHandler(this.m_dksNetdongle_OnRecvEncReaderCard);

                        m_dksNetdongle.OnRecvEncReader2ndGen += new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvEncReader2ndGenEventHandler(m_dksNetdongle_OnRecvEncReader2ndGen);
                    }
                    else
                    {
                        Status = DeviceStatus.InitError;
                    }
                }
                else
                {
                    TraceHelper.Instance.TraceWrite("POSSignPad", "SignPad Port Error.");
                    Status = DeviceStatus.OpenError;
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
                Status = DeviceStatus.OpenError;
                //throw new Exception("싸인패드 오픈 오류.", ex);
            }
            finally
            {
                IsOpening = false;
            }

            base.Open();
            return Status;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReInitialize(bool modeNormal)
        {
            short port = 0;   //포트
            int baud = 0;

            //포트와 통신속도를 받아 온다.
            port = Convert.ToInt16(Config.Port);
            baud = Convert.ToInt32(Config.Speed);

            if (modeNormal)
            {
                //포트 연결
                m_Rtn = m_dksNetdongle.SetComPortEncReaderSync(port, baud);
                m_dksNetdongle.SetComPort(port, baud);
            }
            else
            {
                m_Rtn = m_dksNetdongle.SetComPortEncReaderSync(port, baud);
                m_sRtn = m_dksNetdongle.EncReaderComInit(m_sYear, m_sCardData);
                m_Reader = m_sRtn.Length > 2 ? m_sRtn.Substring(2, 16) : m_sRtn;
                m_sRtn = m_dksNetdongle.EncReaderComCheck(m_Reader, "K");
                m_sRtn = m_dksNetdongle.EncReaderComCheck(m_Reader, "S");
            }

            m_dksNetdongle.CheckPort();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Close()
        {
            if (m_dksNetdongle != null && !m_dksNetdongle.IsDisposed)
            {
                m_dksNetdongle.SignComReqAC();
                m_dksNetdongle.SignComReqA0();
                m_dksNetdongle.ClosePort();
                if (Status == DeviceStatus.Opened)
                {
                    m_dksNetdongle.OnRecvSignData -=
                        new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvSignDataEventHandler(m_dksNetdongle_OnRecvSignData);
                    m_dksNetdongle.OnReadMSR -=
                        new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnReadMSREventHandler(m_dksNetdongle_OnReadMSR);
                    m_dksNetdongle.OnRecvPinData -=
                        new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvPinDataEventHandler(m_dksNetdongle_OnRecvPinData);
                    m_dksNetdongle.OnRecvF2 -=
                        new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvF2EventHandler(m_dksNetdongle_OnRecvF2);
                    m_dksNetdongle.OnRecvBC -= new EventHandler(m_dksNetdongle_OnRecvBC);
                    m_dksNetdongle.OnRecvF0 -=
                        new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvF0EventHandler(m_dksNetdongle_OnRecvF0);

                    // 여전법 변경 0702
                    // 새로운 dongle ocx
                    m_dksNetdongle.OnRecv51 -=
                        new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecv51EventHandler(m_dksNetdongle_OnRecv51);

                    // 여전법 변경 0702
                    m_dksNetdongle.OnRecv54 -=
                        new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecv54EventHandler(m_dksNetdongle_OnRecv54);

                    // new event removed

                    m_dksNetdongle.OnRecvEncReaderRemove -= new System.EventHandler(this.m_dksNetdongle_OnRecvEncReaderRemove);
                    m_dksNetdongle.OnRecvEncReaderInsert -= new System.EventHandler(this.m_dksNetdongle_OnRecvEncReaderInsert);
                    m_dksNetdongle.OnRecvEncReaderCard -=
                        new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvEncReaderCardEventHandler(this.m_dksNetdongle_OnRecvEncReaderCard);

                }

                m_dksNetdongle = null;
            }

            Status = DeviceStatus.Closed;
            return Status == DeviceStatus.Closed;
        }

        #endregion

        #region SIGN 정보

        /// <summary>
        /// 마지막 base64 sign data
        /// </summary>
        public string LastSignData { get; private set; }

        public string TempSignFile { get; private set; }

        #endregion

        #region 요청함수

        public void Initialize(AxKSNET_DONGLELib.AxKSNet_Dongle dongle)
        {
            Initialize(dongle, false);
        }

        public void Initialize(AxKSNET_DONGLELib.AxKSNet_Dongle dongle, bool useCashIC)
        {
            m_dksNetdongle = dongle;
            m_useCashICMode = useCashIC;
            // m_dksNetdongle.EnableLogging();
            // m_dksNetdongle.DebugLog(1);

            this.Open();
        }

        /// <summary>
        /// 전자서명요청
        /// Return false if 전자서명요청실패 / 오픈하지 않은상태
        /// </summary>
        /// <param name="msg1"></param>
        /// <param name="msg2"></param>
        /// <returns></returns>
        public bool RequestSign(string msg1, string msg2)
        {
            if (Status != DeviceStatus.Opened)
            {
                return false;
            }

            ResetSignData();

            // 서명이미지를 화면에 표시할때 확대한다
            m_dksNetdongle.SetImageSize(2);

            // 최소 픽셀수를 지정한다.(10pixel 이상 서명이 되어야 서명한것으로 생각하고 처리함)
            m_dksNetdongle.SetMinSignPixel(10);

            // 서명완료 후 3초 후에 자동으러 서명이 전송되게 끔 처리
            // 무한
            m_dksNetdongle.SetReqSignTimeout(0);

            //axKSNet_Dongle1.SignComReqAC();

            // 전자서명 입력 요청
            int rtn = m_dksNetdongle.SignComReqA1(msg1, msg2, string.Empty, string.Empty);

            return rtn == 0;
        }

        /// <summary>
        /// Close and receive sign data
        /// 
        /// </summary>
        /// <returns>true if success</returns>
        public bool CloseSign()
        {
            // 데이터를 BMP 파일로 저장
            try
            {
                if (string.IsNullOrEmpty(LastSignData))
                {
                    LastSignData = m_dksNetdongle.GetSignComReqA2(3);
                }

                m_dksNetdongle.SaveImage(LastSignData, TempSignFile);
            }
            catch { }

            return !string.IsNullOrEmpty(LastSignData);
        }

        /// <summary>
        /// 승인후 SIGN파일 저장
        /// </summary>
        /// <param name="approvalNo"></param>
        public void SaveSignData(string approvalNo)
        {
            SaveSignData(approvalNo, TempSignFile);
        }

        /// <summary>
        /// 승인후 SIGN파일 저장
        /// </summary>
        /// <param name="approvalNo"></param>
        /// <param name="tempSignFilePath"></param>
        public void SaveSignData(string approvalNo, string tempSignFilePath)
        {
            if (!File.Exists(tempSignFilePath))
            {
                return;
            }

            try
            {
                // 영업일자-점코드-포스번호-거래번호-승인번호.SIGN 
                string fileName = string.Format("{0}-{1}-{2}-{3}-{4}.SIGN",
                    ConfigData.Current.AppConfig.PosInfo.SaleDate,
                    ConfigData.Current.AppConfig.PosInfo.StoreNo,
                    ConfigData.Current.AppConfig.PosInfo.PosNo,
                    ConfigData.Current.AppConfig.PosInfo.TrxnNo,
                    approvalNo);

                File.Copy(tempSignFilePath, Path.Combine(FXConsts.FOLDER_DATA_SIGN.GetFolder(), fileName));
                File.Delete(tempSignFilePath);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Backup sign file to temp file
        /// </summary>
        /// <param name="newTempSignFile"></param>
        public string BackupSignFile()
        {
            if (!File.Exists(TempSignFile))
            {
                return string.Empty;
            }

            string newTempSignFile = Path.Combine(Path.GetTempPath(), "WmallSignTempDCC.bmp");
            File.Copy(TempSignFile, newTempSignFile, true);
            return newTempSignFile;
        }

        /// <summary>
        /// RFID card reading request
        /// </summary>
        /// <param name="msg1"></param>
        /// <param name="msg2"></param>
        /// <param name="msg3"></param>
        /// <param name="msg4"></param>
        /// <returns></returns>
        public bool RequestRFCardRead(string msg1, string msg2, string msg3, string msg4)
        {
            int result = 0;

            try
            {
                result = m_dksNetdongle.SignComReqA8(2, msg1, msg2, msg3, msg4, 10);
            }
            catch
            {
            }

            return result == 0;
        }

        /// <summary>
        /// PIN Data 입력 요청
        /// </summary>
        /// <param name="msg1">메세지1</param>
        /// <param name="msg2">메세지2</param>
        /// <param name="msg3">메세지3</param>
        /// <param name="msg4">메세지4</param>
        /// <param name="iExp_flag">0:숫자*처리, 1:숫자표시</param>
        /// <param name="iMax_Len">최대 자릿수</param>
        /// <returns>0:성공, 음수:실패</returns>
        public int RequestPinData(string msg1, string msg2, string msg3, string msg4, short iExp_flag, short iMax_Len)
        {
            return m_dksNetdongle.PinComReqA3(msg1, msg2, msg3, msg4, 1, iMax_Len, 1);
        }

        /// <summary>
        /// PIN Data 입력 요청
        /// Encrypted 된 데이타 가져옴
        /// </summary>
        /// <param name="beforeMsgLine1"></param>
        /// <param name="beforeMsgLine2"></param>
        /// <param name="afterMsgLine1"></param>
        /// <param name="afterMsgLine2"></param>
        /// <param name="cardNo"></param>
        /// <param name="minPassLength"></param>
        /// <param name="maxPassLength"></param>
        /// <param name="iTimeout"></param>
        /// <returns></returns>

        public int RequestPinData(string beforeMsgLine1, string beforeMsgLine2, string afterMsgLine1, string afterMsgLine2, string cardNo, short minPassLength,
            short maxPassLength, short iTimeout)
        {
            return m_dksNetdongle.PinComReqF2(beforeMsgLine1, beforeMsgLine2, afterMsgLine1,
                afterMsgLine2, cardNo, minPassLength, maxPassLength, iTimeout);
        }

        /// <summary>
        /// 핀&사인 요청취소
        /// </summary>
        public void ClearPinDataRequest()
        {
            m_dksNetdongle.SignComReqA0();
        }

        /// <summary>
        /// 카드비밀번호 입력요청
        /// 
        /// 여전법 변경 0622
        /// PinComReqA4 -> PinComReqF2
        /// </summary>
        /// <param name="msg1"></param>
        /// <param name="msg2"></param>
        /// <param name="msg3"></param>
        /// <param name="msg4"></param>
        /// <param name="cardNo"></param>
        /// <param name="passLength"></param>
        /// <returns></returns>
        public int RequestCardPinData(string msg1, string msg2, string msg3, string msg4, string cardNo,
            short minPassLength, short maxPassLength)
        {
            return m_dksNetdongle.PinComReqA4(msg1, msg2, msg3, msg4, cardNo, minPassLength, maxPassLength, 1);
        }

        /// <summary>
        /// ICCard승인요청시
        /// 난수없이 자동생성 (임시)
        /// Return > 0 OK
        /// </summary>
        /// <param name="msg1"></param>
        /// <param name="msg2"></param>
        /// <param name="msg3"></param>
        /// <param name="msg4"></param>
        /// <param name="timeout"></param>
        public int RequestICCardAppr(string msg1, string msg2, string msg3, string msg4,
            int timeout)
        {
            return RequestICCardAppr(string.Empty, msg1, msg2, msg3, msg4, timeout);
        }

        /// <summary>
        /// ICCard승인요청시
        /// 
        /// </summary>
        /// <param name="randNum"></param>
        /// <param name="msg1"></param>
        /// <param name="msg2"></param>
        /// <param name="msg3"></param>
        /// <param name="msg4"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public int RequestICCardAppr(string randNum, string msg1, string msg2, string msg3, string msg4,
            int timeout)
        {
            m_icCardReqType = 1;
            if (string.IsNullOrEmpty(randNum))
            {
                randNum = GetRandNum();
            }
            return m_dksNetdongle.SignComReq51("1", randNum, msg1, msg2, msg3, msg4, timeout);
        }

        /// <summary>
        /// ICCard 조회요청
        /// Return > 0 OK
        /// </summary>
        /// <param name="msg1"></param>
        /// <param name="msg2"></param>
        /// <param name="msg3"></param>
        /// <param name="msg4"></param>
        /// <param name="timeout"></param>
        public int RequestICCardView(string msg1, string msg2, string msg3, string msg4,
            int timeout)
        {
            m_icCardReqType = 2;
            string randNum = GetRandNum();
            return m_dksNetdongle.SignComReq51("1", randNum, msg1, msg2, msg3, msg4, timeout);
        }

        /// <summary>
        /// IC상태확인
        /// </summary>
        public void CheckICStatus()
        {
            int ret = m_dksNetdongle.SignComReqE0();
        }

        /// <summary>
        /// ICCard 입력요청
        /// </summary>
        /// <returns></returns>
        public int RequestICCardNo()
        {
            string data = string.Empty;
            data += DateTime.Now.ToString("yyMMddHHmmss");
            data += "000001004";    // 금액
            data += "10";           // 대기시간

            // 카드번호 요청
            int rtn = m_dksNetdongle.SignComReqE2(ToHexString(transferEnc.GetBytes(data)));
            return rtn;
        }

        /// <summary>
        /// SIGN PAD 초기화
        /// </summary>
        public void RequestResetSignPad()
        {
            if (m_dksNetdongle != null)
            {
                m_dksNetdongle.SignComReqAC();
            }
        }

        /// <summary>
        /// "10", "KRW", "10,000    ", "USD", "8.96      ", "   Rate: USD 1 = KRW 1,115.80   ", "                             "
        /// </summary>
        /// <param name="localCode">기준통화코드</param>
        /// <param name="localAmount">기준통화금액</param>
        /// <param name="homeCode">자국통화코드</param>
        /// <param name="homeAmount">자국통화금액</param>
        /// <param name="rvsRate">역환율표시</param>
        /// <param name="markupText"></param>
        public void RequestDCCRateSelection(string localCode, string localAmount,
            string homeCode, string homeAmount,
            string rvsRate, string markupText)
        {
            m_dksNetdongle.SignComReq54("20", localCode, localAmount.PadRight(10, ' '), homeCode, homeAmount.PadRight(10, ' '),
                rvsRate.PadRight(32, ' '), markupText.PadRight(30, ' '));
        }

        #endregion

        #region 이벤트정의

        void m_dksNetdongle_OnRecvPinData(object sender, AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvPinDataEvent e)
        {
            if (e.data != null && e.data.Length > 0)
            {
                if (PinEvent != null)
                {
                    PinEvent(e.data);
                }
            }
        }

        void m_dksNetdongle_OnRecvSignData(object sender, AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvSignDataEvent e)
        {
            //Trace.WriteLine("m_dksNetdongle_OnRecvSignData", "program");
            LastSignData = m_dksNetdongle.GetSignComReqA2(2000);
        }

        void m_dksNetdongle_OnReadMSR(object sender, AxKSNET_DONGLELib._DKSNet_DongleEvents_OnReadMSREvent e)
        {
            if (RFCardEvent != null)
            {
                RFCardEvent(e.cardnum);
            }
        }

        /// <summary>
        /// 1. GetCashICData 함수 호출시 type을 1로 한 경우:
        ///   암호화정보(132) + Pos Entry Mode( “S”, 1 ) + track III(30) + IC카드 일련번호(16) + 
        ///   발급기관 대표코드(3) + 발급기관 점별코드(7)
        /// 2. GetCashICData 함수 호출시 type을 2로 한 경우:
        /// IC카드 일련번호 (16) + 발급기관 대표코드(3) + 발급기관 점별코드(7)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //void m_dksNetdongle_OnRecvCashICData(object sender, AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvCashICDataEvent e)
        //{
        //    //m_dksNetdongle.OnRecvCashICData -=
        //    //    new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvCashICDataEventHandler(m_dksNetdongle_OnRecvCashICData);
        //    ParseCashICCard(e.data);
        //}

        void m_dksNetdongle_OnRecv51(object sender, AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecv51Event e)
        {
            //Trace.WriteLine("Dongle RecvCashIC Hex Org:" + e.data);
            RaiseCardICCardData(ToBytes(e.data));
        }

        void RaiseCardICCardData(byte[] data)
        {
            string respCode, encData, posEntryMode;
            string track3Data, icCardSeqNo;
            string issuerCd, issuePosCode;
            ParseCashICCardData(data, out respCode, out encData,
                out posEntryMode, out track3Data, out icCardSeqNo, out issuerCd, out issuePosCode);
        }

        void m_dksNetdongle_OnRecvF0(object sender, AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvF0Event e)
        {
            if (ICStatusEvent != null)
            {
                byte[] data = ToBytes(e.data);
                string errorCode = transferEnc.GetString(data, 0, 2);
                ICStatusEvent("00".Equals(errorCode));
            }
        }

        /// <summary>
        /// IC Card No 요청결과
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_dksNetdongle_OnRecvF2(object sender, AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvF2Event e)
        {
            // hex string 변환
            byte[] data = ToBytes(e.data);

            string errorCode = transferEnc.GetString(data, 0, 2);
            if (errorCode.Equals("00") == false)
            {
                return;
            }
        }

        /// <summary>
        /// 싸인패드에서 취소
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_dksNetdongle_OnRecvBC(object sender, EventArgs e)
        {
            if (this.SignPadCancelledEvent != null)
            {
                SignPadCancelledEvent(sender, e);
            }
        }

        void m_dksNetdongle_OnRecv54(object sender, AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecv54Event e)
        {
            string errorCode = e.data.Substring(0, 2);
            if (errorCode.Equals("00") == false)
            {
                DCCRespEvent(string.Empty);
            }
            else
            {
                DCCRespEvent(e.data.Substring(2, 2));
            }
        }

        #endregion

        #region 신용 ICCard Reading Events SCSPro 단말기

        private int m_icCardTransAmt = 0;

        /// <summary>
        /// 거래금액 산정
        /// </summary>
        /// <param name="amount"></param>
        public void SetICTransAmount(int amount)
        {
            m_icCardTransAmt = amount;
        }


        /// <summary>
        /// 신용IC 우선거래 확인
        /// </summary>
        /// <returns></returns>
        private bool IsCardICSupport()
        {
            return true;
        }


        #region OnRecvEncReaderCard, 신용 IC Card 삽입 관련 이벤트

        /// <summary>
        /// EVENT: Read card infomation
        /// 
        /// Read MSR DATA or IC DATA EVENT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_dksNetdongle_OnRecvEncReaderCard(object sender, AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvEncReaderCardEvent e)
        {
            // 이벤트 처리 없으면 무시
            if (CardICReaderEvent == null)
            {
                return;
            }

            // Reset 거래금액
            string strTransAmt = string.Format("{0:d9}", m_icCardTransAmt);
            string errorMsg = GetRecvEncReaderErrorMsg(e.resCode);
            SignPadCardInfo cardInfo = null;
            if (e.resCode == "00")
            {
                #region Get card info

                LogUtils.Instance.Log(@"카드정보 읽기");
                LogUtils.Instance.Log(String.Format("RES CODE [{0}] ", e.resCode));
                LogUtils.Instance.Log(String.Format("CardType [{0}]", e.cardType));

                string noEncCardNo = e.noEncCardNo;
                string svcCode = string.Empty;

                int idx_Check = noEncCardNo.IndexOf('=');

                if (idx_Check > -1)
                {
                    noEncCardNo = e.noEncCardNo.Substring(1, idx_Check - 1);
                    // 여전법 추가(KSK) 현금영수증 카드 처리 수정
                    if (e.noEncCardNo.Length == idx_Check + 1)
                    {
                        svcCode = "";
                    }
                    else
                    {
                        svcCode = e.noEncCardNo.Substring(idx_Check + 1, 1);
                    }
                }
                else
                {
                    noEncCardNo = e.noEncCardNo.Substring(1, e.noEncCardNo.Length - 1);
                }

                LogUtils.Instance.Log("Service Code [{0}]", svcCode);

                #region 여전법 2016.09.27 추가 : 사인패드에서 입력받은 마스킹 카드 번호의 마스킹 부분을 재마스킹 함
                if (noEncCardNo.Length > 12)
                {
                    string preCardNum = noEncCardNo.Substring(0, 6);
                    string endCardNum = noEncCardNo.Substring(12, noEncCardNo.Length - 12);
                    noEncCardNo = string.Format("{0}******{1}", preCardNum, endCardNum);
                }

                //Console.WriteLine("[{0}]", noCardNum);
                #endregion       
          
                cardInfo = new SignPadCardInfo()
                {
                    EncCardNo = e.encCardNo,
                    NoEncCardNo = noEncCardNo,
                    EncData = e.encData,
                    CardType = e.cardType,
                    CardGubun = e.noEncCardNo.Substring(0, 1),
                    Count = e.count,
                    NoEncExtCardNo = svcCode,
                    ServiceCode = svcCode,
                    ResCode = e.resCode,
                    Reader = e.reader,
                    TransAmt = strTransAmt
                };

                #endregion
            }

            CardICReaderEvent(e.resCode, errorMsg, cardInfo);

            // e.resCode = 01~07, error fallback
        }

        /// <summary>
        /// 1. 호출 시 FB 일경우
        /// 2.FallBack 호출
        /// 
        /// </summary>
        public string RequestFallBackComCard(out string errorMessage, int timeOut)
        {
            string l_sDate = DateTime.Now.ToString("yyMMddHHmmss");
            string sRtn = m_dksNetdongle.EncReaderComCard("FB", l_sDate, string.Format("{0:d9}", m_icCardTransAmt), timeOut.ToString());
            errorMessage = GetRecvEncReaderErrorMsg(sRtn);
            return sRtn;
        }

        /// <summary>
        /// Reader기 초기화
        /// </summary>
        public void ResetICCardReader()
        {
            m_sRtn = m_dksNetdongle.EncReaderComInit(m_sYear, m_sCardData);
            m_Reader = m_sRtn.Length > 2 ? m_sRtn.Substring(2, 16) : m_sRtn;
            m_sRtn = m_dksNetdongle.EncReaderComCheck(m_Reader, "K");
            m_sRtn = m_dksNetdongle.EncReaderComCheck(m_Reader, "S");
        }

        /// <summary>
        /// 카드 삽입 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_dksNetdongle_OnRecvEncReaderInsert(object sender, EventArgs e)
        {
            // 이벤트 처리 없으면 무시
            if (CardICReaderEvent == null)
            {
                return;
            }

            LogUtils.Instance.Log("Call m_dksNetdongle_OnRecvEncReaderInsert");

            string l_sDate = DateTime.Now.ToString("yyMMddHHmmss");

            if (m_icCardTransAmt == 0)
            {
                CardICReaderEvent("FF", "거래금액을 산정 바랍니다. (SetICTransAmount함수참조)", null);
                return;
            }

            // "000001004"
            //포트와 통신속도를 받아 온다.
            short port = Convert.ToInt16(Config.Port);
            int baud = Convert.ToInt32(Config.Speed);
            m_dksNetdongle.SetComPortEncReaderSync(port, baud);

            m_sRtn = m_dksNetdongle.EncReaderComCard("IC", l_sDate, string.Format("{0:d9}", m_icCardTransAmt), "10");

            // 정상일때 m_dksNetdongle_OnRecvEncReaderCard 호출
            LogUtils.Instance.Log(String.Format("Call EncReaderComCard rtn[{0}]", m_sRtn));
            LogUtils.Instance.Log("END m_dksNetdongle_OnRecvEncReaderInsert");
        }

        /// <summary>
        /// 카드 제거 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_dksNetdongle_OnRecvEncReaderRemove(object sender, EventArgs e)
        {
            LogUtils.Instance.Log("Call axKSNet_Dongle1_OnRecvEncReaderRemove");
        }


        /// <summary>
        /// KSNET Dongle OCX
        /// ErrorCode & Msg
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string GetRecvEncReaderErrorMsg(string resCode)
        {
            switch (resCode)
            {
                case "00":
                    return "정상";
                case "20":
                    return "암호화 오류";
                case "21":
                    return "S/W 유효성 오류";
                case "40":
                    return "타임 아웃";
                case "50":
                    return "카드 미 입력 (IC (IC 미 삽입 )";
                case "60":
                    return "2nd Generation 에러 카드 거절";
                case "90":
                    return "리더 상태 변경 실패";
                case "91":
                    return "리더 인증 코드 불일치";
                case "01":
                    return "chip 미 응답";
                case "02":
                    return "application 미 존재";
                case "03":
                    return "chip 데이터 읽기 실패";
                case "04":
                    return "mandatory 데이터 미 포함";
                case "05":
                    return "CVM 커맨드 응답실패";
                case "06":
                    return "EMV 커맨드 오 설정";
                case "07":
                    return "터미널 (리더 ) 오 동작";
                case "30":
                    return "chip block";
                case "31":
                    return "application block";
                case "32":
                    return "카드 자체 block";
                case "11":
                    return "키 유효기간 지남";
                case "12":
                    return "암호화 키 생성 실패";
                case "13":
                    return "이미 암호화 키 있음";
                case "14":
                    return "KEY 유효성 검증 오류";
                case "15":
                    return "IPEK KEY 없음";
                case "16":
                    return "사용될 IPEK 의 년도 Data 없음";
                case "ZA":
                    return "STX 수신 오류";
                case "ZB":
                    return "ETX 수신 오류";
                case "ZC":
                    return "LRC LRC 오류";
                case "ZD":
                    return "단말기 mode 오류";
                case "ZE":
                    return "함수 인자 값 틀림";
                case "ZF":
                    return "시리얼포트 설정 하지 않음";
                case "ZG":
                    return "시리얼포트가 열려 있지 않음";
                case "ZH":
                    return "데이터 생성 실패";
                case "ZI":
                    return "데이터 송신 실패";
                case "ZJ":
                    return "데이터 수신 실패";
                case "ZK":
                    return "데이터 송수신 대기 시간 초과";
                default:
                    return string.Empty;
            }
        }


        #endregion

        #region 2nd Generate request

        /// <summary>
        /// IC Card 정상시,
        /// 2nd Generation Request after 카드사와 요청
        /// VAN 승인 요청 및 응답
        /// 
        /// 2번 호출함
        /// - 카드 삽입 후 승인 요청 후 
        /// - RequestReaderCom2ndGen 후출 정상 아닐 때, 취소 요청 후 호출 해서 종료 함
        /// <param name="readCardInfo">처음 삽입시 받은 카드정보</param>
        /// <param name="errorMsg">오류 메시지</param>
        /// </summary>
        public string RequestReaderCom2ndGen(SignPadCardInfo readCardInfo, string encData, out string errorMsg)
        {
            string dateTime = DateTime.Now.ToString("yyMMddHHmmss");
            string sRtn = m_dksNetdongle.EncReaderCom2ndGen(dateTime, readCardInfo.TransAmt,
                readCardInfo.ResCode, readCardInfo.Count, encData);

            LogUtils.Instance.Log("Call RequestReaderCom2ndGen rtn[{0}]", sRtn);

            errorMsg = GetRecvEncReaderErrorMsg(sRtn);
            return sRtn;
        }

        /// <summary>
        /// 카드번호 정상 받을때 승인 후
        /// RequestReaderCom2ndGen 호출 해서 
        /// 결과 이벤트 받을때 확인
        /// - 취소 
        /// - 완료 (Basket 처리)  resCode = 00 종료, 00 아닐 때 취소 요청 해야함
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_dksNetdongle_OnRecvEncReader2ndGen(object sender, AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvEncReader2ndGenEvent e)
        {
            LogUtils.Instance.Log("Call m_dksNetdongle_OnRecvEncReader2ndGen");
            CardICApproveResult(e.resCode, GetRecvEncReaderErrorMsg(e.resCode));
        }

        #endregion

        #endregion

        #region privates

        private void ResetSignData()
        {
            LastSignData = string.Empty;
            if (File.Exists(TempSignFile))
            {
                File.Delete(this.TempSignFile);
            }
        }

        #endregion

        #region Event forwarding from InputForm

        #endregion

        #region ICCard helper

        // hex string을 byte[]로 변환
        private byte[] ToBytes(string hexString)
        {

            byte[] result = new byte[hexString.Length / 2];

            for (int i = 0; i < hexString.Length; i += 2)
            {
                result[i / 2] = (byte)Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return result;

        }

        // byte[]를 hex string으로 변환
        private string ToHexString(byte[] byteArray)
        {

            string result = "";

            foreach (byte ch in byteArray)
            {
                result += string.Format("{0:X2}", ch);
            }

            return result;
        }

        void ParseCashICCardData(byte[] data, out string respCode, out string encData, out string posEntryMode,
            out string track3Data, out string icCardSeqNo,
            out string issuerCd, out string issuePosCode)
        {
#if DEBUG
            Trace.WriteLine("Dongle RecvCashIC Hex:" + data.BytesToHexString());
#endif

            respCode = String.Empty;
            posEntryMode = "S";
            if (m_icCardReqType == 1)
            {
                byte[] d = data.Copy(0, 2);
                respCode = transferEnc.GetString(d);

                d = data.Copy(2, 132);
                

                // 20170313.osj
                // BytesToHexString 에 대하여 HexString으로 처리되지 않아 변경

                encData = transferEnc.GetString(d);
                
                //encData = d.BytesToHexString();

#if DEBUG
                var bd = transferEnc.GetBytes(encData);
                Trace.WriteLine(string.Format("CashIC-EncData: {0} {1}", bd.Length, encData));
#endif

                d = data.Copy(134, 1);
                posEntryMode = transferEnc.GetString(d);

                d = data.Copy(135, 30);
                track3Data = transferEnc.GetString(d);

                d = data.Copy(165, 16);
                icCardSeqNo = transferEnc.GetString(d);

                d = data.Copy(181, 3);
                issuerCd = transferEnc.GetString(d);

                d = data.Copy(184, 7);
                issuePosCode = transferEnc.GetString(d);

                this.ICCardEvent(encData, posEntryMode, track3Data, icCardSeqNo, issuerCd, issuePosCode);
            }
            else
            {
                encData = string.Empty;
                track3Data = string.Empty;

                byte[] d = data.Copy(0, 2);
                respCode = transferEnc.GetString(d);

                d = data.Copy(2, 16);
                icCardSeqNo = transferEnc.GetString(d);

                d = data.Copy(18, 3);
                issuerCd = transferEnc.GetString(d);

                d = data.Copy(21, 7);
                issuePosCode = transferEnc.GetString(d);

                this.ICCardEvent(string.Empty, posEntryMode, string.Empty, icCardSeqNo, issuerCd, issuePosCode);
            }
        }

        string GetRandNum()
        {
            return new string(' ', 32);
        }


        #endregion

        #region Create telegram - Dongle Version 예전법

        private string CombineTelegram(string carType, string reader, string noEncCardNo, string pinData, string signData)
        {
            string str_telegram = "";
            string str_telegramlen = "";
            byte[] bt_tellegram = { };


            str_telegram += (char)0x02;

            bt_tellegram = Encoding.UTF8.GetBytes(str_telegram);

            str_telegram += "IC"; // 거래구분

            str_telegram += "01"; // 업무구분

            // 전문구분
            str_telegram += "0200";

            str_telegram += "N"; // 거래형태
            str_telegram += "DPT0TEST05"; // 단말기번호
            str_telegram += "    "; // 업체정보
            str_telegram += "000000000000"; // 전문일련번호

            str_telegram += "S";
            int n;
            for (n = 0; n < 20; n++)
            {
                str_telegram += " ";
            }

            str_telegram += noEncCardNo;
            for (n = 0; n < 20; n++)
            {
                str_telegram += " ";
            }
            str_telegram += "1";

            str_telegram += "###KSNETTEST0001"; // SW 모델번호

            str_telegram += reader;

            for (n = 0; n < 40; n++)
            {
                str_telegram += " ";
            }

            str_telegram += pinData;
            for (n = 0; n < 37 - pinData.Length - 1; n++)
            {
                str_telegram += " ";
            }

            str_telegram += (char)0x1C;

            str_telegram += "00";

            str_telegram += "000000001004";  // 총금액
            str_telegram += "000000000000";  // 봉사료
            str_telegram += "000000000000";  // 세금(부가세)
            str_telegram += "000000000000";  // 공급금액
            str_telegram += "000000000000";  // 면세금액


            for (n = 0; n < 12; n++)
            {
                str_telegram += " ";
            }

            for (n = 0; n < 6; n++)
            {
                str_telegram += " ";
            }

            for (n = 0; n < 13; n++) { str_telegram += " "; }
            for (n = 0; n < 2; n++) { str_telegram += " "; }
            for (n = 0; n < 30; n++) { str_telegram += " "; }
            for (n = 0; n < 4; n++) { str_telegram += " "; }
            for (n = 0; n < 20; n++) { str_telegram += " "; }
            for (n = 0; n < 1; n++) { str_telegram += " "; }
            for (n = 0; n < 1; n++) { str_telegram += " "; }
            for (n = 0; n < 1; n++) { str_telegram += " "; }
            for (n = 0; n < 1; n++) { str_telegram += " "; }
            for (n = 0; n < 30; n++) { str_telegram += " "; }
            for (n = 0; n < 60; n++) { str_telegram += " "; }

            if (signData.Length > 0)
            {
                String signDatalen = "";
                signDatalen = String.Format("{0:D4}", signData.Length - 1);

                str_telegram += "S";
                str_telegram += "83";
                str_telegram += "0000000000000000";
                str_telegram += signDatalen;
                str_telegram += signData;

            }
            else
            {
                str_telegram += "N";
            }

            str_telegram += (char)0x03;
            str_telegram += (char)0x0D;

            str_telegramlen = String.Format("{0:D4}", str_telegram.Length - 1);

            string buf = String.Format("{0}{1}", str_telegramlen, str_telegram);

            return buf;
        }

        #endregion

        #region 여전법 추가 0722

        /// <summary>
        /// 포스 무결성 확인
        /// </summary>
        /// <returns></returns>
        public bool CheckPOSIntegrity()
        {
            string rtn = m_dksNetdongle.EncReaderComInit(m_sYear, m_sCardData);
            string reader = rtn.Length > 2 ? rtn.Substring(2, 16) : rtn; 
            
            rtn = m_dksNetdongle.EncReaderComCheck(reader, "K");
            if (rtn != "00")
            {
                return false;
            }

            rtn = m_dksNetdongle.EncReaderComCheck(reader, "S");
            if (rtn != "00")
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 여전법 추가 0729
        /// KEYIN/IC 요청
        /// <param name="reqType">IC, KI, FB, </param>
        /// </summary>
        public void RequestEncReaderCard(string reqType)
        {
            m_dksNetdongle.EncReaderComCard(reqType, DateTime.Now.ToString("yyMMddHHmmss"), m_icCardTransAmt.ToString().PadLeft(9, '0'), "50");
        }

        #endregion
    }

}
