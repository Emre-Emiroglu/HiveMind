using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Bootstrap;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Bootstrap
{
    public sealed class BootstrapModelInstaller : Installer
    {
        #region Bindings
        public override void InstallBindings() =>
            Container.BindInterfacesAndSelfTo<BootstrapModel>().AsSingle().NonLazy();
        #endregion
    }
}
