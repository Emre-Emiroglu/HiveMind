using System;

namespace HiveMind.Core.CharacterSystem.Runtime.Handlers
{
    public abstract class Handler : HandlerLogic
    {
        #region Constructor
        public Handler() : base() { }
        #endregion

        #region Dispose
        public override void Dispose() => base.Dispose();
        #endregion

        #region Set
        public override void SetEnableStatus(bool isEnable) => base.SetEnableStatus(isEnable);
        #endregion

        #region Executes
        public virtual void Execute()
        {
            if (!isEnable)
                return;

            ExecuteProcess();
        }
        protected abstract void ExecuteProcess();
        #endregion
    }

    public abstract class Handler<TParam1> : HandlerLogic
    {
        #region Constructor
        public Handler() : base() { }
        #endregion

        #region Dispose
        public override void Dispose() => base.Dispose();
        #endregion

        #region Set
        public override void SetEnableStatus(bool isEnable) => base.SetEnableStatus(isEnable);
        #endregion

        #region Executes
        public virtual void Execute(TParam1 param1)
        {
            if (!isEnable)
                return;

            ExecuteProcess(param1);
        }
        protected abstract void ExecuteProcess(TParam1 param1);
        #endregion
    }

    public abstract class Handler<TParam1, TParam2> : HandlerLogic
    {
        #region Constructor
        public Handler() : base() { }
        #endregion

        #region Dispose
        public override void Dispose() => base.Dispose();
        #endregion

        #region Set
        public override void SetEnableStatus(bool isEnable) => base.SetEnableStatus(isEnable);
        #endregion

        #region Executes
        public virtual void Execute(TParam1 param1, TParam2 param2)
        {
            if (!isEnable)
                return;

            ExecuteProcess(param1, param2);
        }
        protected abstract void ExecuteProcess(TParam1 param1, TParam2 param2);
        #endregion
    }

    public abstract class Handler<TParam1, TParam2, TParam3> : HandlerLogic
    {
        #region Constructor
        public Handler() : base() { }
        #endregion

        #region Dispose
        public override void Dispose() => base.Dispose();
        #endregion

        #region Set
        public override void SetEnableStatus(bool isEnable) => base.SetEnableStatus(isEnable);
        #endregion

        #region Executes
        public virtual void Execute(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            if (!isEnable)
                return;

            ExecuteProcess(param1, param2, param3);
        }
        protected abstract void ExecuteProcess(TParam1 param1, TParam2 param2, TParam3 param3);
        #endregion
    }

    public abstract class Handler<TParam1, TParam2, TParam3, TParam4> : HandlerLogic
    {
        #region Constructor
        public Handler() : base() { }
        #endregion

        #region Dispose
        public override void Dispose() => base.Dispose();
        #endregion

        #region Set
        public override void SetEnableStatus(bool isEnable) => base.SetEnableStatus(isEnable);
        #endregion

        #region Executes
        public virtual void Execute(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            if (!isEnable)
                return;

            ExecuteProcess(param1, param2, param3, param4);
        }
        protected abstract void ExecuteProcess(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
        #endregion
    }

    public abstract class Handler<TParam1, TParam2, TParam3, TParam4, TParam5> : HandlerLogic
    {
        #region Constructor
        public Handler() : base() { }
        #endregion

        #region Dispose
        public override void Dispose() => base.Dispose();
        #endregion

        #region Set
        public override void SetEnableStatus(bool isEnable) => base.SetEnableStatus(isEnable);
        #endregion

        #region Executes
        public virtual void Execute(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            if (!isEnable)
                return;

            ExecuteProcess(param1, param2, param3, param4, param5);
        }
        protected abstract void ExecuteProcess(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5);
        #endregion
    }

    public abstract class HandlerLogic : IDisposable
    {
        #region Fields
        protected bool isEnable;
        #endregion

        #region Constructor
        public HandlerLogic() => isEnable = false;
        #endregion

        #region Dispose
        public virtual void Dispose() => isEnable = false;
        #endregion

        #region Set
        public virtual void SetEnableStatus(bool isEnable) => this.isEnable = isEnable;
        #endregion
    }
}
