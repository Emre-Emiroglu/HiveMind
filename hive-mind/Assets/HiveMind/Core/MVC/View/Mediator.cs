using Zenject;

namespace HiveMind.MVC.View
{
    public abstract class Mediator<TView>
        where TView : View
    {
        #region Fields
        [Inject] protected readonly TView view;
        #endregion

        #region Constructor
        public Mediator() { }
        #endregion

        #region PostConstruct
        [Inject] public abstract void PostConstruct();
        #endregion
    }
}
