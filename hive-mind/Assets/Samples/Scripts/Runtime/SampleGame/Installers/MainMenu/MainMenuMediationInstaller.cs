using CodeCatGames.HiveMind.Core.Runtime.MVC.Installers;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.MainMenu;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.MainMenu
{
    public sealed class MainMenuMediationInstaller : Installer
    {
        #region Bindings
        public override void InstallBindings()
        {
            Container.Bind<StartPanelView>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<ShopPanelView>().FromComponentInHierarchy().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<StartPanelMediator>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ShopPanelMediator>().AsSingle().NonLazy();

            Container.Install<ViewMediatorInstaller<CurrencyView, CurrencyMediator>>();
            Container.Install<ViewMediatorInstaller<SettingsButtonView, SettingsButtonMediator>>();
        }
        #endregion
    }
}
