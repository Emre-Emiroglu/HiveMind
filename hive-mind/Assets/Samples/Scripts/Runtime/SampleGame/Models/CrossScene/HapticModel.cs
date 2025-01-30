using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene
{
    public sealed class HapticModel : Model<HapticSettings>
    {
        #region Constants
        private const string ResourcePath = "Samples/SampleGame/CrossScene/HapticSettings";
        private const string HapticPath = "HAPTIC_PATH";
        #endregion

        #region Fields
        private bool _isHapticMuted;
        #endregion

        #region Getters
        public bool IsHapticMuted => _isHapticMuted;
        #endregion

        #region Constructor
        public HapticModel() : base(ResourcePath)
        {
            _isHapticMuted = ES3.Load(nameof(_isHapticMuted), HapticPath, false);

            SetHaptic(_isHapticMuted);
        }
        #endregion

        #region PostConstruct
        public override void PostConstruct() { }
        #endregion

        #region Executes
        public void SetHaptic(bool isActive)
        {
            _isHapticMuted = isActive;
            
            Save();
        }
        public void Save() => ES3.Save(nameof(_isHapticMuted), _isHapticMuted, HapticPath);
        #endregion
    }
}
