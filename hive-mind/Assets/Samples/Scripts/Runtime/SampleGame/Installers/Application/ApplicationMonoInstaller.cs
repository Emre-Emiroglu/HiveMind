using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Application;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Application
{
    public sealed class ApplicationMonoInstaller : MonoInstaller
    {
        #region Bindings
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.Install<ApplicationModelInstaller>();
            Container.Install<ApplicationSignalInstaller>();
        }
        #endregion

        #region Cycle
        public override void Start() => Container.Resolve<SignalBus>().Fire(new InitializeApplicationSignal());
        #endregion
    }
}
