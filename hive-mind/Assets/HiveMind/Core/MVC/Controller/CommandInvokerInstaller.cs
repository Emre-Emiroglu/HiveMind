using Zenject;

namespace HiveMind.MVC.Controller
{
    public class CommandInvokerInstaller : Installer<CommandInvokerInstaller>
    {
        #region Core
        public override void InstallBindings()
        {
            Container.Bind<CommandInvoker>().AsSingle().CopyIntoAllSubContainers();
            Container.Bind<CommandPool>().AsSingle().CopyIntoAllSubContainers();
        }
        #endregion
    }
}
