using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SignPadControl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            axKSNet_Dongle1.EnableLogging();  

            // events
            axKSNet_Dongle1.OnRecvPinData += new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvPinDataEventHandler(axKSNet_Dongle1_OnRecvPinData);
            axKSNet_Dongle1.OnRecvSignData += new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvSignDataEventHandler(axKSNet_Dongle1_OnRecvSignData);
            axKSNet_Dongle1.OnReadMSR += new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnReadMSREventHandler(axKSNet_Dongle1_OnReadMSR);
            axKSNet_Dongle1.OnTerminalReadMSR += new AxKSNET_DONGLELib._DKSNet_DongleEvents_OnTerminalReadMSREventHandler(axKSNet_Dongle1_OnTerminalReadMSR);

        }

        void axKSNet_Dongle1_OnTerminalReadMSR(object sender, AxKSNET_DONGLELib._DKSNet_DongleEvents_OnTerminalReadMSREvent e)
        {
            txtRFCardNo.Text = e.data;
        }

        void axKSNet_Dongle1_OnReadMSR(object sender, AxKSNET_DONGLELib._DKSNet_DongleEvents_OnReadMSREvent e)
        {
            iccardata.Text = e.cardnum;
        }

        void axKSNet_Dongle1_OnRecvSignData(object sender, AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvSignDataEvent e)
        {
            // 전자서명 데이터를 텍스트 박스에 출력 (Base64)
            signdata.Text = e.data;

            // 저장할 파일의 경로
            string fPath = ".\\save_signimage.bmp";
            string base64data = e.data;
            // 데이터를 BMP 파일로 저장
            int rtn = axKSNet_Dongle1.SaveImage(base64data, fPath);

            if (rtn != 0)
            {
                MessageBox.Show("이미지 저장 실패!");
            }
            else
            {
                // 이미지를 픽쳐박스에 출력
                pictureBox.Load(fPath);
            }
        }

        void axKSNet_Dongle1_OnRecvPinData(object sender, AxKSNET_DONGLELib._DKSNet_DongleEvents_OnRecvPinDataEvent e)
        {
            pindata.Text = e.data;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            short port = 0;   //포트
            int baud = 0;

            //포트와 통신속도를 받아 온다.
            port = Convert.ToInt16(portnum.Text);
            baud = Convert.ToInt32(baudrate.Text);

            if (OpenDongle(port, baud))
            {
                MessageBox.Show("포트연결에 성공하였습니다");
            }
            else
            {
                MessageBox.Show("포트연결에 실패하였습니다");
            }
        }

        bool OpenDongle(short port, int baudRate)
        {
            int rtn = 0;

            //포트 연결
            axKSNet_Dongle1.SetComPort(port, baudRate);
            rtn = axKSNet_Dongle1.CheckPort();

            return rtn > 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {            
            axKSNet_Dongle1.ClosePort();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 초기화
            int rtn = axKSNet_Dongle1.SignComReqA0();

            if (rtn != 0)
            {
                MessageBox.Show("사인패드 초기화 실패!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string msg1 = "금액:10000원";
            string msg2 = "사인해주세요~";
            string msg3 = " ";
            string msg4 = " ";

            // 서명이미지를 화면에 표시할때 확대한다
            axKSNet_Dongle1.SetImageSize(2);

            // 최소 픽셀수를 지정한다.(10pixel 이상 서명이 되어야 서명한것으로 생각하고 처리함)
            axKSNet_Dongle1.SetMinSignPixel(10);

            // 서명완료 후 3초 후에 자동으러 서명이 전송되게 끔 처리
            axKSNet_Dongle1.SetReqSignTimeout(3);

            //axKSNet_Dongle1.SignComReqAC();

            // 전자서명 입력 요청
            int rtn = axKSNet_Dongle1.SignComReqA1(msg1, msg2, msg3, msg4);

            if (rtn != 0)
            {
                MessageBox.Show("전자서명 요청 실패!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //전자서명 데이터 전송 요청
            int rtn = axKSNet_Dongle1.SignComReqA2();

            if (rtn != 0)
            {
                MessageBox.Show("전자서명 데이터 전송 요청 실패!");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //전자서명 입력 취소
            int rtn = axKSNet_Dongle1.SignComReqAC();

            if (rtn != 0)
            {
                MessageBox.Show("전자서명 취소 실패!");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string msg1 = "비밀번호 입력";
            string msg2 = " ";
            string msg3 = "감사합니다";
            string msg4 = " ";

            // PIN 입력 요청 (입력된 번호를 '*'로 표시, 최대 글자수: 16,  메시지3,4 출력 시간 : 1초)
            int rtn = axKSNet_Dongle1.PinComReqA3(msg1, msg2, msg3, msg4, 0, 16, 1);

            if (rtn != 0)
            {
                MessageBox.Show("PIN 입력 요청 실패!");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string Cardnum = "1111222233334444";
            string msg1 = "비밀번호 입력";
            string msg2 = " ";
            string msg3 = "감사합니다";
            string msg4 = " ";

            // PIN 입력 요청 (입력된 번호를 '*'로 표시, 최대 글자수: 16,  메시지3,4 출력 시간 : 1초)
            int rtn = axKSNet_Dongle1.PinComReqA4(msg1, msg2, msg3, msg4, Cardnum, 0, 16, 1);

            if (rtn != 0)
            {
                MessageBox.Show("PIN 입력(암호화) 요청 실패!");
            }
        }

        /// <summary>
        /// RF Card reading 요청
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            txtRFCardNo.Text = "";

            string msg1 = "11111";
            string msg2 = "111";
            string msg3 = " ";
            string msg4 = " ";

            //' 전자서명 입력 요청
            int result = 0;

            try
            {
                result = axKSNet_Dongle1.SignComReqA8(2, msg1, msg2, msg3, msg4, 10);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (result != 0)
            {
                MessageBox.Show("RF 카드리딩 요청 실패!");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            txtRFCardNo.Text = "";

            string msg1 = "11111";
            string msg2 = "111";
            string msg3 = " ";
            string msg4 = " ";

            //' 전자서명 입력 요청
            int result = 0;

            try
            {
                result = axKSNet_Dongle1.SignComReqA9(msg1, msg2, msg3, msg4, 10);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (result != 0)
            {
                MessageBox.Show("RF 카드리딩 요청 실패!");
            }
        }

        private void axCtrlKeyboard1_KeyboardEvent(object sender, AxKeyBoardHook.__CtrlKeyboard_KeyboardEventEvent e)
        {
            textBox2.Text += e.strData;
        }

        private void axCtrlKeyboard1_MsrEvent(object sender, AxKeyBoardHook.__CtrlKeyboard_MsrEventEvent e)
        {
            textBox1.Text += e.strTrack2;
        }

        private void axCtrlKeyboard1_ScannerEvent(object sender, AxKeyBoardHook.__CtrlKeyboard_ScannerEventEvent e)
        {
            textBox1.Text += e.strData;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.ShowDialog(this);
        }

        private void btnScanPort_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                var res = OpenDongle(Convert.ToInt16(i + 1), 38400);
                if (res)
                {
                    MessageBox.Show(string.Format("Port: {0}", i + 1));
                    return;
                }
            }

            MessageBox.Show("Not Found");
        }
                
    }
}