using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.NetComm.Tasks.PD;
using WSWD.WmallPos.FX.NetComm.Tasks;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Config;
using WSWD.WmallPos.FX.NetComm.Client;
using WSWD.WmallPos.FX.Shared.NetComm.Request;
using WSWD.WmallPos.FX.Shared.NetComm;
using System.Net.Sockets;
using System.Diagnostics;
using System.Net;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using System.IO;

namespace BasketTestApp
{
    public partial class StartupForm : Form, IPackageValidator
    {
        public StartupForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(StartupForm_Load);
        }

        void StartupForm_Load(object sender, EventArgs e)
        {
            //this.Hide();
            //new Form1().ShowDialog(this);
            //this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var msg = GetEncString(textBox1.Text, 20);
            MessageBox.Show(msg + "-" + msg.Length.ToString() + "-bytes:" + Encoding.GetEncoding(949).GetByteCount(msg).ToString());
        }

        private string GetEncString(string textValue, int length)
        {
            Encoding transferEnc = Encoding.GetEncoding(949);
            byte[] allData = Encoding.Default.GetBytes(textValue);

            var textData = transferEnc.GetString(allData);
            var encData = transferEnc.GetBytes(textData);

            byte[] data = new byte[length];
            for (int i = 0; i < length; i++)
            {
                data[i] = (byte)' ';
            }

            System.Buffer.BlockCopy(encData, 0, data, 0, encData.Length);
            return transferEnc.GetString(data);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Create directories
            ConfigData config = new ConfigData()
            {
                AppConfig = AppConfig.Load(),
                DevConfig = DevConfig.Load(),
                KeyMapConfig = KeyMapConfig.Load(),
                SysMessage = SysMessage.Load()
            };

            ConfigData.Initialize(config);

            PD17();
        }

        void PD17()
        {
            var pd17Task = new PD17DataTask();
            pd17Task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pd17Task_Errored);
            pd17Task.DownloadProgressChanged += new WSWD.WmallPos.FX.NetComm.Tasks.DownloadProgressChangedEventHandler(pd17Task_DownloadProgressChanged);
            pd17Task.ExecuteTask();
        }

        void pd17Task_DownloadProgressChanged(DownloadProgressState processState, int done, int total)
        {
            if (processState == DownloadProgressState.Started)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    label1.Text = "할인쿠폰 다운로드 진행합니다.";
                });
            }
            else if (processState == DownloadProgressState.Processing)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    label1.Text = string.Format("할인쿠폰 권종 다운로드 {0}/{1}건 진행중", done, total);
                });
            }
            else
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    MessageBox.Show("할인쿠폰 권종 다운로드 완료.");
                });
            }
        }

        void pd17Task_Errored(string errorMessage, Exception lastException)
        {
            this.BeginInvoke((MethodInvoker)delegate()
                {
                    label1.Text = "할인쿠폰 다운로드 오류.";
                });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Text = MakeRandomNumber();
            MessageBox.Show(label1.Text.Length.ToString());
        }

        private string MakeRandomNumber()
        {
            StringBuilder sb = new StringBuilder();
            var rand = new Random();
            for (int i = 0; i < 32; i++)
            {
                int rn = rand.Next(15);
                sb.Append(rn.ToString("X"));
            }

            return sb.ToString();
        }

        private Encoding transferEnc = Encoding.GetEncoding(NetCommConstants.TRANFER_ENCODING);

        private void button4_Click(object sender1, EventArgs ea)
        {
            //"00044PD140019701  1104  00001000010000501  P"   
            using (var sock = new SyncSockClient("124.137.10.25", 7901, 5000, this))
            {
                sock.OnPackageReceived += new OnReceivedHandler(sock_OnPackageReceived);
                sock.OnEndReceived += new EventHandler<EndReceivedEventArgs>(sock_OnEndReceived);
                sock.Send("00044PD140019701  1104  00001000010000501  P");
            }
        }

        void sock_OnEndReceived(object sender, EndReceivedEventArgs e)
        {
            MessageBox.Show(m_responses.Count.ToString());
        }

        List<ResponseBase> m_responses = new List<ResponseBase>();
        void sock_OnPackageReceived(ResponseBase response)
        {
            m_responses.Add(response);
        }

        /// <summary>
        /// Check start of package
        /// </summary>
        /// <param name="recData"></param>
        /// <param name="totalByteCount"></param>
        /// <returns></returns>
        public bool ReadPackageStart(string receiveData, out int totalByteCount)
        {
            totalByteCount = 0;
            if (string.IsNullOrEmpty(receiveData) || receiveData.Length < 5)
            {
                return false;
            }

            string lenText = receiveData.Substring(0, 5);
            try
            {
                totalByteCount = Convert.ToInt32(lenText);
                return true;
            }
            catch
            {

            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiveData"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public bool HasNextPackage(string receiveData, out ResponseBase response)
        {
            response = null;

            if (string.IsNullOrEmpty(receiveData))
            {
                return false;
            }

            var m_parseByte = false;
            try
            {
                response = (ResponseBase)ResponseBase.Parse(typeof(ResponseBase), receiveData, m_parseByte);
                return int.Parse(response.ReqHeader.PackSeq) < int.Parse(response.ReqHeader.PackCount);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message, FXConsts.SOCKET);
                return false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<BasketTksPresentRtn> list = new List<BasketTksPresentRtn>();
            for (int i = 0; i < 5; i++)
            {
                list.Add(new BasketTksPresentRtn()
                {
                    PresentDate = "20151112",
                    PresentNo = "12312121",
                    PresentNm = "텟트트",
                    PresentAmt = "12000",
                });
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.Append(item.ToString());
                sb.Append(Environment.NewLine);
            }

            Console.WriteLine(sb.ToString());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string classFolder = @"..\..\..\..\..\Shared\WSWD.WmallPos.Shared\NetComm";// @"F:\Apps\Wmall\01.Source\Shared\WSWD.WmallPos.Shared\NetComm";
            
            // get cs files
            var files = Directory.GetFiles(classFolder, "*.cs", SearchOption.AllDirectories);

            foreach (var filePath in files)
            {
                string fileCont = File.ReadAllText(filePath, Encoding.UTF8);

                // search for [//TypeGubun(TypeProperties
                int idx = fileCont.IndexOf("[TypeGubun(TypeProperties.", 0);

                if (idx == -1)
                {
                    continue;
                }

                int seqNo = 1;
                while (idx >= 0)
                {
                    fileCont = fileCont.Substring(0, idx + "[TypeGubun(".Length) + seqNo.ToString() + ", " +
                        fileCont.Substring(idx + "[TypeGubun(".Length);
                    idx = fileCont.IndexOf("[TypeGubun(TypeProperties.", idx + 1);
                    seqNo++;
                }


                File.WriteAllText(filePath, fileCont);
                label2.Text = Path.GetFileName(filePath);
            }
        }

    }

}
