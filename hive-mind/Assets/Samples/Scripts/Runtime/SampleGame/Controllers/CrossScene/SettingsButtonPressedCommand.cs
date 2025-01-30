using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.CrossScene
{
    public sealed class SettingsButtonPressedCommand : Command<SettingsButtonPressedSignal>
    {
        #region ReadonlyFields
        private readonly SignalBus _signalBus;
        private readonly AudioModel _audioModel;
        private readonly HapticModel _hapticModel;
        #endregion

        #region Constructor
        public SettingsButtonPressedCommand(SignalBus signalBus, AudioModel audioModel, HapticModel hapticModel)
        {
            _signalBus = signalBus;
            _audioModel = audioModel;
            _hapticModel = hapticModel;
        }
        #endregion

        #region Executes
        public override void Execute(SettingsButtonPressedSignal signal)
        {
            switch (signal.SettingsType)
            {
                case SettingsTypes.Music:
                    _audioModel.SetMusic(!_audioModel.IsMusicMuted);
                    break;
                case SettingsTypes.Sound:
                    _audioModel.SetSound(!_audioModel.IsSoundMuted);
                    break;
                case SettingsTypes.Haptic:
                    _hapticModel.SetHaptic(!_hapticModel.IsHapticMuted);
                    break;
            }

            _signalBus.Fire(new SettingsButtonRefreshSignal(signal.SettingsType));
        }
        #endregion
    }
}
