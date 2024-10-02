using System;
using Zenject;

namespace HiveMind.Core.MVC.Runtime.View
{
    public abstract class Mediator<TView> : IInitializable, IDisposable
        where TView : View
    {
        #region Fields
        protected readonly TView _view;
        #endregion

        #region Getters
        public TView View => _view;
        #endregion

        #region Constructor
        public Mediator(TView view) => _view = view;
        #endregion

        #region PostConstruct
        [Inject] public abstract void PostConstruct();
        #endregion

        #region Core
        public abstract void Initialize();
        public abstract void Dispose();
        #endregion
    }
}
