using HiveMind.Core.MVC.Runtime.Interfaces;

namespace HiveMind.Core.MVC.Runtime.Controllers
{
    public abstract class Command : ICommand
    {
        #region Core
        public abstract void Execute();
        #endregion
    }
}
