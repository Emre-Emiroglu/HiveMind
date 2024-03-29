using HiveMind.Samples.MVCSample.SampleClasses.Controllers;
using UnityEngine;
using Zenject;

namespace HiveMind.Samples.MVCSample.SampleClasses.Installers
{
    public class SampleMonoInstaller : MonoInstaller<SampleMonoInstaller>
    {
        #region Fields
        [SerializeField] private string key = "MVCSample";
        [SerializeField] private string assemblyName = "HiveMind.MVCSample";
        #endregion

        #region Bindings
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            SampleInstaller.Install(Container, new(Container, key, assemblyName));
        }
        #endregion

        #region Cycle
        public override void Start()
        {
            SignalBus signalBus = Container.Resolve<SignalBus>();
            signalBus.Fire(new SampleSignal1());
            signalBus.Fire(new SampleSignal2() { InjectedValue = 99 });
        }
        #endregion
    }
}
