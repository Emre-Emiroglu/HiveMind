using System;
using UnityEngine;
using HiveMind.Core.Utilities.Enums;
using HiveMind.Core.Utilities.TextFormatter;

namespace HiveMind.Core.Helpers.Countdown
{
    public sealed class Countdown
    {
        #region Fields
        private readonly TimeFormattingTypes timeFormattingType;
        private readonly bool showMilliSeconds;
        private readonly double countDownTime;
        private double countdownInternal;
        private bool countDownOver;
        private bool pause;
        #endregion

        #region Getters
        public double CountDownInternal => countdownInternal;
        public string GetFormattedTime() => TextFormatter.FormatTime(countdownInternal, timeFormattingType, showMilliSeconds);
        public bool IsPause => pause;
        #endregion

        #region Constructor
        public Countdown(TimeFormattingTypes timeFormattingType, bool showMilliSeconds, double countDownTime)
        {
            this.timeFormattingType = timeFormattingType;
            this.showMilliSeconds = showMilliSeconds;
            this.countDownTime = countDownTime;
            this.countdownInternal = this.countDownTime;
            this.countDownOver = false;
            pause = false;
        }
        #endregion

        #region SetPauseStatus
        public void SetPause(bool isPause) => this.pause = isPause;
        #endregion

        #region Update
        public void ExternalUpdate(Action OnCountDownEnded = null)
        {
            if (pause)
                return;

            if (countdownInternal > 0)
            {
                countdownInternal -= Time.deltaTime;
                if (countdownInternal < 0)
                {
                    countdownInternal = 0;
                    OnCountDownEnded?.Invoke();
                }
            }
            else
                if (!countDownOver)
                    countDownOver = true;
        }
        #endregion
    }
}
