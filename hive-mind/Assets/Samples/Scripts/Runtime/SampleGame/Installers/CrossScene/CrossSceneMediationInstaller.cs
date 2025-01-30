using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.CrossScene
{
    public sealed class CrossSceneMediationInstaller: Installer
    {
        #region Bindings
        public override void InstallBindings()
        {
            Container.Bind<LoadingScreenPanelView>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LoadingScreenPanelMediator>().AsSingle().NonLazy();

            Container.Bind<AudioView>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AudioMediator>().AsSingle().NonLazy();
        }
        #endregion
    }
}