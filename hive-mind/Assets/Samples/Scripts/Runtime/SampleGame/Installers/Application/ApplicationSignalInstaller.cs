using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Application;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Application;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Application
{
    public sealed class ApplicationSignalInstaller : Installer
    {
        #region Bindings
        public override void InstallBindings()
        {
            Container.DeclareSignal<InitializeApplicationSignal>();
            Container.DeclareSignal<AppQuitSignal>();

            Container.BindInterfacesAndSelfTo<InitializeApplicationCommand>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AppQuitCommand>().AsSingle().NonLazy();

            Container.BindSignal<InitializeApplicationSignal>()
                .ToMethod<InitializeApplicationCommand>((x, s) => x.Execute(s)).FromResolve();
            Container.BindSignal<AppQuitSignal>().ToMethod<AppQuitCommand>((x, s) => x.Execute(s)).FromResolve();
        }
        #endregion
    }
}
