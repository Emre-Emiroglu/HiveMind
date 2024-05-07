using ModestTree;
using System;
using System.Reflection;
using Zenject;

namespace HiveMind.Core.MVC.Runtime.Controllers
{
    public sealed class BinderSignalToCommand<TSignal> : BindSignalToBinder<TSignal>
    {
        #region Constructor
        public BinderSignalToCommand(DiContainer container, SignalBindingBindInfo signalBindInfo) : base(container, signalBindInfo)
        {
        }
        #endregion

        #region Executes
        public SignalCopyBinder WithCommand<TCommand>()
        {
            GetContainerAndBindStatement<TCommand>(out BindStatement bindStatement, out DiContainer container);

            Assert.That(!bindStatement.HasFinalizer);
            bindStatement.SetFinalizer(new NullBindingFinalizer());

            DeclareSignal(container);
            BindCommand<TCommand>(container);

            var bindInfo = container.Bind<IDisposable>()
                                    .To<CommandSignalCallbackWrapper>()
                                    .AsCached()
                                    .WithArguments(SignalBindInfo)
                                    .NonLazy().BindInfo;

            return new SignalCopyBinder(bindInfo);
        }
        private static void DeclareSignal(DiContainer container)
        {
            container.DeclareSignal<TSignal>();
        }
        private static void BindCommand<TCommand>(DiContainer container)
        {
            CommandDeclarationBindInfo commandBindInfo = new(typeof(TCommand), typeof(TSignal));

            container.Bind<CommandDeclaration>().AsCached()
                     .WithArguments(commandBindInfo).WhenInjectedInto(typeof(CommandInvoker));
        }
        private void GetContainerAndBindStatement<TCommand>(out BindStatement bindStatement, out DiContainer container)
        {
            FieldInfo containerField = typeof(BinderSignalToCommand<TSignal>).BaseType
                                                                                 .GetField("_container", BindingFlags.Instance | BindingFlags.NonPublic);

            FieldInfo bindStatementField = typeof(BinderSignalToCommand<TSignal>).BaseType
                .GetField("_bindStatement", BindingFlags.Instance | BindingFlags.NonPublic);

            container = (DiContainer)containerField.GetValue(this);
            bindStatement = (BindStatement)bindStatementField.GetValue(this);
        }
        #endregion
    }
}
