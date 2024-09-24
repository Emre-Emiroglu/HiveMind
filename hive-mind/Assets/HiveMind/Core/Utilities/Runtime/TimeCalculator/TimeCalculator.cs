using HiveMind.Core.Utilities.Runtime.Enums;
using System;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace HiveMind.Core.Utilities.Runtime.TimeCalculator
{
    public static class TimeCalculator
    {
        #region Constants
        private const int DayAsSecond = 86400;
        private const int HourAsSecond = 3600;
        private const int HourAsDay = 24;
        private const int SecondAsDay = 60;
        private const string TimeApiUrl = "https://www.google.com";
        #endregion

        #region Fields
        private static float _time;
        private static DateTime _cachedDateTime;
        #endregion

        #region Core
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static async void Initialize()
        {
            _cachedDateTime = default;
            await RefreshDateTimeAsync();
        }
        #endregion

        #region Local
        public static double SubtractCurrent(string time, DateTypes dateType = DateTypes.Now)
        {
            DateTime localDate = dateType == DateTypes.Now ? DateTime.Now : DateTime.UtcNow;

            DateTime.TryParse(time, out var timeDate);

            double timeValue = TimeSpan.FromTicks(timeDate.Ticks).TotalSeconds;
            double currentVal = TimeSpan.FromTicks(localDate.Ticks).TotalSeconds;

            return (currentVal - timeValue);
        }
        public static DateTime ConvertToDateTime(string timeStr)
        {
            DateTime.TryParse(timeStr, out var newDate);
            return newDate;
        }
        public static double Subtract(string oldTime, string newTime)
        {
            DateTime.TryParse(newTime, out var newDate);
            DateTime.TryParse(oldTime, out var oldDate);

            double old = TimeSpan.FromTicks(oldDate.Ticks).TotalSeconds;
            double now = TimeSpan.FromTicks(newDate.Ticks).TotalSeconds;

            return (now - old);
        }
        public static double Addition(string time, string addedTime)
        {
            DateTime.TryParse(time, out var newDate);
            DateTime.TryParse(addedTime, out var oldDate);

            double click = TimeSpan.FromTicks(newDate.Ticks).TotalSeconds;
            double server = TimeSpan.FromTicks(oldDate.Ticks).TotalSeconds;

            return (server + click);
        }
        public static string AddTime(string clickTime, int timeStepAsSecond)
        {
            DateTime.TryParse(clickTime, out var clickDate);

            clickDate = clickDate.AddSeconds(timeStepAsSecond);
            var calculatedDate = clickDate.ToString("yyyy/MM/dd HH:mm:ss");

            return calculatedDate;
        }
        public static string AddTime(string localTime, object notificationTimeFreq)
        {
            throw new NotImplementedException();
        }
        public static string GetLocalTimeAsString(DateTypes dateType = DateTypes.Now)
        {
            DateTime now = dateType == DateTypes.Now ? DateTime.Now : DateTime.UtcNow;
            string result = now.ToString("yyyy/MM/dd HH:mm:ss");

            return result;
        }
        public static DateTime GetLocalTime(DateTypes dateType = DateTypes.Now)
        {
            DateTime now = dateType == DateTypes.Now ? DateTime.Now : DateTime.UtcNow;
            return now;
        }
        public static int GetLocalTimeHour(DateTypes dateType = DateTypes.Now)
        {
            DateTime now = dateType == DateTypes.Now ? DateTime.Now : DateTime.UtcNow;
            return now.Hour;
        }
        public static string GetHourAsString(DateTypes dateType = DateTypes.Now)
        {
            DateTime now = dateType == DateTypes.Now ? DateTime.Now : DateTime.UtcNow;
            int h = now.Hour;
            string hourStr;

            if (h < 10)
            {
                hourStr = "0" + h;
            }
            else
            {
                hourStr = h.ToString();
            }

            return hourStr;
        }
        public static string GetHourSpaceAsString(DateTypes dateType = DateTypes.Now)
        {
            DateTime now = dateType == DateTypes.Now ? DateTime.Now : DateTime.UtcNow;
            int h1 = now.Hour;
            int h2 = h1 + 1;

            string hstr1;
            string hstr2;

            if (h1 < 10)
                hstr1 = "0" + h1;
            else
                hstr1 = h1.ToString();


            if (h2 < 10)
                hstr2 = "0" + h2;
            else
                hstr2 = h2.ToString();


            return hstr1 + "_" + hstr2;
        }
        #endregion

        #region Timer
        public static void StartTimer()
        {
            _time = Time.realtimeSinceStartup;
        }
        public static float StopTimer(string title = "")
        {
            float diff = Time.realtimeSinceStartup - _time;
            diff = diff < 0 ? 0 : diff;
            Debug.Log(title + "TIME::" + diff);
            return diff;
        }
        #endregion

        #region Fetch
        public async static Task RefreshDateTimeAsync()
        {
            var localDate = await FetchCurrentLocalDate();
            Debug.Log($"Current local date: {localDate}");
            _cachedDateTime = localDate;
            await Task.Yield();
        }
        private static async Task<DateTime> FetchCurrentLocalDate()
        {
            using UnityWebRequest request = new UnityWebRequest(TimeApiUrl, UnityWebRequest.kHttpVerbHEAD);
            request.redirectLimit = 0;
            request.timeout = 10;

            UnityWebRequestAsyncOperation asyncOp = request.SendWebRequest();

            while (!asyncOp.isDone)
                await Task.Delay(100);

            if (request.result == UnityWebRequest.Result.Success)
            {
                string dateString = request.GetResponseHeader("Date");
                DateTime serverUtcDate = DateTime.Parse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                serverUtcDate.Add(TimeSpan.FromSeconds(-Time.realtimeSinceStartup));
                DateTime localDate = TimeZoneInfo.ConvertTimeFromUtc(serverUtcDate, TimeZoneInfo.Local);
                return localDate;
            }
            else
            {
                Debug.LogError($"Error fetching date: {request.error}");
                return default;
            }
        }
        #endregion

        #region Gets
        public static async Task<float> GetStartCountdownTime(int targetHour, DateTime? savedDateTime)
        {
            DateTime localDate = await GetLocalDateFetcher();

            // ReSharper disable once PossibleInvalidOperationException
            float startCountdownTime = (float)((targetHour * HourAsSecond) -
                                               (localDate - savedDateTime)?.TotalSeconds) + 1;
            return startCountdownTime;
        }
        public static async Task<DateTime> GetLocalDateFetcher()
        {
            float currentPlayedTime = Time.realtimeSinceStartup;
            TimeSpan value = TimeSpan.FromSeconds(currentPlayedTime);
            DateTime localDate = await GetCachedDateTime();
            DateTime localDateFetcher = localDate.Add(value);

            return localDateFetcher;
        }
        public static async Task<DateTime> GetCachedDateTime()
        {
            if (_cachedDateTime == default)
                await Task.Run(() => _cachedDateTime != default);

            return _cachedDateTime;
        }
        #endregion
    }
}
