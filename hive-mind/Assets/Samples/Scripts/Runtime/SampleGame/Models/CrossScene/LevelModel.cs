using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene
{
    public sealed class LevelModel : Model<LevelSettings>
    {
        #region Constants
        private const string ResourcePath = "Samples/SampleGame/CrossScene/LevelSettings";
        private const string LevelPersistentDataPath = "LEVEL_PERSISTENT_DATA_PATH";
        #endregion

        #region Fields
        private LevelPersistentData _levelPersistentData;
        #endregion

        #region Getters
        public LevelPersistentData LevelPersistentData => _levelPersistentData;
        #endregion

        #region Constructor
        public LevelModel() : base(ResourcePath)
        {
            _levelPersistentData = ES3.Load(nameof(_levelPersistentData), LevelPersistentDataPath, new LevelPersistentData(0));

            Save();
        }
        #endregion

        #region PostConstruct
        public override void PostConstruct() { }
        #endregion

        #region DataExecutes
        public void ResetLevelPersistentData()
        {
            _levelPersistentData = new(0);

            Save();
        }
        public void UpdateCurrentLevelIndex(bool isSet, int value)
        {
            _levelPersistentData.CurrentLevelIndex = isSet ? value : _levelPersistentData.CurrentLevelIndex + value;

            Save();
        }
        public void Save() => ES3.Save(nameof(_levelPersistentData), _levelPersistentData, LevelPersistentDataPath);
        #endregion
    }
}
