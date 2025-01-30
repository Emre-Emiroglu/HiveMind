using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.CrossScene
{
    public sealed class LoadSceneCommand : Command<LoadSceneSignal>
    {
        #region ReadonlyFields
        private readonly SignalBus _signalBus;
        #endregion

        #region Constructor
        public LoadSceneCommand(SignalBus signalBus) => _signalBus = signalBus;
        #endregion

        #region Executes
        public override void Execute(LoadSceneSignal signal)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync((int)signal.SceneId);

            _signalBus.Fire(new ChangeLoadingScreenActivationSignal(true, asyncOperation));
        }
        #endregion
    }
}
