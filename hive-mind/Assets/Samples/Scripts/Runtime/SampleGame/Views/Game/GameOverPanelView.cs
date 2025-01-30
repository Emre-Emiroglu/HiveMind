using System;
using System.Collections.Generic;
using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.UI;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Interfaces.UI;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.UI;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Game
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class GameOverPanelView : View, IUIPanel
    {
        #region Fields
        [Header("Game Over Panel View Fields")]
        [SerializeField] private UIPanelVo uiPanelVo;
        [SerializeField] private GameOverPanelsMap gameOverPanels;
        [SerializeField] private Button failHomeButton;
        [SerializeField] private Button successHomeButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button nextButton;
        #endregion

        #region Getters
        public UIPanelVo UIPanelVo => uiPanelVo;
        public Dictionary<bool, GameObject> GameOverPanels => gameOverPanels.Clone();
        public Button FailHomeButton => failHomeButton;
        public Button SuccessHomeButton => successHomeButton;
        public Button RestartButton => restartButton;
        public Button NextButton => nextButton;
        #endregion
    }

    [Serializable]
    public sealed class GameOverPanelsMap : SerializableDictionaryBase<bool, GameObject> { }
}
