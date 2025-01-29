using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.UI;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Interfaces.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Game
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TutorialPanelView : View, IUIPanel
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
