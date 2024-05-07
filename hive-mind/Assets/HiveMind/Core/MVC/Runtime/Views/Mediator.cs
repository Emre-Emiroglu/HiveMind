using Zenject;

namespace HiveMind.Core.MVC.Runtime.Views
{
    public abstract class Mediator<TView> where TView : View
    {
        #region Fields
        protected readonly TView view;
        #endregion

        #region Getters
        public TView View => view;
        #endregion

        #region Constructor
        public Mediator(TView view) => this.view = view;
        #endregion

        #region PostConstruct
        [Inject] public abstract void PostConstruct();
        #endregion
    }
}
