using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

namespace XCopyApp
{
    public partial class UpdateForm : Form
    {
        private bool m_activated = false;
        private string m_updateFolder = string.Empty;

        public UpdateForm(string[] args)
        {
            InitializeComponent();
            m_updateFolder = args.Length > 0 ? args[0] : string.Empty;
        }

        void MainForm_Activated(object sender, System.EventArgs e)
        {
            if (m_activated)
            {
                return;
            }

            m_activated = true;
            Application.DoEvents();

            if (string.IsNullOrEmpty(m_updateFolder))
            {
                RunAppStarter();
                this.Close();
            }
            else
            {
                // this app is in bin
                // m_updateFolder is his folder\..\update\download\yyyyMMdd001 encoded
                // 
                // copy from this folder\..\update\download\yyyyMMdd001 => this\..
                string toFolder = Directory.GetParent(Path.GetDirectoryName(Application.ExecutablePath)).FullName;
                string fromFolder = Encoding.UTF8.GetString(Convert.FromBase64String(m_updateFolder));

                if (Directory.Exists(fromFolder) && Directory.Exists(toFolder))
                {
                    CopyDirectory(fromFolder, toFolder);
                }

                RunAppStarter();
                this.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void RunAppStarter()
        {
            string exePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"..\update\AppStarter.exe");

            if (File.Exists(exePath))
            {
                Process.Start(exePath);
            }
        }

        /// <summary>
        /// Copy directory recursively
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        private void CopyDirectory(string sourcePath, string destPath)
        {
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            foreach (string file in Directory.GetFiles(sourcePath))
            {
                string dest = Path.Combine(destPath, Path.GetFileName(file));
                try
                {
                    File.Copy(file, dest, true);
                    LogMessage(string.Format("File {0} copied", file));
                }
                catch
                {
                    LogMessage(string.Format("File {0} failed", file));
                }
            }

            foreach (string folder in Directory.GetDirectories(sourcePath))
            {
                string dest = Path.Combine(destPath, Path.GetFileName(folder));
                CopyDirectory(folder, dest);
            }
        }

        #region Utilities
        /// <summary>
        /// Log exception to file
        /// </summary>
        /// <param name="ex"></param>
        static public void LogMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendFormat("[Error] {0}", ex.Message);
            sb.AppendLine();
            sb.AppendLine("[StackTrace]");
            sb.Append(ex.StackTrace);
            LogMessage(sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        static public void LogMessage(string msg)
        {
            StreamWriter sw = null;
            try
            {
                string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\update\log");
                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }
                string logFile = Path.Combine(logFolder, string.Format("{0:yyyyMMdd}.log", DateTime.Today));
                if (!File.Exists(logFile))
                {
                    File.Create(logFile).Close();
                }

                sw = new StreamWriter(logFile, true, Encoding.UTF8);
                sw.WriteLine(string.Format("[{0:yyyy-MM-dd HH:mm:ss}] {1}", DateTime.Now, msg));
            }
            catch
            {
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
        }
        #endregion
    }
}
