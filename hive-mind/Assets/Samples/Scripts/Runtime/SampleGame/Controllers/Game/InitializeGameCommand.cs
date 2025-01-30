using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Game;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Game
{
    public sealed class InitializeGameCommand : Command<InitializeGameSignal>
    {
        #region ReadonlyFields
        private readonly SignalBus _signalBus;
        private readonly TutorialModel _tutorialModel;
        #endregion

        #region Constructor
        public InitializeGameCommand(SignalBus signalBus, TutorialModel tutorialModel)
        {
            _signalBus = signalBus;
            _tutorialModel = tutorialModel;
        }
        #endregion

        #region Executes
        public override void Execute(InitializeGameSignal signal)
        {
            _signalBus.Fire(new ChangeLoadingScreenActivationSignal(false, null));

            if (_tutorialModel.IsTutorialShowed)
                _signalBus.Fire(new PlayGameSignal());
            else
                _signalBus.Fire(new ChangeUIPanelSignal(UIPanelTypes.TutorialPanel));
        }
        #endregion
    }
}
