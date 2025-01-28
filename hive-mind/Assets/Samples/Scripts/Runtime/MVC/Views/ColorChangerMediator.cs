using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.MVC.Model;
using CodeCatGames.HiveMind.Samples.Runtime.MVC.Signals;
using TMPro;
using UnityEngine.UI;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Views
{
    public class ColorChangerMediator : Mediator<ColorChangerView>
    {
        #region ReadonlyFields
        private readonly SignalBus _signalBus;
        private readonly MvcSampleModel _mvcSampleModel;
        #endregion
        
        #region Constructor
        public ColorChangerMediator(ColorChangerView view, SignalBus signalBus, MvcSampleModel mvcSampleModel) : base(view)
        {
            _signalBus = signalBus;
            _mvcSampleModel = mvcSampleModel;
        }
        #endregion
        
        #region PostConstruct
        public override void PostConstruct() { }
        #endregion
        
        #region Core
        public override void Initialize()
        {
            GetView.ClickChangeColorButton += OnChangeColorButtonClicked;
            
            SetVisual();
        }
        public override void Dispose() => GetView.ClickChangeColorButton -= OnChangeColorButtonClicked;
        #endregion

        #region ButtonReceivers
        private void OnChangeColorButtonClicked(int index) =>
            _signalBus.Fire(new ChangeColorSignal(_mvcSampleModel.GetSettings.Colors[index]));
        #endregion

        #region Executes
        private void SetVisual()
        {
            Button[] buttons = GetView.ChangeColorButtons;
            TextMeshProUGUI[] texts = GetView.ChangeColorButtonTexts;
            
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].targetGraphic.color = _mvcSampleModel.GetSettings.Colors[i];
            
            for (int i = 0; i < texts.Length; i++)
                texts[i].SetText($"{_mvcSampleModel.GetSettings.ColorNames[i]}");
        }
        #endregion
    }
}