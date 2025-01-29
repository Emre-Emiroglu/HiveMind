using System;
using System.Collections.Generic;
using CodeCatGames.HiveMind.Core.Runtime.Utilities.SerializedDictionary;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene
{
    [CreateAssetMenu(fileName = "CurrencySettings", menuName = "CodeCatGames/HiveMind/Samples/SampleGame/CrossScene/CurrencySettings")]
    public class CurrencySettings : ScriptableObject
    {
        #region Fields
        [Header("Currency Settings Fields")]
        [SerializeField] private DefaultCurrencyValuesMap defaultCurrencyValues;
        [SerializeField] private CurrencyIconsMap currencyIcons;
        #endregion

        #region Getters
        public Dictionary<CurrencyTypes, int> DefaultCurrencyValues => defaultCurrencyValues;
        public Dictionary<CurrencyTypes, Sprite> CurrencyIcons => currencyIcons;
        #endregion
    }

    [Serializable]
    public class DefaultCurrencyValuesMap : SerializedDictionary<CurrencyTypes, int> { }
    [Serializable]
    public class CurrencyIconsMap : SerializedDictionary<CurrencyTypes, Sprite> { }
}
