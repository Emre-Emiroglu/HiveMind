using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.MainMenu;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.MainMenu
{
    public class InitializeMainMenuCommand : Command<InitializeMainMenuSignal>
    {
        #region ReadonlyFields
        private readonly SignalBus _signalBus;
        #endregion

        #region Constructor
        public InitializeMainMenuCommand(SignalBus signalBus) => _signalBus = signalBus;
        #endregion

        #region Executes
        public override void Execute(InitializeMainMenuSignal signal)
        {
            _signalBus.Fire<ChangeLoadingScreenActivationSignal>(new(false, null));
            _signalBus.Fire<ChangeUIPanelSignal>(new(UIPanelTypes.StartPanel));
            _signalBus.Fire<PlayAudioSignal>(new(AudioTypes.Music, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
        }
        #endregion
    }
}
