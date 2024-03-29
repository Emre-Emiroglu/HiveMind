using System;
using Zenject;

namespace HiveMind.Core.MVC.Controllers
{
    public sealed class CommandSignalCallbackWrapper : IDisposable
    {
        #region Fields
        private readonly SignalBus signalBus;
        private readonly CommandInvoker commandInvoker;
        private readonly Type signalType;
        private readonly object identifier;
        #endregion

        #region Constructor
        public CommandSignalCallbackWrapper(SignalBindingBindInfo bindInfo, CommandInvoker commandInvoker, SignalBus signalBus)
        {
            signalType = bindInfo.SignalType;
            identifier = bindInfo.Identifier;
            this.signalBus = signalBus;
            this.commandInvoker = commandInvoker;

            signalBus.SubscribeId(bindInfo.SignalType, identifier, OnSignalFired);
        }
        #endregion

        #region SignalReceiver
        void OnSignalFired(object signal) => commandInvoker.OnSignalFired(signal);
        #endregion

        #region Dispose
        public void Dispose() => signalBus.UnsubscribeId(signalType, identifier, OnSignalFired);
        #endregion
    }
}
