using HiveMind.Core.MVC.Interfaces;
using System;
using System.Collections.Generic;
using Zenject;

namespace HiveMind.Core.MVC.Controllers
{
    public sealed class CommandInvoker
    {
        #region Fields
        private readonly CommandPool commandPool;
        private readonly Dictionary<Type, CommandDeclaration> commandDeclarationMap;
        #endregion

        #region Constructor
        public CommandInvoker([Inject(Source = InjectSources.Local)]List<CommandDeclaration> commandDeclarations, CommandPool commandPool)
        {
            this.commandPool = commandPool;
            this.commandDeclarationMap = new();

            commandDeclarations.ForEach(x => commandDeclarationMap[x.SignalType] = x);
        }
        #endregion

        #region SignalReceiver
        public void OnSignalFired<TSignal>(TSignal signal)
        {
            Type type = typeof(TSignal);
            if (commandDeclarationMap.ContainsKey(type))
            {
                Type commandType = commandDeclarationMap[type].CommandType;
                ICommand command = commandPool.GetCommand(commandType, signal);

                command.Execute();
            }
        }
        #endregion
    }
}
