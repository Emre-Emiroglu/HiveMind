using System.Collections.Generic;
using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene
{
    public class CurrencyView : View
    {
        #region Fields
        [SerializeField] private Dictionary<CurrencyTypes, TextMeshProUGUI> _currencyTexts;
        [SerializeField] private Dictionary<CurrencyTypes, Button> _currencyButtons;
        #endregion

        #region Getters
        public Dictionary<CurrencyTypes, TextMeshProUGUI> CurrencyTexts => _currencyTexts;
        public Dictionary<CurrencyTypes, Button> CurrencyButtons => _currencyButtons;
        #endregion
    }
}