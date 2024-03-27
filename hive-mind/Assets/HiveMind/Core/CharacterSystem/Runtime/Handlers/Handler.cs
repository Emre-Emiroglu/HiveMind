using System;

namespace HiveMind.CharacterSystem.Runtime.Handlers
{
    public abstract class Handler: IDisposable
    {
        #region Fields
        protected bool isEnable;
        #endregion

        #region Constructor
        public Handler() => SetEnableStatus(false);
        public virtual void Dispose() => SetEnableStatus(false);
        #endregion

        #region Executes
        public virtual void SetEnableStatus(bool isEnable) => this.isEnable = isEnable;
        #endregion
    }
}
