using CodeCatGames.HiveMind.Core.Runtime.Helpers.Exploder;
using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.Helpers.ExploderSample
{
    public class ExploderSample : MonoBehaviour
    {
        #region Fields
        [Header("Exploder Sample Fields")]
        [SerializeField] private Exploder exploder;
        #endregion

        #region Executes
        [ContextMenu("Explode")]
        public void Explode() => exploder?.Explode();
        [ContextMenu("Refresh")]
        public void Refresh() => exploder?.Refresh();
        #endregion
    }
}