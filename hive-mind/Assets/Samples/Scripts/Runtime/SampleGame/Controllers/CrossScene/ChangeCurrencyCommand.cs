using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.CrossScene
{
    public sealed class ChangeCurrencyCommand : Command<ChangeCurrencySignal>
    {
        #region ReadonlyFields
        private readonly SignalBus _signalBus;
        private readonly CurrencyModel _currencyModel;
        #endregion

        #region Constructor
        public ChangeCurrencyCommand(SignalBus signalBus, CurrencyModel currencyModel)
        {
            _signalBus = signalBus;
            _currencyModel = currencyModel;
        }
        #endregion

        #region Executes
        public override void Execute(ChangeCurrencySignal signal)
        {
            _currencyModel.ChangeCurrencyValue(signal.CurrencyType, signal.Amount, signal.IsSet);

            _signalBus.Fire(new RefreshCurrencyVisualSignal(signal.CurrencyType));
        }
        #endregion
    }
}
