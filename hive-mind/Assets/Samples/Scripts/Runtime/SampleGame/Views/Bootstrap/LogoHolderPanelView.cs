using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.UI;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Interfaces.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Bootstrap
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LogoHolderPanelView : View, IUIPanel
    {
        #region Fields
        [Header("Logo Holder Panel View Fields")]
        [SerializeField] private UIPanelVo uiPanelVo;
        [SerializeField] private Image logoImage;
        #endregion

        #region Getters
        public UIPanelVo UIPanelVo => uiPanelVo;
        public Image LogoImage => logoImage;
        #endregion
    }
}
