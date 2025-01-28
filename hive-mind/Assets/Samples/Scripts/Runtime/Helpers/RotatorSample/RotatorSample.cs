using CodeCatGames.HiveMind.Core.Runtime.Helpers.Rotator;
using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.Helpers.RotatorSample
{
    public class RotatorSample : MonoBehaviour
    {
        #region Fields
        [Header("Rotator Sample Fields")]
        [SerializeField] private Rotator rotator;
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