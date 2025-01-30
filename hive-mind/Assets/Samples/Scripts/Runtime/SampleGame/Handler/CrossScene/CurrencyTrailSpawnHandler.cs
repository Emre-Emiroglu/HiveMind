﻿using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Factories;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Handler.CrossScene
{
    public sealed class CurrencyTrailSpawnHandler : SpawnHandler<CurrencyModel, CurrencyTrailFactory>
    {
        #region Constructor
        public CurrencyTrailSpawnHandler(SignalBus signalBus, CurrencyModel model, CurrencyTrailFactory factory) :
            base(signalBus, model, factory) => SetSubscriptions(true);
        #endregion

        #region Dispose
        public override void Dispose() => SetSubscriptions(false);
        #endregion

        #region Subscriptions
        protected override void SetSubscriptions(bool isSub)
        {
            if (isSub)
                SignalBus.Subscribe<SpawnCurrencyTrailSignal>(OnSpawnCurrencyTrailSignal);
            else
                SignalBus.Unsubscribe<SpawnCurrencyTrailSignal>(OnSpawnCurrencyTrailSignal);
        }
        #endregion

        #region SignalReceivers
        private void OnSpawnCurrencyTrailSignal(SpawnCurrencyTrailSignal signal) => SpawnCurrencyTrailProcess(signal);
        #endregion

        #region Executes
        private void SpawnCurrencyTrailProcess(SpawnCurrencyTrailSignal signal) =>
            Factory.Create(signal.CurrencyTrailData);
        #endregion
    }
}