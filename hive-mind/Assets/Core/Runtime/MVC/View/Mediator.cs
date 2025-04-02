using System;
using VContainer;
using VContainer.Unity;

namespace CodeCatGames.HiveMind.Core.Runtime.MVC.View
{
    public abstract class Mediator<TView> : IInitializable, IDisposable
        where TView : View
    {
        #region Fields
        private readonly TView _view;
        #endregion

        #region Getters
        public TView GetView => _view;
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
