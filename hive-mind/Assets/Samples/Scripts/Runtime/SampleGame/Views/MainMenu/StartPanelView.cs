using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.UI;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Interfaces.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.MainMenu
{
    [RequireComponent(typeof(CanvasGroup))]
    public class StartPanelView: View, IUIPanel
    {
        #region Fields
        [Header("Main Scene Panel View Fields")]
        [SerializeField] private UIPanelVo uiPanelVo;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Button playButton;
        #endregion

        #region Getters
        public UIPanelVo UIPanelVo => uiPanelVo;
        public TextMeshProUGUI LevelText => levelText;
        public Button PlayButton => playButton;
        #endregion
    }
}
