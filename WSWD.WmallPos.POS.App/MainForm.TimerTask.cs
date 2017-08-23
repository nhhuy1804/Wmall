using System;
using System.Collections.Generic;

using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.Tasks;
using System.Threading;
using WSWD.WmallPos.FX.Shared;

namespace WSWD.WmallPos.POS.App
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainForm
    {
        /// <summary>
        /// Load timer tasks from config file
        /// </summary>
        private void LoadTimerTaskConfig()
        {
            LoadTimerTask("UploadTask", "WSWD.WmallPos.POS.FX.Tasks.dll,WSWD.WmallPos.FX.Tasks.Trans.TransUploadTask");
            LoadTimerTask("StatusTask", "WSWD.WmallPos.POS.FX.Tasks.dll,WSWD.WmallPos.FX.Tasks.Trans.TransStatusTask");
            LoadTimerTask("NoticeTask", "WSWD.WmallPos.POS.FX.Tasks.dll,WSWD.WmallPos.FX.Tasks.Notice.NoticeStatusTask");
            LoadTimerTask("SignTask", "WSWD.WmallPos.POS.FX.Tasks.dll,WSWD.WmallPos.FX.Tasks.Trans.SignUploadTask");
        }

        /// <summary>
        /// Load and add to timer task list
        /// </summary>
        /// <param name="taskKey"></param>
        private void LoadTimerTask(string taskKey, string defaultValue)
        {
            string taskInfo = GetPosOptionValue(taskKey, defaultValue);
            string[] infos = taskInfo.Split(new char[] { ',' });
            if (infos.Length != 2)
            {
                return;
            }

            string dllName = infos[0];
            string className = infos[1];

            string config = className.Substring(className.LastIndexOf(".") + 1);
            var sInterval = GetPosOptionValue(config, "5");
            int interval = TypeHelper.ToInt32(sInterval);

            interval = interval == 0 ? 5 : interval;

            ITimerTaskExcecutor task = null;

            try
            {
                var obj = ClassHelper.SafeClassLoad(dllName, className);
                task = (ITimerTaskExcecutor)obj;
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }

            if (task != null)
            {
                m_timerTasks.Add(Guid.NewGuid().ToString("n"), new TimerTask()
                {
                    Interval = interval,
                    ResetCount = 0,
                    UserState = null,
                    TaskHandler = task.ExecuteTask
                });
            }
        }

        /// <summary>
        /// Get config data from PosOption
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetPosOptionValue(string key, string defValue)
        {
            var t = ConfigData.Current.AppConfig.PosOption.GetType();
            var pi = t.GetProperty(key);
            if (pi != null)
            {
                object val = pi.GetValue(ConfigData.Current.AppConfig.PosOption, null);
                return val == null ? defValue : val.ToString();
            }

            return string.Empty;
        }

        private Dictionary<string, TimerTask> m_timerTasks = new Dictionary<string, TimerTask>();

        private void ProcessTimerTask()
        {
            foreach (KeyValuePair<string, TimerTask> ttask in m_timerTasks)
            {
                // task is running
                if (ttask.Value.ResetCount < 0)
                {
                    continue;
                }

                if (ttask.Value.ResetCount == 0)
                {
                    ttask.Value.ResetCount = -1;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ExecuteTimerTask), ttask.Value);
                }
                else
                {
                    ttask.Value.ResetCount--;
                }
            }
        }

        private void ExecuteTimerTask(object stateInfo)
        {
            TimerTask task = (TimerTask)stateInfo;
            task.TaskHandler(task, task.UserState);
        }
    }
}