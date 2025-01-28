using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Views
{
    public class ChangeColorObjectView : View
    {
        #region Fields
        [Header("Change Color Object View Fields")]
        [SerializeField] private MeshRenderer meshRenderer;
        #endregion

        #region Getters
        public MeshRenderer MeshRenderer => meshRenderer;
        #endregion
    }
}