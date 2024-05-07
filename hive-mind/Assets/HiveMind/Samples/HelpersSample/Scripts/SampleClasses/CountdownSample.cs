using HiveMind.Core.Utilities.Runtime.Enums;
using HiveMind.Core.Helpers.Runtime.Countdown;
using UnityEngine;
using UnityEngine.UI;

namespace HiveMind.Samples.HelpersSample.SampleClasses
{
    public class CountdownSample : MonoBehaviour
    {
        #region Fields
        [Header("Countdown Sample Fields")]
        [SerializeField] private Text countdownText;
        [SerializeField] private TimeFormattingTypes timeFormattingTypes = TimeFormattingTypes.DaysHoursMinutesSeconds;
        [SerializeField] private bool showMiliSeconds = true;
        [Range(0f, 86400)][SerializeField] private float countdownStartTime = 10f;
        private Countdown countdown;
        #endregion

        #region Core
        [ContextMenu("Start Countdown")]
        public void StartCountdown() => countdown = new Countdown(timeFormattingTypes, showMiliSeconds, countdownStartTime);
        #endregion

        #region SetPauseStatus
        [ContextMenu("Pause")]
        public void Pause() => countdown?.SetPause(true);
        [ContextMenu("UnPause")]
        public void UnPause() => countdown?.SetPause(false);
        #endregion

        #region Receivers
        private void OnCountDownEnded() => Debug.Log("Countdown Ended!");
        #endregion

        #region Update
        private void Update()
        {
            countdown?.ExternalUpdate(OnCountDownEnded);
            countdownText.text = countdown == null ? "Countdown not began!" : countdown.GetFormattedTime();
        }
        #endregion
    }
}
