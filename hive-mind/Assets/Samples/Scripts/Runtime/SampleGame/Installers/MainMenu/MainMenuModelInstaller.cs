using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.MainMenu;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.MainMenu
{
    public sealed class MainMenuModelInstaller : Installer
    {
        #region Bindings
        public override void InstallBindings() =>
            Container.BindInterfacesAndSelfTo<MainMenuModel>().AsSingle().NonLazy();
        #endregion
    }
}
