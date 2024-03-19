using HiveMind.MVC.Interfaces;

namespace HiveMind.MVC.Controller
{
    public abstract class Command : ICommand
    {
        #region Core
        public abstract void Execute();
        #endregion
    }
}
