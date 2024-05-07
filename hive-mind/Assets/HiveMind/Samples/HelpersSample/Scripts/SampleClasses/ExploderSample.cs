using HiveMind.Core.Helpers.Runtime.Exploder;
using UnityEngine;

namespace HiveMind.Samples.HelpersSample.SampleClasses
{
    public class ExploderSample : MonoBehaviour
    {
        #region Fields
        [Header("Exploder Sample Fields")]
        [SerializeField] private Exploder exploder;
        #endregion

        #region Core
        [ContextMenu("Explode")]
        public void Explode() => exploder?.Explode();
        [ContextMenu("Refresh")]
        public void Refresh() => exploder?.Refresh();
        #endregion
    }
}
