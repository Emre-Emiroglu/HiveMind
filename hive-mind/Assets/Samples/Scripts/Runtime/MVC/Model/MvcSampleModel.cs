using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
using CodeCatGames.HiveMind.Samples.Runtime.MVC.Data.ScriptableObjects;

namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Model
{
    public sealed class MvcSampleModel : Model<MvcSampleSettings>
    {
        #region Constants
        private const string ResourcePath = "Samples/MVC/MvcSampleSettings";
        #endregion
        
        #region Constructor
        public MvcSampleModel() : base(ResourcePath) { }
        #endregion
        
        #region PostConstruct
        public override void PostConstruct() { }
        #endregion
    }
}