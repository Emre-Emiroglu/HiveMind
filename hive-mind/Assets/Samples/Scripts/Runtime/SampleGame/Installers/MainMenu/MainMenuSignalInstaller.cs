using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.MainMenu;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.MainMenu;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.MainMenu
{
    public sealed class MainMenuSignalInstaller : Installer
    {
        #region Bindings
        public override void InstallBindings()
        {
            Container.DeclareSignal<InitializeMainMenuSignal>();

            Container.BindInterfacesAndSelfTo<InitializeMainMenuCommand>().AsSingle().NonLazy();

            Container.BindSignal<InitializeMainMenuSignal>().ToMethod<InitializeMainMenuCommand>((x, s) => x.Execute(s))
                .FromResolve();
        }
        #endregion
    }
}
