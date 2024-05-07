using HiveMind.Core.Helpers.Runtime.SlowMotion;
using UnityEngine;

namespace HiveMind.Samples.HelpersSample.SampleClasses
{
    public class SlowMotionSample : MonoBehaviour
    {
        #region Fields
        [Header("Slow Motion Sample Fields")]
        [SerializeField] private SlowMotion slowMotion;
        #endregion

        #region SetActivation
        [ContextMenu("Activate")]
        public void Activate() => slowMotion.Activate();
        [ContextMenu("DeActivate")]
        public void DeActivate() => slowMotion.DeActivate();
        #endregion
    }
}
