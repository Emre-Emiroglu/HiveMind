using HiveMind.MVC.Attributes;
using HiveMind.MVC.Controllers;
using HiveMind.MVC.Datas;
using Zenject;

namespace HiveMind.MVC.Binders
{
    public sealed class CommandBinder : Binder<CommandInjectAttribute>
    {
        #region Constructor
        public CommandBinder(BinderData binderData) : base(binderData) { }
        #endregion

        #region Bindings
        public override void Bind()
        {
            base.Bind();

            binderData.Container.Bind<CommandInvoker>().AsSingle().CopyIntoAllSubContainers();
            binderData.Container.Bind<CommandPool>().AsSingle().CopyIntoAllSubContainers();
        }
        public BinderSignalToCommand<TSignal> BindSignal<TSignal>()
        {
            var signalBindInfo = new SignalBindingBindInfo(typeof(TSignal));

            return new BinderSignalToCommand<TSignal>(binderData.Container, signalBindInfo);
        }
        #endregion
    }
}
