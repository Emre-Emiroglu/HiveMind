using CodeCatGames.HiveMind.Core.Runtime.Helpers.Countdown;
using CodeCatGames.HiveMind.Core.Runtime.Utilities.Enums;
using TMPro;
using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.HelpersSample.Runtime.CountdownSample
{
    public class CountdownSample : MonoBehaviour
    {
        #region Fields
        [Header("Countdown Sample Fields")]
        [SerializeField] private TextMeshProUGUI countdownSampleText;
        [SerializeField] private TMP_InputField addSecondsField;
        private Countdown _countdown;
        #endregion

        #region Core
        private void Awake()
        {
            _countdown = new Countdown();
            
            _countdown.Setup(TimeFormattingTypes.DaysHoursMinutesSeconds, true, 3600);
        }
        #endregion

        #region Executes
        public void SetPause(bool isPause) => _countdown?.SetPause(isPause);
        public void AddSeconds() => _countdown?.AddSeconds(int.Parse(addSecondsField.text));
        #endregion

        #region Receivers
        private void OnCountdownEnded() => Debug.Log("Countdown Ended!");
        #endregion

        #region Update
        private void Update()
        {
            _countdown?.ExternalUpdate(OnCountdownEnded);

            countdownSampleText.SetText($"{_countdown?.GetFormattedTime()}");
        }
        #endregion
    }
}