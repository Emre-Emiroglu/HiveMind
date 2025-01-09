namespace CodeCatGames.HiveMind.Core.Runtime.MVC.Controller
{
    public abstract class Command<TSignal>
    where TSignal : struct
    {
        #region Core
        public abstract void Execute(TSignal signal);
        #endregion
    }
}
