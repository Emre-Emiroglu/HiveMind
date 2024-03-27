using System;

namespace HiveMind.Core.CharacterSystem.Runtime.Handlers
{
    public abstract class Handler: IDisposable
    {
        #region Fields
        protected bool isEnable;
        #endregion

        #region Constructor
        public Handler() => isEnable = false;
        public virtual void Dispose() => isEnable = false;
        #endregion

        #region Executes
        public virtual void SetEnableStatus(bool isEnable) => this.isEnable = isEnable;
        #endregion
    }
}
