using System;
using Zenject;

namespace HiveMind.Core.MVC.Controllers
{
    public sealed class CommandDeclaration
    {
        #region Fields
        private readonly BindingId bindingId;
        private readonly Type commandType;
        private readonly Type signalType;
        #endregion

        #region Getters
        public BindingId BindingId => bindingId;
        public Type CommandType => commandType;
        public Type SignalType => signalType;
        #endregion

        #region Constructor
        public CommandDeclaration(CommandDeclarationBindInfo commandDeclarationBindInfo)
        {
            bindingId = new BindingId(commandDeclarationBindInfo.CommandType, commandDeclarationBindInfo.Identifier);
            commandType = commandDeclarationBindInfo.CommandType;
            signalType = commandDeclarationBindInfo.SignalType;
        }
        #endregion
    }

    [NoReflectionBaking]
    public sealed class CommandDeclarationBindInfo
    {
        #region Fields
        private readonly Type commandType;
        private readonly Type signalType;
        #endregion

        #region Getters
        public Type CommandType => commandType;
        public Type SignalType => signalType;
        #endregion

        #region Props
        public object Identifier { get; set; }
        #endregion

        #region Constructor
        public CommandDeclarationBindInfo(Type commandType, Type signalType)
        {
            this.commandType = commandType;
            this.signalType = signalType;
        }
        #endregion
    }
}
