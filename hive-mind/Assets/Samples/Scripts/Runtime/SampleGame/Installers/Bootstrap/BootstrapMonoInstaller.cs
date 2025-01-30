using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Bootstrap;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Bootstrap
{
    public sealed class BootstrapMonoInstaller : MonoInstaller
    {
        #region Bindings
        public override void InstallBindings()
        {
            Container.Install<BootstrapModelInstaller>();
            Container.Install<BootstrapMediationInstaller>();
            Container.Install<BootstrapSignalInstaller>();
        }
        #endregion

        #region Cycle
        public override void Start() => Container.Resolve<SignalBus>().Fire(new InitializeBootstrapSignal());
        #endregion
    }
}
