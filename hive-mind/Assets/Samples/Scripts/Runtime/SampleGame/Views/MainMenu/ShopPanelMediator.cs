using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Utilities.Extensions;
using Lofelt.NiceVibrations;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.MainMenu
{
    public class ShopPanelMediator : Mediator<ShopPanelView>
    {
        #region ReadonlyFields
        private readonly SignalBus _signalBus;
        #endregion

        #region Constructor
        public ShopPanelMediator(ShopPanelView view, SignalBus signalBus) : base(view)
        {
            _signalBus = signalBus;
        }
        #endregion

        #region PostConstruct
        public override void PostConstruct() { }
        #endregion

        #region Core
        public override void Initialize() => SetCycleSubscriptions(true);
        public override void Dispose() => SetCycleSubscriptions(false);
        #endregion

        #region Subscriptions
        private void SetCycleSubscriptions(bool isSub)
        {
            if (isSub)
            {
                _signalBus.Subscribe<ChangeUIPanelSignal>(OnChangeUIPanelSignal);

                GetView.HomeButton.onClick.AddListener(OnHomeButtonClicked);
            }
            else
            {
                _signalBus.Unsubscribe<ChangeUIPanelSignal>(OnChangeUIPanelSignal);

                GetView.HomeButton.onClick.RemoveListener(OnHomeButtonClicked);
            }
        }
        #endregion
        
        #region SignalReceivers
        private void OnChangeUIPanelSignal(ChangeUIPanelSignal signal)
        {
            bool isShow = signal.UIPanelType == GetView.UIPanelVo.UIPanelType;

            GetView.UIPanelVo.CanvasGroup.ChangeUIPanelCanvasGroupActivation(isShow);
            GetView.UIPanelVo.PlayableDirector.ChangeUIPanelTimelineActivation(isShow);
        }
        #endregion

        #region ButtonReceivers
        private void OnHomeButtonClicked()
        {
            _signalBus.Fire<ChangeUIPanelSignal>(new(UIPanelTypes.StartPanel));
            _signalBus.Fire<PlayAudioSignal>(new(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
            _signalBus.Fire<PlayHapticSignal>(new(HapticPatterns.PresetType.LightImpact));
        }
        #endregion
    }
}
