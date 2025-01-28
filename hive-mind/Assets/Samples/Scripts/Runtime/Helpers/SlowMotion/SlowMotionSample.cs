using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.Helpers.SlowMotion
{
    public sealed class SlowMotionSample : MonoBehaviour
    {
        #region Fields
        [Header("Slow Motion Sample Fields")]
        [SerializeField] private float slowMotionFactor;
        private Core.Runtime.Helpers.SlowMotion.SlowMotion _slowMotion;
        #endregion

        #region Core
        private void Awake()
        {
            _slowMotion = new Core.Runtime.Helpers.SlowMotion.SlowMotion();
            
            _slowMotion?.Setup(slowMotionFactor);
        }
        #endregion

        #region Executes
        public void Activate() => _slowMotion?.Activate();
        public void DeActivate() => _slowMotion?.DeActivate();
        #endregion
    }
}