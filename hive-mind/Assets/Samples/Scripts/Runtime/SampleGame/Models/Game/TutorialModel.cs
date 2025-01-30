using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.Game;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Game
{
    public sealed class TutorialModel : Model<TutorialSettings>
    {
        #region Constants
        private const string ResourcePath = "Samples/SampleGame/Game/TutorialSettings";
        private const string TutorialPath = "TUTORIAL_PATH";
        #endregion

        #region Fields
        private bool _isTutorialShowed;
        #endregion

        #region Getters
        public bool IsTutorialShowed => _isTutorialShowed;
        #endregion

        #region Constructor
        public TutorialModel() : base(ResourcePath) =>
            _isTutorialShowed = ES3.Load(nameof(_isTutorialShowed), TutorialPath, false);
        #endregion

        #region PostConstruct
        public override void PostConstruct() { }
        #endregion

        #region Executes
        public void SetTutorial(bool isActive)
        {
            _isTutorialShowed = isActive;
            
            ES3.Save(nameof(_isTutorialShowed), _isTutorialShowed, TutorialPath);
        }
        #endregion
    }
}