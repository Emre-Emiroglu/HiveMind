using UnityEngine;

namespace HiveMind.Core.Utilities.Runtime.FPSDisplayer
{
    public sealed class FPSDisplayer: MonoBehaviour
    {
        #region Fields
        [Header("FPS Displayer Settings")]
        [SerializeField] private bool _show = true;
        [SerializeField] private Rect _rect = new(960, 540, 128, 64);
        [Range(0f, 1f)][SerializeField] private float _updateInterval = .5f;
        private float _accum;
        private int _frames;
        private float _timeLeft;
        private float _fps;
        private readonly GUIStyle _textStyle = new();
        #endregion

        #region Core
        private void Initialize()
        {
            _timeLeft = _updateInterval;

            _textStyle.fontStyle = FontStyle.Bold;
            _textStyle.normal.textColor = Color.white;
        }
        private void Start() => Initialize();
        #endregion

        #region Calculate
        private void CalculateFPS()
        {
            _timeLeft -= Time.deltaTime;
            _accum += Time.timeScale / Time.deltaTime;
            _frames++;

            if (_timeLeft <= 0)
            {
                _fps = _accum / _frames;
                _timeLeft = _updateInterval;
                _accum = 0f;
                _frames = 0;
            }
        }
        #endregion

        #region Updates
        private void Update() => CalculateFPS();
        #endregion

        #region OnGUI
        private void OnGUI()
        {
            if (!_show)
                return;

            GUI.Label(_rect, _fps.ToString("F2") + "FPS", _textStyle);
        }
        #endregion
    }
}
