using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Game;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Utilities.Extensions;
using Lofelt.NiceVibrations;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Game
{
    public class TutorialPanelMediator : Mediator<TutorialPanelView>
    {
        #region ReadonlyFields
        private readonly SignalBus _signalBus;
        private readonly TutorialModel _tutorialModel;
        #endregion

        #region Constructor
        public TutorialPanelMediator(TutorialPanelView view, SignalBus signalBus, TutorialModel tutorialModel) : base(view)
        {
            _signalBus = signalBus;
            _tutorialModel = tutorialModel;
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

                GetView.CloseButton.onClick.AddListener(OnCloseButtonClicked);
            }
            else
            {
                _signalBus.Unsubscribe<ChangeUIPanelSignal>(OnChangeUIPanelSignal);

                GetView.CloseButton.onClick.RemoveListener(OnCloseButtonClicked);
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
        private void OnCloseButtonClicked()
        {
            _tutorialModel.SetTutorial(true);

            _signalBus.Fire<PlayGameSignal>(new());
            _signalBus.Fire<PlayAudioSignal>(new(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
            _signalBus.Fire<PlayHapticSignal>(new(HapticPatterns.PresetType.LightImpact));
        }
        #endregion
    }
}
