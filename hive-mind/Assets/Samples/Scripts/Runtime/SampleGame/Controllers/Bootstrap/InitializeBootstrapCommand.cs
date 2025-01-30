using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Bootstrap;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Bootstrap;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Bootstrap
{
    public sealed class InitializeBootstrapCommand : Command<InitializeBootstrapSignal>
    {
        #region ReadonlyFields
        private readonly SignalBus _signalBus;
        private readonly BootstrapModel _bootstrapModel;
        #endregion

        #region Constructor
        public InitializeBootstrapCommand(SignalBus signalBus, BootstrapModel bootstrapModel)
        {
            _signalBus = signalBus;
            _bootstrapModel = bootstrapModel;
        }
        #endregion

        #region Executes
        // ReSharper disable once AsyncVoidMethod
        public override async void Execute(InitializeBootstrapSignal signal)
        {
            int millisecondsDelay = (int)(_bootstrapModel.GetSettings.SceneActivationDuration * 1000f);
            
            await UniTask.Delay(millisecondsDelay);

            _signalBus.Fire(new LoadSceneSignal(SceneID.MainMenu));
        }
        #endregion
    }
}
