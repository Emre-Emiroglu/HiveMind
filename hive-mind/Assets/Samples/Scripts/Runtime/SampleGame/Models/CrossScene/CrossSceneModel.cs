using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene
{
    public sealed class CrossSceneModel : Model<CrossSceneSettings>
    {
        #region Constants
        private const string ResourcePath = "Samples/SampleGame/CrossScene/CrossSceneSettings";
        #endregion

        #region Constructor
        public CrossSceneModel() : base(ResourcePath) { }
        #endregion

        #region PostConstruct
        public override void PostConstruct() { }
        #endregion
    }
}
