using UnityEngine;

namespace HiveMind.Core.Helpers.Runtime.SlowMotion
{
    public sealed class SlowMotion : MonoBehaviour
    {
        #region Constants
        private const float TimeStep = .02f;
        private const float TimeScale = 1f;
        #endregion

        #region Fields
        [Header("Slow Motion Settings")]
        [Range(0f, TimeScale)][SerializeField] private float factor = .25f;
        #endregion

        #region SetActivation
        public void Activate()
        {
            Time.timeScale = factor;
            Time.fixedDeltaTime = factor * TimeStep;
        }
        public void DeActivate()
        {
            Time.timeScale = TimeScale;
            Time.fixedDeltaTime = TimeStep;
        }
        #endregion
    }
}
