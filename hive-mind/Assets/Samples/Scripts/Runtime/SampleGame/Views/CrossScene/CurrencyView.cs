using System;
using System.Collections.Generic;
using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using RotaryHeart.Lib.SerializableDictionary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene
{
    public sealed class CurrencyView : View
    {
        #region Fields
        [Header("Currency View Fields")]
        [SerializeField] private CurrencyTextsMap currencyTexts;
        [SerializeField] private CurrencyButtonsMap currencyButtons;
        #endregion

        #region Getters
        public Dictionary<CurrencyTypes, TextMeshProUGUI> CurrencyTexts => currencyTexts.Clone();
        public Dictionary<CurrencyTypes, Button> CurrencyButtons => currencyButtons.Clone();
        #endregion
    }
    
    [Serializable]
    public sealed class CurrencyTextsMap : SerializableDictionaryBase<CurrencyTypes, TextMeshProUGUI> { }
    [Serializable]
    public sealed class CurrencyButtonsMap : SerializableDictionaryBase<CurrencyTypes, Button> { }
}