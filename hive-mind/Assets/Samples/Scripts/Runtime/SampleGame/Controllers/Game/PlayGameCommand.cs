using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Game
{
    public sealed class PlayGameCommand : Command<PlayGameSignal>
    {
        #region ReadonlyFields
        private readonly SignalBus _signalBus;
        #endregion

        #region Constructor
        public PlayGameCommand(SignalBus signalBus) => _signalBus = signalBus;
        #endregion

        #region Executes
        public override void Execute(PlayGameSignal signal) =>
            _signalBus.Fire(new ChangeUIPanelSignal(UIPanelTypes.InGamePanel));
        #endregion
    }
}
