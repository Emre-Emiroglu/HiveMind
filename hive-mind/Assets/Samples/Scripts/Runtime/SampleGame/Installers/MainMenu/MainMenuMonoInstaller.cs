using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.MainMenu;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.MainMenu
{
    public sealed class MainMenuMonoInstaller : MonoInstaller
    {
        #region Bindings
        public override void InstallBindings()
        {
            Container.Install<MainMenuModelInstaller>();
            Container.Install<MainMenuMediationInstaller>();
            Container.Install<MainMenuSignalInstaller>();
        }
        #endregion

        #region Cycle
        public override void Start() => Container.Resolve<SignalBus>().Fire(new InitializeMainMenuSignal());
        #endregion
    }
}
