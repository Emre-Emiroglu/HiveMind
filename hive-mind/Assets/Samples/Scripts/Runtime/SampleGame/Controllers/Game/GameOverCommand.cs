using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Game
{
    public sealed class GameOverCommand : Command<GameOverSignal>
    {
        #region ReadonlyFields
        private readonly SignalBus _signalBus;
        private readonly LevelModel _levelModel;
        #endregion

        #region Constructor
        public GameOverCommand(SignalBus signalBus, LevelModel levelModel)
        {
            _signalBus = signalBus;
            _levelModel = levelModel;
        }
        #endregion

        #region Executes
        public override void Execute(GameOverSignal signal)
        {
            _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic,
                signal.IsSuccess ? SoundTypes.GameWin : SoundTypes.GameFail));

            _signalBus.Fire(new ChangeUIPanelSignal(UIPanelTypes.GameOverPanel));
            _signalBus.Fire(new SetupGameOverPanelSignal(signal.IsSuccess));

            _levelModel.UpdateCurrentLevelIndex(false, signal.IsSuccess ? 1 : 0);
        }
        #endregion
    }
}
