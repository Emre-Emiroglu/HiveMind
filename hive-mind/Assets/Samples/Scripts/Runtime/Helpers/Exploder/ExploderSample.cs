using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.Helpers.Exploder
{
    public sealed class ExploderSample : MonoBehaviour
    {
        #region Fields
        [Header("Exploder Sample Fields")]
        [SerializeField] private Core.Runtime.Helpers.Exploder.Exploder exploder;
        #endregion

        #region Executes
        [ContextMenu("Explode")]
        public void Explode() => exploder?.Explode();
        [ContextMenu("Refresh")]
        public void Refresh() => exploder?.Refresh();
        #endregion
    }
}