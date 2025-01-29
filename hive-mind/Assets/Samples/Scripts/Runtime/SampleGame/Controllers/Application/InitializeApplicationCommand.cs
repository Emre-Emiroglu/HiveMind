using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Application;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Application;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Application
{
    public class InitializeApplicationCommand : Command<InitializeApplicationSignal>
    {
        #region ReadonlyFields
        private readonly ApplicationModel _applicationModel;
        #endregion

        #region Constructor
        public InitializeApplicationCommand(ApplicationModel applicationModel) => _applicationModel = applicationModel;
        #endregion

        #region Executes
        public override void Execute(InitializeApplicationSignal signal)
        {
            UnityEngine.Application.targetFrameRate = _applicationModel.GetSettings.TargetFrameRate;
            UnityEngine.Application.runInBackground = _applicationModel.GetSettings.RunInBackground;
        }
        #endregion
    }
}
