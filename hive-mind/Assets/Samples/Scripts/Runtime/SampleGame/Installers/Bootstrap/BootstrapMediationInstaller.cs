using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Bootstrap;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Bootstrap
{
    public sealed class BootstrapMediationInstaller : Installer
    {
        #region Bindings
        public override void InstallBindings()
        {
            Container.Bind<LogoHolderPanelView>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LogoHolderPanelMediator>().AsSingle().NonLazy();
        }
        #endregion
    }
}
