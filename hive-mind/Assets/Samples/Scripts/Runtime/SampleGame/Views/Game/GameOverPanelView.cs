using System;
using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.UI;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Interfaces.UI;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.UI;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Game
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GameOverPanelView : View, IUIPanel
    {
        #region Fields
        [Header("Game Over Panel View Fields")]
        [SerializeField] private UIPanelVo uiPanelVo;
        [SerializeField] private GameOverPanels gameOverPanels;
        [SerializeField] private Button failHomeButton;
        [SerializeField] private Button successHomeButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button nextButton;
        #endregion

        #region Getters
        public UIPanelVo UIPanelVo => uiPanelVo;
        public GameOverPanels GameOverPanels => gameOverPanels;
        public Button FailHomeButton => failHomeButton;
        public Button SuccessHomeButton => successHomeButton;
        public Button RestartButton => restartButton;
        public Button NextButton => nextButton;
        #endregion
    }

    [Serializable]
    public class GameOverPanels : SerializableDictionaryBase<bool, GameObject> { }
}
