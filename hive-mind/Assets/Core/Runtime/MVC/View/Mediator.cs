using System;
using Zenject;

namespace CodeCatGames.HiveMind.Core.Runtime.MVC.View
{
    public abstract class Mediator<TView> : IInitializable, IDisposable
        where TView : View
    {
        #region Fields
        protected readonly TView View;
        #endregion

        #region Getters
        public TView GetView => View;
        #endregion

        #region Constructor
        public Mediator(TView view) => View = view;
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
