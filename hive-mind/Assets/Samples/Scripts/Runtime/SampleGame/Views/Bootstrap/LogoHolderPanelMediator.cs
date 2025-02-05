using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Bootstrap;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Utilities.CrossScene;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Bootstrap
{
    public sealed class LogoHolderPanelMediator : Mediator<LogoHolderPanelView>
    {
        #region ReadonlyFields
        private readonly BootstrapModel _bootstrapModel;
        #endregion

        #region Constructor
        public LogoHolderPanelMediator(LogoHolderPanelView view, BootstrapModel bootstrapModel) : base(view) =>
            _bootstrapModel = bootstrapModel;
        #endregion

        #region PostConstruct
        public override void PostConstruct() { }
        #endregion

        #region Core
        public override void Initialize()
        {
            GetView.LogoImage.sprite = _bootstrapModel.GetSettings.LogoSprite;
            GetView.LogoImage.preserveAspect = true;

            GetView.UIPanelVo.CanvasGroup.ChangeUIPanelCanvasGroupActivation(true);
            GetView.UIPanelVo.PlayableDirector.ChangeUIPanelTimelineActivation(true);
        }
        public override void Dispose() { }
        #endregion
    }
}
