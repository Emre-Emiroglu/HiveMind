using CodeCatGames.HiveMind.Core.Runtime.MVC.Installers;
using CodeCatGames.HiveMind.Samples.Runtime.MVC.Controllers;
using CodeCatGames.HiveMind.Samples.Runtime.MVC.Model;
using CodeCatGames.HiveMind.Samples.Runtime.MVC.Signals;
using CodeCatGames.HiveMind.Samples.Runtime.MVC.Views;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Installers
{
    public sealed class MvcSampleMonoInstaller : MonoInstaller
    {
        #region Bindings
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MvcSampleModel>().AsSingle().NonLazy();
            
            Container.Bind<ColorChangerView>().FromComponentsInHierarchy().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ColorChangerMediator>().AsSingle().NonLazy();

            Container.Install<ViewMediatorInstaller<ChangeColorObjectView, ChangeColorObjectMediator>>();

            Container.DeclareSignal<InitializeSignal>();
            Container.DeclareSignal<ChangeColorSignal>();

            Container.BindInterfacesAndSelfTo<InitializeCommand>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ChangeColorCommand>().AsSingle().NonLazy();

            Container.BindSignal<InitializeSignal>().ToMethod<InitializeCommand>((x, s) => x.Execute(s)).FromResolve();
            Container.BindSignal<ChangeColorSignal>().ToMethod<ChangeColorCommand>((x, s) => x.Execute(s))
                .FromResolve();
        }
        #endregion

        #region Cycle
        public override void Start() => Container.Resolve<SignalBus>().Fire(new InitializeSignal());
        #endregion
    }
}