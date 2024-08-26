namespace HiveMind.Core.MVC.Runtime.Controller
{
    public abstract class Command<TSignal>
    where TSignal : struct
    {
        #region Core
        public abstract void Execute(TSignal signal);
        #endregion
    }
}
