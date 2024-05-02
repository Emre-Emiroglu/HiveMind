using HiveMind.Core.MVC.Attributes;
using HiveMind.Core.MVC.Controllers;
using HiveMind.Core.MVC.Datas;
using Zenject;

namespace HiveMind.Core.MVC.Binders
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
