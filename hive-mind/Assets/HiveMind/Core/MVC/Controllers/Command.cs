using HiveMind.MVC.Interfaces;

namespace HiveMind.MVC.Controllers
{
    public abstract class Command : ICommand
    {
        #region Core
        public abstract void Execute();
        #endregion
    }
}
