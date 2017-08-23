using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace WSWD.WmallPos.POS.FX.Shared.Utils
{
    public class FileUtils
    {
        public static Image ReadImage(string filePath)
        {
            string fPath = filePath;
            if (!File.Exists(fPath))
            {
                fPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fPath);
            }

            return File.Exists(fPath) ? Image.FromFile(fPath) : null;
        }

        public static Image RandomImage(string imgFolder)
        {
            string fPath = imgFolder;
            if (!File.Exists(fPath))
            {
                fPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fPath);
            }

            var files = Directory.GetFiles(fPath, "*.jpg,*.png,*.gif");
            if (files.Length > 0)
            {
                Random rnd = new Random();
                int idx = rnd.Next(files.Length - 1);
                return Image.FromFile(files[idx]);
            }
            else
            {
                return null;
            }
        }

        public static void RegSvr32(string filePath)
        {
            string regSvr = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\regsvr32.exe";
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    FileName = regSvr,
                    Arguments = " /s " + filePath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
            
            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                Trace.WriteLine(line);
            }
        }
    }
}
