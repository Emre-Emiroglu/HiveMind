using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Game;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Game
{
    public sealed class GameModelInstaller : Installer
    {
        #region Bindings
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<TutorialModel>().AsSingle().NonLazy();
        }
        #endregion
    }
}
