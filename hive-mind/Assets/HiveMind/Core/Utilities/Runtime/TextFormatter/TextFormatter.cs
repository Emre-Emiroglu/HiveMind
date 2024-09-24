using HiveMind.Core.Utilities.Runtime.Enums;
using System;

namespace HiveMind.Core.Utilities.Runtime.TextFormatter
{
    public static class TextFormatter
    {
        #region Constants
        private const string Days = "{0:00}:";
        private const string Hours = "{1:00}:";
        private const string Minutes = "{2:00}:";
        private const string Seconds = "{3:00}:";
        private const string Milliseconds = "{4:00}";
        #endregion

        #region Formats
        public static string FormatNumber(int num)
        {
            if (num >= 100000000)
            {
                return (num / 1000000D).ToString("0.#M");
            }
            if (num >= 1000000)
            {
                return (num / 1000000D).ToString("0.##M");
            }
            if (num >= 100000)
            {
                return (num / 1000D).ToString("0.#k");
            }
            if (num >= 10000)
            {
                return (num / 1000D).ToString("0.##k");
            }

            return num.ToString("#,0");
        }
        public static string FormatTime(double totalSecond, TimeFormattingTypes timeFormattingType = TimeFormattingTypes.DaysHoursMinutesSeconds, bool withMilliSeconds = true)
        {
            TimeSpan time = TimeSpan.FromSeconds(totalSecond);
            int days = time.Days;
            int hours = time.Hours;
            int minutes = time.Minutes;
            int seconds = time.Seconds;
            int milliSeconds = (int)(totalSecond * 100);
            milliSeconds %= 100;

            bool withDays = timeFormattingType == TimeFormattingTypes.DaysHoursMinutesSeconds;
            bool withHours = timeFormattingType == TimeFormattingTypes.DaysHoursMinutesSeconds || timeFormattingType == TimeFormattingTypes.HoursMinutesSeconds;
            bool withMinutes = timeFormattingType == TimeFormattingTypes.DaysHoursMinutesSeconds || timeFormattingType == TimeFormattingTypes.HoursMinutesSeconds || timeFormattingType == TimeFormattingTypes.MinutesSeconds;

            string d = withDays ? Days : null;
            string h = withHours ? Hours : null;
            string m = withMinutes ? Minutes : null;
            string s = Seconds;
            string ms = withMilliSeconds ? Milliseconds : null;

            string result = string.Format(d + h + m + s + ms, days, hours, minutes, seconds, milliSeconds);
            result = result.TrimEnd(':');
            return result;
        }
        #endregion
    }
}
