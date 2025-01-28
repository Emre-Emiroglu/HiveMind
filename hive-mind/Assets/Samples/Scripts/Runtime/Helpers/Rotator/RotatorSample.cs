using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.Helpers.Rotator
{
    public class RotatorSample : MonoBehaviour
    {
        #region Fields
        [Header("Rotator Sample Fields")]
        [SerializeField] private Core.Runtime.Helpers.Rotator.Rotator rotator;
        #endregion
        
        #region Executes
        [ContextMenu("Can Rotate")]
        public void Explode() => rotator?.SetCanRotate(true);
        [ContextMenu("Cant Rotate")]
        public void Refresh() => rotator?.SetCanRotate(false);
        #endregion

        #region Update
        private void Update() => rotator?.ExternalUpdate();
        #endregion
    }
}