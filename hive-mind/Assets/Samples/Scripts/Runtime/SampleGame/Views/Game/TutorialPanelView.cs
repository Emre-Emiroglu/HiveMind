using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Interfaces.CrossScene;
using UnityEngine;
using UnityEngine.UI;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Game
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class TutorialPanelView : View, IUIPanel
    {
        #region Fields
        [Header("Tutorial Panel View Fields")]
        [SerializeField] private UIPanelVo uiPanelVo;
        [SerializeField] private Button closeButton;
        #endregion

        #region Getters
        public UIPanelVo UIPanelVo => uiPanelVo;
        public Button CloseButton => closeButton;
        #endregion
    }
}
