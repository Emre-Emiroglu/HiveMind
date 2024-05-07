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
        private const int DAY_AS_SECOND = 86400;
        private const int HOUR_AS_SECOND = 3600;
        private const int HOUR_AS_DAY = 24;
        private const int SECOND_AS_DAY = 60;
        private const string TIME_API_URL = "https://www.google.com";
        #endregion

        #region Fields
        private static float time;
        private static DateTime cachedDateTime;
        #endregion

        #region Core
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static async void Initialize()
        {
            cachedDateTime = default;
            await RefreshDateTimeAsync();
        }
        #endregion

        #region Local
        public static double SubstractCurrent(string time, DateTypes dateType = DateTypes.Now)
        {
            DateTime timeDate = new DateTime();
            DateTime localDate = dateType == DateTypes.Now ? DateTime.Now : DateTime.UtcNow;

            DateTime.TryParse(time, out timeDate);

            double timeValue = TimeSpan.FromTicks(timeDate.Ticks).TotalSeconds;
            double currentVal = TimeSpan.FromTicks(localDate.Ticks).TotalSeconds;

            return (currentVal - timeValue);
        }
        public static DateTime ConvertToDateTime(string timeStr)
        {
            DateTime newDate = new DateTime();
            DateTime.TryParse(timeStr, out newDate);
            return newDate;
        }
        public static double Substract(string oldTime, string newTime)
        {
            DateTime newDate = new DateTime();
            DateTime oldDate = new DateTime();

            DateTime.TryParse(newTime, out newDate);
            DateTime.TryParse(oldTime, out oldDate);

            double old = TimeSpan.FromTicks(oldDate.Ticks).TotalSeconds;
            double now = TimeSpan.FromTicks(newDate.Ticks).TotalSeconds;

            return (now - old);
        }
        public static double Addition(string time, string addedTime)
        {
            DateTime newDate = new DateTime();
            DateTime oldDate = new DateTime();

            DateTime.TryParse(time, out newDate);
            DateTime.TryParse(addedTime, out oldDate);

            double click = TimeSpan.FromTicks(newDate.Ticks).TotalSeconds;
            double server = TimeSpan.FromTicks(oldDate.Ticks).TotalSeconds;

            return (server + click);
        }
        public static string AddTime(string clickTime, int timeStepAsSecond)
        {
            DateTime clickdate = new DateTime(0, DateTimeKind.Utc);
            string calculatedDate = "";

            DateTime.TryParse(clickTime, out clickdate);

            clickdate = clickdate.AddSeconds(timeStepAsSecond);
            calculatedDate = clickdate.ToString("yyyy/MM/dd HH:mm:ss");

            return calculatedDate;
        }
        public static string AddTime(string localTime, object nOTIFICATION_TIME_FREQ)
        {
            throw new NotImplementedException();
        }
        public static string GetLocalTimeAsString(DateTypes dateType = DateTypes.Now)
        {
            string result = "";

            DateTime now = dateType == DateTypes.Now ? DateTime.Now : DateTime.UtcNow;
            result = now.ToString("yyyy/MM/dd HH:mm:ss");

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
            string hourStr = "";

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

            string hstr1 = "";
            string hstr2 = "";

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
            time = Time.realtimeSinceStartup;
        }
        public static float StopTimer(string title = "")
        {
            float diff = Time.realtimeSinceStartup - time;
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
            cachedDateTime = localDate;
            await Task.Yield();
        }
        private static async Task<DateTime> FetchCurrentLocalDate()
        {
            using (UnityWebRequest request = new UnityWebRequest(TIME_API_URL, UnityWebRequest.kHttpVerbHEAD))
            {
                request.redirectLimit = 0;
                request.timeout = 10;

                UnityWebRequestAsyncOperation asyncOp = request.SendWebRequest();

                while (!asyncOp.isDone)
                    await Task.Delay(100);

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string dateString = request.GetResponseHeader("Date");
                    DateTime serverUtcDate = DateTime.Parse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
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
        }
        #endregion

        #region Gets
        public static async Task<float> GetStartCountdownTime(int targetHour, DateTime? savedDateTime)
        {
            DateTime localDate = await GetLocalDateFetcher();

            float startCountdownTime = (float)((targetHour * HOUR_AS_SECOND) -
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
            if (cachedDateTime == default)
                await Task.Run(() => cachedDateTime != default);

            return cachedDateTime;
        }
        #endregion
    }
}
