using UnityEngine;

namespace HiveMind.Core.Helpers.SlowMotion
{
    public sealed class SlowMotion : MonoBehaviour
    {
        #region Constants
        private const float TIME_STEP = .02f;
        private const float TIME_SCALE = 1f;
        #endregion

        #region Fields
        [Header("Slow Motion Settings")]
        [Range(0f, TIME_SCALE)][SerializeField] private float factor = .25f;
        #endregion

        #region SetActivation
        public void Activate()
        {
            Time.timeScale = factor;
            Time.fixedDeltaTime = factor * TIME_STEP;
        }
        public void DeActivate()
        {
            Time.timeScale = TIME_SCALE;
            Time.fixedDeltaTime = TIME_STEP;
        }
        #endregion
    }
}
