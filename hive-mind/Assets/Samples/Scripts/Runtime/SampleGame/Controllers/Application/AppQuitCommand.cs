using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Application;
using UnityEditor;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Application
{
    public sealed class AppQuitCommand : Command<AppQuitSignal>
    {
        #region ReadonlyFields
        private readonly AudioModel _audioModel;
        private readonly HapticModel _hapticModel;
        private readonly CurrencyModel _currencyModel;
        private readonly LevelModel _levelModel;
        #endregion

        #region Constructor
        public AppQuitCommand(AudioModel audioModel, HapticModel hapticModel, CurrencyModel currencyModel, LevelModel levelModel)
        {
            _audioModel = audioModel;
            _hapticModel = hapticModel;
            _currencyModel = currencyModel;
            _levelModel = levelModel;
        }
        #endregion

        #region Executes
        public override void Execute(AppQuitSignal signal)
        {
            _audioModel.Save();
            _hapticModel.Save();
            _currencyModel.Save();
            _levelModel.Save();

#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            UnityEngine.Application.Quit();
#endif
        }
        #endregion
    }
}
