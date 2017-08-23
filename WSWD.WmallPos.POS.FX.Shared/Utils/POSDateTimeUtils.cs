using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WSWD.WmallPos.POS.FX.Shared.Utils
{
    public class POSDateTimeUtils
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME theDateTime); 

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
            public string wNow;
        }

        /// <summary>
        /// 시스템시간 설정
        /// </summary>
        /// <param name="systemTime"></param>
        public static void SetTime(string time)
        {
            SYSTEMTIME sTime = new SYSTEMTIME();

            DateTime dt = DateTime.ParseExact(time, "yyyyMMddHHmmss", new System.Globalization.CultureInfo("ko-KR", true));

            int day_change = 0;  // 일자가 변경되는걸 알기 위해서

            // UTC 타임이라 + 9시간 차이가 난다
            if (Convert.ToInt16(dt.Hour) >= 9)
            {
                sTime.wHour = (ushort)(Convert.ToInt16(dt.Hour) - 9 % 24);
            }
            else
            {
                day_change = 1;
                sTime.wHour = (ushort)(Convert.ToInt16(dt.Hour) + 15 % 24);
            }

            sTime.wYear = (ushort)dt.Year;
            sTime.wMonth = (ushort)dt.Month;
            // 시간이 24시가 넘어가면 day 하루 늘어나게 된다. 그래서  day_change 변수를 사용해서 하루를 빼줌.
            sTime.wDay = (ushort)(Convert.ToInt16(dt.Day) - day_change);

            // Set the system clock ahead one hour.
            sTime.wHour = (ushort)(dt.Hour - 9);
            sTime.wMinute = (ushort)(dt.Minute);
            sTime.wSecond = (ushort)(dt.Second);
            sTime.wNow = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString();
            SetSystemTime(ref sTime);
        }
    }
}
