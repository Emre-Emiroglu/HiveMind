using System;
using System.Collections.Generic;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene
{
    [CreateAssetMenu(fileName = "CurrencySettings", menuName = "CodeCatGames/HiveMind/Samples/SampleGame/CrossScene/CurrencySettings")]
    public sealed class CurrencySettings : ScriptableObject
    {
        #region Fields
        [Header("Currency Settings Fields")]
        [SerializeField] private DefaultCurrencyValuesMap defaultCurrencyValues;
        [SerializeField] private CurrencyIconsMap currencyIcons;
        #endregion

        #region Getters
        public Dictionary<CurrencyTypes, int> DefaultCurrencyValues => defaultCurrencyValues.Clone();
        public Dictionary<CurrencyTypes, Sprite> CurrencyIcons => currencyIcons.Clone();
        #endregion
    }

    [Serializable]
    public sealed class DefaultCurrencyValuesMap : SerializableDictionaryBase<CurrencyTypes, int> { }
    [Serializable]
    public sealed class CurrencyIconsMap : SerializableDictionaryBase<CurrencyTypes, Sprite> { }
}
