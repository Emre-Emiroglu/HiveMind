using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.CrossScene
{
    public sealed class CrossSceneSignalInstaller: Installer
    {
        #region Bindings
        public override void InstallBindings()
        {
            Container.DeclareSignal<ChangeLoadingScreenActivationSignal>();
            Container.DeclareSignal<LoadSceneSignal>();
            Container.DeclareSignal<PlayAudioSignal>();
            Container.DeclareSignal<PlayHapticSignal>();
            Container.DeclareSignal<ChangeCurrencySignal>();
            Container.DeclareSignal<SpawnCurrencyTrailSignal>();
            Container.DeclareSignal<RefreshCurrencyVisualSignal>();
            Container.DeclareSignal<SettingsButtonPressedSignal>();
            Container.DeclareSignal<SettingsButtonRefreshSignal>();
            Container.DeclareSignal<ChangeUIPanelSignal>();

            Container.BindInterfacesAndSelfTo<ChangeCurrencyCommand>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LoadSceneCommand>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayHapticCommand>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SettingsButtonPressedCommand>().AsSingle().NonLazy();

            Container.BindSignal<ChangeCurrencySignal>().ToMethod<ChangeCurrencyCommand>((x, s) => x.Execute(s))
                .FromResolve();
            Container.BindSignal<LoadSceneSignal>().ToMethod<LoadSceneCommand>((x, s) => x.Execute(s)).FromResolve();
            Container.BindSignal<PlayHapticSignal>().ToMethod<PlayHapticCommand>((x, s) => x.Execute(s)).FromResolve();
            Container.BindSignal<SettingsButtonPressedSignal>()
                .ToMethod<SettingsButtonPressedCommand>((x, s) => x.Execute(s)).FromResolve();
        }
        #endregion
    }
}