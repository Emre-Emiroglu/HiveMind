using HiveMind.MVC.Binders;
using HiveMind.MVC.Datas;
using Zenject;

namespace HiveMind.MVC.Installers
{
    public class MVCInstaller : Installer<object, MVCInstaller>
    {
        #region Fields
        protected readonly BinderData binderData;
        protected readonly ModelBinder modelBinder;
        protected readonly MediationBinder mediiationBinder;
        protected readonly CommandBinder commandBinder;
        #endregion

        #region Constructor
        public MVCInstaller(object key)
        {
            binderData = new(Container, key);
            modelBinder = new(binderData);
            mediiationBinder = new(binderData);
            commandBinder = new(binderData);
        }
        #endregion

        #region Bindings
        public override void InstallBindings()
        {
            Container.BindInstance(modelBinder).AsSingle().NonLazy();
            Container.BindInstance(mediiationBinder).AsSingle().NonLazy();
            Container.BindInstance(commandBinder).AsSingle().NonLazy();

            modelBinder.Bind();
            mediiationBinder.Bind();
            commandBinder.Bind();
        }
        #endregion
    }
}
