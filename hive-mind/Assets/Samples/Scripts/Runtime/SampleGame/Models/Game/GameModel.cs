using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.Game;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Game
{
    public sealed class GameModel : Model<GameSettings>
    {
        #region Constants
        private const string ResourcePath = "Samples/SampleGame/Game/GameSettings";
        #endregion

        #region Constructor
        public GameModel() : base(ResourcePath) { }
        #endregion

        #region PostConstruct
        public override void PostConstruct() { }
        #endregion
    }
}
