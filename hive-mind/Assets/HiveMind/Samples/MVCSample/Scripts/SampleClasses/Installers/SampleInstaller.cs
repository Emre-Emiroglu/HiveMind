using HiveMind.MVC.Installers;
using HiveMind.MVCSample.SampleClasses.Controllers;

namespace HiveMind.MVCSample.SampleClasses.Installers
{
    public class SampleInstaller : MVCInstaller
    {
        #region Constructor
        public SampleInstaller(object key) : base(key) { }
        #endregion

        #region Bindings
        public override void InstallBindings()
        {
            base.InstallBindings();

            commandBinder.BindSignal<SampleSignal1>().WithCommand<SampleCommand1>();
            commandBinder.BindSignal<SampleSignal2>().WithCommand<SampleCommand2>();
        }
        #endregion
    }
}
