using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace WSWD.WmallPos.POS.FX.Shared.Utils
{
    public class NetworkUtils
    {
        public static string GetComputerIPAddress()
        {
            string sIPAddr = string.Empty;
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                foreach (var x in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (x.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        string iPAddr = x.Address.ToString();

                        if (iPAddr != "127.0.0.1")
                        {
                            sIPAddr = iPAddr;
                            break;
                        }

                    }
                }

                if (!string.IsNullOrEmpty(sIPAddr))
                {
                    break;
                }

            }

            return sIPAddr;
        }


        /// <summary>
        /// Send a message through socket return ACK
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <param name="message"></param>
        public static string SendSocketStream(string ipAddress, int port, string message)
        {
            TcpClient oTcpClient = null;
            string sAck = null;
            NetworkStream oStream = null;

            try
            {
                oTcpClient = new TcpClient();
                Byte[] baRead = new Byte[100];

                oTcpClient.Connect(ipAddress, port);

                // Get the stream, convert to bytes
                oStream = oTcpClient.GetStream();

                // send and receive the raw bytes without a stream writer
                // or reader
                Byte[] baSend = Encoding.ASCII.GetBytes(message);
                // now send it
                oStream.Write(baSend, 0, baSend.Length);

            }
            catch// (Exception ex)
            {
                //throw new BaseException(ex.Message, ex, false);
            }
            finally
            {
                if (oStream != null)
                    oStream.Close();
                if (oTcpClient != null)
                    oTcpClient.Close();
            }
            return sAck;

        }

    }
}
