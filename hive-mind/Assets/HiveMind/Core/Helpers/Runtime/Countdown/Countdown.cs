using System;
using UnityEngine;
using HiveMind.Core.Utilities.Runtime.Enums;
using HiveMind.Core.Utilities.Runtime.TextFormatter;

namespace HiveMind.Core.Helpers.Runtime.Countdown
{
    public sealed class Countdown
    {
        #region Fields
        private TimeFormattingTypes _timeFormattingType;
        private bool _showMilliSeconds;
        private double _countdownInternal;
        private bool _pause;
        #endregion

        #region Getters
        public double CountDownInternal => _countdownInternal;
        public string GetFormattedTime() => TextFormatter.FormatTime(_countdownInternal, _timeFormattingType, _showMilliSeconds);
        public bool IsPause => _pause;
        #endregion

        #region Core
        public void Setup(TimeFormattingTypes timeFormattingType, bool showMilliSeconds, double countDownTime)
        {
            _timeFormattingType = timeFormattingType;
            _showMilliSeconds = showMilliSeconds;
            _countdownInternal = countDownTime;
            _pause = false;
        }
        #endregion

        #region SetStatus
        public void SetPause(bool isPause) => _pause = isPause;
        public void AddSeconds(int seconds) => _countdownInternal += seconds;
        #endregion

        #region Update
        public void ExternalUpdate(Action countDownEnded = null)
        {
            if (_pause)
                return;

            if (_countdownInternal > 0)
            {
                _countdownInternal -= Time.deltaTime;
                if (_countdownInternal < 0)
                {
                    _countdownInternal = 0;
                    countDownEnded?.Invoke();
                }
            }
        }
        #endregion
    }
}
