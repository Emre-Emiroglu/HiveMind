using System;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Handlers.CrossScene
{
    public abstract class SpawnHandler<TModel, TFactory> : IDisposable
         where TModel : class
         where TFactory : IPlaceholderFactory
    {
        #region ReadonlyFields
        protected readonly SignalBus SignalBus;
        protected readonly TModel Model;
        protected readonly TFactory Factory;
        #endregion

        #region Constructor
        public SpawnHandler(SignalBus signalBus, TModel model, TFactory factory)
        {
            SignalBus = signalBus;
            Model = model;
            Factory = factory;
        }
        #endregion

        #region Dispose
        public abstract void Dispose();
        #endregion

        #region Subscriptions
        protected abstract void SetSubscriptions(bool isSub);
        #endregion
    }
}
