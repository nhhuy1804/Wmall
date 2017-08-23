using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.ComponentModel;
using System.Linq;
using System.Data;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using POS.Devices;

namespace WSWD.WmallPos.POS.SetupInstaller
{
    [RunInstaller(true)]
    public partial class MainInstaller : System.Configuration.Install.Installer
    {
        public MainInstaller()
        {
            InitializeComponent();
        }

        protected override void OnCommitted(IDictionary savedState)
        {
            base.OnCommitted(savedState);
            RunConfigApp(true);
        }

        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            RunConfigApp(false);
            RemoveFiles();
            base.OnBeforeUninstall(savedState);            
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);

            if (!CheckOPOSPrinter())
            {
                string cf = this.Context.Parameters["targetdir"];

                // run OPOS setup..
                string msiFile = Path.Combine(cf, @"msi\OPOS_CCOs_1.14.001.msi");
                Process.Start(msiFile);
            }

        }

        void RunConfigApp(bool run)
        {
            if (run)
            {
                string cf = this.Context.Parameters["targetdir"];

                //2015.09.10 정광호 수정------------------------------------------------------------------
                //기존WmallPOS.exe에서 AppStarter.exe로 변경
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    true).SetValue("W-MALLPOS", Path.Combine(cf, @"update\AppStarter.exe"));
                //Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                //    true).SetValue("W-MALLPOS", Path.Combine(cf, @"bin\WmallPOS.exe"));
                //----------------------------------------------------------------------------------------
                
                string configFileExe = Path.Combine(cf, @"bin\POSConfig.exe");
                Process.Start(configFileExe);
            }
            else
            {
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    true).DeleteValue("W-MALLPOS");
            }
        }

        void RemoveFiles()
        {
            string cf = this.Context.Parameters["targetdir"];
            DirectoryInfo di = new DirectoryInfo(cf);
            di.Empty();
        }

        bool CheckOPOSPrinter()
        {
            try
            {
                new OPOSPOSPrinter();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }

    static class DirectoryExtensions
    {
        public static void Empty(this System.IO.DirectoryInfo directory)
        {
            foreach (System.IO.FileInfo file in directory.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch
                {

                }
            }
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories())
            {
                try
                {
                    subDirectory.Delete(true);
                }
                catch
                {

                }
            }
        }
    }
}
