using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Bootstrap;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Bootstrap;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Bootstrap
{
    public sealed class BootstrapSignalInstaller : Installer
    {
        #region Bindings
        public override void InstallBindings()
        {
            Container.DeclareSignal<InitializeBootstrapSignal>();

            Container.BindInterfacesAndSelfTo<InitializeBootstrapCommand>().AsSingle().NonLazy();

            Container.BindSignal<InitializeBootstrapSignal>()
                .ToMethod<InitializeBootstrapCommand>((x, s) => x.Execute(s)).FromResolve();
        }
        #endregion
    }
}
