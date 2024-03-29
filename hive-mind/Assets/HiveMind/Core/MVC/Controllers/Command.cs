using HiveMind.Core.MVC.Interfaces;

namespace HiveMind.Core.MVC.Controllers
{
    public abstract class Command : ICommand
    {
        #region Core
        public abstract void Execute();
        #endregion
    }
}
