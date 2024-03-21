using HiveMind.MVCSample.SampleClasses.Controllers;
using Zenject;

namespace HiveMind.MVCSample.SampleClasses.Installers
{
    public class SampleMonoInstaller : MonoInstaller<SampleMonoInstaller>
    {
        #region Fields
        private SignalBus signalBus;
        #endregion

        #region Bindings
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            SampleInstaller.Install(Container, "MVCSample");
        }
        #endregion

        #region Cycle
        public override void Start()
        {
            signalBus = Container.Resolve<SignalBus>();
            signalBus.Fire(new SampleSignal1());
            signalBus.Fire(new SampleSignal2() { InjectedValue = 99 });
        }
        #endregion
    }
}
