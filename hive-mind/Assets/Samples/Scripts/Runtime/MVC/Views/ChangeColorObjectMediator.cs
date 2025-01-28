using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.MVC.Signals;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Views
{
    public sealed class ChangeColorObjectMediator : Mediator<ChangeColorObjectView>
    {
        #region ReadonlyFields
        private readonly SignalBus _signalBus;
        #endregion
        
        #region Constructor
        public ChangeColorObjectMediator(ChangeColorObjectView view, SignalBus signalBus) : base(view) => _signalBus = signalBus;
        #endregion

        #region PostConstruct
        public override void PostConstruct() { }
        #endregion
        
        #region Core
        public override void Initialize() => _signalBus.Subscribe<ChangeColorSignal>(OnChangeColorSignal);
        public override void Dispose() => _signalBus.Unsubscribe<ChangeColorSignal>(OnChangeColorSignal);
        #endregion

        #region SignalReceivers
        private void OnChangeColorSignal(ChangeColorSignal signal) => GetView.MeshRenderer.material.color = signal.Color;
        #endregion
    }
}