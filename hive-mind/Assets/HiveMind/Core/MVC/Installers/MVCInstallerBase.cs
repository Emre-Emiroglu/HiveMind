using Zenject;

namespace HiveMind.MVC.Installers
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

    public abstract class MVCInstallerBase<TParam1, TParam2, TDerived, TSubInstaller> : InstallerBase
        where TDerived : MVCInstaller<TSubInstaller>
    {
        public static void Install(DiContainer container, TParam1 p1, TParam2 p2) => MVCInstallerBaseLogic<TDerived, TSubInstaller>.InstallLogic(container, p1, p2);
    }

    public abstract class MVCInstallerBase<TParam1, TParam2, TParam3, TDerived, TSubInstaller> : InstallerBase
        where TDerived : MVCInstaller<TSubInstaller>
    {
        public static void Install(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3) => MVCInstallerBaseLogic<TDerived, TSubInstaller>.InstallLogic(container, p1, p2, p3);
    }

    public abstract class MVCInstallerBase<TParam1, TParam2, TParam3, TParam4, TDerived, TSubInstaller> : InstallerBase
        where TDerived : MVCInstaller<TSubInstaller>
    {
        public static void Install(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4) => MVCInstallerBaseLogic<TDerived, TSubInstaller>.InstallLogic(container, p1, p2, p3, p4);
    }

    public abstract class MVCInstallerBase<TParam1, TParam2, TParam3, TParam4, TParam5, TDerived, TSubInstaller> : InstallerBase
        where TDerived : MVCInstaller<TSubInstaller>
    {
        public static void Install(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5) => MVCInstallerBaseLogic<TDerived, TSubInstaller>.InstallLogic(container, p1, p2, p3, p4, p5);
    }

    internal static class MVCInstallerBaseLogic<TDerived, TSubInstaller>
        where TDerived : MVCInstaller<TSubInstaller>
    {
        #region Installs
        internal static void InstallLogic(DiContainer container, params object[] parameters)
        {
            if (parameters == null)
                InstallWithoutParameters(container);
            else
                InstallWitParameters(container, parameters);
        }
        private static void InstallWithoutParameters(DiContainer container)
        {
            var subClass = container.Instantiate<TSubInstaller>() as TDerived;
            subClass.InstallBindings();
        }
        private static void InstallWitParameters(DiContainer container, params object[] parameters)
        {
            var subClass = container.InstantiateExplicit<TSubInstaller>(InjectUtil.CreateArgListExplicit(parameters)) as TDerived;
            subClass.InstallBindings();
        }
        #endregion
    }
}
