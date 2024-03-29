using Zenject;

namespace HiveMind.Core.MVC.Installers
{
    public abstract class MVCInstallerBase<TDerived, TSubInstaller> : InstallerBase
        where TDerived : MVCInstaller<TSubInstaller>
    {
        public static void Install(DiContainer container) => MVCInstallerBaseLogic<TDerived, TSubInstaller>.InstallLogic(container, null);
    }

    public abstract class MVCInstallerBase<TParam1, TDerived, TSubInstaller> : InstallerBase
        where TDerived : MVCInstaller<TSubInstaller>
    {
        public static void Install(DiContainer container, TParam1 p1) => MVCInstallerBaseLogic<TDerived, TSubInstaller>.InstallLogic(container, p1);
    }

    internal static class MVCInstallerBaseLogic<TDerived, TSubInstaller>
        where TDerived : MVCInstaller<TSubInstaller>
    {
        #region Installs
        internal static void InstallLogic(DiContainer container, object parameter)
        {
            if (parameter == null)
                InstallWithoutParameters(container);
            else
                InstallWitParameters(container, parameter);
        }
        private static void InstallWithoutParameters(DiContainer container)
        {
            var subInstaller = container.Instantiate<TSubInstaller>() as TDerived;
            subInstaller.InstallBindings();
        }
        private static void InstallWitParameters(DiContainer container, object parameter)
        {
            var subInstaller = container.InstantiateExplicit<TSubInstaller>(InjectUtil.CreateArgListExplicit(parameter)) as TDerived;
            subInstaller.InstallBindings();
        }
        #endregion
    }
}
