using CodeCatGames.HiveMind.Core.Runtime.MVC.Installers;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Game;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Game
{
    public sealed class GameMediationInstaller : Installer
    {
        #region Bindings
        public override void InstallBindings()
        {
            Container.Bind<GameOverPanelView>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<InGamePanelView>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<TutorialPanelView>().FromComponentInHierarchy().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<GameOverPanelMediator>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InGamePanelMediator>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<TutorialPanelMediator>().AsSingle().NonLazy();

            Container.Install<ViewMediatorInstaller<CurrencyView, CurrencyMediator>>();
            Container.Install<ViewMediatorInstaller<SettingsButtonView, SettingsButtonMediator>>();
        }
        #endregion
    }
}
