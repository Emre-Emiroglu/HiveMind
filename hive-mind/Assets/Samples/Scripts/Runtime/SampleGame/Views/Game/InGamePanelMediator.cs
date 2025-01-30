using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Utilities.Extensions;
using Lofelt.NiceVibrations;
using PrimeTween;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Game
{
    public sealed class InGamePanelMediator : Mediator<InGamePanelView>
    {
        #region ReadonlyFields
        private readonly SignalBus _signalBus;
        private readonly LevelModel _levelModel;
        #endregion

        #region Constructor
        public InGamePanelMediator(InGamePanelView view, SignalBus signalBus, LevelModel levelModel) : base(view)
        {
            _signalBus = signalBus;
            _levelModel = levelModel;
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

                GetView.WinButton.onClick.AddListener(OnWinButtonPressed);
                GetView.FailButton.onClick.AddListener(OnFailButtonPressed);
                GetView.AddCurrencyButton.onClick.AddListener(OnAddCurrencyButtonPressed);
            }
            else
            {
                _signalBus.Unsubscribe<ChangeUIPanelSignal>(OnChangeUIPanelSignal);

                GetView.WinButton.onClick.RemoveListener(OnWinButtonPressed);
                GetView.FailButton.onClick.RemoveListener(OnFailButtonPressed);
                GetView.AddCurrencyButton.onClick.RemoveListener(OnAddCurrencyButtonPressed);
            }
        }
        #endregion

        #region SignalReceivers
        private void OnChangeUIPanelSignal(ChangeUIPanelSignal signal)
        {
            bool isShow = signal.UIPanelType == GetView.UIPanelVo.UIPanelType;
            GetView.UIPanelVo.CanvasGroup.ChangeUIPanelCanvasGroupActivation(isShow);
            GetView.UIPanelVo.PlayableDirector.ChangeUIPanelTimelineActivation(isShow);

            if (isShow)
                SetLevelText();
        }
        #endregion

        #region ButtonReceivers
        private void OnWinButtonPressed()
        {
            _signalBus.Fire<GameOverSignal>(new(true));
            _signalBus.Fire<PlayAudioSignal>(new(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
            _signalBus.Fire<PlayHapticSignal>(new(HapticPatterns.PresetType.LightImpact));
        }
        private void OnFailButtonPressed()
        {
            _signalBus.Fire<GameOverSignal>(new(false));
            _signalBus.Fire<PlayAudioSignal>(new(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
            _signalBus.Fire<PlayHapticSignal>(new(HapticPatterns.PresetType.LightImpact));
        }
        private void OnAddCurrencyButtonPressed()
        {
            _signalBus.Fire<SpawnCurrencyTrailSignal>(new(new(CurrencyTypes.Coin,
                                                              1,
                                                              .25f,
                                                              Ease.Linear,
                                                              GetView.CurrencyTrailStartTransform.position,
                                                              GetView.CurrencyTrailTargetTransform.position)));
            _signalBus.Fire<PlayAudioSignal>(new(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
            _signalBus.Fire<PlayHapticSignal>(new(HapticPatterns.PresetType.LightImpact));
        }
        #endregion

        #region Executes
        private void SetLevelText()
        {
            int levelNumber = _levelModel.LevelPersistentData.CurrentLevelIndex + 1;
            GetView.LevelText.SetText($"Level {levelNumber}");
        }
        #endregion
    }
}
