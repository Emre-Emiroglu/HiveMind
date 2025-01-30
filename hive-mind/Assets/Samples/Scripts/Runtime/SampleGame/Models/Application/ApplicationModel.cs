using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.Application;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Application
{
    public sealed class ApplicationModel : Model<ApplicationSettings>
    {
        #region Constants
        private const string ResourcePath = "Samples/SampleGame/Application/ApplicationSettings";
        #endregion

        #region Constructor
        public ApplicationModel() : base(ResourcePath) { }
        #endregion

        #region PostConstruct
        public override void PostConstruct() { }
        #endregion
    }
}
