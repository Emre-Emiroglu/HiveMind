using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Game
{
    public sealed class GameExitCommand : Command<GameExitSignal>
    {
        #region ReadonlyFields
        private readonly SignalBus _signalBus;
        #endregion

        #region Constructor
        public GameExitCommand(SignalBus signalBus) => _signalBus = signalBus;
        #endregion

        #region Executes
        public override void Execute(GameExitSignal signal) => _signalBus.Fire(new LoadSceneSignal(SceneID.MainMenu));
        #endregion
    }
}
