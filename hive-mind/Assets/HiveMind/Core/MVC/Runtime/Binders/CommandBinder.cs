using HiveMind.Core.MVC.Runtime.Attributes;
using HiveMind.Core.MVC.Runtime.Controllers;
using HiveMind.Core.MVC.Runtime.Datas;
using Zenject;

namespace HiveMind.Core.MVC.Runtime.Binders
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

            binderData.Container.Bind<CommandInvoker>().AsSingle();
            binderData.Container.Bind<CommandPool>().AsSingle();
        }
        public BinderSignalToCommand<TSignal> BindSignal<TSignal>()
        {
            var signalBindInfo = new SignalBindingBindInfo(typeof(TSignal));

            return new BinderSignalToCommand<TSignal>(binderData.Container, signalBindInfo);
        }
        #endregion
    }
}
