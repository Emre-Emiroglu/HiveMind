using UnityEngine;

namespace CodeCatGames.HiveMind.Core.Runtime.Helpers.SlowMotion
{
    public sealed class SlowMotion 
    {
        #region Constants
        private const float TimeStep = .02f;
        private const float TimeScale = 1f;
        #endregion

        #region Fields
        private float _factor = .25f;
        #endregion

        #region Core
        public void Setup(float factor) => _factor = factor;
        #endregion

        #region SetActivation
        public void Activate()
        {
            Time.timeScale = _factor;
            Time.fixedDeltaTime = _factor * TimeStep;
        }
        public void DeActivate()
        {
            Time.timeScale = TimeScale;
            Time.fixedDeltaTime = TimeStep;
        }
        #endregion
    }
}
