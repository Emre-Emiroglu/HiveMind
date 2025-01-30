using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Game;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Game
{
    public sealed class GameSignalInstaller : Installer
    {
        #region Bindings
        public override void InstallBindings()
        {
            Container.DeclareSignal<InitializeGameSignal>();
            Container.DeclareSignal<PlayGameSignal>();
            Container.DeclareSignal<GameOverSignal>();
            Container.DeclareSignal<GameExitSignal>();
            Container.DeclareSignal<SetupGameOverPanelSignal>();

            Container.BindInterfacesAndSelfTo<InitializeGameCommand>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayGameCommand>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameOverCommand>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameExitCommand>().AsSingle().NonLazy();

            Container.BindSignal<InitializeGameSignal>().ToMethod<InitializeGameCommand>((x, s) => x.Execute(s))
                .FromResolve();
            Container.BindSignal<PlayGameSignal>().ToMethod<PlayGameCommand>((x, s) => x.Execute(s)).FromResolve();
            Container.BindSignal<GameOverSignal>().ToMethod<GameOverCommand>((x, s) => x.Execute(s)).FromResolve();
            Container.BindSignal<GameExitSignal>().ToMethod<GameExitCommand>((x, s) => x.Execute(s)).FromResolve();
        }
        #endregion
    }
}
