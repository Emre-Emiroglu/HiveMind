using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.Bootstrap;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Bootstrap
{
    public sealed class BootstrapModel : Model<BootstrapSettings>
    {
        #region Constants
        private const string ResourcePath = "Samples/SampleGame/Bootstrap/BootstrapSettings";
        #endregion

        #region Constructor
        public BootstrapModel() : base(ResourcePath) { }
        #endregion

        #region PostConstruct
        public override void PostConstruct() { }
        #endregion
    }
}
