using HiveMind.Helpers.Rotator;
using UnityEngine;

namespace HiveMind.HelpersSample.SampleClasses
{
    public class RotatorSample : MonoBehaviour
    {
        #region Fields
        [Header("Rotator Sample Fields")]
        [SerializeField] private Rotator rotator;
        #endregion

        #region SetCanRotates
        [ContextMenu("Can Rotate")]
        public void CanRotate() => rotator?.SetCanRotate(true);
        [ContextMenu("Cant Rotate")]
        public void CantRotate() => rotator?.SetCanRotate(false);
        #endregion

        #region Updates
        private void Update() => rotator?.ExternalUpdate();
        #endregion
    }
}
