using HiveMind.MVC.Binders;
using HiveMind.MVC.Datas;
using Zenject;

namespace HiveMind.MVC.Installers
{
    public class MVCInstaller<TSubInstaller>: MVCInstallerBase<BinderData, MVCInstaller<TSubInstaller>, TSubInstaller>
    {
        #region Fields
        protected readonly BinderData binderData;
        protected readonly ModelBinder modelBinder;
        protected readonly MediationBinder mediationBinder;
        protected readonly CommandBinder commandBinder;
        #endregion

        #region Constructor
        public MVCInstaller(BinderData binderData)
        {
            this.binderData = binderData;
            modelBinder = new(binderData);
            mediationBinder = new(binderData);
            commandBinder = new(binderData);
        }
        #endregion

        #region Bindings
        public override void InstallBindings()
        {
            DiContainer container = binderData.Container;

            container.BindInstance(modelBinder).AsSingle().NonLazy();
            container.BindInstance(mediationBinder).AsSingle().NonLazy();
            container.BindInstance(commandBinder).AsSingle().NonLazy();

            modelBinder.Bind();
            mediationBinder.Bind();
            commandBinder.Bind();
        }
        #endregion
    }
}
