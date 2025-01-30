using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.MainMenu;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.MainMenu
{
    public sealed class MainMenuModel : Model<MainMenuSettings>
    {
        #region Constants
        private const string ResourcePath = "Samples/SampleGame/MainMenu/MainMenuSettings";
        #endregion

        #region Constructor
        public MainMenuModel() : base(ResourcePath) { }
        #endregion

        #region PostConstruct
        public override void PostConstruct() { }
        #endregion
    }
}
