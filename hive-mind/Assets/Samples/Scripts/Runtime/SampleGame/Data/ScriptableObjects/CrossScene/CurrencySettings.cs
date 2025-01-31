using System.Collections.Generic;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene
{
    [CreateAssetMenu(fileName = "CurrencySettings", menuName = "CodeCatGames/HiveMind/Samples/SampleGame/CrossScene/CurrencySettings")]
    public sealed class CurrencySettings : SerializedScriptableObject
    {
        #region Fields
        [Header("Currency Settings Fields")]
        [SerializeField] private Dictionary<CurrencyTypes, int> _defaultCurrencyValues;
        [SerializeField] private Dictionary<CurrencyTypes, Sprite> _currencyIcons;
        #endregion

        #region Getters
        public Dictionary<CurrencyTypes, int> DefaultCurrencyValues => _defaultCurrencyValues;
        public Dictionary<CurrencyTypes, Sprite> CurrencyIcons => _currencyIcons;
        #endregion
    }
}
