using UnityEngine;

namespace HiveMind.Core.Utilities.Runtime.FPSDisplayer
{
    public class FPSDisplayer: MonoBehaviour
    {
        #region Fields
        [Header("FPS Displayer Settings")]
        [SerializeField] private bool show = true;
        [SerializeField] private Rect rect = new Rect(960, 540, 128, 64);
        [Range(0f, 1f)][SerializeField] private float updateInterval = .5f;
        float accum;
        private int frames;
        private float timeLeft;
        private float fps;
        private GUIStyle textStyle = new GUIStyle();
        #endregion

        #region Core
        private void Initialize()
        {
            timeLeft = updateInterval;

            textStyle.fontStyle = FontStyle.Bold;
            textStyle.normal.textColor = Color.white;
        }
        private void Start() => Initialize();
        #endregion

        #region Calculate
        private void CalculateFPS()
        {
            timeLeft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            frames++;

            if (timeLeft <= 0)
            {
                fps = accum / frames;
                timeLeft = updateInterval;
                accum = 0f;
                frames = 0;
            }
        }
        #endregion

        #region Updates
        private void Update() => CalculateFPS();
        #endregion

        #region OnGUI
        private void OnGUI()
        {
            if (!show)
                return;

            GUI.Label(rect, fps.ToString("F2") + "FPS", textStyle);
        }
        #endregion
    }
}
