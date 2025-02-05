using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Utilities.CrossScene;
using Lofelt.NiceVibrations;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Game
{
    public sealed class GameOverPanelMediator : Mediator<GameOverPanelView>
    {
        #region ReadonlyFields
        private readonly SignalBus _signalBus;
        #endregion

        #region Constructor
        public GameOverPanelMediator(GameOverPanelView view, SignalBus signalBus) : base(view) =>
            _signalBus = signalBus;
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
                _signalBus.Subscribe<SetupGameOverPanelSignal>(OnSetupGameOverPanelSignal);

                GetView.FailHomeButton.onClick.AddListener(OnHomeButtonClicked);
                GetView.SuccessHomeButton.onClick.AddListener(OnHomeButtonClicked);
                GetView.RestartButton.onClick.AddListener(OnRestartButtonClicked);
                GetView.NextButton.onClick.AddListener(OnNextButtonClicked);
            }
            else
            {
                _signalBus.Unsubscribe<ChangeUIPanelSignal>(OnChangeUIPanelSignal);
                _signalBus.Unsubscribe<SetupGameOverPanelSignal>(OnSetupGameOverPanelSignal);

                GetView.FailHomeButton.onClick.RemoveListener(OnHomeButtonClicked);
                GetView.SuccessHomeButton.onClick.RemoveListener(OnHomeButtonClicked);
                GetView.RestartButton.onClick.RemoveListener(OnRestartButtonClicked);
                GetView.NextButton.onClick.RemoveListener(OnNextButtonClicked);
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
        private void OnSetupGameOverPanelSignal(SetupGameOverPanelSignal signal)
        {
            foreach (var item in GetView.GameOverPanels)
            {
                bool isActive = item.Key == signal.IsSuccess;
                item.Value.SetActive(isActive);
            }
        }
        #endregion

        #region ButtonReceivers
        private void OnHomeButtonClicked()
        {
            _signalBus.Fire(new GameExitSignal());
            _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
            _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.LightImpact));
        }
        private void OnRestartButtonClicked()
        {
            _signalBus.Fire(new PlayGameSignal());
            _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
            _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.LightImpact));
        }
        private void OnNextButtonClicked()
        {
            _signalBus.Fire(new PlayGameSignal());
            _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
            _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.LightImpact));
        }
        #endregion
    }
}
