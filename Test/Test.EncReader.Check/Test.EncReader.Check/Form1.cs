using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Test.EncReader.Check
{
    enum FLAG_ENC : int { OFF = 0, ON }

    public partial class Form1 : Form
    {


        static public String m_Redaer;

        short m_Rtn = 0;
        short m_Port = 0;
        int m_Baud = 0;

        string m_sYear = "16";
        string m_sCardData = "3";

        string m_sRtn = "";
        //string m_sDate = "";

        string m_pinData = "";
        string m_signData = "";


        public Form1()
        {
            InitializeComponent();

            OpenEncReader();
            //init_Signpad();
        }

        #region FUNC

        private byte[] StringToByte(string str)
        {
            byte[] StrByte = Encoding.UTF8.GetBytes(str);
            return StrByte;
        }


        private void init_Signpad()
        {
            axKSNet_Dongle1.SetImageSize(2);
            axKSNet_Dongle1.SetReqSignTimeout(3);
            int i_rtn = axKSNet_Dongle1.SetComPortEncReaderSync(m_Port, m_Baud);
            axKSNet_Dongle1.SetComPort(m_Port, m_Baud);
            i_rtn = axKSNet_Dongle1.CheckPort();
            listBox_Log.Items.Add(String.Format("SignPad Connect [{0}]", i_rtn));

            if (0 != axKSNet_Dongle1.SignComReqA0())
            {
                listBox_Log.Items.Add("FAILED");
            }
        }

        private short SignComReqA1()
        {
            short i_rtn = 0;
            //if (0 == axKSNet_Dongle1.SignComReqA0())
            //{
            i_rtn = axKSNet_Dongle1.SignComReqA1("1,004원", "서명해 주십시오", "", "감사합니다");

            listBox_Log.Items.Add(String.Format("@@@ SignComReqA1 [{0}]", i_rtn));

            //}
            return i_rtn;
        }

        private void SignComReqA2()
        {

            //            if (0 == axKSNet_Dongle1.SignComReqA0())
            //            {
            if (0 != axKSNet_Dongle1.SignComReqA2())
            {
                listBox_Log.Items.Add("@@@ FAIL SignPad SignComReqA2");
            }

            //            }

        }

        private void SignComReqAC()
        {
            //            if (0 == axKSNet_Dongle1.SignComReqA0())
            //            {
            if (0 != axKSNet_Dongle1.SignComReqAC())
            {
                listBox_Log.Items.Add("@@@ FAIL SignPad SignComreqAC");
            }
            //            }
        }
        private void SignComReqA3()
        {
            String msg1 = "안녕하세요1";
            String msg2 = "안녕하세요2";
            String msg3 = "안녕하세요3";
            String msg4 = "안녕하세요4";

            //if (0 == axKSNet_Dongle1.SignComReqA0())
            //{
            if (0 != axKSNet_Dongle1.PinComReqA3(msg1, msg2, msg3, msg4, 0, 10, 2))
            {
                listBox_Log.Items.Add("@@@ FAIL SignPad Insert Pin Code");
            }
            //}
        }

        private void SignComReqF2()
        {
            String cardno = "1111222233334444";
            String msg1 = "비밀번호 입력";
            String msg2 = "";
            String msg3 = "감사합니다";
            String msg4 = " ";
            //if (0 == axKSNet_Dongle1.SignComReqA0())
            //{
            if (0 != axKSNet_Dongle1.PinComReqF2(msg1, msg2, msg3, msg4, cardno, 0, 16, 20))
            {
                listBox_Log.Items.Add(String.Format("@@@ FAIL PinComReqF2"));
            }
            //}

        }

        private void SignPinComReqA4()
        {
            String cardno = "1111222233334444";
            String msg1 = "비밀번호 입력";
            String msg2 = "";
            String msg3 = "감사합니다";
            String msg4 = " ";
            //if (0 == axKSNet_Dongle1.SignComReqA0())
            //{
            if (0 != axKSNet_Dongle1.PinComReqA4(msg1, msg2, msg3, msg4, cardno, 0, 16, 20))
            {
                listBox_Log.Items.Add(String.Format("@@@ FAIL PinComReqA4"));
            }
            //}

        }


        private void CloseEncReader()
        {
            m_Rtn = axKSNet_Dongle1.SetComPortEncReaderSync(m_Port, m_Baud);
            axKSNet_Dongle1.ClosePort();

            listBox_Log.Items.Add(String.Format("### ENC READER CLOSE {0}", m_Rtn));


            //listBox_Log.Items.Add(this.CombineTelegram("1", "2", "3", "4", "5"));


        }


        private bool OpenEncReader()
        {
            m_Port = Convert.ToInt16(txtBox_Port.Text);
            m_Baud = Convert.ToInt32(txtBox_Baudrate.Text);

            axKSNet_Dongle1.SetReqSignTimeout(1000);
            //axKSNet_Dongle1.EnableLogging();
            //axKSNet_Dongle1.DebugLog(1);

            try
            {
                var res = MessageBox.Show("OpenReader?", "121", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (res == DialogResult.OK)
                {
                    m_Rtn = axKSNet_Dongle1.SetComPortEncReaderSync(m_Port, m_Baud);
                    m_sRtn = axKSNet_Dongle1.EncReaderComInit(m_sYear, m_sCardData);

                    if (m_sRtn.Length > 2)
                    {
                        m_Redaer = m_sRtn.Substring(2, 16);
                        m_sRtn = axKSNet_Dongle1.EncReaderComCheck(m_Redaer, "K");
                        m_sRtn = axKSNet_Dongle1.EncReaderComCheck(m_Redaer, "S");
                    }
                    else
                    {
                        MessageBox.Show(GetRecvEncReaderErrorMsg(m_sRtn));
                    }
                }
            }
            catch (System.Exception)
            {
                Debug.WriteLine("Fail SetComPortEncReaderSync");
                return false;
            }

            return true;
        }

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

        /// <summary>
        /// 전자서명 데이터 이미지 저장
        /// </summary>
        /// <param name="_base64_signData">BASE64형태의 전자서명 데이터</param>
        /// <param name="_file_path">전자서명 데이터 이미지 파일 저장 경로 및 파일 명</param>
        /// <returns></returns>
        private short Save_SignData(String _base64_signData, String _file_path)
        {

            return axKSNet_Dongle1.SaveImage(_base64_signData, _file_path);
        }

        #endregion

        #region CLICK

        private void button1_Click(object sender, EventArgs e)
        {
            if (true == OpenEncReader())
            {
                listBox_Log.Items.Add("### ENC READER OPEN");
            }
            else
            {
                listBox_Log.Items.Add("FAIL ENC READER OPEN");
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.CloseEncReader();
        }

        private void btn_SIgnPadInit_Click(object sender, EventArgs e)
        {
            init_Signpad();

        }



        private void btn_SignComReqA1_Click(object sender, EventArgs e)
        {
            if (0 == SignComReqA1())
                listBox_Log.Items.Add("A1_Click_ok");

        }




        /// <summary>
        /// 전자서명 입력 확인
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SignComReqA2_Click(object sender, EventArgs e)
        {
            SignComReqA2();
        }




        /// <summary>
        /// 전자서명 입력 취소
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SignComReqAC_Click(object sender, EventArgs e)
        {
            SignComReqAC();
        }




        /// <summary>
        /// SignPad PIN 입력 요청
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SignComReqA3_Click(object sender, EventArgs e)
        {
            SignComReqA3();
        }



        /// <summary>
        /// Sign Pin 암호화 입력 요청
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SignComReqF2_Click(object sender, EventArgs e)
        {
            SignComReqF2();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox_Log.Items.Clear();
        }


        private void button4_Click(object sender, EventArgs e)
        {
            SignPinComReqA4();
        }

        #endregion

        #region EVENT


        /// <summary>
        /// 카드 삽입 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axaxKSNet_Dongle1_OnRecvEncReaderInsert(object sender, EventArgs e)
        {

            listBox_Log.Items.Add("Call axaxKSNet_Dongle1_OnRecvEncReaderInsert");

            string l_sDate = DateTime.Now.ToString("yyMMddhhmmss");

            // 카드번호 요청
            m_Rtn = axKSNet_Dongle1.SetComPortEncReaderSync(m_Port, m_Baud);

            m_sRtn = axKSNet_Dongle1.EncReaderComCard("IC", l_sDate, "000001004", "10");

            listBox_Log.Items.Add(String.Format("Call EncReaderComCard rtn[{0}]", m_sRtn));
            listBox_Log.Items.Add("END axaxKSNet_Dongle1_OnRecvEncReaderInsert");
        }

        /// <summary>
        /// 카드 제거 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axKSNet_Dongle1_OnRecvEncReaderRemove(object sender, EventArgs e)
        {
            listBox_Log.Items.Add("Call axKSNet_Dongle1_OnRecvEncReaderRemove");
        }


        /// <summary>
        /// SignPad 수신데이터 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axKSNet_Dongle1_OnRecvSignData(object sender, AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvSignDataEvent e)
        {
            String str_SignData = e.data;
            txtBox_Signdata.Text = e.data;

            String str_file_path = String.Format(@"D:\업무\sample.jpg");
            m_signData = e.data;
            if (0 != this.Save_SignData(e.data, str_file_path))
            {
                listBox_Log.Items.Add("@@@ FAIL SignPad BMP Save Data");
            }
            else
            {
                listBox_Log.Items.Add(String.Format("@@@ SignPad BMP Save Data [{0}]", str_file_path));
            }


            listBox_Log.Items.Add(String.Format("@@@ On RecvSignData [{0}]", str_SignData));

            OpenEncReader();
        }


        /// <summary>
        /// EVENT: SignPad Pin Code Recv Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axKSNet_Dongle1_OnRecvPinData(object sender, AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvPinDataEvent e)
        {
            txtBox_Pindata.Text = e.data;
            m_pinData = e.data.ToString();
            listBox_Log.Items.Add(String.Format("@@@ Recv PIN CODE [{0}]", e.data));
            OpenEncReader();
        }


        /// <summary>
        /// EVENT: Read card infomation
        /// 
        /// Read MSR DATA or IC DATA EVENT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axKSNet_Dongle1_OnRecvEncReaderCard(object sender, AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvEncReaderCardEvent e)
        {
            listBox_Log.Items.Add(@"카드정보 읽기");
            listBox_Log.Items.Add(String.Format("RES CODE [{0}] ", e.resCode));
            listBox_Log.Items.Add(String.Format("CardType [{0}]", e.cardType));
            listBox_Log.Items.Add(String.Format("Count [{0}]", e.count));
            listBox_Log.Items.Add(String.Format("Reader[{0}]", e.reader));
            listBox_Log.Items.Add(String.Format("encData [{0}]", e.encData));
            listBox_Log.Items.Add(String.Format("encCardNo[{0}][{1}]", e.encCardNo.Length, e.encCardNo));
            listBox_Log.Items.Add(String.Format("noEncCardNo[{0}][{1}]", e.noEncCardNo.Length, e.noEncCardNo));

            txtBox_EncCardNo.Text = e.encCardNo.ToString();
            txtBox_NoEncCardNo.Text = e.noEncCardNo.ToString();
            txtBox_EnvData.Text = e.encData.ToString();

            string str_noCardNo_2 = e.noEncCardNo.ToString();
            string svcCode = string.Empty;

            int idx_Check = str_noCardNo_2.IndexOf('=');

            if (idx_Check > -1)
            {
                str_noCardNo_2 = e.noEncCardNo.Substring(1, idx_Check - 1);
                svcCode = e.noEncCardNo.Substring(idx_Check + 1, 1);
            }
            else
            {
                str_noCardNo_2 = e.noEncCardNo.Substring(1, e.noEncCardNo.Length - 1);
            }

            listBox_Log.Items.Add(String.Format("serviceCode [{0}]", svcCode));

            string request = CombineTelegram(e.cardType.ToString(),
                                             e.reader.ToString(),
                                             str_noCardNo_2.ToString(),
                                             m_pinData.ToString(),
                                             m_signData.ToString()
                                             );

            byte[] bt_request = StringToByte(request);
            listBox_Log.Items.Add(request);


            //OpenEncReader();
        }


        #endregion

        #region Create telegram

        private string CombineTelegram(string carType, string reader, string noEncCardNo, string encPwd, string signData)
        {
            string str_telegram = "";
            string str_telegramlen = "";
            byte[] bt_tellegram = { };


            str_telegram += (char)0x02;

            bt_tellegram = StringToByte(str_telegram);

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

            str_telegram += m_pinData;
            for (n = 0; n < 37 - m_pinData.Length - 1; n++)
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

            if (m_signData.Length > 0)
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

        /// <summary>
        /// SignComReq54
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            axKSNet_Dongle1.SignComReqA0();
            axKSNet_Dongle1.SignComReq54("20", "KRW", "1400".PadRight(10, ' '), "KRW", "1400".PadRight(10, ' '),
                "1.4".PadRight(32, ' '), "".PadRight(30, ' '));
        }

    }
}
