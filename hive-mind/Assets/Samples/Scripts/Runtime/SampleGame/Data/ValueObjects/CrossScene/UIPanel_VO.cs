using System;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using UnityEngine;
using UnityEngine.Playables;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene
{
    [Serializable]
    public struct UIPanelVo
    {
        #region Fields
        [Header("UI Panel VO Fields")]
        [SerializeField] private UIPanelTypes uiPanelType;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private PlayableDirector playableDirector;
        #endregion

        #region Getters
        public readonly UIPanelTypes UIPanelType => uiPanelType;
        public readonly CanvasGroup CanvasGroup => canvasGroup;
        public readonly PlayableDirector PlayableDirector => playableDirector;
        #endregion
    }
}
