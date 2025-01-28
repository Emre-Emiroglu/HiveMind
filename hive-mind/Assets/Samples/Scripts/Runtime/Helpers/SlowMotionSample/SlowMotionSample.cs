using CodeCatGames.HiveMind.Core.Runtime.Helpers.SlowMotion;
using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.Helpers.SlowMotionSample
{
    public class SlowMotionSample : MonoBehaviour
    {
        #region Fields
        [Header("Slow Motion Sample Fields")]
        [SerializeField] private float slowMotionFactor;
        private SlowMotion _slowMotion;
        #endregion

        #region Core
        private void Awake()
        {
            _slowMotion = new SlowMotion();
            
            _slowMotion?.Setup(slowMotionFactor);
        }
        #endregion

        #region Executes
        public void Activate() => _slowMotion?.Activate();
        public void DeActivate() => _slowMotion?.DeActivate();
        #endregion
    }
}