using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.POS.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Win;
using System.IO;
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.FX.Win.Utils
{
    /// <summary>
    /// Wmall용 LogUtilities,
    /// 
    /// </summary>
    public class WLogUtils : LogHelper
    {
        public static void InitInstance()
        {
            LogUtils.Instance = new LogUtils();
            WLogUtils.Instance = new WLogUtils();
        }

        /// <summary>
        /// LogUtilites
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="logMessage"></param>
        /// <param name="args"></param>
        public override void LogByType(string logType, string logMessage, params object[] args)
        {
            if (FXConsts.JOURNAL.Equals(logType))
            {
                WriteJournal(logMessage, args);
            }
            else if (FXConsts.TRACE.Equals(logType))
            {
                WriteTrace(logMessage);
            }
            else
            {
                base.LogByType(logType, logMessage, args);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string LogBaseDirectory()
        {
            return FXConsts.FOLDER_LOG.GetFolder();
        }

        #region Journal Writing

        /// <summary>
        /// 저널파일 저장
        /// FileName:   영업일자-점코드-포스번호.JRN   (예: 20150110-01-0121.JRN)
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="args"></param>
        private void WriteJournal(string logMessage, params object[] args)
        {
            StreamWriter sw = null;
            try
            {
                string logFile = Path.Combine(FXConsts.JOURNAL.GetFolder(), string.Format("{0:yyyyMMdd}-{1}-{2}.jrn",
                    DateTime.Today, ConfigData.Current.AppConfig.PosInfo.StoreNo, ConfigData.Current.AppConfig.PosInfo.PosNo));
                string logFolder = Path.GetDirectoryName(logFile);

                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }

                if (!File.Exists(logFile))
                {
                    File.Create(logFile).Close();
                }

                sw = new StreamWriter(logFile, true);
                sw.WriteLine(logMessage);
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

        #region Write Trace

        /// <summary>
        /// Wmall tracing method
        /// </summary>
        /// <param name="traceMessage"></param>
        private void WriteTrace(string traceMessage)
        {
            StreamWriter sw = null;
            try
            {
                string traceFolder = Path.Combine(LogBaseDirectory(), FXConsts.TRACE);
                string logFile = Path.Combine(traceFolder, string.Format("{1}_{0:yyyyMMdd}.log", DateTime.Today, FXConsts.TRACE));
                string logFolder = Path.GetDirectoryName(logFile);

                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }

                if (!File.Exists(logFile))
                {
                    File.Create(logFile).Close();
                }

                sw = new StreamWriter(logFile, true);
                sw.WriteLine(traceMessage);
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
