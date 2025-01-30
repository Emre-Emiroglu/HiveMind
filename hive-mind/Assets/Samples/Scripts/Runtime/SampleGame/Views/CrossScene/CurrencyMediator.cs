using System.Linq;
using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Core.Runtime.Utilities.TextFormatter;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
using Lofelt.NiceVibrations;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene
{
     public sealed class CurrencyMediator : Mediator<CurrencyView>
    {
        #region ReadonlyFields
        private readonly SignalBus _signalBus;
        private readonly CurrencyModel _currencyModel;
        #endregion

        #region Constructor
        public CurrencyMediator(CurrencyView view, SignalBus signalBus, CurrencyModel currencyModel) : base(view)
        {
            _signalBus = signalBus;
            _currencyModel = currencyModel;
        }
        #endregion

        #region PostConstruct
        public override void PostConstruct() { }
        #endregion

        #region Core
        public override void Initialize()
        {
            RefreshAllCurrencyVisual();

            _signalBus.Subscribe<RefreshCurrencyVisualSignal>(OnRefreshCurrencyVisualSignal);

            GetView.CurrencyButtons.Values.ToList().ForEach(x => x.onClick.AddListener(ButtonClicked));
        }
        public override void Dispose()
        {
            _signalBus.Unsubscribe<RefreshCurrencyVisualSignal>(OnRefreshCurrencyVisualSignal);
            
            GetView.CurrencyButtons.Values.ToList().ForEach(x => x.onClick.RemoveAllListeners());
        }
        #endregion

        #region SignalReceivers
        private void OnRefreshCurrencyVisualSignal(RefreshCurrencyVisualSignal signal) =>
            RefreshCurrencyVisual(signal.CurrencyType);
        #endregion

        #region ButtonReceivers
        private void ButtonClicked()
        {
            _signalBus.Fire(new ChangeUIPanelSignal(UIPanelTypes.ShopPanel));
            _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
            _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.LightImpact));
        }
        #endregion

        #region Executes
        private void RefreshAllCurrencyVisual() =>
            _currencyModel.CurrencyValues.Keys.ToList().ForEach(RefreshCurrencyVisual);
        private void RefreshCurrencyVisual(CurrencyTypes currencyType)
        {
            int value = _currencyModel.CurrencyValues[currencyType];

            GetView.CurrencyTexts[currencyType].SetText(TextFormatter.FormatNumber(value));
        }
        #endregion
    }
}