using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Interfaces.CrossScene;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Game
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class InGamePanelView : View, IUIPanel
    {
        #region Fields
        [Header("In Game Panel View Fields")]
        [SerializeField] private UIPanelVo uiPanelVo;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Transform currencyTrailStartTransform;
        [SerializeField] private Transform currencyTrailTargetTransform;
        [SerializeField] private Button winButton;
        [SerializeField] private Button failButton;
        [SerializeField] private Button addCurrencyButton;
        #endregion

        #region Getters
        public UIPanelVo UIPanelVo => uiPanelVo;
        public TextMeshProUGUI LevelText => levelText;
        public Transform CurrencyTrailStartTransform => currencyTrailStartTransform;
        public Transform CurrencyTrailTargetTransform => currencyTrailTargetTransform;
        public Button WinButton => winButton;
        public Button FailButton => failButton;
        public Button AddCurrencyButton => addCurrencyButton;
        #endregion
    }
}
