using System.Collections.Generic;
using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene
{
    public sealed class CurrencyModel : Model<CurrencySettings>
    {
        #region Constants
        private const string ResourcePath = "Samples/SampleGame/CrossScene/CurrencySettings";
        private const string CurrencyPath = "CURRENCY_PATH";
        #endregion

        #region Fields
        private Dictionary<CurrencyTypes, int> _currencyValues;
        #endregion

        #region Getters
        public Dictionary<CurrencyTypes, int> CurrencyValues => _currencyValues;
        #endregion

        #region Constructor
        public CurrencyModel() : base(ResourcePath)
        {
            _currencyValues = ES3.Load(nameof(_currencyValues), CurrencyPath,
                new Dictionary<CurrencyTypes, int>(GetSettings.DefaultCurrencyValues));

            Save();
        }
        #endregion

        #region PostConstruct
        public override void PostConstruct() { }
        #endregion

        #region Executes
        public void ChangeCurrencyValue(CurrencyTypes currencyType, int amount, bool isSet)
        {
            int lasValue = _currencyValues[currencyType];
            _currencyValues[currencyType] = isSet ? amount : lasValue + amount;

            Save();
        }
        public void Save() => ES3.Save(nameof(_currencyValues), _currencyValues, CurrencyPath);
        #endregion
    }
}
