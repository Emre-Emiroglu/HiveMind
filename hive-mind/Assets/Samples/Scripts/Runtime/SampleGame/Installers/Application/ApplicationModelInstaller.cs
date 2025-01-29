using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Application;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Application
{
    public class ApplicationModelInstaller : Installer
    {
        #region Bindings
        public override void InstallBindings() =>
            Container.BindInterfacesAndSelfTo<ApplicationModel>().AsSingle().NonLazy();
        #endregion
    }
}
